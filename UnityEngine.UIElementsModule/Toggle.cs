using System;

namespace UnityEngine.Experimental.UIElements
{
	public class Toggle : VisualElement
	{
		private Action clickEvent;

		public Toggle(Action clickEvent)
		{
			this.clickEvent = clickEvent;
			this.AddManipulator(new Clickable(new Action(this.OnClick)));
		}

		public bool on
		{
			get
			{
				return (base.pseudoStates & PseudoStates.Checked) == PseudoStates.Checked;
			}
			set
			{
				if (value)
				{
					base.pseudoStates |= PseudoStates.Checked;
				}
				else
				{
					base.pseudoStates &= ~PseudoStates.Checked;
				}
			}
		}

		public void OnToggle(Action clickEvent)
		{
			this.clickEvent = clickEvent;
		}

		private void OnClick()
		{
			this.on = !this.on;
			if (this.clickEvent != null)
			{
				this.clickEvent();
			}
		}
	}
}
