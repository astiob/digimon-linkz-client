using Master;
using PartyEdit;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_PartyEdit : CMD
{
	[SerializeField]
	[Header("スクロールビューの左右をクリッピングするオブジェクト")]
	private GameObject[] clipObjects;

	private Vector3 v3DPos = new Vector3(0f, 4000f, 0f);

	private float xPos = 1000f;

	[SerializeField]
	private PartyEditPartyInfo partyInfo;

	[SerializeField]
	private PartyEditPartyMember partyMember;

	[SerializeField]
	private PartyEditBattleInfo battleInfo;

	private PartyEditAction partyEditAction;

	[NonSerialized]
	public CMD parentCMD;

	[NonSerialized]
	public int idxNumber = -1;

	private int backUpIdx = -1;

	public static CMD_PartyEdit instance;

	public static string replayMultiStageId;

	public static string replayMultiAreaId;

	public static string replayMultiDungeonId;

	public static CMD_PartyEdit.MODE_TYPE ModeType { get; set; }

	protected override void Awake()
	{
		base.Awake();
		CMD_PartyEdit.instance = this;
		switch (CMD_PartyEdit.ModeType)
		{
		case CMD_PartyEdit.MODE_TYPE.EDIT:
			this.partyEditAction = new PartyEditAction();
			ClassSingleton<PartyBossIconsAccessor>.Instance.StageEnemies = null;
			ClassSingleton<QuestData>.Instance.SelectDungeon = null;
			break;
		case CMD_PartyEdit.MODE_TYPE.SELECT:
			this.partyEditAction = new PartyEditActionBattle();
			break;
		case CMD_PartyEdit.MODE_TYPE.PVP:
			this.partyEditAction = new PartyEditActionPvP();
			ClassSingleton<PartyBossIconsAccessor>.Instance.StageEnemies = null;
			break;
		case CMD_PartyEdit.MODE_TYPE.MULTI:
			this.partyEditAction = new PartyEditActionMulti();
			break;
		}
		this.partyEditAction.SetUiRoot(this);
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_PartyEdit");
		Vector3 localPosition = base.gameObject.transform.localPosition;
		localPosition.x = 10000f;
		base.gameObject.transform.localPosition = localPosition;
		base.StartCoroutine(this.ShowAll(f, sizeX, sizeY, aT));
	}

	private IEnumerator ShowAll(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.battleInfo.SetView(CMD_PartyEdit.ModeType);
		while (!AssetDataCacheMng.Instance().IsCacheAllReadyType(AssetDataCacheMng.CACHE_TYPE.CHARA_PARTY))
		{
			yield return null;
		}
		base.PartsTitle.SetTitle(StringMaster.GetString("PartyTitleEdit"));
		base.SetTutorialAnyTime("anytime_second_tutorial_partyedit");
		this.battleInfo.SetBossMonsterIcon(ClassSingleton<QuestData>.Instance.GetBossMonsterList(ClassSingleton<PartyBossIconsAccessor>.Instance.StageEnemies));
		this.battleInfo.SetSortieLimit();
		this.partyMember.SetView(this);
		this.partyInfo.SetView(CMD_PartyEdit.ModeType);
		this.partyMember.SetChangeMonsterEvent(new Action(this.UpdateSelectDeckData));
		this.RefreshPartyNumText();
		Vector3 v3 = base.gameObject.transform.localPosition;
		v3.x = 0f;
		base.gameObject.transform.localPosition = v3;
		base.Show(f, sizeX, sizeY, aT);
		if (Loading.IsShow())
		{
			RestrictionInput.EndLoad();
		}
		yield break;
	}

	protected override void Update()
	{
		base.Update();
		this.RefreshPartyNumText();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_PartyEdit.instance = null;
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.EDIT)
		{
			TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
			if (tutorialObserver != null)
			{
				GUIMain.BarrierON(null);
				tutorialObserver.StartSecondTutorial("second_tutorial_partyedit", new Action(GUIMain.BarrierOFF), delegate
				{
					GUICollider.EnableAllCollider("CMD_PartyEdit");
				});
			}
			else
			{
				GUICollider.EnableAllCollider("CMD_PartyEdit");
			}
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_PartyEdit");
		}
	}

	private void OnClickedSave()
	{
		if (!this.partyMember.IsScroll())
		{
			string arg = string.Empty;
			int num = 0;
			bool flag = false;
			GameWebAPI.WD_Req_DngStart lastDngReq = DataMng.Instance().GetResultUtilData().GetLastDngReq();
			if (null != CMD_QuestTOP.instance)
			{
				QuestData.WorldStageData worldStageData = CMD_QuestTOP.instance.GetWorldStageData();
				QuestData.WorldDungeonData stageDataBk = CMD_QuestTOP.instance.StageDataBk;
				if ("8" == worldStageData.worldStageM.worldAreaId)
				{
					flag = true;
					num = stageDataBk.dungeon.dungeonTicketNum.ToInt32();
					arg = worldStageData.worldStageM.name;
				}
			}
			else if (lastDngReq != null)
			{
				GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons ticketQuestDungeonByTicketID = ClassSingleton<QuestData>.Instance.GetTicketQuestDungeonByTicketID(lastDngReq.userDungeonTicketId);
				if (ticketQuestDungeonByTicketID != null)
				{
					flag = true;
					num = ticketQuestDungeonByTicketID.dungeonTicketNum.ToInt32();
					GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonMaster = ClassSingleton<QuestData>.Instance.GetWorldDungeonMaster(ticketQuestDungeonByTicketID.worldDungeonId.ToString());
					arg = worldDungeonMaster.name;
				}
			}
			if (flag)
			{
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(delegate(int idx)
				{
					if (idx == 0)
					{
						this.OnClickedSaveOperation();
					}
				}, "CMD_Confirm") as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("TicketQuestTitle");
				cmd_Confirm.Info = string.Format(StringMaster.GetString("TicketQuestConfirmInfo"), arg, num, num - 1);
				cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonYes");
				cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
			}
			else
			{
				this.OnClickedSaveOperation();
			}
		}
	}

	private void OnClickedSaveOperation()
	{
		this.partyEditAction.ChangeOperation(this.partyMember, this.idxNumber, this.partyInfo.GetFavoriteDeckNo());
	}

	public override void ClosePanel(bool animation = true)
	{
		this.partyEditAction.CloseOperation(this.partyMember, this.idxNumber, this.partyInfo.GetFavoriteDeckNo(), animation);
	}

	public List<MonsterData> GetSelectedMD()
	{
		return this.partyMember.GetMonsterDataList(this.idxNumber - 1);
	}

	public List<string> MakeAllCharaPath()
	{
		List<string> list = new List<string>();
		int partyCount = this.partyMember.GetPartyCount();
		for (int i = 0; i < partyCount; i++)
		{
			list.AddRange(this.partyMember.GetCharaModelPathList(i));
		}
		return list;
	}

	private void UpdateSelectDeckData()
	{
		int partyNo = this.idxNumber - 1;
		List<MonsterData> monsterDataList = this.partyMember.GetMonsterDataList(partyNo);
		if (0 < monsterDataList.Count)
		{
			this.partyInfo.SetPartyInfo(this.idxNumber, monsterDataList[0]);
		}
		bool enable;
		if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.MULTI)
		{
			enable = this.battleInfo.CheckSortieLimit(monsterDataList[0]);
		}
		else
		{
			enable = this.battleInfo.CheckSortieLimit(monsterDataList);
		}
		this.battleInfo.EnableBattleStartButton(enable);
	}

	private void RefreshPartyNumText()
	{
		if (this.backUpIdx != this.idxNumber)
		{
			this.UpdateSelectDeckData();
			this.partyInfo.EnableFavoriteButton(this.partyInfo.GetFavoriteDeckNo() == this.idxNumber);
			this.backUpIdx = this.idxNumber;
		}
	}

	private void OnClickedFavorite()
	{
		this.partyInfo.SetFavoriteDeckNo(this.idxNumber);
	}

	private void OnClickedChangeStatus()
	{
		int partyNo = this.idxNumber - 1;
		PartsPartyMonsInfo leaderMonsterInfo = this.partyMember.GetLeaderMonsterInfo(partyNo);
		int num = leaderMonsterInfo.GetStatusPage();
		num++;
		this.partyMember.SetStatusPage(partyNo, num);
	}

	public Vector3 Get3DPos()
	{
		this.v3DPos.x = this.xPos;
		this.xPos += 1000f;
		return this.v3DPos;
	}

	public void Reset3DPos()
	{
		this.xPos = 1000f;
	}

	public void DispClips()
	{
		foreach (GameObject go in this.clipObjects)
		{
			NGUITools.SetActiveSelf(go, true);
		}
	}

	public void HideClips()
	{
		foreach (GameObject go in this.clipObjects)
		{
			NGUITools.SetActiveSelf(go, false);
		}
		this.partyMember.HideParts();
	}

	public void ReloadAllCharacters(bool flg)
	{
		this.Reset3DPos();
		this.partyMember.RefreshMonsterInfo(flg);
	}

	public void Close(bool animation)
	{
		base.ClosePanel(animation);
	}

	public List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> GetWorldSortieLimit()
	{
		return this.battleInfo.GetSortieLimitList();
	}

	public enum MODE_TYPE
	{
		EDIT,
		SELECT,
		PVP,
		MULTI
	}
}
