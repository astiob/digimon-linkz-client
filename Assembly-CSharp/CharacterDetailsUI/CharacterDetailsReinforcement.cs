using System;
using UnityEngine;

namespace CharacterDetailsUI
{
	public sealed class CharacterDetailsReinforcement : ICharacterDetailsUIAnimation
	{
		private CharacterStatusReinforcement statusReinforcement;

		private CharacterDetailsReinforcementParam uiParam;

		private StatusUpAnimation statusUpAnim;

		private int parentSortingOrder;

		private Action onEndCutin;

		private Action onEndAnimation;

		private Transform cutinParentObject;

		private PartsUpperCutinController cutinController;

		private void OnFinishLevelUpCutin()
		{
			if (0 < this.uiParam.upLuckValue)
			{
				this.cutinController.PlayAnimator(PartsUpperCutinController.AnimeType.LuckUp, new Action(this.OnFinishLuckUpCutin));
			}
			else
			{
				this.OnFinishLuckUpCutin();
			}
		}

		private void OnFinishLuckUpCutin()
		{
			int num = int.Parse(this.uiParam.beforeMonster.level);
			if (num < this.uiParam.afterLevel)
			{
				if (this.onEndCutin != null)
				{
					this.onEndCutin();
				}
				this.statusReinforcement.ShowLevelUpParticle(new Action(this.OnFinishAnimation));
			}
			else
			{
				this.OnFinishAnimation();
			}
		}

		private void OnFinishAnimation()
		{
			if (this.onEndAnimation != null)
			{
				this.onEndAnimation();
			}
		}

		public void OnOpenWindow()
		{
			int defaultLevel = int.Parse(this.uiParam.beforeMonster.level);
			this.statusUpAnim.Initialize(this.uiParam.beforeMonster, defaultLevel);
			this.cutinController = PartsUpperCutinController.Create(this.cutinParentObject, this.parentSortingOrder + 1);
			this.statusReinforcement.CreateLevelUpAnimation(this.parentSortingOrder + 1);
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
			this.statusUpAnim.DisplayDifference(this.uiParam.afterLevel, this.uiParam.upLuckValue);
			int num = int.Parse(this.uiParam.beforeMonster.level);
			if (num < this.uiParam.afterLevel)
			{
				this.cutinController.PlayAnimator(PartsUpperCutinController.AnimeType.LevelUp, new Action(this.OnFinishLevelUpCutin));
			}
			else
			{
				this.OnFinishLevelUpCutin();
			}
		}

		public void Initialize(int windowSortingOrder, Transform windowRoot, CharacterStatusReinforcement reinforcement, StatusUpAnimation statusUpAnimation, CharacterDetailsReinforcementParam param, Action onEndCutinAction, Action onEndAnimationAction)
		{
			this.uiParam = param;
			this.parentSortingOrder = windowSortingOrder;
			this.statusReinforcement = reinforcement;
			this.statusUpAnim = statusUpAnimation;
			this.onEndCutin = onEndCutinAction;
			this.onEndAnimation = onEndAnimationAction;
			this.cutinParentObject = windowRoot;
		}
	}
}
