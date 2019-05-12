using CharacterModelUI;
using System;
using UnityEngine;

namespace EvolutionRouteMap
{
	public sealed class ChangeModelViewerAnimationEvent : StateMachineBehaviour
	{
		private UI_CharacterModelViewer modelViewer;

		private BoxCollider returnButtonCollider;

		private string monsterGroupId;

		private void OnFinishShowAnimation()
		{
			if (!string.IsNullOrEmpty(this.monsterGroupId))
			{
				this.modelViewer.LoadMonsterModel(this.monsterGroupId, Vector3.zero, 170f);
				this.modelViewer.SetCharacterCameraDistance();
			}
			else
			{
				Vector3 characterPosition = new Vector3(0f, 0.1f, 0f);
				this.modelViewer.LoadEggModel(this.monsterGroupId, characterPosition, 0f);
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

		public void SetMonsterData(string monsterGroupId)
		{
			this.monsterGroupId = monsterGroupId;
		}

		public void SetEggData(string monsterEvolutionRouteId)
		{
			GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM respDataMA_MonsterEvolutionRouteM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM;
			this.monsterGroupId = string.Empty;
			for (int i = 0; i < respDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM.Length; i++)
			{
				GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM monsterEvolutionRouteM = respDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM[i];
				if (string.IsNullOrEmpty(monsterEvolutionRouteId) || monsterEvolutionRouteM.monsterEvolutionRouteId == monsterEvolutionRouteId)
				{
					this.monsterGroupId = monsterEvolutionRouteM.eggMonsterId;
					break;
				}
			}
		}
	}
}
