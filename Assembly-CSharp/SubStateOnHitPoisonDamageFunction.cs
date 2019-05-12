using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStateOnHitPoisonDamageFunction : BattleStateController
{
	private HitEffectParams[] hitEffectParams;

	public SubStateOnHitPoisonDamageFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStateEnemiesItemDroppingFunction(null, new Action<EventState>(base.SendEventState)));
	}

	protected override void EnabledThisState()
	{
	}

	protected override IEnumerator MainRoutine()
	{
		IEnumerator[] functions = new IEnumerator[]
		{
			this.RegenerateFunction(),
			this.PoisonFunction(),
			this.UpFunction(),
			this.DownFunction()
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
			base.stateManager.threeDAction.ShowAliveCharactersAction(new CharacterStateControl[]
			{
				characterStateControl
			});
			base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(new CharacterStateControl[]
			{
				characterStateControl
			});
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
			int value = characters[i].currentSufferState.onRegenerate.GetRegenerate(characters[i]);
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
			HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(j, hitIconPositions[j], AffectEffect.Regenerate, damage[j], Strength.None, isMiss[j], false, false, false, ExtraEffectType.Non);
			hitIconlist.Add(hitIcon);
		}
		Action<Vector3[]> reposition = delegate(Vector3[] positions)
		{
			for (int k = 0; k < characters.Count; k++)
			{
				hitIconlist[k].HitIconReposition(positions[k]);
			}
		};
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
			Action updateHitIconCharacters = base.stateManager.threeDAction.GetUpdateHitIconCharacters(reposition, characters.ToArray(), hitIconlist.Count);
			base.stateManager.onPreRenderJustOnce.Add(updateHitIconCharacters);
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
		string key = SufferStateProperty.SufferType.Poison.ToString();
		this.hitEffectParams = base.battleStateData.hitEffects.GetObject(key);
		List<CharacterStateControl> currentDeathCharacters = new List<CharacterStateControl>();
		List<int> damage = new List<int>();
		List<bool> isMiss = new List<bool>();
		CharacterStateControl currentDeathBigBoss = null;
		for (int i = 0; i < characters.Count; i++)
		{
			int poisonDamage = characters[i].currentSufferState.onPoison.GetPoisonDamageFluctuation(characters[i]);
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
		base.stateManager.soundPlayer.TryPlaySE(this.hitEffectParams[0]);
		if (base.battleMode != BattleMode.PvP)
		{
			base.stateManager.deadOrAlive.AfterEnemyDeadFunction(currentDeathCharacters.ToArray());
		}
		List<HitIcon> hitIconlist = new List<HitIcon>();
		Vector3[] hitIconPositions = this.GetHitIconPositions(characters);
		for (int j = 0; j < characters.Count; j++)
		{
			HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(j, hitIconPositions[j], AffectEffect.Poison, damage[j], Strength.None, isMiss[j], false, false, false, ExtraEffectType.Non);
			hitIconlist.Add(hitIcon);
		}
		Action<Vector3[]> reposition = delegate(Vector3[] positions)
		{
			for (int k = 0; k < characters.Count; k++)
			{
				hitIconlist[k].HitIconReposition(positions[k]);
			}
		};
		base.stateManager.uiControl.ShowCharacterHUDFunction(characters.ToArray());
		foreach (CharacterStateControl c in currentDeathCharacters)
		{
			c.isDiedJustBefore = true;
		}
		Action hudReposition = delegate()
		{
			foreach (CharacterStateControl characterStateControl in characters)
			{
				base.stateManager.uiControl.RepositionCharacterHUDPosition(new CharacterStateControl[]
				{
					characterStateControl
				});
			}
			Action updateHitIconCharacters = base.stateManager.threeDAction.GetUpdateHitIconCharacters(reposition, characters.ToArray(), hitIconlist.Count);
			base.stateManager.onPreRenderJustOnce.Add(updateHitIconCharacters);
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
		base.battleStateData.currentDeadCharacters = currentDeathCharacters.ToArray();
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
			base.SetState(typeof(SubStateEnemiesItemDroppingFunction));
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

	private IEnumerator UpFunction()
	{
		SufferStateProperty.SufferType[] sufferTypes = new SufferStateProperty.SufferType[]
		{
			SufferStateProperty.SufferType.AttackUp,
			SufferStateProperty.SufferType.DefenceUp,
			SufferStateProperty.SufferType.SpAttackUp,
			SufferStateProperty.SufferType.SpDefenceUp,
			SufferStateProperty.SufferType.SpeedUp,
			SufferStateProperty.SufferType.HitRateUp,
			SufferStateProperty.SufferType.SatisfactionRateUp
		};
		IEnumerator function = this.HitEffectFunction(sufferTypes, CharacterAnimationType.revival);
		while (function.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator DownFunction()
	{
		SufferStateProperty.SufferType[] sufferTypes = new SufferStateProperty.SufferType[]
		{
			SufferStateProperty.SufferType.AttackDown,
			SufferStateProperty.SufferType.DefenceDown,
			SufferStateProperty.SufferType.SpAttackDown,
			SufferStateProperty.SufferType.SpDefenceDown,
			SufferStateProperty.SufferType.SpeedDown,
			SufferStateProperty.SufferType.HitRateDown,
			SufferStateProperty.SufferType.SatisfactionRateDown
		};
		IEnumerator function = this.HitEffectFunction(sufferTypes, CharacterAnimationType.hit);
		while (function.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator HitEffectFunction(SufferStateProperty.SufferType[] sufferTypes, CharacterAnimationType characterAnimationType)
	{
		List<CharacterStateControl> characters = new List<CharacterStateControl>();
		foreach (SufferStateProperty.SufferType sufferType in sufferTypes)
		{
			List<CharacterStateControl> temp = this.GetSufferCharacters(sufferType);
			foreach (CharacterStateControl character in temp)
			{
				SufferStateProperty property = character.currentSufferState.GetSufferStateProperty(sufferType);
				if (property.turnRate > 0f && !characters.Contains(character))
				{
					characters.Add(character);
				}
			}
		}
		if (characters.Count == 0)
		{
			yield break;
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
		foreach (SufferStateProperty.SufferType sufferType2 in sufferTypes)
		{
			string key = sufferType2.ToString();
			HitEffectParams[] temp2 = base.battleStateData.hitEffects.GetObject(key);
			if (temp2 != null)
			{
				this.hitEffectParams = temp2;
				break;
			}
		}
		base.stateManager.soundPlayer.TryPlaySE(this.hitEffectParams[0]);
		for (int i = 0; i < characters.Count; i++)
		{
			base.stateManager.threeDAction.PlayAnimationCharacterAction(characterAnimationType, new CharacterStateControl[]
			{
				characters[i]
			});
			base.stateManager.threeDAction.PlayHitEffectAction(this.hitEffectParams[i], characters[i]);
		}
		float waitSecond = base.stateManager.stateProperty.poisonHitEffectWaitSecond;
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(waitSecond, null, null);
		while (wait.MoveNext())
		{
			yield return null;
		}
		base.stateManager.soundPlayer.TryStopSE(this.hitEffectParams[0]);
		base.stateManager.soundPlayer.StopHitEffectSE();
		yield break;
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
		base.battleStateData.SEStopFunctionCall();
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
