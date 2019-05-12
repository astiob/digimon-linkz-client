using System;

namespace Mono.Xml
{
	internal class SmallXmlParserException : SystemException
	{
		private int line;

		private int column;

		public SmallXmlParserException(string msg, int line, int column) : base(string.Format("{0}. At ({1},{2})", msg, line, column))
		{
			this.line = line;
			this.column = column;
		}

		public int Line
		{
			get
			{
				return this.line;
			}
		}

		public int Column
		{
			get
			{
				return this.column;
			}
		}
	}
}
