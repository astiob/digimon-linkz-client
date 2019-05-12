using Monster.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscene
{
	public sealed class FusionScriptAnimation : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem arousalGlitter;

		[SerializeField]
		private Animator subCameraAnimator;

		[SerializeField]
		private CameraSwitcher cameraSwitcher;

		[SerializeField]
		private ParticleSystem circleParticle;

		[SerializeField]
		private ParticleSystem auraParticle;

		[SerializeField]
		private UITexture[] arousalTexture;

		[SerializeField]
		private ParticleSystem stretchLightParticle;

		[SerializeField]
		private GameObject tapScreenButton;

		private Material wireMaterial;

		private CutsceneSound sound;

		private GameObject baseMonster;

		private GameObject materialMonster;

		private GameObject digitama;

		private bool rareUp;

		private GameObject allSkipButton;

		private List<Material[]> baseMonsterMaterialList;

		private List<Material[]> materialMonsterMaterialList;

		private Coroutine animationCoroutine;

		private void OnEnable()
		{
			this.Init();
			this.animationCoroutine = base.StartCoroutine(this.CutA());
		}

		private void Init()
		{
			this.arousalGlitter.Clear();
			this.arousalGlitter.Stop();
			this.subCameraAnimator.SetBool("move", true);
		}

		private IEnumerator CutA()
		{
			this.circleParticle.Play();
			yield return new WaitForSeconds(0.2f);
			this.sound.PlaySE("SEInternal/Cutscene/se_221");
			yield return new WaitForSeconds(1.5f);
			this.baseMonsterMaterialList = CutsceneCommon.GetMaterial(this.baseMonster);
			this.materialMonsterMaterialList = CutsceneCommon.GetMaterial(this.materialMonster);
			CutsceneCommon.ChangeMaterial(this.baseMonster, this.wireMaterial);
			CutsceneCommon.ChangeMaterial(this.materialMonster, this.wireMaterial);
			yield return new WaitForSeconds(1.2f);
			CutsceneCommon.SetWireFrameRendering(this.baseMonster);
			CutsceneCommon.SetWireFrameRendering(this.materialMonster);
			yield return new WaitForSeconds(0.5f);
			iTween.MoveTo(this.baseMonster, iTween.Hash(new object[]
			{
				"x",
				0f,
				"time",
				2.2f,
				"islocal",
				true
			}));
			iTween.MoveTo(this.materialMonster, iTween.Hash(new object[]
			{
				"x",
				0f,
				"time",
				2.2f,
				"islocal",
				true
			}));
			yield return new WaitForSeconds(0.8f);
			this.auraParticle.Play();
			yield return new WaitForSeconds(0.2f);
			this.cameraSwitcher.EnableMainCamera();
			CutsceneCommon.ResetRendering(this.baseMonster, this.baseMonsterMaterialList);
			CutsceneCommon.ResetRendering(this.materialMonster, this.materialMonsterMaterialList);
			this.baseMonster.SetActive(false);
			this.materialMonster.SetActive(false);
			yield return new WaitForSeconds(2f);
			this.digitama.SetActive(true);
			if (this.rareUp)
			{
				yield return new WaitForSeconds(2f);
				this.sound.PlaySE("SEInternal/Cutscene/se_214");
				this.arousalTexture[0].alpha = 1f;
				iTween.ScaleTo(this.arousalTexture[0].gameObject, iTween.Hash(new object[]
				{
					"x",
					0.5,
					"y",
					0.5,
					"easetype",
					"easeOutQuart",
					"time",
					0.1f,
					"delay",
					0.2f
				}));
				iTween.ScaleTo(this.arousalTexture[0].gameObject, iTween.Hash(new object[]
				{
					"x",
					1,
					"y",
					1,
					"easetype",
					"easeOutQuart",
					"time",
					0f,
					"delay",
					0.2f
				}));
				this.arousalGlitter.Play();
				yield return new WaitForSeconds(0.45f);
				this.arousalTexture[1].alpha = 1f;
				iTween.ScaleTo(this.arousalTexture[1].gameObject, iTween.Hash(new object[]
				{
					"x",
					0.5,
					"y",
					0.5,
					"easetype",
					"easeOutQuart",
					"time",
					0.1f,
					"delay",
					0.2f
				}));
				iTween.ScaleTo(this.arousalTexture[1].gameObject, iTween.Hash(new object[]
				{
					"x",
					1,
					"y",
					1,
					"easetype",
					"easeOutQuart",
					"time",
					0f,
					"delay",
					0.2f
				}));
				yield return new WaitForSeconds(0.5f);
				this.stretchLightParticle.Play();
			}
			yield return new WaitForSeconds(1f);
			this.tapScreenButton.SetActive(true);
			this.allSkipButton.SetActive(false);
			yield break;
		}

		public void Initialize(CutsceneSound sound, Vector3 subCameraAnchor, GameObject baseMonster, GameObject materialMonster, GameObject digitama, bool upArousal, GameObject allSkipButton)
		{
			this.wireMaterial = new Material(Shader.Find("Unlit/UnlitAlphaWithFade"));
			this.sound = sound;
			this.baseMonster = baseMonster;
			this.materialMonster = materialMonster;
			this.digitama = digitama;
			this.rareUp = upArousal;
			this.allSkipButton = allSkipButton;
			EggModel component = digitama.GetComponent<EggModel>();
			this.cameraSwitcher.SetLookAtObject(component.GetCenter());
			this.cameraSwitcher.EnableSubCamera();
			this.cameraSwitcher.angle = -135f;
			this.cameraSwitcher.SetSubCameraAnchor(subCameraAnchor);
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
			if (this.baseMonsterMaterialList != null)
			{
				CutsceneCommon.ResetRendering(this.baseMonster, this.baseMonsterMaterialList);
			}
			if (this.materialMonsterMaterialList != null)
			{
				CutsceneCommon.ResetRendering(this.materialMonster, this.materialMonsterMaterialList);
			}
		}
	}
}
