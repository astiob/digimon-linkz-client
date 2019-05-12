using System;

namespace UnityEngine.Experimental.UIElements
{
	public class FocusController
	{
		public FocusController(IFocusRing focusRing)
		{
			this.focusRing = focusRing;
			this.focusedElement = null;
			this.imguiKeyboardControl = 0;
		}

		private IFocusRing focusRing { get; set; }

		public Focusable focusedElement { get; private set; }

		private static void AboutToReleaseFocus(Focusable focusable, Focusable willGiveFocusTo, FocusChangeDirection direction)
		{
			FocusOutEvent pooled = FocusEventBase<FocusOutEvent>.GetPooled(focusable, willGiveFocusTo, direction);
			UIElementsUtility.eventDispatcher.DispatchEvent(pooled, null);
			EventBase<FocusOutEvent>.ReleasePooled(pooled);
		}

		private static void ReleaseFocus(Focusable focusable, Focusable willGiveFocusTo, FocusChangeDirection direction)
		{
			BlurEvent pooled = FocusEventBase<BlurEvent>.GetPooled(focusable, willGiveFocusTo, direction);
			UIElementsUtility.eventDispatcher.DispatchEvent(pooled, null);
			EventBase<BlurEvent>.ReleasePooled(pooled);
		}

		private static void AboutToGrabFocus(Focusable focusable, Focusable willTakeFocusFrom, FocusChangeDirection direction)
		{
			FocusInEvent pooled = FocusEventBase<FocusInEvent>.GetPooled(focusable, willTakeFocusFrom, direction);
			UIElementsUtility.eventDispatcher.DispatchEvent(pooled, null);
			EventBase<FocusInEvent>.ReleasePooled(pooled);
		}

		private static void GrabFocus(Focusable focusable, Focusable willTakeFocusFrom, FocusChangeDirection direction)
		{
			FocusEvent pooled = FocusEventBase<FocusEvent>.GetPooled(focusable, willTakeFocusFrom, direction);
			UIElementsUtility.eventDispatcher.DispatchEvent(pooled, null);
			EventBase<FocusEvent>.ReleasePooled(pooled);
		}

		internal void SwitchFocus(Focusable newFocusedElement)
		{
			this.SwitchFocus(newFocusedElement, FocusChangeDirection.unspecified);
		}

		private void SwitchFocus(Focusable newFocusedElement, FocusChangeDirection direction)
		{
			if (newFocusedElement != this.focusedElement)
			{
				Focusable focusedElement = this.focusedElement;
				if (newFocusedElement == null || !newFocusedElement.canGrabFocus)
				{
					if (focusedElement != null)
					{
						FocusController.AboutToReleaseFocus(focusedElement, newFocusedElement, direction);
						this.focusedElement = null;
						FocusController.ReleaseFocus(focusedElement, newFocusedElement, direction);
					}
				}
				else if (newFocusedElement != focusedElement)
				{
					if (focusedElement != null)
					{
						FocusController.AboutToReleaseFocus(focusedElement, newFocusedElement, direction);
					}
					FocusController.AboutToGrabFocus(newFocusedElement, focusedElement, direction);
					this.focusedElement = newFocusedElement;
					if (focusedElement != null)
					{
						FocusController.ReleaseFocus(focusedElement, newFocusedElement, direction);
					}
					FocusController.GrabFocus(newFocusedElement, focusedElement, direction);
				}
			}
		}

		public void SwitchFocusOnEvent(EventBase e)
		{
			FocusChangeDirection focusChangeDirection = this.focusRing.GetFocusChangeDirection(this.focusedElement, e);
			if (focusChangeDirection != FocusChangeDirection.none)
			{
				Focusable nextFocusable = this.focusRing.GetNextFocusable(this.focusedElement, focusChangeDirection);
				this.SwitchFocus(nextFocusable, focusChangeDirection);
			}
		}

		internal int imguiKeyboardControl { get; set; }

		internal void SyncIMGUIFocus(IMGUIContainer imguiContainerHavingKeyboardControl)
		{
			if (GUIUtility.keyboardControl != this.imguiKeyboardControl)
			{
				this.imguiKeyboardControl = GUIUtility.keyboardControl;
				if (GUIUtility.keyboardControl != 0)
				{
					this.SwitchFocus(imguiContainerHavingKeyboardControl, FocusChangeDirection.unspecified);
				}
				else
				{
					this.SwitchFocus(null, FocusChangeDirection.unspecified);
				}
			}
		}
	}
}
