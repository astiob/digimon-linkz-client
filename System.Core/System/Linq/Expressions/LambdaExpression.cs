using System;
using System.Collections.ObjectModel;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	/// <summary>Describes a lambda expression.</summary>
	public class LambdaExpression : Expression
	{
		private Expression body;

		private ReadOnlyCollection<ParameterExpression> parameters;

		internal LambdaExpression(Type delegateType, Expression body, ReadOnlyCollection<ParameterExpression> parameters) : base(ExpressionType.Lambda, delegateType)
		{
			this.body = body;
			this.parameters = parameters;
		}

		/// <summary>Gets the body of the lambda expression.</summary>
		/// <returns>An <see cref="T:System.Linq.Expressions.Expression" /> that represents the body of the lambda expression.</returns>
		public Expression Body
		{
			get
			{
				return this.body;
			}
		}

		/// <summary>Gets the parameters of the lambda expression.</summary>
		/// <returns>A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects that represent the parameters of the lambda expression.</returns>
		public ReadOnlyCollection<ParameterExpression> Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		private void EmitPopIfNeeded(EmitContext ec)
		{
			if (this.GetReturnType() == typeof(void) && this.body.Type != typeof(void))
			{
				ec.ig.Emit(OpCodes.Pop);
			}
		}

		internal override void Emit(EmitContext ec)
		{
			ec.EmitCreateDelegate(this);
		}

		internal void EmitBody(EmitContext ec)
		{
			this.body.Emit(ec);
			this.EmitPopIfNeeded(ec);
			ec.ig.Emit(OpCodes.Ret);
		}

		internal Type GetReturnType()
		{
			return base.Type.GetInvokeMethod().ReturnType;
		}

		/// <summary>Produces a delegate that represents the lambda expression.</summary>
		/// <returns>A <see cref="T:System.Delegate" /> that, when it is executed, has the behavior described by the semantics of the <see cref="T:System.Linq.Expressions.LambdaExpression" />.</returns>
		public Delegate Compile()
		{
			CompilationContext compilationContext = new CompilationContext();
			compilationContext.AddCompilationUnit(this);
			return compilationContext.CreateDelegate();
		}
	}
}
