using System;

namespace System.Text.RegularExpressions
{
	internal class Disassembler
	{
		public static void DisassemblePattern(ushort[] image)
		{
			Disassembler.DisassembleBlock(image, 0, 0);
		}

		public static void DisassembleBlock(ushort[] image, int pc, int depth)
		{
			while (pc < image.Length)
			{
				OpCode opCode;
				OpFlags opFlags;
				PatternCompiler.DecodeOp(image[pc], out opCode, out opFlags);
				Console.Write(Disassembler.FormatAddress(pc) + ": ");
				Console.Write(new string(' ', depth * 2));
				Console.Write(Disassembler.DisassembleOp(image, pc));
				Console.WriteLine();
				int num;
				switch (opCode)
				{
				case OpCode.False:
				case OpCode.True:
				case OpCode.Until:
					num = 1;
					break;
				case OpCode.Position:
				case OpCode.Reference:
				case OpCode.Character:
				case OpCode.Category:
				case OpCode.NotCategory:
				case OpCode.In:
				case OpCode.Open:
				case OpCode.Close:
				case OpCode.Sub:
				case OpCode.Branch:
				case OpCode.Jump:
					num = 2;
					break;
				case OpCode.String:
					num = (int)(image[pc + 1] + 2);
					break;
				case OpCode.Range:
				case OpCode.Balance:
				case OpCode.IfDefined:
				case OpCode.Test:
				case OpCode.Anchor:
					num = 3;
					break;
				case OpCode.Set:
					num = (int)(image[pc + 2] + 3);
					break;
				case OpCode.BalanceStart:
					goto IL_F7;
				case OpCode.Repeat:
				case OpCode.FastRepeat:
				case OpCode.Info:
					num = 4;
					break;
				default:
					goto IL_F7;
				}
				IL_FE:
				pc += num;
				continue;
				IL_F7:
				num = 1;
				goto IL_FE;
			}
		}

		public static string DisassembleOp(ushort[] image, int pc)
		{
			OpCode opCode;
			OpFlags opFlags;
			PatternCompiler.DecodeOp(image[pc], out opCode, out opFlags);
			string text = opCode.ToString();
			if (opFlags != OpFlags.None)
			{
				text = text + "[" + opFlags.ToString("f") + "]";
			}
			switch (opCode)
			{
			case OpCode.Position:
				text = text + " /" + (Position)image[pc + 1];
				break;
			case OpCode.String:
				text = text + " '" + Disassembler.ReadString(image, pc + 1) + "'";
				break;
			case OpCode.Reference:
			case OpCode.Open:
			case OpCode.Close:
				text = text + " " + image[pc + 1];
				break;
			case OpCode.Character:
				text = text + " '" + Disassembler.FormatChar((char)image[pc + 1]) + "'";
				break;
			case OpCode.Category:
			case OpCode.NotCategory:
				text = text + " /" + (Category)image[pc + 1];
				break;
			case OpCode.Range:
				text = text + " '" + Disassembler.FormatChar((char)image[pc + 1]) + "', ";
				text = text + " '" + Disassembler.FormatChar((char)image[pc + 2]) + "'";
				break;
			case OpCode.Set:
				text = text + " " + Disassembler.FormatSet(image, pc + 1);
				break;
			case OpCode.In:
			case OpCode.Sub:
			case OpCode.Branch:
			case OpCode.Jump:
				text = text + " :" + Disassembler.FormatAddress(pc + (int)image[pc + 1]);
				break;
			case OpCode.Balance:
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					" ",
					image[pc + 1],
					" ",
					image[pc + 2]
				});
				break;
			}
			case OpCode.IfDefined:
			case OpCode.Anchor:
				text = text + " :" + Disassembler.FormatAddress(pc + (int)image[pc + 1]);
				text = text + " " + image[pc + 2];
				break;
			case OpCode.Test:
				text = text + " :" + Disassembler.FormatAddress(pc + (int)image[pc + 1]);
				text = text + ", :" + Disassembler.FormatAddress(pc + (int)image[pc + 2]);
				break;
			case OpCode.Repeat:
			case OpCode.FastRepeat:
			{
				text = text + " :" + Disassembler.FormatAddress(pc + (int)image[pc + 1]);
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					" (",
					image[pc + 2],
					", "
				});
				if (image[pc + 3] == 65535)
				{
					text += "Inf";
				}
				else
				{
					text += image[pc + 3];
				}
				text += ")";
				break;
			}
			case OpCode.Info:
			{
				text = text + " " + image[pc + 1];
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					" (",
					image[pc + 2],
					", ",
					image[pc + 3],
					")"
				});
				break;
			}
			}
			return text;
		}

		private static string ReadString(ushort[] image, int pc)
		{
			int num = (int)image[pc];
			char[] array = new char[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (char)image[pc + i + 1];
			}
			return new string(array);
		}

		private static string FormatAddress(int pc)
		{
			return pc.ToString("x4");
		}

		private static string FormatSet(ushort[] image, int pc)
		{
			int num = (int)image[pc++];
			int num2 = ((int)image[pc++] << 4) - 1;
			string str = "[";
			bool flag = false;
			char c = '\0';
			for (int i = 0; i <= num2; i++)
			{
				bool flag2 = ((int)image[pc + (i >> 4)] & 1 << (i & 15)) != 0;
				if (flag2 & !flag)
				{
					c = (char)(num + i);
					flag = true;
				}
				else if (flag & (!flag2 || i == num2))
				{
					char c2 = (char)(num + i - 1);
					str += Disassembler.FormatChar(c);
					if (c2 != c)
					{
						str = str + "-" + Disassembler.FormatChar(c2);
					}
					flag = false;
				}
			}
			return str + "]";
		}

		private static string FormatChar(char c)
		{
			if (c == '-' || c == ']')
			{
				return "\\" + c;
			}
			if (char.IsLetterOrDigit(c) || char.IsSymbol(c))
			{
				return c.ToString();
			}
			if (char.IsControl(c))
			{
				return "^" + ('@' + c);
			}
			string str = "\\u";
			int num = (int)c;
			return str + num.ToString("x4");
		}
	}
}
