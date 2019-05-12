using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
	internal class VisualElementStylesData : ICustomStyle
	{
		public static VisualElementStylesData none = new VisualElementStylesData(true);

		internal readonly bool isShared;

		private Dictionary<string, CustomProperty> m_CustomProperties;

		internal StyleValue<float> width;

		internal StyleValue<float> height;

		internal StyleValue<float> maxWidth;

		internal StyleValue<float> maxHeight;

		internal StyleValue<float> minWidth;

		internal StyleValue<float> minHeight;

		internal StyleValue<float> flex;

		internal StyleValue<int> overflow;

		internal StyleValue<float> positionLeft;

		internal StyleValue<float> positionTop;

		internal StyleValue<float> positionRight;

		internal StyleValue<float> positionBottom;

		internal StyleValue<float> marginLeft;

		internal StyleValue<float> marginTop;

		internal StyleValue<float> marginRight;

		internal StyleValue<float> marginBottom;

		internal StyleValue<float> borderLeft;

		internal StyleValue<float> borderTop;

		internal StyleValue<float> borderRight;

		internal StyleValue<float> borderBottom;

		internal StyleValue<float> paddingLeft;

		internal StyleValue<float> paddingTop;

		internal StyleValue<float> paddingRight;

		internal StyleValue<float> paddingBottom;

		internal StyleValue<int> positionType;

		internal StyleValue<int> alignSelf;

		internal StyleValue<int> textAlignment;

		internal StyleValue<int> fontStyle;

		internal StyleValue<int> textClipping;

		internal StyleValue<Font> font;

		internal StyleValue<int> fontSize;

		internal StyleValue<bool> wordWrap;

		internal StyleValue<Color> textColor;

		internal StyleValue<int> flexDirection;

		internal StyleValue<Color> backgroundColor;

		internal StyleValue<Color> borderColor;

		internal StyleValue<Texture2D> backgroundImage;

		internal StyleValue<int> backgroundSize;

		internal StyleValue<int> alignItems;

		internal StyleValue<int> alignContent;

		internal StyleValue<int> justifyContent;

		internal StyleValue<int> flexWrap;

		internal StyleValue<float> borderLeftWidth;

		internal StyleValue<float> borderTopWidth;

		internal StyleValue<float> borderRightWidth;

		internal StyleValue<float> borderBottomWidth;

		internal StyleValue<float> borderTopLeftRadius;

		internal StyleValue<float> borderTopRightRadius;

		internal StyleValue<float> borderBottomRightRadius;

		internal StyleValue<float> borderBottomLeftRadius;

		internal StyleValue<int> sliceLeft;

		internal StyleValue<int> sliceTop;

		internal StyleValue<int> sliceRight;

		internal StyleValue<int> sliceBottom;

		internal StyleValue<float> opacity;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache0;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache1;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache2;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<Texture2D> <>f__mg$cache3;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache4;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache5;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache6;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache7;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache8;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<Font> <>f__mg$cache9;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cacheA;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cacheB;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cacheC;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cacheD;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cacheE;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cacheF;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache10;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache11;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache12;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache13;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache14;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache15;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache16;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache17;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache18;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache19;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache1A;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache1B;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache1C;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache1D;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache1E;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache1F;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache20;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache21;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache22;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache23;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<Color> <>f__mg$cache24;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache25;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<bool> <>f__mg$cache26;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<Color> <>f__mg$cache27;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache28;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<Color> <>f__mg$cache29;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache2A;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache2B;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache2C;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache2D;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache2E;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache2F;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache30;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache31;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache32;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache33;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache34;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache35;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache36;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache37;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache38;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache39;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache3A;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache3B;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache3C;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<bool> <>f__mg$cache3D;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<Color> <>f__mg$cache3E;

		public VisualElementStylesData(bool isShared)
		{
			this.isShared = isShared;
		}

		public void Apply(VisualElementStylesData other, StylePropertyApplyMode mode)
		{
			this.m_CustomProperties = other.m_CustomProperties;
			this.width.Apply(other.width, mode);
			this.height.Apply(other.height, mode);
			this.maxWidth.Apply(other.maxWidth, mode);
			this.maxHeight.Apply(other.maxHeight, mode);
			this.minWidth.Apply(other.minWidth, mode);
			this.minHeight.Apply(other.minHeight, mode);
			this.flex.Apply(other.flex, mode);
			this.overflow.Apply(other.overflow, mode);
			this.positionLeft.Apply(other.positionLeft, mode);
			this.positionTop.Apply(other.positionTop, mode);
			this.positionRight.Apply(other.positionRight, mode);
			this.positionBottom.Apply(other.positionBottom, mode);
			this.marginLeft.Apply(other.marginLeft, mode);
			this.marginTop.Apply(other.marginTop, mode);
			this.marginRight.Apply(other.marginRight, mode);
			this.marginBottom.Apply(other.marginBottom, mode);
			this.borderLeft.Apply(other.borderLeft, mode);
			this.borderTop.Apply(other.borderTop, mode);
			this.borderRight.Apply(other.borderRight, mode);
			this.borderBottom.Apply(other.borderBottom, mode);
			this.paddingLeft.Apply(other.paddingLeft, mode);
			this.paddingTop.Apply(other.paddingTop, mode);
			this.paddingRight.Apply(other.paddingRight, mode);
			this.paddingBottom.Apply(other.paddingBottom, mode);
			this.positionType.Apply(other.positionType, mode);
			this.alignSelf.Apply(other.alignSelf, mode);
			this.textAlignment.Apply(other.textAlignment, mode);
			this.fontStyle.Apply(other.fontStyle, mode);
			this.textClipping.Apply(other.textClipping, mode);
			this.fontSize.Apply(other.fontSize, mode);
			this.font.Apply(other.font, mode);
			this.wordWrap.Apply(other.wordWrap, mode);
			this.textColor.Apply(other.textColor, mode);
			this.flexDirection.Apply(other.flexDirection, mode);
			this.backgroundColor.Apply(other.backgroundColor, mode);
			this.borderColor.Apply(other.borderColor, mode);
			this.backgroundImage.Apply(other.backgroundImage, mode);
			this.backgroundSize.Apply(other.backgroundSize, mode);
			this.alignItems.Apply(other.alignItems, mode);
			this.alignContent.Apply(other.alignContent, mode);
			this.justifyContent.Apply(other.justifyContent, mode);
			this.flexWrap.Apply(other.flexWrap, mode);
			this.borderLeftWidth.Apply(other.borderLeftWidth, mode);
			this.borderTopWidth.Apply(other.borderTopWidth, mode);
			this.borderRightWidth.Apply(other.borderRightWidth, mode);
			this.borderBottomWidth.Apply(other.borderBottomWidth, mode);
			this.borderTopLeftRadius.Apply(other.borderTopLeftRadius, mode);
			this.borderTopRightRadius.Apply(other.borderTopRightRadius, mode);
			this.borderBottomRightRadius.Apply(other.borderBottomRightRadius, mode);
			this.borderBottomLeftRadius.Apply(other.borderBottomLeftRadius, mode);
			this.sliceLeft.Apply(other.sliceLeft, mode);
			this.sliceTop.Apply(other.sliceTop, mode);
			this.sliceRight.Apply(other.sliceRight, mode);
			this.sliceBottom.Apply(other.sliceBottom, mode);
			this.opacity.Apply(other.opacity, mode);
		}

		public void WriteToGUIStyle(GUIStyle style)
		{
			style.alignment = (TextAnchor)this.textAlignment.GetSpecifiedValueOrDefault((int)style.alignment);
			style.wordWrap = this.wordWrap.GetSpecifiedValueOrDefault(style.wordWrap);
			style.clipping = (TextClipping)this.textClipping.GetSpecifiedValueOrDefault((int)style.clipping);
			if (this.font.value != null)
			{
				style.font = this.font.value;
			}
			style.fontSize = this.fontSize.GetSpecifiedValueOrDefault(style.fontSize);
			style.fontStyle = (FontStyle)this.fontStyle.GetSpecifiedValueOrDefault((int)style.fontStyle);
			this.AssignRect(style.margin, ref this.marginLeft, ref this.marginTop, ref this.marginRight, ref this.marginBottom);
			this.AssignRect(style.padding, ref this.paddingLeft, ref this.paddingTop, ref this.paddingRight, ref this.paddingBottom);
			this.AssignRect(style.border, ref this.sliceLeft, ref this.sliceTop, ref this.sliceRight, ref this.sliceBottom);
			this.AssignState(style.normal);
			this.AssignState(style.focused);
			this.AssignState(style.hover);
			this.AssignState(style.active);
			this.AssignState(style.onNormal);
			this.AssignState(style.onFocused);
			this.AssignState(style.onHover);
			this.AssignState(style.onActive);
		}

		private void AssignState(GUIStyleState state)
		{
			state.textColor = this.textColor.GetSpecifiedValueOrDefault(state.textColor);
			if (this.backgroundImage.value != null)
			{
				state.background = this.backgroundImage.value;
			}
		}

		private void AssignRect(RectOffset rect, ref StyleValue<int> left, ref StyleValue<int> top, ref StyleValue<int> right, ref StyleValue<int> bottom)
		{
			rect.left = left.GetSpecifiedValueOrDefault(rect.left);
			rect.top = top.GetSpecifiedValueOrDefault(rect.top);
			rect.right = right.GetSpecifiedValueOrDefault(rect.right);
			rect.bottom = bottom.GetSpecifiedValueOrDefault(rect.bottom);
		}

		private void AssignRect(RectOffset rect, ref StyleValue<float> left, ref StyleValue<float> top, ref StyleValue<float> right, ref StyleValue<float> bottom)
		{
			rect.left = (int)left.GetSpecifiedValueOrDefault((float)rect.left);
			rect.top = (int)top.GetSpecifiedValueOrDefault((float)rect.top);
			rect.right = (int)right.GetSpecifiedValueOrDefault((float)rect.right);
			rect.bottom = (int)bottom.GetSpecifiedValueOrDefault((float)rect.bottom);
		}

		internal void ApplyRule(StyleSheet registry, int specificity, StyleRule rule, StylePropertyID[] propertyIDs)
		{
			int i = 0;
			while (i < rule.properties.Length)
			{
				StyleProperty styleProperty = rule.properties[i];
				StylePropertyID stylePropertyID = propertyIDs[i];
				StyleValueHandle[] values = styleProperty.values;
				switch (stylePropertyID)
				{
				case StylePropertyID.MarginLeft:
				{
					StyleValueHandle[] handles = values;
					if (VisualElementStylesData.<>f__mg$cache10 == null)
					{
						VisualElementStylesData.<>f__mg$cache10 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles, specificity, ref this.marginLeft, VisualElementStylesData.<>f__mg$cache10);
					break;
				}
				case StylePropertyID.MarginTop:
				{
					StyleValueHandle[] handles2 = values;
					if (VisualElementStylesData.<>f__mg$cache11 == null)
					{
						VisualElementStylesData.<>f__mg$cache11 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles2, specificity, ref this.marginTop, VisualElementStylesData.<>f__mg$cache11);
					break;
				}
				case StylePropertyID.MarginRight:
				{
					StyleValueHandle[] handles3 = values;
					if (VisualElementStylesData.<>f__mg$cache12 == null)
					{
						VisualElementStylesData.<>f__mg$cache12 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles3, specificity, ref this.marginRight, VisualElementStylesData.<>f__mg$cache12);
					break;
				}
				case StylePropertyID.MarginBottom:
				{
					StyleValueHandle[] handles4 = values;
					if (VisualElementStylesData.<>f__mg$cache13 == null)
					{
						VisualElementStylesData.<>f__mg$cache13 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles4, specificity, ref this.marginBottom, VisualElementStylesData.<>f__mg$cache13);
					break;
				}
				case StylePropertyID.PaddingLeft:
				{
					StyleValueHandle[] handles5 = values;
					if (VisualElementStylesData.<>f__mg$cache19 == null)
					{
						VisualElementStylesData.<>f__mg$cache19 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles5, specificity, ref this.paddingLeft, VisualElementStylesData.<>f__mg$cache19);
					break;
				}
				case StylePropertyID.PaddingTop:
				{
					StyleValueHandle[] handles6 = values;
					if (VisualElementStylesData.<>f__mg$cache1A == null)
					{
						VisualElementStylesData.<>f__mg$cache1A = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles6, specificity, ref this.paddingTop, VisualElementStylesData.<>f__mg$cache1A);
					break;
				}
				case StylePropertyID.PaddingRight:
				{
					StyleValueHandle[] handles7 = values;
					if (VisualElementStylesData.<>f__mg$cache1B == null)
					{
						VisualElementStylesData.<>f__mg$cache1B = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles7, specificity, ref this.paddingRight, VisualElementStylesData.<>f__mg$cache1B);
					break;
				}
				case StylePropertyID.PaddingBottom:
				{
					StyleValueHandle[] handles8 = values;
					if (VisualElementStylesData.<>f__mg$cache1C == null)
					{
						VisualElementStylesData.<>f__mg$cache1C = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles8, specificity, ref this.paddingBottom, VisualElementStylesData.<>f__mg$cache1C);
					break;
				}
				case StylePropertyID.BorderLeft:
				{
					StyleValueHandle[] handles9 = values;
					if (VisualElementStylesData.<>f__mg$cache4 == null)
					{
						VisualElementStylesData.<>f__mg$cache4 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles9, specificity, ref this.borderLeft, VisualElementStylesData.<>f__mg$cache4);
					break;
				}
				case StylePropertyID.BorderTop:
				{
					StyleValueHandle[] handles10 = values;
					if (VisualElementStylesData.<>f__mg$cache5 == null)
					{
						VisualElementStylesData.<>f__mg$cache5 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles10, specificity, ref this.borderTop, VisualElementStylesData.<>f__mg$cache5);
					break;
				}
				case StylePropertyID.BorderRight:
				{
					StyleValueHandle[] handles11 = values;
					if (VisualElementStylesData.<>f__mg$cache6 == null)
					{
						VisualElementStylesData.<>f__mg$cache6 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles11, specificity, ref this.borderRight, VisualElementStylesData.<>f__mg$cache6);
					break;
				}
				case StylePropertyID.BorderBottom:
				{
					StyleValueHandle[] handles12 = values;
					if (VisualElementStylesData.<>f__mg$cache7 == null)
					{
						VisualElementStylesData.<>f__mg$cache7 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles12, specificity, ref this.borderBottom, VisualElementStylesData.<>f__mg$cache7);
					break;
				}
				case StylePropertyID.PositionType:
				{
					StyleValueHandle[] handles13 = values;
					if (VisualElementStylesData.<>f__mg$cache1D == null)
					{
						VisualElementStylesData.<>f__mg$cache1D = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<PositionType>);
					}
					registry.Apply(handles13, specificity, ref this.positionType, VisualElementStylesData.<>f__mg$cache1D);
					break;
				}
				case StylePropertyID.PositionLeft:
				{
					StyleValueHandle[] handles14 = values;
					if (VisualElementStylesData.<>f__mg$cache20 == null)
					{
						VisualElementStylesData.<>f__mg$cache20 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles14, specificity, ref this.positionLeft, VisualElementStylesData.<>f__mg$cache20);
					break;
				}
				case StylePropertyID.PositionTop:
				{
					StyleValueHandle[] handles15 = values;
					if (VisualElementStylesData.<>f__mg$cache1E == null)
					{
						VisualElementStylesData.<>f__mg$cache1E = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles15, specificity, ref this.positionTop, VisualElementStylesData.<>f__mg$cache1E);
					break;
				}
				case StylePropertyID.PositionRight:
				{
					StyleValueHandle[] handles16 = values;
					if (VisualElementStylesData.<>f__mg$cache21 == null)
					{
						VisualElementStylesData.<>f__mg$cache21 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles16, specificity, ref this.positionRight, VisualElementStylesData.<>f__mg$cache21);
					break;
				}
				case StylePropertyID.PositionBottom:
				{
					StyleValueHandle[] handles17 = values;
					if (VisualElementStylesData.<>f__mg$cache1F == null)
					{
						VisualElementStylesData.<>f__mg$cache1F = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles17, specificity, ref this.positionBottom, VisualElementStylesData.<>f__mg$cache1F);
					break;
				}
				case StylePropertyID.Width:
				{
					StyleValueHandle[] handles18 = values;
					if (VisualElementStylesData.<>f__mg$cache25 == null)
					{
						VisualElementStylesData.<>f__mg$cache25 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles18, specificity, ref this.width, VisualElementStylesData.<>f__mg$cache25);
					break;
				}
				case StylePropertyID.Height:
				{
					StyleValueHandle[] handles19 = values;
					if (VisualElementStylesData.<>f__mg$cacheE == null)
					{
						VisualElementStylesData.<>f__mg$cacheE = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles19, specificity, ref this.height, VisualElementStylesData.<>f__mg$cacheE);
					break;
				}
				case StylePropertyID.MinWidth:
				{
					StyleValueHandle[] handles20 = values;
					if (VisualElementStylesData.<>f__mg$cache17 == null)
					{
						VisualElementStylesData.<>f__mg$cache17 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles20, specificity, ref this.minWidth, VisualElementStylesData.<>f__mg$cache17);
					break;
				}
				case StylePropertyID.MinHeight:
				{
					StyleValueHandle[] handles21 = values;
					if (VisualElementStylesData.<>f__mg$cache16 == null)
					{
						VisualElementStylesData.<>f__mg$cache16 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles21, specificity, ref this.minHeight, VisualElementStylesData.<>f__mg$cache16);
					break;
				}
				case StylePropertyID.MaxWidth:
				{
					StyleValueHandle[] handles22 = values;
					if (VisualElementStylesData.<>f__mg$cache15 == null)
					{
						VisualElementStylesData.<>f__mg$cache15 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles22, specificity, ref this.maxWidth, VisualElementStylesData.<>f__mg$cache15);
					break;
				}
				case StylePropertyID.MaxHeight:
				{
					StyleValueHandle[] handles23 = values;
					if (VisualElementStylesData.<>f__mg$cache14 == null)
					{
						VisualElementStylesData.<>f__mg$cache14 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles23, specificity, ref this.maxHeight, VisualElementStylesData.<>f__mg$cache14);
					break;
				}
				case StylePropertyID.Flex:
				{
					StyleValueHandle[] handles24 = values;
					if (VisualElementStylesData.<>f__mg$cache8 == null)
					{
						VisualElementStylesData.<>f__mg$cache8 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles24, specificity, ref this.flex, VisualElementStylesData.<>f__mg$cache8);
					break;
				}
				case StylePropertyID.BorderLeftWidth:
				{
					StyleValueHandle[] handles25 = values;
					if (VisualElementStylesData.<>f__mg$cache2A == null)
					{
						VisualElementStylesData.<>f__mg$cache2A = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles25, specificity, ref this.borderLeftWidth, VisualElementStylesData.<>f__mg$cache2A);
					break;
				}
				case StylePropertyID.BorderTopWidth:
				{
					StyleValueHandle[] handles26 = values;
					if (VisualElementStylesData.<>f__mg$cache2B == null)
					{
						VisualElementStylesData.<>f__mg$cache2B = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles26, specificity, ref this.borderTopWidth, VisualElementStylesData.<>f__mg$cache2B);
					break;
				}
				case StylePropertyID.BorderRightWidth:
				{
					StyleValueHandle[] handles27 = values;
					if (VisualElementStylesData.<>f__mg$cache2C == null)
					{
						VisualElementStylesData.<>f__mg$cache2C = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles27, specificity, ref this.borderRightWidth, VisualElementStylesData.<>f__mg$cache2C);
					break;
				}
				case StylePropertyID.BorderBottomWidth:
				{
					StyleValueHandle[] handles28 = values;
					if (VisualElementStylesData.<>f__mg$cache2D == null)
					{
						VisualElementStylesData.<>f__mg$cache2D = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles28, specificity, ref this.borderBottomWidth, VisualElementStylesData.<>f__mg$cache2D);
					break;
				}
				case StylePropertyID.BorderTopLeftRadius:
				{
					StyleValueHandle[] handles29 = values;
					if (VisualElementStylesData.<>f__mg$cache2E == null)
					{
						VisualElementStylesData.<>f__mg$cache2E = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles29, specificity, ref this.borderTopLeftRadius, VisualElementStylesData.<>f__mg$cache2E);
					break;
				}
				case StylePropertyID.BorderTopRightRadius:
				{
					StyleValueHandle[] handles30 = values;
					if (VisualElementStylesData.<>f__mg$cache2F == null)
					{
						VisualElementStylesData.<>f__mg$cache2F = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles30, specificity, ref this.borderTopRightRadius, VisualElementStylesData.<>f__mg$cache2F);
					break;
				}
				case StylePropertyID.BorderBottomRightRadius:
				{
					StyleValueHandle[] handles31 = values;
					if (VisualElementStylesData.<>f__mg$cache30 == null)
					{
						VisualElementStylesData.<>f__mg$cache30 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles31, specificity, ref this.borderBottomRightRadius, VisualElementStylesData.<>f__mg$cache30);
					break;
				}
				case StylePropertyID.BorderBottomLeftRadius:
				{
					StyleValueHandle[] handles32 = values;
					if (VisualElementStylesData.<>f__mg$cache31 == null)
					{
						VisualElementStylesData.<>f__mg$cache31 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles32, specificity, ref this.borderBottomLeftRadius, VisualElementStylesData.<>f__mg$cache31);
					break;
				}
				case StylePropertyID.FlexDirection:
				{
					StyleValueHandle[] handles33 = values;
					if (VisualElementStylesData.<>f__mg$cacheC == null)
					{
						VisualElementStylesData.<>f__mg$cacheC = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<FlexDirection>);
					}
					registry.Apply(handles33, specificity, ref this.flexDirection, VisualElementStylesData.<>f__mg$cacheC);
					break;
				}
				case StylePropertyID.FlexWrap:
				{
					StyleValueHandle[] handles34 = values;
					if (VisualElementStylesData.<>f__mg$cacheD == null)
					{
						VisualElementStylesData.<>f__mg$cacheD = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Wrap>);
					}
					registry.Apply(handles34, specificity, ref this.flexWrap, VisualElementStylesData.<>f__mg$cacheD);
					break;
				}
				case StylePropertyID.JustifyContent:
				{
					StyleValueHandle[] handles35 = values;
					if (VisualElementStylesData.<>f__mg$cacheF == null)
					{
						VisualElementStylesData.<>f__mg$cacheF = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Justify>);
					}
					registry.Apply(handles35, specificity, ref this.justifyContent, VisualElementStylesData.<>f__mg$cacheF);
					break;
				}
				case StylePropertyID.AlignContent:
				{
					StyleValueHandle[] handles36 = values;
					if (VisualElementStylesData.<>f__mg$cache0 == null)
					{
						VisualElementStylesData.<>f__mg$cache0 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Align>);
					}
					registry.Apply(handles36, specificity, ref this.alignContent, VisualElementStylesData.<>f__mg$cache0);
					break;
				}
				case StylePropertyID.AlignSelf:
				{
					StyleValueHandle[] handles37 = values;
					if (VisualElementStylesData.<>f__mg$cache2 == null)
					{
						VisualElementStylesData.<>f__mg$cache2 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Align>);
					}
					registry.Apply(handles37, specificity, ref this.alignSelf, VisualElementStylesData.<>f__mg$cache2);
					break;
				}
				case StylePropertyID.AlignItems:
				{
					StyleValueHandle[] handles38 = values;
					if (VisualElementStylesData.<>f__mg$cache1 == null)
					{
						VisualElementStylesData.<>f__mg$cache1 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Align>);
					}
					registry.Apply(handles38, specificity, ref this.alignItems, VisualElementStylesData.<>f__mg$cache1);
					break;
				}
				case StylePropertyID.TextAlignment:
				{
					StyleValueHandle[] handles39 = values;
					if (VisualElementStylesData.<>f__mg$cache22 == null)
					{
						VisualElementStylesData.<>f__mg$cache22 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<TextAnchor>);
					}
					registry.Apply(handles39, specificity, ref this.textAlignment, VisualElementStylesData.<>f__mg$cache22);
					break;
				}
				case StylePropertyID.TextClipping:
				{
					StyleValueHandle[] handles40 = values;
					if (VisualElementStylesData.<>f__mg$cache23 == null)
					{
						VisualElementStylesData.<>f__mg$cache23 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<TextClipping>);
					}
					registry.Apply(handles40, specificity, ref this.textClipping, VisualElementStylesData.<>f__mg$cache23);
					break;
				}
				case StylePropertyID.Font:
				{
					StyleValueHandle[] handles41 = values;
					if (VisualElementStylesData.<>f__mg$cache9 == null)
					{
						VisualElementStylesData.<>f__mg$cache9 = new HandlesApplicatorFunction<Font>(StyleSheetApplicator.ApplyResource<Font>);
					}
					registry.Apply(handles41, specificity, ref this.font, VisualElementStylesData.<>f__mg$cache9);
					break;
				}
				case StylePropertyID.FontSize:
				{
					StyleValueHandle[] handles42 = values;
					if (VisualElementStylesData.<>f__mg$cacheA == null)
					{
						VisualElementStylesData.<>f__mg$cacheA = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
					}
					registry.Apply(handles42, specificity, ref this.fontSize, VisualElementStylesData.<>f__mg$cacheA);
					break;
				}
				case StylePropertyID.FontStyle:
				{
					StyleValueHandle[] handles43 = values;
					if (VisualElementStylesData.<>f__mg$cacheB == null)
					{
						VisualElementStylesData.<>f__mg$cacheB = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<FontStyle>);
					}
					registry.Apply(handles43, specificity, ref this.fontStyle, VisualElementStylesData.<>f__mg$cacheB);
					break;
				}
				case StylePropertyID.BackgroundSize:
				{
					StyleValueHandle[] handles44 = values;
					if (VisualElementStylesData.<>f__mg$cache28 == null)
					{
						VisualElementStylesData.<>f__mg$cache28 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
					}
					registry.Apply(handles44, specificity, ref this.backgroundSize, VisualElementStylesData.<>f__mg$cache28);
					break;
				}
				case StylePropertyID.WordWrap:
				{
					StyleValueHandle[] handles45 = values;
					if (VisualElementStylesData.<>f__mg$cache26 == null)
					{
						VisualElementStylesData.<>f__mg$cache26 = new HandlesApplicatorFunction<bool>(StyleSheetApplicator.ApplyBool);
					}
					registry.Apply(handles45, specificity, ref this.wordWrap, VisualElementStylesData.<>f__mg$cache26);
					break;
				}
				case StylePropertyID.BackgroundImage:
				{
					StyleValueHandle[] handles46 = values;
					if (VisualElementStylesData.<>f__mg$cache3 == null)
					{
						VisualElementStylesData.<>f__mg$cache3 = new HandlesApplicatorFunction<Texture2D>(StyleSheetApplicator.ApplyResource<Texture2D>);
					}
					registry.Apply(handles46, specificity, ref this.backgroundImage, VisualElementStylesData.<>f__mg$cache3);
					break;
				}
				case StylePropertyID.TextColor:
				{
					StyleValueHandle[] handles47 = values;
					if (VisualElementStylesData.<>f__mg$cache24 == null)
					{
						VisualElementStylesData.<>f__mg$cache24 = new HandlesApplicatorFunction<Color>(StyleSheetApplicator.ApplyColor);
					}
					registry.Apply(handles47, specificity, ref this.textColor, VisualElementStylesData.<>f__mg$cache24);
					break;
				}
				case StylePropertyID.BackgroundColor:
				{
					StyleValueHandle[] handles48 = values;
					if (VisualElementStylesData.<>f__mg$cache27 == null)
					{
						VisualElementStylesData.<>f__mg$cache27 = new HandlesApplicatorFunction<Color>(StyleSheetApplicator.ApplyColor);
					}
					registry.Apply(handles48, specificity, ref this.backgroundColor, VisualElementStylesData.<>f__mg$cache27);
					break;
				}
				case StylePropertyID.BorderColor:
				{
					StyleValueHandle[] handles49 = values;
					if (VisualElementStylesData.<>f__mg$cache29 == null)
					{
						VisualElementStylesData.<>f__mg$cache29 = new HandlesApplicatorFunction<Color>(StyleSheetApplicator.ApplyColor);
					}
					registry.Apply(handles49, specificity, ref this.borderColor, VisualElementStylesData.<>f__mg$cache29);
					break;
				}
				case StylePropertyID.Overflow:
				{
					StyleValueHandle[] handles50 = values;
					if (VisualElementStylesData.<>f__mg$cache18 == null)
					{
						VisualElementStylesData.<>f__mg$cache18 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Overflow>);
					}
					registry.Apply(handles50, specificity, ref this.overflow, VisualElementStylesData.<>f__mg$cache18);
					break;
				}
				case StylePropertyID.SliceLeft:
				{
					StyleValueHandle[] handles51 = values;
					if (VisualElementStylesData.<>f__mg$cache32 == null)
					{
						VisualElementStylesData.<>f__mg$cache32 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
					}
					registry.Apply(handles51, specificity, ref this.sliceLeft, VisualElementStylesData.<>f__mg$cache32);
					break;
				}
				case StylePropertyID.SliceTop:
				{
					StyleValueHandle[] handles52 = values;
					if (VisualElementStylesData.<>f__mg$cache33 == null)
					{
						VisualElementStylesData.<>f__mg$cache33 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
					}
					registry.Apply(handles52, specificity, ref this.sliceTop, VisualElementStylesData.<>f__mg$cache33);
					break;
				}
				case StylePropertyID.SliceRight:
				{
					StyleValueHandle[] handles53 = values;
					if (VisualElementStylesData.<>f__mg$cache34 == null)
					{
						VisualElementStylesData.<>f__mg$cache34 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
					}
					registry.Apply(handles53, specificity, ref this.sliceRight, VisualElementStylesData.<>f__mg$cache34);
					break;
				}
				case StylePropertyID.SliceBottom:
				{
					StyleValueHandle[] handles54 = values;
					if (VisualElementStylesData.<>f__mg$cache35 == null)
					{
						VisualElementStylesData.<>f__mg$cache35 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
					}
					registry.Apply(handles54, specificity, ref this.sliceBottom, VisualElementStylesData.<>f__mg$cache35);
					break;
				}
				case StylePropertyID.Opacity:
				{
					StyleValueHandle[] handles55 = values;
					if (VisualElementStylesData.<>f__mg$cache36 == null)
					{
						VisualElementStylesData.<>f__mg$cache36 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles55, specificity, ref this.opacity, VisualElementStylesData.<>f__mg$cache36);
					break;
				}
				case StylePropertyID.BorderRadius:
				{
					StyleValueHandle[] handles56 = values;
					if (VisualElementStylesData.<>f__mg$cache37 == null)
					{
						VisualElementStylesData.<>f__mg$cache37 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles56, specificity, ref this.borderTopLeftRadius, VisualElementStylesData.<>f__mg$cache37);
					StyleValueHandle[] handles57 = values;
					if (VisualElementStylesData.<>f__mg$cache38 == null)
					{
						VisualElementStylesData.<>f__mg$cache38 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles57, specificity, ref this.borderTopRightRadius, VisualElementStylesData.<>f__mg$cache38);
					StyleValueHandle[] handles58 = values;
					if (VisualElementStylesData.<>f__mg$cache39 == null)
					{
						VisualElementStylesData.<>f__mg$cache39 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles58, specificity, ref this.borderBottomLeftRadius, VisualElementStylesData.<>f__mg$cache39);
					StyleValueHandle[] handles59 = values;
					if (VisualElementStylesData.<>f__mg$cache3A == null)
					{
						VisualElementStylesData.<>f__mg$cache3A = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(handles59, specificity, ref this.borderBottomRightRadius, VisualElementStylesData.<>f__mg$cache3A);
					break;
				}
				case StylePropertyID.Margin:
				case StylePropertyID.Padding:
					goto IL_C91;
				case StylePropertyID.Custom:
				{
					if (this.m_CustomProperties == null)
					{
						this.m_CustomProperties = new Dictionary<string, CustomProperty>();
					}
					CustomProperty value = default(CustomProperty);
					if (!this.m_CustomProperties.TryGetValue(styleProperty.name, out value) || specificity >= value.specificity)
					{
						value.handles = values;
						value.data = registry;
						value.specificity = specificity;
						this.m_CustomProperties[styleProperty.name] = value;
					}
					break;
				}
				default:
					goto IL_C91;
				}
				i++;
				continue;
				IL_C91:
				throw new ArgumentException(string.Format("Non exhaustive switch statement (value={0})", stylePropertyID));
			}
		}

		public void ApplyCustomProperty(string propertyName, ref StyleValue<float> target)
		{
			StyleValueType valueType = StyleValueType.Float;
			if (VisualElementStylesData.<>f__mg$cache3B == null)
			{
				VisualElementStylesData.<>f__mg$cache3B = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
			}
			this.ApplyCustomProperty<float>(propertyName, ref target, valueType, VisualElementStylesData.<>f__mg$cache3B);
		}

		public void ApplyCustomProperty(string propertyName, ref StyleValue<int> target)
		{
			StyleValueType valueType = StyleValueType.Float;
			if (VisualElementStylesData.<>f__mg$cache3C == null)
			{
				VisualElementStylesData.<>f__mg$cache3C = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
			}
			this.ApplyCustomProperty<int>(propertyName, ref target, valueType, VisualElementStylesData.<>f__mg$cache3C);
		}

		public void ApplyCustomProperty(string propertyName, ref StyleValue<bool> target)
		{
			StyleValueType valueType = StyleValueType.Keyword;
			if (VisualElementStylesData.<>f__mg$cache3D == null)
			{
				VisualElementStylesData.<>f__mg$cache3D = new HandlesApplicatorFunction<bool>(StyleSheetApplicator.ApplyBool);
			}
			this.ApplyCustomProperty<bool>(propertyName, ref target, valueType, VisualElementStylesData.<>f__mg$cache3D);
		}

		public void ApplyCustomProperty(string propertyName, ref StyleValue<Color> target)
		{
			StyleValueType valueType = StyleValueType.Color;
			if (VisualElementStylesData.<>f__mg$cache3E == null)
			{
				VisualElementStylesData.<>f__mg$cache3E = new HandlesApplicatorFunction<Color>(StyleSheetApplicator.ApplyColor);
			}
			this.ApplyCustomProperty<Color>(propertyName, ref target, valueType, VisualElementStylesData.<>f__mg$cache3E);
		}

		public void ApplyCustomProperty<T>(string propertyName, ref StyleValue<T> target) where T : Object
		{
			this.ApplyCustomProperty<T>(propertyName, ref target, StyleValueType.ResourcePath, new HandlesApplicatorFunction<T>(StyleSheetApplicator.ApplyResource<T>));
		}

		public void ApplyCustomProperty(string propertyName, ref StyleValue<string> target)
		{
			StyleValue<string> other = new StyleValue<string>(string.Empty);
			CustomProperty customProperty;
			if (this.m_CustomProperties != null && this.m_CustomProperties.TryGetValue(propertyName, out customProperty))
			{
				other.value = customProperty.data.ReadAsString(customProperty.handles[0]);
				other.specificity = customProperty.specificity;
			}
			target.Apply(other, StylePropertyApplyMode.CopyIfNotInline);
		}

		internal void ApplyCustomProperty<T>(string propertyName, ref StyleValue<T> target, StyleValueType valueType, HandlesApplicatorFunction<T> applicatorFunc)
		{
			StyleValue<T> other = default(StyleValue<T>);
			CustomProperty customProperty;
			if (this.m_CustomProperties != null && this.m_CustomProperties.TryGetValue(propertyName, out customProperty))
			{
				StyleValueHandle styleValueHandle = customProperty.handles[0];
				if (styleValueHandle.valueType == valueType)
				{
					customProperty.data.Apply(customProperty.handles, customProperty.specificity, ref other, applicatorFunc);
				}
				else
				{
					Debug.LogWarning(string.Format("Trying to read value as {0} while parsed type is {1}", valueType, styleValueHandle.valueType));
				}
			}
			target.Apply(other, StylePropertyApplyMode.CopyIfNotInline);
		}
	}
}
