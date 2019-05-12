using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal class AnchorInfo
	{
		private Expression expr;

		private Position pos;

		private int offset;

		private string str;

		private int width;

		private bool ignore;

		public AnchorInfo(Expression expr, int width)
		{
			this.expr = expr;
			this.offset = 0;
			this.width = width;
			this.str = null;
			this.ignore = false;
			this.pos = Position.Any;
		}

		public AnchorInfo(Expression expr, int offset, int width, string str, bool ignore)
		{
			this.expr = expr;
			this.offset = offset;
			this.width = width;
			this.str = ((!ignore) ? str : str.ToLower());
			this.ignore = ignore;
			this.pos = Position.Any;
		}

		public AnchorInfo(Expression expr, int offset, int width, Position pos)
		{
			this.expr = expr;
			this.offset = offset;
			this.width = width;
			this.pos = pos;
			this.str = null;
			this.ignore = false;
		}

		public Expression Expression
		{
			get
			{
				return this.expr;
			}
		}

		public int Offset
		{
			get
			{
				return this.offset;
			}
		}

		public int Width
		{
			get
			{
				return this.width;
			}
		}

		public int Length
		{
			get
			{
				return (this.str == null) ? 0 : this.str.Length;
			}
		}

		public bool IsUnknownWidth
		{
			get
			{
				return this.width < 0;
			}
		}

		public bool IsComplete
		{
			get
			{
				return this.Length == this.Width;
			}
		}

		public string Substring
		{
			get
			{
				return this.str;
			}
		}

		public bool IgnoreCase
		{
			get
			{
				return this.ignore;
			}
		}

		public Position Position
		{
			get
			{
				return this.pos;
			}
		}

		public bool IsSubstring
		{
			get
			{
				return this.str != null;
			}
		}

		public bool IsPosition
		{
			get
			{
				return this.pos != Position.Any;
			}
		}

		public Interval GetInterval()
		{
			return this.GetInterval(0);
		}

		public Interval GetInterval(int start)
		{
			if (!this.IsSubstring)
			{
				return Interval.Empty;
			}
			return new Interval(start + this.Offset, start + this.Offset + this.Length - 1);
		}
	}
}
