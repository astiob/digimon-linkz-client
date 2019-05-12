using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IMouseEvent
	{
		EventModifiers modifiers { get; }

		Vector2 mousePosition { get; }

		Vector2 localMousePosition { get; }

		Vector2 mouseDelta { get; }

		int clickCount { get; }

		int button { get; }

		bool shiftKey { get; }

		bool ctrlKey { get; }

		bool commandKey { get; }

		bool altKey { get; }
	}
}
