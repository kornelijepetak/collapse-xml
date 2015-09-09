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
	public class CollapseXmlInitializationTests
	{
		[TestMethod]
		[ExpectedException(typeof(InvalidXmlFormatException))]
		public void InitializationWithMissingTemplateElement()
		{
			new Collapse().ExportToXml(
				new object(),
				Res.FileContent("MissingTemplateElement"));
		}

		[TestMethod]
		[ExpectedException(typeof(MissingNamespaceException))]
		public void InitializationWithMissingNamespace()
		{
			new Collapse().ExportToXml(
				new object(),
				Res.FileContent("MissingNamespace"));
		}

		[TestMethod]
		[ExpectedException(typeof(RootElementNotFoundException))]
		public void InitializationWithoutRootElement()
		{
			new Collapse().ExportToXml(
				new object(),
				Res.FileContent("RootElementNotFound"));
		}

		[TestMethod]
		[ExpectedException(typeof(MultipleRootElementsException))]
		public void InitializationWithMultipleRootElements()
		{
			new Collapse().ExportToXml(
				new object(),
				Res.FileContent("MultipleRootElements"));
		}

		[TestMethod]
		public void InitializationWithValidXml()
		{
			new Collapse().ExportToXml(
				new Repository(),
				Res.FileContent("ValidXml"));
		}

		[TestMethod]
		public void InitializationWithValidXmlInlineTextTransformDefaultValue()
		{
			Collapse collapse = new Collapse();
			Assert.IsTrue(collapse.IsInlineTextTransformEnabled);
		}

		[TestMethod]
		public void InitializationWithValidXmlDisableInlineTextTransform()
		{
			Collapse collapse = new Collapse(transformInlineText: false);

			Assert.IsFalse(collapse.IsInlineTextTransformEnabled);

			XDocument doc = collapse.ExportToXml(
				new Repository(),
				Res.FileContent("ValidInlineText"));

			List<XElement> customers = doc.Root.Elements("customer").ToList();

			foreach (XElement item in customers)
				Assert.AreEqual("(cx|c.Name)", item.Value);
		}

		[TestMethod]
		public void InitializationUsingConstructor()
		{
			Collapse collapse = new Collapse();
			Assert.IsTrue(collapse.IsInlineTextTransformEnabled);
			Assert.AreEqual(MissingFilterBehavior.ThrowException, collapse.MissingFilterBehavior);

			collapse = new Collapse(MissingFilterBehavior.IncludeItem);
			Assert.IsTrue(collapse.IsInlineTextTransformEnabled);
			Assert.AreEqual(MissingFilterBehavior.IncludeItem, collapse.MissingFilterBehavior);

			collapse = new Collapse(MissingFilterBehavior.OmitItem);
			Assert.IsTrue(collapse.IsInlineTextTransformEnabled);
			Assert.AreEqual(MissingFilterBehavior.OmitItem, collapse.MissingFilterBehavior);

			collapse = new Collapse(MissingFilterBehavior.OmitItem, false);
			Assert.IsFalse(collapse.IsInlineTextTransformEnabled);
			Assert.AreEqual(MissingFilterBehavior.OmitItem, collapse.MissingFilterBehavior);

		}
	}
}
