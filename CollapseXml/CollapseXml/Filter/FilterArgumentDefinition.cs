using System;
using System.Linq;
using System.Collections.Generic;
using Kaisean.Tools.CollapseXml.Errors;
using Kaisean.Tools.CollapseXml.Helpers;

namespace Kaisean.Tools.CollapseXml
{
	/// <summary>
	/// Defines a single filter argument
	/// </summary>
	internal class FilterArgumentDefinition
	{
		/// <summary>
		/// Creates a FilterArgumentDefinition
		/// </summary>
		/// <param name="name">Argument name. It is null if it is an unnamed argument (location based)</param>
		/// <param name="path">The path for the argument</param>
		internal FilterArgumentDefinition(String name, String path)
		{
			Name = name;
			Path = path;
		}

		/// <summary>
		/// Creates a filter argument definition from the filter argument expression
		/// </summary>
		/// <param name="argumentExpression">Expression from which to create the argument definition</param>
		/// <returns>A filter argument definition read from the expression</returns>
		internal static FilterArgumentDefinition FromArgumentExpression(String argumentExpression)
		{
			List<string> tokens = argumentExpression.TrimmedSplit(":");

			if (tokens.Count == 0 || tokens.Count >= 3)
				throw new InvalidFilterDefinitionException(
					InvalidFilterDefinitionException.ErrorCause.NamedArgumentInvalid);

			String argument = tokens.First();

			if (tokens.Count == 1)
			{
				if (argument.ContainsWhitespace())
					throw new InvalidFilterDefinitionException(
						InvalidFilterDefinitionException.ErrorCause.ArgumentContainsWhitespace, 
						argument: argument);

				return new FilterArgumentDefinition(null, argument);
			}

			return new FilterArgumentDefinition(argument, tokens[1]);
		}

		/// <summary>
		/// Argument name
		/// </summary>
		internal String Name { get; private set; }

		/// <summary>
		/// The path for the argument
		/// </summary>
		internal String Path { get; private set; }
	}
}
