using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaisean.Tools.CollapseXml
{
	/// <summary>
	/// Represents a variable currently available in scope and its data
	/// </summary>
	internal class ScopeItem
	{
		/// <summary>
		/// Creates a scope item
		/// </summary>
		/// <param name="variableName">Name of the variable</param>
		/// <param name="data">Data for the object</param>
		internal ScopeItem(String variableName, object data)
		{
			Name = variableName;
			Data = data;
		}

		/// <summary>
		/// Name of the variable 
		/// </summary>
		internal String Name { get; private set; }

		/// <summary>
		/// The value of the variable in current scope
		/// </summary>
		internal object Data { get; set; }
	}
}
