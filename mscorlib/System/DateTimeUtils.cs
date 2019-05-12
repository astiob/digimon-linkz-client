using System;
using System.Globalization;
using System.Text;

namespace System
{
	internal static class DateTimeUtils
	{
		public static int CountRepeat(string fmt, int p, char c)
		{
			int length = fmt.Length;
			int num = p + 1;
			while (num < length && fmt[num] == c)
			{
				num++;
			}
			return num - p;
		}

		public unsafe static void ZeroPad(StringBuilder output, int digits, int len)
		{
			char* ptr = stackalloc char[checked(16 * 2)];
			int num = 16;
			do
			{
				ptr[--num * 2] = (char)(48 + digits % 10);
				digits /= 10;
				len--;
			}
			while (digits > 0);
			while (len-- > 0)
			{
				ptr[--num * 2] = '0';
			}
			output.Append(new string(ptr, num, 16 - num));
		}

		public static int ParseQuotedString(string fmt, int pos, StringBuilder output)
		{
			int length = fmt.Length;
			int num = pos;
			char c = fmt[pos++];
			while (pos < length)
			{
				char c2 = fmt[pos++];
				if (c2 == c)
				{
					return pos - num;
				}
				if (c2 == '\\')
				{
					if (pos >= length)
					{
						throw new FormatException("Un-ended quote");
					}
					output.Append(fmt[pos++]);
				}
				else
				{
					output.Append(c2);
				}
			}
			throw new FormatException("Un-ended quote");
		}

		public static string GetStandardPattern(char format, DateTimeFormatInfo dfi, out bool useutc, out bool use_invariant)
		{
			return DateTimeUtils.GetStandardPattern(format, dfi, out useutc, out use_invariant, false);
		}

		public static string GetStandardPattern(char format, DateTimeFormatInfo dfi, out bool useutc, out bool use_invariant, bool date_time_offset)
		{
			useutc = false;
			use_invariant = false;
			string result;
			switch (format)
			{
			case 'm':
				break;
			default:
				switch (format)
				{
				case 'M':
					break;
				default:
					switch (format)
					{
					case 'D':
						return dfi.LongDatePattern;
					default:
						switch (format)
						{
						case 'd':
							return dfi.ShortDatePattern;
						default:
							if (format != 'Y')
							{
								return null;
							}
							goto IL_1BA;
						case 'f':
							return dfi.LongDatePattern + " " + dfi.ShortTimePattern;
						case 'g':
							return dfi.ShortDatePattern + " " + dfi.ShortTimePattern;
						}
						break;
					case 'F':
						return dfi.FullDateTimePattern;
					case 'G':
						return dfi.ShortDatePattern + " " + dfi.LongTimePattern;
					}
					break;
				case 'O':
					goto IL_135;
				case 'R':
					goto IL_144;
				case 'T':
					return dfi.LongTimePattern;
				case 'U':
					if (date_time_offset)
					{
						result = null;
					}
					else
					{
						result = dfi.FullDateTimePattern;
						useutc = true;
					}
					return result;
				}
				break;
			case 'o':
				goto IL_135;
			case 'r':
				goto IL_144;
			case 's':
				result = dfi.SortableDateTimePattern;
				use_invariant = true;
				return result;
			case 't':
				return dfi.ShortTimePattern;
			case 'u':
				result = dfi.UniversalSortableDateTimePattern;
				if (date_time_offset)
				{
					useutc = true;
				}
				use_invariant = true;
				return result;
			case 'y':
				goto IL_1BA;
			}
			return dfi.MonthDayPattern;
			IL_135:
			result = dfi.RoundtripPattern;
			use_invariant = true;
			return result;
			IL_144:
			result = dfi.RFC1123Pattern;
			if (date_time_offset)
			{
				useutc = true;
			}
			use_invariant = true;
			return result;
			IL_1BA:
			result = dfi.YearMonthPattern;
			return result;
		}

		public static string ToString(DateTime dt, string format, DateTimeFormatInfo dfi)
		{
			return DateTimeUtils.ToString(dt, null, format, dfi);
		}

		public static string ToString(DateTime dt, TimeSpan? utc_offset, string format, DateTimeFormatInfo dfi)
		{
			StringBuilder stringBuilder = new StringBuilder(format.Length + 10);
			DateTimeFormatInfo invariantInfo = DateTimeFormatInfo.InvariantInfo;
			if (format == invariantInfo.RFC1123Pattern)
			{
				dfi = invariantInfo;
			}
			else if (format == invariantInfo.UniversalSortableDateTimePattern)
			{
				dfi = invariantInfo;
			}
			int i = 0;
			while (i < format.Length)
			{
				bool flag = false;
				char c = format[i];
				char c2 = c;
				int num;
				switch (c2)
				{
				case 'd':
					num = DateTimeUtils.CountRepeat(format, i, c);
					if (num <= 2)
					{
						DateTimeUtils.ZeroPad(stringBuilder, dfi.Calendar.GetDayOfMonth(dt), (num != 1) ? 2 : 1);
					}
					else if (num == 3)
					{
						stringBuilder.Append(dfi.GetAbbreviatedDayName(dfi.Calendar.GetDayOfWeek(dt)));
					}
					else
					{
						stringBuilder.Append(dfi.GetDayName(dfi.Calendar.GetDayOfWeek(dt)));
					}
					break;
				default:
					switch (c2)
					{
					case 'F':
						flag = true;
						goto IL_1E3;
					default:
						switch (c2)
						{
						case 's':
							num = DateTimeUtils.CountRepeat(format, i, c);
							DateTimeUtils.ZeroPad(stringBuilder, dt.Second, (num != 1) ? 2 : 1);
							break;
						case 't':
						{
							num = DateTimeUtils.CountRepeat(format, i, c);
							string text = (dt.Hour >= 12) ? dfi.PMDesignator : dfi.AMDesignator;
							if (num == 1)
							{
								if (text.Length >= 1)
								{
									stringBuilder.Append(text[0]);
								}
							}
							else
							{
								stringBuilder.Append(text);
							}
							break;
						}
						default:
							switch (c2)
							{
							case '"':
							case '\'':
								num = DateTimeUtils.ParseQuotedString(format, i, stringBuilder);
								break;
							default:
								if (c2 != '/')
								{
									if (c2 != ':')
									{
										if (c2 != '\\')
										{
											stringBuilder.Append(c);
											num = 1;
										}
										else
										{
											if (i >= format.Length - 1)
											{
												throw new FormatException("\\ at end of date time string");
											}
											stringBuilder.Append(format[i + 1]);
											num = 2;
										}
									}
									else
									{
										stringBuilder.Append(dfi.TimeSeparator);
										num = 1;
									}
								}
								else
								{
									stringBuilder.Append(dfi.DateSeparator);
									num = 1;
								}
								break;
							case '%':
								if (i >= format.Length - 1)
								{
									throw new FormatException("% at end of date time string");
								}
								if (format[i + 1] == '%')
								{
									throw new FormatException("%% in date string");
								}
								num = 1;
								break;
							}
							break;
						case 'y':
							num = DateTimeUtils.CountRepeat(format, i, c);
							if (num <= 2)
							{
								DateTimeUtils.ZeroPad(stringBuilder, dfi.Calendar.GetYear(dt) % 100, num);
							}
							else
							{
								DateTimeUtils.ZeroPad(stringBuilder, dfi.Calendar.GetYear(dt), num);
							}
							break;
						case 'z':
						{
							num = DateTimeUtils.CountRepeat(format, i, c);
							TimeSpan timeSpan = (utc_offset == null) ? TimeZone.CurrentTimeZone.GetUtcOffset(dt) : utc_offset.Value;
							if (timeSpan.Ticks >= 0L)
							{
								stringBuilder.Append('+');
							}
							else
							{
								stringBuilder.Append('-');
							}
							int num2 = num;
							if (num2 != 1)
							{
								if (num2 != 2)
								{
									stringBuilder.Append(Math.Abs(timeSpan.Hours).ToString("00"));
									stringBuilder.Append(':');
									stringBuilder.Append(Math.Abs(timeSpan.Minutes).ToString("00"));
								}
								else
								{
									stringBuilder.Append(Math.Abs(timeSpan.Hours).ToString("00"));
								}
							}
							else
							{
								stringBuilder.Append(Math.Abs(timeSpan.Hours));
							}
							break;
						}
						}
						break;
					case 'H':
						num = DateTimeUtils.CountRepeat(format, i, c);
						DateTimeUtils.ZeroPad(stringBuilder, dt.Hour, (num != 1) ? 2 : 1);
						break;
					case 'K':
						num = 1;
						if (utc_offset != null || dt.Kind == DateTimeKind.Local)
						{
							TimeSpan timeSpan = (utc_offset == null) ? TimeZone.CurrentTimeZone.GetUtcOffset(dt) : utc_offset.Value;
							if (timeSpan.Ticks >= 0L)
							{
								stringBuilder.Append('+');
							}
							else
							{
								stringBuilder.Append('-');
							}
							stringBuilder.Append(Math.Abs(timeSpan.Hours).ToString("00"));
							stringBuilder.Append(':');
							stringBuilder.Append(Math.Abs(timeSpan.Minutes).ToString("00"));
						}
						else if (dt.Kind == DateTimeKind.Utc)
						{
							stringBuilder.Append('Z');
						}
						break;
					case 'M':
					{
						num = DateTimeUtils.CountRepeat(format, i, c);
						int month = dfi.Calendar.GetMonth(dt);
						if (num <= 2)
						{
							DateTimeUtils.ZeroPad(stringBuilder, month, num);
						}
						else if (num == 3)
						{
							stringBuilder.Append(dfi.GetAbbreviatedMonthName(month));
						}
						else
						{
							stringBuilder.Append(dfi.GetMonthName(month));
						}
						break;
					}
					}
					break;
				case 'f':
					goto IL_1E3;
				case 'g':
					num = DateTimeUtils.CountRepeat(format, i, c);
					stringBuilder.Append(dfi.GetEraName(dfi.Calendar.GetEra(dt)));
					break;
				case 'h':
				{
					num = DateTimeUtils.CountRepeat(format, i, c);
					int num3 = dt.Hour % 12;
					if (num3 == 0)
					{
						num3 = 12;
					}
					DateTimeUtils.ZeroPad(stringBuilder, num3, (num != 1) ? 2 : 1);
					break;
				}
				case 'm':
					num = DateTimeUtils.CountRepeat(format, i, c);
					DateTimeUtils.ZeroPad(stringBuilder, dt.Minute, (num != 1) ? 2 : 1);
					break;
				}
				IL_6C6:
				i += num;
				continue;
				IL_1E3:
				num = DateTimeUtils.CountRepeat(format, i, c);
				if (num > 7)
				{
					throw new FormatException("Invalid Format String");
				}
				int num4 = (int)(dt.Ticks % 10000000L / (long)Math.Pow(10.0, (double)(7 - num)));
				int length = stringBuilder.Length;
				DateTimeUtils.ZeroPad(stringBuilder, num4, num);
				if (flag)
				{
					while (stringBuilder.Length > length && stringBuilder[stringBuilder.Length - 1] == '0')
					{
						stringBuilder.Length--;
					}
					if (num4 == 0 && length > 0 && stringBuilder[length - 1] == '.')
					{
						stringBuilder.Length--;
					}
				}
				goto IL_6C6;
			}
			return stringBuilder.ToString();
		}
	}
}
