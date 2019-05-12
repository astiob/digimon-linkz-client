using EvolutionRouteMap;
using Master;
using MonsterIcon;
using MonsterIconExtensions;
using Picturebook;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionDiagram
{
	public sealed class GUI_EvolutionDiagramIconListItem : GUIListPartsWrapper
	{
		[SerializeField]
		private GUI_EvolutionDiagramIconList listComponent;

		[SerializeField]
		private int iconSize;

		[SerializeField]
		private Color iconTextColor;

		private MonsterIcon monsterIcon;

		private void Initialize()
		{
			this.playSelectSE = true;
			this.touchBehavior = GUICollider.TouchBehavior.None;
			UIWidget component = this.listComponent.GetComponent<UIWidget>();
			int depth = component.depth;
			this.monsterIcon = this.listComponent.GetMonsterIconObject();
			this.monsterIcon.Initialize(base.transform, this.iconSize, depth);
			this.monsterIcon.Message.SetSortTextColor(this.iconTextColor);
			base.InitializeInputEvent();
		}

		private void OnPushedItem()
		{
			int idx = base.IDX;
			List<EvolutionDiagramData.IconMonster> evolutionData = this.listComponent.GetEvolutionData();
			EvolutionDiagramData.IconMonster iconMonster = evolutionData[idx];
			if (iconMonster != null)
			{
				CMD_EvolutionDiagram dialogRoot = this.listComponent.GetDialogRoot();
				CMD_EvolutionRouteMap.CreateDialog(dialogRoot.gameObject, iconMonster);
			}
		}

		protected override void OnUpdatedParts(int listPartsIndex)
		{
			List<EvolutionDiagramData.IconMonster> evolutionData = this.listComponent.GetEvolutionData();
			EvolutionDiagramData.IconMonster iconMonster = evolutionData[listPartsIndex];
			if (iconMonster != null)
			{
				this.monsterIcon.SetMonsterImage(iconMonster.master);
				if (!MonsterPicturebookData.ExistPicturebook(iconMonster.master.Group.monsterCollectionId))
				{
					this.monsterIcon.Message.SetSortText(StringMaster.GetString("EvolutionUnkown"));
				}
				else
				{
					this.monsterIcon.Message.ClearSortText();
				}
			}
		}

		public override void InitParts()
		{
			this.Initialize();
		}
	}
}
