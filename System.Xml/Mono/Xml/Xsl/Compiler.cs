using Mono.Xml.XPath;
using Mono.Xml.Xsl.Operations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal class Compiler : IStaticXsltContext
	{
		public const string XsltNamespace = "http://www.w3.org/1999/XSL/Transform";

		private ArrayList inputStack = new ArrayList();

		private XPathNavigator currentInput;

		private Stack styleStack = new Stack();

		private XslStylesheet currentStyle;

		private Hashtable keys = new Hashtable();

		private Hashtable globalVariables = new Hashtable();

		private Hashtable attrSets = new Hashtable();

		private XmlNamespaceManager nsMgr = new XmlNamespaceManager(new NameTable());

		private XmlResolver res;

		private Evidence evidence;

		private XslStylesheet rootStyle;

		private Hashtable outputs = new Hashtable();

		private bool keyCompilationMode;

		private string stylesheetVersion;

		private XsltDebuggerWrapper debugger;

		private MSXslScriptManager msScripts = new MSXslScriptManager();

		internal XPathParser xpathParser;

		internal XsltPatternParser patternParser;

		private VariableScope curVarScope;

		private Hashtable decimalFormats = new Hashtable();

		public Compiler(object debugger)
		{
			if (debugger != null)
			{
				this.debugger = new XsltDebuggerWrapper(debugger);
			}
		}

		Expression IStaticXsltContext.TryGetVariable(string nm)
		{
			if (this.curVarScope == null)
			{
				return null;
			}
			XslLocalVariable xslLocalVariable = this.curVarScope.ResolveStatic(XslNameUtil.FromString(nm, this.Input));
			if (xslLocalVariable == null)
			{
				return null;
			}
			return new XPathVariableBinding(xslLocalVariable);
		}

		Expression IStaticXsltContext.TryGetFunction(XmlQualifiedName name, FunctionArguments args)
		{
			string a = this.LookupNamespace(name.Namespace);
			if (a == "urn:schemas-microsoft-com:xslt" && name.Name == "node-set")
			{
				return new MSXslNodeSet(args);
			}
			if (a != string.Empty)
			{
				return null;
			}
			string name2 = name.Name;
			switch (name2)
			{
			case "current":
				return new XsltCurrent(args);
			case "unparsed-entity-uri":
				return new XsltUnparsedEntityUri(args);
			case "element-available":
				return new XsltElementAvailable(args, this);
			case "system-property":
				return new XsltSystemProperty(args, this);
			case "function-available":
				return new XsltFunctionAvailable(args, this);
			case "generate-id":
				return new XsltGenerateId(args);
			case "format-number":
				return new XsltFormatNumber(args, this);
			case "key":
				if (this.KeyCompilationMode)
				{
					throw new XsltCompileException("Cannot use key() function inside key definition", null, this.Input);
				}
				return new XsltKey(args, this);
			case "document":
				return new XsltDocument(args, this);
			}
			return null;
		}

		XmlQualifiedName IStaticXsltContext.LookupQName(string s)
		{
			return XslNameUtil.FromString(s, this.Input);
		}

		public XsltDebuggerWrapper Debugger
		{
			get
			{
				return this.debugger;
			}
		}

		public void CheckExtraAttributes(string element, params string[] validNames)
		{
			if (this.Input.MoveToFirstAttribute())
			{
				for (;;)
				{
					if (this.Input.NamespaceURI.Length <= 0)
					{
						bool flag = false;
						foreach (string b in validNames)
						{
							if (this.Input.LocalName == b)
							{
								flag = true;
							}
						}
						if (!flag)
						{
							break;
						}
					}
					if (!this.Input.MoveToNextAttribute())
					{
						goto Block_5;
					}
				}
				throw new XsltCompileException(string.Format("Invalid attribute '{0}' on element '{1}'", this.Input.LocalName, element), null, this.Input);
				Block_5:
				this.Input.MoveToParent();
			}
		}

		public CompiledStylesheet Compile(XPathNavigator nav, XmlResolver res, Evidence evidence)
		{
			this.xpathParser = new XPathParser(this);
			this.patternParser = new XsltPatternParser(this);
			this.res = res;
			if (res == null)
			{
				this.res = new XmlUrlResolver();
			}
			this.evidence = evidence;
			if (nav.NodeType == XPathNodeType.Root && !nav.MoveToFirstChild())
			{
				throw new XsltCompileException("Stylesheet root element must be either \"stylesheet\" or \"transform\" or any literal element", null, nav);
			}
			while (nav.NodeType != XPathNodeType.Element)
			{
				nav.MoveToNext();
			}
			this.stylesheetVersion = nav.GetAttribute("version", (!(nav.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")) ? string.Empty : "http://www.w3.org/1999/XSL/Transform");
			this.outputs[string.Empty] = new XslOutput(string.Empty, this.stylesheetVersion);
			this.PushInputDocument(nav);
			if (nav.MoveToFirstNamespace(XPathNamespaceScope.ExcludeXml))
			{
				do
				{
					this.nsMgr.AddNamespace(nav.LocalName, nav.Value);
				}
				while (nav.MoveToNextNamespace(XPathNamespaceScope.ExcludeXml));
				nav.MoveToParent();
			}
			try
			{
				this.rootStyle = new XslStylesheet();
				this.rootStyle.Compile(this);
			}
			catch (XsltCompileException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new XsltCompileException("XSLT compile error. " + ex.Message, ex, this.Input);
			}
			return new CompiledStylesheet(this.rootStyle, this.globalVariables, this.attrSets, this.nsMgr, this.keys, this.outputs, this.decimalFormats, this.msScripts);
		}

		public MSXslScriptManager ScriptManager
		{
			get
			{
				return this.msScripts;
			}
		}

		public bool KeyCompilationMode
		{
			get
			{
				return this.keyCompilationMode;
			}
			set
			{
				this.keyCompilationMode = value;
			}
		}

		internal Evidence Evidence
		{
			get
			{
				return this.evidence;
			}
		}

		public XPathNavigator Input
		{
			get
			{
				return this.currentInput;
			}
		}

		public XslStylesheet CurrentStylesheet
		{
			get
			{
				return this.currentStyle;
			}
		}

		public void PushStylesheet(XslStylesheet style)
		{
			if (this.currentStyle != null)
			{
				this.styleStack.Push(this.currentStyle);
			}
			this.currentStyle = style;
		}

		public void PopStylesheet()
		{
			if (this.styleStack.Count == 0)
			{
				this.currentStyle = null;
			}
			else
			{
				this.currentStyle = (XslStylesheet)this.styleStack.Pop();
			}
		}

		public void PushInputDocument(string url)
		{
			Uri baseUri = (!(this.Input.BaseURI == string.Empty)) ? new Uri(this.Input.BaseURI) : null;
			Uri uri = this.res.ResolveUri(baseUri, url);
			string url2 = (!(uri != null)) ? string.Empty : uri.ToString();
			using (Stream stream = (Stream)this.res.GetEntity(uri, null, typeof(Stream)))
			{
				if (stream == null)
				{
					throw new XsltCompileException("Can not access URI " + uri.ToString(), null, this.Input);
				}
				XmlValidatingReader xmlValidatingReader = new XmlValidatingReader(new XmlTextReader(url2, stream, this.nsMgr.NameTable));
				xmlValidatingReader.ValidationType = ValidationType.None;
				XPathNavigator xpathNavigator = new XPathDocument(xmlValidatingReader, XmlSpace.Preserve).CreateNavigator();
				xmlValidatingReader.Close();
				xpathNavigator.MoveToFirstChild();
				while (xpathNavigator.NodeType != XPathNodeType.Element)
				{
					if (!xpathNavigator.MoveToNext())
					{
						IL_F9:
						this.PushInputDocument(xpathNavigator);
						return;
					}
				}
				goto IL_F9;
			}
		}

		public void PushInputDocument(XPathNavigator nav)
		{
			IXmlLineInfo xmlLineInfo = this.currentInput as IXmlLineInfo;
			bool flag = xmlLineInfo != null && !xmlLineInfo.HasLineInfo();
			for (int i = 0; i < this.inputStack.Count; i++)
			{
				XPathNavigator xpathNavigator = (XPathNavigator)this.inputStack[i];
				if (xpathNavigator.BaseURI == nav.BaseURI)
				{
					throw new XsltCompileException(null, this.currentInput.BaseURI, (!flag) ? 0 : xmlLineInfo.LineNumber, (!flag) ? 0 : xmlLineInfo.LinePosition);
				}
			}
			if (this.currentInput != null)
			{
				this.inputStack.Add(this.currentInput);
			}
			this.currentInput = nav;
		}

		public void PopInputDocument()
		{
			int index = this.inputStack.Count - 1;
			this.currentInput = (XPathNavigator)this.inputStack[index];
			this.inputStack.RemoveAt(index);
		}

		public XmlQualifiedName ParseQNameAttribute(string localName)
		{
			return this.ParseQNameAttribute(localName, string.Empty);
		}

		public XmlQualifiedName ParseQNameAttribute(string localName, string ns)
		{
			return XslNameUtil.FromString(this.Input.GetAttribute(localName, ns), this.Input);
		}

		public XmlQualifiedName[] ParseQNameListAttribute(string localName)
		{
			return this.ParseQNameListAttribute(localName, string.Empty);
		}

		public XmlQualifiedName[] ParseQNameListAttribute(string localName, string ns)
		{
			string attribute = this.GetAttribute(localName, ns);
			if (attribute == null)
			{
				return null;
			}
			string[] array = attribute.Split(new char[]
			{
				' ',
				'\r',
				'\n',
				'\t'
			});
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Length != 0)
				{
					num++;
				}
			}
			XmlQualifiedName[] array2 = new XmlQualifiedName[num];
			int j = 0;
			int num2 = 0;
			while (j < array.Length)
			{
				if (array[j].Length != 0)
				{
					array2[num2++] = XslNameUtil.FromString(array[j], this.Input);
				}
				j++;
			}
			return array2;
		}

		public bool ParseYesNoAttribute(string localName, bool defaultVal)
		{
			return this.ParseYesNoAttribute(localName, string.Empty, defaultVal);
		}

		public bool ParseYesNoAttribute(string localName, string ns, bool defaultVal)
		{
			string attribute = this.GetAttribute(localName, ns);
			string text = attribute;
			if (text != null)
			{
				if (Compiler.<>f__switch$mapF == null)
				{
					Compiler.<>f__switch$mapF = new Dictionary<string, int>(2)
					{
						{
							"yes",
							0
						},
						{
							"no",
							1
						}
					};
				}
				int num;
				if (Compiler.<>f__switch$mapF.TryGetValue(text, out num))
				{
					if (num == 0)
					{
						return true;
					}
					if (num == 1)
					{
						return false;
					}
				}
				throw new XsltCompileException("Invalid value for " + localName, null, this.Input);
			}
			return defaultVal;
		}

		public string GetAttribute(string localName)
		{
			return this.GetAttribute(localName, string.Empty);
		}

		public string GetAttribute(string localName, string ns)
		{
			if (!this.Input.MoveToAttribute(localName, ns))
			{
				return null;
			}
			string value = this.Input.Value;
			this.Input.MoveToParent();
			return value;
		}

		public XslAvt ParseAvtAttribute(string localName)
		{
			return this.ParseAvtAttribute(localName, string.Empty);
		}

		public XslAvt ParseAvtAttribute(string localName, string ns)
		{
			return this.ParseAvt(this.GetAttribute(localName, ns));
		}

		public void AssertAttribute(string localName)
		{
			this.AssertAttribute(localName, string.Empty);
		}

		public void AssertAttribute(string localName, string ns)
		{
			if (this.Input.GetAttribute(localName, ns) == null)
			{
				throw new XsltCompileException("Was expecting the " + localName + " attribute", null, this.Input);
			}
		}

		public XslAvt ParseAvt(string s)
		{
			if (s == null)
			{
				return null;
			}
			return new XslAvt(s, this);
		}

		public Pattern CompilePattern(string pattern, XPathNavigator loc)
		{
			if (pattern == null || pattern == string.Empty)
			{
				return null;
			}
			Pattern pattern2 = Pattern.Compile(pattern, this);
			if (pattern2 == null)
			{
				throw new XsltCompileException(string.Format("Invalid pattern '{0}'", pattern), null, loc);
			}
			return pattern2;
		}

		internal CompiledExpression CompileExpression(string expression)
		{
			return this.CompileExpression(expression, false);
		}

		internal CompiledExpression CompileExpression(string expression, bool isKey)
		{
			if (expression == null || expression == string.Empty)
			{
				return null;
			}
			Expression expr = this.xpathParser.Compile(expression);
			if (isKey)
			{
				expr = new ExprKeyContainer(expr);
			}
			return new CompiledExpression(expression, expr);
		}

		public XslOperation CompileTemplateContent()
		{
			return this.CompileTemplateContent(XPathNodeType.All, false);
		}

		public XslOperation CompileTemplateContent(XPathNodeType parentType)
		{
			return this.CompileTemplateContent(parentType, false);
		}

		public XslOperation CompileTemplateContent(XPathNodeType parentType, bool xslForEach)
		{
			return new XslTemplateContent(this, parentType, xslForEach);
		}

		public void AddGlobalVariable(XslGlobalVariable var)
		{
			this.globalVariables[var.Name] = var;
		}

		public void AddKey(XslKey key)
		{
			if (this.keys[key.Name] == null)
			{
				this.keys[key.Name] = new ArrayList();
			}
			((ArrayList)this.keys[key.Name]).Add(key);
		}

		public void AddAttributeSet(XslAttributeSet set)
		{
			XslAttributeSet xslAttributeSet = this.attrSets[set.Name] as XslAttributeSet;
			if (xslAttributeSet != null)
			{
				xslAttributeSet.Merge(set);
				this.attrSets[set.Name] = xslAttributeSet;
			}
			else
			{
				this.attrSets[set.Name] = set;
			}
		}

		public void PushScope()
		{
			this.curVarScope = new VariableScope(this.curVarScope);
		}

		public VariableScope PopScope()
		{
			this.curVarScope.giveHighTideToParent();
			VariableScope result = this.curVarScope;
			this.curVarScope = this.curVarScope.Parent;
			return result;
		}

		public int AddVariable(XslLocalVariable v)
		{
			if (this.curVarScope == null)
			{
				throw new XsltCompileException("Not initialized variable", null, this.Input);
			}
			return this.curVarScope.AddVariable(v);
		}

		public VariableScope CurrentVariableScope
		{
			get
			{
				return this.curVarScope;
			}
		}

		public bool IsExtensionNamespace(string nsUri)
		{
			if (nsUri == "http://www.w3.org/1999/XSL/Transform")
			{
				return true;
			}
			XPathNavigator xpathNavigator = this.Input.Clone();
			XPathNavigator xpathNavigator2 = xpathNavigator.Clone();
			for (;;)
			{
				bool flag = xpathNavigator.NamespaceURI == "http://www.w3.org/1999/XSL/Transform";
				xpathNavigator2.MoveTo(xpathNavigator);
				if (xpathNavigator.MoveToFirstAttribute())
				{
					do
					{
						if (xpathNavigator.LocalName == "extension-element-prefixes" && xpathNavigator.NamespaceURI == ((!flag) ? "http://www.w3.org/1999/XSL/Transform" : string.Empty))
						{
							foreach (string text in xpathNavigator.Value.Split(new char[]
							{
								' '
							}))
							{
								if (xpathNavigator2.GetNamespace((!(text == "#default")) ? text : string.Empty) == nsUri)
								{
									return true;
								}
							}
						}
					}
					while (xpathNavigator.MoveToNextAttribute());
					xpathNavigator.MoveToParent();
				}
				if (!xpathNavigator.MoveToParent())
				{
					return false;
				}
			}
			return true;
		}

		public Hashtable GetNamespacesToCopy()
		{
			Hashtable hashtable = new Hashtable();
			XPathNavigator xpathNavigator = this.Input.Clone();
			XPathNavigator xpathNavigator2 = xpathNavigator.Clone();
			if (xpathNavigator.MoveToFirstNamespace(XPathNamespaceScope.ExcludeXml))
			{
				do
				{
					if (xpathNavigator.Value != "http://www.w3.org/1999/XSL/Transform" && !hashtable.Contains(xpathNavigator.Name))
					{
						hashtable.Add(xpathNavigator.Name, xpathNavigator.Value);
					}
				}
				while (xpathNavigator.MoveToNextNamespace(XPathNamespaceScope.ExcludeXml));
				xpathNavigator.MoveToParent();
			}
			do
			{
				bool flag = xpathNavigator.NamespaceURI == "http://www.w3.org/1999/XSL/Transform";
				xpathNavigator2.MoveTo(xpathNavigator);
				if (xpathNavigator.MoveToFirstAttribute())
				{
					do
					{
						if ((xpathNavigator.LocalName == "extension-element-prefixes" || xpathNavigator.LocalName == "exclude-result-prefixes") && xpathNavigator.NamespaceURI == ((!flag) ? "http://www.w3.org/1999/XSL/Transform" : string.Empty))
						{
							foreach (string text in xpathNavigator.Value.Split(new char[]
							{
								' '
							}))
							{
								string text2 = (!(text == "#default")) ? text : string.Empty;
								if ((string)hashtable[text2] == xpathNavigator2.GetNamespace(text2))
								{
									hashtable.Remove(text2);
								}
							}
						}
					}
					while (xpathNavigator.MoveToNextAttribute());
					xpathNavigator.MoveToParent();
				}
			}
			while (xpathNavigator.MoveToParent());
			return hashtable;
		}

		public void CompileDecimalFormat()
		{
			XmlQualifiedName xmlQualifiedName = this.ParseQNameAttribute("name");
			try
			{
				if (xmlQualifiedName.Name != string.Empty)
				{
					XmlConvert.VerifyNCName(xmlQualifiedName.Name);
				}
			}
			catch (XmlException innerException)
			{
				throw new XsltCompileException("Invalid qualified name", innerException, this.Input);
			}
			XslDecimalFormat xslDecimalFormat = new XslDecimalFormat(this);
			if (this.decimalFormats.Contains(xmlQualifiedName))
			{
				((XslDecimalFormat)this.decimalFormats[xmlQualifiedName]).CheckSameAs(xslDecimalFormat);
			}
			else
			{
				this.decimalFormats[xmlQualifiedName] = xslDecimalFormat;
			}
		}

		public string LookupNamespace(string prefix)
		{
			if (prefix == string.Empty || prefix == null)
			{
				return string.Empty;
			}
			XPathNavigator xpathNavigator = this.Input;
			if (this.Input.NodeType == XPathNodeType.Attribute)
			{
				xpathNavigator = this.Input.Clone();
				xpathNavigator.MoveToParent();
			}
			return xpathNavigator.GetNamespace(prefix);
		}

		public void CompileOutput()
		{
			XPathNavigator input = this.Input;
			string attribute = input.GetAttribute("href", string.Empty);
			XslOutput xslOutput = this.outputs[attribute] as XslOutput;
			if (xslOutput == null)
			{
				xslOutput = new XslOutput(attribute, this.stylesheetVersion);
				this.outputs.Add(attribute, xslOutput);
			}
			xslOutput.Fill(input);
		}
	}
}
