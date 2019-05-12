using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(UISprite))]
public class UIAtlasSkinner : UISpriteSkinnerBase
{
	[FormerlySerializedAs("atlas")]
	[SerializeField]
	private List<UIAtlas> _atlas = new List<UIAtlas>();

	[FormerlySerializedAs("spriteName")]
	[SerializeField]
	private List<string> _spriteName = new List<string>();

	public List<UIAtlas> atlas
	{
		get
		{
			return this._atlas;
		}
		set
		{
			this._atlas = value;
		}
	}

	public override int Length
	{
		get
		{
			return this.atlas.Count;
		}
	}

	public override void SetSlots(int index)
	{
		List<UIAtlas> list = new List<UIAtlas>();
		List<string> list2 = new List<string>();
		for (int i = 0; i < index; i++)
		{
			if (Mathf.Min(list2.Count, this.atlas.Count) > i)
			{
				list.Add(this.atlas[i]);
				list2.Add(list2[i]);
			}
			else
			{
				list.Add(null);
				list2.Add(string.Empty);
			}
		}
	}

	protected override void SetImage(int index)
	{
		((UISprite)this.uiBasicSprite).atlas = this.atlas[Mathf.Clamp(index, 0, this.atlas.Count)];
		((UISprite)this.uiBasicSprite).spriteName = this._spriteName[Mathf.Clamp(index, 0, this._spriteName.Count)];
	}
}
