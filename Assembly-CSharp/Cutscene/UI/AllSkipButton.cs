using System;

namespace Cutscene.UI
{
	public sealed class AllSkipButton : ButtonTouchBehaviourBase, IButtonTouchEvent
	{
		private UITexture buttonTexture;

		protected override void OnInitialize()
		{
			this.buttonTexture = base.GetComponent<UITexture>();
		}

		public void TouchButton(ButtonTouchObserver obsever)
		{
			obsever.RemoveCollider(this.selfCollider);
			base.DoAction();
		}

		public void Hide()
		{
			this.buttonTexture.enabled = false;
			this.selfCollider.enabled = false;
		}
	}
}
