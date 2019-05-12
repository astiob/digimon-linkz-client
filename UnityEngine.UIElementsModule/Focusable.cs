using System;

namespace UnityEngine.Experimental.UIElements
{
	public abstract class Focusable : CallbackEventHandler
	{
		private int m_FocusIndex;

		protected Focusable()
		{
			this.m_FocusIndex = 0;
		}

		public abstract FocusController focusController { get; }

		public int focusIndex
		{
			get
			{
				return this.m_FocusIndex;
			}
			set
			{
				this.m_FocusIndex = value;
			}
		}

		public virtual bool canGrabFocus
		{
			get
			{
				return this.m_FocusIndex >= 0;
			}
		}

		public virtual void Focus()
		{
			if (this.focusController != null)
			{
				this.focusController.SwitchFocus((!this.canGrabFocus) ? null : this);
			}
		}

		public void Blur()
		{
			if (this.focusController != null && this.focusController.focusedElement == this)
			{
				this.focusController.SwitchFocus(null);
			}
		}

		protected internal override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			if (evt.GetEventTypeId() == EventBase<MouseDownEvent>.TypeId())
			{
				MouseDownEvent mouseDownEvent = evt as MouseDownEvent;
				if (mouseDownEvent.button == 0)
				{
					this.Focus();
				}
			}
			if (this.focusController != null)
			{
				this.focusController.SwitchFocusOnEvent(evt);
			}
		}
	}
}
