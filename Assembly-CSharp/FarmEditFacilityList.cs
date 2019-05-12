using FarmData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FarmEditFacilityList : GUISelectPanelBSPartsLR
{
	[SerializeField]
	private GameObject listArrowLeft;

	[SerializeField]
	private GameObject listArrowRight;

	private float buttonWidth;

	public void Initialize()
	{
		GUICollider component = base.GetComponent<GUICollider>();
		BoxCollider component2 = base.GetComponent<BoxCollider>();
		Vector3 localPosition = component.transform.localPosition;
		component.SetOriginalPos(base.selectParts.transform.localPosition);
		component.transform.localPosition = localPosition;
		base.ListWindowViewRect = new Rect
		{
			x = component2.size.x * -0.5f,
			y = component2.size.y * -0.5f,
			width = component2.size.x,
			height = component2.size.y
		};
		this.buttonWidth = base.selectCollider.width;
		base.selectParts.SetActive(false);
	}

	public void CreateStoreFacilityButton()
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmScenery scenery = instance.Scenery;
		List<EditStoreFacility> list = new List<EditStoreFacility>();
		for (int i = 0; i < scenery.farmObjects.Count; i++)
		{
			FarmObject farmObject = scenery.farmObjects[i];
			UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(farmObject.userFacilityID);
			FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(userFacility.facilityId);
			EditStoreFacility editStoreFacility = null;
			if (0 < list.Count)
			{
				if (int.Parse(facilityMaster.type) == 1)
				{
					editStoreFacility = list.SingleOrDefault((EditStoreFacility x) => x.facilityID == farmObject.facilityID && x.level == userFacility.level);
				}
				else
				{
					editStoreFacility = list.SingleOrDefault((EditStoreFacility x) => x.facilityID == farmObject.facilityID);
				}
			}
			if (editStoreFacility == null)
			{
				editStoreFacility = new EditStoreFacility
				{
					facilityID = farmObject.facilityID,
					level = userFacility.level
				};
				list.Add(editStoreFacility);
			}
		}
		this.CreateListItem(list.ToArray(), true);
	}

	private void CreateListItem(EditStoreFacility[] storeFacilities, bool initListPosition)
	{
		FarmEditFacilityButton[] componentsInChildren = base.GetComponentsInChildren<FarmEditFacilityButton>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.SetActive(false);
				UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
				componentsInChildren[i] = null;
			}
		}
		base.initLocation = initListPosition;
		this.AllBuild(storeFacilities);
		this.UpdateListArrow();
	}

	private void AllBuild(EditStoreFacility[] storeFacilities)
	{
		base.InitBuild();
		Resources.UnloadUnusedAssets();
		this.partsCount = storeFacilities.Length;
		float width = base.selectCollider.width;
		float num = width + this.horizontalMargin;
		float num2 = num * (float)this.partsCount + this.horizontalBorder * 2f;
		base.width = num2 - this.horizontalMargin;
		float num3 = base.width * -0.5f + this.horizontalBorder + width * 0.5f;
		float y = 0f;
		for (int i = 0; i < this.partsCount; i++)
		{
			float x = num3 + num * (float)i;
			FarmEditFacilityButton component = base.AddBuildPart().GetComponent<FarmEditFacilityButton>();
			component.SetOriginalPos(new Vector3(x, y, base.transform.localPosition.z));
			component.SetDetail(storeFacilities[i]);
			component.gameObject.SetActive(true);
		}
		base.InitMinMaxLocation(true);
	}

	public FarmEditFacilityButton GetSamFacilityButton(FarmObject farmObject)
	{
		FarmEditFacilityButton[] componentsInChildren = base.GetComponentsInChildren<FarmEditFacilityButton>();
		if (componentsInChildren == null)
		{
			global::Debug.LogError(string.Format("FacilityID = {0}, UserFacilityID = {1}", farmObject.facilityID, farmObject.userFacilityID));
			return null;
		}
		return componentsInChildren.SingleOrDefault((FarmEditFacilityButton x) => x.IsSame(farmObject));
	}

	public void StartSetting(FarmObject farmObject)
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null == instance)
		{
			global::Debug.LogError("FarmRoot Not Found");
			return;
		}
		this.DecrementFacilityCount(farmObject);
		instance.Scenery.RelocationOfStoreFarmObject(farmObject);
	}

	public bool StartSettingAndDragState(FarmObject farmObject, Vector3 mousePosition)
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null == instance)
		{
			global::Debug.LogError("FarmRoot Not Found");
			return false;
		}
		this.DecrementFacilityCount(farmObject);
		return instance.Scenery.RelocationOfStoreFarmObjectAndDragState(farmObject, mousePosition);
	}

	public void DecrementFacilityCount(FarmObject farmObject)
	{
		FarmEditFacilityButton samFacilityButton = this.GetSamFacilityButton(farmObject);
		if (null == samFacilityButton)
		{
			global::Debug.LogError(string.Format("Not Found Button : FacilityID = {0}", farmObject.facilityID));
			return;
		}
		if (samFacilityButton.CountDown(farmObject) == 0)
		{
			this.DeleteStoreFacilityButton(farmObject);
		}
	}

	private void DeleteStoreFacilityButton(FarmObject farmObject)
	{
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(farmObject.userFacilityID);
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(farmObject.facilityID);
		List<EditStoreFacility> storeFacilityButtonList = this.GetStoreFacilityButtonList();
		EditStoreFacility[] storeFacilities;
		if (int.Parse(facilityMaster.type) == 1)
		{
			storeFacilities = storeFacilityButtonList.Where((EditStoreFacility x) => x.facilityID != farmObject.facilityID || (x.facilityID == farmObject.facilityID && x.level != userFacility.level)).ToArray<EditStoreFacility>();
		}
		else
		{
			storeFacilities = storeFacilityButtonList.Where((EditStoreFacility x) => x.facilityID != farmObject.facilityID).ToArray<EditStoreFacility>();
		}
		this.CreateListItem(storeFacilities, false);
	}

	public bool ExistButton()
	{
		FarmEditFacilityButton[] componentsInChildren = base.GetComponentsInChildren<FarmEditFacilityButton>();
		return 0 != componentsInChildren.Length;
	}

	public void AddStoreFacilityButton(FarmObject farmObject)
	{
		List<EditStoreFacility> storeFacilityButtonList = this.GetStoreFacilityButtonList();
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(farmObject.userFacilityID);
		EditStoreFacility item = new EditStoreFacility
		{
			facilityID = userFacility.facilityId,
			level = userFacility.level
		};
		storeFacilityButtonList.Add(item);
		this.CreateListItem(storeFacilityButtonList.ToArray(), false);
	}

	private List<EditStoreFacility> GetStoreFacilityButtonList()
	{
		List<EditStoreFacility> list = new List<EditStoreFacility>();
		FarmEditFacilityButton[] componentsInChildren = base.GetComponentsInChildren<FarmEditFacilityButton>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!componentsInChildren[i].IsEmpty())
				{
					int userFacilityID = componentsInChildren[i].GetUserFacilityID();
					UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(userFacilityID);
					EditStoreFacility item = new EditStoreFacility
					{
						facilityID = userFacility.facilityId,
						level = userFacility.level
					};
					list.Add(item);
				}
			}
		}
		return list;
	}

	public void UpdateListArrow()
	{
		if (base.viewRect.width < this.listViewRect.width)
		{
			float margin = this.buttonWidth * 0.5f;
			if (this.IsListLeftPosition(margin))
			{
				this.listArrowLeft.SetActive(false);
			}
			else
			{
				this.listArrowLeft.SetActive(true);
			}
			if (this.IsListRightPosition(margin))
			{
				this.listArrowRight.SetActive(false);
			}
			else
			{
				this.listArrowRight.SetActive(true);
			}
		}
		else
		{
			this.listArrowLeft.SetActive(false);
			this.listArrowRight.SetActive(false);
		}
	}

	private bool IsListLeftPosition(float margin)
	{
		float maxLocate = base.maxLocate;
		float selectLoc = this.selectLoc;
		return maxLocate - margin < selectLoc;
	}

	private bool IsListRightPosition(float margin)
	{
		float minLocate = base.minLocate;
		float selectLoc = this.selectLoc;
		return minLocate + margin > selectLoc;
	}
}
