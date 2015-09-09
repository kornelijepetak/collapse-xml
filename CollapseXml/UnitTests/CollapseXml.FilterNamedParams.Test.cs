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
	public class CollapseXmlFilterNamedParamsTests
	{
		[TestMethod]
		public void NamedFilters()
		{
			String template = Res.FileContent("FilterNamedArguments");
			Repository rep = new Repository();

			new Collapse()
				.SetFilter("Filter", args =>
				{
					Assert.IsNotNull(args["c"].Value<Customer>());
					Assert.IsNotNull(args["count"].Value<int>() >= 0);

					return true;
				})
				.ExportToXml(rep, template);
		}

		[TestMethod]
		public void FilterDefinitionValid()
		{
			FilterDefinition filter = FilterDefinition.FromExpression("isValid | name: customer.Name");

			Assert.IsNotNull(filter);
			Assert.AreEqual("isValid", filter.Function);
			Assert.IsNotNull(filter.Arguments);
			Assert.AreEqual(1, filter.Arguments.Count);
			Assert.IsNotNull(filter.Arguments.FirstOrDefault());
			Assert.AreEqual("name", filter.Arguments.First().Name);
			Assert.AreEqual("customer.Name", filter.Arguments.First().Path);

			filter = FilterDefinition.FromExpression("func | something, val: c.A.X, str, name: user.Name");

			Assert.IsNotNull(filter);
			Assert.AreEqual("func", filter.Function);
			Assert.IsNotNull(filter.Arguments);
			Assert.AreEqual(4, filter.Arguments.Count);
			Assert.AreEqual(null, filter.Arguments.First().Name);
			Assert.AreEqual("something", filter.Arguments.First().Path);
			Assert.AreEqual("val", filter.Arguments[1].Name);
			Assert.AreEqual("c.A.X", filter.Arguments[1].Path);
			Assert.AreEqual(null, filter.Arguments[2].Name);
			Assert.AreEqual("str", filter.Arguments[2].Path);
			Assert.AreEqual("name", filter.Arguments[3].Name);
			Assert.AreEqual("user.Name", filter.Arguments[3].Path);
		}

	}
}
