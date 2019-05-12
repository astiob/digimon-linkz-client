using FarmData;
using Master;
using System;
using UnityEngine;

public class CMD_FacilityInfoNoneEffect : CMD
{
	[SerializeField]
	private UILabel facilityNameTitle;

	[SerializeField]
	private UILabel facilityName;

	[SerializeField]
	private UILabel detailTitle;

	[SerializeField]
	private UILabel detail;

	[SerializeField]
	private UITexture thumbnail;

	[SerializeField]
	private UILabel closeButtonLabel;

	private void Start()
	{
		this.facilityNameTitle.text = StringMaster.GetString("FacilityInfoTitle");
		this.detailTitle.text = StringMaster.GetString("FacilityInfoDescription");
		this.closeButtonLabel.text = StringMaster.GetString("SystemButtonClose");
	}

	public void SetFacilityInfo(int facilityId)
	{
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(facilityId);
		this.detail.text = facilityMaster.description;
		this.facilityName.text = facilityMaster.facilityName;
		NGUIUtil.ChangeUITextureFromFile(this.thumbnail, facilityMaster.GetIconPath(), false);
	}

	private void OnPushedCloseButton()
	{
		this.ClosePanel(true);
	}
}
