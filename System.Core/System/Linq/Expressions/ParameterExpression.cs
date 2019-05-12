using System;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	/// <summary>Represents a named parameter expression.</summary>
	public sealed class ParameterExpression : Expression
	{
		private string name;

		internal ParameterExpression(Type type, string name) : base(ExpressionType.Parameter, type)
		{
			this.name = name;
		}

		/// <summary>Gets the name of the parameter.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the name of the parameter.</returns>
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		private void EmitLocalParameter(EmitContext ec, int position)
		{
			ec.ig.Emit(OpCodes.Ldarg, position);
		}

		private void EmitHoistedLocal(EmitContext ec, int level, int position)
		{
			ec.EmitScope();
			for (int i = 0; i < level; i++)
			{
				ec.EmitParentScope();
			}
			ec.EmitLoadLocals();
			ec.ig.Emit(OpCodes.Ldc_I4, position);
			ec.ig.Emit(OpCodes.Ldelem, typeof(object));
			ec.EmitLoadStrongBoxValue(base.Type);
		}

		internal override void Emit(EmitContext ec)
		{
			int position = -1;
			if (ec.IsLocalParameter(this, ref position))
			{
				this.EmitLocalParameter(ec, position);
				return;
			}
			int level = 0;
			if (ec.IsHoistedLocal(this, ref level, ref position))
			{
				this.EmitHoistedLocal(ec, level, position);
				return;
			}
			throw new InvalidOperationException("Parameter out of scope");
		}
	}
}
