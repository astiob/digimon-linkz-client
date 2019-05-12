using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CampaignLabelQuest : PartsMenuNotfIcon
{
	[SerializeField]
	private UILabel lbContent;

	private QuestData.WorldStageData data;

	[SerializeField]
	private string _areaId;

	public string AreaId
	{
		set
		{
			this._areaId = value;
		}
	}

	protected override void Start()
	{
		this.lbContent.text = string.Empty;
		base.Start();
	}

	protected override void SetCampaignData(List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> infos)
	{
		if (infos.Count > 1)
		{
			this.lbContent.text = base.GetMultipleHoldingCampaignDescription();
		}
		else if (infos.Count == 1)
		{
			GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo = infos[0];
			GameWebAPI.RespDataCP_Campaign.CampaignType cmpIdByEnum = campaignInfo.GetCmpIdByEnum();
			float num = float.Parse(campaignInfo.rate);
			if (cmpIdByEnum == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDown || cmpIdByEnum == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDownMul)
			{
				num = Mathf.Ceil(1f / num);
			}
			this.lbContent.text = base.GetDescription(cmpIdByEnum, num);
		}
	}

	protected override List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> GetUnderwayCampaignList(GameWebAPI.RespDataCP_Campaign campaign)
	{
		List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> list = new List<GameWebAPI.RespDataCP_Campaign.CampaignInfo>();
		DateTime now = ServerDateTime.Now;
		for (int i = 0; i < campaign.campaignInfo.Length; i++)
		{
			if (base.ExistCampaign(campaign.campaignInfo[i].GetCmpIdByEnum()) && campaign.campaignInfo[i].targetValue == this._areaId && campaign.campaignInfo[i].IsUnderway(now))
			{
				list.Add(campaign.campaignInfo[i]);
			}
		}
		return list;
	}
}
