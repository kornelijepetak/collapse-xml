using System;
using System.Linq;
using System.Collections.Generic;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Thrown if the provided DateTime format is not valid
	/// </summary>
	public class DateTimeFormatInvalidException : CollapseXmlException
	{
		/// <summary>
		/// Format provided that caused the exception to be thrown. 
		/// This property is an empty if the format provider caused the error.
		/// </summary>
		public String Format { get; private set; }

		/// <summary>
		/// Creates a DateTimeFormatInvalidException
		/// </summary>
		/// <param name="format">Format that caused the error, if available.</param>
		internal DateTimeFormatInvalidException(String format = null)
			: base(createMessage(format))
		{
			Format = format ?? "";
		}

		/// <summary>
		/// Creates a message depending
		/// </summary>
		/// <param name="format">Format provided, null if a format provider caused the error</param>
		/// <returns>An exception message</returns>
		private static string createMessage(String format)
		{
			return 
				format == null 
				? "Specified format provider failed to provide a valid format." 
				: "The format specified is not a valid DateTime format.";
		}
	}
}
