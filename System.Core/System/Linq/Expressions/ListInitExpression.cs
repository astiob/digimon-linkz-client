using System;
using System.Collections.ObjectModel;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	/// <summary>Represents a constructor call that has a collection initializer.</summary>
	public sealed class ListInitExpression : Expression
	{
		private NewExpression new_expression;

		private ReadOnlyCollection<ElementInit> initializers;

		internal ListInitExpression(NewExpression new_expression, ReadOnlyCollection<ElementInit> initializers) : base(ExpressionType.ListInit, new_expression.Type)
		{
			this.new_expression = new_expression;
			this.initializers = initializers;
		}

		/// <summary>Gets the expression that contains a call to the constructor of a collection type.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.NewExpression" /> that represents the call to the constructor of a collection type.</returns>
		public NewExpression NewExpression
		{
			get
			{
				return this.new_expression;
			}
		}

		/// <summary>Gets the element initializers that are used to initialize a collection.</summary>
		/// <returns>A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of <see cref="T:System.Linq.Expressions.ElementInit" /> objects which represent the elements that are used to initialize the collection.</returns>
		public ReadOnlyCollection<ElementInit> Initializers
		{
			get
			{
				return this.initializers;
			}
		}

		internal override void Emit(EmitContext ec)
		{
			LocalBuilder local = ec.EmitStored(this.new_expression);
			ec.EmitCollection(this.initializers, local);
			ec.EmitLoad(local);
		}
	}
}
