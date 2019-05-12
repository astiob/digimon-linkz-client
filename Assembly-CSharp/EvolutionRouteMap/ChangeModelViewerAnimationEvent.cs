using CharacterModelUI;
using Monster;
using System;
using UnityEngine;

namespace EvolutionRouteMap
{
	public sealed class ChangeModelViewerAnimationEvent : StateMachineBehaviour
	{
		private UI_CharacterModelViewer modelViewer;

		private BoxCollider returnButtonCollider;

		private string monsterModelId;

		private string eggModelId;

		private void OnFinishShowAnimation()
		{
			if (!string.IsNullOrEmpty(this.monsterModelId))
			{
				this.modelViewer.LoadMonsterModel(this.monsterModelId, Vector3.zero, 170f);
				this.modelViewer.SetCharacterCameraDistance();
			}
			else
			{
				Vector3 characterPosition = new Vector3(0f, 0.1f, 0f);
				this.modelViewer.LoadEggModel(this.eggModelId, characterPosition, 0f);
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

		public void SetMonsterData(string modelId)
		{
			this.monsterModelId = modelId;
			this.eggModelId = string.Empty;
		}

		public void SetEggData(string monsterEvolutionRouteId)
		{
			this.eggModelId = MonsterObject.GetEggModelId(monsterEvolutionRouteId);
			this.monsterModelId = string.Empty;
		}
	}
}
