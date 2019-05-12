using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WebSocketSharp.Net
{
	internal sealed class HttpUtility
	{
		private static Dictionary<string, char> _entities;

		private static char[] _hexChars = "0123456789abcdef".ToCharArray();

		private static object _sync = new object();

		private static Dictionary<string, char> Entities
		{
			get
			{
				object sync = HttpUtility._sync;
				Dictionary<string, char> entities;
				lock (sync)
				{
					if (HttpUtility._entities == null)
					{
						HttpUtility.InitEntities();
					}
					entities = HttpUtility._entities;
				}
				return entities;
			}
		}

		private static string A1(string username, string password, string realm)
		{
			return string.Format("{0}:{1}:{2}", username, realm, password);
		}

		private static string A1(string username, string password, string realm, string nonce, string cnonce)
		{
			return string.Format("{0}:{1}:{2}", HttpUtility.Hash(HttpUtility.A1(username, password, realm)), nonce, cnonce);
		}

		private static string A2(string method, string uri)
		{
			return string.Format("{0}:{1}", method, uri);
		}

		private static string A2(string method, string uri, string entity)
		{
			return string.Format("{0}:{1}:{2}", method, uri, entity);
		}

		private static int GetChar(byte[] bytes, int offset, int length)
		{
			int num = 0;
			int num2 = length + offset;
			for (int i = offset; i < num2; i++)
			{
				int @int = HttpUtility.GetInt(bytes[i]);
				if (@int == -1)
				{
					return -1;
				}
				num = (num << 4) + @int;
			}
			return num;
		}

		private static int GetChar(string s, int offset, int length)
		{
			int num = 0;
			int num2 = length + offset;
			for (int i = offset; i < num2; i++)
			{
				char c = s[i];
				if (c > '\u007f')
				{
					return -1;
				}
				int @int = HttpUtility.GetInt((byte)c);
				if (@int == -1)
				{
					return -1;
				}
				num = (num << 4) + @int;
			}
			return num;
		}

		private static char[] GetChars(MemoryStream buffer, Encoding encoding)
		{
			return encoding.GetChars(buffer.GetBuffer(), 0, (int)buffer.Length);
		}

		private static int GetInt(byte b)
		{
			char c = (char)b;
			return (c < '0' || c > '9') ? ((c < 'a' || c > 'f') ? ((c < 'A' || c > 'F') ? -1 : ((int)(c - 'A' + '\n'))) : ((int)(c - 'a' + '\n'))) : ((int)(c - '0'));
		}

		private static string Hash(string value)
		{
			MD5 md = MD5.Create();
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			byte[] array = md.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder(64);
			foreach (byte b in array)
			{
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		private static void InitEntities()
		{
			HttpUtility._entities = new Dictionary<string, char>();
			HttpUtility._entities.Add("nbsp", '\u00a0');
			HttpUtility._entities.Add("iexcl", '¡');
			HttpUtility._entities.Add("cent", '¢');
			HttpUtility._entities.Add("pound", '£');
			HttpUtility._entities.Add("curren", '¤');
			HttpUtility._entities.Add("yen", '¥');
			HttpUtility._entities.Add("brvbar", '¦');
			HttpUtility._entities.Add("sect", '§');
			HttpUtility._entities.Add("uml", '¨');
			HttpUtility._entities.Add("copy", '©');
			HttpUtility._entities.Add("ordf", 'ª');
			HttpUtility._entities.Add("laquo", '«');
			HttpUtility._entities.Add("not", '¬');
			HttpUtility._entities.Add("shy", '­');
			HttpUtility._entities.Add("reg", '®');
			HttpUtility._entities.Add("macr", '¯');
			HttpUtility._entities.Add("deg", '°');
			HttpUtility._entities.Add("plusmn", '±');
			HttpUtility._entities.Add("sup2", '²');
			HttpUtility._entities.Add("sup3", '³');
			HttpUtility._entities.Add("acute", '´');
			HttpUtility._entities.Add("micro", 'µ');
			HttpUtility._entities.Add("para", '¶');
			HttpUtility._entities.Add("middot", '·');
			HttpUtility._entities.Add("cedil", '¸');
			HttpUtility._entities.Add("sup1", '¹');
			HttpUtility._entities.Add("ordm", 'º');
			HttpUtility._entities.Add("raquo", '»');
			HttpUtility._entities.Add("frac14", '¼');
			HttpUtility._entities.Add("frac12", '½');
			HttpUtility._entities.Add("frac34", '¾');
			HttpUtility._entities.Add("iquest", '¿');
			HttpUtility._entities.Add("Agrave", 'À');
			HttpUtility._entities.Add("Aacute", 'Á');
			HttpUtility._entities.Add("Acirc", 'Â');
			HttpUtility._entities.Add("Atilde", 'Ã');
			HttpUtility._entities.Add("Auml", 'Ä');
			HttpUtility._entities.Add("Aring", 'Å');
			HttpUtility._entities.Add("AElig", 'Æ');
			HttpUtility._entities.Add("Ccedil", 'Ç');
			HttpUtility._entities.Add("Egrave", 'È');
			HttpUtility._entities.Add("Eacute", 'É');
			HttpUtility._entities.Add("Ecirc", 'Ê');
			HttpUtility._entities.Add("Euml", 'Ë');
			HttpUtility._entities.Add("Igrave", 'Ì');
			HttpUtility._entities.Add("Iacute", 'Í');
			HttpUtility._entities.Add("Icirc", 'Î');
			HttpUtility._entities.Add("Iuml", 'Ï');
			HttpUtility._entities.Add("ETH", 'Ð');
			HttpUtility._entities.Add("Ntilde", 'Ñ');
			HttpUtility._entities.Add("Ograve", 'Ò');
			HttpUtility._entities.Add("Oacute", 'Ó');
			HttpUtility._entities.Add("Ocirc", 'Ô');
			HttpUtility._entities.Add("Otilde", 'Õ');
			HttpUtility._entities.Add("Ouml", 'Ö');
			HttpUtility._entities.Add("times", '×');
			HttpUtility._entities.Add("Oslash", 'Ø');
			HttpUtility._entities.Add("Ugrave", 'Ù');
			HttpUtility._entities.Add("Uacute", 'Ú');
			HttpUtility._entities.Add("Ucirc", 'Û');
			HttpUtility._entities.Add("Uuml", 'Ü');
			HttpUtility._entities.Add("Yacute", 'Ý');
			HttpUtility._entities.Add("THORN", 'Þ');
			HttpUtility._entities.Add("szlig", 'ß');
			HttpUtility._entities.Add("agrave", 'à');
			HttpUtility._entities.Add("aacute", 'á');
			HttpUtility._entities.Add("acirc", 'â');
			HttpUtility._entities.Add("atilde", 'ã');
			HttpUtility._entities.Add("auml", 'ä');
			HttpUtility._entities.Add("aring", 'å');
			HttpUtility._entities.Add("aelig", 'æ');
			HttpUtility._entities.Add("ccedil", 'ç');
			HttpUtility._entities.Add("egrave", 'è');
			HttpUtility._entities.Add("eacute", 'é');
			HttpUtility._entities.Add("ecirc", 'ê');
			HttpUtility._entities.Add("euml", 'ë');
			HttpUtility._entities.Add("igrave", 'ì');
			HttpUtility._entities.Add("iacute", 'í');
			HttpUtility._entities.Add("icirc", 'î');
			HttpUtility._entities.Add("iuml", 'ï');
			HttpUtility._entities.Add("eth", 'ð');
			HttpUtility._entities.Add("ntilde", 'ñ');
			HttpUtility._entities.Add("ograve", 'ò');
			HttpUtility._entities.Add("oacute", 'ó');
			HttpUtility._entities.Add("ocirc", 'ô');
			HttpUtility._entities.Add("otilde", 'õ');
			HttpUtility._entities.Add("ouml", 'ö');
			HttpUtility._entities.Add("divide", '÷');
			HttpUtility._entities.Add("oslash", 'ø');
			HttpUtility._entities.Add("ugrave", 'ù');
			HttpUtility._entities.Add("uacute", 'ú');
			HttpUtility._entities.Add("ucirc", 'û');
			HttpUtility._entities.Add("uuml", 'ü');
			HttpUtility._entities.Add("yacute", 'ý');
			HttpUtility._entities.Add("thorn", 'þ');
			HttpUtility._entities.Add("yuml", 'ÿ');
			HttpUtility._entities.Add("fnof", 'ƒ');
			HttpUtility._entities.Add("Alpha", 'Α');
			HttpUtility._entities.Add("Beta", 'Β');
			HttpUtility._entities.Add("Gamma", 'Γ');
			HttpUtility._entities.Add("Delta", 'Δ');
			HttpUtility._entities.Add("Epsilon", 'Ε');
			HttpUtility._entities.Add("Zeta", 'Ζ');
			HttpUtility._entities.Add("Eta", 'Η');
			HttpUtility._entities.Add("Theta", 'Θ');
			HttpUtility._entities.Add("Iota", 'Ι');
			HttpUtility._entities.Add("Kappa", 'Κ');
			HttpUtility._entities.Add("Lambda", 'Λ');
			HttpUtility._entities.Add("Mu", 'Μ');
			HttpUtility._entities.Add("Nu", 'Ν');
			HttpUtility._entities.Add("Xi", 'Ξ');
			HttpUtility._entities.Add("Omicron", 'Ο');
			HttpUtility._entities.Add("Pi", 'Π');
			HttpUtility._entities.Add("Rho", 'Ρ');
			HttpUtility._entities.Add("Sigma", 'Σ');
			HttpUtility._entities.Add("Tau", 'Τ');
			HttpUtility._entities.Add("Upsilon", 'Υ');
			HttpUtility._entities.Add("Phi", 'Φ');
			HttpUtility._entities.Add("Chi", 'Χ');
			HttpUtility._entities.Add("Psi", 'Ψ');
			HttpUtility._entities.Add("Omega", 'Ω');
			HttpUtility._entities.Add("alpha", 'α');
			HttpUtility._entities.Add("beta", 'β');
			HttpUtility._entities.Add("gamma", 'γ');
			HttpUtility._entities.Add("delta", 'δ');
			HttpUtility._entities.Add("epsilon", 'ε');
			HttpUtility._entities.Add("zeta", 'ζ');
			HttpUtility._entities.Add("eta", 'η');
			HttpUtility._entities.Add("theta", 'θ');
			HttpUtility._entities.Add("iota", 'ι');
			HttpUtility._entities.Add("kappa", 'κ');
			HttpUtility._entities.Add("lambda", 'λ');
			HttpUtility._entities.Add("mu", 'μ');
			HttpUtility._entities.Add("nu", 'ν');
			HttpUtility._entities.Add("xi", 'ξ');
			HttpUtility._entities.Add("omicron", 'ο');
			HttpUtility._entities.Add("pi", 'π');
			HttpUtility._entities.Add("rho", 'ρ');
			HttpUtility._entities.Add("sigmaf", 'ς');
			HttpUtility._entities.Add("sigma", 'σ');
			HttpUtility._entities.Add("tau", 'τ');
			HttpUtility._entities.Add("upsilon", 'υ');
			HttpUtility._entities.Add("phi", 'φ');
			HttpUtility._entities.Add("chi", 'χ');
			HttpUtility._entities.Add("psi", 'ψ');
			HttpUtility._entities.Add("omega", 'ω');
			HttpUtility._entities.Add("thetasym", 'ϑ');
			HttpUtility._entities.Add("upsih", 'ϒ');
			HttpUtility._entities.Add("piv", 'ϖ');
			HttpUtility._entities.Add("bull", '•');
			HttpUtility._entities.Add("hellip", '…');
			HttpUtility._entities.Add("prime", '′');
			HttpUtility._entities.Add("Prime", '″');
			HttpUtility._entities.Add("oline", '‾');
			HttpUtility._entities.Add("frasl", '⁄');
			HttpUtility._entities.Add("weierp", '℘');
			HttpUtility._entities.Add("image", 'ℑ');
			HttpUtility._entities.Add("real", 'ℜ');
			HttpUtility._entities.Add("trade", '™');
			HttpUtility._entities.Add("alefsym", 'ℵ');
			HttpUtility._entities.Add("larr", '←');
			HttpUtility._entities.Add("uarr", '↑');
			HttpUtility._entities.Add("rarr", '→');
			HttpUtility._entities.Add("darr", '↓');
			HttpUtility._entities.Add("harr", '↔');
			HttpUtility._entities.Add("crarr", '↵');
			HttpUtility._entities.Add("lArr", '⇐');
			HttpUtility._entities.Add("uArr", '⇑');
			HttpUtility._entities.Add("rArr", '⇒');
			HttpUtility._entities.Add("dArr", '⇓');
			HttpUtility._entities.Add("hArr", '⇔');
			HttpUtility._entities.Add("forall", '∀');
			HttpUtility._entities.Add("part", '∂');
			HttpUtility._entities.Add("exist", '∃');
			HttpUtility._entities.Add("empty", '∅');
			HttpUtility._entities.Add("nabla", '∇');
			HttpUtility._entities.Add("isin", '∈');
			HttpUtility._entities.Add("notin", '∉');
			HttpUtility._entities.Add("ni", '∋');
			HttpUtility._entities.Add("prod", '∏');
			HttpUtility._entities.Add("sum", '∑');
			HttpUtility._entities.Add("minus", '−');
			HttpUtility._entities.Add("lowast", '∗');
			HttpUtility._entities.Add("radic", '√');
			HttpUtility._entities.Add("prop", '∝');
			HttpUtility._entities.Add("infin", '∞');
			HttpUtility._entities.Add("ang", '∠');
			HttpUtility._entities.Add("and", '∧');
			HttpUtility._entities.Add("or", '∨');
			HttpUtility._entities.Add("cap", '∩');
			HttpUtility._entities.Add("cup", '∪');
			HttpUtility._entities.Add("int", '∫');
			HttpUtility._entities.Add("there4", '∴');
			HttpUtility._entities.Add("sim", '∼');
			HttpUtility._entities.Add("cong", '≅');
			HttpUtility._entities.Add("asymp", '≈');
			HttpUtility._entities.Add("ne", '≠');
			HttpUtility._entities.Add("equiv", '≡');
			HttpUtility._entities.Add("le", '≤');
			HttpUtility._entities.Add("ge", '≥');
			HttpUtility._entities.Add("sub", '⊂');
			HttpUtility._entities.Add("sup", '⊃');
			HttpUtility._entities.Add("nsub", '⊄');
			HttpUtility._entities.Add("sube", '⊆');
			HttpUtility._entities.Add("supe", '⊇');
			HttpUtility._entities.Add("oplus", '⊕');
			HttpUtility._entities.Add("otimes", '⊗');
			HttpUtility._entities.Add("perp", '⊥');
			HttpUtility._entities.Add("sdot", '⋅');
			HttpUtility._entities.Add("lceil", '⌈');
			HttpUtility._entities.Add("rceil", '⌉');
			HttpUtility._entities.Add("lfloor", '⌊');
			HttpUtility._entities.Add("rfloor", '⌋');
			HttpUtility._entities.Add("lang", '〈');
			HttpUtility._entities.Add("rang", '〉');
			HttpUtility._entities.Add("loz", '◊');
			HttpUtility._entities.Add("spades", '♠');
			HttpUtility._entities.Add("clubs", '♣');
			HttpUtility._entities.Add("hearts", '♥');
			HttpUtility._entities.Add("diams", '♦');
			HttpUtility._entities.Add("quot", '"');
			HttpUtility._entities.Add("amp", '&');
			HttpUtility._entities.Add("lt", '<');
			HttpUtility._entities.Add("gt", '>');
			HttpUtility._entities.Add("OElig", 'Œ');
			HttpUtility._entities.Add("oelig", 'œ');
			HttpUtility._entities.Add("Scaron", 'Š');
			HttpUtility._entities.Add("scaron", 'š');
			HttpUtility._entities.Add("Yuml", 'Ÿ');
			HttpUtility._entities.Add("circ", 'ˆ');
			HttpUtility._entities.Add("tilde", '˜');
			HttpUtility._entities.Add("ensp", '\u2002');
			HttpUtility._entities.Add("emsp", '\u2003');
			HttpUtility._entities.Add("thinsp", '\u2009');
			HttpUtility._entities.Add("zwnj", '‌');
			HttpUtility._entities.Add("zwj", '‍');
			HttpUtility._entities.Add("lrm", '‎');
			HttpUtility._entities.Add("rlm", '‏');
			HttpUtility._entities.Add("ndash", '–');
			HttpUtility._entities.Add("mdash", '—');
			HttpUtility._entities.Add("lsquo", '‘');
			HttpUtility._entities.Add("rsquo", '’');
			HttpUtility._entities.Add("sbquo", '‚');
			HttpUtility._entities.Add("ldquo", '“');
			HttpUtility._entities.Add("rdquo", '”');
			HttpUtility._entities.Add("bdquo", '„');
			HttpUtility._entities.Add("dagger", '†');
			HttpUtility._entities.Add("Dagger", '‡');
			HttpUtility._entities.Add("permil", '‰');
			HttpUtility._entities.Add("lsaquo", '‹');
			HttpUtility._entities.Add("rsaquo", '›');
			HttpUtility._entities.Add("euro", '€');
		}

		private static string KD(string secret, string data)
		{
			return HttpUtility.Hash(string.Format("{0}:{1}", secret, data));
		}

		private static bool NotEncoded(char c)
		{
			return c == '!' || c == '\'' || c == '(' || c == ')' || c == '*' || c == '-' || c == '.' || c == '_';
		}

		private static void UrlEncodeChar(char c, Stream result, bool isUnicode)
		{
			if (c > 'ÿ')
			{
				result.WriteByte(37);
				result.WriteByte(117);
				int num = (int)(c >> 12);
				result.WriteByte((byte)HttpUtility._hexChars[num]);
				num = (int)(c >> 8 & '\u000f');
				result.WriteByte((byte)HttpUtility._hexChars[num]);
				num = (int)(c >> 4 & '\u000f');
				result.WriteByte((byte)HttpUtility._hexChars[num]);
				num = (int)(c & '\u000f');
				result.WriteByte((byte)HttpUtility._hexChars[num]);
				return;
			}
			if (c > ' ' && HttpUtility.NotEncoded(c))
			{
				result.WriteByte((byte)c);
				return;
			}
			if (c == ' ')
			{
				result.WriteByte(43);
				return;
			}
			if (c < '0' || (c < 'A' && c > '9') || (c > 'Z' && c < 'a') || c > 'z')
			{
				if (isUnicode && c > '\u007f')
				{
					result.WriteByte(37);
					result.WriteByte(117);
					result.WriteByte(48);
					result.WriteByte(48);
				}
				else
				{
					result.WriteByte(37);
				}
				int num2 = (int)(c >> 4);
				result.WriteByte((byte)HttpUtility._hexChars[num2]);
				num2 = (int)(c & '\u000f');
				result.WriteByte((byte)HttpUtility._hexChars[num2]);
			}
			else
			{
				result.WriteByte((byte)c);
			}
		}

		private static void UrlPathEncodeChar(char c, Stream result)
		{
			if (c < '!' || c > '~')
			{
				byte[] bytes = Encoding.UTF8.GetBytes(c.ToString());
				foreach (byte b in bytes)
				{
					result.WriteByte(37);
					int num = b >> 4;
					result.WriteByte((byte)HttpUtility._hexChars[num]);
					num = (int)(b & 15);
					result.WriteByte((byte)HttpUtility._hexChars[num]);
				}
			}
			else if (c == ' ')
			{
				result.WriteByte(37);
				result.WriteByte(50);
				result.WriteByte(48);
			}
			else
			{
				result.WriteByte((byte)c);
			}
		}

		private static void WriteCharBytes(IList buffer, char c, Encoding encoding)
		{
			if (c > 'ÿ')
			{
				foreach (byte b in encoding.GetBytes(new char[]
				{
					c
				}))
				{
					buffer.Add(b);
				}
			}
			else
			{
				buffer.Add((byte)c);
			}
		}

		internal static string CreateBasicAuthChallenge(string realm)
		{
			return string.Format("Basic realm=\"{0}\"", realm);
		}

		internal static string CreateBasicAuthCredentials(string username, string password)
		{
			string s = string.Format("{0}:{1}", username, password);
			string str = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
			return "Basic " + str;
		}

		internal static string CreateDigestAuthChallenge(string realm)
		{
			string text = HttpUtility.CreateNonceValue();
			string text2 = "MD5";
			string text3 = "auth";
			return string.Format("Digest realm=\"{0}\", nonce=\"{1}\", algorithm={2}, qop=\"{3}\"", new object[]
			{
				realm,
				text,
				text2,
				text3
			});
		}

		internal static string CreateDigestAuthCredentials(NameValueCollection authParams)
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.AppendFormat("username=\"{0}\"", authParams["username"]);
			stringBuilder.AppendFormat(", realm=\"{0}\"", authParams["realm"]);
			stringBuilder.AppendFormat(", nonce=\"{0}\"", authParams["nonce"]);
			stringBuilder.AppendFormat(", uri=\"{0}\"", authParams["uri"]);
			string text = authParams["algorithm"];
			if (text != null)
			{
				stringBuilder.AppendFormat(", algorithm={0}", text);
			}
			stringBuilder.AppendFormat(", response=\"{0}\"", authParams["response"]);
			string text2 = authParams["qop"];
			if (text2 != null)
			{
				stringBuilder.AppendFormat(", qop={0}", text2);
				stringBuilder.AppendFormat(", nc={0}", authParams["nc"]);
				stringBuilder.AppendFormat(", cnonce=\"{0}\"", authParams["cnonce"]);
			}
			string text3 = authParams["opaque"];
			if (text3 != null)
			{
				stringBuilder.AppendFormat(", opaque=\"{0}\"", text3);
			}
			return "Digest " + stringBuilder.ToString();
		}

		internal static string CreateNonceValue()
		{
			byte[] array = new byte[16];
			Random random = new Random();
			random.NextBytes(array);
			StringBuilder stringBuilder = new StringBuilder(32);
			foreach (byte b in array)
			{
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		internal static string CreateRequestDigest(NameValueCollection parameters)
		{
			string username = parameters["username"];
			string password = parameters["password"];
			string realm = parameters["realm"];
			string text = parameters["nonce"];
			string uri = parameters["uri"];
			string text2 = parameters["algorithm"];
			string text3 = parameters["qop"];
			string text4 = parameters["nc"];
			string text5 = parameters["cnonce"];
			string method = parameters["method"];
			string value = (text2 == null || !(text2.ToLower() == "md5-sess")) ? HttpUtility.A1(username, password, realm) : HttpUtility.A1(username, password, realm, text, text5);
			string value2 = (text3 == null || !(text3.ToLower() == "auth-int")) ? HttpUtility.A2(method, uri) : HttpUtility.A2(method, uri, parameters["entity"]);
			string secret = HttpUtility.Hash(value);
			string data = (text3 == null) ? string.Format("{0}:{1}", text, HttpUtility.Hash(value2)) : string.Format("{0}:{1}:{2}:{3}:{4}", new object[]
			{
				text,
				text4,
				text5,
				text3,
				HttpUtility.Hash(value2)
			});
			return HttpUtility.KD(secret, data);
		}

		internal static void ParseQueryString(string query, Encoding encoding, NameValueCollection result)
		{
			if (query.Length == 0)
			{
				return;
			}
			string text = HttpUtility.HtmlDecode(query);
			int length = text.Length;
			int i = 0;
			bool flag = true;
			while (i <= length)
			{
				int num = -1;
				int num2 = -1;
				for (int j = i; j < length; j++)
				{
					if (num == -1 && text[j] == '=')
					{
						num = j + 1;
					}
					else if (text[j] == '&')
					{
						num2 = j;
						break;
					}
				}
				if (flag)
				{
					flag = false;
					if (text[i] == '?')
					{
						i++;
					}
				}
				string name;
				if (num == -1)
				{
					name = null;
					num = i;
				}
				else
				{
					name = HttpUtility.UrlDecode(text.Substring(i, num - i - 1), encoding);
				}
				if (num2 < 0)
				{
					i = -1;
					num2 = text.Length;
				}
				else
				{
					i = num2 + 1;
				}
				string val = HttpUtility.UrlDecode(text.Substring(num, num2 - num), encoding);
				result.Add(name, val);
				if (i == -1)
				{
					break;
				}
			}
		}

		internal static string UrlDecodeInternal(byte[] bytes, int offset, int count, Encoding encoding)
		{
			StringBuilder stringBuilder = new StringBuilder();
			MemoryStream memoryStream = new MemoryStream();
			int num = count + offset;
			int i = offset;
			while (i < num)
			{
				if (bytes[i] != 37 || i + 2 >= count || bytes[i + 1] == 37)
				{
					goto IL_C6;
				}
				if (bytes[i + 1] == 117 && i + 5 < num)
				{
					if (memoryStream.Length > 0L)
					{
						stringBuilder.Append(HttpUtility.GetChars(memoryStream, encoding));
						memoryStream.SetLength(0L);
					}
					int @char = HttpUtility.GetChar(bytes, i + 2, 4);
					if (@char == -1)
					{
						goto IL_C6;
					}
					stringBuilder.Append((char)@char);
					i += 5;
				}
				else
				{
					int @char;
					if ((@char = HttpUtility.GetChar(bytes, i + 1, 2)) == -1)
					{
						goto IL_C6;
					}
					memoryStream.WriteByte((byte)@char);
					i += 2;
				}
				IL_10E:
				i++;
				continue;
				IL_C6:
				if (memoryStream.Length > 0L)
				{
					stringBuilder.Append(HttpUtility.GetChars(memoryStream, encoding));
					memoryStream.SetLength(0L);
				}
				if (bytes[i] == 43)
				{
					stringBuilder.Append(' ');
					goto IL_10E;
				}
				stringBuilder.Append((char)bytes[i]);
				goto IL_10E;
			}
			if (memoryStream.Length > 0L)
			{
				stringBuilder.Append(HttpUtility.GetChars(memoryStream, encoding));
			}
			return stringBuilder.ToString();
		}

		internal static byte[] UrlDecodeToBytesInternal(byte[] bytes, int offset, int count)
		{
			MemoryStream memoryStream = new MemoryStream();
			int num = offset + count;
			for (int i = offset; i < num; i++)
			{
				char c = (char)bytes[i];
				if (c == '+')
				{
					c = ' ';
				}
				else if (c == '%' && i < num - 2)
				{
					int @char = HttpUtility.GetChar(bytes, i + 1, 2);
					if (@char != -1)
					{
						c = (char)@char;
						i += 2;
					}
				}
				memoryStream.WriteByte((byte)c);
			}
			return memoryStream.ToArray();
		}

		internal static byte[] UrlEncodeToBytesInternal(byte[] bytes, int offset, int count)
		{
			MemoryStream memoryStream = new MemoryStream(count);
			int num = offset + count;
			for (int i = offset; i < num; i++)
			{
				HttpUtility.UrlEncodeChar((char)bytes[i], memoryStream, false);
			}
			return memoryStream.ToArray();
		}

		internal static byte[] UrlEncodeUnicodeToBytesInternal(string s)
		{
			MemoryStream memoryStream = new MemoryStream(s.Length);
			foreach (char c in s)
			{
				HttpUtility.UrlEncodeChar(c, memoryStream, true);
			}
			return memoryStream.ToArray();
		}

		public static string HtmlAttributeEncode(string s)
		{
			if (s == null || s.Length == 0 || !s.Contains(new char[]
			{
				'&',
				'"',
				'<',
				'>'
			}))
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in s)
			{
				stringBuilder.Append((c != '&') ? ((c != '"') ? ((c != '<') ? ((c != '>') ? c.ToString() : "&gt;") : "&lt;") : "&quot;") : "&amp;");
			}
			return stringBuilder.ToString();
		}

		public static void HtmlAttributeEncode(string s, TextWriter output)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Write(HttpUtility.HtmlAttributeEncode(s));
		}

		public static string HtmlDecode(string s)
		{
			if (s == null || s.Length == 0 || !s.Contains(new char[]
			{
				'&'
			}))
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			int num = 0;
			int num2 = 0;
			bool flag = false;
			foreach (char c in s)
			{
				if (num == 0)
				{
					if (c == '&')
					{
						stringBuilder.Append(c);
						num = 1;
					}
					else
					{
						stringBuilder2.Append(c);
					}
				}
				else if (c == '&')
				{
					num = 1;
					if (flag)
					{
						stringBuilder.Append(num2.ToString(CultureInfo.InvariantCulture));
						flag = false;
					}
					stringBuilder2.Append(stringBuilder.ToString());
					stringBuilder.Length = 0;
					stringBuilder.Append('&');
				}
				else if (num == 1)
				{
					if (c == ';')
					{
						num = 0;
						stringBuilder2.Append(stringBuilder.ToString());
						stringBuilder2.Append(c);
						stringBuilder.Length = 0;
					}
					else
					{
						num2 = 0;
						if (c != '#')
						{
							num = 2;
						}
						else
						{
							num = 3;
						}
						stringBuilder.Append(c);
					}
				}
				else if (num == 2)
				{
					stringBuilder.Append(c);
					if (c == ';')
					{
						string text = stringBuilder.ToString();
						if (text.Length > 1 && HttpUtility.Entities.ContainsKey(text.Substring(1, text.Length - 2)))
						{
							text = HttpUtility.Entities[text.Substring(1, text.Length - 2)].ToString();
						}
						stringBuilder2.Append(text);
						num = 0;
						stringBuilder.Length = 0;
					}
				}
				else if (num == 3)
				{
					if (c == ';')
					{
						if (num2 > 65535)
						{
							stringBuilder2.Append("&#");
							stringBuilder2.Append(num2.ToString(CultureInfo.InvariantCulture));
							stringBuilder2.Append(";");
						}
						else
						{
							stringBuilder2.Append((char)num2);
						}
						num = 0;
						stringBuilder.Length = 0;
						flag = false;
					}
					else if (char.IsDigit(c))
					{
						num2 = num2 * 10 + (int)(c - '0');
						flag = true;
					}
					else
					{
						num = 2;
						if (flag)
						{
							stringBuilder.Append(num2.ToString(CultureInfo.InvariantCulture));
							flag = false;
						}
						stringBuilder.Append(c);
					}
				}
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder2.Append(stringBuilder.ToString());
			}
			else if (flag)
			{
				stringBuilder2.Append(num2.ToString(CultureInfo.InvariantCulture));
			}
			return stringBuilder2.ToString();
		}

		public static void HtmlDecode(string s, TextWriter output)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Write(HttpUtility.HtmlDecode(s));
		}

		public static string HtmlEncode(string s)
		{
			if (s == null || s.Length == 0)
			{
				return s;
			}
			bool flag = false;
			foreach (char c in s)
			{
				if (c == '&' || c == '"' || c == '<' || c == '>' || c > '\u009f')
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c2 in s)
			{
				if (c2 == '&')
				{
					stringBuilder.Append("&amp;");
				}
				else if (c2 == '"')
				{
					stringBuilder.Append("&quot;");
				}
				else if (c2 == '<')
				{
					stringBuilder.Append("&lt;");
				}
				else if (c2 == '>')
				{
					stringBuilder.Append("&gt;");
				}
				else if (c2 > '\u009f')
				{
					stringBuilder.Append("&#");
					StringBuilder stringBuilder2 = stringBuilder;
					int num = (int)c2;
					stringBuilder2.Append(num.ToString(CultureInfo.InvariantCulture));
					stringBuilder.Append(";");
				}
				else
				{
					stringBuilder.Append(c2);
				}
			}
			return stringBuilder.ToString();
		}

		public static void HtmlEncode(string s, TextWriter output)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Write(HttpUtility.HtmlEncode(s));
		}

		public static NameValueCollection ParseQueryString(string query)
		{
			return HttpUtility.ParseQueryString(query, Encoding.UTF8);
		}

		public static NameValueCollection ParseQueryString(string query, Encoding encoding)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}
			int length = query.Length;
			if (length == 0 || (length == 1 && query[0] == '?'))
			{
				return new NameValueCollection();
			}
			if (query[0] == '?')
			{
				query = query.Substring(1);
			}
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			HttpUtility.HttpQSCollection result = new HttpUtility.HttpQSCollection();
			HttpUtility.ParseQueryString(query, encoding, result);
			return result;
		}

		public static string UrlDecode(string s)
		{
			return HttpUtility.UrlDecode(s, Encoding.UTF8);
		}

		public static string UrlDecode(string s, Encoding encoding)
		{
			if (s == null || s.Length == 0 || !s.Contains(new char[]
			{
				'%',
				'+'
			}))
			{
				return s;
			}
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			int length = s.Length;
			List<byte> list = new List<byte>();
			for (int i = 0; i < length; i++)
			{
				char c = s[i];
				if (c == '%' && i + 2 < length && s[i + 1] != '%')
				{
					int @char;
					if (s[i + 1] == 'u' && i + 5 < length)
					{
						@char = HttpUtility.GetChar(s, i + 2, 4);
						if (@char != -1)
						{
							HttpUtility.WriteCharBytes(list, (char)@char, encoding);
							i += 5;
						}
						else
						{
							HttpUtility.WriteCharBytes(list, '%', encoding);
						}
					}
					else if ((@char = HttpUtility.GetChar(s, i + 1, 2)) != -1)
					{
						HttpUtility.WriteCharBytes(list, (char)@char, encoding);
						i += 2;
					}
					else
					{
						HttpUtility.WriteCharBytes(list, '%', encoding);
					}
				}
				else if (c == '+')
				{
					HttpUtility.WriteCharBytes(list, ' ', encoding);
				}
				else
				{
					HttpUtility.WriteCharBytes(list, c, encoding);
				}
			}
			byte[] bytes = list.ToArray();
			return encoding.GetString(bytes);
		}

		public static string UrlDecode(byte[] bytes, Encoding encoding)
		{
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			int count;
			return (bytes != null) ? (((count = bytes.Length) != 0) ? HttpUtility.UrlDecodeInternal(bytes, 0, count, encoding) : string.Empty) : null;
		}

		public static string UrlDecode(byte[] bytes, int offset, int count, Encoding encoding)
		{
			if (bytes == null)
			{
				return null;
			}
			int num = bytes.Length;
			if (num == 0 || count == 0)
			{
				return string.Empty;
			}
			if (offset < 0 || offset >= num)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || count > num - offset)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			return HttpUtility.UrlDecodeInternal(bytes, offset, count, encoding);
		}

		public static byte[] UrlDecodeToBytes(byte[] bytes)
		{
			int count;
			return (bytes != null && (count = bytes.Length) != 0) ? HttpUtility.UrlDecodeToBytesInternal(bytes, 0, count) : bytes;
		}

		public static byte[] UrlDecodeToBytes(string s)
		{
			return HttpUtility.UrlDecodeToBytes(s, Encoding.UTF8);
		}

		public static byte[] UrlDecodeToBytes(string s, Encoding encoding)
		{
			if (s == null)
			{
				return null;
			}
			if (s.Length == 0)
			{
				return new byte[0];
			}
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			byte[] bytes = encoding.GetBytes(s);
			return HttpUtility.UrlDecodeToBytesInternal(bytes, 0, bytes.Length);
		}

		public static byte[] UrlDecodeToBytes(byte[] bytes, int offset, int count)
		{
			int num;
			if (bytes == null || (num = bytes.Length) == 0)
			{
				return bytes;
			}
			if (count == 0)
			{
				return new byte[0];
			}
			if (offset < 0 || offset >= num)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || count > num - offset)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			return HttpUtility.UrlDecodeToBytesInternal(bytes, offset, count);
		}

		public static string UrlEncode(byte[] bytes)
		{
			int count;
			return (bytes != null) ? (((count = bytes.Length) != 0) ? Encoding.ASCII.GetString(HttpUtility.UrlEncodeToBytesInternal(bytes, 0, count)) : string.Empty) : null;
		}

		public static string UrlEncode(string s)
		{
			return HttpUtility.UrlEncode(s, Encoding.UTF8);
		}

		public static string UrlEncode(string s, Encoding encoding)
		{
			int length;
			if (s == null || (length = s.Length) == 0)
			{
				return s;
			}
			bool flag = false;
			foreach (char c in s)
			{
				if (c < '0' || (c < 'A' && c > '9') || (c > 'Z' && c < 'a') || c > 'z')
				{
					if (!HttpUtility.NotEncoded(c))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return s;
			}
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			byte[] bytes = new byte[encoding.GetMaxByteCount(length)];
			int bytes2 = encoding.GetBytes(s, 0, length, bytes, 0);
			return Encoding.ASCII.GetString(HttpUtility.UrlEncodeToBytesInternal(bytes, 0, bytes2));
		}

		public static string UrlEncode(byte[] bytes, int offset, int count)
		{
			byte[] array = HttpUtility.UrlEncodeToBytes(bytes, offset, count);
			return (array != null) ? ((array.Length != 0) ? Encoding.ASCII.GetString(array) : string.Empty) : null;
		}

		public static byte[] UrlEncodeToBytes(byte[] bytes)
		{
			int count;
			return (bytes != null && (count = bytes.Length) != 0) ? HttpUtility.UrlEncodeToBytesInternal(bytes, 0, count) : bytes;
		}

		public static byte[] UrlEncodeToBytes(string s)
		{
			return HttpUtility.UrlEncodeToBytes(s, Encoding.UTF8);
		}

		public static byte[] UrlEncodeToBytes(string s, Encoding encoding)
		{
			if (s == null)
			{
				return null;
			}
			if (s.Length == 0)
			{
				return new byte[0];
			}
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			byte[] bytes = encoding.GetBytes(s);
			return HttpUtility.UrlEncodeToBytesInternal(bytes, 0, bytes.Length);
		}

		public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
		{
			int num;
			if (bytes == null || (num = bytes.Length) == 0)
			{
				return bytes;
			}
			if (count == 0)
			{
				return new byte[0];
			}
			if (offset < 0 || offset >= num)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || count > num - offset)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			return HttpUtility.UrlEncodeToBytesInternal(bytes, offset, count);
		}

		public static string UrlEncodeUnicode(string s)
		{
			return (s != null && s.Length != 0) ? Encoding.ASCII.GetString(HttpUtility.UrlEncodeUnicodeToBytesInternal(s)) : s;
		}

		public static byte[] UrlEncodeUnicodeToBytes(string s)
		{
			if (s == null)
			{
				return null;
			}
			if (s.Length == 0)
			{
				return new byte[0];
			}
			return HttpUtility.UrlEncodeUnicodeToBytesInternal(s);
		}

		public static string UrlPathEncode(string s)
		{
			if (s == null || s.Length == 0)
			{
				return s;
			}
			MemoryStream memoryStream = new MemoryStream();
			foreach (char c in s)
			{
				HttpUtility.UrlPathEncodeChar(c, memoryStream);
			}
			return Encoding.ASCII.GetString(memoryStream.ToArray());
		}

		private sealed class HttpQSCollection : NameValueCollection
		{
			public override string ToString()
			{
				if (this.Count == 0)
				{
					return string.Empty;
				}
				StringBuilder stringBuilder = new StringBuilder();
				string[] allKeys = this.AllKeys;
				foreach (string text in allKeys)
				{
					stringBuilder.AppendFormat("{0}={1}&", text, base[text]);
				}
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Length--;
				}
				return stringBuilder.ToString();
			}
		}
	}
}
