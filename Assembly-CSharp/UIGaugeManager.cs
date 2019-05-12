using System;
using System.Collections.Generic;
using UnityEngine;

public class UIGaugeManager : MonoBehaviour
{
	[SerializeField]
	private UIGaugeManager.GaugeType _gaugeType;

	[SerializeField]
	[Tooltip("ゲージ内部値管理")]
	private UIGaugeManager.ValueType _valueType;

	[SerializeField]
	private int _intValue;

	[SerializeField]
	private int _intMin;

	[SerializeField]
	private int _intMax = 100;

	[SerializeField]
	private float _floatValue;

	[SerializeField]
	private UIProgressBar _uiProgressBar;

	[SerializeField]
	private UIImageGauge _uiImageGauge;

	[SerializeField]
	private UIAnimatedProgressBar _uiAnimatedProgressBar;

	[SerializeField]
	private List<UIGaugeManager> _uiMultipleGaugeManager = new List<UIGaugeManager>();

	public void SetValue(int value)
	{
		if (this._valueType != UIGaugeManager.ValueType.Integer)
		{
			global::Debug.LogWarning("ゲージ内部値モードが整数値ではありません.");
		}
		this._intValue = Mathf.Clamp(value, this._intMin, this._intMax + 1);
		this.Reflesh();
	}

	public void SetValue(float value)
	{
		if (this._valueType != UIGaugeManager.ValueType.Float)
		{
			global::Debug.LogWarning("ゲージ内部値モードが少数値ではありません.");
		}
		this._floatValue = Mathf.Clamp01(value);
		this.Reflesh();
	}

	public void SetMin(int value)
	{
		if (this._valueType != UIGaugeManager.ValueType.Integer)
		{
			global::Debug.LogWarning("ゲージ内部値モードが整数値ではありません.");
		}
		this._intMin = value;
		this.SetValue(this._intValue);
	}

	public void SetMax(int value)
	{
		if (this._valueType != UIGaugeManager.ValueType.Integer)
		{
			global::Debug.LogWarning("ゲージ内部値モードが整数値ではありません.");
		}
		this._intMax = value;
		this.SetValue(this._intValue);
	}

	public float GetValue()
	{
		UIGaugeManager.ValueType valueType = this._valueType;
		if (valueType != UIGaugeManager.ValueType.Integer)
		{
			return this._floatValue;
		}
		return (float)this._intValue;
	}

	public int GetValueToInt(float multiple = 100f, bool onFloor = false, bool onCeil = false)
	{
		UIGaugeManager.ValueType valueType = this._valueType;
		if (valueType == UIGaugeManager.ValueType.Integer)
		{
			return this._intValue;
		}
		if (onFloor)
		{
			return Mathf.FloorToInt(this._floatValue * multiple);
		}
		if (onCeil)
		{
			return Mathf.CeilToInt(this._floatValue * multiple);
		}
		return Mathf.RoundToInt(this._floatValue * multiple);
	}

	public float GetValueToFloat()
	{
		return this.GetValue();
	}

	public float GetValueToNormalize()
	{
		UIGaugeManager.ValueType valueType = this._valueType;
		if (valueType != UIGaugeManager.ValueType.Integer)
		{
			return this._floatValue;
		}
		return Mathf.Clamp01((float)(this._intValue - this._intMin) / (float)(this._intMax - this._intMin));
	}

	public void Reflesh()
	{
		UIGaugeManager.ValueType valueType = this._valueType;
		if (valueType != UIGaugeManager.ValueType.Integer)
		{
			if (valueType == UIGaugeManager.ValueType.Float)
			{
				if (this._uiProgressBar != null && this._gaugeType == UIGaugeManager.GaugeType.ProgressBar)
				{
					this._uiProgressBar.value = this._floatValue;
					this._uiProgressBar.ForceUpdate();
				}
				if (this._uiImageGauge != null && this._gaugeType == UIGaugeManager.GaugeType.ImageGauge)
				{
					this._uiImageGauge.value = Mathf.RoundToInt(Mathf.Lerp((float)this._uiImageGauge.min, (float)this._uiImageGauge.max, this._floatValue));
				}
				if (this._uiAnimatedProgressBar != null && this._gaugeType == UIGaugeManager.GaugeType.AnimatedProgressBar)
				{
					this._uiAnimatedProgressBar.value = this._floatValue;
				}
				if (this._uiMultipleGaugeManager.Count > 0 && this._gaugeType == UIGaugeManager.GaugeType.MultipleGaugeManager)
				{
					for (int i = 0; i < this._uiMultipleGaugeManager.Count; i++)
					{
						if (!(this._uiMultipleGaugeManager[i] == null))
						{
							this._uiMultipleGaugeManager[i].SetValue(this._floatValue);
						}
					}
				}
			}
		}
		else
		{
			if (this._uiProgressBar != null && this._gaugeType == UIGaugeManager.GaugeType.ProgressBar)
			{
				this._uiProgressBar.value = Mathf.Clamp01((float)this._intValue / (float)this._intMax);
				this._uiProgressBar.ForceUpdate();
			}
			if (this._uiImageGauge != null && this._gaugeType == UIGaugeManager.GaugeType.ImageGauge)
			{
				this._uiImageGauge.min = this._intMin;
				this._uiImageGauge.max = this._intMax;
				this._uiImageGauge.value = this._intValue;
			}
			if (this._uiAnimatedProgressBar != null && this._gaugeType == UIGaugeManager.GaugeType.AnimatedProgressBar)
			{
				this._uiAnimatedProgressBar.value = Mathf.Clamp01((float)this._intValue / (float)this._intMax);
			}
			if (this._uiMultipleGaugeManager.Count > 0 && this._gaugeType == UIGaugeManager.GaugeType.MultipleGaugeManager)
			{
				for (int j = 0; j < this._uiMultipleGaugeManager.Count; j++)
				{
					if (!(this._uiMultipleGaugeManager[j] == null))
					{
						this._uiMultipleGaugeManager[j].SetMin(this._intMin);
						this._uiMultipleGaugeManager[j].SetMax(this._intMax);
						this._uiMultipleGaugeManager[j].SetValue(this._intValue);
					}
				}
			}
		}
	}

	public UIGaugeManager.GaugeType gaugeType
	{
		get
		{
			return this._gaugeType;
		}
	}

	public UIGaugeManager.ValueType valueType
	{
		get
		{
			return this._valueType;
		}
		set
		{
			this._valueType = value;
		}
	}

	public void Reset()
	{
		if (this._uiAnimatedProgressBar != null && this._gaugeType == UIGaugeManager.GaugeType.AnimatedProgressBar)
		{
			this._uiAnimatedProgressBar.Reset();
		}
		if (this._uiMultipleGaugeManager.Count > 0 && this._gaugeType == UIGaugeManager.GaugeType.MultipleGaugeManager)
		{
			for (int i = 0; i < this._uiMultipleGaugeManager.Count; i++)
			{
				if (!(this._uiMultipleGaugeManager[i] == null))
				{
					this._uiMultipleGaugeManager[i].Reset();
				}
			}
		}
	}

	public enum GaugeType
	{
		ProgressBar,
		ImageGauge,
		AnimatedProgressBar,
		MultipleGaugeManager
	}

	public enum ValueType
	{
		Integer,
		Float
	}
}
