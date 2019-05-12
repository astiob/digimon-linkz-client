using System;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
	/// <summary>Represents an expression that applies a delegate or lambda expression to a list of argument expressions.</summary>
	public sealed class InvocationExpression : Expression
	{
		private Expression expression;

		private ReadOnlyCollection<Expression> arguments;

		internal InvocationExpression(Expression expression, Type type, ReadOnlyCollection<Expression> arguments) : base(ExpressionType.Invoke, type)
		{
			this.expression = expression;
			this.arguments = arguments;
		}

		/// <summary>Gets the delegate or lambda expression to be applied.</summary>
		/// <returns>An <see cref="T:System.Linq.Expressions.Expression" /> that represents the delegate to be applied.</returns>
		public Expression Expression
		{
			get
			{
				return this.expression;
			}
		}

		/// <summary>Gets the arguments that the delegate is applied to.</summary>
		/// <returns>A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of <see cref="T:System.Linq.Expressions.Expression" /> objects which represent the arguments that the delegate is applied to.</returns>
		public ReadOnlyCollection<Expression> Arguments
		{
			get
			{
				return this.arguments;
			}
		}

		internal override void Emit(EmitContext ec)
		{
			ec.EmitCall(this.expression, this.arguments, this.expression.Type.GetInvokeMethod());
		}
	}
}
