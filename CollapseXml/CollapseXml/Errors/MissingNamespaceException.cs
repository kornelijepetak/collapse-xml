using System;
using System.Linq;
using System.Collections.Generic;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown if a template's root element does not contain a CX namespace
	/// </summary>
	public class MissingNamespaceException : CollapseXmlException
	{
		/// <summary>
		/// Creates a MissingNamespaceException
		/// </summary>
		public MissingNamespaceException()
			: base("Template root element does not contain CollapseXML namespace.")
		{
		}
	}
}
