using FarmData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using WebAPIRequest;

public class FarmScenery : MonoBehaviour
{
	public List<FarmObject> farmObjects = new List<FarmObject>();

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	private GameObject LoadFarmModel(int facilityID)
	{
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(facilityID);
		if (facilityMaster == null)
		{
			global::Debug.LogError("施設のリソース名が見つからない : 施設ID = " + facilityID);
			return null;
		}
		string text = "Farm/Builds/" + facilityMaster.modelResource;
		if (facilityMaster.levelUpFlg == "1")
		{
			List<UserFacility> userFacilityList = Singleton<UserDataMng>.Instance.GetUserFacilityList();
			FacilityUpgradeM facilityUpgradeMaster;
			if (userFacilityList.Any((UserFacility x) => x.facilityId == facilityID))
			{
				UserFacility userFacility = userFacilityList.Find((UserFacility x) => x.facilityId == facilityID);
				facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(facilityID, ((!string.IsNullOrEmpty(userFacility.completeTime)) ? 1 : 0) + userFacility.level);
			}
			else
			{
				facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(facilityID, 1);
			}
			text = "Farm/Builds/" + facilityUpgradeMaster.modelResource;
		}
		GameObject gameObject = AssetDataMng.Instance().LoadObject(text, null, true) as GameObject;
		if (null == gameObject)
		{
			global::Debug.LogError("施設のモデルが見つからない : " + text);
			return null;
		}
		return UnityEngine.Object.Instantiate<GameObject>(gameObject);
	}

	private FarmObject LoadFarmObject(int facilityID)
	{
		GameObject gameObject = this.LoadFarmModel(facilityID);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localEulerAngles = Vector3.zero;
		return gameObject.GetComponent<FarmObject>();
	}

	private FarmObject DuplicateFarmObject(FarmObject src)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(src.gameObject);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localEulerAngles = Vector3.zero;
		return gameObject.GetComponent<FarmObject>();
	}

	public bool BuildFarmObject(int facilityID)
	{
		FarmObject farmObject = this.FindFarmObjectByFacilityId(facilityID);
		FarmObject farmObject2;
		if (null == farmObject)
		{
			farmObject2 = this.LoadFarmObject(facilityID);
		}
		else
		{
			farmObject2 = this.DuplicateFarmObject(farmObject);
		}
		if (null == farmObject2)
		{
			return false;
		}
		farmObject2.facilityID = facilityID;
		FarmObjectSetting component = base.GetComponent<FarmObjectSetting>();
		bool flag = component.SetFarmObject(farmObject2, this.GetScreenCenterPoint());
		if (flag)
		{
			GUICameraControll component2 = FarmRoot.Instance.Camera.GetComponent<GUICameraControll>();
			if (null != component2)
			{
				Vector3 baseGridPosition3D = component.farmObject.GetBaseGridPosition3D();
				base.StartCoroutine(component2.MoveCameraToLookAtPoint(baseGridPosition3D, 0f));
			}
		}
		return true;
	}

	private Vector3 GetScreenCenterPoint()
	{
		Vector3 distanceToGround = FarmUtility.GetDistanceToGround();
		FarmRoot instance = FarmRoot.Instance;
		Camera camera = instance.Camera;
		return camera.transform.localPosition + distanceToGround;
	}

	public void CancelSetting()
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmObjectSetting component = base.GetComponent<FarmObjectSetting>();
		instance.ResetSettingMark();
		component.farmObject.DisplayFloorObject();
		component.CancelBuild();
		if (instance.farmMode == FarmRoot.FarmControlMode.NORMAL)
		{
			instance.SetActiveNotTouchObject(true);
		}
	}

	public APIRequestTask SaveFarmObjectPosition(Action<int> complated)
	{
		RequestFA_FacilityBuild request = new RequestFA_FacilityBuild
		{
			SetSendData = delegate(FacilityBuild param)
			{
				FarmRoot instance = FarmRoot.Instance;
				FarmField field = instance.Field;
				FarmObjectSetting component = this.GetComponent<FarmObjectSetting>();
				FarmGrid.GridPosition gridPosition = field.Grid.GetGridPosition(component.farmObject.GetBaseGridPosition3D());
				param.facilityId = component.farmObject.facilityID;
				param.positionX = gridPosition.x;
				param.positionY = gridPosition.y;
			},
			OnReceived = delegate(FacilityBuildResult response)
			{
				this.SaveResponseToFacilityBuild(response.userFacilityId);
				if (complated != null)
				{
					complated(response.userFacilityId);
				}
			}
		};
		return new APIRequestTask(request, true);
	}

	public void SaveResponseToFacilityBuild(int userFacilityID)
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmObjectSetting component = base.GetComponent<FarmObjectSetting>();
		instance.ResetSettingMark();
		component.farmObject.DisplayFloorObject();
		instance.SetActiveNotTouchObject(true);
		FarmObject farmObject = component.farmObject;
		FarmGrid.GridPosition gridPosition = component.ComplatedSetting();
		farmObject.userFacilityID = userFacilityID;
		UserFacility facility = this.StartBuild(farmObject, gridPosition);
		Singleton<UserDataMng>.Instance.AddUserFacility(facility);
		this.farmObjects.Add(farmObject);
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(farmObject.facilityID);
		FarmUtility.PayCost(facilityMaster.buildingAssetCategoryId1, facilityMaster.buildingAssetNum1);
		if (0 < int.Parse(facilityMaster.buildingTime) || int.Parse(facilityMaster.autoBuildingFlg) == 0)
		{
			this.StartConstruction(userFacilityID);
		}
		else
		{
			farmObject.BuildComplete();
		}
	}

	private UserFacility StartBuild(FarmObject farmObject, FarmGrid.GridPosition gridPosition)
	{
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(farmObject.facilityID);
		int num = int.Parse(facilityMaster.buildingTime);
		string text = null;
		if (facilityMaster.autoBuildingFlg == "0")
		{
			text = ServerDateTime.Now.ToString();
		}
		if (0 < num)
		{
			DateTime time = ServerDateTime.Now.AddSeconds((double)num + 1.0);
			text = FarmUtility.GetDateString(time);
		}
		return new UserFacility
		{
			userFacilityId = farmObject.userFacilityID,
			facilityId = farmObject.facilityID,
			positionX = gridPosition.x,
			positionY = gridPosition.y,
			level = ((text != null) ? 0 : 1),
			completeTime = text
		};
	}

	public void StartUpgrade(UserFacility userFacility)
	{
		FarmRoot instance = FarmRoot.Instance;
		FacilityUpgradeM facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(userFacility.facilityId, userFacility.level + 1);
		FarmUtility.PayCost(facilityUpgradeMaster.upgradeAssetCategoryId1, facilityUpgradeMaster.upgradeAssetNum1);
		int num = 0;
		if (int.TryParse(facilityUpgradeMaster.upgradeTime, out num))
		{
			DateTime time = ServerDateTime.Now.AddSeconds((double)(num + 1));
			userFacility.completeTime = FarmUtility.GetDateString(time);
			FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(userFacility.facilityId);
			if (facilityMaster.levelUpFlg == "1")
			{
				this.ReBuildEventItem(userFacility);
			}
			this.StartConstruction(userFacility.userFacilityId);
			instance.ClearSettingFarmObject();
		}
	}

	private void SetConstructionModel(FarmObject farmObject)
	{
		string constructionModelName = FarmUtility.GetConstructionModelName(farmObject);
		GameObject gameObject = AssetDataMng.Instance().LoadObject(constructionModelName, null, true) as GameObject;
		if (null == gameObject)
		{
			global::Debug.LogError("工事中のモデルが見つからない : " + constructionModelName);
			return;
		}
		GameObject construction = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		farmObject.SetConstruction(construction);
	}

	public void StartConstruction(int userFacilityID)
	{
		FarmObject farmObject = this.farmObjects.SingleOrDefault((FarmObject x) => x.userFacilityID == userFacilityID);
		if (null != farmObject)
		{
			if (!farmObject.IsConstruction())
			{
				this.SetConstructionModel(farmObject);
			}
			farmObject.SetConstructionDetail();
		}
	}

	public void AllStoreFarmObject()
	{
		FarmRoot instance = FarmRoot.Instance;
		for (int i = 0; i < this.farmObjects.Count; i++)
		{
			this.StoreFarmObject(this.farmObjects[i]);
			if (this.farmObjects[i].facilityID == 11)
			{
				((FarmObject_Fence)this.farmObjects[i]).Init();
			}
		}
		instance.Field.ClearPutFlag();
	}

	public void StoreFarmObject(FarmObject farmObject)
	{
		Vector3 localPosition = farmObject.gameObject.transform.localPosition;
		localPosition.y = 8000f;
		farmObject.gameObject.transform.localPosition = localPosition;
	}

	public void InitializeFarmObject()
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		field.ClearPutFlag();
		List<UserFacility> userFacilityList = Singleton<UserDataMng>.Instance.GetUserFacilityList();
		for (int i = 0; i < userFacilityList.Count; i++)
		{
			UserFacility userFacility = userFacilityList[i];
			FarmObject farmObject = this.CreateFacilityObject(userFacility);
			if (null != farmObject)
			{
				this.farmObjects.Add(farmObject);
				this.SetFarmObjectParam(farmObject, userFacility);
			}
		}
		instance.ResetSetteingFence();
	}

	public IEnumerator InitializeFarmObjectParallelRead()
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		FarmField farmField = farmRoot.Field;
		farmField.ClearPutFlag();
		List<UserFacility> userFacilitys = Singleton<UserDataMng>.Instance.GetUserFacilityList();
		int num = userFacilitys.Count / 6;
		if (0 < userFacilitys.Count - 6 * num)
		{
			num++;
		}
		List<UserFacility[]> readFacility = new List<UserFacility[]>();
		for (int j = 0; j < 6; j++)
		{
			readFacility.Add(new UserFacility[num]);
		}
		int parallelIndex = 0;
		int dataIndex = 0;
		for (int k = 0; k < userFacilitys.Count; k++)
		{
			UserFacility[] array = readFacility[parallelIndex];
			array[dataIndex] = userFacilitys[k];
			parallelIndex++;
			if (6 <= parallelIndex)
			{
				parallelIndex = 0;
				dataIndex++;
			}
		}
		Coroutine[] coroutineList = new Coroutine[6];
		for (int l = 0; l < 6; l++)
		{
			coroutineList[l] = base.StartCoroutine(this.ReadFacility(readFacility[l]));
		}
		for (int i = 0; i < 6; i++)
		{
			yield return coroutineList[i];
		}
		farmRoot.ResetSetteingFence();
		yield break;
	}

	private IEnumerator ReadFacility(UserFacility[] userFacility)
	{
		foreach (UserFacility data in userFacility)
		{
			if (data != null)
			{
				FarmObject farmObject = this.CreateFacilityObject(data);
				if (null != farmObject)
				{
					this.farmObjects.Add(farmObject);
					this.SetFarmObjectParam(farmObject, data);
				}
				yield return null;
			}
		}
		yield break;
	}

	public void ResumeFarmObject()
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		field.ClearPutFlag();
		List<UserFacility> userFacilityList = Singleton<UserDataMng>.Instance.GetUserFacilityList();
		for (int i = 0; i < userFacilityList.Count; i++)
		{
			UserFacility userFacility = userFacilityList[i];
			FarmObject farmObject = this.FindFarmObjectByUserFacilityId(userFacility.userFacilityId);
			if (null != farmObject)
			{
				this.SetFarmObjectParam(farmObject, userFacility);
			}
		}
		instance.ResetSetteingFence();
	}

	private void SetFarmObjectParam(FarmObject farmObject, UserFacility userFacility)
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		FarmGrid.GridPosition gridPosition = new FarmGrid.GridPosition
		{
			x = userFacility.positionX,
			y = userFacility.positionY
		};
		int gridIndex = field.Grid.GetGridIndex(gridPosition);
		Vector3 positionGridCenter = field.Grid.GetPositionGridCenter(gridIndex, false);
		farmObject.SetPosition(field.gridHorizontal, field.gridVertical, positionGridCenter);
		field.SetPutFlag(farmObject, gridPosition.x, gridPosition.y, true);
		if (!string.IsNullOrEmpty(userFacility.completeTime))
		{
			this.StartConstruction(userFacility.userFacilityId);
		}
	}

	private FarmObject CreateFacilityObject(UserFacility userFacility)
	{
		FarmObject farmObject = null;
		FarmObject farmObject2 = this.FindFarmObjectByFacilityId(userFacility.facilityId);
		if (null == farmObject2)
		{
			FarmSceneryCache cache = FarmSceneryCache.GetCache();
			if (null != cache)
			{
				FarmObject cacheFarmObject = cache.GetCacheFarmObject(userFacility.facilityId);
				if (null != cacheFarmObject)
				{
					cacheFarmObject.transform.parent = base.transform;
					cacheFarmObject.transform.localScale = Vector3.one;
					cacheFarmObject.transform.localEulerAngles = Vector3.zero;
					farmObject = cacheFarmObject;
					farmObject.OnResumeFromCache();
				}
			}
			if (null == farmObject)
			{
				farmObject = this.LoadFarmObject(userFacility.facilityId);
			}
		}
		else
		{
			farmObject = this.DuplicateFarmObject(farmObject2);
		}
		if (null != farmObject)
		{
			farmObject.facilityID = userFacility.facilityId;
			farmObject.userFacilityID = userFacility.userFacilityId;
		}
		return farmObject;
	}

	private FarmObject FindFarmObjectByFacilityId(int facilityID)
	{
		for (int i = 0; i < this.farmObjects.Count; i++)
		{
			if (this.farmObjects[i].facilityID == facilityID)
			{
				return this.farmObjects[i];
			}
		}
		return null;
	}

	private FarmObject FindFarmObjectByUserFacilityId(int userFacilityId)
	{
		for (int i = 0; i < this.farmObjects.Count; i++)
		{
			if (this.farmObjects[i].userFacilityID == userFacilityId)
			{
				return this.farmObjects[i];
			}
		}
		return null;
	}

	public void RelocationOfStoreFarmObject(FarmObject farmObject)
	{
		bool flag = this.SetFarmObjectOfEditMode(farmObject, this.GetScreenCenterPoint());
		if (flag)
		{
			GUICameraControll component = FarmRoot.Instance.Camera.GetComponent<GUICameraControll>();
			if (null != component)
			{
				Vector3 baseGridPosition3D = farmObject.GetBaseGridPosition3D();
				base.StartCoroutine(component.MoveCameraToLookAtPoint(baseGridPosition3D, 0f));
			}
		}
	}

	public bool RelocationOfStoreFarmObjectAndDragState(FarmObject farmObject, Vector3 mousePosition)
	{
		bool flag = this.SetFarmObjectOfEditModeAndDragState(farmObject, mousePosition);
		if (flag)
		{
			GUICameraControll component = FarmRoot.Instance.Camera.GetComponent<GUICameraControll>();
			if (null != component)
			{
				Vector3 baseGridPosition3D = farmObject.GetBaseGridPosition3D();
				base.StartCoroutine(component.MoveCameraToLookAtPoint(baseGridPosition3D, 0f));
			}
		}
		return flag;
	}

	private bool SetFarmObjectOfEditMode(FarmObject farmObject, Vector3 farmObjectPos)
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmObjectSetting component = base.GetComponent<FarmObjectSetting>();
		bool result = component.SetEditFarmObject(farmObject, farmObjectPos);
		if (farmObject.isPlaceable)
		{
			FarmObjectSelect component2 = base.GetComponent<FarmObjectSelect>();
			component.ComplatedSetting();
			component2.SetSelectObject(farmObject.gameObject);
			if (null != instance.farmUI && !this.IsExistStoreFacility())
			{
				instance.farmUI.EnableEditSaveButton(true);
			}
		}
		else
		{
			FarmObjectEdit component3 = base.GetComponent<FarmObjectEdit>();
			component3.StartEdit(farmObject);
		}
		if (null != instance.farmUI)
		{
			instance.farmUI.EnableEditStoreButton(true);
		}
		return result;
	}

	private bool SetFarmObjectOfEditModeAndDragState(FarmObject farmObject, Vector3 farmObjectPos)
	{
		FarmObjectSetting component = base.GetComponent<FarmObjectSetting>();
		bool flag = component.SetEditFarmObject(farmObject, farmObjectPos);
		if (flag && farmObject.isPlaceable)
		{
			FarmObjectSelect component2 = base.GetComponent<FarmObjectSelect>();
			component.ComplatedSetting();
			component2.SetSelectObject(farmObject.gameObject);
			if (null != FarmRoot.Instance.farmUI && !this.IsExistStoreFacility())
			{
				FarmRoot.Instance.farmUI.EnableEditSaveButton(true);
			}
		}
		else
		{
			FarmObjectEdit component3 = base.GetComponent<FarmObjectEdit>();
			component3.StartEdit(farmObject);
		}
		if (null != FarmRoot.Instance.farmUI)
		{
			FarmRoot.Instance.farmUI.EnableEditStoreButton(true);
		}
		return flag;
	}

	public int GetFacilityCount(int facilityID)
	{
		int num = 0;
		for (int i = 0; i < this.farmObjects.Count; i++)
		{
			if (this.farmObjects[i].facilityID == facilityID)
			{
				num++;
			}
		}
		return num;
	}

	public IEnumerator SaveEdit(Action<bool> completed)
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		FarmField farmField = farmRoot.Field;
		List<FacilityPosition> facilityPositions = new List<FacilityPosition>();
		for (int i = 0; i < this.farmObjects.Count; i++)
		{
			FarmObject farmObject = this.farmObjects[i];
			FarmGrid.GridPosition gridPosition = farmField.Grid.GetGridPosition(farmObject.GetBaseGridPosition3D());
			FacilityPosition item = new FacilityPosition
			{
				userFacilityId = farmObject.userFacilityID,
				positionX = gridPosition.x,
				positionY = gridPosition.y
			};
			facilityPositions.Add(item);
		}
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		RequestFA_FacilityAllArrangement request = new RequestFA_FacilityAllArrangement
		{
			SetSendData = delegate(FacilityAllArrangement param)
			{
				param.userFacilityList = facilityPositions.ToArray();
			},
			OnReceived = delegate(WebAPI.ResponseData response)
			{
				for (int j = 0; j < facilityPositions.Count; j++)
				{
					FacilityPosition facilityPosition = facilityPositions[j];
					UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(facilityPosition.userFacilityId);
					if (userFacility != null)
					{
						userFacility.positionX = facilityPosition.positionX;
						userFacility.positionY = facilityPosition.positionY;
					}
				}
				if (completed != null)
				{
					completed(true);
				}
			}
		};
		RequestBase request2 = request;
		if (FarmScenery.<>f__mg$cache0 == null)
		{
			FarmScenery.<>f__mg$cache0 = new Action(RestrictionInput.EndLoad);
		}
		yield return base.StartCoroutine(request2.Run(FarmScenery.<>f__mg$cache0, null, null));
		yield break;
	}

	public void DeleteFarmObject(int userFacilityID)
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		instance.ClearSettingFarmObject();
		Singleton<UserDataMng>.Instance.DeleteUserFacility(userFacilityID);
		bool flag = false;
		for (int i = 0; i < this.farmObjects.Count; i++)
		{
			if (userFacilityID == this.farmObjects[i].userFacilityID)
			{
				flag = (this.farmObjects[i].facilityID == 11);
				GameObject gameObject = this.farmObjects[i].gameObject;
				field.SetGridPutFlag(this.farmObjects[i], false);
				this.farmObjects.Remove(this.farmObjects[i]);
				UnityEngine.Object.Destroy(gameObject);
				break;
			}
		}
		if (flag)
		{
			FarmRoot.Instance.ResetSetteingFence();
		}
	}

	public bool ExtendBuildFarmObject(int facilityID, int prevUserFacilityID)
	{
		FarmObject farmObject = this.LoadFarmObject(facilityID);
		if (null == farmObject)
		{
			global::Debug.LogError(string.Format("Resource Not Found : {0}, {1}", facilityID, prevUserFacilityID));
			return false;
		}
		farmObject.facilityID = facilityID;
		FarmObject farmObject2 = this.farmObjects.SingleOrDefault((FarmObject x) => x.userFacilityID == prevUserFacilityID);
		if (null == farmObject2)
		{
			global::Debug.LogError(string.Format("Prev FarmObject Not Found : {0}, {1}", facilityID, prevUserFacilityID));
			return false;
		}
		FarmObjectSetting component = base.GetComponent<FarmObjectSetting>();
		Vector3 extendBuildPosition = this.GetExtendBuildPosition(component, farmObject2);
		component.SetFarmObject(farmObject, extendBuildPosition);
		return true;
	}

	private Vector3 GetExtendBuildPosition(FarmObjectSetting farmObjectSetting, FarmObject prevFarmObject)
	{
		FarmObjectSetting.ExtendBuildPositionSearchResult extendBuildPositionSearchResult = farmObjectSetting.SearchExtendBuildGrid(prevFarmObject);
		FarmGrid grid = FarmRoot.Instance.Field.Grid;
		int gridIndex = grid.GetGridIndex(extendBuildPositionSearchResult.grid);
		Vector3 positionGridCenter = grid.GetPositionGridCenter(gridIndex, true);
		if (!extendBuildPositionSearchResult.isOutsideMap)
		{
			GUICameraControll component = FarmRoot.Instance.Camera.GetComponent<GUICameraControll>();
			if (null != component)
			{
				base.StartCoroutine(component.MoveCameraToLookAtPoint(positionGridCenter, 0f));
			}
		}
		return positionGridCenter;
	}

	public bool IsExistStoreFacility()
	{
		return this.farmObjects.Any((FarmObject x) => x.IsStore());
	}

	public void DestroyAllFarmObject()
	{
		for (int i = 0; i < this.farmObjects.Count; i++)
		{
			GameObject gameObject = this.farmObjects[i].gameObject;
			FarmRoot.Instance.Field.SetGridPutFlag(this.farmObjects[i], false);
			UnityEngine.Object.Destroy(gameObject);
		}
		this.farmObjects.Clear();
	}

	public APIRequestTask SaveStockFarmObjectPosition(int userFacilityId, Action<int> complated)
	{
		RequestFA_FacilityStock request = new RequestFA_FacilityStock
		{
			SetSendData = delegate(FacilityStock param)
			{
				param.userFacilityId = userFacilityId;
				param.stockFlg = 0;
			},
			OnReceived = delegate(FacilityStockResult response)
			{
				UserFacility userStockFacility = Singleton<UserDataMng>.Instance.GetUserStockFacility(userFacilityId);
				Singleton<UserDataMng>.Instance.DeleteUserStockFacility(userFacilityId);
				Singleton<UserDataMng>.Instance.AddUserFacility(userStockFacility);
			}
		};
		RequestFA_FacilityMoving request2 = new RequestFA_FacilityMoving
		{
			SetSendData = delegate(FacilityMoving param)
			{
				FarmRoot instance = FarmRoot.Instance;
				FarmField field = instance.Field;
				FarmObjectSetting component = this.GetComponent<FarmObjectSetting>();
				FarmGrid.GridPosition gridPosition = field.Grid.GetGridPosition(component.farmObject.GetBaseGridPosition3D());
				param.userFacilityId = userFacilityId;
				param.positionX = gridPosition.x;
				param.positionY = gridPosition.y;
			},
			OnReceived = delegate(WebAPI.ResponseData response)
			{
				FarmRoot instance = FarmRoot.Instance;
				FarmField field = instance.Field;
				FarmObjectSetting component = this.GetComponent<FarmObjectSetting>();
				FarmGrid.GridPosition gridPosition = field.Grid.GetGridPosition(component.farmObject.GetBaseGridPosition3D());
				UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(userFacilityId);
				userFacility.positionX = gridPosition.x;
				userFacility.positionY = gridPosition.y;
				this.SaveResponseToStockToFarmFacility(userFacilityId);
				if (complated != null)
				{
					complated(userFacilityId);
				}
			}
		};
		APIRequestTask apirequestTask = new APIRequestTask(request, true);
		APIRequestTask task = new APIRequestTask(request2, true);
		apirequestTask.Add(task);
		return apirequestTask;
	}

	public void SaveResponseToStockToFarmFacility(int userFacilityID)
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmObjectSetting component = base.GetComponent<FarmObjectSetting>();
		instance.ResetSettingMark();
		component.farmObject.DisplayFloorObject();
		instance.SetActiveNotTouchObject(true);
		FarmObject farmObject = component.farmObject;
		component.ComplatedSetting();
		farmObject.userFacilityID = userFacilityID;
		this.farmObjects.Add(farmObject);
		farmObject.BuildComplete();
	}

	private void ReBuildEventItem(UserFacility userFacility)
	{
		for (int i = 0; i < this.farmObjects.Count; i++)
		{
			if (userFacility.userFacilityId == this.farmObjects[i].userFacilityID)
			{
				GameObject gameObject = this.farmObjects[i].gameObject;
				this.farmObjects.Remove(this.farmObjects[i]);
				UnityEngine.Object.Destroy(gameObject);
				break;
			}
		}
		FarmObject farmObject = this.LoadFarmObject(userFacility.facilityId);
		if (null != farmObject)
		{
			farmObject.facilityID = userFacility.facilityId;
			farmObject.userFacilityID = userFacility.userFacilityId;
			this.farmObjects.Add(farmObject);
			this.SetFarmObjectParam(farmObject, userFacility);
		}
	}
}
