using System;

namespace System.Reflection.Emit
{
	internal struct ILExceptionBlock
	{
		public const int CATCH = 0;

		public const int FILTER = 1;

		public const int FINALLY = 2;

		public const int FAULT = 4;

		public const int FILTER_START = -1;

		internal Type extype;

		internal int type;

		internal int start;

		internal int len;

		internal int filter_offset;

		internal void Debug()
		{
		}
	}
}
