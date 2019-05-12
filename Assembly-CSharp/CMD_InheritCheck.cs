using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_InheritCheck : CMD
{
	[SerializeField]
	[Header("タイトルのラベル")]
	private UILabel titleLabel;

	[SerializeField]
	[Header("継承技タイトルラベル")]
	private UILabel inheritTitleLabel;

	[SerializeField]
	private MonsterLearnSkill monsterSuccessionSkill;

	[SerializeField]
	[Header("消費クラスタのラベル")]
	private UILabel useClusterLabel;

	[SerializeField]
	[Header("基本的なメッセージのラベル")]
	private UILabel normalMessageLabel;

	[Header("警告のメッセージのラベル")]
	[SerializeField]
	private UILabel warningMessageLabel;

	[SerializeField]
	[Header("警告ダイアログの窓本体")]
	private GameObject dialogPlate;

	[SerializeField]
	[Header("警告ダイアログのボタングループ")]
	private GameObject btnGroup;

	public void SetParams(List<MonsterData> selectedMonsterDataList, string useCluster)
	{
		this.monsterSuccessionSkill.SetSkill(selectedMonsterDataList[0]);
		bool flag = MonsterDataMng.Instance().HasArousal(selectedMonsterDataList);
		bool flag2 = MonsterDataMng.Instance().HasChip(selectedMonsterDataList);
		bool flag3 = MonsterDataMng.Instance().HasGrowStepHigh(selectedMonsterDataList);
		List<string> list = new List<string>();
		if (flag)
		{
			list.Add(StringMaster.GetString("SuccessionCautionArousal"));
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
				list.Add(string.Format(StringMaster.GetString("SuccessionCautionGrowth"), monsterGrowStepM[i].monsterGrowStepName));
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
		this.useClusterLabel.text = useCluster;
	}

	protected override void Awake()
	{
		base.Awake();
		this.titleLabel.text = StringMaster.GetString("SuccessionConfirmTitle");
		this.normalMessageLabel.text = StringMaster.GetString("SuccessionConfirmInfo");
		this.inheritTitleLabel.text = StringMaster.GetString("CharaStatus-20");
	}
}
