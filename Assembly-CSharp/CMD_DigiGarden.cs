using Master;
using Monster;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_DigiGarden : CMD
{
	public static CMD_DigiGarden instance;

	public List<GameObject> goMN_LIST;

	public GameObject goSelectPanel;

	private GUISelectPanelDigiGarden csSelectPanel;

	private Action finishedActionCutScene;

	private MonsterData md_bk;

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
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.PartsTitle.SetTitle(StringMaster.GetString("GardenTitle"));
		this.ResetPushNotice();
		this.SetCommonUI();
		this.InitMonsterList(true);
		this.fullScreenModelTexPos = (this.listScreenModelTexPos = this.modelUiTex.transform.localPosition);
		this.fullScreenModelTexPos.y = -800f;
		base.Show(f, sizeX, sizeY, aT);
		RestrictionInput.EndLoad();
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.isLock)
		{
			return;
		}
		if (FarmObject_DigiGarden.Instance != null)
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

	public void InitMonsterList(bool initLoc = true)
	{
		List<MonsterData> list = MonsterDataMng.Instance().GetMonsterDataList();
		list = MonsterDataMng.Instance().SelectMonsterDataList(list, MonsterFilterType.GROWING_IN_GARDEN);
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
		if (FarmObject_DigiGarden.Instance != null)
		{
			FarmObject_DigiGarden.Instance.SetGrowthPlate(list.Count > 0);
		}
		this.csSelectPanel.initLocation = initLoc;
		this.goMN_LIST[0].SetActive(true);
		this.csSelectPanel.AllBuild(list);
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
			DigimonActionInGarden digimonActionInGarden = this.Show3dModel(monsterData2, this.modelUiTex, false);
			if (monsterData2.userMonster.eggFlg == "1")
			{
				this.eggControllerList[num2].CallMethodOnClick = this.callMeyhodsOnEggClick[num3];
				switch (num)
				{
				case 1:
					digimonActionInGarden.SetPosition(this.eggPos_1);
					break;
				case 2:
				{
					num2++;
					int num4 = num2;
					if (num4 != 1)
					{
						if (num4 == 2)
						{
							digimonActionInGarden.SetPosition(this.eggPos_2);
						}
					}
					else
					{
						digimonActionInGarden.SetPosition(this.eggPos_1);
					}
					break;
				}
				case 3:
					num2++;
					switch (num2)
					{
					case 1:
						digimonActionInGarden.SetPosition(this.eggPos_1);
						break;
					case 2:
						digimonActionInGarden.SetPosition(this.eggPos_2);
						break;
					case 3:
						digimonActionInGarden.SetPosition(this.eggPos_3);
						break;
					}
					break;
				}
				digimonActionInGarden.SetDefaultAnimation(this.eggLoopAnimClipList[num3]);
			}
			else
			{
				digimonActionInGarden.RandomPosition();
				digimonActionInGarden.WalkAction();
			}
			num3++;
		}
	}

	public void BornExec(MonsterData md)
	{
		if (!this.Lock())
		{
			return;
		}
		this.md_bk = new MonsterData(md);
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseBornExec), "CMD_Confirm") as CMD_Confirm;
		cmd_Confirm.Title = StringMaster.GetString("Garden-05");
		cmd_Confirm.Info = StringMaster.GetString("Garden-06");
	}

	private void OnCloseBornExec(int idx)
	{
		if (idx == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			GameWebAPI.RequestMN_MonsterHatching requestMN_MonsterHatching = new GameWebAPI.RequestMN_MonsterHatching();
			requestMN_MonsterHatching.SetSendData = delegate(GameWebAPI.MN_Req_Born param)
			{
				param.userMonsterId = int.Parse(this.md_bk.userMonster.userMonsterId);
			};
			requestMN_MonsterHatching.OnReceived = delegate(GameWebAPI.RespDataMN_BornExec response)
			{
				ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonster);
			};
			GameWebAPI.RequestMN_MonsterHatching request = requestMN_MonsterHatching;
			base.StartCoroutine(request.Run(new Action(this.EndBornSuccess), new Action<Exception>(this.EndBornFailed), null));
		}
		else
		{
			this.UnLock();
		}
	}

	private void EndBornSuccess()
	{
		RestrictionInput.EndLoad();
		base.StartCoroutine(this.StrokingEgg(delegate
		{
			CMD_CharacterDetailed.DataChg = this.md_bk;
			GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed");
			this.DestroyRender3DRT();
			ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
			this.InitMonsterList(true);
			this.UnLock();
		}));
	}

	private void EndBornFailed(Exception noop)
	{
		this.UnLock();
	}

	public void GrowExec(MonsterData md)
	{
		if (!this.Lock())
		{
			return;
		}
		if (!this.IsOfflineModeFlag)
		{
			DateTime d = DateTime.Parse(md.userMonster.growEndDate);
			TimeSpan timeSpan = d - ServerDateTime.Now;
			this.md_bk = new MonsterData(md);
			if (timeSpan.TotalSeconds <= 0.0)
			{
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseGrowExec), "CMD_Confirm") as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("EvolutionTitle");
				cmd_Confirm.Info = StringMaster.GetString("EvolutionConfirmInfo");
				this.growNeedStone = 0;
			}
			else
			{
				base.StartCoroutine(this.GrowExecInfoAPI(timeSpan));
			}
		}
		else
		{
			MonsterData monsterDataByUserMonsterLargeID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterLargeID();
			this.OfflineGrow(monsterDataByUserMonsterLargeID);
		}
	}

	private IEnumerator GrowExecInfoAPI(TimeSpan timeSpan)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
		APIRequestTask task = Singleton<UserDataMng>.Instance.RequestGardenInfo(true);
		return task.Run(delegate
		{
			this.GrowExecConfirm(timeSpan);
			RestrictionInput.EndLoad();
		}, null, null);
	}

	private void GrowExecConfirm(TimeSpan timeSpan)
	{
		GameWebAPI.RespDataUS_GetGardenInfo respDataUS_GardenInfo = DataMng.Instance().RespDataUS_GardenInfo;
		string arg = string.Empty;
		this.growNeedStone = 0;
		int num = 0;
		foreach (GameWebAPI.GardenInfo.MonsterInfo monsterInfo in respDataUS_GardenInfo.gardenInfo.monster)
		{
			if (this.md_bk.userMonster.userMonsterId == monsterInfo.userMonsterId.ToString())
			{
				num = ((!MonsterGrowStepData.IsChild1Scope(monsterInfo.growStep)) ? respDataUS_GardenInfo.gardenInfo.time2 : respDataUS_GardenInfo.gardenInfo.time1);
			}
		}
		if (timeSpan.Hours > 0)
		{
			this.growNeedStone = ((timeSpan.Hours % num != 0) ? (timeSpan.Hours / num + 1) : (timeSpan.Hours / num));
			if (timeSpan.Minutes > 0 || timeSpan.Seconds > 0)
			{
				this.growNeedStone++;
			}
			arg = string.Format(StringMaster.GetString("SystemTimeHM"), timeSpan.Hours.ToString(), timeSpan.Minutes.ToString());
		}
		else if (timeSpan.Minutes > 0)
		{
			this.growNeedStone = 1;
			arg = string.Format(StringMaster.GetString("SystemTimeMS"), timeSpan.Minutes.ToString(), timeSpan.Seconds.ToString());
		}
		else
		{
			this.growNeedStone = 1;
			arg = string.Format(StringMaster.GetString("SystemTimeS"), timeSpan.Seconds.ToString());
		}
		CMD_ChangePOP_STONE cmd_ChangePOP_STONE = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP_STONE") as CMD_ChangePOP_STONE;
		cmd_ChangePOP_STONE.Title = StringMaster.GetString("EvolutionTitle");
		cmd_ChangePOP_STONE.OnPushedYesAction = new Action(this.OnCloseGrowExecYes);
		cmd_ChangePOP_STONE.OnPushedNoAction = new Action(this.OnCloseGrowExecNo);
		cmd_ChangePOP_STONE.Info = string.Format(StringMaster.GetString("Garden-11"), this.growNeedStone.ToString(), arg);
		cmd_ChangePOP_STONE.SetDigistone(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point, this.growNeedStone);
		cmd_ChangePOP_STONE.BtnTextYes = StringMaster.GetString("SystemButtonYes");
	}

	private void OnCloseGrowExecYes()
	{
		this.OnCloseGrowExec(0);
	}

	private void OnCloseGrowExecNo()
	{
		this.OnCloseGrowExec(1);
	}

	private void OnCloseGrowExec(int idx)
	{
		if (idx == 0)
		{
			if (this.growNeedStone <= DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point)
			{
				this.ExecGrow();
			}
			else
			{
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseConfirmShop), "CMD_Confirm") as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("EvolutionTitle");
				cmd_Confirm.Info = StringMaster.GetString("GashaShortage");
				cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonGoShop");
				cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
			}
		}
		else
		{
			this.UnLock();
		}
	}

	private void ExecGrow()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		DateTime d = DateTime.Parse(this.md_bk.userMonster.growEndDate);
		double timeSpan = (double)((int)(d - ServerDateTime.Now).TotalSeconds);
		GameWebAPI.RequestMN_MonsterEvolutionInGarden requestMN_MonsterEvolutionInGarden = new GameWebAPI.RequestMN_MonsterEvolutionInGarden();
		requestMN_MonsterEvolutionInGarden.SetSendData = delegate(GameWebAPI.MN_Req_Grow param)
		{
			param.userMonsterId = int.Parse(this.md_bk.userMonster.userMonsterId);
			param.shorteningFlg = ((timeSpan <= 0.0) ? 0 : 1);
			param.stone = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
		};
		requestMN_MonsterEvolutionInGarden.OnReceived = delegate(GameWebAPI.RespDataMN_GrowExec response)
		{
			ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonster);
		};
		GameWebAPI.RequestMN_MonsterEvolutionInGarden request = requestMN_MonsterEvolutionInGarden;
		base.StartCoroutine(request.Run(new Action(this.EndGrowSuccess), new Action<Exception>(this.EndGrowFailed), null));
		CMD_ChangePOP_STONE cmd_ChangePOP_STONE = UnityEngine.Object.FindObjectOfType<CMD_ChangePOP_STONE>();
		if (null != cmd_ChangePOP_STONE)
		{
			cmd_ChangePOP_STONE.ClosePanel(true);
		}
	}

	private void OnCloseConfirmShop(int idx)
	{
		this.UnLock();
		if (idx == 0)
		{
			GUIMain.ShowCommonDialog(null, "CMD_Shop");
		}
		CMD_ChangePOP_STONE cmd_ChangePOP_STONE = UnityEngine.Object.FindObjectOfType<CMD_ChangePOP_STONE>();
		if (null != cmd_ChangePOP_STONE)
		{
			cmd_ChangePOP_STONE.ClosePanel(true);
		}
	}

	private void EndGrowSuccess()
	{
		int beforeMonsterGroupId = int.Parse(this.md_bk.monsterM.monsterGroupId);
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		this.md_bk = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(this.md_bk.userMonster.userMonsterId, false);
		int afterMonsterGroupId = int.Parse(this.md_bk.monsterM.monsterGroupId);
		Loading.Invisible();
		this.CallEvolutionCutScene(this.md_bk, beforeMonsterGroupId, afterMonsterGroupId);
	}

	private void EndGrowFailed(Exception noop)
	{
		RestrictionInput.EndLoad();
		this.UnLock();
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

	private DigimonActionInGarden Show3dModel(MonsterData CharaData, UITexture DisplayUiTex, bool IsUseDirection = false)
	{
		CommonRender3DRT commonRender3DRT = this.CreateRender3DRT(CharaData, DisplayUiTex);
		Camera componentInChildren = commonRender3DRT.GetComponentInChildren<Camera>();
		componentInChildren.fieldOfView = 30f;
		if (IsUseDirection)
		{
			commonRender3DRT.transform.localPosition = new Vector3(0f, 10000f);
			componentInChildren.transform.localPosition += new Vector3(0f, 0.75f, componentInChildren.transform.localPosition.z);
		}
		else
		{
			componentInChildren.useOcclusionCulling = true;
			componentInChildren.transform.localPosition = ((!(CharaData.userMonster.eggFlg != "1")) ? new Vector3(componentInChildren.transform.localPosition.x, this.backgroundCamera.transform.localPosition.y, componentInChildren.transform.localPosition.z * 2f) : this.digiCameraPos);
			componentInChildren.transform.localEulerAngles = new Vector3(this.backgroundCamera.transform.localEulerAngles.x, (!(CharaData.userMonster.eggFlg != "1")) ? 0f : -155f);
		}
		return this.AttachActionScript(commonRender3DRT.gameObject, CharaData.userMonster.eggFlg != "1");
	}

	private CommonRender3DRT CreateRender3DRT(MonsterData CharaData, UITexture DisplayUiTex)
	{
		GameObject gameObject = GUIManager.LoadCommonGUI("Render3D/Render3DRT", null);
		gameObject.name = "DigiGarden3dStudio_" + this.charaActList.Count.ToString();
		CommonRender3DRT component = gameObject.GetComponent<CommonRender3DRT>();
		if (!CharaData.userMonster.IsEgg())
		{
			string monsterCharaPathByMonsterGroupId = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId(CharaData.monsterM.monsterGroupId);
			component.LoadChara(monsterCharaPathByMonsterGroupId, 0f, 4000f, 0f, 0f, true);
		}
		else
		{
			string monsterGroupId = string.Empty;
			foreach (GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM monsterEvolutionRouteM2 in MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM)
			{
				if (monsterEvolutionRouteM2.monsterEvolutionRouteId == CharaData.userMonster.monsterEvolutionRouteId)
				{
					monsterGroupId = monsterEvolutionRouteM2.eggMonsterId;
					break;
				}
			}
			string monsterCharaPathByMonsterGroupId2 = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId(monsterGroupId);
			component.LoadEgg(monsterCharaPathByMonsterGroupId2, 0f, 4000f, 0f);
		}
		gameObject = UnityEngine.Object.Instantiate<GameObject>(this.digiShadow.gameObject);
		GardenShadow component2 = gameObject.GetComponent<GardenShadow>();
		component2.Initialize(component.gameObject);
		RenderTexture mainTexture = component.SetRenderTarget(DisplayUiTex.width, DisplayUiTex.height, 16);
		DisplayUiTex.mainTexture = mainTexture;
		return component;
	}

	private DigimonActionInGarden AttachActionScript(GameObject TargetParentObj, bool IsChildrenHaveCharacterParams = true)
	{
		DigimonActionInGarden digimonActionInGarden;
		if (IsChildrenHaveCharacterParams)
		{
			CharacterParams componentInChildren = TargetParentObj.transform.GetComponentInChildren<CharacterParams>();
			digimonActionInGarden = componentInChildren.gameObject.AddComponent<DigimonActionInGarden>();
			this.charaActList.Add(digimonActionInGarden);
			digimonActionInGarden.Initialize(componentInChildren);
		}
		else
		{
			Animation componentInChildren2 = TargetParentObj.transform.GetComponentInChildren<Animation>();
			digimonActionInGarden = componentInChildren2.gameObject.AddComponent<DigimonActionInGarden>();
			this.charaActList.Add(digimonActionInGarden);
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
		if (this.fullScreenHeaderPos == Vector3.zero)
		{
			this.fullScreenHeaderPos = (this.listScreenHeaderPos = this.headerObj.transform.localPosition);
			this.fullScreenHeaderPos.y = 480f;
		}
		if (this.fullScreenUiPos == Vector3.zero)
		{
			this.fullScreenUiPos = (this.listScreenUiPos = this.scrollPanel.transform.localPosition);
			this.fullScreenUiPos.x = 1000f;
		}
		if (this.fullScreenListButtonPos == Vector3.zero)
		{
			this.fullScreenListButtonPos = (this.listScreenListButtonPos = this.listButton.transform.localPosition);
			this.fullScreenListButtonPos.x = 1000f;
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

	private IEnumerator StrokingEgg(Action OnStroked)
	{
		PartsMenu.instance.gameObject.SetActive(false);
		foreach (PicturebookDetailController obj in this.eggControllerList)
		{
			obj.gameObject.SetActive(false);
		}
		this.MoveTo(this.modelUiTex.gameObject, this.fullScreenModelTexPos, 0.18f, iTween.EaseType.linear, null);
		this.ChangeDisplayModeToFullScreen();
		this.displayModeButtonLabel.transform.parent.gameObject.SetActive(false);
		this.strokeUiTex.gameObject.SetActive(true);
		this.isStrokeEnd = false;
		DigimonActionInGarden digimonAct = this.Show3dModel(this.md_bk, this.strokeUiTex, true);
		digimonAct.transform.localPosition = Vector3.zero;
		digimonAct.transform.localScale = new Vector3(2f, 2f, 2f);
		this.charaActList.Remove(digimonAct);
		while (!this.isStrokeEnd)
		{
			yield return null;
		}
		this.particle.SetActive(true);
		digimonAct.gameObject.SetActive(false);
		yield return new WaitForSeconds(1.2f);
		UnityEngine.Object.Destroy(digimonAct.transform.parent.gameObject);
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		this.md_bk = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(this.md_bk.userMonster.userMonsterId, false);
		DigimonActionInGarden newDigimonAct = this.Show3dModel(this.md_bk, this.strokeUiTex, true);
		newDigimonAct.transform.localPosition = Vector3.zero;
		newDigimonAct.transform.localScale = new Vector3(2f, 2f, 2f);
		this.charaActList.Remove(newDigimonAct);
		yield return new WaitForSeconds(2f);
		this.strokeUiTex.gameObject.SetActive(false);
		newDigimonAct.transform.parent.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(newDigimonAct.transform.parent.gameObject);
		this.ChangeDisplayModeToList();
		this.MoveTo(this.modelUiTex.gameObject, this.listScreenModelTexPos, 0.18f, iTween.EaseType.linear, null);
		this.displayModeButtonLabel.transform.parent.gameObject.SetActive(true);
		this.isStrokeEnd = false;
		this.particle.SetActive(false);
		OnStroked();
		foreach (PicturebookDetailController obj2 in this.eggControllerList)
		{
			obj2.gameObject.SetActive(true);
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
		CMD_FarewellListRun cmd_FarewellListRun = GUIMain.ShowCommonDialog(null, "CMD_GardenList") as CMD_FarewellListRun;
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

	public void OfflineGrow(MonsterData md)
	{
		this.md_bk = new MonsterData(md);
		TimeSpan timeSpan = TimeSpan.Parse(this.OfflineTimeUntilEvolution);
		string arg = string.Empty;
		this.growNeedStone = this.OfflineGrowNeedStone;
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
		CMD_ChangePOP_STONE cmd_ChangePOP_STONE = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP_STONE") as CMD_ChangePOP_STONE;
		cmd_ChangePOP_STONE.Title = StringMaster.GetString("EvolutionTitle");
		cmd_ChangePOP_STONE.OnPushedYesAction = new Action(this.OfflineGrow_Step2);
		cmd_ChangePOP_STONE.Info = string.Format(StringMaster.GetString("Garden-11"), this.growNeedStone.ToString(), arg);
		cmd_ChangePOP_STONE.SetDigistone(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point, this.growNeedStone);
	}

	private void OfflineGrow_Step2()
	{
		int beforeMonsterGroupId = int.Parse(this.md_bk.monsterM.monsterGroupId);
		string userMonsterId = this.md_bk.userMonster.userMonsterId;
		string monsterEvolutionRouteId = this.md_bk.userMonster.monsterEvolutionRouteId;
		foreach (GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM monsterEvolutionRouteM2 in MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM)
		{
			if (monsterEvolutionRouteId == monsterEvolutionRouteM2.monsterEvolutionRouteId)
			{
				this.md_bk = MonsterDataMng.Instance().CreateMonsterDataByMID(monsterEvolutionRouteM2.growthMonsterId);
				break;
			}
		}
		MonsterDataMng.Instance().GetMonsterDataList()[0].userMonster.growEndDate = string.Empty;
		this.md_bk.userMonster.ex = "0";
		this.md_bk.userMonster.hpAbilityFlg = "0";
		this.md_bk.userMonster.attackAbilityFlg = "0";
		this.md_bk.userMonster.defenseAbilityFlg = "0";
		this.md_bk.userMonster.spAttackAbilityFlg = "0";
		this.md_bk.userMonster.spDefenseAbilityFlg = "0";
		this.md_bk.userMonster.speedAbilityFlg = "0";
		this.md_bk.userMonster.friendship = "0";
		StatusValue statusValue = MonsterStatusData.GetStatusValue(this.md_bk.userMonster.monsterId, this.md_bk.userMonster.level);
		statusValue.luck = 1;
		this.md_bk.SetStatus(statusValue);
		this.md_bk.userMonster.userMonsterId = userMonsterId;
		ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(this.md_bk.userMonster);
		MonsterData userMonster = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(userMonsterId);
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		int afterMonsterGroupId = int.Parse(userMonster.monsterM.monsterGroupId);
		this.CallEvolutionCutScene(userMonster, beforeMonsterGroupId, afterMonsterGroupId);
	}

	private void CallEvolutionCutScene(MonsterData monsterData, int beforeMonsterGroupId, int afterMonsterGroupId)
	{
		List<int> umidList = new List<int>
		{
			beforeMonsterGroupId,
			afterMonsterGroupId
		};
		CutSceneMain.FadeReqCutScene("Cutscenes/Evolution", delegate(int index)
		{
			FarmCameraControlForCMD.Off();
			CMD_CharacterDetailed.DataChg = monsterData;
			GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed");
			this.DestroyRender3DRT();
			if (!this.IsOfflineModeFlag)
			{
				this.InitMonsterList(true);
				DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point -= this.growNeedStone;
			}
			this.UnLock();
		}, delegate(int index)
		{
			FarmCameraControlForCMD.On();
			CutSceneMain.FadeReqCutSceneEnd();
		}, delegate(int index)
		{
			PartsUpperCutinController.Instance.PlayAnimator(PartsUpperCutinController.AnimeType.EvolutionComplete, null);
			RestrictionInput.EndLoad();
			if (this.finishedActionCutScene != null)
			{
				this.finishedActionCutScene();
				this.finishedActionCutScene = null;
			}
		}, umidList, null, 2, 1, 0.5f, 0.5f);
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
