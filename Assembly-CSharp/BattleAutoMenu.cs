using System;
using UnityEngine;

public class BattleAutoMenu : MonoBehaviour
{
	[SerializeField]
	[Header("Autoボタン")]
	private UIButton autoButton;

	[SerializeField]
	[Header("Autoのスキナー")]
	private UIComponentSkinner autoSkinner;

	public void AddEvent(Action callback)
	{
		BattleInputUtility.AddEvent(this.autoButton.onClick, callback);
	}

	public void ApplyAutoPlay(bool isEnable)
	{
		this.autoSkinner.SetSkins((!isEnable) ? 0 : 1);
	}
}
