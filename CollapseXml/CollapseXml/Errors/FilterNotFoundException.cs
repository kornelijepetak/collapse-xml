using System;
using System.Linq;
using System.Collections.Generic;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown if the specified filter is not found and the MissingFilterBehavior is set to ThrowException
	/// </summary>
	public class FilterNotFoundException : CollapseXmlException
	{
		/// <summary>
		/// Filter name that was requested but was not found
		/// </summary>
		public String FilterName { get; private set; }

		/// <summary>
		/// Creates a FilterNotFoundException 
		/// </summary>
		/// <param name="filterName">Requested filter name</param>
		public FilterNotFoundException(String filterName) :
			base("The requested filter was not found.")
		{
			FilterName = filterName;
		}
	}
}