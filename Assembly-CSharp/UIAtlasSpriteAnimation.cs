using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Atlas Sprite Animation")]
[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
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
