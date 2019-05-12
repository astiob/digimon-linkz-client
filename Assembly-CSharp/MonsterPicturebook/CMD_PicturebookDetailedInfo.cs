using Master;
using System;
using UnityEngine;

namespace MonsterPicturebook
{
	public sealed class CMD_PicturebookDetailedInfo : CMDWrapper
	{
		[SerializeField]
		private PictureDetailedInfoView detailedInfoView;

		[SerializeField]
		private PicturebookModelViewTransition modelViewTransition;

		private PicturebookDetailedInfo viewInfo;

		private GameObject parentDialog;

		protected override void OnShowDialog()
		{
			base.PartsTitle.SetTitle(StringMaster.GetString("PicturebookTitle"));
			UI_PictureDetailedInfo ui_PictureDetailedInfo = this.detailedInfoView.LoadUI(this.viewInfo.uniqueSkillCount);
			ui_PictureDetailedInfo.SetMonsterData(this.viewInfo);
			UIWidget component = base.GetComponent<UIWidget>();
			this.modelViewTransition.Initialize(component.depth);
			this.modelViewTransition.LoadModel(this.viewInfo.monster.monsterMaster.Group.modelId);
		}

		protected override void OnOpenedDialog()
		{
			this.parentDialog.SetActive(false);
		}

		protected override void OnCloseStartDialog()
		{
			this.parentDialog.SetActive(true);
		}

		protected override void OnClosedDialog()
		{
			this.modelViewTransition.Delete();
		}

		public static void CreateDialog(GameObject parentDialog, PicturebookMonster monster)
		{
			CMD_PicturebookDetailedInfo cmd_PicturebookDetailedInfo = CMDWrapper.LoadPrefab<CMD_PicturebookDetailedInfo>("CMD_PictureBookDetail 1");
			cmd_PicturebookDetailedInfo.parentDialog = parentDialog;
			cmd_PicturebookDetailedInfo.viewInfo = new PicturebookDetailedInfo(monster);
			cmd_PicturebookDetailedInfo.Show();
		}

		private void OnPushedModelViewerButton()
		{
			this.modelViewTransition.OnPushedButtonViewButton();
		}
	}
}
