using System;
using System.Collections.Generic;

public class CustomUIInput : UIInput
{
	public List<EventDelegate> onSelect = new List<EventDelegate>();

	public List<EventDelegate> onDeselect = new List<EventDelegate>();

	protected override void OnSelect(bool isSelected)
	{
		base.OnSelect(isSelected);
		if (isSelected)
		{
			EventDelegate.Execute(this.onSelect);
		}
		else
		{
			EventDelegate.Execute(this.onDeselect);
		}
	}
}
