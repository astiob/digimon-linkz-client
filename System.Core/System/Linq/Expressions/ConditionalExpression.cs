using System;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	/// <summary>Represents an expression that has a conditional operator.</summary>
	public sealed class ConditionalExpression : Expression
	{
		private Expression test;

		private Expression if_true;

		private Expression if_false;

		internal ConditionalExpression(Expression test, Expression if_true, Expression if_false) : base(ExpressionType.Conditional, if_true.Type)
		{
			this.test = test;
			this.if_true = if_true;
			this.if_false = if_false;
		}

		/// <summary>Gets the test of the conditional operation.</summary>
		/// <returns>An <see cref="T:System.Linq.Expressions.Expression" /> that represents the test of the conditional operation.</returns>
		public Expression Test
		{
			get
			{
				return this.test;
			}
		}

		/// <summary>Gets the expression to execute if the test evaluates to true.</summary>
		/// <returns>An <see cref="T:System.Linq.Expressions.Expression" /> that represents the expression to execute if the test is true.</returns>
		public Expression IfTrue
		{
			get
			{
				return this.if_true;
			}
		}

		/// <summary>Gets the expression to execute if the test evaluates to false.</summary>
		/// <returns>An <see cref="T:System.Linq.Expressions.Expression" /> that represents the expression to execute if the test is false.</returns>
		public Expression IfFalse
		{
			get
			{
				return this.if_false;
			}
		}

		internal override void Emit(EmitContext ec)
		{
			ILGenerator ig = ec.ig;
			Label label = ig.DefineLabel();
			Label label2 = ig.DefineLabel();
			this.test.Emit(ec);
			ig.Emit(OpCodes.Brfalse, label);
			this.if_true.Emit(ec);
			ig.Emit(OpCodes.Br, label2);
			ig.MarkLabel(label);
			this.if_false.Emit(ec);
			ig.MarkLabel(label2);
		}
	}
}
