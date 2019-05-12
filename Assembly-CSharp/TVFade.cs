using System;
using UnityEngine;

public class TVFade : ScreenEffectBase
{
	[SerializeField]
	[Range(0f, 0.5f)]
	private float lightEdge;

	[SerializeField]
	private float vanishTimeLightHeight;

	[SerializeField]
	private float vanishTimeLightWidth;

	[SerializeField]
	private float vanishTimeLightCenter;

	private TVFade.LightRunTimeAnimationInfo lightRunTime;

	private Action onFinishedAction;

	protected override void Awake()
	{
		base.Awake();
		this.material.SetFloat("_Aspect", this.aspectRatio);
		this.SetLightEdge(this.lightEdge);
	}

	public void StartAnimation(Action onCompleted)
	{
		if (this.lightRunTime.height.state != TVFade.AnimeState.NONE || this.lightRunTime.width.state != TVFade.AnimeState.NONE || this.lightRunTime.center.state != TVFade.AnimeState.NONE)
		{
			global::Debug.LogError("StartAnimation : Is Playing");
			if (onCompleted != null)
			{
				onCompleted();
			}
			return;
		}
		this.onFinishedAction = onCompleted;
		this.lightEdge = Mathf.Min(Mathf.Abs(this.lightEdge), 0.5f);
		this.lightRunTime.height.value = 1f;
		this.lightRunTime.width.value = 1f;
		this.lightRunTime.center.value = this.lightEdge;
		this.SetLightHeight(1f);
		this.SetLightWidth(1f);
		this.SetLightEdge(this.lightEdge);
		this.lightRunTime.height.state = TVFade.AnimeState.REVERSE;
		this.lightRunTime.height.elapse = this.vanishTimeLightHeight;
		this.lightRunTime.width.elapse = this.vanishTimeLightWidth;
		this.lightRunTime.center.elapse = this.vanishTimeLightCenter;
	}

	public void StopAnimation()
	{
		this.lightRunTime.height.state = TVFade.AnimeState.NONE;
		this.lightRunTime.width.state = TVFade.AnimeState.NONE;
		this.lightRunTime.center.state = TVFade.AnimeState.NONE;
		if (this.onFinishedAction != null)
		{
			this.onFinishedAction();
			this.onFinishedAction = null;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.lightRunTime.height.state == TVFade.AnimeState.REVERSE)
		{
			this.UpdateValue(0f, 1f, this.vanishTimeLightHeight, ref this.lightRunTime.height, new Action<float>(this.SetLightHeight), delegate
			{
				this.lightRunTime.height.state = TVFade.AnimeState.NONE;
				this.lightRunTime.width.state = TVFade.AnimeState.REVERSE;
			});
		}
		else if (this.lightRunTime.width.state == TVFade.AnimeState.REVERSE)
		{
			this.UpdateValue(0f, 1f, this.vanishTimeLightWidth, ref this.lightRunTime.width, new Action<float>(this.SetLightWidth), delegate
			{
				this.lightRunTime.width.state = TVFade.AnimeState.NONE;
				this.lightRunTime.center.state = TVFade.AnimeState.REVERSE;
			});
		}
		else if (this.lightRunTime.center.state == TVFade.AnimeState.REVERSE)
		{
			this.UpdateValue(0f, this.lightEdge, this.vanishTimeLightCenter, ref this.lightRunTime.center, new Action<float>(this.SetLightEdge), delegate
			{
				this.StopAnimation();
			});
		}
	}

	private void UpdateValue(float min, float max, float duration, ref TVFade.RunTimeAnimationInfo runTimeInfo, Action<float> setAction, Action onCompleted)
	{
		switch (runTimeInfo.state)
		{
		case TVFade.AnimeState.PLAY:
			if (this.PlayForward(ref runTimeInfo.delay, ref runTimeInfo.value, min, max, ref runTimeInfo.elapse, duration))
			{
				onCompleted();
			}
			setAction(runTimeInfo.value);
			break;
		case TVFade.AnimeState.REVERSE:
			if (this.PlayReverse(ref runTimeInfo.delay, ref runTimeInfo.value, min, max, ref runTimeInfo.elapse, duration))
			{
				onCompleted();
			}
			setAction(runTimeInfo.value);
			break;
		case TVFade.AnimeState.STOP:
			runTimeInfo.state = TVFade.AnimeState.NONE;
			break;
		}
	}

	private bool PlayForward(ref float delay, ref float value, float min, float max, ref float currentTime, float duration)
	{
		bool result = false;
		delay -= Time.deltaTime;
		if (delay < 0f)
		{
			delay = 0f;
			currentTime += Time.deltaTime;
			float num = currentTime / duration;
			num = Mathf.Min(num, 1f);
			float num2 = (max - min) * num;
			value = num2 + min;
			value = Mathf.Min(value, max);
			result = (num >= 1f);
		}
		return result;
	}

	private bool PlayReverse(ref float delay, ref float value, float min, float max, ref float currentTime, float duration)
	{
		bool result = false;
		delay -= Time.deltaTime;
		if (delay < 0f)
		{
			delay = 0f;
			currentTime -= Time.deltaTime;
			float num = currentTime / duration;
			num = Mathf.Max(num, 0f);
			float num2 = (max - min) * num;
			value = num2 + min;
			value = Mathf.Max(value, min);
			result = (num <= 0f);
		}
		return result;
	}

	public void SetAlphaMax(float f)
	{
		this.material.SetFloat("_AlphaMax", f);
	}

	public void SetBaseColor(Color color)
	{
		this.material.SetColor("_BaseCol", color);
	}

	public void SetCenterPositionX(float x)
	{
		this.material.SetFloat("_CtrX", x);
	}

	public void SetCenterPositionY(float y)
	{
		this.material.SetFloat("_CtrY", y);
	}

	public void SetLightWidth(float w)
	{
		this.material.SetFloat("_WinX", w);
	}

	public void SetLightHeight(float h)
	{
		this.material.SetFloat("_WinY", h);
	}

	public void SetLightEdge(float e)
	{
		this.material.SetFloat("_Range", e);
	}

	private enum AnimeState
	{
		NONE,
		PLAY,
		REVERSE,
		STOP
	}

	private struct RunTimeAnimationInfo
	{
		public TVFade.AnimeState state;

		public float delay;

		public float value;

		public float elapse;
	}

	private struct LightRunTimeAnimationInfo
	{
		public TVFade.RunTimeAnimationInfo height;

		public TVFade.RunTimeAnimationInfo width;

		public TVFade.RunTimeAnimationInfo center;
	}
}
