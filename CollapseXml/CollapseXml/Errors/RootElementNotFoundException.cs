using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown when output root element is not found in the template
	/// </summary>
	public class RootElementNotFoundException : CollapseXmlException
	{
		/// <summary>
		/// Creates a RootElementNotFoundException
		/// </summary>
		public RootElementNotFoundException()
			: base("Root element not found in the template.")
		{
		}
	}
}
