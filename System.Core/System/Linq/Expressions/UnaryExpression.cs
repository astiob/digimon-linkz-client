using System;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	/// <summary>Represents an expression that has a unary operator.</summary>
	public sealed class UnaryExpression : Expression
	{
		private Expression operand;

		private MethodInfo method;

		private bool is_lifted;

		internal UnaryExpression(ExpressionType node_type, Expression operand, Type type) : base(node_type, type)
		{
			this.operand = operand;
		}

		internal UnaryExpression(ExpressionType node_type, Expression operand, Type type, MethodInfo method, bool is_lifted) : base(node_type, type)
		{
			this.operand = operand;
			this.method = method;
			this.is_lifted = is_lifted;
		}

		/// <summary>Gets the operand of the unary operation.</summary>
		/// <returns>An <see cref="T:System.Linq.Expressions.Expression" /> that represents the operand of the unary operation.</returns>
		public Expression Operand
		{
			get
			{
				return this.operand;
			}
		}

		/// <summary>Gets the implementing method for the unary operation.</summary>
		/// <returns>The <see cref="T:System.Reflection.MethodInfo" /> that represents the implementing method.</returns>
		public MethodInfo Method
		{
			get
			{
				return this.method;
			}
		}

		/// <summary>Gets a value that indicates whether the expression tree node represents a lifted call to an operator.</summary>
		/// <returns>true if the node represents a lifted call; otherwise, false.</returns>
		public bool IsLifted
		{
			get
			{
				return this.is_lifted;
			}
		}

		/// <summary>Gets a value that indicates whether the expression tree node represents a lifted call to an operator whose return type is lifted to a nullable type.</summary>
		/// <returns>true if the operator's return type is lifted to a nullable type; otherwise, false.</returns>
		public bool IsLiftedToNull
		{
			get
			{
				return this.is_lifted && base.Type.IsNullable();
			}
		}

		private void EmitArrayLength(EmitContext ec)
		{
			this.operand.Emit(ec);
			ec.ig.Emit(OpCodes.Ldlen);
		}

		private void EmitTypeAs(EmitContext ec)
		{
			Type type = base.Type;
			ec.EmitIsInst(this.operand, type);
			if (type.IsNullable())
			{
				ec.ig.Emit(OpCodes.Unbox_Any, type);
			}
		}

		private void EmitLiftedUnary(EmitContext ec)
		{
			ILGenerator ig = ec.ig;
			LocalBuilder local = ec.EmitStored(this.operand);
			LocalBuilder local2 = ig.DeclareLocal(base.Type);
			Label label = ig.DefineLabel();
			Label label2 = ig.DefineLabel();
			ec.EmitNullableHasValue(local);
			ig.Emit(OpCodes.Brtrue, label);
			ec.EmitNullableInitialize(local2);
			ig.Emit(OpCodes.Br, label2);
			ig.MarkLabel(label);
			ec.EmitNullableGetValueOrDefault(local);
			this.EmitUnaryOperator(ec);
			ec.EmitNullableNew(base.Type);
			ig.MarkLabel(label2);
		}

		private void EmitUnaryOperator(EmitContext ec)
		{
			ILGenerator ig = ec.ig;
			ExpressionType nodeType = base.NodeType;
			switch (nodeType)
			{
			case ExpressionType.Negate:
				ig.Emit(OpCodes.Neg);
				break;
			default:
				if (nodeType != ExpressionType.Convert && nodeType != ExpressionType.ConvertChecked)
				{
					if (nodeType == ExpressionType.Not)
					{
						if (this.operand.Type.GetNotNullableType() == typeof(bool))
						{
							ig.Emit(OpCodes.Ldc_I4_0);
							ig.Emit(OpCodes.Ceq);
						}
						else
						{
							ig.Emit(OpCodes.Not);
						}
					}
				}
				else
				{
					this.EmitPrimitiveConversion(ec, this.operand.Type.GetNotNullableType(), base.Type.GetNotNullableType());
				}
				break;
			case ExpressionType.NegateChecked:
				ig.Emit(OpCodes.Ldc_I4_M1);
				ig.Emit((!Expression.IsUnsigned(this.operand.Type)) ? OpCodes.Mul_Ovf : OpCodes.Mul_Ovf_Un);
				break;
			}
		}

		private void EmitConvert(EmitContext ec)
		{
			Type type = this.operand.Type;
			Type type2 = base.Type;
			if (type == type2)
			{
				this.operand.Emit(ec);
			}
			else if (type.IsNullable() && !type2.IsNullable())
			{
				this.EmitConvertFromNullable(ec);
			}
			else if (!type.IsNullable() && type2.IsNullable())
			{
				this.EmitConvertToNullable(ec);
			}
			else if (type.IsNullable() && type2.IsNullable())
			{
				this.EmitConvertFromNullableToNullable(ec);
			}
			else if (Expression.IsReferenceConversion(type, type2))
			{
				this.EmitCast(ec);
			}
			else
			{
				if (!Expression.IsPrimitiveConversion(type, type2))
				{
					throw new NotImplementedException();
				}
				this.EmitPrimitiveConversion(ec);
			}
		}

		private void EmitConvertFromNullableToNullable(EmitContext ec)
		{
			this.EmitLiftedUnary(ec);
		}

		private void EmitConvertToNullable(EmitContext ec)
		{
			ec.Emit(this.operand);
			if (this.IsUnBoxing())
			{
				this.EmitUnbox(ec);
				return;
			}
			if (this.operand.Type != base.Type.GetNotNullableType())
			{
				this.EmitPrimitiveConversion(ec, this.operand.Type, base.Type.GetNotNullableType());
			}
			ec.EmitNullableNew(base.Type);
		}

		private void EmitConvertFromNullable(EmitContext ec)
		{
			if (this.IsBoxing())
			{
				ec.Emit(this.operand);
				this.EmitBox(ec);
				return;
			}
			ec.EmitCall(this.operand, this.operand.Type.GetMethod("get_Value"));
			if (this.operand.Type.GetNotNullableType() != base.Type)
			{
				this.EmitPrimitiveConversion(ec, this.operand.Type.GetNotNullableType(), base.Type);
			}
		}

		private bool IsBoxing()
		{
			return this.operand.Type.IsValueType && !base.Type.IsValueType;
		}

		private void EmitBox(EmitContext ec)
		{
			ec.ig.Emit(OpCodes.Box, this.operand.Type);
		}

		private bool IsUnBoxing()
		{
			return !this.operand.Type.IsValueType && base.Type.IsValueType;
		}

		private void EmitUnbox(EmitContext ec)
		{
			ec.ig.Emit(OpCodes.Unbox_Any, base.Type);
		}

		private void EmitCast(EmitContext ec)
		{
			this.operand.Emit(ec);
			if (this.IsBoxing())
			{
				this.EmitBox(ec);
			}
			else if (this.IsUnBoxing())
			{
				this.EmitUnbox(ec);
			}
			else
			{
				ec.ig.Emit(OpCodes.Castclass, base.Type);
			}
		}

		private void EmitPrimitiveConversion(EmitContext ec, bool is_unsigned, OpCode signed, OpCode unsigned, OpCode signed_checked, OpCode unsigned_checked)
		{
			if (base.NodeType != ExpressionType.ConvertChecked)
			{
				ec.ig.Emit((!is_unsigned) ? signed : unsigned);
			}
			else
			{
				ec.ig.Emit((!is_unsigned) ? signed_checked : unsigned_checked);
			}
		}

		private void EmitPrimitiveConversion(EmitContext ec)
		{
			this.operand.Emit(ec);
			this.EmitPrimitiveConversion(ec, this.operand.Type, base.Type);
		}

		private void EmitPrimitiveConversion(EmitContext ec, Type from, Type to)
		{
			bool flag = Expression.IsUnsigned(from);
			switch (Type.GetTypeCode(to))
			{
			case TypeCode.SByte:
				this.EmitPrimitiveConversion(ec, flag, OpCodes.Conv_I1, OpCodes.Conv_U1, OpCodes.Conv_Ovf_I1, OpCodes.Conv_Ovf_I1_Un);
				return;
			case TypeCode.Byte:
				this.EmitPrimitiveConversion(ec, flag, OpCodes.Conv_I1, OpCodes.Conv_U1, OpCodes.Conv_Ovf_U1, OpCodes.Conv_Ovf_U1_Un);
				return;
			case TypeCode.Int16:
				this.EmitPrimitiveConversion(ec, flag, OpCodes.Conv_I2, OpCodes.Conv_U2, OpCodes.Conv_Ovf_I2, OpCodes.Conv_Ovf_I2_Un);
				return;
			case TypeCode.UInt16:
				this.EmitPrimitiveConversion(ec, flag, OpCodes.Conv_I2, OpCodes.Conv_U2, OpCodes.Conv_Ovf_U2, OpCodes.Conv_Ovf_U2_Un);
				return;
			case TypeCode.Int32:
				this.EmitPrimitiveConversion(ec, flag, OpCodes.Conv_I4, OpCodes.Conv_U4, OpCodes.Conv_Ovf_I4, OpCodes.Conv_Ovf_I4_Un);
				return;
			case TypeCode.UInt32:
				this.EmitPrimitiveConversion(ec, flag, OpCodes.Conv_I4, OpCodes.Conv_U4, OpCodes.Conv_Ovf_U4, OpCodes.Conv_Ovf_U4_Un);
				return;
			case TypeCode.Int64:
				this.EmitPrimitiveConversion(ec, flag, OpCodes.Conv_I8, OpCodes.Conv_U8, OpCodes.Conv_Ovf_I8, OpCodes.Conv_Ovf_I8_Un);
				return;
			case TypeCode.UInt64:
				this.EmitPrimitiveConversion(ec, flag, OpCodes.Conv_I8, OpCodes.Conv_U8, OpCodes.Conv_Ovf_U8, OpCodes.Conv_Ovf_U8_Un);
				return;
			case TypeCode.Single:
				if (flag)
				{
					ec.ig.Emit(OpCodes.Conv_R_Un);
				}
				ec.ig.Emit(OpCodes.Conv_R4);
				return;
			case TypeCode.Double:
				if (flag)
				{
					ec.ig.Emit(OpCodes.Conv_R_Un);
				}
				ec.ig.Emit(OpCodes.Conv_R8);
				return;
			default:
				throw new NotImplementedException(base.Type.ToString());
			}
		}

		private void EmitArithmeticUnary(EmitContext ec)
		{
			if (!this.IsLifted)
			{
				this.operand.Emit(ec);
				this.EmitUnaryOperator(ec);
			}
			else
			{
				this.EmitLiftedUnary(ec);
			}
		}

		private void EmitUserDefinedLiftedToNullOperator(EmitContext ec)
		{
			ILGenerator ig = ec.ig;
			LocalBuilder local = ec.EmitStored(this.operand);
			Label label = ig.DefineLabel();
			Label label2 = ig.DefineLabel();
			ec.EmitNullableHasValue(local);
			ig.Emit(OpCodes.Brfalse, label);
			ec.EmitNullableGetValueOrDefault(local);
			ec.EmitCall(this.method);
			ec.EmitNullableNew(base.Type);
			ig.Emit(OpCodes.Br, label2);
			ig.MarkLabel(label);
			LocalBuilder local2 = ig.DeclareLocal(base.Type);
			ec.EmitNullableInitialize(local2);
			ig.MarkLabel(label2);
		}

		private void EmitUserDefinedLiftedOperator(EmitContext ec)
		{
			LocalBuilder local = ec.EmitStored(this.operand);
			ec.EmitNullableGetValue(local);
			ec.EmitCall(this.method);
		}

		private void EmitUserDefinedOperator(EmitContext ec)
		{
			if (!this.IsLifted)
			{
				ec.Emit(this.operand);
				ec.EmitCall(this.method);
			}
			else if (this.IsLiftedToNull)
			{
				this.EmitUserDefinedLiftedToNullOperator(ec);
			}
			else
			{
				this.EmitUserDefinedLiftedOperator(ec);
			}
		}

		private void EmitQuote(EmitContext ec)
		{
			ec.EmitScope();
			ec.EmitReadGlobal(this.operand, typeof(Expression));
			if (ec.HasHoistedLocals)
			{
				ec.EmitLoadHoistedLocalsStore();
			}
			else
			{
				ec.ig.Emit(OpCodes.Ldnull);
			}
			ec.EmitIsolateExpression();
		}

		internal override void Emit(EmitContext ec)
		{
			if (this.method != null)
			{
				this.EmitUserDefinedOperator(ec);
				return;
			}
			ExpressionType nodeType = base.NodeType;
			switch (nodeType)
			{
			case ExpressionType.Negate:
			case ExpressionType.UnaryPlus:
			case ExpressionType.NegateChecked:
			case ExpressionType.Not:
				this.EmitArithmeticUnary(ec);
				return;
			default:
				if (nodeType == ExpressionType.Convert || nodeType == ExpressionType.ConvertChecked)
				{
					this.EmitConvert(ec);
					return;
				}
				if (nodeType == ExpressionType.ArrayLength)
				{
					this.EmitArrayLength(ec);
					return;
				}
				if (nodeType == ExpressionType.Quote)
				{
					this.EmitQuote(ec);
					return;
				}
				if (nodeType != ExpressionType.TypeAs)
				{
					throw new NotImplementedException(base.NodeType.ToString());
				}
				this.EmitTypeAs(ec);
				return;
			}
		}
	}
}
