using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.MockClasses;
using System.Collections.Generic;
using Kaisean.Tools.CollapseXml.Errors;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using Kaisean.Tools.CollapseXml;
using System.Globalization;

namespace UnitTests
{
	[TestClass]
	public class CollapseXmlFormattingTests
	{
		[TestMethod]
		public void DateTimeFormattingWithCulture()
		{
			Customer customer = Repository.SampleCustomer;

			Collapse collapse =
				new Collapse()
				.SetDateTimeFormat(CultureInfo.CurrentCulture);

			XDocument doc =
				collapse.ExportToXml(customer, Res.FileContent("FormattingDates"));

			List<XElement> orders = doc.Root.Elements("order").ToList();
			List<XElement> valOrders = doc.Root.Elements("valOrder").ToList();

			for (int i = 0; i < orders.Count; i++)
			{
				XElement orderElement = orders[i];
				XElement valOrderElement = valOrders[i];
				Order order = customer.Orders[i];

				String expectedDateTimeString = order.Timestamp.ToString(CultureInfo.CurrentCulture);

				Assert.AreEqual(expectedDateTimeString, orderElement.Value);
				Assert.AreEqual(expectedDateTimeString, orderElement.Attribute("time").Value);
				Assert.AreEqual(expectedDateTimeString, valOrderElement.Value.Trim());
			}

		}

		[TestMethod]
		public void DateTimeFormattingWithString()
		{
			String format = "yyyy-MM-dd HH:mm:ss";

			XDocument doc =
				new Collapse()
				.SetDateTimeFormat(format)
				.ExportToXml(Repository.SampleCustomer, Res.FileContent("FormattingDates"));

			List<XElement> orders = doc.Root.Elements("order").ToList();
			List<XElement> valOrders = doc.Root.Elements("valOrder").ToList();

			for (int i = 0; i < orders.Count; i++)
			{
				XElement orderElement = orders[i];
				XElement valOrderElement = valOrders[i];
				Order order = Repository.SampleCustomer.Orders[i];

				String expectedDateTimeString = order.Timestamp.ToString(format);

				Assert.AreEqual(expectedDateTimeString, orderElement.Value);
				Assert.AreEqual(expectedDateTimeString, orderElement.Attribute("time").Value);
				Assert.AreEqual(expectedDateTimeString, valOrderElement.Value.Trim());
			}

			format = "ss . mm . HH hhmmss yyy - MM d";
			doc = new Collapse()
				.SetDateTimeFormat(format)
				.ExportToXml(Repository.SampleCustomer, Res.FileContent("FormattingDates"));

			orders = doc.Root.Elements("order").ToList();
			valOrders = doc.Root.Elements("valOrder").ToList();

			for (int i = 0; i < orders.Count; i++)
			{
				XElement orderElement = orders[i];
				XElement valOrderElement = valOrders[i];
				Order order = Repository.SampleCustomer.Orders[i];

				String expectedDateTimeString = order.Timestamp.ToString(format);

				Assert.AreEqual(expectedDateTimeString, orderElement.Value);
				Assert.AreEqual(expectedDateTimeString, orderElement.Attribute("time").Value);
				Assert.AreEqual(expectedDateTimeString, valOrderElement.Value.Trim());
			}
		}

		[TestMethod]
		[ExpectedException(typeof(DateTimeFormatInvalidException))]
		public void DateTimeFormattingInvalidDateTimeFormat()
		{
			Customer customer = Repository.SampleCustomer;

			String invalidFormat = "a";

			Collapse collapse = new Collapse().SetDateTimeFormat(invalidFormat);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DateTimeFormattingNoProvider()
		{
			IFormatProvider provider = null;
			new Collapse().SetDateTimeFormat(provider);
		}

		[TestMethod]
		public void NumberFormattingWithCulture()
		{
			Repository rep = new Repository();
			Collapse collapse = new Collapse();

			XDocument doc =
				collapse.ExportToXml(rep, Res.FileContent("FormattingNumbers"));

			List<XElement> customerElements = doc.Root.Elements("customer").ToList();
			List<XElement> valCustomers = doc.Root.Elements("valCustomer").ToList();

			for (int i = 0; i < customerElements.Count; i++)
			{
				XElement customerElement = customerElements[i];
				XElement valCustomerElement = customerElements[i];
				Customer customer = rep.Customers[i];

				String expectedNumberString = customer.Coefficient.ToString(CultureInfo.CurrentCulture);

				Assert.AreEqual(expectedNumberString, customerElement.Value);
				Assert.AreEqual(expectedNumberString, customerElement.Attribute("coefficient").Value);
				Assert.AreEqual(expectedNumberString, valCustomerElement.Value.Trim());
			}
		}

		[TestMethod]
		public void NumberFormattingWithFormatProvider()
		{
			NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "@" };
			Repository rep = new Repository();

			Collapse collapse = new Collapse()
				.SetNumberFormat(nfi);

			XDocument doc =
				collapse.ExportToXml(rep, Res.FileContent("FormattingNumbers"));

			List<XElement> customerElements = doc.Root.Elements("customer").ToList();
			List<XElement> valCustomers = doc.Root.Elements("valCustomer").ToList();

			for (int i = 0; i < customerElements.Count; i++)
			{
				XElement customerElement = customerElements[i];
				XElement valCustomerElement = customerElements[i];
				Customer customer = rep.Customers[i];

				String expectedNumberString = customer.Coefficient.ToString(nfi);

				Assert.AreEqual(expectedNumberString, customerElement.Value);
				Assert.AreEqual(expectedNumberString, customerElement.Attribute("coefficient").Value);
				Assert.AreEqual(expectedNumberString, valCustomerElement.Value.Trim());
			}
		}

	}
}
