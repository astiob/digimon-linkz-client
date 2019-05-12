using System;
using UnityEngine;

public class LightColorChanger : MonoBehaviour
{
	[SerializeField]
	private Color[] _color = new Color[]
	{
		Color.white
	};

	[SerializeField]
	private float _speed = 1f;

	[NonSerialized]
	private Light _light;

	private int _currentColor = 1;

	private float _lerpLevel = 1f;

	private bool _isEnable;

	private Color _defaultColor = Color.white;

	public void SetLight(Light light)
	{
		this._light = light;
		this.GetLightDefaultColor();
	}

	public bool isEnable
	{
		get
		{
			return this._isEnable;
		}
		set
		{
			if (value)
			{
				this.GetLightDefaultColor();
			}
			this._isEnable = value;
		}
	}

	public void SetColors(params Color[] colors)
	{
		this._color = colors;
		this.ResetColor();
	}

	public void ResetColor()
	{
		this._currentColor = this._color.Length - 1;
		this._lerpLevel = 0f;
		this.LateUpdate();
	}

	public float speed
	{
		get
		{
			return this._speed;
		}
		set
		{
			this._speed = value;
		}
	}

	public Light light
	{
		get
		{
			return this._light;
		}
	}

	private void OnEnable()
	{
		if (this._light == null && base.GetComponent<Light>())
		{
			this._light = base.GetComponent<Light>();
			this.GetLightDefaultColor();
			this.ResetColor();
		}
	}

	private void GetLightDefaultColor()
	{
		if (this._light != null)
		{
			this._defaultColor = this._light.color;
		}
	}

	private void LateUpdate()
	{
		if (this._light == null)
		{
			return;
		}
		if (!this._isEnable)
		{
			this._light.color = this._defaultColor;
			return;
		}
		this._lerpLevel += Time.deltaTime * this._speed;
		if (this._lerpLevel >= 1f)
		{
			this._currentColor = (this._currentColor + 1) % this._color.Length;
		}
		this._lerpLevel = Mathf.Repeat(this._lerpLevel, 1f);
		this._light.color = Color.Lerp(this._color[(this._currentColor - 1 >= 0) ? ((this._currentColor - 1) % this._color.Length) : (this._color.Length - 1)], this._color[this._currentColor], this._lerpLevel);
	}
}
