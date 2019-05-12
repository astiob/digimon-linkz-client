using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUISelectPanelEvolutionItemList : GUISelectPanelBSPartsUD
{
	private const int SOUL_RARE_ULTIMATE = 5;

	private List<GameWebAPI.UserSoulData> userSoulList;

	private Dictionary<GameWebAPI.UserSoulData, string> pluginDataList;

	private int PARTS_CT_MN;

	protected override void Awake()
	{
		base.Awake();
		this.userSoulList = new List<GameWebAPI.UserSoulData>();
	}

	public void AllBuildPlugin(GameWebAPI.UserSoulData[] userSoulData, GameWebAPI.RespDataMA_GetSoulM soulM)
	{
		this.PARTS_CT_MN = 7;
		base.InitBuild();
		base.initLocation = true;
		if (base.selectCollider != null)
		{
			this.pluginDataList = new Dictionary<GameWebAPI.UserSoulData, string>();
			foreach (GameWebAPI.RespDataMA_GetSoulM.SoulM soul in soulM.soulM)
			{
				if (soul.rare.ToInt32() < 5)
				{
					GameWebAPI.UserSoulData userSoulData2 = userSoulData.Where((GameWebAPI.UserSoulData userSoul) => userSoul.soulId == soul.soulId).Max<GameWebAPI.UserSoulData>();
					if (userSoulData2 != null)
					{
						this.pluginDataList.Add(userSoulData2, soul.rare);
					}
					else
					{
						GameWebAPI.UserSoulData userSoulData3 = new GameWebAPI.UserSoulData();
						userSoulData3.soulId = soul.soulId;
						userSoulData3.num = "0";
						this.pluginDataList.Add(userSoulData3, soul.rare);
					}
				}
			}
			IOrderedEnumerable<KeyValuePair<GameWebAPI.UserSoulData, string>> orderedEnumerable = this.pluginDataList.OrderByDescending((KeyValuePair<GameWebAPI.UserSoulData, string> y) => y.Value);
			foreach (KeyValuePair<GameWebAPI.UserSoulData, string> keyValuePair in orderedEnumerable)
			{
				this.userSoulList.Add(keyValuePair.Key);
			}
			this.BaseBuild(GUISelectPanelEvolutionItemList.LIST_TYPE.PLUGIN);
		}
	}

	public void AllBuildSoul(GameWebAPI.UserSoulData[] userSoulData)
	{
		this.PARTS_CT_MN = 8;
		base.InitBuild();
		base.initLocation = true;
		if (base.selectCollider != null)
		{
			foreach (GameWebAPI.UserSoulData userSoulData2 in userSoulData)
			{
				GameWebAPI.RespDataMA_GetSoulM.SoulM soul = MasterDataMng.Instance().RespDataMA_SoulM.GetSoul(userSoulData2.soulId);
				if (soul.rare.ToInt32() >= 5)
				{
					this.userSoulList.Add(userSoulData2);
				}
			}
			if (this.userSoulList.Count == 0)
			{
				CMD_EvolutionItemList.instance.DispNoEvolutionItemMessage();
			}
			this.BaseBuild(GUISelectPanelEvolutionItemList.LIST_TYPE.SOUL);
		}
	}

	private void BaseBuild(GUISelectPanelEvolutionItemList.LIST_TYPE ListType)
	{
		this.partsCount = this.userSoulList.Count;
		int num = this.partsCount / this.PARTS_CT_MN;
		if (this.partsCount % this.PARTS_CT_MN > 0)
		{
			num++;
		}
		if (ListType == GUISelectPanelEvolutionItemList.LIST_TYPE.PLUGIN)
		{
			this.horizontalMargin = 40f;
		}
		GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(this.PARTS_CT_MN, num, 1f, 1f);
		float num2 = panelBuildData.startY;
		float num3 = panelBuildData.startX;
		if (ListType == GUISelectPanelEvolutionItemList.LIST_TYPE.SOUL)
		{
			float num4 = -20f;
			panelBuildData.startX += num4;
			num3 += num4;
		}
		base.height = panelBuildData.lenH;
		int num5 = 1;
		foreach (GameWebAPI.UserSoulData soulData in this.userSoulList)
		{
			GameObject gameObject = base.AddBuildPart();
			GUIListEvolutionItemParts component = gameObject.GetComponent<GUIListEvolutionItemParts>();
			component.SoulData = soulData;
			component.SetOriginalPos(new Vector3(num3, num2, -5f));
			gameObject.SetActive(true);
			if (num5 % this.PARTS_CT_MN == 0)
			{
				num2 -= panelBuildData.pitchH;
				num3 = panelBuildData.startX;
			}
			else
			{
				num3 += panelBuildData.pitchW;
			}
			num5++;
		}
		base.InitMinMaxLocation(-1, 0f);
	}

	public enum LIST_TYPE
	{
		PLUGIN,
		SOUL
	}
}
