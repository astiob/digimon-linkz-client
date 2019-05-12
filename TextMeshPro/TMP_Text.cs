using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TMPro
{
	public class TMP_Text : MaskableGraphic
	{
		[SerializeField]
		protected string m_text;

		[SerializeField]
		protected bool m_isRightToLeft;

		[SerializeField]
		protected TMP_FontAsset m_fontAsset;

		protected TMP_FontAsset m_currentFontAsset;

		protected bool m_isSDFShader;

		[SerializeField]
		protected Material m_sharedMaterial;

		protected Material m_currentMaterial;

		protected MaterialReference[] m_materialReferences = new MaterialReference[32];

		protected Dictionary<int, int> m_materialReferenceIndexLookup = new Dictionary<int, int>();

		protected TMP_XmlTagStack<MaterialReference> m_materialReferenceStack = new TMP_XmlTagStack<MaterialReference>(new MaterialReference[16]);

		protected int m_currentMaterialIndex;

		[SerializeField]
		protected Material[] m_fontSharedMaterials;

		[SerializeField]
		protected Material m_fontMaterial;

		[SerializeField]
		protected Material[] m_fontMaterials;

		protected bool m_isMaterialDirty;

		[SerializeField]
		protected Color32 m_fontColor32 = Color.white;

		[SerializeField]
		protected Color m_fontColor = Color.white;

		protected static Color32 s_colorWhite = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		protected Color32 m_highlightColor = TMP_Text.s_colorWhite;

		protected Color32 m_underlineColor = TMP_Text.s_colorWhite;

		[SerializeField]
		protected bool m_enableVertexGradient;

		[SerializeField]
		protected VertexGradient m_fontColorGradient = new VertexGradient(Color.white);

		[SerializeField]
		protected TMP_ColorGradient m_fontColorGradientPreset;

		[SerializeField]
		protected TMP_SpriteAsset m_spriteAsset;

		[SerializeField]
		protected bool m_tintAllSprites;

		protected bool m_tintSprite;

		protected Color32 m_spriteColor;

		[SerializeField]
		protected bool m_overrideHtmlColors;

		[SerializeField]
		protected Color32 m_faceColor = Color.white;

		[SerializeField]
		protected Color32 m_outlineColor = Color.black;

		protected float m_outlineWidth;

		[SerializeField]
		protected float m_fontSize = 36f;

		protected float m_currentFontSize;

		[SerializeField]
		protected float m_fontSizeBase = 36f;

		protected TMP_XmlTagStack<float> m_sizeStack = new TMP_XmlTagStack<float>(new float[16]);

		[SerializeField]
		protected int m_fontWeight = 400;

		protected int m_fontWeightInternal;

		protected TMP_XmlTagStack<int> m_fontWeightStack = new TMP_XmlTagStack<int>(new int[16]);

		[SerializeField]
		protected bool m_enableAutoSizing;

		protected float m_maxFontSize;

		protected float m_minFontSize;

		[SerializeField]
		protected float m_fontSizeMin;

		[SerializeField]
		protected float m_fontSizeMax;

		[SerializeField]
		protected FontStyles m_fontStyle;

		protected FontStyles m_style;

		protected TMP_BasicXmlTagStack m_fontStyleStack;

		protected bool m_isUsingBold;

		[SerializeField]
		[FormerlySerializedAs("m_lineJustification")]
		protected TextAlignmentOptions m_textAlignment = TextAlignmentOptions.TopLeft;

		protected TextAlignmentOptions m_lineJustification;

		protected TMP_XmlTagStack<TextAlignmentOptions> m_lineJustificationStack = new TMP_XmlTagStack<TextAlignmentOptions>(new TextAlignmentOptions[16]);

		protected Vector3[] m_textContainerLocalCorners = new Vector3[4];

		[SerializeField]
		protected bool m_isAlignmentEnumConverted;

		[SerializeField]
		protected float m_characterSpacing;

		protected float m_cSpacing;

		protected float m_monoSpacing;

		[SerializeField]
		protected float m_wordSpacing;

		[SerializeField]
		protected float m_lineSpacing;

		protected float m_lineSpacingDelta;

		protected float m_lineHeight = -32767f;

		[SerializeField]
		protected float m_lineSpacingMax;

		[SerializeField]
		protected float m_paragraphSpacing;

		[SerializeField]
		protected float m_charWidthMaxAdj;

		protected float m_charWidthAdjDelta;

		[SerializeField]
		protected bool m_enableWordWrapping;

		protected bool m_isCharacterWrappingEnabled;

		protected bool m_isNonBreakingSpace;

		protected bool m_isIgnoringAlignment;

		[SerializeField]
		protected float m_wordWrappingRatios = 0.4f;

		[SerializeField]
		protected TextOverflowModes m_overflowMode;

		[SerializeField]
		protected int m_firstOverflowCharacterIndex = -1;

		[SerializeField]
		protected TMP_Text m_linkedTextComponent;

		[SerializeField]
		protected bool m_isLinkedTextComponent;

		protected bool m_isTextTruncated;

		[SerializeField]
		protected bool m_enableKerning;

		[SerializeField]
		protected bool m_enableExtraPadding;

		[SerializeField]
		protected bool checkPaddingRequired;

		[SerializeField]
		protected bool m_isRichText = true;

		[SerializeField]
		protected bool m_parseCtrlCharacters = true;

		protected bool m_isOverlay;

		[SerializeField]
		protected bool m_isOrthographic;

		[SerializeField]
		protected bool m_isCullingEnabled;

		[SerializeField]
		protected bool m_ignoreRectMaskCulling;

		[SerializeField]
		protected bool m_ignoreCulling = true;

		[SerializeField]
		protected TextureMappingOptions m_horizontalMapping;

		[SerializeField]
		protected TextureMappingOptions m_verticalMapping;

		[SerializeField]
		protected float m_uvLineOffset;

		protected TextRenderFlags m_renderMode = TextRenderFlags.Render;

		[SerializeField]
		protected VertexSortingOrder m_geometrySortingOrder;

		[SerializeField]
		protected int m_firstVisibleCharacter;

		protected int m_maxVisibleCharacters = 99999;

		protected int m_maxVisibleWords = 99999;

		protected int m_maxVisibleLines = 99999;

		[SerializeField]
		protected bool m_useMaxVisibleDescender = true;

		[SerializeField]
		protected int m_pageToDisplay = 1;

		protected bool m_isNewPage;

		[SerializeField]
		protected Vector4 m_margin = new Vector4(0f, 0f, 0f, 0f);

		protected float m_marginLeft;

		protected float m_marginRight;

		protected float m_marginWidth;

		protected float m_marginHeight;

		protected float m_width = -1f;

		[SerializeField]
		protected TMP_TextInfo m_textInfo;

		[SerializeField]
		protected bool m_havePropertiesChanged;

		[SerializeField]
		protected bool m_isUsingLegacyAnimationComponent;

		protected Transform m_transform;

		protected RectTransform m_rectTransform;

		protected bool m_autoSizeTextContainer;

		protected Mesh m_mesh;

		[SerializeField]
		protected bool m_isVolumetricText;

		[SerializeField]
		protected TMP_SpriteAnimator m_spriteAnimator;

		protected float m_flexibleHeight = -1f;

		protected float m_flexibleWidth = -1f;

		protected float m_minWidth;

		protected float m_minHeight;

		protected float m_maxWidth;

		protected float m_maxHeight;

		protected LayoutElement m_LayoutElement;

		protected float m_preferredWidth;

		protected float m_renderedWidth;

		protected bool m_isPreferredWidthDirty;

		protected float m_preferredHeight;

		protected float m_renderedHeight;

		protected bool m_isPreferredHeightDirty;

		protected bool m_isCalculatingPreferredValues;

		private int m_recursiveCount;

		protected int m_layoutPriority;

		protected bool m_isCalculateSizeRequired;

		protected bool m_isLayoutDirty;

		protected bool m_verticesAlreadyDirty;

		protected bool m_layoutAlreadyDirty;

		protected bool m_isAwake;

		[SerializeField]
		protected bool m_isInputParsingRequired;

		[SerializeField]
		protected TMP_Text.TextInputSources m_inputSource;

		protected string old_text;

		protected float old_arg0;

		protected float old_arg1;

		protected float old_arg2;

		protected float m_fontScale;

		protected float m_fontScaleMultiplier;

		protected char[] m_htmlTag = new char[128];

		protected XML_TagAttribute[] m_xmlAttribute = new XML_TagAttribute[8];

		protected float[] m_attributeParameterValues = new float[16];

		protected float tag_LineIndent;

		protected float tag_Indent;

		protected TMP_XmlTagStack<float> m_indentStack = new TMP_XmlTagStack<float>(new float[16]);

		protected bool tag_NoParsing;

		protected bool m_isParsingText;

		protected Matrix4x4 m_FXMatrix;

		protected bool m_isFXMatrixSet;

		protected int[] m_char_buffer;

		private TMP_CharacterInfo[] m_internalCharacterInfo;

		protected char[] m_input_CharArray = new char[256];

		private int m_charArray_Length;

		protected int m_totalCharacterCount;

		protected WordWrapState m_SavedWordWrapState;

		protected WordWrapState m_SavedLineState;

		protected int m_characterCount;

		protected int m_firstCharacterOfLine;

		protected int m_firstVisibleCharacterOfLine;

		protected int m_lastCharacterOfLine;

		protected int m_lastVisibleCharacterOfLine;

		protected int m_lineNumber;

		protected int m_lineVisibleCharacterCount;

		protected int m_pageNumber;

		protected float m_maxAscender;

		protected float m_maxCapHeight;

		protected float m_maxDescender;

		protected float m_maxLineAscender;

		protected float m_maxLineDescender;

		protected float m_startOfLineAscender;

		protected float m_lineOffset;

		protected Extents m_meshExtents;

		protected Color32 m_htmlColor = new Color(255f, 255f, 255f, 128f);

		protected TMP_XmlTagStack<Color32> m_colorStack = new TMP_XmlTagStack<Color32>(new Color32[16]);

		protected TMP_XmlTagStack<Color32> m_underlineColorStack = new TMP_XmlTagStack<Color32>(new Color32[16]);

		protected TMP_XmlTagStack<Color32> m_highlightColorStack = new TMP_XmlTagStack<Color32>(new Color32[16]);

		protected float m_tabSpacing;

		protected float m_spacing;

		protected TMP_XmlTagStack<int> m_styleStack = new TMP_XmlTagStack<int>(new int[16]);

		protected TMP_XmlTagStack<int> m_actionStack = new TMP_XmlTagStack<int>(new int[16]);

		protected float m_padding;

		protected float m_baselineOffset;

		protected float m_xAdvance;

		protected TMP_TextElementType m_textElementType;

		protected TMP_TextElement m_cached_TextElement;

		protected TMP_Glyph m_cached_Underline_GlyphInfo;

		protected TMP_Glyph m_cached_Ellipsis_GlyphInfo;

		protected TMP_SpriteAsset m_defaultSpriteAsset;

		protected TMP_SpriteAsset m_currentSpriteAsset;

		protected int m_spriteCount;

		protected int m_spriteIndex;

		protected InlineGraphicManager m_inlineGraphics;

		protected int m_spriteAnimationID;

		protected bool m_ignoreActiveState;

		private readonly float[] k_Power = new float[]
		{
			0.5f,
			0.05f,
			0.005f,
			0.0005f,
			5E-05f,
			5E-06f,
			5E-07f,
			5E-08f,
			5E-09f,
			5E-10f
		};

		protected static Vector2 k_LargePositiveVector2 = new Vector2(2.14748365E+09f, 2.14748365E+09f);

		protected static Vector2 k_LargeNegativeVector2 = new Vector2(-2.14748365E+09f, -2.14748365E+09f);

		protected static float k_LargePositiveFloat = 32767f;

		protected static float k_LargeNegativeFloat = -32767f;

		protected static int k_LargePositiveInt = int.MaxValue;

		protected static int k_LargeNegativeInt = -2147483647;

		public string text
		{
			get
			{
				return this.m_text;
			}
			set
			{
				if (this.m_text == value)
				{
					return;
				}
				this.old_text = value;
				this.m_text = value;
				this.m_inputSource = TMP_Text.TextInputSources.String;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public bool isRightToLeftText
		{
			get
			{
				return this.m_isRightToLeft;
			}
			set
			{
				if (this.m_isRightToLeft == value)
				{
					return;
				}
				this.m_isRightToLeft = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public TMP_FontAsset font
		{
			get
			{
				return this.m_fontAsset;
			}
			set
			{
				if (this.m_fontAsset == value)
				{
					return;
				}
				this.m_fontAsset = value;
				this.LoadFontAsset();
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public virtual Material fontSharedMaterial
		{
			get
			{
				return this.m_sharedMaterial;
			}
			set
			{
				if (this.m_sharedMaterial == value)
				{
					return;
				}
				this.SetSharedMaterial(value);
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		public virtual Material[] fontSharedMaterials
		{
			get
			{
				return this.GetSharedMaterials();
			}
			set
			{
				this.SetSharedMaterials(value);
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		public Material fontMaterial
		{
			get
			{
				return this.GetMaterial(this.m_sharedMaterial);
			}
			set
			{
				if (this.m_sharedMaterial != null && this.m_sharedMaterial.GetInstanceID() == value.GetInstanceID())
				{
					return;
				}
				this.m_sharedMaterial = value;
				this.m_padding = this.GetPaddingForMaterial();
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		public virtual Material[] fontMaterials
		{
			get
			{
				return this.GetMaterials(this.m_fontSharedMaterials);
			}
			set
			{
				this.SetSharedMaterials(value);
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		public new Color color
		{
			get
			{
				return this.m_fontColor;
			}
			set
			{
				if (this.m_fontColor == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_fontColor = value;
				this.SetVerticesDirty();
			}
		}

		public float alpha
		{
			get
			{
				return this.m_fontColor.a;
			}
			set
			{
				if (this.m_fontColor.a == value)
				{
					return;
				}
				this.m_fontColor.a = value;
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		public bool enableVertexGradient
		{
			get
			{
				return this.m_enableVertexGradient;
			}
			set
			{
				if (this.m_enableVertexGradient == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_enableVertexGradient = value;
				this.SetVerticesDirty();
			}
		}

		public VertexGradient colorGradient
		{
			get
			{
				return this.m_fontColorGradient;
			}
			set
			{
				this.m_havePropertiesChanged = true;
				this.m_fontColorGradient = value;
				this.SetVerticesDirty();
			}
		}

		public TMP_ColorGradient colorGradientPreset
		{
			get
			{
				return this.m_fontColorGradientPreset;
			}
			set
			{
				this.m_havePropertiesChanged = true;
				this.m_fontColorGradientPreset = value;
				this.SetVerticesDirty();
			}
		}

		public TMP_SpriteAsset spriteAsset
		{
			get
			{
				return this.m_spriteAsset;
			}
			set
			{
				this.m_spriteAsset = value;
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public bool tintAllSprites
		{
			get
			{
				return this.m_tintAllSprites;
			}
			set
			{
				if (this.m_tintAllSprites == value)
				{
					return;
				}
				this.m_tintAllSprites = value;
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		public bool overrideColorTags
		{
			get
			{
				return this.m_overrideHtmlColors;
			}
			set
			{
				if (this.m_overrideHtmlColors == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_overrideHtmlColors = value;
				this.SetVerticesDirty();
			}
		}

		public Color32 faceColor
		{
			get
			{
				if (this.m_sharedMaterial == null)
				{
					return this.m_faceColor;
				}
				this.m_faceColor = this.m_sharedMaterial.GetColor(ShaderUtilities.ID_FaceColor);
				return this.m_faceColor;
			}
			set
			{
				if (this.m_faceColor.Compare(value))
				{
					return;
				}
				this.SetFaceColor(value);
				this.m_havePropertiesChanged = true;
				this.m_faceColor = value;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		public Color32 outlineColor
		{
			get
			{
				if (this.m_sharedMaterial == null)
				{
					return this.m_outlineColor;
				}
				this.m_outlineColor = this.m_sharedMaterial.GetColor(ShaderUtilities.ID_OutlineColor);
				return this.m_outlineColor;
			}
			set
			{
				if (this.m_outlineColor.Compare(value))
				{
					return;
				}
				this.SetOutlineColor(value);
				this.m_havePropertiesChanged = true;
				this.m_outlineColor = value;
				this.SetVerticesDirty();
			}
		}

		public float outlineWidth
		{
			get
			{
				if (this.m_sharedMaterial == null)
				{
					return this.m_outlineWidth;
				}
				this.m_outlineWidth = this.m_sharedMaterial.GetFloat(ShaderUtilities.ID_OutlineWidth);
				return this.m_outlineWidth;
			}
			set
			{
				if (this.m_outlineWidth == value)
				{
					return;
				}
				this.SetOutlineThickness(value);
				this.m_havePropertiesChanged = true;
				this.m_outlineWidth = value;
				this.SetVerticesDirty();
			}
		}

		public float fontSize
		{
			get
			{
				return this.m_fontSize;
			}
			set
			{
				if (this.m_fontSize == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_fontSize = value;
				if (!this.m_enableAutoSizing)
				{
					this.m_fontSizeBase = this.m_fontSize;
				}
			}
		}

		public float fontScale
		{
			get
			{
				return this.m_fontScale;
			}
		}

		public int fontWeight
		{
			get
			{
				return this.m_fontWeight;
			}
			set
			{
				if (this.m_fontWeight == value)
				{
					return;
				}
				this.m_fontWeight = value;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public float pixelsPerUnit
		{
			get
			{
				Canvas canvas = base.canvas;
				if (!canvas)
				{
					return 1f;
				}
				if (!this.font)
				{
					return canvas.scaleFactor;
				}
				if (this.m_currentFontAsset == null || this.m_currentFontAsset.fontInfo.PointSize <= 0f || this.m_fontSize <= 0f)
				{
					return 1f;
				}
				return this.m_fontSize / this.m_currentFontAsset.fontInfo.PointSize;
			}
		}

		public bool enableAutoSizing
		{
			get
			{
				return this.m_enableAutoSizing;
			}
			set
			{
				if (this.m_enableAutoSizing == value)
				{
					return;
				}
				this.m_enableAutoSizing = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public float fontSizeMin
		{
			get
			{
				return this.m_fontSizeMin;
			}
			set
			{
				if (this.m_fontSizeMin == value)
				{
					return;
				}
				this.m_fontSizeMin = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public float fontSizeMax
		{
			get
			{
				return this.m_fontSizeMax;
			}
			set
			{
				if (this.m_fontSizeMax == value)
				{
					return;
				}
				this.m_fontSizeMax = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public FontStyles fontStyle
		{
			get
			{
				return this.m_fontStyle;
			}
			set
			{
				if (this.m_fontStyle == value)
				{
					return;
				}
				this.m_fontStyle = value;
				this.m_havePropertiesChanged = true;
				this.checkPaddingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public bool isUsingBold
		{
			get
			{
				return this.m_isUsingBold;
			}
		}

		public TextAlignmentOptions alignment
		{
			get
			{
				return this.m_textAlignment;
			}
			set
			{
				if (this.m_textAlignment == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_textAlignment = value;
				this.SetVerticesDirty();
			}
		}

		public float characterSpacing
		{
			get
			{
				return this.m_characterSpacing;
			}
			set
			{
				if (this.m_characterSpacing == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_characterSpacing = value;
			}
		}

		public float wordSpacing
		{
			get
			{
				return this.m_wordSpacing;
			}
			set
			{
				if (this.m_wordSpacing == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_wordSpacing = value;
			}
		}

		public float lineSpacing
		{
			get
			{
				return this.m_lineSpacing;
			}
			set
			{
				if (this.m_lineSpacing == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_lineSpacing = value;
			}
		}

		public float lineSpacingAdjustment
		{
			get
			{
				return this.m_lineSpacingMax;
			}
			set
			{
				if (this.m_lineSpacingMax == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_lineSpacingMax = value;
			}
		}

		public float paragraphSpacing
		{
			get
			{
				return this.m_paragraphSpacing;
			}
			set
			{
				if (this.m_paragraphSpacing == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_paragraphSpacing = value;
			}
		}

		public float characterWidthAdjustment
		{
			get
			{
				return this.m_charWidthMaxAdj;
			}
			set
			{
				if (this.m_charWidthMaxAdj == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_charWidthMaxAdj = value;
			}
		}

		public bool enableWordWrapping
		{
			get
			{
				return this.m_enableWordWrapping;
			}
			set
			{
				if (this.m_enableWordWrapping == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.m_isCalculateSizeRequired = true;
				this.m_enableWordWrapping = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public float wordWrappingRatios
		{
			get
			{
				return this.m_wordWrappingRatios;
			}
			set
			{
				if (this.m_wordWrappingRatios == value)
				{
					return;
				}
				this.m_wordWrappingRatios = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public TextOverflowModes overflowMode
		{
			get
			{
				return this.m_overflowMode;
			}
			set
			{
				if (this.m_overflowMode == value)
				{
					return;
				}
				this.m_overflowMode = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public bool isTextOverflowing
		{
			get
			{
				return this.m_firstOverflowCharacterIndex != -1;
			}
		}

		public int firstOverflowCharacterIndex
		{
			get
			{
				return this.m_firstOverflowCharacterIndex;
			}
		}

		public TMP_Text linkedTextComponent
		{
			get
			{
				return this.m_linkedTextComponent;
			}
			set
			{
				if (this.m_linkedTextComponent != value)
				{
					if (this.m_linkedTextComponent != null)
					{
						this.m_linkedTextComponent.overflowMode = TextOverflowModes.Overflow;
						this.m_linkedTextComponent.linkedTextComponent = null;
						this.m_linkedTextComponent.isLinkedTextComponent = false;
					}
					this.m_linkedTextComponent = value;
					if (this.m_linkedTextComponent != null)
					{
						this.m_linkedTextComponent.isLinkedTextComponent = true;
					}
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public bool isLinkedTextComponent
		{
			get
			{
				return this.m_isLinkedTextComponent;
			}
			set
			{
				this.m_isLinkedTextComponent = value;
				if (!this.m_isLinkedTextComponent)
				{
					this.m_firstVisibleCharacter = 0;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public bool isTextTruncated
		{
			get
			{
				return this.m_isTextTruncated;
			}
		}

		public bool enableKerning
		{
			get
			{
				return this.m_enableKerning;
			}
			set
			{
				if (this.m_enableKerning == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_enableKerning = value;
			}
		}

		public bool extraPadding
		{
			get
			{
				return this.m_enableExtraPadding;
			}
			set
			{
				if (this.m_enableExtraPadding == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_enableExtraPadding = value;
				this.UpdateMeshPadding();
				this.SetVerticesDirty();
			}
		}

		public bool richText
		{
			get
			{
				return this.m_isRichText;
			}
			set
			{
				if (this.m_isRichText == value)
				{
					return;
				}
				this.m_isRichText = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_isInputParsingRequired = true;
			}
		}

		public bool parseCtrlCharacters
		{
			get
			{
				return this.m_parseCtrlCharacters;
			}
			set
			{
				if (this.m_parseCtrlCharacters == value)
				{
					return;
				}
				this.m_parseCtrlCharacters = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_isInputParsingRequired = true;
			}
		}

		public bool isOverlay
		{
			get
			{
				return this.m_isOverlay;
			}
			set
			{
				if (this.m_isOverlay == value)
				{
					return;
				}
				this.m_isOverlay = value;
				this.SetShaderDepth();
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		public bool isOrthographic
		{
			get
			{
				return this.m_isOrthographic;
			}
			set
			{
				if (this.m_isOrthographic == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isOrthographic = value;
				this.SetVerticesDirty();
			}
		}

		public bool enableCulling
		{
			get
			{
				return this.m_isCullingEnabled;
			}
			set
			{
				if (this.m_isCullingEnabled == value)
				{
					return;
				}
				this.m_isCullingEnabled = value;
				this.SetCulling();
				this.m_havePropertiesChanged = true;
			}
		}

		public bool ignoreRectMaskCulling
		{
			get
			{
				return this.m_ignoreRectMaskCulling;
			}
			set
			{
				if (this.m_ignoreRectMaskCulling == value)
				{
					return;
				}
				this.m_ignoreRectMaskCulling = value;
				this.m_havePropertiesChanged = true;
			}
		}

		public bool ignoreVisibility
		{
			get
			{
				return this.m_ignoreCulling;
			}
			set
			{
				if (this.m_ignoreCulling == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_ignoreCulling = value;
			}
		}

		public TextureMappingOptions horizontalMapping
		{
			get
			{
				return this.m_horizontalMapping;
			}
			set
			{
				if (this.m_horizontalMapping == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_horizontalMapping = value;
				this.SetVerticesDirty();
			}
		}

		public TextureMappingOptions verticalMapping
		{
			get
			{
				return this.m_verticalMapping;
			}
			set
			{
				if (this.m_verticalMapping == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_verticalMapping = value;
				this.SetVerticesDirty();
			}
		}

		public float mappingUvLineOffset
		{
			get
			{
				return this.m_uvLineOffset;
			}
			set
			{
				if (this.m_uvLineOffset == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_uvLineOffset = value;
				this.SetVerticesDirty();
			}
		}

		public TextRenderFlags renderMode
		{
			get
			{
				return this.m_renderMode;
			}
			set
			{
				if (this.m_renderMode == value)
				{
					return;
				}
				this.m_renderMode = value;
				this.m_havePropertiesChanged = true;
			}
		}

		public VertexSortingOrder geometrySortingOrder
		{
			get
			{
				return this.m_geometrySortingOrder;
			}
			set
			{
				this.m_geometrySortingOrder = value;
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		public int firstVisibleCharacter
		{
			get
			{
				return this.m_firstVisibleCharacter;
			}
			set
			{
				if (this.m_firstVisibleCharacter == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_firstVisibleCharacter = value;
				this.SetVerticesDirty();
			}
		}

		public int maxVisibleCharacters
		{
			get
			{
				return this.m_maxVisibleCharacters;
			}
			set
			{
				if (this.m_maxVisibleCharacters == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_maxVisibleCharacters = value;
				this.SetVerticesDirty();
			}
		}

		public int maxVisibleWords
		{
			get
			{
				return this.m_maxVisibleWords;
			}
			set
			{
				if (this.m_maxVisibleWords == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_maxVisibleWords = value;
				this.SetVerticesDirty();
			}
		}

		public int maxVisibleLines
		{
			get
			{
				return this.m_maxVisibleLines;
			}
			set
			{
				if (this.m_maxVisibleLines == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.m_maxVisibleLines = value;
				this.SetVerticesDirty();
			}
		}

		public bool useMaxVisibleDescender
		{
			get
			{
				return this.m_useMaxVisibleDescender;
			}
			set
			{
				if (this.m_useMaxVisibleDescender == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
			}
		}

		public int pageToDisplay
		{
			get
			{
				return this.m_pageToDisplay;
			}
			set
			{
				if (this.m_pageToDisplay == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_pageToDisplay = value;
				this.SetVerticesDirty();
			}
		}

		public virtual Vector4 margin
		{
			get
			{
				return this.m_margin;
			}
			set
			{
				if (this.m_margin == value)
				{
					return;
				}
				this.m_margin = value;
				this.ComputeMarginSize();
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		public TMP_TextInfo textInfo
		{
			get
			{
				return this.m_textInfo;
			}
		}

		public bool havePropertiesChanged
		{
			get
			{
				return this.m_havePropertiesChanged;
			}
			set
			{
				if (this.m_havePropertiesChanged == value)
				{
					return;
				}
				this.m_havePropertiesChanged = value;
				this.m_isInputParsingRequired = true;
				this.SetAllDirty();
			}
		}

		public bool isUsingLegacyAnimationComponent
		{
			get
			{
				return this.m_isUsingLegacyAnimationComponent;
			}
			set
			{
				this.m_isUsingLegacyAnimationComponent = value;
			}
		}

		public new Transform transform
		{
			get
			{
				if (this.m_transform == null)
				{
					this.m_transform = base.GetComponent<Transform>();
				}
				return this.m_transform;
			}
		}

		public new RectTransform rectTransform
		{
			get
			{
				if (this.m_rectTransform == null)
				{
					this.m_rectTransform = base.GetComponent<RectTransform>();
				}
				return this.m_rectTransform;
			}
		}

		public virtual bool autoSizeTextContainer { get; set; }

		public virtual Mesh mesh
		{
			get
			{
				return this.m_mesh;
			}
		}

		public bool isVolumetricText
		{
			get
			{
				return this.m_isVolumetricText;
			}
			set
			{
				if (this.m_isVolumetricText == value)
				{
					return;
				}
				this.m_havePropertiesChanged = value;
				this.m_textInfo.ResetVertexLayout(value);
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public Bounds bounds
		{
			get
			{
				if (this.m_mesh == null)
				{
					return default(Bounds);
				}
				return this.GetCompoundBounds();
			}
		}

		public Bounds textBounds
		{
			get
			{
				if (this.m_textInfo == null)
				{
					return default(Bounds);
				}
				return this.GetTextBounds();
			}
		}

		protected TMP_SpriteAnimator spriteAnimator
		{
			get
			{
				if (this.m_spriteAnimator == null)
				{
					this.m_spriteAnimator = base.GetComponent<TMP_SpriteAnimator>();
					if (this.m_spriteAnimator == null)
					{
						this.m_spriteAnimator = base.gameObject.AddComponent<TMP_SpriteAnimator>();
					}
				}
				return this.m_spriteAnimator;
			}
		}

		public float flexibleHeight
		{
			get
			{
				return this.m_flexibleHeight;
			}
		}

		public float flexibleWidth
		{
			get
			{
				return this.m_flexibleWidth;
			}
		}

		public float minWidth
		{
			get
			{
				return this.m_minWidth;
			}
		}

		public float minHeight
		{
			get
			{
				return this.m_minHeight;
			}
		}

		public float maxWidth
		{
			get
			{
				return this.m_maxWidth;
			}
		}

		public float maxHeight
		{
			get
			{
				return this.m_maxHeight;
			}
		}

		protected LayoutElement layoutElement
		{
			get
			{
				if (this.m_LayoutElement == null)
				{
					this.m_LayoutElement = base.GetComponent<LayoutElement>();
				}
				return this.m_LayoutElement;
			}
		}

		public virtual float preferredWidth
		{
			get
			{
				if (!this.m_isPreferredWidthDirty)
				{
					return this.m_preferredWidth;
				}
				this.m_preferredWidth = this.GetPreferredWidth();
				return this.m_preferredWidth;
			}
		}

		public virtual float preferredHeight
		{
			get
			{
				if (!this.m_isPreferredHeightDirty)
				{
					return this.m_preferredHeight;
				}
				this.m_preferredHeight = this.GetPreferredHeight();
				return this.m_preferredHeight;
			}
		}

		public virtual float renderedWidth
		{
			get
			{
				return this.GetRenderedWidth();
			}
		}

		public virtual float renderedHeight
		{
			get
			{
				return this.GetRenderedHeight();
			}
		}

		public int layoutPriority
		{
			get
			{
				return this.m_layoutPriority;
			}
		}

		protected virtual void LoadFontAsset()
		{
		}

		protected virtual void SetSharedMaterial(Material mat)
		{
		}

		protected virtual Material GetMaterial(Material mat)
		{
			return null;
		}

		protected virtual void SetFontBaseMaterial(Material mat)
		{
		}

		protected virtual Material[] GetSharedMaterials()
		{
			return null;
		}

		protected virtual void SetSharedMaterials(Material[] materials)
		{
		}

		protected virtual Material[] GetMaterials(Material[] mats)
		{
			return null;
		}

		protected virtual Material CreateMaterialInstance(Material source)
		{
			Material material = new Material(source);
			material.shaderKeywords = source.shaderKeywords;
			material.name += " (Instance)";
			return material;
		}

		protected void SetVertexColorGradient(TMP_ColorGradient gradient)
		{
			if (gradient == null)
			{
				return;
			}
			this.m_fontColorGradient.bottomLeft = gradient.bottomLeft;
			this.m_fontColorGradient.bottomRight = gradient.bottomRight;
			this.m_fontColorGradient.topLeft = gradient.topLeft;
			this.m_fontColorGradient.topRight = gradient.topRight;
			this.SetVerticesDirty();
		}

		protected void SetTextSortingOrder(VertexSortingOrder order)
		{
		}

		protected void SetTextSortingOrder(int[] order)
		{
		}

		protected virtual void SetFaceColor(Color32 color)
		{
		}

		protected virtual void SetOutlineColor(Color32 color)
		{
		}

		protected virtual void SetOutlineThickness(float thickness)
		{
		}

		protected virtual void SetShaderDepth()
		{
		}

		protected virtual void SetCulling()
		{
		}

		protected virtual float GetPaddingForMaterial()
		{
			return 0f;
		}

		protected virtual float GetPaddingForMaterial(Material mat)
		{
			return 0f;
		}

		protected virtual Vector3[] GetTextContainerLocalCorners()
		{
			return null;
		}

		public virtual void ForceMeshUpdate()
		{
		}

		public virtual void ForceMeshUpdate(bool ignoreActiveState)
		{
		}

		internal void SetTextInternal(string text)
		{
			this.m_text = text;
			this.m_renderMode = TextRenderFlags.DontRender;
			this.m_isInputParsingRequired = true;
			this.ForceMeshUpdate();
			this.m_renderMode = TextRenderFlags.Render;
		}

		public virtual void UpdateGeometry(Mesh mesh, int index)
		{
		}

		public virtual void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
		{
		}

		public virtual void UpdateVertexData()
		{
		}

		public virtual void SetVertices(Vector3[] vertices)
		{
		}

		public virtual void UpdateMeshPadding()
		{
		}

		public new void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
			base.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
			this.InternalCrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
		}

		public new void CrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
			base.CrossFadeAlpha(alpha, duration, ignoreTimeScale);
			this.InternalCrossFadeAlpha(alpha, duration, ignoreTimeScale);
		}

		protected virtual void InternalCrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
		}

		protected virtual void InternalCrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
		}

		protected void ParseInputText()
		{
			this.m_isInputParsingRequired = false;
			switch (this.m_inputSource)
			{
			case TMP_Text.TextInputSources.Text:
			case TMP_Text.TextInputSources.String:
				this.StringToCharArray(this.m_text, ref this.m_char_buffer);
				break;
			case TMP_Text.TextInputSources.SetText:
				this.SetTextArrayToCharArray(this.m_input_CharArray, ref this.m_char_buffer);
				break;
			}
			this.SetArraySizes(this.m_char_buffer);
		}

		public void SetText(string text)
		{
			this.SetText(text, true);
		}

		public void SetText(string text, bool syncTextInputBox)
		{
			this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
			this.StringToCharArray(text, ref this.m_char_buffer);
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		public void SetText(string text, float arg0)
		{
			this.SetText(text, arg0, 255f, 255f);
		}

		public void SetText(string text, float arg0, float arg1)
		{
			this.SetText(text, arg0, arg1, 255f);
		}

		public void SetText(string text, float arg0, float arg1, float arg2)
		{
			if (text == this.old_text && arg0 == this.old_arg0 && arg1 == this.old_arg1 && arg2 == this.old_arg2)
			{
				return;
			}
			this.old_text = text;
			this.old_arg1 = 255f;
			this.old_arg2 = 255f;
			int precision = 0;
			int num = 0;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '{')
				{
					if (text[i + 2] == ':')
					{
						precision = (int)(text[i + 3] - '0');
					}
					switch (text[i + 1])
					{
					case '0':
						this.old_arg0 = arg0;
						this.AddFloatToCharArray(arg0, ref num, precision);
						break;
					case '1':
						this.old_arg1 = arg1;
						this.AddFloatToCharArray(arg1, ref num, precision);
						break;
					case '2':
						this.old_arg2 = arg2;
						this.AddFloatToCharArray(arg2, ref num, precision);
						break;
					}
					if (text[i + 2] == ':')
					{
						i += 4;
					}
					else
					{
						i += 2;
					}
				}
				else
				{
					this.m_input_CharArray[num] = c;
					num++;
				}
			}
			this.m_input_CharArray[num] = '\0';
			this.m_charArray_Length = num;
			this.m_inputSource = TMP_Text.TextInputSources.SetText;
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		public void SetText(StringBuilder text)
		{
			this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
			this.StringBuilderToIntArray(text, ref this.m_char_buffer);
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		public void SetCharArray(char[] sourceText)
		{
			if (sourceText == null || sourceText.Length == 0)
			{
				return;
			}
			this.m_styleStack.Clear();
			if (this.m_char_buffer.Length <= sourceText.Length)
			{
				int num = Mathf.NextPowerOfTwo(sourceText.Length + 1);
				this.m_char_buffer = new int[num];
			}
			int num2 = 0;
			int i = 0;
			while (i < sourceText.Length)
			{
				if (sourceText[i] != '\\' || i >= sourceText.Length - 1)
				{
					goto IL_AB;
				}
				int num3 = (int)sourceText[i + 1];
				if (num3 != 110)
				{
					if (num3 != 114)
					{
						if (num3 != 116)
						{
							goto IL_AB;
						}
						this.m_char_buffer[num2] = 9;
						i++;
						num2++;
					}
					else
					{
						this.m_char_buffer[num2] = 13;
						i++;
						num2++;
					}
				}
				else
				{
					this.m_char_buffer[num2] = 10;
					i++;
					num2++;
				}
				IL_13A:
				i++;
				continue;
				IL_AB:
				if (sourceText[i] == '<')
				{
					if (this.IsTagName(ref sourceText, "<BR>", i))
					{
						this.m_char_buffer[num2] = 10;
						num2++;
						i += 3;
						goto IL_13A;
					}
					if (this.IsTagName(ref sourceText, "<STYLE=", i))
					{
						int num4 = 0;
						if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num4, ref this.m_char_buffer, ref num2))
						{
							i = num4;
							goto IL_13A;
						}
					}
					else if (this.IsTagName(ref sourceText, "</STYLE>", i))
					{
						this.ReplaceClosingStyleTag(ref sourceText, i, ref this.m_char_buffer, ref num2);
						i += 7;
						goto IL_13A;
					}
				}
				this.m_char_buffer[num2] = (int)sourceText[i];
				num2++;
				goto IL_13A;
			}
			this.m_char_buffer[num2] = 0;
			this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
			this.m_havePropertiesChanged = true;
			this.m_isInputParsingRequired = true;
		}

		public void SetCharArray(int[] sourceText, int start, int length)
		{
			if (sourceText == null || sourceText.Length == 0 || length == 0)
			{
				return;
			}
			this.m_styleStack.Clear();
			if (this.m_char_buffer.Length <= length)
			{
				int num = Mathf.NextPowerOfTwo(length + 1);
				this.m_char_buffer = new int[num];
			}
			int num2 = 0;
			int i = 0;
			while (i < length)
			{
				if (sourceText[start + i] != 92 || i >= length - 1)
				{
					goto IL_AC;
				}
				int num3 = sourceText[start + i + 1];
				if (num3 != 110)
				{
					if (num3 != 114)
					{
						if (num3 != 116)
						{
							goto IL_AC;
						}
						this.m_char_buffer[num2] = 9;
						i++;
						num2++;
					}
					else
					{
						this.m_char_buffer[num2] = 13;
						i++;
						num2++;
					}
				}
				else
				{
					this.m_char_buffer[num2] = 10;
					i++;
					num2++;
				}
				IL_13D:
				i++;
				continue;
				IL_AC:
				if (sourceText[i] == 60)
				{
					if (this.IsTagName(ref sourceText, "<BR>", i))
					{
						this.m_char_buffer[num2] = 10;
						num2++;
						i += 3;
						goto IL_13D;
					}
					if (this.IsTagName(ref sourceText, "<STYLE=", i))
					{
						int num4 = 0;
						if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num4, ref this.m_char_buffer, ref num2))
						{
							i = num4;
							goto IL_13D;
						}
					}
					else if (this.IsTagName(ref sourceText, "</STYLE>", i))
					{
						this.ReplaceClosingStyleTag(ref sourceText, i, ref this.m_char_buffer, ref num2);
						i += 7;
						goto IL_13D;
					}
				}
				this.m_char_buffer[num2] = sourceText[start + i];
				num2++;
				goto IL_13D;
			}
			this.m_char_buffer[num2] = 0;
			this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
			this.m_havePropertiesChanged = true;
			this.m_isInputParsingRequired = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		protected void SetTextArrayToCharArray(char[] sourceText, ref int[] charBuffer)
		{
			if (sourceText == null || this.m_charArray_Length == 0)
			{
				return;
			}
			this.m_styleStack.Clear();
			if (charBuffer.Length <= this.m_charArray_Length)
			{
				int num = (this.m_charArray_Length > 1024) ? (this.m_charArray_Length + 256) : Mathf.NextPowerOfTwo(this.m_charArray_Length + 1);
				charBuffer = new int[num];
			}
			int num2 = 0;
			for (int i = 0; i < this.m_charArray_Length; i++)
			{
				if (char.IsHighSurrogate(sourceText[i]) && char.IsLowSurrogate(sourceText[i + 1]))
				{
					charBuffer[num2] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
					i++;
					num2++;
				}
				else
				{
					if (sourceText[i] == '<')
					{
						if (this.IsTagName(ref sourceText, "<BR>", i))
						{
							charBuffer[num2] = 10;
							num2++;
							i += 3;
							goto IL_109;
						}
						if (this.IsTagName(ref sourceText, "<STYLE=", i))
						{
							int num3 = 0;
							if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num3, ref charBuffer, ref num2))
							{
								i = num3;
								goto IL_109;
							}
						}
						else if (this.IsTagName(ref sourceText, "</STYLE>", i))
						{
							this.ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref num2);
							i += 7;
							goto IL_109;
						}
					}
					charBuffer[num2] = (int)sourceText[i];
					num2++;
				}
				IL_109:;
			}
			charBuffer[num2] = 0;
		}

		protected void StringToCharArray(string sourceText, ref int[] charBuffer)
		{
			if (sourceText == null)
			{
				charBuffer[0] = 0;
				return;
			}
			this.m_styleStack.SetDefault(0);
			if (charBuffer == null || charBuffer.Length <= sourceText.Length)
			{
				int num = (sourceText.Length > 1024) ? (sourceText.Length + 256) : Mathf.NextPowerOfTwo(sourceText.Length + 1);
				charBuffer = new int[num];
			}
			int num2 = 0;
			int i = 0;
			while (i < sourceText.Length)
			{
				if (this.m_inputSource != TMP_Text.TextInputSources.Text || sourceText[i] != '\\' || sourceText.Length <= i + 1)
				{
					goto IL_1AD;
				}
				int num3 = (int)sourceText[i + 1];
				if (num3 <= 92)
				{
					if (num3 != 85)
					{
						if (num3 != 92)
						{
							goto IL_1AD;
						}
						if (!this.m_parseCtrlCharacters || sourceText.Length <= i + 2)
						{
							goto IL_1AD;
						}
						charBuffer[num2] = (int)sourceText[i + 1];
						charBuffer[num2 + 1] = (int)sourceText[i + 2];
						i += 2;
						num2 += 2;
					}
					else
					{
						if (sourceText.Length <= i + 9)
						{
							goto IL_1AD;
						}
						charBuffer[num2] = this.GetUTF32(i + 2);
						i += 9;
						num2++;
					}
				}
				else if (num3 != 110)
				{
					switch (num3)
					{
					case 114:
						if (!this.m_parseCtrlCharacters)
						{
							goto IL_1AD;
						}
						charBuffer[num2] = 13;
						i++;
						num2++;
						break;
					case 115:
						goto IL_1AD;
					case 116:
						if (!this.m_parseCtrlCharacters)
						{
							goto IL_1AD;
						}
						charBuffer[num2] = 9;
						i++;
						num2++;
						break;
					case 117:
						if (sourceText.Length <= i + 5)
						{
							goto IL_1AD;
						}
						charBuffer[num2] = (int)((ushort)this.GetUTF16(i + 2));
						i += 5;
						num2++;
						break;
					default:
						goto IL_1AD;
					}
				}
				else
				{
					if (!this.m_parseCtrlCharacters)
					{
						goto IL_1AD;
					}
					charBuffer[num2] = 10;
					i++;
					num2++;
				}
				IL_276:
				i++;
				continue;
				IL_1AD:
				if (char.IsHighSurrogate(sourceText[i]) && char.IsLowSurrogate(sourceText[i + 1]))
				{
					charBuffer[num2] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
					i++;
					num2++;
					goto IL_276;
				}
				if (sourceText[i] == '<')
				{
					if (this.IsTagName(ref sourceText, "<BR>", i))
					{
						charBuffer[num2] = 10;
						num2++;
						i += 3;
						goto IL_276;
					}
					if (this.IsTagName(ref sourceText, "<STYLE=", i))
					{
						int num4 = 0;
						if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num4, ref charBuffer, ref num2))
						{
							i = num4;
							goto IL_276;
						}
					}
					else if (this.IsTagName(ref sourceText, "</STYLE>", i))
					{
						this.ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref num2);
						i += 7;
						goto IL_276;
					}
				}
				charBuffer[num2] = (int)sourceText[i];
				num2++;
				goto IL_276;
			}
			charBuffer[num2] = 0;
		}

		protected void StringBuilderToIntArray(StringBuilder sourceText, ref int[] charBuffer)
		{
			if (sourceText == null)
			{
				charBuffer[0] = 0;
				return;
			}
			this.m_styleStack.Clear();
			if (charBuffer == null || charBuffer.Length <= sourceText.Length)
			{
				int num = (sourceText.Length > 1024) ? (sourceText.Length + 256) : Mathf.NextPowerOfTwo(sourceText.Length + 1);
				charBuffer = new int[num];
			}
			int num2 = 0;
			int i = 0;
			while (i < sourceText.Length)
			{
				if (!this.m_parseCtrlCharacters || sourceText[i] != '\\' || sourceText.Length <= i + 1)
				{
					goto IL_186;
				}
				int num3 = (int)sourceText[i + 1];
				if (num3 <= 92)
				{
					if (num3 != 85)
					{
						if (num3 != 92)
						{
							goto IL_186;
						}
						if (sourceText.Length <= i + 2)
						{
							goto IL_186;
						}
						charBuffer[num2] = (int)sourceText[i + 1];
						charBuffer[num2 + 1] = (int)sourceText[i + 2];
						i += 2;
						num2 += 2;
					}
					else
					{
						if (sourceText.Length <= i + 9)
						{
							goto IL_186;
						}
						charBuffer[num2] = this.GetUTF32(i + 2);
						i += 9;
						num2++;
					}
				}
				else if (num3 != 110)
				{
					switch (num3)
					{
					case 114:
						charBuffer[num2] = 13;
						i++;
						num2++;
						break;
					case 115:
						goto IL_186;
					case 116:
						charBuffer[num2] = 9;
						i++;
						num2++;
						break;
					case 117:
						if (sourceText.Length <= i + 5)
						{
							goto IL_186;
						}
						charBuffer[num2] = (int)((ushort)this.GetUTF16(i + 2));
						i += 5;
						num2++;
						break;
					default:
						goto IL_186;
					}
				}
				else
				{
					charBuffer[num2] = 10;
					i++;
					num2++;
				}
				IL_24F:
				i++;
				continue;
				IL_186:
				if (char.IsHighSurrogate(sourceText[i]) && char.IsLowSurrogate(sourceText[i + 1]))
				{
					charBuffer[num2] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
					i++;
					num2++;
					goto IL_24F;
				}
				if (sourceText[i] == '<')
				{
					if (this.IsTagName(ref sourceText, "<BR>", i))
					{
						charBuffer[num2] = 10;
						num2++;
						i += 3;
						goto IL_24F;
					}
					if (this.IsTagName(ref sourceText, "<STYLE=", i))
					{
						int num4 = 0;
						if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num4, ref charBuffer, ref num2))
						{
							i = num4;
							goto IL_24F;
						}
					}
					else if (this.IsTagName(ref sourceText, "</STYLE>", i))
					{
						this.ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref num2);
						i += 7;
						goto IL_24F;
					}
				}
				charBuffer[num2] = (int)sourceText[i];
				num2++;
				goto IL_24F;
			}
			charBuffer[num2] = 0;
		}

		private bool ReplaceOpeningStyleTag(ref string sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(this.GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset));
			if (style == null || srcOffset == 0)
			{
				return false;
			}
			this.m_styleStack.Add(style.hashCode);
			int num = style.styleOpeningTagArray.Length;
			int num2 = srcIndex + num + sourceText.Length - srcOffset;
			if (num2 > charBuffer.Length)
			{
				int newSize = (charBuffer.Length > 1024) ? (charBuffer.Length + 256) : Mathf.NextPowerOfTwo(num2 + 1);
				Array.Resize<int>(ref charBuffer, newSize);
			}
			int[] styleOpeningTagArray = style.styleOpeningTagArray;
			int i = 0;
			while (i < num)
			{
				int num3 = styleOpeningTagArray[i];
				if (num3 != 60)
				{
					goto IL_110;
				}
				if (this.IsTagName(ref styleOpeningTagArray, "<BR>", i))
				{
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleOpeningTagArray, "<STYLE=", i))
				{
					int num4 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleOpeningTagArray, i, out num4, ref charBuffer, ref writeIndex))
					{
						goto IL_110;
					}
					i = num4;
				}
				else
				{
					if (!this.IsTagName(ref styleOpeningTagArray, "</STYLE>", i))
					{
						goto IL_110;
					}
					this.ReplaceClosingStyleTag(ref styleOpeningTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_121:
				i++;
				continue;
				IL_110:
				charBuffer[writeIndex] = num3;
				writeIndex++;
				goto IL_121;
			}
			return true;
		}

		private bool ReplaceOpeningStyleTag(ref int[] sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(this.GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset));
			if (style == null || srcOffset == 0)
			{
				return false;
			}
			this.m_styleStack.Add(style.hashCode);
			int num = style.styleOpeningTagArray.Length;
			int num2 = srcIndex + num + sourceText.Length - srcOffset;
			if (num2 > charBuffer.Length)
			{
				int newSize = (charBuffer.Length > 1024) ? (charBuffer.Length + 256) : Mathf.NextPowerOfTwo(num2 + 1);
				Array.Resize<int>(ref charBuffer, newSize);
			}
			int[] styleOpeningTagArray = style.styleOpeningTagArray;
			int i = 0;
			while (i < num)
			{
				int num3 = styleOpeningTagArray[i];
				if (num3 != 60)
				{
					goto IL_10D;
				}
				if (this.IsTagName(ref styleOpeningTagArray, "<BR>", i))
				{
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleOpeningTagArray, "<STYLE=", i))
				{
					int num4 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleOpeningTagArray, i, out num4, ref charBuffer, ref writeIndex))
					{
						goto IL_10D;
					}
					i = num4;
				}
				else
				{
					if (!this.IsTagName(ref styleOpeningTagArray, "</STYLE>", i))
					{
						goto IL_10D;
					}
					this.ReplaceClosingStyleTag(ref styleOpeningTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_11E:
				i++;
				continue;
				IL_10D:
				charBuffer[writeIndex] = num3;
				writeIndex++;
				goto IL_11E;
			}
			return true;
		}

		private bool ReplaceOpeningStyleTag(ref char[] sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(this.GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset));
			if (style == null || srcOffset == 0)
			{
				return false;
			}
			this.m_styleStack.Add(style.hashCode);
			int num = style.styleOpeningTagArray.Length;
			int num2 = srcIndex + num + sourceText.Length - srcOffset;
			if (num2 > charBuffer.Length)
			{
				int newSize = (charBuffer.Length > 1024) ? (charBuffer.Length + 256) : Mathf.NextPowerOfTwo(num2 + 1);
				Array.Resize<int>(ref charBuffer, newSize);
			}
			int[] styleOpeningTagArray = style.styleOpeningTagArray;
			int i = 0;
			while (i < num)
			{
				int num3 = styleOpeningTagArray[i];
				if (num3 != 60)
				{
					goto IL_10D;
				}
				if (this.IsTagName(ref styleOpeningTagArray, "<BR>", i))
				{
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleOpeningTagArray, "<STYLE=", i))
				{
					int num4 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleOpeningTagArray, i, out num4, ref charBuffer, ref writeIndex))
					{
						goto IL_10D;
					}
					i = num4;
				}
				else
				{
					if (!this.IsTagName(ref styleOpeningTagArray, "</STYLE>", i))
					{
						goto IL_10D;
					}
					this.ReplaceClosingStyleTag(ref styleOpeningTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_11E:
				i++;
				continue;
				IL_10D:
				charBuffer[writeIndex] = num3;
				writeIndex++;
				goto IL_11E;
			}
			return true;
		}

		private bool ReplaceOpeningStyleTag(ref StringBuilder sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(this.GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset));
			if (style == null || srcOffset == 0)
			{
				return false;
			}
			this.m_styleStack.Add(style.hashCode);
			int num = style.styleOpeningTagArray.Length;
			int num2 = srcIndex + num + sourceText.Length - srcOffset;
			if (num2 > charBuffer.Length)
			{
				int newSize = (charBuffer.Length > 1024) ? (charBuffer.Length + 256) : Mathf.NextPowerOfTwo(num2 + 1);
				Array.Resize<int>(ref charBuffer, newSize);
			}
			int[] styleOpeningTagArray = style.styleOpeningTagArray;
			int i = 0;
			while (i < num)
			{
				int num3 = styleOpeningTagArray[i];
				if (num3 != 60)
				{
					goto IL_110;
				}
				if (this.IsTagName(ref styleOpeningTagArray, "<BR>", i))
				{
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleOpeningTagArray, "<STYLE=", i))
				{
					int num4 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleOpeningTagArray, i, out num4, ref charBuffer, ref writeIndex))
					{
						goto IL_110;
					}
					i = num4;
				}
				else
				{
					if (!this.IsTagName(ref styleOpeningTagArray, "</STYLE>", i))
					{
						goto IL_110;
					}
					this.ReplaceClosingStyleTag(ref styleOpeningTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_121:
				i++;
				continue;
				IL_110:
				charBuffer[writeIndex] = num3;
				writeIndex++;
				goto IL_121;
			}
			return true;
		}

		private bool ReplaceClosingStyleTag(ref string sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(this.m_styleStack.CurrentItem());
			this.m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int num2 = srcIndex + num + sourceText.Length - 8;
			if (num2 > charBuffer.Length)
			{
				int newSize = (charBuffer.Length > 1024) ? (charBuffer.Length + 256) : Mathf.NextPowerOfTwo(num2 + 1);
				Array.Resize<int>(ref charBuffer, newSize);
			}
			int[] styleClosingTagArray = style.styleClosingTagArray;
			int i = 0;
			while (i < num)
			{
				int num3 = styleClosingTagArray[i];
				if (num3 != 60)
				{
					goto IL_FF;
				}
				if (this.IsTagName(ref styleClosingTagArray, "<BR>", i))
				{
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleClosingTagArray, "<STYLE=", i))
				{
					int num4 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleClosingTagArray, i, out num4, ref charBuffer, ref writeIndex))
					{
						goto IL_FF;
					}
					i = num4;
				}
				else
				{
					if (!this.IsTagName(ref styleClosingTagArray, "</STYLE>", i))
					{
						goto IL_FF;
					}
					this.ReplaceClosingStyleTag(ref styleClosingTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_10F:
				i++;
				continue;
				IL_FF:
				charBuffer[writeIndex] = num3;
				writeIndex++;
				goto IL_10F;
			}
			return true;
		}

		private bool ReplaceClosingStyleTag(ref int[] sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(this.m_styleStack.CurrentItem());
			this.m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int num2 = srcIndex + num + sourceText.Length - 8;
			if (num2 > charBuffer.Length)
			{
				int newSize = (charBuffer.Length > 1024) ? (charBuffer.Length + 256) : Mathf.NextPowerOfTwo(num2 + 1);
				Array.Resize<int>(ref charBuffer, newSize);
			}
			int[] styleClosingTagArray = style.styleClosingTagArray;
			int i = 0;
			while (i < num)
			{
				int num3 = styleClosingTagArray[i];
				if (num3 != 60)
				{
					goto IL_FC;
				}
				if (this.IsTagName(ref styleClosingTagArray, "<BR>", i))
				{
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleClosingTagArray, "<STYLE=", i))
				{
					int num4 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleClosingTagArray, i, out num4, ref charBuffer, ref writeIndex))
					{
						goto IL_FC;
					}
					i = num4;
				}
				else
				{
					if (!this.IsTagName(ref styleClosingTagArray, "</STYLE>", i))
					{
						goto IL_FC;
					}
					this.ReplaceClosingStyleTag(ref styleClosingTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_10C:
				i++;
				continue;
				IL_FC:
				charBuffer[writeIndex] = num3;
				writeIndex++;
				goto IL_10C;
			}
			return true;
		}

		private bool ReplaceClosingStyleTag(ref char[] sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(this.m_styleStack.CurrentItem());
			this.m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int num2 = srcIndex + num + sourceText.Length - 8;
			if (num2 > charBuffer.Length)
			{
				int newSize = (charBuffer.Length > 1024) ? (charBuffer.Length + 256) : Mathf.NextPowerOfTwo(num2 + 1);
				Array.Resize<int>(ref charBuffer, newSize);
			}
			int[] styleClosingTagArray = style.styleClosingTagArray;
			int i = 0;
			while (i < num)
			{
				int num3 = styleClosingTagArray[i];
				if (num3 != 60)
				{
					goto IL_FC;
				}
				if (this.IsTagName(ref styleClosingTagArray, "<BR>", i))
				{
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleClosingTagArray, "<STYLE=", i))
				{
					int num4 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleClosingTagArray, i, out num4, ref charBuffer, ref writeIndex))
					{
						goto IL_FC;
					}
					i = num4;
				}
				else
				{
					if (!this.IsTagName(ref styleClosingTagArray, "</STYLE>", i))
					{
						goto IL_FC;
					}
					this.ReplaceClosingStyleTag(ref styleClosingTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_10C:
				i++;
				continue;
				IL_FC:
				charBuffer[writeIndex] = num3;
				writeIndex++;
				goto IL_10C;
			}
			return true;
		}

		private bool ReplaceClosingStyleTag(ref StringBuilder sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(this.m_styleStack.CurrentItem());
			this.m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int num2 = srcIndex + num + sourceText.Length - 8;
			if (num2 > charBuffer.Length)
			{
				int newSize = (charBuffer.Length > 1024) ? (charBuffer.Length + 256) : Mathf.NextPowerOfTwo(num2 + 1);
				Array.Resize<int>(ref charBuffer, newSize);
			}
			int[] styleClosingTagArray = style.styleClosingTagArray;
			int i = 0;
			while (i < num)
			{
				int num3 = styleClosingTagArray[i];
				if (num3 != 60)
				{
					goto IL_FF;
				}
				if (this.IsTagName(ref styleClosingTagArray, "<BR>", i))
				{
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleClosingTagArray, "<STYLE=", i))
				{
					int num4 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleClosingTagArray, i, out num4, ref charBuffer, ref writeIndex))
					{
						goto IL_FF;
					}
					i = num4;
				}
				else
				{
					if (!this.IsTagName(ref styleClosingTagArray, "</STYLE>", i))
					{
						goto IL_FF;
					}
					this.ReplaceClosingStyleTag(ref styleClosingTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_10F:
				i++;
				continue;
				IL_FF:
				charBuffer[writeIndex] = num3;
				writeIndex++;
				goto IL_10F;
			}
			return true;
		}

		private bool IsTagName(ref string text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
			{
				return false;
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast(text[index + i]) != tag[i])
				{
					return false;
				}
			}
			return true;
		}

		private bool IsTagName(ref char[] text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
			{
				return false;
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast(text[index + i]) != tag[i])
				{
					return false;
				}
			}
			return true;
		}

		private bool IsTagName(ref int[] text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
			{
				return false;
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast((char)text[index + i]) != tag[i])
				{
					return false;
				}
			}
			return true;
		}

		private bool IsTagName(ref StringBuilder text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
			{
				return false;
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast(text[index + i]) != tag[i])
				{
					return false;
				}
			}
			return true;
		}

		private int GetTagHashCode(ref string text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				if (text[i] != '"')
				{
					if (text[i] == '>')
					{
						closeIndex = i;
						break;
					}
					num = ((num << 5) + num ^ (int)text[i]);
				}
			}
			return num;
		}

		private int GetTagHashCode(ref char[] text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				if (text[i] != '"')
				{
					if (text[i] == '>')
					{
						closeIndex = i;
						break;
					}
					num = ((num << 5) + num ^ (int)text[i]);
				}
			}
			return num;
		}

		private int GetTagHashCode(ref int[] text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				if (text[i] != 34)
				{
					if (text[i] == 62)
					{
						closeIndex = i;
						break;
					}
					num = ((num << 5) + num ^ text[i]);
				}
			}
			return num;
		}

		private int GetTagHashCode(ref StringBuilder text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				if (text[i] != '"')
				{
					if (text[i] == '>')
					{
						closeIndex = i;
						break;
					}
					num = ((num << 5) + num ^ (int)text[i]);
				}
			}
			return num;
		}

		protected void AddFloatToCharArray(float number, ref int index, int precision)
		{
			if (number < 0f)
			{
				char[] input_CharArray = this.m_input_CharArray;
				int num = index;
				index = num + 1;
				input_CharArray[num] = 45;
				number = -number;
			}
			number += this.k_Power[Mathf.Min(9, precision)];
			int num2 = (int)number;
			this.AddIntToCharArray(num2, ref index, precision);
			if (precision > 0)
			{
				char[] input_CharArray2 = this.m_input_CharArray;
				int num = index;
				index = num + 1;
				input_CharArray2[num] = 46;
				number -= (float)num2;
				for (int i = 0; i < precision; i++)
				{
					number *= 10f;
					int num3 = (int)number;
					char[] input_CharArray3 = this.m_input_CharArray;
					num = index;
					index = num + 1;
					input_CharArray3[num] = (ushort)(num3 + 48);
					number -= (float)num3;
				}
			}
		}

		protected void AddIntToCharArray(int number, ref int index, int precision)
		{
			if (number < 0)
			{
				char[] input_CharArray = this.m_input_CharArray;
				int num = index;
				index = num + 1;
				input_CharArray[num] = 45;
				number = -number;
			}
			int num2 = index;
			do
			{
				this.m_input_CharArray[num2++] = (char)(number % 10 + 48);
				number /= 10;
			}
			while (number > 0);
			int num3 = num2;
			while (index + 1 < num2)
			{
				num2--;
				char c = this.m_input_CharArray[index];
				this.m_input_CharArray[index] = this.m_input_CharArray[num2];
				this.m_input_CharArray[num2] = c;
				index++;
			}
			index = num3;
		}

		protected virtual int SetArraySizes(int[] chars)
		{
			return 0;
		}

		protected virtual void GenerateTextMesh()
		{
		}

		public Vector2 GetPreferredValues()
		{
			if (this.m_isInputParsingRequired || this.m_isTextTruncated)
			{
				this.m_isCalculatingPreferredValues = true;
				this.ParseInputText();
			}
			float preferredWidth = this.GetPreferredWidth();
			float preferredHeight = this.GetPreferredHeight();
			return new Vector2(preferredWidth, preferredHeight);
		}

		public Vector2 GetPreferredValues(float width, float height)
		{
			if (this.m_isInputParsingRequired || this.m_isTextTruncated)
			{
				this.m_isCalculatingPreferredValues = true;
				this.ParseInputText();
			}
			Vector2 margin = new Vector2(width, height);
			float preferredWidth = this.GetPreferredWidth(margin);
			float preferredHeight = this.GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		public Vector2 GetPreferredValues(string text)
		{
			this.m_isCalculatingPreferredValues = true;
			this.StringToCharArray(text, ref this.m_char_buffer);
			this.SetArraySizes(this.m_char_buffer);
			Vector2 margin = TMP_Text.k_LargePositiveVector2;
			float preferredWidth = this.GetPreferredWidth(margin);
			float preferredHeight = this.GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		public Vector2 GetPreferredValues(string text, float width, float height)
		{
			this.m_isCalculatingPreferredValues = true;
			this.StringToCharArray(text, ref this.m_char_buffer);
			this.SetArraySizes(this.m_char_buffer);
			Vector2 margin = new Vector2(width, height);
			float preferredWidth = this.GetPreferredWidth(margin);
			float preferredHeight = this.GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		protected float GetPreferredWidth()
		{
			float defaultFontSize = this.m_enableAutoSizing ? this.m_fontSizeMax : this.m_fontSize;
			Vector2 marginSize = TMP_Text.k_LargePositiveVector2;
			if (this.m_isInputParsingRequired || this.m_isTextTruncated)
			{
				this.m_isCalculatingPreferredValues = true;
				this.ParseInputText();
			}
			this.m_recursiveCount = 0;
			float x = this.CalculatePreferredValues(defaultFontSize, marginSize, true).x;
			this.m_isPreferredWidthDirty = false;
			return x;
		}

		protected float GetPreferredWidth(Vector2 margin)
		{
			float defaultFontSize = this.m_enableAutoSizing ? this.m_fontSizeMax : this.m_fontSize;
			this.m_recursiveCount = 0;
			return this.CalculatePreferredValues(defaultFontSize, margin, true).x;
		}

		protected float GetPreferredHeight()
		{
			float defaultFontSize = this.m_enableAutoSizing ? this.m_fontSizeMax : this.m_fontSize;
			Vector2 marginSize = new Vector2((this.m_marginWidth != 0f) ? this.m_marginWidth : TMP_Text.k_LargePositiveFloat, TMP_Text.k_LargePositiveFloat);
			if (this.m_isInputParsingRequired || this.m_isTextTruncated)
			{
				this.m_isCalculatingPreferredValues = true;
				this.ParseInputText();
			}
			this.m_recursiveCount = 0;
			float y = this.CalculatePreferredValues(defaultFontSize, marginSize, !this.m_enableAutoSizing).y;
			this.m_isPreferredHeightDirty = false;
			return y;
		}

		protected float GetPreferredHeight(Vector2 margin)
		{
			float defaultFontSize = this.m_enableAutoSizing ? this.m_fontSizeMax : this.m_fontSize;
			this.m_recursiveCount = 0;
			return this.CalculatePreferredValues(defaultFontSize, margin, true).y;
		}

		public Vector2 GetRenderedValues()
		{
			return this.GetTextBounds().size;
		}

		public Vector2 GetRenderedValues(bool onlyVisibleCharacters)
		{
			return this.GetTextBounds(onlyVisibleCharacters).size;
		}

		protected float GetRenderedWidth()
		{
			return this.GetRenderedValues().x;
		}

		protected float GetRenderedWidth(bool onlyVisibleCharacters)
		{
			return this.GetRenderedValues(onlyVisibleCharacters).x;
		}

		protected float GetRenderedHeight()
		{
			return this.GetRenderedValues().y;
		}

		protected float GetRenderedHeight(bool onlyVisibleCharacters)
		{
			return this.GetRenderedValues(onlyVisibleCharacters).y;
		}

		protected virtual Vector2 CalculatePreferredValues(float defaultFontSize, Vector2 marginSize, bool ignoreTextAutoSizing)
		{
			if (this.m_fontAsset == null || this.m_fontAsset.characterDictionary == null)
			{
				Debug.LogWarning("Can't Generate Mesh! No Font Asset has been assigned to Object ID: " + base.GetInstanceID());
				return Vector2.zero;
			}
			if (this.m_char_buffer == null || this.m_char_buffer.Length == 0 || this.m_char_buffer[0] == 0)
			{
				return Vector2.zero;
			}
			this.m_currentFontAsset = this.m_fontAsset;
			this.m_currentMaterial = this.m_sharedMaterial;
			this.m_currentMaterialIndex = 0;
			this.m_materialReferenceStack.SetDefault(new MaterialReference(0, this.m_currentFontAsset, null, this.m_currentMaterial, this.m_padding));
			int totalCharacterCount = this.m_totalCharacterCount;
			if (this.m_internalCharacterInfo == null || totalCharacterCount > this.m_internalCharacterInfo.Length)
			{
				this.m_internalCharacterInfo = new TMP_CharacterInfo[(totalCharacterCount > 1024) ? (totalCharacterCount + 256) : Mathf.NextPowerOfTwo(totalCharacterCount)];
			}
			this.m_fontScale = defaultFontSize / this.m_currentFontAsset.fontInfo.PointSize * (this.m_isOrthographic ? 1f : 0.1f);
			this.m_fontScaleMultiplier = 1f;
			float num = defaultFontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale * (this.m_isOrthographic ? 1f : 0.1f);
			float num2 = this.m_fontScale;
			this.m_currentFontSize = defaultFontSize;
			this.m_sizeStack.SetDefault(this.m_currentFontSize);
			this.m_style = this.m_fontStyle;
			this.m_lineJustification = this.m_textAlignment;
			this.m_lineJustificationStack.SetDefault(this.m_lineJustification);
			this.m_baselineOffset = 0f;
			this.m_lineOffset = 0f;
			this.m_lineHeight = -32767f;
			float num3 = this.m_currentFontAsset.fontInfo.LineHeight - (this.m_currentFontAsset.fontInfo.Ascender - this.m_currentFontAsset.fontInfo.Descender);
			this.m_cSpacing = 0f;
			this.m_monoSpacing = 0f;
			this.m_xAdvance = 0f;
			float a = 0f;
			this.tag_LineIndent = 0f;
			this.tag_Indent = 0f;
			this.m_indentStack.SetDefault(0f);
			this.tag_NoParsing = false;
			this.m_characterCount = 0;
			this.m_firstCharacterOfLine = 0;
			this.m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
			this.m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
			this.m_lineNumber = 0;
			float x = marginSize.x;
			this.m_marginLeft = 0f;
			this.m_marginRight = 0f;
			this.m_width = -1f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			this.m_isCalculatingPreferredValues = true;
			this.m_maxAscender = 0f;
			this.m_maxDescender = 0f;
			bool flag = true;
			bool flag2 = false;
			WordWrapState wordWrapState = default(WordWrapState);
			this.SaveWordWrappingState(ref wordWrapState, 0, 0);
			WordWrapState wordWrapState2 = default(WordWrapState);
			int num7 = 0;
			this.m_recursiveCount++;
			int num8 = 0;
			int num9 = 0;
			while (this.m_char_buffer[num9] != 0)
			{
				int num10 = this.m_char_buffer[num9];
				this.m_textElementType = this.m_textInfo.characterInfo[this.m_characterCount].elementType;
				this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
				this.m_currentFontAsset = this.m_materialReferences[this.m_currentMaterialIndex].fontAsset;
				int currentMaterialIndex = this.m_currentMaterialIndex;
				if (!this.m_isRichText || num10 != 60)
				{
					goto IL_3C7;
				}
				this.m_isParsingText = true;
				this.m_textElementType = TMP_TextElementType.Character;
				if (!this.ValidateHtmlTag(this.m_char_buffer, num9 + 1, out num8))
				{
					goto IL_3C7;
				}
				num9 = num8;
				if (this.m_textElementType != TMP_TextElementType.Character)
				{
					goto IL_3C7;
				}
				IL_1254:
				num9++;
				continue;
				IL_3C7:
				this.m_isParsingText = false;
				bool isUsingAlternateTypeface = this.m_textInfo.characterInfo[this.m_characterCount].isUsingAlternateTypeface;
				float num11 = 1f;
				if (this.m_textElementType == TMP_TextElementType.Character)
				{
					if ((this.m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
					{
						if (char.IsLower((char)num10))
						{
							num10 = (int)char.ToUpper((char)num10);
						}
					}
					else if ((this.m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
					{
						if (char.IsUpper((char)num10))
						{
							num10 = (int)char.ToLower((char)num10);
						}
					}
					else if (((this.m_fontStyle & FontStyles.SmallCaps) == FontStyles.SmallCaps || (this.m_style & FontStyles.SmallCaps) == FontStyles.SmallCaps) && char.IsLower((char)num10))
					{
						num11 = 0.8f;
						num10 = (int)char.ToUpper((char)num10);
					}
				}
				if (this.m_textElementType == TMP_TextElementType.Sprite)
				{
					this.m_currentSpriteAsset = this.m_textInfo.characterInfo[this.m_characterCount].spriteAsset;
					this.m_spriteIndex = this.m_textInfo.characterInfo[this.m_characterCount].spriteIndex;
					TMP_Sprite tmp_Sprite = this.m_currentSpriteAsset.spriteInfoList[this.m_spriteIndex];
					if (tmp_Sprite == null)
					{
						goto IL_1254;
					}
					if (num10 == 60)
					{
						num10 = 57344 + this.m_spriteIndex;
					}
					this.m_currentFontAsset = this.m_fontAsset;
					float num12 = this.m_currentFontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale * (this.m_isOrthographic ? 1f : 0.1f);
					num2 = this.m_fontAsset.fontInfo.Ascender / tmp_Sprite.height * tmp_Sprite.scale * num12;
					this.m_cached_TextElement = tmp_Sprite;
					this.m_internalCharacterInfo[this.m_characterCount].elementType = TMP_TextElementType.Sprite;
					this.m_internalCharacterInfo[this.m_characterCount].scale = num12;
					this.m_currentMaterialIndex = currentMaterialIndex;
				}
				else if (this.m_textElementType == TMP_TextElementType.Character)
				{
					this.m_cached_TextElement = this.m_textInfo.characterInfo[this.m_characterCount].textElement;
					if (this.m_cached_TextElement == null)
					{
						goto IL_1254;
					}
					this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
					this.m_fontScale = this.m_currentFontSize * num11 / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * (this.m_isOrthographic ? 1f : 0.1f);
					num2 = this.m_fontScale * this.m_fontScaleMultiplier * this.m_cached_TextElement.scale;
					this.m_internalCharacterInfo[this.m_characterCount].elementType = TMP_TextElementType.Character;
				}
				float num13 = num2;
				if (num10 == 173)
				{
					num2 = 0f;
				}
				this.m_internalCharacterInfo[this.m_characterCount].character = (char)num10;
				if (this.m_enableKerning && this.m_characterCount >= 1)
				{
					int character = (int)this.m_internalCharacterInfo[this.m_characterCount - 1].character;
					KerningPairKey kerningPairKey = new KerningPairKey(character, num10);
					KerningPair kerningPair;
					this.m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, out kerningPair);
					if (kerningPair != null)
					{
						this.m_xAdvance += kerningPair.XadvanceOffset * num2;
					}
				}
				float num14 = 0f;
				if (this.m_monoSpacing != 0f)
				{
					num14 = this.m_monoSpacing / 2f - (this.m_cached_TextElement.width / 2f + this.m_cached_TextElement.xOffset) * num2;
					this.m_xAdvance += num14;
				}
				float num15;
				if (this.m_textElementType == TMP_TextElementType.Character && !isUsingAlternateTypeface && ((this.m_style & FontStyles.Bold) == FontStyles.Bold || (this.m_fontStyle & FontStyles.Bold) == FontStyles.Bold))
				{
					num15 = 1f + this.m_currentFontAsset.boldSpacing * 0.01f;
				}
				else
				{
					num15 = 1f;
				}
				this.m_internalCharacterInfo[this.m_characterCount].baseLine = 0f - this.m_lineOffset + this.m_baselineOffset;
				float num16 = this.m_currentFontAsset.fontInfo.Ascender * ((this.m_textElementType == TMP_TextElementType.Character) ? num2 : this.m_internalCharacterInfo[this.m_characterCount].scale) + this.m_baselineOffset;
				this.m_internalCharacterInfo[this.m_characterCount].ascender = num16 - this.m_lineOffset;
				this.m_maxLineAscender = ((num16 > this.m_maxLineAscender) ? num16 : this.m_maxLineAscender);
				float num17 = this.m_currentFontAsset.fontInfo.Descender * ((this.m_textElementType == TMP_TextElementType.Character) ? num2 : this.m_internalCharacterInfo[this.m_characterCount].scale) + this.m_baselineOffset;
				float num18 = this.m_internalCharacterInfo[this.m_characterCount].descender = num17 - this.m_lineOffset;
				this.m_maxLineDescender = ((num17 < this.m_maxLineDescender) ? num17 : this.m_maxLineDescender);
				if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript || (this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
				{
					float num19 = (num16 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
					num16 = this.m_maxLineAscender;
					this.m_maxLineAscender = ((num19 > this.m_maxLineAscender) ? num19 : this.m_maxLineAscender);
					float num20 = (num17 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
					num17 = this.m_maxLineDescender;
					this.m_maxLineDescender = ((num20 < this.m_maxLineDescender) ? num20 : this.m_maxLineDescender);
				}
				if (this.m_lineNumber == 0)
				{
					this.m_maxAscender = ((this.m_maxAscender > num16) ? this.m_maxAscender : num16);
				}
				if (num10 == 9 || (!char.IsWhiteSpace((char)num10) && num10 != 8203) || this.m_textElementType == TMP_TextElementType.Sprite)
				{
					float num21 = (this.m_width != -1f) ? Mathf.Min(x + 0.0001f - this.m_marginLeft - this.m_marginRight, this.m_width) : (x + 0.0001f - this.m_marginLeft - this.m_marginRight);
					bool flag3 = (this.m_lineJustification & (TextAlignmentOptions)16) == (TextAlignmentOptions)16 || (this.m_lineJustification & (TextAlignmentOptions)8) == (TextAlignmentOptions)8;
					num6 = this.m_xAdvance + this.m_cached_TextElement.xAdvance * ((num10 != 173) ? num2 : num13);
					if (num6 > num21 * (flag3 ? 1.05f : 1f))
					{
						if (this.enableWordWrapping && this.m_characterCount != this.m_firstCharacterOfLine)
						{
							if (num7 == wordWrapState2.previous_WordBreak || flag)
							{
								if (!this.m_isCharacterWrappingEnabled)
								{
									this.m_isCharacterWrappingEnabled = true;
								}
								else
								{
									flag2 = true;
								}
							}
							num9 = this.RestoreWordWrappingState(ref wordWrapState2);
							num7 = num9;
							if (this.m_char_buffer[num9] == 173)
							{
								this.m_isTextTruncated = true;
								this.m_char_buffer[num9] = 45;
								this.CalculatePreferredValues(defaultFontSize, marginSize, true);
								return Vector2.zero;
							}
							if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == -32767f)
							{
								float num22 = this.m_maxLineAscender - this.m_startOfLineAscender;
								this.m_lineOffset += num22;
								wordWrapState2.lineOffset = this.m_lineOffset;
								wordWrapState2.previousLineAscender = this.m_maxLineAscender;
							}
							float num23 = this.m_maxLineAscender - this.m_lineOffset;
							float num24 = this.m_maxLineDescender - this.m_lineOffset;
							this.m_maxDescender = ((this.m_maxDescender < num24) ? this.m_maxDescender : num24);
							this.m_firstCharacterOfLine = this.m_characterCount;
							num4 += this.m_xAdvance;
							if (this.m_enableWordWrapping)
							{
								num5 = this.m_maxAscender - this.m_maxDescender;
							}
							else
							{
								num5 = Mathf.Max(num5, num23 - num24);
							}
							this.SaveWordWrappingState(ref wordWrapState, num9, this.m_characterCount - 1);
							this.m_lineNumber++;
							if (this.m_lineHeight == -32767f)
							{
								float num25 = this.m_internalCharacterInfo[this.m_characterCount].ascender - this.m_internalCharacterInfo[this.m_characterCount].baseLine;
								float num26 = 0f - this.m_maxLineDescender + num25 + (num3 + this.m_lineSpacing + this.m_lineSpacingDelta) * num;
								this.m_lineOffset += num26;
								this.m_startOfLineAscender = num25;
							}
							else
							{
								this.m_lineOffset += this.m_lineHeight + this.m_lineSpacing * num;
							}
							this.m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
							this.m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
							this.m_xAdvance = 0f + this.tag_Indent;
							goto IL_1254;
						}
						else if (!ignoreTextAutoSizing && this.m_currentFontSize > this.m_fontSizeMin)
						{
							float charWidthAdjDelta = this.m_charWidthAdjDelta;
							float num27 = this.m_charWidthMaxAdj / 100f;
							this.m_maxFontSize = this.m_currentFontSize;
							this.m_currentFontSize -= Mathf.Max((this.m_currentFontSize - this.m_minFontSize) / 2f, 0.05f);
							this.m_currentFontSize = (float)((int)(Mathf.Max(this.m_currentFontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
							if (this.m_recursiveCount > 20)
							{
								return new Vector2(num4, num5);
							}
							return this.CalculatePreferredValues(this.m_currentFontSize, marginSize, false);
						}
					}
				}
				if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == -32767f && !this.m_isNewPage)
				{
					float num28 = this.m_maxLineAscender - this.m_startOfLineAscender;
					num18 -= num28;
					this.m_lineOffset += num28;
					this.m_startOfLineAscender += num28;
					wordWrapState2.lineOffset = this.m_lineOffset;
					wordWrapState2.previousLineAscender = this.m_startOfLineAscender;
				}
				if (num10 == 9)
				{
					float num29 = this.m_currentFontAsset.fontInfo.TabWidth * num2;
					float num30 = Mathf.Ceil(this.m_xAdvance / num29) * num29;
					this.m_xAdvance = ((num30 > this.m_xAdvance) ? num30 : (this.m_xAdvance + num29));
				}
				else if (this.m_monoSpacing != 0f)
				{
					this.m_xAdvance += this.m_monoSpacing - num14 + (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing;
					if (char.IsWhiteSpace((char)num10) || num10 == 8203)
					{
						this.m_xAdvance += this.m_wordSpacing * num2;
					}
				}
				else
				{
					this.m_xAdvance += (this.m_cached_TextElement.xAdvance * num15 + this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing;
					if (char.IsWhiteSpace((char)num10) || num10 == 8203)
					{
						this.m_xAdvance += this.m_wordSpacing * num2;
					}
				}
				if (num10 == 13)
				{
					a = Mathf.Max(a, num4 + this.m_xAdvance);
					num4 = 0f;
					this.m_xAdvance = 0f + this.tag_Indent;
				}
				if (num10 == 10 || this.m_characterCount == totalCharacterCount - 1)
				{
					if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == -32767f)
					{
						float num31 = this.m_maxLineAscender - this.m_startOfLineAscender;
						num18 -= num31;
						this.m_lineOffset += num31;
					}
					float num32 = this.m_maxLineDescender - this.m_lineOffset;
					this.m_maxDescender = ((this.m_maxDescender < num32) ? this.m_maxDescender : num32);
					this.m_firstCharacterOfLine = this.m_characterCount + 1;
					if (num10 == 10 && this.m_characterCount != totalCharacterCount - 1)
					{
						a = Mathf.Max(a, num4 + num6);
						num4 = 0f;
					}
					else
					{
						num4 = Mathf.Max(a, num4 + num6);
					}
					num5 = this.m_maxAscender - this.m_maxDescender;
					if (num10 == 10)
					{
						this.SaveWordWrappingState(ref wordWrapState, num9, this.m_characterCount);
						this.SaveWordWrappingState(ref wordWrapState2, num9, this.m_characterCount);
						this.m_lineNumber++;
						if (this.m_lineHeight == -32767f)
						{
							float num26 = 0f - this.m_maxLineDescender + num16 + (num3 + this.m_lineSpacing + this.m_paragraphSpacing + this.m_lineSpacingDelta) * num;
							this.m_lineOffset += num26;
						}
						else
						{
							this.m_lineOffset += this.m_lineHeight + (this.m_lineSpacing + this.m_paragraphSpacing) * num;
						}
						this.m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
						this.m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
						this.m_startOfLineAscender = num16;
						this.m_xAdvance = 0f + this.tag_LineIndent + this.tag_Indent;
					}
				}
				if (this.m_enableWordWrapping || this.m_overflowMode == TextOverflowModes.Truncate || this.m_overflowMode == TextOverflowModes.Ellipsis)
				{
					if ((char.IsWhiteSpace((char)num10) || num10 == 8203 || num10 == 45 || num10 == 173) && !this.m_isNonBreakingSpace && num10 != 160 && num10 != 8209 && num10 != 8239 && num10 != 8288)
					{
						this.SaveWordWrappingState(ref wordWrapState2, num9, this.m_characterCount);
						this.m_isCharacterWrappingEnabled = false;
						flag = false;
					}
					else if (((num10 > 4352 && num10 < 4607) || (num10 > 11904 && num10 < 40959) || (num10 > 43360 && num10 < 43391) || (num10 > 44032 && num10 < 55295) || (num10 > 63744 && num10 < 64255) || (num10 > 65072 && num10 < 65103) || (num10 > 65280 && num10 < 65519)) && !this.m_isNonBreakingSpace)
					{
						if (flag || flag2 || (!TMP_Settings.linebreakingRules.leadingCharacters.ContainsKey(num10) && this.m_characterCount < totalCharacterCount - 1 && !TMP_Settings.linebreakingRules.followingCharacters.ContainsKey((int)this.m_internalCharacterInfo[this.m_characterCount + 1].character)))
						{
							this.SaveWordWrappingState(ref wordWrapState2, num9, this.m_characterCount);
							this.m_isCharacterWrappingEnabled = false;
							flag = false;
						}
					}
					else if (flag || this.m_isCharacterWrappingEnabled || flag2)
					{
						this.SaveWordWrappingState(ref wordWrapState2, num9, this.m_characterCount);
					}
				}
				this.m_characterCount++;
				goto IL_1254;
			}
			float num33 = this.m_maxFontSize - this.m_minFontSize;
			if (this.m_isCharacterWrappingEnabled || ignoreTextAutoSizing || num33 <= 0.051f || this.m_currentFontSize >= this.m_fontSizeMax)
			{
				this.m_isCharacterWrappingEnabled = false;
				this.m_isCalculatingPreferredValues = false;
				num4 += ((this.m_margin.x > 0f) ? this.m_margin.x : 0f);
				num4 += ((this.m_margin.z > 0f) ? this.m_margin.z : 0f);
				num5 += ((this.m_margin.y > 0f) ? this.m_margin.y : 0f);
				num5 += ((this.m_margin.w > 0f) ? this.m_margin.w : 0f);
				num4 = (float)((int)(num4 * 100f + 1f)) / 100f;
				num5 = (float)((int)(num5 * 100f + 1f)) / 100f;
				return new Vector2(num4, num5);
			}
			this.m_minFontSize = this.m_currentFontSize;
			this.m_currentFontSize += Mathf.Max((this.m_maxFontSize - this.m_currentFontSize) / 2f, 0.05f);
			this.m_currentFontSize = (float)((int)(Mathf.Min(this.m_currentFontSize, this.m_fontSizeMax) * 20f + 0.5f)) / 20f;
			if (this.m_recursiveCount > 20)
			{
				return new Vector2(num4, num5);
			}
			return this.CalculatePreferredValues(this.m_currentFontSize, marginSize, false);
		}

		protected virtual Bounds GetCompoundBounds()
		{
			return default(Bounds);
		}

		protected Bounds GetTextBounds()
		{
			if (this.m_textInfo == null || this.m_textInfo.characterCount > this.m_textInfo.characterInfo.Length)
			{
				return default(Bounds);
			}
			Extents extents = new Extents(TMP_Text.k_LargePositiveVector2, TMP_Text.k_LargeNegativeVector2);
			int num = 0;
			while (num < this.m_textInfo.characterCount && num < this.m_textInfo.characterInfo.Length)
			{
				if (this.m_textInfo.characterInfo[num].isVisible)
				{
					extents.min.x = Mathf.Min(extents.min.x, this.m_textInfo.characterInfo[num].bottomLeft.x);
					extents.min.y = Mathf.Min(extents.min.y, this.m_textInfo.characterInfo[num].descender);
					extents.max.x = Mathf.Max(extents.max.x, this.m_textInfo.characterInfo[num].xAdvance);
					extents.max.y = Mathf.Max(extents.max.y, this.m_textInfo.characterInfo[num].ascender);
				}
				num++;
			}
			Vector2 v;
			v.x = extents.max.x - extents.min.x;
			v.y = extents.max.y - extents.min.y;
			return new Bounds((extents.min + extents.max) / 2f, v);
		}

		protected Bounds GetTextBounds(bool onlyVisibleCharacters)
		{
			if (this.m_textInfo == null)
			{
				return default(Bounds);
			}
			Extents extents = new Extents(TMP_Text.k_LargePositiveVector2, TMP_Text.k_LargeNegativeVector2);
			int num = 0;
			while (num < this.m_textInfo.characterCount && num < this.m_maxVisibleCharacters)
			{
				if (this.m_textInfo.characterInfo[num].isVisible)
				{
					if ((int)this.m_textInfo.characterInfo[num].lineNumber > this.m_maxVisibleLines)
					{
						break;
					}
					extents.min.x = Mathf.Min(extents.min.x, this.m_textInfo.characterInfo[num].origin);
					extents.min.y = Mathf.Min(extents.min.y, this.m_textInfo.characterInfo[num].descender);
					extents.max.x = Mathf.Max(extents.max.x, this.m_textInfo.characterInfo[num].xAdvance);
					extents.max.y = Mathf.Max(extents.max.y, this.m_textInfo.characterInfo[num].ascender);
				}
				num++;
			}
			Vector2 v;
			v.x = extents.max.x - extents.min.x;
			v.y = extents.max.y - extents.min.y;
			return new Bounds((extents.min + extents.max) / 2f, v);
		}

		protected virtual void AdjustLineOffset(int startIndex, int endIndex, float offset)
		{
		}

		protected void ResizeLineExtents(int size)
		{
			size = ((size > 1024) ? (size + 256) : Mathf.NextPowerOfTwo(size + 1));
			TMP_LineInfo[] array = new TMP_LineInfo[size];
			for (int i = 0; i < size; i++)
			{
				if (i < this.m_textInfo.lineInfo.Length)
				{
					array[i] = this.m_textInfo.lineInfo[i];
				}
				else
				{
					array[i].lineExtents.min = TMP_Text.k_LargePositiveVector2;
					array[i].lineExtents.max = TMP_Text.k_LargeNegativeVector2;
					array[i].ascender = TMP_Text.k_LargeNegativeFloat;
					array[i].descender = TMP_Text.k_LargePositiveFloat;
				}
			}
			this.m_textInfo.lineInfo = array;
		}

		public virtual TMP_TextInfo GetTextInfo(string text)
		{
			return null;
		}

		protected virtual void ComputeMarginSize()
		{
		}

		protected void SaveWordWrappingState(ref WordWrapState state, int index, int count)
		{
			state.currentFontAsset = this.m_currentFontAsset;
			state.currentSpriteAsset = this.m_currentSpriteAsset;
			state.currentMaterial = this.m_currentMaterial;
			state.currentMaterialIndex = this.m_currentMaterialIndex;
			state.previous_WordBreak = index;
			state.total_CharacterCount = count;
			state.visible_CharacterCount = this.m_lineVisibleCharacterCount;
			state.visible_LinkCount = this.m_textInfo.linkCount;
			state.firstCharacterIndex = this.m_firstCharacterOfLine;
			state.firstVisibleCharacterIndex = this.m_firstVisibleCharacterOfLine;
			state.lastVisibleCharIndex = this.m_lastVisibleCharacterOfLine;
			state.fontStyle = this.m_style;
			state.fontScale = this.m_fontScale;
			state.fontScaleMultiplier = this.m_fontScaleMultiplier;
			state.currentFontSize = this.m_currentFontSize;
			state.xAdvance = this.m_xAdvance;
			state.maxCapHeight = this.m_maxCapHeight;
			state.maxAscender = this.m_maxAscender;
			state.maxDescender = this.m_maxDescender;
			state.maxLineAscender = this.m_maxLineAscender;
			state.maxLineDescender = this.m_maxLineDescender;
			state.previousLineAscender = this.m_startOfLineAscender;
			state.preferredWidth = this.m_preferredWidth;
			state.preferredHeight = this.m_preferredHeight;
			state.meshExtents = this.m_meshExtents;
			state.lineNumber = this.m_lineNumber;
			state.lineOffset = this.m_lineOffset;
			state.baselineOffset = this.m_baselineOffset;
			state.vertexColor = this.m_htmlColor;
			state.underlineColor = this.m_underlineColor;
			state.highlightColor = this.m_highlightColor;
			state.tagNoParsing = this.tag_NoParsing;
			state.basicStyleStack = this.m_fontStyleStack;
			state.colorStack = this.m_colorStack;
			state.underlineColorStack = this.m_underlineColorStack;
			state.highlightColorStack = this.m_highlightColorStack;
			state.sizeStack = this.m_sizeStack;
			state.indentStack = this.m_indentStack;
			state.fontWeightStack = this.m_fontWeightStack;
			state.styleStack = this.m_styleStack;
			state.actionStack = this.m_actionStack;
			state.materialReferenceStack = this.m_materialReferenceStack;
			state.lineJustificationStack = this.m_lineJustificationStack;
			state.spriteAnimationID = this.m_spriteAnimationID;
			if (this.m_lineNumber < this.m_textInfo.lineInfo.Length)
			{
				state.lineInfo = this.m_textInfo.lineInfo[this.m_lineNumber];
			}
		}

		protected int RestoreWordWrappingState(ref WordWrapState state)
		{
			int previous_WordBreak = state.previous_WordBreak;
			this.m_currentFontAsset = state.currentFontAsset;
			this.m_currentSpriteAsset = state.currentSpriteAsset;
			this.m_currentMaterial = state.currentMaterial;
			this.m_currentMaterialIndex = state.currentMaterialIndex;
			this.m_characterCount = state.total_CharacterCount + 1;
			this.m_lineVisibleCharacterCount = state.visible_CharacterCount;
			this.m_textInfo.linkCount = state.visible_LinkCount;
			this.m_firstCharacterOfLine = state.firstCharacterIndex;
			this.m_firstVisibleCharacterOfLine = state.firstVisibleCharacterIndex;
			this.m_lastVisibleCharacterOfLine = state.lastVisibleCharIndex;
			this.m_style = state.fontStyle;
			this.m_fontScale = state.fontScale;
			this.m_fontScaleMultiplier = state.fontScaleMultiplier;
			this.m_currentFontSize = state.currentFontSize;
			this.m_xAdvance = state.xAdvance;
			this.m_maxCapHeight = state.maxCapHeight;
			this.m_maxAscender = state.maxAscender;
			this.m_maxDescender = state.maxDescender;
			this.m_maxLineAscender = state.maxLineAscender;
			this.m_maxLineDescender = state.maxLineDescender;
			this.m_startOfLineAscender = state.previousLineAscender;
			this.m_preferredWidth = state.preferredWidth;
			this.m_preferredHeight = state.preferredHeight;
			this.m_meshExtents = state.meshExtents;
			this.m_lineNumber = state.lineNumber;
			this.m_lineOffset = state.lineOffset;
			this.m_baselineOffset = state.baselineOffset;
			this.m_htmlColor = state.vertexColor;
			this.m_underlineColor = state.underlineColor;
			this.m_highlightColor = state.highlightColor;
			this.tag_NoParsing = state.tagNoParsing;
			this.m_fontStyleStack = state.basicStyleStack;
			this.m_colorStack = state.colorStack;
			this.m_underlineColorStack = state.underlineColorStack;
			this.m_highlightColorStack = state.highlightColorStack;
			this.m_sizeStack = state.sizeStack;
			this.m_indentStack = state.indentStack;
			this.m_fontWeightStack = state.fontWeightStack;
			this.m_styleStack = state.styleStack;
			this.m_actionStack = state.actionStack;
			this.m_materialReferenceStack = state.materialReferenceStack;
			this.m_lineJustificationStack = state.lineJustificationStack;
			this.m_spriteAnimationID = state.spriteAnimationID;
			if (this.m_lineNumber < this.m_textInfo.lineInfo.Length)
			{
				this.m_textInfo.lineInfo[this.m_lineNumber] = state.lineInfo;
			}
			return previous_WordBreak;
		}

		protected virtual void SaveGlyphVertexInfo(float padding, float style_padding, Color32 vertexColor)
		{
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.position = this.m_textInfo.characterInfo[this.m_characterCount].topLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.position = this.m_textInfo.characterInfo[this.m_characterCount].topRight;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomRight;
			vertexColor.a = ((this.m_fontColor32.a < vertexColor.a) ? this.m_fontColor32.a : vertexColor.a);
			if (!this.m_enableVertexGradient)
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = vertexColor;
			}
			else if (!this.m_overrideHtmlColors && this.m_colorStack.index > 1)
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = vertexColor;
			}
			else if (this.m_fontColorGradientPreset != null)
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = this.m_fontColorGradientPreset.bottomLeft * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = this.m_fontColorGradientPreset.topLeft * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = this.m_fontColorGradientPreset.topRight * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = this.m_fontColorGradientPreset.bottomRight * vertexColor;
			}
			else
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = this.m_fontColorGradient.bottomLeft * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = this.m_fontColorGradient.topLeft * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = this.m_fontColorGradient.topRight * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = this.m_fontColorGradient.bottomRight * vertexColor;
			}
			if (!this.m_isSDFShader)
			{
				style_padding = 0f;
			}
			FaceInfo fontInfo = this.m_currentFontAsset.fontInfo;
			Vector2 vector;
			vector.x = (this.m_cached_TextElement.x - padding - style_padding) / fontInfo.AtlasWidth;
			vector.y = 1f - (this.m_cached_TextElement.y + padding + style_padding + this.m_cached_TextElement.height) / fontInfo.AtlasHeight;
			Vector2 vector2;
			vector2.x = vector.x;
			vector2.y = 1f - (this.m_cached_TextElement.y - padding - style_padding) / fontInfo.AtlasHeight;
			Vector2 vector3;
			vector3.x = (this.m_cached_TextElement.x + padding + style_padding + this.m_cached_TextElement.width) / fontInfo.AtlasWidth;
			vector3.y = vector2.y;
			Vector2 uv;
			uv.x = vector3.x;
			uv.y = vector.y;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.uv = vector;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.uv = vector2;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.uv = vector3;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.uv = uv;
		}

		protected virtual void SaveSpriteVertexInfo(Color32 vertexColor)
		{
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.position = this.m_textInfo.characterInfo[this.m_characterCount].topLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.position = this.m_textInfo.characterInfo[this.m_characterCount].topRight;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomRight;
			if (this.m_tintAllSprites)
			{
				this.m_tintSprite = true;
			}
			Color32 color = this.m_tintSprite ? this.m_spriteColor.Multiply(vertexColor) : this.m_spriteColor;
			color.a = ((color.a < this.m_fontColor32.a) ? (color.a = ((color.a < vertexColor.a) ? color.a : vertexColor.a)) : this.m_fontColor32.a);
			if (!this.m_enableVertexGradient)
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = color;
			}
			else if (!this.m_overrideHtmlColors && this.m_colorStack.index > 1)
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = color;
			}
			else if (this.m_fontColorGradientPreset != null)
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = (this.m_tintSprite ? color.Multiply(this.m_fontColorGradientPreset.bottomLeft) : color);
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = (this.m_tintSprite ? color.Multiply(this.m_fontColorGradientPreset.topLeft) : color);
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = (this.m_tintSprite ? color.Multiply(this.m_fontColorGradientPreset.topRight) : color);
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = (this.m_tintSprite ? color.Multiply(this.m_fontColorGradientPreset.bottomRight) : color);
			}
			else
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = (this.m_tintSprite ? color.Multiply(this.m_fontColorGradient.bottomLeft) : color);
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = (this.m_tintSprite ? color.Multiply(this.m_fontColorGradient.topLeft) : color);
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = (this.m_tintSprite ? color.Multiply(this.m_fontColorGradient.topRight) : color);
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = (this.m_tintSprite ? color.Multiply(this.m_fontColorGradient.bottomRight) : color);
			}
			Vector2 vector = new Vector2(this.m_cached_TextElement.x / (float)this.m_currentSpriteAsset.spriteSheet.width, this.m_cached_TextElement.y / (float)this.m_currentSpriteAsset.spriteSheet.height);
			Vector2 vector2 = new Vector2(vector.x, (this.m_cached_TextElement.y + this.m_cached_TextElement.height) / (float)this.m_currentSpriteAsset.spriteSheet.height);
			Vector2 vector3 = new Vector2((this.m_cached_TextElement.x + this.m_cached_TextElement.width) / (float)this.m_currentSpriteAsset.spriteSheet.width, vector2.y);
			Vector2 uv = new Vector2(vector3.x, vector.y);
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.uv = vector;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.uv = vector2;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.uv = vector3;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.uv = uv;
		}

		protected virtual void FillCharacterVertexBuffers(int i, int index_X4)
		{
			int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
			this.m_textInfo.characterInfo[i].vertexIndex = index_X4;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + 4;
		}

		protected virtual void FillCharacterVertexBuffers(int i, int index_X4, bool isVolumetric)
		{
			int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
			this.m_textInfo.characterInfo[i].vertexIndex = index_X4;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			if (isVolumetric)
			{
				Vector3 b = new Vector3(0f, 0f, this.m_fontSize * this.m_fontScale);
				this.m_textInfo.meshInfo[materialReferenceIndex].vertices[4 + index_X4] = characterInfo[i].vertex_BL.position + b;
				this.m_textInfo.meshInfo[materialReferenceIndex].vertices[5 + index_X4] = characterInfo[i].vertex_TL.position + b;
				this.m_textInfo.meshInfo[materialReferenceIndex].vertices[6 + index_X4] = characterInfo[i].vertex_TR.position + b;
				this.m_textInfo.meshInfo[materialReferenceIndex].vertices[7 + index_X4] = characterInfo[i].vertex_BR.position + b;
			}
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			if (isVolumetric)
			{
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[4 + index_X4] = characterInfo[i].vertex_BL.uv;
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[5 + index_X4] = characterInfo[i].vertex_TL.uv;
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[6 + index_X4] = characterInfo[i].vertex_TR.uv;
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[7 + index_X4] = characterInfo[i].vertex_BR.uv;
			}
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			if (isVolumetric)
			{
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[4 + index_X4] = characterInfo[i].vertex_BL.uv2;
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[5 + index_X4] = characterInfo[i].vertex_TL.uv2;
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[6 + index_X4] = characterInfo[i].vertex_TR.uv2;
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[7 + index_X4] = characterInfo[i].vertex_BR.uv2;
			}
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			if (isVolumetric)
			{
				Color32 color = new Color32(byte.MaxValue, byte.MaxValue, 128, byte.MaxValue);
				this.m_textInfo.meshInfo[materialReferenceIndex].colors32[4 + index_X4] = color;
				this.m_textInfo.meshInfo[materialReferenceIndex].colors32[5 + index_X4] = color;
				this.m_textInfo.meshInfo[materialReferenceIndex].colors32[6 + index_X4] = color;
				this.m_textInfo.meshInfo[materialReferenceIndex].colors32[7 + index_X4] = color;
			}
			this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + ((!isVolumetric) ? 4 : 8);
		}

		protected virtual void FillSpriteVertexBuffers(int i, int index_X4)
		{
			int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
			this.m_textInfo.characterInfo[i].vertexIndex = index_X4;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + 4;
		}

		protected virtual void DrawUnderlineMesh(Vector3 start, Vector3 end, ref int index, float startScale, float endScale, float maxScale, float sdfScale, Color32 underlineColor)
		{
			if (this.m_cached_Underline_GlyphInfo == null)
			{
				if (!TMP_Settings.warningsDisabled)
				{
					Debug.LogWarning("Unable to add underline since the Font Asset doesn't contain the underline character.", this);
				}
				return;
			}
			int num = index + 12;
			if (num > this.m_textInfo.meshInfo[0].vertices.Length)
			{
				this.m_textInfo.meshInfo[0].ResizeMeshInfo(num / 4);
			}
			start.y = Mathf.Min(start.y, end.y);
			end.y = Mathf.Min(start.y, end.y);
			float num2 = this.m_cached_Underline_GlyphInfo.width / 2f * maxScale;
			if (end.x - start.x < this.m_cached_Underline_GlyphInfo.width * maxScale)
			{
				num2 = (end.x - start.x) / 2f;
			}
			float num3 = this.m_padding * startScale / maxScale;
			float num4 = this.m_padding * endScale / maxScale;
			float height = this.m_cached_Underline_GlyphInfo.height;
			Vector3[] vertices = this.m_textInfo.meshInfo[0].vertices;
			vertices[index] = start + new Vector3(0f, 0f - (height + this.m_padding) * maxScale, 0f);
			vertices[index + 1] = start + new Vector3(0f, this.m_padding * maxScale, 0f);
			vertices[index + 2] = vertices[index + 1] + new Vector3(num2, 0f, 0f);
			vertices[index + 3] = vertices[index] + new Vector3(num2, 0f, 0f);
			vertices[index + 4] = vertices[index + 3];
			vertices[index + 5] = vertices[index + 2];
			vertices[index + 6] = end + new Vector3(-num2, this.m_padding * maxScale, 0f);
			vertices[index + 7] = end + new Vector3(-num2, -(height + this.m_padding) * maxScale, 0f);
			vertices[index + 8] = vertices[index + 7];
			vertices[index + 9] = vertices[index + 6];
			vertices[index + 10] = end + new Vector3(0f, this.m_padding * maxScale, 0f);
			vertices[index + 11] = end + new Vector3(0f, -(height + this.m_padding) * maxScale, 0f);
			Vector2[] uvs = this.m_textInfo.meshInfo[0].uvs0;
			Vector2 vector = new Vector2((this.m_cached_Underline_GlyphInfo.x - num3) / this.m_fontAsset.fontInfo.AtlasWidth, 1f - (this.m_cached_Underline_GlyphInfo.y + this.m_padding + this.m_cached_Underline_GlyphInfo.height) / this.m_fontAsset.fontInfo.AtlasHeight);
			Vector2 vector2 = new Vector2(vector.x, 1f - (this.m_cached_Underline_GlyphInfo.y - this.m_padding) / this.m_fontAsset.fontInfo.AtlasHeight);
			Vector2 vector3 = new Vector2((this.m_cached_Underline_GlyphInfo.x - num3 + this.m_cached_Underline_GlyphInfo.width / 2f) / this.m_fontAsset.fontInfo.AtlasWidth, vector2.y);
			Vector2 vector4 = new Vector2(vector3.x, vector.y);
			Vector2 vector5 = new Vector2((this.m_cached_Underline_GlyphInfo.x + num4 + this.m_cached_Underline_GlyphInfo.width / 2f) / this.m_fontAsset.fontInfo.AtlasWidth, vector2.y);
			Vector2 vector6 = new Vector2(vector5.x, vector.y);
			Vector2 vector7 = new Vector2((this.m_cached_Underline_GlyphInfo.x + num4 + this.m_cached_Underline_GlyphInfo.width) / this.m_fontAsset.fontInfo.AtlasWidth, vector2.y);
			Vector2 vector8 = new Vector2(vector7.x, vector.y);
			uvs[index] = vector;
			uvs[1 + index] = vector2;
			uvs[2 + index] = vector3;
			uvs[3 + index] = vector4;
			uvs[4 + index] = new Vector2(vector3.x - vector3.x * 0.001f, vector.y);
			uvs[5 + index] = new Vector2(vector3.x - vector3.x * 0.001f, vector2.y);
			uvs[6 + index] = new Vector2(vector3.x + vector3.x * 0.001f, vector2.y);
			uvs[7 + index] = new Vector2(vector3.x + vector3.x * 0.001f, vector.y);
			uvs[8 + index] = vector6;
			uvs[9 + index] = vector5;
			uvs[10 + index] = vector7;
			uvs[11 + index] = vector8;
			float x = (vertices[index + 2].x - start.x) / (end.x - start.x);
			float scale = Mathf.Abs(sdfScale);
			Vector2[] uvs2 = this.m_textInfo.meshInfo[0].uvs2;
			uvs2[index] = this.PackUV(0f, 0f, scale);
			uvs2[1 + index] = this.PackUV(0f, 1f, scale);
			uvs2[2 + index] = this.PackUV(x, 1f, scale);
			uvs2[3 + index] = this.PackUV(x, 0f, scale);
			float x2 = (vertices[index + 4].x - start.x) / (end.x - start.x);
			x = (vertices[index + 6].x - start.x) / (end.x - start.x);
			uvs2[4 + index] = this.PackUV(x2, 0f, scale);
			uvs2[5 + index] = this.PackUV(x2, 1f, scale);
			uvs2[6 + index] = this.PackUV(x, 1f, scale);
			uvs2[7 + index] = this.PackUV(x, 0f, scale);
			x2 = (vertices[index + 8].x - start.x) / (end.x - start.x);
			x = (vertices[index + 6].x - start.x) / (end.x - start.x);
			uvs2[8 + index] = this.PackUV(x2, 0f, scale);
			uvs2[9 + index] = this.PackUV(x2, 1f, scale);
			uvs2[10 + index] = this.PackUV(1f, 1f, scale);
			uvs2[11 + index] = this.PackUV(1f, 0f, scale);
			Color32[] colors = this.m_textInfo.meshInfo[0].colors32;
			colors[index] = underlineColor;
			colors[1 + index] = underlineColor;
			colors[2 + index] = underlineColor;
			colors[3 + index] = underlineColor;
			colors[4 + index] = underlineColor;
			colors[5 + index] = underlineColor;
			colors[6 + index] = underlineColor;
			colors[7 + index] = underlineColor;
			colors[8 + index] = underlineColor;
			colors[9 + index] = underlineColor;
			colors[10 + index] = underlineColor;
			colors[11 + index] = underlineColor;
			index += 12;
		}

		protected virtual void DrawTextHighlight(Vector3 start, Vector3 end, ref int index, Color32 highlightColor)
		{
			if (this.m_cached_Underline_GlyphInfo == null)
			{
				if (!TMP_Settings.warningsDisabled)
				{
					Debug.LogWarning("Unable to add underline since the Font Asset doesn't contain the underline character.", this);
				}
				return;
			}
			int num = index + 4;
			if (num > this.m_textInfo.meshInfo[0].vertices.Length)
			{
				this.m_textInfo.meshInfo[0].ResizeMeshInfo(num / 4);
			}
			Vector3[] vertices = this.m_textInfo.meshInfo[0].vertices;
			vertices[index] = start;
			vertices[index + 1] = new Vector3(start.x, end.y, 0f);
			vertices[index + 2] = end;
			vertices[index + 3] = new Vector3(end.x, start.y, 0f);
			Vector2[] uvs = this.m_textInfo.meshInfo[0].uvs0;
			Vector2 vector = new Vector2((this.m_cached_Underline_GlyphInfo.x + this.m_cached_Underline_GlyphInfo.width / 2f) / this.m_fontAsset.fontInfo.AtlasWidth, 1f - (this.m_cached_Underline_GlyphInfo.y + this.m_cached_Underline_GlyphInfo.height / 2f) / this.m_fontAsset.fontInfo.AtlasHeight);
			uvs[index] = vector;
			uvs[1 + index] = vector;
			uvs[2 + index] = vector;
			uvs[3 + index] = vector;
			Vector2[] uvs2 = this.m_textInfo.meshInfo[0].uvs2;
			Vector2 vector2 = new Vector2(0f, 1f);
			uvs2[index] = vector2;
			uvs2[1 + index] = vector2;
			uvs2[2 + index] = vector2;
			uvs2[3 + index] = vector2;
			Color32[] colors = this.m_textInfo.meshInfo[0].colors32;
			highlightColor.a = ((this.m_htmlColor.a < highlightColor.a) ? this.m_htmlColor.a : highlightColor.a);
			colors[index] = highlightColor;
			colors[1 + index] = highlightColor;
			colors[2 + index] = highlightColor;
			colors[3 + index] = highlightColor;
			index += 4;
		}

		protected void LoadDefaultSettings()
		{
			if (this.m_text == null)
			{
				if (TMP_Settings.autoSizeTextContainer)
				{
					this.autoSizeTextContainer = true;
				}
				else if (base.GetType() == typeof(TextMeshPro))
				{
					this.m_rectTransform.sizeDelta = TMP_Settings.defaultTextMeshProTextContainerSize;
				}
				else
				{
					this.m_rectTransform.sizeDelta = TMP_Settings.defaultTextMeshProUITextContainerSize;
				}
				this.m_enableWordWrapping = TMP_Settings.enableWordWrapping;
				this.m_enableKerning = TMP_Settings.enableKerning;
				this.m_enableExtraPadding = TMP_Settings.enableExtraPadding;
				this.m_tintAllSprites = TMP_Settings.enableTintAllSprites;
				this.m_parseCtrlCharacters = TMP_Settings.enableParseEscapeCharacters;
				this.m_fontSize = (this.m_fontSizeBase = TMP_Settings.defaultFontSize);
				this.m_fontSizeMin = this.m_fontSize * TMP_Settings.defaultTextAutoSizingMinRatio;
				this.m_fontSizeMax = this.m_fontSize * TMP_Settings.defaultTextAutoSizingMaxRatio;
				this.m_isAlignmentEnumConverted = true;
				return;
			}
			if (!this.m_isAlignmentEnumConverted)
			{
				this.m_isAlignmentEnumConverted = true;
				this.m_textAlignment = TMP_Compatibility.ConvertTextAlignmentEnumValues(this.m_textAlignment);
			}
		}

		protected void GetSpecialCharacters(TMP_FontAsset fontAsset)
		{
			fontAsset.characterDictionary.TryGetValue(95, out this.m_cached_Underline_GlyphInfo);
			fontAsset.characterDictionary.TryGetValue(8230, out this.m_cached_Ellipsis_GlyphInfo);
		}

		protected void ReplaceTagWithCharacter(int[] chars, int insertionIndex, int tagLength, char c)
		{
			chars[insertionIndex] = (int)c;
			for (int i = insertionIndex + tagLength; i < chars.Length; i++)
			{
				chars[i - 3] = chars[i];
			}
		}

		protected TMP_FontAsset GetFontAssetForWeight(int fontWeight)
		{
			bool flag = (this.m_style & FontStyles.Italic) == FontStyles.Italic || (this.m_fontStyle & FontStyles.Italic) == FontStyles.Italic;
			int num = fontWeight / 100;
			TMP_FontAsset result;
			if (flag)
			{
				result = this.m_currentFontAsset.fontWeights[num].italicTypeface;
			}
			else
			{
				result = this.m_currentFontAsset.fontWeights[num].regularTypeface;
			}
			return result;
		}

		protected virtual void SetActiveSubMeshes(bool state)
		{
		}

		protected virtual void ClearSubMeshObjects()
		{
		}

		public virtual void ClearMesh()
		{
		}

		public virtual void ClearMesh(bool uploadGeometry)
		{
		}

		public virtual string GetParsedText()
		{
			if (this.m_textInfo == null)
			{
				return string.Empty;
			}
			int characterCount = this.m_textInfo.characterCount;
			char[] array = new char[characterCount];
			int num = 0;
			while (num < characterCount && num < this.m_textInfo.characterInfo.Length)
			{
				array[num] = this.m_textInfo.characterInfo[num].character;
				num++;
			}
			return new string(array);
		}

		protected Vector2 PackUV(float x, float y, float scale)
		{
			Vector2 vector;
			vector.x = (float)((int)(x * 511f));
			vector.y = (float)((int)(y * 511f));
			vector.x = vector.x * 4096f + vector.y;
			vector.y = scale;
			return vector;
		}

		protected float PackUV(float x, float y)
		{
			float num = (float)((double)((int)(x * 511f)));
			double num2 = (double)((int)(y * 511f));
			return (float)((double)num * 4096.0 + num2);
		}

		protected int HexToInt(char hex)
		{
			switch (hex)
			{
			case '0':
				return 0;
			case '1':
				return 1;
			case '2':
				return 2;
			case '3':
				return 3;
			case '4':
				return 4;
			case '5':
				return 5;
			case '6':
				return 6;
			case '7':
				return 7;
			case '8':
				return 8;
			case '9':
				return 9;
			case ':':
			case ';':
			case '<':
			case '=':
			case '>':
			case '?':
			case '@':
				break;
			case 'A':
				return 10;
			case 'B':
				return 11;
			case 'C':
				return 12;
			case 'D':
				return 13;
			case 'E':
				return 14;
			case 'F':
				return 15;
			default:
				switch (hex)
				{
				case 'a':
					return 10;
				case 'b':
					return 11;
				case 'c':
					return 12;
				case 'd':
					return 13;
				case 'e':
					return 14;
				case 'f':
					return 15;
				}
				break;
			}
			return 15;
		}

		protected int GetUTF16(int i)
		{
			return (this.HexToInt(this.m_text[i]) << 12) + (this.HexToInt(this.m_text[i + 1]) << 8) + (this.HexToInt(this.m_text[i + 2]) << 4) + this.HexToInt(this.m_text[i + 3]);
		}

		protected int GetUTF32(int i)
		{
			return 0 + (this.HexToInt(this.m_text[i]) << 30) + (this.HexToInt(this.m_text[i + 1]) << 24) + (this.HexToInt(this.m_text[i + 2]) << 20) + (this.HexToInt(this.m_text[i + 3]) << 16) + (this.HexToInt(this.m_text[i + 4]) << 12) + (this.HexToInt(this.m_text[i + 5]) << 8) + (this.HexToInt(this.m_text[i + 6]) << 4) + this.HexToInt(this.m_text[i + 7]);
		}

		protected Color32 HexCharsToColor(char[] hexChars, int tagCount)
		{
			if (tagCount == 4)
			{
				byte r = (byte)(this.HexToInt(hexChars[1]) * 16 + this.HexToInt(hexChars[1]));
				byte g = (byte)(this.HexToInt(hexChars[2]) * 16 + this.HexToInt(hexChars[2]));
				byte b = (byte)(this.HexToInt(hexChars[3]) * 16 + this.HexToInt(hexChars[3]));
				return new Color32(r, g, b, byte.MaxValue);
			}
			if (tagCount == 5)
			{
				byte r2 = (byte)(this.HexToInt(hexChars[1]) * 16 + this.HexToInt(hexChars[1]));
				byte g2 = (byte)(this.HexToInt(hexChars[2]) * 16 + this.HexToInt(hexChars[2]));
				byte b2 = (byte)(this.HexToInt(hexChars[3]) * 16 + this.HexToInt(hexChars[3]));
				byte a = (byte)(this.HexToInt(hexChars[4]) * 16 + this.HexToInt(hexChars[4]));
				return new Color32(r2, g2, b2, a);
			}
			if (tagCount == 7)
			{
				byte r3 = (byte)(this.HexToInt(hexChars[1]) * 16 + this.HexToInt(hexChars[2]));
				byte g3 = (byte)(this.HexToInt(hexChars[3]) * 16 + this.HexToInt(hexChars[4]));
				byte b3 = (byte)(this.HexToInt(hexChars[5]) * 16 + this.HexToInt(hexChars[6]));
				return new Color32(r3, g3, b3, byte.MaxValue);
			}
			if (tagCount == 9)
			{
				byte r4 = (byte)(this.HexToInt(hexChars[1]) * 16 + this.HexToInt(hexChars[2]));
				byte g4 = (byte)(this.HexToInt(hexChars[3]) * 16 + this.HexToInt(hexChars[4]));
				byte b4 = (byte)(this.HexToInt(hexChars[5]) * 16 + this.HexToInt(hexChars[6]));
				byte a2 = (byte)(this.HexToInt(hexChars[7]) * 16 + this.HexToInt(hexChars[8]));
				return new Color32(r4, g4, b4, a2);
			}
			if (tagCount == 10)
			{
				byte r5 = (byte)(this.HexToInt(hexChars[7]) * 16 + this.HexToInt(hexChars[7]));
				byte g5 = (byte)(this.HexToInt(hexChars[8]) * 16 + this.HexToInt(hexChars[8]));
				byte b5 = (byte)(this.HexToInt(hexChars[9]) * 16 + this.HexToInt(hexChars[9]));
				return new Color32(r5, g5, b5, byte.MaxValue);
			}
			if (tagCount == 11)
			{
				byte r6 = (byte)(this.HexToInt(hexChars[7]) * 16 + this.HexToInt(hexChars[7]));
				byte g6 = (byte)(this.HexToInt(hexChars[8]) * 16 + this.HexToInt(hexChars[8]));
				byte b6 = (byte)(this.HexToInt(hexChars[9]) * 16 + this.HexToInt(hexChars[9]));
				byte a3 = (byte)(this.HexToInt(hexChars[10]) * 16 + this.HexToInt(hexChars[10]));
				return new Color32(r6, g6, b6, a3);
			}
			if (tagCount == 13)
			{
				byte r7 = (byte)(this.HexToInt(hexChars[7]) * 16 + this.HexToInt(hexChars[8]));
				byte g7 = (byte)(this.HexToInt(hexChars[9]) * 16 + this.HexToInt(hexChars[10]));
				byte b7 = (byte)(this.HexToInt(hexChars[11]) * 16 + this.HexToInt(hexChars[12]));
				return new Color32(r7, g7, b7, byte.MaxValue);
			}
			if (tagCount == 15)
			{
				byte r8 = (byte)(this.HexToInt(hexChars[7]) * 16 + this.HexToInt(hexChars[8]));
				byte g8 = (byte)(this.HexToInt(hexChars[9]) * 16 + this.HexToInt(hexChars[10]));
				byte b8 = (byte)(this.HexToInt(hexChars[11]) * 16 + this.HexToInt(hexChars[12]));
				byte a4 = (byte)(this.HexToInt(hexChars[13]) * 16 + this.HexToInt(hexChars[14]));
				return new Color32(r8, g8, b8, a4);
			}
			return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}

		protected Color32 HexCharsToColor(char[] hexChars, int startIndex, int length)
		{
			if (length == 7)
			{
				byte r = (byte)(this.HexToInt(hexChars[startIndex + 1]) * 16 + this.HexToInt(hexChars[startIndex + 2]));
				byte g = (byte)(this.HexToInt(hexChars[startIndex + 3]) * 16 + this.HexToInt(hexChars[startIndex + 4]));
				byte b = (byte)(this.HexToInt(hexChars[startIndex + 5]) * 16 + this.HexToInt(hexChars[startIndex + 6]));
				return new Color32(r, g, b, byte.MaxValue);
			}
			if (length == 9)
			{
				byte r2 = (byte)(this.HexToInt(hexChars[startIndex + 1]) * 16 + this.HexToInt(hexChars[startIndex + 2]));
				byte g2 = (byte)(this.HexToInt(hexChars[startIndex + 3]) * 16 + this.HexToInt(hexChars[startIndex + 4]));
				byte b2 = (byte)(this.HexToInt(hexChars[startIndex + 5]) * 16 + this.HexToInt(hexChars[startIndex + 6]));
				byte a = (byte)(this.HexToInt(hexChars[startIndex + 7]) * 16 + this.HexToInt(hexChars[startIndex + 8]));
				return new Color32(r2, g2, b2, a);
			}
			return TMP_Text.s_colorWhite;
		}

		private int GetAttributeParameters(char[] chars, int startIndex, int length, ref float[] parameters)
		{
			int i = startIndex;
			int num = 0;
			while (i < startIndex + length)
			{
				parameters[num] = this.ConvertToFloat(chars, startIndex, length, out i);
				length -= i - startIndex + 1;
				startIndex = i + 1;
				num++;
			}
			return num;
		}

		protected float ConvertToFloat(char[] chars, int startIndex, int length)
		{
			int num = 0;
			return this.ConvertToFloat(chars, startIndex, length, out num);
		}

		protected float ConvertToFloat(char[] chars, int startIndex, int length, out int lastIndex)
		{
			if (startIndex == 0)
			{
				lastIndex = 0;
				return -9999f;
			}
			int num = startIndex + length;
			float num2 = 0f;
			int num3 = 0;
			int num4 = 0;
			int num5 = 1;
			for (int i = startIndex; i < num; i++)
			{
				char c = chars[i];
				if (c != ' ')
				{
					if (c == '.')
					{
						num4 = i;
						num3 = -1;
					}
					else if (c == '-')
					{
						num5 = -1;
					}
					else if (c == '+')
					{
						num5 = 1;
					}
					else
					{
						if (c == ',')
						{
							lastIndex = i;
							return num2 * (float)num5;
						}
						if (!char.IsDigit(c))
						{
							lastIndex = i;
							return -9999f;
						}
						switch (num3)
						{
						case -5:
							num2 += (float)(chars[i] - '0') * 1E-05f;
							break;
						case -4:
							num2 += (float)(chars[i] - '0') * 0.0001f;
							break;
						case -3:
							num2 += (float)(chars[i] - '0') * 0.001f;
							break;
						case -2:
							num2 += (float)(chars[i] - '0') * 0.01f;
							break;
						case -1:
							num2 += (float)(chars[i] - '0') * 0.1f;
							break;
						case 0:
							num2 = (float)(chars[i] - '0');
							break;
						case 1:
						case 2:
						case 3:
						case 4:
						case 5:
						case 6:
							num2 = num2 * 10f + (float)chars[i] - 48f;
							break;
						}
						if (num4 == 0)
						{
							num3++;
						}
						else
						{
							num3--;
						}
					}
				}
			}
			lastIndex = num;
			return num2 * (float)num5;
		}

		protected bool ValidateHtmlTag(int[] chars, int startIndex, out int endIndex)
		{
			int num = 0;
			byte b = 0;
			TagUnits tagUnits = TagUnits.Pixels;
			TagType tagType = TagType.None;
			int num2 = 0;
			this.m_xmlAttribute[num2].nameHashCode = 0;
			this.m_xmlAttribute[num2].valueType = TagType.None;
			this.m_xmlAttribute[num2].valueHashCode = 0;
			this.m_xmlAttribute[num2].valueStartIndex = 0;
			this.m_xmlAttribute[num2].valueLength = 0;
			this.m_xmlAttribute[1].nameHashCode = 0;
			this.m_xmlAttribute[2].nameHashCode = 0;
			this.m_xmlAttribute[3].nameHashCode = 0;
			this.m_xmlAttribute[4].nameHashCode = 0;
			endIndex = startIndex;
			bool flag = false;
			bool flag2 = false;
			int num3 = startIndex;
			while (num3 < chars.Length && chars[num3] != 0 && num < this.m_htmlTag.Length && chars[num3] != 60)
			{
				if (chars[num3] == 62)
				{
					flag2 = true;
					endIndex = num3;
					this.m_htmlTag[num] = '\0';
					break;
				}
				this.m_htmlTag[num] = (char)chars[num3];
				num++;
				if (b == 1)
				{
					if (tagType == TagType.None)
					{
						if (chars[num3] == 43 || chars[num3] == 45 || char.IsDigit((char)chars[num3]))
						{
							tagType = TagType.NumericalValue;
							this.m_xmlAttribute[num2].valueType = TagType.NumericalValue;
							this.m_xmlAttribute[num2].valueStartIndex = num - 1;
							XML_TagAttribute[] xmlAttribute = this.m_xmlAttribute;
							int num4 = num2;
							xmlAttribute[num4].valueLength = xmlAttribute[num4].valueLength + 1;
						}
						else if (chars[num3] == 35)
						{
							tagType = TagType.ColorValue;
							this.m_xmlAttribute[num2].valueType = TagType.ColorValue;
							this.m_xmlAttribute[num2].valueStartIndex = num - 1;
							XML_TagAttribute[] xmlAttribute2 = this.m_xmlAttribute;
							int num5 = num2;
							xmlAttribute2[num5].valueLength = xmlAttribute2[num5].valueLength + 1;
						}
						else if (chars[num3] == 34)
						{
							tagType = TagType.StringValue;
							this.m_xmlAttribute[num2].valueType = TagType.StringValue;
							this.m_xmlAttribute[num2].valueStartIndex = num;
						}
						else
						{
							tagType = TagType.StringValue;
							this.m_xmlAttribute[num2].valueType = TagType.StringValue;
							this.m_xmlAttribute[num2].valueStartIndex = num - 1;
							this.m_xmlAttribute[num2].valueHashCode = ((this.m_xmlAttribute[num2].valueHashCode << 5) + this.m_xmlAttribute[num2].valueHashCode ^ chars[num3]);
							XML_TagAttribute[] xmlAttribute3 = this.m_xmlAttribute;
							int num6 = num2;
							xmlAttribute3[num6].valueLength = xmlAttribute3[num6].valueLength + 1;
						}
					}
					else if (tagType == TagType.NumericalValue)
					{
						if (chars[num3] == 112 || chars[num3] == 101 || chars[num3] == 37 || chars[num3] == 32)
						{
							b = 2;
							tagType = TagType.None;
							num2++;
							this.m_xmlAttribute[num2].nameHashCode = 0;
							this.m_xmlAttribute[num2].valueType = TagType.None;
							this.m_xmlAttribute[num2].valueHashCode = 0;
							this.m_xmlAttribute[num2].valueStartIndex = 0;
							this.m_xmlAttribute[num2].valueLength = 0;
							if (chars[num3] == 101)
							{
								tagUnits = TagUnits.FontUnits;
							}
							else if (chars[num3] == 37)
							{
								tagUnits = TagUnits.Percentage;
							}
						}
						else if (b != 2)
						{
							XML_TagAttribute[] xmlAttribute4 = this.m_xmlAttribute;
							int num7 = num2;
							xmlAttribute4[num7].valueLength = xmlAttribute4[num7].valueLength + 1;
						}
					}
					else if (tagType == TagType.ColorValue)
					{
						if (chars[num3] != 32)
						{
							XML_TagAttribute[] xmlAttribute5 = this.m_xmlAttribute;
							int num8 = num2;
							xmlAttribute5[num8].valueLength = xmlAttribute5[num8].valueLength + 1;
						}
						else
						{
							b = 2;
							tagType = TagType.None;
							num2++;
							this.m_xmlAttribute[num2].nameHashCode = 0;
							this.m_xmlAttribute[num2].valueType = TagType.None;
							this.m_xmlAttribute[num2].valueHashCode = 0;
							this.m_xmlAttribute[num2].valueStartIndex = 0;
							this.m_xmlAttribute[num2].valueLength = 0;
						}
					}
					else if (tagType == TagType.StringValue)
					{
						if (chars[num3] != 34)
						{
							this.m_xmlAttribute[num2].valueHashCode = ((this.m_xmlAttribute[num2].valueHashCode << 5) + this.m_xmlAttribute[num2].valueHashCode ^ chars[num3]);
							XML_TagAttribute[] xmlAttribute6 = this.m_xmlAttribute;
							int num9 = num2;
							xmlAttribute6[num9].valueLength = xmlAttribute6[num9].valueLength + 1;
						}
						else
						{
							b = 2;
							tagType = TagType.None;
							num2++;
							this.m_xmlAttribute[num2].nameHashCode = 0;
							this.m_xmlAttribute[num2].valueType = TagType.None;
							this.m_xmlAttribute[num2].valueHashCode = 0;
							this.m_xmlAttribute[num2].valueStartIndex = 0;
							this.m_xmlAttribute[num2].valueLength = 0;
						}
					}
				}
				if (chars[num3] == 61)
				{
					b = 1;
				}
				if (b == 0 && chars[num3] == 32)
				{
					if (flag)
					{
						return false;
					}
					flag = true;
					b = 2;
					tagType = TagType.None;
					num2++;
					this.m_xmlAttribute[num2].nameHashCode = 0;
					this.m_xmlAttribute[num2].valueType = TagType.None;
					this.m_xmlAttribute[num2].valueHashCode = 0;
					this.m_xmlAttribute[num2].valueStartIndex = 0;
					this.m_xmlAttribute[num2].valueLength = 0;
				}
				if (b == 0)
				{
					this.m_xmlAttribute[num2].nameHashCode = (this.m_xmlAttribute[num2].nameHashCode << 3) - this.m_xmlAttribute[num2].nameHashCode + chars[num3];
				}
				if (b == 2 && chars[num3] == 32)
				{
					b = 0;
				}
				num3++;
			}
			if (!flag2)
			{
				return false;
			}
			if (this.tag_NoParsing && this.m_xmlAttribute[0].nameHashCode != 53822163 && this.m_xmlAttribute[0].nameHashCode != 49429939)
			{
				return false;
			}
			if (this.m_xmlAttribute[0].nameHashCode == 53822163 || this.m_xmlAttribute[0].nameHashCode == 49429939)
			{
				this.tag_NoParsing = false;
				return true;
			}
			if (this.m_htmlTag[0] == '#' && num == 4)
			{
				this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
				this.m_colorStack.Add(this.m_htmlColor);
				return true;
			}
			if (this.m_htmlTag[0] == '#' && num == 5)
			{
				this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
				this.m_colorStack.Add(this.m_htmlColor);
				return true;
			}
			if (this.m_htmlTag[0] == '#' && num == 7)
			{
				this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
				this.m_colorStack.Add(this.m_htmlColor);
				return true;
			}
			if (this.m_htmlTag[0] == '#' && num == 9)
			{
				this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
				this.m_colorStack.Add(this.m_htmlColor);
				return true;
			}
			int num10 = this.m_xmlAttribute[0].nameHashCode;
			float num13;
			if (num10 <= 155913)
			{
				if (num10 <= 926)
				{
					if (num10 > 85)
					{
						if (num10 > 426)
						{
							if (num10 > 656)
							{
								if (num10 <= 670)
								{
									if (num10 == 660)
									{
										return true;
									}
									if (num10 != 670)
									{
										return false;
									}
								}
								else
								{
									if (num10 == 912)
									{
										goto IL_3739;
									}
									if (num10 == 916)
									{
										return true;
									}
									if (num10 != 926)
									{
										return false;
									}
								}
								return true;
							}
							if (num10 <= 434)
							{
								if (num10 == 427)
								{
									goto IL_11F5;
								}
								if (num10 != 434)
								{
									return false;
								}
								goto IL_124D;
							}
							else
							{
								if (num10 == 444)
								{
									goto IL_128B;
								}
								if (num10 == 446)
								{
									goto IL_1358;
								}
								if (num10 != 656)
								{
									return false;
								}
							}
							IL_3739:
							int num11 = 1;
							while (num11 < this.m_xmlAttribute.Length && this.m_xmlAttribute[num11].nameHashCode != 0)
							{
								num10 = this.m_xmlAttribute[num11].nameHashCode;
								if (num10 != 275917)
								{
									if (num10 == 327550)
									{
										float num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[num11].valueStartIndex, this.m_xmlAttribute[num11].valueLength);
										switch (tagUnits)
										{
										case TagUnits.Pixels:
											Debug.Log("Table width = " + num12 + "px.");
											break;
										case TagUnits.FontUnits:
											Debug.Log("Table width = " + num12 + "em.");
											break;
										case TagUnits.Percentage:
											Debug.Log("Table width = " + num12 + "%.");
											break;
										}
									}
								}
								else
								{
									num10 = this.m_xmlAttribute[num11].valueHashCode;
									if (num10 <= -458210101)
									{
										if (num10 != -523808257)
										{
											if (num10 == -458210101)
											{
												Debug.Log("TD align=\"center\".");
											}
										}
										else
										{
											Debug.Log("TD align=\"justified\".");
										}
									}
									else if (num10 != 3774683)
									{
										if (num10 == 136703040)
										{
											Debug.Log("TD align=\"right\".");
										}
									}
									else
									{
										Debug.Log("TD align=\"left\".");
									}
								}
								num11++;
							}
							return true;
						}
						if (num10 <= 117)
						{
							if (num10 <= 105)
							{
								if (num10 == 98)
								{
									goto IL_11BD;
								}
								if (num10 != 105)
								{
									return false;
								}
								goto IL_1230;
							}
							else
							{
								if (num10 == 115)
								{
									goto IL_126C;
								}
								if (num10 != 117)
								{
									return false;
								}
								goto IL_12B8;
							}
						}
						else if (num10 <= 402)
						{
							if (num10 != 395)
							{
								if (num10 != 402)
								{
									return false;
								}
								goto IL_124D;
							}
						}
						else
						{
							if (num10 == 412)
							{
								goto IL_128B;
							}
							if (num10 == 414)
							{
								goto IL_1358;
							}
							if (num10 != 426)
							{
								return false;
							}
							return true;
						}
						IL_11F5:
						if ((this.m_fontStyle & FontStyles.Bold) != FontStyles.Bold)
						{
							this.m_fontWeightInternal = this.m_fontWeightStack.Remove();
							if (this.m_fontStyleStack.Remove(FontStyles.Bold) == 0)
							{
								this.m_style &= (FontStyles)(-2);
							}
						}
						return true;
						IL_124D:
						if (this.m_fontStyleStack.Remove(FontStyles.Italic) == 0)
						{
							this.m_style &= (FontStyles)(-3);
						}
						return true;
						IL_128B:
						if ((this.m_fontStyle & FontStyles.Strikethrough) != FontStyles.Strikethrough && this.m_fontStyleStack.Remove(FontStyles.Strikethrough) == 0)
						{
							this.m_style &= (FontStyles)(-65);
						}
						return true;
						IL_1358:
						if ((this.m_fontStyle & FontStyles.Underline) != FontStyles.Underline)
						{
							this.m_underlineColor = this.m_underlineColorStack.Remove();
							if (this.m_fontStyleStack.Remove(FontStyles.Underline) == 0)
							{
								this.m_style &= (FontStyles)(-5);
							}
						}
						return true;
					}
					if (num10 <= -884817987)
					{
						if (num10 <= -1831660941)
						{
							if (num10 <= -1883544150)
							{
								if (num10 == -1885698441)
								{
									goto IL_182B;
								}
								if (num10 != -1883544150)
								{
									return false;
								}
							}
							else
							{
								if (num10 == -1847322671)
								{
									goto IL_3189;
								}
								if (num10 != -1831660941)
								{
									return false;
								}
								goto IL_314A;
							}
						}
						else
						{
							if (num10 <= -1668324918)
							{
								if (num10 != -1690034531)
								{
									if (num10 != -1668324918)
									{
										return false;
									}
									goto IL_310C;
								}
							}
							else
							{
								if (num10 == -1632103439)
								{
									goto IL_3189;
								}
								if (num10 == -1616441709)
								{
									goto IL_314A;
								}
								if (num10 != -884817987)
								{
									return false;
								}
							}
							num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
							if (num13 == -9999f || num13 == 0f)
							{
								return false;
							}
							this.m_marginRight = num13;
							switch (tagUnits)
							{
							case TagUnits.FontUnits:
								this.m_marginRight *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
								break;
							case TagUnits.Percentage:
								this.m_marginRight = (this.m_marginWidth - ((this.m_width != -1f) ? this.m_width : 0f)) * this.m_marginRight / 100f;
								break;
							}
							this.m_marginRight = ((this.m_marginRight >= 0f) ? this.m_marginRight : 0f);
							return true;
						}
						IL_310C:
						if (this.m_fontStyleStack.Remove(FontStyles.LowerCase) == 0)
						{
							this.m_style &= (FontStyles)(-9);
						}
						return true;
						IL_3189:
						if (this.m_fontStyleStack.Remove(FontStyles.SmallCaps) == 0)
						{
							this.m_style &= (FontStyles)(-33);
						}
						return true;
					}
					if (num10 <= -445537194)
					{
						if (num10 <= -842693512)
						{
							if (num10 == -855002522)
							{
								goto IL_32B6;
							}
							if (num10 != -842693512)
							{
								return false;
							}
							goto IL_3488;
						}
						else
						{
							if (num10 == -842656867)
							{
								goto IL_2B85;
							}
							if (num10 == -445573839)
							{
								goto IL_3531;
							}
							if (num10 != -445537194)
							{
								return false;
							}
							goto IL_2C4C;
						}
					}
					else if (num10 <= 66)
					{
						if (num10 == -330774850)
						{
							goto IL_16BB;
						}
						if (num10 != 66)
						{
							return false;
						}
					}
					else
					{
						if (num10 == 73)
						{
							goto IL_1230;
						}
						if (num10 == 83)
						{
							goto IL_126C;
						}
						if (num10 != 85)
						{
							return false;
						}
						goto IL_12B8;
					}
					IL_11BD:
					this.m_style |= FontStyles.Bold;
					this.m_fontStyleStack.Add(FontStyles.Bold);
					this.m_fontWeightInternal = 700;
					this.m_fontWeightStack.Add(700);
					return true;
					IL_1230:
					this.m_style |= FontStyles.Italic;
					this.m_fontStyleStack.Add(FontStyles.Italic);
					return true;
					IL_126C:
					this.m_style |= FontStyles.Strikethrough;
					this.m_fontStyleStack.Add(FontStyles.Strikethrough);
					return true;
					IL_12B8:
					this.m_style |= FontStyles.Underline;
					this.m_fontStyleStack.Add(FontStyles.Underline);
					if (this.m_xmlAttribute[1].nameHashCode == 281955 || this.m_xmlAttribute[1].nameHashCode == 192323)
					{
						this.m_underlineColor = this.HexCharsToColor(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength);
					}
					else
					{
						this.m_underlineColor = this.m_htmlColor;
					}
					this.m_underlineColorStack.Add(this.m_underlineColor);
					return true;
				}
				if (num10 <= 28511)
				{
					if (num10 <= 4742)
					{
						if (num10 <= 3215)
						{
							if (num10 <= 2963)
							{
								if (num10 != 2959)
								{
									if (num10 != 2963)
									{
										return false;
									}
									return true;
								}
							}
							else
							{
								if (num10 == 2973)
								{
									return true;
								}
								if (num10 != 3215)
								{
									return false;
								}
							}
							return true;
						}
						if (num10 <= 3229)
						{
							if (num10 == 3219)
							{
								return true;
							}
							if (num10 != 3229)
							{
								return false;
							}
						}
						else
						{
							if (num10 == 4556)
							{
								goto IL_185A;
							}
							if (num10 == 4728)
							{
								goto IL_1447;
							}
							if (num10 != 4742)
							{
								return false;
							}
							goto IL_1581;
						}
						return true;
					}
					if (num10 > 20849)
					{
						if (num10 <= 22501)
						{
							if (num10 != 20863)
							{
								if (num10 != 22501)
								{
									return false;
								}
								goto IL_18FB;
							}
						}
						else
						{
							if (num10 == 22673)
							{
								goto IL_14D2;
							}
							if (num10 != 22687)
							{
								if (num10 != 28511)
								{
									return false;
								}
								goto IL_1CA9;
							}
						}
						if ((this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
						{
							if (this.m_fontScaleMultiplier < 1f)
							{
								this.m_baselineOffset -= this.m_currentFontAsset.fontInfo.SuperscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
								this.m_fontScaleMultiplier /= ((this.m_currentFontAsset.fontInfo.SubSize > 0f) ? this.m_currentFontAsset.fontInfo.SubSize : 1f);
							}
							if (this.m_fontStyleStack.Remove(FontStyles.Superscript) == 0)
							{
								this.m_style &= (FontStyles)(-129);
							}
						}
						return true;
					}
					if (num10 <= 6552)
					{
						if (num10 == 6380)
						{
							goto IL_185A;
						}
						if (num10 != 6552)
						{
							return false;
						}
						goto IL_1447;
					}
					else
					{
						if (num10 == 6566)
						{
							goto IL_1581;
						}
						if (num10 == 20677)
						{
							goto IL_18FB;
						}
						if (num10 != 20849)
						{
							return false;
						}
					}
					IL_14D2:
					if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript)
					{
						if (this.m_fontScaleMultiplier < 1f)
						{
							this.m_baselineOffset -= this.m_currentFontAsset.fontInfo.SubscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
							this.m_fontScaleMultiplier /= ((this.m_currentFontAsset.fontInfo.SubSize > 0f) ? this.m_currentFontAsset.fontInfo.SubSize : 1f);
						}
						if (this.m_fontStyleStack.Remove(FontStyles.Subscript) == 0)
						{
							this.m_style &= (FontStyles)(-257);
						}
					}
					return true;
					IL_18FB:
					this.m_isIgnoringAlignment = false;
					return true;
					IL_1447:
					this.m_fontScaleMultiplier *= ((this.m_currentFontAsset.fontInfo.SubSize > 0f) ? this.m_currentFontAsset.fontInfo.SubSize : 1f);
					this.m_baselineOffset += this.m_currentFontAsset.fontInfo.SubscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
					this.m_fontStyleStack.Add(FontStyles.Subscript);
					this.m_style |= FontStyles.Subscript;
					return true;
					IL_1581:
					this.m_fontScaleMultiplier *= ((this.m_currentFontAsset.fontInfo.SubSize > 0f) ? this.m_currentFontAsset.fontInfo.SubSize : 1f);
					this.m_baselineOffset += this.m_currentFontAsset.fontInfo.SuperscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
					this.m_fontStyleStack.Add(FontStyles.Superscript);
					this.m_style |= FontStyles.Superscript;
					return true;
					IL_185A:
					num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
					if (num13 == -9999f)
					{
						return false;
					}
					switch (tagUnits)
					{
					case TagUnits.Pixels:
						this.m_xAdvance = num13;
						return true;
					case TagUnits.FontUnits:
						this.m_xAdvance = num13 * this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
						return true;
					case TagUnits.Percentage:
						this.m_xAdvance = this.m_marginWidth * num13 / 100f;
						return true;
					default:
						return false;
					}
				}
				else
				{
					if (num10 <= 43969)
					{
						if (num10 <= 31191)
						{
							if (num10 <= 30266)
							{
								if (num10 != 30245)
								{
									if (num10 != 30266)
									{
										return false;
									}
									goto IL_230F;
								}
							}
							else
							{
								if (num10 == 31169)
								{
									goto IL_19DD;
								}
								if (num10 != 31191)
								{
									return false;
								}
								goto IL_1999;
							}
						}
						else if (num10 <= 41311)
						{
							if (num10 == 32745)
							{
								goto IL_19EF;
							}
							if (num10 != 41311)
							{
								return false;
							}
							goto IL_1CA9;
						}
						else if (num10 != 43045)
						{
							if (num10 == 43066)
							{
								goto IL_230F;
							}
							if (num10 != 43969)
							{
								return false;
							}
							goto IL_19DD;
						}
						this.m_style |= FontStyles.Highlight;
						this.m_fontStyleStack.Add(FontStyles.Highlight);
						this.m_highlightColor = this.HexCharsToColor(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
						this.m_highlightColorStack.Add(this.m_highlightColor);
						return true;
						IL_19DD:
						this.m_isNonBreakingSpace = true;
						return true;
						IL_230F:
						if (this.m_isParsingText && !this.m_isCalculatingPreferredValues)
						{
							int linkCount = this.m_textInfo.linkCount;
							if (linkCount + 1 > this.m_textInfo.linkInfo.Length)
							{
								TMP_TextInfo.Resize<TMP_LinkInfo>(ref this.m_textInfo.linkInfo, linkCount + 1);
							}
							this.m_textInfo.linkInfo[linkCount].textComponent = this;
							this.m_textInfo.linkInfo[linkCount].hashCode = this.m_xmlAttribute[0].valueHashCode;
							this.m_textInfo.linkInfo[linkCount].linkTextfirstCharacterIndex = this.m_characterCount;
							this.m_textInfo.linkInfo[linkCount].linkIdFirstCharacterIndex = startIndex + this.m_xmlAttribute[0].valueStartIndex;
							this.m_textInfo.linkInfo[linkCount].linkIdLength = this.m_xmlAttribute[0].valueLength;
							this.m_textInfo.linkInfo[linkCount].SetLinkID(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
						}
						return true;
					}
					if (num10 <= 143113)
					{
						if (num10 <= 45545)
						{
							if (num10 == 43991)
							{
								goto IL_1999;
							}
							if (num10 != 45545)
							{
								return false;
							}
							goto IL_19EF;
						}
						else
						{
							if (num10 == 141358)
							{
								goto IL_1F88;
							}
							if (num10 != 143092)
							{
								if (num10 != 143113)
								{
									return false;
								}
								goto IL_2449;
							}
						}
					}
					else if (num10 <= 145592)
					{
						if (num10 == 144016)
						{
							goto IL_19E6;
						}
						if (num10 != 145592)
						{
							return false;
						}
						goto IL_1C53;
					}
					else
					{
						if (num10 == 154158)
						{
							goto IL_1F88;
						}
						if (num10 != 155892)
						{
							if (num10 != 155913)
							{
								return false;
							}
							goto IL_2449;
						}
					}
					if ((this.m_fontStyle & FontStyles.Highlight) != FontStyles.Highlight)
					{
						this.m_highlightColor = this.m_highlightColorStack.Remove();
						if (this.m_fontStyleStack.Remove(FontStyles.Highlight) == 0)
						{
							this.m_style &= (FontStyles)(-513);
						}
					}
					return true;
					IL_1F88:
					MaterialReference materialReference = this.m_materialReferenceStack.Remove();
					this.m_currentFontAsset = materialReference.fontAsset;
					this.m_currentMaterial = materialReference.material;
					this.m_currentMaterialIndex = materialReference.index;
					this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * (this.m_isOrthographic ? 1f : 0.1f);
					return true;
					IL_2449:
					if (this.m_isParsingText && !this.m_isCalculatingPreferredValues)
					{
						this.m_textInfo.linkInfo[this.m_textInfo.linkCount].linkTextLength = this.m_characterCount - this.m_textInfo.linkInfo[this.m_textInfo.linkCount].linkTextfirstCharacterIndex;
						this.m_textInfo.linkCount++;
					}
					return true;
					IL_1999:
					if (this.m_overflowMode == TextOverflowModes.Page)
					{
						this.m_xAdvance = 0f + this.tag_LineIndent + this.tag_Indent;
						this.m_lineOffset = 0f;
						this.m_pageNumber++;
						this.m_isNewPage = true;
					}
					return true;
					IL_19EF:
					num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
					if (num13 == -9999f)
					{
						return false;
					}
					switch (tagUnits)
					{
					case TagUnits.Pixels:
						if (this.m_htmlTag[5] == '+')
						{
							this.m_currentFontSize = this.m_fontSize + num13;
							this.m_sizeStack.Add(this.m_currentFontSize);
							this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * (this.m_isOrthographic ? 1f : 0.1f);
							return true;
						}
						if (this.m_htmlTag[5] == '-')
						{
							this.m_currentFontSize = this.m_fontSize + num13;
							this.m_sizeStack.Add(this.m_currentFontSize);
							this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * (this.m_isOrthographic ? 1f : 0.1f);
							return true;
						}
						this.m_currentFontSize = num13;
						this.m_sizeStack.Add(this.m_currentFontSize);
						this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * (this.m_isOrthographic ? 1f : 0.1f);
						return true;
					case TagUnits.FontUnits:
						this.m_currentFontSize = this.m_fontSize * num13;
						this.m_sizeStack.Add(this.m_currentFontSize);
						this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * (this.m_isOrthographic ? 1f : 0.1f);
						return true;
					case TagUnits.Percentage:
						this.m_currentFontSize = this.m_fontSize * num13 / 100f;
						this.m_sizeStack.Add(this.m_currentFontSize);
						this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * (this.m_isOrthographic ? 1f : 0.1f);
						return true;
					default:
						return false;
					}
				}
				IL_1CA9:
				int valueHashCode = this.m_xmlAttribute[0].valueHashCode;
				int nameHashCode = this.m_xmlAttribute[1].nameHashCode;
				int valueHashCode2 = this.m_xmlAttribute[1].valueHashCode;
				if (valueHashCode == 764638571 || valueHashCode == 523367755)
				{
					this.m_currentFontAsset = this.m_materialReferences[0].fontAsset;
					this.m_currentMaterial = this.m_materialReferences[0].material;
					this.m_currentMaterialIndex = 0;
					this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * (this.m_isOrthographic ? 1f : 0.1f);
					this.m_materialReferenceStack.Add(this.m_materialReferences[0]);
					return true;
				}
				TMP_FontAsset tmp_FontAsset;
				if (!MaterialReferenceManager.TryGetFontAsset(valueHashCode, out tmp_FontAsset))
				{
					tmp_FontAsset = Resources.Load<TMP_FontAsset>(TMP_Settings.defaultFontAssetPath + new string(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength));
					if (tmp_FontAsset == null)
					{
						return false;
					}
					MaterialReferenceManager.AddFontAsset(tmp_FontAsset);
				}
				if (nameHashCode == 0 && valueHashCode2 == 0)
				{
					this.m_currentMaterial = tmp_FontAsset.material;
					this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
					this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
				}
				else
				{
					if (nameHashCode != 103415287 && nameHashCode != 72669687)
					{
						return false;
					}
					Material material;
					if (MaterialReferenceManager.TryGetMaterial(valueHashCode2, out material))
					{
						this.m_currentMaterial = material;
						this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
						this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
					}
					else
					{
						material = Resources.Load<Material>(TMP_Settings.defaultFontAssetPath + new string(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength));
						if (material == null)
						{
							return false;
						}
						MaterialReferenceManager.AddFontMaterial(valueHashCode2, material);
						this.m_currentMaterial = material;
						this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
						this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
					}
				}
				this.m_currentFontAsset = tmp_FontAsset;
				this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * (this.m_isOrthographic ? 1f : 0.1f);
				return true;
			}
			else
			{
				if (num10 <= 2246877)
				{
					if (num10 <= 1027847)
					{
						if (num10 <= 275917)
						{
							if (num10 <= 186622)
							{
								if (num10 <= 158392)
								{
									if (num10 == 156816)
									{
										goto IL_19E6;
									}
									if (num10 != 158392)
									{
										return false;
									}
									goto IL_1C53;
								}
								else if (num10 != 186285)
								{
									if (num10 != 186622)
									{
										return false;
									}
									goto IL_22C7;
								}
							}
							else if (num10 <= 227814)
							{
								if (num10 == 192323)
								{
									goto IL_2641;
								}
								if (num10 != 227814)
								{
									return false;
								}
								goto IL_3675;
							}
							else
							{
								if (num10 == 230446)
								{
									goto IL_2224;
								}
								if (num10 == 237918)
								{
									goto IL_25B9;
								}
								if (num10 != 275917)
								{
									return false;
								}
							}
							num10 = this.m_xmlAttribute[0].valueHashCode;
							if (num10 <= -458210101)
							{
								if (num10 == -523808257)
								{
									this.m_lineJustification = TextAlignmentOptions.Justified;
									this.m_lineJustificationStack.Add(this.m_lineJustification);
									return true;
								}
								if (num10 == -458210101)
								{
									this.m_lineJustification = TextAlignmentOptions.Center;
									this.m_lineJustificationStack.Add(this.m_lineJustification);
									return true;
								}
							}
							else
							{
								if (num10 == 3774683)
								{
									this.m_lineJustification = TextAlignmentOptions.Left;
									this.m_lineJustificationStack.Add(this.m_lineJustification);
									return true;
								}
								if (num10 == 122383428)
								{
									this.m_lineJustification = TextAlignmentOptions.Flush;
									this.m_lineJustificationStack.Add(this.m_lineJustification);
									return true;
								}
								if (num10 == 136703040)
								{
									this.m_lineJustification = TextAlignmentOptions.Right;
									this.m_lineJustificationStack.Add(this.m_lineJustification);
									return true;
								}
							}
							return false;
						}
						if (num10 <= 320078)
						{
							if (num10 <= 280416)
							{
								if (num10 == 276254)
								{
									goto IL_22C7;
								}
								if (num10 != 280416)
								{
									return false;
								}
								return false;
							}
							else
							{
								if (num10 == 281955)
								{
									goto IL_2641;
								}
								if (num10 == 317446)
								{
									goto IL_3675;
								}
								if (num10 != 320078)
								{
									return false;
								}
							}
						}
						else if (num10 <= 976214)
						{
							if (num10 == 327550)
							{
								goto IL_25B9;
							}
							if (num10 != 976214)
							{
								return false;
							}
							goto IL_25A6;
						}
						else
						{
							if (num10 == 982252)
							{
								goto IL_2A8E;
							}
							if (num10 == 1017743)
							{
								return true;
							}
							if (num10 != 1027847)
							{
								return false;
							}
							goto IL_2634;
						}
						IL_2224:
						num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
						if (num13 == -9999f || num13 == 0f)
						{
							return false;
						}
						switch (tagUnits)
						{
						case TagUnits.Pixels:
							this.m_xAdvance += num13;
							return true;
						case TagUnits.FontUnits:
							this.m_xAdvance += num13 * this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
							return true;
						case TagUnits.Percentage:
							return false;
						default:
							return false;
						}
						IL_22C7:
						if (this.m_xmlAttribute[0].valueLength != 3)
						{
							return false;
						}
						this.m_htmlColor.a = (byte)(this.HexToInt(this.m_htmlTag[7]) * 16 + this.HexToInt(this.m_htmlTag[8]));
						return true;
						IL_25B9:
						num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
						if (num13 == -9999f || num13 == 0f)
						{
							return false;
						}
						switch (tagUnits)
						{
						case TagUnits.Pixels:
							this.m_width = num13;
							break;
						case TagUnits.FontUnits:
							return false;
						case TagUnits.Percentage:
							this.m_width = this.m_marginWidth * num13 / 100f;
							break;
						}
						return true;
						IL_2641:
						if (this.m_htmlTag[6] == '#' && num == 10)
						{
							this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
							this.m_colorStack.Add(this.m_htmlColor);
							return true;
						}
						if (this.m_htmlTag[6] == '#' && num == 11)
						{
							this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
							this.m_colorStack.Add(this.m_htmlColor);
							return true;
						}
						if (this.m_htmlTag[6] == '#' && num == 13)
						{
							this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
							this.m_colorStack.Add(this.m_htmlColor);
							return true;
						}
						if (this.m_htmlTag[6] == '#' && num == 15)
						{
							this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
							this.m_colorStack.Add(this.m_htmlColor);
							return true;
						}
						num10 = this.m_xmlAttribute[0].valueHashCode;
						if (num10 <= 26556144)
						{
							if (num10 <= 125395)
							{
								if (num10 == -36881330)
								{
									this.m_htmlColor = new Color32(160, 32, 240, byte.MaxValue);
									this.m_colorStack.Add(this.m_htmlColor);
									return true;
								}
								if (num10 == 125395)
								{
									this.m_htmlColor = Color.red;
									this.m_colorStack.Add(this.m_htmlColor);
									return true;
								}
							}
							else
							{
								if (num10 == 3573310)
								{
									this.m_htmlColor = Color.blue;
									this.m_colorStack.Add(this.m_htmlColor);
									return true;
								}
								if (num10 == 26556144)
								{
									this.m_htmlColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);
									this.m_colorStack.Add(this.m_htmlColor);
									return true;
								}
							}
						}
						else if (num10 <= 121463835)
						{
							if (num10 == 117905991)
							{
								this.m_htmlColor = Color.black;
								this.m_colorStack.Add(this.m_htmlColor);
								return true;
							}
							if (num10 == 121463835)
							{
								this.m_htmlColor = Color.green;
								this.m_colorStack.Add(this.m_htmlColor);
								return true;
							}
						}
						else
						{
							if (num10 == 140357351)
							{
								this.m_htmlColor = Color.white;
								this.m_colorStack.Add(this.m_htmlColor);
								return true;
							}
							if (num10 == 554054276)
							{
								this.m_htmlColor = Color.yellow;
								this.m_colorStack.Add(this.m_htmlColor);
								return true;
							}
						}
						return false;
						IL_3675:
						num10 = this.m_xmlAttribute[1].nameHashCode;
						if (num10 == 327550)
						{
							float num14 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength);
							switch (tagUnits)
							{
							case TagUnits.Pixels:
								Debug.Log("Table width = " + num14 + "px.");
								break;
							case TagUnits.FontUnits:
								Debug.Log("Table width = " + num14 + "em.");
								break;
							case TagUnits.Percentage:
								Debug.Log("Table width = " + num14 + "%.");
								break;
							}
						}
						return true;
					}
					if (num10 <= 1524585)
					{
						if (num10 <= 1117479)
						{
							if (num10 <= 1071884)
							{
								if (num10 == 1065846)
								{
									goto IL_25A6;
								}
								if (num10 != 1071884)
								{
									return false;
								}
								goto IL_2A8E;
							}
							else
							{
								if (num10 == 1107375)
								{
									return true;
								}
								if (num10 != 1117479)
								{
									return false;
								}
								goto IL_2634;
							}
						}
						else if (num10 <= 1356515)
						{
							if (num10 == 1286342)
							{
								goto IL_3547;
							}
							if (num10 != 1356515)
							{
								return false;
							}
						}
						else
						{
							if (num10 == 1441524)
							{
								goto IL_2AA1;
							}
							if (num10 == 1482398)
							{
								goto IL_31A9;
							}
							if (num10 != 1524585)
							{
								return false;
							}
							goto IL_29DE;
						}
					}
					else
					{
						if (num10 <= 1983971)
						{
							if (num10 <= 1619421)
							{
								if (num10 == 1600507)
								{
									goto IL_3602;
								}
								if (num10 != 1619421)
								{
									return false;
								}
							}
							else
							{
								if (num10 == 1750458)
								{
									return false;
								}
								if (num10 == 1913798)
								{
									goto IL_3547;
								}
								if (num10 != 1983971)
								{
									return false;
								}
								goto IL_28E7;
							}
						}
						else if (num10 <= 2109854)
						{
							if (num10 == 2068980)
							{
								goto IL_2AA1;
							}
							if (num10 != 2109854)
							{
								return false;
							}
							goto IL_31A9;
						}
						else
						{
							if (num10 == 2152041)
							{
								goto IL_29DE;
							}
							if (num10 == 2227963)
							{
								goto IL_3602;
							}
							if (num10 != 2246877)
							{
								return false;
							}
						}
						int valueHashCode3 = this.m_xmlAttribute[0].valueHashCode;
						this.m_spriteIndex = -1;
						TMP_SpriteAsset tmp_SpriteAsset;
						if (this.m_xmlAttribute[0].valueType == TagType.None || this.m_xmlAttribute[0].valueType == TagType.NumericalValue)
						{
							if (this.m_spriteAsset != null)
							{
								this.m_currentSpriteAsset = this.m_spriteAsset;
							}
							else if (this.m_defaultSpriteAsset != null)
							{
								this.m_currentSpriteAsset = this.m_defaultSpriteAsset;
							}
							else if (this.m_defaultSpriteAsset == null)
							{
								if (TMP_Settings.defaultSpriteAsset != null)
								{
									this.m_defaultSpriteAsset = TMP_Settings.defaultSpriteAsset;
								}
								else
								{
									this.m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
								}
								this.m_currentSpriteAsset = this.m_defaultSpriteAsset;
							}
							if (this.m_currentSpriteAsset == null)
							{
								return false;
							}
						}
						else if (MaterialReferenceManager.TryGetSpriteAsset(valueHashCode3, out tmp_SpriteAsset))
						{
							this.m_currentSpriteAsset = tmp_SpriteAsset;
						}
						else
						{
							if (tmp_SpriteAsset == null)
							{
								tmp_SpriteAsset = Resources.Load<TMP_SpriteAsset>(TMP_Settings.defaultSpriteAssetPath + new string(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength));
							}
							if (tmp_SpriteAsset == null)
							{
								return false;
							}
							MaterialReferenceManager.AddSpriteAsset(valueHashCode3, tmp_SpriteAsset);
							this.m_currentSpriteAsset = tmp_SpriteAsset;
						}
						if (this.m_xmlAttribute[0].valueType == TagType.NumericalValue)
						{
							int num15 = (int)this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
							if (num15 == -9999)
							{
								return false;
							}
							if (num15 > this.m_currentSpriteAsset.spriteInfoList.Count - 1)
							{
								return false;
							}
							this.m_spriteIndex = num15;
						}
						this.m_spriteColor = TMP_Text.s_colorWhite;
						this.m_tintSprite = false;
						int num16 = 0;
						while (num16 < this.m_xmlAttribute.Length && this.m_xmlAttribute[num16].nameHashCode != 0)
						{
							int nameHashCode2 = this.m_xmlAttribute[num16].nameHashCode;
							if (nameHashCode2 <= 43347)
							{
								if (nameHashCode2 <= 30547)
								{
									if (nameHashCode2 == 26705)
									{
										goto IL_2FF3;
									}
									if (nameHashCode2 != 30547)
									{
										goto IL_3076;
									}
								}
								else
								{
									if (nameHashCode2 == 33019)
									{
										goto IL_2F73;
									}
									if (nameHashCode2 == 39505)
									{
										goto IL_2FF3;
									}
									if (nameHashCode2 != 43347)
									{
										goto IL_3076;
									}
								}
								int num17 = this.m_currentSpriteAsset.GetSpriteIndexFromHashcode(this.m_xmlAttribute[num16].valueHashCode);
								if (num17 == -1)
								{
									return false;
								}
								this.m_spriteIndex = num17;
								goto IL_308A;
								IL_2FF3:
								if (this.GetAttributeParameters(this.m_htmlTag, this.m_xmlAttribute[num16].valueStartIndex, this.m_xmlAttribute[num16].valueLength, ref this.m_attributeParameterValues) != 3)
								{
									return false;
								}
								this.m_spriteIndex = (int)this.m_attributeParameterValues[0];
								if (this.m_isParsingText)
								{
									this.spriteAnimator.DoSpriteAnimation(this.m_characterCount, this.m_currentSpriteAsset, this.m_spriteIndex, (int)this.m_attributeParameterValues[1], (int)this.m_attributeParameterValues[2]);
								}
							}
							else
							{
								if (nameHashCode2 <= 192323)
								{
									if (nameHashCode2 == 45819)
									{
										goto IL_2F73;
									}
									if (nameHashCode2 != 192323)
									{
										goto IL_3076;
									}
								}
								else
								{
									if (nameHashCode2 != 205930)
									{
										if (nameHashCode2 == 281955)
										{
											goto IL_2FB8;
										}
										if (nameHashCode2 != 295562)
										{
											goto IL_3076;
										}
									}
									int num17 = (int)this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength);
									if (num17 == -9999)
									{
										return false;
									}
									if (num17 > this.m_currentSpriteAsset.spriteInfoList.Count - 1)
									{
										return false;
									}
									this.m_spriteIndex = num17;
									goto IL_308A;
								}
								IL_2FB8:
								this.m_spriteColor = this.HexCharsToColor(this.m_htmlTag, this.m_xmlAttribute[num16].valueStartIndex, this.m_xmlAttribute[num16].valueLength);
							}
							IL_308A:
							num16++;
							continue;
							IL_2F73:
							this.m_tintSprite = (this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[num16].valueStartIndex, this.m_xmlAttribute[num16].valueLength) != 0f);
							goto IL_308A;
							IL_3076:
							if (nameHashCode2 != 2246877 && nameHashCode2 != 1619421)
							{
								return false;
							}
							goto IL_308A;
						}
						if (this.m_spriteIndex == -1)
						{
							return false;
						}
						this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentSpriteAsset.material, this.m_currentSpriteAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
						this.m_textElementType = TMP_TextElementType.Sprite;
						return true;
						IL_3602:
						num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
						if (num13 == -9999f)
						{
							return false;
						}
						this.m_FXMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, num13), Vector3.one);
						this.m_isFXMatrixSet = true;
						return true;
					}
					IL_28E7:
					num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
					if (num13 == -9999f || num13 == 0f)
					{
						return false;
					}
					switch (tagUnits)
					{
					case TagUnits.Pixels:
						this.m_cSpacing = num13;
						break;
					case TagUnits.FontUnits:
						this.m_cSpacing = num13;
						this.m_cSpacing *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
						break;
					case TagUnits.Percentage:
						return false;
					}
					return true;
					IL_29DE:
					num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
					if (num13 == -9999f || num13 == 0f)
					{
						return false;
					}
					switch (tagUnits)
					{
					case TagUnits.Pixels:
						this.m_monoSpacing = num13;
						break;
					case TagUnits.FontUnits:
						this.m_monoSpacing = num13;
						this.m_monoSpacing *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
						break;
					case TagUnits.Percentage:
						return false;
					}
					return true;
					IL_2AA1:
					num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
					if (num13 == -9999f || num13 == 0f)
					{
						return false;
					}
					switch (tagUnits)
					{
					case TagUnits.Pixels:
						this.tag_Indent = num13;
						break;
					case TagUnits.FontUnits:
						this.tag_Indent = num13;
						this.tag_Indent *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
						break;
					case TagUnits.Percentage:
						this.tag_Indent = this.m_marginWidth * num13 / 100f;
						break;
					}
					this.m_indentStack.Add(this.tag_Indent);
					this.m_xAdvance = this.tag_Indent;
					return true;
					IL_31A9:
					num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
					if (num13 == -9999f || num13 == 0f)
					{
						return false;
					}
					this.m_marginLeft = num13;
					switch (tagUnits)
					{
					case TagUnits.FontUnits:
						this.m_marginLeft *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
						break;
					case TagUnits.Percentage:
						this.m_marginLeft = (this.m_marginWidth - ((this.m_width != -1f) ? this.m_width : 0f)) * this.m_marginLeft / 100f;
						break;
					}
					this.m_marginLeft = ((this.m_marginLeft >= 0f) ? this.m_marginLeft : 0f);
					this.m_marginRight = this.m_marginLeft;
					return true;
					IL_3547:
					int valueHashCode4 = this.m_xmlAttribute[0].valueHashCode;
					if (this.m_isParsingText)
					{
						this.m_actionStack.Add(valueHashCode4);
						Debug.Log(string.Concat(new object[]
						{
							"Action ID: [",
							valueHashCode4,
							"] First character index: ",
							this.m_characterCount
						}));
					}
					return true;
					IL_25A6:
					this.m_lineJustification = this.m_lineJustificationStack.Remove();
					return true;
					IL_2634:
					this.m_width = -1f;
					return true;
					IL_2A8E:
					this.m_htmlColor = this.m_colorStack.Remove();
					return true;
				}
				if (num10 <= 47840323)
				{
					if (num10 <= 7598483)
					{
						if (num10 <= 7011901)
						{
							if (num10 <= 6886018)
							{
								if (num10 == 6815845)
								{
									goto IL_35A9;
								}
								if (num10 != 6886018)
								{
									return false;
								}
							}
							else
							{
								if (num10 == 6971027)
								{
									goto IL_2B72;
								}
								if (num10 != 7011901)
								{
									return false;
								}
								goto IL_329E;
							}
						}
						else if (num10 <= 7130010)
						{
							if (num10 == 7054088)
							{
								goto IL_2A7F;
							}
							if (num10 != 7130010)
							{
								return false;
							}
							goto IL_366C;
						}
						else
						{
							if (num10 == 7443301)
							{
								goto IL_35A9;
							}
							if (num10 != 7513474)
							{
								if (num10 != 7598483)
								{
									return false;
								}
								goto IL_2B72;
							}
						}
						if (!this.m_isParsingText)
						{
							return true;
						}
						if (this.m_characterCount > 0)
						{
							this.m_xAdvance -= this.m_cSpacing;
							this.m_textInfo.characterInfo[this.m_characterCount - 1].xAdvance = this.m_xAdvance;
						}
						this.m_cSpacing = 0f;
						return true;
						IL_2B72:
						this.tag_Indent = this.m_indentStack.Remove();
						return true;
						IL_35A9:
						if (this.m_isParsingText)
						{
							Debug.Log(string.Concat(new object[]
							{
								"Action ID: [",
								this.m_actionStack.CurrentItem(),
								"] Last character index: ",
								this.m_characterCount - 1
							}));
						}
						this.m_actionStack.Remove();
						return true;
					}
					if (num10 <= 10723418)
					{
						if (num10 <= 7681544)
						{
							if (num10 == 7639357)
							{
								goto IL_329E;
							}
							if (num10 != 7681544)
							{
								return false;
							}
							goto IL_2A7F;
						}
						else
						{
							if (num10 == 7757466)
							{
								goto IL_366C;
							}
							if (num10 == 9133802)
							{
								goto IL_312B;
							}
							if (num10 != 10723418)
							{
								return false;
							}
						}
					}
					else
					{
						if (num10 <= 13526026)
						{
							if (num10 != 11642281)
							{
								if (num10 != 13526026)
								{
									return false;
								}
								goto IL_312B;
							}
						}
						else
						{
							if (num10 == 15115642)
							{
								goto IL_353E;
							}
							if (num10 != 16034505)
							{
								if (num10 != 47840323)
								{
									return false;
								}
								goto IL_314A;
							}
						}
						num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
						if (num13 == -9999f || num13 == 0f)
						{
							return false;
						}
						switch (tagUnits)
						{
						case TagUnits.Pixels:
							this.m_baselineOffset = num13;
							return true;
						case TagUnits.FontUnits:
							this.m_baselineOffset = num13 * this.m_fontScale * this.m_fontAsset.fontInfo.Ascender;
							return true;
						case TagUnits.Percentage:
							return false;
						default:
							return false;
						}
					}
					IL_353E:
					this.tag_NoParsing = true;
					return true;
					IL_2A7F:
					this.m_monoSpacing = 0f;
					return true;
					IL_329E:
					this.m_marginLeft = 0f;
					this.m_marginRight = 0f;
					return true;
					IL_366C:
					this.m_isFXMatrixSet = false;
					return true;
				}
				if (num10 <= 551025096)
				{
					if (num10 <= 103415287)
					{
						if (num10 <= 52232547)
						{
							if (num10 != 50348802)
							{
								if (num10 != 52232547)
								{
									return false;
								}
								goto IL_314A;
							}
						}
						else if (num10 != 54741026)
						{
							if (num10 != 72669687 && num10 != 103415287)
							{
								return false;
							}
							int valueHashCode2 = this.m_xmlAttribute[0].valueHashCode;
							if (valueHashCode2 != 764638571 && valueHashCode2 != 523367755)
							{
								Material material;
								if (MaterialReferenceManager.TryGetMaterial(valueHashCode2, out material))
								{
									if (this.m_currentFontAsset.atlas.GetInstanceID() != material.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
									{
										return false;
									}
									this.m_currentMaterial = material;
									this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
									this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
								}
								else
								{
									material = Resources.Load<Material>(TMP_Settings.defaultFontAssetPath + new string(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength));
									if (material == null)
									{
										return false;
									}
									if (this.m_currentFontAsset.atlas.GetInstanceID() != material.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
									{
										return false;
									}
									MaterialReferenceManager.AddFontMaterial(valueHashCode2, material);
									this.m_currentMaterial = material;
									this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
									this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
								}
								return true;
							}
							if (this.m_currentFontAsset.atlas.GetInstanceID() != this.m_currentMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
							{
								return false;
							}
							this.m_currentMaterial = this.m_materialReferences[0].material;
							this.m_currentMaterialIndex = 0;
							this.m_materialReferenceStack.Add(this.m_materialReferences[0]);
							return true;
						}
						this.m_baselineOffset = 0f;
						return true;
					}
					if (num10 <= 374360934)
					{
						if (num10 != 343615334 && num10 != 374360934)
						{
							return false;
						}
						if (this.m_currentMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() != this.m_materialReferenceStack.PreviousItem().material.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
						{
							return false;
						}
						MaterialReference materialReference2 = this.m_materialReferenceStack.Remove();
						this.m_currentMaterial = materialReference2.material;
						this.m_currentMaterialIndex = materialReference2.index;
						return true;
					}
					else
					{
						if (num10 == 457225591)
						{
							goto IL_182B;
						}
						if (num10 != 514803617)
						{
							if (num10 != 551025096)
							{
								return false;
							}
							goto IL_316A;
						}
					}
				}
				else if (num10 <= 1100728678)
				{
					if (num10 <= 730022849)
					{
						if (num10 == 566686826)
						{
							goto IL_312B;
						}
						if (num10 != 730022849)
						{
							return false;
						}
					}
					else
					{
						if (num10 == 766244328)
						{
							goto IL_316A;
						}
						if (num10 == 781906058)
						{
							goto IL_312B;
						}
						if (num10 != 1100728678)
						{
							return false;
						}
						goto IL_32B6;
					}
				}
				else if (num10 <= 1109386397)
				{
					if (num10 == 1109349752)
					{
						goto IL_3488;
					}
					if (num10 != 1109386397)
					{
						return false;
					}
					goto IL_2B85;
				}
				else
				{
					if (num10 == 1897350193)
					{
						goto IL_3531;
					}
					if (num10 == 1897386838)
					{
						goto IL_2C4C;
					}
					if (num10 != 2012149182)
					{
						return false;
					}
					goto IL_16BB;
				}
				this.m_style |= FontStyles.LowerCase;
				this.m_fontStyleStack.Add(FontStyles.LowerCase);
				return true;
				IL_316A:
				this.m_style |= FontStyles.SmallCaps;
				this.m_fontStyleStack.Add(FontStyles.SmallCaps);
				return true;
				IL_312B:
				this.m_style |= FontStyles.UpperCase;
				this.m_fontStyleStack.Add(FontStyles.UpperCase);
				return true;
			}
			IL_16BB:
			num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
			if (num13 == -9999f || num13 == 0f)
			{
				return false;
			}
			if ((this.m_fontStyle & FontStyles.Bold) == FontStyles.Bold)
			{
				return true;
			}
			this.m_style &= (FontStyles)(-2);
			num10 = (int)num13;
			if (num10 <= 400)
			{
				if (num10 <= 200)
				{
					if (num10 != 100)
					{
						if (num10 == 200)
						{
							this.m_fontWeightInternal = 200;
						}
					}
					else
					{
						this.m_fontWeightInternal = 100;
					}
				}
				else if (num10 != 300)
				{
					if (num10 == 400)
					{
						this.m_fontWeightInternal = 400;
					}
				}
				else
				{
					this.m_fontWeightInternal = 300;
				}
			}
			else if (num10 <= 600)
			{
				if (num10 != 500)
				{
					if (num10 == 600)
					{
						this.m_fontWeightInternal = 600;
					}
				}
				else
				{
					this.m_fontWeightInternal = 500;
				}
			}
			else if (num10 != 700)
			{
				if (num10 != 800)
				{
					if (num10 == 900)
					{
						this.m_fontWeightInternal = 900;
					}
				}
				else
				{
					this.m_fontWeightInternal = 800;
				}
			}
			else
			{
				this.m_fontWeightInternal = 700;
				this.m_style |= FontStyles.Bold;
			}
			this.m_fontWeightStack.Add(this.m_fontWeightInternal);
			return true;
			IL_182B:
			this.m_fontWeightInternal = this.m_fontWeightStack.Remove();
			if (this.m_fontWeightInternal == 400)
			{
				this.m_style &= (FontStyles)(-2);
			}
			return true;
			IL_19E6:
			this.m_isNonBreakingSpace = false;
			return true;
			IL_1C53:
			this.m_currentFontSize = this.m_sizeStack.Remove();
			this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * (this.m_isOrthographic ? 1f : 0.1f);
			return true;
			IL_2B85:
			num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
			if (num13 == -9999f || num13 == 0f)
			{
				return false;
			}
			switch (tagUnits)
			{
			case TagUnits.Pixels:
				this.tag_LineIndent = num13;
				break;
			case TagUnits.FontUnits:
				this.tag_LineIndent = num13;
				this.tag_LineIndent *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
				break;
			case TagUnits.Percentage:
				this.tag_LineIndent = this.m_marginWidth * num13 / 100f;
				break;
			}
			this.m_xAdvance += this.tag_LineIndent;
			return true;
			IL_2C4C:
			this.tag_LineIndent = 0f;
			return true;
			IL_314A:
			if (this.m_fontStyleStack.Remove(FontStyles.UpperCase) == 0)
			{
				this.m_style &= (FontStyles)(-17);
			}
			return true;
			IL_32B6:
			num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
			if (num13 == -9999f || num13 == 0f)
			{
				return false;
			}
			this.m_marginLeft = num13;
			switch (tagUnits)
			{
			case TagUnits.FontUnits:
				this.m_marginLeft *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
				break;
			case TagUnits.Percentage:
				this.m_marginLeft = (this.m_marginWidth - ((this.m_width != -1f) ? this.m_width : 0f)) * this.m_marginLeft / 100f;
				break;
			}
			this.m_marginLeft = ((this.m_marginLeft >= 0f) ? this.m_marginLeft : 0f);
			return true;
			IL_3488:
			num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
			if (num13 == -9999f)
			{
				return false;
			}
			this.m_lineHeight = num13;
			switch (tagUnits)
			{
			case TagUnits.FontUnits:
				this.m_lineHeight *= this.m_fontAsset.fontInfo.LineHeight * this.m_fontScale;
				break;
			case TagUnits.Percentage:
				this.m_lineHeight = this.m_fontAsset.fontInfo.LineHeight * this.m_lineHeight / 100f * this.m_fontScale;
				break;
			}
			return true;
			IL_3531:
			this.m_lineHeight = -32767f;
			return true;
		}

		protected enum TextInputSources
		{
			Text,
			SetText,
			SetCharArray,
			String
		}
	}
}
