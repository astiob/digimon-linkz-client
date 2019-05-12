using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Types of UnityGUI input and processing events.</para>
	/// </summary>
	public enum EventType
	{
		/// <summary>
		///   <para>Mouse button was pressed.</para>
		/// </summary>
		MouseDown,
		/// <summary>
		///   <para>Mouse button was released.</para>
		/// </summary>
		MouseUp,
		/// <summary>
		///   <para>Mouse was moved (editor views only).</para>
		/// </summary>
		MouseMove,
		/// <summary>
		///   <para>Mouse was dragged.</para>
		/// </summary>
		MouseDrag,
		/// <summary>
		///   <para>A keyboard key was pressed.</para>
		/// </summary>
		KeyDown,
		/// <summary>
		///   <para>A keyboard key was released.</para>
		/// </summary>
		KeyUp,
		/// <summary>
		///   <para>The scroll wheel was moved.</para>
		/// </summary>
		ScrollWheel,
		/// <summary>
		///   <para>A repaint event. One is sent every frame.</para>
		/// </summary>
		Repaint,
		/// <summary>
		///   <para>A layout event.</para>
		/// </summary>
		Layout,
		/// <summary>
		///   <para>Editor only: drag &amp; drop operation updated.</para>
		/// </summary>
		DragUpdated,
		/// <summary>
		///   <para>Editor only: drag &amp; drop operation performed.</para>
		/// </summary>
		DragPerform,
		/// <summary>
		///   <para>Editor only: drag &amp; drop operation exited.</para>
		/// </summary>
		DragExited = 15,
		/// <summary>
		///   <para>Event should be ignored.</para>
		/// </summary>
		Ignore = 11,
		/// <summary>
		///   <para>Already processed event.</para>
		/// </summary>
		Used,
		/// <summary>
		///   <para>Validates a special command (e.g. copy &amp; paste).</para>
		/// </summary>
		ValidateCommand,
		/// <summary>
		///   <para>Execute a special command (eg. copy &amp; paste).</para>
		/// </summary>
		ExecuteCommand,
		/// <summary>
		///   <para>User has right-clicked (or control-clicked on the mac).</para>
		/// </summary>
		ContextClick = 16,
		mouseDown = 0,
		mouseUp,
		mouseMove,
		mouseDrag,
		keyDown,
		keyUp,
		scrollWheel,
		repaint,
		layout,
		dragUpdated,
		dragPerform,
		ignore,
		used
	}
}
