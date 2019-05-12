using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_SaleCheck : CMD
{
	[Header("サムネイルのアイコン達")]
	[SerializeField]
	private GUIMonsterIcon[] guiMonsterIcons;

	[SerializeField]
	[Header("タイトルのラベル")]
	private UILabel titleLabel;

	[SerializeField]
	[Header("取得クラスタのタイトルラベル")]
	private UILabel getClusterTitleLabel;

	[Header("取得クラスタのラベル")]
	[SerializeField]
	private UILabel getClusterLabel;

	[Header("基本的なメッセージのラベル")]
	[SerializeField]
	private UILabel normalMessageLabel;

	[SerializeField]
	[Header("警告のメッセージのラベル")]
	private UILabel warningMessageLabel;

	[Header("警告ダイアログの窓本体")]
	[SerializeField]
	private GameObject dialogPlate;

	[SerializeField]
	[Header("警告ダイアログのボタングループ")]
	private GameObject btnGroup;

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	public void SetParams(List<MonsterData> selectedMonsterDataList, string getCluster)
	{
		this.titleLabel.text = StringMaster.GetString("SaleConfirmTitle");
		this.getClusterTitleLabel.text = StringMaster.GetString("SaleConfirmGain");
		this.normalMessageLabel.text = StringMaster.GetString("SaleConfirmInfo");
		bool flag = MonsterDataMng.Instance().HasChip(selectedMonsterDataList);
		bool flag2 = MonsterDataMng.Instance().HasGrowStepHigh(selectedMonsterDataList);
		List<string> list = new List<string>();
		bool flag3 = false;
		bool flag4 = false;
		foreach (MonsterData monsterData in selectedMonsterDataList)
		{
			bool flag5 = monsterData.IsArousal();
			bool flag6 = monsterData.IsVersionUp();
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
			list.Add(StringMaster.GetString("SaleCautionArousal"));
		}
		if (flag4)
		{
			list.Add(StringMaster.GetString("SaleCautionVersionUp"));
		}
		if (flag)
		{
			list.Add(StringMaster.GetString("SaleCautionChip"));
		}
		if (flag2)
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
				list.Add(string.Format(StringMaster.GetString("SaleCautionGrowth"), monsterGrowStepM[i].monsterGrowStepName));
			}
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
		for (int j = 0; j < this.guiMonsterIcons.Length; j++)
		{
			if (selectedMonsterDataList.Count > j)
			{
				GUIMonsterIcon guimonsterIcon = this.guiMonsterIcons[j];
				MonsterData md = selectedMonsterDataList[j];
				this.CreateIcon(j, md, guimonsterIcon.gameObject);
			}
		}
		this.getClusterLabel.text = getCluster;
	}

	public void SetParams(List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> selectedUserChipList, string getCluster)
	{
		this.titleLabel.text = StringMaster.GetString("ChipSaleCheck-01");
		this.getClusterTitleLabel.text = StringMaster.GetString("SaleConfirmGain");
		this.normalMessageLabel.text = StringMaster.GetString("ChipSaleCheck-02");
		bool flag = false;
		for (int i = 0; i < selectedUserChipList.Count; i++)
		{
			GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(selectedUserChipList[i].chipId.ToString());
			if (chipMainData.rank.ToInt32() >= 7)
			{
				flag = true;
			}
			this.CreateIcon(chipMainData, this.guiMonsterIcons[i].gameObject);
		}
		this.warningMessageLabel.text = ((!flag) ? string.Empty : string.Format(StringMaster.GetString("ChipSaleCheck-03"), "A"));
		this.getClusterLabel.text = getCluster;
	}

	protected override void Awake()
	{
		base.Awake();
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

	private void CreateIcon(GameWebAPI.RespDataMA_ChipM.Chip masterChip, GameObject goEmpty)
	{
		Action<UnityEngine.Object> actEnd = delegate(UnityEngine.Object obj)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(obj) as GameObject;
			gameObject.transform.SetParent(goEmpty.transform.parent);
			gameObject.transform.localPosition = goEmpty.transform.localPosition;
			gameObject.transform.localScale = new Vector3(0.92f, 0.92f, 1f);
			UIWidget component = goEmpty.GetComponent<UIWidget>();
			DepthController component2 = gameObject.GetComponent<DepthController>();
			component2.AddWidgetDepth(component.depth);
			ChipIcon component3 = gameObject.GetComponent<ChipIcon>();
			component3.SetData(masterChip, -1, -1);
			NGUITools.DestroyImmediate(goEmpty);
		};
		AssetDataMng.Instance().LoadObjectASync("UICommon/ListParts/ListPartsChip", actEnd);
	}
}
