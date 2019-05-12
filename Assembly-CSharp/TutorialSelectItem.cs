using System;
using UnityEngine;

public sealed class TutorialSelectItem : GUICollider
{
	[SerializeField]
	private UILabel selectItemLabel;

	private Action selectedAction;

	public bool IsSettings { get; set; }

	public void SetInfo(string selectText, Action onSelected)
	{
		this.selectItemLabel.text = selectText;
		this.selectedAction = onSelected;
		this.IsSettings = true;
	}

	private void OnSelected()
	{
		if (this.selectedAction != null)
		{
			this.selectedAction();
			this.selectedAction = null;
		}
	}

	public void OnTweenFinished()
	{
		if (this.IsSettings)
		{
			BoxCollider component = base.GetComponent<BoxCollider>();
			component.enabled = true;
		}
	}
}
