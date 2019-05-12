using Facility;
using FarmData;
using System;
using System.Collections;
using UnityEngine;

public class FarmObject : MonoBehaviour
{
	[Range(1f, 7f)]
	public int sizeX = 1;

	[Range(1f, 7f)]
	public int sizeY = 1;

	[SerializeField]
	protected int baseGridPositionX;

	[SerializeField]
	protected int baseGridPositionY;

	[SerializeField]
	protected GameObject floorObject;

	[SerializeField]
	protected string buttonPrefabPath;

	[NonSerialized]
	public bool isPlaceable;

	[NonSerialized]
	public int facilityID;

	[NonSerialized]
	public int userFacilityID;

	private Vector3 backupPosition;

	private GameObject constructionModel;

	private bool enabledDisplayedInFront;

	private bool isFacilityBuildCompleteRequesting;

	protected virtual void Awake()
	{
		base.gameObject.tag = "Farm.Facility";
	}

	protected virtual void Start()
	{
		Animator component = base.GetComponent<Animator>();
		if (null != component && null == component.runtimeAnimatorController)
		{
			component.runtimeAnimatorController = FarmDataManager.FacilityAnimator;
			component.enabled = true;
		}
		if (!this.IsConstruction())
		{
			FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(this.facilityID);
			if (ConstValue.BUILDING_TYPE_FACILITY == facilityMaster.type)
			{
				this.SetNamePlate(true);
			}
		}
	}

	public virtual void SetSettingMark(FarmField farmField, FarmSettingMark mark)
	{
		mark.SetParent(base.gameObject);
		Vector2 gridSize = new Vector2(farmField.gridHorizontal, farmField.gridVertical);
		mark.SetSize(this.sizeX, this.sizeY, gridSize);
		if (null != this.floorObject)
		{
			this.floorObject.SetActive(false);
		}
	}

	public virtual void DisplayFloorObject()
	{
		if (null != this.floorObject)
		{
			this.floorObject.SetActive(true);
		}
	}

	public void SetSelectMark(FarmField farmField, FarmSelectMark mark)
	{
		mark.gameObject.SetActive(true);
		mark.SetParent(base.gameObject);
		Vector2 gridSize = new Vector2(farmField.gridHorizontal, farmField.gridVertical);
		mark.SetSize(this.sizeX, this.sizeY, gridSize);
	}

	public void SetPosition(float gridHorizontal, float gridVertical, Vector3 gridPosition)
	{
		float num = 0.5f * (float)this.sizeX * gridHorizontal;
		float num2 = 0.5f * (float)this.sizeY * gridVertical;
		Vector3 localPosition = base.transform.localPosition;
		localPosition.x = gridPosition.x + num - gridHorizontal * (float)this.baseGridPositionX - 0.5f * gridHorizontal;
		localPosition.y = 0f;
		localPosition.z = gridPosition.z - num2 + gridVertical * (float)this.baseGridPositionY + 0.5f * gridVertical;
		base.transform.localPosition = localPosition;
	}

	public Vector3 GetBaseGridPosition3D()
	{
		Vector3 pos = this.MovingLocalPosition();
		return this.GetGridPosition3D(pos);
	}

	public Vector3 GetBackupGridPosition3D()
	{
		return this.GetGridPosition3D(this.backupPosition);
	}

	private Vector3 GetGridPosition3D(Vector3 pos)
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		float num = field.gridHorizontal * (float)this.baseGridPositionX + 0.5f * field.gridHorizontal;
		float num2 = field.gridVertical * (float)this.baseGridPositionY + 0.5f * field.gridVertical;
		float num3 = 0.5f * (float)this.sizeX * field.gridHorizontal;
		float num4 = 0.5f * (float)this.sizeY * field.gridVertical;
		Vector3 result = pos;
		result.x -= num3 - num;
		result.y = 0f;
		result.z += num4 - num2;
		return result;
	}

	public void BackupPosition()
	{
		this.backupPosition = base.transform.localPosition;
	}

	public void ResetPosition()
	{
		base.transform.localPosition = this.backupPosition;
	}

	public void DisplayedInFront(bool enable)
	{
		Vector3 b = FarmUtility.GetDistanceToGround() * 0.5f;
		if (enable)
		{
			base.transform.localPosition -= b;
		}
		else
		{
			base.transform.localPosition += b;
		}
		this.enabledDisplayedInFront = enable;
	}

	private Vector3 MovingLocalPosition()
	{
		Vector3 vector = base.transform.localPosition;
		if (this.enabledDisplayedInFront)
		{
			vector += FarmUtility.GetDistanceToGround() * 0.5f;
		}
		return vector;
	}

	public int[] GetGridIndexs(FarmField.Field field, int gridIndex)
	{
		int[] array = new int[this.sizeX * this.sizeY];
		int num = gridIndex % field.fieldHorizontal - this.baseGridPositionX;
		int num2 = gridIndex / field.fieldHorizontal - this.baseGridPositionY;
		if (0 <= num && 0 <= num2)
		{
			int num3 = 0;
			for (int i = 0; i < this.sizeY; i++)
			{
				int num4 = (i + num2) * field.fieldHorizontal;
				for (int j = 0; j < this.sizeX; j++)
				{
					array[num3] = num4 + num + j;
					num3++;
				}
			}
		}
		return array;
	}

	public FarmGrid.GridPosition AdjustGridPosition(int gridX, int gridY)
	{
		return new FarmGrid.GridPosition
		{
			x = gridX - this.baseGridPositionX,
			y = gridY - this.baseGridPositionY
		};
	}

	public bool IsInvalidGrid(FarmField farmField, int[] gridIndexs)
	{
		bool result = false;
		FarmField.Field field = farmField.GetField();
		foreach (int num in gridIndexs)
		{
			if (0 > num || field.grids.Count <= num || field.grids[num].invalid)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool IsPutGrid(FarmField.Field field, int[] gridIndexs)
	{
		bool result = false;
		foreach (int index in gridIndexs)
		{
			if (field.grids[index].put)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public void SetPlaceable(bool placeable)
	{
		this.isPlaceable = placeable;
		FarmSettingMark componentInChildren = base.GetComponentInChildren<FarmSettingMark>();
		componentInChildren.SetColor(placeable);
	}

	public void SetMarkColor(FarmField.Field field, int gridIndex)
	{
		int num = gridIndex % field.fieldHorizontal - this.baseGridPositionX;
		int num2 = gridIndex / field.fieldHorizontal - this.baseGridPositionY;
		this.isPlaceable = true;
		if (0 > num || 0 > num2)
		{
			this.isPlaceable = false;
		}
		else
		{
			for (int i = 0; i < this.sizeY; i++)
			{
				int num3 = (i + num2) * field.fieldHorizontal;
				for (int j = 0; j < this.sizeX; j++)
				{
					int num4 = num3 + num + j;
					if (num + j >= field.fieldHorizontal || field.grids.Count <= num4 || field.grids[num4].invalid || field.grids[num4].put)
					{
						this.isPlaceable = false;
						break;
					}
				}
				if (!this.isPlaceable)
				{
					break;
				}
			}
		}
		FarmSettingMark componentInChildren = base.GetComponentInChildren<FarmSettingMark>();
		componentInChildren.SetColor(this.isPlaceable);
	}

	public IEnumerator OnTouchedEffect(bool isSelected, Action<bool> result)
	{
		bool isEvent = false;
		FarmRoot farmRoot = FarmRoot.Instance;
		if (farmRoot.farmMode == FarmRoot.FarmControlMode.EDIT)
		{
			yield break;
		}
		bool isMovingObject = FarmObjectSetting.SettingMode.MOVE == farmRoot.SettingObject.settingMode;
		if (this.IsConstruction() && !isMovingObject && !this.IsTutorialFacility() && !farmRoot.IsVisitFriendFarm)
		{
			yield return base.StartCoroutine(this.ServerRequestFacilityBuildComplete(delegate(bool e)
			{
				isEvent = e;
			}));
		}
		else
		{
			isEvent = this.StartSelectAnimation(isSelected);
		}
		if (result != null)
		{
			result(isEvent);
		}
		yield break;
	}

	public IEnumerator ServerRequestFacilityBuildComplete(Action<bool> completed)
	{
		bool isEventBuild = false;
		if (!this.isFacilityBuildCompleteRequesting)
		{
			UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.userFacilityID);
			double restSecondes = FarmUtility.RestSeconds(userFacility.completeTime);
			if (0.0 >= restSecondes)
			{
				this.isFacilityBuildCompleteRequesting = true;
				RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
				bool isSuccess = false;
				RequestFA_FacilityBuildComplete request = new RequestFA_FacilityBuildComplete
				{
					SetSendData = delegate(FacilityBuildComplete param)
					{
						param.userFacilityId = this.userFacilityID;
					},
					OnReceived = delegate(FacilityBuildCompleteResult response)
					{
						isSuccess = (response.result == 1);
					}
				};
				yield return base.StartCoroutine(request.Run(delegate()
				{
					RestrictionInput.EndLoad();
					if (isSuccess)
					{
						userFacility.completeTime = null;
						userFacility.level++;
						this.BuildCompleteEffect();
						this.ClearConstruction();
						isEventBuild = true;
					}
				}, null, null));
				this.isFacilityBuildCompleteRequesting = false;
			}
		}
		if (completed != null)
		{
			completed(isEventBuild);
		}
		yield break;
	}

	protected virtual bool StartSelectAnimation(bool selected)
	{
		if (!selected)
		{
			base.StartCoroutine(this.PlayAnimation(FacilityAnimationID.SELECT));
			SoundMng.Instance().TryPlaySE("SEInternal/Farm/se_203", 0f, false, true, null, -1);
		}
		return false;
	}

	public void SetConstruction(GameObject constructionModel)
	{
		this.SetNamePlate(false);
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			child.gameObject.SetActive(false);
		}
		this.constructionModel = constructionModel.gameObject;
		constructionModel.transform.parent = base.transform;
		constructionModel.transform.localPosition = Vector3.zero;
		constructionModel.transform.localScale = Vector3.one;
		constructionModel.transform.localEulerAngles = Vector3.zero;
	}

	public void SetConstructionDetail()
	{
		FarmRoot instance = FarmRoot.Instance;
		instance.farmUI.CreateConstructionDetail(this);
	}

	public bool IsConstruction()
	{
		return null != this.constructionModel;
	}

	public void BuildCompleteEffect()
	{
		SoundMng.Instance().TryPlaySE("SEInternal/Farm/se_202", 0f, false, true, null, -1);
		FarmRoot instance = FarmRoot.Instance;
		EffectAnimatorObserver buildCompleteEffect = instance.GetBuildCompleteEffect(base.transform);
		if (null != buildCompleteEffect)
		{
			EffectAnimatorEventTime component = buildCompleteEffect.GetComponent<EffectAnimatorEventTime>();
			component.SetEvent(0, new Action(this.BuildEffect));
			buildCompleteEffect.Play();
		}
	}

	public virtual void BuildEffect()
	{
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.userFacilityID);
		if (userFacility != null && string.IsNullOrEmpty(userFacility.completeTime))
		{
			if (null != this.constructionModel)
			{
				UnityEngine.Object.Destroy(this.constructionModel.gameObject);
				this.constructionModel = null;
			}
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				child.gameObject.SetActive(true);
			}
			FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(this.facilityID);
			if (ConstValue.BUILDING_TYPE_FACILITY == facilityMaster.type)
			{
				this.SetNamePlate(true);
			}
		}
	}

	public void ClearConstruction()
	{
		FarmRoot instance = FarmRoot.Instance;
		instance.farmUI.DeleteConstructionDetail(this.userFacilityID);
		instance.farmUI.UpdateFacilityButton(this);
		this.BuildComplete();
	}

	public virtual void BuildComplete()
	{
	}

	public bool IsStore()
	{
		return base.transform.localPosition.y >= 8000f;
	}

	public IEnumerator CreatePop(FarmObjectPop.PopType type, Transform cameraTransform, float adjustY)
	{
		GameObject resource = AssetDataMng.Instance().LoadObject("Farm/Builds/Pop/PopLocator", null, true) as GameObject;
		yield return null;
		GameObject pop = UnityEngine.Object.Instantiate<GameObject>(resource);
		Transform t = pop.transform;
		Vector3 originalPos = t.localPosition;
		t.parent = base.transform;
		t.localPosition = originalPos;
		resource = null;
		Resources.UnloadUnusedAssets();
		yield return null;
		FarmObjectPop farmObjectPop = t.GetComponent<FarmObjectPop>();
		farmObjectPop.SetActivePop(type, cameraTransform, adjustY);
		yield break;
	}

	public void DeletePop()
	{
		FarmObjectPop componentInChildren = base.GetComponentInChildren<FarmObjectPop>();
		if (null != componentInChildren)
		{
			componentInChildren.DestroyPop();
		}
	}

	public virtual bool IsTutorialFacility()
	{
		return false;
	}

	public void SetNamePlate(bool display)
	{
		FarmUI farmUI = FarmRoot.Instance.farmUI;
		if (null != farmUI && this.userFacilityID != 0)
		{
			if (display)
			{
				farmUI.CreateFacilityNamePlate(this);
			}
			else
			{
				farmUI.DeleteFacilityNamePlate(this.userFacilityID);
			}
		}
	}

	public string GetButtonPrefabName()
	{
		return "Farm/" + this.buttonPrefabPath;
	}

	public virtual void OnResumeFromCache()
	{
		this.SetNamePlate(true);
	}
}
