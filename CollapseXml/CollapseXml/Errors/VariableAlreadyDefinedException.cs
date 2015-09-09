using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown if cx:foreach path specifies an identifier that is already defined in the scope.
	/// </summary>
	public class VariableAlreadyDefinedException : CollapseXmlException
	{
		/// <summary>
		/// The variable that caused the exception
		/// </summary>
		public String Variable { get; private set; }

		/// <summary>
		/// Creates a VariableAlreadyDefinedException
		/// </summary>
		/// <param name="variable"></param>
		public VariableAlreadyDefinedException(String variable)
			: base(createMessage(variable))
		{
			Variable = variable;
		}

		/// <summary>
		/// Creates an exception message depending on the variable
		/// </summary>
		/// <param name="variable">Variable name that caused an exception</param>
		/// <returns>An exception message</returns>
		private static string createMessage(String variable)
		{
			return String.Format("Variable '{0}' is already defined in this scope.", variable);
		}
	}
}
