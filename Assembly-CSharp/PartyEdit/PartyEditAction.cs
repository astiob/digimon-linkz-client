using System;

namespace PartyEdit
{
	public class PartyEditAction
	{
		private bool enableCloseAnimation;

		protected CMD_PartyEdit uiRoot;

		protected bool IsChangedDeck(int selectDeckNo, int favoriteDeckNo)
		{
			bool result = false;
			if (selectDeckNo != DataMng.Instance().RespDataMN_DeckList.selectDeckNum.ToInt32() || favoriteDeckNo != DataMng.Instance().RespDataMN_DeckList.favoriteDeckNum.ToInt32())
			{
				result = true;
			}
			return result;
		}

		private void OnCompleteCloseOperation()
		{
			if (Loading.IsShow())
			{
				RestrictionInput.EndLoad();
			}
			FarmCameraControlForCMD.On();
			FarmRoot instance = FarmRoot.Instance;
			if (null != instance && null != instance.DigimonManager)
			{
				instance.DigimonManager.RefreshDigimonGameObject(false, null);
			}
			this.uiRoot.Close(this.enableCloseAnimation);
		}

		protected virtual void OnCompleteChangeOperation()
		{
		}

		public void SetUiRoot(CMD_PartyEdit ui)
		{
			this.uiRoot = ui;
		}

		public void CloseOperation(PartyEditPartyMember partyMember, int selectDeckNo, int favoriteDeckNo, bool animation)
		{
			this.enableCloseAnimation = animation;
			APIRequestTask apirequestTask = null;
			if (this.IsChangedDeck(selectDeckNo, favoriteDeckNo) || partyMember.IsDirty())
			{
				apirequestTask = partyMember.RequestSaveUserDeck(selectDeckNo, favoriteDeckNo);
			}
			if (apirequestTask != null)
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
				apirequestTask.Add(Singleton<UserDataMng>.Instance.RequestUserMonsterFriendshipTime(true));
				AppCoroutine.Start(apirequestTask.Run(new Action(this.OnCompleteCloseOperation), null, null), false);
			}
			else
			{
				this.OnCompleteCloseOperation();
			}
		}

		public virtual void ChangeOperation(PartyEditPartyMember partyMember, int selectDeckNo, int favoriteDeckNo)
		{
		}
	}
}
