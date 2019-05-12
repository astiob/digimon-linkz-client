using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IKeyboardEvent
	{
		EventModifiers modifiers { get; }

		char character { get; }

		KeyCode keyCode { get; }

		bool shiftKey { get; }

		bool ctrlKey { get; }

		bool commandKey { get; }

		bool altKey { get; }
	}
}
