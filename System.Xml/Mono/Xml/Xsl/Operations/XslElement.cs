using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslElement : XslCompiledElement
	{
		private XslAvt name;

		private XslAvt ns;

		private string calcName;

		private string calcNs;

		private string calcPrefix;

		private Hashtable nsDecls;

		private bool isEmptyElement;

		private XslOperation value;

		private XmlQualifiedName[] useAttributeSets;

		public XslElement(Compiler c) : base(c)
		{
		}

		protected override void Compile(Compiler c)
		{
			if (c.Debugger != null)
			{
				c.Debugger.DebugCompile(c.Input);
			}
			c.CheckExtraAttributes("element", new string[]
			{
				"name",
				"namespace",
				"use-attribute-sets"
			});
			this.name = c.ParseAvtAttribute("name");
			this.ns = c.ParseAvtAttribute("namespace");
			this.nsDecls = c.GetNamespacesToCopy();
			this.calcName = XslAvt.AttemptPreCalc(ref this.name);
			if (this.calcName != null)
			{
				int num = this.calcName.IndexOf(':');
				if (num == 0)
				{
					throw new XsltCompileException("Invalid name attribute", null, c.Input);
				}
				this.calcPrefix = ((num >= 0) ? this.calcName.Substring(0, num) : string.Empty);
				if (num > 0)
				{
					this.calcName = this.calcName.Substring(num + 1);
				}
				try
				{
					XmlConvert.VerifyNCName(this.calcName);
					if (this.calcPrefix != string.Empty)
					{
						XmlConvert.VerifyNCName(this.calcPrefix);
					}
				}
				catch (XmlException innerException)
				{
					throw new XsltCompileException("Invalid name attribute", innerException, c.Input);
				}
				if (this.ns == null)
				{
					this.calcNs = c.Input.GetNamespace(this.calcPrefix);
					if (this.calcPrefix != string.Empty && this.calcNs == string.Empty)
					{
						throw new XsltCompileException("Invalid name attribute", null, c.Input);
					}
				}
			}
			else if (this.ns != null)
			{
				this.calcNs = XslAvt.AttemptPreCalc(ref this.ns);
			}
			this.useAttributeSets = c.ParseQNameListAttribute("use-attribute-sets");
			this.isEmptyElement = c.Input.IsEmptyElement;
			if (c.Input.MoveToFirstChild())
			{
				this.value = c.CompileTemplateContent(XPathNodeType.Element);
				c.Input.MoveToParent();
			}
		}

		public override void Evaluate(XslTransformProcessor p)
		{
			if (p.Debugger != null)
			{
				p.Debugger.DebugExecute(p, base.DebugInput);
			}
			string text = (this.calcName == null) ? this.name.Evaluate(p) : this.calcName;
			string text2 = (this.calcNs == null) ? ((this.ns == null) ? null : this.ns.Evaluate(p)) : this.calcNs;
			XmlQualifiedName xmlQualifiedName = XslNameUtil.FromString(text, this.nsDecls);
			string text3 = xmlQualifiedName.Name;
			if (text2 == null)
			{
				text2 = xmlQualifiedName.Namespace;
			}
			int num = text.IndexOf(':');
			if (num > 0)
			{
				this.calcPrefix = text.Substring(0, num);
			}
			else if (num == 0)
			{
				XmlConvert.VerifyNCName(string.Empty);
			}
			string text4 = (this.calcPrefix == null) ? string.Empty : this.calcPrefix;
			if (text4 != string.Empty)
			{
				XmlConvert.VerifyNCName(text4);
			}
			XmlConvert.VerifyNCName(text3);
			bool insideCDataElement = p.InsideCDataElement;
			p.PushElementState(text4, text3, text2, false);
			p.Out.WriteStartElement(text4, text3, text2);
			if (this.useAttributeSets != null)
			{
				foreach (XmlQualifiedName xmlQualifiedName2 in this.useAttributeSets)
				{
					p.ResolveAttributeSet(xmlQualifiedName2).Evaluate(p);
				}
			}
			if (this.value != null)
			{
				this.value.Evaluate(p);
			}
			if (this.isEmptyElement && this.useAttributeSets == null)
			{
				p.Out.WriteEndElement();
			}
			else
			{
				p.Out.WriteFullEndElement();
			}
			p.PopCDataState(insideCDataElement);
		}
	}
}
