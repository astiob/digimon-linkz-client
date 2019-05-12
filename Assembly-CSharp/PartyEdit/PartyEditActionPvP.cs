using System;

namespace PartyEdit
{
	public class PartyEditActionPvP : PartyEditAction
	{
		private int battlePartyDeckNo;

		protected override void OnCompleteChangeOperation()
		{
			if (Loading.IsShow())
			{
				RestrictionInput.EndLoad();
			}
			CMD_PvPMatchingWait cmd_PvPMatchingWait = GUIMain.ShowCommonDialog(null, "CMD_PvPMatchingWait") as CMD_PvPMatchingWait;
			cmd_PvPMatchingWait.deckNum = this.battlePartyDeckNo;
		}

		public override void ChangeOperation(PartyEditPartyMember partyMember, int selectDeckNo, int favoriteDeckNo)
		{
			APIRequestTask apirequestTask = null;
			this.battlePartyDeckNo = selectDeckNo;
			if (base.IsChangedDeck(selectDeckNo, favoriteDeckNo) || partyMember.IsDirty())
			{
				apirequestTask = partyMember.RequestSaveUserDeck(selectDeckNo, favoriteDeckNo);
			}
			if (apirequestTask != null)
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
				AppCoroutine.Start(apirequestTask.Run(new Action(this.OnCompleteChangeOperation), null, null), false);
			}
			else
			{
				this.OnCompleteChangeOperation();
			}
		}
	}
}
