using System;
using System.Diagnostics.SymbolStore;

namespace System.Reflection.Emit
{
	internal class SequencePointList
	{
		private const int arrayGrow = 10;

		private ISymbolDocumentWriter doc;

		private SequencePoint[] points;

		private int count;

		public SequencePointList(ISymbolDocumentWriter doc)
		{
			this.doc = doc;
		}

		public ISymbolDocumentWriter Document
		{
			get
			{
				return this.doc;
			}
		}

		public int[] GetOffsets()
		{
			int[] array = new int[this.count];
			for (int i = 0; i < this.count; i++)
			{
				array[i] = this.points[i].Offset;
			}
			return array;
		}

		public int[] GetLines()
		{
			int[] array = new int[this.count];
			for (int i = 0; i < this.count; i++)
			{
				array[i] = this.points[i].Line;
			}
			return array;
		}

		public int[] GetColumns()
		{
			int[] array = new int[this.count];
			for (int i = 0; i < this.count; i++)
			{
				array[i] = this.points[i].Col;
			}
			return array;
		}

		public int[] GetEndLines()
		{
			int[] array = new int[this.count];
			for (int i = 0; i < this.count; i++)
			{
				array[i] = this.points[i].EndLine;
			}
			return array;
		}

		public int[] GetEndColumns()
		{
			int[] array = new int[this.count];
			for (int i = 0; i < this.count; i++)
			{
				array[i] = this.points[i].EndCol;
			}
			return array;
		}

		public int StartLine
		{
			get
			{
				return this.points[0].Line;
			}
		}

		public int EndLine
		{
			get
			{
				return this.points[this.count - 1].Line;
			}
		}

		public int StartColumn
		{
			get
			{
				return this.points[0].Col;
			}
		}

		public int EndColumn
		{
			get
			{
				return this.points[this.count - 1].Col;
			}
		}

		public void AddSequencePoint(int offset, int line, int col, int endLine, int endCol)
		{
			SequencePoint sequencePoint = default(SequencePoint);
			sequencePoint.Offset = offset;
			sequencePoint.Line = line;
			sequencePoint.Col = col;
			sequencePoint.EndLine = endLine;
			sequencePoint.EndCol = endCol;
			if (this.points == null)
			{
				this.points = new SequencePoint[10];
			}
			else if (this.count >= this.points.Length)
			{
				SequencePoint[] destinationArray = new SequencePoint[this.count + 10];
				Array.Copy(this.points, destinationArray, this.points.Length);
				this.points = destinationArray;
			}
			this.points[this.count] = sequencePoint;
			this.count++;
		}
	}
}
