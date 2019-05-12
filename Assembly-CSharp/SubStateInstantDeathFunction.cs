using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubStateInstantDeathFunction : BattleStateController
{
	private SubStateEnemiesItemDroppingFunction subStateEnemiesItemDroppingFunction;

	private bool isBigBoss;

	private string cameraKey = string.Empty;

	public SubStateInstantDeathFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		this.subStateEnemiesItemDroppingFunction = new SubStateEnemiesItemDroppingFunction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateEnemiesItemDroppingFunction);
	}

	protected override void EnabledThisState()
	{
	}

	protected override IEnumerator MainRoutine()
	{
		IEnumerator[] functions = new IEnumerator[]
		{
			this.InstantDeathFunction()
		};
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			this.isBigBoss = true;
		}
		foreach (IEnumerator function in functions)
		{
			while (function.MoveNext())
			{
				yield return null;
			}
		}
		yield break;
	}

	private IEnumerator InstantDeathFunction()
	{
		List<CharacterStateControl> characters = BattleFunctionUtility.GetSufferCharacters(SufferStateProperty.SufferType.InstantDeath, base.battleStateData);
		bool playedDeadSE = false;
		Dictionary<CharacterStateControl, bool> hit = new Dictionary<CharacterStateControl, bool>(characters.Count);
		if (characters.Count == 0)
		{
			yield break;
		}
		List<HitIcon> hitIconlist = new List<HitIcon>();
		Vector3[] hitIconPositions = BattleFunctionUtility.GetHitIconPositions(base.stateManager.uiControl, characters);
		for (int i = 0; i < characters.Count; i++)
		{
			SufferStateProperty sufferStateProperty = characters[i].currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.InstantDeath);
			hit.Add(characters[i], sufferStateProperty.isMiss);
			HitIcon item = base.stateManager.uiControl.ApplyShowHitIcon(i, hitIconPositions[i], AffectEffect.InstantDeath, 0, Strength.None, sufferStateProperty.isMiss, false, false, false, false, ExtraEffectType.Non, true, null);
			hitIconlist.Add(item);
		}
		this.ShowDigimon(characters);
		this.PlayCamera(characters);
		base.stateManager.cameraControl.PlayCameraShake();
		base.stateManager.uiControl.ShowCharacterHUDFunction(characters.ToArray());
		Action hudReposition = delegate()
		{
			foreach (CharacterStateControl characterStateControl2 in characters)
			{
				this.stateManager.uiControl.RepositionCharacterHUDPosition(new CharacterStateControl[]
				{
					characterStateControl2
				});
			}
			for (int j = 0; j < characters.Count; j++)
			{
				Vector3 fixableCharacterCenterPosition2DFunction = this.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(characters[j]);
				hitIconlist[j].HitIconReposition(fixableCharacterCenterPosition2DFunction);
			}
		};
		float waitSecond = base.stateManager.stateProperty.skillAfterWaitSecond;
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(waitSecond, hudReposition, null);
		while (wait.MoveNext())
		{
			yield return null;
		}
		base.stateManager.cameraControl.StopCameraShake();
		foreach (CharacterStateControl characterStateControl in characters)
		{
			if (!hit[characterStateControl])
			{
				Action deathEffectPlay = delegate()
				{
					if (!playedDeadSE)
					{
						this.stateManager.soundPlayer.PlayDeathSE();
					}
					playedDeadSE = true;
				};
				base.stateManager.threeDAction.PlayDeadAnimationCharacterAction(deathEffectPlay, characterStateControl);
				characterStateControl.Kill();
				characterStateControl.isDiedJustBefore = true;
			}
			characterStateControl.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.InstantDeath, false);
		}
		IEnumerator end_wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.skillAfterWaitSecond, null, null);
		while (end_wait.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private CharacterStateControl[] GetTotalCharacters()
	{
		CharacterStateControl[] result;
		if (base.stateManager.battleMode == BattleMode.PvP)
		{
			if (base.stateManager.pvpFunction.IsOwner)
			{
				result = base.battleStateData.GetTotalCharacters();
			}
			else
			{
				result = base.battleStateData.GetTotalCharactersEnemyFirst();
			}
		}
		else
		{
			result = base.battleStateData.GetTotalCharacters();
		}
		return result;
	}

	protected override void DisabledThisState()
	{
		base.stateManager.uiControl.ApplyHideHitIcon();
		base.stateManager.uiControl.HideCharacterHUDFunction();
	}

	protected override void GetEventThisState(EventState eventState)
	{
	}

	private void PlayCamera(List<CharacterStateControl> targets)
	{
		if (targets == null || targets.Count == 0)
		{
			return;
		}
		if (targets.Count > 1)
		{
			if (targets[0].isEnemy && this.isBigBoss)
			{
				this.cameraKey = "BigBoss/skillA";
			}
			else
			{
				this.cameraKey = "skillA";
			}
		}
		else if (targets[0].isEnemy && this.isBigBoss)
		{
			this.cameraKey = "BigBoss/skillF";
		}
		else
		{
			this.cameraKey = "skillF";
		}
		base.stateManager.cameraControl.PlayCameraMotionActionCharacter(this.cameraKey, targets[0]);
	}

	private IEnumerator ShowDigimonAndCamera(List<CharacterStateControl> targets)
	{
		this.PlayCamera(targets);
		this.ShowDigimon(targets);
		yield return null;
		this.PlayCamera(targets);
		yield return null;
		yield break;
	}

	private void ShowDigimon(List<CharacterStateControl> targets)
	{
		CharacterStateControl[] characters;
		CharacterStateControl[] characters2;
		if (!targets[0].isEnemy)
		{
			characters = base.battleStateData.playerCharacters.Where((CharacterStateControl x) => !x.isDied).ToArray<CharacterStateControl>();
			characters2 = base.battleStateData.enemies.Where((CharacterStateControl x) => !x.isDied).ToArray<CharacterStateControl>();
		}
		else
		{
			characters = base.battleStateData.enemies.Where((CharacterStateControl x) => !x.isDied).ToArray<CharacterStateControl>();
			characters2 = base.battleStateData.playerCharacters.Where((CharacterStateControl x) => !x.isDied).ToArray<CharacterStateControl>();
		}
		base.stateManager.threeDAction.ShowAllCharactersAction(characters);
		base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(characters);
		base.stateManager.threeDAction.HideAllCharactersAction(characters2);
	}
}
