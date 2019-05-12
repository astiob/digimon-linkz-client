using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	/// <summary>Represents a constructor call.</summary>
	public sealed class NewExpression : Expression
	{
		private ConstructorInfo constructor;

		private ReadOnlyCollection<Expression> arguments;

		private ReadOnlyCollection<MemberInfo> members;

		internal NewExpression(Type type, ReadOnlyCollection<Expression> arguments) : base(ExpressionType.New, type)
		{
			this.arguments = arguments;
		}

		internal NewExpression(ConstructorInfo constructor, ReadOnlyCollection<Expression> arguments, ReadOnlyCollection<MemberInfo> members) : base(ExpressionType.New, constructor.DeclaringType)
		{
			this.constructor = constructor;
			this.arguments = arguments;
			this.members = members;
		}

		/// <summary>Gets the called constructor.</summary>
		/// <returns>The <see cref="T:System.Reflection.ConstructorInfo" /> that represents the called constructor.</returns>
		public ConstructorInfo Constructor
		{
			get
			{
				return this.constructor;
			}
		}

		/// <summary>Gets the arguments to the constructor.</summary>
		/// <returns>A collection of <see cref="T:System.Linq.Expressions.Expression" /> objects that represent the arguments to the constructor.</returns>
		public ReadOnlyCollection<Expression> Arguments
		{
			get
			{
				return this.arguments;
			}
		}

		/// <summary>Gets the members that can retrieve the values of the fields that were initialized with constructor arguments.</summary>
		/// <returns>A collection of <see cref="T:System.Reflection.MemberInfo" /> objects that represent the members that can retrieve the values of the fields that were initialized with constructor arguments.</returns>
		public ReadOnlyCollection<MemberInfo> Members
		{
			get
			{
				return this.members;
			}
		}

		internal override void Emit(EmitContext ec)
		{
			ILGenerator ig = ec.ig;
			Type type = base.Type;
			LocalBuilder local = null;
			if (type.IsValueType)
			{
				local = ig.DeclareLocal(type);
				ig.Emit(OpCodes.Ldloca, local);
				if (this.constructor == null)
				{
					ig.Emit(OpCodes.Initobj, type);
					ig.Emit(OpCodes.Ldloc, local);
					return;
				}
			}
			ec.EmitCollection<Expression>(this.arguments);
			if (type.IsValueType)
			{
				ig.Emit(OpCodes.Call, this.constructor);
				ig.Emit(OpCodes.Ldloc, local);
			}
			else
			{
				ig.Emit(OpCodes.Newobj, this.constructor ?? NewExpression.GetDefaultConstructor(type));
			}
		}

		private static ConstructorInfo GetDefaultConstructor(Type type)
		{
			return type.GetConstructor(Type.EmptyTypes);
		}
	}
}
