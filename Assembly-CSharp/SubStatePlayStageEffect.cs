using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStatePlayStageEffect : BattleStateController
{
	private bool isShowBattleStageEffect;

	private List<CharacterStateControl> countBarrierList;

	private List<CharacterStateControl> countEvasionList;

	public SubStatePlayStageEffect(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStateEnemiesItemDroppingFunction(null, new Action<EventState>(base.SendEventState)));
		base.AddState(new SubStatePlayHitAnimationAction(null, new Action<EventState>(base.SendEventState)));
	}

	protected override void EnabledThisState()
	{
		this.isShowBattleStageEffect = false;
		this.countBarrierList = new List<CharacterStateControl>();
		this.countEvasionList = new List<CharacterStateControl>();
	}

	protected override IEnumerator MainRoutine()
	{
		foreach (ChipEffectStatus.EffectTriggerType trigger in base.battleStateData.reqestStageEffectTriggerList)
		{
			IEnumerator function = this.PlayStageEffect(trigger);
			while (function.MoveNext())
			{
				yield return null;
			}
		}
		base.battleStateData.reqestStageEffectTriggerList.Clear();
		yield break;
	}

	protected override void DisabledThisState()
	{
	}

	protected override void GetEventThisState(EventState eventState)
	{
	}

	private IEnumerator PlayStageEffect(ChipEffectStatus.EffectTriggerType effectTriggerType)
	{
		bool isBigBoss = base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1;
		List<ExtraEffectStatus> extraEffectStatus = BattleStateManager.current.battleStateData.extraEffectStatus;
		List<ExtraEffectStatus> invocationList = ExtraEffectStatus.GetInvocationList(extraEffectStatus, effectTriggerType);
		if (invocationList.Count == 0)
		{
			yield break;
		}
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.SetBattleScreen(BattleScreen.PoisonHit);
		string cameraKey = "0002_command";
		if (isBigBoss)
		{
			cameraKey = "BigBoss/0002_command";
		}
		base.stateManager.cameraControl.PlayCameraMotionAction(cameraKey, base.battleStateData.stageSpawnPoint, true);
		foreach (ExtraEffectStatus invocation in invocationList)
		{
			IEnumerator function = null;
			ExtraEffectStatus.ExtraEffectType effectType = (ExtraEffectStatus.ExtraEffectType)invocation.EffectType;
			ExtraEffectStatus.ExtraEffectType extraEffectType = effectType;
			if (extraEffectType != ExtraEffectStatus.ExtraEffectType.LeaderChange)
			{
				function = this.PlayDefaultEffect(invocation);
			}
			else
			{
				function = this.PlayLeaderChange(invocation);
			}
			while (function.MoveNext())
			{
				yield return null;
			}
			if (base.stateManager.IsLastBattleAndAllDeath())
			{
				break;
			}
		}
		base.stateManager.cameraControl.StopCameraMotionAction(cameraKey);
		yield break;
	}

	private IEnumerator ShowBattleStageEffect()
	{
		if (!this.isShowBattleStageEffect)
		{
			base.stateManager.uiControl.ShowBattleStageEffect();
			yield return null;
			while (base.stateManager.uiControl.IsBattleStageEffect())
			{
				yield return null;
			}
			base.stateManager.uiControl.HideBattleStageEffect();
			this.isShowBattleStageEffect = true;
		}
		yield break;
	}

	private IEnumerator PlayLeaderChange(ExtraEffectStatus extraEffectStatus)
	{
		IEnumerator showBattleStageEffect = this.ShowBattleStageEffect();
		while (showBattleStageEffect.MoveNext())
		{
			yield return null;
		}
		int leaderindex = (int)extraEffectStatus.EffectValue;
		bool isRandom = leaderindex <= 0 || leaderindex > 3;
		if (isRandom)
		{
			leaderindex = UnityEngine.Random.Range(0, 3);
		}
		else
		{
			leaderindex--;
		}
		IEnumerator function = null;
		switch (base.battleMode)
		{
		case BattleMode.Single:
			function = this.SingleLeaderChangeSync(leaderindex);
			break;
		case BattleMode.Multi:
			function = this.MultiLeaderChangeSync(leaderindex);
			break;
		case BattleMode.PvP:
			function = this.PvPLeaderChangeSync(leaderindex);
			break;
		}
		while (function.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator SingleLeaderChangeSync(int leaderindex)
	{
		base.battleStateData.ChangePlayerLeader(leaderindex);
		base.battleStateData.ChangeEnemyLeader(leaderindex);
		IEnumerator function = this.PlayLeaderChangeAnimation();
		while (function.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator MultiLeaderChangeSync(int leaderindex)
	{
		base.stateManager.uiControlMulti.ShowLoading(false);
		base.stateManager.uiControl.SetTouchEnable(false);
		if (base.stateManager.multiFunction.IsOwner)
		{
			IEnumerator sendCancelCharacterRevival = base.stateManager.multiFunction.SendLeaderChange(leaderindex);
			while (sendCancelCharacterRevival.MoveNext())
			{
				object obj = sendCancelCharacterRevival.Current;
				yield return obj;
			}
		}
		else
		{
			IEnumerator waitAllPlayers = base.stateManager.multiFunction.WaitAllPlayers(TCPMessageType.LeaderChange);
			while (waitAllPlayers.MoveNext())
			{
				object obj2 = waitAllPlayers.Current;
				yield return obj2;
			}
		}
		base.stateManager.uiControl.SetTouchEnable(true);
		base.stateManager.uiControlMulti.HideLoading();
		IEnumerator function = this.PlayLeaderChangeAnimation();
		while (function.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator PvPLeaderChangeSync(int leaderindex)
	{
		base.stateManager.uiControlPvP.ShowLoading(false);
		base.stateManager.uiControl.SetTouchEnable(false);
		if (base.stateManager.pvpFunction.IsOwner)
		{
			IEnumerator sendCancelCharacterRevival = base.stateManager.pvpFunction.SendLeaderChange(leaderindex);
			while (sendCancelCharacterRevival.MoveNext())
			{
				object obj = sendCancelCharacterRevival.Current;
				yield return obj;
			}
		}
		else
		{
			IEnumerator waitAllPlayers = base.stateManager.pvpFunction.WaitAllPlayers(TCPMessageType.LeaderChange);
			while (waitAllPlayers.MoveNext())
			{
				object obj2 = waitAllPlayers.Current;
				yield return obj2;
			}
		}
		base.stateManager.uiControl.SetTouchEnable(true);
		base.stateManager.uiControlPvP.HideLoading();
		IEnumerator function = this.PlayLeaderChangeAnimation();
		while (function.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator PlayLeaderChangeAnimation()
	{
		base.stateManager.uiControl.ApplyBattleStartAction(true);
		base.stateManager.uiControl.ApplyBattleStartActionTitle(false);
		base.stateManager.uiControl.ApplyPlayerLeaderSkill(true, base.battleStateData.leaderCharacter.leaderSkillStatus.name, true);
		base.stateManager.uiControl.ApplyEnemyLeaderSkill(base.battleMode == BattleMode.PvP, base.battleStateData.leaderEnemyCharacter.leaderSkillStatus.name, true);
		base.stateManager.uiControl.ApplyAllMonsterButtonEnable(false);
		string cameraKey = string.Empty;
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			cameraKey = "0001_bossStart";
		}
		else
		{
			cameraKey = "0002_roundStart";
		}
		base.stateManager.cameraControl.PlayCameraMotionAction(cameraKey, base.battleStateData.stageSpawnPoint, true);
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.RoundStartActionWaitSecond, null, null);
		while (wait.MoveNext())
		{
			yield return null;
		}
		base.stateManager.cameraControl.StopCameraMotionAction(cameraKey);
		base.stateManager.uiControl.ApplyBattleStartAction(false);
		base.stateManager.uiControl.ApplyAllMonsterButtonEnable(true);
		base.stateManager.threeDAction.StopAlwaysEffectAction(base.battleStateData.stageGimmickUpEffect);
		yield break;
	}

	private IEnumerator PlayDefaultEffect(ExtraEffectStatus extraEffectStatus)
	{
		List<CharacterStateControl> targetList = new List<CharacterStateControl>();
		foreach (CharacterStateControl character in base.battleStateData.GetTotalCharacters())
		{
			if (!character.isDied && extraEffectStatus.IsHitExtraEffect(character, ExtraEffectStatus.ExtraEffectType.Skill))
			{
				targetList.Add(character);
			}
		}
		if (targetList.Count == 0)
		{
			yield break;
		}
		if (!this.isShowBattleStageEffect)
		{
			base.stateManager.uiControl.ShowBattleStageEffect();
			yield return null;
			while (base.stateManager.uiControl.IsBattleStageEffect())
			{
				yield return null;
			}
			base.stateManager.uiControl.HideBattleStageEffect();
			this.isShowBattleStageEffect = true;
		}
		string key = ((int)extraEffectStatus.EffectValue).ToString();
		SkillStatus status = base.battleStateData.skillStatus.GetObject(key);
		IEnumerator playFunction = null;
		if (status != null)
		{
			playFunction = this.PlaySkill(status, targetList);
		}
		else
		{
			playFunction = this.PlayNotSkill(extraEffectStatus, targetList);
		}
		while (playFunction.MoveNext())
		{
			yield return null;
		}
		base.SetState(typeof(SubStateEnemiesItemDroppingFunction));
		while (base.isWaitState)
		{
			yield return null;
		}
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.skillAfterWaitSecond, null, null);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		base.battleStateData.SEStopFunctionCall();
		while (base.battleStateData.StopHitAnimationCall())
		{
			yield return null;
		}
		base.stateManager.threeDAction.HideDeadCharactersAction(base.battleStateData.GetTotalCharacters());
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.uiControl.ApplyHideHitIcon();
		yield break;
	}

	private IEnumerator PlaySkill(SkillStatus status, List<CharacterStateControl> targetList)
	{
		this.countBarrierList.Clear();
		this.countEvasionList.Clear();
		foreach (AffectEffectProperty affectEffectProperty in status.affectEffect)
		{
			int hitNumber = 1;
			if (affectEffectProperty.type == AffectEffect.Damage || affectEffectProperty.type == AffectEffect.ReferenceTargetHpRate)
			{
				hitNumber = affectEffectProperty.hitNumber;
			}
			for (int i = 0; i < hitNumber; i++)
			{
				IEnumerator playSkill = this.PlaySkill(affectEffectProperty, targetList);
				while (playSkill.MoveNext())
				{
					yield return null;
				}
			}
		}
		this.UpdateCountBarrier();
		this.UpdateCountEvasion();
		yield break;
	}

	private IEnumerator PlaySkill(AffectEffectProperty affectEffectProperty, List<CharacterStateControl> targetList)
	{
		List<CharacterStateControl> skillResultTargets = new List<CharacterStateControl>();
		List<bool> skillResultMisses = new List<bool>();
		List<int> skillResultDamages = new List<int>();
		List<Strength> skillResultStrength = new List<Strength>();
		List<AffectEffect> skillResultAffectEffect = new List<AffectEffect>();
		foreach (CharacterStateControl target in targetList)
		{
			if (!target.isDied)
			{
				int hitIconDigit = 0;
				bool isHit = affectEffectProperty.OnHit(target);
				Strength strength = Strength.None;
				AffectEffect affectEffect = affectEffectProperty.type;
				if (isHit)
				{
					if (affectEffectProperty.type == AffectEffect.Damage || affectEffectProperty.type == AffectEffect.ReferenceTargetHpRate)
					{
						strength = target.tolerance.GetAttributeStrength(affectEffectProperty.attribute);
						if (strength == Strength.Invalid)
						{
							affectEffect = AffectEffect.Invalid;
						}
						else if (target.currentSufferState.onTurnBarrier.isActive)
						{
							affectEffect = AffectEffect.TurnBarrier;
						}
						else if (target.currentSufferState.onCountBarrier.isActive)
						{
							affectEffect = AffectEffect.CountBarrier;
							this.AddCountBarrierList(target);
						}
						else if (target.currentSufferState.onTurnEvasion.isActive)
						{
							affectEffect = AffectEffect.TurnEvasion;
						}
						else if (target.currentSufferState.onCountEvasion.isActive)
						{
							affectEffect = AffectEffect.CountEvasion;
							this.AddCountEvasionList(target);
						}
						else
						{
							float reduceDamageRate = SkillStatus.GetReduceDamageRate(target.currentSufferState);
							if (affectEffectProperty.powerType == PowerType.Fixable)
							{
								hitIconDigit = (int)((float)affectEffectProperty.damagePower * reduceDamageRate);
							}
							else
							{
								hitIconDigit = (int)(affectEffectProperty.damagePercent * (float)target.hp * reduceDamageRate);
							}
							if (strength != Strength.Drain)
							{
								target.hp -= hitIconDigit;
							}
							else
							{
								target.hp += hitIconDigit;
							}
						}
					}
					else if (Tolerance.OnInfluenceToleranceAffectEffect(affectEffectProperty.type))
					{
						strength = target.tolerance.GetAffectEffectStrength(affectEffect);
						if (strength == Strength.Invalid)
						{
							affectEffect = AffectEffect.Invalid;
						}
						else if (target.currentSufferState.onTurnBarrier.isActive)
						{
							affectEffect = AffectEffect.TurnBarrier;
						}
						else if (target.currentSufferState.onCountBarrier.isActive)
						{
							affectEffect = AffectEffect.CountBarrier;
							this.AddCountBarrierList(target);
						}
						else if (target.currentSufferState.onTurnEvasion.isActive)
						{
							affectEffect = AffectEffect.TurnEvasion;
						}
						else if (target.currentSufferState.onCountEvasion.isActive)
						{
							affectEffect = AffectEffect.CountEvasion;
							this.AddCountEvasionList(target);
						}
						else if (affectEffectProperty.type == AffectEffect.InstantDeath)
						{
							target.Kill();
						}
						else
						{
							SufferStateProperty suffer = new SufferStateProperty(affectEffectProperty, base.battleStateData.currentLastGenerateStartTimingSufferState);
							target.currentSufferState.SetSufferState(suffer, null);
							base.battleStateData.currentLastGenerateStartTimingSufferState++;
						}
					}
					else
					{
						AffectEffect type = affectEffectProperty.type;
						if (type != AffectEffect.HpRevival)
						{
							base.stateManager.skillDetails.AddSufferStateOthers(target, affectEffectProperty);
						}
						else
						{
							hitIconDigit = base.stateManager.skillDetails.HpRevival(target, affectEffectProperty);
							skillResultDamages.Add(hitIconDigit);
						}
					}
				}
				skillResultTargets.Add(target);
				skillResultMisses.Add(!isHit);
				skillResultDamages.Add(hitIconDigit);
				skillResultStrength.Add(strength);
				skillResultAffectEffect.Add(affectEffect);
			}
		}
		List<HitIcon> hitIconlist = new List<HitIcon>();
		Vector3[] hitIconPositions = this.GetHitIconPositions(skillResultTargets);
		for (int i = 0; i < skillResultTargets.Count; i++)
		{
			HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(i, hitIconPositions[i], skillResultAffectEffect[i], skillResultDamages[i], skillResultStrength[i], skillResultMisses[i], false, false, false, ExtraEffectType.Non);
			hitIconlist.Add(hitIcon);
		}
		base.battleStateData.SetPlayAnimationActionValues(null, skillResultTargets.ToArray(), affectEffectProperty.type, base.stateManager.stateProperty.multiHitIntervalWaitSecond, skillResultMisses.ToArray(), hitIconlist.ToArray(), affectEffectProperty, false, ExtraEffectType.Non);
		base.SetState(typeof(SubStatePlayHitAnimationAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	private void AddCountBarrierList(CharacterStateControl value)
	{
		if (!this.countBarrierList.Contains(value))
		{
			this.countBarrierList.Add(value);
		}
	}

	private void UpdateCountBarrier()
	{
		foreach (CharacterStateControl characterStateControl in this.countBarrierList)
		{
			characterStateControl.currentSufferState.onCountBarrier.currentKeepRound--;
			if (characterStateControl.currentSufferState.onCountBarrier.currentKeepRound <= 0)
			{
				characterStateControl.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.CountBarrier);
			}
		}
	}

	private void AddCountEvasionList(CharacterStateControl value)
	{
		if (!this.countEvasionList.Contains(value))
		{
			this.countEvasionList.Add(value);
		}
	}

	private void UpdateCountEvasion()
	{
		foreach (CharacterStateControl characterStateControl in this.countEvasionList)
		{
			characterStateControl.currentSufferState.onCountEvasion.currentKeepRound--;
			if (characterStateControl.currentSufferState.onCountEvasion.currentKeepRound <= 0)
			{
				characterStateControl.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.CountEvasion);
			}
		}
	}

	private IEnumerator PlayNotSkill(ExtraEffectStatus extraEffectStatus, List<CharacterStateControl> targetList)
	{
		yield return null;
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
}
