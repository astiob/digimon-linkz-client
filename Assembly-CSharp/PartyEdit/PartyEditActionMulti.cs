using Master;
using Quest;
using System;
using System.Collections.Generic;

namespace PartyEdit
{
	public class PartyEditActionMulti : PartyEditAction
	{
		private int battlePartyDeckNo;

		private List<MonsterData> deckMonsterList;

		private void OnClosedMultiRecruitSettingModal(int selectButtonIndex)
		{
			if (selectButtonIndex == 0)
			{
				CMD_MultiRecruitPartyWait.UserType = CMD_MultiRecruitPartyWait.USER_TYPE.OWNER;
				GUIMain.ShowCommonDialog(null, "CMD_MultiRecruitPartyWait");
			}
		}

		private void OpenMultiRecruitSettingModal()
		{
			CMD_MultiRecruitSettingModal cmd_MultiRecruitSettingModal = GUIMain.ShowCommonDialog(new Action<int>(this.OnClosedMultiRecruitSettingModal), "CMD_MultiRecruitSettingModal") as CMD_MultiRecruitSettingModal;
			cmd_MultiRecruitSettingModal.deckNum = this.battlePartyDeckNo;
			cmd_MultiRecruitSettingModal.SetCallbackAction(delegate(GameWebAPI.RespData_MultiRoomCreate roomData)
			{
				CMD_MultiRecruitPartyWait.roomCreateData = roomData;
			});
		}

		protected override void OnCompleteChangeOperation()
		{
			if (Loading.IsShow())
			{
				RestrictionInput.EndLoad();
			}
			List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> worldSortieLimit = this.uiRoot.GetWorldSortieLimit();
			bool flag = true;
			if (worldSortieLimit != null && 0 < worldSortieLimit.Count)
			{
				for (int i = 0; i < 2; i++)
				{
					string tribe = this.deckMonsterList[i].monsterMG.tribe;
					string growStep = this.deckMonsterList[i].monsterMG.growStep;
					flag = ClassSingleton<QuestData>.Instance.CheckSortieLimit(this.uiRoot.GetWorldSortieLimit(), tribe, growStep);
					if (!flag)
					{
						break;
					}
				}
			}
			if (!flag)
			{
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int noop)
				{
					this.OpenMultiRecruitSettingModal();
				}, "CMD_ModalMessage") as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
				cmd_ModalMessage.Info = StringMaster.GetString("PartySortieLimitConfirmInfo");
			}
			else
			{
				this.OpenMultiRecruitSettingModal();
			}
		}

		public override void ChangeOperation(PartyEditPartyMember partyMember, int selectDeckNo, int favoriteDeckNo)
		{
			APIRequestTask apirequestTask = null;
			this.battlePartyDeckNo = selectDeckNo;
			this.deckMonsterList = partyMember.GetMonsterDataList(selectDeckNo - 1);
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
