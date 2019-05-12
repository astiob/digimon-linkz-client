using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CampaignLabel : PartsMenuNotfIcon
{
	private const int BASE_PLATE_SIZE = 66;

	private const int DELTA_PLATE_SIZE = 14;

	[SerializeField]
	private UILabel lbContent;

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
			if (cmpIdByEnum == GameWebAPI.RespDataCP_Campaign.CampaignType.TrainCostDown)
			{
				num = (1f - num) * 100f;
			}
			string text = base.GetDescription(cmpIdByEnum, num);
			text = text.Replace("\n", string.Empty);
			this.lbContent.text = text;
			UISprite componentInChildren = base.GetComponentInChildren<UISprite>();
			if (componentInChildren != null)
			{
				int num2 = this.lbContent.text.Count((char c) => c == '\n');
				componentInChildren.height = 66 + 14 * num2;
			}
		}
	}
}
