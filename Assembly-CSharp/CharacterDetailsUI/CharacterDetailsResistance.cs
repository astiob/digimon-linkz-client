using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterDetailsUI
{
	public sealed class CharacterDetailsResistance : ICharacterDetailsUIAnimation
	{
		private CharacterStatusResistance statusResistance;

		private CharacterDetailsResistanceParameter uiParam;

		private Action onEndCutin;

		private Action onEndAnimation;

		private Transform cutinParentObject;

		private PartsUpperCutinController cutinController;

		private void OnFinishCutin()
		{
			if (this.onEndCutin != null)
			{
				this.onEndCutin();
			}
			this.statusResistance.StartTranceEffect(new Action(this.OnFinishIconAnimation));
		}

		private void OnFinishIconAnimation()
		{
			if (this.onEndAnimation != null)
			{
				this.onEndAnimation();
			}
		}

		public void OnOpenMenu()
		{
			this.statusResistance.TranceEffectActiveSet(false);
		}

		public void OnCloseMenu()
		{
			this.statusResistance.TranceEffectActiveSet(true);
		}

		public void OnOpenWindow()
		{
			GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceMaster = MonsterResistanceData.GetResistanceMaster(this.uiParam.uniqueResistanceId);
			List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> uniqueResistanceListByJson = MonsterResistanceData.GetUniqueResistanceListByJson(this.uiParam.oldResistanceIds);
			GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM oldResistance = MonsterResistanceData.AddResistanceFromMultipleTranceData(resistanceMaster, uniqueResistanceListByJson);
			this.statusResistance.CreateResistanceCoverEffect();
			int num = this.statusResistance.CreateResistanceIconEffect(oldResistance, this.uiParam.newResistanceIds);
			this.cutinController = PartsUpperCutinController.Create(this.cutinParentObject, num + 1);
		}

		public void OnCloseWindow()
		{
		}

		public void StartAnimation()
		{
			this.cutinController.gameObject.SetActive(true);
			this.cutinController.PlayAnimator(PartsUpperCutinController.AnimeType.ResistanceChange, new Action(this.OnFinishCutin));
		}

		public void Initialize(Transform windowRoot, CharacterStatusResistance resistance, CharacterDetailsResistanceParameter param, Action endCutinAction, Action endResistanceAnimationAction)
		{
			this.statusResistance = resistance;
			this.uiParam = param;
			this.onEndCutin = endCutinAction;
			this.onEndAnimation = endResistanceAnimationAction;
			this.cutinParentObject = windowRoot;
		}
	}
}
