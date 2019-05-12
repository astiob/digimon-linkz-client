using System;
using UnityEngine;

[RequireComponent(typeof(UIComponentSkinner))]
public class UIAnimatedProgressBar : MonoBehaviour
{
	private const float FillProgressMoveStartIntereval = 1f;

	private const float FillProgressMoveSpeed = 0.5f;

	[SerializeField]
	private UIProgressBar _fillProgressBar;

	[SerializeField]
	private UIProgressBar _differenceProgressBar;

	private UIComponentSkinner _differenceSkinner;

	[Range(0f, 1f)]
	[SerializeField]
	private float _value;

	[SerializeField]
	private float _speed = 1f;

	[SerializeField]
	private float _waitInterval = 1f;

	private float _moveStartTime;

	private void Awake()
	{
		this._differenceSkinner = base.GetComponent<UIComponentSkinner>();
		if (this._differenceSkinner.Length != 2)
		{
			this._differenceSkinner.Reset(2);
		}
		this._moveStartTime = Time.time;
	}

	public float value
	{
		get
		{
			return this._value;
		}
		set
		{
			this.SetValue(value);
		}
	}

	private void SetValue(float val)
	{
		if (this._differenceSkinner == null)
		{
			this.Awake();
		}
		if (val < this._fillProgressBar.value)
		{
			this._differenceSkinner.SetSkins(0);
		}
		else
		{
			this._differenceSkinner.SetSkins(1);
		}
		this._value = val;
		this._differenceProgressBar.value = this._value;
		this._moveStartTime = Time.time + 1f * this._waitInterval;
	}

	private void Update()
	{
		if (this._moveStartTime > Time.time)
		{
			return;
		}
		if (this._fillProgressBar.value != this._value)
		{
			float num = 0.5f * this._speed * Time.deltaTime;
			if (this._fillProgressBar.value > this._value)
			{
				this._fillProgressBar.value = Mathf.Clamp(this._fillProgressBar.value - num, this._value, this._fillProgressBar.value);
			}
			else
			{
				this._fillProgressBar.value = Mathf.Clamp(this._fillProgressBar.value + num, this._fillProgressBar.value, this._value);
			}
		}
	}

	public void Reset()
	{
		this._differenceProgressBar.value = this._value;
		this._differenceProgressBar.ForceUpdate();
		this._fillProgressBar.value = this._value;
		this._fillProgressBar.ForceUpdate();
		this.SetValue(this._value);
		this._moveStartTime = Time.time;
	}
}
