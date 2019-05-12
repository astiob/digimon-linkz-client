using Master;
using System;
using UnityEngine;

public class FarmEditFooter : MonoBehaviour
{
	[SerializeField]
	private FarmEditFacilityList facilityList;

	[SerializeField]
	private GameObject warningMessage;

	[SerializeField]
	private UILabel warningMessageLabel;

	[SerializeField]
	private GameObject saveButton;

	[SerializeField]
	private GameObject storeButton;

	private bool enableSaveButton = true;

	private bool enableStoreButton = true;

	private void Start()
	{
		this.facilityList.Initialize();
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			instance.ClearSettingFarmObject();
			instance.farmMode = FarmRoot.FarmControlMode.EDIT;
			instance.isEdit = false;
			instance.SetActiveNotTouchObject(false);
			GUIFaceIndicator.instance.HideLocator(false);
		}
		this.warningMessageLabel.text = StringMaster.GetString("FarmEditInfo");
		this.EnableSaveButton(false);
		GUIScreenHome.enableBackKeyAndroid = false;
	}

	private void OnDestroy()
	{
		GUIScreenHome.enableBackKeyAndroid = true;
	}

	private void Close()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			instance.ClearSettingFarmObject();
			instance.farmMode = FarmRoot.FarmControlMode.NORMAL;
			instance.isEdit = false;
			instance.SetActiveNotTouchObject(true);
			GUIFaceIndicator.instance.ShowLocator();
		}
		GUIFace.instance.ShowGUI();
		GUIFaceIndicator.instance.ShowLocator();
	}

	public void OnPushedCloseButton()
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			if (instance.isEdit)
			{
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnPushedCloseYesButton), "CMD_Confirm") as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("FarmEditCloseTitle");
				cmd_Confirm.Info = StringMaster.GetString("FarmEditCloseInfo");
			}
			else
			{
				this.Close();
			}
		}
		else
		{
			this.Close();
		}
	}

	private void UpdateAndroidBackKey()
	{
		if (GUIManager.IsEnableBackKeyAndroid() && Input.GetKeyDown(KeyCode.Escape))
		{
			this.OnPushedCloseButton();
		}
	}

	private void OnPushedCloseYesButton(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			FarmRoot instance = FarmRoot.Instance;
			if (null != instance)
			{
				this.Close();
				instance.Scenery.ResumeFarmObject();
			}
		}
	}

	public void EnableSaveButton(bool enable)
	{
		GUICollider component = this.saveButton.GetComponent<GUICollider>();
		UISprite[] componentsInChildren = component.GetComponentsInChildren<UISprite>();
		float colorRate = 1f;
		if (enable)
		{
			component.CallBackClass = base.gameObject;
			component.touchBehavior = GUICollider.TouchBehavior.ToLarge;
			if (!this.enableSaveButton)
			{
				colorRate = 2.5f;
			}
		}
		else
		{
			component.CallBackClass = null;
			component.touchBehavior = GUICollider.TouchBehavior.None;
			if (this.enableSaveButton)
			{
				colorRate = 0.4f;
			}
		}
		this.enableSaveButton = enable;
		this.SetButtonColor(componentsInChildren, colorRate);
	}

	public void EnableStoreButton(bool enable)
	{
		GUICollider component = this.storeButton.GetComponent<GUICollider>();
		this.EnableButton(component, enable);
		UISprite[] componentsInChildren = component.GetComponentsInChildren<UISprite>();
		if (enable)
		{
			if (!this.enableStoreButton)
			{
				this.SetButtonColor(componentsInChildren, 2.5f);
			}
		}
		else if (this.enableStoreButton)
		{
			this.SetButtonColor(componentsInChildren, 0.4f);
		}
		this.enableStoreButton = enable;
	}

	private void EnableButton(GUICollider guiCollider, bool enable)
	{
		if (enable)
		{
			guiCollider.CallBackClass = base.gameObject;
			guiCollider.touchBehavior = GUICollider.TouchBehavior.ToLarge;
		}
		else
		{
			guiCollider.CallBackClass = null;
			guiCollider.touchBehavior = GUICollider.TouchBehavior.None;
		}
	}

	private void SetButtonColor(UISprite[] sprites, float colorRate)
	{
		foreach (UISprite uisprite in sprites)
		{
			Color color = uisprite.color;
			color.r = Mathf.Clamp01(color.r * colorRate);
			color.g = Mathf.Clamp01(color.g * colorRate);
			color.b = Mathf.Clamp01(color.b * colorRate);
			uisprite.color = color;
		}
	}

	private void OnPushedSaveButton()
	{
		if (this.facilityList.ExistButton())
		{
			global::Debug.LogError("Exist StoreFacility");
			return;
		}
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnPushedSaveYesButton), "CMD_Confirm") as CMD_Confirm;
		cmd_Confirm.Title = StringMaster.GetString("FarmEditSaveTitle");
		cmd_Confirm.Info = StringMaster.GetString("FarmEditSaveInfo");
	}

	private void OnPushedSaveYesButton(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			FarmRoot instance = FarmRoot.Instance;
			if (null != instance)
			{
				base.StartCoroutine(instance.Scenery.SaveEdit(new Action<bool>(this.RequestCompleted)));
			}
		}
	}

	private void RequestCompleted(bool isSuccess)
	{
		if (isSuccess)
		{
			this.Close();
		}
	}

	private void OnPushedStoreButton()
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null == instance)
		{
			global::Debug.LogError("FarmRoot Not Found");
			return;
		}
		FarmScenery scenery = instance.Scenery;
		instance.isEdit = true;
		instance.ClearSettingFarmObject();
		scenery.AllStoreFarmObject();
		this.facilityList.CreateStoreFacilityButton();
		this.EnableSaveButton(false);
		this.EnableStoreButton(false);
	}

	public void AddFacilityButton(FarmObject farmObject)
	{
		FarmEditFacilityButton samFacilityButton = this.facilityList.GetSamFacilityButton(farmObject);
		if (null != samFacilityButton)
		{
			samFacilityButton.CountUp(farmObject);
		}
		else
		{
			this.facilityList.AddStoreFacilityButton(farmObject);
		}
	}

	public FarmObject GetRestStoreFacility(FarmObject findFarmObject)
	{
		FarmEditFacilityButton samFacilityButton = this.facilityList.GetSamFacilityButton(findFarmObject);
		if (null == samFacilityButton)
		{
			return null;
		}
		FarmObject farmObject = samFacilityButton.GetFarmObject();
		if (null == farmObject)
		{
			global::Debug.LogError("FarmObject == 0");
			return null;
		}
		this.facilityList.DecrementFacilityCount(farmObject);
		return farmObject;
	}

	private void Update()
	{
		if (Input.touchCount > 0 || Input.GetMouseButton(0))
		{
			this.warningMessage.SetActive(false);
		}
		this.UpdateAndroidBackKey();
	}
}
