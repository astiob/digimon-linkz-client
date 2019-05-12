using System;
using UnityEngine;

public class UIImageGauge : MonoBehaviour
{
	[SerializeField]
	private int _value;

	[SerializeField]
	private int _min;

	[SerializeField]
	private int _max = 10;

	[SerializeField]
	private bool inverseVector;

	[SerializeField]
	private bool inverseEnable;

	[SerializeField]
	private UIWidget[] _images = new UIWidget[0];

	[SerializeField]
	private UIWidget[] _disableImages = new UIWidget[0];

	public int value
	{
		get
		{
			return this._value;
		}
		set
		{
			this._value = value;
			this._value = Mathf.Clamp(this._value, this._min, this._max + 1);
			this.Reflesh();
		}
	}

	public int min
	{
		get
		{
			return this._min;
		}
		set
		{
			this._min = value;
			this._value = Mathf.Clamp(this._value, this._min, this._max);
			this.Reflesh();
		}
	}

	public int max
	{
		get
		{
			return this._max;
		}
		set
		{
			this._max = value;
			this._value = Mathf.Clamp(this._value, this._min, this._max);
			this.Reflesh();
		}
	}

	public void Reflesh()
	{
		int num = 0;
		int num2 = 1;
		if (this.inverseVector)
		{
			num = Mathf.Max(this._images.Length, this._disableImages.Length) - 1;
			num2 = -1;
		}
		for (int i = 0; i < Mathf.Max(this._images.Length, this._disableImages.Length); i++)
		{
			if (i < Mathf.RoundToInt(Mathf.Lerp(0f, (float)this._images.Length, Mathf.Clamp01((float)this._value / (float)this._max))))
			{
				if (this._images.Length > num2 * i + num && this._images[num2 * i + num] != null)
				{
					NGUITools.SetActiveSelf(this._images[num2 * i + num].gameObject, !this.inverseEnable);
				}
				if (this._disableImages.Length > num2 * i + num && this._disableImages[num2 * i + num] != null)
				{
					NGUITools.SetActiveSelf(this._disableImages[num2 * i + num].gameObject, this.inverseEnable);
				}
			}
			else
			{
				if (this._images.Length > num2 * i + num && this._images[num2 * i + num] != null)
				{
					NGUITools.SetActiveSelf(this._images[num2 * i + num].gameObject, this.inverseEnable);
				}
				if (this._disableImages.Length > num2 * i + num && this._disableImages[num2 * i + num] != null)
				{
					NGUITools.SetActiveSelf(this._disableImages[num2 * i + num].gameObject, !this.inverseEnable);
				}
			}
		}
	}
}
