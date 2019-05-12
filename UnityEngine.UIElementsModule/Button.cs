using System;

namespace UnityEngine.Experimental.UIElements
{
	public class Button : VisualElement
	{
		public Clickable clickable;

		public Button(Action clickEvent)
		{
			this.clickable = new Clickable(clickEvent);
			this.AddManipulator(this.clickable);
		}
	}
}
