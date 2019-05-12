using Master;
using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_StrengthenCheck : CMD
{
	[SerializeField]
	[Header("サムネイルのアイコン達")]
	private GUIMonsterIcon[] guiMonsterIcons;

	[Header("タイトルのラベル")]
	[SerializeField]
	private UILabel titleLabel;

	[Header("消費クラスタのラベル")]
	[SerializeField]
	private UILabel useClusterLabel;

	[Header("強化前のレベルのラベル")]
	[SerializeField]
	private UILabel beforeLevelLabel;

	[Header("強化後のレベルのラベル")]
	[SerializeField]
	private UILabel afterLevelLabel;

	[SerializeField]
	[Header("上昇値のレベルのラベル")]
	private UILabel plusLevelLabel;

	[SerializeField]
	[Header("基本的なメッセージのラベル")]
	private UILabel normalMessageLabel;

	[SerializeField]
	[Header("警告のメッセージのラベル")]
	private UILabel warningMessageLabel;

	[Header("警告ダイアログの窓本体")]
	[SerializeField]
	private GameObject dialogPlate;

	[Header("警告ダイアログのボタングループ")]
	[SerializeField]
	private GameObject btnGroup;

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	public void SetParams(List<MonsterData> selectedMonsterDataList, string useCluster, string beforeLevel, string afterLevel, string plusLevel, bool isLevelMax)
	{
		string format = string.Empty;
		if (isLevelMax)
		{
			format = StringMaster.GetString("ReinforcementInfoLvMax");
		}
		else
		{
			format = StringMaster.GetString("ReinforcementInfo");
		}
		this.normalMessageLabel.text = string.Format(format, StringMaster.GetString("ReinforcementTitle"));
		bool flag = MonsterUserDataMng.AnyChipEquipMonster(selectedMonsterDataList);
		bool flag2 = MonsterUserDataMng.AnyHighGrowStepMonster(selectedMonsterDataList);
		List<string> list = new List<string>();
		bool flag3 = false;
		bool flag4 = false;
		foreach (MonsterData monsterData in selectedMonsterDataList)
		{
			bool flag5 = MonsterStatusData.IsArousal(monsterData.GetMonsterMaster().Simple.rare);
			bool flag6 = MonsterStatusData.IsVersionUp(monsterData.GetMonsterMaster().Simple.rare);
			if (flag6)
			{
				flag4 = true;
			}
			else if (flag5)
			{
				flag3 = true;
			}
		}
		if (flag3)
		{
			list.Add(StringMaster.GetString("ReinforcementCautionArousal"));
		}
		if (flag4)
		{
			list.Add(StringMaster.GetString("ReinforcementCautionVersionUp"));
		}
		if (flag)
		{
			list.Add(StringMaster.GetString("CautionDisappearChip"));
		}
		if (flag2)
		{
			string growStep = ConstValue.GROW_STEP_HIGH.ToString();
			string growStepName = MonsterGrowStepData.GetGrowStepName(growStep);
			list.Add(string.Format(StringMaster.GetString("ReinforcementCautionGrowth"), growStepName));
		}
		if (list.Count >= 2)
		{
			int num = (this.warningMessageLabel.fontSize + this.warningMessageLabel.spacingY) * (list.Count - 1);
			this.warningMessageLabel.transform.SetLocalY(this.warningMessageLabel.transform.localPosition.y - (float)(num / 2));
			this.dialogPlate.transform.SetLocalY(this.dialogPlate.transform.localPosition.y - (float)(num / 2));
			this.dialogPlate.GetComponent<UIWidget>().height += num;
			this.btnGroup.transform.SetLocalY(this.btnGroup.transform.localPosition.y - (float)num);
		}
		this.warningMessageLabel.text = string.Join("\n", list.ToArray());
		for (int i = 0; i < this.guiMonsterIcons.Length; i++)
		{
			if (selectedMonsterDataList.Count > i)
			{
				GUIMonsterIcon guimonsterIcon = this.guiMonsterIcons[i];
				MonsterData md = selectedMonsterDataList[i];
				this.CreateIcon(i, md, guimonsterIcon.gameObject);
			}
		}
		this.useClusterLabel.text = useCluster;
		this.beforeLevelLabel.text = beforeLevel;
		this.afterLevelLabel.text = afterLevel;
		this.plusLevelLabel.text = plusLevel;
	}

	protected override void Awake()
	{
		base.Awake();
		this.titleLabel.text = StringMaster.GetString("ReinforcementConfirm");
	}

	private void CreateIcon(int index, MonsterData md, GameObject goEmpty)
	{
		Transform transform = goEmpty.transform;
		GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(md, transform.localScale, transform.localPosition, transform.parent, true, false);
		guimonsterIcon.playSelectSE = false;
		guimonsterIcon.SendMoveToParent = false;
		guimonsterIcon.CancelTouchEndByMove = false;
		guimonsterIcon.gameObject.name = "MonsterIcon " + index.ToString();
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
