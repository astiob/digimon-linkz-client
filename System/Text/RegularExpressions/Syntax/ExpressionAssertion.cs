using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal class ExpressionAssertion : Assertion
	{
		private bool reverse;

		private bool negate;

		public ExpressionAssertion()
		{
			base.Expressions.Add(null);
		}

		public bool Reverse
		{
			get
			{
				return this.reverse;
			}
			set
			{
				this.reverse = value;
			}
		}

		public bool Negate
		{
			get
			{
				return this.negate;
			}
			set
			{
				this.negate = value;
			}
		}

		public Expression TestExpression
		{
			get
			{
				return base.Expressions[2];
			}
			set
			{
				base.Expressions[2] = value;
			}
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			LinkRef linkRef = cmp.NewLink();
			LinkRef linkRef2 = cmp.NewLink();
			if (!this.negate)
			{
				cmp.EmitTest(linkRef, linkRef2);
			}
			else
			{
				cmp.EmitTest(linkRef2, linkRef);
			}
			this.TestExpression.Compile(cmp, this.reverse);
			cmp.EmitTrue();
			if (base.TrueExpression == null)
			{
				cmp.ResolveLink(linkRef2);
				cmp.EmitFalse();
				cmp.ResolveLink(linkRef);
			}
			else
			{
				cmp.ResolveLink(linkRef);
				base.TrueExpression.Compile(cmp, reverse);
				if (base.FalseExpression == null)
				{
					cmp.ResolveLink(linkRef2);
				}
				else
				{
					LinkRef linkRef3 = cmp.NewLink();
					cmp.EmitJump(linkRef3);
					cmp.ResolveLink(linkRef2);
					base.FalseExpression.Compile(cmp, reverse);
					cmp.ResolveLink(linkRef3);
				}
			}
		}

		public override bool IsComplex()
		{
			return true;
		}
	}
}
