using System;

namespace UnityEngine.Experimental.UIElements
{
	internal interface IVisualElementPanelActivatable
	{
		VisualElement element { get; }

		bool CanBeActivated();

		void OnPanelActivate();

		void OnPanelDeactivate();
	}
}
