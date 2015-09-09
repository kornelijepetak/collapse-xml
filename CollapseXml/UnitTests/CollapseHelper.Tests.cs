using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.MockClasses;
using System.Collections.Generic;
using Kaisean.Tools.CollapseXml.Errors;
using System.Linq;
using System.Xml.Linq;
using Kaisean.Tools.CollapseXml.Helpers;
using Kaisean.Tools.CollapseXml;

namespace UnitTests
{
	[TestClass]
	public class CollapseHelperTests
	{
		[TestMethod]
		public void IsCollapseForeach()
		{
			Assert.IsTrue(new XElement("element",
				new XAttribute(XName.Get("foreach", Collapse.CollapseNamespace), ""))
				.IsCollapseForeach());

			Assert.IsFalse(new XElement("element",
				new XAttribute(XName.Get("foreach", "http://www.something.com/random/namespace"), ""))
				.IsCollapseForeach());

			Assert.IsFalse(new XElement("element",
				new XAttribute(XName.Get("randomTag", Collapse.CollapseNamespace), ""))
				.IsCollapseForeach());

		}

		[TestMethod]
		public void IsCollapseValue()
		{
			Assert.IsTrue(new XElement(
				XName.Get("value", Collapse.CollapseNamespace))
				.IsCollapseValue());

			Assert.IsFalse(new XElement(
				XName.Get("NotValue", Collapse.CollapseNamespace))
				.IsCollapseValue());

			Assert.IsFalse(new XElement(
				XName.Get("valuex", Collapse.CollapseNamespace))
				.IsCollapseValue());
		}

		[TestMethod]
		public void AsXElementFrame()
		{
			XElement element = new XElement("Parent",
				new XElement("child"),
				new XElement("anotherChild"),
				new XAttribute("name", "parent"),
				new XAttribute("other", "random")
				);

			XElement elementFrame = element.AsXElementFrame();

			Assert.AreEqual(element.Name, elementFrame.Name);
			Assert.IsFalse(elementFrame.HasElements);
			Assert.IsTrue(elementFrame.HasAttributes);
			Assert.AreEqual(2, elementFrame.Attributes().Count());
			Assert.AreEqual("parent", elementFrame.Attribute("name").Value);
			Assert.AreEqual("random", elementFrame.Attribute("other").Value);
		}

		[TestMethod]
		public void AsXElementFrameCollapseAttributes()
		{
			XName collapseAttribute = XName.Get("foreach", Collapse.CollapseNamespace);
			XElement element = new XElement("Parent",
				new XElement("child"),
				new XElement("anotherChild"),
				new XAttribute("name", "parent"),
				new XAttribute("other", "random"),
				new XAttribute(collapseAttribute, "item in Orders")
				);

			XElement elementFrame = element.AsXElementFrame();

			Assert.AreEqual(element.Name, elementFrame.Name);
			Assert.IsFalse(elementFrame.HasElements);
			Assert.IsTrue(elementFrame.HasAttributes);
			Assert.AreEqual(2, elementFrame.Attributes().Count());
			Assert.AreEqual("parent", elementFrame.Attribute("name").Value);
			Assert.AreEqual("random", elementFrame.Attribute("other").Value);
			Assert.IsNull(elementFrame.Attribute(collapseAttribute));
		}

		[TestMethod]
		public void CollapseAttribute()
		{
			XElement element = new XElement("node",
				new XAttribute(XName.Get("val", Collapse.CollapseNamespace), "100")
				);

			XAttribute attr = element.CollapseAttribute("val");

			Assert.IsNotNull(attr);
			Assert.AreEqual("100", attr.Value);
		}

		[TestMethod]
		public void ForeachExpressionNotNull()
		{
			String variable = "item";
			String collection = "collection";
			String expression = String.Format("{0} in {1}", variable, collection);

			XElement element = new XElement("node",
				new XAttribute(XName.Get("foreach", Collapse.CollapseNamespace), expression)
				);

			ForEachDefinition forEach = element.ToForeachDefinition();

			Assert.IsNotNull(forEach);
		}

		[TestMethod]
		public void ForeachExpression()
		{
			String variable = "item";
			String collection = "collection";
			String expression = String.Format("{0} in {1}", variable, collection);

			XElement element = new XElement("node",
				new XAttribute(XName.Get("foreach", Collapse.CollapseNamespace), expression)
				);

			ForEachDefinition forEach = element.ToForeachDefinition();

			Assert.AreEqual(variable, forEach.Variable);
			Assert.AreEqual(collection, forEach.Collection);
		}

		[TestMethod]
		public void ForeachExpressionWithPath()
		{
			String variable = "item";
			String collection = "x.items";
			String expression = String.Format("{0} in {1}", variable, collection);

			XElement element = new XElement("node",
				new XAttribute(XName.Get("foreach", Collapse.CollapseNamespace), expression)
				);

			ForEachDefinition forEach = element.ToForeachDefinition();

			Assert.AreEqual(variable, forEach.Variable);
			Assert.AreEqual(collection, forEach.Collection);

			Assert.AreEqual(2, forEach.CollectionPath.Count);
			Assert.AreEqual("x", forEach.CollectionPath[0]);
			Assert.AreEqual("items", forEach.CollectionPath[1]);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidForeachPathException))]
		public void ForeachExpressionWithInvalidExpression()
		{
			String expression = "item x";

			XElement element = new XElement("node",
				new XAttribute(XName.Get("foreach", Collapse.CollapseNamespace), expression)
				);

			ForEachDefinition forEach = element.ToForeachDefinition();
		}

		[TestMethod]
		public void TrimmedSplit()
		{
			String pathString = "A.B.C";
			List<string> path = pathString.TrimmedSplit();
			Assert.AreEqual(3, path.Count);
			Assert.AreEqual("A", path[0]);
			Assert.AreEqual("B", path[1]);
			Assert.AreEqual("C", path[2]);

			pathString = "";
			path = pathString.TrimmedSplit();
			Assert.AreEqual(0, path.Count);

			pathString = "item";
			path = pathString.TrimmedSplit();
			Assert.AreEqual(1, path.Count);
			Assert.AreEqual("item", path[0]);

			pathString = "item.Address.Country.Abbreviation";
			path = pathString.TrimmedSplit();
			Assert.AreEqual(4, path.Count);
			Assert.AreEqual("item", path[0]);
			Assert.AreEqual("Address", path[1]);
			Assert.AreEqual("Country", path[2]);
			Assert.AreEqual("Abbreviation", path[3]);

			pathString = "A.B.C";
			path = pathString.TrimmedSplit("B");
			Assert.AreEqual(2, path.Count);
			Assert.AreEqual("A.", path[0]);
			Assert.AreEqual(".C", path[1]);

			pathString = "item in Items";
			path = pathString.TrimmedSplit("in");
			Assert.AreEqual(2, path.Count);
			Assert.AreEqual("item", path[0]);
			Assert.AreEqual("Items", path[1]);
		}

		[TestMethod]
		public void ContainsWhitespace()
		{
			Assert.IsTrue("This string contains whitespace.".ContainsWhitespace());
			Assert.IsTrue("This\tstring\tcontains\ttabular\twhitespace.".ContainsWhitespace());
			Assert.IsTrue("This\ncontains\nnew\nlines.".ContainsWhitespace());
			Assert.IsFalse("ThisStringIsWhitespaceless.".ContainsWhitespace());
		}
	}
}
