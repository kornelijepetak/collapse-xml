using System;
using System.Collections.Generic;
using System.Linq;
using Kaisean.Tools.CollapseXml.Helpers;
using Kaisean.Tools.CollapseXml.Errors;

namespace Kaisean.Tools.CollapseXml
{
	internal class ForEachDefinition
	{
		private ForEachDefinition(String variable, String collection)
		{
			CollectionPath = collection.TrimmedSplit();
			Variable = variable;
			Collection = collection;
		}

		internal static ForEachDefinition FromExpression(String expression)
		{
			List<string> tokens = expression.TrimmedSplit(" in ");

			if (tokens.Count != 2)
				throw new InvalidForeachPathException(expression);

			return new ForEachDefinition(tokens[0], tokens[1]);
		}

		internal String Variable { get; private set; }
		internal String Collection { get; private set; }

		internal List<String> CollectionPath { get; private set; }
	}
}