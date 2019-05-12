using System;
using UnityEngine;

public sealed class UISpriteEx : UISprite
{
	[SerializeField]
	private string baseSpriteName = string.Empty;

	protected override void Awake()
	{
		base.Awake();
		string text = this.baseSpriteName + "_" + CountrySetting.GetCountryPrefix(CountrySetting.CountryCode.EN);
		if (base.atlas.GetSprite(text) != null)
		{
			base.spriteName = text;
		}
	}
}
