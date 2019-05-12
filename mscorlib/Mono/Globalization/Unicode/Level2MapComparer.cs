using System;
using System.Collections;

namespace Mono.Globalization.Unicode
{
	internal class Level2MapComparer : IComparer
	{
		public static readonly Level2MapComparer Instance = new Level2MapComparer();

		public int Compare(object o1, object o2)
		{
			Level2Map level2Map = (Level2Map)o1;
			Level2Map level2Map2 = (Level2Map)o2;
			return (int)(level2Map.Source - level2Map2.Source);
		}
	}
}
