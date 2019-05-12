using Facility;
using FarmData;
using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FarmObject_MeatFarm : FarmObject
{
	[SerializeField]
	private GameObject harvestAnimationRoot;

	private Vector3 meatAnimationRootPosition;

	private Coroutine meatAnimation;

	[SerializeField]
	private GameObject[] meatModels;

	private Action actionFinishedHarvest;

	private int dummyMeatHarvestCount;

	private UISprite meatMain;

	private List<GameObject> meatList = new List<GameObject>();

	[SerializeField]
	private GameObject campaignPlate;

	private GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo;

	private bool enabledCamera;

	public bool IsDummyFacility { get; set; }

	protected override void Start()
	{
		base.Start();
		this.meatAnimationRootPosition = this.harvestAnimationRoot.transform.localPosition;
		this.harvestAnimationRoot.transform.localScale = Vector3.zero;
		if (!base.IsConstruction())
		{
			UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.userFacilityID);
			if (userFacility != null && 0 < userFacility.level)
			{
				this.meatAnimation = base.StartCoroutine(this.DrawMeat());
			}
		}
		GameObject gameObject = GUIFaceIndicator.instance.transform.FindChild("PlayerStatus/MeatBase/Meat").gameObject;
		this.meatMain = gameObject.GetComponent<UISprite>();
		GameObject gameObject2 = base.transform.FindChild("Locator/ModelRoot/Meat/Meat1").gameObject;
		for (int i = 0; i < 6; i++)
		{
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
			Vector3 localScale = gameObject3.transform.localScale;
			gameObject3.transform.SetParent(this.meatMain.transform.parent);
			gameObject3.transform.localScale = localScale;
			gameObject3.transform.localRotation = Quaternion.Euler(30f, -130f, 0f);
			gameObject3.SetActive(false);
			gameObject3.layer = LayerMask.NameToLayer("UI");
			gameObject3.GetComponent<MeshRenderer>().material.color = new Color(0.95f, 0.95f, 0.95f);
			this.meatList.Add(gameObject3);
		}
	}

	private void Update()
	{
		if (FarmRoot.Instance != null && this.enabledCamera != FarmRoot.Instance.Camera.enabled)
		{
			this.enabledCamera = FarmRoot.Instance.Camera.enabled;
			if (this.enabledCamera)
			{
				GameWebAPI.RespDataCP_Campaign respDataCP_Campaign = DataMng.Instance().RespDataCP_Campaign;
				if (respDataCP_Campaign != null)
				{
					this.campaignInfo = respDataCP_Campaign.GetCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType.MeatHrvUp, false);
				}
				else
				{
					this.campaignInfo = null;
				}
			}
		}
		this.SetCampaignPlate((!(FarmRoot.Instance.SettingObject.farmObject == this) || FarmRoot.Instance.SettingObject.settingMode != FarmObjectSetting.SettingMode.BUILD) && !base.IsConstruction() && this.campaignInfo != null);
	}

	protected override bool StartSelectAnimation(bool selected)
	{
		bool flag = false;
		if (!base.IsConstruction() && !FarmRoot.Instance.IsVisitFriendFarm)
		{
			flag = this.StartMeatHarvest(selected);
		}
		if (!flag && !selected)
		{
			base.StartCoroutine(this.PlayAnimation(FacilityAnimationID.SELECT));
		}
		return flag;
	}

	public void OnTapShortCutButton()
	{
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.userFacilityID);
		int cropCount = this.GetCropCount(this.GetPassSeconds(), userFacility.level);
		FacilityMeatFieldM facilityMeatFarmMaster = FarmDataManager.GetFacilityMeatFarmMaster(userFacility.level);
		int num = int.Parse(facilityMeatFarmMaster.maxMeatNum);
		if (cropCount >= num)
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("MeatShortCutTitle3");
			cmd_ModalMessage.Info = StringMaster.GetString("MeatShortCutText3");
			cmd_ModalMessage.BtnText = "OK";
		}
		else
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnTapMeatShortCut), "CMD_Confirm") as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("MeatShortCutTitle1");
			cmd_Confirm.Info = string.Format(StringMaster.GetString("MeatShortCutText1"), ConstValue.MEAT_SHORTCUT_DEGISTONE_NUM);
		}
	}

	private void OnTapMeatShortCut(int idx)
	{
		if (idx == 0)
		{
			bool flag = FarmUtility.IsShortage(2.ToString(), ConstValue.MEAT_SHORTCUT_DEGISTONE_NUM.ToString());
			if (flag)
			{
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseConfirmShop), "CMD_Confirm") as CMD_Confirm;
				cmd_Confirm.Title = string.Format(StringMaster.GetString("SystemShortage"), "デジストーン");
				cmd_Confirm.Info = string.Format(StringMaster.GetString("FacilityShortcutShortage"), "デジストーン");
				cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonGoShop");
				cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
			}
			else
			{
				UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.userFacilityID);
				int cropCount = this.GetCropCount(this.GetPassSeconds(), userFacility.level);
				UserFacility userStorehouse = Singleton<UserDataMng>.Instance.GetUserStorehouse();
				FacilityMeatFieldM facilityMeatFarmMaster = FarmDataManager.GetFacilityMeatFarmMaster(userFacility.level);
				int num = int.Parse(facilityMeatFarmMaster.maxMeatNum);
				if (cropCount >= num)
				{
					CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
					cmd_ModalMessage.Title = StringMaster.GetString("MeatShortCutTitle3");
					cmd_ModalMessage.Info = StringMaster.GetString("MeatShortCutText3");
					cmd_ModalMessage.BtnText = "OK";
				}
				else if (!this.IsOverMeatNum(num, userStorehouse))
				{
					this.MeatShortCutSelect(0);
				}
				else
				{
					CMD_Confirm cmd_Confirm2 = GUIMain.ShowCommonDialog(new Action<int>(this.MeatShortCutSelect), "CMD_Confirm") as CMD_Confirm;
					cmd_Confirm2.Title = StringMaster.GetString("MeatShortCutTitle2");
					cmd_Confirm2.Info = StringMaster.GetString("MeatShortCutText2");
				}
			}
		}
		else if (idx == 1)
		{
		}
	}

	private void OnCloseConfirmShop(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			int digiStoneNum = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
			FarmUI farmUI = FarmRoot.Instance.farmUI;
			Action<int> action = delegate(int nop)
			{
				if (digiStoneNum < DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point && farmUI != null)
				{
					farmUI.UpdateFacilityButton(null);
				}
			};
			GUIMain.ShowCommonDialog(action, "CMD_Shop");
		}
	}

	private void MeatShortCutSelect(int idx)
	{
		if (idx == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_OFF);
			base.StartCoroutine(this.RequestHarvestShortCut(this.userFacilityID, 100));
		}
	}

	private bool StartMeatHarvest(bool selected)
	{
		bool result = false;
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.userFacilityID);
		int cropCount = this.GetCropCount(this.GetPassSeconds(), userFacility.level);
		if (0 < cropCount)
		{
			result = true;
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_OFF);
			base.StartCoroutine(this.MeatHarvestCampaign(delegate(FarmObject_MeatFarm.CampaignState campaignState, int campaignRate)
			{
				bool flag = true;
				string info = string.Empty;
				switch (campaignState)
				{
				case FarmObject_MeatFarm.CampaignState.END:
					info = StringMaster.GetString("CampaignFarmEndInfo");
					break;
				case FarmObject_MeatFarm.CampaignState.START:
					info = StringMaster.GetString("CampaignFarmStartInfo");
					break;
				case FarmObject_MeatFarm.CampaignState.CHANGE:
					info = StringMaster.GetString("CampaignFarmChangeInfo");
					break;
				default:
					flag = false;
					break;
				}
				if (flag)
				{
					RestrictionInput.EndLoad();
					CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int noop)
					{
						RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_OFF);
						this.StartRequestHarvest(cropCount, campaignRate);
					}, "CMD_ModalMessage") as CMD_ModalMessage;
					cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
					cmd_ModalMessage.Info = info;
				}
				else
				{
					this.StartRequestHarvest(cropCount, campaignRate);
				}
			}));
		}
		return result;
	}

	private void StartRequestHarvest(int cropCount, int campaignRate)
	{
		cropCount *= campaignRate;
		UserFacility userStorehouse = Singleton<UserDataMng>.Instance.GetUserStorehouse();
		int storehouseUserFacilityID = 0;
		if (userStorehouse != null)
		{
			storehouseUserFacilityID = userStorehouse.userFacilityId;
		}
		if (!this.IsOverMeatNum(cropCount, userStorehouse))
		{
			if (this.meatAnimation != null)
			{
				base.StopCoroutine(this.meatAnimation);
				this.meatAnimation = null;
			}
			base.StartCoroutine(this.RequestHarvest(storehouseUserFacilityID));
		}
		else
		{
			string @string;
			if (userStorehouse != null)
			{
				@string = StringMaster.GetString("FacilityMeatAlert");
				FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(userStorehouse.facilityId);
				int num = 1;
				if (facilityMaster != null && int.TryParse(facilityMaster.maxLevel, out num) && userStorehouse.level >= num)
				{
					@string = StringMaster.GetString("FacilityMeatAlertLvMax");
				}
			}
			else
			{
				@string = StringMaster.GetString("FacilityMeatAlertBuild");
			}
			RestrictionInput.EndLoad();
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("FacilityMeatAlertTitle");
			cmd_ModalMessage.Info = @string;
		}
	}

	private IEnumerator MeatHarvestCampaign(Action<FarmObject_MeatFarm.CampaignState, int> onFinished)
	{
		GameWebAPI.RespDataCP_Campaign campaign = DataMng.Instance().RespDataCP_Campaign;
		if (campaign != null)
		{
			GameWebAPI.RespDataCP_Campaign.CampaignInfo info = campaign.GetCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType.MeatHrvUp, false);
			if (info != null)
			{
				GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo = new GameWebAPI.RespDataCP_Campaign.CampaignInfo();
				campaignInfo.campaignManageId = info.campaignManageId;
				campaignInfo.campaignId = info.campaignId;
				campaignInfo.targetValue = info.targetValue;
				campaignInfo.rate = info.rate;
				campaignInfo.openTime = info.openTime;
				campaignInfo.closeTime = info.closeTime;
			}
		}
		FarmObject_MeatFarm.CampaignState campaignState = FarmObject_MeatFarm.CampaignState.NONE;
		int campaignRate = 1;
		APIRequestTask task = DataMng.Instance().RequestCampaignAll(true);
		yield return base.StartCoroutine(task.Run(delegate
		{
			GameWebAPI.RespDataCP_Campaign respDataCP_Campaign = DataMng.Instance().RespDataCP_Campaign;
			if (respDataCP_Campaign != null)
			{
				GameWebAPI.RespDataCP_Campaign.CampaignInfo campaign2 = respDataCP_Campaign.GetCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType.MeatHrvUp, false);
				if (campaign2 == null)
				{
					GameWebAPI.RespDataCP_Campaign.CampaignInfo localInfo;
					if (localInfo != null)
					{
						campaignState = FarmObject_MeatFarm.CampaignState.END;
					}
				}
				else
				{
					GameWebAPI.RespDataCP_Campaign.CampaignInfo localInfo;
					if (localInfo == null)
					{
						campaignState = FarmObject_MeatFarm.CampaignState.START;
					}
					else if (!localInfo.IsEqualInfo(campaign2))
					{
						campaignState = FarmObject_MeatFarm.CampaignState.CHANGE;
					}
					campaignRate = campaign2.rate.ToInt32();
				}
			}
		}, null, null));
		if (onFinished != null)
		{
			onFinished(campaignState, campaignRate);
		}
		yield break;
	}

	public int GetCropCount(int passSeconds, int meatFarmLevel)
	{
		int num = 0;
		FacilityMeatFieldM facilityMeatFarmMaster = FarmDataManager.GetFacilityMeatFarmMaster(meatFarmLevel);
		if (facilityMeatFarmMaster != null)
		{
			int num2 = int.Parse(facilityMeatFarmMaster.perMeatTime);
			int num3 = int.Parse(facilityMeatFarmMaster.maxMeatNum);
			num = passSeconds / num2;
			if (num > num3)
			{
				num = num3;
			}
		}
		return num;
	}

	private IEnumerator RequestHarvest(int storehouseUserFacilityID)
	{
		RequestFA_FacilityHarvest request = new RequestFA_FacilityHarvest
		{
			SetSendData = delegate(FacilityHarvest param)
			{
				param.userFacilityId1 = this.userFacilityID;
				param.userFacilityId2 = storehouseUserFacilityID;
			},
			OnReceived = delegate(FacilityHarvestResult response)
			{
				int harvestNum = response.harvestNum;
			}
		};
		APIRequestTask task = new APIRequestTask(request, true);
		task.Add(new NormalTask(delegate()
		{
			if (0 < harvestNum)
			{
				return this.MeatHarvestCompleted(harvestNum);
			}
			return null;
		}));
		yield return base.StartCoroutine(task.Run(delegate
		{
			if (this.meatAnimation == null)
			{
				this.meatAnimation = base.StartCoroutine(this.DrawMeat());
			}
			RestrictionInput.EndLoad();
		}, delegate(Exception exception)
		{
			WebAPIException ex = exception as WebAPIException;
			if ("E-FA20" == ex.responseDataError.subject)
			{
				RestrictionInput.EndLoad();
			}
		}, null));
		yield break;
	}

	private IEnumerator RequestHarvestShortCut(int storehouseUserFacilityID, int recoveryRate)
	{
		RequestFA_FacilityHarvestShortCut request = new RequestFA_FacilityHarvestShortCut
		{
			SetSendData = delegate(FacilityHarvestShortCut param)
			{
				param.userFacilityId = storehouseUserFacilityID;
				param.recoveryRate = recoveryRate;
			},
			OnReceived = delegate(FacilityHarvestShortCutResult response)
			{
				LastHarvestTime lastHarvestTime = Singleton<UserDataMng>.Instance.GetLastHarvestTime(this.userFacilityID);
				lastHarvestTime.lastHarvestTime = response.lastHarvestTime;
			}
		};
		APIRequestTask task = new APIRequestTask(request, true);
		yield return base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
			FarmUtility.PayCost(2.ToString(), ConstValue.MEAT_SHORTCUT_DEGISTONE_NUM.ToString());
			FarmRoot instance = FarmRoot.Instance;
			EffectAnimatorObserver buildCompleteEffect = instance.GetBuildCompleteEffect(base.transform);
			if (null != buildCompleteEffect)
			{
				EffectAnimatorEventTime component = buildCompleteEffect.GetComponent<EffectAnimatorEventTime>();
				component.SetEvent(0, new Action(this.BuildEffect));
				buildCompleteEffect.Play();
			}
		}, delegate(Exception exception)
		{
			RestrictionInput.EndLoad();
		}, null));
		yield break;
	}

	private IEnumerator MeatHarvestCompleted(int harvestNum)
	{
		LastHarvestTime lastHarvestTime = Singleton<UserDataMng>.Instance.GetLastHarvestTime(this.userFacilityID);
		lastHarvestTime.lastHarvestTime = FarmUtility.GetDateString(ServerDateTime.Now.AddSeconds(2.0));
		SoundMng.Instance().TryPlaySE("SEInternal/Farm/se_207", 0f, false, true, null, -1);
		if (null != FarmRoot.Instance.farmUI)
		{
			FarmRoot.Instance.farmUI.DeleteSignalMeatHarvest(this.userFacilityID);
		}
		yield return base.StartCoroutine(this.HarvestAnimation(harvestNum));
		yield break;
	}

	private IEnumerator HarvestAnimation(int harvestNum)
	{
		int drawMeatCount = 0;
		foreach (GameObject meatModel in this.meatModels)
		{
			if (meatModel.activeSelf)
			{
				drawMeatCount++;
				meatModel.SetActive(false);
			}
		}
		if (drawMeatCount == 0)
		{
			drawMeatCount = 1;
		}
		int width = this.meatMain.width;
		int height = this.meatMain.height;
		Vector3 scale = this.meatList[0].transform.localScale;
		Vector3 worldToViewportPoint = FarmRoot.Instance.Camera.WorldToViewportPoint(base.transform.position);
		Vector3 viewportToScreenPoint = Camera.main.ViewportToWorldPoint(worldToViewportPoint);
		viewportToScreenPoint.z = this.meatMain.transform.position.z;
		for (int i = 0; i < drawMeatCount; i++)
		{
			this.meatList[i].gameObject.SetActive(true);
			this.meatList[i].gameObject.transform.position = viewportToScreenPoint;
			this.meatList[i].transform.localScale = scale * 120f;
		}
		Coroutine coroutine = base.StartCoroutine(this.RotationMeat());
		yield return base.StartCoroutine(this.OpenMeat(harvestNum, drawMeatCount));
		yield return base.StartCoroutine(this.MoveMeat(harvestNum, drawMeatCount));
		base.StopCoroutine(coroutine);
		this.meatMain.width = width;
		this.meatMain.height = height;
		for (int j = 0; j < drawMeatCount; j++)
		{
			this.meatList[j].gameObject.SetActive(false);
			this.meatList[j].transform.localScale = scale;
		}
		yield return null;
		yield break;
	}

	private IEnumerator RotationMeat()
	{
		float rot = (float)ServerDateTime.Now.Second % 6.28318548f;
		for (;;)
		{
			if ((rot += Time.deltaTime) > 6.28318548f)
			{
				rot = 0f;
			}
			float sin = Mathf.Sin(rot);
			float cos = Mathf.Cos(rot);
			foreach (GameObject temp in this.meatList)
			{
				temp.transform.localPosition += new Vector3(3f * cos, 3f * sin, 0f);
			}
			yield return null;
		}
		yield break;
	}

	private IEnumerator OpenMeat(int harvestNum, int drawMeatCount)
	{
		List<FarmObject_MeatFarm.LerpPosition> lerpPositionList = new List<FarmObject_MeatFarm.LerpPosition>();
		float oneRot = 360f / (float)drawMeatCount;
		for (int i = 0; i < drawMeatCount; i++)
		{
			Vector3 pos = this.meatList[i].gameObject.transform.localPosition;
			Vector3 offset = Quaternion.Euler(0f, 0f, oneRot * (float)i) * new Vector3(75f + 5f * Mathf.Sin((float)(i * harvestNum)), 0f, 0f);
			offset += pos;
			FarmObject_MeatFarm.LerpPosition lerpPosition = new FarmObject_MeatFarm.LerpPosition(this.meatList[i]);
			lerpPosition.Set(0.3f, pos, offset, null);
			lerpPositionList.Add(lerpPosition);
		}
		float time = 0f;
		float maxTime = 0.3f;
		while (time < maxTime)
		{
			foreach (FarmObject_MeatFarm.LerpPosition lerpPosition2 in lerpPositionList)
			{
				lerpPosition2.Update(Time.deltaTime);
			}
			time += Time.deltaTime;
			yield return null;
		}
		yield break;
	}

	private IEnumerator MoveMeat(int harvestNum, int drawMeatCount)
	{
		List<FarmObject_MeatFarm.LerpPosition> lerpPositionList = new List<FarmObject_MeatFarm.LerpPosition>();
		List<FarmObject_MeatFarm.LerpScale> lerpScaleList = new List<FarmObject_MeatFarm.LerpScale>();
		FarmObject_MeatFarm.MeatIconAnimation meatIconAnimation = new FarmObject_MeatFarm.MeatIconAnimation(this.meatMain);
		GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
		int intMeatNum = int.Parse(playerInfo.meatNum);
		float lerpTime = 0.5f;
		for (int i = 0; i < drawMeatCount; i++)
		{
			GameObject meat = this.meatList[i];
			int startMeatNum = (int)((float)intMeatNum + (float)harvestNum * (float)i / (float)drawMeatCount);
			int endMeatNum = (int)((float)intMeatNum + (float)harvestNum * (float)(i + 1) / (float)drawMeatCount);
			FarmObject_MeatFarm.LerpPosition lerpPosition = new FarmObject_MeatFarm.LerpPosition(meat);
			Vector3 startPos = meat.gameObject.transform.localPosition;
			Vector3 endPos = this.meatMain.transform.localPosition;
			lerpPosition.Set(lerpTime, startPos, endPos, delegate
			{
				meatIconAnimation.Set(0.2f, startMeatNum, endMeatNum);
			});
			lerpPositionList.Add(lerpPosition);
			FarmObject_MeatFarm.LerpScale lerpScale = new FarmObject_MeatFarm.LerpScale(meat);
			Vector3 startScale = meat.transform.localScale;
			Vector3 endScale = meat.transform.localScale * 0.5f;
			lerpScale.Set(lerpTime, startScale, endScale, null);
			lerpScaleList.Add(lerpScale);
		}
		int count = 1;
		bool isEnd = false;
		while (!isEnd)
		{
			float time = 0f;
			float maxTime = 0.05f;
			while (time < maxTime)
			{
				for (int j = 0; j < count; j++)
				{
					if (lerpPositionList[j].isEnd)
					{
						this.meatList[j].transform.localScale = Vector3.zero;
					}
					else
					{
						lerpPositionList[j].Update(Time.deltaTime);
						lerpScaleList[j].Update(Time.deltaTime);
					}
				}
				meatIconAnimation.Update(Time.deltaTime);
				time += Time.deltaTime;
				yield return null;
			}
			if (count < lerpPositionList.Count)
			{
				count++;
			}
			isEnd = meatIconAnimation.isEnd;
			foreach (FarmObject_MeatFarm.LerpPosition lerpPosition2 in lerpPositionList)
			{
				if (!lerpPosition2.isEnd)
				{
					isEnd = false;
				}
			}
		}
		yield break;
	}

	private IEnumerator DrawMeat()
	{
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.userFacilityID);
		FacilityMeatFieldM masterMeat = FarmDataManager.GetFacilityMeatFarmMaster(userFacility.level);
		int perMeatTime = int.Parse(masterMeat.perMeatTime);
		int maxMeatNum = int.Parse(masterMeat.maxMeatNum);
		int maxSeconds = perMeatTime * maxMeatNum;
		int passSeconds = this.GetPassSeconds();
		int cropEstimate = 0;
		this.harvestAnimationRoot.transform.localScale = Vector3.one;
		this.harvestAnimationRoot.transform.localPosition = this.meatAnimationRootPosition;
		for (int i = 0; i < this.meatModels.Length; i++)
		{
			this.meatModels[i].SetActive(false);
		}
		base.StartCoroutine(this.PlayAnimation(FacilityAnimationID.IDLE));
		if (passSeconds >= maxSeconds)
		{
			for (int j = 0; j < this.meatModels.Length; j++)
			{
				this.meatModels[j].SetActive(true);
			}
			if (null != FarmRoot.Instance.farmUI)
			{
				FarmRoot.Instance.farmUI.CreateSignalMeatHarvest(this);
				if (FarmRoot.Instance.farmMode == FarmRoot.FarmControlMode.EDIT)
				{
					FarmRoot.Instance.farmUI.SetActiveSignalMeatHarvest(false);
				}
			}
		}
		else if (this.userFacilityID != 0)
		{
			while (passSeconds <= maxSeconds && !base.IsConstruction())
			{
				passSeconds = this.GetPassSeconds();
				int count = this.GetMeatCount(passSeconds, userFacility.level);
				if (count != cropEstimate)
				{
					for (int k = 0; k < count; k++)
					{
						this.meatModels[k].SetActive(true);
					}
					for (int l = count; l < this.meatModels.Length; l++)
					{
						this.meatModels[l].SetActive(false);
					}
					cropEstimate = count;
					if (null != FarmRoot.Instance.farmUI)
					{
						FarmRoot.Instance.farmUI.CreateSignalMeatHarvest(this);
						if (FarmRoot.Instance.farmMode == FarmRoot.FarmControlMode.EDIT)
						{
							FarmRoot.Instance.farmUI.SetActiveSignalMeatHarvest(false);
						}
					}
				}
				yield return new WaitForSeconds(1f);
				passSeconds++;
			}
		}
		this.meatAnimation = null;
		yield break;
	}

	private int GetMeatCount(int passTime, int meatFarmLevel)
	{
		int result = 0;
		FacilityMeatFieldM facilityMeatFarmMaster = FarmDataManager.GetFacilityMeatFarmMaster(meatFarmLevel);
		if (facilityMeatFarmMaster != null)
		{
			int num = int.Parse(facilityMeatFarmMaster.perMeatTime);
			int num2 = int.Parse(facilityMeatFarmMaster.maxMeatNum);
			float num3 = (float)passTime / (float)(num * num2);
			if (1f <= num3)
			{
				result = 6;
			}
			else if (0.8f <= num3)
			{
				result = 5;
			}
			else if (0.64f <= num3)
			{
				result = 4;
			}
			else if (0.5f <= num3)
			{
				result = 3;
			}
			else if (0.32 <= (double)num3)
			{
				result = 2;
			}
			else if (num < passTime)
			{
				result = 1;
			}
		}
		return result;
	}

	public int GetPassSeconds()
	{
		int result = 0;
		LastHarvestTime lastHarvestTime = Singleton<UserDataMng>.Instance.GetLastHarvestTime(this.userFacilityID);
		if (lastHarvestTime != null && !string.IsNullOrEmpty(lastHarvestTime.lastHarvestTime))
		{
			DateTime now = ServerDateTime.Now;
			result = (int)(now - DateTime.Parse(lastHarvestTime.lastHarvestTime)).TotalSeconds;
		}
		return result;
	}

	private bool IsOverMeatNum(int cropEstimate, UserFacility storehouse)
	{
		int num = 50;
		if (storehouse != null && 0 < storehouse.level)
		{
			FacilityWarehouseM facilityStorehouseMaster = FarmDataManager.GetFacilityStorehouseMaster(storehouse.level);
			num = int.Parse(facilityStorehouseMaster.limitMeatNum);
		}
		int num2 = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.meatNum);
		return num2 + cropEstimate > num;
	}

	public override void BuildEffect()
	{
		base.BuildEffect();
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.userFacilityID);
		if (userFacility != null)
		{
			if (!this.IsDummyFacility)
			{
				if (string.IsNullOrEmpty(userFacility.completeTime))
				{
					this.meatAnimation = base.StartCoroutine(this.DrawMeat());
				}
			}
			else
			{
				for (int i = 0; i < this.meatModels.Length; i++)
				{
					this.meatModels[i].SetActive(false);
				}
			}
		}
	}

	public override void BuildComplete()
	{
		base.BuildComplete();
		LastHarvestTime lastHarvestTime = Singleton<UserDataMng>.Instance.GetLastHarvestTime(this.userFacilityID);
		string dateString = FarmUtility.GetDateString(ServerDateTime.Now.AddSeconds(2.0));
		if (lastHarvestTime != null)
		{
			lastHarvestTime.lastHarvestTime = dateString;
		}
		else
		{
			lastHarvestTime = new LastHarvestTime
			{
				userFacilityId = this.userFacilityID,
				lastHarvestTime = dateString
			};
			Singleton<UserDataMng>.Instance.AddLastHarvestTime(lastHarvestTime);
		}
	}

	public void SetDummyMeat(int dummyMeatNum, Action completed)
	{
		if (this.meatAnimation != null)
		{
			base.StopCoroutine(this.meatAnimation);
			this.meatAnimation = null;
		}
		int num = 1;
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.userFacilityID);
		if (userFacility != null && 0 < userFacility.level)
		{
			FacilityMeatFieldM facilityMeatFarmMaster = FarmDataManager.GetFacilityMeatFarmMaster(userFacility.level);
			if (facilityMeatFarmMaster != null)
			{
				int num2 = facilityMeatFarmMaster.maxMeatNum.ToInt32();
				num = dummyMeatNum / (num2 / this.meatModels.Length);
			}
		}
		if (0 >= num)
		{
			num = 1;
		}
		else if (this.meatModels.Length <= num)
		{
			num = this.meatModels.Length;
		}
		for (int i = 0; i < num; i++)
		{
			this.meatModels[i].SetActive(true);
		}
		this.harvestAnimationRoot.transform.localPosition = this.meatAnimationRootPosition;
		this.dummyMeatHarvestCount = dummyMeatNum;
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			instance.Input.AddTouchEndEvent(new Action<InputControll, bool>(this.OnTouch));
		}
		this.actionFinishedHarvest = completed;
	}

	private void OnTouch(InputControll inputController, bool isDraged)
	{
		if (!isDraged && inputController.rayHitObjectType == InputControll.TouchObjectType.FACILITY && base.gameObject == inputController.rayHitColliderObject)
		{
			base.StartCoroutine(this.MeatHarvestCompleted(this.dummyMeatHarvestCount));
			FarmRoot instance = FarmRoot.Instance;
			if (null != instance)
			{
				instance.Input.RemoveAllTouchEndEvent();
			}
			if (this.actionFinishedHarvest != null)
			{
				this.actionFinishedHarvest();
				this.actionFinishedHarvest = null;
			}
		}
	}

	public override bool IsTutorialFacility()
	{
		return this.IsDummyFacility;
	}

	public void SetCampaignPlate(bool isDisplay)
	{
		if (null != this.campaignPlate && this.campaignPlate.activeSelf != isDisplay)
		{
			this.campaignPlate.SetActive(isDisplay);
		}
	}

	private enum CampaignState
	{
		NONE,
		END,
		START,
		CHANGE
	}

	private sealed class MeatIconAnimation
	{
		private UISprite icon;

		private FarmObject_MeatFarm.Timer timer;

		private int width;

		private int height;

		private int start;

		private int end;

		public MeatIconAnimation(UISprite icon)
		{
			this.icon = icon;
			this.timer = new FarmObject_MeatFarm.Timer();
			this.width = icon.width;
			this.height = icon.height;
		}

		public bool isEnd
		{
			get
			{
				return this.timer.isEnd;
			}
		}

		public void Set(float time, int start, int end)
		{
			this.timer.Set(time, null);
			this.start = start;
			this.end = end;
		}

		public void Update(float dt)
		{
			if (this.timer.isEnd)
			{
				return;
			}
			this.timer.Update(dt);
			float num = Mathf.Sin(3.14159274f * this.timer.rate);
			this.icon.width = this.width + (int)((float)this.width * 0.5f * num);
			this.icon.height = this.height + (int)((float)this.height * 0.5f * num);
			GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
			playerInfo.meatNum = (this.start + (int)((float)(this.end - this.start) * this.timer.rate)).ToString();
			GUIPlayerStatus.RefreshParams_S(false);
		}
	}

	private sealed class LerpPosition
	{
		private GameObject target;

		private FarmObject_MeatFarm.Timer timer;

		private Vector3 end = Vector3.zero;

		public LerpPosition(GameObject target)
		{
			this.target = target;
			this.timer = new FarmObject_MeatFarm.Timer();
		}

		public bool isEnd
		{
			get
			{
				return this.timer.isEnd;
			}
		}

		public void Set(float time, Vector3 start, Vector3 end, Action callback)
		{
			this.timer.Set(time, callback);
			this.end = end;
		}

		public void Update(float dt)
		{
			if (this.timer.isEnd)
			{
				return;
			}
			float num = Vector3.Distance(this.target.transform.localPosition, this.end);
			if (num < 25f)
			{
				this.timer.Update(1f);
			}
			else
			{
				this.timer.Update(dt);
			}
			this.target.transform.localPosition = Vector3.Lerp(this.target.transform.localPosition, this.end, this.timer.rate * this.timer.rate);
		}
	}

	private sealed class LerpScale
	{
		private GameObject target;

		private FarmObject_MeatFarm.Timer timer;

		private Vector3 start = Vector3.zero;

		private Vector3 end = Vector3.zero;

		public LerpScale(GameObject target)
		{
			this.target = target;
			this.timer = new FarmObject_MeatFarm.Timer();
		}

		public bool isEnd
		{
			get
			{
				return this.timer.isEnd;
			}
		}

		public void Set(float time, Vector3 start, Vector3 end, Action callback)
		{
			this.timer.Set(time, callback);
			this.start = start;
			this.end = end;
		}

		public void Update(float dt)
		{
			if (this.timer.isEnd)
			{
				return;
			}
			this.timer.Update(dt);
			this.target.transform.localScale = Vector3.Lerp(this.start, this.end, this.timer.rate * this.timer.rate);
		}
	}

	private sealed class Timer
	{
		private float time;

		private float maxTime;

		private Action callback;

		public float rate
		{
			get
			{
				return this.time / this.maxTime;
			}
		}

		public bool isEnd
		{
			get
			{
				return this.time >= this.maxTime;
			}
		}

		public void Set(float time, Action callback)
		{
			this.time = 0f;
			this.maxTime = time;
			this.callback = callback;
		}

		public void Update(float dt)
		{
			if (this.isEnd)
			{
				return;
			}
			if ((this.time += dt) >= this.maxTime)
			{
				this.time = this.maxTime;
				if (this.callback != null)
				{
					this.callback();
				}
			}
		}
	}
}
