using Mono.Xml.Xsl.Operations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal class XslStylesheet
	{
		public const string XsltNamespace = "http://www.w3.org/1999/XSL/Transform";

		public const string MSXsltNamespace = "urn:schemas-microsoft-com:xslt";

		private ArrayList imports = new ArrayList();

		private Hashtable spaceControls = new Hashtable();

		private NameValueCollection namespaceAliases = new NameValueCollection();

		private Hashtable parameters = new Hashtable();

		private Hashtable keys = new Hashtable();

		private Hashtable variables = new Hashtable();

		private XslTemplateTable templates;

		private string baseURI;

		private string version;

		private XmlQualifiedName[] extensionElementPrefixes;

		private XmlQualifiedName[] excludeResultPrefixes;

		private ArrayList stylesheetNamespaces = new ArrayList();

		private Hashtable inProcessIncludes = new Hashtable();

		private bool countedSpaceControlExistence;

		private bool cachedHasSpaceControls;

		private static readonly XmlQualifiedName allMatchName = new XmlQualifiedName("*");

		private bool countedNamespaceAliases;

		private bool cachedHasNamespaceAliases;

		public XmlQualifiedName[] ExtensionElementPrefixes
		{
			get
			{
				return this.extensionElementPrefixes;
			}
		}

		public XmlQualifiedName[] ExcludeResultPrefixes
		{
			get
			{
				return this.excludeResultPrefixes;
			}
		}

		public ArrayList StylesheetNamespaces
		{
			get
			{
				return this.stylesheetNamespaces;
			}
		}

		public ArrayList Imports
		{
			get
			{
				return this.imports;
			}
		}

		public Hashtable SpaceControls
		{
			get
			{
				return this.spaceControls;
			}
		}

		public NameValueCollection NamespaceAliases
		{
			get
			{
				return this.namespaceAliases;
			}
		}

		public Hashtable Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		public XslTemplateTable Templates
		{
			get
			{
				return this.templates;
			}
		}

		public string BaseURI
		{
			get
			{
				return this.baseURI;
			}
		}

		public string Version
		{
			get
			{
				return this.version;
			}
		}

		internal void Compile(Compiler c)
		{
			c.PushStylesheet(this);
			this.templates = new XslTemplateTable(this);
			this.baseURI = c.Input.BaseURI;
			while (c.Input.NodeType != XPathNodeType.Element)
			{
				if (!c.Input.MoveToNext())
				{
					throw new XsltCompileException("Stylesheet root element must be either \"stylesheet\" or \"transform\" or any literal element", null, c.Input);
				}
			}
			if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
			{
				if (c.Input.GetAttribute("version", "http://www.w3.org/1999/XSL/Transform") == string.Empty)
				{
					throw new XsltCompileException("Mandatory global attribute version is missing", null, c.Input);
				}
				this.templates.Add(new XslTemplate(c));
			}
			else
			{
				if (c.Input.LocalName != "stylesheet" && c.Input.LocalName != "transform")
				{
					throw new XsltCompileException("Stylesheet root element must be either \"stylesheet\" or \"transform\" or any literal element", null, c.Input);
				}
				this.version = c.Input.GetAttribute("version", string.Empty);
				if (this.version == string.Empty)
				{
					throw new XsltCompileException("Mandatory attribute version is missing", null, c.Input);
				}
				this.extensionElementPrefixes = this.ParseMappedPrefixes(c.GetAttribute("extension-element-prefixes"), c.Input);
				this.excludeResultPrefixes = this.ParseMappedPrefixes(c.GetAttribute("exclude-result-prefixes"), c.Input);
				if (c.Input.MoveToFirstNamespace(XPathNamespaceScope.Local))
				{
					do
					{
						if (!(c.Input.Value == "http://www.w3.org/1999/XSL/Transform"))
						{
							this.stylesheetNamespaces.Insert(0, new XmlQualifiedName(c.Input.Name, c.Input.Value));
						}
					}
					while (c.Input.MoveToNextNamespace(XPathNamespaceScope.Local));
					c.Input.MoveToParent();
				}
				this.ProcessTopLevelElements(c);
			}
			foreach (object obj in this.variables.Values)
			{
				XslGlobalVariable var = (XslGlobalVariable)obj;
				c.AddGlobalVariable(var);
			}
			foreach (object obj2 in this.keys.Values)
			{
				ArrayList arrayList = (ArrayList)obj2;
				for (int i = 0; i < arrayList.Count; i++)
				{
					c.AddKey((XslKey)arrayList[i]);
				}
			}
			c.PopStylesheet();
			this.inProcessIncludes = null;
		}

		private XmlQualifiedName[] ParseMappedPrefixes(string list, XPathNavigator nav)
		{
			if (list == null)
			{
				return null;
			}
			ArrayList arrayList = new ArrayList();
			foreach (string text in list.Split(XmlChar.WhitespaceChars))
			{
				if (text.Length != 0)
				{
					if (text == "#default")
					{
						arrayList.Add(new XmlQualifiedName(string.Empty, string.Empty));
					}
					else
					{
						string @namespace = nav.GetNamespace(text);
						if (@namespace != string.Empty)
						{
							arrayList.Add(new XmlQualifiedName(text, @namespace));
						}
					}
				}
			}
			return (XmlQualifiedName[])arrayList.ToArray(typeof(XmlQualifiedName));
		}

		public bool HasSpaceControls
		{
			get
			{
				if (!this.countedSpaceControlExistence)
				{
					this.countedSpaceControlExistence = true;
					this.cachedHasSpaceControls = this.ComputeHasSpaceControls();
				}
				return this.cachedHasSpaceControls;
			}
		}

		private bool ComputeHasSpaceControls()
		{
			if (this.spaceControls.Count > 0 && this.HasStripSpace(this.spaceControls))
			{
				return true;
			}
			if (this.imports.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.imports.Count; i++)
			{
				XslStylesheet xslStylesheet = (XslStylesheet)this.imports[i];
				if (xslStylesheet.spaceControls.Count > 0 && this.HasStripSpace(xslStylesheet.spaceControls))
				{
					return true;
				}
			}
			return false;
		}

		private bool HasStripSpace(IDictionary table)
		{
			foreach (object obj in table.Values)
			{
				XmlSpace xmlSpace = (XmlSpace)((int)obj);
				if (xmlSpace == XmlSpace.Default)
				{
					return true;
				}
			}
			return false;
		}

		public bool GetPreserveWhitespace(XPathNavigator nav)
		{
			if (!this.HasSpaceControls)
			{
				return true;
			}
			nav = nav.Clone();
			if (!nav.MoveToParent() || nav.NodeType != XPathNodeType.Element)
			{
				object defaultXmlSpace = this.GetDefaultXmlSpace();
				return defaultXmlSpace == null || (int)defaultXmlSpace == 2;
			}
			string localName = nav.LocalName;
			string namespaceURI = nav.NamespaceURI;
			XmlQualifiedName key = new XmlQualifiedName(localName, namespaceURI);
			object obj = this.spaceControls[key];
			if (obj == null)
			{
				for (int i = 0; i < this.imports.Count; i++)
				{
					obj = ((XslStylesheet)this.imports[i]).SpaceControls[key];
					if (obj != null)
					{
						break;
					}
				}
			}
			if (obj == null)
			{
				key = new XmlQualifiedName("*", namespaceURI);
				obj = this.spaceControls[key];
				if (obj == null)
				{
					for (int j = 0; j < this.imports.Count; j++)
					{
						obj = ((XslStylesheet)this.imports[j]).SpaceControls[key];
						if (obj != null)
						{
							break;
						}
					}
				}
			}
			if (obj == null)
			{
				obj = this.GetDefaultXmlSpace();
			}
			if (obj != null)
			{
				XmlSpace xmlSpace = (XmlSpace)((int)obj);
				if (xmlSpace == XmlSpace.Default)
				{
					return false;
				}
				if (xmlSpace == XmlSpace.Preserve)
				{
					return true;
				}
			}
			throw new SystemException("Mono BUG: should not reach here");
		}

		private object GetDefaultXmlSpace()
		{
			object obj = this.spaceControls[XslStylesheet.allMatchName];
			if (obj == null)
			{
				for (int i = 0; i < this.imports.Count; i++)
				{
					obj = ((XslStylesheet)this.imports[i]).SpaceControls[XslStylesheet.allMatchName];
					if (obj != null)
					{
						break;
					}
				}
			}
			return obj;
		}

		public bool HasNamespaceAliases
		{
			get
			{
				if (!this.countedNamespaceAliases)
				{
					this.countedNamespaceAliases = true;
					if (this.namespaceAliases.Count > 0)
					{
						this.cachedHasNamespaceAliases = true;
					}
					else if (this.imports.Count == 0)
					{
						this.cachedHasNamespaceAliases = false;
					}
					else
					{
						for (int i = 0; i < this.imports.Count; i++)
						{
							if (((XslStylesheet)this.imports[i]).namespaceAliases.Count > 0)
							{
								this.countedNamespaceAliases = true;
							}
						}
						this.cachedHasNamespaceAliases = false;
					}
				}
				return this.cachedHasNamespaceAliases;
			}
		}

		public string GetActualPrefix(string prefix)
		{
			if (!this.HasNamespaceAliases)
			{
				return prefix;
			}
			string text = this.namespaceAliases[prefix];
			if (text == null)
			{
				for (int i = 0; i < this.imports.Count; i++)
				{
					text = ((XslStylesheet)this.imports[i]).namespaceAliases[prefix];
					if (text != null)
					{
						break;
					}
				}
			}
			return (text == null) ? prefix : text;
		}

		private void StoreInclude(Compiler c)
		{
			XPathNavigator key = c.Input.Clone();
			c.PushInputDocument(c.Input.GetAttribute("href", string.Empty));
			this.inProcessIncludes[key] = c.Input;
			this.HandleImportsInInclude(c);
			c.PopInputDocument();
		}

		private void HandleImportsInInclude(Compiler c)
		{
			if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
			{
				if (c.Input.GetAttribute("version", "http://www.w3.org/1999/XSL/Transform") == string.Empty)
				{
					throw new XsltCompileException("Mandatory global attribute version is missing", null, c.Input);
				}
				return;
			}
			else
			{
				if (!c.Input.MoveToFirstChild())
				{
					c.Input.MoveToRoot();
					return;
				}
				this.HandleIncludesImports(c);
				return;
			}
		}

		private void HandleInclude(Compiler c)
		{
			XPathNavigator xpathNavigator = null;
			foreach (object obj in this.inProcessIncludes.Keys)
			{
				XPathNavigator xpathNavigator2 = (XPathNavigator)obj;
				if (xpathNavigator2.IsSamePosition(c.Input))
				{
					xpathNavigator = (XPathNavigator)this.inProcessIncludes[xpathNavigator2];
					break;
				}
			}
			if (xpathNavigator == null)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Should not happen. Current input is ",
					c.Input.BaseURI,
					" / ",
					c.Input.Name,
					", ",
					this.inProcessIncludes.Count
				}));
			}
			if (xpathNavigator.NodeType == XPathNodeType.Root)
			{
				return;
			}
			c.PushInputDocument(xpathNavigator);
			while (c.Input.NodeType != XPathNodeType.Element)
			{
				if (!c.Input.MoveToNext())
				{
					break;
				}
			}
			if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform" && c.Input.NodeType == XPathNodeType.Element)
			{
				this.templates.Add(new XslTemplate(c));
			}
			else
			{
				do
				{
					if (c.Input.NodeType == XPathNodeType.Element)
					{
						this.HandleTopLevelElement(c);
					}
				}
				while (c.Input.MoveToNext());
			}
			c.Input.MoveToParent();
			c.PopInputDocument();
		}

		private void HandleImport(Compiler c, string href)
		{
			c.PushInputDocument(href);
			XslStylesheet xslStylesheet = new XslStylesheet();
			xslStylesheet.Compile(c);
			this.imports.Add(xslStylesheet);
			c.PopInputDocument();
		}

		private void HandleTopLevelElement(Compiler c)
		{
			XPathNavigator input = c.Input;
			string namespaceURI = input.NamespaceURI;
			if (namespaceURI != null)
			{
				if (XslStylesheet.<>f__switch$map26 == null)
				{
					XslStylesheet.<>f__switch$map26 = new Dictionary<string, int>(2)
					{
						{
							"http://www.w3.org/1999/XSL/Transform",
							0
						},
						{
							"urn:schemas-microsoft-com:xslt",
							1
						}
					};
				}
				int num;
				if (XslStylesheet.<>f__switch$map26.TryGetValue(namespaceURI, out num))
				{
					if (num != 0)
					{
						if (num == 1)
						{
							string localName = input.LocalName;
							if (localName != null)
							{
								if (XslStylesheet.<>f__switch$map25 == null)
								{
									XslStylesheet.<>f__switch$map25 = new Dictionary<string, int>(1)
									{
										{
											"script",
											0
										}
									};
								}
								int num2;
								if (XslStylesheet.<>f__switch$map25.TryGetValue(localName, out num2))
								{
									if (num2 == 0)
									{
										c.ScriptManager.AddScript(c);
									}
								}
							}
						}
					}
					else
					{
						string localName = input.LocalName;
						switch (localName)
						{
						case "include":
							this.HandleInclude(c);
							goto IL_2B0;
						case "preserve-space":
							this.AddSpaceControls(c.ParseQNameListAttribute("elements"), XmlSpace.Preserve, input);
							goto IL_2B0;
						case "strip-space":
							this.AddSpaceControls(c.ParseQNameListAttribute("elements"), XmlSpace.Default, input);
							goto IL_2B0;
						case "namespace-alias":
							goto IL_2B0;
						case "attribute-set":
							c.AddAttributeSet(new XslAttributeSet(c));
							goto IL_2B0;
						case "key":
						{
							XslKey xslKey = new XslKey(c);
							if (this.keys[xslKey.Name] == null)
							{
								this.keys[xslKey.Name] = new ArrayList();
							}
							((ArrayList)this.keys[xslKey.Name]).Add(xslKey);
							goto IL_2B0;
						}
						case "output":
							c.CompileOutput();
							goto IL_2B0;
						case "decimal-format":
							c.CompileDecimalFormat();
							goto IL_2B0;
						case "template":
							this.templates.Add(new XslTemplate(c));
							goto IL_2B0;
						case "variable":
						{
							XslGlobalVariable xslGlobalVariable = new XslGlobalVariable(c);
							this.variables[xslGlobalVariable.Name] = xslGlobalVariable;
							goto IL_2B0;
						}
						case "param":
						{
							XslGlobalParam xslGlobalParam = new XslGlobalParam(c);
							this.variables[xslGlobalParam.Name] = xslGlobalParam;
							goto IL_2B0;
						}
						}
						if (this.version == "1.0")
						{
							throw new XsltCompileException("Unrecognized top level element after imports", null, c.Input);
						}
						IL_2B0:;
					}
				}
			}
		}

		private XPathNavigator HandleIncludesImports(Compiler c)
		{
			do
			{
				if (c.Input.NodeType == XPathNodeType.Element)
				{
					if (c.Input.LocalName != "import" || c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
					{
						break;
					}
					this.HandleImport(c, c.GetAttribute("href"));
				}
			}
			while (c.Input.MoveToNext());
			XPathNavigator xpathNavigator = c.Input.Clone();
			do
			{
				if (c.Input.NodeType == XPathNodeType.Element && !(c.Input.LocalName != "include") && !(c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform"))
				{
					this.StoreInclude(c);
				}
			}
			while (c.Input.MoveToNext());
			c.Input.MoveTo(xpathNavigator);
			return xpathNavigator;
		}

		private void ProcessTopLevelElements(Compiler c)
		{
			if (!c.Input.MoveToFirstChild())
			{
				return;
			}
			XPathNavigator other = this.HandleIncludesImports(c);
			do
			{
				if (c.Input.NodeType == XPathNodeType.Element && !(c.Input.LocalName != "namespace-alias") && !(c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform"))
				{
					string text = c.GetAttribute("stylesheet-prefix", string.Empty);
					if (text == "#default")
					{
						text = string.Empty;
					}
					string text2 = c.GetAttribute("result-prefix", string.Empty);
					if (text2 == "#default")
					{
						text2 = string.Empty;
					}
					this.namespaceAliases.Set(text, text2);
				}
			}
			while (c.Input.MoveToNext());
			c.Input.MoveTo(other);
			do
			{
				if (c.Input.NodeType == XPathNodeType.Element)
				{
					this.HandleTopLevelElement(c);
				}
			}
			while (c.Input.MoveToNext());
			c.Input.MoveToParent();
		}

		private void AddSpaceControls(XmlQualifiedName[] names, XmlSpace result, XPathNavigator styleElem)
		{
			foreach (XmlQualifiedName key in names)
			{
				this.spaceControls[key] = result;
			}
		}
	}
}
