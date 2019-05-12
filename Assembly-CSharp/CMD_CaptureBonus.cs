using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_CaptureBonus : CMD
{
	[SerializeField]
	private CaptureBonusItem captureBonusItem;

	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel itemDetileLabel;

	[SerializeField]
	private UILabel presentBoxLabel;

	[SerializeField]
	private float iconMergine = 10f;

	[SerializeField]
	private int maxColumnNum = 3;

	public static CMD_CaptureBonus Create(string title, string info, Action<int> onCloseAction = null)
	{
		return GUIMain.ShowCommonDialog(onCloseAction, "CMD_CaptureBonus") as CMD_CaptureBonus;
	}

	public void DialogDataSet(GameWebAPI.RespDataGA_ExecGacha.GachaRewardsData[] RewardsData)
	{
		this.titleLabel.text = StringMaster.GetString("CaptureBonusTitle");
		this.presentBoxLabel.text = StringMaster.GetString("CaptureBonusText");
		this.captureBonusItem.gameObject.SetActive(true);
		int num = (this.maxColumnNum + 1 >= RewardsData.Length) ? RewardsData.Length : (this.maxColumnNum + 1);
		float num2 = this.captureBonusItem.GetComponent<BoxCollider>().size.x + this.iconMergine;
		float num3 = num2 / 2f * (float)(num - 1);
		List<string> list = new List<string>();
		for (int i = 0; i < RewardsData.Length; i++)
		{
			CaptureBonusItem captureBonusItem = UnityEngine.Object.Instantiate<CaptureBonusItem>(this.captureBonusItem);
			captureBonusItem.transform.parent = base.gameObject.transform;
			captureBonusItem.transform.localScale = this.captureBonusItem.transform.localScale;
			captureBonusItem.transform.localRotation = this.captureBonusItem.transform.localRotation;
			captureBonusItem.transform.localPosition = new Vector3(this.captureBonusItem.transform.localPosition.x - num3 + num2 * (float)i, this.captureBonusItem.transform.localPosition.y, this.captureBonusItem.transform.localPosition.z);
			if (i >= this.maxColumnNum)
			{
				captureBonusItem.ActivateMore();
				list.Add(StringMaster.GetString("ItemTruncated"));
				break;
			}
			GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(RewardsData[i].assetCategoryId);
			MasterDataMng.AssetCategory assetCategoryId = (MasterDataMng.AssetCategory)int.Parse(RewardsData[i].assetCategoryId);
			list.Add(captureBonusItem.DialogDataSet(assetCategory, assetCategoryId, RewardsData[i]));
		}
		this.captureBonusItem.gameObject.SetActive(false);
		this.itemDetileLabel.text = string.Join("\n", list.ToArray());
	}

	private string GetDetailText(GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM masterUpgradeCategory, MasterDataMng.AssetCategory assetCategoryId, GameWebAPI.RespDataGA_ExecGacha.GachaRewardsData RewardsData)
	{
		string text = string.Empty;
		if (masterUpgradeCategory != null)
		{
			text = masterUpgradeCategory.assetTitle;
		}
		string arg = string.Empty;
		switch (assetCategoryId)
		{
		case MasterDataMng.AssetCategory.MONSTER:
		{
			MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(RewardsData.assetValue);
			arg = monsterData.monsterMG.monsterName;
			goto IL_12A;
		}
		case MasterDataMng.AssetCategory.DIGI_STONE:
			arg = text;
			goto IL_12A;
		case MasterDataMng.AssetCategory.LINK_POINT:
			arg = text;
			goto IL_12A;
		case MasterDataMng.AssetCategory.TIP:
			arg = text;
			goto IL_12A;
		case MasterDataMng.AssetCategory.ITEM:
		{
			GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(RewardsData.assetValue);
			if (itemM != null)
			{
				arg = itemM.name;
			}
			goto IL_12A;
		}
		case MasterDataMng.AssetCategory.MEAT:
			arg = text;
			goto IL_12A;
		case MasterDataMng.AssetCategory.SOUL:
		{
			GameWebAPI.RespDataMA_GetSoulM.SoulM soul = MasterDataMng.Instance().RespDataMA_SoulM.GetSoul(RewardsData.assetValue);
			arg = soul.soulName;
			goto IL_12A;
		}
		case MasterDataMng.AssetCategory.FACILITY_KEY:
			arg = text;
			goto IL_12A;
		case MasterDataMng.AssetCategory.CHIP:
		{
			GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(RewardsData.assetValue);
			arg = chipMainData.name;
			goto IL_12A;
		}
		}
		arg = StringMaster.GetString("Present-10");
		IL_12A:
		return string.Format(StringMaster.GetString("CaptureBonusItem"), arg, RewardsData.count);
	}

	public void AdjustSize()
	{
		int num = this.itemDetileLabel.text.Split(new char[]
		{
			'\n'
		}).Length;
		int num2 = (this.itemDetileLabel.fontSize + this.itemDetileLabel.spacingY) * num;
		base.GetComponent<UIWidget>().height += num2;
	}

	protected override void Awake()
	{
		base.Awake();
	}

	private void Start()
	{
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
