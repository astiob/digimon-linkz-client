using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStateOnHitPoisonDamageFunction : BattleStateController
{
	private HitEffectParams[] hitEffectParams;

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
		foreach (CharacterStateControl character in base.stateManager.battleStateData.GetTotalCharacters())
		{
			base.stateManager.threeDAction.ShowAliveCharactersAction(new CharacterStateControl[]
			{
				character
			});
			base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(new CharacterStateControl[]
			{
				character
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
		string key = SufferStateProperty.SufferType.Regenerate.ToString();
		this.hitEffectParams = base.battleStateData.hitEffects.GetObject(key);
		List<int> damage = new List<int>();
		List<bool> isMiss = new List<bool>();
		for (int i = 0; i < characters.Count; i++)
		{
			SufferStateProperty regenerateSuffer = characters[i].currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Regenerate);
			int value = regenerateSuffer.GetRegenerate(characters[i]);
			characters[i].hp += value;
			damage.Add(value);
			isMiss.Add(false);
			base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.revival, new CharacterStateControl[]
			{
				characters[i]
			});
		}
		base.stateManager.soundPlayer.TryPlaySE(this.hitEffectParams[0]);
		List<HitIcon> hitIconlist = new List<HitIcon>();
		Vector3[] hitIconPositions = this.GetHitIconPositions(characters);
		for (int j = 0; j < characters.Count; j++)
		{
			HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(j, hitIconPositions[j], AffectEffect.Regenerate, damage[j], Strength.None, isMiss[j], false, false, false, false, ExtraEffectType.Non);
			hitIconlist.Add(hitIcon);
		}
		base.stateManager.uiControl.ShowCharacterHUDFunction(characters.ToArray());
		Action hudReposition = delegate()
		{
			foreach (CharacterStateControl characterStateControl in characters)
			{
				base.stateManager.uiControl.RepositionCharacterHUDPosition(new CharacterStateControl[]
				{
					characterStateControl
				});
			}
			for (int l = 0; l < characters.Count; l++)
			{
				Vector3 fixableCharacterCenterPosition2DFunction = base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(characters[l]);
				hitIconlist[l].HitIconReposition(fixableCharacterCenterPosition2DFunction);
			}
		};
		float waitSecond = base.stateManager.stateProperty.poisonHitEffectWaitSecond;
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(waitSecond, hudReposition, null);
		while (wait.MoveNext())
		{
			yield return null;
		}
		base.stateManager.soundPlayer.TryStopSE(this.hitEffectParams[0]);
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
		foreach (CharacterStateControl character in base.stateManager.battleStateData.GetTotalCharacters())
		{
			base.stateManager.threeDAction.ShowAliveCharactersAction(new CharacterStateControl[]
			{
				character
			});
			base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(new CharacterStateControl[]
			{
				character
			});
		}
		base.stateManager.SetBattleScreen(BattleScreen.PoisonHit);
		foreach (CharacterStateControl character2 in this.GetTotalCharacters())
		{
			character2.OnChipTrigger(EffectStatusBase.EffectTriggerType.DamagePossibility);
		}
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("BigBoss/0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		else
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		string key = SufferStateProperty.SufferType.Poison.ToString();
		this.hitEffectParams = base.battleStateData.hitEffects.GetObject(key);
		List<CharacterStateControl> currentDeathCharacters = new List<CharacterStateControl>();
		List<int> damage = new List<int>();
		List<bool> isMiss = new List<bool>();
		CharacterStateControl currentDeathBigBoss = null;
		for (int i = 0; i < characters.Count; i++)
		{
			SufferStateProperty poisonSuffer = characters[i].currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Poison);
			int poisonDamage = poisonSuffer.GetPoisonDamageFluctuation(characters[i]);
			characters[i].hp -= poisonDamage;
			damage.Add(poisonDamage);
			isMiss.Add(false);
			if (!characters[i].isDied)
			{
				base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.hit, new CharacterStateControl[]
				{
					characters[i]
				});
			}
			else
			{
				if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1 && characters[i].isEnemy)
				{
					base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.strongHit, new CharacterStateControl[]
					{
						characters[i]
					});
					currentDeathBigBoss = characters[i];
				}
				else
				{
					Action callback = delegate()
					{
						bool playedDeadSE;
						if (!playedDeadSE)
						{
							base.stateManager.soundPlayer.PlayDeathSE();
						}
						playedDeadSE = true;
					};
					base.stateManager.threeDAction.PlayDeadAnimationCharacterAction(callback, characters[i]);
				}
				currentDeathCharacters.Add(characters[i]);
			}
			base.stateManager.threeDAction.PlayHitEffectAction(this.hitEffectParams[i], characters[i]);
		}
		foreach (CharacterStateControl character3 in this.GetTotalCharacters())
		{
			character3.ClearGutsData();
		}
		List<HitIcon> hitIconlist = new List<HitIcon>();
		Vector3[] hitIconPositions = this.GetHitIconPositions(characters);
		for (int j = 0; j < characters.Count; j++)
		{
			HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(j, hitIconPositions[j], AffectEffect.Poison, damage[j], Strength.None, isMiss[j], false, false, false, false, ExtraEffectType.Non);
			hitIconlist.Add(hitIcon);
		}
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.uiControl.ShowCharacterHUDFunction(characters.ToArray());
		base.stateManager.soundPlayer.TryPlaySE(this.hitEffectParams[0]);
		Action hudReposition = delegate()
		{
			foreach (CharacterStateControl characterStateControl in characters)
			{
				base.stateManager.uiControl.RepositionCharacterHUDPosition(new CharacterStateControl[]
				{
					characterStateControl
				});
			}
			for (int n = 0; n < characters.Count; n++)
			{
				Vector3 fixableCharacterCenterPosition2DFunction = base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(characters[n]);
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
		base.stateManager.soundPlayer.TryStopSE(this.hitEffectParams[0]);
		base.stateManager.soundPlayer.StopHitEffectSE();
		foreach (CharacterStateControl c in currentDeathCharacters)
		{
			c.isDiedJustBefore = true;
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
		if (this.hitEffectParams != null)
		{
			base.stateManager.threeDAction.StopHitEffectAction(this.hitEffectParams);
		}
		base.stateManager.uiControl.ApplyHideHitIcon();
		base.stateManager.uiControl.HideCharacterHUDFunction();
	}

	protected override void GetEventThisState(EventState eventState)
	{
		if (this.hitEffectParams != null)
		{
			foreach (HitEffectParams hitEffect in this.hitEffectParams)
			{
				base.stateManager.soundPlayer.TryStopSE(hitEffect);
			}
		}
	}
}
