using System;
using System.Collections.Generic;
using UnityEngine;

public class TipsQuestSelect : MonoBehaviour
{
	[SerializeField]
	private UITexture txNavi;

	[SerializeField]
	private UILabel lbTips;

	private GameWebAPI.RespDataMA_GetWorldAreaM data;

	private List<CMD_Tips.TipsM.Tips> tipsList;

	private CMD_Tips.TipsM.Tips tips;

	private static int dispPlace = -1;

	private static int dispIDX;

	private void Awake()
	{
		this.data = MasterDataMng.Instance().RespDataMA_WorldAreaM;
		int displayPlace = this.GetDisplayPlace(this.data);
		if (displayPlace != TipsQuestSelect.dispPlace)
		{
			TipsQuestSelect.dispIDX = 0;
		}
		TipsQuestSelect.dispPlace = displayPlace;
		this.tipsList = CMD_Tips.GetDisplayTipsData((CMD_Tips.DISPLAY_PLACE)TipsQuestSelect.dispPlace);
		if (this.tipsList.Count > 0)
		{
			if (TipsQuestSelect.dispIDX >= this.tipsList.Count)
			{
				TipsQuestSelect.dispIDX = 0;
			}
			this.tips = this.tipsList[TipsQuestSelect.dispIDX];
			TipsQuestSelect.dispIDX++;
		}
		else
		{
			this.tips = null;
			TipsQuestSelect.dispIDX = 0;
		}
	}

	private void Start()
	{
		if (this.tips != null)
		{
			NGUIUtil.ChangeUITextureFromFile(this.txNavi, "Navi/" + this.tips.img + this.tips.icon, true);
			this.lbTips.text = this.tips.message;
		}
		else
		{
			this.lbTips.text = string.Empty;
		}
	}

	private int GetDisplayPlace(GameWebAPI.RespDataMA_GetWorldAreaM data)
	{
		for (int i = 0; i < data.worldAreaM.Length; i++)
		{
			if (int.Parse(data.worldAreaM[i].worldAreaId) >= 6)
			{
				return 18;
			}
		}
		for (int i = 0; i < data.worldAreaM.Length; i++)
		{
			if (int.Parse(data.worldAreaM[i].worldAreaId) == 3)
			{
				return 17;
			}
		}
		return 16;
	}

	private void OnDestroy()
	{
	}
}
