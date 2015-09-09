using System;
using System.Linq;
using System.Collections.Generic;

namespace Kaisean.Tools.CollapseXml.Errors
{
	/// <summary>
	/// Base class for all CollapseXML exceptions
	/// </summary>
	public abstract class CollapseXmlException : Exception
	{
		/// <summary>
		/// Creates an instance of CollapseXMLException
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="innerException">Inner exception</param>
		public CollapseXmlException(
			String message = "Unknown CollapseXML error.",
			Exception innerException = null)
			: base(message, innerException)
		{
		}
	}
}
