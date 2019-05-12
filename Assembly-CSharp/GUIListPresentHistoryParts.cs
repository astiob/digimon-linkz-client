using Master;
using System;
using UnityEngine;

public sealed class GUIListPresentHistoryParts : GUIListPartBS
{
	[SerializeField]
	private UILabel lbDescription;

	[SerializeField]
	private UILabel lbLimitTime;

	private GameWebAPI.RespDataPR_PrizeReceiveHistory.PrizeReceiveHistory prizeData;

	public GameWebAPI.RespDataPR_PrizeReceiveHistory.PrizeReceiveHistory Data
	{
		get
		{
			return this.prizeData;
		}
		set
		{
			this.prizeData = value;
			this.ShowGUI();
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public override void ShowGUI()
	{
		this.SetCommonUI();
		base.ShowGUI();
	}

	private void SetCommonUI()
	{
		this.lbDescription.text = this.prizeData.message;
		DateTime dateTime;
		if (DateTime.TryParse(this.prizeData.updateTime, out dateTime))
		{
			string str = dateTime.ToString("yyyy/MM/dd");
			this.lbLimitTime.text = StringMaster.GetString("OtherHistory-03") + str;
		}
		else
		{
			this.lbLimitTime.text = StringMaster.GetString("OtherHistory-03") + this.prizeData.updateTime.ToString();
		}
		PresentBoxItem component = base.gameObject.GetComponent<PresentBoxItem>();
		component.SetItem(this.prizeData.assetCategoryId, this.prizeData.assetValue, this.prizeData.assetNum, false, null);
	}
}
