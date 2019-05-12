using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubStatePlayStageEffect : BattleStateController
{
	private bool isShowBattleStageEffect;

	private SufferStatePropertyCounter sufferStatePropertyCounter;

	private SubStateEnemiesItemDroppingFunction subStateEnemiesItemDroppingFunction;

	private SubStatePlayHitAnimationAction subStatePlayHitAnimationAction;

	private string cameraKey = string.Empty;

	public SubStatePlayStageEffect(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		this.subStateEnemiesItemDroppingFunction = new SubStateEnemiesItemDroppingFunction(null, new Action<EventState>(base.SendEventState));
		this.subStatePlayHitAnimationAction = new SubStatePlayHitAnimationAction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateEnemiesItemDroppingFunction);
		base.AddState(this.subStatePlayHitAnimationAction);
	}

	protected override void EnabledThisState()
	{
		this.isShowBattleStageEffect = false;
		this.sufferStatePropertyCounter = new SufferStatePropertyCounter();
	}

	protected override IEnumerator MainRoutine()
	{
		foreach (EffectStatusBase.EffectTriggerType trigger in base.battleStateData.reqestStageEffectTriggerList)
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

	private IEnumerator PlayStageEffect(EffectStatusBase.EffectTriggerType effectTriggerType)
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
		this.cameraKey = "0002_command";
		if (isBigBoss)
		{
			this.cameraKey = "BigBoss/0002_command";
		}
		base.stateManager.cameraControl.PlayCameraMotionAction(this.cameraKey, base.battleStateData.stageSpawnPoint, true);
		foreach (ExtraEffectStatus invocation in invocationList)
		{
			IEnumerator function = null;
			EffectStatusBase.ExtraEffectType effectType = (EffectStatusBase.ExtraEffectType)invocation.EffectType;
			if (effectType != EffectStatusBase.ExtraEffectType.LeaderChange)
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
		base.stateManager.cameraControl.StopCameraMotionAction(this.cameraKey);
		yield break;
	}

	private IEnumerator ShowBattleStageEffect()
	{
		if (!this.isShowBattleStageEffect)
		{
			base.stateManager.uiControl.ShowBattleExtraEffect(BattleExtraEffectUI.AnimationType.Stage);
			yield return null;
			while (base.stateManager.uiControl.IsBattleExtraEffect())
			{
				yield return null;
			}
			base.stateManager.uiControl.HideBattleExtraEffect();
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
		BattleMode battleMode = base.battleMode;
		if (battleMode != BattleMode.Single)
		{
			if (battleMode != BattleMode.Multi)
			{
				if (battleMode == BattleMode.PvP)
				{
					function = this.PvPLeaderChangeSync(leaderindex);
				}
			}
			else
			{
				function = this.MultiLeaderChangeSync(leaderindex);
			}
		}
		else
		{
			function = this.SingleLeaderChangeSync(leaderindex);
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
		foreach (CharacterStateControl characterStateControl in base.battleStateData.GetTotalCharacters())
		{
			if (!characterStateControl.isDied && extraEffectStatus.IsHitExtraEffect(characterStateControl, EffectStatusBase.ExtraEffectType.Skill))
			{
				targetList.Add(characterStateControl);
			}
		}
		if (targetList.Count == 0)
		{
			yield break;
		}
		if (!this.isShowBattleStageEffect)
		{
			base.stateManager.uiControl.ShowBattleExtraEffect(BattleExtraEffectUI.AnimationType.Stage);
			yield return null;
			while (base.stateManager.uiControl.IsBattleExtraEffect())
			{
				yield return null;
			}
			base.stateManager.uiControl.HideBattleExtraEffect();
			this.isShowBattleStageEffect = true;
		}
		string key = ((int)extraEffectStatus.EffectValue).ToString();
		SkillStatus status = base.hierarchyData.GetSkillStatus(key);
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
		this.subStateEnemiesItemDroppingFunction.Init(targetList.Where((CharacterStateControl item) => item.isDied).ToArray<CharacterStateControl>());
		base.SetState(this.subStateEnemiesItemDroppingFunction.GetType());
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
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.uiControl.ApplyHideHitIcon();
		yield break;
	}

	private IEnumerator PlaySkill(SkillStatus status, List<CharacterStateControl> targetList)
	{
		foreach (CharacterStateControl characterStateControl in this.GetTotalCharacters())
		{
			characterStateControl.OnChipTrigger(EffectStatusBase.EffectTriggerType.DamagePossibility);
		}
		foreach (AffectEffectProperty affectEffectProperty in status.affectEffect)
		{
			int hitNumber = 1;
			if (AffectEffectProperty.IsDamage(affectEffectProperty.type))
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
		foreach (CharacterStateControl characterStateControl2 in this.GetTotalCharacters())
		{
			characterStateControl2.ClearGutsData();
		}
		this.sufferStatePropertyCounter.UpdateCount(SufferStateProperty.SufferType.CountGuard, null);
		this.sufferStatePropertyCounter.UpdateCount(SufferStateProperty.SufferType.CountBarrier, null);
		this.sufferStatePropertyCounter.UpdateCount(SufferStateProperty.SufferType.CountEvasion, null);
		yield break;
	}

	private IEnumerator PlaySkill(AffectEffectProperty affectEffectProperty, List<CharacterStateControl> targetList)
	{
		SubStatePlayHitAnimationAction.Data data = new SubStatePlayHitAnimationAction.Data();
		foreach (CharacterStateControl characterStateControl in targetList)
		{
			if (!characterStateControl.isDied)
			{
				SkillResults skillResults;
				if (AffectEffectProperty.IsDamage(affectEffectProperty.type))
				{
					skillResults = SkillStatus.GetStageDamageSkillResult(affectEffectProperty, null, characterStateControl);
					if (!skillResults.onMissHit)
					{
						if (characterStateControl.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
						{
							this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountBarrier, characterStateControl, null);
						}
						else if (characterStateControl.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
						{
							this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountEvasion, characterStateControl, null);
						}
						else if (skillResults.isGuard)
						{
							this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountGuard, characterStateControl, skillResults.damageRateResult.dataList.Select((SufferStateProperty.Data item) => item.id).ToArray<string>());
						}
					}
				}
				else if (Tolerance.OnInfluenceToleranceAffectEffect(affectEffectProperty.type))
				{
					skillResults = base.stateManager.skillDetails.GetToleranceSkillResult(affectEffectProperty, null, characterStateControl);
					if (!skillResults.onMissHit)
					{
						if (characterStateControl.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
						{
							this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountBarrier, characterStateControl, null);
						}
						else if (characterStateControl.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
						{
							this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountEvasion, characterStateControl, null);
						}
					}
				}
				else
				{
					skillResults = base.stateManager.skillDetails.GetOtherSkillResult(affectEffectProperty, null, characterStateControl);
				}
				data.AddHitIcon(characterStateControl, skillResults.hitIconAffectEffect, skillResults.attackPower, skillResults.onWeakHit, skillResults.onMissHit, skillResults.onCriticalHit, false, false, false, skillResults.extraEffectType);
			}
		}
		data.time = base.stateManager.stateProperty.multiHitIntervalWaitSecond;
		data.affectEffectProperty = affectEffectProperty;
		data.cameraKey = this.cameraKey;
		this.subStatePlayHitAnimationAction.Init(data);
		base.SetState(this.subStatePlayHitAnimationAction.GetType());
		while (base.isWaitState)
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
