using System;
using UnityEngine;

namespace Cutscene.UI
{
	public sealed class TouchScreenButton : ButtonTouchBehaviourBase, IButtonTouchEvent
	{
		[SerializeField]
		private UILabel buttonText;

		[SerializeField]
		private UISprite[] buttonSpriteList;

		protected override void OnInitialize()
		{
		}

		public void TouchButton(ButtonTouchObserver obsever)
		{
			obsever.RemoveCollider(this.selfCollider);
			base.DoAction();
		}

		public void Hide()
		{
			this.buttonText.enabled = false;
			for (int i = 0; i < this.buttonSpriteList.Length; i++)
			{
				this.buttonSpriteList[i].enabled = false;
			}
			this.selfCollider.enabled = false;
		}
	}
}
