using Mono.Xml.Xsl.Operations;
using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal class XslAttributeSet : XslCompiledElement
	{
		private XmlQualifiedName name;

		private ArrayList usedAttributeSets = new ArrayList();

		private ArrayList attributes = new ArrayList();

		public XslAttributeSet(Compiler c) : base(c)
		{
		}

		public XmlQualifiedName Name
		{
			get
			{
				return this.name;
			}
		}

		protected override void Compile(Compiler c)
		{
			this.name = c.ParseQNameAttribute("name");
			XmlQualifiedName[] array = c.ParseQNameListAttribute("use-attribute-sets");
			if (array != null)
			{
				foreach (XmlQualifiedName value in array)
				{
					this.usedAttributeSets.Add(value);
				}
			}
			if (!c.Input.MoveToFirstChild())
			{
				return;
			}
			for (;;)
			{
				if (c.Input.NodeType == XPathNodeType.Element)
				{
					if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform" || c.Input.LocalName != "attribute")
					{
						break;
					}
					this.attributes.Add(new XslAttribute(c));
				}
				if (!c.Input.MoveToNext())
				{
					goto Block_5;
				}
			}
			throw new XsltCompileException("Invalid attr set content", null, c.Input);
			Block_5:
			c.Input.MoveToParent();
		}

		public void Merge(XslAttributeSet s)
		{
			this.attributes.AddRange(s.attributes);
			foreach (object obj in s.usedAttributeSets)
			{
				XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)obj;
				if (!this.usedAttributeSets.Contains(xmlQualifiedName))
				{
					this.usedAttributeSets.Add(xmlQualifiedName);
				}
			}
		}

		public override void Evaluate(XslTransformProcessor p)
		{
			p.SetBusy(this);
			if (this.usedAttributeSets != null)
			{
				for (int i = 0; i < this.usedAttributeSets.Count; i++)
				{
					XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)this.usedAttributeSets[i];
					XslAttributeSet xslAttributeSet = p.ResolveAttributeSet(xmlQualifiedName);
					if (xslAttributeSet == null)
					{
						throw new XsltException("Could not resolve attribute set", null, p.CurrentNode);
					}
					if (p.IsBusy(xslAttributeSet))
					{
						throw new XsltException("circular dependency", null, p.CurrentNode);
					}
					xslAttributeSet.Evaluate(p);
				}
			}
			for (int j = 0; j < this.attributes.Count; j++)
			{
				((XslAttribute)this.attributes[j]).Evaluate(p);
			}
			p.SetFree(this);
		}
	}
}
