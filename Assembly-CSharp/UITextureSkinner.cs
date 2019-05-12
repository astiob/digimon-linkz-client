using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UITexture))]
public class UITextureSkinner : UISpriteSkinnerBase
{
	[SerializeField]
	private List<Texture> texture = new List<Texture>();

	public override int Length
	{
		get
		{
			return this.texture.Count;
		}
	}

	public override void SetSlots(int index)
	{
		List<Texture> list = new List<Texture>();
		for (int i = 0; i < index; i++)
		{
			if (this.texture.Count > i)
			{
				list.Add(this.texture[i]);
			}
			else
			{
				list.Add(null);
			}
		}
	}

	protected override void SetImage(int index)
	{
		((UITexture)this.uiBasicSprite).mainTexture = this.texture[Mathf.Clamp(index, 0, this.texture.Count)];
	}
}
