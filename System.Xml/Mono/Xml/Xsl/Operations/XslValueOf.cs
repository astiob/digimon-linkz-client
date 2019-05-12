using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslValueOf : XslCompiledElement
	{
		private XPathExpression select;

		private bool disableOutputEscaping;

		public XslValueOf(Compiler c) : base(c)
		{
		}

		protected override void Compile(Compiler c)
		{
			if (c.Debugger != null)
			{
				c.Debugger.DebugCompile(base.DebugInput);
			}
			c.CheckExtraAttributes("value-of", new string[]
			{
				"select",
				"disable-output-escaping"
			});
			c.AssertAttribute("select");
			this.select = c.CompileExpression(c.GetAttribute("select"));
			this.disableOutputEscaping = c.ParseYesNoAttribute("disable-output-escaping", false);
			if (c.Input.MoveToFirstChild())
			{
				for (;;)
				{
					switch (c.Input.NodeType)
					{
					case XPathNodeType.Element:
						if (c.Input.NamespaceURI == "http://www.w3.org/1999/XSL/Transform")
						{
							goto Block_3;
						}
						break;
					case XPathNodeType.Text:
					case XPathNodeType.SignificantWhitespace:
						goto IL_D2;
					}
					if (!c.Input.MoveToNext())
					{
						return;
					}
				}
				Block_3:
				IL_D2:
				throw new XsltCompileException("XSLT value-of element cannot contain any child.", null, c.Input);
			}
		}

		public override void Evaluate(XslTransformProcessor p)
		{
			if (p.Debugger != null)
			{
				p.Debugger.DebugExecute(p, base.DebugInput);
			}
			if (!this.disableOutputEscaping)
			{
				p.Out.WriteString(p.EvaluateString(this.select));
			}
			else
			{
				p.Out.WriteRaw(p.EvaluateString(this.select));
			}
		}
	}
}
