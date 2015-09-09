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
	public class CollapseXmlValidTests
	{
		[TestMethod]
		public void TextNodeIsValid()
		{
			Repository rep = new Repository();

			XDocument xDocument = 
				new Collapse()
				.ExportToXml(rep, Res.FileContent("TextNode"));

			Assert.AreEqual("customers", xDocument.Root.Name);

			List<XElement> customers = xDocument.Root.Elements("customer").ToList();

			for (int i = 0; i < customers.Count; i++)
			{
				Assert.AreEqual(
					String.Format("Customer's name is {0}.", 
					rep.Customers[i].Name), 
					customers[i].Value);
			}
		}

	}
}
