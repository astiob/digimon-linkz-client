using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CampaignIcon : PartsMenuNotfIcon
{
	[SerializeField]
	private UILabel lbContent;

	[Header("キャンペーン中を必ず表示")]
	[SerializeField]
	private bool forceShowInCampaign;

	private const int BASE_PLATE_SIZE = 66;

	private const int DELTA_PLATE_SIZE = 14;

	protected override void Start()
	{
		this.lbContent.text = string.Empty;
		base.Start();
	}

	protected override void SetCampaignData(List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> infos)
	{
		if (infos.Count > 1 || this.forceShowInCampaign)
		{
			this.lbContent.text = base.GetMultipleHoldingCampaignDescription();
		}
		else if (infos.Count == 1)
		{
			GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo = infos[0];
			GameWebAPI.RespDataCP_Campaign.CampaignType cmpIdByEnum = campaignInfo.GetCmpIdByEnum();
			float num = float.Parse(campaignInfo.rate);
			if (cmpIdByEnum == GameWebAPI.RespDataCP_Campaign.CampaignType.TrainCostDown)
			{
				num = (1f - num) * 100f;
			}
			if (cmpIdByEnum == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDown || cmpIdByEnum == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDownMul)
			{
				num = 1f / num;
			}
			string text = base.GetDescription(cmpIdByEnum, num);
			text = ((!this.IsQuestCampaign(cmpIdByEnum)) ? text : text.Replace("\n", string.Empty));
			this.lbContent.text = text;
			UISprite componentInChildren = base.GetComponentInChildren<UISprite>();
			if (componentInChildren != null)
			{
				int num2 = this.lbContent.text.Count((char c) => c == '\n');
				componentInChildren.height = 66 + 14 * num2;
			}
		}
	}

	private bool IsQuestCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType cpmType)
	{
		return cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestCipUp || cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestExpUp || cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestMatUp || cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestRrDrpUp || cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestFriUp || cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDown || cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestExpUpMul || cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestCipUpMul || cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestMatUpMul || cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestRrDrpUpMul || cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestFriUpMul || cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDownMul;
	}
}
