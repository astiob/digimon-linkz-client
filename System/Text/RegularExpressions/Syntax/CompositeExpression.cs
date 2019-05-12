using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal abstract class CompositeExpression : Expression
	{
		private ExpressionCollection expressions;

		public CompositeExpression()
		{
			this.expressions = new ExpressionCollection();
		}

		protected ExpressionCollection Expressions
		{
			get
			{
				return this.expressions;
			}
		}

		protected void GetWidth(out int min, out int max, int count)
		{
			min = int.MaxValue;
			max = 0;
			bool flag = true;
			for (int i = 0; i < count; i++)
			{
				Expression expression = this.Expressions[i];
				if (expression != null)
				{
					flag = false;
					int num;
					int num2;
					expression.GetWidth(out num, out num2);
					if (num < min)
					{
						min = num;
					}
					if (num2 > max)
					{
						max = num2;
					}
				}
			}
			if (flag)
			{
				min = (max = 0);
			}
		}

		public override bool IsComplex()
		{
			foreach (object obj in this.Expressions)
			{
				Expression expression = (Expression)obj;
				if (expression.IsComplex())
				{
					return true;
				}
			}
			return base.GetFixedWidth() <= 0;
		}
	}
}
