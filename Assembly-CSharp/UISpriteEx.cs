using System;
using UnityEngine;

public class UISpriteEx : UISprite
{
	[SerializeField]
	private string baseSpriteName = string.Empty;

	protected override void Awake()
	{
		base.Awake();
	}
}
