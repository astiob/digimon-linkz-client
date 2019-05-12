using Master;
using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CMD_MultiRecruitFriend : CMD
{
	[SerializeField]
	[Header("リストパーツ")]
	private GameObject partFriendParent;

	[SerializeField]
	private GameObject partFriendList;

	[Header("デフォルトメッセージ")]
	[SerializeField]
	private GameObject goDefaultMessage;

	[SerializeField]
	private UILabel lbDefaultMessage;

	[Header("全選択ボタン")]
	[SerializeField]
	private GameObject goBtnSelectAll;

	[SerializeField]
	private UILabel lbBtnSelectAll;

	[SerializeField]
	private UISprite spBtnSelectAll;

	[Header("誘うボタン")]
	[SerializeField]
	private GameObject goBtnRecruit;

	[SerializeField]
	private UILabel lbBtnRecruit;

	[SerializeField]
	private UISprite spBtnRecruit;

	[SerializeField]
	private BoxCollider coBtnRecruit;

	private GUISelectMultiRecruitFriendPanel csPartFriendParent;

	private List<GameWebAPI.FriendList> friendList;

	private List<int> sendUserList;

	private bool isSelectedAll;

	private static CMD_MultiRecruitFriend instance;

	public int roomId { get; set; }

	public static CMD_MultiRecruitFriend Instance
	{
		get
		{
			return CMD_MultiRecruitFriend.instance;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		CMD_MultiRecruitFriend.instance = this;
		base.DontLookParent = true;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		base.StartCoroutine(this.InitFriendUI(f, sizeX, sizeY, aT));
	}

	private IEnumerator InitFriendUI(Action<int> f, float sizeX, float sizeY, float aT)
	{
		APIRequestTask task = APIUtil.Instance().RequestFriendData(false);
		yield return base.StartCoroutine(task.Run(delegate
		{
			base.ShowDLG();
			this.SetCommonUI();
			this.InitFriendList();
			base.Show(f, sizeX, sizeY, aT);
		}, delegate(Exception noop)
		{
			base.ClosePanel(false);
		}, null));
		MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		yield break;
	}

	private void SetCommonUI()
	{
		this.csPartFriendParent = this.partFriendParent.GetComponent<GUISelectMultiRecruitFriendPanel>();
		this.csPartFriendParent.selectParts = this.partFriendList;
		this.csPartFriendParent.ListWindowViewRect = ConstValue.GetRectWindow3();
		this.lbDefaultMessage.text = StringMaster.GetString("MultiRecruitFriend-01");
		this.lbBtnRecruit.text = StringMaster.GetString("MultiRecruitFriend-02");
		this.SetBtnSelectAll(this.isSelectedAll);
	}

	private void InitFriendList()
	{
		List<string> recruitedFriendIdList = CMD_MultiRecruitPartyWait.Instance.recruitedFriendIdList;
		this.friendList = DataMng.Instance().RespDataFR_FriendList.friendList.ToList<GameWebAPI.FriendList>();
		if (recruitedFriendIdList.Count > 0)
		{
			int i;
			for (i = 0; i < recruitedFriendIdList.Count; i++)
			{
				this.friendList.RemoveAll((GameWebAPI.FriendList f) => f.userData.userId.Equals(recruitedFriendIdList[i].ToString()));
			}
		}
		if (this.friendList.Count > 0)
		{
			this.goDefaultMessage.SetActive(false);
			this.partFriendParent.SetActive(true);
			this.partFriendList.SetActive(true);
			this.csPartFriendParent.initLocation = true;
			this.csPartFriendParent.AllBuild(this.friendList);
			this.partFriendList.SetActive(false);
		}
		else
		{
			this.partFriendList.SetActive(false);
			this.goDefaultMessage.SetActive(true);
			this.goBtnRecruit.SetActive(false);
			this.goBtnSelectAll.SetActive(false);
		}
	}

	private void OnClickedSelectAll()
	{
		this.isSelectedAll = !this.isSelectedAll;
		foreach (GUIListMultiRecruitFriendParts guilistMultiRecruitFriendParts in this.csPartFriendParent.friendPartsList)
		{
			guilistMultiRecruitFriendParts.ForceSelect(this.isSelectedAll);
		}
		this.SetBtnSelectAll(this.isSelectedAll);
		this.SetBtnRecruit(this.isSelectedAll);
	}

	private void SetBtnSelectAll(bool isSelectedAll)
	{
		if (isSelectedAll)
		{
			this.lbBtnSelectAll.text = StringMaster.GetString("MultiRecruitFriend-04");
			this.lbBtnSelectAll.color = ConstValue.DEFAULT_COLOR;
			this.spBtnSelectAll.spriteName = "Common02_Btn_SupportWhite";
		}
		else
		{
			this.lbBtnSelectAll.text = StringMaster.GetString("MultiRecruitFriend-03");
			this.lbBtnSelectAll.color = Color.white;
			this.spBtnSelectAll.spriteName = "Common02_Btn_SupportRed";
		}
	}

	private void OnClickedRecruit()
	{
		MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		this.sendUserList = new List<int>();
		foreach (GUIListMultiRecruitFriendParts guilistMultiRecruitFriendParts in this.csPartFriendParent.friendPartsList)
		{
			if (guilistMultiRecruitFriendParts.isSelected)
			{
				this.sendUserList.Add(int.Parse(guilistMultiRecruitFriendParts.friendData.userData.userId));
				CMD_MultiRecruitPartyWait.Instance.recruitedFriendIdList.Add(guilistMultiRecruitFriendParts.friendData.userData.userId);
			}
		}
		this.SendMultiRecruitFriend();
	}

	private void SendMultiRecruitFriend()
	{
		GameWebAPI.MultiRoomRequestRegist request = new GameWebAPI.MultiRoomRequestRegist
		{
			SetSendData = delegate(GameWebAPI.ReqData_MultiRoomRequestRegist param)
			{
				param.roomId = this.roomId;
				param.userList = this.sendUserList;
			}
		};
		base.StartCoroutine(request.Run(delegate()
		{
			RestrictionInput.EndLoad();
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(new Action<int>(this.CloseModalMessage), "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("MultiRecruitFriend-05");
			cmd_ModalMessage.Info = StringMaster.GetString("MultiRecruitFriend-06");
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void CloseModalMessage(int idx)
	{
		base.ClosePanel(true);
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

	public void CheckEnableBtnRecruit()
	{
		bool btnRecruit = false;
		int num = 0;
		foreach (GUIListMultiRecruitFriendParts guilistMultiRecruitFriendParts in this.csPartFriendParent.friendPartsList)
		{
			if (guilistMultiRecruitFriendParts.isSelected)
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
		else if (num == this.csPartFriendParent.friendPartsList.Count)
		{
			this.isSelectedAll = true;
			this.SetBtnSelectAll(this.isSelectedAll);
		}
		this.SetBtnRecruit(btnRecruit);
	}
}
