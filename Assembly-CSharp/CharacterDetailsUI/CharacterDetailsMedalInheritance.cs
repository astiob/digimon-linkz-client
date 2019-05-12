using Master;
using System;
using UnityEngine;

namespace CharacterDetailsUI
{
	public sealed class CharacterDetailsMedalInheritance : ICharacterDetailsUIAnimation
	{
		private bool isMedalInheritance;

		private bool isResetEquipChip;

		private int cutinSortingOrder;

		private Transform cutinParentObject;

		private PartsUpperCutinController cutinController;

		private Action onEndCutin;

		private void OnFinishCutin()
		{
			if (this.onEndCutin != null)
			{
				this.onEndCutin();
			}
			if (this.isResetEquipChip)
			{
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("MedalInheritTitle");
				cmd_ModalMessage.Info = StringMaster.GetString("MedalInheritCautionChip");
			}
		}

		public void OnOpenWindow()
		{
			this.cutinController = PartsUpperCutinController.Create(this.cutinParentObject, this.cutinSortingOrder);
		}

		public void OnCloseWindow()
		{
		}

		public void OnOpenMenu()
		{
		}

		public void OnCloseMenu()
		{
		}

		public void StartAnimation()
		{
			if (this.isMedalInheritance)
			{
				this.cutinController.PlayAnimator(PartsUpperCutinController.AnimeType.TalentBlooms, new Action(this.OnFinishCutin));
			}
			else
			{
				this.cutinController.PlayAnimator(PartsUpperCutinController.AnimeType.BloomsFailed, new Action(this.OnFinishCutin));
			}
		}

		public void Initialize(int windowSortingOrder, Transform windowRoot, bool medalInheritance, bool resetEquipChip, Action endCutin)
		{
			this.cutinSortingOrder = windowSortingOrder + 1;
			this.onEndCutin = endCutin;
			this.cutinParentObject = windowRoot;
			this.isMedalInheritance = medalInheritance;
			this.isResetEquipChip = resetEquipChip;
		}
	}
}
