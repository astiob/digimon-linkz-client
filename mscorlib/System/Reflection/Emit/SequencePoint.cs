using System;

namespace System.Reflection.Emit
{
	internal struct SequencePoint
	{
		public int Offset;

		public int Line;

		public int Col;

		public int EndLine;

		public int EndCol;
	}
}
