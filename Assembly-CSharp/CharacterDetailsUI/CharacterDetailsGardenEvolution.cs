using System;
using UnityEngine;

namespace CharacterDetailsUI
{
	public class CharacterDetailsGardenEvolution : ICharacterDetailsUIAnimation
	{
		private int cutinSortingOrder;

		private Transform cutinParentObject;

		private PartsUpperCutinController cutinController;

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
			this.cutinController.PlayAnimator(PartsUpperCutinController.AnimeType.EvolutionComplete, null);
		}

		public void Initialize(int windowSortingOrder, Transform windowRoot)
		{
			this.cutinSortingOrder = windowSortingOrder + 1;
			this.cutinParentObject = windowRoot;
		}
	}
}
