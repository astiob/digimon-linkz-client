using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscene
{
	public sealed class VersionUpAnimationEvent : MonoBehaviour
	{
		private CutsceneSound sound;

		private Transform mainCameraTransform;

		private Material wireMaterial;

		private GameObject beforeMonster;

		private GameObject afterMonster;

		private List<Material[]> beforeMonsterMaterialList;

		private List<Material[]> afterMonsterMaterialList;

		private void CharacterA_LineOn()
		{
			this.beforeMonsterMaterialList = CutsceneCommon.SetWireFrameRendering(this.beforeMonster, this.wireMaterial);
			this.afterMonsterMaterialList = CutsceneCommon.SetWireFrameRendering(this.afterMonster, this.wireMaterial);
		}

		private void CharacterA_LineOff()
		{
			CutsceneCommon.ResetRendering(this.beforeMonster, this.beforeMonsterMaterialList);
		}

		private void CharacterB_LineOff()
		{
			CutsceneCommon.ResetRendering(this.afterMonster, this.afterMonsterMaterialList);
		}

		private void AttackAnimation()
		{
			CharacterParams component = this.afterMonster.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.revival, SkillType.Attack, 0, null, null);
		}

		private void monsterBpositionAdjustment()
		{
			this.afterMonster.transform.localPosition = Vector3.zero;
		}

		private void ChaseFlagStarter()
		{
			CharacterParams component = this.afterMonster.GetComponent<CharacterParams>();
			this.mainCameraTransform.LookAt(component.characterFaceCenterTarget);
		}

		public void Initialize(CutsceneSound sound, Transform mainCameraTransform, GameObject beforeMonster, GameObject afterMonster)
		{
			this.wireMaterial = new Material(Shader.Find("Unlit/UnlitAlphaWithFade"));
			this.sound = sound;
			this.mainCameraTransform = mainCameraTransform;
			this.beforeMonster = beforeMonster;
			this.afterMonster = afterMonster;
		}

		public bool IsPlaying()
		{
			return base.gameObject.activeSelf;
		}

		public void StartAnimation()
		{
			base.gameObject.SetActive(true);
			this.sound.PlaySE("SE/Farm/se_222");
			CharacterParams component = this.beforeMonster.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
			component = this.afterMonster.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
		}

		public void ResetMonsterMaterial()
		{
			if (this.beforeMonsterMaterialList != null)
			{
				CutsceneCommon.ResetRendering(this.beforeMonster, this.beforeMonsterMaterialList);
			}
			if (this.afterMonsterMaterialList != null)
			{
				CutsceneCommon.ResetRendering(this.afterMonster, this.afterMonsterMaterialList);
			}
		}
	}
}
