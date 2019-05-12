using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(UISpriteSkinnerBase))]
public class UISkinnerToggle : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	private UISpriteSkinnerBase spriteSkinner;

	[SerializeField]
	private bool isEnable = true;

	private void Awake()
	{
		this.spriteSkinner = base.GetComponent<UISpriteSkinnerBase>();
	}

	public bool value
	{
		get
		{
			return this.isEnable;
		}
		set
		{
			this.isEnable = value;
			this.ApplyValue();
		}
	}

	public void ApplyValue()
	{
		if (this.spriteSkinner.Length < 2)
		{
			return;
		}
		if (this.isEnable)
		{
			this.spriteSkinner.value = 0;
		}
		else
		{
			this.spriteSkinner.value = 1;
		}
	}
}
