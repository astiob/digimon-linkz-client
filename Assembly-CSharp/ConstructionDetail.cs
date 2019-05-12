using FarmData;
using System;
using UnityEngine;

public sealed class ConstructionDetail : MonoBehaviour
{
	[NonSerialized]
	public FarmObject farmObject;

	[SerializeField]
	private UILabel timeCount;

	[SerializeField]
	private UIProgressBar timeGage;

	[SerializeField]
	private UISprite completePop;

	private Camera farmCamera;

	private DateTime complateTime;

	private int totalComplateSeconds;

	private bool disableCount;

	private void Start()
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null == instance)
		{
			return;
		}
		this.farmCamera = instance.Camera;
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.farmObject.userFacilityID);
		if (string.IsNullOrEmpty(userFacility.completeTime))
		{
			this.complateTime = ServerDateTime.Now;
		}
		else
		{
			this.complateTime = DateTime.Parse(userFacility.completeTime);
		}
		if (userFacility.level == 0 || (userFacility.level == 1 && string.IsNullOrEmpty(userFacility.completeTime)))
		{
			FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(userFacility.facilityId);
			this.totalComplateSeconds = int.Parse(facilityMaster.buildingTime);
		}
		else
		{
			FacilityUpgradeM facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(userFacility.facilityId, userFacility.level + 1);
			this.totalComplateSeconds = int.Parse(facilityUpgradeMaster.upgradeTime);
		}
		this.OnUpdate();
	}

	private void Update()
	{
		this.OnUpdate();
	}

	private void OnUpdate()
	{
		this.UpdatePosition();
		this.UpdateInfo();
	}

	private void UpdatePosition()
	{
		Vector3 position = this.farmCamera.WorldToScreenPoint(this.farmObject.transform.position);
		Camera gUICamera = GUIManager.gUICamera;
		Vector3 vector = gUICamera.ScreenToWorldPoint(position);
		Vector3 position2 = base.transform.position;
		position2.x = vector.x;
		position2.y = vector.y;
		base.transform.position = position2;
	}

	private void UpdateInfo()
	{
		if (!this.disableCount)
		{
			int num = (int)(this.complateTime - ServerDateTime.Now).TotalSeconds;
			if (0 >= num)
			{
				num = 0;
				this.SetCompletePop();
			}
			else
			{
				this.timeCount.text = num.ToBuildTime();
			}
			float num2 = (float)(this.totalComplateSeconds - num) / (float)this.totalComplateSeconds;
			this.timeGage.value = ((1f > num2) ? num2 : 1f);
		}
	}

	public void Close()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		this.farmObject = null;
		this.farmCamera = null;
	}

	public void DisableTimeCountDown(int restSecondTime)
	{
		this.disableCount = true;
		this.totalComplateSeconds = restSecondTime;
		this.timeCount.text = restSecondTime.ToBuildTime();
		this.timeGage.value = 0f;
	}

	public int GetComplateSeconds()
	{
		return this.totalComplateSeconds;
	}

	public void DisableCollider()
	{
		if (this.completePop.gameObject.activeSelf)
		{
			BoxCollider component = this.completePop.gameObject.GetComponent<BoxCollider>();
			if (null != component)
			{
				component.enabled = false;
			}
		}
	}

	private void SetCompletePop()
	{
		if (!this.completePop.gameObject.activeSelf)
		{
			this.completePop.gameObject.SetActive(true);
			this.timeCount.gameObject.SetActive(false);
			this.timeGage.gameObject.SetActive(false);
		}
	}

	public void OnTapForCompletePop()
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance && this.farmObject.IsConstruction() && !this.farmObject.IsTutorialFacility() && !FarmRoot.Instance.IsVisitFriendFarm)
		{
			FarmObjectSelect selectObject = instance.SelectObject;
			if (null != selectObject && !selectObject.IsMuchFarmObject(this.farmObject.userFacilityID))
			{
				selectObject.ResetSelectedFarmObject();
				selectObject.SetSelectObject(this.farmObject.gameObject);
			}
			base.StartCoroutine(this.farmObject.ServerRequestFacilityBuildComplete(null));
		}
	}
}
