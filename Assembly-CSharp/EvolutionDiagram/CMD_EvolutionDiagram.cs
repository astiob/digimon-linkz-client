using Master;
using Monster;
using Picturebook;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionDiagram
{
	public sealed class CMD_EvolutionDiagram : CMDWrapper
	{
		[SerializeField]
		private GUI_EvolutionDiagramIconList iconList;

		[SerializeField]
		private UI_EvolutionDiagramRefineButtonList buttonList;

		private EvolutionDiagramData evolutionData;

		private static void SetDialog(GameObject parentDialog)
		{
			EvolutionDiagramData evolutionDiagramData = new EvolutionDiagramData();
			CMD_EvolutionDiagram cmd_EvolutionDiagram = CMDWrapper.LoadPrefab<CMD_EvolutionDiagram>("CMD_EvolutionDiagram");
			cmd_EvolutionDiagram.parentDialogGameObject = parentDialog;
			cmd_EvolutionDiagram.evolutionData = evolutionDiagramData;
			cmd_EvolutionDiagram.Show();
		}

		protected override void OnShowDialog()
		{
			base.SetTutorialAnyTime("anytime_second_tutorial_evolution_rootmap");
			base.PartsTitle.SetTitle(StringMaster.GetString("EvolutionDiagramTitle"));
			this.buttonList.Initialize();
			GrowStep defaultSelectGrowStep = this.buttonList.GetDefaultSelectGrowStep();
			List<EvolutionDiagramData.IconMonster> monsterDataList = this.evolutionData.GetMonsterDataList(defaultSelectGrowStep);
			this.evolutionData.SetViewData(defaultSelectGrowStep, monsterDataList);
			this.iconList.Initialize();
			this.UpdateViewList();
		}

		protected override void OnOpenedDialog()
		{
			FarmCameraControlForCMD.Off();
		}

		protected override void OnCloseStartDialog()
		{
			FarmCameraControlForCMD.On();
		}

		protected override void OnClosedDialog()
		{
		}

		public static void CreateDialog(GameObject parentDialog)
		{
			if (!MonsterPicturebookData.IsReady())
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
				APIRequestTask task = MonsterPicturebookData.RequestUserPicturebook();
				AppCoroutine.Start(task.Run(delegate
				{
					RestrictionInput.EndLoad();
					CMD_EvolutionDiagram.SetDialog(parentDialog);
				}, delegate(Exception noop)
				{
					RestrictionInput.EndLoad();
				}, null), false);
			}
			else
			{
				CMD_EvolutionDiagram.SetDialog(parentDialog);
			}
		}

		public EvolutionDiagramData GetEvolutionData()
		{
			return this.evolutionData;
		}

		public void UpdateViewList()
		{
			this.iconList.OnChangeRefine(this.evolutionData.GetViewData());
		}
	}
}
