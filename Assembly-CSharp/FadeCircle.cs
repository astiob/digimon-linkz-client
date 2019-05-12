using System;
using UnityEngine;

public class FadeCircle : ScreenEffectBase
{
	[SerializeField]
	private Color baseColor;

	[Range(0f, 1f)]
	[SerializeField]
	private float circleCenterX;

	[Range(0f, 1f)]
	[SerializeField]
	private float circleCenterY;

	[SerializeField]
	private float circleRange;

	[SerializeField]
	private FadeCircle.CircleInfo circle;

	[SerializeField]
	private FadeCircle.AlphaInfo fullFaceAlpha;

	private FadeCircle.CircleRunTimeAnimationInfo circleRunTime;

	private FadeCircle.RunTimeAnimationInfo fullFaceAlphaRunTime;

	public void StartAnimation()
	{
		if (this.fullFaceAlphaRunTime.state != FadeCircle.AnimeState.NONE || this.circleRunTime.alpha.state != FadeCircle.AnimeState.NONE || this.circleRunTime.radius.state != FadeCircle.AnimeState.NONE)
		{
			global::Debug.LogError("StartAnimation : Is Playing");
		}
		this.fullFaceAlphaRunTime.value = this.fullFaceAlpha.min;
		this.fullFaceAlphaRunTime.delay = this.fullFaceAlpha.delay;
		this.circleRunTime.alpha.value = this.circle.alpha.min;
		this.circleRunTime.alpha.delay = this.circle.alpha.delay;
		this.circleRunTime.radius.value = this.circle.radius.to;
		this.circleRunTime.radius.delay = this.circle.radius.delay;
		float num = 1f - this.circle.alpha.max;
		if (this.fullFaceAlpha.max > num)
		{
			this.fullFaceAlpha.max = num;
			this.fullFaceAlpha.min = Mathf.Min(this.fullFaceAlpha.min, this.fullFaceAlpha.max);
		}
		this.fullFaceAlpha.blinkingSpeed = Mathf.Max(this.fullFaceAlpha.blinkingSpeed, 0.5f);
		this.circle.alpha.blinkingSpeed = Mathf.Max(this.circle.alpha.blinkingSpeed, 0.5f);
		this.circle.radius.changeSpeed = Mathf.Max(this.circle.radius.changeSpeed, 0.5f);
		this.SetAlphaMin(this.fullFaceAlpha.min);
		this.SetAlphaMax(this.circle.alpha.min);
		this.SetRadius(this.circle.radius.to);
		this.fullFaceAlphaRunTime.state = FadeCircle.AnimeState.PLAY;
		this.circleRunTime.alpha.state = FadeCircle.AnimeState.PLAY;
		this.circleRunTime.radius.state = FadeCircle.AnimeState.PLAY;
	}

	public void StopAnimation()
	{
		this.fullFaceAlphaRunTime.state = FadeCircle.AnimeState.STOP;
		this.circleRunTime.alpha.state = FadeCircle.AnimeState.STOP;
		this.circleRunTime.radius.state = FadeCircle.AnimeState.STOP;
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateValue(this.fullFaceAlpha.min, this.fullFaceAlpha.max, this.fullFaceAlpha.blinkingSpeed, ref this.fullFaceAlphaRunTime, new Action<float>(this.SetAlphaMin));
		this.UpdateValue(this.circle.alpha.min, this.circle.alpha.max, this.circle.alpha.blinkingSpeed, ref this.circleRunTime.alpha, new Action<float>(this.SetAlphaMax));
		this.UpdateValue(this.circle.radius.to, this.circle.radius.from, this.circle.radius.changeSpeed, ref this.circleRunTime.radius, new Action<float>(this.SetRadius));
	}

	private void UpdateValue(float min, float max, float duration, ref FadeCircle.RunTimeAnimationInfo runTimeInfo, Action<float> setAction)
	{
		switch (runTimeInfo.state)
		{
		case FadeCircle.AnimeState.PLAY:
			if (this.PlayForward(ref runTimeInfo.delay, ref runTimeInfo.value, min, max, ref runTimeInfo.elapse, duration))
			{
				runTimeInfo.state = FadeCircle.AnimeState.REVERSE;
			}
			setAction(runTimeInfo.value);
			break;
		case FadeCircle.AnimeState.REVERSE:
			if (this.PlayReverse(ref runTimeInfo.delay, ref runTimeInfo.value, min, max, ref runTimeInfo.elapse, duration))
			{
				runTimeInfo.state = FadeCircle.AnimeState.PLAY;
			}
			setAction(runTimeInfo.value);
			break;
		case FadeCircle.AnimeState.STOP:
			runTimeInfo.state = FadeCircle.AnimeState.NONE;
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

	protected override void Awake()
	{
		base.Awake();
		this.material.SetFloat("_Aspect", this.aspectRatio);
		this.SetBaseColor(this.baseColor);
		this.SetCenterX(this.circleCenterX);
		this.SetCenterY(this.circleCenterY);
		this.SetRange(this.circleRange);
	}

	public void SetRadius(float rad)
	{
		this.material.SetFloat("_Radius", rad);
	}

	public void SetAlphaMin(float f)
	{
		this.material.SetFloat("_AlphaMin", f);
	}

	public void SetAlphaMax(float f)
	{
		this.material.SetFloat("_AlphaMax", f);
	}

	public void SetBaseColor(Color col)
	{
		this.material.SetColor("_BaseCol", col);
	}

	public void SetCenterX(float p)
	{
		this.material.SetFloat("_CtrX", p);
	}

	public void SetCenterY(float p)
	{
		this.material.SetFloat("_CtrY", p);
	}

	public void SetRange(float f)
	{
		this.material.SetFloat("_Range", f);
	}

	private enum AnimeState
	{
		NONE,
		PLAY,
		REVERSE,
		STOP
	}

	[Serializable]
	private struct AlphaInfo
	{
		[SerializeField]
		[Range(0f, 1f)]
		public float min;

		[Range(0f, 1f)]
		[SerializeField]
		public float max;

		[SerializeField]
		[Range(0.5f, 5f)]
		public float blinkingSpeed;

		[SerializeField]
		public float delay;
	}

	[Serializable]
	private sealed class RadiusInfo
	{
		[Range(0f, 1.5f)]
		[SerializeField]
		public float to;

		[SerializeField]
		[Range(0f, 1.5f)]
		public float from;

		[Range(0.5f, 5f)]
		[SerializeField]
		public float changeSpeed;

		[SerializeField]
		public float delay;
	}

	[Serializable]
	private struct CircleInfo
	{
		[SerializeField]
		public FadeCircle.RadiusInfo radius;

		[SerializeField]
		public FadeCircle.AlphaInfo alpha;
	}

	private struct RunTimeAnimationInfo
	{
		public FadeCircle.AnimeState state;

		public float delay;

		public float value;

		public float elapse;
	}

	private struct CircleRunTimeAnimationInfo
	{
		public FadeCircle.RunTimeAnimationInfo alpha;

		public FadeCircle.RunTimeAnimationInfo radius;
	}
}
