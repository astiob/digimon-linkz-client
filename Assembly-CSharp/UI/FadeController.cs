using System;
using UnityEngine;

namespace UI
{
	public sealed class FadeController : MonoBehaviour
	{
		private static FadeController instance;

		private bool isPause;

		private Color startColor;

		private Color endColor;

		private float animationTime;

		private float currentTime;

		private Action onFinishAnimation;

		private Action onPauseAnimation;

		private UISprite fadeSprite;

		private BoxCollider colliderTapLimit;

		private static FadeController Create()
		{
			FadeController result = null;
			GameObject gameObject = Resources.Load<GameObject>("UICommon/Effect/FADE_W");
			if (null != gameObject)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				gameObject2.transform.parent = Singleton<GUIMain>.Instance.transform;
				gameObject2.transform.localPosition = new Vector3(0f, 0f, gameObject2.transform.localPosition.z);
				gameObject2.transform.localScale = Vector3.one;
				gameObject2.transform.localRotation = Quaternion.identity;
				GUIFadeControll component = gameObject2.GetComponent<GUIFadeControll>();
				if (null != component)
				{
					UnityEngine.Object.Destroy(component);
				}
				result = gameObject2.AddComponent<FadeController>();
				gameObject2.name = "Fade";
			}
			return result;
		}

		private void Awake()
		{
			this.fadeSprite = base.gameObject.GetComponentInChildren<UISprite>();
			this.colliderTapLimit = base.gameObject.GetComponentInChildren<BoxCollider>();
		}

		private void Update()
		{
			if (!this.isPause)
			{
				this.currentTime += Time.unscaledDeltaTime;
				if (this.currentTime >= this.animationTime)
				{
					this.currentTime = this.animationTime;
					if (this.onFinishAnimation != null)
					{
						this.onFinishAnimation();
						this.onFinishAnimation = null;
					}
				}
				float t = this.currentTime / this.animationTime;
				Color color = Color.Lerp(this.startColor, this.endColor, t);
				this.fadeSprite.color = color;
			}
			else if (this.onPauseAnimation != null)
			{
				this.onPauseAnimation();
				this.onPauseAnimation = null;
			}
		}

		private void SetAction(Action onFinish, Action onPause)
		{
			if (onFinish != null)
			{
				this.onFinishAnimation = (onFinish.Clone() as Action);
			}
			else
			{
				this.onFinishAnimation = null;
			}
			if (onPause != null)
			{
				this.onPauseAnimation = (onPause.Clone() as Action);
			}
			else
			{
				this.onPauseAnimation = null;
			}
		}

		public static FadeController Instance()
		{
			if (null == FadeController.instance)
			{
				FadeController.instance = FadeController.Create();
			}
			return FadeController.instance;
		}

		public void StartFade(Color startColor, Color endColor, float time, Action onFinish = null, Action onPause = null)
		{
			this.startColor = startColor;
			this.endColor = endColor;
			this.animationTime = time;
			this.currentTime = 0f;
			this.SetAction(onFinish, onPause);
			this.fadeSprite.color = startColor;
			this.colliderTapLimit.enabled = true;
			base.enabled = true;
		}

		public void StartBlackFade(float startAlpha, float endAlpha, float time, Action onFinish = null, Action onPause = null)
		{
			this.startColor = new Color(0f, 0f, 0f, startAlpha);
			this.endColor = new Color(0f, 0f, 0f, endAlpha);
			this.animationTime = time;
			this.currentTime = 0f;
			this.SetAction(onFinish, onPause);
			this.fadeSprite.color = this.startColor;
			this.colliderTapLimit.enabled = true;
			base.enabled = true;
		}

		public void StartWhiteFade(float startAlpha, float endAlpha, float time, Action onFinish = null, Action onPause = null)
		{
			this.startColor = new Color(1f, 1f, 1f, startAlpha);
			this.endColor = new Color(1f, 1f, 1f, endAlpha);
			this.animationTime = time;
			this.currentTime = 0f;
			this.SetAction(onFinish, onPause);
			this.fadeSprite.color = this.startColor;
			this.colliderTapLimit.enabled = true;
			base.enabled = true;
		}

		public void PauseFade(bool isPause)
		{
			this.isPause = isPause;
		}

		public void DeleteFade()
		{
			this.fadeSprite.alpha = 0f;
			this.colliderTapLimit.enabled = false;
			base.enabled = false;
		}
	}
}
