using System;

namespace PartyEdit
{
	public sealed class PartyEditActionBattle : PartyEditAction
	{
		protected override void OnCompleteChangeOperation()
		{
			DataMng.Instance().RespData_WorldMultiStartInfo = null;
			ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.StopGetHistoryIdList();
			this.uiRoot.HideClips();
			TipsLoading.Instance.StartTipsLoad(CMD_Tips.DISPLAY_PLACE.QuestToSoloBattle, false);
			this.uiRoot.Close(true);
		}

		public override void ChangeOperation(PartyEditPartyMember partyMember, int selectDeckNo, int favoriteDeckNo)
		{
			APIRequestTask apirequestTask = null;
			if (null != this.uiRoot.parentCMD)
			{
				CMD_QuestTOP cmd_QuestTOP = this.uiRoot.parentCMD as CMD_QuestTOP;
				if (null != cmd_QuestTOP)
				{
					cmd_QuestTOP.battlePartyDeckNo = selectDeckNo;
				}
			}
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
			if (base.IsChangedDeck(selectDeckNo, favoriteDeckNo) || partyMember.IsDirty())
			{
				apirequestTask = partyMember.RequestSaveUserDeck(selectDeckNo, favoriteDeckNo);
			}
			if (apirequestTask != null)
			{
				AppCoroutine.Start(apirequestTask.Run(new Action(this.OnCompleteChangeOperation), null, null), false);
			}
			else
			{
				this.OnCompleteChangeOperation();
			}
		}
	}
}
