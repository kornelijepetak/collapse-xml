using System;
using System.Linq;
using System.Collections.Generic;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown if inline cx| path is not defined
	/// </summary>
	public class MissingPathException : CollapseXmlException
	{
		/// <summary>
		/// Creates a MissingPathException
		/// </summary>
		public MissingPathException()
			: base("Path missing in inline cx value element.")
		{
		}
	}
}
