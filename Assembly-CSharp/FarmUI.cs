using System;
using UnityEngine;

public sealed class FarmUI : MonoBehaviour
{
	[SerializeField]
	private GameObject facilityButtonAnchor;

	public void CreateFacilityButton(FarmObject farmObject)
	{
		string buttonPrefabName = farmObject.GetButtonPrefabName();
		if (!string.IsNullOrEmpty(buttonPrefabName))
		{
			GameObject gameObject = GUIManager.LoadCommonGUI(buttonPrefabName, this.facilityButtonAnchor);
			if (null != gameObject)
			{
				gameObject.transform.localPosition = Vector3.zero;
				FacilityButtonSet component = gameObject.GetComponent<FacilityButtonSet>();
				component.SetFacility(farmObject);
			}
			GUIFace.ForceHideDigiviceBtn_S();
		}
	}

	public void UpdateFacilityButton(FarmObject farmObject)
	{
		FacilityButtonSet[] componentsInChildren = this.facilityButtonAnchor.GetComponentsInChildren<FacilityButtonSet>(true);
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (null != farmObject)
				{
					componentsInChildren[i].SetFacility(farmObject);
				}
				componentsInChildren[i].SettingButton();
			}
		}
	}

	public void DeleteFacilityButton()
	{
		FacilityButtonSet[] componentsInChildren = this.facilityButtonAnchor.GetComponentsInChildren<FacilityButtonSet>(true);
		if (componentsInChildren == null)
		{
			global::Debug.LogError("DeleteFacilityButton (Button Not Found)");
			return;
		}
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
	}

	public void CreateFacilityConfirmation()
	{
		GameObject gameObject = GUIManager.LoadCommonGUI("Farm/FacilityConfirmation", base.gameObject);
		if (null != gameObject)
		{
			gameObject.name = "FacilityConfirmation";
		}
	}

	public void CreateStockFacilityConfirmation()
	{
		GameObject gameObject = GUIManager.LoadCommonGUI("Farm/StockFacilityConfirmation", base.gameObject);
		if (null != gameObject)
		{
			gameObject.name = "StockFacilityConfirmation";
		}
	}

	public void CreateConstructionDetail(FarmObject farmObject)
	{
		ConstructionDetail constructionDetail = this.GetConstructionDetail(farmObject.userFacilityID);
		if (null == constructionDetail)
		{
			GameObject gameObject = this.CreateFarmObjectUI("Farm/ConstructionDetail", "ConstructionDetail", farmObject.userFacilityID);
			if (null != gameObject)
			{
				constructionDetail = gameObject.GetComponent<ConstructionDetail>();
				constructionDetail.farmObject = farmObject;
			}
		}
	}

	public void DeleteConstructionDetail(int userFacilityID)
	{
		ConstructionDetail constructionDetail = this.GetConstructionDetail(userFacilityID);
		if (null != constructionDetail)
		{
			constructionDetail.Close();
		}
	}

	public ConstructionDetail GetConstructionDetail(int userFacilityID)
	{
		return this.GetFacilityUI<ConstructionDetail>("ConstructionDetail_", userFacilityID);
	}

	public void CreateFacilityNamePlate(FarmObject farmObject)
	{
		ConstructionName constructionName = this.GetFacilityNamePlate(farmObject.userFacilityID);
		if (null == constructionName)
		{
			GameObject gameObject = this.CreateFarmObjectUI("Farm/ConstructionName", "FacilityNamePlate", farmObject.userFacilityID);
			if (null != gameObject)
			{
				constructionName = gameObject.GetComponent<ConstructionName>();
				constructionName.farmObject = farmObject.gameObject;
			}
		}
	}

	public void DeleteFacilityNamePlate(int userFacilityID)
	{
		ConstructionName facilityNamePlate = this.GetFacilityNamePlate(userFacilityID);
		if (null != facilityNamePlate)
		{
			facilityNamePlate.Close();
		}
	}

	public ConstructionName GetFacilityNamePlate(int userFacilityID)
	{
		return this.GetFacilityUI<ConstructionName>("FacilityNamePlate_", userFacilityID);
	}

	public void SetActiveConstructionDetail(bool active)
	{
		ConstructionDetail[] componentsInChildren = base.GetComponentsInChildren<ConstructionDetail>(true);
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.SetActive(active);
			}
		}
	}

	public void CreateSignalMeatHarvest(FarmObject farmObject)
	{
		ConstructionName facilityNamePlate = this.GetFacilityNamePlate(farmObject.userFacilityID);
		if (null != facilityNamePlate)
		{
			SignalMeatHarvest[] componentsInChildren = facilityNamePlate.GetComponentsInChildren<SignalMeatHarvest>(true);
			if (componentsInChildren != null && 0 < componentsInChildren.Length)
			{
				componentsInChildren[0].SetDisplay(true);
			}
			else
			{
				GameObject gameObject = GUIManager.LoadCommonGUI("Farm/SignalMeatHarvest", facilityNamePlate.gameObject);
				if (gameObject != null)
				{
					gameObject.name = "SignalMeatHarvest";
					SignalMeatHarvest component = gameObject.GetComponent<SignalMeatHarvest>();
					component.SetNamePlate(facilityNamePlate);
					component.SetDisplay(true);
				}
			}
		}
	}

	public void DeleteSignalMeatHarvest(int userFacilityId)
	{
		ConstructionName facilityNamePlate = this.GetFacilityNamePlate(userFacilityId);
		if (null != facilityNamePlate)
		{
			SignalMeatHarvest[] componentsInChildren = facilityNamePlate.GetComponentsInChildren<SignalMeatHarvest>(true);
			if (componentsInChildren != null && 0 < componentsInChildren.Length)
			{
				componentsInChildren[0].SetDisplay(false);
			}
		}
	}

	public void SetActiveSignalMeatHarvest(bool enable)
	{
		SignalMeatHarvest[] componentsInChildren = base.GetComponentsInChildren<SignalMeatHarvest>(true);
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetActiveIcon(enable);
			}
		}
	}

	private GameObject CreateFarmObjectUI(string loadPrefabName, string name, int userFacilityID)
	{
		GameObject gameObject = GUIManager.LoadCommonGUI(loadPrefabName, base.gameObject);
		if (null != gameObject)
		{
			gameObject.name = string.Format("{0}_{1}", name, userFacilityID.ToString());
		}
		return gameObject;
	}

	public T GetFacilityUI<T>(string name, int userFacilityID) where T : MonoBehaviour
	{
		T[] componentsInChildren = base.GetComponentsInChildren<T>(true);
		if (componentsInChildren != null)
		{
			string b = name + userFacilityID;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].name == b)
				{
					return componentsInChildren[i];
				}
			}
		}
		return (T)((object)null);
	}

	private void OnDisable()
	{
		this.DestroyALLUI();
	}

	public void DestroyALLUI()
	{
		ConstructionDetail[] componentsInChildren = base.GetComponentsInChildren<ConstructionDetail>(true);
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Close();
			}
		}
		ConstructionName[] componentsInChildren2 = base.GetComponentsInChildren<ConstructionName>(true);
		if (componentsInChildren2 != null)
		{
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].Close();
			}
		}
	}

	public void EnableEditSaveButton(bool enable)
	{
		FarmEditFooter componentInChildren = base.GetComponentInChildren<FarmEditFooter>();
		if (null == componentInChildren)
		{
			global::Debug.LogError("FarmEditFooter Not Found");
			return;
		}
		componentInChildren.EnableSaveButton(enable);
	}

	public void EnableEditStoreButton(bool enable)
	{
		FarmEditFooter componentInChildren = base.GetComponentInChildren<FarmEditFooter>();
		if (null == componentInChildren)
		{
			global::Debug.LogError("FarmEditFooter Not Found");
			return;
		}
		componentInChildren.EnableStoreButton(enable);
	}

	public FarmObject GetStoreFacility(FarmObject findFarmObject)
	{
		FarmEditFooter componentInChildren = base.GetComponentInChildren<FarmEditFooter>();
		if (null == componentInChildren)
		{
			global::Debug.LogError("FarmEditFooter Not Found");
			return null;
		}
		return componentInChildren.GetRestStoreFacility(findFarmObject);
	}

	public void AddStoreFacility(FarmObject addFarmObject)
	{
		FarmEditFooter componentInChildren = base.GetComponentInChildren<FarmEditFooter>();
		if (null == componentInChildren)
		{
			global::Debug.LogError("FarmEditFooter Not Found");
			return;
		}
		componentInChildren.AddFacilityButton(addFarmObject);
	}
}
