using System;
using UnityEngine;

public class BattleUISingleX2PlayButton : MonoBehaviour
{
	[SerializeField]
	private UIButton button;

	[SerializeField]
	private UISkinnerToggle toggle;

	protected virtual void Start()
	{
	}

	public void Apply2xPlay(bool isEnable)
	{
		this.toggle.value = isEnable;
	}

	public void AddEvent(Action callback)
	{
		BattleInputUtility.AddEvent(this.button.onClick, callback);
	}
}
