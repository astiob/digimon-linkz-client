using Master;
using MultiBattle.Tools;
using System;
using UnityEngine;

public class CMD_PvPMockTargetInputModal : CMD
{
	[SerializeField]
	private UILabel ngTX_MESSAGE;

	[SerializeField]
	private UILabel ngTX_BTN_YES;

	[SerializeField]
	private UILabel ngTX_BTN_NO;

	[SerializeField]
	private UIInput pvpMockTargetInput;

	[SerializeField]
	private UILabel pvpMockTargetLabel;

	[SerializeField]
	private GameObject submitButton;

	[SerializeField]
	private GameObject submitButtonGray;

	[SerializeField]
	private BoxCollider submitButtonCollider;

	protected override void Awake()
	{
		base.Awake();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.SetInitLabel();
		this.SetGrayButton(true);
		base.Show(f, sizeX, sizeY, aT);
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	private void SetInitLabel()
	{
		this.ngTX_MESSAGE.text = StringMaster.GetString("ColosseumMockUserCodeInfo");
		this.ngTX_BTN_YES.text = StringMaster.GetString("SystemButtonDecision");
		this.ngTX_BTN_NO.text = StringMaster.GetString("SystemButtonClose");
		this.pvpMockTargetLabel.text = StringMaster.GetString("ColosseumMockUserCode");
	}

	private void ClickDecisionBtn()
	{
		MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		GameWebAPI.ColosseumMatchingValidateLogic request = new GameWebAPI.ColosseumMatchingValidateLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_ColosseumMatchingValidateLogic param)
			{
				param.act = 1;
				param.isMockBattle = 1;
				param.targetUserCode = this.pvpMockTargetInput.value;
			},
			OnReceived = new Action<GameWebAPI.RespData_ColosseumMatchingValidateLogic>(this.EndValidate)
		};
		base.StartCoroutine(request.RunOneTime(new Action(RestrictionInput.EndLoad), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void EndValidate(GameWebAPI.RespData_ColosseumMatchingValidateLogic data)
	{
		MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		int resultCode = data.resultCode;
		switch (resultCode)
		{
		case 92:
			AlertManager.ShowModalMessage(delegate(int modal)
			{
				this.ClosePanel(true);
			}, StringMaster.GetString("ColosseumMockLockTitle"), StringMaster.GetString("ColosseumMockLockInfo"), AlertManager.ButtonActionType.Close, false);
			return;
		default:
			if (resultCode == 1)
			{
				global::Debug.Log("対戦相手UserCode: " + this.pvpMockTargetInput.value);
				ClassSingleton<MultiBattleData>.Instance.MockBattleUserCode = this.pvpMockTargetInput.value;
				CMD_PartyEdit.ModeType = CMD_PartyEdit.MODE_TYPE.PVP;
				GUIMain.ShowCommonDialog(null, "CMD_PartyEdit");
				return;
			}
			break;
		case 94:
			break;
		}
		AlertManager.ShowModalMessage(delegate(int modal)
		{
			this.ClosePanel(true);
		}, StringMaster.GetString("ColosseumMockNotFoundTitle"), StringMaster.GetString("ColosseumMockNotFoundInfo"), AlertManager.ButtonActionType.Close, false);
	}

	public void CheckInputContent()
	{
		bool grayButton = false;
		if (string.IsNullOrEmpty(this.pvpMockTargetLabel.text) || this.pvpMockTargetInput.value.Length < 8)
		{
			grayButton = true;
		}
		this.pvpMockTargetInput.value = this.pvpMockTargetInput.value.Replace("-", string.Empty);
		this.pvpMockTargetInput.value = this.pvpMockTargetInput.value.Replace(" ", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty);
		this.pvpMockTargetInput.label.text = this.pvpMockTargetInput.value;
		this.SetGrayButton(grayButton);
	}

	private void SetGrayButton(bool isGray)
	{
		if (isGray)
		{
			this.submitButton.SetActive(false);
			this.submitButtonGray.SetActive(true);
		}
		else
		{
			this.submitButton.SetActive(true);
			this.submitButtonGray.SetActive(false);
		}
		this.ngTX_BTN_YES.color = ((!isGray) ? Color.white : Color.gray);
		this.submitButtonCollider.enabled = !isGray;
	}
}
