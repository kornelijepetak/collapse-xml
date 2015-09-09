using System;
using System.Linq;
using System.Collections.Generic;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown when the Collection path is invalid or the object with that name can't be found
	/// </summary>
	public class InvalidCollectionPathException : CollapseXmlException
	{
		/// <summary>
		/// Creates an InvalidCollectionPathException
		/// </summary>
		/// <param name="path">The path that has caused the exception</param>
		/// <param name="invalidProperty">The invalid part of the path</param>
		internal InvalidCollectionPathException(List<String> path, String invalidProperty) :
			base("Invalid collection path: " + String.Join(".", path))
		{
			InvalidProperty = invalidProperty;
		}

		/// <summary>
		/// The invalid part of the path
		/// </summary>
		public String InvalidProperty { get; private set; }
	}
}
