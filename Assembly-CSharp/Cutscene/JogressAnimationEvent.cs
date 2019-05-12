using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscene
{
	public sealed class JogressAnimationEvent : MonoBehaviour
	{
		[SerializeField]
		private Transform digimojiRing;

		private CutsceneSound sound;

		private Material wireMaterial;

		private List<Material[]> afterMonsterMaterialList;

		private GameObject beforeMonster;

		private GameObject partnerMonster;

		private GameObject afterMonster;

		private bool chaseFlag;

		private void ChaseFlagStarter()
		{
			this.chaseFlag = true;
		}

		private void Character3_LineOn()
		{
			this.afterMonster.SetActive(true);
			CharacterParams component = this.afterMonster.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
			this.afterMonsterMaterialList = CutsceneCommon.SetWireFrameRendering(this.afterMonster, this.wireMaterial);
		}

		private void Character3_LineOff()
		{
			CutsceneCommon.ResetRendering(this.afterMonster, this.afterMonsterMaterialList);
		}

		private void Character3MotionTrigger()
		{
			CharacterParams component = this.afterMonster.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.win, SkillType.Attack, 0, null, null);
		}

		private void RingSetPositionSetter()
		{
			CharacterParams component = this.afterMonster.GetComponent<CharacterParams>();
			this.digimojiRing.localPosition = component.characterCenterTarget.localPosition;
		}

		private void SoudPlayer1()
		{
			this.sound.PlaySE("SEInternal/Cutscene/bt_643");
		}

		private void SoudPlayer2()
		{
			this.sound.PlaySE("SEInternal/Cutscene/bt_507");
		}

		private void SoudPlayer3()
		{
			this.sound.PlaySE("SEInternal/Cutscene/bt_115");
		}

		private void SoudPlayer4()
		{
			this.sound.PlaySE("SEInternal/Cutscene/bt_549");
		}

		public void Initialize(CutsceneSound sound, GameObject beforeMonster, GameObject partnerMonster, GameObject afterMonster)
		{
			this.wireMaterial = new Material(Shader.Find("Unlit/UnlitAlphaWithFade"));
			this.sound = sound;
			this.beforeMonster = beforeMonster;
			this.partnerMonster = partnerMonster;
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
			component.PlayAnimation(CharacterAnimationType.move, SkillType.Attack, 0, null, null);
			component = this.partnerMonster.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.move, SkillType.Attack, 0, null, null);
		}

		public bool IsCameraChase()
		{
			return this.chaseFlag;
		}

		public void ResetMonsterMaterial()
		{
			this.chaseFlag = false;
			if (this.afterMonsterMaterialList != null)
			{
				CutsceneCommon.ResetRendering(this.afterMonster, this.afterMonsterMaterialList);
			}
		}
	}
}
