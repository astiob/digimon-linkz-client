using EvolutionDiagram;
using Master;
using Monster;
using MonsterIcon;
using Picturebook;
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

		[SerializeField]
		private GameObject topButton;

		private EvolutionRouteMapData routeMapData;

		private bool openEvolutionDiagram;

		private MonsterIcon monsterIconSource;

		private static EvolutionDiagramData.IconMonster CreateMonsterData(MonsterClientMaster monsterMaster)
		{
			return new EvolutionDiagramData.IconMonster
			{
				collectionId = int.Parse(monsterMaster.Group.monsterCollectionId),
				master = monsterMaster
			};
		}

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
			base.SetTutorialAnyTime("anytime_second_tutorial_evolution_rootmap");
			base.PartsTitle.SetTitle(StringMaster.GetString("EvolutionDiagramTitle"));
			this.monsterIconSource = MonsterIconFactory.CreateIcon(1);
			this.selectMonsterInfo.Initialize();
			this.beforeList.Initialize();
			this.afterList.Initialize();
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

		protected override bool OnCloseStartDialog()
		{
			return true;
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

		public static void CreateDialog(GameObject parentDialog, MonsterClientMaster monsterMaster)
		{
			if (!MonsterPicturebookData.IsReady())
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
				APIRequestTask task = MonsterPicturebookData.RequestUserPicturebook();
				AppCoroutine.Start(task.Run(delegate
				{
					RestrictionInput.EndLoad();
					CMD_EvolutionRouteMap.CreateDialog(parentDialog, CMD_EvolutionRouteMap.CreateMonsterData(monsterMaster));
				}, delegate(Exception noop)
				{
					RestrictionInput.EndLoad();
				}, null), false);
			}
			else
			{
				CMD_EvolutionRouteMap.CreateDialog(parentDialog, CMD_EvolutionRouteMap.CreateMonsterData(monsterMaster));
			}
		}

		public static void CreateDialog(GameObject parentDialog, EvolutionDiagramData.IconMonster monsterData)
		{
			EvolutionRouteMapData evolutionRouteMapData = new EvolutionRouteMapData();
			evolutionRouteMapData.SetSelectMonster(monsterData);
			CMD_EvolutionRouteMap cmd_EvolutionRouteMap = CMDWrapper.LoadPrefab<CMD_EvolutionRouteMap>("CMD_EvolutionRouteMap");
			cmd_EvolutionRouteMap.parentDialogGameObject = parentDialog;
			cmd_EvolutionRouteMap.routeMapData = evolutionRouteMapData;
			cmd_EvolutionRouteMap.topButton.SetActive(null != parentDialog);
			cmd_EvolutionRouteMap.Show();
		}

		public EvolutionRouteMapData GetRouteMapData()
		{
			return this.routeMapData;
		}

		public MonsterIcon GetMonsterIconObject()
		{
			return MonsterIconFactory.Copy(this.monsterIconSource);
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
