using System;
using System.Diagnostics;

namespace UnityEngine.Experimental.UIElements
{
	internal class ClampedDragger : Clickable
	{
		public ClampedDragger(Slider slider, Action clickHandler, Action dragHandler) : base(clickHandler, 250L, 30L)
		{
			this.dragDirection = ClampedDragger.DragDirection.None;
			this.slider = slider;
			this.dragging += dragHandler;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action dragging;

		public ClampedDragger.DragDirection dragDirection { get; set; }

		private Slider slider { get; set; }

		public Vector2 startMousePosition { get; private set; }

		public Vector2 delta
		{
			get
			{
				return base.lastMousePosition - this.startMousePosition;
			}
		}

		protected override void RegisterCallbacksOnTarget()
		{
			base.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			base.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
			base.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(base.OnMouseUp), Capture.NoCapture);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			base.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			base.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
			base.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(base.OnMouseUp), Capture.NoCapture);
		}

		private new void OnMouseDown(MouseDownEvent evt)
		{
			if (base.CanStartManipulation(evt))
			{
				this.startMousePosition = evt.localMousePosition;
				this.dragDirection = ClampedDragger.DragDirection.None;
				base.OnMouseDown(evt);
			}
		}

		private new void OnMouseMove(MouseMoveEvent evt)
		{
			if (base.target.HasCapture())
			{
				base.OnMouseMove(evt);
				if (this.dragDirection == ClampedDragger.DragDirection.None)
				{
					this.dragDirection = ClampedDragger.DragDirection.Free;
				}
				if (this.dragDirection == ClampedDragger.DragDirection.Free)
				{
					if (this.dragging != null)
					{
						this.dragging();
					}
				}
			}
		}

		[Flags]
		public enum DragDirection
		{
			None = 0,
			LowToHigh = 1,
			HighToLow = 2,
			Free = 4
		}
	}
}
