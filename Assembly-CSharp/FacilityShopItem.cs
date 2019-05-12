using FarmData;
using Master;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FacilityShopItem : GUIListPartBS
{
	[SerializeField]
	private GameObject lockDetail;

	[SerializeField]
	private UILabel conditionTitle;

	[SerializeField]
	private UILabel[] conditionTexts;

	[SerializeField]
	private GameObject conditionLine;

	[SerializeField]
	private GameObject detail;

	[SerializeField]
	private UITexture facilityIcon;

	[SerializeField]
	private UILabel facilityName;

	[SerializeField]
	private UILabel eventFacilityName;

	[SerializeField]
	private UILabel buildCountTitle;

	[SerializeField]
	private UILabel buildCount;

	[SerializeField]
	private UILabel eventBuildCountTitle;

	[SerializeField]
	private UILabel eventBuildCount;

	[SerializeField]
	private UILabel buildTimeTitle;

	[SerializeField]
	private UILabel buildTime;

	[SerializeField]
	private UILabel eventBuildTimeTitle;

	[SerializeField]
	private UILabel eventBuildTime;

	[SerializeField]
	private UISprite buyTypeTip;

	[SerializeField]
	private UISprite buyTypeStone;

	[SerializeField]
	private UILabel price;

	[SerializeField]
	private UILabel limitLabel;

	private Action<FacilityShopItem> onPushedBuyButton;

	[SerializeField]
	private GUICollider buyButton;

	[SerializeField]
	private GameObject enableButtonImage;

	[SerializeField]
	private GameObject disableButtonImage;

	[NonSerialized]
	public int facilityID;

	[SerializeField]
	private Color nonClearColor;

	[SerializeField]
	private Color notBuyPriceColor;

	[SerializeField]
	private Color limitLabelColor;

	[SerializeField]
	private GameObject newIcon;

	[SerializeField]
	private GameObject normalPartsRoot;

	[SerializeField]
	private GameObject eventPartsRoot;

	public void SetDetail(FacilityM facilityMaster, FacilityConditionM[] facilityConditionMaster, bool[] isClearConditions, Action<FacilityShopItem> touchEvent)
	{
		this.facilityID = int.Parse(facilityMaster.facilityId);
		this.onPushedBuyButton = touchEvent;
		FacilityM facilityMaster2 = FarmDataManager.GetFacilityMaster(this.facilityID);
		NGUIUtil.ChangeUITextureFromFile(this.facilityIcon, facilityMaster2.GetIconPath(), false);
		if (isClearConditions.Any((bool x) => !x))
		{
			this.DetailSetActive(false);
			this.lockDetail.SetActive(true);
			this.conditionTitle.text = string.Format(StringMaster.GetString("FacilityShopConditionTitle"), facilityMaster.facilityName);
			this.buildCountTitle.text = StringMaster.GetString("FacilityShopBuildNum");
			this.buildTimeTitle.text = StringMaster.GetString("FarmEditStockTime");
			this.eventBuildCountTitle.text = StringMaster.GetString("FacilityShopBuildNum");
			this.eventBuildTimeTitle.text = StringMaster.GetString("FarmEditStockTime");
			for (int i = 0; i < facilityConditionMaster.Length; i++)
			{
				if (!(null == this.conditionTexts[i]))
				{
					FacilityConditionM facilityConditionM = facilityConditionMaster[i];
					if (string.IsNullOrEmpty(facilityConditionM.description))
					{
						this.conditionTexts[i].text = StringMaster.GetString("FacilityShopConditionUnknow");
					}
					else
					{
						this.conditionTexts[i].text = facilityConditionM.description;
					}
					if (!isClearConditions[i])
					{
						this.conditionTexts[i].color = this.nonClearColor;
					}
				}
			}
			for (int j = facilityConditionMaster.Length; j < this.conditionTexts.Length; j++)
			{
				if (!(null == this.conditionTexts[j]))
				{
					this.conditionTexts[j].gameObject.SetActive(false);
				}
			}
			if (facilityConditionMaster.Length == 1 && null != this.conditionLine)
			{
				this.conditionLine.SetActive(false);
			}
			if (facilityConditionMaster.Length > 0 && facilityConditionMaster[0].conditionType == 5.ToString())
			{
				this.ChangeEventItemMode();
			}
		}
		else
		{
			this.DetailSetActive(true);
			this.lockDetail.SetActive(false);
			UILabel uilabel = this.facilityName;
			string text = facilityMaster.facilityName;
			this.eventFacilityName.text = text;
			uilabel.text = text;
			int nowCount = this.GetNowCount();
			int num = int.Parse(facilityMaster.maxNum);
			if (nowCount != -1)
			{
				UILabel uilabel2 = this.buildCount;
				text = string.Format(StringMaster.GetString("SystemFraction"), nowCount, facilityMaster.maxNum);
				this.eventBuildCount.text = text;
				uilabel2.text = text;
			}
			this.buildTime.text = facilityMaster.buildingTime.ToBuildTime();
			if (int.Parse(facilityMaster.buildingAssetCategoryId1) == 4)
			{
				this.buyTypeTip.gameObject.SetActive(true);
				this.price.text = StringFormat.Cluster(facilityMaster.buildingAssetNum1);
			}
			else
			{
				this.buyTypeStone.gameObject.SetActive(true);
				this.price.text = facilityMaster.buildingAssetNum1;
			}
			if (FarmUtility.IsShortage(facilityMaster.buildingAssetCategoryId1, facilityMaster.buildingAssetNum1))
			{
				this.SetButtonGrayout();
			}
			if (nowCount >= num)
			{
				this.SetButtonLimit();
			}
			if (facilityConditionMaster.Length > 0 && facilityConditionMaster[0].conditionType == 5.ToString())
			{
				this.BuildEventItemMode();
			}
		}
	}

	public void SetNewIcon(bool isDisplay)
	{
		this.newIcon.SetActive(isDisplay);
	}

	private void SetButtonGrayout()
	{
		this.buyButton.CallBackClass = null;
		this.buyButton.touchBehavior = GUICollider.TouchBehavior.None;
		this.enableButtonImage.SetActive(false);
		this.disableButtonImage.SetActive(true);
		this.price.color = this.notBuyPriceColor;
	}

	private void SetButtonLimit()
	{
		this.buyButton.CallBackClass = null;
		this.buyButton.touchBehavior = GUICollider.TouchBehavior.None;
		this.enableButtonImage.SetActive(false);
		this.disableButtonImage.SetActive(false);
		this.buyTypeStone.gameObject.SetActive(false);
		this.buyTypeTip.gameObject.SetActive(false);
		this.price.gameObject.SetActive(false);
		this.limitLabel.gameObject.SetActive(true);
		this.limitLabel.text = StringMaster.GetString("FacilityShopBuildMaxInfo");
		this.limitLabel.color = this.limitLabelColor;
	}

	private int GetNowCount()
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null == instance)
		{
			global::Debug.LogError("FarmRoot Not Found");
			return -1;
		}
		int facilityCount = instance.Scenery.GetFacilityCount(this.facilityID);
		List<UserFacility> stockFacilityListByfacilityIdAndLevel = Singleton<UserDataMng>.Instance.GetStockFacilityListByfacilityIdAndLevel(this.facilityID, -1);
		int count = stockFacilityListByfacilityIdAndLevel.Count;
		return facilityCount + count;
	}

	private void ChangeEventItemMode()
	{
		this.normalPartsRoot.SetActive(false);
		this.eventPartsRoot.SetActive(true);
	}

	private void BuildEventItemMode()
	{
		this.buyButton.gameObject.SetActive(true);
		this.normalPartsRoot.SetActive(false);
		this.eventPartsRoot.SetActive(true);
	}

	private void DetailSetActive(bool isActive)
	{
		this.detail.SetActive(isActive);
		this.facilityName.gameObject.SetActive(isActive);
		this.eventFacilityName.gameObject.SetActive(isActive);
		this.buildCount.gameObject.SetActive(isActive);
		this.eventBuildCount.gameObject.SetActive(isActive);
		this.buildTime.gameObject.SetActive(isActive);
		this.eventBuildTime.gameObject.SetActive(isActive);
	}

	private void OnPushedBuyButton()
	{
		int nowCount = this.GetNowCount();
		if (nowCount != -1)
		{
			global::Debug.Log("this.facilityID = " + this.facilityID);
			FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(this.facilityID);
			if (int.Parse(facilityMaster.maxNum) <= nowCount)
			{
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("FacilityShopBuildMaxTitle");
				cmd_ModalMessage.Info = StringMaster.GetString("FacilityShopMuxNumInfo");
			}
			else if (2 <= FarmUtility.GetBuildFacilityCount())
			{
				CMD_ModalMessage cmd_ModalMessage2 = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
				cmd_ModalMessage2.Title = StringMaster.GetString("FacilityShopBuildMaxNumTitle");
				cmd_ModalMessage2.Info = StringMaster.GetString("FacilityShopBuildMaxNumInfo");
			}
			else if (this.onPushedBuyButton != null)
			{
				this.onPushedBuyButton(this);
			}
		}
	}

	private void OnInfoButton()
	{
		CMD_FacilityInfoNoneEffect cmd_FacilityInfoNoneEffect = GUIMain.ShowCommonDialog(null, "CMD_FacilityInfo_only") as CMD_FacilityInfoNoneEffect;
		cmd_FacilityInfoNoneEffect.SetFacilityInfo(this.facilityID);
	}
}
