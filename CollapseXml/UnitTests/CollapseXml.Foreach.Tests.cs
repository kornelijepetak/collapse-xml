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
	public class CollapseXmlForeachTests
	{
		[TestMethod]
		public void RootValidAndContainsCorrectNumberOfChildren()
		{
			Repository repository = new Repository();
			
			XDocument doc = 
				new Collapse()
				.ExportToXml(repository, Res.FileContent("ForeachCustomer"));

			Assert.AreEqual("customers", doc.Root.Name);
			Assert.AreEqual(repository.Customers.Count, doc.Root.Elements("customer").Count());
			Assert.AreEqual(doc.Root.Elements().Count(), doc.Root.Elements("customer").Count());
		}

		[TestMethod]
		public void ForeachElementCorrectlyTransformed()
		{
			Repository repository = new Repository();

			XDocument doc =
				new Collapse()
				.ExportToXml(repository, Res.FileContent("ForeachCustomer"));

			List<XElement> customersXml = doc.Root.Elements().ToList();
			for (int i = 0; i < repository.Customers.Count; i++)
			{
				Customer customer = repository.Customers[i];
				XElement customerXml = customersXml[i];

				Assert.AreEqual(customer.Name, customerXml.Attribute("name").Value);
			}
		}

		[TestMethod]
		public void ForeachCollectionIdentifierContainsWordIn()
		{
			Repository repository = new Repository();

			XDocument doc =
				new Collapse()
				.ExportToXml(repository, Res.FileContent("ForeachCollectionIdentifierContainsIn"));
		}

		[TestMethod]
		public void ForeachFromIEnumerableOfValueTypeCovariance()
		{
			Repository repository = new Repository();

			XDocument doc =
				new Collapse()
				.ExportToXml(repository, Res.FileContent("ValidXmlCovarianceInValueTypes"));
		}

	}
}
