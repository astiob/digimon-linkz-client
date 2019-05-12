using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UI2DSprite))]
public class UISpriteSkinner : UISpriteSkinnerBase
{
	[SerializeField]
	private List<Sprite> sprite = new List<Sprite>();

	public override int Length
	{
		get
		{
			return this.sprite.Count;
		}
	}

	public override void SetSlots(int index)
	{
		List<Sprite> list = new List<Sprite>();
		for (int i = 0; i < index; i++)
		{
			if (this.sprite.Count > i)
			{
				list.Add(this.sprite[i]);
			}
			else
			{
				list.Add(null);
			}
		}
	}

	protected override void SetImage(int index)
	{
		((UI2DSprite)this.uiBasicSprite).sprite2D = this.sprite[Mathf.Clamp(index, 0, this.sprite.Count)];
	}
}
