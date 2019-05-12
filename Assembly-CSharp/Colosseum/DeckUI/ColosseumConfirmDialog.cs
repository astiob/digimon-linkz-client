using Master;
using System;

namespace Colosseum.DeckUI
{
	public sealed class ColosseumConfirmDialog
	{
		private CMD_ColosseumDeck parentDialog;

		public ColosseumConfirmDialog(CMD_ColosseumDeck dialog)
		{
			this.parentDialog = dialog;
		}

		public void OnCloseAction(int selectButtonIndex)
		{
			if (selectButtonIndex == 0)
			{
				this.parentDialog.CloseColosseumDeckDialog();
			}
		}

		public static void OpenDialog(CMD_ColosseumDeck deckDialog)
		{
			ColosseumConfirmDialog @object = new ColosseumConfirmDialog(deckDialog);
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(@object.OnCloseAction), "CMD_Confirm", null) as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("SystemConfirm");
			cmd_Confirm.Info = StringMaster.GetString("ColosseumDeckConfirmInfo");
		}
	}
}
