using Mono.Xml.XPath;
using Mono.Xml.Xsl.Operations;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal class XslTransformProcessor
	{
		private XsltDebuggerWrapper debugger;

		private CompiledStylesheet compiledStyle;

		private XslStylesheet style;

		private Stack currentTemplateStack = new Stack();

		private XPathNavigator root;

		private XsltArgumentList args;

		private XmlResolver resolver;

		private string currentOutputUri;

		internal readonly XsltCompiledContext XPathContext;

		internal Hashtable globalVariableTable = new Hashtable();

		private Hashtable docCache;

		private Stack outputStack = new Stack();

		private StringBuilder avtSB;

		private Stack paramPassingCache = new Stack();

		private ArrayList nodesetStack = new ArrayList();

		private Stack variableStack = new Stack();

		private object[] currentStack;

		private Hashtable busyTable = new Hashtable();

		private static object busyObject = new object();

		public XslTransformProcessor(CompiledStylesheet style, object debugger)
		{
			this.XPathContext = new XsltCompiledContext(this);
			this.compiledStyle = style;
			this.style = style.Style;
			if (debugger != null)
			{
				this.debugger = new XsltDebuggerWrapper(debugger);
			}
		}

		public void Process(XPathNavigator root, Outputter outputtter, XsltArgumentList args, XmlResolver resolver)
		{
			this.args = args;
			this.root = root;
			this.resolver = ((resolver == null) ? new XmlUrlResolver() : resolver);
			this.currentOutputUri = string.Empty;
			this.PushNodeset(new SelfIterator(root, this.XPathContext));
			this.CurrentNodeset.MoveNext();
			if (args != null)
			{
				foreach (object obj in this.CompiledStyle.Variables.Values)
				{
					XslGlobalVariable xslGlobalVariable = (XslGlobalVariable)obj;
					if (xslGlobalVariable is XslGlobalParam)
					{
						object param = args.GetParam(xslGlobalVariable.Name.Name, xslGlobalVariable.Name.Namespace);
						if (param != null)
						{
							((XslGlobalParam)xslGlobalVariable).Override(this, param);
						}
						xslGlobalVariable.Evaluate(this);
					}
				}
			}
			foreach (object obj2 in this.CompiledStyle.Variables.Values)
			{
				XslGlobalVariable xslGlobalVariable2 = (XslGlobalVariable)obj2;
				if (args == null || !(xslGlobalVariable2 is XslGlobalParam))
				{
					xslGlobalVariable2.Evaluate(this);
				}
			}
			this.PopNodeset();
			this.PushOutput(outputtter);
			this.ApplyTemplates(new SelfIterator(root, this.XPathContext), XmlQualifiedName.Empty, null);
			this.PopOutput();
		}

		public XsltDebuggerWrapper Debugger
		{
			get
			{
				return this.debugger;
			}
		}

		public CompiledStylesheet CompiledStyle
		{
			get
			{
				return this.compiledStyle;
			}
		}

		public XsltArgumentList Arguments
		{
			get
			{
				return this.args;
			}
		}

		public XPathNavigator Root
		{
			get
			{
				return this.root;
			}
		}

		public MSXslScriptManager ScriptManager
		{
			get
			{
				return this.compiledStyle.ScriptManager;
			}
		}

		public XmlResolver Resolver
		{
			get
			{
				return this.resolver;
			}
		}

		public XPathNavigator GetDocument(Uri uri)
		{
			XPathNavigator xpathNavigator;
			if (this.docCache != null)
			{
				xpathNavigator = (this.docCache[uri] as XPathNavigator);
				if (xpathNavigator != null)
				{
					return xpathNavigator.Clone();
				}
			}
			else
			{
				this.docCache = new Hashtable();
			}
			XmlReader xmlReader = null;
			try
			{
				xmlReader = new XmlTextReader(uri.ToString(), (Stream)this.resolver.GetEntity(uri, null, null), this.root.NameTable);
				xpathNavigator = new XPathDocument(new XmlValidatingReader(xmlReader)
				{
					ValidationType = ValidationType.None
				}, XmlSpace.Preserve).CreateNavigator();
			}
			finally
			{
				if (xmlReader != null)
				{
					xmlReader.Close();
				}
			}
			this.docCache[uri] = xpathNavigator.Clone();
			return xpathNavigator;
		}

		public Outputter Out
		{
			get
			{
				return (Outputter)this.outputStack.Peek();
			}
		}

		public void PushOutput(Outputter newOutput)
		{
			this.outputStack.Push(newOutput);
		}

		public Outputter PopOutput()
		{
			Outputter outputter = (Outputter)this.outputStack.Pop();
			outputter.Done();
			return outputter;
		}

		public Hashtable Outputs
		{
			get
			{
				return this.compiledStyle.Outputs;
			}
		}

		public XslOutput Output
		{
			get
			{
				return this.Outputs[this.currentOutputUri] as XslOutput;
			}
		}

		public string CurrentOutputUri
		{
			get
			{
				return this.currentOutputUri;
			}
		}

		public bool InsideCDataElement
		{
			get
			{
				return this.XPathContext.IsCData;
			}
		}

		public StringBuilder GetAvtStringBuilder()
		{
			if (this.avtSB == null)
			{
				this.avtSB = new StringBuilder();
			}
			return this.avtSB;
		}

		public string ReleaseAvtStringBuilder()
		{
			string result = this.avtSB.ToString();
			this.avtSB.Length = 0;
			return result;
		}

		private Hashtable GetParams(ArrayList withParams)
		{
			if (withParams == null)
			{
				return null;
			}
			Hashtable hashtable;
			if (this.paramPassingCache.Count != 0)
			{
				hashtable = (Hashtable)this.paramPassingCache.Pop();
				hashtable.Clear();
			}
			else
			{
				hashtable = new Hashtable();
			}
			int count = withParams.Count;
			for (int i = 0; i < count; i++)
			{
				XslVariableInformation xslVariableInformation = (XslVariableInformation)withParams[i];
				hashtable.Add(xslVariableInformation.Name, xslVariableInformation.Evaluate(this));
			}
			return hashtable;
		}

		public void ApplyTemplates(XPathNodeIterator nodes, XmlQualifiedName mode, ArrayList withParams)
		{
			Hashtable @params = this.GetParams(withParams);
			while (this.NodesetMoveNext(nodes))
			{
				this.PushNodeset(nodes);
				XslTemplate xslTemplate = this.FindTemplate(this.CurrentNode, mode);
				this.currentTemplateStack.Push(xslTemplate);
				xslTemplate.Evaluate(this, @params);
				this.currentTemplateStack.Pop();
				this.PopNodeset();
			}
			if (@params != null)
			{
				this.paramPassingCache.Push(@params);
			}
		}

		public void CallTemplate(XmlQualifiedName name, ArrayList withParams)
		{
			Hashtable @params = this.GetParams(withParams);
			XslTemplate xslTemplate = this.FindTemplate(name);
			this.currentTemplateStack.Push(null);
			xslTemplate.Evaluate(this, @params);
			this.currentTemplateStack.Pop();
			if (@params != null)
			{
				this.paramPassingCache.Push(@params);
			}
		}

		public void ApplyImports()
		{
			XslTemplate xslTemplate = (XslTemplate)this.currentTemplateStack.Peek();
			if (xslTemplate == null)
			{
				throw new XsltException("Invalid context for apply-imports", null, this.CurrentNode);
			}
			XslTemplate xslTemplate2;
			for (int i = xslTemplate.Parent.Imports.Count - 1; i >= 0; i--)
			{
				XslStylesheet xslStylesheet = (XslStylesheet)xslTemplate.Parent.Imports[i];
				xslTemplate2 = xslStylesheet.Templates.FindMatch(this.CurrentNode, xslTemplate.Mode, this);
				if (xslTemplate2 != null)
				{
					this.currentTemplateStack.Push(xslTemplate2);
					xslTemplate2.Evaluate(this);
					this.currentTemplateStack.Pop();
					return;
				}
			}
			switch (this.CurrentNode.NodeType)
			{
			case XPathNodeType.Root:
			case XPathNodeType.Element:
				if (xslTemplate.Mode == XmlQualifiedName.Empty)
				{
					xslTemplate2 = XslDefaultNodeTemplate.Instance;
				}
				else
				{
					xslTemplate2 = new XslDefaultNodeTemplate(xslTemplate.Mode);
				}
				goto IL_131;
			case XPathNodeType.Attribute:
			case XPathNodeType.Text:
			case XPathNodeType.SignificantWhitespace:
			case XPathNodeType.Whitespace:
				xslTemplate2 = XslDefaultTextTemplate.Instance;
				goto IL_131;
			case XPathNodeType.ProcessingInstruction:
			case XPathNodeType.Comment:
				xslTemplate2 = XslEmptyTemplate.Instance;
				goto IL_131;
			}
			xslTemplate2 = XslEmptyTemplate.Instance;
			IL_131:
			this.currentTemplateStack.Push(xslTemplate2);
			xslTemplate2.Evaluate(this);
			this.currentTemplateStack.Pop();
		}

		internal void OutputLiteralNamespaceUriNodes(Hashtable nsDecls, ArrayList excludedPrefixes, string localPrefixInCopy)
		{
			if (nsDecls == null)
			{
				return;
			}
			foreach (object obj in nsDecls)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = (string)dictionaryEntry.Key;
				string text2 = (string)dictionaryEntry.Value;
				if (!(localPrefixInCopy == text))
				{
					if (localPrefixInCopy == null || text.Length != 0 || this.XPathContext.ElementNamespace.Length != 0)
					{
						bool flag = false;
						if (this.style.ExcludeResultPrefixes != null)
						{
							foreach (XmlQualifiedName xmlQualifiedName in this.style.ExcludeResultPrefixes)
							{
								if (xmlQualifiedName.Namespace == text2)
								{
									flag = true;
								}
							}
						}
						if (!flag)
						{
							if (this.style.NamespaceAliases[text] == null)
							{
								string text3 = text2;
								switch (text3)
								{
								case "http://www.w3.org/1999/XSL/Transform":
									continue;
								case "http://www.w3.org/XML/1998/namespace":
									if ("xml" == text)
									{
										continue;
									}
									break;
								case "http://www.w3.org/2000/xmlns/":
									if ("xmlns" == text)
									{
										continue;
									}
									break;
								}
								if (excludedPrefixes == null || !excludedPrefixes.Contains(text))
								{
									this.Out.WriteNamespaceDecl(text, text2);
								}
							}
						}
					}
				}
			}
		}

		private XslTemplate FindTemplate(XPathNavigator node, XmlQualifiedName mode)
		{
			XslTemplate xslTemplate = this.style.Templates.FindMatch(this.CurrentNode, mode, this);
			if (xslTemplate != null)
			{
				return xslTemplate;
			}
			switch (node.NodeType)
			{
			case XPathNodeType.Root:
			case XPathNodeType.Element:
				if (mode == XmlQualifiedName.Empty)
				{
					return XslDefaultNodeTemplate.Instance;
				}
				return new XslDefaultNodeTemplate(mode);
			case XPathNodeType.Attribute:
			case XPathNodeType.Text:
			case XPathNodeType.SignificantWhitespace:
			case XPathNodeType.Whitespace:
				return XslDefaultTextTemplate.Instance;
			case XPathNodeType.ProcessingInstruction:
			case XPathNodeType.Comment:
				return XslEmptyTemplate.Instance;
			}
			return XslEmptyTemplate.Instance;
		}

		private XslTemplate FindTemplate(XmlQualifiedName name)
		{
			XslTemplate xslTemplate = this.style.Templates.FindTemplate(name);
			if (xslTemplate != null)
			{
				return xslTemplate;
			}
			throw new XsltException("Could not resolve named template " + name, null, this.CurrentNode);
		}

		public void PushForEachContext()
		{
			this.currentTemplateStack.Push(null);
		}

		public void PopForEachContext()
		{
			this.currentTemplateStack.Pop();
		}

		public XPathNodeIterator CurrentNodeset
		{
			get
			{
				return (XPathNodeIterator)this.nodesetStack[this.nodesetStack.Count - 1];
			}
		}

		public XPathNavigator CurrentNode
		{
			get
			{
				XPathNavigator xpathNavigator = this.CurrentNodeset.Current;
				if (xpathNavigator != null)
				{
					return xpathNavigator;
				}
				for (int i = this.nodesetStack.Count - 2; i >= 0; i--)
				{
					xpathNavigator = ((XPathNodeIterator)this.nodesetStack[i]).Current;
					if (xpathNavigator != null)
					{
						return xpathNavigator;
					}
				}
				return null;
			}
		}

		public bool NodesetMoveNext()
		{
			return this.NodesetMoveNext(this.CurrentNodeset);
		}

		public bool NodesetMoveNext(XPathNodeIterator iter)
		{
			return iter.MoveNext() && (iter.Current.NodeType != XPathNodeType.Whitespace || this.XPathContext.PreserveWhitespace(iter.Current) || this.NodesetMoveNext(iter));
		}

		public void PushNodeset(XPathNodeIterator itr)
		{
			BaseIterator baseIterator = itr as BaseIterator;
			baseIterator = ((baseIterator == null) ? new WrapperIterator(itr, null) : baseIterator);
			baseIterator.NamespaceManager = this.XPathContext;
			this.nodesetStack.Add(baseIterator);
		}

		public void PopNodeset()
		{
			this.nodesetStack.RemoveAt(this.nodesetStack.Count - 1);
		}

		public bool Matches(Pattern p, XPathNavigator n)
		{
			return p.Matches(n, this.XPathContext);
		}

		public object Evaluate(XPathExpression expr)
		{
			XPathNodeIterator currentNodeset = this.CurrentNodeset;
			BaseIterator baseIterator = (BaseIterator)currentNodeset;
			CompiledExpression compiledExpression = (CompiledExpression)expr;
			if (baseIterator.NamespaceManager == null)
			{
				baseIterator.NamespaceManager = compiledExpression.NamespaceManager;
			}
			return compiledExpression.Evaluate(baseIterator);
		}

		public string EvaluateString(XPathExpression expr)
		{
			XPathNodeIterator currentNodeset = this.CurrentNodeset;
			return currentNodeset.Current.EvaluateString(expr, currentNodeset, this.XPathContext);
		}

		public bool EvaluateBoolean(XPathExpression expr)
		{
			XPathNodeIterator currentNodeset = this.CurrentNodeset;
			return currentNodeset.Current.EvaluateBoolean(expr, currentNodeset, this.XPathContext);
		}

		public double EvaluateNumber(XPathExpression expr)
		{
			XPathNodeIterator currentNodeset = this.CurrentNodeset;
			return currentNodeset.Current.EvaluateNumber(expr, currentNodeset, this.XPathContext);
		}

		public XPathNodeIterator Select(XPathExpression expr)
		{
			return this.CurrentNodeset.Current.Select(expr, this.XPathContext);
		}

		public XslAttributeSet ResolveAttributeSet(XmlQualifiedName name)
		{
			return this.CompiledStyle.ResolveAttributeSet(name);
		}

		public int StackItemCount
		{
			get
			{
				if (this.currentStack == null)
				{
					return 0;
				}
				for (int i = 0; i < this.currentStack.Length; i++)
				{
					if (this.currentStack[i] == null)
					{
						return i;
					}
				}
				return this.currentStack.Length;
			}
		}

		public object GetStackItem(int slot)
		{
			return this.currentStack[slot];
		}

		public void SetStackItem(int slot, object o)
		{
			this.currentStack[slot] = o;
		}

		public void PushStack(int stackSize)
		{
			this.variableStack.Push(this.currentStack);
			this.currentStack = new object[stackSize];
		}

		public void PopStack()
		{
			this.currentStack = (object[])this.variableStack.Pop();
		}

		public void SetBusy(object o)
		{
			this.busyTable[o] = XslTransformProcessor.busyObject;
		}

		public void SetFree(object o)
		{
			this.busyTable.Remove(o);
		}

		public bool IsBusy(object o)
		{
			return this.busyTable[o] == XslTransformProcessor.busyObject;
		}

		public bool PushElementState(string prefix, string name, string ns, bool preserveWhitespace)
		{
			bool flag = this.IsCData(name, ns);
			this.XPathContext.PushScope();
			Outputter @out = this.Out;
			bool flag2 = flag;
			this.XPathContext.IsCData = flag2;
			@out.InsideCDataSection = flag2;
			this.XPathContext.WhitespaceHandling = true;
			this.XPathContext.ElementPrefix = prefix;
			this.XPathContext.ElementNamespace = ns;
			return flag;
		}

		private bool IsCData(string name, string ns)
		{
			for (int i = 0; i < this.Output.CDataSectionElements.Length; i++)
			{
				XmlQualifiedName xmlQualifiedName = this.Output.CDataSectionElements[i];
				if (xmlQualifiedName.Name == name && xmlQualifiedName.Namespace == ns)
				{
					return true;
				}
			}
			return false;
		}

		public void PopCDataState(bool isCData)
		{
			this.XPathContext.PopScope();
			this.Out.InsideCDataSection = this.XPathContext.IsCData;
		}

		public bool PreserveOutputWhitespace
		{
			get
			{
				return this.XPathContext.Whitespace;
			}
		}
	}
}
