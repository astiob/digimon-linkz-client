using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CampaignLabelArea : PartsMenuNotfIcon
{
	[SerializeField]
	private UILabel lbContent;

	[SerializeField]
	private UISprite spBaloon;

	private List<QuestData.WorldStageData> stageDataList;

	public List<QuestData.WorldStageData> StageDataList
	{
		set
		{
			this.stageDataList = value;
		}
	}

	protected override void Start()
	{
		this.lbContent.text = string.Empty;
		if (this.spBaloon != null)
		{
			this.spBaloon.gameObject.SetActive(false);
		}
		base.Start();
	}

	protected override void SetCampaignData(List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> infos)
	{
		if (infos.Count > 1)
		{
			this.lbContent.text = base.GetMultipleHoldingCampaignDescription();
			if (this.spBaloon != null)
			{
				this.spBaloon.gameObject.SetActive(true);
			}
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
			if (this.spBaloon != null)
			{
				this.spBaloon.gameObject.SetActive(true);
			}
		}
	}

	protected override List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> GetUnderwayCampaignList(GameWebAPI.RespDataCP_Campaign campaign)
	{
		List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> list = new List<GameWebAPI.RespDataCP_Campaign.CampaignInfo>();
		DateTime now = ServerDateTime.Now;
		for (int i = 0; i < campaign.campaignInfo.Length; i++)
		{
			bool flag = false;
			if (this.stageDataList.Count > 0)
			{
				int num = int.Parse(this.stageDataList[0].worldStageM.worldStageId);
				int num2 = int.Parse(this.stageDataList[this.stageDataList.Count - 1].worldStageM.worldStageId);
				int num3 = int.Parse(campaign.campaignInfo[i].targetValue);
				if (num <= num3 && num3 <= num2)
				{
					flag = true;
				}
			}
			if (base.ExistCampaign(campaign.campaignInfo[i].GetCmpIdByEnum()) && flag && campaign.campaignInfo[i].IsUnderway(now))
			{
				list.Add(campaign.campaignInfo[i]);
			}
		}
		return list;
	}
}
