using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kaisean.Tools.CollapseXml.Helpers;
using Kaisean.Tools.CollapseXml.Errors;
using System.Collections;

namespace Kaisean.Tools.CollapseXml
{
	/// <summary>
	/// Represents a context in which Collapse operations are executed
	/// </summary>
	internal class CollapseContext
	{
		/// <summary>
		/// Creates a Collapse context
		/// </summary>
		/// <param name="rootData">Root object in which all data is stored</param>
		/// <param name="filterMap">A map of filters</param>
		/// <param name="dateTimeFormat">Specifies how to format dates</param>
		/// <param name="numberFormatProvider">Specifies how to format numbers</param>
		internal CollapseContext(
			object rootData,
			Dictionary<String, FilterFunction> filterMap,
			String dateTimeFormat,
			IFormatProvider numberFormatProvider)
		{
			if (filterMap == null)
				filterMap = new Dictionary<string, FilterFunction>();

			this.rootData = rootData;
			this.filterMap = filterMap;
			this.dateTimeFormat = dateTimeFormat;
			this.numberFormatProvider = numberFormatProvider;
		}

		/// <summary>
		/// Returns a textual representation of the object. 
		/// This formats the dates and numeric values, and for all other 
		/// object types, it calles ToString()
		/// </summary>
		/// <param name="obj">Object for which the textual representation is needed</param>
		/// <returns>A textual representation of the object</returns>
		internal String GetTextualRepresentation(Object obj)
		{
			// The order of elements is determined by the expected chance of appearance

			if (obj is int)
				return ((int)obj).ToString(numberFormatProvider);
			
			if (obj is float)
				return ((float)obj).ToString(numberFormatProvider);

			if (obj is double)
				return ((double)obj).ToString(numberFormatProvider);
			
			if (obj is DateTime)
				return ((DateTime)obj).ToString(dateTimeFormat);

			if (obj is long)
				return ((long)obj).ToString(numberFormatProvider);

			if (obj is decimal)
				return ((decimal)obj).ToString(numberFormatProvider);

			if (obj is byte)
				return ((byte)obj).ToString(numberFormatProvider);

			if (obj is short)
				return ((short)obj).ToString(numberFormatProvider);
			
			if (obj is ushort)
				return ((ushort)obj).ToString(numberFormatProvider);

			if (obj is uint)
				return ((uint)obj).ToString(numberFormatProvider);

			if (obj is ulong)
				return ((ulong)obj).ToString(numberFormatProvider);

			return obj.ToString();
		}

		/// <summary>
		/// Determines whether the variable name is defined in current scope
		/// </summary>
		/// <param name="variable">Variable name</param>
		/// <returns>True if the variable name is in current scope</returns>
		internal bool IsVariableDefined(string variable)
		{
			return scopesItemMap.ContainsKey(variable);
		}

		/// <summary>
		/// Removes a single variable from the scope.
		/// The scope is structured as a stack so this effectively
		/// removes a variable from the top of the stack (in nested foreach loops)
		/// </summary>
		internal void RemoveSingleVariableScopeItem()
		{
			if (scopes.Count == 0)
				return;

			scopesItemMap.Remove(scopes.Last().Name);
			scopes.RemoveAt(scopes.Count - 1);
		}

		/// <summary>
		/// Adds a variable onto the scope stack with some data
		/// </summary>
		/// <param name="variable">Variable name</param>
		/// <param name="data">Data to assign to the variable</param>
		internal void SetVariable(String variable, object data)
		{
			if (scopesItemMap.ContainsKey(variable))
			{
				scopesItemMap[variable].Data = data;
			}
			else
			{
				ScopeItem newScope = new ScopeItem(variable, data);
				scopesItemMap[variable] = newScope;
				scopes.Add(newScope);
			}
		}

		/// <summary>
		/// Find a collection by its path
		/// </summary>
		/// <param name="collectionPath">Collection path</param>
		/// <returns>The collection assigned to the element at specified path</returns>
		internal IEnumerable<object> GetCollection(List<String> collectionPath)
		{
			string variable = collectionPath.First();

			// Find object from root with a variable
			object obj = findObjectInScope(variable);
			if (obj == null)
				throw new InvalidCollectionPathException(collectionPath, variable);

			// Skip first element, then try to find object
			foreach (string pathSegment in collectionPath.Skip(1))
			{
				obj = findProperty(obj, pathSegment);
				if (obj == null)
					throw new InvalidCollectionPathException(collectionPath, pathSegment);
			}

			// Now, the obj should be a IEnumerable<T>
			IEnumerable<object> collection = obj as IEnumerable<object>;

			// Note that IEnumerable<T> is not an IEnumerable<object> if T is a value type
			if (collection == null)
			{
				IEnumerable enumerable = obj as IEnumerable;
				if (enumerable == null)
					throw new ObjectNotACollectionException(collectionPath);

				// The value-typed enumerable must be wrapped
				collection = wrapValueTypedIEnumerable(enumerable);
			}

			return collection;
		}

		/// <summary>
		/// Adapts the IEnumerable to IEnumerable of object
		/// </summary>
		/// <param name="enumerable">Enumerable to adapt</param>
		/// <returns>The collection as IEnumerable of object</returns>
		private static IEnumerable<object> wrapValueTypedIEnumerable(IEnumerable enumerable)
		{
			foreach (object item in enumerable)
				yield return item;
		}

		/// <summary>
		/// Checks whether the item should be included in the output
		/// </summary>
		/// <param name="filterDefinition">A filter definition object with name and arguments</param>
		/// <param name="behavior">Specifies what happens when a filter is missing</param>
		/// <returns>True if current item should be included</returns>
		internal bool IsIncludedWithFilter(FilterDefinition filterDefinition, MissingFilterBehavior behavior)
		{
			if (!filterMap.ContainsKey(filterDefinition.Function))
			{
				switch (behavior)
				{
					case MissingFilterBehavior.IncludeItem:
						return true;
					case MissingFilterBehavior.OmitItem:
						return false;
					case MissingFilterBehavior.ThrowException:
					default:
						throw new FilterNotFoundException(filterDefinition.Function);
				}
			}

			// Find a filter
			FilterFunction predicate = filterMap[filterDefinition.Function];

			// Create filter arguments
			IEnumerable<CollapseFilterArgument> arguments =
				filterDefinition.Arguments
				.Select(arg => new CollapseFilterArgument(GetObject(arg.Path), arg.Name));

			// Run the predicate to see if the item should be included
			return predicate(
				new CollapseFilterArgumentCollection(arguments));
		}

		/// <summary>
		/// Gets the object at the specified path
		/// </summary>
		/// <param name="pathSegments">Path from which to get the object</param>
		/// <returns>The object at specified path</returns>
		internal object GetObject(List<String> pathSegments)
		{
			String variable = pathSegments.First();

			object obj = findObjectInScope(variable);
			if (obj == null)
				throw new InvalidItemPathException(pathSegments, variable);

			// Skip first element, then try to find object
			foreach (string pathSegment in pathSegments.Skip(1))
			{
				obj = findProperty(obj, pathSegment);
				if (obj == null)
					throw new InvalidItemPathException(pathSegments, pathSegment);
			}

			return obj;
		}

		/// <summary>
		/// Gets the object at the specified path
		/// </summary>
		/// <param name="path">Path from which to get the object</param>
		/// <returns>The object at specified path</returns>
		internal object GetObject(String path)
		{
			return GetObject(path.TrimmedSplit());
		}

		/// <summary>
		/// Finds the property value
		/// </summary>
		/// <param name="obj">The object whose property is being queried</param>
		/// <param name="name">Name of the property</param>
		/// <returns>The value of the specified property</returns>
		private static object findProperty(object obj, String name)
		{
			PropertyInfo property =
				obj.GetType()
				.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.FirstOrDefault(p => p.Name == name);

			if (property == null)
				return null;

			return property.GetValue(obj, null);
		}


		/// <summary>
		/// Searches for an object in current scope,
		/// by looking at ScopeItems on the stack. 
		/// </summary>
		/// <param name="name">Variable name</param>
		/// <returns>The object that was searched for, or null if the search did not yield a result</returns>
		private object findObjectInScope(String name)
		{
			// First check the root data
			object obj = findObjectInRootData(name);
			if (obj != null)
				return obj;

			for (int i = scopes.Count - 1; i >= 0; i--)
			{
				if (scopes[i].Name == name)
					return scopes[i].Data;
			}

			return null;
		}

		/// <summary>
		/// Gets a property value by searching the root data object
		/// </summary>
		/// <param name="name">Property name</param>
		/// <returns>The value of the root data object's property</returns>
		private object findObjectInRootData(String name)
		{
			PropertyInfo property =
				rootData.GetType()
				.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.FirstOrDefault(p => p.Name == name);

			if (property == null)
				return null;

			return property.GetValue(rootData, null);
		}

		/// <summary>
		/// Root data object
		/// </summary>
		private readonly object rootData;

		/// <summary>
		/// DateTime format used to determine the output of DateTime values
		/// </summary>
		private readonly String dateTimeFormat;

		/// <summary>
		/// Specifies the number format that determines the output of numerical valuse
		/// </summary>
		private readonly IFormatProvider numberFormatProvider;

		/// <summary>
		/// Maps names to corresponding filter functions
		/// </summary>
		private readonly Dictionary<String, FilterFunction> filterMap;

		/// <summary>
		/// Maps names to corresponding scopeItems
		/// </summary>
		private readonly Dictionary<String, ScopeItem> scopesItemMap = new Dictionary<string, ScopeItem>();

		/// <summary>
		/// A stack of scopes
		/// </summary>
		private readonly List<ScopeItem> scopes = new List<ScopeItem>();
	}
}