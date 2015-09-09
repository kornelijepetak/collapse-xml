using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Kaisean.Tools.CollapseXml
{
	public partial class Collapse
	{
		/// <summary>
		/// Exports the <paramref name="rootData"/> as XML document, using the <paramref name="template"/> template.
		/// </summary>
		/// <param name="rootData">Object to export</param>
		/// <param name="template">Collapse transformation template</param>
		/// <returns>Transformed XML filled with data, returned as an XDocument</returns>
		public static XDocument Export(object rootData, String template)
		{
			return new Collapse().ExportToXml(rootData, template);
		}

	}
}
