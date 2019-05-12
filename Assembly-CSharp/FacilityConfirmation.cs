using FarmData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FacilityConfirmation : MonoBehaviour
{
	private FarmObject farmObject;

	private Camera farmCamera;

	private bool isProcessing;

	[SerializeField]
	private GameObject yesButton;

	private bool enableYesButton = true;

	private void Start()
	{
		FarmRoot instance = FarmRoot.Instance;
		this.farmObject = instance.SettingObject.farmObject;
		this.farmCamera = instance.Camera;
		GUIScreenHome.enableBackKeyAndroid = false;
	}

	private void Update()
	{
		if (null != this.farmObject)
		{
			Vector3 position = this.farmCamera.WorldToScreenPoint(this.farmObject.transform.position);
			Camera componentInParent = base.GetComponentInParent<Camera>();
			Vector3 vector = componentInParent.ScreenToWorldPoint(position);
			Vector3 position2 = base.transform.position;
			position2.x = vector.x;
			position2.y = vector.y;
			base.transform.position = position2;
			if (this.enableYesButton != this.farmObject.isPlaceable)
			{
				this.EnableYesButton(this.farmObject.isPlaceable);
			}
		}
		this.UpdateAndroidBackKey();
	}

	private void UpdateAndroidBackKey()
	{
		if (GUIManager.IsEnableBackKeyAndroid() && Input.GetKeyDown(KeyCode.Escape))
		{
			this.OnPushedNo();
		}
	}

	protected int GetFacilityID()
	{
		return this.farmObject.facilityID;
	}

	private void OnPushedYes()
	{
		if (!this.CheckExtendBuild())
		{
			return;
		}
		if (!this.isProcessing)
		{
			this.isProcessing = true;
			if (this.farmObject.isPlaceable)
			{
				FarmRoot instance = FarmRoot.Instance;
				if (null != instance)
				{
					FarmScenery scenery = instance.Scenery;
					RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
					this.TaskSaveFarmObject(scenery);
				}
			}
		}
	}

	protected virtual void TaskSaveFarmObject(FarmScenery farmScenery)
	{
		APIRequestTask task = farmScenery.SaveFarmObjectPosition(new Action<int>(this.OnFinishedToSave));
		base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
		}, delegate(Exception exception)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	protected void OnFinishedToSave(int userFacilityID)
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(userFacilityID);
			if (this.CanExtendBuild(userFacility.facilityId))
			{
				FarmScenery scenery = instance.Scenery;
				if (scenery.ExtendBuildFarmObject(userFacility.facilityId, userFacilityID))
				{
					instance.SetActiveNotTouchObject(false);
					this.farmObject = instance.SettingObject.farmObject;
					this.isProcessing = false;
				}
				else
				{
					this.Close(true);
				}
			}
			else
			{
				this.Close(true);
			}
			if (this.farmObject.facilityID == 11)
			{
				FarmRoot.Instance.ResetSetteingFence();
			}
			this.PlaySavedSE();
		}
		else
		{
			this.isProcessing = false;
		}
	}

	protected virtual bool CanExtendBuild(int facilityId)
	{
		return FarmUtility.IsExtendBuild(facilityId);
	}

	protected virtual void PlaySavedSE()
	{
		SoundMng.Instance().TryPlaySE("SEInternal/Farm/se_204", 0f, false, true, null, -1);
	}

	private void OnPushedNo()
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			FarmScenery scenery = instance.Scenery;
			scenery.CancelSetting();
			this.Close(false);
			SoundMng.Instance().TryPlaySE("SEInternal/Common/se_106", 0f, false, true, null, -1);
		}
	}

	protected virtual void Close(bool returnHomeUI)
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			FarmObjectSelect selectObject = instance.SelectObject;
			selectObject.EnabledTouchedEvent(true);
			this.DeleteObject();
		}
		if (returnHomeUI)
		{
			GUIFace.instance.ShowGUI();
			GUIFaceIndicator.instance.ShowLocator();
			GUIFace.ShowLocator();
		}
		else
		{
			this.BackToUI();
		}
		GUIScreenHome.enableBackKeyAndroid = true;
	}

	protected virtual void BackToUI()
	{
		GUIMain.ShowCommonDialog(null, "CMD_FacilityShop");
	}

	public void DeleteObject()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void EnableYesButton(bool enable)
	{
		GUICollider component = this.yesButton.GetComponent<GUICollider>();
		UISprite[] componentsInChildren = component.GetComponentsInChildren<UISprite>();
		float num = 1f;
		if (enable)
		{
			component.CallBackClass = base.gameObject;
			component.touchBehavior = GUICollider.TouchBehavior.ToLarge;
			if (!this.enableYesButton)
			{
				num = 2.5f;
			}
		}
		else
		{
			component.CallBackClass = null;
			component.touchBehavior = GUICollider.TouchBehavior.None;
			if (this.enableYesButton)
			{
				num = 0.4f;
			}
		}
		this.enableYesButton = enable;
		foreach (UISprite uisprite in componentsInChildren)
		{
			Color color = uisprite.color;
			float num2 = num;
			color.r = Mathf.Clamp01(color.r * num2);
			color.g = Mathf.Clamp01(color.g * num2);
			color.b = Mathf.Clamp01(color.b * num2);
			uisprite.color = color;
		}
	}

	protected virtual bool CheckExtendBuild()
	{
		FarmRoot instance = FarmRoot.Instance;
		if (!(instance != null))
		{
			return false;
		}
		int num = instance.Scenery.GetFacilityCount(this.farmObject.facilityID);
		List<UserFacility> stockFacilityListByfacilityIdAndLevel = Singleton<UserDataMng>.Instance.GetStockFacilityListByfacilityIdAndLevel(this.farmObject.facilityID, -1);
		int count = stockFacilityListByfacilityIdAndLevel.Count;
		num += count;
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(this.farmObject.facilityID);
		if (FarmUtility.IsShortage(facilityMaster.buildingAssetCategoryId1, facilityMaster.buildingAssetNum1))
		{
			this.ShowExtendBuildErrorDialog("C-FA01", true);
			return false;
		}
		if (int.Parse(facilityMaster.maxNum) <= num)
		{
			this.ShowExtendBuildErrorDialog("E-FA03", false);
			return false;
		}
		return true;
	}

	private void ShowExtendBuildErrorDialog(string errorCode, bool returnHomeUI)
	{
		AlertManager.ShowAlertDialog(delegate(int x)
		{
			FarmRoot.Instance.Scenery.CancelSetting();
			this.Close(returnHomeUI);
		}, errorCode);
	}
}
