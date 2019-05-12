using System;

namespace UnityEngine
{
	internal interface IInterval
	{
		long intervalStart { get; }

		long intervalEnd { get; }

		int intervalBit { get; set; }
	}
}
