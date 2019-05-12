using CharacterModelUI;
using System;
using UnityEngine;

namespace EvolutionRouteMap
{
	public class ChangeModelViewerAnimationEvent : StateMachineBehaviour
	{
		private UI_CharacterModelViewer modelViewer;

		private BoxCollider returnButtonCollider;

		private MonsterData modelMonsterData;

		private void OnFinishShowAnimation()
		{
			if (!MonsterData.IsEgg(this.modelMonsterData.monsterMG.growStep))
			{
				this.modelViewer.LoadCharacterModel(this.modelMonsterData, Vector3.zero, 170f);
				this.modelViewer.SetCharacterCameraDistance();
			}
			else
			{
				Vector3 characterPosition = new Vector3(0f, 0.1f, 0f);
				this.modelViewer.LoadCharacterModel(this.modelMonsterData, characterPosition, 0f);
			}
			this.modelViewer.EnableTouchEvent(true);
			this.returnButtonCollider.enabled = true;
		}

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_OFF);
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (1f <= stateInfo.normalizedTime)
			{
				animator.SetTrigger("Idle");
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (stateInfo.IsName("Show"))
			{
				this.OnFinishShowAnimation();
			}
			animator.ResetTrigger("Idle");
			RestrictionInput.EndLoad();
		}

		public void SetModelViewer(UI_CharacterModelViewer viewer, BoxCollider collider)
		{
			this.modelViewer = viewer;
			this.returnButtonCollider = collider;
		}

		public void SetMonsterData(MonsterData monsterData)
		{
			this.modelMonsterData = monsterData;
		}
	}
}
