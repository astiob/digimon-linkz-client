using System;
using UnityEngine;
using UnityExtension;

public sealed class UIMaterialSwitcher : UIMaterialSwitcherBase
{
	[SerializeField]
	private Material[] _uiMaterial = new Material[1];

	private UIBasicSprite cachedBasicSprite;

	private Material currentMaterial
	{
		get
		{
			return this._uiMaterial[base.value];
		}
	}

	public override int Length
	{
		get
		{
			return this._uiMaterial.Length;
		}
	}

	protected override void Awake()
	{
		if (this.cachedBasicSprite == null)
		{
			this.cachedBasicSprite = GameObjectExtension.GetComponentEvenIfDeactive<UIBasicSprite>(base.gameObject);
		}
	}

	protected override void ApplyContent()
	{
		this.Awake();
		if (this.cachedBasicSprite != null)
		{
			this.cachedBasicSprite.material = this.currentMaterial;
			this.cachedBasicSprite.Update();
		}
	}
}
