using DigiChat.Tools;
using Master;
using Monster;
using Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUIListChatLogParts : GUIListPartBS
{
	[SerializeField]
	private GameObject goCONTENT_WRAP;

	[SerializeField]
	private GameObject goTX_USERNAME;

	[SerializeField]
	private GameObject goTX_DATE;

	[SerializeField]
	private GameObject goTX_COMMENT;

	[SerializeField]
	private GameObject goCONTAINER;

	[SerializeField]
	private GameObject goBG_BALLOON;

	[SerializeField]
	private GameObject goMONSTER_ICON;

	[SerializeField]
	private GameObject goTITLE_ICON;

	[SerializeField]
	private GameObject goSELF_CONTENT_WRAP;

	[SerializeField]
	private GameObject goSELF_TX_USERNAME;

	[SerializeField]
	private GameObject goSELF_TX_DATE;

	[SerializeField]
	private GameObject goSELF_TX_COMMENT;

	[SerializeField]
	private GameObject goSELF_CONTAINER;

	[SerializeField]
	private GameObject goSELF_BG_BALLOON;

	[SerializeField]
	private GameObject goSELF_MONSTER_ICON;

	[SerializeField]
	private GameObject goSELF_TITLE_ICON;

	[SerializeField]
	private GameObject goSYS_CONTENT_WRAP;

	[SerializeField]
	private GameObject goSYS_TX_COMMENT;

	[SerializeField]
	private GameObject goSYS_CONTAINER;

	[SerializeField]
	private GameObject goSYS_BG;

	[SerializeField]
	private Color multiPassCodeTextColor;

	private UILabel ngTX_USERNAME;

	private UILabel ngTX_DATE;

	private UILabel ngTX_COMMENT;

	private GameObject ngMONSTER_ICON;

	private GameObject ngTITLE_ICON;

	private GUICollider ngMULTICODE_COL;

	private BoxCollider ngMULTICODE_BCOL;

	private string thumbMid;

	private string multiWorldAreaId;

	private string multiWorldDungeonId;

	private string multiRoomId;

	public GUISelectChatLogPanel selectChatLogPanel;

	public float listColliderHeight;

	public List<GameObject> goEffList;

	private GameWebAPI.Common_MessageData respDatachatMessageDataResult;

	public GameWebAPI.Common_MessageData RespDataChatMessageDataResult
	{
		get
		{
			return this.respDatachatMessageDataResult;
		}
		set
		{
			this.respDatachatMessageDataResult = value;
			this.ShowGUI();
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		if (this.RespDataChatMessageDataResult.type == 3)
		{
			this.ngTX_COMMENT = this.goSYS_TX_COMMENT.GetComponent<UILabel>();
			this.goCONTENT_WRAP.SetActive(false);
			this.goSELF_CONTENT_WRAP.SetActive(false);
		}
		else if (DataMng.Instance().UserId == this.RespDataChatMessageDataResult.userId)
		{
			this.ngTX_USERNAME = this.goSELF_TX_USERNAME.GetComponent<UILabel>();
			this.ngTX_DATE = this.goSELF_TX_DATE.GetComponent<UILabel>();
			this.ngTX_COMMENT = this.goSELF_TX_COMMENT.GetComponent<UILabel>();
			this.ngMONSTER_ICON = this.goSELF_MONSTER_ICON;
			this.ngTITLE_ICON = this.goSELF_TITLE_ICON;
			this.goCONTENT_WRAP.SetActive(false);
			this.goSYS_CONTENT_WRAP.SetActive(false);
		}
		else
		{
			this.ngTX_USERNAME = this.goTX_USERNAME.GetComponent<UILabel>();
			this.ngTX_DATE = this.goTX_DATE.GetComponent<UILabel>();
			this.ngTX_COMMENT = this.goTX_COMMENT.GetComponent<UILabel>();
			this.ngMONSTER_ICON = this.goMONSTER_ICON;
			this.ngTITLE_ICON = this.goTITLE_ICON;
			if (this.RespDataChatMessageDataResult.type == 4)
			{
				this.ngMULTICODE_COL = this.goBG_BALLOON.GetComponent<GUICollider>();
				this.ngMULTICODE_BCOL = this.goBG_BALLOON.GetComponent<BoxCollider>();
			}
			this.goSELF_CONTENT_WRAP.SetActive(false);
			this.goSYS_CONTENT_WRAP.SetActive(false);
		}
		string text = ChatTools.OutputDateCtrl(this.RespDataChatMessageDataResult.createTime);
		if (this.RespDataChatMessageDataResult.type != 3)
		{
			this.ngTX_DATE.text = text;
		}
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		string text2;
		component.size = ChatTools.GetColliderSize(this.RespDataChatMessageDataResult, component, CMD_ChatWindow.instance.goBaseTXT, out text2);
		this.listColliderHeight = component.size.y;
		int type = this.RespDataChatMessageDataResult.type;
		if (type != 3)
		{
			if (type != 4)
			{
				this.ngTX_COMMENT.text = text2;
			}
			else
			{
				string[] dungeonIds = text2.Split(new char[]
				{
					','
				});
				string arg;
				if (dungeonIds.Length > 2)
				{
					GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
					GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
					GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 = worldStageM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM x) => x.worldStageId == dungeonIds[0]);
					GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM2 = worldDungeonM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == dungeonIds[1]);
					arg = string.Format(StringMaster.GetString("MultiRecruitChat-07"), worldStageM2.name, worldDungeonM2.name);
					this.multiWorldAreaId = worldStageM2.worldAreaId;
					this.multiWorldDungeonId = worldDungeonM2.worldDungeonId;
					this.multiRoomId = dungeonIds[2];
				}
				else
				{
					arg = text2;
				}
				this.ngTX_COMMENT.text = string.Format(StringMaster.GetString("ChatLog-11"), arg);
				this.ngTX_COMMENT.color = this.multiPassCodeTextColor;
				if (DataMng.Instance().UserId != this.RespDataChatMessageDataResult.userId)
				{
					this.ngMULTICODE_COL.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnMultiPassCodeClicked();
					};
				}
			}
		}
		else
		{
			this.ngTX_COMMENT.text = string.Format("{0}\n{1}", text, text2);
		}
		if (DataMng.Instance().UserId == this.RespDataChatMessageDataResult.userId && this.RespDataChatMessageDataResult.type != 3)
		{
			this.ngTX_COMMENT.pivot = UIWidget.Pivot.Left;
		}
	}

	private void UpdateMonsterIcon()
	{
		if (!this.isUpdate)
		{
			if (this.RespDataChatMessageDataResult.type != 3)
			{
				this.ngTX_USERNAME.text = this.RespDataChatMessageDataResult.userInfo.nickname;
				TitleDataMng.SetTitleIcon(this.RespDataChatMessageDataResult.userInfo.titleId, this.ngTITLE_ICON.GetComponent<UITexture>());
				this.thumbMid = this.RespDataChatMessageDataResult.userInfo.monsterId;
				MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.thumbMid);
				if (monsterData != null)
				{
					GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(monsterData, this.ngMONSTER_ICON.transform.localScale, this.ngMONSTER_ICON.transform.localPosition, this.ngMONSTER_ICON.transform.parent, true, true);
					guimonsterIcon.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnThumbnailClicked();
					};
					DepthController depthController = guimonsterIcon.GetDepthController();
					depthController.AddWidgetDepth(guimonsterIcon.transform, 1200);
				}
			}
			if (this.RespDataChatMessageDataResult.type == 4 && DataMng.Instance().UserId != this.RespDataChatMessageDataResult.userId)
			{
				this.ngMULTICODE_BCOL.size = new Vector3(this.ngMULTICODE_BCOL.size.x, this.ngMULTICODE_BCOL.size.y, 5f);
			}
			this.isUpdate = true;
		}
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchBegan(touch, pos);
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateMonsterIcon();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	private void OnOpenMemberProfile()
	{
		CMD_ProfileFriend.chatLogData = this.respDatachatMessageDataResult;
		GUIMain.ShowCommonDialog(null, "CMD_ProfileFriend", null);
	}

	private void OnThumbnailClicked()
	{
		if (this.respDatachatMessageDataResult.userId != DataMng.Instance().UserId && CMD_ChatTop.instance.isGetBlockList)
		{
			this.OnOpenMemberProfile();
		}
	}

	private void OnMultiPassCodeClicked()
	{
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OpenMultiRecruitPartyWait), "CMD_Confirm", null) as CMD_Confirm;
		cmd_Confirm.Title = StringMaster.GetString("SystemConfirm");
		cmd_Confirm.Info = StringMaster.GetString("ChatMultiConfirmInfo");
	}

	private void OpenMultiRecruitPartyWait(int idx)
	{
		if (idx == 0)
		{
			CMD_ChatWindow.instance.SetCloseAction(delegate(int close)
			{
				if (!Singleton<UserDataMng>.Instance.IsOverUnitLimit(ClassSingleton<MonsterUserDataMng>.Instance.GetMonsterNum() + ConstValue.ENABLE_SPACE_TOEXEC_DUNGEON))
				{
					if (!Singleton<UserDataMng>.Instance.IsOverChipLimit(ConstValue.ENABLE_SPACE_TOEXEC_DUNGEON))
					{
						ChatTools.ChatLoadDisplay(true);
						GameWebAPI.RespDataWD_GetDungeonInfo respDataWD_GetDungeonInfo = new GameWebAPI.RespDataWD_GetDungeonInfo();
						respDataWD_GetDungeonInfo = ClassSingleton<QuestData>.Instance.GetDngeonInfoByWorldAreaId(this.multiWorldAreaId);
						foreach (GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo worldDungeonInfo2 in respDataWD_GetDungeonInfo.worldDungeonInfo)
						{
							foreach (GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dungeons2 in worldDungeonInfo2.dungeons)
							{
								if (dungeons2.worldDungeonId == int.Parse(this.multiWorldDungeonId))
								{
									ClassSingleton<PartyBossIconsAccessor>.Instance.StageEnemies = dungeons2.encountEnemies;
								}
							}
						}
						ClassSingleton<QuestData>.Instance.SelectDungeon = ClassSingleton<QuestData>.Instance.GetWorldDungeonMaster(this.multiWorldDungeonId);
						DataMng.Instance().GetResultUtilData().SetLastDngReq(this.multiWorldDungeonId, "-1", "-1");
						GameWebAPI.MultiRoomJoin multiRoomJoin = new GameWebAPI.MultiRoomJoin();
						multiRoomJoin.SetSendData = delegate(GameWebAPI.ReqData_MultiRoomJoin param)
						{
							param.roomId = int.Parse(this.multiRoomId);
							param.password = string.Empty;
						};
						multiRoomJoin.OnReceived = delegate(GameWebAPI.RespData_MultiRoomJoin response)
						{
							CMD_MultiRecruitPartyWait.roomJoinData = response;
							CMD_MultiRecruitPartyWait.StageDataBk = new GameWebAPI.ResponseData_Common_MultiRoomList.room
							{
								worldAreaId = response.multiRoomInfo.worldAreaId,
								worldDungeonId = response.multiRoomInfo.worldDungeonId,
								worldStageId = response.multiRoomInfo.worldStageId
							};
						};
						GameWebAPI.MultiRoomJoin request = multiRoomJoin;
						AppCoroutine.Start(request.RunOneTime(delegate()
						{
							RestrictionInput.EndLoad();
							if (CMD_ChatTop.instance != null)
							{
								CMD_ChatTop.instance.SetReOpendAction(delegate(int _idx)
								{
									CMD_MultiRecruitPartyWait.UserType = CMD_MultiRecruitPartyWait.USER_TYPE.MEMBER;
									GUIMain.ShowCommonDialog(null, "CMD_MultiRecruitPartyWait", null);
									return true;
								});
							}
						}, delegate(Exception noop)
						{
							RestrictionInput.EndLoad();
						}, null), false);
					}
					else
					{
						CMD_UpperlimitChip cmd_UpperlimitChip = GUIMain.ShowCommonDialog(null, "CMD_UpperlimitChip", null) as CMD_UpperlimitChip;
						cmd_UpperlimitChip.SetType(CMD_UpperlimitChip.MessageType.QUEST);
					}
				}
				else
				{
					CMD_UpperLimit cmd_UpperLimit = GUIMain.ShowCommonDialog(null, "CMD_Upperlimit", null) as CMD_UpperLimit;
					cmd_UpperLimit.SetType(CMD_UpperLimit.MessageType.QUEST);
				}
			});
			CMD_ChatWindow.instance.ClosePanel(true);
		}
	}
}
