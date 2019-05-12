using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_OptionSettings : CMD
{
	private readonly int pushNoticeOn = 1;

	private readonly int pushNoticeOff;

	[SerializeField]
	private UISlider bgmSlider;

	[SerializeField]
	private UISlider seSlider;

	[SerializeField]
	private List<UISprite> pushNoticeOnSprite;

	[SerializeField]
	private List<UISprite> pushNoticeOffSprite;

	[SerializeField]
	private UISprite autoButtonOnSprite;

	[SerializeField]
	private UISprite autoButtonOffSprite;

	[SerializeField]
	private UILabel autoButtonOnLabel;

	[SerializeField]
	private UILabel autoButtonOffLabel;

	[SerializeField]
	private List<UILabel> pushNoticeOnLabel;

	[SerializeField]
	private List<UILabel> pushNoticeOffLabel;

	[SerializeField]
	private List<GUICollider> pushNoticeOnCollider;

	[SerializeField]
	private List<GUICollider> pushNoticeOffCollider;

	private Dictionary<string, int> editOptionList = new Dictionary<string, int>();

	private bool isRecieveEventPushNotice;

	private bool isRecieveGachaPushNotice;

	private bool isRecieveBuildedPushNotice;

	private bool isRecieveStaminaMaxPushNotice;

	private bool isRecieveGardenPushNotice;

	private OptionSetting.LANGUAGE language = OptionSetting.LANGUAGE.Japanese;

	private OptionSetting.LEVEL_3D level_3D = OptionSetting.LEVEL_3D.High;

	private bool isInitialized;

	private CMD_OptionSettings.TAB currentTab;

	private Coroutine progressBarUpdatingCoroutine;

	private int autoBattleState;

	private int initAutoBattleState;

	private const int AUTO_FLAG_OFF = 1;

	private const int AUTO_FLAG_ON = 0;

	[SerializeField]
	private GameObject systemEnvironmentWindow;

	[SerializeField]
	private GameObject pushNoticeWindow;

	[SerializeField]
	private CMD_OptionSettings.SettingType settingType;

	[SerializeField]
	private GameObject goLanguageSettings;

	[SerializeField]
	private UIWidget widgetWindow;

	[SerializeField]
	private UILabel currentLanguageSetting;

	[SerializeField]
	private UILabel pushTitle;

	[SerializeField]
	private UILabel eventStartTitle;

	[SerializeField]
	private UILabel eventStartSubTitle;

	[SerializeField]
	private UILabel captureUpdateTitle;

	[SerializeField]
	private UILabel captureUpdateSubTitle;

	[SerializeField]
	private UILabel facilityBuildTitle;

	[SerializeField]
	private UILabel facilityBuildSubTitle;

	[SerializeField]
	private UILabel staminaMaxTitle;

	[SerializeField]
	private UILabel staminaMaxSubTitle;

	[SerializeField]
	private UILabel gardenEvolutionTitle;

	[SerializeField]
	private UILabel gardenEvolutionSubTitle;

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.Initialize();
		this.progressBarUpdatingCoroutine = base.StartCoroutine(this.ProgressBarUpdating());
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void WindowOpened()
	{
		base.StopCoroutine(this.progressBarUpdatingCoroutine);
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.CompareOptionList(OptionSetting.Instance.OptionList, this.editOptionList))
		{
			this.SaveOptionSettings();
		}
		if (this.AutoBattleStateCheck())
		{
			OptionSetting.Instance.SaveAutoBattle(this.autoBattleState);
			if (this.autoBattleState == 1)
			{
				OptionSetting.Instance.ResetAutoBattlePrefas();
			}
		}
		this.CloseAndFarmCamOn(animation);
	}

	private void Initialize()
	{
		if (base.PartsTitle != null)
		{
			base.PartsTitle.SetTitle(StringMaster.GetString("OptionTitle"));
		}
		if (this.pushNoticeOnLabel != null)
		{
			foreach (UILabel uilabel in this.pushNoticeOnLabel)
			{
				uilabel.text = StringMaster.GetString("SystemOn");
			}
		}
		if (this.pushNoticeOffLabel != null)
		{
			foreach (UILabel uilabel2 in this.pushNoticeOffLabel)
			{
				uilabel2.text = StringMaster.GetString("SystemOff");
			}
		}
		if (base.MultiTab != null)
		{
			List<string> tabLabelTexts = new List<string>
			{
				StringMaster.GetString("OptionSetting"),
				StringMaster.GetString("OptionPushNotice")
			};
			base.MultiTab.InitMultiTab(new List<Action<int>>
			{
				new Action<int>(this.OnClickedSystemEnvironmentTab),
				new Action<int>(this.OnClickedPushNoticeTab)
			}, tabLabelTexts);
			base.MultiTab.SetOnOffColor(ConstValue.TAB_YELLOW, Color.white);
			base.MultiTab.SetFocus(1);
		}
		if (null != this.pushTitle)
		{
			this.pushTitle.text = StringMaster.GetString("Option-05");
		}
		if (null != this.eventStartTitle)
		{
			this.eventStartTitle.text = StringMaster.GetString("Option-06");
		}
		if (null != this.eventStartSubTitle)
		{
			this.eventStartSubTitle.text = StringMaster.GetString("Option-07");
		}
		if (null != this.captureUpdateTitle)
		{
			this.captureUpdateTitle.text = StringMaster.GetString("Option-08");
		}
		if (null != this.captureUpdateSubTitle)
		{
			this.captureUpdateSubTitle.text = StringMaster.GetString("Option-09");
		}
		if (null != this.facilityBuildTitle)
		{
			this.facilityBuildTitle.text = StringMaster.GetString("Option-10");
		}
		if (null != this.facilityBuildSubTitle)
		{
			this.facilityBuildSubTitle.text = StringMaster.GetString("Option-11");
		}
		if (null != this.staminaMaxTitle)
		{
			this.staminaMaxTitle.text = StringMaster.GetString("Option-12");
		}
		if (null != this.staminaMaxSubTitle)
		{
			this.staminaMaxSubTitle.text = StringMaster.GetString("Option-13");
		}
		if (null != this.gardenEvolutionTitle)
		{
			this.gardenEvolutionTitle.text = StringMaster.GetString("Option-14");
		}
		if (null != this.gardenEvolutionSubTitle)
		{
			this.gardenEvolutionSubTitle.text = StringMaster.GetString("Option-15");
		}
		this.CopyOptionList(OptionSetting.Instance.OptionList, this.editOptionList);
		if (this.editOptionList != null)
		{
			this.SetupController();
		}
		bool flag = PlayerPrefs.GetInt("BattleAuto", 0) == 0;
		if (this.autoButtonOnSprite != null)
		{
			this.autoButtonOnSprite.spriteName = ((!flag) ? "Common02_Btn_SupportWhite" : "Common02_Btn_SupportRed");
		}
		if (this.autoButtonOffSprite != null)
		{
			this.autoButtonOffSprite.spriteName = ((!flag) ? "Common02_Btn_SupportRed" : "Common02_Btn_SupportWhite");
		}
		this.autoBattleState = PlayerPrefs.GetInt("BattleAuto", 0);
		this.initAutoBattleState = PlayerPrefs.GetInt("BattleAuto", 0);
		if (this.autoButtonOnLabel != null)
		{
			this.autoButtonOnLabel.color = ((!flag) ? Color.gray : Color.white);
		}
		if (this.autoButtonOffLabel != null)
		{
			this.autoButtonOffLabel.color = ((!flag) ? Color.white : Color.gray);
		}
		this.ChangeTab(this.currentTab);
		if (this.goLanguageSettings != null)
		{
			if (CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN) != 1.ToString())
			{
				this.currentLanguageSetting.text = StringMaster.GetString(string.Format("OptionLanguage_{0}", CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN)));
				this.goLanguageSettings.SetActive(true);
			}
			else
			{
				this.widgetWindow.height -= 80;
				this.goLanguageSettings.SetActive(false);
			}
		}
		this.isInitialized = true;
	}

	private void SetupController()
	{
		float value = (float)this.editOptionList[OptionSetting.bgmKey] / 10f;
		float value2 = (float)this.editOptionList[OptionSetting.seKey] / 10f;
		this.bgmSlider.value = value;
		this.seSlider.value = value2;
		this.ChangePushNoticeOnOff(this.editOptionList[OptionSetting.pushEventKey] == this.pushNoticeOn, CMD_OptionSettings.PushNoticeType.Event);
		this.ChangePushNoticeOnOff(this.editOptionList[OptionSetting.pushGashaKey] == this.pushNoticeOn, CMD_OptionSettings.PushNoticeType.Gasha);
		this.ChangePushNoticeOnOff(this.editOptionList[OptionSetting.pushBuildedKey] == this.pushNoticeOn, CMD_OptionSettings.PushNoticeType.Builded);
		this.ChangePushNoticeOnOff(this.editOptionList[OptionSetting.pushStaminaMaxKey] == this.pushNoticeOn, CMD_OptionSettings.PushNoticeType.StaminaMax);
		this.ChangePushNoticeOnOff(this.editOptionList[OptionSetting.pushGardenKey] == this.pushNoticeOn, CMD_OptionSettings.PushNoticeType.Garden);
		this.language = (OptionSetting.LANGUAGE)this.editOptionList[OptionSetting.languageKey];
		this.level_3D = (OptionSetting.LEVEL_3D)this.editOptionList[OptionSetting.graphicsQualityKey];
	}

	private IEnumerator ProgressBarUpdating()
	{
		for (;;)
		{
			this.bgmSlider.ForceUpdate();
			this.seSlider.ForceUpdate();
			yield return null;
		}
		yield break;
	}

	private void ChangePushNoticeOnOff(bool IsRecieve, CMD_OptionSettings.PushNoticeType ButtonType)
	{
		if (this.pushNoticeWindow == null)
		{
			return;
		}
		switch (ButtonType)
		{
		case CMD_OptionSettings.PushNoticeType.Event:
			this.editOptionList[OptionSetting.pushEventKey] = ((!IsRecieve) ? this.pushNoticeOff : this.pushNoticeOn);
			this.isRecieveEventPushNotice = IsRecieve;
			break;
		case CMD_OptionSettings.PushNoticeType.Gasha:
			this.editOptionList[OptionSetting.pushGashaKey] = ((!IsRecieve) ? this.pushNoticeOff : this.pushNoticeOn);
			this.isRecieveGachaPushNotice = IsRecieve;
			break;
		case CMD_OptionSettings.PushNoticeType.Builded:
			this.editOptionList[OptionSetting.pushBuildedKey] = ((!IsRecieve) ? this.pushNoticeOff : this.pushNoticeOn);
			this.isRecieveBuildedPushNotice = IsRecieve;
			break;
		case CMD_OptionSettings.PushNoticeType.StaminaMax:
			this.editOptionList[OptionSetting.pushStaminaMaxKey] = ((!IsRecieve) ? this.pushNoticeOff : this.pushNoticeOn);
			this.isRecieveStaminaMaxPushNotice = IsRecieve;
			break;
		case CMD_OptionSettings.PushNoticeType.Garden:
			this.editOptionList[OptionSetting.pushGardenKey] = ((!IsRecieve) ? this.pushNoticeOff : this.pushNoticeOn);
			this.isRecieveGardenPushNotice = IsRecieve;
			break;
		}
		this.pushNoticeOnSprite[(int)ButtonType].spriteName = ((!IsRecieve) ? "Common02_Btn_SupportWhite" : "Common02_Btn_SupportRed");
		this.pushNoticeOnLabel[(int)ButtonType].color = ((!IsRecieve) ? Color.gray : Color.white);
		this.pushNoticeOffSprite[(int)ButtonType].spriteName = ((!IsRecieve) ? "Common02_Btn_SupportRed" : "Common02_Btn_SupportWhite");
		this.pushNoticeOffLabel[(int)ButtonType].color = ((!IsRecieve) ? Color.white : Color.gray);
		this.pushNoticeOnCollider[(int)ButtonType].activeCollider = !IsRecieve;
		this.pushNoticeOffCollider[(int)ButtonType].activeCollider = IsRecieve;
	}

	private bool CompareOptionList(Dictionary<string, int> arg_1, Dictionary<string, int> arg_2)
	{
		foreach (KeyValuePair<string, int> keyValuePair in arg_1)
		{
			if (arg_2.ContainsKey(keyValuePair.Key))
			{
				if (arg_2[keyValuePair.Key] != keyValuePair.Value)
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool AutoBattleStateCheck()
	{
		return this.autoBattleState != this.initAutoBattleState;
	}

	private void CopyOptionList(Dictionary<string, int> from, Dictionary<string, int> to)
	{
		foreach (KeyValuePair<string, int> keyValuePair in from)
		{
			to[keyValuePair.Key] = keyValuePair.Value;
		}
	}

	private void SaveOptionSettings()
	{
		if (this.CompareOptionList(OptionSetting.Instance.OptionList, this.editOptionList))
		{
			OptionSetting.Instance.SaveOptionSetting(this.isRecieveEventPushNotice, this.isRecieveGachaPushNotice, this.isRecieveBuildedPushNotice, this.isRecieveStaminaMaxPushNotice, this.isRecieveGardenPushNotice, (int)(this.bgmSlider.value * 10f), (int)(this.seSlider.value * 10f), this.language, this.level_3D);
		}
	}

	private void ChangeTab(CMD_OptionSettings.TAB Tab)
	{
		if (Tab != CMD_OptionSettings.TAB.SystemEnvironment)
		{
			if (Tab == CMD_OptionSettings.TAB.PushNotice)
			{
				this.currentTab = CMD_OptionSettings.TAB.PushNotice;
				if (this.systemEnvironmentWindow != null)
				{
					this.systemEnvironmentWindow.transform.gameObject.SetActive(false);
				}
				if (this.pushNoticeWindow != null)
				{
					this.pushNoticeWindow.transform.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			this.currentTab = CMD_OptionSettings.TAB.SystemEnvironment;
			if (this.systemEnvironmentWindow != null)
			{
				this.systemEnvironmentWindow.transform.gameObject.SetActive(true);
			}
			if (this.pushNoticeWindow != null)
			{
				this.pushNoticeWindow.transform.gameObject.SetActive(false);
			}
		}
	}

	public void ChangeBgmVolume()
	{
		if (!this.isInitialized)
		{
			return;
		}
		SoundMng soundMng = SoundMng.Instance();
		int num = (int)(this.bgmSlider.value * 10f);
		this.editOptionList[OptionSetting.bgmKey] = num;
		soundMng.VolumeBGM = (float)num;
	}

	public void ChangeSeVolume()
	{
		if (!this.isInitialized)
		{
			return;
		}
		SoundMng soundMng = SoundMng.Instance();
		int num = (int)(this.seSlider.value * 10f);
		this.editOptionList[OptionSetting.seKey] = num;
		soundMng.VolumeSE = (float)num;
	}

	public void OnClickEventPushNoticeOnButton()
	{
		if (!this.isInitialized || this.isRecieveEventPushNotice)
		{
			return;
		}
		this.isRecieveEventPushNotice = true;
		this.ChangePushNoticeOnOff(this.isRecieveEventPushNotice, CMD_OptionSettings.PushNoticeType.Event);
	}

	public void OnClickEventPushNoticeOffButton()
	{
		if (!this.isInitialized || !this.isRecieveEventPushNotice)
		{
			return;
		}
		this.isRecieveEventPushNotice = false;
		this.ChangePushNoticeOnOff(this.isRecieveEventPushNotice, CMD_OptionSettings.PushNoticeType.Event);
	}

	public void OnClickGachaPushNoticeOnButton()
	{
		if (!this.isInitialized || this.isRecieveGachaPushNotice)
		{
			return;
		}
		this.isRecieveGachaPushNotice = true;
		this.ChangePushNoticeOnOff(this.isRecieveGachaPushNotice, CMD_OptionSettings.PushNoticeType.Gasha);
	}

	public void OnClickGachaPushNoticeOffButton()
	{
		if (!this.isInitialized || !this.isRecieveGachaPushNotice)
		{
			return;
		}
		this.isRecieveGachaPushNotice = false;
		this.ChangePushNoticeOnOff(this.isRecieveGachaPushNotice, CMD_OptionSettings.PushNoticeType.Gasha);
	}

	public void OnClickBuildedPushNoticeOnButton()
	{
		if (!this.isInitialized || this.isRecieveBuildedPushNotice)
		{
			return;
		}
		this.isRecieveBuildedPushNotice = true;
		this.ChangePushNoticeOnOff(this.isRecieveBuildedPushNotice, CMD_OptionSettings.PushNoticeType.Builded);
	}

	public void OnClickBuildedPushNoticeOffButton()
	{
		if (!this.isInitialized || !this.isRecieveBuildedPushNotice)
		{
			return;
		}
		this.isRecieveBuildedPushNotice = false;
		this.ChangePushNoticeOnOff(this.isRecieveBuildedPushNotice, CMD_OptionSettings.PushNoticeType.Builded);
	}

	public void OnClickStaminaMaxPushNoticeOnButton()
	{
		if (!this.isInitialized || this.isRecieveStaminaMaxPushNotice)
		{
			return;
		}
		this.isRecieveStaminaMaxPushNotice = true;
		this.ChangePushNoticeOnOff(this.isRecieveStaminaMaxPushNotice, CMD_OptionSettings.PushNoticeType.StaminaMax);
	}

	public void OnClickStaminaMaxPushNoticeOffButton()
	{
		if (!this.isInitialized || !this.isRecieveStaminaMaxPushNotice)
		{
			return;
		}
		this.isRecieveStaminaMaxPushNotice = false;
		this.ChangePushNoticeOnOff(this.isRecieveStaminaMaxPushNotice, CMD_OptionSettings.PushNoticeType.StaminaMax);
	}

	public void OnClickGardenPushNoticeOnButton()
	{
		if (!this.isInitialized || this.isRecieveGardenPushNotice)
		{
			return;
		}
		this.isRecieveGardenPushNotice = true;
		this.ChangePushNoticeOnOff(this.isRecieveGardenPushNotice, CMD_OptionSettings.PushNoticeType.Garden);
	}

	public void OnClickGardenPushNoticeOffButton()
	{
		if (!this.isInitialized || !this.isRecieveGardenPushNotice)
		{
			return;
		}
		this.isRecieveGardenPushNotice = false;
		this.ChangePushNoticeOnOff(this.isRecieveGardenPushNotice, CMD_OptionSettings.PushNoticeType.Garden);
	}

	public void OnClickedSystemEnvironmentTab(int index)
	{
		this.ChangeTab(CMD_OptionSettings.TAB.SystemEnvironment);
	}

	public void OnClickedPushNoticeTab(int index)
	{
		this.ChangeTab(CMD_OptionSettings.TAB.PushNotice);
	}

	public void OnClickedAutoBattleOn()
	{
		if (this.autoButtonOnSprite != null && this.autoButtonOffSprite != null)
		{
			this.autoButtonOnSprite.spriteName = "Common02_Btn_SupportRed";
			this.autoButtonOffSprite.spriteName = "Common02_Btn_SupportWhite";
			this.autoButtonOnLabel.color = Color.white;
			this.autoButtonOffLabel.color = Color.gray;
		}
		this.autoBattleState = 0;
	}

	public void OnClickedAutoBattleOff()
	{
		if (this.autoButtonOnSprite != null && this.autoButtonOffSprite != null)
		{
			this.autoButtonOnSprite.spriteName = "Common02_Btn_SupportWhite";
			this.autoButtonOffSprite.spriteName = "Common02_Btn_SupportRed";
			this.autoButtonOnLabel.color = Color.gray;
			this.autoButtonOffLabel.color = Color.white;
		}
		this.autoBattleState = 1;
	}

	private void OnMultiLangSettingSelect()
	{
	}

	public enum TAB
	{
		SystemEnvironment,
		PushNotice
	}

	public enum SettingType
	{
		None,
		Title,
		Farm
	}

	public enum PushNoticeType
	{
		Event,
		Gasha,
		Builded,
		StaminaMax,
		Garden
	}
}
