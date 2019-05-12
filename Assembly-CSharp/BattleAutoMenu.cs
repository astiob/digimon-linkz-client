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

	[SerializeField]
	private UISprite autoSprite;

	[SerializeField]
	private GameObject autoLoopObj;

	public void AddEvent(Action callback)
	{
		BattleInputUtility.AddEvent(this.autoButton.onClick, callback);
	}

	public void ApplyAutoPlay(int isEnable)
	{
		switch (isEnable)
		{
		case 0:
			if (this.autoSprite != null)
			{
				this.autoSprite.spriteName = "Battle_Autobtn_ALLOFF";
			}
			if (this.autoLoopObj != null)
			{
				this.autoLoopObj.SetActive(false);
			}
			break;
		case 1:
			if (this.autoSprite != null)
			{
				this.autoSprite.spriteName = "Battle_Autobtn_OFF";
			}
			if (this.autoLoopObj != null)
			{
				this.autoLoopObj.SetActive(true);
			}
			break;
		case 2:
			if (this.autoSprite != null)
			{
				this.autoSprite.spriteName = "Battle_Autobtn_ON";
			}
			if (this.autoLoopObj != null)
			{
				this.autoLoopObj.SetActive(true);
			}
			break;
		}
	}

	public enum AutoPlayType
	{
		NON,
		ATTACK_SELECT,
		SKILL_SELECT
	}
}
