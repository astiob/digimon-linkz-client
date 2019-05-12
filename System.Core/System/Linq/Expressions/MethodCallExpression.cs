using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
	/// <summary>Represents calling a method.</summary>
	public sealed class MethodCallExpression : Expression
	{
		private Expression obj;

		private MethodInfo method;

		private ReadOnlyCollection<Expression> arguments;

		internal MethodCallExpression(MethodInfo method, ReadOnlyCollection<Expression> arguments) : base(ExpressionType.Call, method.ReturnType)
		{
			this.method = method;
			this.arguments = arguments;
		}

		internal MethodCallExpression(Expression obj, MethodInfo method, ReadOnlyCollection<Expression> arguments) : base(ExpressionType.Call, method.ReturnType)
		{
			this.obj = obj;
			this.method = method;
			this.arguments = arguments;
		}

		/// <summary>Gets the receiving object of the method.</summary>
		/// <returns>An <see cref="T:System.Linq.Expressions.Expression" /> that represents the receiving object of the method.</returns>
		public Expression Object
		{
			get
			{
				return this.obj;
			}
		}

		/// <summary>Gets the called method.</summary>
		/// <returns>The <see cref="T:System.Reflection.MethodInfo" /> that represents the called method.</returns>
		public MethodInfo Method
		{
			get
			{
				return this.method;
			}
		}

		/// <summary>Gets the arguments to the called method.</summary>
		/// <returns>A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of <see cref="T:System.Linq.Expressions.Expression" /> objects which represent the arguments to the called method.</returns>
		public ReadOnlyCollection<Expression> Arguments
		{
			get
			{
				return this.arguments;
			}
		}

		internal override void Emit(EmitContext ec)
		{
			ec.EmitCall(this.obj, this.arguments, this.method);
		}
	}
}
