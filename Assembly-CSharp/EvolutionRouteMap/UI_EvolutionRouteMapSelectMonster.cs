using EvolutionDiagram;
using System;
using UnityEngine;

namespace EvolutionRouteMap
{
	public class UI_EvolutionRouteMapSelectMonster : MonoBehaviour
	{
		[SerializeField]
		private CMD_EvolutionRouteMap dialogRoot;

		[SerializeField]
		private MonsterThumbnail monsterIcon;

		[SerializeField]
		private MonsterBasicInfo basicInfo;

		private void SetMonsterInfo(EvolutionDiagramData.IconMonster selectMonster)
		{
			this.monsterIcon.SetImage(selectMonster.singleData.iconId, selectMonster.groupData.growStep);
			MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(selectMonster.singleData.monsterId);
			this.basicInfo.SetMonsterData(monsterData);
		}

		private void OnPushedCharacterModelButton()
		{
			this.dialogRoot.OnPushed3DButton();
		}

		public void Initialize()
		{
			this.monsterIcon.Initialize();
		}

		public void OnChangeMonster(EvolutionDiagramData.IconMonster selectMonster)
		{
			this.SetMonsterInfo(selectMonster);
		}
	}
}
