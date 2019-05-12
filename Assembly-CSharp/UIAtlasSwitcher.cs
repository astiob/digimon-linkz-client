using System;
using System.Collections.Generic;
using UnityEngine;
using UnityExtension;

public sealed class UIAtlasSwitcher : UIMaterialSwitcherBase
{
	[SerializeField]
	private UIAtlas[] _uiAtlas = new UIAtlas[1];

	private UISprite cachedAtlas;

	private UIAtlasSkinner cachedAtlasSkinner;

	private UIAtlas currentAtlas
	{
		get
		{
			return this._uiAtlas[base.value];
		}
	}

	public override int Length
	{
		get
		{
			return this._uiAtlas.Length;
		}
	}

	protected override void Awake()
	{
		if (this.cachedAtlasSkinner == null)
		{
			this.cachedAtlasSkinner = GameObjectExtension.GetComponentEvenIfDeactive<UIAtlasSkinner>(base.gameObject);
		}
		if (this.cachedAtlas == null)
		{
			this.cachedAtlas = GameObjectExtension.GetComponentEvenIfDeactive<UISprite>(base.gameObject);
		}
	}

	protected override void ApplyContent()
	{
		this.Awake();
		if (this.cachedAtlas != null)
		{
			this.cachedAtlas.atlas = this.currentAtlas;
			this.cachedAtlas.Update();
		}
		if (this.cachedAtlasSkinner != null)
		{
			List<UIAtlas> list = new List<UIAtlas>();
			for (int i = 0; i < this.cachedAtlasSkinner.atlas.Count; i++)
			{
				list.Add(this.currentAtlas);
			}
			this.cachedAtlasSkinner.atlas = list;
			this.cachedAtlasSkinner.value = this.cachedAtlasSkinner.value;
		}
	}
}
