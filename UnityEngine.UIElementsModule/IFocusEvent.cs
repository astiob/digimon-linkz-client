using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IFocusEvent
	{
		Focusable relatedTarget { get; }

		FocusChangeDirection direction { get; }
	}
}
