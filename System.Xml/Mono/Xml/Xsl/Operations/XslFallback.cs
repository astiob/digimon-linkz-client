using System;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslFallback : XslCompiledElement
	{
		private XslOperation children;

		public XslFallback(Compiler c) : base(c)
		{
		}

		protected override void Compile(Compiler c)
		{
			if (c.Debugger != null)
			{
				c.Debugger.DebugCompile(c.Input);
			}
			c.CheckExtraAttributes("fallback", new string[0]);
			if (!c.Input.MoveToFirstChild())
			{
				return;
			}
			this.children = c.CompileTemplateContent();
			c.Input.MoveToParent();
		}

		public override void Evaluate(XslTransformProcessor p)
		{
			if (p.Debugger != null)
			{
				p.Debugger.DebugExecute(p, base.DebugInput);
			}
			this.children.Evaluate(p);
		}
	}
}
