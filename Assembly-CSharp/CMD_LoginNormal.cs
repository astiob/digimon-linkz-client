using Evolution;
using FarmData;
using Master;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CMD_LoginNormal : CMD_LoginBase
{
	[SerializeField]
	private UILabel loginBonusTitle;

	[SerializeField]
	private UILabel todayTitle;

	[SerializeField]
	private UILabel tomorrowTitle;

	public string GET_ICON_PATH = "UISPR_ITEM_GET_";

	public string NEXT_ICON_PATH = "UISPR_ITEM_NEXT_";

	public string GET_SOUL_PATH = "UISPR_SOUL_GET_";

	public string NEXT_SOUL_PATH = "UISPR_SOUL_NEXT_";

	private List<UISprite> getIcons = new List<UISprite>();

	private List<UITexture> getSouls = new List<UITexture>();

	private List<UISprite> nextIcons = new List<UISprite>();

	private List<UITexture> nextSouls = new List<UITexture>();

	private List<int> textureCategoryList = new List<int>
	{
		14,
		6,
		16,
		18
	};

	private GUIMonsterIcon monsterIcon;

	protected override void Awake()
	{
		base.Awake();
		if (this.loginBonus == null)
		{
			return;
		}
		this.SetIcons(this.GET_ICON_PATH, this.getIcons, this.loginBonus.rewardList);
		this.SetIcons(this.NEXT_ICON_PATH, this.nextIcons, this.loginBonus.nextRewardList);
		this.SetSouls(this.GET_SOUL_PATH, this.getSouls, this.loginBonus.rewardList);
		this.SetSouls(this.NEXT_SOUL_PATH, this.nextSouls, this.loginBonus.nextRewardList);
	}

	protected new void Start()
	{
		this.loginBonusTitle.text = StringMaster.GetString("LoginBonus");
		this.todayTitle.text = StringMaster.GetString("NormalLogin_txt");
		this.tomorrowTitle.text = StringMaster.GetString("NormalLogin_txt2");
		base.Start();
	}

	protected void SetIcons(string path, List<UISprite> list, GameWebAPI.RespDataCM_LoginBonus.LoginReward[] rewardList)
	{
		int num = 1;
		string name = path + num;
		if (base.transform.Find(name) == null)
		{
			return;
		}
		UISprite component = base.transform.Find(name).GetComponent<UISprite>();
		while (component != null)
		{
			list.Add(component);
			if (rewardList.Length < num)
			{
				component.gameObject.SetActive(false);
			}
			else
			{
				int num2 = num - 1;
				string rewardIcon = this.GetRewardIcon(rewardList[num2]);
				string assetCategoryId = rewardList[num2].assetCategoryId;
				string assetValue = rewardList[num2].assetValue;
				if (string.IsNullOrEmpty(assetCategoryId) || string.IsNullOrEmpty(rewardIcon) || this.textureCategoryList.Contains(assetCategoryId.ToInt32()))
				{
					component.gameObject.SetActive(false);
				}
				else if (assetCategoryId.ToInt32() == 17)
				{
					GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(assetValue);
					ChipDataMng.MakePrefabByChipData(chipMainData, component.gameObject, component.transform.localPosition, component.transform.localScale, null, -1, -1, true);
				}
				else if (assetCategoryId.ToInt32() == 1)
				{
					this.monsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(MonsterDataMng.Instance().CreateMonsterDataByMID(assetValue), Vector3.one, Vector3.zero, component.transform, true, false);
					this.monsterIcon.ResizeIcon(component.width, component.height);
					if (null != this.monsterIcon)
					{
						DepthController depthController = this.monsterIcon.GetDepthController();
						if (null != depthController)
						{
							depthController.AddWidgetDepth(this.monsterIcon.transform, component.depth + 1);
						}
					}
				}
				else
				{
					component.spriteName = rewardIcon;
				}
			}
			num++;
			name = path + num;
			if (!(base.transform.Find(name) != null))
			{
				break;
			}
			component = base.transform.Find(name).GetComponent<UISprite>();
		}
	}

	protected void SetSouls(string path, List<UITexture> list, GameWebAPI.RespDataCM_LoginBonus.LoginReward[] rewardList)
	{
		int num = 1;
		string name = path + num;
		if (base.transform.Find(name) == null)
		{
			return;
		}
		UITexture component = base.transform.Find(name).GetComponent<UITexture>();
		while (component != null)
		{
			list.Add(component);
			if (rewardList.Length < num)
			{
				component.gameObject.SetActive(false);
			}
			else
			{
				int num2 = num - 1;
				string rewardIcon = this.GetRewardIcon(rewardList[num2]);
				string assetCategoryId = rewardList[num2].assetCategoryId;
				if (string.IsNullOrEmpty(assetCategoryId) || string.IsNullOrEmpty(rewardIcon) || !this.textureCategoryList.Contains(assetCategoryId.ToInt32()))
				{
					component.gameObject.SetActive(false);
				}
				else
				{
					NGUIUtil.ChangeUITextureFromFile(component, rewardIcon, false);
				}
			}
			num++;
			name = path + num;
			if (!(base.transform.Find(name) != null))
			{
				break;
			}
			component = base.transform.Find(name).GetComponent<UITexture>();
		}
	}

	protected string GetRewardIcon(GameWebAPI.RespDataCM_LoginBonus.LoginReward lr)
	{
		int num = int.Parse(lr.assetCategoryId);
		string result = string.Empty;
		switch (num)
		{
		case 1:
			result = "monster_dummy";
			break;
		case 2:
			result = "Common02_LB_Stone";
			break;
		case 3:
			result = "Common02_LB_Link";
			break;
		case 4:
			result = "Common02_LB_Chip";
			break;
		case 6:
		{
			GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(lr.assetValue);
			if (itemM != null)
			{
				result = itemM.GetLargeImagePath();
			}
			break;
		}
		case 13:
			result = "Common02_item_meat";
			break;
		case 14:
			result = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(lr.assetValue);
			break;
		case 16:
		{
			FacilityConditionM[] facilityCondition = FarmDataManager.GetFacilityCondition(lr.assetValue);
			FacilityConditionM facilityConditionM = facilityCondition.FirstOrDefault((FacilityConditionM x) => int.Parse(x.conditionType) == 1);
			FacilityM facilityMasterByReleaseId = FarmDataManager.GetFacilityMasterByReleaseId(int.Parse(facilityConditionM.releaseId));
			result = facilityMasterByReleaseId.GetIconPath();
			break;
		}
		case 17:
			result = "chip_dummy";
			break;
		case 18:
		{
			GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM dungeonTicketM = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster.dungeonTicketM.FirstOrDefault((GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM x) => lr.assetValue == x.dungeonTicketId);
			if (dungeonTicketM != null)
			{
				result = dungeonTicketM.img;
			}
			break;
		}
		}
		return result;
	}

	protected override GameWebAPI.RespDataCM_LoginBonus.LoginBonus GetLoginBonus()
	{
		int showLoginBonusNumN = DataMng.Instance().ShowLoginBonusNumN;
		int num = DataMng.Instance().RespDataCM_LoginBonus.loginBonus.normal.Length;
		if (num > 0)
		{
			return DataMng.Instance().RespDataCM_LoginBonus.loginBonus.normal[showLoginBonusNumN];
		}
		return null;
	}

	protected override void GetStamps()
	{
		int num = 0;
		while (this.IsAnimationStamp(num))
		{
			Transform transform = base.transform.Find(this.TODAY_STAMP_PREF_NAME + num);
			if (transform != null)
			{
				this.stamps.Add(transform);
			}
			num++;
		}
	}

	protected override void InitStamps(GameObject go, int index)
	{
		go.SetActive(true);
	}

	protected override bool IsAnimationStamp(int listIndex)
	{
		return listIndex <= this.loginBonus.rewardList.Length;
	}
}
