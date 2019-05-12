using System;
using UnityEngine;

namespace Cutscene.EffectParts
{
	[Serializable]
	public sealed class CutsceneFade
	{
		[SerializeField]
		private float fadeInSpeed;

		[SerializeField]
		private float fadeOutSpeed;

		[SerializeField]
		private GUISprite fadeSprite;

		[SerializeField]
		private UITexture fadeImage;

		private CutsceneFade.FadeInfo fadeInfo;

		public void Initialize()
		{
			this.fadeInfo = default(CutsceneFade.FadeInfo);
			if (null != this.fadeSprite)
			{
				UIPanel uipanel = GUIMain.GetUIPanel();
				Vector2 windowSize = uipanel.GetWindowSize();
				this.fadeSprite.SetBoardSize(windowSize.x, windowSize.y);
			}
		}

		public void UpdateFade()
		{
			if (this.fadeInfo.isActive)
			{
				Color color;
				if (null != this.fadeSprite)
				{
					color = this.fadeSprite.GetColor();
				}
				else
				{
					color = this.fadeImage.color;
				}
				color.a += this.fadeInfo.speed * Time.deltaTime;
				if (color.a < 0f || 1f < color.a)
				{
					this.fadeInfo.isActive = false;
					if (this.fadeInfo.endCallback != null)
					{
						this.fadeInfo.endCallback();
						this.fadeInfo.endCallback = null;
					}
				}
				color.a = Mathf.Clamp01(color.a);
				if (null != this.fadeSprite)
				{
					this.fadeSprite.SetColor(color);
				}
				else
				{
					this.fadeImage.color = color;
				}
			}
		}

		public void StartFadeIn(Action action)
		{
			this.fadeInfo.isActive = true;
			this.fadeInfo.speed = this.fadeInSpeed * -1f;
			this.fadeInfo.endCallback = action;
		}

		public void StartFadeOut(Action action)
		{
			this.fadeInfo.isActive = true;
			this.fadeInfo.speed = this.fadeOutSpeed;
			this.fadeInfo.endCallback = action;
		}

		private struct FadeInfo
		{
			public bool isActive;

			public float speed;

			public Action endCallback;
		}
	}
}
