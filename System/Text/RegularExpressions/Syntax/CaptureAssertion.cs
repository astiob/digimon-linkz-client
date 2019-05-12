using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal class CaptureAssertion : Assertion
	{
		private ExpressionAssertion alternate;

		private CapturingGroup group;

		private Literal literal;

		public CaptureAssertion(Literal l)
		{
			this.literal = l;
		}

		public CapturingGroup CapturingGroup
		{
			get
			{
				return this.group;
			}
			set
			{
				this.group = value;
			}
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			if (this.group == null)
			{
				this.Alternate.Compile(cmp, reverse);
				return;
			}
			int index = this.group.Index;
			LinkRef linkRef = cmp.NewLink();
			if (base.FalseExpression == null)
			{
				cmp.EmitIfDefined(index, linkRef);
				base.TrueExpression.Compile(cmp, reverse);
			}
			else
			{
				LinkRef linkRef2 = cmp.NewLink();
				cmp.EmitIfDefined(index, linkRef2);
				base.TrueExpression.Compile(cmp, reverse);
				cmp.EmitJump(linkRef);
				cmp.ResolveLink(linkRef2);
				base.FalseExpression.Compile(cmp, reverse);
			}
			cmp.ResolveLink(linkRef);
		}

		public override bool IsComplex()
		{
			if (this.group == null)
			{
				return this.Alternate.IsComplex();
			}
			return (base.TrueExpression != null && base.TrueExpression.IsComplex()) || (base.FalseExpression != null && base.FalseExpression.IsComplex()) || base.GetFixedWidth() <= 0;
		}

		private ExpressionAssertion Alternate
		{
			get
			{
				if (this.alternate == null)
				{
					this.alternate = new ExpressionAssertion();
					this.alternate.TrueExpression = base.TrueExpression;
					this.alternate.FalseExpression = base.FalseExpression;
					this.alternate.TestExpression = this.literal;
				}
				return this.alternate;
			}
		}
	}
}
