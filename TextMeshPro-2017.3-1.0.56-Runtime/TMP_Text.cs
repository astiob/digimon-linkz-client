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

		protected Color32 m_underlineColor = TMP_Text.s_colorWhite;

		protected Color32 m_strikethroughColor = TMP_Text.s_colorWhite;

		protected Color32 m_highlightColor = TMP_Text.s_colorWhite;

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

		protected WordWrapState m_SavedWordWrapState = default(WordWrapState);

		protected WordWrapState m_SavedLineState = default(WordWrapState);

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

		protected TMP_XmlTagStack<Color32> m_strikethroughColorStack = new TMP_XmlTagStack<Color32>(new Color32[16]);

		protected TMP_XmlTagStack<Color32> m_highlightColorStack = new TMP_XmlTagStack<Color32>(new Color32[16]);

		protected TMP_ColorGradient m_colorGradientPreset;

		protected TMP_XmlTagStack<TMP_ColorGradient> m_colorGradientStack = new TMP_XmlTagStack<TMP_ColorGradient>(new TMP_ColorGradient[16]);

		protected float m_tabSpacing;

		protected float m_spacing;

		protected TMP_XmlTagStack<int> m_styleStack = new TMP_XmlTagStack<int>(new int[16]);

		protected TMP_XmlTagStack<int> m_actionStack = new TMP_XmlTagStack<int>(new int[16]);

		protected float m_padding;

		protected float m_baselineOffset;

		protected TMP_XmlTagStack<float> m_baselineOffsetStack = new TMP_XmlTagStack<float>(new float[16]);

		protected float m_xAdvance;

		protected TMP_TextElementType m_textElementType;

		protected TMP_TextElement m_cached_TextElement;

		protected TMP_Glyph m_cached_Underline_GlyphInfo;

		protected TMP_Glyph m_cached_Ellipsis_GlyphInfo;

		protected TMP_SpriteAsset m_defaultSpriteAsset;

		protected TMP_SpriteAsset m_currentSpriteAsset;

		protected int m_spriteCount;

		protected int m_spriteIndex;

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

		public override Color color
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
				this.m_fontSize = value;
				if (!this.m_enableAutoSizing)
				{
					this.m_fontSizeBase = this.m_fontSize;
				}
				this.SetVerticesDirty();
				this.SetLayoutDirty();
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
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.m_isInputParsingRequired = true;
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
				this.m_isCalculateSizeRequired = true;
				this.m_isInputParsingRequired = true;
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
				this.m_characterSpacing = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
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
				this.m_wordSpacing = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
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
				this.m_lineSpacing = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
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
				this.m_lineSpacingMax = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
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
				this.m_paragraphSpacing = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
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
				this.m_charWidthMaxAdj = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
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
				this.m_enableKerning = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
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
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
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
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
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
			Material material2 = material;
			material2.name += " (Instance)";
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

		public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
			base.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
			this.InternalCrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
		}

		public override void CrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
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
					int num2 = (int)(text[i + 1] - '0');
					if (num2 != 0)
					{
						if (num2 != 1)
						{
							if (num2 == 2)
							{
								this.AddFloatToCharArray(arg2, ref num, precision);
							}
						}
						else
						{
							this.AddFloatToCharArray(arg1, ref num, precision);
						}
					}
					else
					{
						this.AddFloatToCharArray(arg0, ref num, precision);
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
			if (this.m_char_buffer == null)
			{
				this.m_char_buffer = new int[8];
			}
			this.m_styleStack.Clear();
			int num = 0;
			int i = 0;
			while (i < sourceText.Length)
			{
				if (sourceText[i] != '\\' || i >= sourceText.Length - 1)
				{
					goto IL_105;
				}
				int num2 = (int)sourceText[i + 1];
				if (num2 != 110)
				{
					if (num2 != 114)
					{
						if (num2 != 116)
						{
							goto IL_105;
						}
						if (num == this.m_char_buffer.Length)
						{
							this.ResizeInternalArray<int>(ref this.m_char_buffer);
						}
						this.m_char_buffer[num] = 9;
						i++;
						num++;
					}
					else
					{
						if (num == this.m_char_buffer.Length)
						{
							this.ResizeInternalArray<int>(ref this.m_char_buffer);
						}
						this.m_char_buffer[num] = 13;
						i++;
						num++;
					}
				}
				else
				{
					if (num == this.m_char_buffer.Length)
					{
						this.ResizeInternalArray<int>(ref this.m_char_buffer);
					}
					this.m_char_buffer[num] = 10;
					i++;
					num++;
				}
				IL_1E3:
				i++;
				continue;
				IL_105:
				if (sourceText[i] == '<')
				{
					if (this.IsTagName(ref sourceText, "<BR>", i))
					{
						if (num == this.m_char_buffer.Length)
						{
							this.ResizeInternalArray<int>(ref this.m_char_buffer);
						}
						this.m_char_buffer[num] = 10;
						num++;
						i += 3;
						goto IL_1E3;
					}
					if (this.IsTagName(ref sourceText, "<STYLE=", i))
					{
						int num3 = 0;
						if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num3, ref this.m_char_buffer, ref num))
						{
							i = num3;
							goto IL_1E3;
						}
					}
					else if (this.IsTagName(ref sourceText, "</STYLE>", i))
					{
						this.ReplaceClosingStyleTag(ref sourceText, i, ref this.m_char_buffer, ref num);
						i += 7;
						goto IL_1E3;
					}
				}
				if (num == this.m_char_buffer.Length)
				{
					this.ResizeInternalArray<int>(ref this.m_char_buffer);
				}
				this.m_char_buffer[num] = (int)sourceText[i];
				num++;
				goto IL_1E3;
			}
			if (num == this.m_char_buffer.Length)
			{
				this.ResizeInternalArray<int>(ref this.m_char_buffer);
			}
			this.m_char_buffer[num] = 0;
			this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		public void SetCharArray(char[] sourceText, int start, int length)
		{
			if (sourceText == null || sourceText.Length == 0 || length == 0)
			{
				return;
			}
			if (this.m_char_buffer == null)
			{
				this.m_char_buffer = new int[8];
			}
			this.m_styleStack.Clear();
			int num = 0;
			int i = start;
			int num2 = start + length;
			while (i < num2)
			{
				if (sourceText[i] != '\\' || i >= length - 1)
				{
					goto IL_10D;
				}
				int num3 = (int)sourceText[i + 1];
				if (num3 != 110)
				{
					if (num3 != 114)
					{
						if (num3 != 116)
						{
							goto IL_10D;
						}
						if (num == this.m_char_buffer.Length)
						{
							this.ResizeInternalArray<int>(ref this.m_char_buffer);
						}
						this.m_char_buffer[num] = 9;
						i++;
						num++;
					}
					else
					{
						if (num == this.m_char_buffer.Length)
						{
							this.ResizeInternalArray<int>(ref this.m_char_buffer);
						}
						this.m_char_buffer[num] = 13;
						i++;
						num++;
					}
				}
				else
				{
					if (num == this.m_char_buffer.Length)
					{
						this.ResizeInternalArray<int>(ref this.m_char_buffer);
					}
					this.m_char_buffer[num] = 10;
					i++;
					num++;
				}
				IL_1ED:
				i++;
				continue;
				IL_10D:
				if (sourceText[i] == '<')
				{
					if (this.IsTagName(ref sourceText, "<BR>", i))
					{
						if (num == this.m_char_buffer.Length)
						{
							this.ResizeInternalArray<int>(ref this.m_char_buffer);
						}
						this.m_char_buffer[num] = 10;
						num++;
						i += 3;
						goto IL_1ED;
					}
					if (this.IsTagName(ref sourceText, "<STYLE=", i))
					{
						int num4 = 0;
						if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num4, ref this.m_char_buffer, ref num))
						{
							i = num4;
							goto IL_1ED;
						}
					}
					else if (this.IsTagName(ref sourceText, "</STYLE>", i))
					{
						this.ReplaceClosingStyleTag(ref sourceText, i, ref this.m_char_buffer, ref num);
						i += 7;
						goto IL_1ED;
					}
				}
				if (num == this.m_char_buffer.Length)
				{
					this.ResizeInternalArray<int>(ref this.m_char_buffer);
				}
				this.m_char_buffer[num] = (int)sourceText[i];
				num++;
				goto IL_1ED;
			}
			if (num == this.m_char_buffer.Length)
			{
				this.ResizeInternalArray<int>(ref this.m_char_buffer);
			}
			this.m_char_buffer[num] = 0;
			this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
			this.m_havePropertiesChanged = true;
			this.m_isInputParsingRequired = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		public void SetCharArray(int[] sourceText, int start, int length)
		{
			if (sourceText == null || sourceText.Length == 0 || length == 0)
			{
				return;
			}
			if (this.m_char_buffer == null)
			{
				this.m_char_buffer = new int[8];
			}
			this.m_styleStack.Clear();
			int num = 0;
			int i = start;
			int num2 = start + length;
			while (i < num2)
			{
				if (sourceText[i] != 92 || i >= length - 1)
				{
					goto IL_10D;
				}
				int num3 = sourceText[i + 1];
				if (num3 != 110)
				{
					if (num3 != 114)
					{
						if (num3 != 116)
						{
							goto IL_10D;
						}
						if (num == this.m_char_buffer.Length)
						{
							this.ResizeInternalArray<int>(ref this.m_char_buffer);
						}
						this.m_char_buffer[num] = 9;
						i++;
						num++;
					}
					else
					{
						if (num == this.m_char_buffer.Length)
						{
							this.ResizeInternalArray<int>(ref this.m_char_buffer);
						}
						this.m_char_buffer[num] = 13;
						i++;
						num++;
					}
				}
				else
				{
					if (num == this.m_char_buffer.Length)
					{
						this.ResizeInternalArray<int>(ref this.m_char_buffer);
					}
					this.m_char_buffer[num] = 10;
					i++;
					num++;
				}
				IL_1ED:
				i++;
				continue;
				IL_10D:
				if (sourceText[i] == 60)
				{
					if (this.IsTagName(ref sourceText, "<BR>", i))
					{
						if (num == this.m_char_buffer.Length)
						{
							this.ResizeInternalArray<int>(ref this.m_char_buffer);
						}
						this.m_char_buffer[num] = 10;
						num++;
						i += 3;
						goto IL_1ED;
					}
					if (this.IsTagName(ref sourceText, "<STYLE=", i))
					{
						int num4 = 0;
						if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num4, ref this.m_char_buffer, ref num))
						{
							i = num4;
							goto IL_1ED;
						}
					}
					else if (this.IsTagName(ref sourceText, "</STYLE>", i))
					{
						this.ReplaceClosingStyleTag(ref sourceText, i, ref this.m_char_buffer, ref num);
						i += 7;
						goto IL_1ED;
					}
				}
				if (num == this.m_char_buffer.Length)
				{
					this.ResizeInternalArray<int>(ref this.m_char_buffer);
				}
				this.m_char_buffer[num] = sourceText[i];
				num++;
				goto IL_1ED;
			}
			if (num == this.m_char_buffer.Length)
			{
				this.ResizeInternalArray<int>(ref this.m_char_buffer);
			}
			this.m_char_buffer[num] = 0;
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
			if (charBuffer == null)
			{
				charBuffer = new int[8];
			}
			this.m_styleStack.Clear();
			int num = 0;
			for (int i = 0; i < this.m_charArray_Length; i++)
			{
				if (char.IsHighSurrogate(sourceText[i]) && char.IsLowSurrogate(sourceText[i + 1]))
				{
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
					i++;
					num++;
				}
				else
				{
					if (sourceText[i] == '<')
					{
						if (this.IsTagName(ref sourceText, "<BR>", i))
						{
							if (num == charBuffer.Length)
							{
								this.ResizeInternalArray<int>(ref charBuffer);
							}
							charBuffer[num] = 10;
							num++;
							i += 3;
							goto IL_13A;
						}
						if (this.IsTagName(ref sourceText, "<STYLE=", i))
						{
							int num2 = 0;
							if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num2, ref charBuffer, ref num))
							{
								i = num2;
								goto IL_13A;
							}
						}
						else if (this.IsTagName(ref sourceText, "</STYLE>", i))
						{
							this.ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref num);
							i += 7;
							goto IL_13A;
						}
					}
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = (int)sourceText[i];
					num++;
				}
				IL_13A:;
			}
			if (num == charBuffer.Length)
			{
				this.ResizeInternalArray<int>(ref charBuffer);
			}
			charBuffer[num] = 0;
		}

		protected void StringToCharArray(string sourceText, ref int[] charBuffer)
		{
			if (sourceText == null)
			{
				charBuffer[0] = 0;
				return;
			}
			if (charBuffer == null)
			{
				charBuffer = new int[8];
			}
			this.m_styleStack.SetDefault(0);
			int num = 0;
			int i = 0;
			while (i < sourceText.Length)
			{
				if (this.m_inputSource != TMP_Text.TextInputSources.Text || sourceText[i] != '\\' || sourceText.Length <= i + 1)
				{
					goto IL_211;
				}
				int num2 = (int)sourceText[i + 1];
				switch (num2)
				{
				case 114:
					if (!this.m_parseCtrlCharacters)
					{
						goto IL_211;
					}
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = 13;
					i++;
					num++;
					break;
				default:
					if (num2 != 85)
					{
						if (num2 != 92)
						{
							if (num2 != 110)
							{
								goto IL_211;
							}
							if (!this.m_parseCtrlCharacters)
							{
								goto IL_211;
							}
							if (num == charBuffer.Length)
							{
								this.ResizeInternalArray<int>(ref charBuffer);
							}
							charBuffer[num] = 10;
							i++;
							num++;
						}
						else
						{
							if (!this.m_parseCtrlCharacters)
							{
								goto IL_211;
							}
							if (sourceText.Length <= i + 2)
							{
								goto IL_211;
							}
							if (num + 2 > charBuffer.Length)
							{
								this.ResizeInternalArray<int>(ref charBuffer);
							}
							charBuffer[num] = (int)sourceText[i + 1];
							charBuffer[num + 1] = (int)sourceText[i + 2];
							i += 2;
							num += 2;
						}
					}
					else
					{
						if (sourceText.Length <= i + 9)
						{
							goto IL_211;
						}
						if (num == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[num] = this.GetUTF32(i + 2);
						i += 9;
						num++;
					}
					break;
				case 116:
					if (!this.m_parseCtrlCharacters)
					{
						goto IL_211;
					}
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = 9;
					i++;
					num++;
					break;
				case 117:
					if (sourceText.Length <= i + 5)
					{
						goto IL_211;
					}
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = (int)((ushort)this.GetUTF16(i + 2));
					i += 5;
					num++;
					break;
				}
				IL_339:
				i++;
				continue;
				IL_211:
				if (char.IsHighSurrogate(sourceText[i]) && char.IsLowSurrogate(sourceText[i + 1]))
				{
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
					i++;
					num++;
					goto IL_339;
				}
				if (sourceText[i] == '<' && this.m_isRichText)
				{
					if (this.IsTagName(ref sourceText, "<BR>", i))
					{
						if (num == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[num] = 10;
						num++;
						i += 3;
						goto IL_339;
					}
					if (this.IsTagName(ref sourceText, "<STYLE=", i))
					{
						int num3 = 0;
						if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num3, ref charBuffer, ref num))
						{
							i = num3;
							goto IL_339;
						}
					}
					else if (this.IsTagName(ref sourceText, "</STYLE>", i))
					{
						this.ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref num);
						i += 7;
						goto IL_339;
					}
				}
				if (num == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[num] = (int)sourceText[i];
				num++;
				goto IL_339;
			}
			if (num == charBuffer.Length)
			{
				this.ResizeInternalArray<int>(ref charBuffer);
			}
			charBuffer[num] = 0;
		}

		protected void StringBuilderToIntArray(StringBuilder sourceText, ref int[] charBuffer)
		{
			if (sourceText == null)
			{
				charBuffer[0] = 0;
				return;
			}
			if (charBuffer == null)
			{
				charBuffer = new int[8];
			}
			this.m_styleStack.Clear();
			int num = 0;
			int i = 0;
			while (i < sourceText.Length)
			{
				if (!this.m_parseCtrlCharacters || sourceText[i] != '\\' || sourceText.Length <= i + 1)
				{
					goto IL_1D0;
				}
				int num2 = (int)sourceText[i + 1];
				switch (num2)
				{
				case 114:
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = 13;
					i++;
					num++;
					break;
				default:
					if (num2 != 85)
					{
						if (num2 != 92)
						{
							if (num2 != 110)
							{
								goto IL_1D0;
							}
							if (num == charBuffer.Length)
							{
								this.ResizeInternalArray<int>(ref charBuffer);
							}
							charBuffer[num] = 10;
							i++;
							num++;
						}
						else
						{
							if (sourceText.Length <= i + 2)
							{
								goto IL_1D0;
							}
							if (num + 2 > charBuffer.Length)
							{
								this.ResizeInternalArray<int>(ref charBuffer);
							}
							charBuffer[num] = (int)sourceText[i + 1];
							charBuffer[num + 1] = (int)sourceText[i + 2];
							i += 2;
							num += 2;
						}
					}
					else
					{
						if (sourceText.Length <= i + 9)
						{
							goto IL_1D0;
						}
						if (num == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[num] = this.GetUTF32(i + 2);
						i += 9;
						num++;
					}
					break;
				case 116:
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = 9;
					i++;
					num++;
					break;
				case 117:
					if (sourceText.Length <= i + 5)
					{
						goto IL_1D0;
					}
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = (int)((ushort)this.GetUTF16(i + 2));
					i += 5;
					num++;
					break;
				}
				IL_2ED:
				i++;
				continue;
				IL_1D0:
				if (char.IsHighSurrogate(sourceText[i]) && char.IsLowSurrogate(sourceText[i + 1]))
				{
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
					i++;
					num++;
					goto IL_2ED;
				}
				if (sourceText[i] == '<')
				{
					if (this.IsTagName(ref sourceText, "<BR>", i))
					{
						if (num == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[num] = 10;
						num++;
						i += 3;
						goto IL_2ED;
					}
					if (this.IsTagName(ref sourceText, "<STYLE=", i))
					{
						int num3 = 0;
						if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num3, ref charBuffer, ref num))
						{
							i = num3;
							goto IL_2ED;
						}
					}
					else if (this.IsTagName(ref sourceText, "</STYLE>", i))
					{
						this.ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref num);
						i += 7;
						goto IL_2ED;
					}
				}
				if (num == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[num] = (int)sourceText[i];
				num++;
				goto IL_2ED;
			}
			if (num == charBuffer.Length)
			{
				this.ResizeInternalArray<int>(ref charBuffer);
			}
			charBuffer[num] = 0;
		}

		private bool ReplaceOpeningStyleTag(ref string sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			int tagHashCode = this.GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset);
			TMP_Style style = TMP_StyleSheet.GetStyle(tagHashCode);
			if (style == null || srcOffset == 0)
			{
				return false;
			}
			this.m_styleStack.Add(style.hashCode);
			int num = style.styleOpeningTagArray.Length;
			int[] styleOpeningTagArray = style.styleOpeningTagArray;
			int i = 0;
			while (i < num)
			{
				int num2 = styleOpeningTagArray[i];
				if (num2 != 60)
				{
					goto IL_107;
				}
				if (this.IsTagName(ref styleOpeningTagArray, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleOpeningTagArray, "<STYLE=", i))
				{
					int num3 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleOpeningTagArray, i, out num3, ref charBuffer, ref writeIndex))
					{
						goto IL_107;
					}
					i = num3;
				}
				else
				{
					if (!this.IsTagName(ref styleOpeningTagArray, "</STYLE>", i))
					{
						goto IL_107;
					}
					this.ReplaceClosingStyleTag(ref styleOpeningTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_12D:
				i++;
				continue;
				IL_107:
				if (writeIndex == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
				goto IL_12D;
			}
			return true;
		}

		private bool ReplaceOpeningStyleTag(ref int[] sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			int tagHashCode = this.GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset);
			TMP_Style style = TMP_StyleSheet.GetStyle(tagHashCode);
			if (style == null || srcOffset == 0)
			{
				return false;
			}
			this.m_styleStack.Add(style.hashCode);
			int num = style.styleOpeningTagArray.Length;
			int[] styleOpeningTagArray = style.styleOpeningTagArray;
			int i = 0;
			while (i < num)
			{
				int num2 = styleOpeningTagArray[i];
				if (num2 != 60)
				{
					goto IL_107;
				}
				if (this.IsTagName(ref styleOpeningTagArray, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleOpeningTagArray, "<STYLE=", i))
				{
					int num3 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleOpeningTagArray, i, out num3, ref charBuffer, ref writeIndex))
					{
						goto IL_107;
					}
					i = num3;
				}
				else
				{
					if (!this.IsTagName(ref styleOpeningTagArray, "</STYLE>", i))
					{
						goto IL_107;
					}
					this.ReplaceClosingStyleTag(ref styleOpeningTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_12D:
				i++;
				continue;
				IL_107:
				if (writeIndex == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
				goto IL_12D;
			}
			return true;
		}

		private bool ReplaceOpeningStyleTag(ref char[] sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			int tagHashCode = this.GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset);
			TMP_Style style = TMP_StyleSheet.GetStyle(tagHashCode);
			if (style == null || srcOffset == 0)
			{
				return false;
			}
			this.m_styleStack.Add(style.hashCode);
			int num = style.styleOpeningTagArray.Length;
			int[] styleOpeningTagArray = style.styleOpeningTagArray;
			int i = 0;
			while (i < num)
			{
				int num2 = styleOpeningTagArray[i];
				if (num2 != 60)
				{
					goto IL_107;
				}
				if (this.IsTagName(ref styleOpeningTagArray, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleOpeningTagArray, "<STYLE=", i))
				{
					int num3 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleOpeningTagArray, i, out num3, ref charBuffer, ref writeIndex))
					{
						goto IL_107;
					}
					i = num3;
				}
				else
				{
					if (!this.IsTagName(ref styleOpeningTagArray, "</STYLE>", i))
					{
						goto IL_107;
					}
					this.ReplaceClosingStyleTag(ref styleOpeningTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_12D:
				i++;
				continue;
				IL_107:
				if (writeIndex == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
				goto IL_12D;
			}
			return true;
		}

		private bool ReplaceOpeningStyleTag(ref StringBuilder sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			int tagHashCode = this.GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset);
			TMP_Style style = TMP_StyleSheet.GetStyle(tagHashCode);
			if (style == null || srcOffset == 0)
			{
				return false;
			}
			this.m_styleStack.Add(style.hashCode);
			int num = style.styleOpeningTagArray.Length;
			int[] styleOpeningTagArray = style.styleOpeningTagArray;
			int i = 0;
			while (i < num)
			{
				int num2 = styleOpeningTagArray[i];
				if (num2 != 60)
				{
					goto IL_107;
				}
				if (this.IsTagName(ref styleOpeningTagArray, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleOpeningTagArray, "<STYLE=", i))
				{
					int num3 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleOpeningTagArray, i, out num3, ref charBuffer, ref writeIndex))
					{
						goto IL_107;
					}
					i = num3;
				}
				else
				{
					if (!this.IsTagName(ref styleOpeningTagArray, "</STYLE>", i))
					{
						goto IL_107;
					}
					this.ReplaceClosingStyleTag(ref styleOpeningTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_12D:
				i++;
				continue;
				IL_107:
				if (writeIndex == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
				goto IL_12D;
			}
			return true;
		}

		private bool ReplaceClosingStyleTag(ref string sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			int hashCode = this.m_styleStack.CurrentItem();
			TMP_Style style = TMP_StyleSheet.GetStyle(hashCode);
			this.m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int[] styleClosingTagArray = style.styleClosingTagArray;
			int i = 0;
			while (i < num)
			{
				int num2 = styleClosingTagArray[i];
				if (num2 != 60)
				{
					goto IL_F6;
				}
				if (this.IsTagName(ref styleClosingTagArray, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleClosingTagArray, "<STYLE=", i))
				{
					int num3 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleClosingTagArray, i, out num3, ref charBuffer, ref writeIndex))
					{
						goto IL_F6;
					}
					i = num3;
				}
				else
				{
					if (!this.IsTagName(ref styleClosingTagArray, "</STYLE>", i))
					{
						goto IL_F6;
					}
					this.ReplaceClosingStyleTag(ref styleClosingTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_119:
				i++;
				continue;
				IL_F6:
				if (writeIndex == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
				goto IL_119;
			}
			return true;
		}

		private bool ReplaceClosingStyleTag(ref int[] sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			int hashCode = this.m_styleStack.CurrentItem();
			TMP_Style style = TMP_StyleSheet.GetStyle(hashCode);
			this.m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int[] styleClosingTagArray = style.styleClosingTagArray;
			int i = 0;
			while (i < num)
			{
				int num2 = styleClosingTagArray[i];
				if (num2 != 60)
				{
					goto IL_F6;
				}
				if (this.IsTagName(ref styleClosingTagArray, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleClosingTagArray, "<STYLE=", i))
				{
					int num3 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleClosingTagArray, i, out num3, ref charBuffer, ref writeIndex))
					{
						goto IL_F6;
					}
					i = num3;
				}
				else
				{
					if (!this.IsTagName(ref styleClosingTagArray, "</STYLE>", i))
					{
						goto IL_F6;
					}
					this.ReplaceClosingStyleTag(ref styleClosingTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_119:
				i++;
				continue;
				IL_F6:
				if (writeIndex == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
				goto IL_119;
			}
			return true;
		}

		private bool ReplaceClosingStyleTag(ref char[] sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			int hashCode = this.m_styleStack.CurrentItem();
			TMP_Style style = TMP_StyleSheet.GetStyle(hashCode);
			this.m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int[] styleClosingTagArray = style.styleClosingTagArray;
			int i = 0;
			while (i < num)
			{
				int num2 = styleClosingTagArray[i];
				if (num2 != 60)
				{
					goto IL_F6;
				}
				if (this.IsTagName(ref styleClosingTagArray, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleClosingTagArray, "<STYLE=", i))
				{
					int num3 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleClosingTagArray, i, out num3, ref charBuffer, ref writeIndex))
					{
						goto IL_F6;
					}
					i = num3;
				}
				else
				{
					if (!this.IsTagName(ref styleClosingTagArray, "</STYLE>", i))
					{
						goto IL_F6;
					}
					this.ReplaceClosingStyleTag(ref styleClosingTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_119:
				i++;
				continue;
				IL_F6:
				if (writeIndex == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
				goto IL_119;
			}
			return true;
		}

		private bool ReplaceClosingStyleTag(ref StringBuilder sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			int hashCode = this.m_styleStack.CurrentItem();
			TMP_Style style = TMP_StyleSheet.GetStyle(hashCode);
			this.m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int[] styleClosingTagArray = style.styleClosingTagArray;
			int i = 0;
			while (i < num)
			{
				int num2 = styleClosingTagArray[i];
				if (num2 != 60)
				{
					goto IL_F6;
				}
				if (this.IsTagName(ref styleClosingTagArray, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleClosingTagArray, "<STYLE=", i))
				{
					int num3 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleClosingTagArray, i, out num3, ref charBuffer, ref writeIndex))
					{
						goto IL_F6;
					}
					i = num3;
				}
				else
				{
					if (!this.IsTagName(ref styleClosingTagArray, "</STYLE>", i))
					{
						goto IL_F6;
					}
					this.ReplaceClosingStyleTag(ref styleClosingTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_119:
				i++;
				continue;
				IL_F6:
				if (writeIndex == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
				goto IL_119;
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

		private void ResizeInternalArray<T>(ref T[] array)
		{
			int newSize = Mathf.NextPowerOfTwo(array.Length + 1);
			Array.Resize<T>(ref array, newSize);
		}

		protected void AddFloatToCharArray(float number, ref int index, int precision)
		{
			if (number < 0f)
			{
				this.m_input_CharArray[index++] = '-';
				number = -number;
			}
			number += this.k_Power[Mathf.Min(9, precision)];
			int num = (int)number;
			this.AddIntToCharArray(num, ref index, precision);
			if (precision > 0)
			{
				this.m_input_CharArray[index++] = '.';
				number -= (float)num;
				for (int i = 0; i < precision; i++)
				{
					number *= 10f;
					int num2 = (int)number;
					this.m_input_CharArray[index++] = (char)(num2 + 48);
					number -= (float)num2;
				}
			}
		}

		protected void AddIntToCharArray(int number, ref int index, int precision)
		{
			if (number < 0)
			{
				this.m_input_CharArray[index++] = '-';
				number = -number;
			}
			int num = index;
			do
			{
				this.m_input_CharArray[num++] = (char)(number % 10 + 48);
				number /= 10;
			}
			while (number > 0);
			int num2 = num;
			while (index + 1 < num)
			{
				num--;
				char c = this.m_input_CharArray[index];
				this.m_input_CharArray[index] = this.m_input_CharArray[num];
				this.m_input_CharArray[num] = c;
				index++;
			}
			index = num2;
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
			float defaultFontSize = (!this.m_enableAutoSizing) ? this.m_fontSize : this.m_fontSizeMax;
			this.m_minFontSize = this.m_fontSizeMin;
			this.m_maxFontSize = this.m_fontSizeMax;
			this.m_charWidthAdjDelta = 0f;
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
			float defaultFontSize = (!this.m_enableAutoSizing) ? this.m_fontSize : this.m_fontSizeMax;
			this.m_minFontSize = this.m_fontSizeMin;
			this.m_maxFontSize = this.m_fontSizeMax;
			this.m_charWidthAdjDelta = 0f;
			this.m_recursiveCount = 0;
			return this.CalculatePreferredValues(defaultFontSize, margin, true).x;
		}

		protected float GetPreferredHeight()
		{
			float defaultFontSize = (!this.m_enableAutoSizing) ? this.m_fontSize : this.m_fontSizeMax;
			this.m_minFontSize = this.m_fontSizeMin;
			this.m_maxFontSize = this.m_fontSizeMax;
			this.m_charWidthAdjDelta = 0f;
			Vector2 marginSize = new Vector2((this.m_marginWidth == 0f) ? TMP_Text.k_LargePositiveFloat : this.m_marginWidth, TMP_Text.k_LargePositiveFloat);
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
			float defaultFontSize = (!this.m_enableAutoSizing) ? this.m_fontSize : this.m_fontSizeMax;
			this.m_minFontSize = this.m_fontSizeMin;
			this.m_maxFontSize = this.m_fontSizeMax;
			this.m_charWidthAdjDelta = 0f;
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
				this.m_internalCharacterInfo = new TMP_CharacterInfo[(totalCharacterCount <= 1024) ? Mathf.NextPowerOfTwo(totalCharacterCount) : (totalCharacterCount + 256)];
			}
			float num = this.m_fontScale = defaultFontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
			float num2 = num;
			this.m_fontScaleMultiplier = 1f;
			this.m_currentFontSize = defaultFontSize;
			this.m_sizeStack.SetDefault(this.m_currentFontSize);
			this.m_style = this.m_fontStyle;
			this.m_lineJustification = this.m_textAlignment;
			this.m_lineJustificationStack.SetDefault(this.m_lineJustification);
			this.m_baselineOffset = 0f;
			this.m_baselineOffsetStack.Clear();
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
					goto IL_3DB;
				}
				this.m_isParsingText = true;
				this.m_textElementType = TMP_TextElementType.Character;
				if (!this.ValidateHtmlTag(this.m_char_buffer, num9 + 1, out num8))
				{
					goto IL_3DB;
				}
				num9 = num8;
				if (this.m_textElementType != TMP_TextElementType.Character)
				{
					goto IL_3DB;
				}
				IL_153E:
				num9++;
				continue;
				IL_3DB:
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
						goto IL_153E;
					}
					if (num10 == 60)
					{
						num10 = 57344 + this.m_spriteIndex;
					}
					this.m_currentFontAsset = this.m_fontAsset;
					float num12 = this.m_currentFontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
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
						goto IL_153E;
					}
					this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
					this.m_fontScale = this.m_currentFontSize * num11 / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
					num2 = this.m_fontScale * this.m_fontScaleMultiplier * this.m_cached_TextElement.scale;
					this.m_internalCharacterInfo[this.m_characterCount].elementType = TMP_TextElementType.Character;
				}
				float num13 = num2;
				if (num10 == 173)
				{
					num2 = 0f;
				}
				this.m_internalCharacterInfo[this.m_characterCount].character = (char)num10;
				GlyphValueRecord a2 = default(GlyphValueRecord);
				if (this.m_enableKerning)
				{
					KerningPair kerningPair = null;
					if (this.m_characterCount < totalCharacterCount - 1)
					{
						uint character = (uint)this.m_textInfo.characterInfo[this.m_characterCount + 1].character;
						KerningPairKey kerningPairKey = new KerningPairKey((uint)num10, character);
						this.m_currentFontAsset.kerningDictionary.TryGetValue((int)kerningPairKey.key, out kerningPair);
						if (kerningPair != null)
						{
							a2 = kerningPair.firstGlyphAdjustments;
						}
					}
					if (this.m_characterCount >= 1)
					{
						uint character2 = (uint)this.m_textInfo.characterInfo[this.m_characterCount - 1].character;
						KerningPairKey kerningPairKey2 = new KerningPairKey(character2, (uint)num10);
						this.m_currentFontAsset.kerningDictionary.TryGetValue((int)kerningPairKey2.key, out kerningPair);
						if (kerningPair != null)
						{
							a2 += kerningPair.secondGlyphAdjustments;
						}
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
				float num16 = this.m_currentFontAsset.fontInfo.Ascender * ((this.m_textElementType != TMP_TextElementType.Character) ? this.m_internalCharacterInfo[this.m_characterCount].scale : (num2 / num11)) + this.m_baselineOffset;
				this.m_internalCharacterInfo[this.m_characterCount].ascender = num16 - this.m_lineOffset;
				this.m_maxLineAscender = ((num16 <= this.m_maxLineAscender) ? this.m_maxLineAscender : num16);
				float num17 = this.m_currentFontAsset.fontInfo.Descender * ((this.m_textElementType != TMP_TextElementType.Character) ? this.m_internalCharacterInfo[this.m_characterCount].scale : (num2 / num11)) + this.m_baselineOffset;
				float num18 = this.m_internalCharacterInfo[this.m_characterCount].descender = num17 - this.m_lineOffset;
				this.m_maxLineDescender = ((num17 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num17);
				if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript || (this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
				{
					float num19 = (num16 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
					num16 = this.m_maxLineAscender;
					this.m_maxLineAscender = ((num19 <= this.m_maxLineAscender) ? this.m_maxLineAscender : num19);
					float num20 = (num17 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
					num17 = this.m_maxLineDescender;
					this.m_maxLineDescender = ((num20 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num20);
				}
				if (this.m_lineNumber == 0)
				{
					this.m_maxAscender = ((this.m_maxAscender <= num16) ? num16 : this.m_maxAscender);
				}
				if (num10 == 9 || (!char.IsWhiteSpace((char)num10) && num10 != 8203) || this.m_textElementType == TMP_TextElementType.Sprite)
				{
					float num21 = (this.m_width == -1f) ? (x + 0.0001f - this.m_marginLeft - this.m_marginRight) : Mathf.Min(x + 0.0001f - this.m_marginLeft - this.m_marginRight, this.m_width);
					bool flag3 = (this.m_lineJustification & (TextAlignmentOptions)16) == (TextAlignmentOptions)16 || (this.m_lineJustification & (TextAlignmentOptions)8) == (TextAlignmentOptions)8;
					num6 = this.m_xAdvance + this.m_cached_TextElement.xAdvance * (1f - this.m_charWidthAdjDelta) * ((num10 == 173) ? num13 : num2);
					if (num6 > num21 * ((!flag3) ? 1f : 1.05f))
					{
						if (this.enableWordWrapping && this.m_characterCount != this.m_firstCharacterOfLine)
						{
							if (num7 == wordWrapState2.previous_WordBreak || flag)
							{
								if (!ignoreTextAutoSizing && this.m_currentFontSize > this.m_fontSizeMin)
								{
									if (this.m_charWidthAdjDelta < this.m_charWidthMaxAdj / 100f)
									{
										this.m_recursiveCount = 0;
										this.m_charWidthAdjDelta += 0.01f;
										return this.CalculatePreferredValues(defaultFontSize, marginSize, false);
									}
									this.m_maxFontSize = defaultFontSize;
									defaultFontSize -= Mathf.Max((defaultFontSize - this.m_minFontSize) / 2f, 0.05f);
									defaultFontSize = (float)((int)(Mathf.Max(defaultFontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
									if (this.m_recursiveCount > 20)
									{
										return new Vector2(num4, num5);
									}
									return this.CalculatePreferredValues(defaultFontSize, marginSize, false);
								}
								else if (!this.m_isCharacterWrappingEnabled)
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
								return this.CalculatePreferredValues(defaultFontSize, marginSize, true);
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
							this.m_maxDescender = ((this.m_maxDescender >= num24) ? num24 : this.m_maxDescender);
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
							this.m_xAdvance = this.tag_Indent;
							goto IL_153E;
						}
						else if (!ignoreTextAutoSizing && defaultFontSize > this.m_fontSizeMin)
						{
							if (this.m_charWidthAdjDelta < this.m_charWidthMaxAdj / 100f)
							{
								this.m_recursiveCount = 0;
								this.m_charWidthAdjDelta += 0.01f;
								return this.CalculatePreferredValues(defaultFontSize, marginSize, false);
							}
							this.m_maxFontSize = defaultFontSize;
							defaultFontSize -= Mathf.Max((defaultFontSize - this.m_minFontSize) / 2f, 0.05f);
							defaultFontSize = (float)((int)(Mathf.Max(defaultFontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
							if (this.m_recursiveCount > 20)
							{
								return new Vector2(num4, num5);
							}
							return this.CalculatePreferredValues(defaultFontSize, marginSize, false);
						}
					}
				}
				if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == -32767f && !this.m_isNewPage)
				{
					float num27 = this.m_maxLineAscender - this.m_startOfLineAscender;
					num18 -= num27;
					this.m_lineOffset += num27;
					this.m_startOfLineAscender += num27;
					wordWrapState2.lineOffset = this.m_lineOffset;
					wordWrapState2.previousLineAscender = this.m_startOfLineAscender;
				}
				if (num10 == 9)
				{
					float num28 = this.m_currentFontAsset.fontInfo.TabWidth * num2;
					float num29 = Mathf.Ceil(this.m_xAdvance / num28) * num28;
					this.m_xAdvance = ((num29 <= this.m_xAdvance) ? (this.m_xAdvance + num28) : num29);
				}
				else if (this.m_monoSpacing != 0f)
				{
					this.m_xAdvance += (this.m_monoSpacing - num14 + (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
					if (char.IsWhiteSpace((char)num10) || num10 == 8203)
					{
						this.m_xAdvance += this.m_wordSpacing * num2;
					}
				}
				else
				{
					this.m_xAdvance += ((this.m_cached_TextElement.xAdvance * num15 + this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset + a2.xAdvance) * num2 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
					if (char.IsWhiteSpace((char)num10) || num10 == 8203)
					{
						this.m_xAdvance += this.m_wordSpacing * num2;
					}
				}
				if (num10 == 13)
				{
					a = Mathf.Max(a, num4 + this.m_xAdvance);
					num4 = 0f;
					this.m_xAdvance = this.tag_Indent;
				}
				if (num10 == 10 || this.m_characterCount == totalCharacterCount - 1)
				{
					if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == -32767f)
					{
						float num30 = this.m_maxLineAscender - this.m_startOfLineAscender;
						num18 -= num30;
						this.m_lineOffset += num30;
					}
					float num31 = this.m_maxLineDescender - this.m_lineOffset;
					this.m_maxDescender = ((this.m_maxDescender >= num31) ? num31 : this.m_maxDescender);
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
						this.m_xAdvance = this.tag_LineIndent + this.tag_Indent;
						this.m_characterCount++;
						goto IL_153E;
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
				goto IL_153E;
			}
			float num32 = this.m_maxFontSize - this.m_minFontSize;
			if (this.m_isCharacterWrappingEnabled || ignoreTextAutoSizing || num32 <= 0.051f || defaultFontSize >= this.m_fontSizeMax)
			{
				this.m_isCharacterWrappingEnabled = false;
				this.m_isCalculatingPreferredValues = false;
				num4 += ((this.m_margin.x <= 0f) ? 0f : this.m_margin.x);
				num4 += ((this.m_margin.z <= 0f) ? 0f : this.m_margin.z);
				num5 += ((this.m_margin.y <= 0f) ? 0f : this.m_margin.y);
				num5 += ((this.m_margin.w <= 0f) ? 0f : this.m_margin.w);
				num4 = (float)((int)(num4 * 100f + 1f)) / 100f;
				num5 = (float)((int)(num5 * 100f + 1f)) / 100f;
				return new Vector2(num4, num5);
			}
			this.m_minFontSize = defaultFontSize;
			defaultFontSize += Mathf.Max((this.m_maxFontSize - defaultFontSize) / 2f, 0.05f);
			defaultFontSize = (float)((int)(Mathf.Min(defaultFontSize, this.m_fontSizeMax) * 20f + 0.5f)) / 20f;
			if (this.m_recursiveCount > 20)
			{
				return new Vector2(num4, num5);
			}
			return this.CalculatePreferredValues(defaultFontSize, marginSize, false);
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
			Vector3 center = (extents.min + extents.max) / 2f;
			return new Bounds(center, v);
		}

		protected Bounds GetTextBounds(bool onlyVisibleCharacters)
		{
			if (this.m_textInfo == null)
			{
				return default(Bounds);
			}
			Extents extents = new Extents(TMP_Text.k_LargePositiveVector2, TMP_Text.k_LargeNegativeVector2);
			for (int i = 0; i < this.m_textInfo.characterCount; i++)
			{
				if ((i > this.maxVisibleCharacters || this.m_textInfo.characterInfo[i].lineNumber > this.m_maxVisibleLines) && onlyVisibleCharacters)
				{
					break;
				}
				if (!onlyVisibleCharacters || this.m_textInfo.characterInfo[i].isVisible)
				{
					extents.min.x = Mathf.Min(extents.min.x, this.m_textInfo.characterInfo[i].origin);
					extents.min.y = Mathf.Min(extents.min.y, this.m_textInfo.characterInfo[i].descender);
					extents.max.x = Mathf.Max(extents.max.x, this.m_textInfo.characterInfo[i].xAdvance);
					extents.max.y = Mathf.Max(extents.max.y, this.m_textInfo.characterInfo[i].ascender);
				}
			}
			Vector2 v;
			v.x = extents.max.x - extents.min.x;
			v.y = extents.max.y - extents.min.y;
			Vector2 v2 = (extents.min + extents.max) / 2f;
			return new Bounds(v2, v);
		}

		protected virtual void AdjustLineOffset(int startIndex, int endIndex, float offset)
		{
		}

		protected void ResizeLineExtents(int size)
		{
			size = ((size <= 1024) ? Mathf.NextPowerOfTwo(size + 1) : (size + 256));
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
			state.strikethroughColor = this.m_strikethroughColor;
			state.highlightColor = this.m_highlightColor;
			state.isNonBreakingSpace = this.m_isNonBreakingSpace;
			state.tagNoParsing = this.tag_NoParsing;
			state.basicStyleStack = this.m_fontStyleStack;
			state.colorStack = this.m_colorStack;
			state.underlineColorStack = this.m_underlineColorStack;
			state.strikethroughColorStack = this.m_strikethroughColorStack;
			state.highlightColorStack = this.m_highlightColorStack;
			state.colorGradientStack = this.m_colorGradientStack;
			state.sizeStack = this.m_sizeStack;
			state.indentStack = this.m_indentStack;
			state.fontWeightStack = this.m_fontWeightStack;
			state.styleStack = this.m_styleStack;
			state.baselineStack = this.m_baselineOffsetStack;
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
			this.m_strikethroughColor = state.strikethroughColor;
			this.m_highlightColor = state.highlightColor;
			this.m_isNonBreakingSpace = state.isNonBreakingSpace;
			this.tag_NoParsing = state.tagNoParsing;
			this.m_fontStyleStack = state.basicStyleStack;
			this.m_colorStack = state.colorStack;
			this.m_underlineColorStack = state.underlineColorStack;
			this.m_strikethroughColorStack = state.strikethroughColorStack;
			this.m_highlightColorStack = state.highlightColorStack;
			this.m_colorGradientStack = state.colorGradientStack;
			this.m_sizeStack = state.sizeStack;
			this.m_indentStack = state.indentStack;
			this.m_fontWeightStack = state.fontWeightStack;
			this.m_styleStack = state.styleStack;
			this.m_baselineOffsetStack = state.baselineStack;
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
			vertexColor.a = ((this.m_fontColor32.a >= vertexColor.a) ? vertexColor.a : this.m_fontColor32.a);
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
			if (this.m_colorGradientPreset != null)
			{
				TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
				int characterCount = this.m_characterCount;
				characterInfo[characterCount].vertex_BL.color = characterInfo[characterCount].vertex_BL.color * this.m_colorGradientPreset.bottomLeft;
				TMP_CharacterInfo[] characterInfo2 = this.m_textInfo.characterInfo;
				int characterCount2 = this.m_characterCount;
				characterInfo2[characterCount2].vertex_TL.color = characterInfo2[characterCount2].vertex_TL.color * this.m_colorGradientPreset.topLeft;
				TMP_CharacterInfo[] characterInfo3 = this.m_textInfo.characterInfo;
				int characterCount3 = this.m_characterCount;
				characterInfo3[characterCount3].vertex_TR.color = characterInfo3[characterCount3].vertex_TR.color * this.m_colorGradientPreset.topRight;
				TMP_CharacterInfo[] characterInfo4 = this.m_textInfo.characterInfo;
				int characterCount4 = this.m_characterCount;
				characterInfo4[characterCount4].vertex_BR.color = characterInfo4[characterCount4].vertex_BR.color * this.m_colorGradientPreset.bottomRight;
			}
			if (!this.m_isSDFShader)
			{
				style_padding = 0f;
			}
			FaceInfo fontInfo = this.m_currentFontAsset.fontInfo;
			Vector2 uv;
			uv.x = (this.m_cached_TextElement.x - padding - style_padding) / fontInfo.AtlasWidth;
			uv.y = 1f - (this.m_cached_TextElement.y + padding + style_padding + this.m_cached_TextElement.height) / fontInfo.AtlasHeight;
			Vector2 uv2;
			uv2.x = uv.x;
			uv2.y = 1f - (this.m_cached_TextElement.y - padding - style_padding) / fontInfo.AtlasHeight;
			Vector2 uv3;
			uv3.x = (this.m_cached_TextElement.x + padding + style_padding + this.m_cached_TextElement.width) / fontInfo.AtlasWidth;
			uv3.y = uv2.y;
			Vector2 uv4;
			uv4.x = uv3.x;
			uv4.y = uv.y;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.uv = uv;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.uv = uv2;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.uv = uv3;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.uv = uv4;
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
			Color32 color = (!this.m_tintSprite) ? this.m_spriteColor : this.m_spriteColor.Multiply(vertexColor);
			color.a = ((color.a >= this.m_fontColor32.a) ? this.m_fontColor32.a : (color.a = ((color.a >= vertexColor.a) ? vertexColor.a : color.a)));
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
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradientPreset.bottomLeft));
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradientPreset.topLeft));
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradientPreset.topRight));
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradientPreset.bottomRight));
			}
			else
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradient.bottomLeft));
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradient.topLeft));
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradient.topRight));
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradient.bottomRight));
			}
			Vector2 uv = new Vector2(this.m_cached_TextElement.x / (float)this.m_currentSpriteAsset.spriteSheet.width, this.m_cached_TextElement.y / (float)this.m_currentSpriteAsset.spriteSheet.height);
			Vector2 uv2 = new Vector2(uv.x, (this.m_cached_TextElement.y + this.m_cached_TextElement.height) / (float)this.m_currentSpriteAsset.spriteSheet.height);
			Vector2 uv3 = new Vector2((this.m_cached_TextElement.x + this.m_cached_TextElement.width) / (float)this.m_currentSpriteAsset.spriteSheet.width, uv2.y);
			Vector2 uv4 = new Vector2(uv3.x, uv.y);
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.uv = uv;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.uv = uv2;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.uv = uv3;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.uv = uv4;
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
			this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + (isVolumetric ? 8 : 4);
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
			highlightColor.a = ((this.m_htmlColor.a >= highlightColor.a) ? highlightColor.a : this.m_htmlColor.a);
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
				else
				{
					this.m_rectTransform = this.rectTransform;
					if (base.GetType() == typeof(TextMeshPro))
					{
						this.m_rectTransform.sizeDelta = TMP_Settings.defaultTextMeshProTextContainerSize;
					}
					else
					{
						this.m_rectTransform.sizeDelta = TMP_Settings.defaultTextMeshProUITextContainerSize;
					}
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
			}
			else if (!this.m_isAlignmentEnumConverted)
			{
				this.m_isAlignmentEnumConverted = true;
				this.m_textAlignment = TMP_Compatibility.ConvertTextAlignmentEnumValues(this.m_textAlignment);
			}
		}

		protected void GetSpecialCharacters(TMP_FontAsset fontAsset)
		{
			if (!fontAsset.characterDictionary.TryGetValue(95, out this.m_cached_Underline_GlyphInfo))
			{
			}
			if (!fontAsset.characterDictionary.TryGetValue(8230, out this.m_cached_Ellipsis_GlyphInfo))
			{
			}
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
			Vector2 result;
			result.x = (float)((int)(x * 511f));
			result.y = (float)((int)(y * 511f));
			result.x = result.x * 4096f + result.y;
			result.y = scale;
			return result;
		}

		protected float PackUV(float x, float y)
		{
			double num = (double)((int)(x * 511f));
			double num2 = (double)((int)(y * 511f));
			return (float)(num * 4096.0 + num2);
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
				default:
					return 15;
				}
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
			}
		}

		protected int GetUTF16(int i)
		{
			int num = this.HexToInt(this.m_text[i]) << 12;
			num += this.HexToInt(this.m_text[i + 1]) << 8;
			num += this.HexToInt(this.m_text[i + 2]) << 4;
			return num + this.HexToInt(this.m_text[i + 3]);
		}

		protected int GetUTF32(int i)
		{
			int num = 0;
			num += this.HexToInt(this.m_text[i]) << 30;
			num += this.HexToInt(this.m_text[i + 1]) << 24;
			num += this.HexToInt(this.m_text[i + 2]) << 20;
			num += this.HexToInt(this.m_text[i + 3]) << 16;
			num += this.HexToInt(this.m_text[i + 4]) << 12;
			num += this.HexToInt(this.m_text[i + 5]) << 8;
			num += this.HexToInt(this.m_text[i + 6]) << 4;
			return num + this.HexToInt(this.m_text[i + 7]);
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
			bool flag = true;
			float num2 = 0f;
			int num3 = 1;
			if (chars[startIndex] == '+')
			{
				num3 = 1;
				startIndex++;
			}
			else if (chars[startIndex] == '-')
			{
				num3 = -1;
				startIndex++;
			}
			float num4 = 0f;
			for (int i = startIndex; i < num; i++)
			{
				uint num5 = (uint)chars[i];
				if ((num5 >= 48u && num5 <= 57u) || num5 == 46u)
				{
					if (num5 == 46u)
					{
						flag = false;
						num2 = 0.1f;
					}
					else if (flag)
					{
						num4 = num4 * 10f + (float)((ulong)(num5 - 48u) * (ulong)((long)num3));
					}
					else
					{
						num4 += (num5 - 48u) * num2 * (float)num3;
						num2 *= 0.1f;
					}
				}
				else if (num5 == 44u)
				{
					if (i + 1 < num && chars[i + 1] == ' ')
					{
						lastIndex = i + 1;
					}
					else
					{
						lastIndex = i;
					}
					return num4;
				}
			}
			lastIndex = num;
			return num4;
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
						if (chars[num3] == 43 || chars[num3] == 45 || chars[num3] == 46 || (chars[num3] >= 48 && chars[num3] <= 57))
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
			int nameHashCode = this.m_xmlAttribute[0].nameHashCode;
			switch (nameHashCode)
			{
			case 83:
				break;
			default:
				switch (nameHashCode)
				{
				case 115:
					break;
				default:
					switch (nameHashCode)
					{
					case 412:
						break;
					default:
						if (nameHashCode != 426)
						{
							if (nameHashCode != 427)
							{
								switch (nameHashCode)
								{
								case 444:
									goto IL_108A;
								default:
									if (nameHashCode != -1885698441)
									{
										if (nameHashCode != -1883544150)
										{
											if (nameHashCode != -1847322671)
											{
												if (nameHashCode != -1831660941)
												{
													float num10;
													if (nameHashCode != -1690034531)
													{
														if (nameHashCode == -1668324918)
														{
															goto IL_3057;
														}
														if (nameHashCode == -1632103439)
														{
															goto IL_30DA;
														}
														if (nameHashCode == -1616441709)
														{
															goto IL_3098;
														}
														if (nameHashCode != -884817987)
														{
															if (nameHashCode != -855002522)
															{
																if (nameHashCode != -842693512)
																{
																	if (nameHashCode != -842656867)
																	{
																		if (nameHashCode != -445573839)
																		{
																			if (nameHashCode != -445537194)
																			{
																				if (nameHashCode != -330774850)
																				{
																					if (nameHashCode != 66)
																					{
																						if (nameHashCode != 73)
																						{
																							if (nameHashCode == 98)
																							{
																								goto IL_F27;
																							}
																							if (nameHashCode != 105)
																							{
																								if (nameHashCode == 395)
																								{
																									break;
																								}
																								if (nameHashCode != 402 && nameHashCode != 434)
																								{
																									if (nameHashCode != 656)
																									{
																										if (nameHashCode != 660)
																										{
																											if (nameHashCode != 670)
																											{
																												if (nameHashCode == 912)
																												{
																													goto IL_378F;
																												}
																												if (nameHashCode == 916)
																												{
																													return true;
																												}
																												if (nameHashCode != 926)
																												{
																													if (nameHashCode != 2959)
																													{
																														if (nameHashCode != 2963)
																														{
																															if (nameHashCode != 2973)
																															{
																																if (nameHashCode == 3215)
																																{
																																	return true;
																																}
																																if (nameHashCode == 3219)
																																{
																																	return true;
																																}
																																if (nameHashCode != 3229)
																																{
																																	if (nameHashCode != 4556)
																																	{
																																		if (nameHashCode != 4728)
																																		{
																																			if (nameHashCode != 4742)
																																			{
																																				if (nameHashCode == 6380)
																																				{
																																					goto IL_1697;
																																				}
																																				if (nameHashCode == 6552)
																																				{
																																					goto IL_1261;
																																				}
																																				if (nameHashCode != 6566)
																																				{
																																					if (nameHashCode != 20677)
																																					{
																																						if (nameHashCode != 20849)
																																						{
																																							if (nameHashCode != 20863)
																																							{
																																								if (nameHashCode == 22501)
																																								{
																																									goto IL_1740;
																																								}
																																								if (nameHashCode == 22673)
																																								{
																																									goto IL_1303;
																																								}
																																								if (nameHashCode != 22687)
																																								{
																																									int valueHashCode;
																																									if (nameHashCode != 28511)
																																									{
																																										if (nameHashCode != 30245)
																																										{
																																											if (nameHashCode != 30266)
																																											{
																																												if (nameHashCode != 31169)
																																												{
																																													if (nameHashCode != 31191)
																																													{
																																														if (nameHashCode != 32745)
																																														{
																																															if (nameHashCode == 41311)
																																															{
																																																goto IL_1B19;
																																															}
																																															if (nameHashCode == 43045)
																																															{
																																																goto IL_11A7;
																																															}
																																															if (nameHashCode == 43066)
																																															{
																																																goto IL_2137;
																																															}
																																															if (nameHashCode == 43969)
																																															{
																																																goto IL_181E;
																																															}
																																															if (nameHashCode == 43991)
																																															{
																																																goto IL_17DD;
																																															}
																																															if (nameHashCode != 45545)
																																															{
																																																if (nameHashCode != 141358)
																																																{
																																																	if (nameHashCode != 143092)
																																																	{
																																																		if (nameHashCode != 143113)
																																																		{
																																																			if (nameHashCode != 144016)
																																																			{
																																																				if (nameHashCode != 145592)
																																																				{
																																																					if (nameHashCode == 154158)
																																																					{
																																																						goto IL_1E38;
																																																					}
																																																					if (nameHashCode == 155892)
																																																					{
																																																						goto IL_1211;
																																																					}
																																																					if (nameHashCode == 155913)
																																																					{
																																																						goto IL_2274;
																																																					}
																																																					if (nameHashCode == 156816)
																																																					{
																																																						goto IL_1827;
																																																					}
																																																					if (nameHashCode != 158392)
																																																					{
																																																						if (nameHashCode != 186285)
																																																						{
																																																							if (nameHashCode != 186622)
																																																							{
																																																								if (nameHashCode != 192323)
																																																								{
																																																									if (nameHashCode != 226050)
																																																									{
																																																										if (nameHashCode != 227814)
																																																										{
																																																											if (nameHashCode != 230446)
																																																											{
																																																												if (nameHashCode != 237918)
																																																												{
																																																													if (nameHashCode == 275917)
																																																													{
																																																														goto IL_22E6;
																																																													}
																																																													if (nameHashCode == 276254)
																																																													{
																																																														goto IL_20EC;
																																																													}
																																																													if (nameHashCode == 280416)
																																																													{
																																																														return false;
																																																													}
																																																													if (nameHashCode == 281955)
																																																													{
																																																														goto IL_2474;
																																																													}
																																																													if (nameHashCode == 315682)
																																																													{
																																																														goto IL_35C5;
																																																													}
																																																													if (nameHashCode == 317446)
																																																													{
																																																														goto IL_36B1;
																																																													}
																																																													if (nameHashCode == 320078)
																																																													{
																																																														goto IL_204A;
																																																													}
																																																													if (nameHashCode != 327550)
																																																													{
																																																														if (nameHashCode != 976214)
																																																														{
																																																															if (nameHashCode != 982252)
																																																															{
																																																																if (nameHashCode != 1015979)
																																																																{
																																																																	if (nameHashCode != 1017743)
																																																																	{
																																																																		if (nameHashCode != 1027847)
																																																																		{
																																																																			if (nameHashCode == 1065846)
																																																																			{
																																																																				goto IL_23D2;
																																																																			}
																																																																			if (nameHashCode == 1071884)
																																																																			{
																																																																				goto IL_2987;
																																																																			}
																																																																			if (nameHashCode == 1105611)
																																																																			{
																																																																				goto IL_3632;
																																																																			}
																																																																			if (nameHashCode == 1107375)
																																																																			{
																																																																				return true;
																																																																			}
																																																																			if (nameHashCode != 1117479)
																																																																			{
																																																																				if (nameHashCode != 1286342)
																																																																				{
																																																																					if (nameHashCode != 1356515)
																																																																					{
																																																																						if (nameHashCode != 1441524)
																																																																						{
																																																																							if (nameHashCode != 1482398)
																																																																							{
																																																																								if (nameHashCode != 1524585)
																																																																								{
																																																																									if (nameHashCode != 1600507)
																																																																									{
																																																																										if (nameHashCode != 1619421)
																																																																										{
																																																																											if (nameHashCode == 1750458)
																																																																											{
																																																																												return false;
																																																																											}
																																																																											if (nameHashCode == 1913798)
																																																																											{
																																																																												goto IL_3504;
																																																																											}
																																																																											if (nameHashCode == 1983971)
																																																																											{
																																																																												goto IL_27D0;
																																																																											}
																																																																											if (nameHashCode == 2068980)
																																																																											{
																																																																												goto IL_299A;
																																																																											}
																																																																											if (nameHashCode == 2109854)
																																																																											{
																																																																												goto IL_30FD;
																																																																											}
																																																																											if (nameHashCode == 2152041)
																																																																											{
																																																																												goto IL_28D2;
																																																																											}
																																																																											if (nameHashCode == 2227963)
																																																																											{
																																																																												goto IL_363B;
																																																																											}
																																																																											if (nameHashCode != 2246877)
																																																																											{
																																																																												if (nameHashCode != 6815845)
																																																																												{
																																																																													if (nameHashCode != 6886018)
																																																																													{
																																																																														if (nameHashCode != 6971027)
																																																																														{
																																																																															if (nameHashCode != 7011901)
																																																																															{
																																																																																if (nameHashCode != 7054088)
																																																																																{
																																																																																	if (nameHashCode != 7130010)
																																																																																	{
																																																																																		if (nameHashCode == 7443301)
																																																																																		{
																																																																																			goto IL_3569;
																																																																																		}
																																																																																		if (nameHashCode == 7513474)
																																																																																		{
																																																																																			goto IL_2876;
																																																																																		}
																																																																																		if (nameHashCode == 7598483)
																																																																																		{
																																																																																			goto IL_2A75;
																																																																																		}
																																																																																		if (nameHashCode == 7639357)
																																																																																		{
																																																																																			goto IL_320A;
																																																																																		}
																																																																																		if (nameHashCode == 7681544)
																																																																																		{
																																																																																			goto IL_2978;
																																																																																		}
																																																																																		if (nameHashCode != 7757466)
																																																																																		{
																																																																																			if (nameHashCode != 9133802)
																																																																																			{
																																																																																				if (nameHashCode != 10723418)
																																																																																				{
																																																																																					if (nameHashCode != 11642281)
																																																																																					{
																																																																																						if (nameHashCode == 13526026)
																																																																																						{
																																																																																							goto IL_3079;
																																																																																						}
																																																																																						if (nameHashCode == 15115642)
																																																																																						{
																																																																																							goto IL_34FB;
																																																																																						}
																																																																																						if (nameHashCode != 16034505)
																																																																																						{
																																																																																							if (nameHashCode != 47840323)
																																																																																							{
																																																																																								if (nameHashCode != 50348802)
																																																																																								{
																																																																																									if (nameHashCode == 52232547)
																																																																																									{
																																																																																										goto IL_3098;
																																																																																									}
																																																																																									if (nameHashCode != 54741026)
																																																																																									{
																																																																																										if (nameHashCode != 69403544)
																																																																																										{
																																																																																											if (nameHashCode != 72669687)
																																																																																											{
																																																																																												if (nameHashCode == 100149144)
																																																																																												{
																																																																																													goto IL_2711;
																																																																																												}
																																																																																												if (nameHashCode != 103415287)
																																																																																												{
																																																																																													if (nameHashCode != 340349191)
																																																																																													{
																																																																																														if (nameHashCode != 343615334)
																																																																																														{
																																																																																															if (nameHashCode == 371094791)
																																																																																															{
																																																																																																goto IL_27BD;
																																																																																															}
																																																																																															if (nameHashCode != 374360934)
																																																																																															{
																																																																																																if (nameHashCode != 457225591)
																																																																																																{
																																																																																																	if (nameHashCode != 514803617)
																																																																																																	{
																																																																																																		if (nameHashCode != 551025096)
																																																																																																		{
																																																																																																			if (nameHashCode == 566686826)
																																																																																																			{
																																																																																																				goto IL_3079;
																																																																																																			}
																																																																																																			if (nameHashCode == 730022849)
																																																																																																			{
																																																																																																				goto IL_303A;
																																																																																																			}
																																																																																																			if (nameHashCode != 766244328)
																																																																																																			{
																																																																																																				if (nameHashCode == 781906058)
																																																																																																				{
																																																																																																					goto IL_3079;
																																																																																																				}
																																																																																																				if (nameHashCode == 1100728678)
																																																																																																				{
																																																																																																					goto IL_3222;
																																																																																																				}
																																																																																																				if (nameHashCode == 1109349752)
																																																																																																				{
																																																																																																					goto IL_3424;
																																																																																																				}
																																																																																																				if (nameHashCode == 1109386397)
																																																																																																				{
																																																																																																					goto IL_2A88;
																																																																																																				}
																																																																																																				if (nameHashCode == 1897350193)
																																																																																																				{
																																																																																																					goto IL_34EE;
																																																																																																				}
																																																																																																				if (nameHashCode == 1897386838)
																																																																																																				{
																																																																																																					goto IL_2B59;
																																																																																																				}
																																																																																																				if (nameHashCode != 2012149182)
																																																																																																				{
																																																																																																					return false;
																																																																																																				}
																																																																																																				goto IL_14E7;
																																																																																																			}
																																																																																																		}
																																																																																																		this.m_style |= FontStyles.SmallCaps;
																																																																																																		this.m_fontStyleStack.Add(FontStyles.SmallCaps);
																																																																																																		return true;
																																																																																																	}
																																																																																																	IL_303A:
																																																																																																	this.m_style |= FontStyles.LowerCase;
																																																																																																	this.m_fontStyleStack.Add(FontStyles.LowerCase);
																																																																																																	return true;
																																																																																																}
																																																																																																goto IL_1665;
																																																																																															}
																																																																																														}
																																																																																														MaterialReference materialReference = this.m_materialReferenceStack.Remove();
																																																																																														this.m_currentMaterial = materialReference.material;
																																																																																														this.m_currentMaterialIndex = materialReference.index;
																																																																																														return true;
																																																																																													}
																																																																																													IL_27BD:
																																																																																													this.m_colorGradientPreset = this.m_colorGradientStack.Remove();
																																																																																													return true;
																																																																																												}
																																																																																											}
																																																																																											valueHashCode = this.m_xmlAttribute[0].valueHashCode;
																																																																																											if (valueHashCode == 764638571 || valueHashCode == 523367755)
																																																																																											{
																																																																																												this.m_currentMaterial = this.m_materialReferences[0].material;
																																																																																												this.m_currentMaterialIndex = 0;
																																																																																												this.m_materialReferenceStack.Add(this.m_materialReferences[0]);
																																																																																												return true;
																																																																																											}
																																																																																											Material material;
																																																																																											if (MaterialReferenceManager.TryGetMaterial(valueHashCode, out material))
																																																																																											{
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
																																																																																												MaterialReferenceManager.AddFontMaterial(valueHashCode, material);
																																																																																												this.m_currentMaterial = material;
																																																																																												this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																																																																																												this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
																																																																																											}
																																																																																											return true;
																																																																																										}
																																																																																										IL_2711:
																																																																																										int valueHashCode2 = this.m_xmlAttribute[0].valueHashCode;
																																																																																										TMP_ColorGradient tmp_ColorGradient;
																																																																																										if (MaterialReferenceManager.TryGetColorGradientPreset(valueHashCode2, out tmp_ColorGradient))
																																																																																										{
																																																																																											this.m_colorGradientPreset = tmp_ColorGradient;
																																																																																										}
																																																																																										else
																																																																																										{
																																																																																											if (tmp_ColorGradient == null)
																																																																																											{
																																																																																												tmp_ColorGradient = Resources.Load<TMP_ColorGradient>(TMP_Settings.defaultColorGradientPresetsPath + new string(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength));
																																																																																											}
																																																																																											if (tmp_ColorGradient == null)
																																																																																											{
																																																																																												return false;
																																																																																											}
																																																																																											MaterialReferenceManager.AddColorGradientPreset(valueHashCode2, tmp_ColorGradient);
																																																																																											this.m_colorGradientPreset = tmp_ColorGradient;
																																																																																										}
																																																																																										this.m_colorGradientStack.Add(this.m_colorGradientPreset);
																																																																																										return true;
																																																																																									}
																																																																																								}
																																																																																								this.m_baselineOffset = 0f;
																																																																																								return true;
																																																																																							}
																																																																																							goto IL_3098;
																																																																																						}
																																																																																					}
																																																																																					num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																																					if (num10 == -9999f)
																																																																																					{
																																																																																						return false;
																																																																																					}
																																																																																					if (tagUnits == TagUnits.Pixels)
																																																																																					{
																																																																																						this.m_baselineOffset = num10;
																																																																																						return true;
																																																																																					}
																																																																																					if (tagUnits != TagUnits.FontUnits)
																																																																																					{
																																																																																						return tagUnits != TagUnits.Percentage && false;
																																																																																					}
																																																																																					this.m_baselineOffset = num10 * this.m_fontScale * this.m_fontAsset.fontInfo.Ascender;
																																																																																					return true;
																																																																																				}
																																																																																				IL_34FB:
																																																																																				this.tag_NoParsing = true;
																																																																																				return true;
																																																																																			}
																																																																																			IL_3079:
																																																																																			this.m_style |= FontStyles.UpperCase;
																																																																																			this.m_fontStyleStack.Add(FontStyles.UpperCase);
																																																																																			return true;
																																																																																		}
																																																																																	}
																																																																																	this.m_isFXMatrixSet = false;
																																																																																	return true;
																																																																																}
																																																																																IL_2978:
																																																																																this.m_monoSpacing = 0f;
																																																																																return true;
																																																																															}
																																																																															IL_320A:
																																																																															this.m_marginLeft = 0f;
																																																																															this.m_marginRight = 0f;
																																																																															return true;
																																																																														}
																																																																														IL_2A75:
																																																																														this.tag_Indent = this.m_indentStack.Remove();
																																																																														return true;
																																																																													}
																																																																													IL_2876:
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
																																																																												}
																																																																												IL_3569:
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
																																																																											int num11 = (int)this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																											if (num11 == -9999)
																																																																											{
																																																																												return false;
																																																																											}
																																																																											if (num11 > this.m_currentSpriteAsset.spriteInfoList.Count - 1)
																																																																											{
																																																																												return false;
																																																																											}
																																																																											this.m_spriteIndex = num11;
																																																																										}
																																																																										this.m_spriteColor = TMP_Text.s_colorWhite;
																																																																										this.m_tintSprite = false;
																																																																										int num12 = 0;
																																																																										while (num12 < this.m_xmlAttribute.Length && this.m_xmlAttribute[num12].nameHashCode != 0)
																																																																										{
																																																																											int nameHashCode2 = this.m_xmlAttribute[num12].nameHashCode;
																																																																											if (nameHashCode2 == 26705)
																																																																											{
																																																																												goto IL_2F20;
																																																																											}
																																																																											int num13;
																																																																											if (nameHashCode2 != 30547)
																																																																											{
																																																																												if (nameHashCode2 != 33019)
																																																																												{
																																																																													if (nameHashCode2 == 39505)
																																																																													{
																																																																														goto IL_2F20;
																																																																													}
																																																																													if (nameHashCode2 == 43347)
																																																																													{
																																																																														goto IL_2E03;
																																																																													}
																																																																													if (nameHashCode2 != 45819)
																																																																													{
																																																																														if (nameHashCode2 != 192323)
																																																																														{
																																																																															if (nameHashCode2 != 205930)
																																																																															{
																																																																																if (nameHashCode2 == 281955)
																																																																																{
																																																																																	goto IL_2EE5;
																																																																																}
																																																																																if (nameHashCode2 != 295562)
																																																																																{
																																																																																	if (nameHashCode2 != 2246877 && nameHashCode2 != 1619421)
																																																																																	{
																																																																																		return false;
																																																																																	}
																																																																																	goto IL_2FCF;
																																																																																}
																																																																															}
																																																																															num13 = (int)this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength);
																																																																															if (num13 == -9999)
																																																																															{
																																																																																return false;
																																																																															}
																																																																															if (num13 > this.m_currentSpriteAsset.spriteInfoList.Count - 1)
																																																																															{
																																																																																return false;
																																																																															}
																																																																															this.m_spriteIndex = num13;
																																																																															goto IL_2FCF;
																																																																														}
																																																																														IL_2EE5:
																																																																														this.m_spriteColor = this.HexCharsToColor(this.m_htmlTag, this.m_xmlAttribute[num12].valueStartIndex, this.m_xmlAttribute[num12].valueLength);
																																																																														goto IL_2FCF;
																																																																													}
																																																																												}
																																																																												this.m_tintSprite = (this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[num12].valueStartIndex, this.m_xmlAttribute[num12].valueLength) != 0f);
																																																																												goto IL_2FCF;
																																																																											}
																																																																											IL_2E03:
																																																																											num13 = this.m_currentSpriteAsset.GetSpriteIndexFromHashcode(this.m_xmlAttribute[num12].valueHashCode);
																																																																											if (num13 == -1)
																																																																											{
																																																																												return false;
																																																																											}
																																																																											this.m_spriteIndex = num13;
																																																																											IL_2FCF:
																																																																											num12++;
																																																																											continue;
																																																																											IL_2F20:
																																																																											int attributeParameters = this.GetAttributeParameters(this.m_htmlTag, this.m_xmlAttribute[num12].valueStartIndex, this.m_xmlAttribute[num12].valueLength, ref this.m_attributeParameterValues);
																																																																											if (attributeParameters != 3)
																																																																											{
																																																																												return false;
																																																																											}
																																																																											this.m_spriteIndex = (int)this.m_attributeParameterValues[0];
																																																																											if (this.m_isParsingText)
																																																																											{
																																																																												this.spriteAnimator.DoSpriteAnimation(this.m_characterCount, this.m_currentSpriteAsset, this.m_spriteIndex, (int)this.m_attributeParameterValues[1], (int)this.m_attributeParameterValues[2]);
																																																																											}
																																																																											goto IL_2FCF;
																																																																										}
																																																																										if (this.m_spriteIndex == -1)
																																																																										{
																																																																											return false;
																																																																										}
																																																																										this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentSpriteAsset.material, this.m_currentSpriteAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																																																																										this.m_textElementType = TMP_TextElementType.Sprite;
																																																																										return true;
																																																																									}
																																																																									IL_363B:
																																																																									num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																									if (num10 == -9999f)
																																																																									{
																																																																										return false;
																																																																									}
																																																																									this.m_FXMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, num10), Vector3.one);
																																																																									this.m_isFXMatrixSet = true;
																																																																									return true;
																																																																								}
																																																																								IL_28D2:
																																																																								num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																								if (num10 == -9999f)
																																																																								{
																																																																									return false;
																																																																								}
																																																																								if (tagUnits != TagUnits.Pixels)
																																																																								{
																																																																									if (tagUnits != TagUnits.FontUnits)
																																																																									{
																																																																										if (tagUnits == TagUnits.Percentage)
																																																																										{
																																																																											return false;
																																																																										}
																																																																									}
																																																																									else
																																																																									{
																																																																										this.m_monoSpacing = num10;
																																																																										this.m_monoSpacing *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																																																																									}
																																																																								}
																																																																								else
																																																																								{
																																																																									this.m_monoSpacing = num10;
																																																																								}
																																																																								return true;
																																																																							}
																																																																							IL_30FD:
																																																																							num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																							if (num10 == -9999f)
																																																																							{
																																																																								return false;
																																																																							}
																																																																							this.m_marginLeft = num10;
																																																																							if (tagUnits != TagUnits.Pixels)
																																																																							{
																																																																								if (tagUnits != TagUnits.FontUnits)
																																																																								{
																																																																									if (tagUnits == TagUnits.Percentage)
																																																																									{
																																																																										this.m_marginLeft = (this.m_marginWidth - ((this.m_width == -1f) ? 0f : this.m_width)) * this.m_marginLeft / 100f;
																																																																									}
																																																																								}
																																																																								else
																																																																								{
																																																																									this.m_marginLeft *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																																																																								}
																																																																							}
																																																																							this.m_marginLeft = ((this.m_marginLeft < 0f) ? 0f : this.m_marginLeft);
																																																																							this.m_marginRight = this.m_marginLeft;
																																																																							return true;
																																																																						}
																																																																						IL_299A:
																																																																						num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																						if (num10 == -9999f)
																																																																						{
																																																																							return false;
																																																																						}
																																																																						if (tagUnits != TagUnits.Pixels)
																																																																						{
																																																																							if (tagUnits != TagUnits.FontUnits)
																																																																							{
																																																																								if (tagUnits == TagUnits.Percentage)
																																																																								{
																																																																									this.tag_Indent = this.m_marginWidth * num10 / 100f;
																																																																								}
																																																																							}
																																																																							else
																																																																							{
																																																																								this.tag_Indent = num10;
																																																																								this.tag_Indent *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																																																																							}
																																																																						}
																																																																						else
																																																																						{
																																																																							this.tag_Indent = num10;
																																																																						}
																																																																						this.m_indentStack.Add(this.tag_Indent);
																																																																						this.m_xAdvance = this.tag_Indent;
																																																																						return true;
																																																																					}
																																																																					IL_27D0:
																																																																					num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																					if (num10 == -9999f)
																																																																					{
																																																																						return false;
																																																																					}
																																																																					if (tagUnits != TagUnits.Pixels)
																																																																					{
																																																																						if (tagUnits != TagUnits.FontUnits)
																																																																						{
																																																																							if (tagUnits == TagUnits.Percentage)
																																																																							{
																																																																								return false;
																																																																							}
																																																																						}
																																																																						else
																																																																						{
																																																																							this.m_cSpacing = num10;
																																																																							this.m_cSpacing *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																																																																						}
																																																																					}
																																																																					else
																																																																					{
																																																																						this.m_cSpacing = num10;
																																																																					}
																																																																					return true;
																																																																				}
																																																																				IL_3504:
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
																																																																			}
																																																																		}
																																																																		this.m_width = -1f;
																																																																		return true;
																																																																	}
																																																																	return true;
																																																																}
																																																																IL_3632:
																																																																this.m_isFXMatrixSet = false;
																																																																return true;
																																																															}
																																																															IL_2987:
																																																															this.m_htmlColor = this.m_colorStack.Remove();
																																																															return true;
																																																														}
																																																														IL_23D2:
																																																														this.m_lineJustification = this.m_lineJustificationStack.Remove();
																																																														return true;
																																																													}
																																																												}
																																																												num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																												if (num10 == -9999f)
																																																												{
																																																													return false;
																																																												}
																																																												if (tagUnits != TagUnits.Pixels)
																																																												{
																																																													if (tagUnits == TagUnits.FontUnits)
																																																													{
																																																														return false;
																																																													}
																																																													if (tagUnits == TagUnits.Percentage)
																																																													{
																																																														this.m_width = this.m_marginWidth * num10 / 100f;
																																																													}
																																																												}
																																																												else
																																																												{
																																																													this.m_width = num10;
																																																												}
																																																												return true;
																																																											}
																																																											IL_204A:
																																																											num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																											if (num10 == -9999f)
																																																											{
																																																												return false;
																																																											}
																																																											if (tagUnits == TagUnits.Pixels)
																																																											{
																																																												this.m_xAdvance += num10;
																																																												return true;
																																																											}
																																																											if (tagUnits != TagUnits.FontUnits)
																																																											{
																																																												return tagUnits != TagUnits.Percentage && false;
																																																											}
																																																											this.m_xAdvance += num10 * this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																																																											return true;
																																																										}
																																																										IL_36B1:
																																																										int nameHashCode3 = this.m_xmlAttribute[1].nameHashCode;
																																																										if (nameHashCode3 == 327550)
																																																										{
																																																											float num14 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength);
																																																											if (tagUnits != TagUnits.Pixels)
																																																											{
																																																												if (tagUnits != TagUnits.FontUnits)
																																																												{
																																																													if (tagUnits == TagUnits.Percentage)
																																																													{
																																																														Debug.Log("Table width = " + num14 + "%.");
																																																													}
																																																												}
																																																												else
																																																												{
																																																													Debug.Log("Table width = " + num14 + "em.");
																																																												}
																																																											}
																																																											else
																																																											{
																																																												Debug.Log("Table width = " + num14 + "px.");
																																																											}
																																																										}
																																																										return true;
																																																									}
																																																									IL_35C5:
																																																									num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																									if (num10 == -9999f)
																																																									{
																																																										return false;
																																																									}
																																																									this.m_FXMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(num10, 1f, 1f));
																																																									this.m_isFXMatrixSet = true;
																																																									return true;
																																																								}
																																																								IL_2474:
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
																																																								int valueHashCode5 = this.m_xmlAttribute[0].valueHashCode;
																																																								if (valueHashCode5 == -36881330)
																																																								{
																																																									this.m_htmlColor = new Color32(160, 32, 240, byte.MaxValue);
																																																									this.m_colorStack.Add(this.m_htmlColor);
																																																									return true;
																																																								}
																																																								if (valueHashCode5 == 125395)
																																																								{
																																																									this.m_htmlColor = Color.red;
																																																									this.m_colorStack.Add(this.m_htmlColor);
																																																									return true;
																																																								}
																																																								if (valueHashCode5 == 3573310)
																																																								{
																																																									this.m_htmlColor = Color.blue;
																																																									this.m_colorStack.Add(this.m_htmlColor);
																																																									return true;
																																																								}
																																																								if (valueHashCode5 == 26556144)
																																																								{
																																																									this.m_htmlColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);
																																																									this.m_colorStack.Add(this.m_htmlColor);
																																																									return true;
																																																								}
																																																								if (valueHashCode5 == 117905991)
																																																								{
																																																									this.m_htmlColor = Color.black;
																																																									this.m_colorStack.Add(this.m_htmlColor);
																																																									return true;
																																																								}
																																																								if (valueHashCode5 == 121463835)
																																																								{
																																																									this.m_htmlColor = Color.green;
																																																									this.m_colorStack.Add(this.m_htmlColor);
																																																									return true;
																																																								}
																																																								if (valueHashCode5 == 140357351)
																																																								{
																																																									this.m_htmlColor = Color.white;
																																																									this.m_colorStack.Add(this.m_htmlColor);
																																																									return true;
																																																								}
																																																								if (valueHashCode5 != 554054276)
																																																								{
																																																									return false;
																																																								}
																																																								this.m_htmlColor = Color.yellow;
																																																								this.m_colorStack.Add(this.m_htmlColor);
																																																								return true;
																																																							}
																																																							IL_20EC:
																																																							if (this.m_xmlAttribute[0].valueLength != 3)
																																																							{
																																																								return false;
																																																							}
																																																							this.m_htmlColor.a = (byte)(this.HexToInt(this.m_htmlTag[7]) * 16 + this.HexToInt(this.m_htmlTag[8]));
																																																							return true;
																																																						}
																																																						IL_22E6:
																																																						int valueHashCode6 = this.m_xmlAttribute[0].valueHashCode;
																																																						if (valueHashCode6 == -523808257)
																																																						{
																																																							this.m_lineJustification = TextAlignmentOptions.Justified;
																																																							this.m_lineJustificationStack.Add(this.m_lineJustification);
																																																							return true;
																																																						}
																																																						if (valueHashCode6 == -458210101)
																																																						{
																																																							this.m_lineJustification = TextAlignmentOptions.Center;
																																																							this.m_lineJustificationStack.Add(this.m_lineJustification);
																																																							return true;
																																																						}
																																																						if (valueHashCode6 == 3774683)
																																																						{
																																																							this.m_lineJustification = TextAlignmentOptions.Left;
																																																							this.m_lineJustificationStack.Add(this.m_lineJustification);
																																																							return true;
																																																						}
																																																						if (valueHashCode6 == 122383428)
																																																						{
																																																							this.m_lineJustification = TextAlignmentOptions.Flush;
																																																							this.m_lineJustificationStack.Add(this.m_lineJustification);
																																																							return true;
																																																						}
																																																						if (valueHashCode6 != 136703040)
																																																						{
																																																							return false;
																																																						}
																																																						this.m_lineJustification = TextAlignmentOptions.Right;
																																																						this.m_lineJustificationStack.Add(this.m_lineJustification);
																																																						return true;
																																																					}
																																																				}
																																																				this.m_currentFontSize = this.m_sizeStack.Remove();
																																																				this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																																																				return true;
																																																			}
																																																			IL_1827:
																																																			this.m_isNonBreakingSpace = false;
																																																			return true;
																																																		}
																																																		IL_2274:
																																																		if (this.m_isParsingText && !this.m_isCalculatingPreferredValues)
																																																		{
																																																			this.m_textInfo.linkInfo[this.m_textInfo.linkCount].linkTextLength = this.m_characterCount - this.m_textInfo.linkInfo[this.m_textInfo.linkCount].linkTextfirstCharacterIndex;
																																																			this.m_textInfo.linkCount++;
																																																		}
																																																		return true;
																																																	}
																																																	IL_1211:
																																																	if ((this.m_fontStyle & FontStyles.Highlight) != FontStyles.Highlight)
																																																	{
																																																		this.m_highlightColor = this.m_highlightColorStack.Remove();
																																																		if (this.m_fontStyleStack.Remove(FontStyles.Highlight) == 0)
																																																		{
																																																			this.m_style &= (FontStyles)(-513);
																																																		}
																																																	}
																																																	return true;
																																																}
																																																IL_1E38:
																																																MaterialReference materialReference2 = this.m_materialReferenceStack.Remove();
																																																this.m_currentFontAsset = materialReference2.fontAsset;
																																																this.m_currentMaterial = materialReference2.material;
																																																this.m_currentMaterialIndex = materialReference2.index;
																																																this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																																																return true;
																																															}
																																														}
																																														num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																														if (num10 == -9999f)
																																														{
																																															return false;
																																														}
																																														if (tagUnits != TagUnits.Pixels)
																																														{
																																															if (tagUnits == TagUnits.FontUnits)
																																															{
																																																this.m_currentFontSize = this.m_fontSize * num10;
																																																this.m_sizeStack.Add(this.m_currentFontSize);
																																																this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																																																return true;
																																															}
																																															if (tagUnits != TagUnits.Percentage)
																																															{
																																																return false;
																																															}
																																															this.m_currentFontSize = this.m_fontSize * num10 / 100f;
																																															this.m_sizeStack.Add(this.m_currentFontSize);
																																															this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																																															return true;
																																														}
																																														else
																																														{
																																															if (this.m_htmlTag[5] == '+')
																																															{
																																																this.m_currentFontSize = this.m_fontSize + num10;
																																																this.m_sizeStack.Add(this.m_currentFontSize);
																																																this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																																																return true;
																																															}
																																															if (this.m_htmlTag[5] == '-')
																																															{
																																																this.m_currentFontSize = this.m_fontSize + num10;
																																																this.m_sizeStack.Add(this.m_currentFontSize);
																																																this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																																																return true;
																																															}
																																															this.m_currentFontSize = num10;
																																															this.m_sizeStack.Add(this.m_currentFontSize);
																																															this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																																															return true;
																																														}
																																													}
																																													IL_17DD:
																																													if (this.m_overflowMode == TextOverflowModes.Page)
																																													{
																																														this.m_xAdvance = this.tag_LineIndent + this.tag_Indent;
																																														this.m_lineOffset = 0f;
																																														this.m_pageNumber++;
																																														this.m_isNewPage = true;
																																													}
																																													return true;
																																												}
																																												IL_181E:
																																												this.m_isNonBreakingSpace = true;
																																												return true;
																																											}
																																											IL_2137:
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
																																										IL_11A7:
																																										this.m_style |= FontStyles.Highlight;
																																										this.m_fontStyleStack.Add(FontStyles.Highlight);
																																										this.m_highlightColor = this.HexCharsToColor(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																										this.m_highlightColorStack.Add(this.m_highlightColor);
																																										return true;
																																									}
																																									IL_1B19:
																																									int valueHashCode7 = this.m_xmlAttribute[0].valueHashCode;
																																									int nameHashCode4 = this.m_xmlAttribute[1].nameHashCode;
																																									valueHashCode = this.m_xmlAttribute[1].valueHashCode;
																																									if (valueHashCode7 == 764638571 || valueHashCode7 == 523367755)
																																									{
																																										this.m_currentFontAsset = this.m_materialReferences[0].fontAsset;
																																										this.m_currentMaterial = this.m_materialReferences[0].material;
																																										this.m_currentMaterialIndex = 0;
																																										this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																																										this.m_materialReferenceStack.Add(this.m_materialReferences[0]);
																																										return true;
																																									}
																																									TMP_FontAsset tmp_FontAsset;
																																									if (!MaterialReferenceManager.TryGetFontAsset(valueHashCode7, out tmp_FontAsset))
																																									{
																																										tmp_FontAsset = Resources.Load<TMP_FontAsset>(TMP_Settings.defaultFontAssetPath + new string(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength));
																																										if (tmp_FontAsset == null)
																																										{
																																											return false;
																																										}
																																										MaterialReferenceManager.AddFontAsset(tmp_FontAsset);
																																									}
																																									if (nameHashCode4 == 0 && valueHashCode == 0)
																																									{
																																										this.m_currentMaterial = tmp_FontAsset.material;
																																										this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																																										this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
																																									}
																																									else
																																									{
																																										if (nameHashCode4 != 103415287 && nameHashCode4 != 72669687)
																																										{
																																											return false;
																																										}
																																										Material material;
																																										if (MaterialReferenceManager.TryGetMaterial(valueHashCode, out material))
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
																																											MaterialReferenceManager.AddFontMaterial(valueHashCode, material);
																																											this.m_currentMaterial = material;
																																											this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																																											this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
																																										}
																																									}
																																									this.m_currentFontAsset = tmp_FontAsset;
																																									this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																																									return true;
																																								}
																																							}
																																							if ((this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
																																							{
																																								if (this.m_fontScaleMultiplier < 1f)
																																								{
																																									this.m_baselineOffset = this.m_baselineOffsetStack.Pop();
																																									this.m_fontScaleMultiplier /= ((this.m_currentFontAsset.fontInfo.SubSize <= 0f) ? 1f : this.m_currentFontAsset.fontInfo.SubSize);
																																								}
																																								if (this.m_fontStyleStack.Remove(FontStyles.Superscript) == 0)
																																								{
																																									this.m_style &= (FontStyles)(-129);
																																								}
																																							}
																																							return true;
																																						}
																																						IL_1303:
																																						if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript)
																																						{
																																							if (this.m_fontScaleMultiplier < 1f)
																																							{
																																								this.m_baselineOffset = this.m_baselineOffsetStack.Pop();
																																								this.m_fontScaleMultiplier /= ((this.m_currentFontAsset.fontInfo.SubSize <= 0f) ? 1f : this.m_currentFontAsset.fontInfo.SubSize);
																																							}
																																							if (this.m_fontStyleStack.Remove(FontStyles.Subscript) == 0)
																																							{
																																								this.m_style &= (FontStyles)(-257);
																																							}
																																						}
																																						return true;
																																					}
																																					IL_1740:
																																					this.m_isIgnoringAlignment = false;
																																					return true;
																																				}
																																			}
																																			this.m_fontScaleMultiplier *= ((this.m_currentFontAsset.fontInfo.SubSize <= 0f) ? 1f : this.m_currentFontAsset.fontInfo.SubSize);
																																			this.m_baselineOffsetStack.Push(this.m_baselineOffset);
																																			this.m_baselineOffset += this.m_currentFontAsset.fontInfo.SuperscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
																																			this.m_fontStyleStack.Add(FontStyles.Superscript);
																																			this.m_style |= FontStyles.Superscript;
																																			return true;
																																		}
																																		IL_1261:
																																		this.m_fontScaleMultiplier *= ((this.m_currentFontAsset.fontInfo.SubSize <= 0f) ? 1f : this.m_currentFontAsset.fontInfo.SubSize);
																																		this.m_baselineOffsetStack.Push(this.m_baselineOffset);
																																		this.m_baselineOffset += this.m_currentFontAsset.fontInfo.SubscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
																																		this.m_fontStyleStack.Add(FontStyles.Subscript);
																																		this.m_style |= FontStyles.Subscript;
																																		return true;
																																	}
																																	IL_1697:
																																	num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																	if (num10 == -9999f)
																																	{
																																		return false;
																																	}
																																	if (tagUnits == TagUnits.Pixels)
																																	{
																																		this.m_xAdvance = num10;
																																		return true;
																																	}
																																	if (tagUnits == TagUnits.FontUnits)
																																	{
																																		this.m_xAdvance = num10 * this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																																		return true;
																																	}
																																	if (tagUnits != TagUnits.Percentage)
																																	{
																																		return false;
																																	}
																																	this.m_xAdvance = this.m_marginWidth * num10 / 100f;
																																	return true;
																																}
																															}
																															return true;
																														}
																														return true;
																													}
																													return true;
																												}
																											}
																											return true;
																										}
																										return true;
																									}
																									IL_378F:
																									int num15 = 1;
																									while (num15 < this.m_xmlAttribute.Length && this.m_xmlAttribute[num15].nameHashCode != 0)
																									{
																										int nameHashCode5 = this.m_xmlAttribute[num15].nameHashCode;
																										if (nameHashCode5 != 327550)
																										{
																											if (nameHashCode5 == 275917)
																											{
																												int valueHashCode8 = this.m_xmlAttribute[num15].valueHashCode;
																												if (valueHashCode8 != -523808257)
																												{
																													if (valueHashCode8 != -458210101)
																													{
																														if (valueHashCode8 != 3774683)
																														{
																															if (valueHashCode8 == 136703040)
																															{
																																Debug.Log("TD align=\"right\".");
																															}
																														}
																														else
																														{
																															Debug.Log("TD align=\"left\".");
																														}
																													}
																													else
																													{
																														Debug.Log("TD align=\"center\".");
																													}
																												}
																												else
																												{
																													Debug.Log("TD align=\"justified\".");
																												}
																											}
																										}
																										else
																										{
																											float num16 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[num15].valueStartIndex, this.m_xmlAttribute[num15].valueLength);
																											if (tagUnits != TagUnits.Pixels)
																											{
																												if (tagUnits != TagUnits.FontUnits)
																												{
																													if (tagUnits == TagUnits.Percentage)
																													{
																														Debug.Log("Table width = " + num16 + "%.");
																													}
																												}
																												else
																												{
																													Debug.Log("Table width = " + num16 + "em.");
																												}
																											}
																											else
																											{
																												Debug.Log("Table width = " + num16 + "px.");
																											}
																										}
																										num15++;
																									}
																									return true;
																								}
																								if (this.m_fontStyleStack.Remove(FontStyles.Italic) == 0)
																								{
																									this.m_style &= (FontStyles)(-3);
																								}
																								return true;
																							}
																						}
																						this.m_style |= FontStyles.Italic;
																						this.m_fontStyleStack.Add(FontStyles.Italic);
																						return true;
																					}
																					IL_F27:
																					this.m_style |= FontStyles.Bold;
																					this.m_fontStyleStack.Add(FontStyles.Bold);
																					this.m_fontWeightInternal = 700;
																					this.m_fontWeightStack.Add(700);
																					return true;
																				}
																				IL_14E7:
																				num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																				if (num10 == -9999f)
																				{
																					return false;
																				}
																				if ((this.m_fontStyle & FontStyles.Bold) == FontStyles.Bold)
																				{
																					return true;
																				}
																				this.m_style &= (FontStyles)(-2);
																				int num17 = (int)num10;
																				if (num17 != 100)
																				{
																					if (num17 != 200)
																					{
																						if (num17 != 300)
																						{
																							if (num17 != 400)
																							{
																								if (num17 != 500)
																								{
																									if (num17 != 600)
																									{
																										if (num17 != 700)
																										{
																											if (num17 != 800)
																											{
																												if (num17 == 900)
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
																									}
																									else
																									{
																										this.m_fontWeightInternal = 600;
																									}
																								}
																								else
																								{
																									this.m_fontWeightInternal = 500;
																								}
																							}
																							else
																							{
																								this.m_fontWeightInternal = 400;
																							}
																						}
																						else
																						{
																							this.m_fontWeightInternal = 300;
																						}
																					}
																					else
																					{
																						this.m_fontWeightInternal = 200;
																					}
																				}
																				else
																				{
																					this.m_fontWeightInternal = 100;
																				}
																				this.m_fontWeightStack.Add(this.m_fontWeightInternal);
																				return true;
																			}
																			IL_2B59:
																			this.tag_LineIndent = 0f;
																			return true;
																		}
																		IL_34EE:
																		this.m_lineHeight = -32767f;
																		return true;
																	}
																	IL_2A88:
																	num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																	if (num10 == -9999f)
																	{
																		return false;
																	}
																	if (tagUnits != TagUnits.Pixels)
																	{
																		if (tagUnits != TagUnits.FontUnits)
																		{
																			if (tagUnits == TagUnits.Percentage)
																			{
																				this.tag_LineIndent = this.m_marginWidth * num10 / 100f;
																			}
																		}
																		else
																		{
																			this.tag_LineIndent = num10;
																			this.tag_LineIndent *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																		}
																	}
																	else
																	{
																		this.tag_LineIndent = num10;
																	}
																	this.m_xAdvance += this.tag_LineIndent;
																	return true;
																}
																IL_3424:
																num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																if (num10 == -9999f || num10 == 0f)
																{
																	return false;
																}
																this.m_lineHeight = num10;
																if (tagUnits != TagUnits.Pixels)
																{
																	if (tagUnits != TagUnits.FontUnits)
																	{
																		if (tagUnits == TagUnits.Percentage)
																		{
																			this.m_lineHeight = this.m_fontAsset.fontInfo.LineHeight * this.m_lineHeight / 100f * this.m_fontScale;
																		}
																	}
																	else
																	{
																		this.m_lineHeight *= this.m_fontAsset.fontInfo.LineHeight * this.m_fontScale;
																	}
																}
																return true;
															}
															IL_3222:
															num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
															if (num10 == -9999f)
															{
																return false;
															}
															this.m_marginLeft = num10;
															if (tagUnits != TagUnits.Pixels)
															{
																if (tagUnits != TagUnits.FontUnits)
																{
																	if (tagUnits == TagUnits.Percentage)
																	{
																		this.m_marginLeft = (this.m_marginWidth - ((this.m_width == -1f) ? 0f : this.m_width)) * this.m_marginLeft / 100f;
																	}
																}
																else
																{
																	this.m_marginLeft *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																}
															}
															this.m_marginLeft = ((this.m_marginLeft < 0f) ? 0f : this.m_marginLeft);
															return true;
														}
													}
													num10 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
													if (num10 == -9999f)
													{
														return false;
													}
													this.m_marginRight = num10;
													if (tagUnits != TagUnits.Pixels)
													{
														if (tagUnits != TagUnits.FontUnits)
														{
															if (tagUnits == TagUnits.Percentage)
															{
																this.m_marginRight = (this.m_marginWidth - ((this.m_width == -1f) ? 0f : this.m_width)) * this.m_marginRight / 100f;
															}
														}
														else
														{
															this.m_marginRight *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
														}
													}
													this.m_marginRight = ((this.m_marginRight < 0f) ? 0f : this.m_marginRight);
													return true;
												}
												IL_3098:
												if (this.m_fontStyleStack.Remove(FontStyles.UpperCase) == 0)
												{
													this.m_style &= (FontStyles)(-17);
												}
												return true;
											}
											IL_30DA:
											if (this.m_fontStyleStack.Remove(FontStyles.SmallCaps) == 0)
											{
												this.m_style &= (FontStyles)(-33);
											}
											return true;
										}
										IL_3057:
										if (this.m_fontStyleStack.Remove(FontStyles.LowerCase) == 0)
										{
											this.m_style &= (FontStyles)(-9);
										}
										return true;
									}
									IL_1665:
									this.m_fontWeightInternal = this.m_fontWeightStack.Remove();
									if (this.m_fontWeightInternal == 400)
									{
										this.m_style &= (FontStyles)(-2);
									}
									return true;
								case 446:
									goto IL_1166;
								}
							}
							if ((this.m_fontStyle & FontStyles.Bold) != FontStyles.Bold)
							{
								this.m_fontWeightInternal = this.m_fontWeightStack.Remove();
								if (this.m_fontStyleStack.Remove(FontStyles.Bold) == 0)
								{
									this.m_style &= (FontStyles)(-2);
								}
							}
							return true;
						}
						return true;
					case 414:
						goto IL_1166;
					}
					IL_108A:
					if ((this.m_fontStyle & FontStyles.Strikethrough) != FontStyles.Strikethrough && this.m_fontStyleStack.Remove(FontStyles.Strikethrough) == 0)
					{
						this.m_style &= (FontStyles)(-65);
					}
					return true;
					IL_1166:
					if ((this.m_fontStyle & FontStyles.Underline) != FontStyles.Underline)
					{
						this.m_underlineColor = this.m_underlineColorStack.Remove();
						if (this.m_fontStyleStack.Remove(FontStyles.Underline) == 0)
						{
							this.m_style &= (FontStyles)(-5);
						}
					}
					return true;
				case 117:
					goto IL_10BD;
				}
				break;
			case 85:
				goto IL_10BD;
			}
			this.m_style |= FontStyles.Strikethrough;
			this.m_fontStyleStack.Add(FontStyles.Strikethrough);
			if (this.m_xmlAttribute[1].nameHashCode == 281955 || this.m_xmlAttribute[1].nameHashCode == 192323)
			{
				this.m_strikethroughColor = this.HexCharsToColor(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength);
			}
			else
			{
				this.m_strikethroughColor = this.m_htmlColor;
			}
			this.m_strikethroughColorStack.Add(this.m_strikethroughColor);
			return true;
			IL_10BD:
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

		protected enum TextInputSources
		{
			Text,
			SetText,
			SetCharArray,
			String
		}
	}
}
