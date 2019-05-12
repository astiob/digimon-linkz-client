using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscene
{
	public sealed class ModeChangeAnimationEvent : MonoBehaviour
	{
		[SerializeField]
		private Material blueDigitalPatternMaterial;

		[SerializeField]
		private Material yellowDigitalPatternMaterial;

		private CutsceneSound sound;

		private GameObject beforeMonster;

		private GameObject afterMonster;

		private List<Material[]> afterMonsterMaterialList;

		private bool cameraRollFlag;

		private void OnCameraRoll()
		{
			this.cameraRollFlag = true;
		}

		private void OffCameraRoll()
		{
			this.cameraRollFlag = false;
		}

		private void MaterialConverterA()
		{
			CutsceneCommon.ChangeMaterial(this.beforeMonster, this.blueDigitalPatternMaterial);
		}

		private void MaterialConverterB()
		{
			CutsceneCommon.ChangeMaterial(this.afterMonster, this.yellowDigitalPatternMaterial);
		}

		private void OffMaterialConvertB()
		{
			CutsceneCommon.ChangeMaterial(this.afterMonster, this.afterMonsterMaterialList);
		}

		private void OnAttackAnimation()
		{
			CharacterParams component = this.afterMonster.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.win, SkillType.Attack, 0, null, null);
		}

		private void SoudPlayer1()
		{
			this.sound.PlaySE("SEInternal/Cutscene/se_co_memorial_a_rs");
		}

		private void SoudPlayer2()
		{
			this.sound.PlaySE("SEInternal/Cutscene/se_evo00_24k");
		}

		public void Initialize(CutsceneSound sound, GameObject beforeMonster, GameObject afterMonster)
		{
			this.afterMonsterMaterialList = CutsceneCommon.GetMaterial(afterMonster);
			this.sound = sound;
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
			CharacterParams component = this.beforeMonster.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
			component = this.afterMonster.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
		}

		public bool IsRotateCamera()
		{
			return this.cameraRollFlag;
		}
	}
}
