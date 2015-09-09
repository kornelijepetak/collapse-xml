using System;
using System.Collections.Generic;
using System.Linq;
using Kaisean.Tools.CollapseXml.Helpers;
using System.Xml.Linq;
using Kaisean.Tools.CollapseXml.Errors;

namespace Kaisean.Tools.CollapseXml
{
	internal class CollapseValueDefinition
	{
		private CollapseValueDefinition(String text)
		{
			Text = text;
		}

		internal static CollapseValueDefinition FromXElement(XElement element, CollapseContext context)
		{
			if (!element.IsCollapseValue())
				return null;

			XAttribute sourceAttribute = element.Attribute("source");

			if (sourceAttribute == null)
				throw new MissingSourceAttributeOnValueElementException();

			object obj = context.GetObject(sourceAttribute.Value.TrimmedSplit());

			String valueText = context.GetTextualRepresentation(obj);

			return new CollapseValueDefinition(valueText);
		}

		public String Text { get; private set; }
	}
}
