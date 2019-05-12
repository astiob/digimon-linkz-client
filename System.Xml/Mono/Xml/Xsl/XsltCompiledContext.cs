using Mono.Xml.XPath;
using System;
using System.Collections;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal class XsltCompiledContext : XsltContext
	{
		private Hashtable keyNameCache = new Hashtable();

		private Hashtable keyIndexTables = new Hashtable();

		private Hashtable patternNavCaches = new Hashtable();

		private XslTransformProcessor p;

		private XsltCompiledContext.XsltContextInfo[] scopes;

		private int scopeAt;

		public XsltCompiledContext(XslTransformProcessor p) : base(new NameTable())
		{
			this.p = p;
			this.scopes = new XsltCompiledContext.XsltContextInfo[10];
			for (int i = 0; i < 10; i++)
			{
				this.scopes[i] = new XsltCompiledContext.XsltContextInfo();
			}
		}

		public XslTransformProcessor Processor
		{
			get
			{
				return this.p;
			}
		}

		public override string DefaultNamespace
		{
			get
			{
				return string.Empty;
			}
		}

		public XPathNavigator GetNavCache(Pattern p, XPathNavigator node)
		{
			XPathNavigator xpathNavigator = this.patternNavCaches[p] as XPathNavigator;
			if (xpathNavigator == null || !xpathNavigator.MoveTo(node))
			{
				xpathNavigator = node.Clone();
				this.patternNavCaches[p] = xpathNavigator;
			}
			return xpathNavigator;
		}

		public object EvaluateKey(IStaticXsltContext staticContext, BaseIterator iter, Expression nameExpr, Expression valueExpr)
		{
			XmlQualifiedName keyName = this.GetKeyName(staticContext, iter, nameExpr);
			KeyIndexTable indexTable = this.GetIndexTable(keyName);
			return indexTable.Evaluate(iter, valueExpr);
		}

		public bool MatchesKey(XPathNavigator nav, IStaticXsltContext staticContext, string name, string value)
		{
			XmlQualifiedName name2 = XslNameUtil.FromString(name, staticContext);
			KeyIndexTable indexTable = this.GetIndexTable(name2);
			return indexTable.Matches(nav, value, this);
		}

		private XmlQualifiedName GetKeyName(IStaticXsltContext staticContext, BaseIterator iter, Expression nameExpr)
		{
			XmlQualifiedName xmlQualifiedName;
			if (nameExpr.HasStaticValue)
			{
				xmlQualifiedName = (XmlQualifiedName)this.keyNameCache[nameExpr];
				if (xmlQualifiedName == null)
				{
					xmlQualifiedName = XslNameUtil.FromString(nameExpr.EvaluateString(iter), staticContext);
					this.keyNameCache[nameExpr] = xmlQualifiedName;
				}
			}
			else
			{
				xmlQualifiedName = XslNameUtil.FromString(nameExpr.EvaluateString(iter), this);
			}
			return xmlQualifiedName;
		}

		private KeyIndexTable GetIndexTable(XmlQualifiedName name)
		{
			KeyIndexTable keyIndexTable = this.keyIndexTables[name] as KeyIndexTable;
			if (keyIndexTable == null)
			{
				keyIndexTable = new KeyIndexTable(this, this.p.CompiledStyle.ResolveKey(name));
				this.keyIndexTables[name] = keyIndexTable;
			}
			return keyIndexTable;
		}

		public override string LookupNamespace(string prefix)
		{
			throw new InvalidOperationException("we should never get here");
		}

		internal override IXsltContextFunction ResolveFunction(XmlQualifiedName name, XPathResultType[] argTypes)
		{
			string @namespace = name.Namespace;
			if (@namespace == null)
			{
				return null;
			}
			object obj = null;
			if (this.p.Arguments != null)
			{
				obj = this.p.Arguments.GetExtensionObject(@namespace);
			}
			bool isScript = false;
			if (obj == null)
			{
				obj = this.p.ScriptManager.GetExtensionObject(@namespace);
				if (obj == null)
				{
					return null;
				}
				isScript = true;
			}
			MethodInfo methodInfo = this.FindBestMethod(obj.GetType(), name.Name, argTypes, isScript);
			if (methodInfo != null)
			{
				return new XsltExtensionFunction(obj, methodInfo, this.p.CurrentNode);
			}
			return null;
		}

		private MethodInfo FindBestMethod(Type t, string name, XPathResultType[] argTypes, bool isScript)
		{
			MethodInfo[] methods = t.GetMethods(((!isScript) ? BindingFlags.Public : (BindingFlags.Public | BindingFlags.NonPublic)) | BindingFlags.Instance | BindingFlags.Static);
			if (methods.Length == 0)
			{
				return null;
			}
			if (argTypes == null)
			{
				return methods[0];
			}
			int num = 0;
			int num2 = argTypes.Length;
			for (int i = 0; i < methods.Length; i++)
			{
				if (methods[i].Name == name && methods[i].GetParameters().Length == num2)
				{
					methods[num++] = methods[i];
				}
			}
			int num3 = num;
			if (num3 == 0)
			{
				return null;
			}
			if (num3 == 1)
			{
				return methods[0];
			}
			for (int j = 0; j < num3; j++)
			{
				bool flag = true;
				ParameterInfo[] parameters = methods[j].GetParameters();
				for (int k = 0; k < parameters.Length; k++)
				{
					XPathResultType xpathResultType = argTypes[k];
					if (xpathResultType != XPathResultType.Any)
					{
						XPathResultType xpathType = XPFuncImpl.GetXPathType(parameters[k].ParameterType, this.p.CurrentNode);
						if (xpathType != xpathResultType && xpathType != XPathResultType.Any)
						{
							flag = false;
							break;
						}
						if (xpathType == XPathResultType.Any && xpathResultType != XPathResultType.NodeSet && parameters[k].ParameterType != typeof(object))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					return methods[j];
				}
			}
			return null;
		}

		public override IXsltContextVariable ResolveVariable(string prefix, string name)
		{
			throw new InvalidOperationException("shouldn't get here");
		}

		public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] ArgTypes)
		{
			throw new InvalidOperationException("XsltCompiledContext exception: shouldn't get here.");
		}

		internal override IXsltContextVariable ResolveVariable(XmlQualifiedName q)
		{
			return this.p.CompiledStyle.ResolveVariable(q);
		}

		public override int CompareDocument(string baseUri, string nextBaseUri)
		{
			return baseUri.GetHashCode().CompareTo(nextBaseUri.GetHashCode());
		}

		public override bool PreserveWhitespace(XPathNavigator nav)
		{
			return this.p.CompiledStyle.Style.GetPreserveWhitespace(nav);
		}

		public override bool Whitespace
		{
			get
			{
				return this.WhitespaceHandling;
			}
		}

		public bool IsCData
		{
			get
			{
				return this.scopes[this.scopeAt].IsCData;
			}
			set
			{
				this.scopes[this.scopeAt].IsCData = value;
			}
		}

		public bool WhitespaceHandling
		{
			get
			{
				return this.scopes[this.scopeAt].PreserveWhitespace;
			}
			set
			{
				this.scopes[this.scopeAt].PreserveWhitespace = value;
			}
		}

		public string ElementPrefix
		{
			get
			{
				return this.scopes[this.scopeAt].ElementPrefix;
			}
			set
			{
				this.scopes[this.scopeAt].ElementPrefix = value;
			}
		}

		public string ElementNamespace
		{
			get
			{
				return this.scopes[this.scopeAt].ElementNamespace;
			}
			set
			{
				this.scopes[this.scopeAt].ElementNamespace = value;
			}
		}

		private void ExtendScope()
		{
			XsltCompiledContext.XsltContextInfo[] sourceArray = this.scopes;
			this.scopes = new XsltCompiledContext.XsltContextInfo[this.scopeAt * 2 + 1];
			if (this.scopeAt > 0)
			{
				Array.Copy(sourceArray, 0, this.scopes, 0, this.scopeAt);
			}
		}

		public override bool PopScope()
		{
			base.PopScope();
			if (this.scopeAt == -1)
			{
				return false;
			}
			this.scopeAt--;
			return true;
		}

		public override void PushScope()
		{
			base.PushScope();
			this.scopeAt++;
			if (this.scopeAt == this.scopes.Length)
			{
				this.ExtendScope();
			}
			if (this.scopes[this.scopeAt] == null)
			{
				this.scopes[this.scopeAt] = new XsltCompiledContext.XsltContextInfo();
			}
			else
			{
				this.scopes[this.scopeAt].Clear();
			}
		}

		private class XsltContextInfo
		{
			public bool IsCData;

			public bool PreserveWhitespace = true;

			public string ElementPrefix;

			public string ElementNamespace;

			public void Clear()
			{
				this.IsCData = false;
				this.PreserveWhitespace = true;
				this.ElementPrefix = (this.ElementNamespace = null);
			}
		}
	}
}
