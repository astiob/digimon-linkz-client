using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UISprite))]
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Atlas Sprite Animation")]
public class UIAtlasSpriteAnimation : CustomUISpriteAnimation
{
	[SerializeField]
	private List<string> mSpriteName = new List<string>();

	public override void RebuildSpriteList()
	{
		if (this.mSprite == null)
		{
			this.mSprite = base.GetComponent<UISprite>();
		}
		this.mSpriteNames.Clear();
		if (this.mSprite != null && this.mSprite.atlas != null)
		{
			this.mSpriteNames = this.mSpriteName;
		}
	}
}
