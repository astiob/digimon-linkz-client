using System;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	/// <summary>Provides the base class from which the classes that represent bindings that are used to initialize members of a newly created object derive.</summary>
	public abstract class MemberBinding
	{
		private MemberBindingType binding_type;

		private MemberInfo member;

		/// <summary>Initializes a new instance of the <see cref="T:System.Linq.Expressions.MemberBinding" /> class.</summary>
		/// <param name="type">The <see cref="T:System.Linq.Expressions.MemberBindingType" /> that discriminates the type of binding that is represented.</param>
		/// <param name="member">The <see cref="T:System.Reflection.MemberInfo" /> that represents a field or property to be initialized.</param>
		protected MemberBinding(MemberBindingType binding_type, MemberInfo member)
		{
			this.binding_type = binding_type;
			this.member = member;
		}

		/// <summary>Gets the type of binding that is represented.</summary>
		/// <returns>One of the <see cref="T:System.Linq.Expressions.MemberBindingType" /> values.</returns>
		public MemberBindingType BindingType
		{
			get
			{
				return this.binding_type;
			}
		}

		/// <summary>Gets the field or property to be initialized.</summary>
		/// <returns>The <see cref="T:System.Reflection.MemberInfo" /> that represents the field or property to be initialized.</returns>
		public MemberInfo Member
		{
			get
			{
				return this.member;
			}
		}

		/// <summary>Returns a textual representation of the <see cref="T:System.Linq.Expressions.MemberBinding" />.</summary>
		/// <returns>A textual representation of the <see cref="T:System.Linq.Expressions.MemberBinding" />.</returns>
		public override string ToString()
		{
			return ExpressionPrinter.ToString(this);
		}

		internal abstract void Emit(EmitContext ec, LocalBuilder local);

		internal LocalBuilder EmitLoadMember(EmitContext ec, LocalBuilder local)
		{
			ec.EmitLoadSubject(local);
			return this.member.OnFieldOrProperty((FieldInfo field) => this.EmitLoadField(ec, field), (PropertyInfo prop) => this.EmitLoadProperty(ec, prop));
		}

		private LocalBuilder EmitLoadProperty(EmitContext ec, PropertyInfo property)
		{
			MethodInfo getMethod = property.GetGetMethod(true);
			if (getMethod == null)
			{
				throw new NotSupportedException();
			}
			LocalBuilder localBuilder = ec.ig.DeclareLocal(property.PropertyType);
			ec.EmitCall(getMethod);
			ec.ig.Emit(OpCodes.Stloc, localBuilder);
			return localBuilder;
		}

		private LocalBuilder EmitLoadField(EmitContext ec, FieldInfo field)
		{
			LocalBuilder localBuilder = ec.ig.DeclareLocal(field.FieldType);
			ec.ig.Emit(OpCodes.Ldfld, field);
			ec.ig.Emit(OpCodes.Stloc, localBuilder);
			return localBuilder;
		}
	}
}
