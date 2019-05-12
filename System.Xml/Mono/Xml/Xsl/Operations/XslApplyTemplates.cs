using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslApplyTemplates : XslCompiledElement
	{
		private XPathExpression select;

		private XmlQualifiedName mode;

		private ArrayList withParams;

		private XslSortEvaluator sortEvaluator;

		public XslApplyTemplates(Compiler c) : base(c)
		{
		}

		protected override void Compile(Compiler c)
		{
			if (c.Debugger != null)
			{
				c.Debugger.DebugCompile(c.Input);
			}
			c.CheckExtraAttributes("apply-templates", new string[]
			{
				"select",
				"mode"
			});
			this.select = c.CompileExpression(c.GetAttribute("select"));
			this.mode = c.ParseQNameAttribute("mode");
			ArrayList arrayList = null;
			if (c.Input.MoveToFirstChild())
			{
				for (;;)
				{
					switch (c.Input.NodeType)
					{
					case XPathNodeType.Element:
					{
						if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
						{
							goto Block_3;
						}
						string localName = c.Input.LocalName;
						if (localName == null)
						{
							goto IL_1AF;
						}
						if (XslApplyTemplates.<>f__switch$map7 == null)
						{
							XslApplyTemplates.<>f__switch$map7 = new Dictionary<string, int>(2)
							{
								{
									"with-param",
									0
								},
								{
									"sort",
									1
								}
							};
						}
						int num;
						if (XslApplyTemplates.<>f__switch$map7.TryGetValue(localName, out num))
						{
							if (num != 0)
							{
								if (num != 1)
								{
									goto Block_8;
								}
								if (arrayList == null)
								{
									arrayList = new ArrayList();
								}
								if (this.select == null)
								{
									this.select = c.CompileExpression("*");
								}
								arrayList.Add(new Sort(c));
							}
							else
							{
								if (this.withParams == null)
								{
									this.withParams = new ArrayList();
								}
								this.withParams.Add(new XslVariableInformation(c));
							}
							goto IL_1ED;
						}
						goto IL_1AF;
					}
					case XPathNodeType.SignificantWhitespace:
					case XPathNodeType.Whitespace:
					case XPathNodeType.ProcessingInstruction:
					case XPathNodeType.Comment:
						goto IL_1ED;
					}
					break;
					IL_1ED:
					if (!c.Input.MoveToNext())
					{
						goto Block_12;
					}
				}
				goto IL_1C6;
				Block_3:
				throw new XsltCompileException("Unexpected element", null, c.Input);
				Block_8:
				IL_1AF:
				throw new XsltCompileException("Unexpected element", null, c.Input);
				IL_1C6:
				throw new XsltCompileException("Unexpected node type " + c.Input.NodeType, null, c.Input);
				Block_12:
				c.Input.MoveToParent();
			}
			if (arrayList != null)
			{
				this.sortEvaluator = new XslSortEvaluator(this.select, (Sort[])arrayList.ToArray(typeof(Sort)));
			}
		}

		public override void Evaluate(XslTransformProcessor p)
		{
			if (p.Debugger != null)
			{
				p.Debugger.DebugExecute(p, base.DebugInput);
			}
			if (this.select == null)
			{
				p.ApplyTemplates(p.CurrentNode.SelectChildren(XPathNodeType.All), this.mode, this.withParams);
			}
			else
			{
				XPathNodeIterator nodes = (this.sortEvaluator == null) ? p.Select(this.select) : this.sortEvaluator.SortedSelect(p);
				p.ApplyTemplates(nodes, this.mode, this.withParams);
			}
		}
	}
}
