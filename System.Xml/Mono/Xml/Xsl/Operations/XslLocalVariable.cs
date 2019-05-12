using System;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslLocalVariable : XslGeneralVariable
	{
		protected int slot;

		public XslLocalVariable(Compiler c) : base(c)
		{
			this.slot = c.AddVariable(this);
		}

		public override void Evaluate(XslTransformProcessor p)
		{
			if (p.Debugger != null)
			{
				p.Debugger.DebugExecute(p, base.DebugInput);
			}
			p.SetStackItem(this.slot, this.var.Evaluate(p));
		}

		protected override object GetValue(XslTransformProcessor p)
		{
			return p.GetStackItem(this.slot);
		}

		public bool IsEvaluated(XslTransformProcessor p)
		{
			return p.GetStackItem(this.slot) != null;
		}

		public override bool IsLocal
		{
			get
			{
				return true;
			}
		}

		public override bool IsParam
		{
			get
			{
				return false;
			}
		}
	}
}
