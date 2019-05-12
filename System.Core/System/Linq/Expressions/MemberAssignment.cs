using System;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	/// <summary>Represents initializing a field or property of a newly created object.</summary>
	public sealed class MemberAssignment : MemberBinding
	{
		private Expression expression;

		internal MemberAssignment(MemberInfo member, Expression expression) : base(MemberBindingType.Assignment, member)
		{
			this.expression = expression;
		}

		/// <summary>Gets the expression to assign to the field or property.</summary>
		/// <returns>The <see cref="T:System.Linq.Expressions.Expression" /> that represents the value to assign to the field or property.</returns>
		public Expression Expression
		{
			get
			{
				return this.expression;
			}
		}

		internal override void Emit(EmitContext ec, LocalBuilder local)
		{
			base.Member.OnFieldOrProperty(delegate(FieldInfo field)
			{
				this.EmitFieldAssignment(ec, field, local);
			}, delegate(PropertyInfo prop)
			{
				this.EmitPropertyAssignment(ec, prop, local);
			});
		}

		private void EmitFieldAssignment(EmitContext ec, FieldInfo field, LocalBuilder local)
		{
			ec.EmitLoadSubject(local);
			this.expression.Emit(ec);
			ec.ig.Emit(OpCodes.Stfld, field);
		}

		private void EmitPropertyAssignment(EmitContext ec, PropertyInfo property, LocalBuilder local)
		{
			MethodInfo setMethod = property.GetSetMethod(true);
			if (setMethod == null)
			{
				throw new InvalidOperationException();
			}
			ec.EmitLoadSubject(local);
			this.expression.Emit(ec);
			ec.EmitCall(setMethod);
		}
	}
}
