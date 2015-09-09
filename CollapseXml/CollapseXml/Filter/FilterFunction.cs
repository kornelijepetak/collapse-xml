using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaisean.Tools.CollapseXml
{
	/// <summary>
	/// Predicate that decides whether the XML element or attribute should be included into 
	/// resulting XML, depending on the filter arguments
	/// </summary>
	/// <param name="args">Filter arguments</param>
	/// <returns>True if an item should be inlcuded in the resulting XML</returns>
	public delegate bool FilterFunction(CollapseFilterArgumentCollection args);
}
