using Master;
using Monster;
using MultiBattle.Tools;
using System;
using UnityEngine;

public sealed class CMD_MultiRecruitPass : CMD
{
	[SerializeField]
	private UILabel ngTX_MESSAGE;

	[SerializeField]
	private UILabel ngTX_BTN_YES;

	[SerializeField]
	private UILabel ngTX_BTN_NO;

	[SerializeField]
	private UIInput multiRecruitPassInput;

	[SerializeField]
	private UILabel multiRecruitPassLabel;

	[SerializeField]
	private GameObject submitButton;

	[SerializeField]
	private GameObject submitButtonGray;

	[SerializeField]
	private BoxCollider submitButtonCollider;

	private CMD_MultiRecruitTop parentDialog;

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
		this.SetInitLabel();
		this.SetGrayButton(true);
	}

	private void SetInitLabel()
	{
		this.ngTX_MESSAGE.text = StringMaster.GetString("Recruit-02");
		this.ngTX_BTN_YES.text = StringMaster.GetString("SystemButtonDecision");
		this.ngTX_BTN_NO.text = StringMaster.GetString("SystemButtonClose");
		this.multiRecruitPassLabel.text = StringMaster.GetString("SystemInput");
	}

	private void ClickDecisionBtn()
	{
		if (!Singleton<UserDataMng>.Instance.IsOverUnitLimit(ClassSingleton<MonsterUserDataMng>.Instance.GetMonsterNum() + ConstValue.ENABLE_MONSTER_SPACE_TOEXEC_DUNGEON))
		{
			if (!Singleton<UserDataMng>.Instance.IsOverChipLimit(ConstValue.ENABLE_CHIP_SPACE_TOEXEC_DUNGEON))
			{
				MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
				GameWebAPI.MultiRoomJoin request = new GameWebAPI.MultiRoomJoin
				{
					SetSendData = delegate(GameWebAPI.ReqData_MultiRoomJoin param)
					{
						param.roomId = 0;
						param.password = this.multiRecruitPassInput.value;
					},
					OnReceived = delegate(GameWebAPI.RespData_MultiRoomJoin response)
					{
						this.parentDialog.passInputJoinData = response;
					}
				};
				base.StartCoroutine(request.RunOneTime(delegate()
				{
					RestrictionInput.EndLoad();
					this.ClosePanel(true);
				}, delegate(Exception noop)
				{
					RestrictionInput.EndLoad();
				}, null));
			}
			else
			{
				CMD_UpperlimitChip cmd_UpperlimitChip = GUIMain.ShowCommonDialog(null, "CMD_UpperlimitChip", null) as CMD_UpperlimitChip;
				cmd_UpperlimitChip.SetType(CMD_UpperlimitChip.MessageType.QUEST);
			}
		}
		else
		{
			CMD_UpperLimit cmd_UpperLimit = GUIMain.ShowCommonDialog(null, "CMD_Upperlimit", null) as CMD_UpperLimit;
			cmd_UpperLimit.SetType(CMD_UpperLimit.MessageType.QUEST);
		}
	}

	public void CheckInputContent()
	{
		bool grayButton = false;
		if (string.IsNullOrEmpty(this.multiRecruitPassLabel.text))
		{
			grayButton = true;
		}
		this.multiRecruitPassInput.value = this.multiRecruitPassInput.value.Replace("-", string.Empty);
		this.multiRecruitPassInput.value = this.multiRecruitPassInput.value.Replace(" ", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty);
		this.multiRecruitPassInput.label.text = this.multiRecruitPassInput.value;
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

	public void SetParentDialog(CMD_MultiRecruitTop dialog)
	{
		this.parentDialog = dialog;
	}
}
