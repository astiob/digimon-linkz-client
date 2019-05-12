using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CampaignFacilityIcon : MonoBehaviour
{
	[SerializeField]
	private UISprite popTxtImg;

	[NonSerialized]
	public FarmObject farmObject;

	private Camera farmCamera;

	public static CampaignFacilityIcon Create(GameWebAPI.RespDataCP_Campaign.CampaignType cpmType, GameObject parent)
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null == instance)
		{
			return null;
		}
		if (DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.MedalTakeOverUp) == null)
		{
			return null;
		}
		if (cpmType != GameWebAPI.RespDataCP_Campaign.CampaignType.MedalTakeOverUp)
		{
			return null;
		}
		int num = 5;
		int facilityCount = instance.Scenery.GetFacilityCount(num);
		if (facilityCount <= 0)
		{
			return null;
		}
		GameObject gameObject = GUIManager.LoadCommonGUI("Farm/CampaignBalloon", parent);
		CampaignFacilityIcon component = gameObject.GetComponent<CampaignFacilityIcon>();
		component.setLocalizedPopTxtImg();
		gameObject.SetActive(false);
		List<FarmObject> farmObjects = instance.Scenery.farmObjects;
		for (int i = 0; i < farmObjects.Count; i++)
		{
			if (farmObjects[i].facilityID == num)
			{
				gameObject.SetActive(true);
				component.farmObject = farmObjects[i];
				break;
			}
		}
		return component;
	}

	private void Start()
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null == instance)
		{
			return;
		}
		this.farmCamera = instance.Camera;
		this.Update();
	}

	private void Update()
	{
		if (this.farmCamera != null && this.farmObject != null)
		{
			this.UpdateIconPos();
		}
	}

	private void UpdateIconPos()
	{
		Vector3 position = this.farmCamera.WorldToScreenPoint(this.farmObject.transform.position);
		Camera gUICamera = GUIManager.gUICamera;
		Vector3 vector = gUICamera.ScreenToWorldPoint(position);
		Vector3 position2 = base.transform.position;
		position2.x = vector.x;
		position2.y = vector.y;
		base.transform.position = position2;
	}

	public void Close()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		this.farmObject = null;
		this.farmCamera = null;
	}

	public void setLocalizedPopTxtImg()
	{
		if (this.popTxtImg != null)
		{
			this.popTxtImg.spriteName = string.Format("{0}_{1}", this.popTxtImg.spriteName, CountrySetting.GetCountryPrefix(CountrySetting.CountryCode.EN));
		}
	}
}
