using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslChoose : XslCompiledElement
	{
		private XslOperation defaultChoice;

		private ArrayList conditions = new ArrayList();

		public XslChoose(Compiler c) : base(c)
		{
		}

		protected override void Compile(Compiler c)
		{
			if (c.Debugger != null)
			{
				c.Debugger.DebugCompile(c.Input);
			}
			c.CheckExtraAttributes("choose", new string[0]);
			if (!c.Input.MoveToFirstChild())
			{
				throw new XsltCompileException("Expecting non-empty element", null, c.Input);
			}
			for (;;)
			{
				if (c.Input.NodeType == XPathNodeType.Element)
				{
					if (!(c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform"))
					{
						if (this.defaultChoice != null)
						{
							break;
						}
						string localName = c.Input.LocalName;
						if (localName != null)
						{
							if (XslChoose.<>f__switch$map9 == null)
							{
								XslChoose.<>f__switch$map9 = new Dictionary<string, int>(2)
								{
									{
										"when",
										0
									},
									{
										"otherwise",
										1
									}
								};
							}
							int num;
							if (XslChoose.<>f__switch$map9.TryGetValue(localName, out num))
							{
								if (num == 0)
								{
									this.conditions.Add(new XslIf(c));
									goto IL_18C;
								}
								if (num == 1)
								{
									c.CheckExtraAttributes("otherwise", new string[0]);
									if (c.Input.MoveToFirstChild())
									{
										this.defaultChoice = c.CompileTemplateContent();
										c.Input.MoveToParent();
									}
									goto IL_18C;
								}
							}
						}
						if (c.CurrentStylesheet.Version == "1.0")
						{
							goto Block_12;
						}
					}
				}
				IL_18C:
				if (!c.Input.MoveToNext())
				{
					goto Block_13;
				}
			}
			throw new XsltCompileException("otherwise attribute must be last", null, c.Input);
			Block_12:
			throw new XsltCompileException("XSLT choose element accepts only when and otherwise elements", null, c.Input);
			Block_13:
			c.Input.MoveToParent();
			if (this.conditions.Count == 0)
			{
				throw new XsltCompileException("Choose must have 1 or more when elements", null, c.Input);
			}
		}

		public override void Evaluate(XslTransformProcessor p)
		{
			if (p.Debugger != null)
			{
				p.Debugger.DebugExecute(p, base.DebugInput);
			}
			int count = this.conditions.Count;
			for (int i = 0; i < count; i++)
			{
				if (((XslIf)this.conditions[i]).EvaluateIfTrue(p))
				{
					return;
				}
			}
			if (this.defaultChoice != null)
			{
				this.defaultChoice.Evaluate(p);
			}
		}
	}
}
