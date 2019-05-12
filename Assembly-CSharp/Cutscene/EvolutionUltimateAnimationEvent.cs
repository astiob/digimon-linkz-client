using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscene
{
	public sealed class EvolutionUltimateAnimationEvent : MonoBehaviour
	{
		[SerializeField]
		private Material digitalPatternMaterial;

		[SerializeField]
		private Material wireFrameMaterial;

		[SerializeField]
		private Transform lastCutCamera;

		private CutsceneSound sound;

		private GameObject beforeMonster;

		private GameObject afterMonster;

		private List<Material[]> beforeMonsterMaterialList;

		private List<Material[]> afterMonsterMaterialList;

		private bool spiralBRotatoFlag;

		private void spriralRotater()
		{
			this.spiralBRotatoFlag = true;
		}

		private void CharacterA_LineOn()
		{
			this.beforeMonsterMaterialList = CutsceneCommon.SetWireFrameRendering(this.beforeMonster, this.wireFrameMaterial);
		}

		private void CharacterB_LineOn()
		{
			this.afterMonsterMaterialList = CutsceneCommon.SetWireFrameRendering(this.afterMonster, this.wireFrameMaterial);
		}

		private void CharacterA_LineOff()
		{
			CutsceneCommon.ResetRendering(this.beforeMonster, this.beforeMonsterMaterialList);
		}

		private void CharacterB_LineOff()
		{
			CutsceneCommon.ResetRendering(this.afterMonster);
		}

		private void MaterialConverterB()
		{
			CutsceneCommon.ChangeMaterial(this.afterMonster, this.digitalPatternMaterial);
		}

		private void MaterialResetB()
		{
			CutsceneCommon.ChangeMaterial(this.afterMonster, this.afterMonsterMaterialList);
		}

		private void AttackAnimation()
		{
			CharacterParams component = this.afterMonster.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.revival, SkillType.Attack, 0, null, null);
		}

		private void cameraPositionAdjustment()
		{
			CharacterParams component = this.afterMonster.GetComponent<CharacterParams>();
			float num = component.RootToCenterDistance();
			num = num / 2f + 1.5f;
			this.lastCutCamera.localPosition = new Vector3(0f, 0f, num);
		}

		private void SoudPlayer1()
		{
			this.sound.PlaySE("SEInternal/Cutscene/bt_504");
		}

		private void SoudPlayer2()
		{
			this.sound.PlaySE("SEInternal/Cutscene/bt_115");
		}

		private void SoudPlayer3()
		{
			this.sound.PlaySE("SEInternal/Cutscene/ev_127");
		}

		private void SoudPlayer4()
		{
			this.sound.PlaySE("SEInternal/Cutscene/bt_519");
		}

		public void Initialize(CutsceneSound sound, GameObject beforeMonster, GameObject afterMonster)
		{
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

		public bool IsRotateDigimoji()
		{
			return this.spiralBRotatoFlag;
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
