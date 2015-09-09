using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown if a template's root element contains more than one element.
	/// The root element must contain only a single element because that element
	/// will become the new root element in output XML.
	/// </summary>
	public class MultipleRootElementsException : CollapseXmlException
	{
		/// <summary>
		/// Creates a MultipleRootElementsException
		/// </summary>
		public MultipleRootElementsException()
			: base("Template root element must contain only a single element.")
		{
		}
	}
}
