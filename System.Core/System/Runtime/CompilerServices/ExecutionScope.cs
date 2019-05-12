using System;
using System.Linq.Expressions;

namespace System.Runtime.CompilerServices
{
	/// <summary>Represents the runtime state of a dynamically generated method.</summary>
	/// <filterpriority>2</filterpriority>
	public class ExecutionScope
	{
		/// <summary>Represents the non-trivial constants and locally executable expressions that are referenced by a dynamically generated method.</summary>
		public object[] Globals;

		/// <summary>Represents the hoisted local variables from the parent context.</summary>
		public object[] Locals;

		/// <summary>Represents the execution scope of the calling delegate.</summary>
		public ExecutionScope Parent;

		internal CompilationContext context;

		internal int compilation_unit;

		private ExecutionScope(CompilationContext context, int compilation_unit)
		{
			this.context = context;
			this.compilation_unit = compilation_unit;
			this.Globals = context.GetGlobals();
		}

		internal ExecutionScope(CompilationContext context) : this(context, 0)
		{
		}

		internal ExecutionScope(CompilationContext context, int compilation_unit, ExecutionScope parent, object[] locals) : this(context, compilation_unit)
		{
			this.Parent = parent;
			this.Locals = locals;
		}

		/// <summary>Creates a delegate that can be used to execute a dynamically generated method.</summary>
		/// <returns>A <see cref="T:System.Delegate" /> that can execute a dynamically generated method.</returns>
		/// <param name="indexLambda">The index of the object that stores information about associated lambda expression of the dynamic method.</param>
		/// <param name="locals">An array that contains the hoisted local variables from the parent context.</param>
		public Delegate CreateDelegate(int indexLambda, object[] locals)
		{
			return this.context.CreateDelegate(indexLambda, new ExecutionScope(this.context, indexLambda, this, locals));
		}

		/// <summary>Creates an array to store the hoisted local variables.</summary>
		/// <returns>An array to store hoisted local variables.</returns>
		public object[] CreateHoistedLocals()
		{
			return this.context.CreateHoistedLocals(this.compilation_unit);
		}

		/// <summary>Frees a specified expression tree of external parameter references by replacing the parameter with its current value.</summary>
		/// <returns>An expression tree that does not contain external parameter references.</returns>
		/// <param name="expression">An expression tree to free of external parameter references.</param>
		/// <param name="locals">An array that contains the hoisted local variables.</param>
		public Expression IsolateExpression(Expression expression, object[] locals)
		{
			return this.context.IsolateExpression(this, locals, expression);
		}
	}
}
