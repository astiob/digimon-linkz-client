using Master;
using System;
using System.Linq;
using UnityEngine;

public sealed class CMD_ChipReinforcementModal : CMD
{
	[SerializeField]
	private UILabel title;

	[SerializeField]
	private CMD_ChipReinforcementModal.ReinforcementInfo prevReinforcementInfo;

	[SerializeField]
	private CMD_ChipReinforcementModal.ReinforcementInfo nextReinforcementInfo;

	[SerializeField]
	private CMD_ChipReinforcementModal.ConsumptionInfo consumptionInfo;

	[SerializeField]
	private GUICollider decisionButton;

	[SerializeField]
	private UILabel decisionButtonLabel;

	[SerializeField]
	private GUICollider cancelButton;

	[SerializeField]
	private UILabel cancelButtonLabel;

	[SerializeField]
	private GUICollider closeButton;

	private GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChip;

	public static CMD_ChipReinforcementModal Create(GameWebAPI.RespDataCS_ChipListLogic.UserChipList data, Action<int> callback = null)
	{
		CMD_ChipReinforcementModal cmd_ChipReinforcementModal = GUIMain.ShowCommonDialog(callback, "CMD_ChipReinforcedModal") as CMD_ChipReinforcementModal;
		cmd_ChipReinforcementModal.SetParam(data);
		return cmd_ChipReinforcementModal;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
		this.title.text = StringMaster.GetString("ChipReinforcementModal-01");
		this.prevReinforcementInfo.title.text = StringMaster.GetString("ChipReinforcementModal-02");
		this.nextReinforcementInfo.title.text = StringMaster.GetString("ChipReinforcementModal-03");
		this.consumptionInfo.title.text = StringMaster.GetString("ChipReinforcementModal-04");
		this.decisionButtonLabel.text = StringMaster.GetString("SystemButtonYes");
		this.cancelButtonLabel.text = StringMaster.GetString("SystemButtonNo");
		this.decisionButton.CallBackClass = base.gameObject;
		this.decisionButton.MethodToInvoke = "OnDecisionButton";
		this.cancelButton.CallBackClass = base.gameObject;
		this.cancelButton.MethodToInvoke = "OnCancelButton";
		this.closeButton.CallBackClass = base.gameObject;
		this.closeButton.MethodToInvoke = "OnCloseButton";
	}

	public void SetParam(GameWebAPI.RespDataCS_ChipListLogic.UserChipList data)
	{
		this.userChip = data;
		GameWebAPI.RespDataMA_ChipM.Chip prevMaterChip = ChipDataMng.GetChipMainData(this.userChip.chipId.ToString());
		this.prevReinforcementInfo.name.text = prevMaterChip.name;
		this.prevReinforcementInfo.description.text = prevMaterChip.detail;
		this.prevReinforcementInfo.chipIcon.SetData(prevMaterChip, -1, -1);
		GameWebAPI.RespDataMA_ChipM.Chip chipEnhancedData = ChipDataMng.GetChipEnhancedData(this.userChip.chipId.ToString());
		this.nextReinforcementInfo.name.text = chipEnhancedData.name;
		this.nextReinforcementInfo.description.text = chipEnhancedData.detail;
		this.nextReinforcementInfo.chipIcon.SetData(chipEnhancedData, -1, -1);
		int num = prevMaterChip.needChip.ToInt32();
		foreach (ChipIcon chipIcon in this.consumptionInfo.chipIcons)
		{
			chipIcon.SetActive(false);
		}
		for (int j = 0; j < num; j++)
		{
			this.consumptionInfo.chipIcons[j].SetActive(true);
			this.consumptionInfo.chipIcons[j].SetData(prevMaterChip, -1, -1);
		}
		GameWebAPI.RespDataCS_ChipListLogic userChipData = ChipDataMng.userChipData;
		int num2 = userChipData.userChipList.Count((GameWebAPI.RespDataCS_ChipListLogic.UserChipList x) => this.userChip.userChipId != x.userChipId && x.chipId.ToString() == prevMaterChip.chipId && x.userMonsterId <= 0);
		bool flag = num2 >= num;
		if (flag)
		{
			this.consumptionInfo.message.text = StringMaster.GetString("ChipReinforcementModal-05");
			this.consumptionInfo.message.color = ConstValue.DEFAULT_COLOR;
			this.EnableDecisionButton(true);
		}
		else
		{
			for (int k = 0; k < num; k++)
			{
				bool flag2 = k >= num2;
				this.consumptionInfo.chipIcons[k].SetSelectColor(flag2);
				this.consumptionInfo.chipIcons[k].SetSelectRankColor(flag2);
			}
			this.consumptionInfo.message.text = StringMaster.GetString("ChipReinforcementModal-06");
			this.consumptionInfo.message.color = Color.red;
			this.EnableDecisionButton(false);
		}
	}

	private void OnDecisionButton()
	{
		base.SetForceReturnValue(1);
		this.ClosePanel(true);
	}

	private void OnCancelButton()
	{
		base.SetForceReturnValue(0);
		this.ClosePanel(true);
	}

	private void OnCloseButton()
	{
		base.SetForceReturnValue(0);
		this.ClosePanel(true);
	}

	private void EnableDecisionButton(bool value)
	{
		UISprite component = this.decisionButton.GetComponent<UISprite>();
		component.GetComponent<BoxCollider>().enabled = value;
		component.spriteName = ((!value) ? "Common02_Btn_BaseG" : "Common02_Btn_BaseON1");
		this.decisionButtonLabel.color = ((!value) ? ConstValue.DEACTIVE_BUTTON_LABEL : Color.white);
	}

	[Serializable]
	public class ReinforcementInfo
	{
		public UILabel title;

		public UILabel name;

		public UILabel description;

		public ChipIcon chipIcon;
	}

	[Serializable]
	public class ConsumptionInfo
	{
		public UILabel title;

		public UILabel message;

		public ChipIcon[] chipIcons;
	}
}
