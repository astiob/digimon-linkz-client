using Monster;
using System;
using System.Collections;
using UnityEngine;

namespace Cutscene.Gasha
{
	public sealed class GashaRareText : GashaAnimationBase, IGashaAnimation
	{
		public ParticleSystem circleParticleRoot;

		public Camera mainCamera;

		public Camera subCamera;

		public CameraSwitcher cameraSwitcher;

		public Color bgColorRareHigh;

		public CutsceneSound sound;

		public ParticleSystem rareGlitterParticle;

		public UITexture[] rareLow;

		public UITexture[] rareMiddle;

		public UITexture[] rareHigh;

		public GameObject gashaMonster;

		public string growStep;

		private int seHandle1;

		private int seHandle2;

		private IEnumerator animationTextImage;

		private IEnumerator StartAnimationRareTextImage(UITexture[] textImageList)
		{
			this.seHandle1 = this.sound.PlaySE("SEInternal/Cutscene/se_214");
			this.rareGlitterParticle.Play();
			textImageList[0].alpha = 1f;
			for (int i = 0; i < textImageList.Length; i++)
			{
				yield return new WaitForSeconds(0.02f);
				textImageList[i].alpha = 1f;
			}
			yield return new WaitForSeconds(1.3f);
			yield break;
		}

		public IEnumerator StartAnimation()
		{
			this.circleParticleRoot.Play();
			this.subCamera.fieldOfView = 30f;
			this.subCamera.transform.localPosition = new Vector3(2f, 1f, 0f);
			if (MonsterGrowStepData.IsUltimateScope(this.growStep))
			{
				this.mainCamera.backgroundColor = this.bgColorRareHigh;
				this.subCamera.backgroundColor = this.bgColorRareHigh;
			}
			this.gashaMonster.SetActive(true);
			CharacterParams charaParam = this.gashaMonster.GetComponent<CharacterParams>();
			charaParam.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
			CutsceneCommon.SetBillBoardCamera(this.gashaMonster, this.subCamera);
			this.cameraSwitcher.SetLookAtObject(charaParam.characterFaceCenterTarget);
			yield return new WaitForSeconds(1f);
			if (MonsterGrowStepData.IsRipeScope(this.growStep))
			{
				this.animationTextImage = this.StartAnimationRareTextImage(this.rareLow);
				yield return AppCoroutine.Start(this.animationTextImage, false);
			}
			else if (MonsterGrowStepData.IsPerfectScope(this.growStep))
			{
				this.animationTextImage = this.StartAnimationRareTextImage(this.rareMiddle);
				yield return AppCoroutine.Start(this.animationTextImage, false);
			}
			else if (MonsterGrowStepData.IsUltimateScope(this.growStep))
			{
				this.animationTextImage = this.StartAnimationRareTextImage(this.rareHigh);
				yield return AppCoroutine.Start(this.animationTextImage, false);
			}
			this.seHandle2 = this.sound.PlaySE("SEInternal/Cutscene/se_214");
			GashaAnimationCommon.SetTextureAlpha(this.rareLow, 0f);
			GashaAnimationCommon.SetTextureAlpha(this.rareMiddle, 0f);
			GashaAnimationCommon.SetTextureAlpha(this.rareHigh, 0f);
			base.EndCallback();
			yield break;
		}

		public void SkipAnimation()
		{
			AppCoroutine.Stop(this.animationTextImage, false);
			GashaAnimationCommon.PlayParticle(this.circleParticleRoot);
			this.subCamera.fieldOfView = 30f;
			this.subCamera.transform.localPosition = new Vector3(2f, 1f, 0f);
			if (MonsterGrowStepData.IsUltimateScope(this.growStep))
			{
				this.mainCamera.backgroundColor = this.bgColorRareHigh;
				this.subCamera.backgroundColor = this.bgColorRareHigh;
			}
			CharacterParams component = this.gashaMonster.GetComponent<CharacterParams>();
			if (!this.gashaMonster.activeSelf)
			{
				this.gashaMonster.SetActive(true);
				component.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
				CutsceneCommon.SetBillBoardCamera(this.gashaMonster, this.subCamera);
			}
			this.cameraSwitcher.SetLookAtObject(component.characterFaceCenterTarget);
			if (!SoundMng.Instance().IsPlayingSE_Ex(this.seHandle1) && !SoundMng.Instance().IsPlayingSE_Ex(this.seHandle2))
			{
				this.sound.PlaySE("SEInternal/Cutscene/se_214");
			}
			GashaAnimationCommon.SetTextureAlpha(this.rareLow, 0f);
			GashaAnimationCommon.SetTextureAlpha(this.rareMiddle, 0f);
			GashaAnimationCommon.SetTextureAlpha(this.rareHigh, 0f);
		}
	}
}
