using EvolutionDiagram;
using Master;
using Monster;
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

		[SerializeField]
		private GameObject noticeLabel;

		private void SetMonsterInfo(EvolutionDiagramData.IconMonster selectMonster)
		{
			this.monsterIcon.SetImage(selectMonster.singleData.iconId, selectMonster.groupData.growStep);
			if (!MonsterDataMng.ExistPicturebook(selectMonster.groupData.monsterCollectionId))
			{
				this.monsterIcon.SetBottomText(StringMaster.GetString("EvolutionUnkown"));
			}
			else
			{
				this.monsterIcon.ClearBottomText();
			}
			if (MonsterArousalData.IsVersionUp(selectMonster.singleData.rare))
			{
				this.noticeLabel.SetActive(true);
			}
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
