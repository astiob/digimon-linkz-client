using CharacterMiniStatusUI;
using Master;
using Monster;
using Quest;
using System;
using System.Collections;
using UI.MonsterInfoParts;
using UnityEngine;

namespace Colosseum.DeckUI
{
	public sealed class CMD_ColosseumDeck : CMDWrapper
	{
		[SerializeField]
		private MonsterBasicInfo monsterBasicInfo;

		[SerializeField]
		private MonsterSelectedIcon monsterSelectedIcon;

		[SerializeField]
		private ChipBaseSelect chipSlotInfo;

		[SerializeField]
		private UI_MonsterMiniStatus miniStatus;

		[SerializeField]
		private SortieLimitList sortieLimitList;

		[SerializeField]
		private GameObject deckTitle;

		[SerializeField]
		private UI_ColosseumDeckButton deckButton;

		[SerializeField]
		private UI_ColosseumDeckList deckList;

		private ColosseumDeckData deckData;

		private static void CreateDialog(CMD_ColosseumDeck.Mode targetMode)
		{
			CMD_ColosseumDeck cmd_ColosseumDeck = CMDWrapper.LoadPrefab<CMD_ColosseumDeck>("CMD_PartyEdit_PVP");
			cmd_ColosseumDeck.SetDialogData(targetMode);
			cmd_ColosseumDeck.Show();
		}

		private void SetDialogData(CMD_ColosseumDeck.Mode targetMode)
		{
			this.sortieLimitList.Initialize();
			if (ClassSingleton<QuestData>.Instance.SelectDungeon != null)
			{
				string worldDungeonId = ClassSingleton<QuestData>.Instance.SelectDungeon.worldDungeonId;
				if (!string.IsNullOrEmpty(worldDungeonId))
				{
					this.sortieLimitList.SetSortieLimit(int.Parse(worldDungeonId));
				}
			}
			this.deckData = new ColosseumDeckData();
			this.deckData.RootDialog = this;
			this.deckData.Mode = targetMode;
			this.deckData.MonsterBasicInfo = this.monsterBasicInfo;
			this.deckData.MonsterChipSlotInfo = this.chipSlotInfo;
			this.deckData.MonsterSelectedIcon = this.monsterSelectedIcon;
			this.deckData.MiniStatus = this.miniStatus;
			this.deckData.SortieLimitList = this.sortieLimitList;
			this.deckData.DeckButton = this.deckButton;
			this.deckData.DeckList = this.deckList;
		}

		private void SaveColosseumDeck(bool animation)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			string[] deckMonsterUserMonsterIdList = this.deckList.GetDeckMonsterUserMonsterIdList();
			APIRequestTask task = ColosseumDeckWeb.RequestSave(deckMonsterUserMonsterIdList);
			AppCoroutine.Start(task.Run(delegate
			{
				RestrictionInput.EndLoad();
				this.ClosePanel(animation);
			}, null, null), false);
		}

		private IEnumerator WaitCloseParentDialog(bool animation)
		{
			if (GUIManager.IsCloseAllMode())
			{
				while (null != GameObject.Find("CMD_PartyEdit") || null != CMD_PartyEdit.instance)
				{
					yield return new WaitForSeconds(0.01f);
				}
			}
			this.SaveColosseumDeck(animation);
			yield break;
		}

		protected override void OnShowDialog()
		{
			this.deckData.MonsterSelectedIcon.Initialize();
			this.deckButton.Initialize(this.deckData);
			this.deckList.Initialize(this.deckData);
			base.PartsTitle.SetTitle(StringMaster.GetString("ColosseumDeckTitle"));
			if (this.deckData.Mode == CMD_ColosseumDeck.Mode.FROM_PARTY_EDIT)
			{
				this.deckTitle.SetActive(true);
			}
			this.deckList.UpdateSelectedMonster();
			this.deckButton.UpdateButton();
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
		}

		public override void ClosePanel(bool animation = true)
		{
			if (!this.deckList.IsComplete())
			{
				ColosseumConfirmDialog.OpenDialog(this);
			}
			else if (this.deckList.IsDirty())
			{
				AppCoroutine.Start(this.WaitCloseParentDialog(animation), false);
			}
			else
			{
				base.ClosePanel(animation);
			}
		}

		public void CloseColosseumDeckDialog()
		{
			base.ClosePanel(true);
		}

		public static void Create(CMD_ColosseumDeck.Mode targetMode)
		{
			if (CMD_ChatTop.instance != null)
			{
				CMD_ChatTop.instance.isRecruitListLock = false;
			}
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			if (!ClassSingleton<MonsterUserDataMng>.Instance.ExistColosseumDeckData())
			{
				APIRequestTask task = ColosseumDeckWeb.RequestDeck();
				AppCoroutine.Start(task.Run(delegate
				{
					CMD_ColosseumDeck.CreateDialog(targetMode);
					RestrictionInput.EndLoad();
				}, delegate(Exception noop)
				{
					RestrictionInput.EndLoad();
				}, null), false);
			}
			else
			{
				CMD_ColosseumDeck.CreateDialog(targetMode);
				RestrictionInput.EndLoad();
			}
		}

		public enum Mode
		{
			FROM_PARTY_EDIT,
			FROM_COLOSSEUM_TOP
		}
	}
}
