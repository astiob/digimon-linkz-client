using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class UITimeStop : MonoBehaviour
{
	private Animator _animator;

	private float animatorSpeed;

	private UITweener[] _uiTweeners = new UITweener[0];

	private bool[] ignoreTimeScales = new bool[0];

	private UITextAnimation _uiTextAnimation;

	private float? previousTimeScale;

	private void GetComponents()
	{
		this._animator = base.GetComponent<Animator>();
		if (this._animator != null)
		{
			this.animatorSpeed = this._animator.speed;
		}
		this._uiTweeners = base.GetComponents<UITweener>();
		this.ignoreTimeScales = new bool[this._uiTweeners.Length];
		for (int i = 0; i < this._uiTweeners.Length; i++)
		{
			if (this._uiTweeners[i] != null)
			{
				this.ignoreTimeScales[i] = this._uiTweeners[i].ignoreTimeScale;
			}
		}
		this._uiTextAnimation = base.GetComponent<UITextAnimation>();
	}

	private void ControlEnabled(bool enabled)
	{
		if (this._animator != null)
		{
			if (enabled)
			{
				this._animator.speed = this.animatorSpeed;
			}
			else
			{
				this.animatorSpeed = this._animator.speed;
				this._animator.speed = 0f;
			}
		}
		for (int i = 0; i < this._uiTweeners.Length; i++)
		{
			if (this._uiTweeners[i] != null)
			{
				if (enabled)
				{
					this._uiTweeners[i].ignoreTimeScale = this.ignoreTimeScales[i];
				}
				else
				{
					this.ignoreTimeScales[i] = this._uiTweeners[i].ignoreTimeScale;
					this._uiTweeners[i].ignoreTimeScale = false;
				}
			}
		}
		if (this._uiTextAnimation != null)
		{
			this._uiTextAnimation.isPause = !enabled;
		}
	}

	private void OnEnable()
	{
		this.GetComponents();
	}

	private void Start()
	{
		if (Time.timeScale == 0f)
		{
			this.ControlEnabled(false);
		}
		else
		{
			this.ControlEnabled(true);
		}
		this.previousTimeScale = new float?(Time.timeScale);
	}

	private void Update()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.previousTimeScale == null || Time.timeScale == this.previousTimeScale.Value)
		{
			return;
		}
		if (Time.timeScale == 0f)
		{
			this.ControlEnabled(false);
		}
		else
		{
			this.ControlEnabled(true);
		}
		this.previousTimeScale = new float?(Time.timeScale);
	}
}
