using Enemy.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityExtension;

public class BattleTargetSelect : BattleFunctionBase
{
	private const int AutoPlayApSieveThreshold = 2;

	private int _currentTargettingCharacter_Enemy;

	private int _currentTargettingCharacter_Ally;

	public void AutoPlayCharacterAndSkillSelectFunction(CharacterStateControl currentCharacter)
	{
		if (currentCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SkillLock))
		{
			currentCharacter.isSelectSkill = 0;
		}
		else
		{
			SkillStatus[] removedAttackSkillStatus = currentCharacter.GetRemovedAttackSkillStatus(base.stateManager.publicAttackSkillId);
			SkillStatus skillStatus = currentCharacter.skillStatus[currentCharacter.SkillIdToIndexOf(base.stateManager.publicAttackSkillId)];
			foreach (SkillStatus skillStatus2 in removedAttackSkillStatus)
			{
				if (skillStatus2.skillType == SkillType.Deathblow && skillStatus2.needAp <= currentCharacter.ap)
				{
					skillStatus = skillStatus2;
					break;
				}
			}
			for (int j = 0; j < currentCharacter.skillStatus.Length; j++)
			{
				if (currentCharacter.skillStatus[j] == skillStatus)
				{
					currentCharacter.isSelectSkill = j;
					break;
				}
			}
		}
		CharacterStateControl[] characterStatus = null;
		switch (currentCharacter.currentSkillStatus.target)
		{
		case EffectTarget.Enemy:
			characterStatus = base.battleStateData.enemies;
			break;
		case EffectTarget.Ally:
			characterStatus = base.battleStateData.playerCharacters;
			break;
		case EffectTarget.Attacker:
			characterStatus = new CharacterStateControl[]
			{
				currentCharacter
			};
			break;
		case EffectTarget.EnemyWithoutAttacker:
			characterStatus = base.battleStateData.enemies.Where((CharacterStateControl item) => item.myIndex != currentCharacter.myIndex).ToArray<CharacterStateControl>();
			break;
		case EffectTarget.AllyWithoutAttacker:
			characterStatus = base.battleStateData.playerCharacters.Where((CharacterStateControl item) => item.myIndex != currentCharacter.myIndex).ToArray<CharacterStateControl>();
			break;
		}
		CharacterStateControl[] aliveCharacters = CharacterStateControl.GetAliveCharacters(characterStatus);
		CharacterStateControl[] array2 = CharacterStateControlSorter.SortedTargetSelect(aliveCharacters, currentCharacter.currentSkillStatus, null);
		currentCharacter.targetCharacter = array2[0];
	}

	public void AutoPlayCharacterAndAttackSelectFunction(CharacterStateControl currentCharacter)
	{
		currentCharacter.isSelectSkill = 0;
		CharacterStateControl[] aliveCharacters = CharacterStateControl.GetAliveCharacters(base.battleStateData.enemies);
		CharacterStateControl[] array = CharacterStateControlSorter.SortedTargetSelect(aliveCharacters, currentCharacter.currentSkillStatus, null);
		currentCharacter.targetCharacter = array[0];
	}

	public void EnemyAICharacterAndSkillSelectFunction(CharacterStateControl currentCharacters)
	{
		EnemyAIPattern enemyAiPattern = currentCharacters.enemyStatus.enemyAiPattern;
		AIActionPattern currentActionPattern = enemyAiPattern.GetCurrentActionPattern(currentCharacters, base.battleStateData.currentRoundNumber - 1);
		AIActionClip randomActionClip = currentActionPattern.GetRandomActionClip();
		int isSelectSkill = currentCharacters.SkillIdToIndexOf(base.stateManager.publicAttackSkillId);
		if (!currentCharacters.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SkillLock))
		{
			int num = currentCharacters.SkillIdToIndexOf(randomActionClip.useSkillId);
			if (!currentCharacters.isApShortness(num))
			{
				isSelectSkill = num;
			}
		}
		currentCharacters.isSelectSkill = isSelectSkill;
		CharacterStateControl[] array = this.GetSkillTargetList(currentCharacters, currentCharacters.currentSkillStatus.target);
		if (!randomActionClip.IsFindMachConditionTargets(array))
		{
			currentCharacters.isSelectSkill = currentCharacters.SkillIdToIndexOf(base.stateManager.publicAttackSkillId);
			array = base.battleStateData.playerCharacters;
		}
		CharacterStateControl[] aliveCharacters = CharacterStateControl.GetAliveCharacters(array);
		CharacterStateControl[] array2 = CharacterStateControlSorter.SortedTargetSelect(aliveCharacters, currentCharacters.currentSkillStatus, randomActionClip);
		currentCharacters.targetCharacter = array2[0];
	}

	public void TargetManualSelectAndApplyUIFunction(CharacterStateControl currentCharacters = null)
	{
		if (currentCharacters != null && currentCharacters.IsSelectedSkill)
		{
			CharacterStateControl targetCharacter = null;
			if (base.battleStateData.isPossibleTargetSelect && Input.GetKeyUp(KeyCode.Mouse0) && BoolExtension.AllMachValue(false, new bool[]
			{
				base.battleStateData.isShowMenuWindow,
				base.battleStateData.isShowRevivalWindow
			}) && !base.stateManager.uiControl.GetIsClickedUI())
			{
				Ray ray = base.hierarchyData.cameraObject.camera3D.ScreenPointToRay(Input.mousePosition);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, base.battleStateData.characterColliderLayerMask))
				{
					CharacterParams component = raycastHit.collider.gameObject.GetComponent<CharacterParams>();
					CharacterStateControl[] totalCharacters = base.battleStateData.GetTotalCharacters();
					foreach (CharacterStateControl characterStateControl in totalCharacters)
					{
						if (characterStateControl.CharacterParams == component)
						{
							targetCharacter = characterStateControl;
							break;
						}
					}
				}
			}
			this.SetTarget(currentCharacters, targetCharacter);
		}
		else
		{
			this.AllHideTargetIcon();
		}
	}

	public void AllHideTargetIcon()
	{
		for (int i = 0; i < base.battleStateData.maxCharacterLength; i++)
		{
			base.stateManager.uiControl.ApplyManualSelectTarget(i, false, Strength.None);
			base.stateManager.uiControl.ApplyTargetToleranceIcon(i, false, Strength.None);
		}
	}

	public CharacterStateControl[] GetSkillTargetList(CharacterStateControl attacker, EffectTarget effectTarget)
	{
		List<CharacterStateControl> list = new List<CharacterStateControl>();
		CharacterStateControl[] array;
		CharacterStateControl[] array2;
		if (attacker.isEnemy)
		{
			array = base.battleStateData.playerCharacters;
			array2 = base.battleStateData.enemies;
		}
		else
		{
			array2 = base.battleStateData.playerCharacters;
			array = base.battleStateData.enemies;
		}
		switch (effectTarget)
		{
		case EffectTarget.Enemy:
			foreach (CharacterStateControl characterStateControl in array)
			{
				if (!characterStateControl.isDied)
				{
					list.Add(characterStateControl);
				}
			}
			break;
		case EffectTarget.Ally:
			foreach (CharacterStateControl characterStateControl2 in array2)
			{
				if (!characterStateControl2.isDied)
				{
					list.Add(characterStateControl2);
				}
			}
			break;
		case EffectTarget.Attacker:
			if (!attacker.isDied)
			{
				list.Add(attacker);
			}
			break;
		case EffectTarget.EnemyWithoutAttacker:
			foreach (CharacterStateControl characterStateControl3 in array)
			{
				if (!characterStateControl3.isDied && attacker != characterStateControl3)
				{
					list.Add(characterStateControl3);
				}
			}
			break;
		case EffectTarget.AllyWithoutAttacker:
			foreach (CharacterStateControl characterStateControl4 in array2)
			{
				if (!characterStateControl4.isDied && attacker != characterStateControl4)
				{
					list.Add(characterStateControl4);
				}
			}
			break;
		}
		return list.ToArray();
	}

	public void SetTarget(CharacterStateControl currentCharacter, CharacterStateControl targetCharacter = null)
	{
		CharacterStateControl characterStateControl = targetCharacter;
		bool flag = characterStateControl == null || characterStateControl.isDied;
		SkillStatus currentSkillStatus = currentCharacter.currentSkillStatus;
		if (currentSkillStatus == null)
		{
			return;
		}
		CharacterStateControl[] skillTargetList = this.GetSkillTargetList(currentCharacter, currentSkillStatus.target);
		bool flag2 = true;
		if (characterStateControl != null)
		{
			foreach (CharacterStateControl a in skillTargetList)
			{
				if (a == characterStateControl)
				{
					flag2 = false;
					break;
				}
			}
		}
		if (flag || flag2)
		{
			int currentTargettingCharacter = this.GetCurrentTargettingCharacter(currentSkillStatus.target);
			foreach (CharacterStateControl characterStateControl2 in skillTargetList)
			{
				if (characterStateControl2.myIndex == currentTargettingCharacter)
				{
					characterStateControl = characterStateControl2;
					break;
				}
			}
			if (characterStateControl == null)
			{
				characterStateControl = skillTargetList[0];
			}
		}
		List<CharacterStateControl> list = new List<CharacterStateControl>();
		EffectNumbers numbers = currentSkillStatus.numbers;
		if (numbers != EffectNumbers.Simple)
		{
			if (numbers == EffectNumbers.All)
			{
				list.AddRange(skillTargetList);
			}
		}
		else
		{
			list.Add(characterStateControl);
		}
		for (int k = 0; k < base.battleStateData.maxCharacterLength; k++)
		{
			if (k < list.Count)
			{
				this.ShowTargetIcon(k, list[k], currentSkillStatus);
			}
			else
			{
				this.HideTargetIcon(k);
			}
		}
		currentCharacter.targetCharacter = characterStateControl;
		this.SetCurrentTargettingCharacter(currentCharacter.currentSkillStatus.target, currentCharacter.targetCharacter.myIndex);
		BattleStateManager.current.cameraControl.PlayTweenCameraMotion(base.battleStateData.commandSelectTweenTargetCamera, currentCharacter.targetCharacter);
	}

	private void ShowTargetIcon(int index, CharacterStateControl target, SkillStatus skillStatus)
	{
		base.stateManager.uiControl.SetManualSelectTarget(index, true, target, skillStatus);
		base.stateManager.uiControl.SetTargetToleranceIcon(index, true, target, skillStatus);
		base.stateManager.uiControl.SetManualSelectTargetReposition(index, target);
		base.stateManager.uiControl.SetTargetToleranceIconReposition(index, target);
	}

	private void HideTargetIcon(int index)
	{
		base.stateManager.uiControl.SetManualSelectTarget(index, false, null, null);
		base.stateManager.uiControl.SetTargetToleranceIcon(index, false, null, null);
	}

	private int GetCurrentTargettingCharacter(EffectTarget target)
	{
		switch (target)
		{
		case EffectTarget.Enemy:
		case EffectTarget.EnemyWithoutAttacker:
			return this._currentTargettingCharacter_Enemy;
		case EffectTarget.Ally:
		case EffectTarget.AllyWithoutAttacker:
			return this._currentTargettingCharacter_Ally;
		case EffectTarget.Attacker:
			return base.battleStateData.currentSelectCharacterIndex;
		default:
			return base.battleStateData.currentSelectCharacterIndex;
		}
	}

	private void SetCurrentTargettingCharacter(EffectTarget target, int num)
	{
		switch (target)
		{
		case EffectTarget.Enemy:
		case EffectTarget.EnemyWithoutAttacker:
			this._currentTargettingCharacter_Enemy = num;
			break;
		case EffectTarget.Ally:
		case EffectTarget.AllyWithoutAttacker:
			this._currentTargettingCharacter_Ally = num;
			break;
		}
	}
}
