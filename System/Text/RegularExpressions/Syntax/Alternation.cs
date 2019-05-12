using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal class Alternation : CompositeExpression
	{
		public ExpressionCollection Alternatives
		{
			get
			{
				return base.Expressions;
			}
		}

		public void AddAlternative(Expression e)
		{
			this.Alternatives.Add(e);
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			LinkRef linkRef = cmp.NewLink();
			foreach (object obj in this.Alternatives)
			{
				Expression expression = (Expression)obj;
				LinkRef linkRef2 = cmp.NewLink();
				cmp.EmitBranch(linkRef2);
				expression.Compile(cmp, reverse);
				cmp.EmitJump(linkRef);
				cmp.ResolveLink(linkRef2);
				cmp.EmitBranchEnd();
			}
			cmp.EmitFalse();
			cmp.ResolveLink(linkRef);
			cmp.EmitAlternationEnd();
		}

		public override void GetWidth(out int min, out int max)
		{
			base.GetWidth(out min, out max, this.Alternatives.Count);
		}
	}
}
