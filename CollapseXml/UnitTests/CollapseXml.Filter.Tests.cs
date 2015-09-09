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

namespace UnitTests
{
	[TestClass]
	public class CollapseXmlFilterTests
	{
		
		[TestMethod]
		public void ValidFilter()
		{
			String template = Res.FileContent("ValidXmlWithFilter");
			Repository repository = new Repository();

			// Normally, repository contains 2 customers which should be in results

			// Set filter which should leave only 1 customer in results
			Collapse collapse = new Collapse()
				.SetFilter("isCustomerValid", args => args[0].Value<String>().Contains("Mark"))
				.SetFilter("hasCustomers", args => args[0].Value<List<Customer>>().Any());

			XDocument xDocument = collapse.ExportToXml(repository, template);
			List<XElement> customers = xDocument.Root.Elements("customer").ToList();
			Assert.AreEqual(1, customers.Count);

			// Set filter which should filter out all customers from results
			collapse = new Collapse()
				.SetFilter("isCustomerValid", args => args[1].Value<int>() > 20)
				.SetFilter("hasCustomers", args => args[0].Value<List<Customer>>().Any());

			xDocument = collapse.ExportToXml(repository, template);
			customers = xDocument.Root.Elements("customer").ToList();
			Assert.AreEqual(0, customers.Count);
			Assert.IsTrue(xDocument.Root.Elements("description").Count() == 1);

			// Set filter which should filter out all customers from results
			collapse = new Collapse()
				.SetFilter("isCustomerValid", args => args[1].Value<int>() > 20)
				.SetFilter("hasCustomers", args => false);

			xDocument = collapse.ExportToXml(repository, template);
			customers = xDocument.Root.Elements("customer").ToList();
			Assert.AreEqual(0, customers.Count);
			Assert.IsTrue(xDocument.Root.Elements("description").Count() == 0);
		}

		[TestMethod]
		public void FilterOnValue()
		{
			// Only Mark should have his name shown
			Collapse collapse = new Collapse()
				.SetFilter("isCustomerValid", args => args[0].Value<Customer>().Name.ToLower().Contains("mark"));

			Repository rep = new Repository();

			XDocument doc = collapse.ExportToXml(
				rep,
				Res.FileContent("FilterOnValue"));

			Debug.WriteLine(doc.ToString());

			List<XElement> customers = doc.Root.Elements("customer").ToList();
			Assert.AreEqual(rep.Customers[0].Name, customers[0].Value.Trim());
			Assert.AreEqual(string.Empty, customers[1].Value.Trim());
		}

		[TestMethod]
		[ExpectedException(typeof(FilterNotFoundException))]
		public void ValidFilterMissingFilterBehaviorThrowException()
		{
			new Collapse().ExportToXml(
				new Repository(),
				Res.FileContent("ValidXmlWithFilter"));
		}

		[TestMethod]
		public void ValidFilterMissingFilterBehaviorInclude()
		{
			Repository rep = new Repository();

			XDocument doc =
				new Collapse(MissingFilterBehavior.IncludeItem)
				.ExportToXml(rep, Res.FileContent("ValidXmlWithFilter"));

			Assert.AreEqual(1, doc.Root.Elements("description").Count());
			Assert.AreEqual(rep.Customers.Count, doc.Root.Elements("customer").Count());
		}

		[TestMethod]
		public void ValidFilterMissingFilterBehaviorSomeFound()
		{
			Collapse collapse = new Collapse(MissingFilterBehavior.OmitItem)
				.SetFilter("hasCustomers", args => args[0].Value<List<Customer>>().Any());

			XDocument doc = collapse.ExportToXml(
				new Repository(),
				Res.FileContent("ValidXmlWithFilter"));

			Assert.AreEqual(1, doc.Root.Elements("description").Count());
			Assert.AreEqual(0, doc.Root.Elements("customer").Count());
		}

		[TestMethod]
		public void ValidFilterMissingFilterBehaviorOmit()
		{
			Collapse collapse = new Collapse(MissingFilterBehavior.OmitItem);

			XDocument doc = collapse.ExportToXml(
				new Repository(),
				Res.FileContent("ValidXmlWithFilter"));

			Assert.AreEqual(0, doc.Root.Elements("description").Count());
			Assert.AreEqual(0, doc.Root.Elements("customer").Count());
		}

		[TestMethod]
		public void FilterAttribute()
		{
			Collapse collapse = new Collapse()
				.SetFilter("isValidName", args => args[0].Value<Customer>().Name.Contains("Mark"));

			XDocument doc = collapse.ExportToXml(
				new Repository(),
				Res.FileContent("FilterAttribute"));

			List<XElement> elements = doc.Root.Elements("customer").ToList();

			Assert.AreEqual(1, elements[0].Attributes("name").Count());
			Assert.AreEqual(0, elements[1].Attributes("name").Count());
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidFilterDefinitionException))]
		public void FilterDefinitionErrorEmpty()
		{
			FilterDefinition.FromExpression("");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidFilterDefinitionException))]
		public void FilterDefinitionErrorArgumentsEmpty()
		{
			FilterDefinition.FromExpression("function|");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidFilterDefinitionException))]
		public void FilterDefinitionErrorFunctionNameEmpty()
		{
			FilterDefinition.FromExpression("|arg1, arg2");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidFilterDefinitionException))]
		public void FilterDefinitionErrorFunctionNameNotValid()
		{
			FilterDefinition.FromExpression("func word|arg1, arg2");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidFilterDefinitionException))]
		public void FilterDefinitionErrorTooManySegments()
		{
			FilterDefinition.FromExpression("func|something|arg1, arg2");
		}

		[TestMethod]
		public void FilterDefinitionValid()
		{
			FilterDefinition filter = FilterDefinition.FromExpression("isValid");

			Assert.IsNotNull(filter);
			Assert.AreEqual("isValid", filter.Function);
			Assert.IsNotNull(filter.Arguments);
			Assert.AreEqual(0, filter.Arguments.Count);

			filter = FilterDefinition.FromExpression("func | something");

			Assert.IsNotNull(filter);
			Assert.AreEqual("func", filter.Function);
			Assert.IsNotNull(filter.Arguments);
			Assert.AreEqual(1, filter.Arguments.Count);
			Assert.AreEqual("something", filter.Arguments.First().Path);

			filter = FilterDefinition.FromExpression("filterName | arg1, arg2, arg3");

			Assert.IsNotNull(filter);
			Assert.AreEqual("filterName", filter.Function);
			Assert.IsNotNull(filter.Arguments);
			Assert.AreEqual(3, filter.Arguments.Count);
			Assert.AreEqual("arg1", filter.Arguments[0].Path);
			Assert.AreEqual("arg2", filter.Arguments[1].Path);
			Assert.AreEqual("arg3", filter.Arguments[2].Path);
		}

	}
}
