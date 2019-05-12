using System;

namespace TMPro
{
	public struct KerningPairKey
	{
		public uint ascii_Left;

		public uint ascii_Right;

		public uint key;

		public KerningPairKey(uint ascii_left, uint ascii_right)
		{
			this.ascii_Left = ascii_left;
			this.ascii_Right = ascii_right;
			this.key = (ascii_right << 16) + ascii_left;
		}
	}
}
