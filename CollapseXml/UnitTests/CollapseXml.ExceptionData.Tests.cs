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
	public class CollapseXmlExceptionDataTests
	{
		[TestMethod]
		public void DateTimeFormatInvalid()
		{
			try
			{
				new Collapse()
					.SetDateTimeFormat("a")
					.ExportToXml(new Repository(), Res.FileContent("ValidXml"));
			}
			catch (DateTimeFormatInvalidException ex)
			{
				Assert.AreEqual("a", ex.Format);
			}

			try
			{
				new Collapse()
					.SetDateTimeFormat(new MockFormatProvider())
					.ExportToXml(new Repository(), Res.FileContent("ValidXml"));
			}
			catch (DateTimeFormatInvalidException ex)
			{
				Assert.AreEqual(String.Empty, ex.Format);
			}
		}

		[TestMethod]
		public void FilterNotFound()
		{
			try
			{
				Collapse.Export(new Repository(), Res.FileContent("ValidXmlWithFilter"));
			}
			catch (FilterNotFoundException ex)
			{
				Assert.AreEqual("hasCustomers", ex.FilterName);
			}
		}

		[TestMethod]
		public void InvalidCollectionPath()
		{
			try
			{
				Collapse.Export(new Repository(), Res.FileContent("InvalidCollectionPath"));
			}
			catch (InvalidCollectionPathException ex)
			{
				Assert.AreEqual("Customersx", ex.InvalidProperty);
			}

			try
			{
				Collapse.Export(new Repository(), Res.FileContent("InvalidCollectionPathLong"));
			}
			catch (InvalidCollectionPathException ex)
			{
				Assert.AreEqual("Counter", ex.InvalidProperty);
			}
		}

		[TestMethod]
		public void InvalidFilterDefinition()
		{
			try
			{
				Collapse.Export(new Repository(), Res.FileContent("FilterDefinitionInvalidNumberOfElementsTooFew"));
			}
			catch (InvalidFilterDefinitionException ex)
			{
				Assert.AreEqual(
					InvalidFilterDefinitionException.ErrorCause.InvalidNumberOfDefinitionElements,
					ex.Cause);

				Assert.IsNull(ex.FilterName);
				Assert.IsNull(ex.Argument);
			}

			try
			{
				Collapse.Export(new Repository(), Res.FileContent("FilterDefinitionInvalidNumberOfElementsTooMany"));
			}
			catch (InvalidFilterDefinitionException ex)
			{
				Assert.AreEqual(
					InvalidFilterDefinitionException.ErrorCause.InvalidNumberOfDefinitionElements,
					ex.Cause);

				Assert.IsNull(ex.FilterName);
				Assert.IsNull(ex.Argument);
			}

			try
			{
				Collapse.Export(new Repository(), Res.FileContent("FilterDefinitionMissingFunctionName"));
			}
			catch (InvalidFilterDefinitionException ex)
			{
				Assert.AreEqual(
					InvalidFilterDefinitionException.ErrorCause.FunctionNameOrArgumentMissing,
					ex.Cause);

				Assert.IsNull(ex.FilterName);
				Assert.IsNull(ex.Argument);
			}

			try
			{
				Collapse.Export(new Repository(), Res.FileContent("FilterDefinitionMissingArguments"));
			}
			catch (InvalidFilterDefinitionException ex)
			{
				Assert.AreEqual(
					InvalidFilterDefinitionException.ErrorCause.FunctionNameOrArgumentMissing,
					ex.Cause);

				Assert.IsNull(ex.FilterName);
				Assert.IsNull(ex.Argument);
			}

			try
			{
				Collapse.Export(new Repository(), Res.FileContent("FilterDefinitionFunctionNameContainsWhitespace"));
			}
			catch (InvalidFilterDefinitionException ex)
			{
				Assert.AreEqual(
					InvalidFilterDefinitionException.ErrorCause.FunctionNameContainsWhitespace,
					ex.Cause);

				Assert.AreEqual("filter name", ex.FilterName);
				Assert.IsNull(ex.Argument);
			}

			try
			{
				Collapse.Export(new Repository(), Res.FileContent("FilterDefinitionArgumentNameContainsWhitespace"));
			}
			catch (InvalidFilterDefinitionException ex)
			{
				Assert.AreEqual(
					InvalidFilterDefinitionException.ErrorCause.ArgumentContainsWhitespace,
					ex.Cause);

				Assert.IsNull(ex.FilterName);
				Assert.AreEqual("argument one", ex.Argument);
			}
		}

		[TestMethod]
		public void InvalidForeachPath()
		{
			try
			{
				Collapse.Export(new Repository(), Res.FileContent("InvalidForeachPathTooFew"));
			}
			catch (InvalidForeachPathException ex)
			{
				Assert.AreEqual("c", ex.Expression);
			}

			try
			{
				Collapse.Export(new Repository(), Res.FileContent("InvalidForeachPathTooMany"));
			}
			catch (InvalidForeachPathException ex)
			{
				Assert.AreEqual("c in Customers in Other", ex.Expression);
			}
		}

		[TestMethod]
		public void InvalidItemPath()
		{
			try
			{
				Collapse.Export(new Repository(), Res.FileContent("InvalidItemPath"));
			}
			catch (InvalidItemPathException ex)
			{
				Assert.AreEqual("Namer", ex.InvalidProperty);
			}

			try
			{
				Collapse.Export(new Repository(), Res.FileContent("InvalidItemPathLong"));
			}
			catch (InvalidItemPathException ex)
			{
				Assert.AreEqual("Leng", ex.InvalidProperty);
			}
		}

		[TestMethod]
		public void VariableAlreadyDefined()
		{
			try
			{
				Collapse.Export(new Repository(), Res.FileContent("VariableAlreadyDefined"));
			}
			catch (VariableAlreadyDefinedException ex)
			{
				Assert.AreEqual("x", ex.Variable);
			}
		}

	}
}