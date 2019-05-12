using System;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	/// <summary>Represents an expression that has a constant value.</summary>
	public sealed class ConstantExpression : Expression
	{
		private object value;

		internal ConstantExpression(object value, Type type) : base(ExpressionType.Constant, type)
		{
			this.value = value;
		}

		/// <summary>Gets the value of the constant expression.</summary>
		/// <returns>An <see cref="T:System.Object" /> equal to the value of the represented expression.</returns>
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		internal override void Emit(EmitContext ec)
		{
			if (base.Type.IsNullable())
			{
				this.EmitNullableConstant(ec, base.Type, this.value);
				return;
			}
			this.EmitConstant(ec, base.Type, this.value);
		}

		private void EmitNullableConstant(EmitContext ec, Type type, object value)
		{
			if (value == null)
			{
				ILGenerator ig = ec.ig;
				LocalBuilder local = ig.DeclareLocal(type);
				ig.Emit(OpCodes.Ldloca, local);
				ig.Emit(OpCodes.Initobj, type);
				ig.Emit(OpCodes.Ldloc, local);
			}
			else
			{
				this.EmitConstant(ec, type.GetFirstGenericArgument(), value);
				ec.EmitNullableNew(type);
			}
		}

		private void EmitConstant(EmitContext ec, Type type, object value)
		{
			ILGenerator ig = ec.ig;
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Object:
				this.EmitIfNotNull(ec, delegate(EmitContext c)
				{
					c.EmitReadGlobal(value);
				});
				return;
			case TypeCode.DBNull:
				ig.Emit(OpCodes.Ldsfld, typeof(DBNull).GetField("Value", BindingFlags.Static | BindingFlags.Public));
				return;
			case TypeCode.Boolean:
				if ((bool)this.Value)
				{
					ig.Emit(OpCodes.Ldc_I4_1);
				}
				else
				{
					ec.ig.Emit(OpCodes.Ldc_I4_0);
				}
				return;
			case TypeCode.Char:
				ig.Emit(OpCodes.Ldc_I4, (int)((char)value));
				return;
			case TypeCode.SByte:
				ig.Emit(OpCodes.Ldc_I4, (int)((sbyte)value));
				return;
			case TypeCode.Byte:
				ig.Emit(OpCodes.Ldc_I4, (int)((byte)value));
				return;
			case TypeCode.Int16:
				ig.Emit(OpCodes.Ldc_I4, (int)((short)value));
				return;
			case TypeCode.UInt16:
				ig.Emit(OpCodes.Ldc_I4, (int)((ushort)value));
				return;
			case TypeCode.Int32:
				ig.Emit(OpCodes.Ldc_I4, (int)value);
				return;
			case TypeCode.UInt32:
				ig.Emit(OpCodes.Ldc_I4, (int)((uint)this.Value));
				return;
			case TypeCode.Int64:
				ig.Emit(OpCodes.Ldc_I8, (long)value);
				return;
			case TypeCode.UInt64:
				ig.Emit(OpCodes.Ldc_I8, (long)((ulong)value));
				return;
			case TypeCode.Single:
				ig.Emit(OpCodes.Ldc_R4, (float)value);
				return;
			case TypeCode.Double:
				ig.Emit(OpCodes.Ldc_R8, (double)value);
				return;
			case TypeCode.Decimal:
			{
				decimal num = (decimal)value;
				int[] bits = decimal.GetBits(num);
				int num2 = bits[3] >> 16 & 255;
				Type typeFromHandle = typeof(int);
				if (num2 == 0 && num <= 2147483647m && num >= -2147483648m)
				{
					ig.Emit(OpCodes.Ldc_I4, (int)num);
					ig.Emit(OpCodes.Newobj, typeof(decimal).GetConstructor(new Type[]
					{
						typeFromHandle
					}));
					return;
				}
				ig.Emit(OpCodes.Ldc_I4, bits[0]);
				ig.Emit(OpCodes.Ldc_I4, bits[1]);
				ig.Emit(OpCodes.Ldc_I4, bits[2]);
				ig.Emit(OpCodes.Ldc_I4, bits[3] >> 31);
				ig.Emit(OpCodes.Ldc_I4, num2);
				ig.Emit(OpCodes.Newobj, typeof(decimal).GetConstructor(new Type[]
				{
					typeFromHandle,
					typeFromHandle,
					typeFromHandle,
					typeof(bool),
					typeof(byte)
				}));
				return;
			}
			case TypeCode.DateTime:
			{
				DateTime dateTime = (DateTime)value;
				LocalBuilder local = ig.DeclareLocal(typeof(DateTime));
				ig.Emit(OpCodes.Ldloca, local);
				ig.Emit(OpCodes.Ldc_I8, dateTime.Ticks);
				ig.Emit(OpCodes.Ldc_I4, (int)dateTime.Kind);
				ig.Emit(OpCodes.Call, typeof(DateTime).GetConstructor(new Type[]
				{
					typeof(long),
					typeof(DateTimeKind)
				}));
				ig.Emit(OpCodes.Ldloc, local);
				return;
			}
			case TypeCode.String:
				this.EmitIfNotNull(ec, delegate(EmitContext c)
				{
					c.ig.Emit(OpCodes.Ldstr, (string)value);
				});
				return;
			}
			throw new NotImplementedException(string.Format("No support for constants of type {0} yet", base.Type));
		}

		private void EmitIfNotNull(EmitContext ec, Action<EmitContext> emit)
		{
			if (this.value == null)
			{
				ec.ig.Emit(OpCodes.Ldnull);
				return;
			}
			emit(ec);
		}
	}
}
