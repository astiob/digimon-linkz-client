using System;
using System.Collections;
using System.Globalization;

namespace System.Text.RegularExpressions.Syntax
{
	internal class Parser
	{
		private string pattern;

		private int ptr;

		private ArrayList caps;

		private Hashtable refs;

		private int num_groups;

		private int gap;

		public Parser()
		{
			this.caps = new ArrayList();
			this.refs = new Hashtable();
		}

		public static int ParseDecimal(string str, ref int ptr)
		{
			return Parser.ParseNumber(str, ref ptr, 10, 1, int.MaxValue);
		}

		public static int ParseOctal(string str, ref int ptr)
		{
			return Parser.ParseNumber(str, ref ptr, 8, 1, 3);
		}

		public static int ParseHex(string str, ref int ptr, int digits)
		{
			return Parser.ParseNumber(str, ref ptr, 16, digits, digits);
		}

		public static int ParseNumber(string str, ref int ptr, int b, int min, int max)
		{
			int num = ptr;
			int num2 = 0;
			int num3 = 0;
			if (max < min)
			{
				max = int.MaxValue;
			}
			while (num3 < max && num < str.Length)
			{
				int num4 = Parser.ParseDigit(str[num++], b, num3);
				if (num4 < 0)
				{
					num--;
					break;
				}
				num2 = num2 * b + num4;
				num3++;
			}
			if (num3 < min)
			{
				return -1;
			}
			ptr = num;
			return num2;
		}

		public static string ParseName(string str, ref int ptr)
		{
			if (char.IsDigit(str[ptr]))
			{
				int num = Parser.ParseNumber(str, ref ptr, 10, 1, 0);
				if (num > 0)
				{
					return num.ToString();
				}
				return null;
			}
			else
			{
				int num2 = ptr;
				while (Parser.IsNameChar(str[ptr]))
				{
					ptr++;
				}
				if (ptr - num2 > 0)
				{
					return str.Substring(num2, ptr - num2);
				}
				return null;
			}
		}

		public static string Escape(string str)
		{
			string text = string.Empty;
			int i = 0;
			while (i < str.Length)
			{
				char c = str[i];
				char c2 = c;
				switch (c2)
				{
				case ' ':
				case '#':
				case '$':
				case '(':
				case ')':
				case '*':
				case '+':
				case '.':
					goto IL_AF;
				default:
					switch (c2)
					{
					case '\t':
						text += "\\t";
						break;
					case '\n':
						text += "\\n";
						break;
					default:
						switch (c2)
						{
						case '[':
						case '\\':
						case '^':
							goto IL_AF;
						default:
							if (c2 == '{' || c2 == '|' || c2 == '?')
							{
								goto IL_AF;
							}
							text += c;
							break;
						}
						break;
					case '\f':
						text += "\\f";
						break;
					case '\r':
						text += "\\r";
						break;
					}
					break;
				}
				IL_11C:
				i++;
				continue;
				IL_AF:
				text = text + "\\" + c;
				goto IL_11C;
			}
			return text;
		}

		public static string Unescape(string str)
		{
			if (str.IndexOf('\\') == -1)
			{
				return str;
			}
			return new Parser().ParseString(str);
		}

		public RegularExpression ParseRegularExpression(string pattern, RegexOptions options)
		{
			this.pattern = pattern;
			this.ptr = 0;
			this.caps.Clear();
			this.refs.Clear();
			this.num_groups = 0;
			RegularExpression result;
			try
			{
				RegularExpression regularExpression = new RegularExpression();
				this.ParseGroup(regularExpression, options, null);
				this.ResolveReferences();
				regularExpression.GroupCount = this.num_groups;
				result = regularExpression;
			}
			catch (IndexOutOfRangeException)
			{
				throw this.NewParseException("Unexpected end of pattern.");
			}
			return result;
		}

		public int GetMapping(Hashtable mapping)
		{
			int count = this.caps.Count;
			mapping.Add("0", 0);
			for (int i = 0; i < count; i++)
			{
				CapturingGroup capturingGroup = (CapturingGroup)this.caps[i];
				string key = (capturingGroup.Name == null) ? capturingGroup.Index.ToString() : capturingGroup.Name;
				if (mapping.Contains(key))
				{
					if ((int)mapping[key] != capturingGroup.Index)
					{
						throw new SystemException("invalid state");
					}
				}
				else
				{
					mapping.Add(key, capturingGroup.Index);
				}
			}
			return this.gap;
		}

		private void ParseGroup(Group group, RegexOptions options, Assertion assertion)
		{
			bool flag = group is RegularExpression;
			Alternation alternation = null;
			string text = null;
			Group group2 = new Group();
			Expression expression = null;
			bool flag2 = false;
			do
			{
				this.ConsumeWhitespace(Parser.IsIgnorePatternWhitespace(options));
				if (this.ptr >= this.pattern.Length)
				{
					break;
				}
				char c = this.pattern[this.ptr++];
				char c2 = c;
				switch (c2)
				{
				case '$':
				{
					Position pos = (!Parser.IsMultiline(options)) ? Position.End : Position.EndOfLine;
					expression = new PositionAssertion(pos);
					break;
				}
				default:
					switch (c2)
					{
					case '[':
						expression = this.ParseCharacterClass(options);
						break;
					case '\\':
					{
						int num = this.ParseEscape();
						if (num >= 0)
						{
							c = (char)num;
						}
						else
						{
							expression = this.ParseSpecial(options);
							if (expression == null)
							{
								c = this.pattern[this.ptr++];
							}
						}
						break;
					}
					default:
						if (c2 == '?')
						{
							goto IL_25F;
						}
						if (c2 == '|')
						{
							if (text != null)
							{
								group2.AppendExpression(new Literal(text, Parser.IsIgnoreCase(options)));
								text = null;
							}
							if (assertion != null)
							{
								if (assertion.TrueExpression == null)
								{
									assertion.TrueExpression = group2;
								}
								else
								{
									if (assertion.FalseExpression != null)
									{
										goto IL_230;
									}
									assertion.FalseExpression = group2;
								}
							}
							else
							{
								if (alternation == null)
								{
									alternation = new Alternation();
								}
								alternation.AddAlternative(group2);
							}
							group2 = new Group();
							continue;
						}
						break;
					case '^':
					{
						Position pos2 = (!Parser.IsMultiline(options)) ? Position.Start : Position.StartOfLine;
						expression = new PositionAssertion(pos2);
						break;
					}
					}
					break;
				case '(':
				{
					bool flag3 = Parser.IsIgnoreCase(options);
					expression = this.ParseGroupingConstruct(ref options);
					if (expression == null)
					{
						if (text != null && Parser.IsIgnoreCase(options) != flag3)
						{
							group2.AppendExpression(new Literal(text, Parser.IsIgnoreCase(options)));
							text = null;
						}
						continue;
					}
					break;
				}
				case ')':
					goto IL_1DA;
				case '*':
				case '+':
					goto IL_25F;
				case '.':
				{
					Category cat = (!Parser.IsSingleline(options)) ? Category.Any : Category.AnySingleline;
					expression = new CharacterClass(cat, false);
					break;
				}
				}
				this.ConsumeWhitespace(Parser.IsIgnorePatternWhitespace(options));
				if (this.ptr < this.pattern.Length)
				{
					char c3 = this.pattern[this.ptr];
					int min = 0;
					int max = 0;
					bool lazy = false;
					bool flag4 = false;
					if (c3 == '?' || c3 == '*' || c3 == '+')
					{
						this.ptr++;
						flag4 = true;
						c2 = c3;
						if (c2 != '*')
						{
							if (c2 != '+')
							{
								if (c2 == '?')
								{
									min = 0;
									max = 1;
								}
							}
							else
							{
								min = 1;
								max = int.MaxValue;
							}
						}
						else
						{
							min = 0;
							max = int.MaxValue;
						}
					}
					else if (c3 == '{' && this.ptr + 1 < this.pattern.Length)
					{
						int num2 = this.ptr;
						this.ptr++;
						flag4 = this.ParseRepetitionBounds(out min, out max, options);
						if (!flag4)
						{
							this.ptr = num2;
						}
					}
					if (flag4)
					{
						this.ConsumeWhitespace(Parser.IsIgnorePatternWhitespace(options));
						if (this.ptr < this.pattern.Length && this.pattern[this.ptr] == '?')
						{
							this.ptr++;
							lazy = true;
						}
						Repetition repetition = new Repetition(min, max, lazy);
						if (expression == null)
						{
							repetition.Expression = new Literal(c.ToString(), Parser.IsIgnoreCase(options));
						}
						else
						{
							repetition.Expression = expression;
						}
						expression = repetition;
					}
				}
				if (expression == null)
				{
					if (text == null)
					{
						text = string.Empty;
					}
					text += c;
				}
				else
				{
					if (text != null)
					{
						group2.AppendExpression(new Literal(text, Parser.IsIgnoreCase(options)));
						text = null;
					}
					group2.AppendExpression(expression);
					expression = null;
				}
			}
			while (!flag || this.ptr < this.pattern.Length);
			goto IL_484;
			IL_1DA:
			flag2 = true;
			goto IL_484;
			IL_230:
			throw this.NewParseException("Too many | in (?()|).");
			IL_25F:
			throw this.NewParseException("Bad quantifier.");
			IL_484:
			if (flag && flag2)
			{
				throw this.NewParseException("Too many )'s.");
			}
			if (!flag && !flag2)
			{
				throw this.NewParseException("Not enough )'s.");
			}
			if (text != null)
			{
				group2.AppendExpression(new Literal(text, Parser.IsIgnoreCase(options)));
			}
			if (assertion != null)
			{
				if (assertion.TrueExpression == null)
				{
					assertion.TrueExpression = group2;
				}
				else
				{
					assertion.FalseExpression = group2;
				}
				group.AppendExpression(assertion);
			}
			else if (alternation != null)
			{
				alternation.AddAlternative(group2);
				group.AppendExpression(alternation);
			}
			else
			{
				group.AppendExpression(group2);
			}
		}

		private Expression ParseGroupingConstruct(ref RegexOptions options)
		{
			if (this.pattern[this.ptr] != '?')
			{
				Group group;
				if (Parser.IsExplicitCapture(options))
				{
					group = new Group();
				}
				else
				{
					group = new CapturingGroup();
					this.caps.Add(group);
				}
				this.ParseGroup(group, options, null);
				return group;
			}
			this.ptr++;
			char c = this.pattern[this.ptr];
			switch (c)
			{
			case '!':
				break;
			default:
			{
				switch (c)
				{
				case 'i':
				case 'm':
				case 'n':
					break;
				default:
					switch (c)
					{
					case ':':
					{
						this.ptr++;
						Group group2 = new Group();
						this.ParseGroup(group2, options, null);
						return group2;
					}
					default:
						if (c != '-' && c != 's' && c != 'x')
						{
							throw this.NewParseException("Bad grouping construct.");
						}
						break;
					case '<':
					case '=':
						goto IL_1E5;
					case '>':
					{
						this.ptr++;
						Group group3 = new NonBacktrackingGroup();
						this.ParseGroup(group3, options, null);
						return group3;
					}
					}
					break;
				}
				RegexOptions regexOptions = options;
				this.ParseOptions(ref regexOptions, false);
				if (this.pattern[this.ptr] == '-')
				{
					this.ptr++;
					this.ParseOptions(ref regexOptions, true);
				}
				if (this.pattern[this.ptr] == ':')
				{
					this.ptr++;
					Group group4 = new Group();
					this.ParseGroup(group4, regexOptions, null);
					return group4;
				}
				if (this.pattern[this.ptr] == ')')
				{
					this.ptr++;
					options = regexOptions;
					return null;
				}
				throw this.NewParseException("Bad options");
			}
			case '#':
				this.ptr++;
				while (this.pattern[this.ptr++] != ')')
				{
					if (this.ptr >= this.pattern.Length)
					{
						throw this.NewParseException("Unterminated (?#...) comment.");
					}
				}
				return null;
			case '\'':
				goto IL_21C;
			case '(':
			{
				this.ptr++;
				int num = this.ptr;
				string text = this.ParseName();
				Assertion assertion;
				if (text == null || this.pattern[this.ptr] != ')')
				{
					this.ptr = num;
					ExpressionAssertion expressionAssertion = new ExpressionAssertion();
					if (this.pattern[this.ptr] == '?')
					{
						this.ptr++;
						if (!this.ParseAssertionType(expressionAssertion))
						{
							throw this.NewParseException("Bad conditional.");
						}
					}
					else
					{
						expressionAssertion.Negate = false;
						expressionAssertion.Reverse = false;
					}
					Group group5 = new Group();
					this.ParseGroup(group5, options, null);
					expressionAssertion.TestExpression = group5;
					assertion = expressionAssertion;
				}
				else
				{
					this.ptr++;
					assertion = new CaptureAssertion(new Literal(text, Parser.IsIgnoreCase(options)));
					this.refs.Add(assertion, text);
				}
				Group group6 = new Group();
				this.ParseGroup(group6, options, assertion);
				return group6;
			}
			}
			IL_1E5:
			ExpressionAssertion expressionAssertion2 = new ExpressionAssertion();
			if (this.ParseAssertionType(expressionAssertion2))
			{
				Group group7 = new Group();
				this.ParseGroup(group7, options, null);
				expressionAssertion2.TestExpression = group7;
				return expressionAssertion2;
			}
			IL_21C:
			char c2;
			if (this.pattern[this.ptr] == '<')
			{
				c2 = '>';
			}
			else
			{
				c2 = '\'';
			}
			this.ptr++;
			string text2 = this.ParseName();
			if (this.pattern[this.ptr] == c2)
			{
				if (text2 == null)
				{
					throw this.NewParseException("Bad group name.");
				}
				this.ptr++;
				CapturingGroup capturingGroup = new CapturingGroup();
				capturingGroup.Name = text2;
				this.caps.Add(capturingGroup);
				this.ParseGroup(capturingGroup, options, null);
				return capturingGroup;
			}
			else
			{
				if (this.pattern[this.ptr] != '-')
				{
					throw this.NewParseException("Bad group name.");
				}
				this.ptr++;
				string text3 = this.ParseName();
				if (text3 == null || this.pattern[this.ptr] != c2)
				{
					throw this.NewParseException("Bad balancing group name.");
				}
				this.ptr++;
				BalancingGroup balancingGroup = new BalancingGroup();
				balancingGroup.Name = text2;
				if (balancingGroup.IsNamed)
				{
					this.caps.Add(balancingGroup);
				}
				this.refs.Add(balancingGroup, text3);
				this.ParseGroup(balancingGroup, options, null);
				return balancingGroup;
			}
		}

		private bool ParseAssertionType(ExpressionAssertion assertion)
		{
			if (this.pattern[this.ptr] == '<')
			{
				char c = this.pattern[this.ptr + 1];
				if (c != '!')
				{
					if (c != '=')
					{
						return false;
					}
					assertion.Negate = false;
				}
				else
				{
					assertion.Negate = true;
				}
				assertion.Reverse = true;
				this.ptr += 2;
			}
			else
			{
				char c = this.pattern[this.ptr];
				if (c != '!')
				{
					if (c != '=')
					{
						return false;
					}
					assertion.Negate = false;
				}
				else
				{
					assertion.Negate = true;
				}
				assertion.Reverse = false;
				this.ptr++;
			}
			return true;
		}

		private void ParseOptions(ref RegexOptions options, bool negate)
		{
			for (;;)
			{
				char c = this.pattern[this.ptr];
				switch (c)
				{
				case 'i':
					if (negate)
					{
						options &= ~RegexOptions.IgnoreCase;
					}
					else
					{
						options |= RegexOptions.IgnoreCase;
					}
					break;
				default:
					if (c != 's')
					{
						if (c != 'x')
						{
							return;
						}
						if (negate)
						{
							options &= ~RegexOptions.IgnorePatternWhitespace;
						}
						else
						{
							options |= RegexOptions.IgnorePatternWhitespace;
						}
					}
					else if (negate)
					{
						options &= ~RegexOptions.Singleline;
					}
					else
					{
						options |= RegexOptions.Singleline;
					}
					break;
				case 'm':
					if (negate)
					{
						options &= ~RegexOptions.Multiline;
					}
					else
					{
						options |= RegexOptions.Multiline;
					}
					break;
				case 'n':
					if (negate)
					{
						options &= ~RegexOptions.ExplicitCapture;
					}
					else
					{
						options |= RegexOptions.ExplicitCapture;
					}
					break;
				}
				this.ptr++;
			}
		}

		private Expression ParseCharacterClass(RegexOptions options)
		{
			bool negate = false;
			if (this.pattern[this.ptr] == '^')
			{
				negate = true;
				this.ptr++;
			}
			bool flag = Parser.IsECMAScript(options);
			CharacterClass characterClass = new CharacterClass(negate, Parser.IsIgnoreCase(options));
			if (this.pattern[this.ptr] == ']')
			{
				characterClass.AddCharacter(']');
				this.ptr++;
			}
			int num = -1;
			bool flag2 = false;
			bool flag3 = false;
			while (this.ptr < this.pattern.Length)
			{
				int num2 = (int)this.pattern[this.ptr++];
				if (num2 == 93)
				{
					flag3 = true;
					break;
				}
				if (num2 == 45 && num >= 0 && !flag2)
				{
					flag2 = true;
				}
				else
				{
					if (num2 == 92)
					{
						num2 = this.ParseEscape();
						if (num2 < 0)
						{
							num2 = (int)this.pattern[this.ptr++];
							int num3 = num2;
							switch (num3)
							{
							case 80:
								goto IL_1D1;
							default:
								switch (num3)
								{
								case 112:
									goto IL_1D1;
								default:
									switch (num3)
									{
									case 98:
										num2 = 8;
										goto IL_212;
									default:
										if (num3 != 68)
										{
											if (num3 != 87 && num3 != 119)
											{
												goto IL_212;
											}
											characterClass.AddCategory((!flag) ? Category.Word : Category.EcmaWord, num2 == 87);
											goto IL_1EC;
										}
										break;
									case 100:
										break;
									}
									characterClass.AddCategory((!flag) ? Category.Digit : Category.EcmaDigit, num2 == 68);
									break;
								case 115:
									goto IL_1B3;
								}
								break;
							case 83:
								goto IL_1B3;
							}
							IL_1EC:
							if (flag2)
							{
								throw this.NewParseException("character range cannot have category \\" + num2);
							}
							num = -1;
							continue;
							IL_1B3:
							characterClass.AddCategory((!flag) ? Category.WhiteSpace : Category.EcmaWhiteSpace, num2 == 83);
							goto IL_1EC;
							IL_1D1:
							characterClass.AddCategory(this.ParseUnicodeCategory(), num2 == 80);
							goto IL_1EC;
						}
					}
					IL_212:
					if (flag2)
					{
						if (num2 < num)
						{
							throw this.NewParseException(string.Concat(new object[]
							{
								"[",
								num,
								"-",
								num2,
								"] range in reverse order."
							}));
						}
						characterClass.AddRange((char)num, (char)num2);
						num = -1;
						flag2 = false;
					}
					else
					{
						characterClass.AddCharacter((char)num2);
						num = num2;
					}
				}
			}
			if (!flag3)
			{
				throw this.NewParseException("Unterminated [] set.");
			}
			if (flag2)
			{
				characterClass.AddCharacter('-');
			}
			return characterClass;
		}

		private bool ParseRepetitionBounds(out int min, out int max, RegexOptions options)
		{
			min = (max = 0);
			this.ConsumeWhitespace(Parser.IsIgnorePatternWhitespace(options));
			int num;
			if (this.pattern[this.ptr] == ',')
			{
				num = -1;
			}
			else
			{
				num = this.ParseNumber(10, 1, 0);
				this.ConsumeWhitespace(Parser.IsIgnorePatternWhitespace(options));
			}
			char c = this.pattern[this.ptr++];
			int num2;
			if (c != ',')
			{
				if (c != '}')
				{
					return false;
				}
				num2 = num;
			}
			else
			{
				this.ConsumeWhitespace(Parser.IsIgnorePatternWhitespace(options));
				num2 = this.ParseNumber(10, 1, 0);
				this.ConsumeWhitespace(Parser.IsIgnorePatternWhitespace(options));
				if (this.pattern[this.ptr++] != '}')
				{
					return false;
				}
			}
			if (num > 2147483647 || num2 > 2147483647)
			{
				throw this.NewParseException("Illegal {x, y} - maximum of 2147483647.");
			}
			if (num2 >= 0 && num2 < num)
			{
				throw this.NewParseException("Illegal {x, y} with x > y.");
			}
			min = num;
			if (num2 > 0)
			{
				max = num2;
			}
			else
			{
				max = int.MaxValue;
			}
			return true;
		}

		private Category ParseUnicodeCategory()
		{
			if (this.pattern[this.ptr++] != '{')
			{
				throw this.NewParseException("Incomplete \\p{X} character escape.");
			}
			string text = Parser.ParseName(this.pattern, ref this.ptr);
			if (text == null)
			{
				throw this.NewParseException("Incomplete \\p{X} character escape.");
			}
			Category category = CategoryUtils.CategoryFromName(text);
			if (category == Category.None)
			{
				throw this.NewParseException("Unknown property '" + text + "'.");
			}
			if (this.pattern[this.ptr++] != '}')
			{
				throw this.NewParseException("Incomplete \\p{X} character escape.");
			}
			return category;
		}

		private Expression ParseSpecial(RegexOptions options)
		{
			int num = this.ptr;
			bool flag = Parser.IsECMAScript(options);
			char c = this.pattern[this.ptr++];
			Expression expression;
			switch (c)
			{
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
			{
				this.ptr--;
				int num2 = this.ParseNumber(10, 1, 0);
				if (num2 < 0)
				{
					this.ptr = num;
					return null;
				}
				Reference reference = new BackslashNumber(Parser.IsIgnoreCase(options), flag);
				this.refs.Add(reference, num2.ToString());
				expression = reference;
				break;
			}
			default:
				switch (c)
				{
				case 'P':
					expression = new CharacterClass(this.ParseUnicodeCategory(), true);
					break;
				default:
					switch (c)
					{
					case 'W':
						expression = new CharacterClass((!flag) ? Category.Word : Category.EcmaWord, true);
						break;
					default:
						switch (c)
						{
						case 'p':
							expression = new CharacterClass(this.ParseUnicodeCategory(), false);
							break;
						default:
							switch (c)
							{
							case 'w':
								expression = new CharacterClass((!flag) ? Category.Word : Category.EcmaWord, false);
								break;
							default:
								switch (c)
								{
								case 'b':
									expression = new PositionAssertion(Position.Boundary);
									break;
								default:
									if (c != 'k')
									{
										expression = null;
									}
									else
									{
										char c2 = this.pattern[this.ptr++];
										if (c2 == '<')
										{
											c2 = '>';
										}
										else if (c2 != '\'')
										{
											throw this.NewParseException("Malformed \\k<...> named backreference.");
										}
										string text = this.ParseName();
										if (text == null || this.pattern[this.ptr] != c2)
										{
											throw this.NewParseException("Malformed \\k<...> named backreference.");
										}
										this.ptr++;
										Reference reference2 = new Reference(Parser.IsIgnoreCase(options));
										this.refs.Add(reference2, text);
										expression = reference2;
									}
									break;
								case 'd':
									expression = new CharacterClass((!flag) ? Category.Digit : Category.EcmaDigit, false);
									break;
								}
								break;
							case 'z':
								expression = new PositionAssertion(Position.EndOfString);
								break;
							}
							break;
						case 's':
							expression = new CharacterClass((!flag) ? Category.WhiteSpace : Category.EcmaWhiteSpace, false);
							break;
						}
						break;
					case 'Z':
						expression = new PositionAssertion(Position.End);
						break;
					}
					break;
				case 'S':
					expression = new CharacterClass((!flag) ? Category.WhiteSpace : Category.EcmaWhiteSpace, true);
					break;
				}
				break;
			case 'A':
				expression = new PositionAssertion(Position.StartOfString);
				break;
			case 'B':
				expression = new PositionAssertion(Position.NonBoundary);
				break;
			case 'D':
				expression = new CharacterClass((!flag) ? Category.Digit : Category.EcmaDigit, true);
				break;
			case 'G':
				expression = new PositionAssertion(Position.StartOfScan);
				break;
			}
			if (expression == null)
			{
				this.ptr = num;
			}
			return expression;
		}

		private int ParseEscape()
		{
			int num = this.ptr;
			if (num >= this.pattern.Length)
			{
				throw new ArgumentException(string.Format("Parsing \"{0}\" - Illegal \\ at end of pattern.", this.pattern), this.pattern);
			}
			char c = this.pattern[this.ptr++];
			switch (c)
			{
			case 'n':
				return 10;
			default:
				switch (c)
				{
				case 'a':
					return 7;
				default:
					if (c != '0')
					{
						if (c != '\\')
						{
							this.ptr = num;
							return -1;
						}
						return 92;
					}
					else
					{
						this.ptr--;
						int num2 = this.ptr;
						int num3 = Parser.ParseOctal(this.pattern, ref this.ptr);
						if (num3 == -1 && num2 == this.ptr)
						{
							return 0;
						}
						return num3;
					}
					break;
				case 'c':
				{
					int num4 = (int)this.pattern[this.ptr++];
					if (num4 >= 64 && num4 <= 95)
					{
						return num4 - 64;
					}
					throw this.NewParseException("Unrecognized control character.");
				}
				case 'e':
					return 27;
				case 'f':
					return 12;
				}
				break;
			case 'r':
				return 13;
			case 't':
				return 9;
			case 'u':
			{
				int num4 = Parser.ParseHex(this.pattern, ref this.ptr, 4);
				if (num4 < 0)
				{
					throw this.NewParseException("Insufficient hex digits");
				}
				return num4;
			}
			case 'v':
				return 11;
			case 'x':
			{
				int num4 = Parser.ParseHex(this.pattern, ref this.ptr, 2);
				if (num4 < 0)
				{
					throw this.NewParseException("Insufficient hex digits");
				}
				return num4;
			}
			}
		}

		private string ParseName()
		{
			return Parser.ParseName(this.pattern, ref this.ptr);
		}

		private static bool IsNameChar(char c)
		{
			UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
			return unicodeCategory != UnicodeCategory.ModifierLetter && (unicodeCategory == UnicodeCategory.ConnectorPunctuation || char.IsLetterOrDigit(c));
		}

		private int ParseNumber(int b, int min, int max)
		{
			return Parser.ParseNumber(this.pattern, ref this.ptr, b, min, max);
		}

		private static int ParseDigit(char c, int b, int n)
		{
			switch (b)
			{
			case 8:
				if (c >= '0' && c <= '7')
				{
					return (int)(c - '0');
				}
				return -1;
			default:
				if (b != 16)
				{
					return -1;
				}
				if (c >= '0' && c <= '9')
				{
					return (int)(c - '0');
				}
				if (c >= 'a' && c <= 'f')
				{
					return (int)('\n' + c - 'a');
				}
				if (c >= 'A' && c <= 'F')
				{
					return (int)('\n' + c - 'A');
				}
				return -1;
			case 10:
				if (c >= '0' && c <= '9')
				{
					return (int)(c - '0');
				}
				return -1;
			}
		}

		private void ConsumeWhitespace(bool ignore)
		{
			while (this.ptr < this.pattern.Length)
			{
				if (this.pattern[this.ptr] == '(')
				{
					if (this.ptr + 3 >= this.pattern.Length)
					{
						return;
					}
					if (this.pattern[this.ptr + 1] != '?' || this.pattern[this.ptr + 2] != '#')
					{
						return;
					}
					this.ptr += 3;
					while (this.ptr < this.pattern.Length && this.pattern[this.ptr++] != ')')
					{
					}
				}
				else if (ignore && this.pattern[this.ptr] == '#')
				{
					while (this.ptr < this.pattern.Length && this.pattern[this.ptr++] != '\n')
					{
					}
				}
				else
				{
					if (!ignore || !char.IsWhiteSpace(this.pattern[this.ptr]))
					{
						return;
					}
					while (this.ptr < this.pattern.Length && char.IsWhiteSpace(this.pattern[this.ptr]))
					{
						this.ptr++;
					}
				}
			}
		}

		private string ParseString(string pattern)
		{
			this.pattern = pattern;
			this.ptr = 0;
			StringBuilder stringBuilder = new StringBuilder(pattern.Length);
			while (this.ptr < pattern.Length)
			{
				int num = (int)pattern[this.ptr++];
				if (num == 92)
				{
					num = this.ParseEscape();
					if (num < 0)
					{
						num = (int)pattern[this.ptr++];
						if (num == 98)
						{
							num = 8;
						}
					}
				}
				stringBuilder.Append((char)num);
			}
			return stringBuilder.ToString();
		}

		private void ResolveReferences()
		{
			int num = 1;
			Hashtable hashtable = new Hashtable();
			ArrayList arrayList = null;
			foreach (object obj in this.caps)
			{
				CapturingGroup capturingGroup = (CapturingGroup)obj;
				if (capturingGroup.Name == null)
				{
					hashtable.Add(num.ToString(), capturingGroup);
					capturingGroup.Index = num++;
					this.num_groups++;
				}
			}
			foreach (object obj2 in this.caps)
			{
				CapturingGroup capturingGroup2 = (CapturingGroup)obj2;
				if (capturingGroup2.Name != null)
				{
					if (hashtable.Contains(capturingGroup2.Name))
					{
						CapturingGroup capturingGroup3 = (CapturingGroup)hashtable[capturingGroup2.Name];
						capturingGroup2.Index = capturingGroup3.Index;
						if (capturingGroup2.Index == num)
						{
							num++;
						}
						else if (capturingGroup2.Index > num)
						{
							arrayList.Add(capturingGroup2);
						}
					}
					else
					{
						if (char.IsDigit(capturingGroup2.Name[0]))
						{
							int num2 = 0;
							int num3 = Parser.ParseDecimal(capturingGroup2.Name, ref num2);
							if (num2 == capturingGroup2.Name.Length)
							{
								capturingGroup2.Index = num3;
								hashtable.Add(capturingGroup2.Name, capturingGroup2);
								this.num_groups++;
								if (num3 == num)
								{
									num++;
								}
								else
								{
									if (arrayList == null)
									{
										arrayList = new ArrayList(4);
									}
									arrayList.Add(capturingGroup2);
								}
								continue;
							}
						}
						string key = num.ToString();
						while (hashtable.Contains(key))
						{
							int num4;
							num = (num4 = num + 1);
							key = num4.ToString();
						}
						hashtable.Add(key, capturingGroup2);
						hashtable.Add(capturingGroup2.Name, capturingGroup2);
						capturingGroup2.Index = num++;
						this.num_groups++;
					}
				}
			}
			this.gap = num;
			if (arrayList != null)
			{
				this.HandleExplicitNumericGroups(arrayList);
			}
			foreach (object obj3 in this.refs.Keys)
			{
				Expression expression = (Expression)obj3;
				string text = (string)this.refs[expression];
				if (!hashtable.Contains(text))
				{
					if (!(expression is CaptureAssertion) || char.IsDigit(text[0]))
					{
						BackslashNumber backslashNumber = expression as BackslashNumber;
						if (backslashNumber == null || !backslashNumber.ResolveReference(text, hashtable))
						{
							throw this.NewParseException("Reference to undefined group " + ((!char.IsDigit(text[0])) ? "name " : "number ") + text);
						}
					}
				}
				else
				{
					CapturingGroup capturingGroup4 = (CapturingGroup)hashtable[text];
					if (expression is Reference)
					{
						((Reference)expression).CapturingGroup = capturingGroup4;
					}
					else if (expression is CaptureAssertion)
					{
						((CaptureAssertion)expression).CapturingGroup = capturingGroup4;
					}
					else if (expression is BalancingGroup)
					{
						((BalancingGroup)expression).Balance = capturingGroup4;
					}
				}
			}
		}

		private void HandleExplicitNumericGroups(ArrayList explicit_numeric_groups)
		{
			int num = this.gap;
			int i = 0;
			int count = explicit_numeric_groups.Count;
			explicit_numeric_groups.Sort();
			while (i < count)
			{
				CapturingGroup capturingGroup = (CapturingGroup)explicit_numeric_groups[i];
				if (capturingGroup.Index > num)
				{
					break;
				}
				if (capturingGroup.Index == num)
				{
					num++;
				}
				i++;
			}
			this.gap = num;
			int num2 = num;
			while (i < count)
			{
				CapturingGroup capturingGroup2 = (CapturingGroup)explicit_numeric_groups[i];
				if (capturingGroup2.Index == num2)
				{
					capturingGroup2.Index = num - 1;
				}
				else
				{
					num2 = capturingGroup2.Index;
					capturingGroup2.Index = num++;
				}
				i++;
			}
		}

		private static bool IsIgnoreCase(RegexOptions options)
		{
			return (options & RegexOptions.IgnoreCase) != RegexOptions.None;
		}

		private static bool IsMultiline(RegexOptions options)
		{
			return (options & RegexOptions.Multiline) != RegexOptions.None;
		}

		private static bool IsExplicitCapture(RegexOptions options)
		{
			return (options & RegexOptions.ExplicitCapture) != RegexOptions.None;
		}

		private static bool IsSingleline(RegexOptions options)
		{
			return (options & RegexOptions.Singleline) != RegexOptions.None;
		}

		private static bool IsIgnorePatternWhitespace(RegexOptions options)
		{
			return (options & RegexOptions.IgnorePatternWhitespace) != RegexOptions.None;
		}

		private static bool IsECMAScript(RegexOptions options)
		{
			return (options & RegexOptions.ECMAScript) != RegexOptions.None;
		}

		private ArgumentException NewParseException(string msg)
		{
			msg = "parsing \"" + this.pattern + "\" - " + msg;
			return new ArgumentException(msg, this.pattern);
		}
	}
}
