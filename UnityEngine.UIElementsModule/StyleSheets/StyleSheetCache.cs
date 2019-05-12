using System;
using System.Collections.Generic;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
	internal static class StyleSheetCache
	{
		private static StyleSheetCache.SheetHandleKeyComparer s_Comparer = new StyleSheetCache.SheetHandleKeyComparer();

		private static Dictionary<StyleSheetCache.SheetHandleKey, int> s_EnumToIntCache = new Dictionary<StyleSheetCache.SheetHandleKey, int>(StyleSheetCache.s_Comparer);

		private static Dictionary<StyleSheetCache.SheetHandleKey, StylePropertyID[]> s_RulePropertyIDsCache = new Dictionary<StyleSheetCache.SheetHandleKey, StylePropertyID[]>(StyleSheetCache.s_Comparer);

		private static Dictionary<string, StylePropertyID> s_NameToIDCache = new Dictionary<string, StylePropertyID>
		{
			{
				"width",
				StylePropertyID.Width
			},
			{
				"height",
				StylePropertyID.Height
			},
			{
				"max-width",
				StylePropertyID.MaxWidth
			},
			{
				"max-height",
				StylePropertyID.MaxHeight
			},
			{
				"min-width",
				StylePropertyID.MinWidth
			},
			{
				"min-height",
				StylePropertyID.MinHeight
			},
			{
				"flex",
				StylePropertyID.Flex
			},
			{
				"overflow",
				StylePropertyID.Overflow
			},
			{
				"position-left",
				StylePropertyID.PositionLeft
			},
			{
				"position-top",
				StylePropertyID.PositionTop
			},
			{
				"position-right",
				StylePropertyID.PositionRight
			},
			{
				"position-bottom",
				StylePropertyID.PositionBottom
			},
			{
				"margin-left",
				StylePropertyID.MarginLeft
			},
			{
				"margin-top",
				StylePropertyID.MarginTop
			},
			{
				"margin-right",
				StylePropertyID.MarginRight
			},
			{
				"margin-bottom",
				StylePropertyID.MarginBottom
			},
			{
				"border-left",
				StylePropertyID.BorderLeft
			},
			{
				"border-top",
				StylePropertyID.BorderTop
			},
			{
				"border-right",
				StylePropertyID.BorderRight
			},
			{
				"border-bottom",
				StylePropertyID.BorderBottom
			},
			{
				"padding-left",
				StylePropertyID.PaddingLeft
			},
			{
				"padding-top",
				StylePropertyID.PaddingTop
			},
			{
				"padding-right",
				StylePropertyID.PaddingRight
			},
			{
				"padding-bottom",
				StylePropertyID.PaddingBottom
			},
			{
				"position-type",
				StylePropertyID.PositionType
			},
			{
				"align-self",
				StylePropertyID.AlignSelf
			},
			{
				"text-alignment",
				StylePropertyID.TextAlignment
			},
			{
				"font-style",
				StylePropertyID.FontStyle
			},
			{
				"text-clipping",
				StylePropertyID.TextClipping
			},
			{
				"font",
				StylePropertyID.Font
			},
			{
				"font-size",
				StylePropertyID.FontSize
			},
			{
				"word-wrap",
				StylePropertyID.WordWrap
			},
			{
				"text-color",
				StylePropertyID.TextColor
			},
			{
				"flex-direction",
				StylePropertyID.FlexDirection
			},
			{
				"background-color",
				StylePropertyID.BackgroundColor
			},
			{
				"border-color",
				StylePropertyID.BorderColor
			},
			{
				"background-image",
				StylePropertyID.BackgroundImage
			},
			{
				"background-size",
				StylePropertyID.BackgroundSize
			},
			{
				"align-items",
				StylePropertyID.AlignItems
			},
			{
				"align-content",
				StylePropertyID.AlignContent
			},
			{
				"justify-content",
				StylePropertyID.JustifyContent
			},
			{
				"flex-wrap",
				StylePropertyID.FlexWrap
			},
			{
				"border-left-width",
				StylePropertyID.BorderLeftWidth
			},
			{
				"border-top-width",
				StylePropertyID.BorderTopWidth
			},
			{
				"border-right-width",
				StylePropertyID.BorderRightWidth
			},
			{
				"border-bottom-width",
				StylePropertyID.BorderBottomWidth
			},
			{
				"border-radius",
				StylePropertyID.BorderRadius
			},
			{
				"border-top-left-radius",
				StylePropertyID.BorderTopLeftRadius
			},
			{
				"border-top-right-radius",
				StylePropertyID.BorderTopRightRadius
			},
			{
				"border-bottom-right-radius",
				StylePropertyID.BorderBottomRightRadius
			},
			{
				"border-bottom-left-radius",
				StylePropertyID.BorderBottomLeftRadius
			},
			{
				"slice-left",
				StylePropertyID.SliceLeft
			},
			{
				"slice-top",
				StylePropertyID.SliceTop
			},
			{
				"slice-right",
				StylePropertyID.SliceRight
			},
			{
				"slice-bottom",
				StylePropertyID.SliceBottom
			},
			{
				"opacity",
				StylePropertyID.Opacity
			}
		};

		internal static void ClearCaches()
		{
			StyleSheetCache.s_EnumToIntCache.Clear();
			StyleSheetCache.s_RulePropertyIDsCache.Clear();
		}

		internal static int GetEnumValue<T>(StyleSheet sheet, StyleValueHandle handle)
		{
			Debug.Assert(handle.valueType == StyleValueType.Enum);
			StyleSheetCache.SheetHandleKey key = new StyleSheetCache.SheetHandleKey(sheet, handle.valueIndex);
			int num;
			if (!StyleSheetCache.s_EnumToIntCache.TryGetValue(key, out num))
			{
				string value = sheet.ReadEnum(handle).Replace("-", string.Empty);
				object obj = Enum.Parse(typeof(T), value, true);
				num = (int)obj;
				StyleSheetCache.s_EnumToIntCache.Add(key, num);
			}
			Debug.Assert(Enum.GetName(typeof(T), num) != null);
			return num;
		}

		internal static StylePropertyID[] GetPropertyIDs(StyleSheet sheet, int ruleIndex)
		{
			StyleSheetCache.SheetHandleKey key = new StyleSheetCache.SheetHandleKey(sheet, ruleIndex);
			StylePropertyID[] array;
			if (!StyleSheetCache.s_RulePropertyIDsCache.TryGetValue(key, out array))
			{
				StyleRule styleRule = sheet.rules[ruleIndex];
				array = new StylePropertyID[styleRule.properties.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = StyleSheetCache.GetPropertyID(styleRule.properties[i].name);
				}
				StyleSheetCache.s_RulePropertyIDsCache.Add(key, array);
			}
			return array;
		}

		private static StylePropertyID GetPropertyID(string name)
		{
			StylePropertyID result;
			if (!StyleSheetCache.s_NameToIDCache.TryGetValue(name, out result))
			{
				result = StylePropertyID.Custom;
			}
			return result;
		}

		private struct SheetHandleKey
		{
			public readonly int sheetInstanceID;

			public readonly int index;

			public SheetHandleKey(StyleSheet sheet, int index)
			{
				this.sheetInstanceID = sheet.GetInstanceID();
				this.index = index;
			}
		}

		private class SheetHandleKeyComparer : IEqualityComparer<StyleSheetCache.SheetHandleKey>
		{
			public bool Equals(StyleSheetCache.SheetHandleKey x, StyleSheetCache.SheetHandleKey y)
			{
				return x.sheetInstanceID == y.sheetInstanceID && x.index == y.index;
			}

			public int GetHashCode(StyleSheetCache.SheetHandleKey key)
			{
				return key.sheetInstanceID.GetHashCode() ^ key.index.GetHashCode();
			}
		}
	}
}
