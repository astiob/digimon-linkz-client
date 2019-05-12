using System;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslLocalParam : XslLocalVariable
	{
		public XslLocalParam(Compiler c) : base(c)
		{
		}

		public override void Evaluate(XslTransformProcessor p)
		{
			if (p.Debugger != null)
			{
				p.Debugger.DebugExecute(p, base.DebugInput);
			}
			if (p.GetStackItem(this.slot) != null)
			{
				return;
			}
			if (p.Arguments != null && this.var.Select == null && this.var.Content == null)
			{
				object param = p.Arguments.GetParam(base.Name.Name, base.Name.Namespace);
				if (param != null)
				{
					this.Override(p, param);
					return;
				}
			}
			base.Evaluate(p);
		}

		public void Override(XslTransformProcessor p, object paramVal)
		{
			p.SetStackItem(this.slot, paramVal);
		}

		public override bool IsParam
		{
			get
			{
				return true;
			}
		}
	}
}
