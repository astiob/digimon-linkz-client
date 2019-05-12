using Master;
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
			base.PartsTitle.SetTitle(StringMaster.GetString("EvolutionDiagramTitle"));
			this.buttonList.Initialize();
			GrowStep defaultSelectGrowStep = this.buttonList.GetDefaultSelectGrowStep();
			List<EvolutionDiagramData.IconMonster> monsterDataList = this.evolutionData.GetMonsterDataList(defaultSelectGrowStep);
			this.evolutionData.SetViewData(defaultSelectGrowStep, monsterDataList);
			this.iconList.InitializeView(2);
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
			if (MonsterDataMng.userPicturebookData == null)
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
				APIRequestTask picturebookData = MonsterDataMng.Instance().GetPicturebookData(DataMng.Instance().RespDataCM_Login.playerInfo.userId, false);
				AppCoroutine.Start(picturebookData.Run(delegate
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
