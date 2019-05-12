using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslAttribute : XslCompiledElement
	{
		private XslAvt name;

		private XslAvt ns;

		private string calcName;

		private string calcNs;

		private string calcPrefix;

		private Hashtable nsDecls;

		private XslOperation value;

		public XslAttribute(Compiler c) : base(c)
		{
		}

		protected override void Compile(Compiler c)
		{
			if (c.Debugger != null)
			{
				c.Debugger.DebugCompile(c.Input);
			}
			XPathNavigator xpathNavigator = c.Input.Clone();
			this.nsDecls = c.GetNamespacesToCopy();
			c.CheckExtraAttributes("attribute", new string[]
			{
				"name",
				"namespace"
			});
			this.name = c.ParseAvtAttribute("name");
			if (this.name == null)
			{
				throw new XsltCompileException("Attribute \"name\" is required on XSLT attribute element", null, c.Input);
			}
			this.ns = c.ParseAvtAttribute("namespace");
			this.calcName = XslAvt.AttemptPreCalc(ref this.name);
			this.calcPrefix = string.Empty;
			if (this.calcName != null)
			{
				int num = this.calcName.IndexOf(':');
				this.calcPrefix = ((num >= 0) ? this.calcName.Substring(0, num) : string.Empty);
				this.calcName = ((num >= 0) ? this.calcName.Substring(num + 1, this.calcName.Length - num - 1) : this.calcName);
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
					throw new XsltCompileException("Invalid attribute name", innerException, c.Input);
				}
			}
			if (this.calcPrefix != string.Empty)
			{
				this.calcPrefix = c.CurrentStylesheet.GetActualPrefix(this.calcPrefix);
				if (this.calcPrefix == null)
				{
					this.calcPrefix = string.Empty;
				}
			}
			if (this.calcPrefix != string.Empty && this.ns == null)
			{
				this.calcNs = xpathNavigator.GetNamespace(this.calcPrefix);
			}
			else if (this.ns != null)
			{
				this.calcNs = XslAvt.AttemptPreCalc(ref this.ns);
			}
			if (c.Input.MoveToFirstChild())
			{
				this.value = c.CompileTemplateContent(XPathNodeType.Attribute);
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
			string text2 = (this.calcNs == null) ? ((this.ns == null) ? string.Empty : this.ns.Evaluate(p)) : this.calcNs;
			string text3 = (this.calcPrefix == null) ? string.Empty : this.calcPrefix;
			if (text == "xmlns")
			{
				return;
			}
			int num = text.IndexOf(':');
			if (num > 0)
			{
				text3 = text.Substring(0, num);
				text = text.Substring(num + 1);
				if (text2 == string.Empty && text3 == "xml")
				{
					text2 = "http://www.w3.org/XML/1998/namespace";
				}
				else if (text2 == string.Empty)
				{
					text2 = (string)this.nsDecls[text3];
					if (text2 == null)
					{
						text2 = string.Empty;
					}
				}
			}
			if (text3 == "xmlns")
			{
				text3 = string.Empty;
			}
			XmlConvert.VerifyName(text);
			p.Out.WriteAttributeString(text3, text, text2, (this.value != null) ? this.value.EvaluateAsString(p) : string.Empty);
		}
	}
}
