using System;
using System.Collections.ObjectModel;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	/// <summary>Represents calling a constructor and initializing one or more members of the new object.</summary>
	public sealed class MemberInitExpression : Expression
	{
		private NewExpression new_expression;

		private ReadOnlyCollection<MemberBinding> bindings;

		internal MemberInitExpression(NewExpression new_expression, ReadOnlyCollection<MemberBinding> bindings) : base(ExpressionType.MemberInit, new_expression.Type)
		{
			this.new_expression = new_expression;
			this.bindings = bindings;
		}

		/// <summary>Gets the expression that represents the constructor call.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.NewExpression" /> that represents the constructor call.</returns>
		public NewExpression NewExpression
		{
			get
			{
				return this.new_expression;
			}
		}

		/// <summary>Gets the bindings that describe how to initialize the members of the newly created object.</summary>
		/// <returns>A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of <see cref="T:System.Linq.Expressions.MemberBinding" /> objects which describe how to initialize the members.</returns>
		public ReadOnlyCollection<MemberBinding> Bindings
		{
			get
			{
				return this.bindings;
			}
		}

		internal override void Emit(EmitContext ec)
		{
			LocalBuilder local = ec.EmitStored(this.new_expression);
			ec.EmitCollection(this.bindings, local);
			ec.EmitLoad(local);
		}
	}
}
