using System;
using UnityEngine;

public abstract class UIMaterialSwitcherBase : MonoBehaviour
{
	[SerializeField]
	protected int _value;

	public int value
	{
		get
		{
			return this._value;
		}
		set
		{
			this._value = Mathf.Clamp(value, 0, this.Length);
			this.ApplyContent();
		}
	}

	public abstract int Length { get; }

	protected abstract void Awake();

	public void Apply()
	{
		this.ApplyContent();
	}

	protected abstract void ApplyContent();
}
