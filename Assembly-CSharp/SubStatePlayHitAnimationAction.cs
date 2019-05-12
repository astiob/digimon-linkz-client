using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubStatePlayHitAnimationAction : BattleStateBase
{
	private SubStatePlayHitAnimationAction.Data data;

	public SubStatePlayHitAnimationAction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	public void Init(SubStatePlayHitAnimationAction.Data data)
	{
		this.data = data;
	}

	protected override void EnabledThisState()
	{
	}

	protected override IEnumerator MainRoutine()
	{
		IEnumerator[] functions = new IEnumerator[]
		{
			this.PlayAdventureScene(BattleAdventureSceneManager.TriggerType.SkillHitStart),
			this.PlayHitAnimation(),
			this.PlayAdventureScene(BattleAdventureSceneManager.TriggerType.SkillHitEnd)
		};
		foreach (IEnumerator function in functions)
		{
			while (function.MoveNext())
			{
				object obj = function.Current;
				yield return obj;
			}
		}
		yield break;
	}

	private IEnumerator PlayHitAnimation()
	{
		if (!base.stateManager.cameraControl.IsPlaying(this.data.cameraKey))
		{
			foreach (SubStatePlayHitAnimationAction.Data.HitIcon temp in this.data.hitIconList)
			{
				temp.target.CharacterParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
			}
			base.stateManager.cameraControl.PlayCameraMotionActionCharacter(this.data.cameraKey, this.data.hitIconList[0].target);
		}
		List<HitIcon> hitIconList = new List<HitIcon>();
		foreach (SubStatePlayHitAnimationAction.Data.HitIcon hitIcon in this.data.hitIconList)
		{
			int index = hitIconList.Count;
			HitIcon temp2 = base.stateManager.uiControl.ApplyShowHitIcon(index, base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(hitIcon.target), hitIcon.affectEffect, hitIcon.damage, hitIcon.strength, hitIcon.isMiss, hitIcon.isCritical, hitIcon.isDrain, hitIcon.isCounter, hitIcon.isReflection, hitIcon.extraEffectType);
			hitIconList.Add(temp2);
		}
		HitEffectParams[] currentHitEffect = this.GetHitEffectParams();
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.threeDAction.ShowAllCharactersAction(this.data.hitIconList.Select((SubStatePlayHitAnimationAction.Data.HitIcon item) => item.target).ToArray<CharacterStateControl>());
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
		for (int i = 0; i < this.data.hitIconList.Count; i++)
		{
			if (this.data.hitIconList[i].target.isDiedJustBefore)
			{
				this.data.hitIconList[i].target.CharacterParams.gameObject.SetActive(false);
			}
			else
			{
				base.stateManager.threeDAction.PlayHitEffectAction(currentHitEffect[i], this.data.hitIconList[i].target);
				if (!isPlayedHitEffectSE)
				{
					isPlayedHitEffectSE = true;
					base.stateManager.soundPlayer.TryPlaySE(currentHitEffect[i]);
				}
				bool isDamage = AffectEffectProperty.IsDamage(this.data.affectEffectProperty.type);
				bool isTolerance = Tolerance.OnInfluenceToleranceAffectEffect(this.data.affectEffectProperty.type);
				bool isBarrier = this.data.hitIconList[i].affectEffect == AffectEffect.TurnBarrier || this.data.hitIconList[i].affectEffect == AffectEffect.CountBarrier;
				bool isEvasion = this.data.hitIconList[i].affectEffect == AffectEffect.TurnEvasion || this.data.hitIconList[i].affectEffect == AffectEffect.CountEvasion;
				bool isInvalid = this.data.hitIconList[i].strength == Strength.Invalid;
				if (!this.data.hitIconList[i].target.isDied)
				{
					if ((isDamage || isTolerance) && (isBarrier || isEvasion || isInvalid))
					{
						base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(new CharacterStateControl[]
						{
							this.data.hitIconList[i].target
						});
					}
					else if (!this.data.hitIconList[i].isMiss)
					{
						switch (this.data.hitIconList[i].affectEffect)
						{
						case AffectEffect.Damage:
						case AffectEffect.ReferenceTargetHpRate:
						case AffectEffect.HpBorderlineDamage:
						case AffectEffect.HpBorderlineSpDamage:
						case AffectEffect.DefenseThroughDamage:
						case AffectEffect.SpDefenseThroughDamage:
							if (this.data.hitIconList[i].strength == Strength.Weak)
							{
								base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.strongHit, new CharacterStateControl[]
								{
									this.data.hitIconList[i].target
								});
							}
							else if (this.data.hitIconList[i].strength == Strength.Drain)
							{
								base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.revival, new CharacterStateControl[]
								{
									this.data.hitIconList[i].target
								});
							}
							else
							{
								base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.hit, new CharacterStateControl[]
								{
									this.data.hitIconList[i].target
								});
							}
							break;
						case AffectEffect.AttackUp:
						case AffectEffect.DefenceUp:
						case AffectEffect.SpAttackUp:
						case AffectEffect.SpDefenceUp:
						case AffectEffect.SpeedUp:
						case AffectEffect.CorrectionDownReset:
						case AffectEffect.HpRevival:
						case AffectEffect.Counter:
						case AffectEffect.Reflection:
						case AffectEffect.Protect:
						case AffectEffect.HateDown:
						case AffectEffect.PowerCharge:
						case AffectEffect.Destruct:
						case AffectEffect.HitRateUp:
						case AffectEffect.InstantDeath:
						case AffectEffect.SufferStatusClear:
						case AffectEffect.SatisfactionRateUp:
						case AffectEffect.ApRevival:
						case AffectEffect.ApUp:
						case AffectEffect.ApConsumptionDown:
						case AffectEffect.CountGuard:
						case AffectEffect.TurnBarrier:
						case AffectEffect.CountBarrier:
						case AffectEffect.Invalid:
						case AffectEffect.Recommand:
						case AffectEffect.DamageRateUp:
						case AffectEffect.DamageRateDown:
						case AffectEffect.Regenerate:
						case AffectEffect.TurnEvasion:
						case AffectEffect.CountEvasion:
						case AffectEffect.ApDrain:
							goto IL_896;
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
								this.data.hitIconList[i].target
							});
							break;
						case AffectEffect.HpSettingFixable:
						case AffectEffect.HpSettingPercentage:
							if (this.data.hitIconList[i].damage > 0)
							{
								base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.hit, new CharacterStateControl[]
								{
									this.data.hitIconList[i].target
								});
							}
							else
							{
								base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.revival, new CharacterStateControl[]
								{
									this.data.hitIconList[i].target
								});
							}
							break;
						default:
							goto IL_896;
						}
						goto IL_91E;
						IL_896:
						base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.revival, new CharacterStateControl[]
						{
							this.data.hitIconList[i].target
						});
					}
					else
					{
						base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.guard, new CharacterStateControl[]
						{
							this.data.hitIconList[i].target
						});
					}
					IL_91E:;
				}
				else
				{
					if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1 && this.data.hitIconList[i].target.isEnemy)
					{
						base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.strongHit, new CharacterStateControl[]
						{
							this.data.hitIconList[i].target
						});
						isBigBossFindDeathCharacter = true;
					}
					else
					{
						base.stateManager.threeDAction.PlayDeadAnimationCharacterAction(PlayDeadSE, this.data.hitIconList[i].target);
					}
					isFindDeathCharacter = true;
				}
			}
		}
		base.stateManager.uiControl.ShowCharacterHUDFunction(this.data.hitIconList.Select((SubStatePlayHitAnimationAction.Data.HitIcon item) => item.target).ToArray<CharacterStateControl>());
		IEnumerator slowMotion = null;
		if (base.stateManager.IsLastBattleAndAllDeath())
		{
			if (base.stateManager.battleMode == BattleMode.Multi)
			{
				base.stateManager.uiControlMulti.HideAllDIalog();
			}
			slowMotion = base.stateManager.threeDAction.SlowMotionWaitAction();
		}
		if (isBigBossFindDeathCharacter)
		{
			base.stateManager.uiControl.Fade(Color.white, 1f, 1f);
		}
		float waitSecond = this.data.time;
		if (isFindDeathCharacter)
		{
			waitSecond += 1f;
		}
		while (waitSecond > 0f)
		{
			waitSecond -= Time.deltaTime;
			CharacterStateControl[] targets = this.data.hitIconList.Select((SubStatePlayHitAnimationAction.Data.HitIcon item) => item.target).ToArray<CharacterStateControl>();
			base.stateManager.uiControl.RepositionCharacterHUDPosition(targets);
			if (slowMotion != null)
			{
				slowMotion.MoveNext();
			}
			for (int j = 0; j < this.data.hitIconList.Count; j++)
			{
				Vector3 position = base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(this.data.hitIconList[j].target);
				hitIconList[j].HitIconReposition(position);
			}
			yield return null;
		}
		AppCoroutine.Start(this.StopHitAnimation(currentHitEffect), false);
		yield break;
	}

	private IEnumerator StopHitAnimation(HitEffectParams[] hitEffectParams)
	{
		if (hitEffectParams == null)
		{
			yield break;
		}
		yield return new WaitForSeconds(1.5f);
		yield return base.stateManager.threeDAction.StopHitAnimation(hitEffectParams);
		yield break;
	}

	private HitEffectParams[] GetHitEffectParams()
	{
		List<HitEffectParams> list = new List<HitEffectParams>();
		for (int i = 0; i < this.data.hitIconList.Count; i++)
		{
			if (AffectEffectProperty.IsDamage(this.data.hitIconList[i].affectEffect))
			{
				if (this.data.hitIconList[i].isMiss)
				{
					HitEffectParams item = base.battleStateData.hitEffects.GetObject(AffectEffect.Damage.ToString(), Strength.None.ToString())[i];
					list.Add(item);
				}
				else if (!base.stateManager.onEnableTutorial && this.data.hitIconList[i].extraEffectType == ExtraEffectType.Up)
				{
					HitEffectParams item2 = base.battleStateData.hitEffects.GetObject(AffectEffect.gimmickSpecialAttackUp.ToString())[i];
					list.Add(item2);
				}
				else if (!base.stateManager.onEnableTutorial && this.data.hitIconList[i].extraEffectType == ExtraEffectType.Down)
				{
					HitEffectParams item3 = base.battleStateData.hitEffects.GetObject(AffectEffect.gimmickSpecialAttackDown.ToString())[i];
					list.Add(item3);
				}
				else
				{
					HitEffectParams item4 = base.battleStateData.hitEffects.GetObject(AffectEffect.Damage.ToString(), this.data.hitIconList[i].strength.ToString())[i];
					list.Add(item4);
				}
			}
			else
			{
				HitEffectParams item5 = base.battleStateData.hitEffects.GetObject(this.data.hitIconList[i].affectEffect.ToString())[i];
				list.Add(item5);
			}
		}
		return list.ToArray();
	}

	private IEnumerator PlayAdventureScene(BattleAdventureSceneManager.TriggerType triggerType)
	{
		base.stateManager.battleAdventureSceneManager.OnTrigger(triggerType);
		IEnumerator Update = base.stateManager.battleAdventureSceneManager.Update();
		while (Update.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		foreach (SubStatePlayHitAnimationAction.Data.HitIcon hitIcon in this.data.hitIconList)
		{
			if (hitIcon.target.isDied)
			{
				hitIcon.target.isDiedJustBefore = true;
				base.stateManager.threeDAction.HideAllCharactersAction(new CharacterStateControl[]
				{
					hitIcon.target
				});
			}
		}
	}

	protected override void GetEventThisState(EventState eventState)
	{
		base.stateManager.uiControl.HideCharacterHUDFunction();
		if (base.battleStateData.isSlowMotion)
		{
			base.stateManager.threeDAction.StopSlowMotionAction();
		}
	}

	public class Data
	{
		public float time;

		public AffectEffectProperty affectEffectProperty;

		public string cameraKey = string.Empty;

		public List<SubStatePlayHitAnimationAction.Data.HitIcon> hitIconList = new List<SubStatePlayHitAnimationAction.Data.HitIcon>();

		public void AddHitIcon(CharacterStateControl target, AffectEffect affectEffect, int damage, Strength strength, bool isMiss, bool isCritical, bool isDrain, bool isCounter, bool isReflection, ExtraEffectType extraEffectType)
		{
			SubStatePlayHitAnimationAction.Data.HitIcon hitIcon = new SubStatePlayHitAnimationAction.Data.HitIcon();
			hitIcon.target = target;
			hitIcon.affectEffect = affectEffect;
			hitIcon.damage = damage;
			hitIcon.strength = strength;
			hitIcon.isMiss = isMiss;
			hitIcon.isCritical = isCritical;
			hitIcon.isDrain = isDrain;
			hitIcon.isCounter = isCounter;
			hitIcon.isReflection = isReflection;
			hitIcon.extraEffectType = extraEffectType;
			this.hitIconList.Add(hitIcon);
		}

		public class HitIcon
		{
			public CharacterStateControl target;

			public AffectEffect affectEffect;

			public int damage;

			public Strength strength;

			public bool isMiss = true;

			public bool isCritical;

			public bool isDrain;

			public bool isCounter;

			public bool isReflection;

			public ExtraEffectType extraEffectType;
		}
	}
}
