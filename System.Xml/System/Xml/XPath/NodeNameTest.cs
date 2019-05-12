using System;
using System.Xml.Xsl;

namespace System.Xml.XPath
{
	internal class NodeNameTest : NodeTest
	{
		protected XmlQualifiedName _name;

		protected readonly bool resolvedName;

		public NodeNameTest(Axes axis, XmlQualifiedName name, IStaticXsltContext ctx) : base(axis)
		{
			if (ctx != null)
			{
				name = ctx.LookupQName(name.ToString());
				this.resolvedName = true;
			}
			this._name = name;
		}

		public NodeNameTest(Axes axis, XmlQualifiedName name, bool resolvedName) : base(axis)
		{
			this._name = name;
			this.resolvedName = resolvedName;
		}

		internal NodeNameTest(NodeNameTest source, Axes axis) : base(axis)
		{
			this._name = source._name;
			this.resolvedName = source.resolvedName;
		}

		public override string ToString()
		{
			return this._axis.ToString() + "::" + this._name.ToString();
		}

		public XmlQualifiedName Name
		{
			get
			{
				return this._name;
			}
		}

		public override bool Match(IXmlNamespaceResolver nsm, XPathNavigator nav)
		{
			if (nav.NodeType != this._axis.NodeType)
			{
				return false;
			}
			if (this._name.Name != string.Empty && this._name.Name != nav.LocalName)
			{
				return false;
			}
			string text = string.Empty;
			if (this._name.Namespace != string.Empty)
			{
				if (this.resolvedName)
				{
					text = this._name.Namespace;
				}
				else if (nsm != null)
				{
					text = nsm.LookupNamespace(this._name.Namespace);
				}
				if (text == null)
				{
					throw new XPathException("Invalid namespace prefix: " + this._name.Namespace);
				}
			}
			return text == nav.NamespaceURI;
		}

		public override void GetInfo(out string name, out string ns, out XPathNodeType nodetype, IXmlNamespaceResolver nsm)
		{
			nodetype = this._axis.NodeType;
			if (this._name.Name != string.Empty)
			{
				name = this._name.Name;
			}
			else
			{
				name = null;
			}
			ns = string.Empty;
			if (nsm != null && this._name.Namespace != string.Empty)
			{
				if (this.resolvedName)
				{
					ns = this._name.Namespace;
				}
				else
				{
					ns = nsm.LookupNamespace(this._name.Namespace);
				}
				if (ns == null)
				{
					throw new XPathException("Invalid namespace prefix: " + this._name.Namespace);
				}
			}
		}
	}
}
