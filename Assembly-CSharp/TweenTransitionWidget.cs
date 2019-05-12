using System;
using System.Collections.Generic;
using UnityEngine;

public class TweenTransitionWidget : UITweener
{
	[Range(0f, 1f)]
	public float from = 1f;

	[Range(0f, 1f)]
	public float to = 1f;

	[SerializeField]
	private List<UIWidget> _widgets = new List<UIWidget>();

	[SerializeField]
	private int _max;

	[SerializeField]
	private int _index;

	private float currentFactor;

	public int max
	{
		get
		{
			return this._max;
		}
		set
		{
			this._max = value;
		}
	}

	public int fixableMax
	{
		get
		{
			return this._widgets.Count;
		}
	}

	public void Reset(int index = 0)
	{
		this._index = index;
		base.ResetToBeginning();
		this.UpdateActiveWidgets();
	}

	protected override void Start()
	{
		base.Start();
		this.UpdateActiveWidgets();
	}

	private bool isFindWidget
	{
		get
		{
			return this._widgets.Count > 0 && this._index < this._widgets.Count && this._widgets[this._index] != null;
		}
	}

	public float value
	{
		get
		{
			return (this._widgets.Count <= 0 || this._index >= this._widgets.Count || !(this._widgets[this._index] != null)) ? 0f : this._widgets[this._index].alpha;
		}
		set
		{
			if (this._widgets.Count > 0 && this._index < this._widgets.Count && this._widgets[this._index] != null)
			{
				this._widgets[this._index].alpha = value;
			}
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Mathf.Lerp(this.from, this.to, factor);
		this.currentFactor = ((base.tweenFactor <= this.currentFactor) ? (this.currentFactor + ((!this.ignoreTimeScale) ? Time.deltaTime : Time.unscaledDeltaTime)) : base.tweenFactor);
		if (this.currentFactor >= 1f)
		{
			this._index = ((this._max != 0) ? ((this._index + 1) % this._max) : 0);
			this.UpdateActiveWidgets();
			this.currentFactor = base.tweenFactor;
		}
	}

	private void UpdateActiveWidgets()
	{
		for (int i = 0; i < this._widgets.Count; i++)
		{
			if (i != this._index)
			{
				this._widgets[i].alpha = 0f;
			}
			NGUITools.SetActiveSelf(this._widgets[i].gameObject, i == this._index);
		}
	}

	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	public override void ApplyTweenFromValue()
	{
		this.value = this.from;
	}

	public override void ApplyTweenToValue()
	{
		this.value = this.to;
	}
}
