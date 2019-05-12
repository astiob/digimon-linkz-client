using Master;
using System;
using System.Collections;
using UnityEngine;

public sealed class CMD_ProfileFriend : CMD_ProfileBase
{
	private bool isAnimation;

	private CharacterParams monsterParam;

	[SerializeField]
	private UILabel lbName;

	[SerializeField]
	private UILabel lbLastLogin;

	[SerializeField]
	private UILabel lbDescription;

	[SerializeField]
	private UILabel lbCollection;

	[SerializeField]
	private BoxCollider colCloseBtn;

	[SerializeField]
	private BoxCollider colBackBtn;

	[SerializeField]
	private BoxCollider colLinkBtn;

	[SerializeField]
	private BoxCollider colBlockBtn;

	[SerializeField]
	private BoxCollider colVisitBtn;

	[SerializeField]
	private UISprite btnLink;

	[SerializeField]
	private UISprite btnBlock;

	[SerializeField]
	private UISprite btnVisit;

	[SerializeField]
	private UILabel lbLink;

	[SerializeField]
	private UILabel lbBlock;

	[SerializeField]
	private UILabel lbVisit;

	[SerializeField]
	private UISprite colosseumRankSprite;

	[SerializeField]
	private GameObject colosseumNoneDataObj;

	[SerializeField]
	private UILabel outComeNumLabel;

	private GameWebAPI.RespDataPRF_Profile friendProfile;

	public static GameWebAPI.FriendList friendData;

	public static GameWebAPI.ResponseData_ChatUserList.respUserList chatMemberData;

	public static GameWebAPI.Common_MessageData chatLogData;

	private int prevFriendStatus;

	private Func<string, bool, APIRequestTask>[] requestAPIs;

	protected override void Awake()
	{
		base.Awake();
		this.lbName.text = string.Empty;
		this.lbDescription.text = string.Empty;
		this.lbLastLogin.text = string.Empty;
		this.lbCollection.text = string.Empty;
		this.lbLink.text = string.Empty;
		this.lbBlock.text = StringMaster.GetString("Profile-01");
		this.lbVisit.text = StringMaster.GetString("Profile-21");
		this.requestAPIs = new Func<string, bool, APIRequestTask>[]
		{
			null,
			new Func<string, bool, APIRequestTask>(this.RequestFriendBreak),
			new Func<string, bool, APIRequestTask>(APIUtil.Instance().RequestFriendApplication),
			new Func<string, bool, APIRequestTask>(this.RequestFriendApplicationCancel),
			new Func<string, bool, APIRequestTask>(this.RequestFriendApplicationApprove)
		};
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
		CMD_ProfileFriend.friendData = null;
		CMD_ProfileFriend.chatMemberData = null;
		CMD_ProfileFriend.chatLogData = null;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (GUIManager.GetTopDialog(null, false) != null && GUIManager.GetTopDialog(null, false).name == "CMD_FriendTop")
		{
			CMD_PartsFriendIDSearch.SaveInputId();
		}
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		if (CMD_ProfileFriend.friendData == null && CMD_ProfileFriend.chatMemberData == null && CMD_ProfileFriend.chatLogData == null)
		{
			this.ClosePanel(false);
		}
		else
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			base.HideDLG();
			APIRequestTask apirequestTask = new APIRequestTask();
			if (CMD_ProfileFriend.chatMemberData != null)
			{
				apirequestTask.Add(Singleton<UserDataMng>.Instance.RequestFriendProfile(CMD_ProfileFriend.chatMemberData.userId, new Action<GameWebAPI.RespDataPRF_Profile>(this.OnReceivedFriendProfile), false));
			}
			else if (CMD_ProfileFriend.chatLogData != null)
			{
				apirequestTask.Add(Singleton<UserDataMng>.Instance.RequestFriendProfile(CMD_ProfileFriend.chatLogData.userId, new Action<GameWebAPI.RespDataPRF_Profile>(this.OnReceivedFriendProfile), false));
			}
			else
			{
				apirequestTask.Add(Singleton<UserDataMng>.Instance.RequestFriendProfile(CMD_ProfileFriend.friendData.userData.userId, new Action<GameWebAPI.RespDataPRF_Profile>(this.OnReceivedFriendProfile), false));
			}
			base.StartCoroutine(apirequestTask.Run(delegate
			{
				RestrictionInput.EndLoad();
				if (this.friendProfile.userData == null || string.IsNullOrEmpty(this.friendProfile.userData.userId))
				{
					this.SetCloseAction(delegate(int idx)
					{
						CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
						cmd_ModalMessage.Title = StringMaster.GetString("UserDontExistTitle");
						cmd_ModalMessage.Info = StringMaster.GetString("UserDontExistInfo");
					});
					this.ClosePanel(false);
				}
				else
				{
					this.ShowDLG();
					this.Show(f, sizeX, sizeY, aT);
					this.monsterParam = this.characterCameraView.csRender3DRT.GetCharacterParams();
				}
			}, delegate(Exception nop)
			{
				RestrictionInput.EndLoad();
				this.ClosePanel(false);
			}, null));
		}
	}

	private GameWebAPI.FriendList CreateFriendData()
	{
		return new GameWebAPI.FriendList
		{
			monsterData = new GameWebAPI.FriendList.MonsterData(),
			userData = new GameWebAPI.FriendList.UserData(),
			monsterData = 
			{
				monsterId = this.userProfile.monsterData.monsterId
			},
			userData = 
			{
				userId = this.userProfile.userData.userId,
				nickname = this.userProfile.userData.nickname,
				description = this.userProfile.userData.description,
				loginTimeSort = this.userProfile.userData.loginTimeSort,
				loginTime = ((!string.IsNullOrEmpty(this.lbLastLogin.text)) ? this.lbLastLogin.text : this.userProfile.userData.loginTime)
			}
		};
	}

	protected override void dataReload()
	{
		this.userProfile = this.friendProfile;
	}

	protected override void RefreshComponents()
	{
		this.dataReload();
		this.lbName.text = this.userProfile.userData.nickname;
		this.lbDescription.text = this.userProfile.userData.description;
		if (string.IsNullOrEmpty(this.lbLastLogin.text))
		{
			this.lbLastLogin.text = this.userProfile.userData.loginTime;
		}
		this.lbCollection.text = string.Format(StringMaster.GetString("SystemFraction"), this.userProfile.collection.possessionNum, this.userProfile.collection.totalNum);
		switch (this.userProfile.friendStatus)
		{
		case 1:
			this.lbLink.text = StringMaster.GetString("Profile-18");
			break;
		case 2:
			this.lbLink.text = StringMaster.GetString("Profile-10");
			break;
		case 3:
			this.lbLink.text = StringMaster.GetString("Friend-02");
			break;
		case 4:
			this.lbLink.text = StringMaster.GetString("Profile-15");
			break;
		}
		bool flag = BlockManager.instance().CheckBlock(this.userProfile.userData.userId);
		if ((this.userProfile.friendStatus == 4 && flag) || (this.userProfile.friendStatus == 2 && flag))
		{
			this.btnLink.spriteName = "Common02_Btn_BaseG";
			this.lbLink.color = Color.gray;
			this.colLinkBtn.enabled = false;
		}
		else
		{
			this.btnLink.spriteName = "Common02_Btn_BaseON1";
			this.lbLink.color = Color.white;
			this.colLinkBtn.enabled = true;
		}
		if (this.userProfile.friendStatus == 3)
		{
			this.btnBlock.spriteName = "Common02_Btn_BaseG";
			this.lbBlock.color = Color.gray;
			this.colBlockBtn.enabled = false;
		}
		else
		{
			this.btnBlock.spriteName = "Common02_Btn_BaseON2";
			this.lbBlock.color = Color.white;
			this.colBlockBtn.enabled = true;
		}
		if (flag)
		{
			this.colVisitBtn.enabled = false;
			this.btnVisit.spriteName = "Common02_Btn_BaseG";
			this.lbVisit.color = Color.gray;
			this.lbBlock.text = StringMaster.GetString("Profile-05");
		}
		else
		{
			this.colVisitBtn.enabled = true;
			this.btnVisit.spriteName = "Common02_Btn_BaseON1";
			this.lbVisit.color = Color.white;
			this.lbBlock.text = StringMaster.GetString("Profile-01");
		}
	}

	public void SetLastLoginTime(string lastLoginTime)
	{
		this.lbLastLogin.text = lastLoginTime;
	}

	private void OnTouchEndLinkBtn()
	{
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFriendLink), "CMD_Confirm") as CMD_Confirm;
		if (null != cmd_Confirm)
		{
			switch (this.userProfile.friendStatus)
			{
			case 1:
				cmd_Confirm.Title = StringMaster.GetString("Profile-22");
				break;
			case 2:
				cmd_Confirm.Title = StringMaster.GetString("Profile-11");
				break;
			case 3:
				cmd_Confirm.Title = StringMaster.GetString("Profile-23");
				break;
			case 4:
				cmd_Confirm.Title = StringMaster.GetString("Profile-24");
				break;
			}
			switch (this.userProfile.friendStatus)
			{
			case 1:
				cmd_Confirm.Info = StringMaster.GetString("Profile-25");
				break;
			case 2:
				cmd_Confirm.Info = StringMaster.GetString("Profile-26");
				break;
			case 3:
				cmd_Confirm.Info = StringMaster.GetString("Profile-27");
				break;
			case 4:
				cmd_Confirm.Info = StringMaster.GetString("Profile-28");
				break;
			}
		}
	}

	private void OnCloseFriendLink(int idx)
	{
		if (idx == 0)
		{
			this.prevFriendStatus = this.userProfile.friendStatus;
			CMD_FriendTop instance = CMD_FriendTop.instance;
			if (null != instance)
			{
				instance.ProfOpeType = (CMD_ProfileFriend.OperationType)this.userProfile.friendStatus;
			}
			Func<string, bool, APIRequestTask> func = this.requestAPIs[this.userProfile.friendStatus];
			if (func != null)
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
				APIRequestTask apirequestTask = func(this.userProfile.userData.userId, false);
				apirequestTask.Add(Singleton<UserDataMng>.Instance.RequestFriendProfile(this.userProfile.userData.userId, new Action<GameWebAPI.RespDataPRF_Profile>(this.OnReceivedFriendProfile), false)).Add(DataMng.Instance().RequestMyPageData(true));
				base.StartCoroutine(apirequestTask.Run(delegate
				{
					RestrictionInput.EndLoad();
					this.RefreshComponents();
					this.OpenResultFriendAPI();
				}, delegate(Exception nop)
				{
					RestrictionInput.EndLoad();
					this.ClosePanel(true);
				}, null));
			}
		}
	}

	private void OnReceivedFriendProfile(GameWebAPI.RespDataPRF_Profile response)
	{
		this.friendProfile = response;
	}

	private void OpenResultFriendAPI()
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int index)
		{
			if (CMD_FriendTop.instance != null)
			{
				CMD_FriendTop.instance.CloseFriendProfile(0);
			}
		}, "CMD_ModalMessage") as CMD_ModalMessage;
		if (null != cmd_ModalMessage)
		{
			switch (this.prevFriendStatus)
			{
			case 1:
				cmd_ModalMessage.Title = StringMaster.GetString("Profile-29");
				break;
			case 2:
				cmd_ModalMessage.Title = StringMaster.GetString("Profile-30");
				break;
			case 3:
				cmd_ModalMessage.Title = StringMaster.GetString("Profile-31");
				break;
			case 4:
				cmd_ModalMessage.Title = StringMaster.GetString("Profile-32");
				break;
			}
			switch (this.prevFriendStatus)
			{
			case 1:
				cmd_ModalMessage.Info = StringMaster.GetString("Profile-33");
				break;
			case 2:
				cmd_ModalMessage.Info = StringMaster.GetString("Profile-34");
				break;
			case 3:
				cmd_ModalMessage.Info = StringMaster.GetString("Profile-35");
				break;
			case 4:
				cmd_ModalMessage.Info = StringMaster.GetString("Profile-36");
				break;
			}
		}
	}

	private void OnTouchEndBlockBtn()
	{
		if (BlockManager.instance().CheckBlock(this.userProfile.userData.userId))
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFriendBlock), "CMD_Confirm") as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("Profile-01");
			cmd_Confirm.Info = StringMaster.GetString("Profile-06");
		}
		else if (BlockManager.instance().enableBlock)
		{
			CMD_Confirm cmd_Confirm2 = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFriendBlock), "CMD_Confirm") as CMD_Confirm;
			cmd_Confirm2.Title = StringMaster.GetString("Profile-01");
			cmd_Confirm2.Info = StringMaster.GetString("Profile-02");
		}
		else
		{
			AlertManager.ShowAlertDialog(null, StringMaster.GetString("Profile-01"), string.Format(StringMaster.GetString("Profile-04"), ConstValue.MAX_BLOCK_COUNT), AlertManager.ButtonActionType.Close, false);
		}
	}

	private void OnCloseFriendBlock(int idx)
	{
		if (idx != 0)
		{
			return;
		}
		APIRequestTask apirequestTask = new APIRequestTask();
		if (BlockManager.instance().CheckBlock(this.userProfile.userData.userId))
		{
			apirequestTask.Add(this.RequestBlockReset(this.userProfile.userData.userId, false));
		}
		else
		{
			apirequestTask.Add(this.RequestBlockSet(this.userProfile.userData.userId, false));
		}
		apirequestTask.Add(Singleton<UserDataMng>.Instance.RequestFriendProfile(this.userProfile.userData.userId, new Action<GameWebAPI.RespDataPRF_Profile>(this.OnReceivedFriendProfile), false));
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		base.StartCoroutine(apirequestTask.Run(delegate
		{
			RestrictionInput.EndLoad();
			this.RefreshComponents();
			this.OpenResultBlockAPI();
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(true);
		}, null));
	}

	private void OpenResultBlockAPI()
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
		if (null != cmd_ModalMessage)
		{
			cmd_ModalMessage.Title = StringMaster.GetString("Profile-01");
			if (BlockManager.instance().CheckBlock(this.userProfile.userData.userId))
			{
				cmd_ModalMessage.Info = StringMaster.GetString("Profile-03");
			}
			else
			{
				cmd_ModalMessage.Info = StringMaster.GetString("Profile-07");
			}
		}
	}

	private void OnTouchEndVisitBtn()
	{
		if (FarmRoot.Instance == null)
		{
			GUIManager.CloseAllCommonDialog(null);
			CMD_BattleNextChoice.GoToFarm();
			GUIScreenHome.homeOpenCallback = delegate()
			{
				CMD_FriendTop.onWindowOpened = delegate()
				{
					CMD_FriendTop.instance.ShowFriendFarm(this.CreateFriendData(), null);
				};
				GUIMain.ShowCommonDialog(null, "CMD_FriendTop");
			};
		}
		else if (CMD_FriendTop.instance == null)
		{
			GUIManager.CloseAllCommonDialog(null);
			CMD_FriendTop.onWindowOpened = delegate()
			{
				CMD_FriendTop.instance.ShowFriendFarm(this.CreateFriendData(), null);
			};
			GUIMain.ShowCommonDialog(null, "CMD_FriendTop");
		}
		else
		{
			CMD_FriendTop.instance.ShowFriendFarm(this.CreateFriendData(), delegate
			{
				this.ClosePanel(true);
			});
		}
	}

	private APIRequestTask RequestFriendBreak(string friendUserId, bool requestRetry = true)
	{
		return APIUtil.Instance().RequestFriendBreak(new string[]
		{
			friendUserId
		}, requestRetry);
	}

	private APIRequestTask RequestFriendApplicationCancel(string friendUserId, bool requestRetry = true)
	{
		return APIUtil.Instance().RequestFriendApplicationCancel(new string[]
		{
			friendUserId
		}, requestRetry);
	}

	private APIRequestTask RequestFriendApplicationApprove(string friendUserId, bool requestRetry = true)
	{
		return APIUtil.Instance().RequestFriendApplicationDecision(new string[]
		{
			friendUserId
		}, GameWebAPI.FR_Req_FriendDecision.DecisionType.APPROVE, requestRetry);
	}

	private APIRequestTask RequestFriendApplicationRefusal(string friendUserId, bool requestRetry = true)
	{
		return APIUtil.Instance().RequestFriendApplicationDecision(new string[]
		{
			friendUserId
		}, GameWebAPI.FR_Req_FriendDecision.DecisionType.REFUSAL, requestRetry);
	}

	private APIRequestTask RequestBlockSet(string friendUserId, bool requestRetry = true)
	{
		GameWebAPI.RequestBL_BlockSet request = new GameWebAPI.RequestBL_BlockSet
		{
			SetSendData = delegate(GameWebAPI.BL_Req_BlockSet param)
			{
				param.targetUserId = int.Parse(friendUserId);
			},
			OnReceived = delegate(GameWebAPI.RespDataBL_BlockSet nop)
			{
				if (CMD_FriendTop.instance != null)
				{
					CMD_FriendTop.instance.OnBlockSet(this.userProfile.userData.userId);
				}
				if (CMD_ProfileFriend.chatMemberData == null && CMD_ProfileFriend.chatLogData == null)
				{
					BlockManager.instance().blockList.Add(CMD_ProfileFriend.friendData);
				}
				else
				{
					GameWebAPI.FriendList item = this.CreateFriendData();
					BlockManager.instance().blockList.Add(item);
				}
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	private APIRequestTask RequestBlockReset(string friendUserId, bool requestRetry = true)
	{
		GameWebAPI.RequestBL_BlockReset request = new GameWebAPI.RequestBL_BlockReset
		{
			SetSendData = delegate(GameWebAPI.BL_Req_BlockReset param)
			{
				param.targetUserId = int.Parse(friendUserId);
			},
			OnReceived = delegate(WebAPI.ResponseData nop)
			{
				int index = BlockManager.instance().blockList.FindIndex((GameWebAPI.FriendList x) => x.userData.userId == this.userProfile.userData.userId);
				BlockManager.instance().blockList.RemoveAt(index);
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	private IEnumerator RandomAnimateModel()
	{
		if (this.monsterParam == null || this.isAnimation)
		{
			yield break;
		}
		this.isAnimation = true;
		int rand = UnityEngine.Random.Range(0, 3);
		float animeClipLength = 0f;
		float playTime = 0f;
		switch (rand)
		{
		case 0:
			this.monsterParam.PlayAnimation(CharacterAnimationType.win, SkillType.Attack, 0, null, null);
			animeClipLength = this.monsterParam.AnimationClipLength;
			break;
		case 1:
			this.monsterParam.PlayAnimation(CharacterAnimationType.eat, SkillType.Attack, 0, null, null);
			animeClipLength = this.monsterParam.AnimationClipLength;
			break;
		case 2:
			this.monsterParam.PlayAnimation(CharacterAnimationType.attacks, SkillType.Attack, 0, null, null);
			animeClipLength = this.monsterParam.AnimationClipLength;
			break;
		}
		while (playTime < animeClipLength)
		{
			playTime += Time.deltaTime;
			yield return null;
		}
		this.isAnimation = false;
		yield break;
	}

	public void OnDisplayClick()
	{
		if (!this.isOpenScreen)
		{
			return;
		}
		if (!this.isAnimation)
		{
			base.StartCoroutine(this.RandomAnimateModel());
		}
	}

	protected override void SetColosseumUserStatus()
	{
		if (this.colosseumUserStatus == null)
		{
			this.colosseumRankSprite.gameObject.SetActive(false);
			this.colosseumNoneDataObj.SetActive(true);
			this.outComeNumLabel.text = string.Format(StringMaster.GetString("ColosseumScore"), "0", "0");
		}
		else
		{
			this.colosseumRankSprite.spriteName = "Rank_" + this.colosseumUserStatus.colosseumRankId.ToString();
			this.outComeNumLabel.text = string.Format(StringMaster.GetString("ColosseumScore"), this.colosseumUserStatus.winWeek, this.colosseumUserStatus.loseWeek);
		}
	}

	public enum OperationType
	{
		None,
		Break,
		Request,
		Cancel,
		Approve
	}
}
