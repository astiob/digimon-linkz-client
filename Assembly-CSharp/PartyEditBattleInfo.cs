using Master;
using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PartyEditBattleInfo : MonoBehaviour
{
	[SerializeField]
	private GameObject bossIconRootObject;

	[SerializeField]
	private List<BossThumbnail> bossIconList;

	[SerializeField]
	private GameObject battleStartButton;

	[SerializeField]
	private UILabel battleStartButtonLabel;

	[SerializeField]
	private SortieLimitList sortieLimitList;

	[SerializeField]
	private GameObject colosseumButton;

	[SerializeField]
	private GameObject colosseumDeckTitle;

	private GUICollider battleStartButtonCollider;

	private UISprite battleStartButtonBackground;

	private void InitializeBossMonsterIcon(CMD_PartyEdit.MODE_TYPE type)
	{
		for (int i = 0; i < this.bossIconList.Count; i++)
		{
			this.bossIconList[i].Initialize();
		}
	}

	private void InitializeBattleStartButton(CMD_PartyEdit.MODE_TYPE type)
	{
		switch (type)
		{
		case CMD_PartyEdit.MODE_TYPE.SELECT:
		case CMD_PartyEdit.MODE_TYPE.PVP:
			this.battleStartButtonLabel.text = StringMaster.GetString("PartyBattleStart");
			break;
		case CMD_PartyEdit.MODE_TYPE.MULTI:
			this.battleStartButtonLabel.text = StringMaster.GetString("PartyRecruit");
			break;
		default:
			this.battleStartButton.SetActive(false);
			break;
		}
		this.battleStartButtonCollider = this.battleStartButton.GetComponent<GUICollider>();
		this.battleStartButtonBackground = this.battleStartButton.GetComponent<UISprite>();
	}

	private void InitializeSortieLimitList(CMD_PartyEdit.MODE_TYPE type)
	{
		this.sortieLimitList.Initialize();
	}

	private void InitializeColosseumButton(CMD_PartyEdit.MODE_TYPE type)
	{
		if (type == CMD_PartyEdit.MODE_TYPE.EDIT && DataMng.Instance().IsReleaseColosseum)
		{
			this.colosseumButton.SetActive(true);
			this.colosseumDeckTitle.SetActive(true);
		}
	}

	public void SetView(CMD_PartyEdit.MODE_TYPE type)
	{
		this.InitializeBossMonsterIcon(type);
		this.InitializeBattleStartButton(type);
		this.InitializeSortieLimitList(type);
		this.InitializeColosseumButton(type);
	}

	public void SetBossMonsterIcon(List<GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy> enemyList)
	{
		int num = Mathf.Min(enemyList.Count, this.bossIconList.Count);
		if (0 < num)
		{
			this.bossIconRootObject.SetActive(true);
		}
		for (int i = 0; i < num; i++)
		{
			this.bossIconList[i].SetBossInfo(enemyList[i]);
		}
	}

	public void SetSortieLimit()
	{
		if (ClassSingleton<QuestData>.Instance.SelectDungeon != null)
		{
			string worldDungeonId = ClassSingleton<QuestData>.Instance.SelectDungeon.worldDungeonId;
			if (!string.IsNullOrEmpty(worldDungeonId))
			{
				this.sortieLimitList.SetSortieLimit(worldDungeonId.ToInt32());
			}
		}
	}

	public List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> GetSortieLimitList()
	{
		return this.sortieLimitList.GetSortieLimitList();
	}

	public bool CheckSortieLimit(MonsterData monsterData)
	{
		bool result = true;
		List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> list = this.sortieLimitList.GetSortieLimitList();
		if (list != null && 0 < list.Count)
		{
			result = ClassSingleton<QuestData>.Instance.CheckSortieLimit(list, monsterData.monsterMG.tribe, monsterData.monsterMG.growStep);
		}
		return result;
	}

	public bool CheckSortieLimit(List<MonsterData> monsterList)
	{
		bool flag = true;
		List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> list = this.sortieLimitList.GetSortieLimitList();
		if (list != null && 0 < list.Count)
		{
			for (int i = 0; i < monsterList.Count; i++)
			{
				flag = ClassSingleton<QuestData>.Instance.CheckSortieLimit(list, monsterList[i].monsterMG.tribe, monsterList[i].monsterMG.growStep);
				if (!flag)
				{
					break;
				}
			}
		}
		return flag;
	}

	public void EnableBattleStartButton(bool enable)
	{
		this.battleStartButtonCollider.activeCollider = enable;
		if (enable)
		{
			this.battleStartButtonBackground.spriteName = "Common02_Btn_Red";
			this.battleStartButtonLabel.color = Color.white;
		}
		else
		{
			this.battleStartButtonBackground.spriteName = "Common02_Btn_Gray";
			this.battleStartButtonLabel.color = Color.gray;
		}
	}
}
