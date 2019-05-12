using EvolutionDiagram;
using Master;
using Monster;
using MonsterIcon;
using MonsterIconExtensions;
using Picturebook;
using System;
using UnityEngine;

namespace EvolutionRouteMap
{
	public class UI_EvolutionRouteMapSelectMonster : MonoBehaviour
	{
		[SerializeField]
		private CMD_EvolutionRouteMap dialogRoot;

		[SerializeField]
		private int iconSize;

		[SerializeField]
		private Vector2 iconPosition;

		[SerializeField]
		private Color iconTextColor;

		[SerializeField]
		private MonsterBasicInfo basicInfo;

		[SerializeField]
		private GameObject noticeLabel;

		private MonsterIcon monsterIcon;

		private void SetMonsterInfo(EvolutionDiagramData.IconMonster selectMonster)
		{
			this.monsterIcon.SetMonsterImage(selectMonster.master);
			if (!MonsterPicturebookData.ExistPicturebook(selectMonster.master.Group.monsterCollectionId))
			{
				this.monsterIcon.Message.SetSortText(StringMaster.GetString("EvolutionUnkown"));
			}
			else
			{
				this.monsterIcon.Message.ClearSortText();
			}
			if (MonsterArousalData.IsVersionUp(selectMonster.master.Simple.rare))
			{
				this.noticeLabel.SetActive(true);
			}
			MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(selectMonster.master.Simple.monsterId);
			this.basicInfo.SetMonsterData(monsterData);
		}

		private void OnPushedCharacterModelButton()
		{
			this.dialogRoot.OnPushed3DButton();
		}

		public void Initialize()
		{
			UIWidget component = base.GetComponent<UIWidget>();
			int depth = component.depth;
			this.monsterIcon = this.dialogRoot.GetMonsterIconObject();
			this.monsterIcon.Initialize(base.transform, this.iconSize, depth);
			this.monsterIcon.Message.SetSortTextColor(this.iconTextColor);
			GameObject rootObject = this.monsterIcon.GetRootObject();
			rootObject.transform.localPosition = new Vector3(this.iconPosition.x, this.iconPosition.y, 0f);
		}

		public void OnChangeMonster(EvolutionDiagramData.IconMonster selectMonster)
		{
			this.SetMonsterInfo(selectMonster);
		}
	}
}
