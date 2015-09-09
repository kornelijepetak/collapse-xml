using System;
using System.Linq;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown if the filter definition is invalid
	/// </summary>
	public class InvalidFilterDefinitionException : CollapseXmlException
	{
		/// <summary>
		/// The cause of the exception
		/// </summary>
		public ErrorCause Cause { get; private set; }

		/// <summary>
		/// An expression that caused the exception 
		/// </summary>
		public String Argument { get; private set; }

		/// <summary>
		/// The name of the filter that caused the exception
		/// (in case the exception was caused by the filter name)
		/// </summary>
		public String FilterName { get; private set; }

		/// <summary>
		/// Creates a InvalidFilterDefinitionException
		/// </summary>
		/// <param name="cause">Cause for the exception</param>
		/// <param name="filterName">Name of the filter</param>
		/// <param name="argument">Argument that caused the exception</param>
		public InvalidFilterDefinitionException(ErrorCause cause, String filterName = null, String argument = null)
			: base(createMessage(cause))
		{
			Cause = cause;
			FilterName = filterName;
			Argument = argument;
		}

		/// <summary>
		/// Enumerates possible causes for InvalidFilterDefinitionException
		/// </summary>
		public enum ErrorCause
		{
			/// <summary>
			/// The filter did not contain only a function name or a function name and arguments separated with pipe character.
			/// </summary>
			InvalidNumberOfDefinitionElements,

			/// <summary>
			/// Function name or arguments are missing
			/// </summary>
			FunctionNameOrArgumentMissing,

			/// <summary>
			/// Function name contains whitespace
			/// </summary>
			FunctionNameContainsWhitespace,

			/// <summary>
			/// An argument contains whitespace
			/// </summary>
			ArgumentContainsWhitespace,

			/// <summary>
			/// A requested argument name was not found
			/// </summary>
			ArgumentNameNotFound,
			
			/// <summary>
			/// The argument is not a valid argument
			/// </summary>
			NamedArgumentInvalid
		}

		/// <summary>
		/// Creates an exception message depending on the cause
		/// </summary>
		/// <param name="cause">Cause of the exception</param>
		/// <returns>An exception message</returns>
		private static string createMessage(ErrorCause cause)
		{
			switch (cause)
			{
				case ErrorCause.InvalidNumberOfDefinitionElements:
					return
						"Filter definition must either be only a function name, or a " +
						"function name and a list of arguments separated by a pipe (|) character.";
				case ErrorCause.FunctionNameOrArgumentMissing:
					return "Function or arguments are missing.";
				case ErrorCause.FunctionNameContainsWhitespace:
					return "Function name contains whitespace";
				case ErrorCause.ArgumentContainsWhitespace:
					return "An argument contains whitespace.";
				case ErrorCause.NamedArgumentInvalid:
					return "The argument is not a valid argument.";
				case ErrorCause.ArgumentNameNotFound:
					return "Requested argument is not defined in the template.";
				default:
					return "Filter definition is not in a valid format.";
			}
		}
	}
}
