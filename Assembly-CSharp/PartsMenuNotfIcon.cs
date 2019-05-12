using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PartsMenuNotfIcon : MonoBehaviour
{
	public GameWebAPI.RespDataCP_Campaign.CampaignType[] refCampaignType;

	[SerializeField]
	private bool removeLineFeedCode;

	[SerializeField]
	private bool useLongDescription;

	protected virtual void Start()
	{
		DataMng dataMng = DataMng.Instance();
		dataMng.OnCampaignUpdate = (Action<GameWebAPI.RespDataCP_Campaign, bool>)Delegate.Combine(dataMng.OnCampaignUpdate, new Action<GameWebAPI.RespDataCP_Campaign, bool>(this.OnCampaignUpdate));
		this.Refresh();
	}

	private void OnDestroy()
	{
		DataMng dataMng = DataMng.Instance();
		dataMng.OnCampaignUpdate = (Action<GameWebAPI.RespDataCP_Campaign, bool>)Delegate.Remove(dataMng.OnCampaignUpdate, new Action<GameWebAPI.RespDataCP_Campaign, bool>(this.OnCampaignUpdate));
	}

	protected bool ExistCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType type)
	{
		for (int i = 0; i < this.refCampaignType.Length; i++)
		{
			if (this.refCampaignType[i] == type)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual void SetCampaignData(List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> infos)
	{
		base.gameObject.SetActive(true);
	}

	protected virtual List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> GetUnderwayCampaignList(GameWebAPI.RespDataCP_Campaign campaign)
	{
		List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> list = new List<GameWebAPI.RespDataCP_Campaign.CampaignInfo>();
		DateTime now = ServerDateTime.Now;
		for (int i = 0; i < campaign.campaignInfo.Length; i++)
		{
			if (this.ExistCampaign(campaign.campaignInfo[i].GetCmpIdByEnum()) && campaign.campaignInfo[i].IsUnderway(now))
			{
				list.Add(campaign.campaignInfo[i]);
			}
		}
		return list;
	}

	private void OnCampaignUpdate(GameWebAPI.RespDataCP_Campaign cmpList, bool forceHide)
	{
		if (cmpList == null || forceHide)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> underwayCampaignList = this.GetUnderwayCampaignList(cmpList);
			if (underwayCampaignList.Count > 0)
			{
				this.SetCampaignData(underwayCampaignList);
				base.gameObject.SetActive(true);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	public void Refresh()
	{
		this.OnCampaignUpdate(DataMng.Instance().RespDataCP_Campaign, DataMng.Instance().CampaignForceHide);
	}

	protected string GetDescription(GameWebAPI.RespDataCP_Campaign.CampaignType cpmType, float rate)
	{
		return CampaignUtil.GetDescription(cpmType, rate, this.useLongDescription);
	}

	protected string GetMultipleHoldingCampaignDescription()
	{
		return StringMaster.GetString("Campaign");
	}
}
