using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStateMultiSkillDetailsFunction : SubStateSkillDetailsFunction
{
	public SubStateMultiSkillDetailsFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(OnExit, OnExitGotEvent)
	{
	}

	protected override void CheckFraud(int damage, CharacterStateControl currentCharacter, CharacterStateControl targetCharacter)
	{
	}

	protected override void ApplyMonsterIconEnabled(CharacterStateControl currentCharacter)
	{
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			bool isSelect = currentCharacter == base.battleStateData.playerCharacters[i];
			base.stateManager.uiControlMulti.ApplyMonsterButtonEnable(i, isSelect, base.battleStateData.playerCharacters[i].isDied);
		}
	}

	protected override void SetRandomSeed(AffectEffectProperty currentSuffer)
	{
		currentSuffer.SetRandomSeed(UnityEngine.Random.seed);
	}

	protected override IEnumerator AffectEffectApDrain(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer)
	{
		IEnumerator function = base.AffectEffectApDrain(currentCharacter, targetDataList, currentSuffer);
		while (function.MoveNext())
		{
			yield return null;
		}
		base.stateManager.battleUiComponentsMulti.sharedApMulti.PlayApUpAnimations();
		base.stateManager.uiControlMulti.RefreshSharedAP(false);
		while (base.stateManager.uiControlMulti.isPlayingSharedAp)
		{
			yield return null;
		}
		yield break;
	}
}
