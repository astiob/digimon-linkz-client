using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle3DAction : BattleFunctionBase
{
	public void BigBossInsertAction(CharacterStateControl cState, CharacterStateControl[] playerCharacters)
	{
		this.PlayIdleAnimationActiveCharacterAction(new CharacterStateControl[]
		{
			cState
		});
		foreach (CharacterStateControl characterStateControl in playerCharacters)
		{
			if (characterStateControl.isDied)
			{
				characterStateControl.CharacterParams.gameObject.SetActive(false);
			}
			else
			{
				characterStateControl.CharacterParams.gameObject.SetActive(true);
				characterStateControl.CharacterParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
			}
		}
		cState.CharacterParams.gameObject.SetActive(true);
		cState.CharacterParams.PlayAnimation(CharacterAnimationType.eat, SkillType.Attack, 0, null, null);
		base.stateManager.soundPlayer.TryPlaySE(cState.CharacterParams.battleInSE, 0f, false);
	}

	public IEnumerator BigBossExitAction(CharacterStateControl character)
	{
		if (character.isEnemy && character.isDied)
		{
			base.stateManager.uiControl.Fade(Color.white, 0.5f, 0f);
			base.stateManager.cameraControl.PlayCameraMotionAction("BigBoss/0008_withdrawal", base.battleStateData.stageSpawnPoint, true);
			character.CharacterParams.gameObject.SetActive(true);
			character.CharacterParams.PlayAnimation(CharacterAnimationType.move, SkillType.Attack, 0, null, null);
			base.stateManager.soundPlayer.TryPlaySE(character.CharacterParams.battleOutSE, 0f, false);
			IEnumerator transitionCount = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.battleEndBigBossActionWaitSecond, null, null);
			while (transitionCount.MoveNext())
			{
				yield return null;
			}
			character.CharacterParams.gameObject.SetActive(false);
			base.stateManager.cameraControl.PlayCameraMotionActionCharacter("skillA", character);
		}
		yield break;
	}

	public IEnumerator SmallToBigTransition(float delayTime, CharacterStateControl cState, AlwaysEffectParams insertCharacterEffect)
	{
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(delayTime, null, null);
		IEnumerator smooth = this.SmoothIncreaseCharactersAction(base.stateManager.stateProperty.revivalActionSpeed, new CharacterStateControl[]
		{
			cState
		});
		bool isPlayerd = false;
		bool isAttackPlayed = false;
		bool isEndAnimation = false;
		this.ShowAllCharactersAction(new CharacterStateControl[]
		{
			cState
		});
		this.PlayIdleAnimationActiveCharacterAction(new CharacterStateControl[]
		{
			cState
		});
		this.SmoothIncreaseCharacterInitializeAction(new CharacterStateControl[]
		{
			cState
		});
		while (wait.MoveNext())
		{
			yield return null;
		}
		insertCharacterEffect.SetPosition(cState.CharacterParams.transform, null);
		this.PlayAlwaysEffectAction(insertCharacterEffect, AlwaysEffectState.In);
		base.stateManager.soundPlayer.TryPlaySE("bt_541", 0f, false);
		for (;;)
		{
			if (insertCharacterEffect.currentState == AlwaysEffectState.Always && !isPlayerd)
			{
				this.PlayAlwaysEffectAction(insertCharacterEffect, AlwaysEffectState.Out);
				base.stateManager.soundPlayer.TryPlaySE(insertCharacterEffect, AlwaysEffectState.Out);
				isPlayerd = true;
			}
			if (isPlayerd && !smooth.MoveNext())
			{
				isEndAnimation = true;
			}
			if (!isAttackPlayed && isEndAnimation)
			{
				cState.CharacterParams.PlayAttackAnimation(SkillType.Attack, 0);
				isAttackPlayed = true;
			}
			yield return isAttackPlayed;
		}
		yield break;
	}

	public void SmallToBigTransitionAfter(CharacterStateControl[] characters, IEnumerator[] isPlayedAttackIEnumerator)
	{
		List<bool> list = new List<bool>();
		foreach (IEnumerator enumerator in isPlayedAttackIEnumerator)
		{
			list.Add((bool)enumerator.Current);
		}
		this.SmallToBigTransitionAfter(characters, list.ToArray());
	}

	private void SmallToBigTransitionAfter(CharacterStateControl[] characters, bool[] isPlayedAttack)
	{
		for (int i = 0; i < characters.Length; i++)
		{
			characters[i].CharacterParams.transform.localScale = Vector3.one;
			if (!isPlayedAttack[i])
			{
				characters[i].CharacterParams.PlayAttackAnimation(SkillType.Attack, 0);
			}
		}
	}

	private void SmoothIncreaseCharacterInitializeAction(params CharacterStateControl[] characters)
	{
		foreach (CharacterStateControl characterStateControl in characters)
		{
			characterStateControl.CharacterParams.transform.localScale = Vector3.zero;
		}
	}

	public IEnumerator SmoothIncreaseCharactersAction(float time, params CharacterStateControl[] characters)
	{
		this.SmoothIncreaseCharacterInitializeAction(characters);
		for (float current = 0f; current < time; current += Time.deltaTime)
		{
			float duration = TimeExtension.GetTimeScaleDivided(base.stateProperty.revivalActionSpeed);
			float smoothLevel = Mathf.Clamp01(Mathf.SmoothStep(0f, 1f, current) / duration);
			foreach (CharacterStateControl c in characters)
			{
				c.CharacterParams.transform.localScale = Vector3.one * smoothLevel;
			}
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	public IEnumerator SlowMotionWaitAction()
	{
		base.hierarchyData.isPossibleRetire = false;
		IEnumerator wait1st = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateProperty.lastAttackSlowMotionStartWaitSecond, null, null);
		while (wait1st.MoveNext())
		{
			object obj = wait1st.Current;
			yield return obj;
		}
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateProperty.lastAttackSlowMotionWaitSecond, true, null, null);
		base.battleStateData.isSlowMotion = true;
		bool isNotMulti = base.battleMode != BattleMode.Multi;
		base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, base.battleStateData.isShowMenuWindow && isNotMulti);
		while (wait.MoveNext())
		{
			object obj2 = wait.Current;
			yield return obj2;
		}
		base.battleStateData.isSlowMotion = false;
		base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, base.battleStateData.isShowMenuWindow && isNotMulti);
		yield break;
	}

	public void StopSlowMotionAction()
	{
		bool flag = base.battleMode != BattleMode.Multi;
		base.battleStateData.isSlowMotion = false;
		base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, base.battleStateData.isShowMenuWindow && flag);
	}

	public IEnumerator StopHitAnimation(params HitEffectParams[] currentHitEffect)
	{
		this.StopHitEffectAction(currentHitEffect);
		foreach (HitEffectParams h in currentHitEffect)
		{
			base.stateManager.soundPlayer.TryStopSE(h);
		}
		base.stateManager.soundPlayer.StopHitEffectSE();
		yield break;
	}

	public void PlayIdleAnimationUndeadCharactersAction(params CharacterStateControl[] characters)
	{
		this.PlayIdleAnimationCharactersAction(CharacterStateControl.GetAliveCharacters(characters));
	}

	public void PlayIdleAnimationCharactersAction(params CharacterStateControl[] characters)
	{
		foreach (CharacterParams characterParams in CharacterStateControl.ToParams(characters))
		{
			if (!characterParams.gameObject.activeSelf)
			{
				characterParams.gameObject.SetActive(true);
			}
			this.PlayIdleAnimationInternal(characterParams);
		}
	}

	public IEnumerator PlayGimmickAnimation(CharacterStateControl[] players, CharacterStateControl[] enemies)
	{
		List<CharacterParams> upCharacters = new List<CharacterParams>();
		List<CharacterParams> downCharacters = new List<CharacterParams>();
		if (base.battleStateData.currentWaveNumber < 1)
		{
			foreach (CharacterStateControl player in players)
			{
				int upCount = 0;
				int downCount = 0;
				player.GetDifferenceExtraPram(out upCount, out downCount);
				bool isPoint = player.IsPoint();
				if (upCount > 0 || isPoint)
				{
					upCharacters.Add(player.CharacterParams);
				}
				if (downCount > 0)
				{
					downCharacters.Add(player.CharacterParams);
				}
			}
		}
		foreach (CharacterStateControl enemy in enemies)
		{
			int upCount2 = 0;
			int downCount2 = 0;
			enemy.GetDifferenceExtraPram(out upCount2, out downCount2);
			if (upCount2 > 0)
			{
				upCharacters.Add(enemy.CharacterParams);
			}
			if (downCount2 > 0)
			{
				downCharacters.Add(enemy.CharacterParams);
			}
		}
		if (base.battleStateData.currentWaveNumber == 0 && upCharacters.Count > 0)
		{
			base.stateManager.uiControl.ShowBattleExtraEffect(BattleExtraEffectUI.AnimationType.Extra);
			yield return null;
			while (base.stateManager.uiControl.IsBattleExtraEffect())
			{
				yield return null;
			}
			base.stateManager.uiControl.HideBattleExtraEffect();
		}
		IEnumerator playUpGimmickEffect = this.PlayUpGimmickEffect(upCharacters);
		while (playUpGimmickEffect.MoveNext())
		{
			object obj = playUpGimmickEffect.Current;
			yield return obj;
		}
		if (base.battleStateData.currentWaveNumber == 0 && downCharacters.Count > 0)
		{
			base.stateManager.uiControl.ShowBattleExtraEffect(BattleExtraEffectUI.AnimationType.Stage);
			yield return null;
			while (base.stateManager.uiControl.IsBattleExtraEffect())
			{
				yield return null;
			}
			base.stateManager.uiControl.HideBattleExtraEffect();
		}
		IEnumerator playDownGimmickEffect = this.PlayDownGimmickEffect(downCharacters);
		while (playDownGimmickEffect.MoveNext())
		{
			object obj2 = playDownGimmickEffect.Current;
			yield return obj2;
		}
		yield break;
	}

	private IEnumerator PlayUpGimmickEffect(List<CharacterParams> upCharacters)
	{
		int count = 0;
		foreach (CharacterParams param in upCharacters)
		{
			base.battleStateData.stageGimmickUpEffect[count].SetPosition(param.transform, null);
			this.PlayAlwaysEffectAction(base.battleStateData.stageGimmickUpEffect[count], AlwaysEffectState.In);
			yield return new WaitForEndOfFrame();
			base.stateManager.soundPlayer.TryPlaySE(base.battleStateData.stageGimmickUpEffect[count], AlwaysEffectState.In);
			Vector3 pos = base.hierarchyData.cameraObject.camera3D.WorldToViewportPoint(base.battleStateData.stageGimmickUpEffect[count].targetPosition.position);
			base.stateManager.uiControl.PlayBattleGimmickStatusAnimator(count, pos, true);
			count++;
		}
		if (upCharacters.Count > 0)
		{
			yield return new WaitForSeconds(0.5f);
			base.StartCoroutine(this.EndStageGimmickUpEffect(0, upCharacters.Count - 1));
		}
		if (upCharacters.Count > 0)
		{
			yield return new WaitForSeconds(0.5f);
		}
		this.StopAlwaysEffectAction(base.battleStateData.stageGimmickUpEffect);
		yield break;
	}

	private IEnumerator PlayDownGimmickEffect(List<CharacterParams> downCharacters)
	{
		int count = 0;
		foreach (CharacterParams param in downCharacters)
		{
			base.battleStateData.stageGimmickDownEffect[count].SetPosition(param.transform, null);
			this.PlayAlwaysEffectAction(base.battleStateData.stageGimmickDownEffect[count], AlwaysEffectState.In);
			base.stateManager.soundPlayer.TryPlaySE(base.battleStateData.stageGimmickDownEffect[count], AlwaysEffectState.In);
			yield return new WaitForEndOfFrame();
			Vector3 pos = base.hierarchyData.cameraObject.camera3D.WorldToViewportPoint(base.battleStateData.stageGimmickDownEffect[count].targetPosition.position);
			base.stateManager.uiControl.PlayBattleGimmickStatusAnimator(count, pos, false);
			count++;
		}
		if (downCharacters.Count > 0)
		{
			yield return new WaitForSeconds(0.5f);
			base.StartCoroutine(this.EndStageGimmickUpEffect(0, downCharacters.Count - 1));
		}
		if (downCharacters.Count > 0)
		{
			yield return new WaitForSeconds(0.5f);
		}
		this.StopAlwaysEffectAction(base.battleStateData.stageGimmickUpEffect);
		yield break;
	}

	private IEnumerator EndStageGimmickUpEffect(int startIndex, int endIndex)
	{
		yield return new WaitForSeconds(0.5f);
		for (int i = startIndex; i < endIndex; i++)
		{
			if (i >= base.battleStateData.stageGimmickUpEffect.Length)
			{
				break;
			}
			this.PlayAlwaysEffectAction(base.battleStateData.stageGimmickUpEffect[i], AlwaysEffectState.Out);
		}
		yield break;
	}

	public void PlayIdleAnimationActiveCharacterAction(params CharacterStateControl[] characters)
	{
		foreach (CharacterParams characterParams in CharacterStateControl.ToParams(characters))
		{
			if (characterParams.gameObject.activeSelf)
			{
				this.PlayIdleAnimationInternal(characterParams);
			}
		}
	}

	public void PlayAnimationCharacterAction(CharacterAnimationType animationType, params CharacterStateControl[] characters)
	{
		foreach (CharacterStateControl characterStateControl in characters)
		{
			characterStateControl.CharacterParams.PlayAnimation(animationType, SkillType.Attack, 0, null, null);
		}
	}

	public void PlaySmoothAnimationCharacterAction(CharacterAnimationType animationType, params CharacterStateControl[] characters)
	{
		foreach (CharacterStateControl characterStateControl in characters)
		{
			characterStateControl.CharacterParams.PlayAnimationSmooth(animationType, SkillType.Attack, 0, null, null);
		}
	}

	public void PlayDeadAnimationCharacterAction(Action deathEffectPlay, CharacterStateControl character)
	{
		bool flag = base.stateManager.IsLastBattleAndAllDeath();
		int myIndex = character.myIndex;
		HitEffectParams hitEffectParams;
		if (character.isEnemy)
		{
			if (flag)
			{
				hitEffectParams = base.battleStateData.enemiesLastDeadEffect[myIndex];
			}
			else
			{
				hitEffectParams = base.battleStateData.enemiesDeathEffect[myIndex];
			}
		}
		else
		{
			hitEffectParams = base.battleStateData.playersDeathEffect[myIndex];
		}
		character.CharacterParams.PlayDeadAnimation(hitEffectParams, deathEffectPlay);
	}

	public void HideDeadCharactersAction(params CharacterStateControl[] characters)
	{
		foreach (CharacterStateControl characterStateControl in characters)
		{
			if (characterStateControl.isDied)
			{
				characterStateControl.CharacterParams.gameObject.SetActive(false);
			}
		}
	}

	public void ShowAllCharactersAction(params CharacterStateControl[] characters)
	{
		foreach (CharacterStateControl characterStateControl in characters)
		{
			characterStateControl.CharacterParams.gameObject.SetActive(true);
		}
	}

	public void HideAllCharactersAction(params CharacterStateControl[] characters)
	{
		foreach (CharacterStateControl characterStateControl in characters)
		{
			characterStateControl.CharacterParams.gameObject.SetActive(false);
		}
	}

	public void ShowAliveCharactersAction(params CharacterStateControl[] characters)
	{
		foreach (CharacterStateControl characterStateControl in characters)
		{
			characterStateControl.CharacterParams.gameObject.SetActive(!characterStateControl.isDied);
		}
	}

	public void HideAllPreloadEnemiesAction()
	{
		foreach (CharacterParams characterParams in base.battleStateData.preloadEnemiesParams.GetAllObject())
		{
			characterParams.gameObject.SetActive(false);
		}
	}

	public IEnumerator MotionResetAliveCharacterAction(params CharacterStateControl[] characters)
	{
		bool[] charactersActive = new bool[characters.Length];
		int index = 0;
		foreach (CharacterStateControl c in characters)
		{
			charactersActive[index] = c.CharacterParams.gameObject.activeSelf;
			c.CharacterParams.gameObject.SetActive(true);
			c.CharacterParams.StopAnimation();
			c.CharacterParams.PlayIdleAnimation();
			index++;
		}
		yield return new WaitForEndOfFrame();
		index = 0;
		foreach (CharacterStateControl c2 in characters)
		{
			c2.CharacterParams.gameObject.SetActive(charactersActive[index]);
			index++;
		}
		yield break;
	}

	public void PlayHitEffectAction(HitEffectParams hitEffects, CharacterStateControl characters)
	{
		hitEffects.PlayAnimationTrigger(characters.CharacterParams);
	}

	public void StopHitEffectAction(params HitEffectParams[] hitEffects)
	{
		foreach (HitEffectParams hitEffectParams in hitEffects)
		{
			hitEffectParams.StopAnimation();
		}
	}

	public void PlayAlwaysEffectAction(AlwaysEffectParams alwaysEffect, AlwaysEffectState state)
	{
		alwaysEffect.PlayAnimationTrigger(state);
	}

	public void StopAlwaysEffectAction(params AlwaysEffectParams[] alwaysEffect)
	{
		foreach (AlwaysEffectParams alwaysEffectParams in alwaysEffect)
		{
			if (alwaysEffectParams.isActiveAndEnabled)
			{
				alwaysEffectParams.StopAnimation();
			}
		}
	}

	public void SetPositionAlwaysEffectAction(AlwaysEffectParams alwaysEffect, CharacterStateControl character, Vector3? offsetPosition = null)
	{
		Vector3 b = (offsetPosition == null) ? Vector3.zero : offsetPosition.Value;
		alwaysEffect.SetPosition(character.CharacterParams.transform, new Vector3?(character.CharacterParams.dropItemOffsetPosition + b));
	}

	private void PlayIdleAnimationInternal(CharacterParams c)
	{
		if (!c.isActiveAnimation)
		{
			c.PlayIdleAnimation();
			return;
		}
		switch (c.currentAnimationType)
		{
		case CharacterAnimationType.dead:
		case CharacterAnimationType.win:
		case CharacterAnimationType.eat:
		case CharacterAnimationType.attacks:
			c.PlayIdleAnimation();
			return;
		}
	}
}
