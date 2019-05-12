using EvolutionDiagram;
using Master;
using Monster;
using MonsterIcon;
using MonsterIconExtensions;
using Picturebook;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionRouteMap
{
	public class GUI_EvolutionRouteMapListItem : GUIListPartsWrapper
	{
		[SerializeField]
		private GUI_EvolutionRouteMapList listRoot;

		[SerializeField]
		private UILabel growStep;

		[SerializeField]
		private UILabel specificName;

		[SerializeField]
		private UILabel monsterName;

		[SerializeField]
		private MonsterIcon monsterIcon;

		[SerializeField]
		private Vector2 iconPosition;

		[SerializeField]
		private int iconSize;

		[SerializeField]
		private Color iconTextColor;

		private void Initialize()
		{
			this.playSelectSE = true;
			this.touchBehavior = GUICollider.TouchBehavior.None;
			UIWidget component = this.listRoot.GetComponent<UIWidget>();
			int depth = component.depth;
			this.monsterIcon = this.listRoot.GetMonsterIconObject();
			this.monsterIcon.Initialize(base.transform, this.iconSize, depth);
			this.monsterIcon.Message.SetSortTextColor(this.iconTextColor);
			GameObject rootObject = this.monsterIcon.GetRootObject();
			rootObject.transform.localPosition = new Vector3(this.iconPosition.x, this.iconPosition.y, 0f);
			base.InitializeInputEvent();
		}

		private void OnPushed()
		{
			int idx = base.IDX;
			List<EvolutionDiagramData.IconMonster> routeMapData = this.listRoot.GetRouteMapData();
			if (routeMapData != null && idx < routeMapData.Count && routeMapData[idx] != null)
			{
				this.listRoot.OnChangeSelectMonster(routeMapData[idx]);
			}
		}

		protected override void OnUpdatedParts(int listPartsIndex)
		{
			List<EvolutionDiagramData.IconMonster> routeMapData = this.listRoot.GetRouteMapData();
			if (routeMapData != null && listPartsIndex < routeMapData.Count && routeMapData[listPartsIndex] != null)
			{
				EvolutionDiagramData.IconMonster iconMonster = routeMapData[listPartsIndex];
				this.monsterIcon.SetMonsterImage(iconMonster.master);
				if (!MonsterPicturebookData.ExistPicturebook(iconMonster.master.Group.monsterCollectionId))
				{
					this.monsterIcon.Message.SetSortText(StringMaster.GetString("EvolutionUnkown"));
				}
				else
				{
					this.monsterIcon.Message.ClearSortText();
				}
				this.growStep.text = MonsterGrowStepData.GetGrowStepName(iconMonster.master.Group.growStep);
				this.monsterName.text = iconMonster.master.Group.monsterName;
				this.specificName.text = MonsterSpecificTypeData.GetSpecificTypeName(iconMonster.master.Group.monsterStatusId);
			}
		}

		public override void InitParts()
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			this.Initialize();
		}
	}
}
