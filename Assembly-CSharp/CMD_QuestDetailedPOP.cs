using FarmData;
using Master;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.Quest;
using UnityEngine;

public sealed class CMD_QuestDetailedPOP : CMD
{
	[Header("出現する敵リスト")]
	[SerializeField]
	private List<GameObject> goENCOUNT_MONS_LIST;

	[Header("ドロップするアイテムリスト")]
	[SerializeField]
	private List<PresentBoxItem> itemDROP_ITEM_LIST;

	[SerializeField]
	private GameObject goTEX_DROP;

	[SerializeField]
	private UILabel ngTX_A_STAGE_DNG_TITLE;

	[Header("ステージの数字のラベル")]
	[SerializeField]
	private UILabel ngTX_A_STAGE_DNG_NUMBER;

	[Header("コンテンツ・ルート (ヒナと吹き出し)")]
	[SerializeField]
	private GameObject goContentsROOT;

	[Header("ポイントコンテンツ・ルート (ボーナス)")]
	[SerializeField]
	private GameObject goPointContentsROOT;

	[Header("詳細説明 (ヒナの吹き出し)")]
	[SerializeField]
	private UILabel ngTX_A_STAGE_DNG_DESCRIPT;

	[Header("ポイント・タイトルのリスト)")]
	[SerializeField]
	private List<UILabel> bonusPointLabelList;

	[SerializeField]
	private GameObject changeBonusButton;

	[Header("取得できるEXPのタイトル")]
	[SerializeField]
	private UILabel getEXPTitleLabel;

	[Header("取得できるEXP")]
	[SerializeField]
	private UILabel getEXPLabel;

	[Header("取得できるクラスタタイトル")]
	[SerializeField]
	private UILabel getClusterTitleLabel;

	[Header("取得できるクラスタ数")]
	[SerializeField]
	private UILabel getClusterLabel;

	[Header("消費するスタミナタイトル")]
	[SerializeField]
	private UILabel getStaminaTitleLabel;

	[Header("消費するスタミナ")]
	[SerializeField]
	private UILabel getStaminaLabel;

	[Header("バトルボタン")]
	[SerializeField]
	private GameObject goBattleBtn;

	private UISprite spBattleBtn;

	[Header("マルチバトルボタン")]
	[SerializeField]
	private GameObject goMultiBattleBtn;

	[SerializeField]
	private RewardIconRoot rewardIconRoot;

	[Header("初回報酬取得済みカラー")]
	[SerializeField]
	private Color clearColor = Color.gray;

	[SerializeField]
	private QuestDetailedBonusPoint bonusPointUI;

	private string areaId;

	private QuestData.WorldDungeonData dungeonData;

	private string dungeonNo;

	private bool isClosed;

	private List<GameWebAPI.RespDataWD_GetDungeonInfo.DropAsset> dropAssetList;

	private const int BOSS_ID = 2;

	private const int BOSS_ID_AP_FREE = 4;

	private static GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo;

	public static GameWebAPI.RespDataCP_Campaign.CampaignInfo CampaignInfo
	{
		set
		{
			CMD_QuestDetailedPOP.campaignInfo = value;
		}
	}

	public void SetQuestData(string worldAreaId, QuestData.WorldDungeonData worldDungeonData, string worldDungeonNo)
	{
		this.areaId = worldAreaId;
		this.dungeonData = worldDungeonData;
		this.dungeonNo = worldDungeonNo;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.HideDLG();
		base.StartCoroutine(this.Initialize(f, sizeX, sizeY, aT));
	}

	private IEnumerator Initialize(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.getEXPTitleLabel.text = StringMaster.GetString("QuestDetailsExp");
		this.getClusterTitleLabel.text = StringMaster.GetString("QuestDetailsTip");
		this.getStaminaTitleLabel.text = StringMaster.GetString("QuestDetailsCost");
		this.ShowInfo();
		yield return null;
		int stamina = int.Parse(this.dungeonData.worldDungeonM.needStamina);
		if (CMD_QuestDetailedPOP.campaignInfo != null)
		{
			float num = (float)stamina;
			stamina = Mathf.CeilToInt(num * float.Parse(CMD_QuestDetailedPOP.campaignInfo.rate));
		}
		this.getStaminaLabel.text = stamina.ToString();
		this.InitializeRewardList();
		this.DispMultiBattleBtn();
		yield return null;
		base.ShowDLG();
		base.Show(f, sizeX, sizeY, aT);
		yield break;
	}

	public override void ClosePanel(bool animation = true)
	{
		this.isClosed = true;
		base.ClosePanel(animation);
	}

	private void ShowInfo()
	{
		GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dungeon = this.dungeonData.dungeon;
		ClassSingleton<PartyBossIconsAccessor>.Instance.StageEnemies = dungeon.encountEnemies;
		this.ngTX_A_STAGE_DNG_NUMBER.text = this.dungeonNo;
		this.ngTX_A_STAGE_DNG_TITLE.text = this.dungeonData.worldDungeonM.name;
		this.ngTX_A_STAGE_DNG_DESCRIPT.text = this.dungeonData.worldDungeonM.description;
		this.getEXPLabel.text = dungeon.exp.ToString();
		this.getClusterLabel.text = StringFormat.Cluster(dungeon.money);
		int num = 0;
		int i;
		for (i = 0; i < dungeon.encountEnemies.Length; i++)
		{
			if (num >= this.goENCOUNT_MONS_LIST.Count)
			{
				break;
			}
			GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy encountEnemy = dungeon.encountEnemies[i];
			if (encountEnemy.type == 2 || encountEnemy.type == 4)
			{
				string text = encountEnemy.monsterId.ToString();
				if (!string.IsNullOrEmpty(text))
				{
					MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(text);
					if (monsterData != null)
					{
						GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(monsterData, this.goENCOUNT_MONS_LIST[num].transform.localScale, this.goENCOUNT_MONS_LIST[num].transform.localPosition, this.goENCOUNT_MONS_LIST[num].transform.parent, true, false);
						DepthController depthController = guimonsterIcon.GetDepthController();
						depthController.AddWidgetDepth(guimonsterIcon.transform, 40);
						guimonsterIcon.SetTouchAct_S(delegate(MonsterData tappedMonsterData)
						{
							this.ActCallBackEncMons(tappedMonsterData, encountEnemy.resistanceId);
						});
						this.goENCOUNT_MONS_LIST[num].SetActive(false);
						num++;
					}
				}
			}
		}
		if (this.dungeonData.dungeon.isExtraWave == 1 && num < this.goENCOUNT_MONS_LIST.Count)
		{
			GUIMonsterIcon guimonsterIcon2 = GUIMonsterIcon.MakeQuestionPrefab(this.goENCOUNT_MONS_LIST[num].transform.localScale, this.goENCOUNT_MONS_LIST[num].transform.localPosition, (int)(this.goENCOUNT_MONS_LIST[num].transform.localPosition.z + 35f), this.goENCOUNT_MONS_LIST[num].transform.parent);
			guimonsterIcon2.SetTouchAct_S(null);
			guimonsterIcon2.SetTouchAct_L(null);
		}
		while (i < this.goENCOUNT_MONS_LIST.Count)
		{
			this.goENCOUNT_MONS_LIST[i].SetActive(false);
			i++;
		}
		this.ShowDropItems();
		this.goContentsROOT.SetActive(true);
		this.goPointContentsROOT.SetActive(true);
		this.bonusPointUI.Initialize(this.areaId, this.dungeonData.worldDungeonM.worldStageId, this.dungeonData.worldDungeonM.worldDungeonId);
		this.bonusPointUI.SetBonusUI();
	}

	private void ShowDropItems()
	{
		GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dungeon = this.dungeonData.dungeon;
		this.dropAssetList = new List<GameWebAPI.RespDataWD_GetDungeonInfo.DropAsset>();
		QuestData.CreateDropAssetList(dungeon, this.dropAssetList);
		int num = 0;
		for (int i = 0; i < this.dropAssetList.Count; i++)
		{
			if (i >= this.itemDROP_ITEM_LIST.Count)
			{
				break;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.goTEX_DROP);
			Transform transform = gameObject.transform;
			Transform transform2 = this.itemDROP_ITEM_LIST[i].transform;
			transform.parent = transform2.parent;
			transform.localScale = transform2.transform.localScale;
			transform.localPosition = transform2.localPosition;
			int index = i;
			GUICollider component = gameObject.GetComponent<GUICollider>();
			component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
			{
				this.ActCallBackDropItem(index);
			};
			UIWidget component2 = this.itemDROP_ITEM_LIST[i].GetComponent<UIWidget>();
			UIWidget component3 = gameObject.GetComponent<UIWidget>();
			component3.depth = component2.depth;
			string assetCategoryId = this.dropAssetList[i].assetCategoryId.ToString();
			string objectId = this.dropAssetList[i].assetValue.ToString();
			this.itemDROP_ITEM_LIST[i].SetItem(assetCategoryId, objectId, "1", true, null);
			BoxCollider[] componentsInChildren = this.itemDROP_ITEM_LIST[i].GetComponentsInChildren<BoxCollider>();
			if (componentsInChildren != null)
			{
				foreach (BoxCollider boxCollider in componentsInChildren)
				{
					boxCollider.enabled = false;
				}
			}
			num++;
		}
		if (this.dungeonData.dungeon.isExtraWave == 1 && num < this.itemDROP_ITEM_LIST.Count)
		{
			GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakeQuestionPrefab(new Vector3(0.46f, 0.46f, 1f), this.itemDROP_ITEM_LIST[num].transform.localPosition, (int)(this.itemDROP_ITEM_LIST[num].transform.localPosition.z + 35f), this.itemDROP_ITEM_LIST[num].transform.parent);
			guimonsterIcon.SetTouchAct_S(null);
			guimonsterIcon.SetTouchAct_L(null);
		}
		this.goTEX_DROP.SetActive(false);
	}

	private void ActCallBackEncMons(MonsterData md, int resistanceId)
	{
		if (this.isClosed)
		{
			return;
		}
		CMD_QuestMonsterPOP cmd_QuestMonsterPOP = GUIMain.ShowCommonDialog(null, "CMD_QuestMonsterPOP", null) as CMD_QuestMonsterPOP;
		cmd_QuestMonsterPOP.SetBossDetails(md, resistanceId);
	}

	private void ActCallBackDropItem(int idx)
	{
		if (this.isClosed)
		{
			return;
		}
		MasterDataMng.AssetCategory assetCategoryId = (MasterDataMng.AssetCategory)this.dropAssetList[idx].assetCategoryId;
		string text = this.dropAssetList[idx].assetValue.ToString();
		GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(this.dropAssetList[idx].assetCategoryId.ToString());
		switch (assetCategoryId)
		{
		case MasterDataMng.AssetCategory.MONSTER:
			break;
		case MasterDataMng.AssetCategory.DIGI_STONE:
			CMD_QuestItemPOP.Create(assetCategory);
			break;
		case MasterDataMng.AssetCategory.LINK_POINT:
			CMD_QuestItemPOP.Create(assetCategory);
			break;
		case MasterDataMng.AssetCategory.TIP:
			CMD_QuestItemPOP.Create(assetCategory);
			break;
		default:
			switch (assetCategoryId)
			{
			case MasterDataMng.AssetCategory.SOUL:
			{
				GameWebAPI.RespDataMA_GetSoulM.SoulM soul = MasterDataMng.Instance().RespDataMA_SoulM.GetSoul(text);
				CMD_QuestItemPOP.Create(soul);
				break;
			}
			case MasterDataMng.AssetCategory.FACILITY_KEY:
			{
				FacilityConditionM[] facilityCondition = FarmDataManager.GetFacilityCondition(text);
				FacilityConditionM facilityConditionM = facilityCondition.FirstOrDefault((FacilityConditionM x) => int.Parse(x.conditionType) == 1);
				FacilityM facilityMasterByReleaseId = FarmDataManager.GetFacilityMasterByReleaseId(facilityConditionM.releaseId);
				CMD_QuestItemPOP.Create(facilityConditionM, text, facilityMasterByReleaseId);
				break;
			}
			case MasterDataMng.AssetCategory.CHIP:
			{
				GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(text);
				CMD_QuestItemPOP.Create(chipMainData);
				break;
			}
			case MasterDataMng.AssetCategory.DUNGEON_TICKET:
			{
				string ticketValue = text;
				GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM data = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster.dungeonTicketM.FirstOrDefault((GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM x) => x.dungeonTicketId == ticketValue);
				CMD_QuestItemPOP.Create(data);
				break;
			}
			}
			break;
		case MasterDataMng.AssetCategory.ITEM:
		{
			GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(text);
			CMD_QuestItemPOP.Create(itemM);
			break;
		}
		}
	}

	private void InitializeRewardList()
	{
		List<GameWebAPI.RespDataMA_GetWorldDungeonRewardM.WorldDungeonReward> list = new List<GameWebAPI.RespDataMA_GetWorldDungeonRewardM.WorldDungeonReward>();
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonRewardM.WorldDungeonReward worldDungeonReward in MasterDataMng.Instance().RespDataMA_WorldDungeonRewardM.worldDungeonRewardM)
		{
			if (worldDungeonReward.worldDungeonId == this.dungeonData.worldDungeonM.worldDungeonId && worldDungeonReward.everyTimeFlg == "0")
			{
				list.Add(worldDungeonReward);
			}
		}
		int status = this.dungeonData.status;
		if (status != 2 && status != 3)
		{
			if (status == 4)
			{
				this.rewardIconRoot.SetRewardList(list, this.clearColor);
			}
		}
		else
		{
			this.rewardIconRoot.SetRewardList(list, Color.white);
		}
	}

	private void DispMultiBattleBtn()
	{
		if (this.goMultiBattleBtn != null)
		{
			this.goMultiBattleBtn.SetActive(true);
		}
	}

	private void OnTapMultiBattleBtn()
	{
		GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseMultiBattleMenu), "CMD_MultiBattleParticipateMenu", null);
	}

	private void OnCloseMultiBattleMenu(int idx)
	{
		int forceReturnValue;
		switch (idx)
		{
		case 1:
			forceReturnValue = 10;
			goto IL_2F;
		case 2:
			forceReturnValue = 20;
			goto IL_2F;
		}
		forceReturnValue = 99;
		IL_2F:
		base.SetForceReturnValue(forceReturnValue);
		base.ClosePanel(true);
	}
}
