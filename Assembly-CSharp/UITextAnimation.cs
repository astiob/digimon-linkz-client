using System;
using UnityEngine;

[RequireComponent(typeof(UITextReplacer))]
[DisallowMultipleComponent]
public class UITextAnimation : MonoBehaviour
{
	[SerializeField]
	private float _interval = 0.5f;

	[SerializeField]
	private bool _ignoreTimeScale = true;

	[SerializeField]
	private bool _activeReset;

	private float time;

	private int currentIndex;

	private UITextReplacer textReplacer;

	private bool isPausing;

	public bool isPause
	{
		get
		{
			return this.isPausing;
		}
		set
		{
			this.isPausing = value;
		}
	}

	private void OnEnable()
	{
		if (this.textReplacer == null)
		{
			this.textReplacer = base.GetComponent<UITextReplacer>();
		}
		if (this._activeReset)
		{
			this.Reset();
		}
	}

	private void Update()
	{
		if (this.isPausing)
		{
			return;
		}
		float num = (!this._ignoreTimeScale) ? Time.time : Time.unscaledTime;
		float num2 = (!this._ignoreTimeScale) ? TimeExtension.GetTimeScaleDivided(this._interval) : this._interval;
		if (num >= this.time)
		{
			this.time = num + num2;
			this.ApplyText((this.currentIndex + 1) % this.textReplacer.PatternLength);
		}
	}

	private void Reset()
	{
		this.ApplyText(0);
	}

	private void ApplyText(int index)
	{
		this.currentIndex = index;
		this.textReplacer.value = this.currentIndex;
		this.textReplacer.Apply();
	}
}
