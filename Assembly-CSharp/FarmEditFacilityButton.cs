using FarmData;
using Master;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class FarmEditFacilityButton : GUIListPartBS
{
	private List<FarmObject> farmObjects = new List<FarmObject>();

	[SerializeField]
	private UITexture iconTexture;

	[SerializeField]
	private UILabel countLabel;

	[SerializeField]
	private UILabel levelLabel;

	[SerializeField]
	private FarmDragCreate dragCreate;

	protected override void OnDestroy()
	{
		if (null != this.iconTexture)
		{
			this.iconTexture.mainTexture = null;
		}
		base.OnDestroy();
	}

	public void SetDetail(EditStoreFacility storeFacility)
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null == instance)
		{
			return;
		}
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(storeFacility.facilityID);
		bool flag = 1 == int.Parse(facilityMaster.type);
		FarmObject[] array = instance.Scenery.farmObjects.Where((FarmObject x) => x.facilityID == storeFacility.facilityID && x.IsStore()).ToArray<FarmObject>();
		if (flag)
		{
			for (int i = 0; i < array.Length; i++)
			{
				UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(array[i].userFacilityID);
				if (userFacility.level == storeFacility.level)
				{
					this.farmObjects.Add(array[i]);
				}
			}
			FacilityUpgradeM facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(storeFacility.facilityID, storeFacility.level);
			if (facilityUpgradeMaster != null)
			{
				this.levelLabel.text = string.Format(StringMaster.GetString("FarmEditButtonLevel"), storeFacility.level.ToString());
			}
			else if (storeFacility.level == 0)
			{
				this.levelLabel.text = StringMaster.GetString("FarmEditButtonLevelZero");
			}
			else
			{
				this.levelLabel.text = string.Empty;
			}
		}
		else
		{
			this.farmObjects.AddRange(array);
			this.levelLabel.text = string.Empty;
		}
		this.countLabel.text = string.Format(StringMaster.GetString("FarmEditButtonCount"), this.farmObjects.Count.ToString());
		NGUIUtil.ChangeUITextureFromFile(this.iconTexture, facilityMaster.GetIconPath(), false);
	}

	public bool IsSame(FarmObject farmObject)
	{
		if (0 >= this.farmObjects.Count)
		{
			global::Debug.LogError(string.Format("FacilityID = {0}, UserFacilityID = {1}", farmObject.facilityID, farmObject.userFacilityID));
			return false;
		}
		if (this.farmObjects[0].facilityID != farmObject.facilityID)
		{
			return false;
		}
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(farmObject.userFacilityID);
		if (userFacility == null)
		{
			global::Debug.LogError(string.Format("FacilityID = {0}, UserFacilityID = {1}", farmObject.facilityID, farmObject.userFacilityID));
			return false;
		}
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(farmObject.facilityID);
		return int.Parse(facilityMaster.type) != 1 || userFacility.level == this.GetLevel();
	}

	private int GetLevel()
	{
		if (0 >= this.farmObjects.Count)
		{
			global::Debug.LogError("0 == this.farmObjects.Count");
			return -1;
		}
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.farmObjects[0].userFacilityID);
		if (userFacility == null)
		{
			global::Debug.LogError(string.Format("FacilityID = {0}, UserFacilityID = {1}", this.farmObjects[0].facilityID, this.farmObjects[0].userFacilityID));
			return -1;
		}
		return userFacility.level;
	}

	public void CountUp(FarmObject farmObject)
	{
		this.farmObjects.Add(farmObject);
		this.countLabel.text = string.Format(StringMaster.GetString("FarmEditButtonCount"), this.farmObjects.Count.ToString());
	}

	public int CountDown(FarmObject farmObject)
	{
		this.farmObjects.Remove(farmObject);
		int count = this.farmObjects.Count;
		this.countLabel.text = string.Format(StringMaster.GetString("FarmEditButtonCount"), count.ToString());
		return count;
	}

	private void OnPress(bool IsDown)
	{
		if (IsDown)
		{
			this.dragCreate.SetCallFacilityButton = this;
		}
		else
		{
			this.dragCreate.SetCallFacilityButton = null;
		}
	}

	public void DragCreate()
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null == instance)
		{
			global::Debug.LogError("FarmRoot Not Found");
			return;
		}
		if (0 >= this.farmObjects.Count)
		{
			global::Debug.LogError("0 == this.farmObjects.Count");
			return;
		}
		instance.ClearSettingFarmObject();
		FarmObject farmObject = this.farmObjects[0];
		Camera componentInChildren = instance.GetComponentInChildren<Camera>();
		Vector3 vector = componentInChildren.ScreenToWorldPoint(Input.mousePosition);
		FarmEditFacilityList componentInParent = base.GetComponentInParent<FarmEditFacilityList>();
		if (!componentInParent.StartSettingAndDragState(farmObject, vector))
		{
			vector = componentInChildren.ScreenToWorldPoint(Input.mousePosition);
			farmObject.transform.position = vector;
		}
		GUIManager.ResetTouchingCount();
	}

	private void OnPushed()
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null == instance)
		{
			global::Debug.LogError("FarmRoot Not Found");
			return;
		}
		if (0 >= this.farmObjects.Count)
		{
			global::Debug.LogError("0 == this.farmObjects.Count");
			return;
		}
		instance.ClearSettingFarmObject();
		FarmObject farmObject = this.farmObjects[0];
		FarmEditFacilityList componentInParent = base.GetComponentInParent<FarmEditFacilityList>();
		componentInParent.StartSetting(farmObject);
	}

	public bool IsEmpty()
	{
		return 0 == this.farmObjects.Count;
	}

	public int GetUserFacilityID()
	{
		if (this.farmObjects.Count == 0)
		{
			global::Debug.LogError("Empty this.farmObjects");
			return -1;
		}
		return this.farmObjects[0].userFacilityID;
	}

	public FarmObject GetFarmObject()
	{
		if (0 >= this.farmObjects.Count)
		{
			global::Debug.LogError("0 == this.farmObjects.Count");
			return null;
		}
		return this.farmObjects[0];
	}
}
