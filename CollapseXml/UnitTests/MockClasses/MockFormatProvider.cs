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

namespace UnitTests.MockClasses
{
	internal class MockFormatProvider : IFormatProvider
	{
		public object GetFormat(Type formatType)
		{
			return null;
		}
	}
}
