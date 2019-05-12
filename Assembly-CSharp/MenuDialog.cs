using BattleStateMachineInternal;
using Master;
using System;
using System.Collections;
using UnityEngine;

public class MenuDialog : MonoBehaviour
{
	[SerializeField]
	private UIButton retireButton;

	[SerializeField]
	private UIButton closeButton;

	[SerializeField]
	private UIButton helpButton;

	[SerializeField]
	private UIComponentSkinner dialogComponentSkinner;

	[SerializeField]
	private UIComponentSkinner retireComponentSkinner;

	[SerializeField]
	private UIOpenCloseDialog openCloseDialog;

	public UILabel title;

	public UILabel waveRoundCount;

	public UILabel retireButtonName;

	public UILabel areaName;

	public UILabel coinCount;

	public UILabel dropNormalItemCount;

	public UILabel dropRareItemCount;

	public UILabel expCount;

	[Header("ヘルプローカライズ")]
	[SerializeField]
	private UILabel helpLocalize;

	[SerializeField]
	[Header("リタイアローカライズ")]
	private UILabel retireLocalize;

	[SerializeField]
	[Header("獲得EXPローカライズ")]
	private UILabel expLocalize;

	[SerializeField]
	[Header("獲得コインローカライズ")]
	private UILabel coinLocalize;

	[SerializeField]
	[Header("DialogMENUの左上")]
	private UILabel menuTitleLocalize;

	private void Awake()
	{
		this.SetupLocalize();
	}

	private void SetupLocalize()
	{
		if (this.menuTitleLocalize == null)
		{
			return;
		}
		this.menuTitleLocalize.text = StringMaster.GetString("BattleUI-01");
		this.retireLocalize.text = StringMaster.GetString("BattleUI-05");
		this.helpLocalize.text = StringMaster.GetString("SystemHelp");
		this.expLocalize.text = StringMaster.GetString("BattleUI-03");
		this.coinLocalize.text = StringMaster.GetString("BattleUI-04");
	}

	public void SetActive(bool value)
	{
		NGUITools.SetActiveSelf(base.gameObject, value);
	}

	public void AddRetireButtonEvent(Action callback)
	{
		BattleInputUtility.AddEvent(this.retireButton.onClick, callback);
	}

	public void AddCloseButtonEvent(Action callback)
	{
		BattleInputUtility.AddEvent(this.closeButton.onClick, callback);
	}

	public void AddHelpButtonEvent(Action callback)
	{
		BattleInputUtility.AddEvent(this.helpButton.onClick, callback);
	}

	public IEnumerator ApplyShowMenuWindow(bool isShow, bool isEnableRetire = true, Action onFinishedAction = null)
	{
		if (isShow)
		{
			this.retireButton.gameObject.GetComponent<Collider>().enabled = isEnableRetire;
			this.retireComponentSkinner.SetSkins((!isEnableRetire) ? 1 : 0);
			return BattleStateManager.current.uiControl.WaitOpenCloseDialog(isShow, base.gameObject, this.openCloseDialog, null);
		}
		return BattleStateManager.current.uiControl.WaitOpenCloseDialog(isShow, base.gameObject, this.openCloseDialog, onFinishedAction);
	}

	public IEnumerator ApplyShowRetireWindow(bool isShow, Action onFinishedAction = null)
	{
		BattleUIComponents ui = BattleStateManager.current.battleUiComponents;
		if (isShow)
		{
			if (ui.dialogContinue != null)
			{
				this.ApplyShowHideBG(false);
				ui.dialogContinue.ApplySpecificTrade(true);
			}
			return BattleStateManager.current.uiControl.WaitOpenCloseDialog(isShow, ui.dialogRetire.gameObject, ui.dialogRetire.openCloseDialog, null);
		}
		Action onFinishedClose = delegate()
		{
			BattleStateManager.current.battleStateData.isShowRetireWindow = false;
			if (onFinishedAction != null)
			{
				onFinishedAction();
			}
			if (ui.dialogContinue != null)
			{
				this.ApplyShowHideBG(true);
				ui.dialogContinue.ApplySpecificTrade(false);
			}
		};
		return BattleStateManager.current.uiControl.WaitOpenCloseDialog(isShow, ui.dialogRetire.gameObject, ui.dialogRetire.openCloseDialog, onFinishedClose);
	}

	public void ApplyShowHideBG(bool value)
	{
		this.dialogComponentSkinner.SetSkins((!value) ? 1 : 0);
	}

	public void SetValuetextReplacer(int waveValue, int roundValue, int maxWave)
	{
		this.waveRoundCount.text = string.Format(StringMaster.GetString("BattleUI-02"), waveValue, maxWave, roundValue);
	}

	public void SetValuetextReplacer(BattleWave battleWave, int roundValue, int maxWave)
	{
		string key = "BattleUI-02";
		if (battleWave.floorType == 3)
		{
			key = "BattleUI-45";
		}
		this.waveRoundCount.text = string.Format(StringMaster.GetString(key), battleWave.floorNum, maxWave, roundValue);
	}
}
