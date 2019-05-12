using System;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
	internal class Panel : BaseVisualElementPanel
	{
		private StyleContext m_StyleContext;

		private VisualElement m_RootContainer;

		private IDataWatchService m_DataWatch;

		private TimerEventScheduler m_Scheduler;

		internal static LoadResourceFunction loadResourceFunc = null;

		private bool m_KeepPixelCacheOnWorldBoundChange;

		private const int kMaxValidatePersistentDataCount = 5;

		private const int kMaxValidateLayoutCount = 5;

		public Panel(ScriptableObject ownerObject, ContextType contextType, IDataWatchService dataWatch = null, IEventDispatcher dispatcher = null)
		{
			this.ownerObject = ownerObject;
			this.contextType = contextType;
			this.m_DataWatch = dataWatch;
			this.dispatcher = dispatcher;
			this.stylePainter = new StylePainter();
			this.m_RootContainer = new VisualElement();
			this.m_RootContainer.name = VisualElementUtils.GetUniqueName("PanelContainer");
			this.m_RootContainer.persistenceKey = "PanelContainer";
			this.visualTree.ChangePanel(this);
			this.focusController = new FocusController(new VisualElementFocusRing(this.visualTree, VisualElementFocusRing.DefaultFocusOrder.ChildOrder));
			this.m_StyleContext = new StyleContext(this.m_RootContainer);
			this.allowPixelCaching = true;
		}

		public override VisualElement visualTree
		{
			get
			{
				return this.m_RootContainer;
			}
		}

		public override IEventDispatcher dispatcher { get; protected set; }

		internal override IDataWatchService dataWatch
		{
			get
			{
				return this.m_DataWatch;
			}
		}

		public TimerEventScheduler timerEventScheduler
		{
			get
			{
				TimerEventScheduler result;
				if ((result = this.m_Scheduler) == null)
				{
					result = (this.m_Scheduler = new TimerEventScheduler());
				}
				return result;
			}
		}

		internal override IScheduler scheduler
		{
			get
			{
				return this.timerEventScheduler;
			}
		}

		internal StyleContext styleContext
		{
			get
			{
				return this.m_StyleContext;
			}
		}

		public override ScriptableObject ownerObject { get; protected set; }

		public bool allowPixelCaching { get; set; }

		public override ContextType contextType { get; protected set; }

		public override SavePersistentViewData savePersistentViewData { get; set; }

		public override GetViewDataDictionary getViewDataDictionary { get; set; }

		public override FocusController focusController { get; set; }

		public override EventInterests IMGUIEventInterests { get; set; }

		public override bool keepPixelCacheOnWorldBoundChange
		{
			get
			{
				return this.m_KeepPixelCacheOnWorldBoundChange;
			}
			set
			{
				if (this.m_KeepPixelCacheOnWorldBoundChange != value)
				{
					this.m_KeepPixelCacheOnWorldBoundChange = value;
					if (!value)
					{
						this.m_RootContainer.Dirty(ChangeType.Transform | ChangeType.Repaint);
					}
				}
			}
		}

		public override int IMGUIContainersCount { get; set; }

		private VisualElement PickAll(VisualElement root, Vector2 point, List<VisualElement> picked = null)
		{
			VisualElement result;
			if ((root.pseudoStates & PseudoStates.Invisible) == PseudoStates.Invisible)
			{
				result = null;
			}
			else
			{
				Vector3 vector = root.transform.matrix.inverse.MultiplyPoint3x4(point);
				bool flag = root.ContainsPoint(vector);
				if (!flag && root.clippingOptions != VisualElement.ClippingOptions.NoClipping)
				{
					result = null;
				}
				else
				{
					if (picked != null && root.enabledInHierarchy && root.pickingMode == PickingMode.Position)
					{
						picked.Add(root);
					}
					vector -= new Vector3(root.layout.position.x, root.layout.position.y, 0f);
					VisualElement visualElement = null;
					for (int i = root.shadow.childCount - 1; i >= 0; i--)
					{
						VisualElement root2 = root.shadow[i];
						VisualElement visualElement2 = this.PickAll(root2, vector, picked);
						if (visualElement == null && visualElement2 != null)
						{
							visualElement = visualElement2;
						}
					}
					if (visualElement != null)
					{
						result = visualElement;
					}
					else
					{
						PickingMode pickingMode = root.pickingMode;
						if (pickingMode != PickingMode.Position)
						{
							if (pickingMode != PickingMode.Ignore)
							{
							}
						}
						else if (flag && root.enabledInHierarchy)
						{
							return root;
						}
						result = null;
					}
				}
			}
			return result;
		}

		public override VisualElement LoadTemplate(string path, Dictionary<string, VisualElement> slots = null)
		{
			VisualTreeAsset visualTreeAsset = Panel.loadResourceFunc(path, typeof(VisualTreeAsset)) as VisualTreeAsset;
			VisualElement result;
			if (visualTreeAsset == null)
			{
				result = null;
			}
			else
			{
				result = visualTreeAsset.CloneTree(slots);
			}
			return result;
		}

		public override VisualElement PickAll(Vector2 point, List<VisualElement> picked)
		{
			this.ValidateLayout();
			if (picked != null)
			{
				picked.Clear();
			}
			return this.PickAll(this.visualTree, point, picked);
		}

		public override VisualElement Pick(Vector2 point)
		{
			this.ValidateLayout();
			return this.PickAll(this.visualTree, point, null);
		}

		private void ValidatePersistentData()
		{
			int num = 0;
			while (this.visualTree.AnyDirty(ChangeType.PersistentData | ChangeType.PersistentDataPath))
			{
				this.ValidatePersistentDataOnSubTree(this.visualTree, true);
				num++;
				if (num > 5)
				{
					Debug.LogError("UIElements: Too many children recursively added that rely on persistent data: " + this.visualTree);
					break;
				}
			}
		}

		private void ValidatePersistentDataOnSubTree(VisualElement root, bool enablePersistence)
		{
			if (!root.IsPersitenceSupportedOnChildren())
			{
				enablePersistence = false;
			}
			if (root.IsDirty(ChangeType.PersistentData))
			{
				root.OnPersistentDataReady(enablePersistence);
				root.ClearDirty(ChangeType.PersistentData);
			}
			if (root.IsDirty(ChangeType.PersistentDataPath))
			{
				for (int i = 0; i < root.shadow.childCount; i++)
				{
					this.ValidatePersistentDataOnSubTree(root.shadow[i], enablePersistence);
				}
				root.ClearDirty(ChangeType.PersistentDataPath);
			}
		}

		private void ValidateStyling()
		{
			if (!Mathf.Approximately(this.m_StyleContext.currentPixelsPerPoint, GUIUtility.pixelsPerPoint))
			{
				this.m_RootContainer.Dirty(ChangeType.Styles);
				this.m_StyleContext.currentPixelsPerPoint = GUIUtility.pixelsPerPoint;
			}
			if (this.m_RootContainer.AnyDirty(ChangeType.Styles | ChangeType.StylesPath))
			{
				this.m_StyleContext.ApplyStyles();
			}
		}

		public override void ValidateLayout()
		{
			this.ValidateStyling();
			int num = 0;
			while (this.visualTree.cssNode.IsDirty)
			{
				this.visualTree.cssNode.CalculateLayout();
				this.ValidateSubTree(this.visualTree);
				if (num++ >= 5)
				{
					Debug.LogError("ValidateLayout is struggling to process current layout (consider simplifying to avoid recursive layout): " + this.visualTree);
					break;
				}
			}
		}

		private bool ValidateSubTree(VisualElement root)
		{
			if (root.renderData.lastLayout != new Rect(root.cssNode.LayoutX, root.cssNode.LayoutY, root.cssNode.LayoutWidth, root.cssNode.LayoutHeight))
			{
				root.Dirty(ChangeType.Transform);
				root.renderData.lastLayout = new Rect(root.cssNode.LayoutX, root.cssNode.LayoutY, root.cssNode.LayoutWidth, root.cssNode.LayoutHeight);
			}
			bool hasNewLayout = root.cssNode.HasNewLayout;
			if (hasNewLayout)
			{
				for (int i = 0; i < root.shadow.childCount; i++)
				{
					this.ValidateSubTree(root.shadow[i]);
				}
			}
			PostLayoutEvent pooled = PostLayoutEvent.GetPooled(hasNewLayout);
			pooled.target = root;
			UIElementsUtility.eventDispatcher.DispatchEvent(pooled, this);
			EventBase<PostLayoutEvent>.ReleasePooled(pooled);
			root.ClearDirty(ChangeType.Layout);
			root.cssNode.MarkLayoutSeen();
			return hasNewLayout;
		}

		private Rect ComputeAAAlignedBound(Rect position, Matrix4x4 mat)
		{
			Rect rect = position;
			Vector3 vector = mat.MultiplyPoint3x4(new Vector3(rect.x, rect.y, 0f));
			Vector3 vector2 = mat.MultiplyPoint3x4(new Vector3(rect.x + rect.width, rect.y, 0f));
			Vector3 vector3 = mat.MultiplyPoint3x4(new Vector3(rect.x, rect.y + rect.height, 0f));
			Vector3 vector4 = mat.MultiplyPoint3x4(new Vector3(rect.x + rect.width, rect.y + rect.height, 0f));
			return Rect.MinMaxRect(Mathf.Min(vector.x, Mathf.Min(vector2.x, Mathf.Min(vector3.x, vector4.x))), Mathf.Min(vector.y, Mathf.Min(vector2.y, Mathf.Min(vector3.y, vector4.y))), Mathf.Max(vector.x, Mathf.Max(vector2.x, Mathf.Max(vector3.x, vector4.x))), Mathf.Max(vector.y, Mathf.Max(vector2.y, Mathf.Max(vector3.y, vector4.y))));
		}

		private bool ShouldUsePixelCache(VisualElement root)
		{
			return this.allowPixelCaching && root.clippingOptions == VisualElement.ClippingOptions.ClipAndCacheContents && root.worldBound.size.magnitude > Mathf.Epsilon;
		}

		public void PaintSubTree(Event e, VisualElement root, Matrix4x4 offset, Rect currentGlobalClip)
		{
			if ((root.pseudoStates & PseudoStates.Invisible) != PseudoStates.Invisible)
			{
				if (root.clippingOptions != VisualElement.ClippingOptions.NoClipping)
				{
					Rect rect = this.ComputeAAAlignedBound(root.layout, offset * root.worldTransform);
					if (!rect.Overlaps(currentGlobalClip))
					{
						return;
					}
					float num = Mathf.Max(rect.x, currentGlobalClip.x);
					float num2 = Mathf.Min(rect.x + rect.width, currentGlobalClip.x + currentGlobalClip.width);
					float num3 = Mathf.Max(rect.y, currentGlobalClip.y);
					float num4 = Mathf.Min(rect.y + rect.height, currentGlobalClip.y + currentGlobalClip.height);
					currentGlobalClip = new Rect(num, num3, num2 - num, num4 - num3);
				}
				if (this.ShouldUsePixelCache(root))
				{
					IStylePainter stylePainter = this.stylePainter;
					Rect worldBound = root.worldBound;
					int num5 = (int)worldBound.width;
					int num6 = (int)worldBound.height;
					int num7 = (int)(worldBound.width * GUIUtility.pixelsPerPoint);
					int num8 = (int)(worldBound.height * GUIUtility.pixelsPerPoint);
					RenderTexture renderTexture = root.renderData.pixelCache;
					if (renderTexture != null && !this.keepPixelCacheOnWorldBoundChange && (renderTexture.width != num7 || renderTexture.height != num8))
					{
						Object.DestroyImmediate(renderTexture);
						renderTexture = (root.renderData.pixelCache = null);
					}
					float opacity = this.stylePainter.opacity;
					if (root.IsDirty(ChangeType.Repaint) || root.renderData.pixelCache == null)
					{
						if (renderTexture == null)
						{
							renderTexture = (root.renderData.pixelCache = new RenderTexture(num7, num8, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear));
						}
						RenderTexture active = RenderTexture.active;
						RenderTexture.active = renderTexture;
						GL.Clear(true, true, new Color(0f, 0f, 0f, 0f));
						offset = Matrix4x4.Translate(new Vector3(-worldBound.x, -worldBound.y, 0f));
						Rect rect2 = new Rect(0f, 0f, (float)num5, (float)num6);
						stylePainter.currentTransform = offset * root.worldTransform;
						GUIClip.SetTransform(stylePainter.currentTransform, rect2);
						stylePainter.currentWorldClip = rect2;
						root.DoRepaint(stylePainter);
						root.ClearDirty(ChangeType.Repaint);
						this.PaintSubTreeChildren(e, root, offset, rect2);
						RenderTexture.active = active;
					}
					stylePainter.currentWorldClip = currentGlobalClip;
					stylePainter.currentTransform = root.worldTransform;
					GUIClip.SetTransform(stylePainter.currentTransform, currentGlobalClip);
					TextureStylePainterParameters painterParams = new TextureStylePainterParameters
					{
						layout = root.layout,
						texture = root.renderData.pixelCache,
						color = Color.white,
						scaleMode = ScaleMode.ScaleAndCrop
					};
					stylePainter.DrawTexture(painterParams);
				}
				else
				{
					this.stylePainter.currentTransform = offset * root.worldTransform;
					GUIClip.SetTransform(this.stylePainter.currentTransform, currentGlobalClip);
					this.stylePainter.currentWorldClip = currentGlobalClip;
					this.stylePainter.mousePosition = root.worldTransform.inverse.MultiplyPoint3x4(e.mousePosition);
					this.stylePainter.opacity = root.style.opacity.GetSpecifiedValueOrDefault(1f);
					root.DoRepaint(this.stylePainter);
					this.stylePainter.opacity = 1f;
					root.ClearDirty(ChangeType.Repaint);
					this.PaintSubTreeChildren(e, root, offset, currentGlobalClip);
				}
			}
		}

		private void PaintSubTreeChildren(Event e, VisualElement root, Matrix4x4 offset, Rect textureClip)
		{
			int childCount = root.shadow.childCount;
			for (int i = 0; i < childCount; i++)
			{
				VisualElement root2 = root.shadow[i];
				this.PaintSubTree(e, root2, offset, textureClip);
				if (childCount != root.shadow.childCount)
				{
					throw new NotImplementedException("Visual tree is read-only during repaint");
				}
			}
		}

		public override void Repaint(Event e)
		{
			this.ValidatePersistentData();
			this.ValidateLayout();
			this.stylePainter.repaintEvent = e;
			Rect currentGlobalClip = (this.visualTree.clippingOptions != VisualElement.ClippingOptions.NoClipping) ? this.visualTree.layout : GUIClip.topmostRect;
			this.PaintSubTree(e, this.visualTree, Matrix4x4.identity, currentGlobalClip);
		}
	}
}
