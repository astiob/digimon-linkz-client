using System;
using UnityEngine;

namespace Cutscene
{
	public sealed class MedalInheritAnimationEvent : MonoBehaviour
	{
		private CutsceneSound sound;

		private CharacterParams baseMonsterCharaParam;

		private CharacterParams materialMonsterCharaParam;

		private bool chaseFlag;

		private void ChaseFlagStarter()
		{
			this.chaseFlag = true;
		}

		private void ChaseFlagStopper()
		{
			this.chaseFlag = false;
		}

		private void MotionCallerOfAttack()
		{
			this.baseMonsterCharaParam.PlayAnimation(CharacterAnimationType.attacks, SkillType.Attack, 0, null, null);
		}

		private void SoudPlayer1()
		{
			this.sound.PlaySE("SEInternal/Cutscene/bt_605");
		}

		private void SoudPlayer2()
		{
			this.sound.PlaySE("SEInternal/Cutscene/bt_504");
		}

		private void SoudPlayer3()
		{
			this.sound.PlaySE("SEInternal/Cutscene/bt_530");
		}

		public void Initialize(CutsceneSound sound, CharacterParams baseMonster, CharacterParams materialMonster)
		{
			this.sound = sound;
			this.baseMonsterCharaParam = baseMonster;
			this.materialMonsterCharaParam = materialMonster;
		}

		public bool IsPlaying()
		{
			return base.gameObject.activeSelf;
		}

		public void StartAnimation()
		{
			base.gameObject.SetActive(true);
			this.baseMonsterCharaParam.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
			this.materialMonsterCharaParam.PlayAnimation(CharacterAnimationType.move, SkillType.Attack, 0, null, null);
		}

		public bool IsCameraChase()
		{
			return this.chaseFlag;
		}
	}
}
