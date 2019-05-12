using System;

namespace UnityEngine.Experimental.UIElements
{
	public abstract class FocusEventBase<T> : EventBase<T>, IFocusEvent, IPropagatableEvent where T : FocusEventBase<T>, new()
	{
		protected FocusEventBase()
		{
			this.Init();
		}

		public Focusable relatedTarget { get; protected set; }

		public FocusChangeDirection direction { get; protected set; }

		protected override void Init()
		{
			base.Init();
			this.flags = EventBase.EventFlags.Capturable;
			this.relatedTarget = null;
			this.direction = FocusChangeDirection.unspecified;
		}

		public static T GetPooled(IEventHandler target, Focusable relatedTarget, FocusChangeDirection direction)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.target = target;
			pooled.relatedTarget = relatedTarget;
			pooled.direction = direction;
			return pooled;
		}
	}
}
