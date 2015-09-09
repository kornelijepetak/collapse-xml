using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Kaisean.Tools.CollapseXml.Errors;

namespace Kaisean.Tools.CollapseXml
{
	/// <summary>
	/// A collection of filter arguments
	/// </summary>
	public class CollapseFilterArgumentCollection : IEnumerable<CollapseFilterArgument>
	{
		/// <summary>
		/// Creates a CollapseFilterArgumentCollection
		/// </summary>
		/// <param name="args">A collection of arguments</param>
		internal CollapseFilterArgumentCollection(IEnumerable<CollapseFilterArgument> args)
		{
			arguments = new List<CollapseFilterArgument>(args);
			argumentsMap = arguments
				.Where(a => !string.IsNullOrEmpty(a.Variable))
				.ToDictionary(a => a.Variable, a => a);
		}

		/// <summary>
		/// Gets the filter argument by its index
		/// </summary>
		/// <param name="index">0-based index of a filter argument, as stated in XML template</param>
		/// <returns>The filter argument at specified index</returns>
		public CollapseFilterArgument this[int index]
		{
			get
			{
				return arguments[index];
			}
		}

		/// <summary>
		/// Gets the filter argument by its name
		/// </summary>
		/// <param name="namedArgument">The name of the argument, as stated in filter in template</param>
		/// <returns>The filter argument with the specified name</returns>
		public CollapseFilterArgument this[string namedArgument]
		{
			get
			{
				if (argumentsMap.ContainsKey(namedArgument))
					return argumentsMap[namedArgument];

				throw new InvalidFilterDefinitionException(
					InvalidFilterDefinitionException.ErrorCause.ArgumentNameNotFound,
					argument: namedArgument);
			}
		}

		/// <summary>
		/// The list of arguments
		/// </summary>
		private readonly List<CollapseFilterArgument> arguments;

		/// <summary>
		/// Maps argument names
		/// </summary>
		private readonly Dictionary<String, CollapseFilterArgument> argumentsMap;

		#region IEnumerable

		/// <summary>
		/// Returns the enumerator for arguments
		/// </summary>
		/// <returns>The enumerator for arguments</returns>
		public IEnumerator<CollapseFilterArgument> GetEnumerator()
		{
			return arguments.GetEnumerator();
		}

		/// <summary>
		/// Returns the enumerator for arguments
		/// </summary>
		/// <returns>The enumerator for arguments</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return arguments.GetEnumerator();
		}
		#endregion
	}
}
