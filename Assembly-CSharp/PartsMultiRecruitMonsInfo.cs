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
	private UISprite spTAG_PLAYER;

	[SerializeField]
	private GameObject emotionParent;

	private MonsterData baseMonster;

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
		int[] chipIdList = monsterData.GetChipIdList();
		this.chipBaseSelect.SetSelectedCharChg(chipIdList, false, 0);
	}

	public override void OnTapedMonster()
	{
		CMD_MultiRecruitPartyWait.Instance.ClearStExchange();
		this.baseMonster = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(base.Data.userMonster.userMonsterId, false);
		CMD_DeckList.OriginMonsterData = this.baseMonster;
		CMD_DeckList cmd_DeckList = GUIMain.ShowCommonDialog(new Action<int>(this.OnClosedDeckList), "CMD_DeckList") as CMD_DeckList;
		cmd_DeckList.PartsTitle.DisableCloseBtn(true);
		cmd_DeckList.PPMI_Instance = this;
		cmd_DeckList.ppmiList = new List<PartsPartyMonsInfo>();
		if (PartsMenu.instance != null)
		{
			PartsMenu.instance.Active(false);
		}
		base.HideStatusPanel();
	}

	private void OnClosedDeckList(int noop)
	{
		if (CMD_DeckList.OriginMonsterData.userMonster.userMonsterId != base.Data.userMonster.userMonsterId)
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

	public void AddInitLabel(string nickname, CMD_MultiRecruitPartyWait.USER_TYPE playerNum, bool leaderFlg)
	{
		this.lbTXT_NICKNAME.text = nickname;
		this.spTAG_PLAYER.spriteName = "MultiBattle_P" + ((int)(playerNum + 1)).ToString();
	}

	public void ChangeRecruitMode(bool isRecruit)
	{
		if (isRecruit)
		{
			this.goUSER_DATA.SetActive(false);
			this.goANIM_RECRUIT_TXT.SetActive(true);
			this.goANIM_RECRUIT_OUT_CIRCLE.SetActive(true);
			this.goANIM_RECRUIT.SetActive(true);
			this.stageGimmickObj.SetActive(false);
		}
		else
		{
			this.goUSER_DATA.SetActive(true);
			this.goANIM_RECRUIT.SetActive(false);
		}
	}

	public void ChangeWaitMode()
	{
		this.goUSER_DATA.SetActive(false);
		this.goANIM_RECRUIT_TXT.SetActive(false);
		this.goANIM_RECRUIT_OUT_CIRCLE.SetActive(false);
		this.goANIM_RECRUIT.SetActive(true);
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
}
