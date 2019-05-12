using System;
using UnityEngine;

[RequireComponent(typeof(UIBasicSprite))]
public abstract class UISpriteSkinnerBase : MonoBehaviour
{
	[SerializeField]
	protected int _value;

	protected UIBasicSprite uiBasicSprite;

	private void Awake()
	{
		this.uiBasicSprite = base.GetComponent<UIBasicSprite>();
	}

	protected void GetSprite()
	{
		if (this.uiBasicSprite == null)
		{
			this.uiBasicSprite = base.GetComponent<UIBasicSprite>();
		}
	}

	private void OnEnable()
	{
		this.GetSprite();
	}

	public abstract int Length { get; }

	public abstract void SetSlots(int index);

	public int GetSlots()
	{
		return this.Length;
	}

	public int value
	{
		get
		{
			return this._value;
		}
		set
		{
			this.GetSprite();
			this._value = Mathf.Clamp(value, 0, this.Length);
			this.SetImage(this._value);
		}
	}

	protected abstract void SetImage(int index);
}
