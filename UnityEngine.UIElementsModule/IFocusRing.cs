using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IFocusRing
	{
		FocusChangeDirection GetFocusChangeDirection(Focusable currentFocusable, EventBase e);

		Focusable GetNextFocusable(Focusable currentFocusable, FocusChangeDirection direction);
	}
}
