using Facility;
using FarmData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class FarmRoot : MonoBehaviour
{
	[SerializeField]
	private GameObject childGrid;

	[SerializeField]
	private GameObject childScenery;

	[SerializeField]
	private GameObject childCamera;

	[SerializeField]
	private FarmDigimonManager childDigimon;

	[NonSerialized]
	public FarmUI farmUI;

	[SerializeField]
	private FarmColosseum farmColosseum;

	private FarmSettingMark settingMark;

	private FarmSelectMark selectMark;

	private List<EffectAnimatorObserver> buildCompleteEffects = new List<EffectAnimatorObserver>();

	[NonSerialized]
	public FarmRoot.FarmControlMode farmMode;

	[NonSerialized]
	public bool isEdit;

	private static FarmRoot instance;

	private FarmVisitFace farmVisitFace;

	public FarmField Field
	{
		get
		{
			return this.childGrid.GetComponent<FarmField>();
		}
	}

	public FarmScenery Scenery
	{
		get
		{
			return this.childScenery.GetComponent<FarmScenery>();
		}
	}

	public FarmObjectSetting SettingObject
	{
		get
		{
			return this.childScenery.GetComponent<FarmObjectSetting>();
		}
	}

	public FarmObjectSelect SelectObject
	{
		get
		{
			return this.childScenery.GetComponent<FarmObjectSelect>();
		}
	}

	public FarmObjectEdit EditObject
	{
		get
		{
			return this.childScenery.GetComponent<FarmObjectEdit>();
		}
	}

	public Camera Camera
	{
		get
		{
			return this.childCamera.GetComponent<Camera>();
		}
	}

	public InputControll Input
	{
		get
		{
			return this.childCamera.GetComponent<InputControll>();
		}
	}

	public FarmSettingMark SettingMark
	{
		get
		{
			return this.settingMark;
		}
	}

	public FarmSelectMark SelectMark
	{
		get
		{
			return this.selectMark;
		}
	}

	public FarmDigimonManager DigimonManager
	{
		get
		{
			return this.childDigimon;
		}
	}

	public static FarmRoot Instance
	{
		get
		{
			return FarmRoot.instance;
		}
	}

	public bool IsVisitFriendFarm
	{
		get
		{
			return this.visitFriendData != null;
		}
	}

	public GameWebAPI.FriendList visitFriendData { get; private set; }

	private void Awake()
	{
		FarmRoot.instance = this;
		string path = "Farm/Builds/SettingMark/SettingMark";
		GameObject gameObject = AssetDataMng.Instance().LoadObject(path, null, true) as GameObject;
		if (null == gameObject)
		{
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		gameObject2.transform.parent = base.transform;
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localEulerAngles = Vector3.zero;
		this.settingMark = gameObject2.GetComponent<FarmSettingMark>();
		path = "Farm/Builds/SelectMark/SelectMark";
		gameObject = (AssetDataMng.Instance().LoadObject(path, null, true) as GameObject);
		if (null == gameObject)
		{
			return;
		}
		gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		gameObject2.transform.parent = base.transform;
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localEulerAngles = Vector3.zero;
		this.selectMark = gameObject2.GetComponent<FarmSelectMark>();
		gameObject2.SetActive(false);
	}

	private void OnDestroy()
	{
		if (Singleton<UserDataMng>.IsInstance())
		{
			Singleton<UserDataMng>.Instance.DestroyUserFacilityData();
		}
		for (int i = 0; i < this.buildCompleteEffects.Count; i++)
		{
			if (null != this.buildCompleteEffects[i] && null != this.buildCompleteEffects[i].gameObject)
			{
				UnityEngine.Object.Destroy(this.buildCompleteEffects[i].gameObject);
			}
		}
		this.buildCompleteEffects.Clear();
		ServerDateTime.isUpdateServerDateTime = false;
	}

	private void Start()
	{
		this.LoadFacilityData();
		this.LoadFieldData();
	}

	private void LoadFacilityData()
	{
		string path = "Farm/Params/FacilityData";
		FarmDataManager.FacilityInfo = (AssetDataMng.Instance().LoadObject(path, null, true) as FarmFacilityData);
		path = "Farm/Animations/FacilityAnimationData";
		FarmDataManager.FacilityAnimationData = (AssetDataMng.Instance().LoadObject(path, null, true) as FarmFacilityAnimationData);
		path = "Farm/FacilityAnimation";
		FarmDataManager.FacilityAnimator = (AssetDataMng.Instance().LoadObject(path, null, true) as RuntimeAnimatorController);
	}

	private void LoadFieldData()
	{
		string path = "Farm/Fields/farm_01/FieldData_01";
		FarmFieldData fieldData = AssetDataMng.Instance().LoadObject(path, null, true) as FarmFieldData;
		this.Field.SetFieldData(fieldData);
	}

	public IEnumerator Initialize(FarmUI ui)
	{
		this.farmUI = ui;
		this.childDigimon.CreateDigimonGameObject();
		FarmScenery scenery = this.Scenery;
		yield return base.StartCoroutine(scenery.InitializeFarmObjectParallelRead());
		FarmSceneryCache.ClearCache();
		FarmSceneryCache.SetCacheAction(scenery);
		yield break;
	}

	public void ResetSettingMark()
	{
		this.settingMark.SetParent(base.gameObject);
		this.settingMark.InactiveColor();
	}

	public void ResetSelectMark()
	{
		this.selectMark.SetParent(base.gameObject);
		this.selectMark.gameObject.SetActive(false);
	}

	public void ClearSettingFarmObject()
	{
		FarmObjectSelect selectObject = this.SelectObject;
		this.SelectObject.ClearSelectState();
		if (null != this.SettingObject.farmObject)
		{
			this.Scenery.CancelSetting();
			selectObject.EnabledTouchedEvent(true);
			if (this.farmMode == FarmRoot.FarmControlMode.NORMAL)
			{
				FacilityConfirmation componentInChildren = Singleton<GUIMain>.Instance.GetComponentInChildren<FacilityConfirmation>();
				componentInChildren.DeleteObject();
			}
		}
		if (this.farmMode == FarmRoot.FarmControlMode.EDIT)
		{
			this.EditObject.CancelEdit();
		}
	}

	public EffectAnimatorObserver GetBuildCompleteEffect(Transform parentObject)
	{
		EffectAnimatorObserver effectAnimatorObserver = this.buildCompleteEffects.FirstOrDefault((EffectAnimatorObserver x) => x.IsStoped());
		if (null == effectAnimatorObserver)
		{
			GameObject gameObject = AssetDataMng.Instance().LoadObject("Cutscenes/NewFX3", null, true) as GameObject;
			if (null == gameObject)
			{
				global::Debug.LogError("NOT FOUND : Cutscenes/NewFX3");
			}
			else
			{
				effectAnimatorObserver = UnityEngine.Object.Instantiate<GameObject>(gameObject).GetComponent<EffectAnimatorObserver>();
				effectAnimatorObserver.transform.parent = base.transform;
				effectAnimatorObserver.transform.localScale = Vector3.one;
				effectAnimatorObserver.transform.position = parentObject.position;
				this.buildCompleteEffects.Add(effectAnimatorObserver);
				Resources.UnloadUnusedAssets();
			}
		}
		else
		{
			effectAnimatorObserver.transform.position = parentObject.position;
		}
		return effectAnimatorObserver;
	}

	public void StartColosseumOpenAnimation(Action callback)
	{
		this.farmColosseum.StartOpenAnimation(callback);
	}

	public void SetActiveNotTouchObject(bool active)
	{
		this.farmUI.SetActiveConstructionDetail(active);
		this.farmUI.SetActiveSignalMeatHarvest(active);
		this.farmColosseum.EnableTouch = active;
	}

	public void ShowFriendFarm(GameWebAPI.FriendList friendData, Action onFriendProfile, Action onFriendList, Action onBackFarm, Action callback = null)
	{
		if (this.farmVisitFace == null)
		{
			this.farmVisitFace = FarmVisitFace.Create();
		}
		this.farmVisitFace.friendUserName = friendData.userData.nickname;
		this.farmVisitFace.onFriendProfile = onFriendProfile;
		this.farmVisitFace.onFriendList = onFriendList;
		this.farmVisitFace.onBackFarm = onBackFarm;
		if (this.visitFriendData != null && this.visitFriendData.userData.userId == friendData.userData.userId)
		{
			this.ChangeFriendFarmMode(true, callback);
		}
		else
		{
			this.visitFriendData = new GameWebAPI.FriendList();
			this.visitFriendData.monsterData = new GameWebAPI.FriendList.MonsterData();
			this.visitFriendData.monsterData.monsterId = friendData.monsterData.monsterId;
			this.visitFriendData.userData = new GameWebAPI.FriendList.UserData();
			this.visitFriendData.userData.userId = friendData.userData.userId;
			this.visitFriendData.userData.nickname = friendData.userData.nickname;
			this.visitFriendData.userData.description = friendData.userData.description;
			this.visitFriendData.userData.loginTime = friendData.userData.loginTime;
			this.visitFriendData.userData.loginTimeSort = friendData.userData.loginTimeSort;
			APIRequestTask task = Singleton<UserDataMng>.Instance.RequestUserFacilityData(int.Parse(this.visitFriendData.userData.userId), true);
			Action onSuccess = delegate()
			{
				FarmRoot.Instance.SelectObject.ResetSelectedFarmObject();
				FarmRoot.Instance.farmUI.DestroyALLUI();
				FarmRoot.Instance.Scenery.DestroyAllFarmObject();
				FarmRoot.Instance.Scenery.InitializeFarmObject();
				FarmRoot.Instance.DigimonManager.CreateFriendDigimonGameObject(Singleton<UserDataMng>.Instance.monsterIdsInFarm, delegate
				{
					this.ChangeFriendFarmMode(true, callback);
				});
			};
			base.StartCoroutine(task.Run(onSuccess, null, null));
		}
	}

	public void ChangeFriendFarmMode(bool isFriendFarmMode, Action callback = null)
	{
		if (FarmRoot.Instance.Camera.enabled != isFriendFarmMode)
		{
			GUIFace.instance.gameObject.SetActive(!isFriendFarmMode);
			GUIFaceIndicator.instance.gameObject.SetActive(!isFriendFarmMode);
			PartsMenu.instance.gameObject.SetActive(!isFriendFarmMode);
			FarmRoot.Instance.DigimonManager.SetFriendFarmMode(isFriendFarmMode);
			if (!isFriendFarmMode)
			{
				FarmCameraControlForCMD.Off();
				GUIManager.ShowBarrier();
			}
			else
			{
				FarmCameraControlForCMD.On();
				GUIManager.HideBarrier();
			}
		}
		if (FarmObject_DigiGarden.Instance != null && isFriendFarmMode)
		{
			FarmObject_DigiGarden.Instance.DisbledEvolveParticle();
			FarmObject_DigiGarden.Instance.SetGrowthPlate(false);
		}
		GUIBase gui = GUIManager.GetGUI("UIHome");
		if (null != gui)
		{
			GUIScreenHome component = gui.GetComponent<GUIScreenHome>();
			if (null != component)
			{
				component.CloseAllCampaignFacilityIcon();
				component.ShowCampaignFacilityIcon();
			}
		}
		if (callback != null)
		{
			callback();
		}
	}

	public void HideFriendFarm(Action callback = null)
	{
		if (this.farmVisitFace != null)
		{
			this.farmVisitFace.Destroy();
		}
		if (!this.IsVisitFriendFarm)
		{
			if (callback != null)
			{
				callback();
			}
			return;
		}
		APIRequestTask task = Singleton<UserDataMng>.Instance.RequestUserFacilityData(0, true);
		Action onSuccess = delegate()
		{
			FarmRoot.Instance.SelectObject.ResetSelectedFarmObject();
			FarmRoot.Instance.farmUI.DestroyALLUI();
			FarmRoot.Instance.Scenery.DestroyAllFarmObject();
			FarmRoot.Instance.Scenery.InitializeFarmObject();
			FarmRoot.Instance.DigimonManager.RefreshDigimonGameObject(false, delegate
			{
				this.ChangeFriendFarmMode(false, callback);
				this.visitFriendData = null;
				this.ClearSettingFarmObject();
			});
		};
		base.StartCoroutine(task.Run(onSuccess, null, null));
	}

	public void ResetSetteingFence()
	{
		List<FarmObject> list = this.Scenery.farmObjects.Where((FarmObject x) => x.transform.localPosition.y < 8000f).ToList<FarmObject>();
		int[,] array = new int[this.Field.GetField().fieldHorizontal, this.Field.GetField().fieldVertical];
		List<UserFacility> list2 = new List<UserFacility>();
		for (int k = 0; k < list.Count; k++)
		{
			if (list[k].facilityID == 11)
			{
				FarmGrid.GridPosition gridPosition = this.Field.Grid.GetGridPosition(list[k].GetBaseGridPosition3D());
				UserFacility item = new UserFacility
				{
					userFacilityId = list[k].userFacilityID,
					facilityId = list[k].facilityID,
					positionX = gridPosition.x,
					positionY = gridPosition.y,
					level = 0,
					completeTime = string.Empty
				};
				list2.Add(item);
				array[gridPosition.x, gridPosition.y] = 1;
			}
		}
		int i;
		for (i = 0; i < this.Field.GetField().fieldVertical; i++)
		{
			int j;
			for (j = 0; j < this.Field.GetField().fieldHorizontal; j++)
			{
				if (array[j, i] > 0)
				{
					UserFacility fence = list2.First((UserFacility x) => x.positionX == j && x.positionY == i);
					FarmObject farmObject = list.First((FarmObject x) => x.userFacilityID == fence.userFacilityId);
					FarmObject_Fence farmObject_Fence = (FarmObject_Fence)farmObject;
					bool flag = i - 1 >= 0 && array[j, i - 1] > 0;
					bool flag2 = j + 1 < this.Field.GetField().fieldHorizontal && array[j + 1, i] > 0;
					bool flag3 = i + 1 < this.Field.GetField().fieldVertical && array[j, i + 1] > 0;
					bool flag4 = j - 1 >= 0 && array[j - 1, i] > 0;
					int num = 0;
					if (flag && flag2 && flag3 && flag4)
					{
						farmObject_Fence.ChangeFenceType(FarmObject_Fence.Type.Mesh03);
					}
					else if ((flag && flag3 && (flag2 || flag4)) || (flag2 && flag4 && (flag || flag3)))
					{
						if (flag && flag3 && flag2)
						{
							num = 270;
						}
						else if (flag && flag3 && flag4)
						{
							num = 90;
						}
						else if (flag2 && flag4 && flag)
						{
							num = 180;
						}
						else if (flag2 && flag4 && flag3)
						{
							num = 360;
						}
						farmObject_Fence.ChangeFenceType(FarmObject_Fence.Type.Mesh04);
					}
					else if ((flag && (flag2 || flag4)) || (flag3 && (flag2 || flag4)))
					{
						if (flag && flag2)
						{
							num = 90;
						}
						else if (flag && flag4)
						{
							num = 360;
						}
						else if (flag3 && flag2)
						{
							num = 180;
						}
						else if (flag3 && flag4)
						{
							num = 270;
						}
						farmObject_Fence.ChangeFenceType(FarmObject_Fence.Type.Mesh02);
					}
					else if (flag || flag3)
					{
						num = 90;
						farmObject_Fence.ChangeFenceType(FarmObject_Fence.Type.Mesh01);
					}
					else
					{
						farmObject_Fence.ChangeFenceType(FarmObject_Fence.Type.Mesh01);
					}
					farmObject_Fence.transform.localRotation = Quaternion.Euler(0f, (float)num, 0f);
				}
			}
		}
	}

	public enum FarmControlMode
	{
		NORMAL,
		EDIT
	}
}
