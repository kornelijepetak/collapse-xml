using System;
using System.Linq;
using System.Collections.Generic;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown if a template is not a valid XML element
	/// </summary>
	public class InvalidXmlFormatException : CollapseXmlException
	{
		/// <summary>
		/// Creates an InvalidXmlFormatException
		/// </summary>
		public InvalidXmlFormatException()
			: base("A template is not a valid XML document.")
		{

		}
	}
}
