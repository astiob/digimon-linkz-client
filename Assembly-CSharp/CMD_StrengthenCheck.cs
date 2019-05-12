using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_StrengthenCheck : CMD
{
	[Header("サムネイルのアイコン達")]
	[SerializeField]
	private GUIMonsterIcon[] guiMonsterIcons;

	[SerializeField]
	[Header("タイトルのラベル")]
	private UILabel titleLabel;

	[SerializeField]
	[Header("消費クラスタのラベル")]
	private UILabel useClusterLabel;

	[Header("強化前のレベルのラベル")]
	[SerializeField]
	private UILabel beforeLevelLabel;

	[SerializeField]
	[Header("強化後のレベルのラベル")]
	private UILabel afterLevelLabel;

	[Header("上昇値のレベルのラベル")]
	[SerializeField]
	private UILabel plusLevelLabel;

	[Header("基本的なメッセージのラベル")]
	[SerializeField]
	private UILabel normalMessageLabel;

	[Header("警告のメッセージのラベル")]
	[SerializeField]
	private UILabel warningMessageLabel;

	[SerializeField]
	[Header("警告ダイアログの窓本体")]
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
		bool flag = MonsterDataMng.Instance().HasArousal(selectedMonsterDataList);
		bool flag2 = MonsterDataMng.Instance().HasChip(selectedMonsterDataList);
		bool flag3 = MonsterDataMng.Instance().HasGrowStepHigh(selectedMonsterDataList);
		List<string> list = new List<string>();
		if (flag)
		{
			list.Add(StringMaster.GetString("ReinforcementCautionArousal"));
		}
		if (flag2)
		{
			list.Add(StringMaster.GetString("CautionDisappearChip"));
		}
		if (flag3)
		{
			GameWebAPI.RespDataMA_GetMonsterGrowStepM.MonsterGrowStepM[] monsterGrowStepM = MasterDataMng.Instance().RespDataMA_MonsterGrowStepM.monsterGrowStepM;
			string b = ConstValue.GROW_STEP_HIGH.ToString();
			int i;
			for (i = 0; i < monsterGrowStepM.Length; i++)
			{
				if (monsterGrowStepM[i].monsterGrowStepId == b)
				{
					break;
				}
			}
			if (i < monsterGrowStepM.Length)
			{
				list.Add(string.Format(StringMaster.GetString("ReinforcementCautionGrowth"), monsterGrowStepM[i].monsterGrowStepName));
			}
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
		for (int j = 0; j < this.guiMonsterIcons.Length; j++)
		{
			if (selectedMonsterDataList.Count > j)
			{
				GUIMonsterIcon guimonsterIcon = this.guiMonsterIcons[j];
				MonsterData md = selectedMonsterDataList[j];
				this.CreateIcon(j, md, guimonsterIcon.gameObject);
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
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		Transform transform = goEmpty.transform;
		GUIMonsterIcon guimonsterIcon = monsterDataMng.MakePrefabByMonsterData(md, transform.localScale, transform.localPosition, transform.parent, true, false);
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
