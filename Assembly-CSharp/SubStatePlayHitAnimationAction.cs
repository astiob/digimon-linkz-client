using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStatePlayHitAnimationAction : BattleStateBase
{
	private HitEffectParams[] currentHitEffect;

	public SubStatePlayHitAnimationAction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void EnabledThisState()
	{
		this.currentHitEffect = null;
	}

	protected override IEnumerator MainRoutine()
	{
		CharacterStateControl currentCharacter = (CharacterStateControl)base.battleStateData.sendValues["currentCharacter"];
		CharacterStateControl[] isTargetsStatus = base.battleStateData.sendValues["isTargetsStatus"] as CharacterStateControl[];
		AffectEffect affectEffect = (AffectEffect)((int)base.battleStateData.sendValues["affectEffect"]);
		float waitTime = (float)base.battleStateData.sendValues["waitTime"];
		bool[] onMissHit = base.battleStateData.sendValues["onMissHit"] as bool[];
		HitIcon[] hitIconList = base.battleStateData.sendValues["hitIconList"] as HitIcon[];
		AffectEffectProperty status = base.battleStateData.sendValues["status"] as AffectEffectProperty;
		bool useSlowMotion = (bool)base.battleStateData.sendValues["useSlowMotion"];
		ExtraEffectType extraEffectType = (ExtraEffectType)((int)base.battleStateData.sendValues["extraEffectType"]);
		this.currentHitEffect = this.GetHitEffectParams(affectEffect, isTargetsStatus, status, extraEffectType);
		while (base.battleStateData.StopHitAnimationCall())
		{
			yield return null;
		}
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.threeDAction.ShowAllCharactersAction(isTargetsStatus);
		Action PlayDeadSE = delegate()
		{
			bool isPlayedDeathSE;
			if (!isPlayedDeathSE)
			{
				base.stateManager.soundPlayer.PlayDeathSE();
			}
			isPlayedDeathSE = true;
		};
		bool isFindDeathCharacter = false;
		bool isBigBossFindDeathCharacter = false;
		bool isPlayedHitEffectSE = false;
		for (int i = 0; i < isTargetsStatus.Length; i++)
		{
			if (currentCharacter != null && currentCharacter.Contains(new CharacterStateControl[]
			{
				isTargetsStatus[i]
			}) && !isTargetsStatus[i].isDied)
			{
				base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.idle, new CharacterStateControl[]
				{
					isTargetsStatus[i]
				});
			}
			if (isTargetsStatus[i].isDiedJustBefore)
			{
				isTargetsStatus[i].CharacterParams.gameObject.SetActive(false);
			}
			else
			{
				base.stateManager.threeDAction.PlayHitEffectAction(this.currentHitEffect[i], isTargetsStatus[i]);
				if (!isPlayedHitEffectSE)
				{
					isPlayedHitEffectSE = true;
					base.stateManager.soundPlayer.TryPlaySE(this.currentHitEffect[i]);
				}
				if (!isTargetsStatus[i].isDied)
				{
					if (this.CheckBarrier(isTargetsStatus[i], affectEffect) || this.CheckEvasion(isTargetsStatus[i], affectEffect) || this.CheckInvalid(isTargetsStatus[i], affectEffect, status))
					{
						base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.idle, new CharacterStateControl[]
						{
							isTargetsStatus[i]
						});
					}
					else if (!onMissHit[i])
					{
						AffectEffect affectEffect2 = affectEffect;
						switch (affectEffect2)
						{
						case AffectEffect.Damage:
						{
							Strength strength = isTargetsStatus[i].tolerance.GetAttributeStrength(status.attribute);
							if (strength == Strength.Weak)
							{
								base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.strongHit, new CharacterStateControl[]
								{
									isTargetsStatus[i]
								});
							}
							else if (strength == Strength.Drain)
							{
								base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.revival, new CharacterStateControl[]
								{
									isTargetsStatus[i]
								});
							}
							else
							{
								base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.hit, new CharacterStateControl[]
								{
									isTargetsStatus[i]
								});
							}
							break;
						}
						default:
							if (affectEffect2 != AffectEffect.ReferenceTargetHpRate)
							{
								goto IL_604;
							}
							base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.hit, new CharacterStateControl[]
							{
								isTargetsStatus[i]
							});
							break;
						case AffectEffect.AttackDown:
						case AffectEffect.DefenceDown:
						case AffectEffect.SpAttackDown:
						case AffectEffect.SpDefenceDown:
						case AffectEffect.SpeedDown:
						case AffectEffect.CorrectionUpReset:
						case AffectEffect.HateUp:
						case AffectEffect.Paralysis:
						case AffectEffect.Poison:
						case AffectEffect.Sleep:
						case AffectEffect.SkillLock:
						case AffectEffect.HitRateDown:
						case AffectEffect.Confusion:
						case AffectEffect.Stun:
						case AffectEffect.SatisfactionRateDown:
						case AffectEffect.ApDown:
						case AffectEffect.ApConsumptionUp:
							base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.hit, new CharacterStateControl[]
							{
								isTargetsStatus[i]
							});
							break;
						case AffectEffect.Counter:
						case AffectEffect.Reflection:
						case AffectEffect.PowerCharge:
						case AffectEffect.Destruct:
							goto IL_604;
						}
						goto IL_666;
						IL_604:
						base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.revival, new CharacterStateControl[]
						{
							isTargetsStatus[i]
						});
					}
					else
					{
						base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.guard, new CharacterStateControl[]
						{
							isTargetsStatus[i]
						});
					}
					IL_666:;
				}
				else
				{
					if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1 && isTargetsStatus[i].isEnemy)
					{
						base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.strongHit, new CharacterStateControl[]
						{
							isTargetsStatus[i]
						});
						isBigBossFindDeathCharacter = true;
					}
					else
					{
						base.stateManager.threeDAction.PlayDeadAnimationCharacterAction(PlayDeadSE, isTargetsStatus[i]);
					}
					isFindDeathCharacter = true;
				}
			}
		}
		base.stateManager.uiControl.ShowCharacterHUDFunction(isTargetsStatus);
		IEnumerator slowMotion;
		if (useSlowMotion && base.stateManager.IsLastBattleAndAllDeath())
		{
			if (base.stateManager.battleMode == BattleMode.Multi)
			{
				base.stateManager.uiControlMulti.HideAllDIalog();
			}
			slowMotion = base.stateManager.threeDAction.SlowMotionWaitAction();
		}
		Action hudReposition = delegate()
		{
			base.stateManager.uiControl.RepositionCharacterHUDPosition(isTargetsStatus);
			if (slowMotion != null)
			{
				slowMotion.MoveNext();
			}
			for (int j = 0; j < isTargetsStatus.Length; j++)
			{
				Vector3 fixableCharacterCenterPosition2DFunction = base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(isTargetsStatus[j]);
				hitIconList[j].HitIconReposition(fixableCharacterCenterPosition2DFunction);
			}
		};
		if (isBigBossFindDeathCharacter)
		{
			base.stateManager.uiControl.Fade(Color.white, 1f, 1f);
		}
		float waitSecond = waitTime;
		if (isFindDeathCharacter)
		{
			waitSecond += 1f;
		}
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(waitSecond, hudReposition, null);
		while (wait.MoveNext())
		{
			yield return null;
		}
		base.battleStateData.stopHitAnimation = base.stateManager.threeDAction.StopHitAnimation(this.currentHitEffect);
		yield break;
	}

	private HitEffectParams[] GetHitEffectParams(AffectEffect affectEffect, CharacterStateControl[] isTargetsStatus, AffectEffectProperty status, ExtraEffectType extraEffectType)
	{
		HitEffectParams[] result;
		if (affectEffect == AffectEffect.Damage || affectEffect == AffectEffect.ReferenceTargetHpRate)
		{
			if (!base.stateManager.onEnableTutorial && extraEffectType == ExtraEffectType.Up)
			{
				result = base.battleStateData.hitEffects.GetObject(AffectEffect.gimmickSpecialAttackUp.ToString());
			}
			else if (!base.stateManager.onEnableTutorial && extraEffectType == ExtraEffectType.Down)
			{
				result = base.battleStateData.hitEffects.GetObject(AffectEffect.gimmickSpecialAttackDown.ToString());
			}
			else if (status != null)
			{
				List<HitEffectParams> list = new List<HitEffectParams>();
				for (int i = 0; i < isTargetsStatus.Length; i++)
				{
					HitEffectParams item;
					if (isTargetsStatus[i].currentSufferState.onTurnBarrier.isActive)
					{
						item = base.battleStateData.hitEffects.GetObject(AffectEffect.TurnBarrier.ToString())[i];
					}
					else if (isTargetsStatus[i].currentSufferState.onCountBarrier.isActive)
					{
						item = base.battleStateData.hitEffects.GetObject(AffectEffect.CountBarrier.ToString())[i];
					}
					else if (isTargetsStatus[i].currentSufferState.onTurnEvasion.isActive)
					{
						item = base.battleStateData.hitEffects.GetObject(AffectEffect.TurnEvasion.ToString())[i];
					}
					else if (isTargetsStatus[i].currentSufferState.onCountEvasion.isActive)
					{
						item = base.battleStateData.hitEffects.GetObject(AffectEffect.CountEvasion.ToString())[i];
					}
					else if (affectEffect == AffectEffect.ReferenceTargetHpRate)
					{
						item = base.battleStateData.hitEffects.GetObject(AffectEffect.ReferenceTargetHpRate.ToString())[i];
					}
					else
					{
						Strength attributeStrength = isTargetsStatus[i].tolerance.GetAttributeStrength(status.attribute);
						item = base.battleStateData.GetDamageEffect(attributeStrength)[i];
					}
					list.Add(item);
				}
				result = list.ToArray();
			}
			else
			{
				result = base.battleStateData.hitEffects.GetObject(affectEffect.ToString(), Strength.None.ToString());
			}
		}
		else if (Tolerance.OnInfluenceToleranceAffectEffect(affectEffect))
		{
			List<HitEffectParams> list2 = new List<HitEffectParams>();
			for (int j = 0; j < isTargetsStatus.Length; j++)
			{
				HitEffectParams item2;
				if (isTargetsStatus[j].currentSufferState.onTurnBarrier.isActive)
				{
					item2 = base.battleStateData.hitEffects.GetObject(AffectEffect.TurnBarrier.ToString())[j];
				}
				else if (isTargetsStatus[j].currentSufferState.onCountBarrier.isActive)
				{
					item2 = base.battleStateData.hitEffects.GetObject(AffectEffect.CountBarrier.ToString())[j];
				}
				else if (isTargetsStatus[j].currentSufferState.onTurnEvasion.isActive)
				{
					item2 = base.battleStateData.hitEffects.GetObject(AffectEffect.TurnEvasion.ToString())[j];
				}
				else if (isTargetsStatus[j].currentSufferState.onCountEvasion.isActive)
				{
					item2 = base.battleStateData.hitEffects.GetObject(AffectEffect.CountEvasion.ToString())[j];
				}
				else
				{
					item2 = base.battleStateData.hitEffects.GetObject(affectEffect.ToString())[j];
				}
				list2.Add(item2);
			}
			result = list2.ToArray();
		}
		else if (affectEffect == AffectEffect.HpRevival && status != null && status.useDrain)
		{
			List<HitEffectParams> list3 = new List<HitEffectParams>();
			for (int k = 0; k < isTargetsStatus.Length; k++)
			{
				list3.Add(base.battleStateData.drainEffect);
			}
			result = list3.ToArray();
		}
		else
		{
			result = base.battleStateData.hitEffects.GetObject(affectEffect.ToString());
		}
		return result;
	}

	private bool CheckBarrier(CharacterStateControl characterStateControl, AffectEffect affectEffect)
	{
		bool flag = affectEffect == AffectEffect.Damage;
		bool flag2 = Tolerance.OnInfluenceToleranceAffectEffect(affectEffect);
		bool isActive = characterStateControl.currentSufferState.onTurnBarrier.isActive;
		bool isActive2 = characterStateControl.currentSufferState.onCountBarrier.isActive;
		return (flag || flag2) && (isActive || isActive2);
	}

	private bool CheckEvasion(CharacterStateControl characterStateControl, AffectEffect affectEffect)
	{
		bool flag = affectEffect == AffectEffect.Damage;
		bool flag2 = Tolerance.OnInfluenceToleranceAffectEffect(affectEffect);
		bool isActive = characterStateControl.currentSufferState.onTurnEvasion.isActive;
		bool isActive2 = characterStateControl.currentSufferState.onCountEvasion.isActive;
		return (flag || flag2) && (isActive || isActive2);
	}

	private bool CheckInvalid(CharacterStateControl characterStateControl, AffectEffect affectEffect, AffectEffectProperty status)
	{
		bool flag = affectEffect == AffectEffect.Damage;
		bool flag2 = Tolerance.OnInfluenceToleranceAffectEffect(affectEffect);
		Tolerance tolerance = characterStateControl.tolerance;
		if (flag)
		{
			Strength attributeStrength = tolerance.GetAttributeStrength(status.attribute);
			return attributeStrength == Strength.Invalid;
		}
		if (flag2)
		{
			Strength affectEffectStrength = tolerance.GetAffectEffectStrength(affectEffect);
			return affectEffectStrength == Strength.Invalid;
		}
		return false;
	}

	protected override void DisabledThisState()
	{
		CharacterStateControl[] array = base.battleStateData.sendValues["isTargetsStatus"] as CharacterStateControl[];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].isDied)
			{
				array[i].isDiedJustBefore = true;
			}
		}
	}

	protected override void GetEventThisState(EventState eventState)
	{
		base.stateManager.threeDAction.StopHitAnimation(this.currentHitEffect);
		base.stateManager.soundPlayer.StopHitEffectSE();
		base.stateManager.uiControl.HideCharacterHUDFunction();
		if (base.battleStateData.isSlowMotion)
		{
			base.stateManager.threeDAction.StopSlowMotionAction();
		}
	}
}
