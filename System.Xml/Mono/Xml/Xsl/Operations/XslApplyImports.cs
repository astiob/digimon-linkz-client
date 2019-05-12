using System;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslApplyImports : XslCompiledElement
	{
		public XslApplyImports(Compiler c) : base(c)
		{
		}

		protected override void Compile(Compiler c)
		{
			c.CheckExtraAttributes("apply-imports", new string[0]);
			if (c.Debugger != null)
			{
				c.Debugger.DebugCompile(c.Input);
			}
		}

		public override void Evaluate(XslTransformProcessor p)
		{
			if (p.Debugger != null)
			{
				p.Debugger.DebugExecute(p, base.DebugInput);
			}
			p.ApplyImports();
		}
	}
}
