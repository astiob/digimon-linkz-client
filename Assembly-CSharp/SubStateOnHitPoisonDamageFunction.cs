using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStateOnHitPoisonDamageFunction : BattleStateController
{
	private List<HitEffectParams> use_effect_params_ = new List<HitEffectParams>();

	private SubStateEnemiesItemDroppingFunction subStateEnemiesItemDroppingFunction;

	public SubStateOnHitPoisonDamageFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
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
			this.RegenerateFunction(),
			this.PoisonFunction()
		};
		foreach (IEnumerator function in functions)
		{
			while (function.MoveNext())
			{
				yield return null;
			}
		}
		yield break;
	}

	private List<CharacterStateControl> GetSufferCharacters(SufferStateProperty.SufferType sufferType)
	{
		List<CharacterStateControl> list = new List<CharacterStateControl>();
		foreach (CharacterStateControl characterStateControl in base.battleStateData.GetTotalCharacters())
		{
			if (!characterStateControl.isDied)
			{
				if (characterStateControl.currentSufferState.FindSufferState(sufferType))
				{
					list.Add(characterStateControl);
				}
			}
		}
		return list;
	}

	private IEnumerator RegenerateFunction()
	{
		List<CharacterStateControl> characters = this.GetSufferCharacters(SufferStateProperty.SufferType.Regenerate);
		if (characters.Count == 0)
		{
			yield break;
		}
		foreach (CharacterStateControl characterStateControl in base.stateManager.battleStateData.GetTotalCharacters())
		{
			base.stateManager.threeDAction.ShowAliveCharactersAction(new CharacterStateControl[]
			{
				characterStateControl
			});
			base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(new CharacterStateControl[]
			{
				characterStateControl
			});
		}
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.SetBattleScreen(BattleScreen.PoisonHit);
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("BigBoss/0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		else
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		HitEffectParams regenerate_effect = BattleEffectManager.Instance.GetEffect(AffectEffect.HpRevival.ToString()) as HitEffectParams;
		this.use_effect_params_.Add(regenerate_effect);
		List<int> damage = new List<int>();
		List<bool> isMiss = new List<bool>();
		for (int j = 0; j < characters.Count; j++)
		{
			SufferStateProperty sufferStateProperty = characters[j].currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Regenerate);
			int regenerate = sufferStateProperty.GetRegenerate(characters[j]);
			characters[j].hp += regenerate;
			damage.Add(regenerate);
			isMiss.Add(false);
			base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.revival, new CharacterStateControl[]
			{
				characters[j]
			});
		}
		base.stateManager.soundPlayer.TryPlaySE(regenerate_effect);
		List<HitIcon> hitIconlist = new List<HitIcon>();
		Vector3[] hitIconPositions = this.GetHitIconPositions(characters);
		for (int k = 0; k < characters.Count; k++)
		{
			HitIcon item = base.stateManager.uiControl.ApplyShowHitIcon(k, hitIconPositions[k], AffectEffect.Regenerate, damage[k], Strength.None, isMiss[k], false, false, false, false, ExtraEffectType.Non, true, null);
			hitIconlist.Add(item);
		}
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
			for (int l = 0; l < characters.Count; l++)
			{
				Vector3 fixableCharacterCenterPosition2DFunction = this.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(characters[l]);
				hitIconlist[l].HitIconReposition(fixableCharacterCenterPosition2DFunction);
			}
		};
		float waitSecond = base.stateManager.stateProperty.poisonHitEffectWaitSecond;
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(waitSecond, hudReposition, null);
		while (wait.MoveNext())
		{
			yield return null;
		}
		base.stateManager.soundPlayer.TryStopSE(regenerate_effect);
		base.stateManager.soundPlayer.StopHitEffectSE();
		yield break;
	}

	private IEnumerator PoisonFunction()
	{
		List<CharacterStateControl> characters = this.GetSufferCharacters(SufferStateProperty.SufferType.Poison);
		if (characters.Count == 0)
		{
			yield break;
		}
		foreach (CharacterStateControl characterStateControl in base.stateManager.battleStateData.GetTotalCharacters())
		{
			base.stateManager.threeDAction.ShowAliveCharactersAction(new CharacterStateControl[]
			{
				characterStateControl
			});
			base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(new CharacterStateControl[]
			{
				characterStateControl
			});
		}
		base.stateManager.SetBattleScreen(BattleScreen.PoisonHit);
		foreach (CharacterStateControl characterStateControl2 in this.GetTotalCharacters())
		{
			characterStateControl2.OnChipTrigger(EffectStatusBase.EffectTriggerType.DamagePossibility);
		}
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("BigBoss/0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		else
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		List<CharacterStateControl> currentDeathCharacters = new List<CharacterStateControl>();
		List<int> damage = new List<int>();
		List<bool> isMiss = new List<bool>();
		CharacterStateControl currentDeathBigBoss = null;
		bool playedDeadSE = false;
		for (int k = 0; k < characters.Count; k++)
		{
			SufferStateProperty sufferStateProperty = characters[k].currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Poison);
			int poisonDamageFluctuation = sufferStateProperty.GetPoisonDamageFluctuation(characters[k]);
			characters[k].hp -= poisonDamageFluctuation;
			damage.Add(poisonDamageFluctuation);
			isMiss.Add(false);
			if (!characters[k].isDied)
			{
				base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.hit, new CharacterStateControl[]
				{
					characters[k]
				});
			}
			else
			{
				if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1 && characters[k].isEnemy)
				{
					base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.strongHit, new CharacterStateControl[]
					{
						characters[k]
					});
					currentDeathBigBoss = characters[k];
				}
				else
				{
					Action deathEffectPlay = delegate()
					{
						if (!playedDeadSE)
						{
							this.stateManager.soundPlayer.PlayDeathSE();
						}
						playedDeadSE = true;
					};
					base.stateManager.threeDAction.PlayDeadAnimationCharacterAction(deathEffectPlay, characters[k]);
				}
				currentDeathCharacters.Add(characters[k]);
			}
			HitEffectParams item = BattleEffectManager.Instance.GetEffect(AffectEffect.Poison.ToString()) as HitEffectParams;
			this.use_effect_params_.Add(item);
			base.stateManager.threeDAction.PlayHitEffectAction(this.use_effect_params_[k], characters[k]);
		}
		foreach (CharacterStateControl characterStateControl3 in this.GetTotalCharacters())
		{
			characterStateControl3.ClearGutsData();
		}
		List<HitIcon> hitIconlist = new List<HitIcon>();
		Vector3[] hitIconPositions = this.GetHitIconPositions(characters);
		for (int m = 0; m < characters.Count; m++)
		{
			HitIcon item2 = base.stateManager.uiControl.ApplyShowHitIcon(m, hitIconPositions[m], AffectEffect.Poison, damage[m], Strength.None, isMiss[m], false, false, false, false, ExtraEffectType.Non, true, null);
			hitIconlist.Add(item2);
		}
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.uiControl.ShowCharacterHUDFunction(characters.ToArray());
		if (this.use_effect_params_.Count > 0)
		{
			base.stateManager.soundPlayer.TryPlaySE(this.use_effect_params_[0]);
		}
		Action hudReposition = delegate()
		{
			foreach (CharacterStateControl characterStateControl5 in characters)
			{
				this.stateManager.uiControl.RepositionCharacterHUDPosition(new CharacterStateControl[]
				{
					characterStateControl5
				});
			}
			for (int n = 0; n < characters.Count; n++)
			{
				Vector3 fixableCharacterCenterPosition2DFunction = this.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(characters[n]);
				hitIconlist[n].HitIconReposition(fixableCharacterCenterPosition2DFunction);
			}
		};
		if (currentDeathBigBoss != null)
		{
			base.stateManager.uiControl.Fade(Color.white, 1f, 1f);
		}
		float waitSecond = base.stateManager.stateProperty.poisonHitEffectWaitSecond + ((currentDeathCharacters.Count <= 0) ? 0f : 1f);
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(waitSecond, hudReposition, null);
		while (wait.MoveNext())
		{
			yield return null;
		}
		if (this.use_effect_params_.Count > 0)
		{
			base.stateManager.soundPlayer.TryStopSE(this.use_effect_params_[0]);
		}
		base.stateManager.soundPlayer.StopHitEffectSE();
		foreach (CharacterStateControl characterStateControl4 in currentDeathCharacters)
		{
			characterStateControl4.isDiedJustBefore = true;
		}
		if (base.battleMode != BattleMode.PvP)
		{
			if (currentDeathBigBoss != null)
			{
				base.stateManager.cameraControl.StopCameraMotionAction("0002_command");
				IEnumerator transitionCount = base.stateManager.threeDAction.BigBossExitAction(currentDeathBigBoss);
				while (transitionCount.MoveNext())
				{
					yield return null;
				}
			}
			this.subStateEnemiesItemDroppingFunction.Init(currentDeathCharacters.ToArray());
			base.SetState(this.subStateEnemiesItemDroppingFunction.GetType());
			while (base.isWaitState)
			{
				yield return null;
			}
			if (currentDeathBigBoss != null)
			{
				base.stateManager.cameraControl.StopCameraMotionAction("skillA");
			}
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

	private Vector3[] GetHitIconPositions(List<CharacterStateControl> characters)
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < characters.Count; i++)
		{
			Vector3 characterCenterPosition2DFunction = base.stateManager.uiControl.GetCharacterCenterPosition2DFunction(characters[i]);
			list.Add(characterCenterPosition2DFunction);
		}
		return list.ToArray();
	}

	protected override void DisabledThisState()
	{
		base.stateManager.cameraControl.StopCameraMotionAction("0002_command");
		base.stateManager.cameraControl.StopCameraMotionAction("skillA");
		base.stateManager.cameraControl.StopCameraMotionAction("BigBoss/0002_command");
		if (this.use_effect_params_.Count > 0)
		{
			base.stateManager.threeDAction.StopHitEffectAction(this.use_effect_params_.ToArray());
		}
		base.stateManager.uiControl.ApplyHideHitIcon();
		base.stateManager.uiControl.HideCharacterHUDFunction();
		this.use_effect_params_.Clear();
	}

	protected override void GetEventThisState(EventState eventState)
	{
		if (this.use_effect_params_.Count > 0)
		{
			foreach (HitEffectParams hitEffect in this.use_effect_params_)
			{
				base.stateManager.soundPlayer.TryStopSE(hitEffect);
			}
		}
	}
}
