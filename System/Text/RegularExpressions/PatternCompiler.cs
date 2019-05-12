using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	internal class PatternCompiler : ICompiler
	{
		private ArrayList pgm;

		public PatternCompiler()
		{
			this.pgm = new ArrayList();
		}

		public static ushort EncodeOp(OpCode op, OpFlags flags)
		{
			return (ushort)(op | (OpCode)(flags & (OpFlags)65280));
		}

		public static void DecodeOp(ushort word, out OpCode op, out OpFlags flags)
		{
			op = (OpCode)(word & 255);
			flags = (OpFlags)(word & 65280);
		}

		public void Reset()
		{
			this.pgm.Clear();
		}

		public IMachineFactory GetMachineFactory()
		{
			ushort[] array = new ushort[this.pgm.Count];
			this.pgm.CopyTo(array);
			return new InterpreterFactory(array);
		}

		public void EmitFalse()
		{
			this.Emit(OpCode.False);
		}

		public void EmitTrue()
		{
			this.Emit(OpCode.True);
		}

		private void EmitCount(int count)
		{
			this.Emit((ushort)(count & 65535));
			this.Emit((ushort)((uint)count >> 16));
		}

		public void EmitCharacter(char c, bool negate, bool ignore, bool reverse)
		{
			this.Emit(OpCode.Character, PatternCompiler.MakeFlags(negate, ignore, reverse, false));
			if (ignore)
			{
				c = char.ToLower(c);
			}
			this.Emit((ushort)c);
		}

		public void EmitCategory(Category cat, bool negate, bool reverse)
		{
			this.Emit(OpCode.Category, PatternCompiler.MakeFlags(negate, false, reverse, false));
			this.Emit((ushort)cat);
		}

		public void EmitNotCategory(Category cat, bool negate, bool reverse)
		{
			this.Emit(OpCode.NotCategory, PatternCompiler.MakeFlags(negate, false, reverse, false));
			this.Emit((ushort)cat);
		}

		public void EmitRange(char lo, char hi, bool negate, bool ignore, bool reverse)
		{
			this.Emit(OpCode.Range, PatternCompiler.MakeFlags(negate, ignore, reverse, false));
			this.Emit((ushort)lo);
			this.Emit((ushort)hi);
		}

		public void EmitSet(char lo, BitArray set, bool negate, bool ignore, bool reverse)
		{
			this.Emit(OpCode.Set, PatternCompiler.MakeFlags(negate, ignore, reverse, false));
			this.Emit((ushort)lo);
			int num = set.Length + 15 >> 4;
			this.Emit((ushort)num);
			int num2 = 0;
			while (num-- != 0)
			{
				ushort num3 = 0;
				for (int i = 0; i < 16; i++)
				{
					if (num2 >= set.Length)
					{
						break;
					}
					if (set[num2++])
					{
						num3 |= (ushort)(1 << i);
					}
				}
				this.Emit(num3);
			}
		}

		public void EmitString(string str, bool ignore, bool reverse)
		{
			this.Emit(OpCode.String, PatternCompiler.MakeFlags(false, ignore, reverse, false));
			int length = str.Length;
			this.Emit((ushort)length);
			if (ignore)
			{
				str = str.ToLower();
			}
			for (int i = 0; i < length; i++)
			{
				this.Emit((ushort)str[i]);
			}
		}

		public void EmitPosition(Position pos)
		{
			this.Emit(OpCode.Position, OpFlags.None);
			this.Emit((ushort)pos);
		}

		public void EmitOpen(int gid)
		{
			this.Emit(OpCode.Open);
			this.Emit((ushort)gid);
		}

		public void EmitClose(int gid)
		{
			this.Emit(OpCode.Close);
			this.Emit((ushort)gid);
		}

		public void EmitBalanceStart(int gid, int balance, bool capture, LinkRef tail)
		{
			this.BeginLink(tail);
			this.Emit(OpCode.BalanceStart);
			this.Emit((ushort)gid);
			this.Emit((ushort)balance);
			this.Emit((!capture) ? 0 : 1);
			this.EmitLink(tail);
		}

		public void EmitBalance()
		{
			this.Emit(OpCode.Balance);
		}

		public void EmitReference(int gid, bool ignore, bool reverse)
		{
			this.Emit(OpCode.Reference, PatternCompiler.MakeFlags(false, ignore, reverse, false));
			this.Emit((ushort)gid);
		}

		public void EmitIfDefined(int gid, LinkRef tail)
		{
			this.BeginLink(tail);
			this.Emit(OpCode.IfDefined);
			this.EmitLink(tail);
			this.Emit((ushort)gid);
		}

		public void EmitSub(LinkRef tail)
		{
			this.BeginLink(tail);
			this.Emit(OpCode.Sub);
			this.EmitLink(tail);
		}

		public void EmitTest(LinkRef yes, LinkRef tail)
		{
			this.BeginLink(yes);
			this.BeginLink(tail);
			this.Emit(OpCode.Test);
			this.EmitLink(yes);
			this.EmitLink(tail);
		}

		public void EmitBranch(LinkRef next)
		{
			this.BeginLink(next);
			this.Emit(OpCode.Branch, OpFlags.None);
			this.EmitLink(next);
		}

		public void EmitJump(LinkRef target)
		{
			this.BeginLink(target);
			this.Emit(OpCode.Jump, OpFlags.None);
			this.EmitLink(target);
		}

		public void EmitRepeat(int min, int max, bool lazy, LinkRef until)
		{
			this.BeginLink(until);
			this.Emit(OpCode.Repeat, PatternCompiler.MakeFlags(false, false, false, lazy));
			this.EmitLink(until);
			this.EmitCount(min);
			this.EmitCount(max);
		}

		public void EmitUntil(LinkRef repeat)
		{
			this.ResolveLink(repeat);
			this.Emit(OpCode.Until);
		}

		public void EmitFastRepeat(int min, int max, bool lazy, LinkRef tail)
		{
			this.BeginLink(tail);
			this.Emit(OpCode.FastRepeat, PatternCompiler.MakeFlags(false, false, false, lazy));
			this.EmitLink(tail);
			this.EmitCount(min);
			this.EmitCount(max);
		}

		public void EmitIn(LinkRef tail)
		{
			this.BeginLink(tail);
			this.Emit(OpCode.In);
			this.EmitLink(tail);
		}

		public void EmitAnchor(bool reverse, int offset, LinkRef tail)
		{
			this.BeginLink(tail);
			this.Emit(OpCode.Anchor, PatternCompiler.MakeFlags(false, false, reverse, false));
			this.EmitLink(tail);
			this.Emit((ushort)offset);
		}

		public void EmitInfo(int count, int min, int max)
		{
			this.Emit(OpCode.Info);
			this.EmitCount(count);
			this.EmitCount(min);
			this.EmitCount(max);
		}

		public LinkRef NewLink()
		{
			return new PatternCompiler.PatternLinkStack();
		}

		public void ResolveLink(LinkRef lref)
		{
			PatternCompiler.PatternLinkStack patternLinkStack = (PatternCompiler.PatternLinkStack)lref;
			while (patternLinkStack.Pop())
			{
				this.pgm[patternLinkStack.OffsetAddress] = (ushort)patternLinkStack.GetOffset(this.CurrentAddress);
			}
		}

		public void EmitBranchEnd()
		{
		}

		public void EmitAlternationEnd()
		{
		}

		private static OpFlags MakeFlags(bool negate, bool ignore, bool reverse, bool lazy)
		{
			OpFlags opFlags = OpFlags.None;
			if (negate)
			{
				opFlags |= OpFlags.Negate;
			}
			if (ignore)
			{
				opFlags |= OpFlags.IgnoreCase;
			}
			if (reverse)
			{
				opFlags |= OpFlags.RightToLeft;
			}
			if (lazy)
			{
				opFlags |= OpFlags.Lazy;
			}
			return opFlags;
		}

		private void Emit(OpCode op)
		{
			this.Emit(op, OpFlags.None);
		}

		private void Emit(OpCode op, OpFlags flags)
		{
			this.Emit(PatternCompiler.EncodeOp(op, flags));
		}

		private void Emit(ushort word)
		{
			this.pgm.Add(word);
		}

		private int CurrentAddress
		{
			get
			{
				return this.pgm.Count;
			}
		}

		private void BeginLink(LinkRef lref)
		{
			PatternCompiler.PatternLinkStack patternLinkStack = (PatternCompiler.PatternLinkStack)lref;
			patternLinkStack.BaseAddress = this.CurrentAddress;
		}

		private void EmitLink(LinkRef lref)
		{
			PatternCompiler.PatternLinkStack patternLinkStack = (PatternCompiler.PatternLinkStack)lref;
			patternLinkStack.OffsetAddress = this.CurrentAddress;
			this.Emit(0);
			patternLinkStack.Push();
		}

		private class PatternLinkStack : LinkStack
		{
			private PatternCompiler.PatternLinkStack.Link link;

			public int BaseAddress
			{
				set
				{
					this.link.base_addr = value;
				}
			}

			public int OffsetAddress
			{
				get
				{
					return this.link.offset_addr;
				}
				set
				{
					this.link.offset_addr = value;
				}
			}

			public int GetOffset(int target_addr)
			{
				return target_addr - this.link.base_addr;
			}

			protected override object GetCurrent()
			{
				return this.link;
			}

			protected override void SetCurrent(object l)
			{
				this.link = (PatternCompiler.PatternLinkStack.Link)l;
			}

			private struct Link
			{
				public int base_addr;

				public int offset_addr;
			}
		}
	}
}
