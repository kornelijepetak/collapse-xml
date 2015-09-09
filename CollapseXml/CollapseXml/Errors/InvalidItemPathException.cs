using System;
using System.Linq;
using System.Collections.Generic;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown when the Item path is invalid or the object with that name can't be found
	/// </summary>
	public class InvalidItemPathException : CollapseXmlException
	{
		/// <summary>
		/// Creates an InvalidItemPathException
		/// </summary>
		/// <param name="path">The path that has caused the exception</param>
		/// <param name="invalidProperty">The invalid part of the path</param>
		public InvalidItemPathException(List<String> path, String invalidProperty) :
			base("Invalid item path: " + String.Join(".", path))
		{
			InvalidProperty = invalidProperty;
		}

		/// <summary>
		/// The invalid part of the path
		/// </summary>
		public String InvalidProperty { get; private set; }
	}
}
