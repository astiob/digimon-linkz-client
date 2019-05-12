using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.PartyEdit
{
	public sealed class PartyEditQuestBonusButton : MonoBehaviour
	{
		private QuestBonusPack stageBonus;

		private QuestBonusPack activateStageBonus;

		private QuestBonusTargetCheck bonusTargetChecker;

		private CMD_SPBonusList bonusList;

		private void OnDestroy()
		{
			if (null != this.bonusList)
			{
				this.bonusList.ClosePanel(true);
			}
		}

		private void OnPushButton()
		{
			List<string> bonusText = QuestBonus.GetBonusText(this.activateStageBonus.bonusChipIds, this.activateStageBonus.eventBonuses, this.activateStageBonus.dungeonBonuses);
			this.bonusList = (GUIMain.ShowCommonDialog(null, "CMD_SPBonusList", null) as CMD_SPBonusList);
			if (null != this.bonusList)
			{
				this.bonusList.SetViewData(bonusText);
			}
		}

		public void Initialize(QuestBonusPack questBonus, QuestBonusTargetCheck checker)
		{
			this.bonusTargetChecker = checker;
			this.stageBonus = questBonus;
		}

		public void SetTargetMonster(MonsterData monsterData)
		{
			if (this.stageBonus != null)
			{
				this.activateStageBonus = new QuestBonusPack
				{
					bonusChipIds = QuestBonusFilter.GetActivateBonusChips(monsterData, this.stageBonus.bonusChipIds),
					eventBonuses = QuestBonusFilter.GetActivateEventBonuses(this.bonusTargetChecker, monsterData, this.stageBonus.eventBonuses),
					dungeonBonuses = QuestBonusFilter.GetActivateDungeonBonuses(this.bonusTargetChecker, monsterData, this.stageBonus.dungeonBonuses)
				};
			}
		}

		public void SetActive()
		{
			bool flag = false;
			if (this.activateStageBonus != null && this.activateStageBonus.ExistBonus())
			{
				flag = true;
			}
			base.gameObject.SetActive(flag);
			if (!flag && null != this.bonusList)
			{
				this.bonusList.ClosePanel(true);
			}
		}
	}
}
