using System;
using System.Collections;
using System.Xml.XPath;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslForEach : XslCompiledElement
	{
		private XPathExpression select;

		private XslOperation children;

		private XslSortEvaluator sortEvaluator;

		public XslForEach(Compiler c) : base(c)
		{
		}

		protected override void Compile(Compiler c)
		{
			if (c.Debugger != null)
			{
				c.Debugger.DebugCompile(c.Input);
			}
			c.CheckExtraAttributes("for-each", new string[]
			{
				"select"
			});
			c.AssertAttribute("select");
			this.select = c.CompileExpression(c.GetAttribute("select"));
			ArrayList arrayList = null;
			if (c.Input.MoveToFirstChild())
			{
				bool flag = true;
				while (c.Input.NodeType != XPathNodeType.Text)
				{
					if (c.Input.NodeType == XPathNodeType.Element)
					{
						if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
						{
							flag = false;
							goto IL_104;
						}
						if (c.Input.LocalName != "sort")
						{
							flag = false;
							goto IL_104;
						}
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						arrayList.Add(new Sort(c));
					}
					if (c.Input.MoveToNext())
					{
						continue;
					}
					IL_104:
					if (!flag)
					{
						this.children = c.CompileTemplateContent();
					}
					c.Input.MoveToParent();
					goto IL_122;
				}
				flag = false;
				goto IL_104;
			}
			IL_122:
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
			XPathNodeIterator xpathNodeIterator = (this.sortEvaluator == null) ? p.Select(this.select) : this.sortEvaluator.SortedSelect(p);
			while (p.NodesetMoveNext(xpathNodeIterator))
			{
				p.PushNodeset(xpathNodeIterator);
				p.PushForEachContext();
				this.children.Evaluate(p);
				p.PopForEachContext();
				p.PopNodeset();
			}
		}
	}
}
