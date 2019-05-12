using System;

namespace System.Net
{
	internal class DigestHeaderParser
	{
		private string header;

		private int length;

		private int pos;

		private static string[] keywords = new string[]
		{
			"realm",
			"opaque",
			"nonce",
			"algorithm",
			"qop"
		};

		private string[] values = new string[DigestHeaderParser.keywords.Length];

		public DigestHeaderParser(string header)
		{
			this.header = header.Trim();
		}

		public string Realm
		{
			get
			{
				return this.values[0];
			}
		}

		public string Opaque
		{
			get
			{
				return this.values[1];
			}
		}

		public string Nonce
		{
			get
			{
				return this.values[2];
			}
		}

		public string Algorithm
		{
			get
			{
				return this.values[3];
			}
		}

		public string QOP
		{
			get
			{
				return this.values[4];
			}
		}

		public bool Parse()
		{
			if (!this.header.ToLower().StartsWith("digest "))
			{
				return false;
			}
			this.pos = 6;
			this.length = this.header.Length;
			while (this.pos < this.length)
			{
				string value;
				string text;
				if (!this.GetKeywordAndValue(out value, out text))
				{
					return false;
				}
				this.SkipWhitespace();
				if (this.pos < this.length && this.header[this.pos] == ',')
				{
					this.pos++;
				}
				int num = Array.IndexOf<string>(DigestHeaderParser.keywords, value);
				if (num != -1)
				{
					if (this.values[num] != null)
					{
						return false;
					}
					this.values[num] = text;
				}
			}
			return this.Realm != null && this.Nonce != null;
		}

		private void SkipWhitespace()
		{
			char c = ' ';
			while (this.pos < this.length && (c == ' ' || c == '\t' || c == '\r' || c == '\n'))
			{
				c = this.header[this.pos++];
			}
			this.pos--;
		}

		private string GetKey()
		{
			this.SkipWhitespace();
			int num = this.pos;
			while (this.pos < this.length && this.header[this.pos] != '=')
			{
				this.pos++;
			}
			return this.header.Substring(num, this.pos - num).Trim().ToLower();
		}

		private bool GetKeywordAndValue(out string key, out string value)
		{
			key = null;
			value = null;
			key = this.GetKey();
			if (this.pos >= this.length)
			{
				return false;
			}
			this.SkipWhitespace();
			if (this.pos + 1 >= this.length || this.header[this.pos++] != '=')
			{
				return false;
			}
			this.SkipWhitespace();
			if (this.pos + 1 >= this.length)
			{
				return false;
			}
			bool flag = false;
			if (this.header[this.pos] == '"')
			{
				this.pos++;
				flag = true;
			}
			int num = this.pos;
			if (flag)
			{
				this.pos = this.header.IndexOf('"', this.pos);
				if (this.pos == -1)
				{
					return false;
				}
			}
			else
			{
				do
				{
					char c = this.header[this.pos];
					if (c == ',' || c == ' ' || c == '\t' || c == '\r' || c == '\n')
					{
						break;
					}
				}
				while (++this.pos < this.length);
				if (this.pos >= this.length && num == this.pos)
				{
					return false;
				}
			}
			value = this.header.Substring(num, this.pos - num);
			this.pos += 2;
			return true;
		}
	}
}
