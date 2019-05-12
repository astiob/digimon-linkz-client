using FarmData;
using System;
using System.Collections;
using UnityEngine;

public class FarmObjectSelect : MonoBehaviour
{
	private FarmObject farmObject;

	private int selectUserFacilityID = -1;

	private bool isTouchRelease;

	private void Start()
	{
		this.EnabledTouchedEvent(true);
	}

	public bool IsMuchFarmObject(int userFacilityId)
	{
		return null != this.farmObject && userFacilityId == this.farmObject.userFacilityID;
	}

	public void EnabledTouchedEvent(bool enabled)
	{
		FarmRoot instance = FarmRoot.Instance;
		if (enabled)
		{
			instance.Input.AddTouchEndEvent(new Action<InputControll, bool>(this.OnTouchUp));
		}
		else
		{
			instance.Input.RemoveTouchEndEvent(new Action<InputControll, bool>(this.OnTouchUp));
			instance.Input.RemoveTouchDragEvent(new Func<InputControll, bool>(this.OnDrag));
			this.RemoveTouchDragEventForFriendFarm();
		}
	}

	private void OnTouchUp(InputControll inputControll, bool isDraged)
	{
		if (!isDraged)
		{
			if (inputControll.rayHitObjectType == InputControll.TouchObjectType.FACILITY)
			{
				if (this.selectUserFacilityID != -1)
				{
					GameObject gameObject = this.farmObject.gameObject;
					if (gameObject == inputControll.rayHitColliderObject)
					{
						this.RemoveTouchDragEventForFriendFarm();
						base.StartCoroutine(this.OnTouchedSelectFarmObject());
					}
					else
					{
						this.AddTouchDragEventForFriendFarm();
						this.ResetSelectedFarmObject();
						this.SetSelectObject(inputControll.rayHitColliderObject);
					}
				}
				else
				{
					this.AddTouchDragEventForFriendFarm();
					this.SetSelectObject(inputControll.rayHitColliderObject);
				}
			}
			else
			{
				this.RemoveTouchDragEventForFriendFarm();
				this.ClearSelectState();
			}
		}
		else if (this.selectUserFacilityID != -1)
		{
			if (this.farmObject.gameObject == inputControll.rayHitColliderObject)
			{
				this.CompletedMove();
			}
			this.AddTouchDragEventForFriendFarm();
		}
	}

	private void AddTouchDragEventForFriendFarm()
	{
		if (FarmRoot.Instance.IsVisitFriendFarm)
		{
			FarmRoot.Instance.Input.AddTouchDragEvent(new Func<InputControll, bool>(this.OnFriendFarmDrag));
		}
	}

	private void RemoveTouchDragEventForFriendFarm()
	{
		FarmRoot.Instance.Input.RemoveTouchDragEvent(new Func<InputControll, bool>(this.OnFriendFarmDrag));
	}

	private IEnumerator OnTouchedSelectFarmObject()
	{
		bool isEvent = false;
		yield return base.StartCoroutine(this.farmObject.OnTouchedEffect(true, delegate(bool res)
		{
			isEvent = res;
		}));
		if (!isEvent)
		{
			this.ResetSelectedFarmObject();
		}
		yield break;
	}

	private bool OnDrag(InputControll inputControll)
	{
		if (this.farmObject.gameObject == inputControll.rayHitColliderObject)
		{
			FarmObjectSetting component = base.GetComponent<FarmObjectSetting>();
			if (null == component.farmObject)
			{
				FarmRoot instance = FarmRoot.Instance;
				if (instance.IsVisitFriendFarm)
				{
					return false;
				}
				instance.SetActiveNotTouchObject(false);
				this.SetFacilityButton(false, instance);
				component.SetMoveFarmObject(this.farmObject);
				if (instance.farmMode == FarmRoot.FarmControlMode.EDIT)
				{
					instance.farmUI.EnableEditSaveButton(false);
				}
				return true;
			}
		}
		else
		{
			this.RemoveTouchDragEventForFriendFarm();
		}
		return false;
	}

	private bool OnFriendFarmDrag(InputControll inputControll)
	{
		return true;
	}

	public void SetSelectObject(GameObject selectObject)
	{
		FarmRoot instance = FarmRoot.Instance;
		if (instance == null)
		{
			return;
		}
		if (selectObject == null || !selectObject.activeSelf)
		{
			return;
		}
		this.farmObject = selectObject.GetComponent<FarmObject>();
		this.selectUserFacilityID = this.farmObject.userFacilityID;
		base.StartCoroutine(this.farmObject.OnTouchedEffect(false, null));
		this.CreateFacilityButton();
		this.farmObject.SetSelectMark(instance.Field, instance.SelectMark);
		instance.Input.AddTouchDragEvent(new Func<InputControll, bool>(this.OnDrag));
	}

	private void CreateFacilityButton()
	{
		FarmRoot instance = FarmRoot.Instance;
		this.SetFacilityButton(true, instance);
	}

	public void ClearSelectState()
	{
		if (this.selectUserFacilityID != -1)
		{
			this.ResetSelectedFarmObject();
		}
	}

	private void CompletedMove()
	{
		FarmObjectSetting component = base.GetComponent<FarmObjectSetting>();
		if (null != component.farmObject && component.farmObject.isPlaceable)
		{
			FarmRoot instance = FarmRoot.Instance;
			if (instance.farmMode == FarmRoot.FarmControlMode.NORMAL)
			{
				base.StartCoroutine(this.SaveFarmObjectMove());
			}
			else
			{
				instance.ResetSettingMark();
				this.farmObject.DisplayFloorObject();
				component.ComplatedSetting();
				instance.isEdit = true;
				if (instance.farmMode == FarmRoot.FarmControlMode.EDIT)
				{
					if (!instance.Scenery.IsExistStoreFacility())
					{
						instance.farmUI.EnableEditSaveButton(true);
					}
					instance.farmUI.EnableEditStoreButton(true);
				}
			}
		}
	}

	private IEnumerator SaveFarmObjectMove()
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		FarmField farmField = farmRoot.Field;
		FarmObjectSetting farmObjectSetting = base.GetComponent<FarmObjectSetting>();
		FarmGrid.GridPosition gridPosition = farmField.Grid.GetGridPosition(this.farmObject.GetBaseGridPosition3D());
		FarmGrid.GridPosition gridBackupPosition = farmField.Grid.GetGridPosition(this.farmObject.GetBackupGridPosition3D());
		if (gridPosition.x != gridBackupPosition.x || gridPosition.y != gridBackupPosition.y)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_OFF);
			RequestFA_FacilityMoving request = new RequestFA_FacilityMoving
			{
				SetSendData = delegate(FacilityMoving param)
				{
					param.userFacilityId = this.farmObject.userFacilityID;
					param.positionX = gridPosition.x;
					param.positionY = gridPosition.y;
				},
				OnReceived = delegate(WebAPI.ResponseData response)
				{
					UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.farmObject.userFacilityID);
					userFacility.positionX = gridPosition.x;
					userFacility.positionY = gridPosition.y;
				}
			};
			yield return base.StartCoroutine(request.Run(delegate()
			{
				RestrictionInput.EndLoad();
				this.MoveComplate(farmRoot, farmObjectSetting);
			}, null, null));
		}
		else
		{
			this.MoveComplate(farmRoot, farmObjectSetting);
		}
		yield break;
	}

	private void MoveComplate(FarmRoot farmRoot, FarmObjectSetting farmObjectSetting)
	{
		farmRoot.ResetSettingMark();
		this.farmObject.DisplayFloorObject();
		farmRoot.SetActiveNotTouchObject(true);
		this.SetFacilityButton(true, farmRoot);
		farmObjectSetting.ComplatedSetting();
	}

	public void ResetSelectedFarmObject()
	{
		FarmRoot instance = FarmRoot.Instance;
		this.SetFacilityButton(false, instance);
		instance.ResetSelectMark();
		FarmObjectSetting component = base.GetComponent<FarmObjectSetting>();
		if (null != component.farmObject)
		{
			component.CancelMove();
			instance.ResetSettingMark();
			this.farmObject.DisplayFloorObject();
			if (instance.farmMode == FarmRoot.FarmControlMode.NORMAL)
			{
				instance.SetActiveNotTouchObject(true);
			}
			else
			{
				if (!instance.Scenery.IsExistStoreFacility())
				{
					instance.farmUI.EnableEditSaveButton(true);
				}
				instance.farmUI.EnableEditStoreButton(true);
			}
		}
		this.selectUserFacilityID = -1;
		this.farmObject = null;
		instance.Input.RemoveTouchDragEvent(new Func<InputControll, bool>(this.OnDrag));
		this.RemoveTouchDragEventForFriendFarm();
	}

	private void SetFacilityButton(bool enable, FarmRoot farmRoot)
	{
		if (farmRoot.farmMode == FarmRoot.FarmControlMode.NORMAL)
		{
			if (enable)
			{
				farmRoot.farmUI.CreateFacilityButton(this.farmObject);
			}
			else
			{
				farmRoot.farmUI.DeleteFacilityButton();
			}
		}
	}
}
