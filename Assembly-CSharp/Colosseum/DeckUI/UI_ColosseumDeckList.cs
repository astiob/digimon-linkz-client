using Monster;
using MonsterIcon;
using Quest;
using System;
using System.Collections.Generic;
using UI.MonsterInfoParts;
using UnityEngine;

namespace Colosseum.DeckUI
{
	public sealed class UI_ColosseumDeckList : MonoBehaviour
	{
		private const int DECK_MONSTER_NUM = 6;

		[SerializeField]
		private GUI_ColosseumDeckListItem[] listItemList;

		private ColosseumDeckData deckData;

		private string[] saveDeckUserMonsterIdList;

		private MonsterUserData[] deckMonsterList;

		private MonsterUserData selectMonster;

		private int selectItemIndex;

		private void CreateDeckUserMonsterID(List<MonsterData> saveDeckMonsterList)
		{
			if (this.saveDeckUserMonsterIdList == null)
			{
				this.saveDeckUserMonsterIdList = new string[6];
			}
			for (int i = 0; i < saveDeckMonsterList.Count; i++)
			{
				if (i < 6)
				{
					this.saveDeckUserMonsterIdList[i] = saveDeckMonsterList[i].GetMonster().userMonsterId;
				}
			}
		}

		private void CreateDeckMonster(List<MonsterData> saveDeckMonsterList)
		{
			if (saveDeckMonsterList.Count < 6)
			{
				this.deckMonsterList = new MonsterUserData[6];
			}
			else
			{
				this.deckMonsterList = saveDeckMonsterList.ToArray();
			}
		}

		private void SetMonsterIcon()
		{
			for (int i = 0; i < 6; i++)
			{
				if (this.deckMonsterList[i] != null)
				{
					this.listItemList[i].SetItemDetailed(this.deckMonsterList[i]);
				}
				else
				{
					this.listItemList[i].SetItemEmpty();
				}
			}
		}

		private void InitializeDeckInfo()
		{
			List<MonsterData> colosseumDeckUserMonsterList = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList();
			this.CreateDeckUserMonsterID(colosseumDeckUserMonsterList);
			this.CreateDeckMonster(colosseumDeckUserMonsterList);
			this.SetMonsterIcon();
		}

		private bool ExistDeckMonster(string userMonsterId)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(userMonsterId))
			{
				for (int i = 0; i < 6; i++)
				{
					if (this.deckMonsterList[i] != null && this.deckMonsterList[i].GetMonster().userMonsterId == userMonsterId)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		private int GetDeckIndex(string userMonsterId)
		{
			int result = 6;
			if (!string.IsNullOrEmpty(userMonsterId))
			{
				for (int i = 0; i < 6; i++)
				{
					if (this.deckMonsterList[i] != null && this.deckMonsterList[i].GetMonster().userMonsterId == userMonsterId)
					{
						result = i;
						break;
					}
				}
			}
			return result;
		}

		private void OnLongPressIcon()
		{
			if (this.deckMonsterList[this.selectItemIndex] != null)
			{
				CMD_CharacterDetailed.DataChg = (this.deckMonsterList[this.selectItemIndex] as MonsterData);
				CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
				cmd_CharacterDetailed.DisableEvolutionButton();
			}
		}

		public void Initialize(ColosseumDeckData data)
		{
			this.deckData = data;
			MonsterIcon icon = MonsterIconFactory.CreateIcon(13);
			for (int i = 0; i < 6; i++)
			{
				this.listItemList[i].Initialize(data, MonsterIconFactory.Copy(icon));
			}
			this.InitializeDeckInfo();
		}

		public void SetSelectItem(int index)
		{
			MonsterUserData monsterUserData = this.deckMonsterList[this.selectItemIndex];
			if (monsterUserData != null)
			{
				MonsterClientMaster monsterMaster = monsterUserData.GetMonsterMaster();
				this.listItemList[this.selectItemIndex].ClearSelect(monsterMaster.Group);
			}
			this.selectItemIndex = index;
			monsterUserData = this.deckMonsterList[index];
			if (monsterUserData != null)
			{
				this.listItemList[index].SetSelect();
			}
		}

		public MonsterUserData[] GetDeckMonsterList()
		{
			return this.deckMonsterList;
		}

		public MonsterUserData GetSelectMonster()
		{
			return this.deckMonsterList[this.selectItemIndex];
		}

		public MonsterUserData GetMonster(int index)
		{
			return this.deckMonsterList[index];
		}

		public string[] GetDeckMonsterUserMonsterIdList()
		{
			string[] array = new string[6];
			for (int i = 0; i < 6; i++)
			{
				if (this.deckMonsterList[i] != null)
				{
					array[i] = this.deckMonsterList[i].GetMonster().userMonsterId;
				}
				else
				{
					array[i] = "0";
				}
			}
			return array;
		}

		public int GetSelectItem()
		{
			return this.selectItemIndex;
		}

		public void UpdateList(MonsterUserData changeMonster)
		{
			if (this.ExistDeckMonster(changeMonster.GetMonster().userMonsterId))
			{
				int deckIndex = this.GetDeckIndex(changeMonster.GetMonster().userMonsterId);
				if (deckIndex < 6)
				{
					this.deckMonsterList[deckIndex] = this.deckMonsterList[this.selectItemIndex];
				}
			}
			this.deckMonsterList[this.selectItemIndex] = changeMonster;
			this.SetMonsterIcon();
			this.UpdateSelectedMonster();
			this.deckData.DeckButton.UpdateButton();
		}

		public void UpdateSelectedMonster()
		{
			MonsterUserData monsterUserData = this.deckMonsterList[this.selectItemIndex];
			if (monsterUserData != null)
			{
				MonsterBasicInfoExtensions.SetData(this.deckData.MonsterBasicInfo, monsterUserData);
				this.deckData.MonsterChipSlotInfo.SetSelectedCharChg(monsterUserData as MonsterData);
				this.deckData.MonsterSelectedIcon.SetMonsterData(monsterUserData, false);
				this.deckData.MonsterSelectedIcon.SetTouchAction(null);
				this.deckData.MonsterSelectedIcon.SetPressAction(new Action(this.OnLongPressIcon));
				this.deckData.MiniStatus.SetMonsterData(monsterUserData as MonsterData);
			}
			else
			{
				this.deckData.MonsterBasicInfo.ClearMonsterData();
				this.deckData.MonsterChipSlotInfo.ClearChipIcons();
				this.deckData.MonsterSelectedIcon.ClearMonsterData();
				this.deckData.MiniStatus.ClearMonsterData();
			}
		}

		public bool IsComplete()
		{
			bool result = true;
			for (int i = 0; i < 6; i++)
			{
				if (this.deckMonsterList[i] == null)
				{
					result = false;
					break;
				}
				if (!ClassSingleton<QuestData>.Instance.CheckSortieLimit(this.deckData.SortieLimitList.GetSortieLimitList(), this.deckMonsterList[i].GetMonsterMaster().Group.tribe, this.deckMonsterList[i].GetMonsterMaster().Group.growStep))
				{
					result = false;
					break;
				}
			}
			return result;
		}

		public bool IsDirty()
		{
			bool result = false;
			for (int i = 0; i < 6; i++)
			{
				global::Debug.Assert(null != this.deckMonsterList[i], "デッキが完成していないのに保存しようとしています.");
				if (this.saveDeckUserMonsterIdList[i] != this.deckMonsterList[i].GetMonster().userMonsterId)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public void UpdateDeckMonster()
		{
			List<MonsterData> colosseumDeckUserMonsterList = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList();
			this.CreateDeckUserMonsterID(colosseumDeckUserMonsterList);
		}
	}
}
