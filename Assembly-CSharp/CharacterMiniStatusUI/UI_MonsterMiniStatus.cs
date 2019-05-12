using System;
using UnityEngine;

namespace CharacterMiniStatusUI
{
	public sealed class UI_MonsterMiniStatus : MonoBehaviour
	{
		[SerializeField]
		private MonsterStatusList statusList;

		[SerializeField]
		private MonsterMedalList medalList;

		[SerializeField]
		private UI_MonsterSkillPanel skillPanelShort;

		[SerializeField]
		private UI_MonsterSkillPanel skillPanelLong;

		[SerializeField]
		private StatusPanelViewControl statusPanel;

		private void OnEnable()
		{
			this.statusPanel.Initialize();
		}

		private void OnPushChangeButton()
		{
			this.statusPanel.SetNextPage();
		}

		public void SetMonsterData(MonsterData monsterData)
		{
			this.statusList.SetValues(monsterData, false, false);
			this.medalList.SetValues(monsterData.userMonster);
			this.skillPanelShort.SetMonsterData(monsterData);
			this.skillPanelLong.SetMonsterData(monsterData);
		}

		public void ClearMonsterData()
		{
			this.statusList.ClearValues();
			this.medalList.SetActive(false);
			this.skillPanelShort.ClearData();
			this.skillPanelLong.ClearData();
		}
	}
}
