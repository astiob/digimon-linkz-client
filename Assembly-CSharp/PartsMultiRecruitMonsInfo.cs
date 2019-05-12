using Monster;
using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PartsMultiRecruitMonsInfo : PartsPartyMonsInfo
{
	[SerializeField]
	private GameObject goTXT_READY;

	[SerializeField]
	private GameObject goANIM_RECRUIT;

	[SerializeField]
	private GameObject goANIM_RECRUIT_TXT;

	[SerializeField]
	private GameObject goANIM_RECRUIT_OUT_CIRCLE;

	[SerializeField]
	private GameObject goUSER_DATA;

	[SerializeField]
	private UILabel lbTXT_NICKNAME;

	[SerializeField]
	private GameObject goTITLE_ICON;

	[SerializeField]
	private UISprite spTAG_PLAYER;

	[SerializeField]
	private GameObject emotionParent;

	[SerializeField]
	private GameObject sortieNG_Label;

	[SerializeField]
	private float sortieNgLabelPositionY_Member;

	[SerializeField]
	private float sortieNgLabelPositionY_Owner;

	private MonsterData baseMonster;

	private CMD_MultiRecruitPartyWait.USER_TYPE monsterUserType;

	private List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> sortieLimitList;

	public int positionNumber { get; set; }

	protected override void Update()
	{
		base.UpdateCharacter();
	}

	public void DoDestroy()
	{
		base.OnDestroy();
	}

	protected override void SetSelectedCharChg(MonsterData monsterData)
	{
		MonsterChipEquipData chipEquip = monsterData.GetChipEquip();
		int[] chipIdList = chipEquip.GetChipIdList();
		this.chipBaseSelect.SetSelectedCharChg(chipIdList, false, 0);
	}

	public override void OnTapedMonster()
	{
		CMD_MultiRecruitPartyWait.Instance.ClearStExchange();
		this.baseMonster = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(base.Data.userMonster.userMonsterId, false);
		CMD_DeckList.SelectMonsterData = this.baseMonster;
		CMD_DeckList cmd_DeckList = GUIMain.ShowCommonDialog(new Action<int>(this.OnClosedDeckList), "CMD_DeckList") as CMD_DeckList;
		cmd_DeckList.PartsTitle.DisableCloseBtn(true);
		cmd_DeckList.PPMI_Instance = this;
		cmd_DeckList.ppmiList = new List<PartsPartyMonsInfo>();
		cmd_DeckList.SetSortieLimit(this.sortieLimitList);
		if (PartsMenu.instance != null)
		{
			PartsMenu.instance.Active(false);
		}
		base.HideStatusPanel();
	}

	private void OnClosedDeckList(int noop)
	{
		if (CMD_DeckList.SelectMonsterData.userMonster.userMonsterId != base.Data.userMonster.userMonsterId)
		{
			if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
			{
				GameWebAPI.MultiRoomUpdate request = new GameWebAPI.MultiRoomUpdate
				{
					SetSendData = delegate(GameWebAPI.ReqData_MultiRoomUpdate param)
					{
						param.roomId = int.Parse(CMD_MultiRecruitPartyWait.roomCreateData.multiRoomInfo.multiRoomId);
						param.user_monster_id = int.Parse(base.Data.userMonster.userMonsterId);
					}
				};
				base.StartCoroutine(request.RunOneTime(new Action(RestrictionInput.EndLoad), delegate(Exception nop)
				{
					RestrictionInput.EndLoad();
				}, null));
			}
			if (CMD_MultiRecruitPartyWait.Instance.ownerSubMonsterData != null && CMD_MultiRecruitPartyWait.Instance.ownerSubMonsterData.userMonster.userMonsterId == base.Data.userMonster.userMonsterId)
			{
				CMD_MultiRecruitPartyWait.Instance.ownerSubMonsterData = this.baseMonster;
				CMD_MultiRecruitPartyWait.Instance.ChangeMonster(base.Data, this.positionNumber, true);
			}
			else
			{
				CMD_MultiRecruitPartyWait.Instance.ChangeMonster(base.Data, this.positionNumber, false);
			}
			this.ClearArousal();
		}
	}

	public void SetTagReady(bool isReady)
	{
		this.goTXT_READY.SetActive(isReady);
	}

	public void ClearArousal()
	{
		base.ShowRare();
	}

	public bool GetTagReady()
	{
		return this.goTXT_READY.activeSelf;
	}

	public override void SetRenderPos()
	{
		if (this.csRender3DRT != null)
		{
			UnityEngine.Object.DestroyImmediate(this.csRender3DRT.gameObject);
		}
		else
		{
			this.v3Chara = CMD_MultiRecruitPartyWait.Instance.Get3DPos();
		}
	}

	public void AddInitLabel(string nickname, string titleId, CMD_MultiRecruitPartyWait.USER_TYPE playerNum, bool leaderFlg)
	{
		this.lbTXT_NICKNAME.text = nickname;
		TitleDataMng.SetTitleIcon(titleId, this.goTITLE_ICON.GetComponent<UITexture>());
		this.spTAG_PLAYER.spriteName = "MultiBattle_P" + ((int)(playerNum + 1)).ToString();
	}

	public void ChangeRecruitMode(bool isRecruit)
	{
		if (isRecruit)
		{
			this.ChangeWaitMode();
		}
		else
		{
			if (!this.goUSER_DATA.activeSelf)
			{
				this.goUSER_DATA.SetActive(true);
			}
			if (this.goANIM_RECRUIT.activeSelf)
			{
				this.goANIM_RECRUIT.SetActive(false);
			}
		}
	}

	public void ChangeWaitMode()
	{
		if (this.goUSER_DATA.activeSelf)
		{
			this.goUSER_DATA.SetActive(false);
		}
		if (!this.goANIM_RECRUIT.activeSelf)
		{
			this.goANIM_RECRUIT_TXT.SetActive(true);
			this.goANIM_RECRUIT_OUT_CIRCLE.SetActive(true);
			this.goANIM_RECRUIT.SetActive(true);
			base.SpButtonActive(false);
		}
		this.stageGimmickObj.SetActive(false);
		if (this.sortieNG.activeSelf)
		{
			this.sortieNG.SetActive(false);
		}
	}

	public void SetStatusPanelActiveCollider(bool active)
	{
		for (int i = 0; i < this.statusPanel.pageList.Length; i++)
		{
			BoxCollider component = this.statusPanel.pageList[i].GetComponent<BoxCollider>();
			if (null != component)
			{
				component.enabled = active;
			}
		}
	}

	public GameObject GetEmotionParentObject()
	{
		return this.emotionParent;
	}

	public void SetSortieLimit(List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> limitList)
	{
		this.sortieLimitList = limitList;
	}

	protected override void SetSortieLimitCondition(MonsterData monsterData)
	{
		bool flag = true;
		if (this.sortieLimitList != null && 0 < this.sortieLimitList.Count)
		{
			string tribe = monsterData.monsterMG.tribe;
			string growStep = monsterData.monsterMG.growStep;
			flag = ClassSingleton<QuestData>.Instance.CheckSortieLimit(this.sortieLimitList, tribe, growStep);
		}
		if (!flag)
		{
			if (!this.sortieNG.activeSelf)
			{
				this.sortieNG.SetActive(true);
				if (this.monsterUserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
				{
					if (!this.goANIM_RECRUIT.activeSelf)
					{
						this.goANIM_RECRUIT_TXT.SetActive(true);
						this.goANIM_RECRUIT_OUT_CIRCLE.SetActive(true);
						this.goANIM_RECRUIT.SetActive(true);
						base.SpButtonActive(false);
					}
					this.sortieNG_Label.transform.localPosition = new Vector3(0f, this.sortieNgLabelPositionY_Owner, 0f);
				}
				else
				{
					this.sortieNG_Label.transform.localPosition = new Vector3(0f, this.sortieNgLabelPositionY_Member, 0f);
				}
			}
		}
		else if (this.sortieNG.activeSelf)
		{
			this.sortieNG.SetActive(false);
		}
	}

	public void SetMonsterUserType(CMD_MultiRecruitPartyWait.USER_TYPE type)
	{
		this.monsterUserType = type;
	}
}
