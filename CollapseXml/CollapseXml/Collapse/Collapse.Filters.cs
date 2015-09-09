using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Kaisean.Tools.CollapseXml.Helpers;

namespace Kaisean.Tools.CollapseXml
{
	public partial class Collapse
	{
		/// <summary>
		/// Adds a filter 
		/// </summary>
		/// <param name="name">Filter name</param>
		/// <param name="filterFunction">Function that returns true for elements that should be included in the output</param>
		/// <returns>A Collapse instance, used for chaining</returns>
		public Collapse SetFilter(string name, FilterFunction filterFunction)
		{
			if (filterMap == null)
				filterMap = new Dictionary<string, FilterFunction>();

			filterMap[name] = filterFunction;

			return this;
		}

		/// <summary>
		/// Determines whether an element should be ommited from output
		/// depending on the cx:filter function
		/// </summary>
		/// <param name="sourceElement">Element to check</param>
		/// <returns>True if the element should not be output, false otherwise</returns>
		private bool isElementFilteredOut(XElement sourceElement)
		{
			// Test conditions here
			if (sourceElement.IsCollapseFilter())
			{
				FilterDefinition filterDefinition = sourceElement.ToFilterDefinition();

				if (!context.IsIncludedWithFilter(filterDefinition, MissingFilterBehavior))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Determines whether an attribute should be ommited from output
		/// depending on the cx:filter-* function
		/// </summary>
		/// <param name="expression">Attribute expression</param>
		/// <returns>True if the attribute should not be output, false otherwise</returns>
		private bool isAttributeFilteredOut(String expression)
		{
			FilterDefinition filterDefinition = FilterDefinition.FromExpression(expression);

			if (!context.IsIncludedWithFilter(filterDefinition, MissingFilterBehavior))
				return true;

			return false;
		}
	}
}