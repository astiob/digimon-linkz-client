using Master;
using MultiBattle.Tools;
using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_MultiRecruitTop : CMD
{
	public static CMD_MultiRecruitTop instance;

	[SerializeField]
	private GameObject goRecruitDefaultText;

	private UILabel lbRecruitDefaultText;

	private int worldDungeonId;

	[Header("フレンドラベル")]
	[SerializeField]
	private UILabel friendLabel;

	[Header("全国ラベル")]
	[SerializeField]
	private UILabel nationwideLebel;

	[Header("PASSラベル")]
	[SerializeField]
	private UILabel passLabel;

	[SerializeField]
	private UILabel lbUpdateBtnLabel;

	[SerializeField]
	private GameObject lbUpdateBtn;

	[SerializeField]
	private GameObject lbUpdateDisableBtn;

	[SerializeField]
	private BoxCollider lbUpdateBtnCollider;

	[SerializeField]
	private GameObject partMultiRecruitParent;

	[SerializeField]
	private GameObject partMultiRecruit;

	[SerializeField]
	private GameObject goBtnRecruitFriend;

	[SerializeField]
	private BoxCollider btnRecruitFriendCollider;

	[SerializeField]
	private GameObject goBtnRecruitAll;

	[SerializeField]
	private BoxCollider btnRecruitAllCollider;

	[SerializeField]
	private UISprite spriteRecruitFriendsBtn;

	[SerializeField]
	private UISprite spriteRecruitFriendsIcon;

	[SerializeField]
	private UILabel labelRecruitFriends;

	[SerializeField]
	private UISprite spriteRecruitAllBtn;

	[SerializeField]
	private UISprite spriteRecruitAllIcon;

	[SerializeField]
	private UILabel labelRecruitAll;

	[SerializeField]
	private Color colorRecruitDisable;

	[SerializeField]
	private Color colorRecruitDefault;

	[SerializeField]
	private GameObject friendAlertObject;

	public GameWebAPI.RespData_MultiRoomJoin passInputJoinData;

	private GUISelectMultiRecruitListPanel csMultiRecruitListPanel;

	private List<string> excludeRoomIdList;

	public bool isRecruitListLock { get; set; }

	protected override void Awake()
	{
		base.Awake();
		CMD_MultiRecruitTop.instance = this;
	}

	protected override void WindowClosed()
	{
		base.WindowClosed();
		CMD_MultiRecruitTop.instance = null;
	}

	public static CMD_MultiRecruitTop Create()
	{
		CMD_MultiRecruitTop cmd_MultiRecruitTop = GUIMain.ShowCommonDialog(null, "CMD_MultiRecruitTop") as CMD_MultiRecruitTop;
		cmd_MultiRecruitTop.SetMultiRecruitList(0);
		return cmd_MultiRecruitTop;
	}

	public static CMD_MultiRecruitTop CreateTargetStage(string worldDungeonId)
	{
		CMD_MultiRecruitTop cmd_MultiRecruitTop = GUIMain.ShowCommonDialog(null, "CMD_MultiRecruitTop") as CMD_MultiRecruitTop;
		cmd_MultiRecruitTop.worldDungeonId = int.Parse(worldDungeonId);
		cmd_MultiRecruitTop.SetMultiRecruitList(0);
		return cmd_MultiRecruitTop;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.SetCommonUI();
		base.SetTutorialAnyTime("anytime_second_tutorial_multi_recruite");
		base.Show(f, sizeX, sizeY, aT);
		this.SetInitLabel();
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	public override void ClosePanel(bool animation = true)
	{
		if (CMD_MultiRecruitPartyWait.Instance == null)
		{
			FarmCameraControlForCMD.On();
		}
		base.ClosePanel(animation);
	}

	private void SetInitLabel()
	{
		this.friendLabel.text = StringMaster.GetString("FriendTitle");
		this.nationwideLebel.text = StringMaster.GetString("Recruit-03");
		this.passLabel.text = StringMaster.GetString("Recruit-01");
		base.PartsTitle.SetTitle(StringMaster.GetString("RecruitTitleNationwide"));
		this.lbRecruitDefaultText = this.goRecruitDefaultText.GetComponent<UILabel>();
		this.lbRecruitDefaultText.text = StringMaster.GetString("RecruitNone");
		this.lbUpdateBtnLabel.text = StringMaster.GetString("SystemUpdate");
	}

	private void SetUpdateLock()
	{
		this.lbUpdateBtn.SetActive(false);
		this.lbUpdateDisableBtn.SetActive(true);
		this.lbUpdateBtnLabel.color = Color.gray;
		this.lbUpdateBtnCollider.enabled = false;
		this.SetRecruitBtnDisabled(true);
		base.Invoke("ReleaseUpdateLock", 3f);
	}

	private void ReleaseUpdateLock()
	{
		this.lbUpdateBtn.SetActive(true);
		this.lbUpdateDisableBtn.SetActive(false);
		this.lbUpdateBtnLabel.color = Color.white;
		this.lbUpdateBtnCollider.enabled = true;
		this.SetRecruitBtnDisabled(false);
	}

	private void SetRecruitBtnDisabled(bool flg)
	{
		if (flg)
		{
			this.spriteRecruitFriendsBtn.color = this.colorRecruitDisable;
			this.spriteRecruitFriendsIcon.color = this.colorRecruitDisable;
			this.labelRecruitFriends.color = Color.gray;
			this.spriteRecruitAllBtn.color = this.colorRecruitDisable;
			this.spriteRecruitAllIcon.color = this.colorRecruitDisable;
			this.labelRecruitAll.color = Color.gray;
		}
		else
		{
			this.spriteRecruitFriendsBtn.color = this.colorRecruitDefault;
			this.spriteRecruitFriendsIcon.color = this.colorRecruitDefault;
			this.labelRecruitFriends.color = Color.white;
			this.spriteRecruitAllBtn.color = this.colorRecruitDefault;
			this.spriteRecruitAllIcon.color = this.colorRecruitDefault;
			this.labelRecruitAll.color = Color.white;
		}
		this.btnRecruitFriendCollider.enabled = !flg;
		this.btnRecruitAllCollider.enabled = !flg;
	}

	public void ClickUpdateBtn()
	{
		this.isRecruitListLock = true;
		if (!this.goBtnRecruitAll.activeSelf)
		{
			this.SetMultiRecruitList(0);
		}
		else
		{
			this.SetMultiRecruitList(1);
		}
	}

	private void ClickRecruitAllBtn()
	{
		this.SetMultiRecruitList(0);
		base.PartsTitle.SetTitle(StringMaster.GetString("RecruitTitleNationwide"));
		this.goBtnRecruitFriend.SetActive(true);
		this.goBtnRecruitAll.SetActive(false);
	}

	private void ClickRecruitFriendBtn()
	{
		this.SetMultiRecruitList(1);
		base.PartsTitle.SetTitle(StringMaster.GetString("RecruitTitleFriend"));
		this.goBtnRecruitFriend.SetActive(false);
		this.goBtnRecruitAll.SetActive(true);
	}

	private void ClickPasswordBtn()
	{
		CMD_MultiRecruitPass cmd_MultiRecruitPass = GUIMain.ShowCommonDialog(delegate(int idx)
		{
			if (idx == 1 && this.passInputJoinData != null)
			{
				GameWebAPI.RespDataWD_GetDungeonInfo respDataWD_GetDungeonInfo = new GameWebAPI.RespDataWD_GetDungeonInfo();
				respDataWD_GetDungeonInfo = ClassSingleton<QuestData>.Instance.GetDngeonInfoByWorldId(this.passInputJoinData.multiRoomInfo.worldAreaId);
				foreach (GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo worldDungeonInfo2 in respDataWD_GetDungeonInfo.worldDungeonInfo)
				{
					foreach (GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dungeons2 in worldDungeonInfo2.dungeons)
					{
						if (dungeons2.worldDungeonId == int.Parse(this.passInputJoinData.multiRoomInfo.worldDungeonId))
						{
							ClassSingleton<PartyBossIconsAccessor>.Instance.StageEnemies = dungeons2.encountEnemies;
						}
					}
				}
				ClassSingleton<QuestData>.Instance.SelectDungeon = ClassSingleton<QuestData>.Instance.GetWorldDungeonMaster(this.passInputJoinData.multiRoomInfo.worldDungeonId);
				DataMng.Instance().GetResultUtilData().SetLastDngReq(this.passInputJoinData.multiRoomInfo.worldDungeonId, "-1", "-1");
				CMD_MultiRecruitPartyWait.UserType = CMD_MultiRecruitPartyWait.USER_TYPE.MEMBER;
				CMD_MultiRecruitPartyWait.roomJoinData = this.passInputJoinData;
				CMD_MultiRecruitPartyWait cmd_MultiRecruitPartyWait = GUIMain.ShowCommonDialog(null, "CMD_MultiRecruitPartyWait") as CMD_MultiRecruitPartyWait;
				cmd_MultiRecruitPartyWait.SetParentDialog(this);
			}
			this.passInputJoinData = null;
		}, "CMD_MultiRecruitPass") as CMD_MultiRecruitPass;
		cmd_MultiRecruitPass.SetParentDialog(this);
	}

	private void SetCommonUI()
	{
		this.csMultiRecruitListPanel = this.partMultiRecruitParent.GetComponent<GUISelectMultiRecruitListPanel>();
		this.csMultiRecruitListPanel.selectParts = this.partMultiRecruit;
		this.csMultiRecruitListPanel.ListWindowViewRect = MultiTools.MakeRecruitListRectWindow();
	}

	private void SetMultiRecruitList(int reqFrom)
	{
		this.excludeRoomIdList = new List<string>();
		this.SetUpdateLock();
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_OFF);
		GameWebAPI.ResponseData_Common_MultiRoomList data = null;
		GameWebAPI.MultiRoomList request = new GameWebAPI.MultiRoomList
		{
			SetSendData = delegate(GameWebAPI.ReqData_MultiRoomList param)
			{
				param.isFriend = reqFrom;
				param.dungeonId = this.worldDungeonId;
			},
			OnReceived = delegate(GameWebAPI.RespData_MultiRoomList response)
			{
				data = response;
			}
		};
		AppCoroutine.Start(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.UpdateMultiRecruitList(data);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null), false);
	}

	private void UpdateMultiRecruitList(GameWebAPI.ResponseData_Common_MultiRoomList data)
	{
		if (this.partMultiRecruitParent == null)
		{
			return;
		}
		this.partMultiRecruitParent.SetActive(true);
		this.partMultiRecruit.SetActive(true);
		this.csMultiRecruitListPanel.initLocation = true;
		this.csMultiRecruitListPanel.AllBuild(data, this);
		this.partMultiRecruit.SetActive(false);
		if (data.multiRoomList != null)
		{
			this.goRecruitDefaultText.SetActive(false);
		}
		else
		{
			this.goRecruitDefaultText.SetActive(true);
		}
		if (data.friendRequestFlag == 1)
		{
			this.friendAlertObject.SetActive(true);
		}
		else
		{
			this.friendAlertObject.SetActive(false);
		}
		this.isRecruitListLock = false;
		RestrictionInput.EndLoad();
	}

	public void AddExcludeRoomIdList(string roomId)
	{
		this.excludeRoomIdList.Add(roomId);
	}

	public void ReBuildMultiRecruitList()
	{
		this.partMultiRecruitParent.SetActive(true);
		this.partMultiRecruit.SetActive(true);
		this.csMultiRecruitListPanel.initLocation = true;
		this.csMultiRecruitListPanel.ReBuild(this.excludeRoomIdList, this);
		this.partMultiRecruit.SetActive(false);
		this.goRecruitDefaultText.SetActive(false);
		this.isRecruitListLock = false;
	}
}
