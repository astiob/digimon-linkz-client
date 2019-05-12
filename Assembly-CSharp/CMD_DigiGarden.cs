using Cutscene;
using Master;
using Monster;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_DigiGarden : CMD
{
	public static CMD_DigiGarden instance;

	public List<GameObject> goMN_LIST;

	[SerializeField]
	private GameObject goSelectPanel;

	private GUISelectPanelDigiGarden csSelectPanel;

	private Action finishedActionCutScene;

	private readonly Vector3 eggPos_1 = new Vector3(-1.12f, 0f, -1.03f);

	private readonly Vector3 eggPos_2 = new Vector3(1.07f, 0f, 0.67f);

	private readonly Vector3 eggPos_3 = new Vector3(-0.82f, 0f, 2.18f);

	private readonly string[] callMeyhodsOnEggClick = new string[]
	{
		"OnClickEgg_1",
		"OnClickEgg_2",
		"OnClickEgg_3",
		"OnClickEgg_Dummy"
	};

	[SerializeField]
	private UITexture modelUiTex;

	[SerializeField]
	private UITexture backgroundUiTex;

	[SerializeField]
	private UITexture strokeUiTex;

	[SerializeField]
	private GameObject headerObj;

	[SerializeField]
	private GameObject scrollPanel;

	[SerializeField]
	private GameObject listButton;

	[SerializeField]
	private UILabel displayModeButtonLabel;

	[SerializeField]
	private UILabel cautionTextLabel;

	[SerializeField]
	private List<AnimationClip> eggAnimClipList = new List<AnimationClip>();

	[SerializeField]
	private List<AnimationClip> eggLoopAnimClipList = new List<AnimationClip>();

	[SerializeField]
	private List<PicturebookDetailController> eggControllerList;

	[SerializeField]
	private GameObject particle;

	[SerializeField]
	private GardenShadow digiShadow;

	[SerializeField]
	private Vector3 digiCameraPos = new Vector3(2.5f, 1.3f, 5.3f);

	private Camera backgroundCamera;

	private RenderTexture backgroundRenderTex;

	private List<DigimonActionInGarden> charaActList = new List<DigimonActionInGarden>();

	private CMD_DigiGarden.DISPLAY_MODE displayMode;

	private Vector3 listScreenHeaderPos = Vector3.zero;

	private Vector3 listScreenUiPos = Vector3.zero;

	private Vector3 listScreenModelTexPos = Vector3.zero;

	private Vector3 listScreenListButtonPos = Vector3.zero;

	private Vector3 fullScreenHeaderPos = Vector3.zero;

	private Vector3 fullScreenUiPos = Vector3.zero;

	private Vector3 fullScreenModelTexPos = Vector3.zero;

	private Vector3 fullScreenListButtonPos = Vector3.zero;

	private bool isStrokeEnd;

	private bool isLock;

	private int growNeedStone;

	private CameraClearFlags preCameraClearFlag = CameraClearFlags.Nothing;

	private readonly float uiAnimationTime = 0.18f;

	private Action<int> movedAct;

	public bool IsOfflineModeFlag;

	public string OfflineTimeUntilEvolution = "23:59:00";

	public int OfflineGrowNeedStone = 24;

	protected override void Awake()
	{
		base.Awake();
		CMD_DigiGarden.instance = this;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.SetTutorialAnyTime("anytime_second_tutorial_digigarden");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.PartsTitle.SetTitle(StringMaster.GetString("GardenTitle"));
		this.ResetPushNotice();
		this.SetCommonUI();
		this.InitMonsterList();
		this.fullScreenModelTexPos = (this.listScreenModelTexPos = this.modelUiTex.transform.localPosition);
		this.fullScreenModelTexPos.y = -800f;
		this.fadeInTime = 0f;
		base.Show(f, sizeX, sizeY, aT);
		RestrictionInput.EndLoad();
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.isLock)
		{
			return;
		}
		if (null != FarmObject_DigiGarden.Instance)
		{
			FarmObject_DigiGarden.Instance.SetAutoActiveCanEvolveParticle();
		}
		if (this.backgroundCamera != null)
		{
			this.backgroundCamera.clearFlags = this.preCameraClearFlag;
		}
		this.DestroyRender3DRT();
		this.DestroyBackground();
		base.ClosePanel(animation);
		if (this.csSelectPanel != null)
		{
			this.csSelectPanel.FadeOutAllListParts(null, false);
			this.csSelectPanel.SetHideScrollBarAllWays(true);
		}
	}

	private void OnApplicationQuit()
	{
		this.SetPushNotice();
	}

	protected override void OnDestroy()
	{
		this.DestroyRender3DRT();
		this.DestroyBackground();
		CMD_DigiGarden.instance = null;
		base.OnDestroy();
	}

	private void SetCommonUI()
	{
		this.csSelectPanel = this.goSelectPanel.GetComponent<GUISelectPanelDigiGarden>();
		this.csSelectPanel.selectParts = this.goMN_LIST[0];
		for (int i = 0; i < this.goMN_LIST.Count; i++)
		{
			this.goMN_LIST[i].SetActive(false);
		}
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -240f;
		listWindowViewRect.xMax = 240f;
		listWindowViewRect.yMin = -280f - GUIMain.VerticalSpaceSize;
		listWindowViewRect.yMax = 230f + GUIMain.VerticalSpaceSize;
		this.csSelectPanel.ListWindowViewRect = listWindowViewRect;
		this.SetBackground();
	}

	public void InitMonsterList()
	{
		List<MonsterData> list = MonsterDataMng.Instance().GetMonsterDataList();
		list = MonsterFilter.Filter(list, MonsterFilterType.GROWING_IN_GARDEN);
		MonsterDataMng.Instance().SortMDList(list);
		PushNotice.Instance.SyncGardenPushNoticeData(list);
		if (list.Count == 0)
		{
			this.cautionTextLabel.gameObject.SetActive(true);
			this.cautionTextLabel.text = StringMaster.GetString("Garden-01");
		}
		else if (this.cautionTextLabel.gameObject.activeSelf)
		{
			this.cautionTextLabel.gameObject.SetActive(false);
		}
		if (null != FarmObject_DigiGarden.Instance)
		{
			FarmObject_DigiGarden.Instance.SetAutoActiveCanEvolveParticle();
		}
		this.csSelectPanel.initLocation = true;
		this.goMN_LIST[0].SetActive(true);
		this.csSelectPanel.AllBuild(list, new Action<CMD, string, string>(this.OnBornExec), new Action<MonsterData>(this.OnPushEvolutionButton));
		this.goMN_LIST[0].SetActive(false);
		this.modelUiTex.gameObject.SetActive(list.Count != 0);
		int num = 0;
		foreach (MonsterData monsterData in list)
		{
			if (monsterData.userMonster.eggFlg == "1")
			{
				num++;
			}
		}
		foreach (PicturebookDetailController picturebookDetailController in this.eggControllerList)
		{
			picturebookDetailController.CallMethodOnClick = "OnClickEgg_Dummy";
		}
		int num2 = 0;
		int num3 = 0;
		foreach (MonsterData monsterData2 in list)
		{
			if (monsterData2.userMonster.IsEgg())
			{
				string eggModelId = MonsterObject.GetEggModelId(monsterData2.userMonster.monsterEvolutionRouteId);
				CommonRender3DRT commonRender3DRT = this.CreateRender3DRT(true, eggModelId, this.modelUiTex);
				this.SetRender3Dcamera(true, commonRender3DRT, false);
				DigimonActionInGarden digimonActionInGarden = this.AttachActionScript(commonRender3DRT.gameObject, true);
				this.charaActList.Add(digimonActionInGarden);
				this.eggControllerList[num2].CallMethodOnClick = this.callMeyhodsOnEggClick[num3];
				if (num != 1)
				{
					if (num != 2)
					{
						if (num == 3)
						{
							num2++;
							if (num2 != 1)
							{
								if (num2 != 2)
								{
									if (num2 == 3)
									{
										digimonActionInGarden.SetPosition(this.eggPos_3);
									}
								}
								else
								{
									digimonActionInGarden.SetPosition(this.eggPos_2);
								}
							}
							else
							{
								digimonActionInGarden.SetPosition(this.eggPos_1);
							}
						}
					}
					else
					{
						num2++;
						if (num2 != 1)
						{
							if (num2 == 2)
							{
								digimonActionInGarden.SetPosition(this.eggPos_2);
							}
						}
						else
						{
							digimonActionInGarden.SetPosition(this.eggPos_1);
						}
					}
				}
				else
				{
					digimonActionInGarden.SetPosition(this.eggPos_1);
				}
				digimonActionInGarden.SetDefaultAnimation(this.eggLoopAnimClipList[num3]);
			}
			else
			{
				CommonRender3DRT commonRender3DRT2 = this.CreateRender3DRT(false, monsterData2.GetMonsterMaster().Group.modelId, this.modelUiTex);
				this.SetRender3Dcamera(false, commonRender3DRT2, false);
				DigimonActionInGarden digimonActionInGarden2 = this.AttachActionScript(commonRender3DRT2.gameObject, false);
				this.charaActList.Add(digimonActionInGarden2);
				digimonActionInGarden2.RandomPosition();
				digimonActionInGarden2.WalkAction();
			}
			num3++;
		}
	}

	private void OnBornExec(CMD confirmPopup, string userMonsterId, string eggModelId)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		confirmPopup.SetCloseAction(delegate(int noop)
		{
			this.RequestBorn(userMonsterId, eggModelId);
		});
	}

	private void RequestBorn(string userMonsterId, string eggModelId)
	{
		GameWebAPI.RequestMN_MonsterHatching requestMN_MonsterHatching = new GameWebAPI.RequestMN_MonsterHatching();
		requestMN_MonsterHatching.SetSendData = delegate(GameWebAPI.MN_Req_Born param)
		{
			param.userMonsterId = userMonsterId;
		};
		requestMN_MonsterHatching.OnReceived = delegate(GameWebAPI.RespDataMN_BornExec response)
		{
			ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonster);
		};
		GameWebAPI.RequestMN_MonsterHatching request = requestMN_MonsterHatching;
		base.StartCoroutine(request.Run(delegate()
		{
			this.EndBornSuccess(userMonsterId, eggModelId);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void EndBornSuccess(string userMonsterId, string eggModelId)
	{
		RestrictionInput.EndLoad();
		this.Lock();
		base.StartCoroutine(this.StrokingEgg(userMonsterId, eggModelId, delegate(MonsterData afterMonsterData)
		{
			this.DestroyRender3DRT();
			CMD_CharacterDetailed.DataChg = afterMonsterData;
			GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed", null);
			ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
			this.InitMonsterList();
			this.UnLock();
		}));
	}

	private void OnPushEvolutionButton(MonsterData monsterData)
	{
		if (!this.IsOfflineModeFlag)
		{
			DateTime d = DateTime.Parse(monsterData.userMonster.growEndDate);
			TimeSpan timeSpan = d - ServerDateTime.Now;
			if (timeSpan.TotalSeconds <= 0.0)
			{
				this.growNeedStone = 0;
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(null, "CMD_Confirm", null) as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("EvolutionTitle");
				cmd_Confirm.Info = StringMaster.GetString("EvolutionConfirmInfo");
				cmd_Confirm.SetActionYesButton(delegate(CMD confirmPopup)
				{
					this.OnPushEvolutionConfirmYesButton(confirmPopup, monsterData);
				});
			}
			else
			{
				base.StartCoroutine(this.GrowExecInfoAPI(monsterData, timeSpan));
			}
		}
		else
		{
			this.OpenConfirmShortenTime(this.OfflineGrowNeedStone, TimeSpan.Parse(this.OfflineTimeUntilEvolution), delegate(CMD confirmPopup)
			{
				this.OfflineGrow_Step2(monsterData);
			});
		}
	}

	private IEnumerator GrowExecInfoAPI(MonsterData monsterData, TimeSpan timeSpan)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
		GameWebAPI.RespDataUS_GetGardenInfo gardenInfoList = null;
		GameWebAPI.RequestUS_GetGardenInfo request = new GameWebAPI.RequestUS_GetGardenInfo
		{
			OnReceived = delegate(GameWebAPI.RespDataUS_GetGardenInfo response)
			{
				gardenInfoList = response;
			}
		};
		return request.Run(delegate()
		{
			int shortenTimeValue = this.GetShortenTimeValue(gardenInfoList, monsterData.userMonster.userMonsterId);
			this.growNeedStone = this.GetCostEvolution(shortenTimeValue, timeSpan);
			this.OpenConfirmShortenTime(this.growNeedStone, timeSpan, delegate(CMD confirmPopup)
			{
				this.OnPushEvolutionConfirmYesButton(confirmPopup, monsterData);
				confirmPopup.ClosePanel(true);
			});
			RestrictionInput.EndLoad();
		}, null, null);
	}

	private int GetShortenTimeValue(GameWebAPI.RespDataUS_GetGardenInfo gardenInfoList, string userMonsterId)
	{
		int result = 0;
		foreach (GameWebAPI.GardenInfo.MonsterInfo monsterInfo in gardenInfoList.gardenInfo.monster)
		{
			if (userMonsterId == monsterInfo.userMonsterId.ToString())
			{
				if (MonsterGrowStepData.IsChild1Scope(monsterInfo.growStep))
				{
					result = gardenInfoList.gardenInfo.time1;
				}
				else
				{
					result = gardenInfoList.gardenInfo.time2;
				}
			}
		}
		return result;
	}

	private int GetCostEvolution(int shortenTime, TimeSpan timeSpan)
	{
		int num = 0;
		if (timeSpan.Hours > 0)
		{
			if (timeSpan.Hours % shortenTime == 0)
			{
				num = timeSpan.Hours / shortenTime;
			}
			else
			{
				num = timeSpan.Hours / shortenTime + 1;
			}
		}
		if (timeSpan.Minutes > 0 || timeSpan.Seconds > 0)
		{
			num++;
		}
		return num;
	}

	private void OpenConfirmShortenTime(int cost, TimeSpan timeSpan, Action<CMD> pushYesButton)
	{
		string arg = string.Empty;
		if (timeSpan.Hours > 0)
		{
			arg = string.Format(StringMaster.GetString("SystemTimeHM"), timeSpan.Hours.ToString(), timeSpan.Minutes.ToString());
		}
		else if (timeSpan.Minutes > 0)
		{
			arg = string.Format(StringMaster.GetString("SystemTimeMS"), timeSpan.Minutes.ToString(), timeSpan.Seconds.ToString());
		}
		else
		{
			arg = string.Format(StringMaster.GetString("SystemTimeS"), timeSpan.Seconds.ToString());
		}
		CMD_ChangePOP_STONE popup = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP_STONE", null) as CMD_ChangePOP_STONE;
		popup.Title = StringMaster.GetString("EvolutionTitle");
		popup.OnPushedYesAction = delegate()
		{
			pushYesButton(popup);
		};
		popup.Info = string.Format(StringMaster.GetString("Garden-11"), cost.ToString(), arg);
		popup.SetDigistone(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point, cost);
	}

	private void OnPushEvolutionConfirmYesButton(CMD confirmPopup, MonsterData monsterData)
	{
		if (this.growNeedStone <= DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			confirmPopup.SetCloseAction(delegate(int noop)
			{
				this.ExecGrow(monsterData);
			});
		}
		else
		{
			confirmPopup.SetCloseAction(delegate(int noop)
			{
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(null, "CMD_Confirm", null) as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("EvolutionTitle");
				cmd_Confirm.Info = StringMaster.GetString("GashaShortage");
				cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonGoShop");
				cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
				cmd_Confirm.SetActionYesButton(new Action<CMD>(this.OnPushConfirmShopButton));
			});
		}
	}

	private void ExecGrow(MonsterData monsterData)
	{
		DateTime d = DateTime.Parse(monsterData.userMonster.growEndDate);
		double timeSpan = (double)((int)(d - ServerDateTime.Now).TotalSeconds);
		GameWebAPI.RequestMN_MonsterEvolutionInGarden requestMN_MonsterEvolutionInGarden = new GameWebAPI.RequestMN_MonsterEvolutionInGarden();
		requestMN_MonsterEvolutionInGarden.SetSendData = delegate(GameWebAPI.MN_Req_Grow param)
		{
			param.userMonsterId = monsterData.userMonster.userMonsterId;
			param.shorteningFlg = ((timeSpan <= 0.0) ? 0 : 1);
			param.stone = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
		};
		requestMN_MonsterEvolutionInGarden.OnReceived = delegate(GameWebAPI.RespDataMN_GrowExec response)
		{
			ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonster);
		};
		GameWebAPI.RequestMN_MonsterEvolutionInGarden request = requestMN_MonsterEvolutionInGarden;
		MonsterClientMaster monsterMaster = monsterData.GetMonsterMaster();
		string beforeMonsterModelId = monsterMaster.Group.modelId;
		string beforeMonsterGrowStep = monsterMaster.Group.growStep;
		base.StartCoroutine(request.Run(delegate()
		{
			this.EndGrowSuccess(monsterData.GetMonster().userMonsterId, beforeMonsterModelId, beforeMonsterGrowStep);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void OnPushConfirmShopButton(CMD confirmPopup)
	{
		confirmPopup.SetCloseAction(delegate(int noop)
		{
			GUIMain.ShowCommonDialog(null, "CMD_Shop", null);
		});
	}

	private void EndGrowSuccess(string userMonsterId, string beforeModelId, string beforeGrowStep)
	{
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		MonsterData userMonster = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(userMonsterId);
		MonsterClientMaster monsterMaster = userMonster.GetMonsterMaster();
		CutsceneDataEvolution cutsceneDataEvolution = new CutsceneDataEvolution();
		cutsceneDataEvolution.path = "Cutscenes/Evolution";
		cutsceneDataEvolution.beforeModelId = beforeModelId;
		cutsceneDataEvolution.beforeGrowStep = beforeGrowStep;
		cutsceneDataEvolution.afterModelId = monsterMaster.Group.modelId;
		cutsceneDataEvolution.afterGrowStep = monsterMaster.Group.growStep;
		cutsceneDataEvolution.endCallback = delegate()
		{
			FarmCameraControlForCMD.On();
			CutSceneMain.FadeReqCutSceneEnd();
		};
		CutsceneDataEvolution cutsceneData = cutsceneDataEvolution;
		Loading.Invisible();
		this.CallEvolutionCutScene(userMonster, cutsceneData);
	}

	private void SetBackground()
	{
		GameObject gameObject = new GameObject("BackgroundCamera");
		FarmRoot farmRoot = FarmRoot.Instance;
		gameObject.transform.SetParent(farmRoot.transform);
		Vector3 vector = default(Vector3);
		foreach (FarmObject farmObject in farmRoot.Scenery.farmObjects)
		{
			if (farmObject.facilityID == 5)
			{
				vector = farmObject.transform.localPosition;
				break;
			}
		}
		gameObject.transform.localPosition = new Vector3(vector.x - 3.635f, vector.y + 1.2f, vector.z - 1.21f);
		gameObject.transform.Rotate(new Vector3(15f, 125f));
		this.backgroundCamera = gameObject.AddComponent<Camera>();
		this.preCameraClearFlag = this.backgroundCamera.clearFlags;
		this.backgroundCamera.clearFlags = CameraClearFlags.Color;
		this.backgroundRenderTex = new RenderTexture(this.backgroundUiTex.width, this.backgroundUiTex.height, 16);
		this.backgroundCamera.targetTexture = this.backgroundRenderTex;
		this.backgroundUiTex.mainTexture = this.backgroundRenderTex;
		this.backgroundCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Cutscene"));
	}

	private void SetRender3Dcamera(bool isEgg, CommonRender3DRT render3DRT, bool isStroke)
	{
		Camera componentInChildren = render3DRT.GetComponentInChildren<Camera>();
		componentInChildren.fieldOfView = 30f;
		Transform transform = componentInChildren.transform;
		if (isStroke)
		{
			render3DRT.transform.localPosition = new Vector3(0f, 10000f);
			transform.localPosition += new Vector3(0f, 0.75f, transform.localPosition.z);
		}
		else
		{
			componentInChildren.useOcclusionCulling = true;
			Transform transform2 = this.backgroundCamera.transform;
			if (!isEgg)
			{
				transform.localPosition = this.digiCameraPos;
				transform.localEulerAngles = new Vector3(transform2.localEulerAngles.x, -155f);
			}
			else
			{
				transform.localPosition = new Vector3(transform.localPosition.x, transform2.localPosition.y, transform.localPosition.z * 2f);
				transform.localEulerAngles = new Vector3(transform2.localEulerAngles.x, 0f);
			}
		}
	}

	private CommonRender3DRT CreateRender3DRT(bool isEgg, string modelId, UITexture displayUiTex)
	{
		GameObject gameObject = GUIManager.LoadCommonGUI("Render3D/Render3DRT", null);
		gameObject.name = "DigiGarden3dStudio_" + this.charaActList.Count.ToString();
		CommonRender3DRT component = gameObject.GetComponent<CommonRender3DRT>();
		string filePath = MonsterObject.GetFilePath(modelId);
		if (!isEgg)
		{
			component.LoadChara(filePath, 0f, 4000f, 0f, 0f, true);
		}
		else
		{
			component.LoadEgg(filePath, 0f, 4000f, 0f);
		}
		gameObject = UnityEngine.Object.Instantiate<GameObject>(this.digiShadow.gameObject);
		GardenShadow component2 = gameObject.GetComponent<GardenShadow>();
		component2.Initialize(component.gameObject);
		RenderTexture mainTexture = component.SetRenderTarget(displayUiTex.width, displayUiTex.height, 16);
		displayUiTex.mainTexture = mainTexture;
		return component;
	}

	private DigimonActionInGarden AttachActionScript(GameObject TargetParentObj, bool isEgg)
	{
		DigimonActionInGarden digimonActionInGarden;
		if (!isEgg)
		{
			CharacterParams componentInChildren = TargetParentObj.transform.GetComponentInChildren<CharacterParams>();
			digimonActionInGarden = componentInChildren.gameObject.AddComponent<DigimonActionInGarden>();
			digimonActionInGarden.Initialize(componentInChildren);
		}
		else
		{
			Animation componentInChildren2 = TargetParentObj.transform.GetComponentInChildren<Animation>();
			digimonActionInGarden = componentInChildren2.gameObject.AddComponent<DigimonActionInGarden>();
			digimonActionInGarden.Initialize(componentInChildren2);
		}
		return digimonActionInGarden;
	}

	private void ChangeDisplayModeToList()
	{
		this.MoveTo(this.headerObj, this.listScreenHeaderPos, this.uiAnimationTime, iTween.EaseType.linear, null);
		this.MoveTo(this.scrollPanel, this.listScreenUiPos, this.uiAnimationTime, iTween.EaseType.linear, null);
		this.MoveTo(this.listButton, this.listScreenListButtonPos, this.uiAnimationTime, iTween.EaseType.linear, null);
		this.displayModeButtonLabel.text = StringMaster.GetString("CharaDetailsFullScreen");
		this.displayMode = CMD_DigiGarden.DISPLAY_MODE.ListMode;
	}

	private void ChangeDisplayModeToFullScreen()
	{
		UIPanel uipanel = GUIMain.GetUIPanel();
		Vector2 windowSize = uipanel.GetWindowSize();
		if (this.fullScreenHeaderPos == Vector3.zero)
		{
			this.fullScreenHeaderPos = (this.listScreenHeaderPos = this.headerObj.transform.localPosition);
			this.fullScreenHeaderPos.y = windowSize.y;
		}
		if (this.fullScreenUiPos == Vector3.zero)
		{
			this.fullScreenUiPos = (this.listScreenUiPos = this.scrollPanel.transform.localPosition);
			this.fullScreenUiPos.x = windowSize.x;
		}
		if (this.fullScreenListButtonPos == Vector3.zero)
		{
			this.fullScreenListButtonPos = (this.listScreenListButtonPos = this.listButton.transform.localPosition);
			this.fullScreenListButtonPos.x = windowSize.x;
		}
		this.MoveTo(this.headerObj, this.fullScreenHeaderPos, this.uiAnimationTime, iTween.EaseType.linear, null);
		this.MoveTo(this.scrollPanel, this.fullScreenUiPos, this.uiAnimationTime, iTween.EaseType.linear, null);
		this.MoveTo(this.listButton, this.fullScreenListButtonPos, this.uiAnimationTime, iTween.EaseType.linear, null);
		this.displayModeButtonLabel.text = StringMaster.GetString("SystemButtonReturn");
		this.displayMode = CMD_DigiGarden.DISPLAY_MODE.FullScreenMode;
	}

	private void MoveTo(GameObject go, Vector3 vP, float time, iTween.EaseType type = iTween.EaseType.linear, Action<int> onComplete = null)
	{
		this.movedAct = onComplete;
		Hashtable hashtable = new Hashtable();
		hashtable.Add("x", vP.x);
		hashtable.Add("y", vP.y);
		hashtable.Add("time", time);
		hashtable.Add("easetype", type);
		hashtable.Add("isLocal", true);
		if (onComplete != null)
		{
			hashtable.Add("oncomplete", "MoveEnd");
			hashtable.Add("oncompleteparams", 0);
		}
		iTween.MoveTo(go, hashtable);
	}

	private void MoveEnd(int id)
	{
		if (this.movedAct != null)
		{
			this.movedAct(id);
		}
	}

	private void ChangeDisplayMode()
	{
		CMD_DigiGarden.DISPLAY_MODE display_MODE = this.displayMode;
		if (display_MODE != CMD_DigiGarden.DISPLAY_MODE.ListMode)
		{
			if (display_MODE != CMD_DigiGarden.DISPLAY_MODE.FullScreenMode)
			{
				this.ChangeDisplayModeToFullScreen();
			}
			else
			{
				this.ChangeDisplayModeToList();
			}
		}
		else
		{
			this.ChangeDisplayModeToFullScreen();
		}
	}

	private IEnumerator StrokingEgg(string userMonsterId, string eggModelId, Action<MonsterData> OnStroked)
	{
		PartsMenu.instance.gameObject.SetActive(false);
		foreach (PicturebookDetailController picturebookDetailController in this.eggControllerList)
		{
			picturebookDetailController.gameObject.SetActive(false);
		}
		this.MoveTo(this.modelUiTex.gameObject, this.fullScreenModelTexPos, 0.18f, iTween.EaseType.linear, null);
		this.ChangeDisplayModeToFullScreen();
		this.displayModeButtonLabel.transform.parent.gameObject.SetActive(false);
		this.strokeUiTex.gameObject.SetActive(true);
		this.isStrokeEnd = false;
		CommonRender3DRT render3DRT = this.CreateRender3DRT(true, eggModelId, this.strokeUiTex);
		this.SetRender3Dcamera(true, render3DRT, true);
		DigimonActionInGarden digimonAct = this.AttachActionScript(render3DRT.gameObject, true);
		digimonAct.transform.localPosition = Vector3.zero;
		digimonAct.transform.localScale = new Vector3(2f, 2f, 2f);
		while (!this.isStrokeEnd)
		{
			yield return null;
		}
		this.particle.SetActive(true);
		digimonAct.gameObject.SetActive(false);
		yield return new WaitForSeconds(1.2f);
		UnityEngine.Object.Destroy(digimonAct.transform.parent.gameObject);
		MonsterData afterMonsterData = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(userMonsterId);
		render3DRT = this.CreateRender3DRT(false, afterMonsterData.GetMonsterMaster().Group.modelId, this.strokeUiTex);
		this.SetRender3Dcamera(false, render3DRT, true);
		DigimonActionInGarden newDigimonAct = this.AttachActionScript(render3DRT.gameObject, false);
		newDigimonAct.transform.localPosition = Vector3.zero;
		newDigimonAct.transform.localScale = new Vector3(2f, 2f, 2f);
		yield return new WaitForSeconds(2f);
		this.strokeUiTex.gameObject.SetActive(false);
		newDigimonAct.transform.parent.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(newDigimonAct.transform.parent.gameObject);
		this.ChangeDisplayModeToList();
		this.MoveTo(this.modelUiTex.gameObject, this.listScreenModelTexPos, 0.18f, iTween.EaseType.linear, null);
		this.displayModeButtonLabel.transform.parent.gameObject.SetActive(true);
		this.isStrokeEnd = false;
		this.particle.SetActive(false);
		OnStroked(afterMonsterData);
		foreach (PicturebookDetailController picturebookDetailController2 in this.eggControllerList)
		{
			picturebookDetailController2.gameObject.SetActive(true);
		}
		PartsMenu.instance.gameObject.SetActive(true);
		yield break;
	}

	private void DestroyBackground()
	{
		if (null != this.backgroundUiTex)
		{
			this.backgroundUiTex.gameObject.SetActive(false);
		}
		if (null != this.backgroundCamera)
		{
			UnityEngine.Object.Destroy(this.backgroundCamera.gameObject);
		}
		this.backgroundRenderTex = null;
	}

	public void DestroyRender3DRT()
	{
		if (this.modelUiTex != null)
		{
			this.modelUiTex.gameObject.SetActive(false);
		}
		int i = 0;
		while (i < this.charaActList.Count)
		{
			if (this.charaActList[i] == null)
			{
				i++;
			}
			else
			{
				if (this.charaActList[i].transform.parent != null)
				{
					UnityEngine.Object.Destroy(this.charaActList[i].transform.parent.gameObject);
				}
				this.charaActList.RemoveAt(i);
			}
		}
	}

	private void EggAction(int Num)
	{
		if (!this.charaActList[Num].IsEgg)
		{
			return;
		}
		this.charaActList[Num].PlayAnimationClip(this.eggAnimClipList[UnityEngine.Random.Range(0, this.eggAnimClipList.Count)]);
	}

	private void SetPushNotice()
	{
		PushNotice.Instance.SetGardenPushNotice();
	}

	private void ResetPushNotice()
	{
		PushNotice.Instance.ClearGardenPushNotice();
	}

	private bool Lock()
	{
		if (this.isLock)
		{
			return false;
		}
		this.isLock = true;
		return true;
	}

	private void UnLock()
	{
		this.isLock = false;
	}

	public void OnClickDisplayModeButton()
	{
		this.ChangeDisplayMode();
	}

	public void OnClickListButton()
	{
		CMD_FarewellListRun.Mode = CMD_FarewellListRun.MODE.GARDEN;
		CMD_FarewellListRun cmd_FarewellListRun = GUIMain.ShowCommonDialog(null, "CMD_GardenList", null) as CMD_FarewellListRun;
		cmd_FarewellListRun.SetOfflineMode(this.IsOfflineModeFlag);
	}

	public void OnStrokeEnd()
	{
		this.isStrokeEnd = true;
	}

	public void OnClickEgg_1()
	{
		this.EggAction(0);
	}

	public void OnClickEgg_2()
	{
		this.EggAction(1);
	}

	public void OnClickEgg_3()
	{
		this.EggAction(2);
	}

	public void OnClickEgg_Dummy()
	{
	}

	private void OfflineGrow_Step2(MonsterData monsterData)
	{
		MonsterClientMaster monsterMaster = monsterData.GetMonsterMaster();
		string modelId = monsterMaster.Group.modelId;
		string growStep = monsterMaster.Group.growStep;
		string userMonsterId = monsterData.GetMonster().userMonsterId;
		string monsterEvolutionRouteId = monsterData.GetMonster().monsterEvolutionRouteId;
		foreach (GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM monsterEvolutionRouteM2 in MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM)
		{
			if (monsterEvolutionRouteId == monsterEvolutionRouteM2.monsterEvolutionRouteId)
			{
				monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(monsterEvolutionRouteM2.growthMonsterId);
				break;
			}
		}
		MonsterDataMng.Instance().GetMonsterDataList()[0].userMonster.growEndDate = string.Empty;
		monsterData.userMonster.ex = "0";
		monsterData.userMonster.hpAbilityFlg = "0";
		monsterData.userMonster.attackAbilityFlg = "0";
		monsterData.userMonster.defenseAbilityFlg = "0";
		monsterData.userMonster.spAttackAbilityFlg = "0";
		monsterData.userMonster.spDefenseAbilityFlg = "0";
		monsterData.userMonster.speedAbilityFlg = "0";
		monsterData.userMonster.friendship = "0";
		StatusValue statusValue = MonsterStatusData.GetStatusValue(monsterData.userMonster.monsterId, monsterData.userMonster.level);
		statusValue.luck = 1;
		monsterData.SetStatus(statusValue);
		monsterData.userMonster.userMonsterId = userMonsterId;
		ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(monsterData.userMonster);
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		monsterData = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(userMonsterId);
		monsterMaster = monsterData.GetMonsterMaster();
		CutsceneDataEvolution cutsceneDataEvolution = new CutsceneDataEvolution();
		cutsceneDataEvolution.path = "Cutscenes/Evolution";
		cutsceneDataEvolution.beforeModelId = modelId;
		cutsceneDataEvolution.beforeGrowStep = growStep;
		cutsceneDataEvolution.afterModelId = monsterMaster.Group.modelId;
		cutsceneDataEvolution.afterGrowStep = monsterMaster.Group.growStep;
		cutsceneDataEvolution.endCallback = delegate()
		{
			FarmCameraControlForCMD.On();
			CutSceneMain.FadeReqCutSceneEnd();
		};
		CutsceneDataEvolution cutsceneData = cutsceneDataEvolution;
		this.CallEvolutionCutScene(monsterData, cutsceneData);
	}

	private void CallEvolutionCutScene(MonsterData monsterData, CutsceneDataEvolution cutsceneData)
	{
		CMD_CharacterDetailed detailedWindow = null;
		CutSceneMain.FadeReqCutScene(cutsceneData, delegate()
		{
			FarmCameraControlForCMD.Off();
			detailedWindow = CMD_CharacterDetailed.CreateWindow(monsterData);
			this.DestroyRender3DRT();
			if (!this.IsOfflineModeFlag)
			{
				this.InitMonsterList();
				DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point -= this.growNeedStone;
			}
		}, delegate()
		{
			detailedWindow.StartAnimation();
			RestrictionInput.EndLoad();
			if (this.finishedActionCutScene != null)
			{
				this.finishedActionCutScene();
				this.finishedActionCutScene = null;
			}
		}, 0.5f, 0.5f);
	}

	public void SetFinishedActionCutScene(Action action)
	{
		this.finishedActionCutScene = action;
	}

	public enum DISPLAY_MODE
	{
		ListMode,
		FullScreenMode
	}
}
