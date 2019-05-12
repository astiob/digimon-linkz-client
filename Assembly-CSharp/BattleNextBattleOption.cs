using Quest;
using System;
using UnityEngine;

public class BattleNextBattleOption : MonoBehaviour
{
	[SerializeField]
	private UISprite autoButtonSprite;

	[SerializeField]
	private Collider autoButtonCollider;

	[SerializeField]
	private string autoButtonOnSpriteName = "Battle_Autobtn_ON";

	[SerializeField]
	private string autoButtonOffSpriteName = "Battle_Autobtn_OFF";

	[SerializeField]
	private UISprite autoButtonLoopSprite;

	[SerializeField]
	private TweenRotation autoButtonLoopTweenRotation;

	[SerializeField]
	private UISprite speedButtonSprite;

	[SerializeField]
	private Collider speedButtonCollider;

	[SerializeField]
	private string speedButtonOnSpriteName = "Battle_Speedbtn_ON";

	[SerializeField]
	private string speedButtonOffSpriteName = "Battle_Speedbtn_OFF";

	private BattleNextBattleOption.BattleOption battleOptionSettings;

	private void Awake()
	{
		if (ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic != null)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			GameWebAPI.WD_Req_DngStart last_dng_req = DataMng.Instance().GetResultUtilData().last_dng_req;
			if (!ClassSingleton<QuestData>.Instance.ExistEvent(last_dng_req.dungeonId))
			{
				this.InitBattleOptionButton();
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	private void InitBattleOptionButton()
	{
		this.battleOptionSettings.speed = PlayerPrefs.GetInt("Battle2xSpeedPlay", 0);
		this.battleOptionSettings.auto = PlayerPrefs.GetInt("BattleAutoPlay", 0);
		if (this.battleOptionSettings.speed == 1)
		{
			this.speedButtonSprite.spriteName = this.speedButtonOnSpriteName;
		}
		else
		{
			this.speedButtonSprite.spriteName = this.speedButtonOffSpriteName;
		}
		if (this.battleOptionSettings.auto == 1)
		{
			this.autoButtonSprite.spriteName = this.autoButtonOnSpriteName;
		}
		else
		{
			this.autoButtonSprite.spriteName = this.autoButtonOffSpriteName;
			this.autoButtonLoopSprite.enabled = false;
			this.autoButtonLoopTweenRotation.enabled = false;
		}
	}

	private void OnPushedDoubleSpeedButton()
	{
		if (this.battleOptionSettings.speed == 0)
		{
			this.battleOptionSettings.speed = 1;
			this.speedButtonSprite.spriteName = this.speedButtonOnSpriteName;
		}
		else
		{
			this.battleOptionSettings.speed = 0;
			this.speedButtonSprite.spriteName = this.speedButtonOffSpriteName;
		}
	}

	private void OnPushedAutoPlayButton()
	{
		if (this.battleOptionSettings.auto == 0)
		{
			this.battleOptionSettings.auto = 1;
			this.autoButtonSprite.spriteName = this.autoButtonOnSpriteName;
			this.autoButtonLoopSprite.enabled = true;
			this.autoButtonLoopTweenRotation.enabled = true;
		}
		else
		{
			this.battleOptionSettings.auto = 0;
			this.autoButtonSprite.spriteName = this.autoButtonOffSpriteName;
			this.autoButtonLoopSprite.enabled = false;
			this.autoButtonLoopTweenRotation.enabled = false;
		}
	}

	public BattleNextBattleOption.BattleOption GetBattleOptionSettings()
	{
		return this.battleOptionSettings;
	}

	public static void SaveBattleMenuSettings(BattleNextBattleOption.BattleOption optionSettings)
	{
		bool flag = false;
		if (optionSettings.speed != PlayerPrefs.GetInt("Battle2xSpeedPlay", 0) && BattleNextBattleOption.BattleOption.IsValid(optionSettings.speed))
		{
			PlayerPrefs.SetInt("Battle2xSpeedPlay", optionSettings.speed);
			flag = true;
		}
		if (optionSettings.auto != PlayerPrefs.GetInt("BattleAutoPlay", 0) && BattleNextBattleOption.BattleOption.IsValid(optionSettings.auto))
		{
			PlayerPrefs.SetInt("BattleAutoPlay", optionSettings.auto);
			flag = true;
		}
		if (flag)
		{
			PlayerPrefs.Save();
		}
	}

	public static void ClearBattleMenuSettings()
	{
		bool flag = false;
		if (PlayerPrefs.GetInt("Battle2xSpeedPlay", 0) == 1)
		{
			PlayerPrefs.SetInt("Battle2xSpeedPlay", 0);
			flag = true;
		}
		if (PlayerPrefs.GetInt("BattleAutoPlay", 0) == 1)
		{
			PlayerPrefs.SetInt("BattleAutoPlay", 0);
			flag = true;
		}
		if (flag)
		{
			PlayerPrefs.Save();
		}
	}

	public struct BattleOption
	{
		public const int FLAG_ON = 1;

		public const int FLAG_OFF = 0;

		public int auto;

		public int speed;

		public static bool IsValid(int value)
		{
			return value == 1 || value == 0;
		}
	}
}
