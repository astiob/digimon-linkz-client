using Master;
using MultiBattle.Tools;
using Quest;
using System;
using System.Linq;
using UnityEngine;

public class GUIListMultiRecruitListParts : GUIListPartBS
{
	[SerializeField]
	private UILabel lbUserName;

	[SerializeField]
	private GameObject goTitleIcon;

	[SerializeField]
	private UILabel lbQuestName;

	[SerializeField]
	private UILabel lbComment;

	[SerializeField]
	private UILabel lbNowMemberNum;

	[SerializeField]
	private UILabel lbMoodType;

	[SerializeField]
	private UISprite bgFrame;

	[SerializeField]
	private GameObject goMasterIcon;

	private bool isOpenedQuest = true;

	private GameWebAPI.ResponseData_Common_MultiRoomList.room data;

	public GameWebAPI.ResponseData_Common_MultiRoomList.room Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
			this.ShowGUI();
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
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

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchEnded(touch, pos, flag);
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		if (CMD_ChatTop.instance != null)
		{
			this.isOpenedQuest = (this.data.isAllow == 1);
		}
		this.SetInitLabel();
	}

	private void SetInitLabel()
	{
		this.lbUserName.text = this.data.ownerName;
		this.lbComment.text = this.data.introduction;
		TitleDataMng.SetTitleIcon(this.data.titleId, this.goTitleIcon.GetComponent<UITexture>());
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 = worldStageM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM x) => x.worldStageId == this.data.worldStageId);
		this.lbQuestName.text = worldStageM2.name + "\n" + this.data.dungeonName;
		string moodType = this.data.moodType;
		switch (moodType)
		{
		case "1":
			this.lbMoodType.text = StringMaster.GetString("RecruitRule-06");
			goto IL_15D;
		case "2":
			this.lbMoodType.text = StringMaster.GetString("RecruitRule-07");
			goto IL_15D;
		}
		this.lbMoodType.text = StringMaster.GetString("RecruitRule-08");
		IL_15D:
		this.lbNowMemberNum.text = string.Format(StringMaster.GetString("SystemFraction"), this.data.memberCount, 3);
		if (!this.isOpenedQuest)
		{
			this.bgFrame.color = ConstValue.DEFAULT_COLOR;
			this.lbUserName.color = ConstValue.DEACTIVE_BUTTON_LABEL;
			this.lbQuestName.color = ConstValue.DEACTIVE_BUTTON_LABEL;
			this.lbComment.color = ConstValue.DEACTIVE_BUTTON_LABEL;
		}
		if (this.data.ownerMonsterId != null)
		{
			MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.data.ownerMonsterId);
			if (monsterData != null)
			{
				GUIMonsterIcon guimonsterIcon = MonsterDataMng.Instance().MakePrefabByMonsterData(monsterData, this.goMasterIcon.transform.localScale, this.goMasterIcon.transform.localPosition, this.goMasterIcon.transform.parent, true, false);
				UIWidget component = base.gameObject.GetComponent<UIWidget>();
				if (component != null)
				{
					DepthController depthController = guimonsterIcon.GetDepthController();
					depthController.AddWidgetDepth(guimonsterIcon.transform, component.depth + 2);
				}
			}
		}
	}

	private void OnClickedInfo()
	{
		bool isRecruitListLock;
		if (CMD_ChatTop.instance != null)
		{
			isRecruitListLock = CMD_ChatTop.instance.isRecruitListLock;
		}
		else
		{
			isRecruitListLock = CMD_MultiRecruitTop.instance.isRecruitListLock;
		}
		if (!isRecruitListLock)
		{
			if (!this.isOpenedQuest)
			{
				AlertManager.ShowModalMessage(delegate(int modal)
				{
				}, StringMaster.GetString("Recruit-04"), StringMaster.GetString("Recruit-05"), AlertManager.ButtonActionType.Close, false);
				return;
			}
			if (!Singleton<UserDataMng>.Instance.IsOverUnitLimit(ConstValue.ENABLE_MONSTER_SPACE_TOEXEC_DUNGEON))
			{
				if (!Singleton<UserDataMng>.Instance.IsOverChipLimit(ConstValue.ENABLE_CHIP_SPACE_TOEXEC_DUNGEON))
				{
					MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
					GameWebAPI.RespDataWD_GetDungeonInfo respDataWD_GetDungeonInfo = new GameWebAPI.RespDataWD_GetDungeonInfo();
					respDataWD_GetDungeonInfo = ClassSingleton<QuestData>.Instance.GetDngeonInfoByWorldId(this.data.worldAreaId);
					if (respDataWD_GetDungeonInfo != null)
					{
						foreach (GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo worldDungeonInfo2 in respDataWD_GetDungeonInfo.worldDungeonInfo)
						{
							foreach (GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dungeons2 in worldDungeonInfo2.dungeons)
							{
								if (dungeons2.worldDungeonId == int.Parse(this.data.worldDungeonId))
								{
									ClassSingleton<PartyBossIconsAccessor>.Instance.StageEnemies = dungeons2.encountEnemies;
								}
							}
						}
						ClassSingleton<QuestData>.Instance.SelectDungeon = ClassSingleton<QuestData>.Instance.GetWorldDungeonMaster(this.data.worldDungeonId);
						GameWebAPI.MultiRoomJoin multiRoomJoin = new GameWebAPI.MultiRoomJoin();
						multiRoomJoin.SetSendData = delegate(GameWebAPI.ReqData_MultiRoomJoin param)
						{
							param.roomId = int.Parse(this.data.multiRoomId);
							param.password = string.Empty;
						};
						multiRoomJoin.OnReceived = delegate(GameWebAPI.RespData_MultiRoomJoin response)
						{
							CMD_MultiRecruitPartyWait.roomJoinData = response;
						};
						GameWebAPI.MultiRoomJoin request = multiRoomJoin;
						base.StartCoroutine(request.RunOneTime(delegate()
						{
							RestrictionInput.EndLoad();
							CMD_MultiRecruitPartyWait.UserType = CMD_MultiRecruitPartyWait.USER_TYPE.MEMBER;
							CMD_MultiRecruitPartyWait.StageDataBk = this.data;
							GUIMain.ShowCommonDialog(null, "CMD_MultiRecruitPartyWait");
						}, delegate(Exception noop)
						{
							RestrictionInput.EndLoad();
							CMD_MultiRecruitTop.instance.AddExcludeRoomIdList(this.data.multiRoomId);
							CMD_MultiRecruitTop.instance.ReBuildMultiRecruitList();
						}, null));
					}
					else
					{
						CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
						cmd_ModalMessage.Title = StringMaster.GetString("QuestEventTitle");
						cmd_ModalMessage.Info = StringMaster.GetString("QuestEventInfo2");
					}
				}
				else
				{
					CMD_UpperlimitChip cmd_UpperlimitChip = GUIMain.ShowCommonDialog(null, "CMD_UpperlimitChip") as CMD_UpperlimitChip;
					cmd_UpperlimitChip.SetType(CMD_UpperlimitChip.MessageType.QUEST);
				}
			}
			else
			{
				CMD_UpperLimit cmd_UpperLimit = GUIMain.ShowCommonDialog(null, "CMD_Upperlimit") as CMD_UpperLimit;
				cmd_UpperLimit.SetType(CMD_UpperLimit.MessageType.QUEST);
			}
		}
	}
}
