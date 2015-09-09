using System;
using System.Linq;
using System.Collections.Generic;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown if a cx:value element does not contain a "source" attribute
	/// </summary>
	public class MissingSourceAttributeOnValueElementException : CollapseXmlException
	{
		/// <summary>
		/// Creates a MissingSourceAttributeOnValueElementException
		/// </summary>
		public MissingSourceAttributeOnValueElementException()
			: base("Missing source attribute on cx:value element.")
		{
		}
	}
}
