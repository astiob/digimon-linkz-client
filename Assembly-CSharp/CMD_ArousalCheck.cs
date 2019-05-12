using Master;
using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_ArousalCheck : CMD
{
	[SerializeField]
	private GUIMonsterIcon icon;

	[SerializeField]
	private MonsterResistanceList monsterResistanceList;

	[Header("タイトルのラベル")]
	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	[Header("基本的なメッセージのラベル")]
	private UILabel normalMessageLabel;

	[Header("警告のメッセージのラベル")]
	[SerializeField]
	private UILabel warningMessageLabel;

	[SerializeField]
	[Header("Yesボタンのラベル")]
	private UILabel buttonYesLabel;

	[Header("Noボタンのラベル")]
	[SerializeField]
	private UILabel buttonNoLabel;

	[Header("警告ダイアログの窓本体")]
	[SerializeField]
	private GameObject dialogPlate;

	[SerializeField]
	[Header("警告ダイアログのボタングループ")]
	private GameObject btnGroup;

	protected override void Awake()
	{
		base.Awake();
		this.titleLabel.text = StringMaster.GetString("ArousalConfirmTitle");
		this.buttonYesLabel.text = StringMaster.GetString("SystemButtonYes");
		this.buttonNoLabel.text = StringMaster.GetString("SystemButtonNo");
	}

	private bool AnyArousalMonster(List<MonsterData> monsterDataList)
	{
		bool result = false;
		for (int i = 0; i < monsterDataList.Count; i++)
		{
			if (MonsterStatusData.IsArousal(monsterDataList[i].GetMonsterMaster().Simple.rare))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public void SetParams(MonsterData mData, List<MonsterData> partnerDigimons, bool isGrowStepMax)
	{
		this.CreateIcon(mData, this.icon.gameObject);
		this.normalMessageLabel.text = StringMaster.GetString("ArousalConfirmInfo");
		bool flag = this.AnyArousalMonster(partnerDigimons);
		bool flag2 = MonsterUserDataMng.AnyChipEquipMonster(partnerDigimons);
		List<string> list = new List<string>();
		if (flag)
		{
			list.Add(StringMaster.GetString("ArousalCautionArousal"));
		}
		if (flag2)
		{
			list.Add(StringMaster.GetString("CautionDisappearChip"));
		}
		if (list.Count >= 2)
		{
			int num = (this.warningMessageLabel.fontSize + this.warningMessageLabel.spacingY * 2) * (list.Count - 1);
			this.warningMessageLabel.transform.SetLocalY(this.warningMessageLabel.transform.localPosition.y - (float)(num / 2));
			this.dialogPlate.transform.SetLocalY(this.dialogPlate.transform.localPosition.y - (float)(num / 2));
			this.dialogPlate.GetComponent<UIWidget>().height += num;
			this.btnGroup.transform.SetLocalY(this.btnGroup.transform.localPosition.y - (float)num);
		}
		this.warningMessageLabel.text = string.Join("\n", list.ToArray());
		if (!isGrowStepMax)
		{
			UILabel uilabel = this.normalMessageLabel;
			uilabel.text += StringMaster.GetString("ArousalCautionLost");
		}
	}

	private void CreateIcon(MonsterData md, GameObject goEmpty)
	{
		Transform transform = goEmpty.transform;
		GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(md, transform.localScale, transform.localPosition, transform.parent, true, false);
		guimonsterIcon.playSelectSE = false;
		guimonsterIcon.SendMoveToParent = false;
		guimonsterIcon.CancelTouchEndByMove = false;
		guimonsterIcon.gameObject.name = "MonsterIcon ";
		this.monsterResistanceList.SetValues(md);
		UIWidget component = goEmpty.GetComponent<UIWidget>();
		UIWidget component2 = guimonsterIcon.gameObject.GetComponent<UIWidget>();
		if (component != null && component2 != null)
		{
			int add = component.depth - component2.depth;
			DepthController component3 = guimonsterIcon.GetComponent<DepthController>();
			component3.AddWidgetDepth(guimonsterIcon.transform, add);
		}
		NGUITools.DestroyImmediate(goEmpty);
	}
}
