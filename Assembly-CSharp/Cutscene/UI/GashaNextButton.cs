using System;
using UnityEngine;

namespace Cutscene.UI
{
	public sealed class GashaNextButton : ButtonTouchBehaviourBase, IButtonTouchEvent
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
			base.DoAction();
		}

		public void ShowSprite()
		{
			this.buttonText.enabled = true;
			for (int i = 0; i < this.buttonSpriteList.Length; i++)
			{
				this.buttonSpriteList[i].enabled = true;
			}
		}

		public void HideSprite()
		{
			this.buttonText.enabled = false;
			for (int i = 0; i < this.buttonSpriteList.Length; i++)
			{
				this.buttonSpriteList[i].enabled = false;
			}
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
