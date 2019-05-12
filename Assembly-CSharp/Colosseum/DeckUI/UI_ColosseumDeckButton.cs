using Monster;
using MonsterList.ChangeMonster;
using System;
using UnityEngine;

namespace Colosseum.DeckUI
{
	public sealed class UI_ColosseumDeckButton : MonoBehaviour
	{
		[SerializeField]
		private GameObject questButton;

		[SerializeField]
		private GameObject battleStartButton;

		private ColosseumDeckData deckData;

		private void InitializeButton(CMD_ColosseumDeck.Mode mode)
		{
			if (mode != CMD_ColosseumDeck.Mode.FROM_PARTY_EDIT)
			{
				if (mode == CMD_ColosseumDeck.Mode.FROM_COLOSSEUM_TOP)
				{
					this.battleStartButton.SetActive(true);
				}
			}
			else
			{
				this.questButton.SetActive(true);
			}
		}

		private void OnPushedQuestButton()
		{
			this.deckData.RootDialog.ClosePanel(true);
		}

		private void OnPushedChangeButton()
		{
			MonsterUserData selectMonster = this.deckData.DeckList.GetSelectMonster();
			CMD_ChangeMonster.SelectMonsterData = (selectMonster as MonsterData);
			CMD_ChangeMonster cmd_ChangeMonster = GUIMain.ShowCommonDialog(null, "CMD_ChangeMonster") as CMD_ChangeMonster;
			cmd_ChangeMonster.SetChangedAction(new Action<MonsterUserData>(this.deckData.DeckList.UpdateList));
			cmd_ChangeMonster.SetIconColosseumDeck(this.deckData.DeckList.GetSelectMonster(), this.deckData.DeckList.GetDeckMonsterList(), this.deckData.SortieLimitList.GetSortieLimitList());
		}

		private void OnPushedBattleStartButton()
		{
			if (!this.deckData.DeckList.IsComplete())
			{
				global::Debug.Log("確認ダイアログ");
			}
			else if (this.deckData.DeckList.IsDirty())
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
				string[] deckMonsterUserMonsterIdList = this.deckData.DeckList.GetDeckMonsterUserMonsterIdList();
				APIRequestTask task = ColosseumDeckWeb.RequestSave(deckMonsterUserMonsterIdList);
				AppCoroutine.Start(task.Run(delegate
				{
					RestrictionInput.EndLoad();
					this.deckData.DeckList.UpdateDeckMonster();
					this.OpenPvPMatchingDialog();
				}, null, null), false);
			}
			else
			{
				this.OpenPvPMatchingDialog();
			}
		}

		private void OpenPvPMatchingDialog()
		{
			GUIMain.ShowCommonDialog(null, "CMD_PvPMatchingWait");
		}

		public void Initialize(ColosseumDeckData data)
		{
			this.deckData = data;
			this.InitializeButton(data.Mode);
		}

		public void UpdateButton()
		{
			UISprite component = this.battleStartButton.GetComponent<UISprite>();
			GUICollider component2 = this.battleStartButton.GetComponent<GUICollider>();
			bool flag = this.deckData.DeckList.IsComplete();
			component2.activeCollider = flag;
			if (flag)
			{
				component.spriteName = "Common02_Btn_Red";
			}
			else
			{
				component.spriteName = "Common02_Btn_Gray";
			}
		}
	}
}
