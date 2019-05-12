using System;
using System.Xml.Xsl;

namespace System.Xml.XPath
{
	internal class ExprVariable : Expression
	{
		protected XmlQualifiedName _name;

		protected bool resolvedName;

		public ExprVariable(XmlQualifiedName name, IStaticXsltContext ctx)
		{
			if (ctx != null)
			{
				name = ctx.LookupQName(name.ToString());
				this.resolvedName = true;
			}
			this._name = name;
		}

		public override string ToString()
		{
			return "$" + this._name.ToString();
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Any;
			}
		}

		public override XPathResultType GetReturnType(BaseIterator iter)
		{
			return XPathResultType.Any;
		}

		public override object Evaluate(BaseIterator iter)
		{
			XsltContext xsltContext = iter.NamespaceManager as XsltContext;
			if (xsltContext == null)
			{
				throw new XPathException(string.Format("XSLT context is required to resolve variable. Current namespace manager in current node-set '{0}' is '{1}'", iter.GetType(), (iter.NamespaceManager == null) ? null : iter.NamespaceManager.GetType()));
			}
			IXsltContextVariable xsltContextVariable;
			if (this.resolvedName)
			{
				xsltContextVariable = xsltContext.ResolveVariable(this._name);
			}
			else
			{
				xsltContextVariable = xsltContext.ResolveVariable(new XmlQualifiedName(this._name.Name, this._name.Namespace));
			}
			if (xsltContextVariable == null)
			{
				throw new XPathException("variable " + this._name.ToString() + " not found");
			}
			object obj = xsltContextVariable.Evaluate(xsltContext);
			XPathNodeIterator xpathNodeIterator = obj as XPathNodeIterator;
			if (xpathNodeIterator != null)
			{
				return (!(xpathNodeIterator is BaseIterator)) ? new WrapperIterator(xpathNodeIterator, iter.NamespaceManager) : xpathNodeIterator;
			}
			return obj;
		}

		internal override bool Peer
		{
			get
			{
				return false;
			}
		}
	}
}
