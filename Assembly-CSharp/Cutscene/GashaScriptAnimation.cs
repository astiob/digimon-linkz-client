using Cutscene.Gasha;
using Cutscene.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscene
{
	public sealed class GashaScriptAnimation : MonoBehaviour
	{
		[SerializeField]
		private Camera mainCamera;

		[SerializeField]
		private CameraSwitcher cameraSwitcher;

		[SerializeField]
		private ParticleSystem rareSignParticleSpark;

		[SerializeField]
		private ParticleSystem rareSignParticleRect;

		[SerializeField]
		private GameObject rareSignRainbowBox;

		[SerializeField]
		private ParticleSystem circleParticleAppear;

		[SerializeField]
		private ParticleSystem circleParticleRoot;

		[SerializeField]
		private ParticleSystem circleParticleRect;

		[SerializeField]
		private ParticleSystem circleParticleRectFill;

		[SerializeField]
		private ParticleSystem shockWaveparticle;

		[SerializeField]
		private ParticleSystem auraParticleBlue;

		[SerializeField]
		private ParticleSystem auraParticleYellow;

		[SerializeField]
		private ParticleSystem auraParticleRed;

		[SerializeField]
		private ParticleSystem rareGlitterParticle;

		[SerializeField]
		[Header("成長帯の文字画像一覧")]
		private UITexture[] textImageChild;

		[SerializeField]
		private UITexture[] textImageGrowing;

		[SerializeField]
		private UITexture[] textImageRipe;

		[SerializeField]
		private UITexture[] textImagePerfect;

		[SerializeField]
		private UITexture[] textImageUltimate;

		[SerializeField]
		private UITexture[] textImageArmor;

		[SerializeField]
		private UITexture[] textImageHybrid;

		[SerializeField]
		[Header("レアリティの文字画像一覧")]
		private UITexture[] rareLow;

		[SerializeField]
		private UITexture[] rareMiddle;

		[SerializeField]
		private UITexture[] rareHigh;

		[SerializeField]
		private GashaNextButton gashaNextButton;

		[SerializeField]
		private GameObject tapScreenButton;

		[SerializeField]
		private GameObject allSkipButton;

		[SerializeField]
		private Color bgColorRareNone;

		[SerializeField]
		private Color bgColorRareLow;

		[SerializeField]
		private Color bgColorRareMiddle;

		[SerializeField]
		private Color bgColorRareHigh;

		[SerializeField]
		private Animator rareSignAnimation;

		[SerializeField]
		private Animator subCameraAnimation;

		private string[] modelIdList;

		private string[] growStepList;

		private CutsceneSound sound;

		private Camera subCamera;

		private Transform rareSignParticle;

		private int resultIndex;

		private GameObject gashaMonster;

		private GashaZeroFrame zeroFrame;

		private GashaRareSignFirst rareSignFirst;

		private GashaRareSign rareSign;

		private GashaRareSignFallFirst signFallFirst;

		private GashaRareSignFall signFall;

		private GashaRareText rareText;

		private GashaGrowStepText growStepText;

		private List<IGashaAnimation> animationList = new List<IGashaAnimation>();

		private Coroutine animationCoroutine;

		private void OnEnable()
		{
			this.rareSignParticle.localPosition = new Vector3(0f, 16f, 0f);
			this.StartAnimation(this.resultIndex);
		}

		private void NextAnimation()
		{
			this.DeleteAnimation();
			this.rareSignAnimation.SetTrigger("start");
			this.subCameraAnimation.SetTrigger("start");
			this.rareSignParticle.localPosition = new Vector3(0f, 2f, 0f);
			this.StartAnimation(this.resultIndex);
			this.gashaNextButton.HideSprite();
			this.gashaNextButton.RemoveAction(new Action(this.NextAnimation));
			this.gashaNextButton.AddAction(new Action(this.SkipAnimation));
		}

		private void StartAnimation(int index)
		{
			this.gashaMonster = this.CreateMonster(this.modelIdList[index]);
			if (index == 0)
			{
				this.animationList.Add(this.rareSignFirst);
				this.animationList.Add(this.signFallFirst);
				this.signFallFirst.growStep = this.growStepList[index];
			}
			else
			{
				this.animationList.Add(this.rareSign);
				this.animationList.Add(this.signFall);
				this.signFall.growStep = this.growStepList[index];
			}
			this.animationList.Add(this.rareText);
			this.rareText.growStep = this.growStepList[index];
			this.rareText.gashaMonster = this.gashaMonster;
			this.animationList.Add(this.growStepText);
			this.growStepText.growStep = this.growStepList[index];
			this.growStepText.gashaMonster = this.gashaMonster;
			this.zeroFrame.StartAnimation(this.growStepList[index]);
		}

		private GameObject CreateMonster(string modelId)
		{
			GameObject gameObject = CutsceneCommon.LoadMonsterModel(base.transform, modelId);
			gameObject.transform.localPosition = Vector3.zero;
			CharacterParams component = gameObject.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
			gameObject.SetActive(false);
			return gameObject;
		}

		private void StartNextAnimation(GashaAnimationBase nowAnimation)
		{
			IGashaAnimation gashaAnimation = nowAnimation as IGashaAnimation;
			if (gashaAnimation != null)
			{
				this.animationList.Remove(gashaAnimation);
			}
			if (0 < this.animationList.Count)
			{
				GashaAnimationBase gashaAnimationBase = this.animationList[0] as GashaAnimationBase;
				gashaAnimationBase.SetEndCallback(new Action<GashaAnimationBase>(this.StartNextAnimation));
				this.animationCoroutine = base.StartCoroutine(this.animationList[0].StartAnimation());
			}
			else
			{
				this.EndAnimation();
			}
		}

		private void SkipAnimation()
		{
			base.StopCoroutine(this.animationCoroutine);
			this.animationCoroutine = null;
			for (int i = 0; i < this.animationList.Count; i++)
			{
				GashaAnimationBase gashaAnimationBase = this.animationList[i] as GashaAnimationBase;
				gashaAnimationBase.ClearEndCallback();
				this.animationList[i].SkipAnimation();
			}
			this.EndAnimation();
		}

		private void EndAnimation()
		{
			this.animationList.Clear();
			this.resultIndex++;
			this.SetTapScreenButton();
		}

		private void SetTapScreenButton()
		{
			if (this.resultIndex < this.modelIdList.Length)
			{
				this.gashaNextButton.ShowSprite();
				this.gashaNextButton.RemoveAction(new Action(this.SkipAnimation));
				this.gashaNextButton.AddAction(new Action(this.NextAnimation));
			}
			else
			{
				this.allSkipButton.SetActive(false);
				this.gashaNextButton.gameObject.SetActive(false);
				this.tapScreenButton.SetActive(true);
			}
		}

		private void DeleteAnimation()
		{
			UnityEngine.Object.Destroy(this.gashaMonster);
			GashaAnimationCommon.StopParticle(this.rareGlitterParticle);
			GashaAnimationCommon.StopParticle(this.circleParticleAppear);
			GashaAnimationCommon.StopParticle(this.circleParticleRectFill);
			GashaAnimationCommon.StopParticle(this.circleParticleRect);
			GashaAnimationCommon.StopParticle(this.circleParticleRoot);
			GashaAnimationCommon.StopParticle(this.shockWaveparticle);
			GashaAnimationCommon.SetTextureAlpha(this.textImageChild, 0f);
			GashaAnimationCommon.SetTextureAlpha(this.textImageGrowing, 0f);
			GashaAnimationCommon.SetTextureAlpha(this.textImageHybrid, 0f);
			GashaAnimationCommon.SetTextureAlpha(this.textImageRipe, 0f);
			GashaAnimationCommon.SetTextureAlpha(this.textImagePerfect, 0f);
			GashaAnimationCommon.SetTextureAlpha(this.textImageUltimate, 0f);
			GashaAnimationCommon.SetTextureAlpha(this.textImageArmor, 0f);
		}

		public void Initialize(CutsceneSound sound, Camera subCamera, Transform rareSignParticle, string[] modelIdList, string[] growStepList)
		{
			this.sound = sound;
			this.subCamera = subCamera;
			this.rareSignParticle = rareSignParticle;
			this.modelIdList = modelIdList;
			this.growStepList = growStepList;
			this.gashaNextButton.Initialize();
			this.gashaNextButton.AddAction(new Action(this.SkipAnimation));
			this.cameraSwitcher.SetSubCameraAnchor(base.transform.position);
			this.zeroFrame = new GashaZeroFrame
			{
				mainCamera = this.mainCamera,
				subCamera = this.subCamera,
				cameraSwitcher = this.cameraSwitcher,
				rareSignParticleSpark = this.rareSignParticleSpark,
				rareSignRainbowBox = this.rareSignRainbowBox,
				bgColorRareNone = this.bgColorRareNone,
				bgColorRareLow = this.bgColorRareLow,
				bgColorRareMiddle = this.bgColorRareMiddle,
				rareSignParticle = this.rareSignParticle
			};
			this.zeroFrame.SetEndCallback(new Action<GashaAnimationBase>(this.StartNextAnimation));
			this.rareSignFirst = new GashaRareSignFirst
			{
				rareSignParticleRect = this.rareSignParticleRect,
				rareSignParticleSpark = this.rareSignParticleSpark
			};
			this.rareSign = new GashaRareSign
			{
				rareSignParticleSpark = this.rareSignParticleSpark
			};
			this.signFallFirst = new GashaRareSignFallFirst
			{
				rareSignParticleSpark = this.rareSignParticleSpark,
				rareSignAnimation = this.rareSignAnimation,
				subCameraAnimation = this.subCameraAnimation,
				sound = this.sound,
				shockWaveparticle = this.shockWaveparticle,
				circleParticleAppear = this.circleParticleAppear,
				auraParticleBlue = this.auraParticleBlue,
				auraParticleYellow = this.auraParticleYellow,
				auraParticleRed = this.auraParticleRed
			};
			this.signFall = new GashaRareSignFall
			{
				rareSignParticleSpark = this.rareSignParticleSpark,
				rareSignAnimation = this.rareSignAnimation,
				subCameraAnimation = this.subCameraAnimation,
				sound = this.sound,
				shockWaveparticle = this.shockWaveparticle,
				circleParticleAppear = this.circleParticleAppear,
				auraParticleBlue = this.auraParticleBlue,
				auraParticleYellow = this.auraParticleYellow,
				auraParticleRed = this.auraParticleRed
			};
			this.rareText = new GashaRareText
			{
				circleParticleRoot = this.circleParticleRoot,
				mainCamera = this.mainCamera,
				subCamera = this.subCamera,
				cameraSwitcher = this.cameraSwitcher,
				bgColorRareHigh = this.bgColorRareHigh,
				sound = this.sound,
				rareGlitterParticle = this.rareGlitterParticle,
				rareLow = this.rareLow,
				rareMiddle = this.rareMiddle,
				rareHigh = this.rareHigh
			};
			this.growStepText = new GashaGrowStepText
			{
				mainCamera = this.mainCamera,
				cameraSwitcher = this.cameraSwitcher,
				rareGlitterParticle = this.rareGlitterParticle,
				textImageChild = this.textImageChild,
				textImageGrowing = this.textImageGrowing,
				textImageRipe = this.textImageRipe,
				textImagePerfect = this.textImagePerfect,
				textImageUltimate = this.textImageUltimate,
				textImageArmor = this.textImageArmor,
				textImageHybrid = this.textImageHybrid
			};
		}

		public bool IsPlaying()
		{
			return base.gameObject.activeSelf;
		}

		public void StartAnimation()
		{
			base.gameObject.SetActive(true);
		}

		public void StopAnimation()
		{
			if (this.animationCoroutine != null)
			{
				base.StopCoroutine(this.animationCoroutine);
				this.animationCoroutine = null;
			}
		}
	}
}
