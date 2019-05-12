using EvolutionDiagram;
using Master;
using System;
using UnityEngine;

namespace EvolutionRouteMap
{
	public sealed class CMD_EvolutionRouteMap : CMDWrapper
	{
		[SerializeField]
		private UI_EvolutionRouteMapSelectMonster selectMonsterInfo;

		[SerializeField]
		private GUI_EvolutionRouteMapList beforeList;

		[SerializeField]
		private GUI_EvolutionRouteMapList afterList;

		private EvolutionRouteMapData routeMapData;

		private bool openEvolutionDiagram;

		private void OnPushedReturnTopButton()
		{
			if (null == this.parentDialogGameObject || null == this.parentDialogGameObject.GetComponent<CMD_EvolutionDiagram>())
			{
				this.openEvolutionDiagram = true;
			}
			base.ClosePanel(true);
		}

		protected override void OnShowDialog()
		{
			base.PartsTitle.SetTitle(StringMaster.GetString("EvolutionDiagramTitle"));
			this.selectMonsterInfo.Initialize();
			this.beforeList.InitializeView(1);
			this.afterList.InitializeView(1);
			this.UpdateSelectMonster();
			this.UpdateViewList();
			UI_EvolutionCharacterModelViewer component = base.gameObject.GetComponent<UI_EvolutionCharacterModelViewer>();
			if (null != component)
			{
				component.Initialize();
			}
		}

		protected override void OnOpenedDialog()
		{
		}

		protected override void OnCloseStartDialog()
		{
		}

		protected override void OnClosedDialog()
		{
			UI_EvolutionCharacterModelViewer component = base.gameObject.GetComponent<UI_EvolutionCharacterModelViewer>();
			if (null != component)
			{
				component.DeleteModelViewer();
			}
			if (this.openEvolutionDiagram)
			{
				CMD_EvolutionDiagram.CreateDialog(this.parentDialogGameObject);
			}
		}

		public static void CreateDialog(GameObject parentDialog, MonsterData monsterData)
		{
			EvolutionDiagramData.IconMonster monsterData2 = new EvolutionDiagramData.IconMonster
			{
				collectionId = monsterData.monsterMG.monsterCollectionId.ToInt32(),
				singleData = monsterData.monsterM,
				groupData = monsterData.monsterMG
			};
			CMD_EvolutionRouteMap.CreateDialog(parentDialog, monsterData2);
		}

		public static void CreateDialog(GameObject parentDialog, EvolutionDiagramData.IconMonster monsterData)
		{
			EvolutionRouteMapData evolutionRouteMapData = new EvolutionRouteMapData();
			evolutionRouteMapData.SetSelectMonster(monsterData);
			CMD_EvolutionRouteMap cmd_EvolutionRouteMap = CMDWrapper.LoadPrefab<CMD_EvolutionRouteMap>("CMD_EvolutionRouteMap");
			cmd_EvolutionRouteMap.parentDialogGameObject = parentDialog;
			cmd_EvolutionRouteMap.routeMapData = evolutionRouteMapData;
			cmd_EvolutionRouteMap.Show();
		}

		public EvolutionRouteMapData GetRouteMapData()
		{
			return this.routeMapData;
		}

		public void UpdateSelectMonster()
		{
			this.selectMonsterInfo.OnChangeMonster(this.routeMapData.GetSelectMonster());
		}

		public void UpdateViewList()
		{
			this.beforeList.OnChangeRouteMap(this.routeMapData.GetBeforeRouteList());
			this.afterList.OnChangeRouteMap(this.routeMapData.GetAfterRouteList());
		}

		public void OnPushed3DButton()
		{
			UI_EvolutionCharacterModelViewer component = base.GetComponent<UI_EvolutionCharacterModelViewer>();
			component.OnPushed3DButton();
		}

		public void OnPushedReturnButton()
		{
			UI_EvolutionCharacterModelViewer component = base.GetComponent<UI_EvolutionCharacterModelViewer>();
			component.OnPushedReturnButton();
		}
	}
}
