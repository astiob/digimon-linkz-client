using System;

namespace UnityEngine
{
	public enum EventType
	{
		MouseDown,
		MouseUp,
		MouseMove,
		MouseDrag,
		KeyDown,
		KeyUp,
		ScrollWheel,
		Repaint,
		Layout,
		DragUpdated,
		DragPerform,
		DragExited = 15,
		Ignore = 11,
		Used,
		ValidateCommand,
		ExecuteCommand,
		ContextClick = 16,
		MouseEnterWindow = 20,
		MouseLeaveWindow,
		[Obsolete("Use MouseDown instead (UnityUpgradable) -> MouseDown", true)]
		mouseDown = 0,
		[Obsolete("Use MouseUp instead (UnityUpgradable) -> MouseUp", true)]
		mouseUp,
		[Obsolete("Use MouseMove instead (UnityUpgradable) -> MouseMove", true)]
		mouseMove,
		[Obsolete("Use MouseDrag instead (UnityUpgradable) -> MouseDrag", true)]
		mouseDrag,
		[Obsolete("Use KeyDown instead (UnityUpgradable) -> KeyDown", true)]
		keyDown,
		[Obsolete("Use KeyUp instead (UnityUpgradable) -> KeyUp", true)]
		keyUp,
		[Obsolete("Use ScrollWheel instead (UnityUpgradable) -> ScrollWheel", true)]
		scrollWheel,
		[Obsolete("Use Repaint instead (UnityUpgradable) -> Repaint", true)]
		repaint,
		[Obsolete("Use Layout instead (UnityUpgradable) -> Layout", true)]
		layout,
		[Obsolete("Use DragUpdated instead (UnityUpgradable) -> DragUpdated", true)]
		dragUpdated,
		[Obsolete("Use DragPerform instead (UnityUpgradable) -> DragPerform", true)]
		dragPerform,
		[Obsolete("Use Ignore instead (UnityUpgradable) -> Ignore", true)]
		ignore,
		[Obsolete("Use Used instead (UnityUpgradable) -> Used", true)]
		used
	}
}
