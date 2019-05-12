using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextUtil
{
	public static string GetWinTextSkipColorCode(string str, int iCMax)
	{
		if (str == null)
		{
			return string.Empty;
		}
		string text = str;
		int i = 0;
		int num = iCMax;
		while (i < text.Length)
		{
			i = TextUtil.SkipColorCode(text, i);
			if (i == -1)
			{
				return string.Empty;
			}
			if (i < text.Length)
			{
				int num2 = i;
				i = TextUtil.SkipCRLFCode(text, i);
				if (i != num2)
				{
					num = iCMax;
				}
				if (i < text.Length)
				{
					i++;
					num--;
					if (i < text.Length)
					{
						i = TextUtil.SkipColorCode(text, i);
						if (i == -1)
						{
							return string.Empty;
						}
						if (i < text.Length)
						{
							num2 = i;
							i = TextUtil.SkipCRLFCode(text, i);
							if (i != num2)
							{
								num = iCMax;
							}
							if (i < text.Length)
							{
								if (num <= 0 || (num == 1 && i + 1 < text.Length && TextUtil.IsBadTopChar(text, i + 1)))
								{
									text = text.Insert(i, "\n");
									i++;
									num = iCMax;
								}
								if (i < text.Length)
								{
									continue;
								}
							}
						}
					}
				}
			}
			return text;
		}
		return text;
	}

	private static bool IsBadTopChar(string tmp, int idx)
	{
		return tmp[idx] == '.' || tmp[idx] == '・' || tmp[idx] == '。' || tmp[idx] == '、';
	}

	private static int SkipColorCode(string tmp, int cidx)
	{
		if (tmp[cidx] != '<')
		{
			return cidx;
		}
		while (tmp[cidx] != '>')
		{
			cidx++;
			if (cidx >= tmp.Length)
			{
				return -1;
			}
		}
		return cidx + 1;
	}

	private static int SkipCRLFCode(string tmp, int cidx)
	{
		if (tmp[cidx] == '\r' && tmp[cidx + 1] == '\n')
		{
			return cidx + 2;
		}
		if (tmp[cidx] == '\n')
		{
			return cidx + 1;
		}
		return cidx;
	}

	public static bool SurrogateCheck(string s)
	{
		foreach (char c in s)
		{
			if (TextUtil.IsEmojiCheck(c))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsEmojiCheck(char c)
	{
		if (char.IsSurrogate(c))
		{
			return true;
		}
		if (c != '‍' && c != '⃣' && c != '️')
		{
			switch (c)
			{
			case '⏩':
			case '⏪':
			case '⏫':
			case '⏬':
			case '⏭':
			case '⏮':
			case '⏯':
			case '⏰':
			case '⏱':
			case '⏲':
			case '⏳':
			case '⏸':
			case '⏹':
			case '⏺':
				break;
			default:
				switch (c)
				{
				case '↔':
				case '↕':
				case '↖':
				case '↗':
				case '↘':
				case '↙':
					break;
				default:
					switch (c)
					{
					case '◻':
					case '◼':
					case '◽':
					case '◾':
						break;
					default:
						if (c != '↩' && c != '↪' && c != '⌚' && c != '⌛' && c != '▪' && c != '▫' && c != '©' && c != '®' && c != '‼' && c != '⁉' && c != '™' && c != 'ℹ' && c != '⌨' && c != '⏏' && c != 'Ⓜ' && c != '▶' && c != '◀')
						{
							switch (c)
							{
							case '♈':
							case '♉':
							case '♊':
							case '♋':
							case '♌':
							case '♍':
							case '♎':
							case '♏':
							case '♐':
							case '♑':
							case '♒':
							case '♓':
							case '♠':
							case '♣':
							case '♥':
							case '♦':
							case '♨':
								break;
							default:
								switch (c)
								{
								case '⚒':
								case '⚓':
								case '⚔':
								case '⚕':
								case '⚖':
								case '⚗':
								case '⚙':
								case '⚛':
								case '⚜':
								case '⚠':
								case '⚡':
								case '⚪':
								case '⚫':
									break;
								default:
									switch (c)
									{
									case '⛩':
									case '⛪':
									case '⛰':
									case '⛱':
									case '⛲':
									case '⛳':
									case '⛴':
									case '⛵':
									case '⛷':
									case '⛸':
									case '⛹':
									case '⛺':
									case '⛽':
										break;
									default:
										switch (c)
										{
										case '☝':
										case '☠':
										case '☢':
										case '☣':
										case '☦':
											break;
										default:
											switch (c)
											{
											case '⛎':
											case '⛏':
											case '⛑':
											case '⛓':
											case '⛔':
												break;
											default:
												switch (c)
												{
												case '☪':
												case '☮':
												case '☯':
													break;
												default:
													switch (c)
													{
													case '☀':
													case '☁':
													case '☂':
													case '☃':
													case '☄':
														break;
													default:
														switch (c)
														{
														case '☔':
														case '☕':
														case '☘':
															break;
														default:
															switch (c)
															{
															case '⛄':
															case '⛅':
															case '⛈':
																break;
															default:
																switch (c)
																{
																case '☎':
																case '☑':
																	break;
																default:
																	switch (c)
																	{
																	case '☸':
																	case '☹':
																	case '☺':
																		break;
																	default:
																		switch (c)
																		{
																		case '♀':
																		case '♂':
																			break;
																		default:
																			if (c != '⚰' && c != '⚱' && c != '⚽' && c != '⚾' && c != '♻' && c != '♿')
																			{
																				switch (c)
																				{
																				case '✈':
																				case '✉':
																				case '✊':
																				case '✋':
																				case '✌':
																				case '✍':
																				case '✏':
																				case '✒':
																				case '✔':
																				case '✖':
																				case '✝':
																					break;
																				default:
																					switch (c)
																					{
																					case '❌':
																					case '❎':
																					case '❓':
																					case '❔':
																					case '❕':
																					case '❗':
																						break;
																					default:
																						switch (c)
																						{
																						case '✂':
																						case '✅':
																							break;
																						default:
																							switch (c)
																							{
																							case '❄':
																							case '❇':
																								break;
																							default:
																								switch (c)
																								{
																								case '➕':
																								case '➖':
																								case '➗':
																									break;
																								default:
																									switch (c)
																									{
																									case '⬅':
																									case '⬆':
																									case '⬇':
																										break;
																									default:
																										switch (c)
																										{
																										case '㊗':
																										case '㊙':
																											break;
																										default:
																											if (c != '✳' && c != '✴' && c != '❣' && c != '❤' && c != '⤴' && c != '⤵' && c != '⬛' && c != '⬜' && c != '✡' && c != '✨' && c != '➡' && c != '➰' && c != '➿' && c != '⭐' && c != '⭕' && c != '〰' && c != '〽')
																											{
																												return false;
																											}
																											break;
																										}
																										break;
																									}
																									break;
																								}
																								break;
																							}
																							break;
																						}
																						break;
																					}
																					break;
																				}
																				return true;
																			}
																			break;
																		}
																		break;
																	}
																	break;
																}
																break;
															}
															break;
														}
														break;
													}
													break;
												}
												break;
											}
											break;
										}
										break;
									}
									break;
								}
								break;
							}
							return true;
						}
						break;
					}
					break;
				}
				break;
			}
			return true;
		}
		return true;
	}

	private static bool IsHalfChar(char c)
	{
		return TextUtil.IsHalfChar(char.ToString(c));
	}

	private static bool IsHalfChar(string s)
	{
		string pattern = "^[^ -~｡-ﾟ]+$";
		return Regex.IsMatch(s, pattern);
	}

	private static int TextLengthFullAndHalf(string str)
	{
		int num = 0;
		int num2 = 0;
		foreach (char c in str)
		{
			if (TextUtil.IsHalfChar(c))
			{
				num++;
			}
			else
			{
				num2++;
			}
		}
		return num * 2 + num2;
	}

	public static bool IsOverLengthFullAndHalf(string str, int limit)
	{
		return TextUtil.TextLengthFullAndHalf(str) > limit;
	}

	public static void ShowNumber(List<UISprite> spL, string sprName, int num, bool dontSHowZero = false)
	{
		string text = num.ToString();
		int num2 = text.Length - 1;
		for (int i = 0; i < spL.Count; i++)
		{
			if (dontSHowZero && num == 0)
			{
				spL[i].gameObject.SetActive(false);
			}
			else
			{
				if (num2 >= 0)
				{
					spL[i].gameObject.SetActive(true);
					string str = text[num2].ToString();
					spL[i].spriteName = sprName + str;
				}
				else
				{
					spL[i].gameObject.SetActive(false);
				}
				num2--;
			}
		}
	}
}
