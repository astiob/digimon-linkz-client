using Monster;
using System;
using System.Collections;
using UnityEngine;

namespace Cutscene.Gasha
{
	public sealed class GashaGrowStepText : GashaAnimationBase, IGashaAnimation
	{
		public Camera mainCamera;

		public CameraSwitcher cameraSwitcher;

		public Animator growAnimator;

		public GameObject gashaMonster;

		public string growStep;

		private void StartAnimationGrowStepText()
		{
			if (MonsterGrowStepData.IsChild1Scope(this.growStep))
			{
				this.growAnimator.SetTrigger("Child_1");
			}
			else if (MonsterGrowStepData.IsChild2Scope(this.growStep))
			{
				this.growAnimator.SetTrigger("Child_2");
			}
			else if (MonsterGrowStepData.IsGrowingGroup(this.growStep))
			{
				this.growAnimator.SetTrigger("Growing");
			}
			else if (MonsterGrowStepData.IsRipeGroup(this.growStep))
			{
				this.growAnimator.SetTrigger("Ripe");
			}
			else if (MonsterGrowStepData.IsPerfectGroup(this.growStep))
			{
				this.growAnimator.SetTrigger("Perfect");
			}
			else if (MonsterGrowStepData.IsUltimateGroup(this.growStep))
			{
				this.growAnimator.SetTrigger("Ultimate");
			}
			else if (MonsterGrowStepData.IsArmorGroup(this.growStep))
			{
				this.growAnimator.SetTrigger("Armor");
			}
			else if (MonsterGrowStepData.IsHybridGroup(this.growStep))
			{
				this.growAnimator.SetTrigger("Hybrid");
			}
		}

		public IEnumerator StartAnimation()
		{
			this.StartAnimationGrowStepText();
			yield return new WaitForSeconds(2f);
			this.cameraSwitcher.EnableMainCamera();
			CutsceneCommon.SetBillBoardCamera(this.gashaMonster, this.mainCamera);
			CharacterParams charaParam = this.gashaMonster.GetComponent<CharacterParams>();
			charaParam.PlayAnimation(CharacterAnimationType.win, SkillType.Attack, 0, null, null);
			base.EndCallback();
			yield break;
		}

		public void SkipAnimation()
		{
			if (!this.growAnimator.GetCurrentAnimatorStateInfo(0).IsTag("GrowStep"))
			{
				this.StartAnimationGrowStepText();
			}
			this.cameraSwitcher.EnableMainCamera();
			CutsceneCommon.SetBillBoardCamera(this.gashaMonster, this.mainCamera);
			CharacterParams component = this.gashaMonster.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.win, SkillType.Attack, 0, null, null);
		}
	}
}
