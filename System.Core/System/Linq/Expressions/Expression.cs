using System;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
	/// <summary>Represents a strongly typed lambda expression as a data structure in the form of an expression tree. This class cannot be inherited.</summary>
	/// <typeparam name="TDelegate">The type of the delegate that the <see cref="T:System.Linq.Expressions.Expression`1" /> represents.</typeparam>
	public sealed class Expression<TDelegate> : LambdaExpression
	{
		internal Expression(Expression body, ReadOnlyCollection<ParameterExpression> parameters) : base(typeof(TDelegate), body, parameters)
		{
		}

		/// <summary>Compiles the lambda expression described by the expression tree into executable code.</summary>
		/// <returns>A delegate of type <paramref name="TDelegate" /> that represents the lambda expression described by the <see cref="T:System.Linq.Expressions.Expression`1" />.</returns>
		public new TDelegate Compile()
		{
			return (TDelegate)((object)base.Compile());
		}
	}
}
