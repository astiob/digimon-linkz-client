using System;

namespace System.Net
{
	internal class CookieParser
	{
		private string header;

		private int pos;

		private int length;

		public CookieParser(string header) : this(header, 0)
		{
		}

		public CookieParser(string header, int position)
		{
			this.header = header;
			this.pos = position;
			this.length = header.Length;
		}

		public bool GetNextNameValue(out string name, out string val)
		{
			name = null;
			val = null;
			if (this.pos >= this.length)
			{
				return false;
			}
			name = this.GetCookieName();
			if (this.pos < this.header.Length && this.header[this.pos] == '=')
			{
				this.pos++;
				val = this.GetCookieValue();
			}
			if (this.pos < this.length && this.header[this.pos] == ';')
			{
				this.pos++;
			}
			return true;
		}

		private string GetCookieName()
		{
			int num = this.pos;
			while (num < this.length && char.IsWhiteSpace(this.header[num]))
			{
				num++;
			}
			int num2 = num;
			while (num < this.length && this.header[num] != ';' && this.header[num] != '=')
			{
				num++;
			}
			this.pos = num;
			return this.header.Substring(num2, num - num2).Trim();
		}

		private string GetCookieValue()
		{
			if (this.pos >= this.length)
			{
				return null;
			}
			int num = this.pos;
			while (num < this.length && char.IsWhiteSpace(this.header[num]))
			{
				num++;
			}
			int num2;
			if (this.header[num] == '"')
			{
				num = (num2 = num + 1);
				while (num < this.length && this.header[num] != '"')
				{
					num++;
				}
				int num3 = num;
				while (num3 < this.length && this.header[num3] != ';')
				{
					num3++;
				}
				this.pos = num3;
			}
			else
			{
				num2 = num;
				while (num < this.length && this.header[num] != ';')
				{
					num++;
				}
				this.pos = num;
			}
			return this.header.Substring(num2, num - num2).Trim();
		}
	}
}
