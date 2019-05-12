using System;
using UnityEngine;

namespace Cutscene.UI
{
	public sealed class AllSkipButtonForGUI : ButtonTouchBehaviourBase, IButtonTouchEvent
	{
		private MeshRenderer buttonMeshRenderer;

		protected override void OnInitialize()
		{
			this.buttonMeshRenderer = base.GetComponent<MeshRenderer>();
			BoxCollider component = base.GetComponent<BoxCollider>();
			GUISprite component2 = base.GetComponent<GUISprite>();
			component2.SetBoardSize(component.size.x, component.size.y);
		}

		public void TouchButton(ButtonTouchObserver obsever)
		{
			obsever.RemoveCollider(this.selfCollider);
			base.DoAction();
		}

		public void Hide()
		{
			this.buttonMeshRenderer.enabled = false;
			this.selfCollider.enabled = false;
		}
	}
}
