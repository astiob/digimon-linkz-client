using System;
using UnityEngine;

public class FarmObjectEdit : MonoBehaviour
{
	private FarmObject farmObject;

	public void StartEdit(FarmObject farmObject)
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmObjectSetting settingObject = instance.SettingObject;
		FarmObjectSelect selectObject = instance.SelectObject;
		this.farmObject = farmObject;
		this.farmObject.SetSelectMark(instance.Field, instance.SelectMark);
		this.farmObject.SetSettingMark(instance.Field, instance.SettingMark);
		this.farmObject.SetPlaceable(false);
		selectObject.EnabledTouchedEvent(false);
		instance.Input.AddTouchEndEvent(new Action<InputControll, bool>(this.OnTouchUp));
		instance.Input.AddTouchDragEvent(new Func<InputControll, bool>(settingObject.OnDrag));
	}

	private void OnTouchUp(InputControll inputControll, bool isDraged)
	{
		if (!isDraged)
		{
			if (this.farmObject.gameObject != inputControll.rayHitColliderObject)
			{
				FarmObjectSelect component = base.GetComponent<FarmObjectSelect>();
				this.CancelEdit();
				component.EnabledTouchedEvent(true);
			}
		}
		else if (this.farmObject.isPlaceable)
		{
			FarmRoot instance = FarmRoot.Instance;
			FarmScenery component2 = base.GetComponent<FarmScenery>();
			FarmObjectSelect component3 = base.GetComponent<FarmObjectSelect>();
			FarmObjectSetting component4 = base.GetComponent<FarmObjectSetting>();
			instance.ResetSettingMark();
			this.farmObject.DisplayFloorObject();
			component4.ComplatedSetting();
			component3.SetSelectObject(this.farmObject.gameObject);
			this.farmObject = null;
			component3.EnabledTouchedEvent(true);
			instance.Input.RemoveTouchEndEvent(new Action<InputControll, bool>(this.OnTouchUp));
			if (!component2.IsExistStoreFacility())
			{
				instance.farmUI.EnableEditSaveButton(true);
			}
		}
	}

	public void CancelEdit()
	{
		if (null != this.farmObject)
		{
			FarmRoot instance = FarmRoot.Instance;
			FarmScenery component = base.GetComponent<FarmScenery>();
			FarmObjectSetting component2 = base.GetComponent<FarmObjectSetting>();
			component2.CancelBuild();
			instance.ResetSettingMark();
			instance.ResetSelectMark();
			this.farmObject.DisplayFloorObject();
			component.StoreFarmObject(this.farmObject);
			instance.farmUI.AddStoreFacility(this.farmObject);
			this.farmObject = null;
			instance.Input.RemoveTouchEndEvent(new Action<InputControll, bool>(this.OnTouchUp));
		}
	}
}
