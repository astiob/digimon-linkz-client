using System;
using System.Globalization;

namespace System.Text.RegularExpressions
{
	internal sealed class RxInterpreter : BaseMachine
	{
		private byte[] program;

		private string str;

		private int string_start;

		private int string_end;

		private int group_count;

		private int[] groups;

		private EvalDelegate eval_del;

		private Mark[] marks;

		private int mark_start;

		private int mark_end;

		private RxInterpreter.IntStack stack;

		private RxInterpreter.RepeatContext repeat;

		private RxInterpreter.RepeatContext deep;

		public static readonly bool trace_rx;

		public RxInterpreter(byte[] program, EvalDelegate eval_del)
		{
			this.program = program;
			this.eval_del = eval_del;
			this.group_count = 1 + ((int)program[1] | (int)program[2] << 8);
			this.groups = new int[this.group_count];
			this.stack = default(RxInterpreter.IntStack);
			this.ResetGroups();
		}

		private static int ReadInt(byte[] code, int pc)
		{
			int num = (int)code[pc];
			num |= (int)code[pc + 1] << 8;
			num |= (int)code[pc + 2] << 16;
			return num | (int)code[pc + 3] << 24;
		}

		public override Match Scan(Regex regex, string text, int start, int end)
		{
			this.str = text;
			this.string_start = start;
			this.string_end = end;
			int end2 = 0;
			bool flag;
			if (this.eval_del != null)
			{
				flag = this.eval_del(this, start, ref end2);
			}
			else
			{
				flag = this.EvalByteCode(11, start, ref end2);
			}
			this.marks[this.groups[0]].End = end2;
			if (flag)
			{
				return this.GenerateMatch(regex);
			}
			return Match.Empty;
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
				this.marks = new Mark[num];
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
					Capture cap = new Capture(this.str, this.marks[i].Index, this.marks[i].Length);
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
				return new Match(regex, this, this.str, this.string_end, 0, this.marks[num].Index, this.marks[num].Length);
			}
			Match match = new Match(regex, this, this.str, this.string_end, this.groups.Length, this.marks[num].Index, this.marks[num].Length, n_caps);
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
					g = new Group(this.str, this.marks[num].Index, this.marks[num].Length, n_caps);
					this.PopulateGroup(g, num, n_caps);
				}
				match.Groups.SetValue(g, i);
			}
			return match;
		}

		internal void SetStartOfMatch(int pos)
		{
			this.marks[this.groups[0]].Start = pos;
		}

		private static bool IsWordChar(char c)
		{
			return char.IsLetterOrDigit(c) || char.GetUnicodeCategory(c) == UnicodeCategory.ConnectorPunctuation;
		}

		private bool EvalByteCode(int pc, int strpos, ref int strpos_result)
		{
			int num = 0;
			int i;
			int j;
			int num2;
			int num31;
			for (;;)
			{
				if (RxInterpreter.trace_rx)
				{
					Console.WriteLine("evaluating: {0} at pc: {1}, strpos: {2}, cge: {3}", new object[]
					{
						(RxOp)this.program[pc],
						pc,
						strpos,
						num
					});
				}
				switch (this.program[pc])
				{
				case 1:
					return false;
				case 2:
					if (num != 0)
					{
						pc = num;
						num = 0;
						continue;
					}
					goto IL_2DF;
				case 3:
					pc++;
					continue;
				case 4:
					if (strpos != 0)
					{
						return false;
					}
					pc++;
					continue;
				case 5:
					if (strpos == 0 || this.str[strpos - 1] == '\n')
					{
						pc++;
						continue;
					}
					return false;
				case 6:
					if (strpos != this.string_start)
					{
						return false;
					}
					pc++;
					continue;
				case 7:
					if (strpos != this.string_end)
					{
						return false;
					}
					pc++;
					continue;
				case 8:
					if (strpos == this.string_end || this.str[strpos] == '\n')
					{
						pc++;
						continue;
					}
					return false;
				case 9:
					if (strpos == this.string_end || (strpos == this.string_end - 1 && this.str[strpos] == '\n'))
					{
						pc++;
						continue;
					}
					return false;
				case 10:
					if (this.string_end == 0)
					{
						return false;
					}
					if (strpos == 0)
					{
						if (RxInterpreter.IsWordChar(this.str[strpos]))
						{
							pc++;
							continue;
						}
						return false;
					}
					else if (strpos == this.string_end)
					{
						if (RxInterpreter.IsWordChar(this.str[strpos - 1]))
						{
							pc++;
							continue;
						}
						return false;
					}
					else
					{
						if (RxInterpreter.IsWordChar(this.str[strpos]) != RxInterpreter.IsWordChar(this.str[strpos - 1]))
						{
							pc++;
							continue;
						}
						return false;
					}
					break;
				case 11:
					if (this.string_end == 0)
					{
						return false;
					}
					if (strpos == 0)
					{
						if (!RxInterpreter.IsWordChar(this.str[strpos]))
						{
							pc++;
							continue;
						}
						return false;
					}
					else if (strpos == this.string_end)
					{
						if (!RxInterpreter.IsWordChar(this.str[strpos - 1]))
						{
							pc++;
							continue;
						}
						return false;
					}
					else
					{
						if (RxInterpreter.IsWordChar(this.str[strpos]) == RxInterpreter.IsWordChar(this.str[strpos - 1]))
						{
							pc++;
							continue;
						}
						return false;
					}
					break;
				case 12:
					i = pc + 2;
					j = (int)this.program[pc + 1];
					if (strpos + j > this.string_end)
					{
						return false;
					}
					num2 = i + j;
					while (i < num2)
					{
						if (this.str[strpos] != (char)this.program[i])
						{
							return false;
						}
						strpos++;
						i++;
					}
					pc = num2;
					continue;
				case 13:
					i = pc + 2;
					j = (int)this.program[pc + 1];
					if (strpos + j > this.string_end)
					{
						return false;
					}
					num2 = i + j;
					while (i < num2)
					{
						if (this.str[strpos] != (char)this.program[i] && char.ToLower(this.str[strpos]) != (char)this.program[i])
						{
							return false;
						}
						strpos++;
						i++;
					}
					pc = num2;
					continue;
				case 14:
				{
					i = pc + 2;
					j = (int)this.program[pc + 1];
					if (strpos < j)
					{
						return false;
					}
					int num3 = strpos - j;
					num2 = i + j;
					while (i < num2)
					{
						if (this.str[num3] != (char)this.program[i])
						{
							return false;
						}
						i++;
						num3++;
					}
					strpos -= j;
					pc = num2;
					continue;
				}
				case 15:
				{
					i = pc + 2;
					j = (int)this.program[pc + 1];
					if (strpos < j)
					{
						return false;
					}
					int num4 = strpos - j;
					num2 = i + j;
					while (i < num2)
					{
						if (this.str[num4] != (char)this.program[i] && char.ToLower(this.str[num4]) != (char)this.program[i])
						{
							return false;
						}
						i++;
						num4++;
					}
					strpos -= j;
					pc = num2;
					continue;
				}
				case 16:
					i = pc + 3;
					j = ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					if (strpos + j > this.string_end)
					{
						return false;
					}
					num2 = i + j * 2;
					while (i < num2)
					{
						int num5 = (int)this.program[i] | (int)this.program[i + 1] << 8;
						if ((int)this.str[strpos] != num5)
						{
							return false;
						}
						strpos++;
						i += 2;
					}
					pc = num2;
					continue;
				case 17:
					i = pc + 3;
					j = ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					if (strpos + j > this.string_end)
					{
						return false;
					}
					num2 = i + j * 2;
					while (i < num2)
					{
						int num6 = (int)this.program[i] | (int)this.program[i + 1] << 8;
						if ((int)this.str[strpos] != num6 && (int)char.ToLower(this.str[strpos]) != num6)
						{
							return false;
						}
						strpos++;
						i += 2;
					}
					pc = num2;
					continue;
				case 18:
				{
					i = pc + 3;
					j = ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					if (strpos < j)
					{
						return false;
					}
					int num7 = strpos - j;
					num2 = i + j * 2;
					while (i < num2)
					{
						int num8 = (int)this.program[i] | (int)this.program[i + 1] << 8;
						if ((int)this.str[num7] != num8)
						{
							return false;
						}
						i += 2;
						num7 += 2;
					}
					strpos -= j;
					pc = num2;
					continue;
				}
				case 19:
				{
					i = pc + 3;
					j = ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					if (strpos < j)
					{
						return false;
					}
					int num9 = strpos - j;
					num2 = i + j * 2;
					while (i < num2)
					{
						int num10 = (int)this.program[i] | (int)this.program[i + 1] << 8;
						if ((int)this.str[num9] != num10 && (int)char.ToLower(this.str[num9]) != num10)
						{
							return false;
						}
						i += 2;
						num9 += 2;
					}
					strpos -= j;
					pc = num2;
					continue;
				}
				case 20:
					if (strpos < this.string_end)
					{
						char c = this.str[strpos];
						if (c == (char)this.program[pc + 1])
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 2;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 2;
					continue;
				case 21:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c2 = this.str[strpos];
					if (c2 != (char)this.program[pc + 1])
					{
						pc += 2;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 22:
					if (strpos < this.string_end)
					{
						char c3 = char.ToLower(this.str[strpos]);
						if (c3 == (char)this.program[pc + 1])
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 2;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 2;
					continue;
				case 23:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c4 = char.ToLower(this.str[strpos]);
					if (c4 != (char)this.program[pc + 1])
					{
						pc += 2;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 24:
					if (strpos > 0)
					{
						char c5 = this.str[strpos - 1];
						if (c5 == (char)this.program[pc + 1])
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 2;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 2;
					continue;
				case 25:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c6 = this.str[strpos - 1];
					if (c6 != (char)this.program[pc + 1])
					{
						pc += 2;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 26:
					if (strpos > 0)
					{
						char c7 = char.ToLower(this.str[strpos - 1]);
						if (c7 == (char)this.program[pc + 1])
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 2;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 2;
					continue;
				case 27:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c8 = char.ToLower(this.str[strpos - 1]);
					if (c8 != (char)this.program[pc + 1])
					{
						pc += 2;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 28:
					if (strpos < this.string_end)
					{
						char c9 = this.str[strpos];
						if (c9 >= (char)this.program[pc + 1] && c9 <= (char)this.program[pc + 2])
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 3;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 3;
					continue;
				case 29:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c10 = this.str[strpos];
					if (c10 < (char)this.program[pc + 1] || c10 > (char)this.program[pc + 2])
					{
						pc += 3;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 30:
					if (strpos < this.string_end)
					{
						char c11 = char.ToLower(this.str[strpos]);
						if (c11 >= (char)this.program[pc + 1] && c11 <= (char)this.program[pc + 2])
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 3;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 3;
					continue;
				case 31:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c12 = char.ToLower(this.str[strpos]);
					if (c12 < (char)this.program[pc + 1] || c12 > (char)this.program[pc + 2])
					{
						pc += 3;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 32:
					if (strpos > 0)
					{
						char c13 = this.str[strpos - 1];
						if (c13 >= (char)this.program[pc + 1] && c13 <= (char)this.program[pc + 2])
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 3;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 3;
					continue;
				case 33:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c14 = this.str[strpos - 1];
					if (c14 < (char)this.program[pc + 1] || c14 > (char)this.program[pc + 2])
					{
						pc += 3;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 34:
					if (strpos > 0)
					{
						char c15 = char.ToLower(this.str[strpos - 1]);
						if (c15 >= (char)this.program[pc + 1] && c15 <= (char)this.program[pc + 2])
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 3;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 3;
					continue;
				case 35:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c16 = char.ToLower(this.str[strpos - 1]);
					if (c16 < (char)this.program[pc + 1] || c16 > (char)this.program[pc + 2])
					{
						pc += 3;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 36:
					if (strpos < this.string_end)
					{
						char c17 = this.str[strpos];
						int num11 = (int)c17;
						num11 -= (int)this.program[pc + 1];
						j = (int)this.program[pc + 2];
						if (num11 >= 0 && num11 < j << 3 && ((int)this.program[pc + 3 + (num11 >> 3)] & 1 << (num11 & 7)) != 0)
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += (int)(3 + this.program[pc + 2]);
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += (int)(3 + this.program[pc + 2]);
					continue;
				case 37:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c18 = this.str[strpos];
					int num12 = (int)c18;
					num12 -= (int)this.program[pc + 1];
					j = (int)this.program[pc + 2];
					if (num12 < 0 || num12 >= j << 3 || ((int)this.program[pc + 3 + (num12 >> 3)] & 1 << (num12 & 7)) == 0)
					{
						pc += (int)(3 + this.program[pc + 2]);
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 38:
					if (strpos < this.string_end)
					{
						char c19 = char.ToLower(this.str[strpos]);
						int num13 = (int)c19;
						num13 -= (int)this.program[pc + 1];
						j = (int)this.program[pc + 2];
						if (num13 >= 0 && num13 < j << 3 && ((int)this.program[pc + 3 + (num13 >> 3)] & 1 << (num13 & 7)) != 0)
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += (int)(3 + this.program[pc + 2]);
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += (int)(3 + this.program[pc + 2]);
					continue;
				case 39:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c20 = char.ToLower(this.str[strpos]);
					int num14 = (int)c20;
					num14 -= (int)this.program[pc + 1];
					j = (int)this.program[pc + 2];
					if (num14 < 0 || num14 >= j << 3 || ((int)this.program[pc + 3 + (num14 >> 3)] & 1 << (num14 & 7)) == 0)
					{
						pc += (int)(3 + this.program[pc + 2]);
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 40:
					if (strpos > 0)
					{
						char c21 = this.str[strpos - 1];
						int num15 = (int)c21;
						num15 -= (int)this.program[pc + 1];
						j = (int)this.program[pc + 2];
						if (num15 >= 0 && num15 < j << 3 && ((int)this.program[pc + 3 + (num15 >> 3)] & 1 << (num15 & 7)) != 0)
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += (int)(3 + this.program[pc + 2]);
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += (int)(3 + this.program[pc + 2]);
					continue;
				case 41:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c22 = this.str[strpos - 1];
					int num16 = (int)c22;
					num16 -= (int)this.program[pc + 1];
					j = (int)this.program[pc + 2];
					if (num16 < 0 || num16 >= j << 3 || ((int)this.program[pc + 3 + (num16 >> 3)] & 1 << (num16 & 7)) == 0)
					{
						pc += (int)(3 + this.program[pc + 2]);
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 42:
					if (strpos > 0)
					{
						char c23 = char.ToLower(this.str[strpos - 1]);
						int num17 = (int)c23;
						num17 -= (int)this.program[pc + 1];
						j = (int)this.program[pc + 2];
						if (num17 >= 0 && num17 < j << 3 && ((int)this.program[pc + 3 + (num17 >> 3)] & 1 << (num17 & 7)) != 0)
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += (int)(3 + this.program[pc + 2]);
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += (int)(3 + this.program[pc + 2]);
					continue;
				case 43:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c24 = char.ToLower(this.str[strpos - 1]);
					int num18 = (int)c24;
					num18 -= (int)this.program[pc + 1];
					j = (int)this.program[pc + 2];
					if (num18 < 0 || num18 >= j << 3 || ((int)this.program[pc + 3 + (num18 >> 3)] & 1 << (num18 & 7)) == 0)
					{
						pc += (int)(3 + this.program[pc + 2]);
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 44:
					if (strpos < this.string_end)
					{
						char c25 = this.str[strpos];
						if ((int)c25 == ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8))
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 3;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 3;
					continue;
				case 45:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c26 = this.str[strpos];
					if ((int)c26 != ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8))
					{
						pc += 3;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 46:
					if (strpos < this.string_end)
					{
						char c27 = char.ToLower(this.str[strpos]);
						if ((int)c27 == ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8))
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 3;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 3;
					continue;
				case 47:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c28 = char.ToLower(this.str[strpos]);
					if ((int)c28 != ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8))
					{
						pc += 3;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 48:
					if (strpos > 0)
					{
						char c29 = this.str[strpos - 1];
						if ((int)c29 == ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8))
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 3;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 3;
					continue;
				case 49:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c30 = this.str[strpos - 1];
					if ((int)c30 != ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8))
					{
						pc += 3;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 50:
					if (strpos > 0)
					{
						char c31 = char.ToLower(this.str[strpos - 1]);
						if ((int)c31 == ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8))
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 3;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 3;
					continue;
				case 51:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c32 = char.ToLower(this.str[strpos - 1]);
					if ((int)c32 != ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8))
					{
						pc += 3;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 52:
					if (strpos < this.string_end)
					{
						char c33 = this.str[strpos];
						if ((int)c33 >= ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8) && (int)c33 <= ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8))
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 5;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 5;
					continue;
				case 53:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c34 = this.str[strpos];
					if ((int)c34 < ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8) || (int)c34 > ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8))
					{
						pc += 5;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 54:
					if (strpos < this.string_end)
					{
						char c35 = char.ToLower(this.str[strpos]);
						if ((int)c35 >= ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8) && (int)c35 <= ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8))
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 5;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 5;
					continue;
				case 55:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c36 = char.ToLower(this.str[strpos]);
					if ((int)c36 < ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8) || (int)c36 > ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8))
					{
						pc += 5;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 56:
					if (strpos > 0)
					{
						char c37 = this.str[strpos - 1];
						if ((int)c37 >= ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8) && (int)c37 <= ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8))
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 5;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 5;
					continue;
				case 57:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c38 = this.str[strpos - 1];
					if ((int)c38 < ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8) || (int)c38 > ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8))
					{
						pc += 5;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 58:
					if (strpos > 0)
					{
						char c39 = char.ToLower(this.str[strpos - 1]);
						if ((int)c39 >= ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8) && (int)c39 <= ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8))
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 5;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 5;
					continue;
				case 59:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c40 = char.ToLower(this.str[strpos - 1]);
					if ((int)c40 < ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8) || (int)c40 > ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8))
					{
						pc += 5;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 60:
					if (strpos < this.string_end)
					{
						char c41 = this.str[strpos];
						int num19 = (int)c41;
						num19 -= ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
						j = ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
						if (num19 >= 0 && num19 < j << 3 && ((int)this.program[pc + 5 + (num19 >> 3)] & 1 << (num19 & 7)) != 0)
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 5 + ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 5 + ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
					continue;
				case 61:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c42 = this.str[strpos];
					int num20 = (int)c42;
					num20 -= ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					j = ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
					if (num20 < 0 || num20 >= j << 3 || ((int)this.program[pc + 5 + (num20 >> 3)] & 1 << (num20 & 7)) == 0)
					{
						pc += 5 + ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 62:
					if (strpos < this.string_end)
					{
						char c43 = char.ToLower(this.str[strpos]);
						int num21 = (int)c43;
						num21 -= ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
						j = ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
						if (num21 >= 0 && num21 < j << 3 && ((int)this.program[pc + 5 + (num21 >> 3)] & 1 << (num21 & 7)) != 0)
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 5 + ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 5 + ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
					continue;
				case 63:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c44 = char.ToLower(this.str[strpos]);
					int num22 = (int)c44;
					num22 -= ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					j = ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
					if (num22 < 0 || num22 >= j << 3 || ((int)this.program[pc + 5 + (num22 >> 3)] & 1 << (num22 & 7)) == 0)
					{
						pc += 5 + ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 64:
					if (strpos > 0)
					{
						char c45 = this.str[strpos - 1];
						int num23 = (int)c45;
						num23 -= ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
						j = ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
						if (num23 >= 0 && num23 < j << 3 && ((int)this.program[pc + 5 + (num23 >> 3)] & 1 << (num23 & 7)) != 0)
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 5 + ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 5 + ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
					continue;
				case 65:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c46 = this.str[strpos - 1];
					int num24 = (int)c46;
					num24 -= ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					j = ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
					if (num24 < 0 || num24 >= j << 3 || ((int)this.program[pc + 5 + (num24 >> 3)] & 1 << (num24 & 7)) == 0)
					{
						pc += 5 + ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 66:
					if (strpos > 0)
					{
						char c47 = char.ToLower(this.str[strpos - 1]);
						int num25 = (int)c47;
						num25 -= ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
						j = ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
						if (num25 >= 0 && num25 < j << 3 && ((int)this.program[pc + 5 + (num25 >> 3)] & 1 << (num25 & 7)) != 0)
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 5 + ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 5 + ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
					continue;
				case 67:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c48 = char.ToLower(this.str[strpos - 1]);
					int num26 = (int)c48;
					num26 -= ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					j = ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
					if (num26 < 0 || num26 >= j << 3 || ((int)this.program[pc + 5 + (num26 >> 3)] & 1 << (num26 & 7)) == 0)
					{
						pc += 5 + ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 68:
					if (strpos < this.string_end)
					{
						char c49 = this.str[strpos];
						if (c49 != '\n')
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc++;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc++;
					continue;
				case 69:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c50 = this.str[strpos];
					if (c50 == '\n')
					{
						pc++;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 70:
					if (strpos > 0)
					{
						char c51 = this.str[strpos - 1];
						if (c51 != '\n')
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc++;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc++;
					continue;
				case 71:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c52 = this.str[strpos - 1];
					if (c52 == '\n')
					{
						pc++;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 72:
					if (strpos < this.string_end)
					{
						char c53 = this.str[strpos];
						strpos++;
						if (num != 0)
						{
							goto IL_3E1B;
						}
						pc++;
						continue;
					}
					else
					{
						if (num == 0)
						{
							return false;
						}
						pc++;
						continue;
					}
					break;
				case 73:
					goto IL_1C0E;
				case 74:
					if (strpos > 0)
					{
						char c54 = this.str[strpos - 1];
						strpos--;
						if (num != 0)
						{
							goto IL_3E1B;
						}
						pc++;
						continue;
					}
					else
					{
						if (num == 0)
						{
							return false;
						}
						pc++;
						continue;
					}
					break;
				case 75:
					goto IL_2FF7;
				case 76:
					if (strpos < this.string_end)
					{
						char c55 = this.str[strpos];
						if (char.IsDigit(c55))
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc++;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc++;
					continue;
				case 77:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c56 = this.str[strpos];
					if (!char.IsDigit(c56))
					{
						pc++;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 78:
					if (strpos > 0)
					{
						char c57 = this.str[strpos - 1];
						if (char.IsDigit(c57))
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc++;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc++;
					continue;
				case 79:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c58 = this.str[strpos - 1];
					if (!char.IsDigit(c58))
					{
						pc++;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 80:
					if (strpos < this.string_end)
					{
						char c59 = this.str[strpos];
						if (char.IsLetterOrDigit(c59) || char.GetUnicodeCategory(c59) == UnicodeCategory.ConnectorPunctuation)
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc++;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc++;
					continue;
				case 81:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c60 = this.str[strpos];
					if (!char.IsLetterOrDigit(c60) && char.GetUnicodeCategory(c60) != UnicodeCategory.ConnectorPunctuation)
					{
						pc++;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 82:
					if (strpos > 0)
					{
						char c61 = this.str[strpos - 1];
						if (char.IsLetterOrDigit(c61) || char.GetUnicodeCategory(c61) == UnicodeCategory.ConnectorPunctuation)
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc++;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc++;
					continue;
				case 83:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c62 = this.str[strpos - 1];
					if (!char.IsLetterOrDigit(c62) && char.GetUnicodeCategory(c62) != UnicodeCategory.ConnectorPunctuation)
					{
						pc++;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 84:
					if (strpos < this.string_end)
					{
						char c63 = this.str[strpos];
						if (char.IsWhiteSpace(c63))
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc++;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc++;
					continue;
				case 85:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c64 = this.str[strpos];
					if (!char.IsWhiteSpace(c64))
					{
						pc++;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 86:
					if (strpos > 0)
					{
						char c65 = this.str[strpos - 1];
						if (char.IsWhiteSpace(c65))
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc++;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc++;
					continue;
				case 87:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c66 = this.str[strpos - 1];
					if (!char.IsWhiteSpace(c66))
					{
						pc++;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 88:
					if (strpos < this.string_end)
					{
						char c67 = this.str[strpos];
						if (('a' <= c67 && c67 <= 'z') || ('A' <= c67 && c67 <= 'Z') || ('0' <= c67 && c67 <= '9') || c67 == '_')
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc++;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc++;
					continue;
				case 89:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c68 = this.str[strpos];
					if (('a' > c68 || c68 > 'z') && ('A' > c68 || c68 > 'Z') && ('0' > c68 || c68 > '9') && c68 != '_')
					{
						pc++;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 90:
					if (strpos > 0)
					{
						char c69 = this.str[strpos - 1];
						if (('a' <= c69 && c69 <= 'z') || ('A' <= c69 && c69 <= 'Z') || ('0' <= c69 && c69 <= '9') || c69 == '_')
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc++;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc++;
					continue;
				case 91:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c70 = this.str[strpos - 1];
					if (('a' > c70 || c70 > 'z') && ('A' > c70 || c70 > 'Z') && ('0' > c70 || c70 > '9') && c70 != '_')
					{
						pc++;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 92:
					if (strpos < this.string_end)
					{
						char c71 = this.str[strpos];
						if (c71 == ' ' || c71 == '\t' || c71 == '\n' || c71 == '\r' || c71 == '\f' || c71 == '\v')
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc++;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc++;
					continue;
				case 93:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c72 = this.str[strpos];
					if (c72 != ' ' && c72 != '\t' && c72 != '\n' && c72 != '\r' && c72 != '\f' && c72 != '\v')
					{
						pc++;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 94:
					if (strpos > 0)
					{
						char c73 = this.str[strpos - 1];
						if (c73 == ' ' || c73 == '\t' || c73 == '\n' || c73 == '\r' || c73 == '\f' || c73 == '\v')
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc++;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc++;
					continue;
				case 95:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c74 = this.str[strpos - 1];
					if (c74 != ' ' && c74 != '\t' && c74 != '\n' && c74 != '\r' && c74 != '\f' && c74 != '\v')
					{
						pc++;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 96:
					if (strpos < this.string_end)
					{
						char c75 = this.str[strpos];
						if (char.GetUnicodeCategory(c75) == (UnicodeCategory)this.program[pc + 1])
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 2;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 2;
					continue;
				case 97:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c76 = this.str[strpos];
					if (char.GetUnicodeCategory(c76) != (UnicodeCategory)this.program[pc + 1])
					{
						pc += 2;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 98:
					if (strpos > 0)
					{
						char c77 = this.str[strpos - 1];
						if (char.GetUnicodeCategory(c77) == (UnicodeCategory)this.program[pc + 1])
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 2;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 2;
					continue;
				case 99:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c78 = this.str[strpos - 1];
					if (char.GetUnicodeCategory(c78) != (UnicodeCategory)this.program[pc + 1])
					{
						pc += 2;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 124:
					if (strpos < this.string_end)
					{
						char c79 = this.str[strpos];
						if (('﻿' <= c79 && c79 <= '﻿') || ('￰' <= c79 && c79 <= '�'))
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc++;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc++;
					continue;
				case 125:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c80 = this.str[strpos];
					if (('﻿' > c80 || c80 > '﻿') && ('￰' > c80 || c80 > '�'))
					{
						pc++;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 126:
					if (strpos > 0)
					{
						char c81 = this.str[strpos - 1];
						if (('﻿' <= c81 && c81 <= '﻿') || ('￰' <= c81 && c81 <= '�'))
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc++;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc++;
					continue;
				case 127:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c82 = this.str[strpos - 1];
					if (('﻿' > c82 || c82 > '﻿') && ('￰' > c82 || c82 > '�'))
					{
						pc++;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 132:
					if (strpos < this.string_end)
					{
						char c83 = this.str[strpos];
						if (CategoryUtils.IsCategory((Category)this.program[pc + 1], c83))
						{
							strpos++;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 2;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 2;
					continue;
				case 133:
				{
					if (strpos >= this.string_end)
					{
						return false;
					}
					char c84 = this.str[strpos];
					if (!CategoryUtils.IsCategory((Category)this.program[pc + 1], c84))
					{
						pc += 2;
						if (num == 0 || pc + 1 == num)
						{
							strpos++;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 134:
					if (strpos > 0)
					{
						char c85 = this.str[strpos - 1];
						if (CategoryUtils.IsCategory((Category)this.program[pc + 1], c85))
						{
							strpos--;
							if (num != 0)
							{
								goto IL_3E1B;
							}
							pc += 2;
							continue;
						}
					}
					if (num == 0)
					{
						return false;
					}
					pc += 2;
					continue;
				case 135:
				{
					if (strpos <= 0)
					{
						return false;
					}
					char c86 = this.str[strpos - 1];
					if (!CategoryUtils.IsCategory((Category)this.program[pc + 1], c86))
					{
						pc += 2;
						if (num == 0 || pc + 1 == num)
						{
							strpos--;
							if (pc + 1 == num)
							{
								goto IL_3E1B;
							}
						}
						continue;
					}
					return false;
				}
				case 136:
					j = this.GetLastDefined((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					if (j < 0)
					{
						return false;
					}
					i = this.marks[j].Index;
					j = this.marks[j].Length;
					if (strpos + j > this.string_end)
					{
						return false;
					}
					num2 = i + j;
					while (i < num2)
					{
						if (this.str[strpos] != this.str[i])
						{
							return false;
						}
						strpos++;
						i++;
					}
					pc += 3;
					continue;
				case 137:
					j = this.GetLastDefined((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					if (j < 0)
					{
						return false;
					}
					i = this.marks[j].Index;
					j = this.marks[j].Length;
					if (strpos + j > this.string_end)
					{
						return false;
					}
					num2 = i + j;
					while (i < num2)
					{
						if (this.str[strpos] != this.str[i] && char.ToLower(this.str[strpos]) != char.ToLower(this.str[i]))
						{
							return false;
						}
						strpos++;
						i++;
					}
					pc += 3;
					continue;
				case 138:
				{
					j = this.GetLastDefined((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					if (j < 0)
					{
						return false;
					}
					i = this.marks[j].Index;
					j = this.marks[j].Length;
					if (strpos - j < 0)
					{
						return false;
					}
					int num27 = strpos - j;
					num2 = i + j;
					while (i < num2)
					{
						if (this.str[num27] != this.str[i])
						{
							return false;
						}
						i++;
						num27++;
					}
					strpos -= j;
					pc += 3;
					continue;
				}
				case 140:
					this.Open((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8, strpos);
					pc += 3;
					continue;
				case 141:
					this.Close((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8, strpos);
					pc += 3;
					continue;
				case 142:
				{
					int num28 = 0;
					if (!this.EvalByteCode(pc + 8, strpos, ref num28))
					{
						goto Block_58;
					}
					int gid = (int)this.program[pc + 1] | (int)this.program[pc + 2] << 8;
					int balance_gid = (int)this.program[pc + 3] | (int)this.program[pc + 4] << 8;
					bool capture = this.program[pc + 5] > 0;
					if (!this.Balance(gid, balance_gid, capture, strpos))
					{
						goto Block_59;
					}
					strpos = num28;
					pc += ((int)this.program[pc + 6] | (int)this.program[pc + 7] << 8);
					continue;
				}
				case 143:
					goto IL_BAD;
				case 144:
					if (this.GetLastDefined((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8) >= 0)
					{
						pc += 5;
					}
					else
					{
						pc += ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					}
					continue;
				case 145:
					pc += ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					continue;
				case 146:
				{
					int num29 = 0;
					if (this.EvalByteCode(pc + 3, strpos, ref num29))
					{
						pc += ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
						strpos = num29;
						continue;
					}
					return false;
				}
				case 147:
				{
					int num30 = 0;
					if (this.EvalByteCode(pc + 5, strpos, ref num30))
					{
						pc += ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					}
					else
					{
						pc += ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
					}
					continue;
				}
				case 148:
					num31 = 0;
					if (this.EvalByteCode(pc + 3, strpos, ref num31))
					{
						goto Block_498;
					}
					pc += ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					continue;
				case 149:
					num = pc + ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
					pc += 3;
					continue;
				case 150:
					goto IL_4FD;
				case 151:
					goto IL_71A;
				case 152:
				case 153:
					goto IL_37FA;
				case 154:
					goto IL_3884;
				case 155:
				case 156:
					goto IL_3C23;
				}
				break;
				IL_3E1B:
				pc = num;
				num = 0;
			}
			goto IL_3DE6;
			IL_2DF:
			strpos_result = strpos;
			return true;
			IL_4FD:
			int num32 = (int)this.program[pc + 1] | (int)this.program[pc + 2] << 8;
			int num33 = (int)this.program[pc + 3] | (int)this.program[pc + 4] << 8;
			int num34 = pc + 5;
			RxOp rxOp = (RxOp)(this.program[num34] & byte.MaxValue);
			bool flag = false;
			if ((rxOp == RxOp.String || rxOp == RxOp.StringIgnoreCase) && pc + num32 == num34 + 2 + (int)this.program[num34 + 1] + 1)
			{
				flag = true;
				if (RxInterpreter.trace_rx)
				{
					Console.WriteLine("  string anchor at {0}, offset {1}", num34, num33);
				}
			}
			pc += num32;
			if (this.program[pc] == 4)
			{
				if (strpos == 0)
				{
					int num35 = strpos;
					if (this.groups.Length > 1)
					{
						this.ResetGroups();
						this.marks[this.groups[0]].Start = strpos;
					}
					if (this.EvalByteCode(pc + 1, strpos, ref num35))
					{
						this.marks[this.groups[0]].Start = strpos;
						if (this.groups.Length > 1)
						{
							this.marks[this.groups[0]].End = num35;
						}
						strpos_result = num35;
						return true;
					}
				}
				return false;
			}
			num2 = this.string_end + 1;
			while (strpos < num2)
			{
				if (flag && (rxOp == RxOp.String || rxOp == RxOp.StringIgnoreCase))
				{
					int num36 = strpos;
					if (!this.EvalByteCode(num34, strpos + num33, ref num36))
					{
						strpos++;
						continue;
					}
				}
				int num37 = strpos;
				if (this.groups.Length > 1)
				{
					this.ResetGroups();
					this.marks[this.groups[0]].Start = strpos;
				}
				if (this.EvalByteCode(pc, strpos, ref num37))
				{
					this.marks[this.groups[0]].Start = strpos;
					if (this.groups.Length > 1)
					{
						this.marks[this.groups[0]].End = num37;
					}
					strpos_result = num37;
					return true;
				}
				strpos++;
			}
			return false;
			IL_71A:
			j = ((int)this.program[pc + 3] | (int)this.program[pc + 4] << 8);
			pc += ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
			while (strpos >= 0)
			{
				int num38 = strpos;
				if (this.groups.Length > 1)
				{
					this.ResetGroups();
					this.marks[this.groups[0]].Start = strpos;
				}
				if (this.EvalByteCode(pc, strpos, ref num38))
				{
					this.marks[this.groups[0]].Start = strpos;
					if (this.groups.Length > 1)
					{
						this.marks[this.groups[0]].End = num38;
					}
					strpos_result = num38;
					return true;
				}
				strpos--;
			}
			return false;
			Block_58:
			Block_59:
			return false;
			IL_BAD:
			goto IL_3E14;
			IL_1C0E:
			if (strpos < this.string_end)
			{
				char c87 = this.str[strpos];
			}
			return false;
			IL_2FF7:
			if (strpos > 0)
			{
				char c88 = this.str[strpos - 1];
			}
			return false;
			Block_498:
			strpos_result = num31;
			return true;
			IL_37FA:
			int num39 = 0;
			this.repeat = new RxInterpreter.RepeatContext(this.repeat, RxInterpreter.ReadInt(this.program, pc + 3), RxInterpreter.ReadInt(this.program, pc + 7), this.program[pc] == 153, pc + 11);
			int pc2 = pc + ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
			if (!this.EvalByteCode(pc2, strpos, ref num39))
			{
				this.repeat = this.repeat.Previous;
				return false;
			}
			strpos = num39;
			strpos_result = strpos;
			return true;
			IL_3884:
			RxInterpreter.RepeatContext repeatContext = this.repeat;
			int num40 = 0;
			if (this.deep == repeatContext)
			{
				goto IL_3E14;
			}
			i = repeatContext.Start;
			int count = repeatContext.Count;
			while (!repeatContext.IsMinimum)
			{
				repeatContext.Count++;
				repeatContext.Start = strpos;
				this.deep = repeatContext;
				if (!this.EvalByteCode(repeatContext.Expression, strpos, ref num40))
				{
					repeatContext.Start = i;
					repeatContext.Count = count;
					return false;
				}
				strpos = num40;
				if (this.deep != repeatContext)
				{
					goto IL_3E14;
				}
			}
			if (strpos == repeatContext.Start)
			{
				this.repeat = repeatContext.Previous;
				this.deep = null;
				if (this.EvalByteCode(pc + 1, strpos, ref num40))
				{
					strpos = num40;
					goto IL_3E14;
				}
				this.repeat = repeatContext;
				return false;
			}
			else
			{
				if (repeatContext.IsLazy)
				{
					for (;;)
					{
						this.repeat = repeatContext.Previous;
						this.deep = null;
						int cp = this.Checkpoint();
						if (this.EvalByteCode(pc + 1, strpos, ref num40))
						{
							break;
						}
						this.Backtrack(cp);
						this.repeat = repeatContext;
						if (repeatContext.IsMaximum)
						{
							goto Block_508;
						}
						repeatContext.Count++;
						repeatContext.Start = strpos;
						this.deep = repeatContext;
						if (!this.EvalByteCode(repeatContext.Expression, strpos, ref num40))
						{
							goto Block_509;
						}
						strpos = num40;
						if (this.deep != repeatContext)
						{
							goto Block_510;
						}
						if (strpos == repeatContext.Start)
						{
							goto Block_511;
						}
					}
					strpos = num40;
					goto IL_3E14;
					Block_508:
					return false;
					Block_509:
					repeatContext.Start = i;
					repeatContext.Count = count;
					return false;
					Block_510:
					goto IL_3E14;
					Block_511:
					return false;
				}
				int count2 = this.stack.Count;
				while (!repeatContext.IsMaximum)
				{
					int num41 = this.Checkpoint();
					int value = strpos;
					int start = repeatContext.Start;
					repeatContext.Count++;
					if (RxInterpreter.trace_rx)
					{
						Console.WriteLine("recurse with count {0}.", repeatContext.Count);
					}
					repeatContext.Start = strpos;
					this.deep = repeatContext;
					if (!this.EvalByteCode(repeatContext.Expression, strpos, ref num40))
					{
						repeatContext.Count--;
						repeatContext.Start = start;
						this.Backtrack(num41);
						break;
					}
					strpos = num40;
					if (this.deep != repeatContext)
					{
						this.stack.Count = count2;
						goto IL_3E14;
					}
					this.stack.Push(num41);
					this.stack.Push(value);
					if (strpos == repeatContext.Start)
					{
						break;
					}
				}
				if (RxInterpreter.trace_rx)
				{
					Console.WriteLine("matching tail: {0} pc={1}", strpos, pc + 1);
				}
				this.repeat = repeatContext.Previous;
				for (;;)
				{
					this.deep = null;
					if (this.EvalByteCode(pc + 1, strpos, ref num40))
					{
						break;
					}
					if (this.stack.Count == count2)
					{
						goto Block_518;
					}
					repeatContext.Count--;
					strpos = this.stack.Pop();
					this.Backtrack(this.stack.Pop());
					if (RxInterpreter.trace_rx)
					{
						Console.WriteLine("backtracking to {0} expr={1} pc={2}", strpos, repeatContext.Expression, pc);
					}
				}
				strpos = num40;
				this.stack.Count = count2;
				goto IL_3E14;
				Block_518:
				this.repeat = repeatContext;
				return false;
			}
			IL_3C23:
			bool flag2 = this.program[pc] == 156;
			int num42 = 0;
			int num43 = pc + ((int)this.program[pc + 1] | (int)this.program[pc + 2] << 8);
			i = RxInterpreter.ReadInt(this.program, pc + 3);
			num2 = RxInterpreter.ReadInt(this.program, pc + 7);
			j = 0;
			this.deep = null;
			while (j < i)
			{
				if (!this.EvalByteCode(pc + 11, strpos, ref num42))
				{
					return false;
				}
				strpos = num42;
				j++;
			}
			if (flag2)
			{
				for (;;)
				{
					int cp2 = this.Checkpoint();
					if (this.EvalByteCode(num43, strpos, ref num42))
					{
						break;
					}
					this.Backtrack(cp2);
					if (j >= num2)
					{
						return false;
					}
					if (!this.EvalByteCode(pc + 11, strpos, ref num42))
					{
						return false;
					}
					strpos = num42;
					j++;
				}
				strpos = num42;
			}
			else
			{
				int count3 = this.stack.Count;
				while (j < num2)
				{
					int num44 = this.Checkpoint();
					if (!this.EvalByteCode(pc + 11, strpos, ref num42))
					{
						this.Backtrack(num44);
						break;
					}
					this.stack.Push(num44);
					this.stack.Push(strpos);
					strpos = num42;
					j++;
				}
				if (num43 <= pc)
				{
					throw new Exception();
				}
				while (!this.EvalByteCode(num43, strpos, ref num42))
				{
					if (this.stack.Count == count3)
					{
						return false;
					}
					strpos = this.stack.Pop();
					this.Backtrack(this.stack.Pop());
					if (RxInterpreter.trace_rx)
					{
						Console.WriteLine("backtracking to: {0}", strpos);
					}
				}
				strpos = num42;
				this.stack.Count = count3;
			}
			goto IL_3E14;
			IL_3DE6:
			Console.WriteLine("evaluating: {0} at pc: {1}, strpos: {2}", (RxOp)this.program[pc], pc, strpos);
			throw new NotSupportedException();
			IL_3E14:
			strpos_result = strpos;
			return true;
		}

		internal struct IntStack
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

			private RxInterpreter.RepeatContext previous;

			private int count;

			public RepeatContext(RxInterpreter.RepeatContext previous, int min, int max, bool lazy, int expr_pc)
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

			public RxInterpreter.RepeatContext Previous
			{
				get
				{
					return this.previous;
				}
			}
		}
	}
}
