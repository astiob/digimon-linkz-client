using System;
using System.Collections;
using UnityEngine;

namespace UI.Common
{
	public sealed class UIToggleAlphaTween : MonoBehaviour
	{
		[SerializeField]
		private TweenAlpha tweenAlpha;

		[SerializeField]
		private AnimationCurve animaCurve;

		[SerializeField]
		private float fadeTime;

		[SerializeField]
		private float delayFadeOutTime;

		private Action onCompleteFadeIn;

		private Action onCompleteFadeOut;

		private IEnumerator RemoveTweenDelegate(EventDelegate.Callback removeCallback, Action onRemoved)
		{
			yield return new WaitForEndOfFrame();
			EventDelegate.Remove(this.tweenAlpha.onFinished, removeCallback);
			onRemoved();
			yield break;
		}

		private void SetTween(float from, float to, float delay)
		{
			this.tweenAlpha.from = from;
			this.tweenAlpha.to = to;
			this.tweenAlpha.style = UITweener.Style.Once;
			this.tweenAlpha.duration = this.fadeTime;
			this.tweenAlpha.animationCurve = this.animaCurve;
			this.tweenAlpha.delay = delay;
			this.tweenAlpha.tweenFactor = 0f;
			this.tweenAlpha.PlayForward();
		}

		private void OnCompleteFadeIn()
		{
			if (this.onCompleteFadeIn != null)
			{
				this.onCompleteFadeIn();
			}
			base.StartCoroutine(this.RemoveTweenDelegate(new EventDelegate.Callback(this.OnCompleteFadeIn), new Action(this.StartFadeOut)));
		}

		private void OnCompleteFadeOut()
		{
			if (this.onCompleteFadeOut != null)
			{
				this.onCompleteFadeOut();
			}
			base.StartCoroutine(this.RemoveTweenDelegate(new EventDelegate.Callback(this.OnCompleteFadeOut), new Action(this.StartFadeIn)));
		}

		public void StartFadeIn()
		{
			EventDelegate.Set(this.tweenAlpha.onFinished, new EventDelegate.Callback(this.OnCompleteFadeIn));
			this.SetTween(0f, 1f, 0f);
		}

		public void StartFadeOut()
		{
			EventDelegate.Set(this.tweenAlpha.onFinished, new EventDelegate.Callback(this.OnCompleteFadeOut));
			this.SetTween(1f, 0f, this.delayFadeOutTime);
		}

		public void StopFade()
		{
			this.tweenAlpha.enabled = false;
			EventDelegate.Remove(this.tweenAlpha.onFinished, new EventDelegate.Callback(this.OnCompleteFadeIn));
			EventDelegate.Remove(this.tweenAlpha.onFinished, new EventDelegate.Callback(this.OnCompleteFadeOut));
		}

		public void SetActionCompleteFadeIn(Action action)
		{
			this.onCompleteFadeIn = action;
		}

		public void SetActionCompleteFadeOut(Action action)
		{
			this.onCompleteFadeOut = action;
		}

		public void SetAlpha(float alpha)
		{
			this.tweenAlpha.value = alpha;
		}
	}
}
