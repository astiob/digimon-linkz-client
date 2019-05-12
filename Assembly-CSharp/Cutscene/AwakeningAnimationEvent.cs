using System;
using UnityEngine;

namespace Cutscene
{
	public sealed class AwakeningAnimationEvent : MonoBehaviour
	{
		private CutsceneSound sound;

		private CharacterParams charaParam;

		private bool chaseFlag;

		private bool rotateSpeedSlow;

		private void ChaseFlagStarter()
		{
			this.chaseFlag = true;
		}

		private void ChaseFlagStopper()
		{
			this.chaseFlag = false;
		}

		private void RotateSlower()
		{
			this.rotateSpeedSlow = true;
		}

		private void SoudPlayer1()
		{
			this.sound.PlaySE("SEInternal/Cutscene/se_216");
		}

		private void WinMotion()
		{
			this.charaParam.PlayAnimation(CharacterAnimationType.revival, SkillType.Attack, 0, null, null);
		}

		public void Initialize(CutsceneSound sound, CharacterParams charaParam)
		{
			this.sound = sound;
			this.charaParam = charaParam;
		}

		public bool IsPlaying()
		{
			return base.gameObject.activeSelf;
		}

		public void StartAnimation()
		{
			base.gameObject.SetActive(true);
			this.charaParam.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
		}

		public bool IsCameraChase()
		{
			return this.chaseFlag;
		}

		public bool IsCircleSlowRotate()
		{
			return this.rotateSpeedSlow;
		}
	}
}
