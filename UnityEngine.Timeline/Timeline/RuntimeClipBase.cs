using System;

namespace UnityEngine.Timeline
{
	internal abstract class RuntimeClipBase : RuntimeElement
	{
		public abstract double start { get; }

		public abstract double duration { get; }

		public override long intervalStart
		{
			get
			{
				return DiscreteTime.GetNearestTick(this.start);
			}
		}

		public override long intervalEnd
		{
			get
			{
				return DiscreteTime.GetNearestTick(this.start + this.duration);
			}
		}
	}
}
