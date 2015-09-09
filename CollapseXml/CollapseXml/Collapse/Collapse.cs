using Kaisean.Tools.CollapseXml.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Kaisean.Tools.CollapseXml.Helpers;

namespace Kaisean.Tools.CollapseXml
{
	/// <summary>
	/// Main class that exports the XML template into a resulting XML
	/// </summary>
	public partial class Collapse
	{
		/// <summary>
		/// Exports the <paramref name="rootData"/> as XML document, using the <paramref name="template"/> template.
		/// </summary>
		/// <exception cref="Kaisean.Tools.CollapseXml.Errors.InvalidXmlFormatException">XML document is not a valid XML.</exception>
		/// <exception cref="Kaisean.Tools.CollapseXml.Errors.MissingNamespaceException">Collapse namespace is missing from the template root element.</exception>
		/// <exception cref="Kaisean.Tools.CollapseXml.Errors.MultipleRootElementsException">Multiple root elements have been defined.</exception>
		/// <exception cref="Kaisean.Tools.CollapseXml.Errors.RootElementNotFoundException">A template does not contain a root element.</exception>
		/// <exception cref="Kaisean.Tools.CollapseXml.Errors.RootElementIsAForeachException">Root element in template contains cx:foreach attribute.</exception>
		/// <param name="rootData">Object to export</param>
		/// <param name="template">Collapse transformation template</param>
		/// <returns>Transformed XML filled with data, returned as an XDocument</returns>
		public XDocument ExportToXml(object rootData, string template)
		{
			// Create a context that contains the data provided
			context = new CollapseContext(rootData, filterMap, dateTimeFormat, numberFormatProvider);

			// A new document is created
			XDocument newDocument = new XDocument();

			// Get the template root element (root element in the resulting XML)
			XElement templateRoot = getXTemplate(template);

			// Create a frame for the template root and add it to the new document
			XElement newRootElement = templateRoot.AsXElementFrame();
			newDocument.Add(newRootElement);

			// Processes the node - in parallel : an original node and its frame
			processElement(templateRoot, newRootElement, null);

			// Return the created document
			return newDocument;
		}

		/// <summary>
		/// Processes the node. The node provided is checked for its type and content, 
		/// and an appropriate action is taken. Then for each child of the source node, 
		/// the method is recursively called.
		/// </summary>
		/// <param name="sourceElement">Source XML element of the template</param>
		/// <param name="destinationElement">Destination XML frame element</param>
		/// <param name="parent">Parent of the destination element (in XML tree that is being generated)</param>
		private void processElement(XElement sourceElement, XElement destinationElement, XElement parent)
		{
			if (sourceElement.IsCollapseValue())
			{
				processValue(sourceElement, parent);
			}
			else if (sourceElement.IsCollapseForeach())
			{
				processForeach(sourceElement, parent);
			}
			else
			{
				// This else works for regular XML tags that just need 
				// to be copied to the destination XML

				// Filter out the elements that should not be included in output
				if (isElementFilteredOut(sourceElement))
					return;

				if (parent != null)
					parent.Add(destinationElement);

				// Transform attributes for the XML tag
				transformAttributes(sourceElement, destinationElement);

				/* 
				 * Process all the children recursively.
				 * 
				 * The reason this recursion doesn't directly goes to processNode at this point
				 * is that children of the XElement can be various types which are not
				 * XElements (XText for one). This way the processing of the children
				 * is separated in another method.
				 */
				processChildrenRecursively(sourceElement, destinationElement);
			}
		}

		/// <summary>
		/// Processes the value element.
		/// The algorithm replaces the Value element with a new XText node
		/// whose content is calculated from the path defined in the source element's attribute.
		/// </summary>
		/// <param name="sourceElement">Value XML element in the source</param>
		/// <param name="parent">Parent element in which to put the new TextNode</param>
		private void processValue(XElement sourceElement, XElement parent)
		{
			if (isElementFilteredOut(sourceElement))
				return;

			CollapseValueDefinition val = CollapseValueDefinition.FromXElement(sourceElement, context);
			XText text = new XText(val.Text);
			parent.Add(text);
		}

		/// <summary>
		/// Processes a foreach element.
		/// 
		/// The collection is read from the data and for each instance, an
		/// XML element defined by the <paramref name="sourceElement"/> is created
		/// and placed into parent element.
		/// </summary>
		/// <param name="sourceElement">Foreach XML element in the source</param>
		/// <param name="parent">Parent element in which to put the sequence of elements</param>
		private void processForeach(XElement sourceElement, XElement parent)
		{
			ForEachDefinition forEach = sourceElement.ToForeachDefinition();

			if (context.IsVariableDefined(forEach.Variable))
				throw new VariableAlreadyDefinedException(forEach.Variable);

			IEnumerable<object> collection = context.GetCollection(forEach.CollectionPath);

			// Foreach object in collection ...
			foreach (object item in collection)
			{
				context.SetVariable(forEach.Variable, item);

				// Test conditions here
				if (isElementFilteredOut(sourceElement))
					continue;

				// Fill Attributes here
				XElement forEachElementFrame = sourceElement.AsXElementFrame();
				transformAttributes(sourceElement, forEachElementFrame);

				// Attach an item to the parent
				parent.Add(forEachElementFrame);

				// Call recursively here (so the context remains valid)
				processChildrenRecursively(sourceElement, forEachElementFrame);
			}

			context.RemoveSingleVariableScopeItem();
		}

		/// <summary>
		/// Transforms the attributes that start with cx|
		/// </summary>
		/// <param name="source">Source element whose attributes needs to be transformed</param>
		/// <param name="destination">Destination XML element in which to add transformed attributes</param>
		private void transformAttributes(XElement source, XElement destination)
		{
			// Fetch only those that start with cx| but are not in cx namespace
			List<XAttribute> attributesToTransform =
				source.Attributes()
				.Where(a => a.Name.NamespaceName != Collapse.CollapseNamespace)
				.Where(a => a.Value.StartsWith("cx|"))
				.ToList();

			// Get all cx:filter-* attributes
			Dictionary<String, String> filterAttributesMap = source.Attributes()
				.Where(a => a.Name.Namespace == Collapse.CollapseNamespace)
				.Where(a => a.Name.LocalName.StartsWith("filter-"))
				.ToDictionary(a => a.Name.LocalName.Substring("filter-".Length), a => a.Value);

			// For each such element...
			foreach (XAttribute attribute in attributesToTransform)
			{
				string attributeName = attribute.Name.LocalName;

				// Find the appropriate cx:filter-* attribute
				if (filterAttributesMap.ContainsKey(attributeName))
				{
					string expression = filterAttributesMap[attributeName];
					if (isAttributeFilteredOut(expression))
						continue;
				}

				// ... get path and...
				string path = attribute.Value.Substring("cx|".Length).Trim();
				if (path.Length == 0)
					throw new MissingPathException();

				// ... copy the transformed attribute.
				object obj = context.GetObject(path);
				destination.AddAttribute(attribute.Name, context.GetTextualRepresentation(obj));
			}
		}

		/// <summary>
		/// Processes the children of the <paramref name="sourceElement"/> depending on their types
		/// </summary>
		/// <param name="sourceElement">Source element in the template XML</param>
		/// <param name="parentElement">Generated XML element in which to add processed children of the source element</param>
		private void processChildrenRecursively(XElement sourceElement, XElement parentElement)
		{
			// Process each child node in source element.
			foreach (XNode childNode in sourceElement.Nodes())
			{
				if (childNode is XElement)
				{
					// For elements, simply set up the new frame and call the processNode recursively

					XElement childElement = childNode as XElement;
					XElement childElementFrame = childElement.AsXElementFrame();
					processElement(childElement, childElementFrame, parentElement);
				}
				else if (childNode is XText)
				{
					// For texts, create and process a text node
					parentElement.Add(processTextNode(childNode as XText));
				}
			}
		}

		/// <summary>
		/// Processes a text node. 
		/// Text node can contain placeholders for values in format (cx|path).
		/// If your data has a chance to contain text in such format so that the parsing would be
		/// ambiguous, please turn off the Inline Text Transform.
		/// </summary>
		/// <param name="textNode">A text node to process</param>
		/// <returns>A XText node with processed text</returns>
		private XText processTextNode(XText textNode)
		{
			// Skip transformation if Inline Text Transform is disabled
			if (IsInlineTextTransformEnabled == false)
				return new XText(textNode.Value);

			string newText = replaceInlineTextElements(textNode.Value);

			return new XText(newText);
		}

		/// <summary>
		/// Replaces inline text elements with appropriate values from the data
		/// </summary>
		/// <param name="text">Element's text content</param>
		/// <returns>The transformed element content</returns>
		private String replaceInlineTextElements(String text)
		{
			const String prefix = "(cx|";

			StringBuilder textBuilder = new StringBuilder();
			int startIndex = 0;

			// TODO: Consider changing this method to regular expression
			while (true)
			{
				// Find first
				int cxValueStartIx = text.IndexOf(prefix, startIndex);
				if (cxValueStartIx < 0)
					break;

				// Find next closing parenthesis
				int cxValueEndIx = text.IndexOf(")", cxValueStartIx);

				// Get the content between
				string substring = text.Substring(
					cxValueStartIx + prefix.Length,
					cxValueEndIx - cxValueStartIx - prefix.Length)
					.Trim();

				// Append everything before the placeholder
				textBuilder.Append(text.Substring(startIndex, cxValueStartIx - startIndex));

				object obj = context.GetObject(substring.TrimmedSplit());

				// Append the transformed content
				textBuilder.Append(context.GetTextualRepresentation(obj));

				// Move the startIndex to read only unscanned part of the original text
				startIndex = cxValueEndIx + 1;
			}

			// Output the rest of the original text
			textBuilder.Append(text.Substring(startIndex));

			return textBuilder.ToString();
		}

		/// <summary>
		/// Retrieves the template root element from the <paramref name="template"/>.
		/// <remarks>
		/// Provided XML template must contain a XML root element (usually a "template" element) 
		/// with CollapseXML namespace: http://kaisean.com/tools/collapse.
		/// That root element is not output, but instead it contains a resulting XML root element.
		/// The resulting XML template element must not contain a cx:foreach attribute. 
		/// </remarks>
		/// </summary>
		/// <param name="template">Template to parse.</param>
		/// <returns>An XElement representing the template root element.</returns>
		private static XElement getXTemplate(String template)
		{
			XDocument doc = null;

			try
			{
				doc = XDocument.Parse(template, LoadOptions.PreserveWhitespace);
			}
			catch (XmlException)
			{
				// Document is not a valid XML document
				throw new InvalidXmlFormatException();
			}

			XAttribute namespaceAttribute =
				doc.Root.Attributes()
				.FirstOrDefault(a => a.IsNamespaceDeclaration
					&& a.Name.LocalName == "cx"
					&& a.Value == CollapseNamespace);

			// Template element is not a valid Collapse namespace declaration
			if (namespaceAttribute == null)
				throw new MissingNamespaceException();

			if (doc.Root.Elements().Count() > 1)
			{
				// Only a single root element in destination xml is allowed
				throw new MultipleRootElementsException();
			}

			if (!doc.Root.HasElements)
			{
				// A destination root element must be defined
				throw new RootElementNotFoundException();
			}

			// There is only a single root element...
			XElement templateRoot = doc.Root.Elements().FirstOrDefault();

			if (templateRoot.IsCollapseForeach())
			{
				// ... but it must not be a cx:foreach attribute
				throw new RootElementIsAForeachException();
			}

			return templateRoot;
		}
	}
}
