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
			foreach (SubStatePlayHitAnimationAction.Data.HitIcon hitIcon in this.data.hitIconList)
			{
				hitIcon.target.CharacterParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
			}
			if (this.data.hitIconList.Count == 1)
			{
				base.stateManager.cameraControl.PlayCameraMotionActionCharacter(this.data.cameraKey, this.data.hitIconList[0].target);
			}
			else if (this.data.hitIconList.Count >= 2 && this.data.hitIconList[0].affectEffect != AffectEffect.InstantDeath)
			{
				base.stateManager.cameraControl.PlayCameraMotionActionCharacter(this.data.cameraKey, this.data.hitIconList[0].target);
			}
		}
		List<HitIcon> hitIconList = new List<HitIcon>();
		foreach (SubStatePlayHitAnimationAction.Data.HitIcon hitIcon2 in this.data.hitIconList)
		{
			int count = hitIconList.Count;
			HitIcon item2 = base.stateManager.uiControl.ApplyShowHitIcon(count, base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(hitIcon2.target), hitIcon2.affectEffect, hitIcon2.damage, hitIcon2.strength, hitIcon2.isMiss, hitIcon2.isCritical, hitIcon2.isDrain, hitIcon2.isCounter, hitIcon2.isReflection, hitIcon2.extraEffectType, hitIcon2.affectEffect != AffectEffect.InstantDeath);
			hitIconList.Add(item2);
		}
		HitEffectParams[] currentHitEffect = this.GetHitEffectParams();
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.threeDAction.ShowAllCharactersAction(this.data.hitIconList.Select((SubStatePlayHitAnimationAction.Data.HitIcon item) => item.target).ToArray<CharacterStateControl>());
		bool isPlayedDeathSE = false;
		Action PlayDeadSE = delegate()
		{
			if (!isPlayedDeathSE)
			{
				this.stateManager.soundPlayer.PlayDeathSE();
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
				bool flag = AffectEffectProperty.IsDamage(this.data.affectEffectProperty.type);
				bool flag2 = Tolerance.OnInfluenceToleranceAffectEffect(this.data.affectEffectProperty.type);
				bool flag3 = this.data.hitIconList[i].affectEffect == AffectEffect.TurnBarrier || this.data.hitIconList[i].affectEffect == AffectEffect.CountBarrier;
				bool flag4 = this.data.hitIconList[i].affectEffect == AffectEffect.TurnEvasion || this.data.hitIconList[i].affectEffect == AffectEffect.CountEvasion;
				bool flag5 = this.data.hitIconList[i].strength == Strength.Invalid;
				if (!this.data.hitIconList[i].target.isDied)
				{
					if ((flag || flag2) && (flag3 || flag4 || flag5))
					{
						base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(new CharacterStateControl[]
						{
							this.data.hitIconList[i].target
						});
					}
					else if (!this.data.hitIconList[i].isMiss)
					{
						AffectEffect affectEffect = this.data.hitIconList[i].affectEffect;
						switch (affectEffect)
						{
						case AffectEffect.Damage:
							goto IL_67E;
						default:
							switch (affectEffect)
							{
							case AffectEffect.ReferenceTargetHpRate:
							case AffectEffect.HpBorderlineDamage:
							case AffectEffect.HpBorderlineSpDamage:
							case AffectEffect.DefenseThroughDamage:
							case AffectEffect.SpDefenseThroughDamage:
							case AffectEffect.RefHpRateNonAttribute:
								goto IL_67E;
							case AffectEffect.ApDrain:
							case AffectEffect.Escape:
							case AffectEffect.Nothing:
								goto IL_86A;
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
								goto IL_86A;
							}
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
								this.data.hitIconList[i].target
							});
							break;
						case AffectEffect.Counter:
						case AffectEffect.Reflection:
						case AffectEffect.Destruct:
							goto IL_86A;
						case AffectEffect.InstantDeath:
							break;
						}
						IL_8AA:
						goto IL_8EA;
						IL_67E:
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
						goto IL_8AA;
						IL_86A:
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
					IL_8EA:;
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
				Vector3 fixableCharacterCenterPosition2DFunction = base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(this.data.hitIconList[j].target);
				hitIconList[j].HitIconReposition(fixableCharacterCenterPosition2DFunction);
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
		BattleEffectManager.Instance.ReturnEffect(hitEffectParams);
		yield break;
	}

	private HitEffectParams[] GetHitEffectParams()
	{
		List<HitEffectParams> list = new List<HitEffectParams>();
		string[] array = new string[]
		{
			"EFF_COM_HIT_NORMAL",
			"EFF_COM_HIT_WEAK",
			"EFF_COM_HIT_CRITICAL",
			"EFF_COM_S_HEAL",
			"EFF_COM_HIT_WEAK"
		};
		for (int i = 0; i < this.data.hitIconList.Count; i++)
		{
			if (this.data.hitIconList[i].affectEffect == AffectEffect.Invalid)
			{
				HitEffectParams item = BattleEffectManager.Instance.GetEffect("EFF_COM_HIT_WEAK") as HitEffectParams;
				list.Add(item);
			}
			else if (AffectEffectProperty.IsDamage(this.data.hitIconList[i].affectEffect))
			{
				if (this.data.hitIconList[i].isMiss)
				{
					HitEffectParams item2 = BattleEffectManager.Instance.GetEffect(array[0]) as HitEffectParams;
					list.Add(item2);
				}
				else if (!base.stateManager.onEnableTutorial && this.data.hitIconList[i].extraEffectType == ExtraEffectType.Up)
				{
					HitEffectParams item3 = BattleEffectManager.Instance.GetEffect(AffectEffect.gimmickSpecialAttackUp.ToString()) as HitEffectParams;
					list.Add(item3);
				}
				else if (!base.stateManager.onEnableTutorial && this.data.hitIconList[i].extraEffectType == ExtraEffectType.Down)
				{
					HitEffectParams item4 = BattleEffectManager.Instance.GetEffect(AffectEffect.gimmickSpecialAttackDown.ToString()) as HitEffectParams;
					list.Add(item4);
				}
				else
				{
					HitEffectParams item5 = BattleEffectManager.Instance.GetEffect(array[(int)this.data.hitIconList[i].strength]) as HitEffectParams;
					list.Add(item5);
				}
			}
			else
			{
				HitEffectParams item6 = BattleEffectManager.Instance.GetEffect(this.data.hitIconList[i].affectEffect.ToString()) as HitEffectParams;
				list.Add(item6);
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
