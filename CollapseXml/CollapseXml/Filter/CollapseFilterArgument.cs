using System;
using System.Linq;
using System.Collections.Generic;

namespace Kaisean.Tools.CollapseXml
{
	/// <summary>
	/// Represents a single argument in filter function
	/// </summary>
	public class CollapseFilterArgument
	{
		/// <summary>
		/// Creates a CollapseFilterArgument 
		/// </summary>
		/// <param name="obj">Object that is used for this argument</param>
		/// <param name="variable">Argument name</param>
		public CollapseFilterArgument(object obj, String variable)
		{
			Data = obj;
			Variable = variable;
		}

		/// <summary>
		/// Gets an underlying object cast to the desired type
		/// </summary>
		/// <typeparam name="T">Desired object type</typeparam>
		/// <returns>An underlying object cast to the desired type</returns>
		public T Value<T>()
		{
			Type requestedType = typeof(T);

			if (Data == null)
			{
				// If Data is null, a type can't be determined, but if the 
				// requested data type is a reference type or an interface
				// a null can safely be returned (since the data is null)
				if (requestedType.IsClass || requestedType.IsInterface)
					return default(T);
			
				// The Data is null, but the requested value is a Value Type
				// an exception should be thrown.
				throw new InvalidOperationException("Null value (found in Data) can't be converted to");
			}

			try
			{
				return (T)Data;
			}
			catch
			{
				throw new InvalidCastException(
					String.Format("Unable to cast an object from {0} to {1}.", 
					Data.GetType().Name, 
					requestedType.Name));
			}

		}
		
		/// <summary>
		/// Underlying object
		/// </summary>
		public object Data { get; private set; }

		/// <summary>
		/// Argument name
		/// </summary>
		public String Variable { get; private set; }
	}
}
