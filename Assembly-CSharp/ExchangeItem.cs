using Evolution;
using FarmData;
using Master;
using Monster;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ExchangeItem : GUIListPartBS
{
	private const string SP_NAME_PLUS = "Common02_Meal_UP";

	private const string SP_NAME_MINUS = "Common02_Meal_Down";

	private const string SP_NAME_PLUS_GRAY = "Common02_Meal_UP_G";

	private const string SP_NAME_MINUS_GRAY = "Common02_Meal_Down_G";

	private const int chipTexWidth = 128;

	private const int chipTexHeight = 128;

	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel buildCountNumLabel;

	[SerializeField]
	private UILabel buildNumLabel;

	[SerializeField]
	private GameObject pushedButton;

	[SerializeField]
	private GameObject pushedButtonGreenSprite;

	[SerializeField]
	private GameObject pushedButtonGraySprite;

	[SerializeField]
	private GameObject pushedButtonExchangeLabel;

	[SerializeField]
	private GameObject pushedButtonLimitedLabel;

	[SerializeField]
	private UISprite viewIcon;

	[SerializeField]
	private UITexture viewIconTexture;

	[SerializeField]
	private UISprite exchangeViewIcon;

	[SerializeField]
	private UITexture exchangeViewTexture;

	[SerializeField]
	private GameObject iconRoot;

	private GUIMonsterIcon micon;

	private Action<ExchangeItem> onPushedButton;

	[NonSerialized]
	public int exchangeDetailId;

	[NonSerialized]
	public string exchangeDetailName;

	[NonSerialized]
	public string exchangeDetailNum;

	[NonSerialized]
	public string exchangeDetailCategoryID;

	[NonSerialized]
	public GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result.Detail.Item exchangeItemData;

	[NonSerialized]
	public string exchangeConsumeNum;

	[SerializeField]
	private UISprite numDownButtonSprite;

	[SerializeField]
	private GUICollider numDownButtonCollider;

	[SerializeField]
	private UISprite numUpButtonSprite;

	[SerializeField]
	private GUICollider numUpButtonCollider;

	[SerializeField]
	private UILabel numCountLabel;

	private int updateExCT;

	[NonSerialized]
	public int numCounter = 1;

	[NonSerialized]
	public int selectNum;

	[NonSerialized]
	public GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result.Detail exchangeInfoData;

	protected override void Update()
	{
		base.Update();
		this.UpdateExecute();
	}

	public void OnClickNumCountUp()
	{
		this.updateExCT++;
		SoundMng.Instance().PlaySE("SEInternal/Common/se_107", 0f, false, true, null, -1, 1f);
		this.OnExecute(true, 1);
	}

	public void OnClickNumCountDown()
	{
		this.updateExCT++;
		SoundMng.Instance().PlaySE("SEInternal/Common/se_107", 0f, false, true, null, -1, 1f);
		this.OnExecute(false, 1);
	}

	private void OnExecute(bool IsCountUp, int VariationValue = 1)
	{
		int maxExchangeNum = this.GetMaxExchangeNum();
		if (IsCountUp && maxExchangeNum - VariationValue < 0 && this.updateExCT != 0)
		{
			VariationValue = maxExchangeNum;
		}
		if (this.updateExCT > 1)
		{
			SoundMng.Instance().PlaySE("SEInternal/Common/se_107", 0f, false, true, null, -1, 1f);
		}
		int i = VariationValue;
		while (i > 0)
		{
			i--;
			if (IsCountUp)
			{
				this.numCounter++;
			}
			else
			{
				this.numCounter--;
			}
			if (this.numCounter == 0)
			{
				i = 0;
			}
			if (this.numCounter > maxExchangeNum)
			{
				this.numCounter = maxExchangeNum;
			}
			else if (this.numCounter < 1)
			{
				this.numCounter = 1;
			}
		}
		this.numCountLabel.text = this.numCounter.ToString();
		if (this.numCounter >= maxExchangeNum)
		{
			this.numUpButtonCollider.activeCollider = false;
			this.numUpButtonSprite.spriteName = "Common02_Meal_UP_G";
		}
		else
		{
			this.numUpButtonCollider.activeCollider = true;
			this.numUpButtonSprite.spriteName = "Common02_Meal_UP";
		}
		if (this.numCounter == 1)
		{
			this.numDownButtonCollider.activeCollider = false;
			this.numDownButtonSprite.spriteName = "Common02_Meal_Down_G";
		}
		else if (this.numCounter > 1)
		{
			this.numDownButtonCollider.activeCollider = true;
			this.numDownButtonSprite.spriteName = "Common02_Meal_Down";
		}
	}

	private int GetMaxExchangeNum()
	{
		int num = this.exchangeInfoData.item.count / int.Parse(this.exchangeInfoData.needNum);
		if (num > int.Parse(this.exchangeInfoData.limitNum) && int.Parse(this.exchangeInfoData.limitNum) > 0)
		{
			num = int.Parse(this.exchangeInfoData.limitNum);
			if (num > this.exchangeInfoData.remainCount)
			{
				num = this.exchangeInfoData.remainCount;
			}
			else if (num > 100)
			{
				num = 100;
			}
		}
		else if (num > 100)
		{
			num = 100;
		}
		else if (num > this.exchangeInfoData.remainCount && int.Parse(this.exchangeInfoData.limitNum) != 0)
		{
			num = this.exchangeInfoData.remainCount;
		}
		return num;
	}

	private void UpdateExecute()
	{
		if ((this.numCounter >= this.GetMaxExchangeNum() || this.numCounter <= 1) && this.updateExCT != 0)
		{
			this.updateExCT = 0;
			return;
		}
		if (this.numUpButtonCollider.isTouching || this.numDownButtonCollider.isTouching)
		{
			if (++this.updateExCT > 20)
			{
				if (this.updateExCT > 52)
				{
					if (this.updateExCT > 82)
					{
						this.OnExecute(this.numUpButtonCollider.isTouching, 10);
					}
					else
					{
						this.OnExecute(this.numUpButtonCollider.isTouching, 1);
					}
				}
				else if (this.updateExCT % 4 == 0)
				{
					this.OnExecute(this.numUpButtonCollider.isTouching, 1);
				}
			}
		}
		else
		{
			this.updateExCT = 0;
		}
	}

	public void SetDetail(GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result.Detail exchangeInfo, int selectItemNum, Action<ExchangeItem> touchEvent)
	{
		this.onPushedButton = touchEvent;
		GUICollider component = this.pushedButton.GetComponent<GUICollider>();
		component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			this.onPushedButton(this);
		};
		this.selectNum = selectItemNum;
		int.TryParse(exchangeInfo.eventExchangeDetailId, out this.exchangeDetailId);
		this.exchangeItemData = exchangeInfo.item;
		string text = string.Empty;
		string value = string.Empty;
		this.exchangeInfoData = exchangeInfo;
		this.viewIcon.gameObject.SetActive(true);
		this.viewIconTexture.gameObject.SetActive(false);
		GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(exchangeInfo.assetCategoryId);
		string text2 = string.Empty;
		if (assetCategory != null)
		{
			text2 = assetCategory.assetTitle;
		}
		this.exchangeDetailCategoryID = exchangeInfo.assetCategoryId;
		MasterDataMng.AssetCategory assetCategory2 = (MasterDataMng.AssetCategory)int.Parse(exchangeInfo.assetCategoryId);
		this.numCountLabel.text = "1";
		switch (assetCategory2)
		{
		case MasterDataMng.AssetCategory.MONSTER:
		{
			text = string.Empty;
			if (this.micon != null)
			{
				UnityEngine.Object.Destroy(this.micon.gameObject);
			}
			this.viewIcon.gameObject.SetActive(false);
			MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(exchangeInfo.assetValue);
			this.exchangeDetailName = monsterData.monsterMG.monsterName;
			this.micon = GUIMonsterIcon.MakePrefabByMonsterData(monsterData, new Vector3(1f, 1f, 1f), new Vector3(-165f, 25f, 0f), this.iconRoot.transform, true, false);
			UIWidget component2 = base.gameObject.GetComponent<UIWidget>();
			if (component2 != null)
			{
				DepthController component3 = this.micon.GetComponent<DepthController>();
				if (component3 != null)
				{
					component3.AddWidgetDepth(this.micon.transform, component2.depth + 10);
				}
			}
			this.micon.GetDepthController();
			BoxCollider component4 = this.micon.gameObject.GetComponent<BoxCollider>();
			if (component4 != null)
			{
				component4.enabled = false;
			}
			goto IL_58D;
		}
		case MasterDataMng.AssetCategory.DIGI_STONE:
			text = "Common02_LB_Stone";
			this.exchangeDetailName = text2;
			goto IL_58D;
		case MasterDataMng.AssetCategory.LINK_POINT:
			text = "Common02_LB_Link";
			this.exchangeDetailName = text2;
			goto IL_58D;
		case MasterDataMng.AssetCategory.TIP:
			text = "Common02_LB_Chip";
			this.exchangeDetailName = text2;
			goto IL_58D;
		case MasterDataMng.AssetCategory.ITEM:
		{
			GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(exchangeInfo.assetValue);
			if (itemM != null)
			{
				this.exchangeDetailName = itemM.name;
				this.viewIconTexture.gameObject.SetActive(true);
				this.viewIcon.gameObject.SetActive(false);
				string largeImagePath = itemM.GetLargeImagePath();
				NGUIUtil.ChangeUITextureFromFile(this.viewIconTexture, largeImagePath, false);
			}
			goto IL_58D;
		}
		case MasterDataMng.AssetCategory.MEAT:
			text = "Common02_item_meat";
			this.exchangeDetailName = text2;
			goto IL_58D;
		case MasterDataMng.AssetCategory.SOUL:
		{
			this.exchangeDetailName = text2;
			GameWebAPI.RespDataMA_GetSoulM.SoulM soul = MasterDataMng.Instance().RespDataMA_SoulM.GetSoul(exchangeInfo.assetValue);
			value = soul.soulName;
			this.viewIconTexture.gameObject.SetActive(true);
			string evolveItemIconPathByID = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(exchangeInfo.assetValue);
			NGUIUtil.ChangeUITextureFromFile(this.viewIconTexture, evolveItemIconPathByID, false);
			this.viewIcon.gameObject.SetActive(false);
			goto IL_58D;
		}
		case MasterDataMng.AssetCategory.FACILITY_KEY:
		{
			this.viewIconTexture.gameObject.SetActive(true);
			this.viewIcon.gameObject.SetActive(false);
			FacilityConditionM[] facilityCondition = FarmDataManager.GetFacilityCondition(exchangeInfo.assetValue);
			FacilityConditionM facilityConditionM = facilityCondition.FirstOrDefault((FacilityConditionM x) => int.Parse(x.conditionType) == 1);
			FacilityM facilityMasterByReleaseId = FarmDataManager.GetFacilityMasterByReleaseId(int.Parse(facilityConditionM.releaseId));
			NGUIUtil.ChangeUITextureFromFile(this.viewIconTexture, facilityMasterByReleaseId.GetIconPath(), false);
			FacilityKeyM facilityKeyMaster = FarmDataManager.GetFacilityKeyMaster(exchangeInfo.assetValue);
			if (facilityKeyMaster != null)
			{
				this.exchangeDetailName = facilityKeyMaster.facilityKeyName;
			}
			goto IL_58D;
		}
		case MasterDataMng.AssetCategory.CHIP:
		{
			GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(exchangeInfo.assetValue);
			ChipDataMng.MakePrefabByChipData(chipMainData, this.viewIcon.gameObject, this.viewIcon.gameObject.transform.localPosition, this.viewIcon.gameObject.transform.localScale, null, 128, 128, false);
			this.exchangeDetailName = chipMainData.name;
			this.viewIcon.gameObject.SetActive(false);
			goto IL_58D;
		}
		case MasterDataMng.AssetCategory.DUNGEON_TICKET:
		{
			this.viewIconTexture.gameObject.SetActive(true);
			this.viewIcon.gameObject.SetActive(false);
			GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM dungeonTicketM = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster.dungeonTicketM.FirstOrDefault((GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM x) => exchangeInfo.assetValue == x.dungeonTicketId);
			if (dungeonTicketM != null)
			{
				global::Debug.Log(dungeonTicketM.img);
				NGUIUtil.ChangeUITextureFromFile(this.viewIconTexture, dungeonTicketM.img, false);
				this.exchangeDetailName = dungeonTicketM.name;
			}
			goto IL_58D;
		}
		}
		text = string.Empty;
		this.exchangeDetailName = StringMaster.GetString("Present-10");
		IL_58D:
		if (!string.IsNullOrEmpty(text) && assetCategory2 != MasterDataMng.AssetCategory.MONSTER)
		{
			this.viewIcon.spriteName = text;
		}
		if (assetCategory2 == MasterDataMng.AssetCategory.TIP)
		{
			this.exchangeDetailNum = StringFormat.Cluster(exchangeInfo.assetNum);
		}
		else
		{
			this.exchangeDetailNum = exchangeInfo.assetNum;
		}
		if (assetCategory2 != MasterDataMng.AssetCategory.MONSTER)
		{
			this.titleLabel.text = string.Format(StringMaster.GetString("SystemItemCount"), this.exchangeDetailName, this.exchangeDetailNum);
		}
		else
		{
			this.titleLabel.text = this.exchangeDetailName;
		}
		if (!string.IsNullOrEmpty(value))
		{
			this.exchangeDetailName = value;
		}
		this.exchangeConsumeNum = exchangeInfo.needNum;
		this.buildNumLabel.text = this.exchangeConsumeNum;
		this.ExchangeIconSet(exchangeInfo);
		this.ResetNum(exchangeInfo);
		this.SetButton(exchangeInfo);
		this.ShowGUI();
	}

	private void ExchangeIconSet(GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result.Detail exchangeInfo)
	{
		this.exchangeViewIcon.gameObject.SetActive(false);
		this.exchangeViewTexture.gameObject.SetActive(false);
		MasterDataMng.AssetCategory assetCategory = (MasterDataMng.AssetCategory)int.Parse(exchangeInfo.item.assetCategoryId);
		string text = string.Empty;
		switch (assetCategory)
		{
		case MasterDataMng.AssetCategory.MONSTER:
			text = string.Empty;
			goto IL_2BB;
		case MasterDataMng.AssetCategory.DIGI_STONE:
			text = "Common02_Icon_Stone";
			this.exchangeViewIcon.gameObject.SetActive(true);
			goto IL_2BB;
		case MasterDataMng.AssetCategory.LINK_POINT:
			text = "Common02_LB_Link";
			this.exchangeViewIcon.gameObject.SetActive(true);
			goto IL_2BB;
		case MasterDataMng.AssetCategory.TIP:
			text = "Common02_LB_Chip";
			this.exchangeViewIcon.gameObject.SetActive(true);
			goto IL_2BB;
		case MasterDataMng.AssetCategory.ITEM:
		{
			GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(exchangeInfo.item.assetValue);
			if (itemM != null)
			{
				this.exchangeViewTexture.gameObject.SetActive(true);
				string largeImagePath = itemM.GetLargeImagePath();
				NGUIUtil.ChangeUITextureFromFile(this.exchangeViewTexture, largeImagePath, false);
			}
			goto IL_2BB;
		}
		case MasterDataMng.AssetCategory.MEAT:
			text = "Common02_item_meat";
			this.exchangeViewIcon.gameObject.SetActive(true);
			goto IL_2BB;
		case MasterDataMng.AssetCategory.SOUL:
		{
			this.exchangeViewTexture.gameObject.SetActive(true);
			string evolveItemIconPathByID = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(exchangeInfo.item.assetValue);
			NGUIUtil.ChangeUITextureFromFile(this.exchangeViewTexture, evolveItemIconPathByID, false);
			goto IL_2BB;
		}
		case MasterDataMng.AssetCategory.FACILITY_KEY:
		{
			FacilityConditionM[] facilityCondition = FarmDataManager.GetFacilityCondition(exchangeInfo.item.assetValue);
			FacilityConditionM facilityConditionM = facilityCondition.FirstOrDefault((FacilityConditionM x) => int.Parse(x.conditionType) == 1);
			FacilityM facilityMasterByReleaseId = FarmDataManager.GetFacilityMasterByReleaseId(int.Parse(facilityConditionM.releaseId));
			this.exchangeViewTexture.gameObject.SetActive(true);
			NGUIUtil.ChangeUITextureFromFile(this.exchangeViewTexture, facilityMasterByReleaseId.GetIconPath(), false);
			goto IL_2BB;
		}
		case MasterDataMng.AssetCategory.CHIP:
			goto IL_2BB;
		case MasterDataMng.AssetCategory.DUNGEON_TICKET:
		{
			GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM dungeonTicketM = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster.dungeonTicketM.FirstOrDefault((GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM x) => exchangeInfo.item.assetValue == x.dungeonTicketId);
			if (dungeonTicketM != null)
			{
				global::Debug.Log(dungeonTicketM.img);
				this.exchangeViewTexture.gameObject.SetActive(true);
				NGUIUtil.ChangeUITextureFromFile(this.exchangeViewTexture, dungeonTicketM.img, false);
				this.exchangeDetailName = dungeonTicketM.name;
			}
			goto IL_2BB;
		}
		}
		text = string.Empty;
		this.exchangeDetailName = StringMaster.GetString("Present-10");
		IL_2BB:
		if (!string.IsNullOrEmpty(text) && assetCategory != MasterDataMng.AssetCategory.MONSTER)
		{
			this.exchangeViewIcon.spriteName = text;
		}
	}

	private IEnumerator TextureLoad(string path, Action<Texture2D> callback)
	{
		yield return TextureManager.instance.Load(path, callback, 30f, true);
		yield break;
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
	}

	public void ResetNum(GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result.Detail exchangeInfo)
	{
		int num = int.Parse(exchangeInfo.limitNum);
		int remainCount = exchangeInfo.remainCount;
		if (num == 0)
		{
			this.buildCountNumLabel.text = StringMaster.GetString("ExchangeLimitless");
		}
		else
		{
			this.buildCountNumLabel.text = string.Format(StringMaster.GetString("SystemFraction"), num - remainCount, num);
		}
		this.exchangeInfoData = exchangeInfo;
		this.exchangeItemData = exchangeInfo.item;
	}

	public void SetButton(GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result.Detail exchangeInfo)
	{
		int num = int.Parse(exchangeInfo.limitNum);
		int remainCount = exchangeInfo.remainCount;
		int count = exchangeInfo.item.count;
		int num2 = int.Parse(exchangeInfo.needNum);
		BoxCollider component = this.pushedButton.GetComponent<BoxCollider>();
		if (remainCount == 0 && num > 0)
		{
			this.pushedButtonGreenSprite.SetActive(false);
			this.pushedButtonGraySprite.SetActive(false);
			this.pushedButtonExchangeLabel.SetActive(false);
			this.pushedButtonLimitedLabel.SetActive(true);
			this.numUpButtonSprite.spriteName = "Common02_Meal_UP_G";
			this.numDownButtonSprite.spriteName = "Common02_Meal_Down_G";
			this.numDownButtonCollider.activeCollider = false;
			this.numUpButtonCollider.activeCollider = false;
			this.numCountLabel.text = "0";
			this.numCounter = 0;
			component.enabled = false;
		}
		else if (count < num2 || exchangeInfo.canExchange == 0)
		{
			this.pushedButtonGreenSprite.SetActive(false);
			this.pushedButtonGraySprite.SetActive(true);
			this.pushedButtonExchangeLabel.SetActive(true);
			this.pushedButtonLimitedLabel.SetActive(false);
			this.numUpButtonSprite.spriteName = "Common02_Meal_UP_G";
			this.numDownButtonSprite.spriteName = "Common02_Meal_Down_G";
			this.numDownButtonCollider.activeCollider = false;
			this.numUpButtonCollider.activeCollider = false;
			this.numCountLabel.text = "0";
			this.numCounter = 0;
			component.enabled = false;
		}
		else
		{
			this.pushedButtonGreenSprite.SetActive(true);
			this.pushedButtonGraySprite.SetActive(false);
			this.pushedButtonExchangeLabel.SetActive(true);
			this.pushedButtonLimitedLabel.SetActive(false);
			int maxExchangeNum = this.GetMaxExchangeNum();
			this.numCounter = 1;
			if (this.numCounter >= maxExchangeNum)
			{
				this.numUpButtonCollider.activeCollider = false;
				this.numUpButtonSprite.spriteName = "Common02_Meal_UP_G";
			}
			else
			{
				this.numUpButtonCollider.activeCollider = true;
				this.numUpButtonSprite.spriteName = "Common02_Meal_UP";
			}
			this.numDownButtonSprite.spriteName = "Common02_Meal_Down_G";
			this.numDownButtonCollider.activeCollider = false;
			this.numCountLabel.text = "1";
			component.enabled = true;
		}
	}

	public void ItemIconOnTap()
	{
		if (this.numUpButtonCollider.isTouching || this.numDownButtonCollider.isTouching)
		{
			return;
		}
		MasterDataMng.AssetCategory assetCategory = (MasterDataMng.AssetCategory)int.Parse(this.exchangeInfoData.assetCategoryId);
		GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory2 = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(this.exchangeInfoData.assetCategoryId);
		switch (assetCategory)
		{
		case MasterDataMng.AssetCategory.MONSTER:
			if (!string.IsNullOrEmpty(this.exchangeInfoData.monsterFixedValueId))
			{
				MonsterFixedM fixedValues = MonsterFixedData.GetMonsterFixedMaster(this.exchangeInfoData.monsterFixedValueId);
				if (fixedValues != null)
				{
					CMD_MonsterParamPop cmd_MonsterParamPop = GUIMain.ShowCommonDialog(null, "CMD_MonsterParamPop") as CMD_MonsterParamPop;
					MonsterData digimonData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.exchangeInfoData.assetValue);
					GameWebAPI.RespDataMA_GetSkillM.SkillM[] skillM = MasterDataMng.Instance().RespDataMA_SkillM.skillM;
					GameWebAPI.RespDataMA_GetSkillM.SkillM skillM2 = skillM.FirstOrDefault((GameWebAPI.RespDataMA_GetSkillM.SkillM x) => x.skillGroupId == digimonData.monsterM.skillGroupId && x.skillGroupSubId == fixedValues.defaultSkillGroupSubId);
					if (int.Parse(fixedValues.level) > int.Parse(digimonData.monsterM.maxLevel))
					{
						fixedValues.level = digimonData.monsterM.maxLevel;
					}
					int lvMAXExperienceInfo = DataMng.Instance().GetLvMAXExperienceInfo(int.Parse(fixedValues.level));
					DataMng.ExperienceInfo experienceInfo = DataMng.Instance().GetExperienceInfo(lvMAXExperienceInfo);
					digimonData.userMonster.luck = fixedValues.luck;
					digimonData.userMonster.friendship = "0";
					digimonData.userMonster.level = fixedValues.level;
					digimonData.userMonster.hpAbility = fixedValues.hpAbility;
					digimonData.userMonster.hpAbilityFlg = fixedValues.hpAbilityFlg.ToString();
					digimonData.userMonster.attackAbility = fixedValues.attackAbility;
					digimonData.userMonster.attackAbilityFlg = fixedValues.attackAbilityFlg.ToString();
					digimonData.userMonster.defenseAbility = fixedValues.defenseAbility;
					digimonData.userMonster.defenseAbilityFlg = fixedValues.defenseAbilityFlg.ToString();
					digimonData.userMonster.spAttackAbility = fixedValues.spAttackAbility;
					digimonData.userMonster.spAttackAbilityFlg = fixedValues.spAttackAbilityFlg.ToString();
					digimonData.userMonster.spDefenseAbility = fixedValues.spDefenseAbility;
					digimonData.userMonster.spDefenseAbilityFlg = fixedValues.spDefenseAbilityFlg.ToString();
					digimonData.userMonster.speedAbility = fixedValues.speedAbility;
					digimonData.userMonster.speedAbilityFlg = fixedValues.speedAbilityFlg.ToString();
					digimonData.userMonster.commonSkillId = fixedValues.commonSkillId;
					digimonData.userMonster.leaderSkillId = fixedValues.leaderSkillId;
					digimonData.userMonster.defaultSkillGroupSubId = fixedValues.defaultSkillGroupSubId;
					digimonData.userMonster.uniqueSkillId = skillM2.skillId;
					digimonData.InitSkillInfo();
					cmd_MonsterParamPop.MonsterDataSet(digimonData, experienceInfo, int.Parse(this.exchangeInfoData.maxExtraSlotNum), this.exchangeItemData.eventExchangeId);
				}
			}
			break;
		case MasterDataMng.AssetCategory.DIGI_STONE:
			CMD_QuestItemPOP.Create(assetCategory2);
			break;
		case MasterDataMng.AssetCategory.LINK_POINT:
			CMD_QuestItemPOP.Create(assetCategory2);
			break;
		case MasterDataMng.AssetCategory.TIP:
			CMD_QuestItemPOP.Create(assetCategory2);
			break;
		case MasterDataMng.AssetCategory.ITEM:
		{
			GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(this.exchangeInfoData.assetValue);
			CMD_QuestItemPOP.Create(itemM);
			break;
		}
		case MasterDataMng.AssetCategory.SOUL:
		{
			GameWebAPI.RespDataMA_GetSoulM.SoulM soul = MasterDataMng.Instance().RespDataMA_SoulM.GetSoul(this.exchangeInfoData.assetValue);
			CMD_QuestItemPOP.Create(soul);
			break;
		}
		case MasterDataMng.AssetCategory.FACILITY_KEY:
		{
			FacilityConditionM[] facilityCondition = FarmDataManager.GetFacilityCondition(this.exchangeInfoData.assetValue);
			FacilityConditionM facilityConditionM = facilityCondition.FirstOrDefault((FacilityConditionM x) => int.Parse(x.conditionType) == 1);
			FacilityM facilityMasterByReleaseId = FarmDataManager.GetFacilityMasterByReleaseId(int.Parse(facilityConditionM.releaseId));
			CMD_QuestItemPOP.Create(facilityConditionM, this.exchangeInfoData.assetValue, facilityMasterByReleaseId);
			break;
		}
		case MasterDataMng.AssetCategory.CHIP:
		{
			GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(this.exchangeInfoData.assetValue);
			CMD_QuestItemPOP.Create(chipMainData);
			break;
		}
		case MasterDataMng.AssetCategory.DUNGEON_TICKET:
		{
			string ticketValue = this.exchangeInfoData.assetValue;
			GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM data = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster.dungeonTicketM.FirstOrDefault((GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM x) => x.dungeonTicketId == ticketValue);
			CMD_QuestItemPOP.Create(data);
			break;
		}
		}
	}
}
