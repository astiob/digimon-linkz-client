using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	/// <summary>Describes a Microsoft intermediate language (MSIL) instruction.</summary>
	[ComVisible(true)]
	public struct OpCode
	{
		internal byte op1;

		internal byte op2;

		private byte push;

		private byte pop;

		private byte size;

		private byte type;

		private byte args;

		private byte flow;

		internal OpCode(int p, int q)
		{
			this.op1 = (byte)(p & 255);
			this.op2 = (byte)(p >> 8 & 255);
			this.push = (byte)(p >> 16 & 255);
			this.pop = (byte)(p >> 24 & 255);
			this.size = (byte)(q & 255);
			this.type = (byte)(q >> 8 & 255);
			this.args = (byte)(q >> 16 & 255);
			this.flow = (byte)(q >> 24 & 255);
		}

		/// <summary>Returns the generated hash code for this Opcode.</summary>
		/// <returns>Returns the hash code for this instance.</returns>
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		/// <summary>Tests whether the given object is equal to this Opcode.</summary>
		/// <returns>true if <paramref name="obj" /> is an instance of Opcode and is equal to this object; otherwise, false.</returns>
		/// <param name="obj">The object to compare to this object. </param>
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is OpCode))
			{
				return false;
			}
			OpCode opCode = (OpCode)obj;
			return opCode.op1 == this.op1 && opCode.op2 == this.op2;
		}

		/// <summary>Indicates whether the current instance is equal to the specified <see cref="T:System.Reflection.Emit.OpCode" />.</summary>
		/// <returns>true if the value of <paramref name="obj" /> is equal to the value of the current instance; otherwise, false.</returns>
		/// <param name="obj">The <see cref="T:System.Reflection.Emit.OpCode" /> to compare to the current instance.</param>
		public bool Equals(OpCode obj)
		{
			return obj.op1 == this.op1 && obj.op2 == this.op2;
		}

		/// <summary>Returns this Opcode as a <see cref="T:System.String" />.</summary>
		/// <returns>Returns a <see cref="T:System.String" /> containing the name of this Opcode.</returns>
		public override string ToString()
		{
			return this.Name;
		}

		/// <summary>The name of the Microsoft intermediate language (MSIL) instruction.</summary>
		/// <returns>Read-only. The name of the MSIL instruction.</returns>
		public string Name
		{
			get
			{
				if (this.op1 == 255)
				{
					return OpCodeNames.names[(int)this.op2];
				}
				return OpCodeNames.names[256 + (int)this.op2];
			}
		}

		/// <summary>The size of the Microsoft intermediate language (MSIL) instruction.</summary>
		/// <returns>Read-only. The size of the MSIL instruction.</returns>
		public int Size
		{
			get
			{
				return (int)this.size;
			}
		}

		/// <summary>The type of Microsoft intermediate language (MSIL) instruction.</summary>
		/// <returns>Read-only. The type of Microsoft intermediate language (MSIL) instruction.</returns>
		public OpCodeType OpCodeType
		{
			get
			{
				return (OpCodeType)this.type;
			}
		}

		/// <summary>The operand type of an Microsoft intermediate language (MSIL) instruction.</summary>
		/// <returns>Read-only. The operand type of an MSIL instruction.</returns>
		public OperandType OperandType
		{
			get
			{
				return (OperandType)this.args;
			}
		}

		/// <summary>The flow control characteristics of the Microsoft intermediate language (MSIL) instruction.</summary>
		/// <returns>Read-only. The type of flow control.</returns>
		public FlowControl FlowControl
		{
			get
			{
				return (FlowControl)this.flow;
			}
		}

		/// <summary>How the Microsoft intermediate language (MSIL) instruction pops the stack.</summary>
		/// <returns>Read-only. The way the MSIL instruction pops the stack.</returns>
		public StackBehaviour StackBehaviourPop
		{
			get
			{
				return (StackBehaviour)this.pop;
			}
		}

		/// <summary>How the Microsoft intermediate language (MSIL) instruction pushes operand onto the stack.</summary>
		/// <returns>Read-only. The way the MSIL instruction pushes operand onto the stack.</returns>
		public StackBehaviour StackBehaviourPush
		{
			get
			{
				return (StackBehaviour)this.push;
			}
		}

		/// <summary>The value of the immediate operand of the Microsoft intermediate language (MSIL) instruction.</summary>
		/// <returns>Read-only. The value of the immediate operand of the MSIL instruction.</returns>
		public short Value
		{
			get
			{
				if (this.size == 1)
				{
					return (short)this.op2;
				}
				return (short)((int)this.op1 << 8 | (int)this.op2);
			}
		}

		/// <summary>Indicates whether two <see cref="T:System.Reflection.Emit.OpCode" /> structures are equal.</summary>
		/// <returns>true if <paramref name="a" /> is equal to <paramref name="b" />; otherwise, false.</returns>
		/// <param name="a">The <see cref="T:System.Reflection.Emit.OpCode" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">The <see cref="T:System.Reflection.Emit.OpCode" /> to compare to <paramref name="a" />.</param>
		public static bool operator ==(OpCode a, OpCode b)
		{
			return a.op1 == b.op1 && a.op2 == b.op2;
		}

		/// <summary>Indicates whether two <see cref="T:System.Reflection.Emit.OpCode" /> structures are not equal.</summary>
		/// <returns>true if <paramref name="a" /> is not equal to <paramref name="b" />; otherwise, false.</returns>
		/// <param name="a">The <see cref="T:System.Reflection.Emit.OpCode" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">The <see cref="T:System.Reflection.Emit.OpCode" /> to compare to <paramref name="a" />.</param>
		public static bool operator !=(OpCode a, OpCode b)
		{
			return a.op1 != b.op1 || a.op2 != b.op2;
		}
	}
}
