using System;
using System.Collections.Generic;
using UnityEngine;

public class OptionSetting
{
	public static readonly string pushEventKey = "1";

	public static readonly string pushGashaKey = "2";

	public static readonly string pushBuildedKey = "3";

	public static readonly string pushStaminaMaxKey = "4";

	public static readonly string pushGardenKey = "5";

	public static readonly string bgmKey = "101";

	public static readonly string seKey = "102";

	public static readonly string languageKey = "103";

	public static readonly string graphicsQualityKey = "104";

	public static readonly string autoBattleKey = "105";

	private static readonly int bgmSeDefaultVolume = 5;

	private static OptionSetting instance;

	private Dictionary<string, int> optionList;

	private OptionSetting()
	{
	}

	public static OptionSetting Instance
	{
		get
		{
			if (OptionSetting.instance == null)
			{
				OptionSetting.instance = new OptionSetting();
			}
			return OptionSetting.instance;
		}
	}

	public Dictionary<string, int> OptionList
	{
		get
		{
			return this.optionList;
		}
	}

	public void Initialize(Dictionary<string, int> option)
	{
		if (this.optionList != null)
		{
			this.optionList.Clear();
			this.optionList = null;
		}
		this.optionList = new Dictionary<string, int>(option);
		SoundMng.Instance().VolumeBGM = (float)this.optionList[OptionSetting.bgmKey];
		SoundMng.Instance().VolumeSE = (float)this.optionList[OptionSetting.seKey];
		PushNotice.Instance.IsRecieveEndBuildingPushNotice = (this.optionList[OptionSetting.pushBuildedKey] == 1);
		PushNotice.Instance.IsRecieveStaminaMaxPushNotice = (this.optionList[OptionSetting.pushStaminaMaxKey] == 1);
		PushNotice.Instance.IsRecieveGardenPushNotice = (this.optionList[OptionSetting.pushGardenKey] == 1);
		this.SaveSoundVolume();
	}

	public void SaveOptionSetting(bool IsEventPushNotice, bool IsGachaPushNotice, bool IsBuildedPushNotice, bool IsStaminaMaxPushNotice, bool IsGardenPushNotice, int BgmVolume, int SeVolume, OptionSetting.LANGUAGE Language, OptionSetting.LEVEL_3D Level_3D)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		this.optionList[OptionSetting.pushEventKey] = ((!IsEventPushNotice) ? 0 : 1);
		this.optionList[OptionSetting.pushGashaKey] = ((!IsGachaPushNotice) ? 0 : 1);
		this.optionList[OptionSetting.pushBuildedKey] = ((!IsBuildedPushNotice) ? 0 : 1);
		this.optionList[OptionSetting.pushStaminaMaxKey] = ((!IsStaminaMaxPushNotice) ? 0 : 1);
		this.optionList[OptionSetting.pushGardenKey] = ((!IsGardenPushNotice) ? 0 : 1);
		this.optionList[OptionSetting.bgmKey] = BgmVolume;
		this.optionList[OptionSetting.seKey] = SeVolume;
		this.optionList[OptionSetting.languageKey] = (int)Language;
		this.optionList[OptionSetting.graphicsQualityKey] = (int)Level_3D;
		this.SaveSoundVolume();
		PushNotice.Instance.IsRecieveEndBuildingPushNotice = (this.optionList[OptionSetting.pushBuildedKey] == 1);
		PushNotice.Instance.IsRecieveStaminaMaxPushNotice = (this.optionList[OptionSetting.pushStaminaMaxKey] == 1);
		PushNotice.Instance.IsRecieveGardenPushNotice = true;
		GameWebAPI.RequestUS_RegisterOptionInfo request = new GameWebAPI.RequestUS_RegisterOptionInfo
		{
			SetSendData = delegate(GameWebAPI.US_Req_RegisterOptionInfo param)
			{
				param.optionList = this.optionList;
			}
		};
		AppCoroutine.Start(request.Run(new Action(RestrictionInput.EndLoad), null, null), false);
	}

	private void SaveSoundVolume()
	{
		PlayerPrefs.SetInt("VolumeBGM", this.optionList[OptionSetting.bgmKey]);
		PlayerPrefs.SetInt("VolumeSE", this.optionList[OptionSetting.seKey]);
		PlayerPrefs.Save();
	}

	public void SaveAutoBattle(int auto)
	{
		PlayerPrefs.SetInt("BattleAuto", auto);
	}

	public void ResetAutoBattlePrefas()
	{
		PlayerPrefs.SetInt("Battle2xSpeedPlay", 0);
		PlayerPrefs.SetInt("BattleAutoPlay", 0);
	}

	public static void LoadSoundVolume()
	{
		SoundMng.Instance().VolumeBGM = (float)PlayerPrefs.GetInt("VolumeBGM", OptionSetting.bgmSeDefaultVolume);
		SoundMng.Instance().VolumeSE = (float)PlayerPrefs.GetInt("VolumeSE", OptionSetting.bgmSeDefaultVolume);
	}

	public enum LANGUAGE
	{
		Japanese = 1
	}

	public enum LEVEL_3D
	{
		High = 1,
		Normal,
		Low
	}
}
