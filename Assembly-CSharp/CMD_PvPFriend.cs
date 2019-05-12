using Master;
using MultiBattle.Tools;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CMD_PvPFriend : CMD
{
	[Header("リストパーツ")]
	[SerializeField]
	private GameObject partFriendParent;

	[SerializeField]
	private GameObject partFriendList;

	[Header("デフォルトメッセージ")]
	[SerializeField]
	private GameObject goDefaultMessage;

	[SerializeField]
	private UILabel lbDefaultMessage;

	[Header("誘うボタン")]
	[SerializeField]
	private GameObject goBtnRecruit;

	[SerializeField]
	private UILabel lbBtnRecruit;

	[SerializeField]
	private UISprite spBtnRecruit;

	[SerializeField]
	private BoxCollider coBtnRecruit;

	private GUISelectPvPFriendPanel csPartFriendParent;

	private List<GameWebAPI.FriendList> friendList;

	private string pvpMockTargetUserCode;

	private static CMD_PvPFriend instance;

	public static CMD_PvPFriend Instance
	{
		get
		{
			return CMD_PvPFriend.instance;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		CMD_PvPFriend.instance = this;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		MultiTools.DispLoading(true, RestrictionInput.LoadType.SMALL_IMAGE_MASK_OFF);
		base.HideDLG();
		base.StartCoroutine(this.InitFriendUI(f, sizeX, sizeY, aT));
	}

	private IEnumerator InitFriendUI(Action<int> f, float sizeX, float sizeY, float aT)
	{
		APIRequestTask task = APIUtil.Instance().RequestFriendData(false);
		yield return base.StartCoroutine(task.Run(delegate
		{
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
		this.csPartFriendParent = this.partFriendParent.GetComponent<GUISelectPvPFriendPanel>();
		this.csPartFriendParent.selectParts = this.partFriendList;
		this.csPartFriendParent.ListWindowViewRect = ConstValue.GetRectWindow3();
		this.lbDefaultMessage.text = StringMaster.GetString("MultiRecruitFriend-01");
		this.lbBtnRecruit.text = StringMaster.GetString("MultiRecruitFriend-02");
	}

	private void InitFriendList()
	{
		this.friendList = DataMng.Instance().RespDataFR_FriendList.friendList.ToList<GameWebAPI.FriendList>();
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
		}
	}

	public void UnSelectedAnother(GUIListPvPFriendParts part)
	{
		this.csPartFriendParent.UnSelectedAnother(part);
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
		foreach (GUIListPvPFriendParts guilistPvPFriendParts in this.csPartFriendParent.FriendPartsList)
		{
			if (guilistPvPFriendParts.IsSelected)
			{
				btnRecruit = true;
			}
		}
		this.SetBtnRecruit(btnRecruit);
	}

	private void OnClickedPvPFriend()
	{
		MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		int sendUserId = 0;
		foreach (GUIListPvPFriendParts guilistPvPFriendParts in this.csPartFriendParent.FriendPartsList)
		{
			if (guilistPvPFriendParts.IsSelected)
			{
				sendUserId = int.Parse(guilistPvPFriendParts.FriendData.userData.userId);
				break;
			}
		}
		if (sendUserId == 0)
		{
			return;
		}
		GameWebAPI.RequestUS_UserProfile request = new GameWebAPI.RequestUS_UserProfile
		{
			SetSendData = delegate(GameWebAPI.PRF_Req_ProfileData param)
			{
				param.targetUserId = sendUserId;
			},
			OnReceived = delegate(GameWebAPI.RespDataPRF_Profile resData)
			{
				if (resData.userData != null)
				{
					this.OnClickedPvPFriendExec(resData.userData.userCode.Replace(" ", string.Empty));
				}
			}
		};
		base.StartCoroutine(request.RunOneTime(new Action(RestrictionInput.EndLoad), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void OnClickedPvPFriendExec(string userCode)
	{
		this.pvpMockTargetUserCode = userCode;
		GameWebAPI.RespData_ColosseumMatchingValidateLogic responseData = null;
		GameWebAPI.ColosseumMatchingValidateLogic request = new GameWebAPI.ColosseumMatchingValidateLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_ColosseumMatchingValidateLogic param)
			{
				param.act = 1;
				param.isMockBattle = 1;
				param.targetUserCode = userCode;
			},
			OnReceived = delegate(GameWebAPI.RespData_ColosseumMatchingValidateLogic response)
			{
				responseData = response;
			}
		};
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.EndValidate(responseData);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void EndValidate(GameWebAPI.RespData_ColosseumMatchingValidateLogic data)
	{
		MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		int resultCode = data.resultCode;
		if (resultCode != 1)
		{
			if (resultCode != 92)
			{
				AlertManager.ShowModalMessage(delegate(int modal)
				{
				}, StringMaster.GetString("ColosseumMockNotFoundTitle"), StringMaster.GetString("ColosseumMockNotFoundInfo"), AlertManager.ButtonActionType.Close, false);
			}
			else
			{
				AlertManager.ShowModalMessage(delegate(int modal)
				{
				}, StringMaster.GetString("ColosseumMockLockTitle"), StringMaster.GetString("ColosseumMockLockInfo"), AlertManager.ButtonActionType.Close, false);
			}
		}
		else
		{
			global::Debug.Log("対戦相手UserCode: " + this.pvpMockTargetUserCode);
			ClassSingleton<MultiBattleData>.Instance.MockBattleUserCode = this.pvpMockTargetUserCode;
			ClassSingleton<QuestData>.Instance.SelectDungeon = null;
			CMD_PartyEdit.ModeType = CMD_PartyEdit.MODE_TYPE.PVP;
			GUIMain.ShowCommonDialog(null, "CMD_PartyEdit");
		}
	}
}
