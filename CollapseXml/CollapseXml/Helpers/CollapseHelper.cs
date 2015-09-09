using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Kaisean.Tools.CollapseXml.Helpers
{
	internal static class CollapseHelper
	{
		internal static bool IsCollapseForeach(this XElement element)
		{
			return element.isCollapseAttribute("foreach");
		}

		internal static bool IsCollapseFilter(this XElement element)
		{
			return element.isCollapseAttribute("filter");
		}

		internal static bool IsCollapseValue(this XElement element)
		{
			return element.isCollapseElement("value");
		}

		private static bool isCollapseAttribute(this XElement element, String attributeName)
		{
			return element
				.Attributes()
				.Any(a =>
					a.Name.LocalName == attributeName
					&& a.Name.NamespaceName == Collapse.CollapseNamespace);
		}

		private static bool isCollapseElement(this XElement element, String elementName)
		{
			return element.Name.LocalName == elementName &&
				element.Name.NamespaceName == Collapse.CollapseNamespace;
		}

		internal static ForEachDefinition ToForeachDefinition(this XElement element)
		{
			return ForEachDefinition.FromExpression(element.CollapseAttribute("foreach").Value);
		}

		internal static FilterDefinition ToFilterDefinition(this XElement element)
		{
			return FilterDefinition.FromExpression(element.CollapseAttribute("filter").Value);
		}

		internal static XAttribute CollapseAttribute(this XElement element, String name)
		{
			return element.Attribute(XName.Get(name, Collapse.CollapseNamespace));
		}

		/// <summary>
		///	Creates an XML element that is a copy of another one, but does not
		///	contain its content. It only contains non-Collapse attributes
		/// </summary>
		/// <param name="element">Original XML element</param>
		/// <returns>
		///	A new XML element with name and non-collapse attsributes 
		///	copied from the original.
		/// </returns>
		internal static XElement AsXElementFrame(this XElement element)
		{
			XElement newElement = new XElement(element.Name);

			newElement.Add(
				element.Attributes()
				.Where(a => a.Name.NamespaceName != Collapse.CollapseNamespace)
				.Where(a => !a.Value.StartsWith("cx|"))
				.ToArray());

			return newElement;
		}

		internal static List<String> TrimmedSplit(this String text, params string[] delimiter)
		{
			if (delimiter == null || delimiter.Length == 0)
				delimiter = new string[] { "." };

			return text
				.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
				.Select(t => t.Trim())
				.ToList();
		}

		internal static void AddAttribute(this XElement element, XName attributeName, Object objValue)
		{
			element.Add(new XAttribute(attributeName, objValue.ToString()));
		}

		/// <summary>
		/// Returns true if the string contains any whitespace character (determined with Char.IsWhiteSpace)
		/// </summary>
		/// <param name="str">String to check</param>
		/// <returns>True if the string contains whitespace, false otherwise</returns>
		internal static bool ContainsWhitespace(this String str)
		{
			return str.Any(c => char.IsWhiteSpace(c));
		}
	}
}
