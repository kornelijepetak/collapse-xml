using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaisean.Tools.CollapseXml
{
	public partial class Collapse
	{
		/// <summary>
		/// Decides whether the inline (cx|path) elements are transformed.
		/// </summary>
		public bool IsInlineTextTransformEnabled { get; set; }
		
		/// <summary>
		/// Decides how Collapse behaves when encountering missing filter.
		/// Default value is ThrowException.
		/// </summary>
		public MissingFilterBehavior MissingFilterBehavior { get; set; }

		/// <summary>
		/// A predefined XML namespace for the CollapseXML
		/// </summary>
		internal readonly static String CollapseNamespace = "http://kaisean.com/tools/collapse";

		/// <summary>
		/// The context that stores the scopes stack during the code generation
		/// </summary>
		private CollapseContext context;
		
		/// <summary>
		/// Maps filter names to filter functions
		/// </summary>
		private Dictionary<String, FilterFunction> filterMap;

		/// <summary>
		/// Defines a DateTime format
		/// </summary>
		private String dateTimeFormat;

		/// <summary>
		/// Defines a Number format provider
		/// </summary>
		private IFormatProvider numberFormatProvider;
	}
}
