using Master;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class CMD_TitleSelect : CMD
{
	private const string CMD_NAME = "CMD_TitleSelect";

	private int currentSelectedTitleId;

	[SerializeField]
	private GUISelectPanelTitle titleList;

	[SerializeField]
	private GameObject titleListOriginalItem;

	[SerializeField]
	private UILabel titleName;

	[SerializeField]
	private UILabel titleNameLbl;

	[SerializeField]
	private UITexture titleIcon;

	[SerializeField]
	private UILabel titleDetail;

	[SerializeField]
	private GameObject equipBtn;

	[SerializeField]
	private UILabel equipBtnLbl;

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		AppCoroutine.Start(this.Init(f, sizeX, sizeY, aT), false);
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		GUICollider.EnableAllCollider("CMD_TitleSelect");
	}

	public override void ClosePanel(bool animation = true)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	private IEnumerator Init(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_TitleSelect");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		base.PartsTitle.SetTitle(StringMaster.GetString("EditUserTitle"));
		this.titleIcon.mainTexture = null;
		this.titleNameLbl.text = string.Empty;
		this.titleName.text = string.Empty;
		this.titleDetail.text = string.Empty;
		this.equipBtnLbl.text = StringMaster.GetString("EquipUserTitle");
		this.equipBtn.GetComponent<UISprite>().spriteName = "Common02_Btn_Gray";
		this.equipBtnLbl.color = ConstValue.DEACTIVE_BUTTON_LABEL;
		this.equipBtn.GetComponent<BoxCollider>().enabled = false;
		this.titleListOriginalItem.SetActive(true);
		GameWebAPI.RespDataMA_TitleMaster.TitleM[] titles = TitleDataMng.GetAvailableTitleM();
		this.CreateTitleList(this.titleList, titles.Length);
		this.SetTitleDetail(this.titleList, titles);
		this.titleListOriginalItem.SetActive(false);
		base.ShowDLG();
		base.Show(f, sizeX, sizeY, aT);
		RestrictionInput.EndLoad();
		yield return null;
		yield break;
	}

	private void FakeMethod()
	{
	}

	private void CreateTitleList(GUISelectPanelTitle listUI, int listItemCount)
	{
		GUICollider component = listUI.GetComponent<GUICollider>();
		BoxCollider component2 = component.GetComponent<BoxCollider>();
		Vector3 localPosition = component.transform.localPosition;
		component.SetOriginalPos(this.titleListOriginalItem.transform.localPosition);
		component.transform.localPosition = localPosition;
		Rect rect = default(Rect);
		Rect rect2 = rect;
		rect2.xMin = component2.size.x * -0.5f;
		rect2.xMax = component2.size.x * 0.5f;
		rect2.yMin = component2.size.y * -0.5f - 40f;
		rect2.yMax = component2.size.y * 0.5f;
		rect = rect2;
		rect.yMin = rect.y - GUIMain.VerticalSpaceSize;
		rect.yMax = rect.y + rect.height + GUIMain.VerticalSpaceSize;
		listUI.ListWindowViewRect = rect;
		listUI.selectParts = this.titleListOriginalItem;
		listUI.initLocation = true;
		listUI.AllBuild(listItemCount);
	}

	private void SetTitleDetail(GUISelectPanelTitle listUI, GameWebAPI.RespDataMA_TitleMaster.TitleM[] titleData)
	{
		TitleListItem[] componentsInChildren = listUI.GetComponentsInChildren<TitleListItem>();
		if (componentsInChildren == null)
		{
			return;
		}
		GameWebAPI.RespDataTL_GetUserTitleList.UserTitleList equipedUserTitle = TitleDataMng.GetEquipedUserTitle();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			bool flag = equipedUserTitle != null && equipedUserTitle.titleId == titleData[i].titleId;
			GameWebAPI.RespDataTL_GetUserTitleList.UserTitleList userTitleByMasterId = TitleDataMng.GetUserTitleByMasterId(titleData[i].titleId);
			componentsInChildren[i].SetDetail(titleData[i], userTitleByMasterId != null, flag, new Action<TitleListItem>(this.OnSelectTitle));
			if (i == 0 || flag)
			{
				componentsInChildren[i].OnSelectTitle();
			}
		}
	}

	public void OnSelectTitle(TitleListItem titleItem)
	{
		GameWebAPI.RespDataMA_TitleMaster.TitleM titleM = TitleDataMng.GetDictionaryTitleM()[titleItem.GetTitleId()];
		this.titleName.text = titleM.name;
		this.titleDetail.text = titleM.detail;
		TitleDataMng.SetTitleIcon(titleM.titleId, this.titleIcon);
		if (titleItem.GetEquiped() || !titleItem.GetOwned())
		{
			this.equipBtn.GetComponent<UISprite>().spriteName = "Common02_Btn_Gray";
			this.equipBtnLbl.color = ConstValue.DEACTIVE_BUTTON_LABEL;
			this.equipBtn.GetComponent<BoxCollider>().enabled = false;
		}
		else
		{
			this.equipBtn.GetComponent<UISprite>().spriteName = "Common02_Btn_Green";
			this.equipBtnLbl.color = Color.white;
			this.equipBtn.GetComponent<BoxCollider>().enabled = true;
		}
		if (titleItem.GetEquiped())
		{
			this.titleNameLbl.text = StringMaster.GetString("CurrentUserTitle");
		}
		else
		{
			this.titleNameLbl.text = StringMaster.GetString("SelectedUserTitle");
		}
		this.currentSelectedTitleId = titleItem.GetTitleId();
	}

	public void OnEquipTitle()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		TitleListItem[] titles = this.titleList.GetComponentsInChildren<TitleListItem>();
		TitleListItem currentEquipedTitle = titles.FirstOrDefault((TitleListItem _title) => _title.GetTitleId() == this.currentSelectedTitleId);
		APIRequestTask apirequestTask = new APIRequestTask();
		apirequestTask.Add(TitleDataMng.RequestUpdateEquipedTitle(this.currentSelectedTitleId, false));
		base.StartCoroutine(apirequestTask.Run(delegate
		{
			RestrictionInput.EndLoad();
			TitleListItem[] titles;
			foreach (TitleListItem titleListItem in titles)
			{
				titleListItem.unequip();
			}
			currentEquipedTitle.equip();
			currentEquipedTitle.OnSelectTitle();
			GUIPlayerStatus.RefreshParams_S(false);
			CMD_Profile.RefreshParams();
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}
}
