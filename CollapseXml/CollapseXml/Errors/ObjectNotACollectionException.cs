using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown when the collection path references an object that is not an IEnumerable
	/// </summary>
	public class ObjectNotACollectionException : CollapseXmlException
	{
		/// <summary>
		/// Creates an ObjectNotACollectionException
		/// </summary>
		/// <param name="path">The path that has caused the exception</param>
		public ObjectNotACollectionException(List<String> path) :
			base(createMessage(path)) { }

		/// <summary>
		/// Creates a message for the exception, depending on the path
		/// </summary>
		/// <param name="path">Path that caused the error</param>
		/// <returns>An exception message</returns>
		private static string createMessage(List<String> path)
		{
			return String.Format("The object accessed at {0} is not a collection.", String.Join(".", path));
		}
	}
}
