using System;

namespace Mono.Globalization.Unicode
{
	internal class CodePointIndexer
	{
		private readonly CodePointIndexer.TableRange[] ranges;

		public readonly int TotalCount;

		private int defaultIndex;

		private int defaultCP;

		public CodePointIndexer(int[] starts, int[] ends, int defaultIndex, int defaultCP)
		{
			this.defaultIndex = defaultIndex;
			this.defaultCP = defaultCP;
			this.ranges = new CodePointIndexer.TableRange[starts.Length];
			for (int i = 0; i < this.ranges.Length; i++)
			{
				this.ranges[i] = new CodePointIndexer.TableRange(starts[i], ends[i], (i != 0) ? (this.ranges[i - 1].IndexStart + this.ranges[i - 1].Count) : 0);
			}
			for (int j = 0; j < this.ranges.Length; j++)
			{
				this.TotalCount += this.ranges[j].Count;
			}
		}

		public static Array CompressArray(Array source, Type type, CodePointIndexer indexer)
		{
			int num = 0;
			for (int i = 0; i < indexer.ranges.Length; i++)
			{
				num += indexer.ranges[i].Count;
			}
			Array array = Array.CreateInstance(type, num);
			for (int j = 0; j < indexer.ranges.Length; j++)
			{
				Array.Copy(source, indexer.ranges[j].Start, array, indexer.ranges[j].IndexStart, indexer.ranges[j].Count);
			}
			return array;
		}

		public int ToIndex(int cp)
		{
			for (int i = 0; i < this.ranges.Length; i++)
			{
				if (cp < this.ranges[i].Start)
				{
					return this.defaultIndex;
				}
				if (cp < this.ranges[i].End)
				{
					return cp - this.ranges[i].Start + this.ranges[i].IndexStart;
				}
			}
			return this.defaultIndex;
		}

		public int ToCodePoint(int i)
		{
			for (int j = 0; j < this.ranges.Length; j++)
			{
				if (i < this.ranges[j].IndexStart)
				{
					return this.defaultCP;
				}
				if (i < this.ranges[j].IndexEnd)
				{
					return i - this.ranges[j].IndexStart + this.ranges[j].Start;
				}
			}
			return this.defaultCP;
		}

		[Serializable]
		internal struct TableRange
		{
			public readonly int Start;

			public readonly int End;

			public readonly int Count;

			public readonly int IndexStart;

			public readonly int IndexEnd;

			public TableRange(int start, int end, int indexStart)
			{
				this.Start = start;
				this.End = end;
				this.Count = this.End - this.Start;
				this.IndexStart = indexStart;
				this.IndexEnd = this.IndexStart + this.Count;
			}
		}
	}
}
