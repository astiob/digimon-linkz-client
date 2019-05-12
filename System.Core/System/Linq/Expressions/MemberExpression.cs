using System;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	/// <summary>Represents accessing a field or property.</summary>
	public sealed class MemberExpression : Expression
	{
		private Expression expression;

		private MemberInfo member;

		internal MemberExpression(Expression expression, MemberInfo member, Type type) : base(ExpressionType.MemberAccess, type)
		{
			this.expression = expression;
			this.member = member;
		}

		/// <summary>Gets the containing object of the field or property.</summary>
		/// <returns>An <see cref="T:System.Linq.Expressions.Expression" /> that represents the containing object of the field or property.</returns>
		public Expression Expression
		{
			get
			{
				return this.expression;
			}
		}

		/// <summary>Gets the field or property to be accessed.</summary>
		/// <returns>The <see cref="T:System.Reflection.MemberInfo" /> that represents the field or property to be accessed.</returns>
		public MemberInfo Member
		{
			get
			{
				return this.member;
			}
		}

		internal override void Emit(EmitContext ec)
		{
			this.member.OnFieldOrProperty(delegate(FieldInfo field)
			{
				this.EmitFieldAccess(ec, field);
			}, delegate(PropertyInfo prop)
			{
				this.EmitPropertyAccess(ec, prop);
			});
		}

		private void EmitPropertyAccess(EmitContext ec, PropertyInfo property)
		{
			MethodInfo getMethod = property.GetGetMethod(true);
			if (!getMethod.IsStatic)
			{
				ec.EmitLoadSubject(this.expression);
			}
			ec.EmitCall(getMethod);
		}

		private void EmitFieldAccess(EmitContext ec, FieldInfo field)
		{
			if (!field.IsStatic)
			{
				ec.EmitLoadSubject(this.expression);
				ec.ig.Emit(OpCodes.Ldfld, field);
			}
			else
			{
				ec.ig.Emit(OpCodes.Ldsfld, field);
			}
		}
	}
}
