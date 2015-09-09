using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown if a an output root element contains a cx:foreach attribute
	/// </summary>
	public class RootElementIsAForeachException : CollapseXmlException
	{
		/// <summary>
		/// Creates a RootElementIsAForeachException
		/// </summary>
		public RootElementIsAForeachException()
			: base("Template root element must not contain a cx:foreach attribute.")
		{
		}
	}
}
