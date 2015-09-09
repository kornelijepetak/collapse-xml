using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaisean.Tools.CollapseXml
{
	/// <summary>
	/// Determines what to do if a filter is expected but has not been defined
	/// </summary>
	public enum MissingFilterBehavior
	{
		/// <summary>
		/// CollapseXML will throw a FilterNotFoundException if a filter is not defined
		/// </summary>
		ThrowException,

		/// <summary>
		/// Item will be included if a filter is not defined
		/// </summary>
		IncludeItem,

		/// <summary>
		/// Item will be omitted if a filter is not defined
		/// </summary>
		OmitItem
	}
}
