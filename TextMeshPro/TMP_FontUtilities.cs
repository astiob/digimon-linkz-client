using System;
using System.Collections.Generic;

namespace TMPro
{
	public static class TMP_FontUtilities
	{
		public static TMP_FontAsset SearchForGlyph(TMP_FontAsset font, int character, out TMP_Glyph glyph)
		{
			glyph = null;
			if (font == null)
			{
				return null;
			}
			if (font.characterDictionary.TryGetValue(character, out glyph))
			{
				return font;
			}
			if (font.fallbackFontAssets != null && font.fallbackFontAssets.Count > 0)
			{
				int num = 0;
				while (num < font.fallbackFontAssets.Count && glyph == null)
				{
					TMP_FontAsset tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(font.fallbackFontAssets[num], character, out glyph);
					if (tmp_FontAsset != null)
					{
						return tmp_FontAsset;
					}
					num++;
				}
			}
			return null;
		}

		public static TMP_FontAsset SearchForGlyph(List<TMP_FontAsset> fonts, int character, out TMP_Glyph glyph)
		{
			glyph = null;
			if (fonts != null && fonts.Count > 0)
			{
				for (int i = 0; i < fonts.Count; i++)
				{
					TMP_FontAsset tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(fonts[i], character, out glyph);
					if (tmp_FontAsset != null)
					{
						return tmp_FontAsset;
					}
				}
			}
			return null;
		}
	}
}
