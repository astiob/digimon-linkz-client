using EvolutionRouteMap;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionDiagram
{
	public sealed class GUI_EvolutionDiagramIconListItem : GUIListPartsWrapper
	{
		[SerializeField]
		private GUI_EvolutionDiagramIconList listComponent;

		private void OnPushedIcon()
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
				MonsterThumbnail component = base.gameObject.GetComponent<MonsterThumbnail>();
				component.SetImage(iconMonster.singleData.iconId, iconMonster.groupData.growStep);
			}
		}

		public override void InitParts()
		{
			this.playSelectSE = true;
			this.touchBehavior = GUICollider.TouchBehavior.None;
			MonsterThumbnail component = base.gameObject.GetComponent<MonsterThumbnail>();
			component.Initialize();
		}
	}
}
