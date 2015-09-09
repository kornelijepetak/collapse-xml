using System;
using System.Linq;
using System.Collections.Generic;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown when cx:foreach path is not in a valid format.
	/// Valid format is "item in collection", where "item" is a variable named used 
	/// in the scope, and "collection" is path to the collection object.
	/// </summary>
	public class InvalidForeachPathException : CollapseXmlException
	{
		/// <summary>
		/// The foreach expression that caused the exception
		/// </summary>
		public String Expression { get; private set; }

		/// <summary>
		/// Creates an InvalidForeachPathException
		/// </summary>
		/// <param name="expression">An expression that caused the exception</param>
		public InvalidForeachPathException(String expression)
			: base("cx:foreach path is not in a valid format (item in collection).")
		{
			Expression = expression;
		}
	}
}
