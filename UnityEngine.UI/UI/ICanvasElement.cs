using System;

namespace UnityEngine.UI
{
	public interface ICanvasElement
	{
		void Rebuild(CanvasUpdate executing);

		Transform transform { get; }

		void LayoutComplete();

		void GraphicUpdateComplete();

		bool IsDestroyed();
	}
}
