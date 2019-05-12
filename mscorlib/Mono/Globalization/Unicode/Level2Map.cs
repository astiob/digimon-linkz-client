using System;

namespace Mono.Globalization.Unicode
{
	internal class Level2Map
	{
		public byte Source;

		public byte Replace;

		public Level2Map(byte source, byte replace)
		{
			this.Source = source;
			this.Replace = replace;
		}
	}
}
