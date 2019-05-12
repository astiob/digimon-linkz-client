using FarmData;
using Master;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FacilityButtonSet : MonoBehaviour
{
	private FarmObject farmObject;

	[SerializeField]
	protected UILabel facilityName;

	private int buttonSpace = 20;

	private GUICollider infoButton;

	private GUICollider upgradeButton;

	private GUICollider shortCutButton;

	private GUICollider sellButton;

	private GUICollider stockButton;

	private BuildCostLabel buildCostLabel;

	private float buildCostLabelPosY;

	private void Start()
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.y -= GUIMain.VerticalSpaceSize;
		base.transform.localPosition = localPosition;
		GameObject gameObject = AssetDataMng.Instance().LoadObject("UICommon/Farm/BuildCostLabel", null, true) as GameObject;
		if (null != gameObject)
		{
			this.buildCostLabelPosY = gameObject.transform.localPosition.y;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			this.buildCostLabel = gameObject2.GetComponent<BuildCostLabel>();
			this.buildCostLabel.gameObject.SetActive(false);
			Transform transform = gameObject2.transform;
			transform.parent = base.transform;
			transform.localScale = Vector3.one;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}
		GUICollider[] componentsInChildren = base.gameObject.GetComponentsInChildren<GUICollider>();
		if (componentsInChildren != null)
		{
			foreach (GUICollider guicollider in componentsInChildren)
			{
				if ("InfoButton" == guicollider.name)
				{
					guicollider.onTouchEnded += this.OnPushedInfoButton;
					this.infoButton = guicollider;
				}
				if ("UpgradeButton" == guicollider.name)
				{
					guicollider.onTouchEnded += this.OnPushedUpgradeButton;
					this.upgradeButton = guicollider;
				}
				if ("ShortCutButton" == guicollider.name)
				{
					guicollider.onTouchEnded += this.OnPushedShortCutButton;
					this.shortCutButton = guicollider;
				}
				if ("SellButton" == guicollider.name)
				{
					guicollider.onTouchEnded += this.OnPushedSellButton;
					this.sellButton = guicollider;
				}
				if ("StockButton" == guicollider.name)
				{
					guicollider.onTouchEnded += this.OnPushedStockButton;
					this.stockButton = guicollider;
				}
			}
		}
		this.SettingButton();
	}

	private void OnDestroy()
	{
		if (null != this.infoButton)
		{
			this.infoButton.onTouchEnded -= this.OnPushedInfoButton;
		}
		if (null != this.upgradeButton)
		{
			this.upgradeButton.onTouchEnded -= this.OnPushedUpgradeButton;
		}
		if (null != this.shortCutButton)
		{
			this.shortCutButton.onTouchEnded -= this.OnPushedShortCutButton;
		}
		if (null != this.sellButton)
		{
			this.sellButton.onTouchEnded -= this.OnPushedSellButton;
		}
		if (null != this.stockButton)
		{
			this.stockButton.onTouchEnded -= this.OnPushedStockButton;
		}
	}

	public void SetFacility(FarmObject facility)
	{
		this.farmObject = facility;
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(this.farmObject.facilityID);
		FacilityUpgradeM facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(this.farmObject.facilityID, 1);
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(facility.userFacilityID);
		if (userFacility.level != 0 && int.Parse(facilityMaster.type) == 1 && facilityUpgradeMaster != null)
		{
			this.facilityName.text = string.Format(StringMaster.GetString("FacilityInfoLv"), facilityMaster.facilityName, userFacility.level);
		}
		else
		{
			this.facilityName.text = facilityMaster.facilityName;
		}
	}

	public void SettingButton()
	{
		this.SetButtonEnabled();
		this.SetButtonFriendFarm();
		this.SetCostInfo();
		this.SetButtonAlign();
	}

	private void SetButtonEnabled()
	{
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.farmObject.userFacilityID);
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(this.farmObject.facilityID);
		FacilityUpgradeM facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(userFacility.facilityId, userFacility.level);
		bool flag = !string.IsNullOrEmpty(userFacility.completeTime);
		DrawFacilityButton[] componentsInChildren = base.GetComponentsInChildren<DrawFacilityButton>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			bool active;
			if (flag)
			{
				if (userFacility.level == 0)
				{
					active = componentsInChildren[i].build;
				}
				else
				{
					active = componentsInChildren[i].upgrade;
				}
			}
			else if (facilityUpgradeMaster != null && int.Parse(facilityMaster.maxLevel) <= userFacility.level)
			{
				active = componentsInChildren[i].levelMax;
			}
			else
			{
				active = componentsInChildren[i].usually;
			}
			componentsInChildren[i].gameObject.SetActive(active);
		}
		if (this.shortCutButton.gameObject.activeSelf)
		{
			if (userFacility.level == 0)
			{
				this.shortCutButton.gameObject.SetActive(facilityMaster.shorteningFlg == "1");
			}
			else
			{
				this.shortCutButton.gameObject.SetActive(facilityUpgradeMaster.shorteningFlg == "1");
			}
		}
		if (null != this.stockButton && null != this.stockButton.gameObject)
		{
			bool active2 = facilityMaster.stockFlg == "1" && !flag;
			this.stockButton.gameObject.SetActive(active2);
		}
	}

	private void SetButtonFriendFarm()
	{
		DrawFacilityButton[] componentsInChildren = base.GetComponentsInChildren<DrawFacilityButton>(false);
		if (FarmRoot.Instance.IsVisitFriendFarm)
		{
			for (int i = 1; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.SetActive(false);
			}
			componentsInChildren[0].gameObject.SetActive(true);
		}
	}

	private void SetCostInfo()
	{
		DrawFacilityButton[] componentsInChildren = base.GetComponentsInChildren<DrawFacilityButton>();
		if (componentsInChildren == null || null == this.buildCostLabel)
		{
			return;
		}
		this.buildCostLabel.gameObject.SetActive(false);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Transform transform = componentsInChildren[i].transform;
			if ("UpgradeButton" == transform.name)
			{
				this.buildCostLabel.SetUpgradeCostDetail(this.farmObject.userFacilityID);
				this.buildCostLabel.gameObject.SetActive(true);
				this.SetBuildCostLabelPosition(transform.parent.gameObject);
				break;
			}
			if ("ShortCutButton" == transform.name)
			{
				if (this.farmObject.IsTutorialFacility())
				{
					FarmUI componentInParent = base.GetComponentInParent<FarmUI>();
					ConstructionDetail constructionDetail = componentInParent.GetConstructionDetail(this.farmObject.userFacilityID);
					int complateSeconds = constructionDetail.GetComplateSeconds();
					UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.farmObject.userFacilityID);
					userFacility.completeTime = FarmUtility.GetDateString(ServerDateTime.Now.AddSeconds((double)complateSeconds));
				}
				this.buildCostLabel.SetShortCutCostDetail(this.farmObject.userFacilityID);
				this.buildCostLabel.gameObject.SetActive(true);
				this.SetBuildCostLabelPosition(transform.parent.gameObject);
				break;
			}
		}
	}

	private void SetBuildCostLabelPosition(GameObject relationButton)
	{
		Vector3 localPosition = this.buildCostLabel.transform.localPosition;
		localPosition.x = relationButton.transform.localPosition.x;
		localPosition.y = this.buildCostLabelPosY + relationButton.transform.localPosition.y;
		this.buildCostLabel.transform.localPosition = localPosition;
	}

	private void SetButtonAlign()
	{
		DrawFacilityButton[] componentsInChildren = base.GetComponentsInChildren<DrawFacilityButton>();
		if (componentsInChildren == null)
		{
			return;
		}
		BoxCollider component = componentsInChildren[0].GetComponent<BoxCollider>();
		float x = component.size.x;
		int num = this.buttonSpace;
		int num2 = componentsInChildren.Length;
		int num3 = (num2 - 1) / 2;
		float num6;
		if ((num2 & 1) == 1)
		{
			float num4 = x * (float)num3;
			int num5 = num * num3;
			num6 = -(num4 + (float)num5);
		}
		else
		{
			float num7 = x * (float)num3 + x * 0.5f;
			float num8 = (float)(num * num3) + (float)num * 0.5f;
			num6 = -(num7 + num8);
		}
		List<int> list = new List<int>();
		list.Add(0);
		for (int i = 1; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < list.Count; j++)
			{
				DrawFacilityButton drawFacilityButton = componentsInChildren[list[j]];
				if (drawFacilityButton.transform.parent.localPosition.x > componentsInChildren[i].transform.parent.localPosition.x)
				{
					list.Insert(j, i);
					break;
				}
			}
			if (i + 1 != list.Count)
			{
				list.Add(i);
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			DrawFacilityButton drawFacilityButton2 = componentsInChildren[list[k]];
			Vector3 localPosition = drawFacilityButton2.transform.localPosition;
			localPosition.x = num6 - drawFacilityButton2.transform.parent.localPosition.x;
			drawFacilityButton2.transform.localPosition = localPosition;
			num6 += x + (float)num;
		}
	}

	protected void OnPushedInfoButton(Touch touch, Vector2 pos, bool touchOver)
	{
		if (!touchOver)
		{
			return;
		}
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.farmObject.userFacilityID);
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(this.farmObject.facilityID);
		this.DrawInfo(facilityMaster, userFacility);
	}

	protected virtual void DrawInfo(FacilityM master, UserFacility userFacility)
	{
		bool flag = FarmDataManager.facilityUpgradeMaster.Any((FacilityUpgradeM x) => int.Parse(x.facilityId) == this.farmObject.facilityID);
		bool isFriendFarm = FarmRoot.Instance.IsVisitFriendFarm;
		Action<int> action = delegate(int i)
		{
			if (isFriendFarm)
			{
				GUIManager.HideBarrier();
			}
		};
		if (flag && 0 < userFacility.level && !isFriendFarm)
		{
			CMD_FacilityInfo cmd_FacilityInfo = GUIMain.ShowCommonDialog(action, "CMD_FacilityInfo", null) as CMD_FacilityInfo;
			cmd_FacilityInfo.SetFacilityInfo(userFacility);
		}
		else
		{
			CMD_FacilityInfoNoneEffect cmd_FacilityInfoNoneEffect = GUIMain.ShowCommonDialog(action, "CMD_FacilityInfo_only", null) as CMD_FacilityInfoNoneEffect;
			cmd_FacilityInfoNoneEffect.SetFacilityInfo(userFacility.facilityId);
		}
	}

	protected virtual void OnPushedUpgradeButton(Touch touch, Vector2 pos, bool touchOver)
	{
		if (!touchOver)
		{
			return;
		}
		this.OpenUpgradeDialog();
	}

	private void OpenUpgradeDialog()
	{
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(this.farmObject.facilityID);
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.farmObject.userFacilityID);
		if (userFacility != null && !string.IsNullOrEmpty(userFacility.completeTime))
		{
			return;
		}
		if (int.Parse(facilityMaster.maxLevel) <= userFacility.level)
		{
			return;
		}
		int level = userFacility.level + 1;
		FacilityUpgradeM facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(userFacility.facilityId, level);
		MasterDataMng.AssetCategory assetCategory = (MasterDataMng.AssetCategory)facilityUpgradeMaster.upgradeAssetCategoryId1.ToInt32();
		if (assetCategory == MasterDataMng.AssetCategory.TIP)
		{
			CMD_UpgradeConfirmation cmd_UpgradeConfirmation = GUIMain.ShowCommonDialog(null, "CMD_UpgradeConfirmation", null) as CMD_UpgradeConfirmation;
			cmd_UpgradeConfirmation.SetUserFacility(userFacility);
		}
		else
		{
			CMD_UpgradeConfirmationDigistone cmd_UpgradeConfirmationDigistone = GUIMain.ShowCommonDialog(null, "CMD_UpgradeConfirm_STONE", null) as CMD_UpgradeConfirmationDigistone;
			cmd_UpgradeConfirmationDigistone.SetUserFacility(userFacility);
		}
	}

	protected virtual void OnPushedShortCutButton(Touch touch, Vector2 pos, bool touchOver)
	{
		if (!touchOver)
		{
			return;
		}
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.farmObject.userFacilityID);
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(userFacility.facilityId);
		string shorteningAssetCategoryId = facilityMaster.shorteningAssetCategoryId1;
		GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(shorteningAssetCategoryId);
		FacilityUpgradeM facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(userFacility.facilityId, userFacility.level);
		GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategoryM = (facilityUpgradeMaster == null) ? null : MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(facilityUpgradeMaster.shorteningAssetCategoryId1);
		bool flag = false;
		string arg = string.Empty;
		if (userFacility.level == 0)
		{
			flag = FarmUtility.IsShortage(facilityMaster.shorteningAssetCategoryId1, this.buildCostLabel.GetCost().ToString());
			arg = assetCategory.assetTitle;
		}
		else if (facilityUpgradeMaster != null)
		{
			flag = FarmUtility.IsShortage(facilityUpgradeMaster.shorteningAssetCategoryId1, this.buildCostLabel.GetCost().ToString());
			arg = assetCategoryM.assetTitle;
		}
		if (flag)
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseConfirmShop), "CMD_Confirm", null) as CMD_Confirm;
			cmd_Confirm.Title = string.Format(StringMaster.GetString("SystemShortage"), arg);
			cmd_Confirm.Info = string.Format(StringMaster.GetString("FacilityShortcutShortage"), arg);
			cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonGoShop");
			cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
		}
		else
		{
			this.OpenShortCutDialog(userFacility);
		}
	}

	private bool OpenShortCutDialog(UserFacility userFacility)
	{
		bool flag = null != this.buildCostLabel;
		if (flag)
		{
			FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(userFacility.facilityId);
			string shorteningAssetCategoryId = facilityMaster.shorteningAssetCategoryId1;
			GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(shorteningAssetCategoryId);
			FacilityUpgradeM facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(userFacility.facilityId, userFacility.level);
			GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategoryM = (facilityUpgradeMaster == null) ? null : MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(facilityUpgradeMaster.shorteningAssetCategoryId1);
			CMD_ChangePOP_STONE cmd_ChangePOP_STONE = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP_STONE", null) as CMD_ChangePOP_STONE;
			cmd_ChangePOP_STONE.Title = StringMaster.GetString("FacilityShortcutTitle");
			cmd_ChangePOP_STONE.OnPushedYesAction = new Action(this.OnPushedShortCutYesButton);
			int point = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
			if (userFacility.level == 0)
			{
				cmd_ChangePOP_STONE.Info = string.Format(StringMaster.GetString("FacilityShortcutBuild"), assetCategory.assetTitle);
			}
			else if (facilityUpgradeMaster != null)
			{
				cmd_ChangePOP_STONE.Info = string.Format(StringMaster.GetString("FacilityShortcutUpgrage"), assetCategoryM.assetTitle);
			}
			cmd_ChangePOP_STONE.SetDigistone(point, this.buildCostLabel.GetCost());
		}
		return flag;
	}

	private void OnPushedShortCutYesButton()
	{
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.farmObject.userFacilityID);
		if (userFacility.level == 0)
		{
			this.RequestBuildShortening();
		}
		else
		{
			this.RequestUpgradeShortening();
		}
	}

	private void OnCloseConfirmShop(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			int digiStoneNum = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
			FarmUI farmUI = base.GetComponentInParent<FarmUI>();
			Action<int> action = delegate(int nop)
			{
				if (digiStoneNum < DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point)
				{
					farmUI.UpdateFacilityButton(null);
				}
			};
			GUIMain.ShowCommonDialog(action, "CMD_Shop", null);
		}
	}

	private void RequestBuildShortening()
	{
		if (!this.farmObject.IsTutorialFacility())
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			RequestFA_FacilityBuildShortening request = new RequestFA_FacilityBuildShortening
			{
				SetSendData = delegate(FacilityBuildShortening param)
				{
					param.userFacilityId = this.farmObject.userFacilityID;
				},
				OnReceived = new Action<FacilityBuildShorteningResult>(this.OnSuccessedShortening)
			};
			base.StartCoroutine(request.Run(delegate()
			{
				this.CloseChangePop();
				RestrictionInput.EndLoad();
			}, null, null));
		}
		else
		{
			this.SuccessShortening();
			this.CloseChangePop();
		}
	}

	private void RequestUpgradeShortening()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		RequestFA_FacilityUpgradeShortening request = new RequestFA_FacilityUpgradeShortening
		{
			SetSendData = delegate(FacilityUpgradeShortening param)
			{
				param.userFacilityId = this.farmObject.userFacilityID;
			},
			OnReceived = new Action<FacilityUpgradeShorteningResult>(this.OnSuccessedShortening)
		};
		base.StartCoroutine(request.Run(delegate()
		{
			this.CloseChangePop();
			RestrictionInput.EndLoad();
		}, null, null));
	}

	private void OnSuccessedShortening(WebAPI.ResponseData response)
	{
		int num = 0;
		FacilityBuildShorteningResult facilityBuildShorteningResult = response as FacilityBuildShorteningResult;
		if (facilityBuildShorteningResult != null)
		{
			num = facilityBuildShorteningResult.num;
			UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.farmObject.userFacilityID);
			FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(userFacility.facilityId);
			FarmUtility.PayCost(facilityMaster.shorteningAssetCategoryId1, num.ToString());
		}
		else
		{
			FacilityUpgradeShorteningResult facilityUpgradeShorteningResult = response as FacilityUpgradeShorteningResult;
			if (facilityUpgradeShorteningResult != null)
			{
				num = facilityUpgradeShorteningResult.num;
				UserFacility userFacility2 = Singleton<UserDataMng>.Instance.GetUserFacility(this.farmObject.userFacilityID);
				FacilityUpgradeM facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(userFacility2.facilityId, userFacility2.level);
				FarmUtility.PayCost(facilityUpgradeMaster.shorteningAssetCategoryId1, num.ToString());
			}
		}
		if (0 >= num)
		{
			CMD_ChangePOP_STONE cmd_ChangePOP_STONE = UnityEngine.Object.FindObjectOfType<CMD_ChangePOP_STONE>();
			if (null != cmd_ChangePOP_STONE)
			{
				cmd_ChangePOP_STONE.SetCloseAction(new Action<int>(this.OpenNoNeedShortening));
			}
		}
		this.SuccessShortening();
	}

	private void OpenNoNeedShortening(int noop)
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("FacilityShortcutTitle");
		cmd_ModalMessage.Info = StringMaster.GetString("FacilityShortcutAlready");
	}

	private void CloseChangePop()
	{
		CMD_ChangePOP_STONE cmd_ChangePOP_STONE = UnityEngine.Object.FindObjectOfType<CMD_ChangePOP_STONE>();
		if (null != cmd_ChangePOP_STONE)
		{
			cmd_ChangePOP_STONE.ClosePanel(true);
		}
	}

	private void SuccessShortening()
	{
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.farmObject.userFacilityID);
		userFacility.completeTime = null;
		userFacility.level++;
		this.farmObject.BuildCompleteEffect();
		this.farmObject.ClearConstruction();
		this.SetFacility(this.farmObject);
		this.SettingButton();
	}

	protected virtual void OnPushedSellButton(Touch touch, Vector2 pos, bool touchOver)
	{
		if (!touchOver)
		{
			return;
		}
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(this.farmObject.facilityID);
		if (facilityMaster.sellFlg == "1")
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnPushedSellYesButton), "CMD_Confirm", null) as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("SystemConfirm");
			cmd_Confirm.Info = string.Format(StringMaster.GetString("FacilitySaleComfirmInfo"), facilityMaster.facilityName, facilityMaster.sellPrice);
		}
	}

	private void OnPushedSellYesButton(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			base.StartCoroutine(this.RequestSell().Run(delegate
			{
				RestrictionInput.EndLoad();
				FarmRoot instance = FarmRoot.Instance;
				if (null != instance)
				{
					instance.Scenery.DeleteFarmObject(this.farmObject.userFacilityID);
				}
			}, null, null));
		}
	}

	private APIRequestTask RequestSell()
	{
		GameWebAPI.RequestFA_FacilitySell requestFA_FacilitySell = new GameWebAPI.RequestFA_FacilitySell();
		requestFA_FacilitySell.SetSendData = delegate(GameWebAPI.FA_Req_FacilitySell param)
		{
			param.userFacilityId = this.farmObject.userFacilityID;
		};
		requestFA_FacilitySell.OnReceived = delegate(GameWebAPI.RespDataFA_FacilitySell response)
		{
			int num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
			num += response.sellPrice;
			DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney = num.ToString();
			GUIPlayerStatus.RefreshParams_S(false);
		};
		GameWebAPI.RequestFA_FacilitySell request = requestFA_FacilitySell;
		return new APIRequestTask(request, true);
	}

	protected virtual void OnPushedStockButton(Touch touch, Vector2 pos, bool touchOver)
	{
		if (!touchOver)
		{
			return;
		}
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(this.farmObject.facilityID);
		if (facilityMaster.stockFlg == "1")
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnPushedStockYesButton), "CMD_Confirm", null) as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("SystemConfirm");
			cmd_Confirm.Info = string.Format(StringMaster.GetString("FacilityStockConfirmInfo"), facilityMaster.facilityName);
		}
	}

	private void OnPushedStockYesButton(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			base.StartCoroutine(this.RequestStock().Run(delegate
			{
				RestrictionInput.EndLoad();
				FarmRoot instance = FarmRoot.Instance;
				if (null != instance)
				{
					UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.farmObject.userFacilityID);
					if (Singleton<UserDataMng>.Instance.IsLoadedUserStockFacility)
					{
						Singleton<UserDataMng>.Instance.AddUserStockFacility(userFacility);
					}
					instance.Scenery.DeleteFarmObject(this.farmObject.userFacilityID);
				}
			}, null, null));
		}
	}

	private APIRequestTask RequestStock()
	{
		RequestFA_FacilityStock request = new RequestFA_FacilityStock
		{
			SetSendData = delegate(FacilityStock param)
			{
				param.userFacilityId = this.farmObject.userFacilityID;
				param.stockFlg = 1;
			}
		};
		return new APIRequestTask(request, true);
	}
}
