using CharacterModelUI;
using EvolutionDiagram;
using System;
using UnityEngine;

namespace EvolutionRouteMap
{
	public sealed class UI_EvolutionCharacterModelViewer : MonoBehaviour
	{
		[SerializeField]
		private UI_CharacterModelViewer modelViewer;

		[SerializeField]
		private BoxCollider returnButtonCollider;

		public void Initialize()
		{
			this.modelViewer.Initialize(new Vector3(0f, 5000f, 0f));
			this.modelViewer.SetCameraViewOffset(-80f, 0f);
			this.modelViewer.EnableTouchEvent(false);
			this.returnButtonCollider.enabled = false;
			Animator component = base.gameObject.GetComponent<Animator>();
			ChangeModelViewerAnimationEvent behaviour = component.GetBehaviour<ChangeModelViewerAnimationEvent>();
			if (null != behaviour)
			{
				behaviour.SetModelViewer(this.modelViewer, this.returnButtonCollider);
			}
		}

		public void DeleteModelViewer()
		{
			this.modelViewer.DestroyRenderTexture();
		}

		public void OnPushed3DButton()
		{
			CMD_EvolutionRouteMap component = base.GetComponent<CMD_EvolutionRouteMap>();
			EvolutionRouteMapData routeMapData = component.GetRouteMapData();
			EvolutionDiagramData.IconMonster selectMonster = routeMapData.GetSelectMonster();
			Animator component2 = base.gameObject.GetComponent<Animator>();
			if (null != component2)
			{
				ChangeModelViewerAnimationEvent behaviour = component2.GetBehaviour<ChangeModelViewerAnimationEvent>();
				if (null != behaviour)
				{
					behaviour.SetMonsterData(selectMonster.master.Simple.monsterGroupId);
				}
				component2.SetTrigger("Show");
			}
		}

		public void OnPushedReturnButton()
		{
			this.returnButtonCollider.enabled = false;
			this.modelViewer.EnableTouchEvent(false);
			this.modelViewer.DeleteCharacterModel();
			Animator component = base.gameObject.GetComponent<Animator>();
			if (null != component)
			{
				component.SetTrigger("Delete");
			}
		}
	}
}
