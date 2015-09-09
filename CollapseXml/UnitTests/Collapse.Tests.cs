using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.MockClasses;
using System.Collections.Generic;
using Kaisean.Tools.CollapseXml.Errors;
using System.Linq;
using System.Xml.Linq;
using Kaisean.Tools.CollapseXml.Helpers;
using Kaisean.Tools.CollapseXml;
using System.Globalization;

namespace UnitTests
{
	[TestClass]
	public class CollapseTests
	{
		[TestMethod]
		public void Chaining()
		{
			Collapse collapse = new Collapse();

			Assert.AreSame(collapse, collapse.SetNumberFormat(CultureInfo.CurrentCulture));
			Assert.AreSame(collapse, collapse.SetDateTimeFormat(CultureInfo.CurrentCulture));
			Assert.AreSame(collapse, collapse.SetDateTimeFormat("yyyy"));
			Assert.AreSame(collapse, collapse.SetFilter("isValid", args => true));
		}
	}
}
