using Mono.Xml.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the World Wide Web Consortium (W3C) selector element.</summary>
	public class XmlSchemaXPath : XmlSchemaAnnotated
	{
		private string xpath;

		private XmlNamespaceManager nsmgr;

		internal bool isSelector;

		private XsdIdentityPath[] compiledExpression;

		private XsdIdentityPath currentPath;

		/// <summary>Gets or sets the attribute for the XPath expression.</summary>
		/// <returns>The string attribute value for the XPath expression.</returns>
		[XmlAttribute("xpath")]
		[DefaultValue("")]
		public string XPath
		{
			get
			{
				return this.xpath;
			}
			set
			{
				this.xpath = value;
			}
		}

		internal override int Compile(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.CompilationId == schema.CompilationId)
			{
				return 0;
			}
			if (this.nsmgr == null)
			{
				this.nsmgr = new XmlNamespaceManager(new NameTable());
				if (base.Namespaces != null)
				{
					foreach (XmlQualifiedName xmlQualifiedName in base.Namespaces.ToArray())
					{
						this.nsmgr.AddNamespace(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
					}
				}
			}
			this.currentPath = new XsdIdentityPath();
			this.ParseExpression(this.xpath, h, schema);
			XmlSchemaUtil.CompileID(base.Id, this, schema.IDCollection, h);
			this.CompilationId = schema.CompilationId;
			return this.errorCount;
		}

		internal XsdIdentityPath[] CompiledExpression
		{
			get
			{
				return this.compiledExpression;
			}
		}

		private void ParseExpression(string xpath, ValidationEventHandler h, XmlSchema schema)
		{
			ArrayList arrayList = new ArrayList();
			this.ParsePath(xpath, 0, arrayList, h, schema);
			this.compiledExpression = (XsdIdentityPath[])arrayList.ToArray(typeof(XsdIdentityPath));
		}

		private void ParsePath(string xpath, int pos, ArrayList paths, ValidationEventHandler h, XmlSchema schema)
		{
			pos = this.SkipWhitespace(xpath, pos);
			if (xpath.Length >= pos + 3 && xpath[pos] == '.')
			{
				int num = pos;
				pos++;
				pos = this.SkipWhitespace(xpath, pos);
				if (xpath.Length > pos + 2 && xpath.IndexOf("//", pos, 2) == pos)
				{
					this.currentPath.Descendants = true;
					pos += 2;
				}
				else
				{
					pos = num;
				}
			}
			ArrayList steps = new ArrayList();
			this.ParseStep(xpath, pos, steps, paths, h, schema);
		}

		private void ParseStep(string xpath, int pos, ArrayList steps, ArrayList paths, ValidationEventHandler h, XmlSchema schema)
		{
			pos = this.SkipWhitespace(xpath, pos);
			if (xpath.Length == pos)
			{
				base.error(h, "Empty xpath expression is specified");
				return;
			}
			XsdIdentityStep xsdIdentityStep = new XsdIdentityStep();
			char c = xpath[pos];
			switch (c)
			{
			case 'a':
				if (xpath.Length > pos + 9 && xpath.IndexOf("attribute", pos, 9) == pos)
				{
					int num = pos;
					pos += 9;
					pos = this.SkipWhitespace(xpath, pos);
					if (xpath.Length > pos && xpath[pos] == ':' && xpath[pos + 1] == ':')
					{
						if (this.isSelector)
						{
							base.error(h, "Selector cannot include attribute axes.");
							this.currentPath = null;
							return;
						}
						pos += 2;
						xsdIdentityStep.IsAttribute = true;
						if (xpath.Length > pos && xpath[pos] == '*')
						{
							pos++;
							xsdIdentityStep.IsAnyName = true;
							goto IL_3D5;
						}
						pos = this.SkipWhitespace(xpath, pos);
					}
					else
					{
						pos = num;
					}
				}
				break;
			default:
				if (c == '*')
				{
					pos++;
					xsdIdentityStep.IsAnyName = true;
					goto IL_3D5;
				}
				if (c == '.')
				{
					pos++;
					xsdIdentityStep.IsCurrent = true;
					goto IL_3D5;
				}
				if (c == '@')
				{
					if (this.isSelector)
					{
						base.error(h, "Selector cannot include attribute axes.");
						this.currentPath = null;
						return;
					}
					pos++;
					xsdIdentityStep.IsAttribute = true;
					pos = this.SkipWhitespace(xpath, pos);
					if (xpath.Length > pos && xpath[pos] == '*')
					{
						pos++;
						xsdIdentityStep.IsAnyName = true;
						goto IL_3D5;
					}
				}
				break;
			case 'c':
				if (xpath.Length > pos + 5 && xpath.IndexOf("child", pos, 5) == pos)
				{
					int num2 = pos;
					pos += 5;
					pos = this.SkipWhitespace(xpath, pos);
					if (xpath.Length > pos && xpath[pos] == ':' && xpath[pos + 1] == ':')
					{
						pos += 2;
						if (xpath.Length > pos && xpath[pos] == '*')
						{
							pos++;
							xsdIdentityStep.IsAnyName = true;
							goto IL_3D5;
						}
						pos = this.SkipWhitespace(xpath, pos);
					}
					else
					{
						pos = num2;
					}
				}
				break;
			}
			int num3 = pos;
			while (xpath.Length > pos)
			{
				if (!XmlChar.IsNCNameChar((int)xpath[pos]))
				{
					break;
				}
				pos++;
			}
			if (pos == num3)
			{
				base.error(h, "Invalid path format for a field.");
				this.currentPath = null;
				return;
			}
			if (xpath.Length == pos || xpath[pos] != ':')
			{
				xsdIdentityStep.Name = xpath.Substring(num3, pos - num3);
			}
			else
			{
				string text = xpath.Substring(num3, pos - num3);
				pos++;
				if (xpath.Length > pos && xpath[pos] == '*')
				{
					string text2 = this.nsmgr.LookupNamespace(text, false);
					if (text2 == null)
					{
						base.error(h, "Specified prefix '" + text + "' is not declared.");
						this.currentPath = null;
						return;
					}
					xsdIdentityStep.NsName = text2;
					pos++;
				}
				else
				{
					int num4 = pos;
					while (xpath.Length > pos)
					{
						if (!XmlChar.IsNCNameChar((int)xpath[pos]))
						{
							break;
						}
						pos++;
					}
					xsdIdentityStep.Name = xpath.Substring(num4, pos - num4);
					string text3 = this.nsmgr.LookupNamespace(text, false);
					if (text3 == null)
					{
						base.error(h, "Specified prefix '" + text + "' is not declared.");
						this.currentPath = null;
						return;
					}
					xsdIdentityStep.Namespace = text3;
				}
			}
			IL_3D5:
			if (!xsdIdentityStep.IsCurrent)
			{
				steps.Add(xsdIdentityStep);
			}
			pos = this.SkipWhitespace(xpath, pos);
			if (xpath.Length == pos)
			{
				this.currentPath.OrderedSteps = (XsdIdentityStep[])steps.ToArray(typeof(XsdIdentityStep));
				paths.Add(this.currentPath);
				return;
			}
			if (xpath[pos] == '/')
			{
				pos++;
				if (xsdIdentityStep.IsAttribute)
				{
					base.error(h, "Unexpected xpath token after Attribute NameTest.");
					this.currentPath = null;
					return;
				}
				this.ParseStep(xpath, pos, steps, paths, h, schema);
				if (this.currentPath == null)
				{
					return;
				}
				this.currentPath.OrderedSteps = (XsdIdentityStep[])steps.ToArray(typeof(XsdIdentityStep));
			}
			else
			{
				if (xpath[pos] != '|')
				{
					base.error(h, "Unexpected xpath token after NameTest.");
					this.currentPath = null;
					return;
				}
				pos++;
				this.currentPath.OrderedSteps = (XsdIdentityStep[])steps.ToArray(typeof(XsdIdentityStep));
				paths.Add(this.currentPath);
				this.currentPath = new XsdIdentityPath();
				this.ParsePath(xpath, pos, paths, h, schema);
			}
		}

		private int SkipWhitespace(string xpath, int pos)
		{
			bool flag = true;
			while (flag && xpath.Length > pos)
			{
				char c = xpath[pos];
				switch (c)
				{
				case '\t':
				case '\n':
				case '\r':
					break;
				default:
					if (c != ' ')
					{
						flag = false;
						continue;
					}
					break;
				}
				pos++;
			}
			return pos;
		}

		internal static XmlSchemaXPath Read(XmlSchemaReader reader, ValidationEventHandler h, string name)
		{
			XmlSchemaXPath xmlSchemaXPath = new XmlSchemaXPath();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != name)
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaComplexContentRestriction.Read, name=" + reader.Name, null);
				reader.Skip();
				return null;
			}
			xmlSchemaXPath.LineNumber = reader.LineNumber;
			xmlSchemaXPath.LinePosition = reader.LinePosition;
			xmlSchemaXPath.SourceUri = reader.BaseURI;
			XmlNamespaceManager namespaceManager = XmlSchemaUtil.GetParserContext(reader.Reader).NamespaceManager;
			if (namespaceManager != null)
			{
				xmlSchemaXPath.nsmgr = new XmlNamespaceManager(reader.NameTable);
				foreach (object obj in namespaceManager)
				{
					string text = obj as string;
					string text2 = text;
					if (text2 != null)
					{
						if (XmlSchemaXPath.<>f__switch$map3C == null)
						{
							XmlSchemaXPath.<>f__switch$map3C = new Dictionary<string, int>(2)
							{
								{
									"xml",
									0
								},
								{
									"xmlns",
									0
								}
							};
						}
						int num;
						if (XmlSchemaXPath.<>f__switch$map3C.TryGetValue(text2, out num))
						{
							if (num == 0)
							{
								continue;
							}
						}
					}
					xmlSchemaXPath.nsmgr.AddNamespace(text, namespaceManager.LookupNamespace(text, false));
				}
			}
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "id")
				{
					xmlSchemaXPath.Id = reader.Value;
				}
				else if (reader.Name == "xpath")
				{
					xmlSchemaXPath.xpath = reader.Value;
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for " + name, null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaXPath);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaXPath;
			}
			int num2 = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != name)
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaXPath.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num2 <= 1 && reader.LocalName == "annotation")
				{
					num2 = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaXPath.Annotation = xmlSchemaAnnotation;
					}
				}
				else
				{
					reader.RaiseInvalidElementError();
				}
			}
			return xmlSchemaXPath;
		}
	}
}
