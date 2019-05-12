using System;

namespace UnityEngine.Experimental.UIElements
{
	public class PostLayoutEvent : EventBase<PostLayoutEvent>, IPropagatableEvent
	{
		public PostLayoutEvent()
		{
			this.Init();
		}

		public static PostLayoutEvent GetPooled(bool hasNewLayout)
		{
			PostLayoutEvent pooled = EventBase<PostLayoutEvent>.GetPooled();
			pooled.hasNewLayout = hasNewLayout;
			return pooled;
		}

		protected override void Init()
		{
			base.Init();
			this.hasNewLayout = false;
		}

		public bool hasNewLayout { get; private set; }
	}
}
