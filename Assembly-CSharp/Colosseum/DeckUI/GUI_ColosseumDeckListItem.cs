using Master;
using Monster;
using MonsterIcon;
using MonsterIconExtensions;
using MonsterList.ChangeMonster;
using Quest;
using System;
using UI.MonsterInfoParts;
using UnityEngine;

namespace Colosseum.DeckUI
{
	[RequireComponent(typeof(BoxCollider))]
	public sealed class GUI_ColosseumDeckListItem : MonsterIconTouchEvent
	{
		[SerializeField]
		private UI_ColosseumDeckList parentList;

		[SerializeField]
		private int iconSize;

		[SerializeField]
		private int index;

		private ColosseumDeckData deckData;

		private MonsterIcon monsterIcon;

		private void OnPushedItem()
		{
			bool flag = false;
			if (this.parentList.GetSelectItem() == this.index || this.parentList.GetMonster(this.index) == null)
			{
				flag = true;
			}
			if (this.parentList.GetSelectItem() != this.index)
			{
				this.parentList.SetSelectItem(this.index);
				this.parentList.UpdateSelectedMonster();
			}
			if (flag)
			{
				CMD_ChangeMonster.SelectMonsterData = (this.parentList.GetSelectMonster() as MonsterData);
				CMD_ChangeMonster cmd_ChangeMonster = GUIMain.ShowCommonDialog(null, "CMD_ChangeMonster", null) as CMD_ChangeMonster;
				cmd_ChangeMonster.SetChangedAction(new Action<MonsterUserData>(this.parentList.UpdateList));
				cmd_ChangeMonster.SetIconColosseumDeck(this.parentList.GetSelectMonster(), this.parentList.GetDeckMonsterList(), this.deckData.SortieLimitList.GetSortieLimitList());
			}
		}

		private void OnLongPressItem()
		{
			MonsterUserData monster = this.parentList.GetMonster(this.index);
			if (monster != null)
			{
				CMD_CharacterDetailed.DataChg = (monster as MonsterData);
				CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
				cmd_CharacterDetailed.DisableEvolutionButton();
			}
		}

		private void SetSortieLimitState(GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterMaster)
		{
			if (!ClassSingleton<QuestData>.Instance.CheckSortieLimit(this.deckData.SortieLimitList.GetSortieLimitList(), monsterMaster.tribe, monsterMaster.growStep))
			{
				this.monsterIcon.Message.SetStateText(StringMaster.GetString("PartySortieLimitNG"));
				MonsterIconGrayout.SetGrayout(this.monsterIcon.GetRootObject(), GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE);
			}
			else
			{
				this.monsterIcon.Message.ClearStateText();
				MonsterIconGrayout.SetGrayout(this.monsterIcon.GetRootObject(), GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
			}
		}

		public void Initialize(ColosseumDeckData data, MonsterIcon icon)
		{
			this.deckData = data;
			UIWidget component = this.parentList.GetComponent<UIWidget>();
			int depth = component.depth;
			this.monsterIcon = icon;
			this.monsterIcon.Initialize(base.transform, this.iconSize, depth);
			base.InitializeInputEvent();
			this.actionTouch = new Action(this.OnPushedItem);
			this.actionLongPress = new Action(this.OnLongPressItem);
		}

		public void SetItemEmpty()
		{
			this.monsterIcon.ClaerDetailed();
		}

		public void SetItemDetailed(MonsterUserData monster)
		{
			this.monsterIcon.SetColosseumDeckMonsterDetailed(monster);
			GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = monster.GetMonsterMaster().Group;
			this.SetSortieLimitState(group);
		}

		public void SetSelect()
		{
			this.monsterIcon.Message.SetStateText(StringMaster.GetString("SystemSelect"));
			MonsterIconGrayout.SetGrayout(this.monsterIcon.GetRootObject(), GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE);
		}

		public void ClearSelect(GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterMaster)
		{
			this.SetSortieLimitState(monsterMaster);
		}
	}
}
