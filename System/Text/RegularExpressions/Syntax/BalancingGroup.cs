using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal class BalancingGroup : CapturingGroup
	{
		private CapturingGroup balance;

		public BalancingGroup()
		{
			this.balance = null;
		}

		public CapturingGroup Balance
		{
			get
			{
				return this.balance;
			}
			set
			{
				this.balance = value;
			}
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			LinkRef linkRef = cmp.NewLink();
			cmp.EmitBalanceStart(base.Index, this.balance.Index, base.IsNamed, linkRef);
			int count = base.Expressions.Count;
			for (int i = 0; i < count; i++)
			{
				Expression expression;
				if (reverse)
				{
					expression = base.Expressions[count - i - 1];
				}
				else
				{
					expression = base.Expressions[i];
				}
				expression.Compile(cmp, reverse);
			}
			cmp.EmitBalance();
			cmp.ResolveLink(linkRef);
		}
	}
}
