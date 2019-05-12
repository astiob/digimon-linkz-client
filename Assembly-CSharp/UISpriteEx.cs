using System;
using UnityEngine;

public sealed class UISpriteEx : UISprite
{
	[SerializeField]
	private string baseSpriteName = string.Empty;

	protected override void Awake()
	{
		base.Awake();
	}
}
