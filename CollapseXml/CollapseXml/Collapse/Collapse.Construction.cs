using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Kaisean.Tools.CollapseXml
{
	public partial class Collapse
	{
		/// <summary>
		/// Initializes the CollapseXML object
		/// </summary>
		public Collapse(
			MissingFilterBehavior missingFilterBehavior = MissingFilterBehavior.ThrowException,
			bool transformInlineText = true)
		{
			IsInlineTextTransformEnabled = transformInlineText;
			MissingFilterBehavior = missingFilterBehavior;

			dateTimeFormat = "";
			numberFormatProvider = CultureInfo.CurrentCulture;
		}
	}
}
