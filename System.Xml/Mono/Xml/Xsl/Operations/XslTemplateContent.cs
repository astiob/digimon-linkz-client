using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslTemplateContent : XslCompiledElementBase
	{
		private ArrayList content = new ArrayList();

		private bool hasStack;

		private int stackSize;

		private XPathNodeType parentType;

		private bool xslForEach;

		public XslTemplateContent(Compiler c, XPathNodeType parentType, bool xslForEach) : base(c)
		{
			this.parentType = parentType;
			this.xslForEach = xslForEach;
			this.Compile(c);
		}

		public XPathNodeType ParentType
		{
			get
			{
				return this.parentType;
			}
		}

		protected override void Compile(Compiler c)
		{
			if (c.Debugger != null)
			{
				c.Debugger.DebugCompile(base.DebugInput);
			}
			this.hasStack = (c.CurrentVariableScope == null);
			c.PushScope();
			XPathNavigator input;
			for (;;)
			{
				input = c.Input;
				switch (input.NodeType)
				{
				case XPathNodeType.Element:
				{
					string namespaceURI = input.NamespaceURI;
					if (namespaceURI == null)
					{
						goto IL_458;
					}
					if (XslTemplateContent.<>f__switch$mapD == null)
					{
						XslTemplateContent.<>f__switch$mapD = new Dictionary<string, int>(1)
						{
							{
								"http://www.w3.org/1999/XSL/Transform",
								0
							}
						};
					}
					int num;
					if (!XslTemplateContent.<>f__switch$mapD.TryGetValue(namespaceURI, out num))
					{
						goto IL_458;
					}
					if (num != 0)
					{
						goto IL_458;
					}
					string localName = input.LocalName;
					if (localName == null)
					{
						goto IL_43C;
					}
					if (XslTemplateContent.<>f__switch$mapC == null)
					{
						XslTemplateContent.<>f__switch$mapC = new Dictionary<string, int>(19)
						{
							{
								"apply-imports",
								0
							},
							{
								"apply-templates",
								1
							},
							{
								"attribute",
								2
							},
							{
								"call-template",
								3
							},
							{
								"choose",
								4
							},
							{
								"comment",
								5
							},
							{
								"copy",
								6
							},
							{
								"copy-of",
								7
							},
							{
								"element",
								8
							},
							{
								"fallback",
								9
							},
							{
								"for-each",
								10
							},
							{
								"if",
								11
							},
							{
								"message",
								12
							},
							{
								"number",
								13
							},
							{
								"processing-instruction",
								14
							},
							{
								"text",
								15
							},
							{
								"value-of",
								16
							},
							{
								"variable",
								17
							},
							{
								"sort",
								18
							}
						};
					}
					int num2;
					if (!XslTemplateContent.<>f__switch$mapC.TryGetValue(localName, out num2))
					{
						goto IL_43C;
					}
					switch (num2)
					{
					case 0:
						this.content.Add(new XslApplyImports(c));
						break;
					case 1:
						this.content.Add(new XslApplyTemplates(c));
						break;
					case 2:
						if (this.ParentType == XPathNodeType.All || this.ParentType == XPathNodeType.Element)
						{
							this.content.Add(new XslAttribute(c));
						}
						break;
					case 3:
						this.content.Add(new XslCallTemplate(c));
						break;
					case 4:
						this.content.Add(new XslChoose(c));
						break;
					case 5:
						if (this.ParentType == XPathNodeType.All || this.ParentType == XPathNodeType.Element)
						{
							this.content.Add(new XslComment(c));
						}
						break;
					case 6:
						this.content.Add(new XslCopy(c));
						break;
					case 7:
						this.content.Add(new XslCopyOf(c));
						break;
					case 8:
						if (this.ParentType == XPathNodeType.All || this.ParentType == XPathNodeType.Element)
						{
							this.content.Add(new XslElement(c));
						}
						break;
					case 9:
						break;
					case 10:
						this.content.Add(new XslForEach(c));
						break;
					case 11:
						this.content.Add(new XslIf(c));
						break;
					case 12:
						this.content.Add(new XslMessage(c));
						break;
					case 13:
						this.content.Add(new XslNumber(c));
						break;
					case 14:
						if (this.ParentType == XPathNodeType.All || this.ParentType == XPathNodeType.Element)
						{
							this.content.Add(new XslProcessingInstruction(c));
						}
						break;
					case 15:
						this.content.Add(new XslText(c, false));
						break;
					case 16:
						this.content.Add(new XslValueOf(c));
						break;
					case 17:
						this.content.Add(new XslLocalVariable(c));
						break;
					case 18:
						if (!this.xslForEach)
						{
							goto IL_42F;
						}
						break;
					default:
						goto IL_43C;
					}
					break;
					IL_43C:
					this.content.Add(new XslNotSupportedOperation(c));
					break;
					IL_458:
					if (!c.IsExtensionNamespace(input.NamespaceURI))
					{
						this.content.Add(new XslLiteralElement(c));
					}
					else if (input.MoveToFirstChild())
					{
						do
						{
							if (input.NamespaceURI == "http://www.w3.org/1999/XSL/Transform" && input.LocalName == "fallback")
							{
								this.content.Add(new XslFallback(c));
							}
						}
						while (input.MoveToNext());
						input.MoveToParent();
					}
					break;
				}
				case XPathNodeType.Text:
					this.content.Add(new XslText(c, false));
					break;
				case XPathNodeType.SignificantWhitespace:
					this.content.Add(new XslText(c, true));
					break;
				}
				IL_518:
				if (!c.Input.MoveToNext())
				{
					goto Block_20;
				}
				continue;
				goto IL_518;
			}
			IL_42F:
			throw new XsltCompileException("'sort' element is not allowed here as a templete content", null, input);
			Block_20:
			if (this.hasStack)
			{
				this.stackSize = c.PopScope().VariableHighTide;
				this.hasStack = (this.stackSize > 0);
			}
			else
			{
				c.PopScope();
			}
		}

		public override void Evaluate(XslTransformProcessor p)
		{
			if (p.Debugger != null)
			{
				p.Debugger.DebugExecute(p, base.DebugInput);
			}
			if (this.hasStack)
			{
				p.PushStack(this.stackSize);
			}
			int count = this.content.Count;
			for (int i = 0; i < count; i++)
			{
				((XslOperation)this.content[i]).Evaluate(p);
			}
			if (this.hasStack)
			{
				p.PopStack();
			}
		}
	}
}
