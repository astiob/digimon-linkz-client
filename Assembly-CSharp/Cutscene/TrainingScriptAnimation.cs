using Cutscene.EffectParts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscene
{
	public sealed class TrainingScriptAnimation : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem circleParticle;

		[SerializeField]
		private ParticleSystem auraParticle;

		[SerializeField]
		private ParticleSystem sphereParticle;

		private Material wireMaterial;

		private CutsceneSound sound;

		private GameObject baseMonster;

		private Coroutine animationCoroutine;

		private Action endAnimationCallback;

		private List<Material[]> baseMonsterMaterialList;

		private FxLaser[] laserEffectList;

		private void OnEnable()
		{
			this.animationCoroutine = base.StartCoroutine(this.PlayScene());
		}

		private IEnumerator PlayScene()
		{
			yield return base.StartCoroutine(this.CutA());
			if (this.endAnimationCallback != null)
			{
				this.endAnimationCallback();
				this.endAnimationCallback = null;
			}
			yield break;
		}

		private IEnumerator CutA()
		{
			yield return new WaitForSeconds(0.5f);
			this.sound.PlaySE("SEInternal/Cutscene/se_213");
			yield return new WaitForSeconds(0.5f);
			for (int i = 0; i < this.laserEffectList.Length; i++)
			{
				this.laserEffectList[i].StartMoveEffect();
			}
			yield return new WaitForSeconds(0.3f);
			this.baseMonsterMaterialList = CutsceneCommon.GetMaterial(this.baseMonster);
			CutsceneCommon.ChangeMaterial(this.baseMonster, this.wireMaterial);
			this.sphereParticle.Play();
			yield return new WaitForSeconds(0.3f);
			for (int j = 0; j < this.laserEffectList.Length; j++)
			{
				this.laserEffectList[j].gameObject.SetActive(false);
			}
			yield return new WaitForSeconds(0.8f);
			CutsceneCommon.SetWireFrameRendering(this.baseMonster);
			yield return new WaitForSeconds(1.6f);
			this.auraParticle.Play();
			yield return new WaitForSeconds(0.2f);
			this.circleParticle.Play();
			yield return new WaitForSeconds(1.5f);
			CutsceneCommon.ResetRendering(this.baseMonster, this.baseMonsterMaterialList);
			yield break;
		}

		public void Initialize(CutsceneSound sound, GameObject baseMonster, int materialNum, Action endAnimationCallback)
		{
			this.wireMaterial = new Material(Shader.Find("Unlit/UnlitAlphaWithFade"));
			this.sound = sound;
			this.baseMonster = baseMonster;
			this.endAnimationCallback = endAnimationCallback;
			CharacterParams component = baseMonster.GetComponent<CharacterParams>();
			Transform characterCenterTarget = component.characterCenterTarget;
			GameObject resource = FxLaser.LoadPrefab();
			this.laserEffectList = new FxLaser[materialNum];
			for (int i = 0; i < this.laserEffectList.Length; i++)
			{
				this.laserEffectList[i] = FxLaser.Create(resource, baseMonster.transform);
				float rotationEulerAngleY = FxLaser.GetRotationEulerAngleY(materialNum, i);
				this.laserEffectList[i].SetTransform(characterCenterTarget.position.y, rotationEulerAngleY);
			}
			this.sphereParticle.transform.position = characterCenterTarget.position;
			this.auraParticle.transform.position = characterCenterTarget.position;
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
		}
	}
}
