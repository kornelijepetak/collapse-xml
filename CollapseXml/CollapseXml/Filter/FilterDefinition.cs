using System;
using System.Collections.Generic;
using System.Linq;
using Kaisean.Tools.CollapseXml.Helpers;
using Kaisean.Tools.CollapseXml.Errors;

namespace Kaisean.Tools.CollapseXml
{
	internal class FilterDefinition
	{
		/// <summary>
		/// Function name
		/// </summary>
		internal String Function { get; private set; }

		/// <summary>
		/// A list of filter arguments
		/// </summary>
		internal List<FilterArgumentDefinition> Arguments { get; private set; }

		/// <summary>
		/// Creates a filter definition
		/// </summary>
		/// <param name="function"></param>
 		private FilterDefinition(String function)
		{
			Function = function;
			Arguments = new List<FilterArgumentDefinition>();
		}

		/// <summary>
		/// Retrieves the filter definition from the expression
		/// </summary>
		/// <param name="expression">Filter expression</param>
		/// <returns>A filter definition based on an expression</returns>
		internal static FilterDefinition FromExpression(String expression)
		{
			List<string> filterTokens = expression.TrimmedSplit("|");

			// Invalid number of segments
			if (filterTokens.Count != 1 && filterTokens.Count != 2)
				throw new InvalidFilterDefinitionException(
					InvalidFilterDefinitionException.ErrorCause.InvalidNumberOfDefinitionElements);

			// Missing function name or arguments
			if (filterTokens.Count == 1 && expression.Contains("|"))
				throw new InvalidFilterDefinitionException(
					InvalidFilterDefinitionException.ErrorCause.FunctionNameOrArgumentMissing);

			string functionName = filterTokens.First();

			if (functionName.ContainsWhitespace())
				throw new InvalidFilterDefinitionException(
					InvalidFilterDefinitionException.ErrorCause.FunctionNameContainsWhitespace,
					functionName);

			FilterDefinition definition = new FilterDefinition(functionName);

			if (filterTokens.Count == 2)
			{
				List<string> arguments = filterTokens[1].TrimmedSplit(",");

				List<FilterArgumentDefinition> argumentDefinitions = 
					arguments
					.Select(a => FilterArgumentDefinition.FromArgumentExpression(a))
					.ToList();

				definition.Arguments.AddRange(argumentDefinitions);
			}

			return definition;
		}
	}
}
