using System;
using UnityEngine;

public class BattleAutoMenu : MonoBehaviour
{
	[Header("Autoボタン")]
	[SerializeField]
	private UIButton autoButton;

	[Header("Autoのスキナー")]
	[SerializeField]
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
		if (isEnable != 0)
		{
			if (isEnable != 1)
			{
				if (isEnable == 2)
				{
					if (this.autoSprite != null)
					{
						this.autoSprite.spriteName = "Battle_Autobtn_ON";
					}
					if (this.autoLoopObj != null)
					{
						this.autoLoopObj.SetActive(true);
					}
				}
			}
			else
			{
				if (this.autoSprite != null)
				{
					this.autoSprite.spriteName = "Battle_Autobtn_OFF";
				}
				if (this.autoLoopObj != null)
				{
					this.autoLoopObj.SetActive(true);
				}
			}
		}
		else
		{
			if (this.autoSprite != null)
			{
				this.autoSprite.spriteName = "Battle_Autobtn_ALLOFF";
			}
			if (this.autoLoopObj != null)
			{
				this.autoLoopObj.SetActive(false);
			}
		}
	}

	public enum AutoPlayType
	{
		NON,
		ATTACK_SELECT,
		SKILL_SELECT
	}
}
