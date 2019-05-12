using Master;
using MultiBattle.Tools;
using System;
using System.Collections;
using UnityEngine;

public class CMD_MultiRecruitChatList : CMD
{
	[Header("リストパーツ")]
	[SerializeField]
	private GameObject partChatListParent;

	[SerializeField]
	private GameObject partChatList;

	[SerializeField]
	[Header("デフォルトメッセージ")]
	private GameObject goDefaultMessage;

	[SerializeField]
	private UILabel lbDefaultMessage;

	[SerializeField]
	[Header("全選択ボタン")]
	private GameObject goBtnSelectAll;

	[SerializeField]
	private UILabel lbBtnSelectAll;

	[SerializeField]
	private UISprite spBtnSelectAll;

	[SerializeField]
	[Header("誘うボタン")]
	private GameObject goBtnRecruit;

	[SerializeField]
	private UILabel lbBtnRecruit;

	[SerializeField]
	private UISprite spBtnRecruit;

	[SerializeField]
	private BoxCollider coBtnRecruit;

	private static CMD_MultiRecruitChatList instance;

	private bool isSelectedAll;

	private GUISelectChatGroupPanel csPartChatListParent;

	public static CMD_MultiRecruitChatList Instance
	{
		get
		{
			return CMD_MultiRecruitChatList.instance;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		CMD_MultiRecruitChatList.instance = this;
		base.DontLookParent = true;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		this.InitChatListUI(f, sizeX, sizeY, aT);
	}

	private void InitChatListUI(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GameWebAPI.UserChatGroupList userChatGroupList = new GameWebAPI.UserChatGroupList();
		userChatGroupList.OnReceived = delegate(GameWebAPI.RespData_UserChatGroupList response)
		{
			ClassSingleton<ChatData>.Instance.CurrentChatInfo.joinGroupData = response;
		};
		GameWebAPI.UserChatGroupList request = userChatGroupList;
		AppCoroutine.Start(request.RunOneTime(delegate()
		{
			this.SetCommonUI();
			this.Show(f, sizeX, sizeY, aT);
			RestrictionInput.EndLoad();
		}, delegate(Exception noop)
		{
			this.ClosePanel(false);
			RestrictionInput.EndLoad();
		}, null), false);
	}

	private void SetCommonUI()
	{
		this.csPartChatListParent = this.partChatListParent.GetComponent<GUISelectChatGroupPanel>();
		this.csPartChatListParent.selectParts = this.partChatList;
		this.partChatListParent.SetActive(true);
		this.partChatList.SetActive(true);
		this.csPartChatListParent.initLocation = true;
		this.csPartChatListParent.AllBuild(ClassSingleton<ChatData>.Instance.CurrentChatInfo.joinGroupData);
		this.partChatList.SetActive(false);
		this.lbDefaultMessage.text = StringMaster.GetString("MultiRecruitChat-01");
		this.lbBtnRecruit.text = StringMaster.GetString("MultiRecruitChat-02");
		this.SetBtnSelectAll(this.isSelectedAll);
		if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.joinGroupData.groupList == null)
		{
			this.goDefaultMessage.SetActive(true);
			this.goBtnRecruit.SetActive(false);
			this.goBtnSelectAll.SetActive(false);
		}
	}

	private void SetBtnRecruit(bool isEnable)
	{
		this.coBtnRecruit.enabled = isEnable;
		if (isEnable)
		{
			this.spBtnRecruit.spriteName = "Common02_Btn_Green";
		}
		else
		{
			this.spBtnRecruit.spriteName = "Common02_Btn_Gray";
		}
	}

	private void SetBtnSelectAll(bool isSelectedAll)
	{
		if (isSelectedAll)
		{
			this.lbBtnSelectAll.text = StringMaster.GetString("MultiRecruitChat-04");
			this.lbBtnSelectAll.color = ConstValue.DEFAULT_COLOR;
			this.spBtnSelectAll.spriteName = "Common02_Btn_SupportWhite";
		}
		else
		{
			this.lbBtnSelectAll.text = StringMaster.GetString("MultiRecruitChat-03");
			this.lbBtnSelectAll.color = Color.white;
			this.spBtnSelectAll.spriteName = "Common02_Btn_SupportRed";
		}
	}

	private void OnClickedSelectAll()
	{
		this.isSelectedAll = !this.isSelectedAll;
		foreach (GUIListChatGroupParts guilistChatGroupParts in this.csPartChatListParent.chatPartsList)
		{
			guilistChatGroupParts.ForceSelect(this.isSelectedAll);
		}
		this.SetBtnSelectAll(this.isSelectedAll);
		this.SetBtnRecruit(this.isSelectedAll);
	}

	private void OnClickedRecruit()
	{
		base.StartCoroutine(this.SendMultiRecruitChat());
	}

	private IEnumerator SendMultiRecruitChat()
	{
		MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		foreach (GUIListChatGroupParts chat in this.csPartChatListParent.chatPartsList)
		{
			if (chat.isSelected)
			{
				base.StartCoroutine(Singleton<TCPUtil>.Instance.SendChatMessage(int.Parse(chat.Data.chatGroupId), string.Concat(new string[]
				{
					CMD_MultiRecruitPartyWait.roomCreateData.multiRoomInfo.worldStageId,
					",",
					CMD_MultiRecruitPartyWait.roomCreateData.multiRoomInfo.worldDungeonId,
					",",
					CMD_MultiRecruitPartyWait.roomCreateData.multiRoomInfo.multiRoomId
				}), 4, null));
				yield return null;
			}
		}
		MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		yield return null;
		CMD_ModalMessage cd = GUIMain.ShowCommonDialog(new Action<int>(this.CloseModalMessage), "CMD_ModalMessage") as CMD_ModalMessage;
		cd.Title = StringMaster.GetString("MultiRecruitChat-05");
		cd.Info = StringMaster.GetString("MultiRecruitChat-06");
		yield break;
	}

	private void CloseModalMessage(int idx)
	{
		base.ClosePanel(true);
	}

	public void CheckEnableBtnRecruit()
	{
		bool btnRecruit = false;
		int num = 0;
		foreach (GUIListChatGroupParts guilistChatGroupParts in this.csPartChatListParent.chatPartsList)
		{
			if (guilistChatGroupParts.isSelected)
			{
				btnRecruit = true;
				num++;
			}
		}
		if (num == 0)
		{
			this.isSelectedAll = false;
			this.SetBtnSelectAll(this.isSelectedAll);
		}
		else if (num == this.csPartChatListParent.chatPartsList.Count)
		{
			this.isSelectedAll = true;
			this.SetBtnSelectAll(this.isSelectedAll);
		}
		this.SetBtnRecruit(btnRecruit);
	}
}
