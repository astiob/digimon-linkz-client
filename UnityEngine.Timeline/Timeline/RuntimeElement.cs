using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	internal abstract class RuntimeElement : IInterval
	{
		public abstract long intervalStart { get; }

		public abstract long intervalEnd { get; }

		public int intervalBit { get; set; }

		public abstract bool enable { set; }

		public abstract void EvaluateAt(double localTime, FrameData frameData);
	}
}
