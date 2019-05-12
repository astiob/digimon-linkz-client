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
	private GameObject favoriteButton;

	[SerializeField]
	private UILabel favoriteButtonLabel;

	[SerializeField]
	private Color favoriteButtonLabelColor;

	[SerializeField]
	private SortieLimitList sortieLimitList;

	private GUICollider favoriteButtonCollider;

	private UISprite favoriteButtonBackground;

	private GUICollider battleStartButtonCollider;

	private UISprite battleStartButtonBackground;

	private int favoriteDeckNo;

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

	private void InitializeFavoriteButton(CMD_PartyEdit.MODE_TYPE type)
	{
		string favoriteDeckNum = DataMng.Instance().RespDataMN_DeckList.favoriteDeckNum;
		this.favoriteDeckNo = favoriteDeckNum.ToInt32();
		this.favoriteButtonCollider = this.favoriteButton.GetComponent<GUICollider>();
		this.favoriteButtonBackground = this.favoriteButton.GetComponent<UISprite>();
		switch (type)
		{
		case CMD_PartyEdit.MODE_TYPE.SELECT:
		case CMD_PartyEdit.MODE_TYPE.PVP:
		case CMD_PartyEdit.MODE_TYPE.MULTI:
			this.favoriteButton.SetActive(false);
			break;
		default:
			if (!this.favoriteButton.activeSelf)
			{
				this.favoriteButton.SetActive(true);
			}
			break;
		}
	}

	private void InitializeSortieLimitList(CMD_PartyEdit.MODE_TYPE type)
	{
		this.sortieLimitList.Initialize();
	}

	public void SetView(CMD_PartyEdit.MODE_TYPE type)
	{
		this.InitializeBossMonsterIcon(type);
		this.InitializeBattleStartButton(type);
		this.InitializeFavoriteButton(type);
		this.InitializeSortieLimitList(type);
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

	public void EnableFavoriteButton(bool enable)
	{
		if (this.favoriteButton.activeSelf)
		{
			this.favoriteButtonCollider.activeCollider = !enable;
			if (enable)
			{
				this.favoriteButtonBackground.spriteName = "Common02_Btn_SupportRed";
				this.favoriteButtonLabel.color = Color.white;
			}
			else
			{
				this.favoriteButtonBackground.spriteName = "Common02_Btn_SupportWhite";
				this.favoriteButtonLabel.color = this.favoriteButtonLabelColor;
			}
		}
	}

	public void SetFavoriteDeckNo(int partyNo)
	{
		this.favoriteDeckNo = partyNo;
		this.EnableFavoriteButton(true);
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
		if (cmd_ModalMessage != null)
		{
			cmd_ModalMessage.Title = StringMaster.GetString("PartyFavoriteTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("PartyFavoriteInfo");
		}
	}

	public int GetFavoriteDeckNo()
	{
		return this.favoriteDeckNo;
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
