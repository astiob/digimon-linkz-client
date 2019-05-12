using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Mono.Xml.Xsl
{
	internal class HtmlEmitter : Emitter
	{
		private TextWriter writer;

		private Stack elementNameStack;

		private bool openElement;

		private bool openAttribute;

		private int nonHtmlDepth;

		private bool indent;

		private Encoding outputEncoding;

		private string mediaType;

		public HtmlEmitter(TextWriter writer, XslOutput output)
		{
			this.writer = writer;
			this.indent = (output.Indent == "yes" || output.Indent == null);
			this.elementNameStack = new Stack();
			this.nonHtmlDepth = -1;
			this.outputEncoding = ((writer.Encoding != null) ? writer.Encoding : output.Encoding);
			this.mediaType = output.MediaType;
			if (this.mediaType == null || this.mediaType.Length == 0)
			{
				this.mediaType = "text/html";
			}
		}

		public override void WriteStartDocument(Encoding encoding, StandaloneType standalone)
		{
		}

		public override void WriteEndDocument()
		{
		}

		public override void WriteDocType(string name, string publicId, string systemId)
		{
			this.writer.Write("<!DOCTYPE html ");
			if (publicId != null)
			{
				this.writer.Write("PUBLIC \"");
				this.writer.Write(publicId);
				this.writer.Write("\" ");
				if (systemId != null)
				{
					this.writer.Write("\"");
					this.writer.Write(systemId);
					this.writer.Write("\"");
				}
			}
			else if (systemId != null)
			{
				this.writer.Write("SYSTEM \"");
				this.writer.Write(systemId);
				this.writer.Write('"');
			}
			this.writer.Write('>');
			if (this.indent)
			{
				this.writer.WriteLine();
			}
		}

		private void CloseAttribute()
		{
			this.writer.Write('"');
			this.openAttribute = false;
		}

		private void CloseStartElement()
		{
			if (this.openAttribute)
			{
				this.CloseAttribute();
			}
			this.writer.Write('>');
			this.openElement = false;
			if (this.outputEncoding != null && this.elementNameStack.Count > 0)
			{
				string text = ((string)this.elementNameStack.Peek()).ToUpper(CultureInfo.InvariantCulture);
				string text2 = text;
				if (text2 != null)
				{
					if (HtmlEmitter.<>f__switch$map13 == null)
					{
						HtmlEmitter.<>f__switch$map13 = new Dictionary<string, int>(3)
						{
							{
								"HEAD",
								0
							},
							{
								"STYLE",
								1
							},
							{
								"SCRIPT",
								1
							}
						};
					}
					int num;
					if (HtmlEmitter.<>f__switch$map13.TryGetValue(text2, out num))
					{
						if (num != 0)
						{
							if (num == 1)
							{
								this.writer.WriteLine();
								for (int i = 0; i <= this.elementNameStack.Count; i++)
								{
									this.writer.Write("  ");
								}
							}
						}
						else
						{
							this.WriteStartElement(string.Empty, "META", string.Empty);
							this.WriteAttributeString(string.Empty, "http-equiv", string.Empty, "Content-Type");
							this.WriteAttributeString(string.Empty, "content", string.Empty, this.mediaType + "; charset=" + this.outputEncoding.WebName);
							this.WriteEndElement();
						}
					}
				}
			}
		}

		private void Indent(string elementName, bool endIndent)
		{
			if (!this.indent)
			{
				return;
			}
			string text = elementName.ToUpper(CultureInfo.InvariantCulture);
			if (text == null)
			{
				goto IL_211;
			}
			if (HtmlEmitter.<>f__switch$map14 == null)
			{
				HtmlEmitter.<>f__switch$map14 = new Dictionary<string, int>(31)
				{
					{
						"ADDRESS",
						0
					},
					{
						"APPLET",
						0
					},
					{
						"BDO",
						0
					},
					{
						"BLOCKQUOTE",
						0
					},
					{
						"BODY",
						0
					},
					{
						"BUTTON",
						0
					},
					{
						"CAPTION",
						0
					},
					{
						"CENTER",
						0
					},
					{
						"DD",
						0
					},
					{
						"DEL",
						0
					},
					{
						"DIR",
						0
					},
					{
						"DIV",
						0
					},
					{
						"DL",
						0
					},
					{
						"DT",
						0
					},
					{
						"FIELDSET",
						0
					},
					{
						"HEAD",
						0
					},
					{
						"HTML",
						0
					},
					{
						"IFRAME",
						0
					},
					{
						"INS",
						0
					},
					{
						"LI",
						0
					},
					{
						"MAP",
						0
					},
					{
						"MENU",
						0
					},
					{
						"NOFRAMES",
						0
					},
					{
						"NOSCRIPT",
						0
					},
					{
						"OBJECT",
						0
					},
					{
						"OPTION",
						0
					},
					{
						"PRE",
						0
					},
					{
						"TABLE",
						0
					},
					{
						"TD",
						0
					},
					{
						"TH",
						0
					},
					{
						"TR",
						0
					}
				};
			}
			int num;
			if (!HtmlEmitter.<>f__switch$map14.TryGetValue(text, out num))
			{
				goto IL_211;
			}
			if (num != 0)
			{
				goto IL_211;
			}
			IL_1C8:
			this.writer.Write(this.writer.NewLine);
			int count = this.elementNameStack.Count;
			for (int i = 0; i < count; i++)
			{
				this.writer.Write("  ");
			}
			return;
			IL_211:
			if (elementName.Length > 0 && this.nonHtmlDepth > 0)
			{
				goto IL_1C8;
			}
		}

		public override void WriteStartElement(string prefix, string localName, string nsURI)
		{
			if (this.openElement)
			{
				this.CloseStartElement();
			}
			this.Indent((this.elementNameStack.Count <= 0) ? string.Empty : (this.elementNameStack.Peek() as string), false);
			string text = localName;
			this.writer.Write('<');
			if (nsURI != string.Empty)
			{
				if (prefix != string.Empty)
				{
					text = prefix + ":" + localName;
				}
				if (this.nonHtmlDepth < 0)
				{
					this.nonHtmlDepth = this.elementNameStack.Count + 1;
				}
			}
			this.writer.Write(text);
			this.elementNameStack.Push(text);
			this.openElement = true;
		}

		private bool IsHtmlElement(string localName)
		{
			string text = localName.ToUpper(CultureInfo.InvariantCulture);
			if (text != null)
			{
				if (HtmlEmitter.<>f__switch$map15 == null)
				{
					HtmlEmitter.<>f__switch$map15 = new Dictionary<string, int>(91)
					{
						{
							"A",
							0
						},
						{
							"ABBR",
							0
						},
						{
							"ACRONYM",
							0
						},
						{
							"ADDRESS",
							0
						},
						{
							"APPLET",
							0
						},
						{
							"AREA",
							0
						},
						{
							"B",
							0
						},
						{
							"BASE",
							0
						},
						{
							"BASEFONT",
							0
						},
						{
							"BDO",
							0
						},
						{
							"BIG",
							0
						},
						{
							"BLOCKQUOTE",
							0
						},
						{
							"BODY",
							0
						},
						{
							"BR",
							0
						},
						{
							"BUTTON",
							0
						},
						{
							"CAPTION",
							0
						},
						{
							"CENTER",
							0
						},
						{
							"CITE",
							0
						},
						{
							"CODE",
							0
						},
						{
							"COL",
							0
						},
						{
							"COLGROUP",
							0
						},
						{
							"DD",
							0
						},
						{
							"DEL",
							0
						},
						{
							"DFN",
							0
						},
						{
							"DIR",
							0
						},
						{
							"DIV",
							0
						},
						{
							"DL",
							0
						},
						{
							"DT",
							0
						},
						{
							"EM",
							0
						},
						{
							"FIELDSET",
							0
						},
						{
							"FONT",
							0
						},
						{
							"FORM",
							0
						},
						{
							"FRAME",
							0
						},
						{
							"FRAMESET",
							0
						},
						{
							"H1",
							0
						},
						{
							"H2",
							0
						},
						{
							"H3",
							0
						},
						{
							"H4",
							0
						},
						{
							"H5",
							0
						},
						{
							"H6",
							0
						},
						{
							"HEAD",
							0
						},
						{
							"HR",
							0
						},
						{
							"HTML",
							0
						},
						{
							"I",
							0
						},
						{
							"IFRAME",
							0
						},
						{
							"IMG",
							0
						},
						{
							"INPUT",
							0
						},
						{
							"INS",
							0
						},
						{
							"ISINDEX",
							0
						},
						{
							"KBD",
							0
						},
						{
							"LABEL",
							0
						},
						{
							"LEGEND",
							0
						},
						{
							"LI",
							0
						},
						{
							"LINK",
							0
						},
						{
							"MAP",
							0
						},
						{
							"MENU",
							0
						},
						{
							"META",
							0
						},
						{
							"NOFRAMES",
							0
						},
						{
							"NOSCRIPT",
							0
						},
						{
							"OBJECT",
							0
						},
						{
							"OL",
							0
						},
						{
							"OPTGROUP",
							0
						},
						{
							"OPTION",
							0
						},
						{
							"P",
							0
						},
						{
							"PARAM",
							0
						},
						{
							"PRE",
							0
						},
						{
							"Q",
							0
						},
						{
							"S",
							0
						},
						{
							"SAMP",
							0
						},
						{
							"SCRIPT",
							0
						},
						{
							"SELECT",
							0
						},
						{
							"SMALL",
							0
						},
						{
							"SPAN",
							0
						},
						{
							"STRIKE",
							0
						},
						{
							"STRONG",
							0
						},
						{
							"STYLE",
							0
						},
						{
							"SUB",
							0
						},
						{
							"SUP",
							0
						},
						{
							"TABLE",
							0
						},
						{
							"TBODY",
							0
						},
						{
							"TD",
							0
						},
						{
							"TEXTAREA",
							0
						},
						{
							"TFOOT",
							0
						},
						{
							"TH",
							0
						},
						{
							"THEAD",
							0
						},
						{
							"TITLE",
							0
						},
						{
							"TR",
							0
						},
						{
							"TT",
							0
						},
						{
							"U",
							0
						},
						{
							"UL",
							0
						},
						{
							"VAR",
							0
						}
					};
				}
				int num;
				if (HtmlEmitter.<>f__switch$map15.TryGetValue(text, out num))
				{
					if (num == 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override void WriteEndElement()
		{
			this.WriteFullEndElement();
		}

		public override void WriteFullEndElement()
		{
			string text = this.elementNameStack.Peek() as string;
			string text2 = text.ToUpper(CultureInfo.InvariantCulture);
			if (text2 != null)
			{
				if (HtmlEmitter.<>f__switch$map16 == null)
				{
					HtmlEmitter.<>f__switch$map16 = new Dictionary<string, int>(13)
					{
						{
							"AREA",
							0
						},
						{
							"BASE",
							0
						},
						{
							"BASEFONT",
							0
						},
						{
							"BR",
							0
						},
						{
							"COL",
							0
						},
						{
							"FRAME",
							0
						},
						{
							"HR",
							0
						},
						{
							"IMG",
							0
						},
						{
							"INPUT",
							0
						},
						{
							"ISINDEX",
							0
						},
						{
							"LINK",
							0
						},
						{
							"META",
							0
						},
						{
							"PARAM",
							0
						}
					};
				}
				int num;
				if (HtmlEmitter.<>f__switch$map16.TryGetValue(text2, out num))
				{
					if (num == 0)
					{
						if (this.openAttribute)
						{
							this.CloseAttribute();
						}
						if (this.openElement)
						{
							this.writer.Write('>');
						}
						this.elementNameStack.Pop();
						goto IL_190;
					}
				}
			}
			if (this.openElement)
			{
				this.CloseStartElement();
			}
			this.elementNameStack.Pop();
			if (this.IsHtmlElement(text))
			{
				this.Indent(text, true);
			}
			this.writer.Write("</");
			this.writer.Write(text);
			this.writer.Write(">");
			IL_190:
			if (this.nonHtmlDepth > this.elementNameStack.Count)
			{
				this.nonHtmlDepth = -1;
			}
			this.openElement = false;
		}

		public override void WriteAttributeString(string prefix, string localName, string nsURI, string value)
		{
			this.writer.Write(' ');
			if (prefix != null && prefix.Length != 0)
			{
				this.writer.Write(prefix);
				this.writer.Write(":");
			}
			this.writer.Write(localName);
			if (this.nonHtmlDepth >= 0)
			{
				this.writer.Write("=\"");
				this.openAttribute = true;
				this.WriteFormattedString(value);
				this.openAttribute = false;
				this.writer.Write('"');
				return;
			}
			string a = localName.ToUpper(CultureInfo.InvariantCulture);
			string text = ((string)this.elementNameStack.Peek()).ToLower(CultureInfo.InvariantCulture);
			if ((a == "SELECTED" && text == "option") || (a == "CHECKED" && text == "input"))
			{
				return;
			}
			this.writer.Write("=\"");
			this.openAttribute = true;
			string text2 = null;
			string[] array = null;
			string text3 = text;
			switch (text3)
			{
			case "q":
			case "blockquote":
			case "ins":
			case "del":
				text2 = "cite";
				break;
			case "form":
				text2 = "action";
				break;
			case "a":
			case "area":
			case "link":
			case "base":
				text2 = "href";
				break;
			case "head":
				text2 = "profile";
				break;
			case "input":
				array = new string[]
				{
					"src",
					"usemap"
				};
				break;
			case "img":
				array = new string[]
				{
					"src",
					"usemap",
					"longdesc"
				};
				break;
			case "object":
				array = new string[]
				{
					"classid",
					"codebase",
					"data",
					"archive",
					"usemap"
				};
				break;
			case "script":
				array = new string[]
				{
					"src",
					"for"
				};
				break;
			}
			if (array != null)
			{
				string b = localName.ToLower(CultureInfo.InvariantCulture);
				foreach (string a2 in array)
				{
					if (a2 == b)
					{
						value = HtmlEmitter.HtmlUriEscape.EscapeUri(value);
						break;
					}
				}
			}
			else if (text2 != null && text2 == localName.ToLower(CultureInfo.InvariantCulture))
			{
				value = HtmlEmitter.HtmlUriEscape.EscapeUri(value);
			}
			this.WriteFormattedString(value);
			this.openAttribute = false;
			this.writer.Write('"');
		}

		public override void WriteComment(string text)
		{
			if (this.openElement)
			{
				this.CloseStartElement();
			}
			this.writer.Write("<!--");
			this.writer.Write(text);
			this.writer.Write("-->");
		}

		public override void WriteProcessingInstruction(string name, string text)
		{
			if (text.IndexOf("?>") > 0)
			{
				throw new ArgumentException("Processing instruction cannot contain \"?>\" as its value.");
			}
			if (this.openElement)
			{
				this.CloseStartElement();
			}
			if (this.elementNameStack.Count > 0)
			{
				this.Indent(this.elementNameStack.Peek() as string, false);
			}
			this.writer.Write("<?");
			this.writer.Write(name);
			if (text != null && text != string.Empty)
			{
				this.writer.Write(' ');
				this.writer.Write(text);
			}
			if (this.nonHtmlDepth >= 0)
			{
				this.writer.Write("?>");
			}
			else
			{
				this.writer.Write(">");
			}
		}

		public override void WriteString(string text)
		{
			if (this.openElement)
			{
				this.CloseStartElement();
			}
			this.WriteFormattedString(text);
		}

		private void WriteFormattedString(string text)
		{
			if (!this.openAttribute && this.elementNameStack.Count > 0)
			{
				string text2 = ((string)this.elementNameStack.Peek()).ToUpper(CultureInfo.InvariantCulture);
				string text3 = text2;
				if (text3 != null)
				{
					if (HtmlEmitter.<>f__switch$map18 == null)
					{
						HtmlEmitter.<>f__switch$map18 = new Dictionary<string, int>(2)
						{
							{
								"SCRIPT",
								0
							},
							{
								"STYLE",
								0
							}
						};
					}
					int num;
					if (HtmlEmitter.<>f__switch$map18.TryGetValue(text3, out num))
					{
						if (num == 0)
						{
							this.writer.Write(text);
							return;
						}
					}
				}
			}
			int num2 = 0;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				switch (c)
				{
				case '<':
					if (!this.openAttribute)
					{
						this.writer.Write(text.ToCharArray(), num2, i - num2);
						this.writer.Write("&lt;");
						num2 = i + 1;
					}
					break;
				default:
					if (c != '"')
					{
						if (c == '&')
						{
							if (this.nonHtmlDepth >= 0 || i + 1 >= text.Length || text[i + 1] != '{')
							{
								this.writer.Write(text.ToCharArray(), num2, i - num2);
								this.writer.Write("&amp;");
								num2 = i + 1;
							}
						}
					}
					else if (this.openAttribute)
					{
						this.writer.Write(text.ToCharArray(), num2, i - num2);
						this.writer.Write("&quot;");
						num2 = i + 1;
					}
					break;
				case '>':
					if (!this.openAttribute)
					{
						this.writer.Write(text.ToCharArray(), num2, i - num2);
						this.writer.Write("&gt;");
						num2 = i + 1;
					}
					break;
				}
			}
			if (text.Length > num2)
			{
				this.writer.Write(text.ToCharArray(), num2, text.Length - num2);
			}
		}

		public override void WriteRaw(string data)
		{
			if (this.openElement)
			{
				this.CloseStartElement();
			}
			this.writer.Write(data);
		}

		public override void WriteCDataSection(string text)
		{
			if (this.openElement)
			{
				this.CloseStartElement();
			}
			this.writer.Write(text);
		}

		public override void WriteWhitespace(string value)
		{
			if (this.openElement)
			{
				this.CloseStartElement();
			}
			this.writer.Write(value);
		}

		public override void Done()
		{
			this.writer.Flush();
		}

		private class HtmlUriEscape : Uri
		{
			private HtmlUriEscape() : base("urn:foo")
			{
			}

			public static string EscapeUri(string input)
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				for (int i = 0; i < input.Length; i++)
				{
					char c = input[i];
					if (c >= ' ' && c <= '\u007f')
					{
						char c2 = c;
						bool flag;
						switch (c2)
						{
						case '"':
						case '&':
						case '\'':
							goto IL_6F;
						default:
							switch (c2)
							{
							case '<':
							case '>':
								goto IL_6F;
							}
							flag = Uri.IsExcludedCharacter(c);
							break;
						}
						IL_84:
						if (flag)
						{
							stringBuilder.Append(Uri.EscapeString(input.Substring(num, i - num)));
							stringBuilder.Append(c);
							num = i + 1;
							goto IL_AD;
						}
						goto IL_AD;
						IL_6F:
						flag = true;
						goto IL_84;
					}
					IL_AD:;
				}
				if (num < input.Length)
				{
					stringBuilder.Append(Uri.EscapeString(input.Substring(num)));
				}
				return stringBuilder.ToString();
			}
		}
	}
}
