using System;

namespace UnityEngine.UIComponentSkinnerInternal
{
	[Serializable]
	public sealed class UIComponentSkinnerObjectParts
	{
		[SerializeField]
		private UISkinnerPartsType _uiSkinnerPartsType;

		[SerializeField]
		private GameObject _gameObject;

		[SerializeField]
		private GameObject[] _gameObjects = new GameObject[0];

		[SerializeField]
		private bool _setActive = true;

		[SerializeField]
		private UIBasicSprite _uiBasicSprite;

		[SerializeField]
		private Color _uiBasicSpriteColor = Color.white;

		[SerializeField]
		private UIWidget _uiWidget;

		[SerializeField]
		private AnimatedAlpha _animatedAlpha;

		[SerializeField]
		private float _alpha = 1f;

		[SerializeField]
		private int _depth;

		[SerializeField]
		private UISpriteSkinnerBase _spriteSkinner;

		[SerializeField]
		private int _skinnerIndex;

		[SerializeField]
		private UISkinnerToggle _skinnerToggle;

		[SerializeField]
		private UILabel _uiLabel;

		[SerializeField]
		private int _uiLabelFontSize = 16;

		[SerializeField]
		private bool _useUnityFont;

		[SerializeField]
		private UIFont _uiLabelNguiFont;

		[SerializeField]
		private Font _uiLabelUnityFont;

		[SerializeField]
		private FontStyle _uiLabelFontStyle;

		[SerializeField]
		private string _uiLabelText;

		[SerializeField]
		private Color _uiLabelColor = Color.white;

		[SerializeField]
		private bool _uiLabelUseGradient;

		[SerializeField]
		private Color _uiLabelGradientTopColor = Color.white;

		[SerializeField]
		private Color _uiLabelGradientBottomColor = Color.gray;

		[SerializeField]
		private UILabel.Effect _uiLabelEffectStyle;

		[SerializeField]
		private Color _uiLabelEffectColor = Color.white;

		[SerializeField]
		private Vector2 _uiLabelEffectDistance = Vector3.one;

		[SerializeField]
		private UITextReplacer _uiTextReplacer;

		[SerializeField]
		private int _replacerIndex;

		[SerializeField]
		private UIScreenPosition _uiScreenPosition;

		[SerializeField]
		private int _uiScreenPositionIndex;

		[SerializeField]
		private UIMaterialSwitcherBase _materialSwicher;

		[SerializeField]
		private UIMaterialSwitcherBase[] _materialSwithcers = new UIMaterialSwitcherBase[0];

		[SerializeField]
		private int _materialSwicherIndex;

		private DepthControllerHash cachedDepthControllerHash;

		public UIComponentSkinnerObjectParts()
		{
		}

		public UIComponentSkinnerObjectParts(UIComponentSkinnerObjectParts baseParts)
		{
			this._uiSkinnerPartsType = baseParts._uiSkinnerPartsType;
			this._gameObject = baseParts._gameObject;
			this._setActive = baseParts._setActive;
			this._uiBasicSprite = baseParts._uiBasicSprite;
			this._uiBasicSpriteColor = baseParts._uiBasicSpriteColor;
			this._uiWidget = baseParts._uiWidget;
			this._animatedAlpha = baseParts._animatedAlpha;
			this._alpha = baseParts._alpha;
			this._spriteSkinner = baseParts._spriteSkinner;
			this._skinnerIndex = baseParts._skinnerIndex;
			this._skinnerToggle = baseParts._skinnerToggle;
			this._uiLabel = baseParts._uiLabel;
			this._uiLabelFontSize = baseParts._uiLabelFontSize;
			this._useUnityFont = baseParts._useUnityFont;
			this._uiLabelNguiFont = baseParts._uiLabelNguiFont;
			this._uiLabelUnityFont = baseParts._uiLabelUnityFont;
			this._uiLabelFontStyle = baseParts._uiLabelFontStyle;
			this._uiLabelText = baseParts._uiLabelText;
			this._uiLabelColor = baseParts._uiLabelColor;
			this._uiLabelUseGradient = baseParts._uiLabelUseGradient;
			this._uiLabelGradientTopColor = baseParts._uiLabelGradientTopColor;
			this._uiLabelGradientBottomColor = baseParts._uiLabelGradientBottomColor;
			this._uiLabelEffectStyle = baseParts._uiLabelEffectStyle;
			this._uiLabelEffectColor = baseParts._uiLabelEffectColor;
			this._uiLabelEffectDistance = baseParts._uiLabelEffectDistance;
			this._uiScreenPosition = baseParts._uiScreenPosition;
			this._uiScreenPositionIndex = baseParts._uiScreenPositionIndex;
			this._replacerIndex = baseParts._replacerIndex;
			this._gameObjects = baseParts._gameObjects;
			this._materialSwicher = baseParts._materialSwicher;
			this._materialSwithcers = baseParts._materialSwithcers;
			this._materialSwicherIndex = baseParts._materialSwicherIndex;
		}

		public UIComponentSkinnerObjectParts Clone()
		{
			return new UIComponentSkinnerObjectParts(this);
		}

		private void EditorApply(Object obj)
		{
		}

		public void ApplySkin()
		{
			switch (this._uiSkinnerPartsType)
			{
			case UISkinnerPartsType.NGUISpriteColor:
				if (this._uiBasicSprite != null)
				{
					this._uiBasicSprite.color = this._uiBasicSpriteColor;
					this.EditorApply(this._uiBasicSprite);
				}
				break;
			case UISkinnerPartsType.NGUIWidgetAlpha:
				if (this._uiWidget != null)
				{
					this._uiWidget.alpha = this._alpha;
					this.EditorApply(this._uiWidget);
				}
				break;
			case UISkinnerPartsType.NGUIAnimatedAlpha:
				if (this._animatedAlpha != null)
				{
					this._animatedAlpha.alpha = this._alpha;
					this.EditorApply(this._animatedAlpha);
				}
				break;
			case UISkinnerPartsType.SpriteSkinner:
				if (this._spriteSkinner != null)
				{
					this._spriteSkinner.value = this._skinnerIndex;
					this.EditorApply(this._spriteSkinner);
				}
				break;
			case UISkinnerPartsType.UISkinnerToggle:
				if (this._skinnerToggle != null)
				{
					this._skinnerToggle.value = this._setActive;
				}
				break;
			case UISkinnerPartsType.NGUILabelStyle:
				if (this._uiLabel != null)
				{
					this._uiLabel.fontSize = this._uiLabelFontSize;
					this._uiLabel.fontStyle = this._uiLabelFontStyle;
					this.EditorApply(this._uiLabel);
				}
				break;
			case UISkinnerPartsType.NGUILabelText:
				if (this._uiLabel != null)
				{
					this._uiLabel.text = this._uiLabelText;
					this.EditorApply(this._uiLabel);
				}
				break;
			case UISkinnerPartsType.NGUILabelColor:
				if (this._uiLabel != null)
				{
					this._uiLabel.color = this._uiLabelColor;
					this._uiLabel.applyGradient = this._uiLabelUseGradient;
					this._uiLabel.gradientTop = this._uiLabelGradientTopColor;
					this._uiLabel.gradientBottom = this._uiLabelGradientBottomColor;
					this.EditorApply(this._uiLabel);
				}
				break;
			case UISkinnerPartsType.NGUILabelEffect:
				if (this._uiLabel != null)
				{
					this._uiLabel.effectStyle = this._uiLabelEffectStyle;
					this._uiLabel.effectColor = this._uiLabelEffectColor;
					this._uiLabel.effectDistance = this._uiLabelEffectDistance;
					this.EditorApply(this._uiLabel);
				}
				break;
			case UISkinnerPartsType.ScreenPosition:
				if (this._uiScreenPosition != null)
				{
					this._uiScreenPosition.SetScreenPosition(this._uiScreenPositionIndex);
				}
				break;
			case UISkinnerPartsType.NGUILabelFont:
				if (this._uiLabel != null)
				{
					if (!this._useUnityFont && this._uiLabelNguiFont != null)
					{
						this._uiLabel.bitmapFont = this._uiLabelNguiFont;
					}
					if (this._useUnityFont && this._uiLabelUnityFont != null)
					{
						this._uiLabel.trueTypeFont = this._uiLabelUnityFont;
					}
					this.EditorApply(this._uiLabel);
				}
				break;
			case UISkinnerPartsType.TextReplacer:
				if (this._uiTextReplacer != null)
				{
					this._uiTextReplacer.value = this._replacerIndex;
					this._uiTextReplacer.Apply();
				}
				break;
			case UISkinnerPartsType.ObjectsSetActive:
				if (this._gameObjects != null)
				{
					foreach (GameObject gameObject in this._gameObjects)
					{
						if (gameObject != null)
						{
							NGUITools.SetActiveSelf(gameObject, this._setActive);
						}
					}
				}
				break;
			case UISkinnerPartsType.WidgetDepth:
				if (this._uiWidget != null)
				{
					this._uiWidget.depth = this._depth;
					if (!this.cachedDepthControllerHash)
					{
						this.cachedDepthControllerHash = this._uiWidget.gameObject.GetComponent<DepthControllerHash>();
					}
					if (this.cachedDepthControllerHash)
					{
						this.cachedDepthControllerHash.originDepth = this._depth;
					}
					this.EditorApply(this._uiWidget);
				}
				break;
			case UISkinnerPartsType.MaterialSwicher:
				if (this._materialSwicher != null)
				{
					this._materialSwicher.value = this._materialSwicherIndex;
					this._materialSwicher.Apply();
				}
				break;
			case UISkinnerPartsType.MaterialSwichers:
				if (this._materialSwicher != null)
				{
					foreach (UIMaterialSwitcherBase uimaterialSwitcherBase in this._materialSwithcers)
					{
						uimaterialSwitcherBase.value = this._materialSwicherIndex;
						uimaterialSwitcherBase.Apply();
					}
				}
				break;
			default:
				if (this._gameObject != null)
				{
					NGUITools.SetActiveSelf(this._gameObject, this._setActive);
					this.EditorApply(this._gameObject);
				}
				break;
			}
		}
	}
}
