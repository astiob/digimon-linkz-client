using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	/// <summary>Represents an initializer for a single element of an <see cref="T:System.Collections.IEnumerable" /> collection.</summary>
	public sealed class ElementInit
	{
		private MethodInfo add_method;

		private ReadOnlyCollection<Expression> arguments;

		internal ElementInit(MethodInfo add_method, ReadOnlyCollection<Expression> arguments)
		{
			this.add_method = add_method;
			this.arguments = arguments;
		}

		/// <summary>Gets the instance method that is used to add an element to an <see cref="T:System.Collections.IEnumerable" /> collection.</summary>
		/// <returns>A <see cref="T:System.Reflection.MethodInfo" /> that represents an instance method that adds an element to a collection.</returns>
		public MethodInfo AddMethod
		{
			get
			{
				return this.add_method;
			}
		}

		/// <summary>Gets the collection of arguments that are passed to a method that adds an element to an <see cref="T:System.Collections.IEnumerable" /> collection.</summary>
		/// <returns>A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of <see cref="T:System.Linq.Expressions.Expression" /> objects that represent the arguments for a method that adds an element to a collection.</returns>
		public ReadOnlyCollection<Expression> Arguments
		{
			get
			{
				return this.arguments;
			}
		}

		/// <summary>Returns a textual representation of an <see cref="T:System.Linq.Expressions.ElementInit" /> object.</summary>
		/// <returns>A textual representation of the <see cref="T:System.Linq.Expressions.ElementInit" /> object.</returns>
		public override string ToString()
		{
			return ExpressionPrinter.ToString(this);
		}

		private void EmitPopIfNeeded(EmitContext ec)
		{
			if (this.add_method.ReturnType == typeof(void))
			{
				return;
			}
			ec.ig.Emit(OpCodes.Pop);
		}

		internal void Emit(EmitContext ec, LocalBuilder local)
		{
			ec.EmitCall(local, this.arguments, this.add_method);
			this.EmitPopIfNeeded(ec);
		}
	}
}
