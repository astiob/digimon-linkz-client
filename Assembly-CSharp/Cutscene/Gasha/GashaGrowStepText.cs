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

		public ParticleSystem rareGlitterParticle;

		public UITexture[] textImageChild;

		public UITexture[] textImageGrowing;

		public UITexture[] textImageRipe;

		public UITexture[] textImagePerfect;

		public UITexture[] textImageUltimate;

		public UITexture[] textImageArmor;

		public UITexture[] textImageHybrid;

		public GameObject gashaMonster;

		public string growStep;

		private IEnumerator animationTextImage;

		private static readonly Vector3 INITIAL_TEXT_IMAGE_SCALE = new Vector3(5f, 5f, 1f);

		private static readonly Hashtable START_HASH_TABLE = iTween.Hash(new object[]
		{
			"x",
			0.5f,
			"y",
			0.5f,
			"easetype",
			"easeOutQuart",
			"time",
			0.1f,
			"delay",
			0.2f
		});

		private static readonly Hashtable FINISH_HASH_TABLE = iTween.Hash(new object[]
		{
			"x",
			1f,
			"y",
			1f,
			"easetype",
			"easeOutQuart",
			"time",
			0f,
			"delay",
			0.2f
		});

		private void SetAnimationGrowStepTextImage(UITexture textImage)
		{
			textImage.alpha = 1f;
			textImage.gameObject.transform.localScale = GashaGrowStepText.INITIAL_TEXT_IMAGE_SCALE;
			iTween.ScaleTo(textImage.gameObject, GashaGrowStepText.START_HASH_TABLE);
			iTween.ScaleTo(textImage.gameObject, GashaGrowStepText.FINISH_HASH_TABLE);
		}

		private IEnumerator StartAnimationGrowStepTextImage(UITexture[] textImageList)
		{
			this.rareGlitterParticle.Play();
			for (int i = 0; i < textImageList.Length; i++)
			{
				this.SetAnimationGrowStepTextImage(textImageList[i]);
				if (i < textImageList.Length - 1)
				{
					yield return new WaitForSeconds(0.45f);
				}
			}
			yield break;
		}

		public IEnumerator StartAnimation()
		{
			if (MonsterGrowStepData.IsChildScope(this.growStep))
			{
				this.textImageChild[0].alpha = 1f;
				yield return new WaitForSeconds(0.02f);
				this.textImageChild[1].alpha = 1f;
				yield return new WaitForSeconds(0.02f);
				this.textImageChild[2].alpha = 1f;
				yield return new WaitForSeconds(0.02f);
				if (MonsterGrowStepData.IsChild1Scope(this.growStep))
				{
					this.textImageChild[3].alpha = 1f;
				}
				else
				{
					this.textImageChild[4].alpha = 1f;
				}
			}
			else if (MonsterGrowStepData.IsGrowingGroup(this.growStep))
			{
				this.textImageGrowing[0].alpha = 1f;
				yield return new WaitForSeconds(0.02f);
				this.textImageGrowing[1].alpha = 1f;
				yield return new WaitForSeconds(0.02f);
				this.textImageGrowing[2].alpha = 1f;
			}
			else if (MonsterGrowStepData.IsRipeGroup(this.growStep))
			{
				this.animationTextImage = this.StartAnimationGrowStepTextImage(this.textImageRipe);
				yield return AppCoroutine.Start(this.animationTextImage, false);
			}
			else if (MonsterGrowStepData.IsPerfectGroup(this.growStep))
			{
				this.animationTextImage = this.StartAnimationGrowStepTextImage(this.textImagePerfect);
				yield return AppCoroutine.Start(this.animationTextImage, false);
			}
			else if (MonsterGrowStepData.IsUltimateGroup(this.growStep))
			{
				this.animationTextImage = this.StartAnimationGrowStepTextImage(this.textImageUltimate);
				yield return AppCoroutine.Start(this.animationTextImage, false);
			}
			else if (MonsterGrowStepData.IsArmorGroup(this.growStep))
			{
				this.animationTextImage = this.StartAnimationGrowStepTextImage(this.textImageArmor);
				yield return AppCoroutine.Start(this.animationTextImage, false);
			}
			else if (MonsterGrowStepData.IsHybridGroup(this.growStep))
			{
				this.animationTextImage = this.StartAnimationGrowStepTextImage(this.textImageHybrid);
				yield return AppCoroutine.Start(this.animationTextImage, false);
			}
			this.cameraSwitcher.EnableMainCamera();
			CutsceneCommon.SetBillBoardCamera(this.gashaMonster, this.mainCamera);
			CharacterParams charaParam = this.gashaMonster.GetComponent<CharacterParams>();
			charaParam.PlayAnimation(CharacterAnimationType.win, SkillType.Attack, 0, null, null);
			base.EndCallback();
			yield break;
		}

		public void SkipAnimation()
		{
			AppCoroutine.Stop(this.animationTextImage, false);
			if (MonsterGrowStepData.IsChildScope(this.growStep))
			{
				GashaAnimationCommon.SetTextureAlpha(this.textImageChild, 1f);
			}
			else if (MonsterGrowStepData.IsGrowingGroup(this.growStep))
			{
				GashaAnimationCommon.SetTextureAlpha(this.textImageGrowing, 1f);
			}
			else if (MonsterGrowStepData.IsRipeGroup(this.growStep))
			{
				GashaAnimationCommon.SetTextureAlpha(this.textImageRipe, 1f);
			}
			else if (MonsterGrowStepData.IsPerfectGroup(this.growStep))
			{
				GashaAnimationCommon.SetTextureAlpha(this.textImagePerfect, 1f);
			}
			else if (MonsterGrowStepData.IsUltimateGroup(this.growStep))
			{
				GashaAnimationCommon.SetTextureAlpha(this.textImageUltimate, 1f);
			}
			else if (MonsterGrowStepData.IsArmorGroup(this.growStep))
			{
				GashaAnimationCommon.SetTextureAlpha(this.textImageArmor, 1f);
			}
			else if (MonsterGrowStepData.IsHybridGroup(this.growStep))
			{
				GashaAnimationCommon.SetTextureAlpha(this.textImageHybrid, 1f);
			}
			this.cameraSwitcher.EnableMainCamera();
			CutsceneCommon.SetBillBoardCamera(this.gashaMonster, this.mainCamera);
			CharacterParams component = this.gashaMonster.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.win, SkillType.Attack, 0, null, null);
		}
	}
}
