﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(UITweener))]
[DisallowMultipleComponent]
public class UITweenerActivePlay : MonoBehaviour
{
	[Header("ONなら終了後、非表示にする")]
	[SerializeField]
	private bool autoDisable;

	private List<UITweener> tween;

	private bool isActive;

	[CompilerGenerated]
	private static Comparison<UITweener> <>f__mg$cache0;

	public bool allDisabled { get; private set; }

	private static int SortedDelayPlusDuration(UITweener x, UITweener y)
	{
		if (x == y)
		{
			return 0;
		}
		float num = x.delay + x.duration;
		float num2 = y.delay + y.duration;
		if (num > num2)
		{
			return 1;
		}
		if (num < num2)
		{
			return -1;
		}
		return 0;
	}

	private void PlayTween()
	{
		this.GetTweens();
		this.allDisabled = false;
		foreach (UITweener uitweener in this.tween)
		{
			if (!(uitweener == null))
			{
				if (uitweener.style != UITweener.Style.Once)
				{
					UITweener.Style style = uitweener.style;
					uitweener.style = UITweener.Style.Once;
					uitweener.ResetToBeginning();
					uitweener.style = style;
				}
				else
				{
					uitweener.ResetToBeginning();
				}
				uitweener.PlayForward();
				if (uitweener.delay > 0f)
				{
					uitweener.ApplyTweenFromValue();
				}
			}
		}
	}

	private void GetTweens()
	{
		if (this.tween == null)
		{
			this.tween = new List<UITweener>(base.GetComponents<UITweener>());
			List<UITweener> list = this.tween;
			if (UITweenerActivePlay.<>f__mg$cache0 == null)
			{
				UITweenerActivePlay.<>f__mg$cache0 = new Comparison<UITweener>(UITweenerActivePlay.SortedDelayPlusDuration);
			}
			list.Sort(UITweenerActivePlay.<>f__mg$cache0);
		}
	}

	private void OnEnable()
	{
		this.isActive = true;
		this.allDisabled = false;
	}

	private void OnDisable()
	{
		this.GetTweens();
		foreach (UITweener uitweener in this.tween)
		{
			if (uitweener != null)
			{
				uitweener.ApplyTweenFromValue();
			}
		}
	}

	private void GetAndSetActiveTweens(bool enabled)
	{
		this.GetTweens();
		foreach (UITweener uitweener in this.tween)
		{
			if (uitweener != null)
			{
				uitweener.enabled = enabled;
			}
		}
	}

	private void LateUpdate()
	{
		if (this.isActive)
		{
			this.PlayTween();
			this.isActive = false;
		}
		if (!this.autoDisable)
		{
			return;
		}
		if (!this.allDisabled && !this.tween[this.tween.Count - 1].enabled)
		{
			base.gameObject.SetActive(false);
			this.allDisabled = true;
		}
	}
}
