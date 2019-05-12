using System;

namespace System.Text.RegularExpressions
{
	internal class Interpreter : BaseMachine
	{
		private ushort[] program;

		private int program_start;

		private string text;

		private int text_end;

		private int group_count;

		private int match_min;

		private QuickSearch qs;

		private int scan_ptr;

		private Interpreter.RepeatContext repeat;

		private Interpreter.RepeatContext fast;

		private Interpreter.IntStack stack = default(Interpreter.IntStack);

		private Interpreter.RepeatContext deep;

		private Mark[] marks;

		private int mark_start;

		private int mark_end;

		private int[] groups;

		public Interpreter(ushort[] program)
		{
			this.program = program;
			this.qs = null;
			this.group_count = this.ReadProgramCount(1) + 1;
			this.match_min = this.ReadProgramCount(3);
			this.program_start = 7;
			this.groups = new int[this.group_count];
		}

		private int ReadProgramCount(int ptr)
		{
			int num = (int)this.program[ptr + 1];
			num <<= 16;
			return num + (int)this.program[ptr];
		}

		public override Match Scan(Regex regex, string text, int start, int end)
		{
			this.text = text;
			this.text_end = end;
			this.scan_ptr = start;
			if (this.Eval(Interpreter.Mode.Match, ref this.scan_ptr, this.program_start))
			{
				return this.GenerateMatch(regex);
			}
			return Match.Empty;
		}

		private void Reset()
		{
			this.ResetGroups();
			this.fast = (this.repeat = null);
		}

		private bool Eval(Interpreter.Mode mode, ref int ref_ptr, int pc)
		{
			int num = ref_ptr;
			Interpreter.RepeatContext repeatContext;
			int start;
			int count;
			for (;;)
			{
				OpFlags opFlags;
				for (;;)
				{
					ushort num2 = this.program[pc];
					OpCode opCode = (OpCode)(num2 & 255);
					opFlags = (OpFlags)(num2 & 65280);
					switch (opCode)
					{
					case OpCode.False:
						goto IL_4B8;
					case OpCode.True:
						goto IL_4BD;
					case OpCode.Position:
						if (!this.IsPosition((Position)this.program[pc + 1], num))
						{
							goto Block_44;
						}
						pc += 2;
						break;
					case OpCode.String:
					{
						bool flag = (ushort)(opFlags & OpFlags.RightToLeft) != 0;
						bool flag2 = (ushort)(opFlags & OpFlags.IgnoreCase) != 0;
						int num3 = (int)this.program[pc + 1];
						if (flag)
						{
							num -= num3;
							if (num < 0)
							{
								goto Block_46;
							}
						}
						else if (num + num3 > this.text_end)
						{
							goto Block_47;
						}
						pc += 2;
						for (int i = 0; i < num3; i++)
						{
							char c = this.text[num + i];
							if (flag2)
							{
								c = char.ToLower(c);
							}
							if (c != (char)this.program[pc++])
							{
								goto Block_49;
							}
						}
						if (!flag)
						{
							num += num3;
						}
						break;
					}
					case OpCode.Reference:
					{
						bool flag3 = (ushort)(opFlags & OpFlags.RightToLeft) != 0;
						bool flag4 = (ushort)(opFlags & OpFlags.IgnoreCase) != 0;
						int lastDefined = this.GetLastDefined((int)this.program[pc + 1]);
						if (lastDefined < 0)
						{
							goto Block_52;
						}
						int index = this.marks[lastDefined].Index;
						int length = this.marks[lastDefined].Length;
						if (flag3)
						{
							num -= length;
							if (num < 0)
							{
								goto Block_54;
							}
						}
						else if (num + length > this.text_end)
						{
							goto Block_55;
						}
						pc += 2;
						if (flag4)
						{
							for (int j = 0; j < length; j++)
							{
								if (char.ToLower(this.text[num + j]) != char.ToLower(this.text[index + j]))
								{
									goto Block_57;
								}
							}
						}
						else
						{
							for (int k = 0; k < length; k++)
							{
								if (this.text[num + k] != this.text[index + k])
								{
									goto Block_59;
								}
							}
						}
						if (!flag3)
						{
							num += length;
						}
						break;
					}
					case OpCode.Character:
					case OpCode.Category:
					case OpCode.NotCategory:
					case OpCode.Range:
					case OpCode.Set:
						if (!this.EvalChar(mode, ref num, ref pc, false))
						{
							goto Block_61;
						}
						break;
					case OpCode.In:
					{
						int num4 = pc + (int)this.program[pc + 1];
						pc += 2;
						if (!this.EvalChar(mode, ref num, ref pc, true))
						{
							goto Block_62;
						}
						pc = num4;
						break;
					}
					case OpCode.Open:
						this.Open((int)this.program[pc + 1], num);
						pc += 2;
						break;
					case OpCode.Close:
						this.Close((int)this.program[pc + 1], num);
						pc += 2;
						break;
					case OpCode.Balance:
						goto IL_7DB;
					case OpCode.BalanceStart:
					{
						int ptr = num;
						if (!this.Eval(Interpreter.Mode.Match, ref num, pc + 5))
						{
							goto Block_63;
						}
						if (!this.Balance((int)this.program[pc + 1], (int)this.program[pc + 2], this.program[pc + 3] == 1, ptr))
						{
							goto Block_65;
						}
						pc += (int)this.program[pc + 4];
						break;
					}
					case OpCode.IfDefined:
					{
						int lastDefined2 = this.GetLastDefined((int)this.program[pc + 2]);
						if (lastDefined2 < 0)
						{
							pc += (int)this.program[pc + 1];
						}
						else
						{
							pc += 3;
						}
						break;
					}
					case OpCode.Sub:
						if (!this.Eval(Interpreter.Mode.Match, ref num, pc + 2))
						{
							goto Block_67;
						}
						pc += (int)this.program[pc + 1];
						break;
					case OpCode.Test:
					{
						int cp = this.Checkpoint();
						int num5 = num;
						if (this.Eval(Interpreter.Mode.Match, ref num5, pc + 3))
						{
							pc += (int)this.program[pc + 1];
						}
						else
						{
							this.Backtrack(cp);
							pc += (int)this.program[pc + 2];
						}
						break;
					}
					case OpCode.Branch:
						goto IL_88A;
					case OpCode.Jump:
						pc += (int)this.program[pc + 1];
						break;
					case OpCode.Repeat:
						goto IL_8EE;
					case OpCode.Until:
						goto IL_957;
					case OpCode.FastRepeat:
						goto IL_C6F;
					case OpCode.Anchor:
						goto IL_96;
					case OpCode.Info:
						goto IL_FE9;
					}
				}
				for (;;)
				{
					IL_88A:
					int cp2 = this.Checkpoint();
					if (this.Eval(Interpreter.Mode.Match, ref num, pc + 2))
					{
						break;
					}
					this.Backtrack(cp2);
					pc += (int)this.program[pc + 1];
					if ((this.program[pc] & 255) == 0)
					{
						goto Block_70;
					}
				}
				IL_FF3:
				ref_ptr = num;
				if (mode == Interpreter.Mode.Match)
				{
					return true;
				}
				if (mode != Interpreter.Mode.Count)
				{
					break;
				}
				this.fast.Count++;
				if (this.fast.IsMaximum || (this.fast.IsLazy && this.fast.IsMinimum))
				{
					return true;
				}
				pc = this.fast.Expression;
				continue;
				IL_96:
				int num6 = (int)this.program[pc + 1];
				int num7 = (int)this.program[pc + 2];
				bool flag5 = (ushort)(opFlags & OpFlags.RightToLeft) != 0;
				int num8 = (!flag5) ? (num + num7) : (num - num7);
				int num9 = this.text_end - this.match_min + num7;
				int num10 = 0;
				OpCode opCode2 = (OpCode)(this.program[pc + 3] & 255);
				if (opCode2 == OpCode.Position && num6 == 6)
				{
					switch (this.program[pc + 4])
					{
					case 2:
						if (flag5 || num7 == 0)
						{
							if (flag5)
							{
								num = num7;
							}
							if (this.TryMatch(ref num, pc + num6))
							{
								goto IL_FF3;
							}
						}
						break;
					case 3:
						if (num8 == 0)
						{
							num = 0;
							if (this.TryMatch(ref num, pc + num6))
							{
								goto IL_FF3;
							}
							num8++;
						}
						while ((flag5 && num8 >= 0) || (!flag5 && num8 <= num9))
						{
							if (num8 == 0 || this.text[num8 - 1] == '\n')
							{
								if (flag5)
								{
									num = ((num8 != num9) ? (num8 + num7) : num8);
								}
								else
								{
									num = ((num8 != 0) ? (num8 - num7) : num8);
								}
								if (this.TryMatch(ref num, pc + num6))
								{
									goto IL_FF3;
								}
							}
							if (flag5)
							{
								num8--;
							}
							else
							{
								num8++;
							}
						}
						break;
					case 4:
						if (num8 == this.scan_ptr)
						{
							num = ((!flag5) ? (this.scan_ptr - num7) : (this.scan_ptr + num7));
							if (this.TryMatch(ref num, pc + num6))
							{
								goto IL_FF3;
							}
						}
						break;
					}
					break;
				}
				if (this.qs != null || (opCode2 == OpCode.String && num6 == (int)(6 + this.program[pc + 4])))
				{
					bool flag6 = (this.program[pc + 3] & 1024) != 0;
					if (this.qs == null)
					{
						bool ignore = (this.program[pc + 3] & 512) != 0;
						string @string = this.GetString(pc + 3);
						this.qs = new QuickSearch(@string, ignore, flag6);
					}
					while ((flag5 && num8 >= num10) || (!flag5 && num8 <= num9))
					{
						if (flag6)
						{
							num8 = this.qs.Search(this.text, num8, num10);
							if (num8 != -1)
							{
								num8 += this.qs.Length;
							}
						}
						else
						{
							num8 = this.qs.Search(this.text, num8, num9);
						}
						if (num8 < 0)
						{
							break;
						}
						num = ((!flag6) ? (num8 - num7) : (num8 + num7));
						if (this.TryMatch(ref num, pc + num6))
						{
							goto IL_FF3;
						}
						if (flag6)
						{
							num8 -= 2;
						}
						else
						{
							num8++;
						}
					}
					break;
				}
				if (opCode2 == OpCode.True)
				{
					while ((flag5 && num8 >= num10) || (!flag5 && num8 <= num9))
					{
						num = num8;
						if (this.TryMatch(ref num, pc + num6))
						{
							goto IL_FF3;
						}
						if (flag5)
						{
							num8--;
						}
						else
						{
							num8++;
						}
					}
					break;
				}
				while ((flag5 && num8 >= num10) || (!flag5 && num8 <= num9))
				{
					num = num8;
					if (this.Eval(Interpreter.Mode.Match, ref num, pc + 3))
					{
						num = ((!flag5) ? (num8 - num7) : (num8 + num7));
						if (this.TryMatch(ref num, pc + num6))
						{
							goto IL_FF3;
						}
					}
					if (flag5)
					{
						num8--;
					}
					else
					{
						num8++;
					}
				}
				break;
				IL_4BD:
				IL_7DB:
				goto IL_FF3;
				IL_8EE:
				this.repeat = new Interpreter.RepeatContext(this.repeat, this.ReadProgramCount(pc + 2), this.ReadProgramCount(pc + 4), (ushort)(opFlags & OpFlags.Lazy) != 0, pc + 6);
				if (this.Eval(Interpreter.Mode.Match, ref num, pc + (int)this.program[pc + 1]))
				{
					goto IL_FF3;
				}
				goto IL_941;
				IL_957:
				repeatContext = this.repeat;
				if (this.deep == repeatContext)
				{
					goto IL_FF3;
				}
				start = repeatContext.Start;
				count = repeatContext.Count;
				while (!repeatContext.IsMinimum)
				{
					repeatContext.Count++;
					repeatContext.Start = num;
					this.deep = repeatContext;
					if (!this.Eval(Interpreter.Mode.Match, ref num, repeatContext.Expression))
					{
						goto Block_73;
					}
					if (this.deep != repeatContext)
					{
						goto IL_FF3;
					}
				}
				if (num == repeatContext.Start)
				{
					this.repeat = repeatContext.Previous;
					this.deep = null;
					if (this.Eval(Interpreter.Mode.Match, ref num, pc + 1))
					{
						goto IL_FF3;
					}
					goto IL_A28;
				}
				else
				{
					if (repeatContext.IsLazy)
					{
						for (;;)
						{
							this.repeat = repeatContext.Previous;
							this.deep = null;
							int cp3 = this.Checkpoint();
							if (this.Eval(Interpreter.Mode.Match, ref num, pc + 1))
							{
								break;
							}
							this.Backtrack(cp3);
							this.repeat = repeatContext;
							if (repeatContext.IsMaximum)
							{
								goto Block_80;
							}
							repeatContext.Count++;
							repeatContext.Start = num;
							this.deep = repeatContext;
							if (!this.Eval(Interpreter.Mode.Match, ref num, repeatContext.Expression))
							{
								goto Block_81;
							}
							if (this.deep != repeatContext)
							{
								break;
							}
							if (num == repeatContext.Start)
							{
								goto Block_83;
							}
						}
						goto IL_FF3;
					}
					int count2 = this.stack.Count;
					while (!repeatContext.IsMaximum)
					{
						int num11 = this.Checkpoint();
						int value = num;
						int start2 = repeatContext.Start;
						repeatContext.Count++;
						repeatContext.Start = num;
						this.deep = repeatContext;
						if (!this.Eval(Interpreter.Mode.Match, ref num, repeatContext.Expression))
						{
							repeatContext.Count--;
							repeatContext.Start = start2;
							this.Backtrack(num11);
							break;
						}
						if (this.deep != repeatContext)
						{
							this.stack.Count = count2;
							goto IL_FF3;
						}
						this.stack.Push(num11);
						this.stack.Push(value);
						if (num == repeatContext.Start)
						{
							break;
						}
					}
					this.repeat = repeatContext.Previous;
					for (;;)
					{
						this.deep = null;
						if (this.Eval(Interpreter.Mode.Match, ref num, pc + 1))
						{
							break;
						}
						if (this.stack.Count == count2)
						{
							goto Block_88;
						}
						repeatContext.Count--;
						num = this.stack.Pop();
						this.Backtrack(this.stack.Pop());
					}
					this.stack.Count = count2;
					goto IL_FF3;
				}
				IL_C6F:
				this.fast = new Interpreter.RepeatContext(this.fast, this.ReadProgramCount(pc + 2), this.ReadProgramCount(pc + 4), (ushort)(opFlags & OpFlags.Lazy) != 0, pc + 6);
				this.fast.Start = num;
				int cp4 = this.Checkpoint();
				pc += (int)this.program[pc + 1];
				ushort num12 = this.program[pc];
				int num13 = -1;
				int num14 = -1;
				int num15 = 0;
				OpCode opCode3 = (OpCode)(num12 & 255);
				if (opCode3 == OpCode.Character || opCode3 == OpCode.String)
				{
					OpFlags opFlags2 = (OpFlags)(num12 & 65280);
					if ((ushort)(opFlags2 & OpFlags.Negate) == 0)
					{
						if (opCode3 == OpCode.String)
						{
							int num16 = 0;
							if ((ushort)(opFlags2 & OpFlags.RightToLeft) != 0)
							{
								num16 = (int)(this.program[pc + 1] - 1);
							}
							num13 = (int)this.program[pc + 2 + num16];
						}
						else
						{
							num13 = (int)this.program[pc + 1];
						}
						if ((ushort)(opFlags2 & OpFlags.IgnoreCase) != 0)
						{
							num14 = (int)char.ToUpper((char)num13);
						}
						else
						{
							num14 = num13;
						}
						if ((ushort)(opFlags2 & OpFlags.RightToLeft) != 0)
						{
							num15 = -1;
						}
						else
						{
							num15 = 0;
						}
					}
				}
				if (this.fast.IsLazy)
				{
					if (!this.fast.IsMinimum && !this.Eval(Interpreter.Mode.Count, ref num, this.fast.Expression))
					{
						goto Block_97;
					}
					for (;;)
					{
						int num17 = num + num15;
						if (num13 < 0 || (num17 >= 0 && num17 < this.text_end && (num13 == (int)this.text[num17] || num14 == (int)this.text[num17])))
						{
							this.deep = null;
							if (this.Eval(Interpreter.Mode.Match, ref num, pc))
							{
								break;
							}
						}
						if (this.fast.IsMaximum)
						{
							goto Block_103;
						}
						this.Backtrack(cp4);
						if (!this.Eval(Interpreter.Mode.Count, ref num, this.fast.Expression))
						{
							goto Block_104;
						}
					}
					this.fast = this.fast.Previous;
					goto IL_FF3;
				}
				else
				{
					if (!this.Eval(Interpreter.Mode.Count, ref num, this.fast.Expression))
					{
						goto Block_105;
					}
					int num18;
					if (this.fast.Count > 0)
					{
						num18 = (num - this.fast.Start) / this.fast.Count;
					}
					else
					{
						num18 = 0;
					}
					for (;;)
					{
						int num19 = num + num15;
						if (num13 < 0 || (num19 >= 0 && num19 < this.text_end && (num13 == (int)this.text[num19] || num14 == (int)this.text[num19])))
						{
							this.deep = null;
							if (this.Eval(Interpreter.Mode.Match, ref num, pc))
							{
								break;
							}
						}
						this.fast.Count--;
						if (!this.fast.IsMinimum)
						{
							goto Block_112;
						}
						num -= num18;
						this.Backtrack(cp4);
					}
					this.fast = this.fast.Previous;
					goto IL_FF3;
				}
			}
			IL_4B8:
			Block_44:
			Block_46:
			Block_47:
			Block_49:
			Block_52:
			Block_54:
			Block_55:
			Block_57:
			Block_59:
			Block_61:
			Block_62:
			Block_63:
			Block_65:
			Block_67:
			Block_70:
			goto IL_1067;
			IL_941:
			this.repeat = this.repeat.Previous;
			goto IL_1067;
			Block_73:
			repeatContext.Start = start;
			repeatContext.Count = count;
			goto IL_1067;
			IL_A28:
			this.repeat = repeatContext;
			Block_80:
			goto IL_1067;
			Block_81:
			repeatContext.Start = start;
			repeatContext.Count = count;
			Block_83:
			goto IL_1067;
			Block_88:
			this.repeat = repeatContext;
			goto IL_1067;
			Block_97:
			this.fast = this.fast.Previous;
			goto IL_1067;
			Block_103:
			this.fast = this.fast.Previous;
			goto IL_1067;
			Block_104:
			this.fast = this.fast.Previous;
			goto IL_1067;
			Block_105:
			this.fast = this.fast.Previous;
			goto IL_1067;
			Block_112:
			this.fast = this.fast.Previous;
			IL_FE9:
			IL_1067:
			if (mode == Interpreter.Mode.Match)
			{
				return false;
			}
			if (mode != Interpreter.Mode.Count)
			{
				return false;
			}
			if (!this.fast.IsLazy && this.fast.IsMinimum)
			{
				return true;
			}
			ref_ptr = this.fast.Start;
			return false;
		}

		private bool EvalChar(Interpreter.Mode mode, ref int ptr, ref int pc, bool multi)
		{
			bool flag = false;
			char c = '\0';
			bool flag3;
			for (;;)
			{
				ushort num = this.program[pc];
				OpCode opCode = (OpCode)(num & 255);
				OpFlags opFlags = (OpFlags)(num & 65280);
				pc++;
				bool flag2 = (ushort)(opFlags & OpFlags.IgnoreCase) != 0;
				if (!flag)
				{
					if ((ushort)(opFlags & OpFlags.RightToLeft) != 0)
					{
						if (ptr <= 0)
						{
							break;
						}
						c = this.text[--ptr];
					}
					else
					{
						if (ptr >= this.text_end)
						{
							return false;
						}
						c = this.text[ptr++];
					}
					if (flag2)
					{
						c = char.ToLower(c);
					}
					flag = true;
				}
				flag3 = ((ushort)(opFlags & OpFlags.Negate) != 0);
				switch (opCode)
				{
				case OpCode.False:
					return false;
				case OpCode.True:
					return true;
				case OpCode.Character:
					if (c == (char)this.program[pc++])
					{
						goto Block_7;
					}
					break;
				case OpCode.Category:
					if (CategoryUtils.IsCategory((Category)this.program[pc++], c))
					{
						goto Block_8;
					}
					break;
				case OpCode.NotCategory:
					if (!CategoryUtils.IsCategory((Category)this.program[pc++], c))
					{
						goto Block_9;
					}
					break;
				case OpCode.Range:
				{
					int num2 = (int)this.program[pc++];
					int num3 = (int)this.program[pc++];
					if (num2 <= (int)c && (int)c <= num3)
					{
						goto Block_11;
					}
					break;
				}
				case OpCode.Set:
				{
					int num4 = (int)this.program[pc++];
					int num5 = (int)this.program[pc++];
					int num6 = pc;
					pc += num5;
					int num7 = (int)c - num4;
					if (num7 >= 0 && num7 < num5 << 4)
					{
						if (((int)this.program[num6 + (num7 >> 4)] & 1 << (num7 & 15)) != 0)
						{
							goto Block_13;
						}
					}
					break;
				}
				}
				if (!multi)
				{
					return flag3;
				}
			}
			return false;
			Block_7:
			return !flag3;
			Block_8:
			return !flag3;
			Block_9:
			return !flag3;
			Block_11:
			return !flag3;
			Block_13:
			return !flag3;
		}

		private bool TryMatch(ref int ref_ptr, int pc)
		{
			this.Reset();
			int num = ref_ptr;
			this.marks[this.groups[0]].Start = num;
			if (this.Eval(Interpreter.Mode.Match, ref num, pc))
			{
				this.marks[this.groups[0]].End = num;
				ref_ptr = num;
				return true;
			}
			return false;
		}

		private bool IsPosition(Position pos, int ptr)
		{
			switch (pos)
			{
			case Position.Start:
			case Position.StartOfString:
				return ptr == 0;
			case Position.StartOfLine:
				return ptr == 0 || this.text[ptr - 1] == '\n';
			case Position.StartOfScan:
				return ptr == this.scan_ptr;
			case Position.End:
				return ptr == this.text_end || (ptr == this.text_end - 1 && this.text[ptr] == '\n');
			case Position.EndOfString:
				return ptr == this.text_end;
			case Position.EndOfLine:
				return ptr == this.text_end || this.text[ptr] == '\n';
			case Position.Boundary:
				if (this.text_end == 0)
				{
					return false;
				}
				if (ptr == 0)
				{
					return this.IsWordChar(this.text[ptr]);
				}
				if (ptr == this.text_end)
				{
					return this.IsWordChar(this.text[ptr - 1]);
				}
				return this.IsWordChar(this.text[ptr]) != this.IsWordChar(this.text[ptr - 1]);
			case Position.NonBoundary:
				if (this.text_end == 0)
				{
					return false;
				}
				if (ptr == 0)
				{
					return !this.IsWordChar(this.text[ptr]);
				}
				if (ptr == this.text_end)
				{
					return !this.IsWordChar(this.text[ptr - 1]);
				}
				return this.IsWordChar(this.text[ptr]) == this.IsWordChar(this.text[ptr - 1]);
			default:
				return false;
			}
		}

		private bool IsWordChar(char c)
		{
			return CategoryUtils.IsCategory(Category.Word, c);
		}

		private string GetString(int pc)
		{
			int num = (int)this.program[pc + 1];
			int num2 = pc + 2;
			char[] array = new char[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (char)this.program[num2++];
			}
			return new string(array);
		}

		private void Open(int gid, int ptr)
		{
			int num = this.groups[gid];
			if (num < this.mark_start || this.marks[num].IsDefined)
			{
				num = this.CreateMark(num);
				this.groups[gid] = num;
			}
			this.marks[num].Start = ptr;
		}

		private void Close(int gid, int ptr)
		{
			this.marks[this.groups[gid]].End = ptr;
		}

		private bool Balance(int gid, int balance_gid, bool capture, int ptr)
		{
			int num = this.groups[balance_gid];
			if (num == -1 || this.marks[num].Index < 0)
			{
				return false;
			}
			if (gid > 0 && capture)
			{
				this.Open(gid, this.marks[num].Index + this.marks[num].Length);
				this.Close(gid, ptr);
			}
			this.groups[balance_gid] = this.marks[num].Previous;
			return true;
		}

		private int Checkpoint()
		{
			this.mark_start = this.mark_end;
			return this.mark_start;
		}

		private void Backtrack(int cp)
		{
			for (int i = 0; i < this.groups.Length; i++)
			{
				int num = this.groups[i];
				while (cp <= num)
				{
					num = this.marks[num].Previous;
				}
				this.groups[i] = num;
			}
		}

		private void ResetGroups()
		{
			int num = this.groups.Length;
			if (this.marks == null)
			{
				this.marks = new Mark[num * 10];
			}
			for (int i = 0; i < num; i++)
			{
				this.groups[i] = i;
				this.marks[i].Start = -1;
				this.marks[i].End = -1;
				this.marks[i].Previous = -1;
			}
			this.mark_start = 0;
			this.mark_end = num;
		}

		private int GetLastDefined(int gid)
		{
			int num = this.groups[gid];
			while (num >= 0 && !this.marks[num].IsDefined)
			{
				num = this.marks[num].Previous;
			}
			return num;
		}

		private int CreateMark(int previous)
		{
			if (this.mark_end == this.marks.Length)
			{
				Mark[] array = new Mark[this.marks.Length * 2];
				this.marks.CopyTo(array, 0);
				this.marks = array;
			}
			int num = this.mark_end++;
			this.marks[num].Start = (this.marks[num].End = -1);
			this.marks[num].Previous = previous;
			return num;
		}

		private void GetGroupInfo(int gid, out int first_mark_index, out int n_caps)
		{
			first_mark_index = -1;
			n_caps = 0;
			for (int i = this.groups[gid]; i >= 0; i = this.marks[i].Previous)
			{
				if (this.marks[i].IsDefined)
				{
					if (first_mark_index < 0)
					{
						first_mark_index = i;
					}
					n_caps++;
				}
			}
		}

		private void PopulateGroup(Group g, int first_mark_index, int n_caps)
		{
			int num = 1;
			for (int i = this.marks[first_mark_index].Previous; i >= 0; i = this.marks[i].Previous)
			{
				if (this.marks[i].IsDefined)
				{
					Capture cap = new Capture(this.text, this.marks[i].Index, this.marks[i].Length);
					g.Captures.SetValue(cap, n_caps - 1 - num);
					num++;
				}
			}
		}

		private Match GenerateMatch(Regex regex)
		{
			int num;
			int n_caps;
			this.GetGroupInfo(0, out num, out n_caps);
			if (!this.needs_groups_or_captures)
			{
				return new Match(regex, this, this.text, this.text_end, 0, this.marks[num].Index, this.marks[num].Length);
			}
			Match match = new Match(regex, this, this.text, this.text_end, this.groups.Length, this.marks[num].Index, this.marks[num].Length, n_caps);
			this.PopulateGroup(match, num, n_caps);
			for (int i = 1; i < this.groups.Length; i++)
			{
				this.GetGroupInfo(i, out num, out n_caps);
				Group g;
				if (num < 0)
				{
					g = Group.Fail;
				}
				else
				{
					g = new Group(this.text, this.marks[num].Index, this.marks[num].Length, n_caps);
					this.PopulateGroup(g, num, n_caps);
				}
				match.Groups.SetValue(g, i);
			}
			return match;
		}

		private struct IntStack
		{
			private int[] values;

			private int count;

			public int Pop()
			{
				return this.values[--this.count];
			}

			public void Push(int value)
			{
				if (this.values == null)
				{
					this.values = new int[8];
				}
				else if (this.count == this.values.Length)
				{
					int num = this.values.Length;
					num += num >> 1;
					int[] array = new int[num];
					for (int i = 0; i < this.count; i++)
					{
						array[i] = this.values[i];
					}
					this.values = array;
				}
				this.values[this.count++] = value;
			}

			public int Top
			{
				get
				{
					return this.values[this.count - 1];
				}
			}

			public int Count
			{
				get
				{
					return this.count;
				}
				set
				{
					if (value > this.count)
					{
						throw new SystemException("can only truncate the stack");
					}
					this.count = value;
				}
			}
		}

		private class RepeatContext
		{
			private int start;

			private int min;

			private int max;

			private bool lazy;

			private int expr_pc;

			private Interpreter.RepeatContext previous;

			private int count;

			public RepeatContext(Interpreter.RepeatContext previous, int min, int max, bool lazy, int expr_pc)
			{
				this.previous = previous;
				this.min = min;
				this.max = max;
				this.lazy = lazy;
				this.expr_pc = expr_pc;
				this.start = -1;
				this.count = 0;
			}

			public int Count
			{
				get
				{
					return this.count;
				}
				set
				{
					this.count = value;
				}
			}

			public int Start
			{
				get
				{
					return this.start;
				}
				set
				{
					this.start = value;
				}
			}

			public bool IsMinimum
			{
				get
				{
					return this.min <= this.count;
				}
			}

			public bool IsMaximum
			{
				get
				{
					return this.max <= this.count;
				}
			}

			public bool IsLazy
			{
				get
				{
					return this.lazy;
				}
			}

			public int Expression
			{
				get
				{
					return this.expr_pc;
				}
			}

			public Interpreter.RepeatContext Previous
			{
				get
				{
					return this.previous;
				}
			}
		}

		private enum Mode
		{
			Search,
			Match,
			Count
		}
	}
}
