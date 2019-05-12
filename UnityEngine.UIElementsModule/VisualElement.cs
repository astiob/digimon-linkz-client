using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine.CSSLayout;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
	public class VisualElement : Focusable, ITransform, IUIElementDataWatch, IEnumerable<VisualElement>, IVisualElementScheduler, IStyle, IEnumerable
	{
		private static uint s_NextId;

		private string m_Name;

		private HashSet<string> m_ClassList;

		private string m_TypeName;

		private string m_FullTypeName;

		private string m_PersistenceKey;

		private RenderData m_RenderData;

		private Vector3 m_Position = Vector3.zero;

		private Quaternion m_Rotation = Quaternion.identity;

		private Vector3 m_Scale = Vector3.one;

		private Rect m_Layout;

		private PseudoStates m_PseudoStates;

		internal VisualElementStylesData m_SharedStyle = VisualElementStylesData.none;

		internal VisualElementStylesData m_Style = VisualElementStylesData.none;

		internal readonly uint controlid;

		private ChangeType changesNeeded;

		[SerializeField]
		private string m_Text;

		private bool m_Enabled;

		internal const Align DefaultAlignContent = Align.FlexStart;

		internal const Align DefaultAlignItems = Align.Stretch;

		private VisualElement.ClippingOptions m_ClippingOptions;

		private VisualElement m_PhysicalParent;

		private VisualElement m_LogicalParent;

		private static readonly VisualElement[] s_EmptyList = new VisualElement[0];

		private List<VisualElement> m_Children;

		private List<StyleSheet> m_StyleSheets;

		private List<string> m_StyleSheetPaths;

		public VisualElement()
		{
			this.controlid = (VisualElement.s_NextId += 1u);
			this.shadow = new VisualElement.Hierarchy(this);
			this.m_ClassList = new HashSet<string>();
			this.m_FullTypeName = string.Empty;
			this.m_TypeName = string.Empty;
			this.SetEnabled(true);
			this.visible = true;
			base.focusIndex = -1;
			this.name = string.Empty;
			this.cssNode = new CSSNode();
			this.cssNode.SetMeasureFunction(new MeasureFunction(this.Measure));
			this.changesNeeded = ChangeType.All;
			this.clippingOptions = VisualElement.ClippingOptions.ClipContents;
		}

		public string persistenceKey
		{
			get
			{
				return this.m_PersistenceKey;
			}
			set
			{
				if (this.m_PersistenceKey != value)
				{
					this.m_PersistenceKey = value;
					if (!string.IsNullOrEmpty(value))
					{
						this.Dirty(ChangeType.PersistentData);
					}
				}
			}
		}

		internal bool enablePersistence { get; private set; }

		public object userData { get; set; }

		public override bool canGrabFocus
		{
			get
			{
				return this.enabledInHierarchy && base.canGrabFocus;
			}
		}

		public override FocusController focusController
		{
			get
			{
				return (this.panel != null) ? this.panel.focusController : null;
			}
		}

		internal RenderData renderData
		{
			get
			{
				RenderData result;
				if ((result = this.m_RenderData) == null)
				{
					result = (this.m_RenderData = new RenderData());
				}
				return result;
			}
		}

		public ITransform transform
		{
			get
			{
				return this;
			}
		}

		Vector3 ITransform.position
		{
			get
			{
				return this.m_Position;
			}
			set
			{
				if (!(this.m_Position == value))
				{
					this.m_Position = value;
					this.Dirty(ChangeType.Transform);
				}
			}
		}

		Quaternion ITransform.rotation
		{
			get
			{
				return this.m_Rotation;
			}
			set
			{
				if (!(this.m_Rotation == value))
				{
					this.m_Rotation = value;
					this.Dirty(ChangeType.Transform);
				}
			}
		}

		Vector3 ITransform.scale
		{
			get
			{
				return this.m_Scale;
			}
			set
			{
				if (!(this.m_Scale == value))
				{
					this.m_Scale = value;
					this.Dirty(ChangeType.Transform);
				}
			}
		}

		Matrix4x4 ITransform.matrix
		{
			get
			{
				return Matrix4x4.TRS(this.m_Position, this.m_Rotation, this.m_Scale);
			}
		}

		public Rect layout
		{
			get
			{
				Rect layout = this.m_Layout;
				if (this.cssNode != null && this.style.positionType.value != PositionType.Manual)
				{
					layout.x = this.cssNode.LayoutX;
					layout.y = this.cssNode.LayoutY;
					layout.width = this.cssNode.LayoutWidth;
					layout.height = this.cssNode.LayoutHeight;
				}
				return layout;
			}
			set
			{
				if (this.cssNode == null)
				{
					this.cssNode = new CSSNode();
				}
				if (this.style.positionType.value != PositionType.Manual || !(this.m_Layout == value))
				{
					this.m_Layout = value;
					((IStyle)this).positionType = PositionType.Manual;
					((IStyle)this).marginLeft = 0f;
					((IStyle)this).marginRight = 0f;
					((IStyle)this).marginBottom = 0f;
					((IStyle)this).marginTop = 0f;
					((IStyle)this).positionLeft = value.x;
					((IStyle)this).positionTop = value.y;
					((IStyle)this).positionRight = float.NaN;
					((IStyle)this).positionBottom = float.NaN;
					((IStyle)this).width = value.width;
					((IStyle)this).height = value.height;
					this.Dirty(ChangeType.Layout);
				}
			}
		}

		public Rect contentRect
		{
			get
			{
				Spacing a = new Spacing(this.m_Style.paddingLeft, this.m_Style.paddingTop, this.m_Style.paddingRight, this.m_Style.paddingBottom);
				return this.paddingRect - a;
			}
		}

		protected Rect paddingRect
		{
			get
			{
				Spacing a = new Spacing(this.style.borderLeftWidth, this.style.borderTopWidth, this.style.borderRightWidth, this.style.borderBottomWidth);
				return this.layout - a;
			}
		}

		public Rect worldBound
		{
			get
			{
				Matrix4x4 worldTransform = this.worldTransform;
				Vector3 vector = worldTransform.MultiplyPoint3x4(this.layout.min);
				Vector3 vector2 = worldTransform.MultiplyPoint3x4(this.layout.max);
				return Rect.MinMaxRect(Math.Min(vector.x, vector2.x), Math.Min(vector.y, vector2.y), Math.Max(vector.x, vector2.x), Math.Max(vector.y, vector2.y));
			}
		}

		public Rect localBound
		{
			get
			{
				Matrix4x4 matrix = this.transform.matrix;
				Vector3 vector = matrix.MultiplyPoint3x4(this.layout.min);
				Vector3 vector2 = matrix.MultiplyPoint3x4(this.layout.max);
				return Rect.MinMaxRect(Math.Min(vector.x, vector2.x), Math.Min(vector.y, vector2.y), Math.Max(vector.x, vector2.x), Math.Max(vector.y, vector2.y));
			}
		}

		public Matrix4x4 worldTransform
		{
			get
			{
				if (this.IsDirty(ChangeType.Transform))
				{
					if (this.shadow.parent != null)
					{
						this.renderData.worldTransForm = this.shadow.parent.worldTransform * Matrix4x4.Translate(new Vector3(this.shadow.parent.layout.x, this.shadow.parent.layout.y, 0f)) * this.transform.matrix;
					}
					else
					{
						this.renderData.worldTransForm = this.transform.matrix;
					}
					this.ClearDirty(ChangeType.Transform);
				}
				return this.renderData.worldTransForm;
			}
		}

		internal PseudoStates pseudoStates
		{
			get
			{
				return this.m_PseudoStates;
			}
			set
			{
				if (this.m_PseudoStates != value)
				{
					this.m_PseudoStates = value;
					this.Dirty(ChangeType.Styles);
				}
			}
		}

		public PickingMode pickingMode { get; set; }

		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				if (!(this.m_Name == value))
				{
					this.m_Name = value;
					this.Dirty(ChangeType.Styles);
				}
			}
		}

		internal string fullTypeName
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_FullTypeName))
				{
					this.m_FullTypeName = base.GetType().FullName;
				}
				return this.m_FullTypeName;
			}
		}

		internal string typeName
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_TypeName))
				{
					this.m_TypeName = base.GetType().Name;
				}
				return this.m_TypeName;
			}
		}

		internal CSSNode cssNode { get; private set; }

		public virtual void OnStyleResolved(ICustomStyle style)
		{
			this.FinalizeLayout();
		}

		internal VisualElementStylesData sharedStyle
		{
			get
			{
				return this.m_SharedStyle;
			}
		}

		internal VisualElementStylesData effectiveStyle
		{
			get
			{
				return this.m_Style;
			}
		}

		internal bool hasInlineStyle
		{
			get
			{
				return this.m_Style != this.m_SharedStyle;
			}
		}

		private VisualElementStylesData inlineStyle
		{
			get
			{
				if (!this.hasInlineStyle)
				{
					VisualElementStylesData visualElementStylesData = new VisualElementStylesData(false);
					visualElementStylesData.Apply(this.m_SharedStyle, StylePropertyApplyMode.Copy);
					this.m_Style = visualElementStylesData;
				}
				return this.m_Style;
			}
		}

		internal float opacity
		{
			get
			{
				return this.style.opacity.value;
			}
			set
			{
				this.style.opacity = value;
			}
		}

		protected internal override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			if (evt.GetEventTypeId() == EventBase<MouseEnterEvent>.TypeId())
			{
				this.pseudoStates |= PseudoStates.Hover;
			}
			else if (evt.GetEventTypeId() == EventBase<MouseLeaveEvent>.TypeId())
			{
				this.pseudoStates &= ~PseudoStates.Hover;
			}
			else if (evt.GetEventTypeId() == EventBase<BlurEvent>.TypeId())
			{
				this.pseudoStates &= ~PseudoStates.Focus;
			}
			else if (evt.GetEventTypeId() == EventBase<FocusEvent>.TypeId())
			{
				this.pseudoStates |= PseudoStates.Focus;
			}
		}

		public sealed override void Focus()
		{
			if (!this.canGrabFocus && this.shadow.parent != null)
			{
				this.shadow.parent.Focus();
			}
			else
			{
				base.Focus();
			}
		}

		internal virtual void ChangePanel(BaseVisualElementPanel p)
		{
			if (this.panel != p)
			{
				if (this.panel != null)
				{
					DetachFromPanelEvent pooled = EventBase<DetachFromPanelEvent>.GetPooled();
					pooled.target = this;
					UIElementsUtility.eventDispatcher.DispatchEvent(pooled, this.panel);
					EventBase<DetachFromPanelEvent>.ReleasePooled(pooled);
				}
				this.elementPanel = p;
				if (this.panel != null)
				{
					AttachToPanelEvent pooled2 = EventBase<AttachToPanelEvent>.GetPooled();
					pooled2.target = this;
					UIElementsUtility.eventDispatcher.DispatchEvent(pooled2, this.panel);
					EventBase<AttachToPanelEvent>.ReleasePooled(pooled2);
				}
				this.Dirty(ChangeType.Styles);
				if (this.m_Children != null)
				{
					foreach (VisualElement visualElement in this.m_Children)
					{
						visualElement.ChangePanel(p);
					}
				}
			}
		}

		private void PropagateToChildren(ChangeType type)
		{
			if ((type & this.changesNeeded) != type)
			{
				this.changesNeeded |= type;
				type &= (ChangeType.Styles | ChangeType.Transform);
				if (type != (ChangeType)0)
				{
					if (this.m_Children != null)
					{
						foreach (VisualElement visualElement in this.m_Children)
						{
							visualElement.PropagateToChildren(type);
						}
					}
				}
			}
		}

		private void PropagateChangesToParents()
		{
			ChangeType changeType = (ChangeType)0;
			if (this.changesNeeded != (ChangeType)0)
			{
				changeType |= ChangeType.Repaint;
				if ((this.changesNeeded & ChangeType.Styles) > (ChangeType)0)
				{
					changeType |= ChangeType.StylesPath;
				}
				if ((this.changesNeeded & (ChangeType.PersistentData | ChangeType.PersistentDataPath)) > (ChangeType)0)
				{
					changeType |= ChangeType.PersistentDataPath;
				}
			}
			for (VisualElement parent = this.shadow.parent; parent != null; parent = parent.shadow.parent)
			{
				if ((parent.changesNeeded & changeType) == changeType)
				{
					break;
				}
				parent.changesNeeded |= changeType;
			}
		}

		public void Dirty(ChangeType type)
		{
			if ((type & this.changesNeeded) != type)
			{
				if ((type & ChangeType.Layout) > (ChangeType)0)
				{
					if (this.cssNode != null && this.cssNode.IsMeasureDefined)
					{
						this.cssNode.MarkDirty();
					}
					type |= ChangeType.Repaint;
				}
				this.PropagateToChildren(type);
				this.PropagateChangesToParents();
			}
		}

		internal bool AnyDirty()
		{
			return this.changesNeeded != (ChangeType)0;
		}

		public bool IsDirty(ChangeType type)
		{
			return (this.changesNeeded & type) == type;
		}

		public bool AnyDirty(ChangeType type)
		{
			return (this.changesNeeded & type) > (ChangeType)0;
		}

		public void ClearDirty(ChangeType type)
		{
			this.changesNeeded &= ~type;
		}

		public string text
		{
			get
			{
				return this.m_Text ?? string.Empty;
			}
			set
			{
				if (!(this.m_Text == value))
				{
					this.m_Text = value;
					this.Dirty(ChangeType.Layout);
					if (!string.IsNullOrEmpty(this.persistenceKey))
					{
						this.SavePersistentData();
					}
				}
			}
		}

		[Obsolete("enabled is deprecated. Use SetEnabled as setter, and enabledSelf/enabledInHierarchy as getters.", true)]
		public virtual bool enabled
		{
			get
			{
				return this.enabledInHierarchy;
			}
			set
			{
				this.SetEnabled(value);
			}
		}

		protected internal bool SetEnabledFromHierarchy(bool state)
		{
			bool result;
			if (state == ((this.pseudoStates & PseudoStates.Disabled) != PseudoStates.Disabled))
			{
				result = false;
			}
			else
			{
				if (state && this.m_Enabled && (this.parent == null || this.parent.enabledInHierarchy))
				{
					this.pseudoStates &= ~PseudoStates.Disabled;
				}
				else
				{
					this.pseudoStates |= PseudoStates.Disabled;
				}
				result = true;
			}
			return result;
		}

		public bool enabledInHierarchy
		{
			get
			{
				return (this.pseudoStates & PseudoStates.Disabled) != PseudoStates.Disabled;
			}
		}

		public bool enabledSelf
		{
			get
			{
				return this.m_Enabled;
			}
		}

		public virtual void SetEnabled(bool value)
		{
			if (this.m_Enabled != value)
			{
				this.m_Enabled = value;
				this.PropagateEnabledToChildren(value);
			}
		}

		private void PropagateEnabledToChildren(bool value)
		{
			if (this.SetEnabledFromHierarchy(value))
			{
				for (int i = 0; i < this.shadow.childCount; i++)
				{
					this.shadow[i].PropagateEnabledToChildren(value);
				}
			}
		}

		public bool visible
		{
			get
			{
				return (this.pseudoStates & PseudoStates.Invisible) != PseudoStates.Invisible;
			}
			set
			{
				if (value)
				{
					this.pseudoStates &= (PseudoStates)2147483647;
				}
				else
				{
					this.pseudoStates |= PseudoStates.Invisible;
				}
			}
		}

		public virtual void DoRepaint()
		{
			IStylePainter stylePainter = this.elementPanel.stylePainter;
			stylePainter.DrawBackground(this);
			stylePainter.DrawBorder(this);
			stylePainter.DrawText(this);
		}

		internal virtual void DoRepaint(IStylePainter painter)
		{
			if ((this.pseudoStates & PseudoStates.Invisible) != PseudoStates.Invisible)
			{
				this.DoRepaint();
			}
		}

		private void GetFullHierarchicalPersistenceKey(StringBuilder key)
		{
			if (this.parent != null)
			{
				this.parent.GetFullHierarchicalPersistenceKey(key);
			}
			if (!string.IsNullOrEmpty(this.persistenceKey))
			{
				key.Append("__");
				key.Append(this.persistenceKey);
			}
		}

		public string GetFullHierarchicalPersistenceKey()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.GetFullHierarchicalPersistenceKey(stringBuilder);
			return stringBuilder.ToString();
		}

		public T GetOrCreatePersistentData<T>(object existing, string key) where T : class, new()
		{
			Debug.Assert(this.elementPanel != null, "VisualElement.elementPanel is null! Cannot load persistent data.");
			ISerializableJsonDictionary serializableJsonDictionary = (this.elementPanel != null && this.elementPanel.getViewDataDictionary != null) ? this.elementPanel.getViewDataDictionary() : null;
			T result;
			if (serializableJsonDictionary == null || string.IsNullOrEmpty(this.persistenceKey) || !this.enablePersistence)
			{
				if (existing != null)
				{
					result = (existing as T);
				}
				else
				{
					result = Activator.CreateInstance<T>();
				}
			}
			else
			{
				string key2 = key + "__" + typeof(T).ToString();
				if (!serializableJsonDictionary.ContainsKey(key2))
				{
					serializableJsonDictionary.Set<T>(key2, Activator.CreateInstance<T>());
				}
				result = serializableJsonDictionary.Get<T>(key2);
			}
			return result;
		}

		public T GetOrCreatePersistentData<T>(ScriptableObject existing, string key) where T : ScriptableObject
		{
			Debug.Assert(this.elementPanel != null, "VisualElement.elementPanel is null! Cannot load persistent data.");
			ISerializableJsonDictionary serializableJsonDictionary = (this.elementPanel != null && this.elementPanel.getViewDataDictionary != null) ? this.elementPanel.getViewDataDictionary() : null;
			T result;
			if (serializableJsonDictionary == null || string.IsNullOrEmpty(this.persistenceKey) || !this.enablePersistence)
			{
				if (existing != null)
				{
					result = (existing as T);
				}
				else
				{
					result = ScriptableObject.CreateInstance<T>();
				}
			}
			else
			{
				string key2 = key + "__" + typeof(T).ToString();
				if (!serializableJsonDictionary.ContainsKey(key2))
				{
					serializableJsonDictionary.Set<T>(key2, ScriptableObject.CreateInstance<T>());
				}
				result = serializableJsonDictionary.GetScriptable<T>(key2);
			}
			return result;
		}

		public void OverwriteFromPersistedData(object obj, string key)
		{
			Debug.Assert(this.elementPanel != null, "VisualElement.elementPanel is null! Cannot load persistent data.");
			ISerializableJsonDictionary serializableJsonDictionary = (this.elementPanel != null && this.elementPanel.getViewDataDictionary != null) ? this.elementPanel.getViewDataDictionary() : null;
			if (serializableJsonDictionary != null && !string.IsNullOrEmpty(this.persistenceKey) && this.enablePersistence)
			{
				string key2 = key + "__" + obj.GetType();
				if (!serializableJsonDictionary.ContainsKey(key2))
				{
					serializableJsonDictionary.Set<object>(key2, obj);
				}
				else
				{
					serializableJsonDictionary.Overwrite(obj, key2);
				}
			}
		}

		public void SavePersistentData()
		{
			if (this.elementPanel != null && this.elementPanel.savePersistentViewData != null && !string.IsNullOrEmpty(this.persistenceKey))
			{
				this.elementPanel.savePersistentViewData();
			}
		}

		internal bool IsPersitenceSupportedOnChildren()
		{
			return base.GetType() == typeof(VisualElement) || !string.IsNullOrEmpty(this.persistenceKey);
		}

		internal void OnPersistentDataReady(bool enablePersistence)
		{
			this.enablePersistence = enablePersistence;
			this.OnPersistentDataReady();
		}

		public virtual void OnPersistentDataReady()
		{
		}

		public virtual bool ContainsPoint(Vector2 localPoint)
		{
			return this.layout.Contains(localPoint);
		}

		public virtual bool Overlaps(Rect rectangle)
		{
			return this.layout.Overlaps(rectangle, true);
		}

		protected internal virtual Vector2 DoMeasure(float width, VisualElement.MeasureMode widthMode, float height, VisualElement.MeasureMode heightMode)
		{
			IStylePainter stylePainter = this.elementPanel.stylePainter;
			float num = float.NaN;
			float num2 = float.NaN;
			Font font = this.style.font;
			Vector2 result;
			if (this.m_Text == null || font == null)
			{
				result = new Vector2(num, num2);
			}
			else
			{
				if (widthMode == VisualElement.MeasureMode.Exactly)
				{
					num = width;
				}
				else
				{
					TextStylePainterParameters defaultTextParameters = stylePainter.GetDefaultTextParameters(this);
					defaultTextParameters.text = this.text;
					defaultTextParameters.font = font;
					defaultTextParameters.wordWrapWidth = 0f;
					defaultTextParameters.wordWrap = false;
					defaultTextParameters.richText = true;
					num = stylePainter.ComputeTextWidth(defaultTextParameters);
					if (widthMode == VisualElement.MeasureMode.AtMost)
					{
						num = Mathf.Min(num, width);
					}
				}
				if (heightMode == VisualElement.MeasureMode.Exactly)
				{
					num2 = height;
				}
				else
				{
					TextStylePainterParameters defaultTextParameters2 = stylePainter.GetDefaultTextParameters(this);
					defaultTextParameters2.text = this.text;
					defaultTextParameters2.font = font;
					defaultTextParameters2.wordWrapWidth = num;
					defaultTextParameters2.richText = true;
					num2 = stylePainter.ComputeTextHeight(defaultTextParameters2);
					if (heightMode == VisualElement.MeasureMode.AtMost)
					{
						num2 = Mathf.Min(num2, height);
					}
				}
				result = new Vector2(num, num2);
			}
			return result;
		}

		internal long Measure(CSSNode node, float width, CSSMeasureMode widthMode, float height, CSSMeasureMode heightMode)
		{
			Debug.Assert(node == this.cssNode, "CSSNode instance mismatch");
			Vector2 vector = this.DoMeasure(width, (VisualElement.MeasureMode)widthMode, height, (VisualElement.MeasureMode)heightMode);
			return MeasureOutput.Make(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
		}

		public void SetSize(Vector2 size)
		{
			Rect layout = this.layout;
			layout.width = size.x;
			layout.height = size.y;
			this.layout = layout;
		}

		private void FinalizeLayout()
		{
			this.cssNode.Flex = this.style.flex.GetSpecifiedValueOrDefault(float.NaN);
			this.cssNode.SetPosition(CSSEdge.Left, this.style.positionLeft.GetSpecifiedValueOrDefault(float.NaN));
			this.cssNode.SetPosition(CSSEdge.Top, this.style.positionTop.GetSpecifiedValueOrDefault(float.NaN));
			this.cssNode.SetPosition(CSSEdge.Right, this.style.positionRight.GetSpecifiedValueOrDefault(float.NaN));
			this.cssNode.SetPosition(CSSEdge.Bottom, this.style.positionBottom.GetSpecifiedValueOrDefault(float.NaN));
			this.cssNode.SetMargin(CSSEdge.Left, this.style.marginLeft.GetSpecifiedValueOrDefault(float.NaN));
			this.cssNode.SetMargin(CSSEdge.Top, this.style.marginTop.GetSpecifiedValueOrDefault(float.NaN));
			this.cssNode.SetMargin(CSSEdge.Right, this.style.marginRight.GetSpecifiedValueOrDefault(float.NaN));
			this.cssNode.SetMargin(CSSEdge.Bottom, this.style.marginBottom.GetSpecifiedValueOrDefault(float.NaN));
			this.cssNode.SetPadding(CSSEdge.Left, this.style.paddingLeft.GetSpecifiedValueOrDefault(float.NaN));
			this.cssNode.SetPadding(CSSEdge.Top, this.style.paddingTop.GetSpecifiedValueOrDefault(float.NaN));
			this.cssNode.SetPadding(CSSEdge.Right, this.style.paddingRight.GetSpecifiedValueOrDefault(float.NaN));
			this.cssNode.SetPadding(CSSEdge.Bottom, this.style.paddingBottom.GetSpecifiedValueOrDefault(float.NaN));
			this.cssNode.SetBorder(CSSEdge.Left, this.style.borderLeft.GetSpecifiedValueOrDefault(this.style.borderLeftWidth.GetSpecifiedValueOrDefault(float.NaN)));
			this.cssNode.SetBorder(CSSEdge.Top, this.style.borderTop.GetSpecifiedValueOrDefault(this.style.borderTopWidth.GetSpecifiedValueOrDefault(float.NaN)));
			this.cssNode.SetBorder(CSSEdge.Right, this.style.borderRight.GetSpecifiedValueOrDefault(this.style.borderRightWidth.GetSpecifiedValueOrDefault(float.NaN)));
			this.cssNode.SetBorder(CSSEdge.Bottom, this.style.borderBottom.GetSpecifiedValueOrDefault(this.style.borderBottomWidth.GetSpecifiedValueOrDefault(float.NaN)));
			this.cssNode.Width = this.style.width.GetSpecifiedValueOrDefault(float.NaN);
			this.cssNode.Height = this.style.height.GetSpecifiedValueOrDefault(float.NaN);
			PositionType positionType = this.style.positionType;
			if (positionType != PositionType.Absolute && positionType != PositionType.Manual)
			{
				if (positionType == PositionType.Relative)
				{
					this.cssNode.PositionType = CSSPositionType.Relative;
				}
			}
			else
			{
				this.cssNode.PositionType = CSSPositionType.Absolute;
			}
			this.cssNode.Overflow = (CSSOverflow)this.style.overflow.value;
			this.cssNode.AlignSelf = (CSSAlign)this.style.alignSelf.value;
			this.cssNode.MaxWidth = this.style.maxWidth.GetSpecifiedValueOrDefault(float.NaN);
			this.cssNode.MaxHeight = this.style.maxHeight.GetSpecifiedValueOrDefault(float.NaN);
			this.cssNode.MinWidth = this.style.minWidth.GetSpecifiedValueOrDefault(float.NaN);
			this.cssNode.MinHeight = this.style.minHeight.GetSpecifiedValueOrDefault(float.NaN);
			this.cssNode.FlexDirection = (CSSFlexDirection)this.style.flexDirection.value;
			this.cssNode.AlignContent = (CSSAlign)this.style.alignContent.GetSpecifiedValueOrDefault(Align.FlexStart);
			this.cssNode.AlignItems = (CSSAlign)this.style.alignItems.GetSpecifiedValueOrDefault(Align.Stretch);
			this.cssNode.JustifyContent = (CSSJustify)this.style.justifyContent.value;
			this.cssNode.Wrap = (CSSWrap)this.style.flexWrap.value;
			this.Dirty(ChangeType.Layout);
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event OnStylesResolved onStylesResolved;

		internal void SetInlineStyles(VisualElementStylesData inlineStyle)
		{
			Debug.Assert(!inlineStyle.isShared);
			inlineStyle.Apply(this.m_Style, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity);
			this.m_Style = inlineStyle;
		}

		internal void SetSharedStyles(VisualElementStylesData sharedStyle)
		{
			Debug.Assert(sharedStyle.isShared);
			this.ClearDirty(ChangeType.Styles | ChangeType.StylesPath);
			if (sharedStyle != this.m_SharedStyle)
			{
				if (this.hasInlineStyle)
				{
					this.m_Style.Apply(sharedStyle, StylePropertyApplyMode.CopyIfNotInline);
				}
				else
				{
					this.m_Style = sharedStyle;
				}
				this.m_SharedStyle = sharedStyle;
				if (this.onStylesResolved != null)
				{
					this.onStylesResolved(this.m_Style);
				}
				this.OnStyleResolved(this.m_Style);
				this.Dirty(ChangeType.Repaint);
			}
		}

		public void ResetPositionProperties()
		{
			if (this.hasInlineStyle)
			{
				VisualElementStylesData inlineStyle = this.inlineStyle;
				inlineStyle.positionType = StyleValue<int>.nil;
				inlineStyle.marginLeft = StyleValue<float>.nil;
				inlineStyle.marginRight = StyleValue<float>.nil;
				inlineStyle.marginBottom = StyleValue<float>.nil;
				inlineStyle.marginTop = StyleValue<float>.nil;
				inlineStyle.positionLeft = StyleValue<float>.nil;
				inlineStyle.positionTop = StyleValue<float>.nil;
				inlineStyle.positionRight = StyleValue<float>.nil;
				inlineStyle.positionBottom = StyleValue<float>.nil;
				inlineStyle.width = StyleValue<float>.nil;
				inlineStyle.height = StyleValue<float>.nil;
				this.m_Style.Apply(this.sharedStyle, StylePropertyApplyMode.CopyIfNotInline);
				this.FinalizeLayout();
				this.Dirty(ChangeType.Layout);
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.name,
				" ",
				this.layout,
				" world rect: ",
				this.worldBound
			});
		}

		internal IEnumerable<string> GetClasses()
		{
			return this.m_ClassList;
		}

		public void ClearClassList()
		{
			if (this.m_ClassList != null && this.m_ClassList.Count > 0)
			{
				this.m_ClassList.Clear();
				this.Dirty(ChangeType.Styles);
			}
		}

		public void AddToClassList(string className)
		{
			if (this.m_ClassList == null)
			{
				this.m_ClassList = new HashSet<string>();
			}
			if (this.m_ClassList.Add(className))
			{
				this.Dirty(ChangeType.Styles);
			}
		}

		public void RemoveFromClassList(string className)
		{
			if (this.m_ClassList != null && this.m_ClassList.Remove(className))
			{
				this.Dirty(ChangeType.Styles);
			}
		}

		public bool ClassListContains(string cls)
		{
			return this.m_ClassList != null && this.m_ClassList.Contains(cls);
		}

		public object FindAncestorUserData()
		{
			for (VisualElement parent = this.parent; parent != null; parent = parent.parent)
			{
				if (parent.userData != null)
				{
					return parent.userData;
				}
			}
			return null;
		}

		public IUIElementDataWatch dataWatch
		{
			get
			{
				return this;
			}
		}

		IUIElementDataWatchRequest IUIElementDataWatch.RegisterWatch(Object toWatch, Action<Object> watchNotification)
		{
			VisualElement.DataWatchRequest dataWatchRequest = new VisualElement.DataWatchRequest(this)
			{
				notification = watchNotification,
				watchedObject = toWatch
			};
			dataWatchRequest.Start();
			return dataWatchRequest;
		}

		void IUIElementDataWatch.UnregisterWatch(IUIElementDataWatchRequest requested)
		{
			VisualElement.DataWatchRequest dataWatchRequest = requested as VisualElement.DataWatchRequest;
			if (dataWatchRequest != null)
			{
				dataWatchRequest.Stop();
			}
		}

		public VisualElement.Hierarchy shadow { get; private set; }

		public VisualElement.ClippingOptions clippingOptions
		{
			get
			{
				return this.m_ClippingOptions;
			}
			set
			{
				if (this.m_ClippingOptions != value)
				{
					this.m_ClippingOptions = value;
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		public VisualElement parent
		{
			get
			{
				return this.m_LogicalParent;
			}
		}

		internal BaseVisualElementPanel elementPanel { get; private set; }

		public IPanel panel
		{
			get
			{
				return this.elementPanel;
			}
		}

		public virtual VisualElement contentContainer
		{
			get
			{
				return this;
			}
		}

		public void Add(VisualElement child)
		{
			if (this.contentContainer == this)
			{
				this.shadow.Add(child);
			}
			else
			{
				this.contentContainer.Add(child);
			}
			child.m_LogicalParent = this;
		}

		public void Insert(int index, VisualElement element)
		{
			if (this.contentContainer == this)
			{
				this.shadow.Insert(index, element);
			}
			else
			{
				this.contentContainer.Insert(index, element);
			}
			element.m_LogicalParent = this;
		}

		public void Remove(VisualElement element)
		{
			if (this.contentContainer == this)
			{
				this.shadow.Remove(element);
			}
			else
			{
				this.contentContainer.Remove(element);
			}
		}

		public void RemoveAt(int index)
		{
			if (this.contentContainer == this)
			{
				this.shadow.RemoveAt(index);
			}
			else
			{
				this.contentContainer.RemoveAt(index);
			}
		}

		public void Clear()
		{
			if (this.contentContainer == this)
			{
				this.shadow.Clear();
			}
			else
			{
				this.contentContainer.Clear();
			}
		}

		public VisualElement ElementAt(int index)
		{
			VisualElement result;
			if (this.contentContainer == this)
			{
				result = this.shadow.ElementAt(index);
			}
			else
			{
				result = this.contentContainer.ElementAt(index);
			}
			return result;
		}

		public VisualElement this[int key]
		{
			get
			{
				return this.ElementAt(key);
			}
		}

		public int childCount
		{
			get
			{
				int childCount;
				if (this.contentContainer == this)
				{
					childCount = this.shadow.childCount;
				}
				else
				{
					childCount = this.contentContainer.childCount;
				}
				return childCount;
			}
		}

		public IEnumerable<VisualElement> Children()
		{
			IEnumerable<VisualElement> result;
			if (this.contentContainer == this)
			{
				result = this.shadow.Children();
			}
			else
			{
				result = this.contentContainer.Children();
			}
			return result;
		}

		public void Sort(Comparison<VisualElement> comp)
		{
			if (this.contentContainer == this)
			{
				this.shadow.Sort(comp);
			}
			else
			{
				this.contentContainer.Sort(comp);
			}
		}

		public void RemoveFromHierarchy()
		{
			if (this.shadow.parent != null)
			{
				this.shadow.parent.shadow.Remove(this);
			}
		}

		public T GetFirstOfType<T>() where T : class
		{
			T t = this as T;
			T result;
			if (t != null)
			{
				result = t;
			}
			else
			{
				result = this.GetFirstAncestorOfType<T>();
			}
			return result;
		}

		public T GetFirstAncestorOfType<T>() where T : class
		{
			for (VisualElement parent = this.shadow.parent; parent != null; parent = parent.shadow.parent)
			{
				T t = parent as T;
				if (t != null)
				{
					return t;
				}
			}
			return (T)((object)null);
		}

		public bool Contains(VisualElement child)
		{
			while (child != null)
			{
				if (child.shadow.parent == this)
				{
					return true;
				}
				child = child.shadow.parent;
			}
			return false;
		}

		public IEnumerator<VisualElement> GetEnumerator()
		{
			IEnumerator<VisualElement> enumerator;
			if (this.contentContainer == this)
			{
				enumerator = this.shadow.Children().GetEnumerator();
			}
			else
			{
				enumerator = this.contentContainer.GetEnumerator();
			}
			return enumerator;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			IEnumerator enumerator;
			if (this.contentContainer == this)
			{
				enumerator = this.shadow.Children().GetEnumerator();
			}
			else
			{
				enumerator = ((IEnumerable)this.contentContainer).GetEnumerator();
			}
			return enumerator;
		}

		public IVisualElementScheduler schedule
		{
			get
			{
				return this;
			}
		}

		IVisualElementScheduledItem IVisualElementScheduler.Execute(Action<TimerState> timerUpdateEvent)
		{
			VisualElement.TimerStateScheduledItem timerStateScheduledItem = new VisualElement.TimerStateScheduledItem(this, timerUpdateEvent)
			{
				timerUpdateStopCondition = ScheduledItem.OnceCondition
			};
			timerStateScheduledItem.Resume();
			return timerStateScheduledItem;
		}

		IVisualElementScheduledItem IVisualElementScheduler.Execute(Action updateEvent)
		{
			VisualElement.SimpleScheduledItem simpleScheduledItem = new VisualElement.SimpleScheduledItem(this, updateEvent)
			{
				timerUpdateStopCondition = ScheduledItem.OnceCondition
			};
			simpleScheduledItem.Resume();
			return simpleScheduledItem;
		}

		public IStyle style
		{
			get
			{
				return this;
			}
		}

		private static bool ApplyAndCompare(ref StyleValue<float> current, StyleValue<float> other)
		{
			float value = current.value;
			return current.Apply(other, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity) && value != other.value;
		}

		private static bool ApplyAndCompare(ref StyleValue<int> current, StyleValue<int> other)
		{
			int value = current.value;
			return current.Apply(other, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity) && value != other.value;
		}

		private static bool ApplyAndCompare(ref StyleValue<bool> current, StyleValue<bool> other)
		{
			bool value = current.value;
			return current.Apply(other, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity) && value != other.value;
		}

		private static bool ApplyAndCompare(ref StyleValue<Color> current, StyleValue<Color> other)
		{
			Color value = current.value;
			return current.Apply(other, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity) && value != other.value;
		}

		private static bool ApplyAndCompare<T>(ref StyleValue<T> current, StyleValue<T> other) where T : Object
		{
			T value = current.value;
			return current.Apply(other, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity) && value != other.value;
		}

		StyleValue<float> IStyle.width
		{
			get
			{
				return this.effectiveStyle.width;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.width, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.Width = value.value;
				}
			}
		}

		StyleValue<float> IStyle.height
		{
			get
			{
				return this.effectiveStyle.height;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.height, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.Height = value.value;
				}
			}
		}

		StyleValue<float> IStyle.maxWidth
		{
			get
			{
				return this.effectiveStyle.maxWidth;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.maxWidth, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.MaxWidth = value.value;
				}
			}
		}

		StyleValue<float> IStyle.maxHeight
		{
			get
			{
				return this.effectiveStyle.maxHeight;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.maxHeight, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.MaxHeight = value.value;
				}
			}
		}

		StyleValue<float> IStyle.minWidth
		{
			get
			{
				return this.effectiveStyle.minWidth;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.minWidth, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.MinWidth = value.value;
				}
			}
		}

		StyleValue<float> IStyle.minHeight
		{
			get
			{
				return this.effectiveStyle.minHeight;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.minHeight, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.MinHeight = value.value;
				}
			}
		}

		StyleValue<float> IStyle.flex
		{
			get
			{
				return this.effectiveStyle.flex;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.flex, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.Flex = value.value;
				}
			}
		}

		StyleValue<Overflow> IStyle.overflow
		{
			get
			{
				return new StyleValue<Overflow>((Overflow)this.effectiveStyle.overflow.value, this.effectiveStyle.overflow.specificity);
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.overflow, new StyleValue<int>((int)value.value, value.specificity)))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.Overflow = (CSSOverflow)value.value;
				}
			}
		}

		StyleValue<float> IStyle.positionLeft
		{
			get
			{
				return this.effectiveStyle.positionLeft;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.positionLeft, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetPosition(CSSEdge.Left, value.value);
				}
			}
		}

		StyleValue<float> IStyle.positionTop
		{
			get
			{
				return this.effectiveStyle.positionTop;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.positionTop, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetPosition(CSSEdge.Top, value.value);
				}
			}
		}

		StyleValue<float> IStyle.positionRight
		{
			get
			{
				return this.effectiveStyle.positionRight;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.positionRight, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetPosition(CSSEdge.Right, value.value);
				}
			}
		}

		StyleValue<float> IStyle.positionBottom
		{
			get
			{
				return this.effectiveStyle.positionBottom;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.positionBottom, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetPosition(CSSEdge.Bottom, value.value);
				}
			}
		}

		StyleValue<float> IStyle.marginLeft
		{
			get
			{
				return this.effectiveStyle.marginLeft;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.marginLeft, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetMargin(CSSEdge.Left, value.value);
				}
			}
		}

		StyleValue<float> IStyle.marginTop
		{
			get
			{
				return this.effectiveStyle.marginTop;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.marginTop, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetMargin(CSSEdge.Top, value.value);
				}
			}
		}

		StyleValue<float> IStyle.marginRight
		{
			get
			{
				return this.effectiveStyle.marginRight;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.marginRight, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetMargin(CSSEdge.Right, value.value);
				}
			}
		}

		StyleValue<float> IStyle.marginBottom
		{
			get
			{
				return this.effectiveStyle.marginBottom;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.marginBottom, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetMargin(CSSEdge.Bottom, value.value);
				}
			}
		}

		StyleValue<float> IStyle.borderLeft
		{
			get
			{
				return this.effectiveStyle.borderLeft;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.borderLeft, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetBorder(CSSEdge.Left, value.value);
				}
			}
		}

		StyleValue<float> IStyle.borderTop
		{
			get
			{
				return this.effectiveStyle.borderTop;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.borderTop, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetBorder(CSSEdge.Top, value.value);
				}
			}
		}

		StyleValue<float> IStyle.borderRight
		{
			get
			{
				return this.effectiveStyle.borderRight;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.borderRight, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetBorder(CSSEdge.Right, value.value);
				}
			}
		}

		StyleValue<float> IStyle.borderBottom
		{
			get
			{
				return this.effectiveStyle.borderBottom;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.borderBottom, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetBorder(CSSEdge.Bottom, value.value);
				}
			}
		}

		StyleValue<float> IStyle.borderLeftWidth
		{
			get
			{
				return this.effectiveStyle.borderLeftWidth;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.borderLeftWidth, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetBorder(CSSEdge.Left, value.value);
				}
			}
		}

		StyleValue<float> IStyle.borderTopWidth
		{
			get
			{
				return this.effectiveStyle.borderTopWidth;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.borderTopWidth, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetBorder(CSSEdge.Top, value.value);
				}
			}
		}

		StyleValue<float> IStyle.borderRightWidth
		{
			get
			{
				return this.effectiveStyle.borderRightWidth;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.borderRightWidth, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetBorder(CSSEdge.Right, value.value);
				}
			}
		}

		StyleValue<float> IStyle.borderBottomWidth
		{
			get
			{
				return this.effectiveStyle.borderBottomWidth;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.borderBottomWidth, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetBorder(CSSEdge.Bottom, value.value);
				}
			}
		}

		StyleValue<float> IStyle.borderRadius
		{
			get
			{
				return this.style.borderTopLeftRadius;
			}
			set
			{
				this.style.borderTopLeftRadius = value;
				this.style.borderTopRightRadius = value;
				this.style.borderBottomLeftRadius = value;
				this.style.borderBottomRightRadius = value;
			}
		}

		StyleValue<float> IStyle.borderTopLeftRadius
		{
			get
			{
				return this.effectiveStyle.borderTopLeftRadius;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.borderTopLeftRadius, value))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<float> IStyle.borderTopRightRadius
		{
			get
			{
				return this.effectiveStyle.borderTopRightRadius;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.borderTopRightRadius, value))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<float> IStyle.borderBottomRightRadius
		{
			get
			{
				return this.effectiveStyle.borderBottomRightRadius;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.borderBottomRightRadius, value))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<float> IStyle.borderBottomLeftRadius
		{
			get
			{
				return this.effectiveStyle.borderBottomLeftRadius;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.borderBottomLeftRadius, value))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<float> IStyle.paddingLeft
		{
			get
			{
				return this.effectiveStyle.paddingLeft;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.paddingLeft, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetPadding(CSSEdge.Left, value.value);
				}
			}
		}

		StyleValue<float> IStyle.paddingTop
		{
			get
			{
				return this.effectiveStyle.paddingTop;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.paddingTop, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetPadding(CSSEdge.Top, value.value);
				}
			}
		}

		StyleValue<float> IStyle.paddingRight
		{
			get
			{
				return this.effectiveStyle.paddingRight;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.paddingRight, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetPadding(CSSEdge.Right, value.value);
				}
			}
		}

		StyleValue<float> IStyle.paddingBottom
		{
			get
			{
				return this.effectiveStyle.paddingBottom;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.paddingBottom, value))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.SetPadding(CSSEdge.Bottom, value.value);
				}
			}
		}

		StyleValue<PositionType> IStyle.positionType
		{
			get
			{
				return new StyleValue<PositionType>((PositionType)this.effectiveStyle.positionType.value, this.effectiveStyle.positionType.specificity);
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.positionType, new StyleValue<int>((int)value.value, value.specificity)))
				{
					this.Dirty(ChangeType.Layout);
					PositionType value2 = value.value;
					if (value2 != PositionType.Absolute && value2 != PositionType.Manual)
					{
						if (value2 == PositionType.Relative)
						{
							this.cssNode.PositionType = CSSPositionType.Relative;
						}
					}
					else
					{
						this.cssNode.PositionType = CSSPositionType.Absolute;
					}
				}
			}
		}

		StyleValue<Align> IStyle.alignSelf
		{
			get
			{
				return new StyleValue<Align>((Align)this.effectiveStyle.alignSelf.value, this.effectiveStyle.alignSelf.specificity);
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.alignSelf, new StyleValue<int>((int)value.value, value.specificity)))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.AlignSelf = (CSSAlign)value.value;
				}
			}
		}

		StyleValue<TextAnchor> IStyle.textAlignment
		{
			get
			{
				return new StyleValue<TextAnchor>((TextAnchor)this.effectiveStyle.textAlignment.value, this.effectiveStyle.textAlignment.specificity);
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.textAlignment, new StyleValue<int>((int)value.value, value.specificity)))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<FontStyle> IStyle.fontStyle
		{
			get
			{
				return new StyleValue<FontStyle>((FontStyle)this.effectiveStyle.fontStyle.value, this.effectiveStyle.fontStyle.specificity);
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.fontStyle, new StyleValue<int>((int)value.value, value.specificity)))
				{
					this.Dirty(ChangeType.Layout);
				}
			}
		}

		StyleValue<TextClipping> IStyle.textClipping
		{
			get
			{
				return new StyleValue<TextClipping>((TextClipping)this.effectiveStyle.textClipping.value, this.effectiveStyle.textClipping.specificity);
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.textClipping, new StyleValue<int>((int)value.value, value.specificity)))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<Font> IStyle.font
		{
			get
			{
				return this.effectiveStyle.font;
			}
			set
			{
				if (VisualElement.ApplyAndCompare<Font>(ref this.inlineStyle.font, value))
				{
					this.Dirty(ChangeType.Layout);
				}
			}
		}

		StyleValue<int> IStyle.fontSize
		{
			get
			{
				return this.effectiveStyle.fontSize;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.fontSize, value))
				{
					this.Dirty(ChangeType.Layout);
				}
			}
		}

		StyleValue<bool> IStyle.wordWrap
		{
			get
			{
				return this.effectiveStyle.wordWrap;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.wordWrap, value))
				{
					this.Dirty(ChangeType.Layout);
				}
			}
		}

		StyleValue<Color> IStyle.textColor
		{
			get
			{
				return this.effectiveStyle.textColor;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.textColor, value))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<FlexDirection> IStyle.flexDirection
		{
			get
			{
				return new StyleValue<FlexDirection>((FlexDirection)this.effectiveStyle.flexDirection.value, this.effectiveStyle.flexDirection.specificity);
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.flexDirection, new StyleValue<int>((int)value.value, value.specificity)))
				{
					this.Dirty(ChangeType.Repaint);
					this.cssNode.FlexDirection = (CSSFlexDirection)value.value;
				}
			}
		}

		StyleValue<Color> IStyle.backgroundColor
		{
			get
			{
				return this.effectiveStyle.backgroundColor;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.backgroundColor, value))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<Color> IStyle.borderColor
		{
			get
			{
				return this.effectiveStyle.borderColor;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.borderColor, value))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<Texture2D> IStyle.backgroundImage
		{
			get
			{
				return this.effectiveStyle.backgroundImage;
			}
			set
			{
				if (VisualElement.ApplyAndCompare<Texture2D>(ref this.inlineStyle.backgroundImage, value))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<ScaleMode> IStyle.backgroundSize
		{
			get
			{
				return new StyleValue<ScaleMode>((ScaleMode)this.effectiveStyle.backgroundSize.value, this.effectiveStyle.backgroundSize.specificity);
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.backgroundSize, new StyleValue<int>((int)value.value, value.specificity)))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<Align> IStyle.alignItems
		{
			get
			{
				return new StyleValue<Align>((Align)this.effectiveStyle.alignItems.value, this.effectiveStyle.alignItems.specificity);
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.alignItems, new StyleValue<int>((int)value.value, value.specificity)))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.AlignItems = (CSSAlign)value.value;
				}
			}
		}

		StyleValue<Align> IStyle.alignContent
		{
			get
			{
				return new StyleValue<Align>((Align)this.effectiveStyle.alignContent.value, this.effectiveStyle.alignContent.specificity);
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.alignContent, new StyleValue<int>((int)value.value, value.specificity)))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.AlignContent = (CSSAlign)value.value;
				}
			}
		}

		StyleValue<Justify> IStyle.justifyContent
		{
			get
			{
				return new StyleValue<Justify>((Justify)this.effectiveStyle.justifyContent.value, this.effectiveStyle.justifyContent.specificity);
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.justifyContent, new StyleValue<int>((int)value.value, value.specificity)))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.JustifyContent = (CSSJustify)value.value;
				}
			}
		}

		StyleValue<Wrap> IStyle.flexWrap
		{
			get
			{
				return new StyleValue<Wrap>((Wrap)this.effectiveStyle.flexWrap.value, this.effectiveStyle.flexWrap.specificity);
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.flexWrap, new StyleValue<int>((int)value.value, value.specificity)))
				{
					this.Dirty(ChangeType.Layout);
					this.cssNode.Wrap = (CSSWrap)value.value;
				}
			}
		}

		StyleValue<int> IStyle.sliceLeft
		{
			get
			{
				return this.effectiveStyle.sliceLeft;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.sliceLeft, value))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<int> IStyle.sliceTop
		{
			get
			{
				return this.effectiveStyle.sliceTop;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.sliceTop, value))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<int> IStyle.sliceRight
		{
			get
			{
				return this.effectiveStyle.sliceRight;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.sliceRight, value))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<int> IStyle.sliceBottom
		{
			get
			{
				return this.effectiveStyle.sliceBottom;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.sliceBottom, value))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		StyleValue<float> IStyle.opacity
		{
			get
			{
				return this.effectiveStyle.opacity;
			}
			set
			{
				if (VisualElement.ApplyAndCompare(ref this.inlineStyle.opacity, value))
				{
					this.Dirty(ChangeType.Repaint);
				}
			}
		}

		internal IEnumerable<StyleSheet> styleSheets
		{
			get
			{
				if (this.m_StyleSheets == null && this.m_StyleSheetPaths != null)
				{
					this.LoadStyleSheetsFromPaths();
				}
				return this.m_StyleSheets;
			}
		}

		public void AddStyleSheetPath(string sheetPath)
		{
			if (this.m_StyleSheetPaths == null)
			{
				this.m_StyleSheetPaths = new List<string>();
			}
			this.m_StyleSheetPaths.Add(sheetPath);
			this.m_StyleSheets = null;
			this.Dirty(ChangeType.Styles);
		}

		public void RemoveStyleSheetPath(string sheetPath)
		{
			if (this.m_StyleSheetPaths == null)
			{
				Debug.LogWarning("Attempting to remove from null style sheet path list");
			}
			else
			{
				this.m_StyleSheetPaths.Remove(sheetPath);
				this.m_StyleSheets = null;
				this.Dirty(ChangeType.Styles);
			}
		}

		public bool HasStyleSheetPath(string sheetPath)
		{
			return this.m_StyleSheetPaths != null && this.m_StyleSheetPaths.Contains(sheetPath);
		}

		internal void LoadStyleSheetsFromPaths()
		{
			if (this.m_StyleSheetPaths != null && this.elementPanel != null)
			{
				this.m_StyleSheets = new List<StyleSheet>();
				foreach (string text in this.m_StyleSheetPaths)
				{
					StyleSheet styleSheet = Panel.loadResourceFunc(text, typeof(StyleSheet)) as StyleSheet;
					if (styleSheet != null)
					{
						int i = 0;
						int num = styleSheet.complexSelectors.Length;
						while (i < num)
						{
							styleSheet.complexSelectors[i].CachePseudoStateMasks();
							i++;
						}
						this.m_StyleSheets.Add(styleSheet);
					}
					else
					{
						Debug.LogWarning(string.Format("Style sheet not found for path \"{0}\"", text));
					}
				}
			}
		}

		public enum MeasureMode
		{
			Undefined,
			Exactly,
			AtMost
		}

		private class DataWatchRequest : IUIElementDataWatchRequest, IVisualElementPanelActivatable, IDisposable
		{
			private VisualElementPanelActivator m_Activator;

			public DataWatchRequest(VisualElement handler)
			{
				this.element = handler;
				this.m_Activator = new VisualElementPanelActivator(this);
			}

			public Action<Object> notification { get; set; }

			public Object watchedObject { get; set; }

			public IDataWatchHandle requestedHandle { get; set; }

			public VisualElement element { get; set; }

			public void Start()
			{
				this.m_Activator.SetActive(true);
			}

			public void Stop()
			{
				this.m_Activator.SetActive(false);
			}

			public bool CanBeActivated()
			{
				return this.element != null && this.element.elementPanel != null && this.element.elementPanel.dataWatch != null;
			}

			public void OnPanelActivate()
			{
				if (this.requestedHandle == null)
				{
					this.requestedHandle = this.element.elementPanel.dataWatch.AddWatch(this.watchedObject, this.notification);
				}
			}

			public void OnPanelDeactivate()
			{
				if (this.requestedHandle != null)
				{
					this.element.elementPanel.dataWatch.RemoveWatch(this.requestedHandle);
					this.requestedHandle = null;
				}
			}

			public void Dispose()
			{
				this.Stop();
			}
		}

		public enum ClippingOptions
		{
			ClipContents,
			NoClipping,
			ClipAndCacheContents
		}

		public struct Hierarchy
		{
			private readonly VisualElement m_Owner;

			internal Hierarchy(VisualElement element)
			{
				this.m_Owner = element;
			}

			public VisualElement parent
			{
				get
				{
					return this.m_Owner.m_PhysicalParent;
				}
			}

			public void Add(VisualElement child)
			{
				if (child == null)
				{
					throw new ArgumentException("Cannot add null child");
				}
				this.Insert(this.childCount, child);
			}

			public void Insert(int index, VisualElement child)
			{
				if (child == null)
				{
					throw new ArgumentException("Cannot insert null child");
				}
				if (index > this.childCount)
				{
					throw new IndexOutOfRangeException("Index out of range: " + index);
				}
				if (child == this.m_Owner)
				{
					throw new ArgumentException("Cannot insert element as its own child");
				}
				child.RemoveFromHierarchy();
				child.shadow.SetParent(this.m_Owner);
				if (this.m_Owner.m_Children == null)
				{
					this.m_Owner.m_Children = new List<VisualElement>();
				}
				if (this.m_Owner.cssNode.IsMeasureDefined)
				{
					this.m_Owner.cssNode.SetMeasureFunction(null);
				}
				if (index >= this.m_Owner.m_Children.Count)
				{
					this.m_Owner.m_Children.Add(child);
					this.m_Owner.cssNode.Insert(this.m_Owner.cssNode.Count, child.cssNode);
				}
				else
				{
					this.m_Owner.m_Children.Insert(index, child);
					this.m_Owner.cssNode.Insert(index, child.cssNode);
				}
				child.SetEnabledFromHierarchy(this.m_Owner.enabledInHierarchy);
				child.Dirty(ChangeType.Styles);
				this.m_Owner.Dirty(ChangeType.Layout);
				if (!string.IsNullOrEmpty(child.persistenceKey))
				{
					child.Dirty(ChangeType.PersistentData);
				}
			}

			public void Remove(VisualElement child)
			{
				if (child == null)
				{
					throw new ArgumentException("Cannot remove null child");
				}
				if (child.shadow.parent != this.m_Owner)
				{
					throw new ArgumentException("This visualElement is not my child");
				}
				if (this.m_Owner.m_Children != null)
				{
					int index = this.m_Owner.m_Children.IndexOf(child);
					this.RemoveAt(index);
				}
			}

			public void RemoveAt(int index)
			{
				if (index < 0 || index >= this.childCount)
				{
					throw new IndexOutOfRangeException("Index out of range: " + index);
				}
				VisualElement visualElement = this.m_Owner.m_Children[index];
				visualElement.shadow.SetParent(null);
				this.m_Owner.m_Children.RemoveAt(index);
				this.m_Owner.cssNode.RemoveAt(index);
				if (this.childCount == 0)
				{
					this.m_Owner.cssNode.SetMeasureFunction(new MeasureFunction(this.m_Owner.Measure));
				}
				this.m_Owner.Dirty(ChangeType.Layout);
			}

			public void Clear()
			{
				if (this.childCount > 0)
				{
					foreach (VisualElement visualElement in this.m_Owner.m_Children)
					{
						visualElement.shadow.SetParent(null);
						visualElement.m_LogicalParent = null;
					}
					this.m_Owner.m_Children.Clear();
					this.m_Owner.cssNode.Clear();
					this.m_Owner.Dirty(ChangeType.Layout);
				}
			}

			public int childCount
			{
				get
				{
					return (this.m_Owner.m_Children == null) ? 0 : this.m_Owner.m_Children.Count;
				}
			}

			public VisualElement this[int key]
			{
				get
				{
					return this.ElementAt(key);
				}
			}

			public VisualElement ElementAt(int index)
			{
				if (this.m_Owner.m_Children != null)
				{
					return this.m_Owner.m_Children[index];
				}
				throw new IndexOutOfRangeException("Index out of range: " + index);
			}

			public IEnumerable<VisualElement> Children()
			{
				IEnumerable<VisualElement> result;
				if (this.m_Owner.m_Children != null)
				{
					result = this.m_Owner.m_Children;
				}
				else
				{
					result = VisualElement.s_EmptyList;
				}
				return result;
			}

			private void SetParent(VisualElement value)
			{
				this.m_Owner.m_PhysicalParent = value;
				this.m_Owner.m_LogicalParent = value;
				if (value != null)
				{
					this.m_Owner.ChangePanel(this.m_Owner.m_PhysicalParent.elementPanel);
					this.m_Owner.PropagateChangesToParents();
				}
				else
				{
					this.m_Owner.ChangePanel(null);
				}
			}

			public void Sort(Comparison<VisualElement> comp)
			{
				this.m_Owner.m_Children.Sort(comp);
				this.m_Owner.cssNode.Clear();
				for (int i = 0; i < this.m_Owner.m_Children.Count; i++)
				{
					this.m_Owner.cssNode.Insert(i, this.m_Owner.m_Children[i].cssNode);
				}
				this.m_Owner.Dirty(ChangeType.Layout);
			}
		}

		private abstract class BaseVisualElementScheduledItem : ScheduledItem, IVisualElementScheduledItem, IVisualElementPanelActivatable
		{
			public bool isScheduled = false;

			private VisualElementPanelActivator m_Activator;

			protected BaseVisualElementScheduledItem(VisualElement handler)
			{
				this.element = handler;
				this.m_Activator = new VisualElementPanelActivator(this);
			}

			public VisualElement element { get; private set; }

			public bool isActive
			{
				get
				{
					return this.m_Activator.isActive;
				}
			}

			public IVisualElementScheduledItem StartingIn(long delayMs)
			{
				base.delayMs = delayMs;
				return this;
			}

			public IVisualElementScheduledItem Until(Func<bool> stopCondition)
			{
				if (stopCondition == null)
				{
					stopCondition = ScheduledItem.ForeverCondition;
				}
				this.timerUpdateStopCondition = stopCondition;
				return this;
			}

			public IVisualElementScheduledItem ForDuration(long durationMs)
			{
				base.SetDuration(durationMs);
				return this;
			}

			public IVisualElementScheduledItem Every(long intervalMs)
			{
				base.intervalMs = intervalMs;
				if (this.timerUpdateStopCondition == ScheduledItem.OnceCondition)
				{
					this.timerUpdateStopCondition = ScheduledItem.ForeverCondition;
				}
				return this;
			}

			internal override void OnItemUnscheduled()
			{
				base.OnItemUnscheduled();
				this.isScheduled = false;
				if (!this.m_Activator.isDetaching)
				{
					this.m_Activator.SetActive(false);
				}
			}

			public void Resume()
			{
				this.isScheduled = true;
				this.m_Activator.SetActive(true);
			}

			public void Pause()
			{
				this.m_Activator.SetActive(false);
			}

			public void ExecuteLater(long delayMs)
			{
				if (!this.isScheduled)
				{
					this.Resume();
				}
				base.ResetStartTime();
				this.StartingIn(delayMs);
			}

			public void OnPanelActivate()
			{
				this.isScheduled = true;
				base.ResetStartTime();
				this.element.elementPanel.scheduler.Schedule(this);
			}

			public void OnPanelDeactivate()
			{
				if (this.isScheduled)
				{
					this.isScheduled = false;
					this.element.elementPanel.scheduler.Unschedule(this);
				}
			}

			public bool CanBeActivated()
			{
				return this.element != null && this.element.elementPanel != null && this.element.elementPanel.scheduler != null;
			}
		}

		private abstract class VisualElementScheduledItem<ActionType> : VisualElement.BaseVisualElementScheduledItem
		{
			public ActionType updateEvent;

			public VisualElementScheduledItem(VisualElement handler, ActionType upEvent) : base(handler)
			{
				this.updateEvent = upEvent;
			}

			public static bool Matches(ScheduledItem item, ActionType updateEvent)
			{
				VisualElement.VisualElementScheduledItem<ActionType> visualElementScheduledItem = item as VisualElement.VisualElementScheduledItem<ActionType>;
				return visualElementScheduledItem != null && EqualityComparer<ActionType>.Default.Equals(visualElementScheduledItem.updateEvent, updateEvent);
			}
		}

		private class TimerStateScheduledItem : VisualElement.VisualElementScheduledItem<Action<TimerState>>
		{
			public TimerStateScheduledItem(VisualElement handler, Action<TimerState> updateEvent) : base(handler, updateEvent)
			{
			}

			public override void PerformTimerUpdate(TimerState state)
			{
				if (this.isScheduled)
				{
					this.updateEvent(state);
				}
			}
		}

		private class SimpleScheduledItem : VisualElement.VisualElementScheduledItem<Action>
		{
			public SimpleScheduledItem(VisualElement handler, Action updateEvent) : base(handler, updateEvent)
			{
			}

			public override void PerformTimerUpdate(TimerState state)
			{
				if (this.isScheduled)
				{
					this.updateEvent();
				}
			}
		}
	}
}
