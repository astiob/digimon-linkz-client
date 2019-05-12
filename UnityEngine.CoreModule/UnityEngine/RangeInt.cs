using System;

namespace UnityEngine
{
	public struct RangeInt
	{
		public int start;

		public int length;

		public RangeInt(int start, int length)
		{
			this.start = start;
			this.length = length;
		}

		public int end
		{
			get
			{
				return this.start + this.length;
			}
		}
	}
}
