using System;

namespace System.Resources
{
	internal class ICONDIRENTRY
	{
		public byte bWidth;

		public byte bHeight;

		public byte bColorCount;

		public byte bReserved;

		public short wPlanes;

		public short wBitCount;

		public int dwBytesInRes;

		public int dwImageOffset;

		public byte[] image;

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"ICONDIRENTRY (",
				this.bWidth,
				"x",
				this.bHeight,
				" ",
				this.wBitCount,
				" bpp)"
			});
		}
	}
}
