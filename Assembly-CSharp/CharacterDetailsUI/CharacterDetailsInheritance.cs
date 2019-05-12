using System;
using UnityEngine;

namespace CharacterDetailsUI
{
	public sealed class CharacterDetailsInheritance : ICharacterDetailsUIAnimation
	{
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
			this.cutinController.PlayAnimator(PartsUpperCutinController.AnimeType.InheritanceComplete, new Action(this.OnFinishCutin));
		}

		public void Initialize(int windowSortingOrder, Transform windowRoot, Action endCutin)
		{
			this.cutinSortingOrder = windowSortingOrder + 1;
			this.onEndCutin = endCutin;
			this.cutinParentObject = windowRoot;
		}
	}
}
