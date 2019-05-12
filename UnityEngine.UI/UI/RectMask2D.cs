using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Rect Mask 2D", 13)]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public class RectMask2D : UIBehaviour, IClipper, ICanvasRaycastFilter
	{
		[NonSerialized]
		private readonly RectangularVertexClipper m_VertexClipper = new RectangularVertexClipper();

		[NonSerialized]
		private RectTransform m_RectTransform;

		[NonSerialized]
		private HashSet<IClippable> m_ClipTargets = new HashSet<IClippable>();

		[NonSerialized]
		private bool m_ShouldRecalculateClipRects;

		[NonSerialized]
		private List<RectMask2D> m_Clippers = new List<RectMask2D>();

		[NonSerialized]
		private Rect m_LastClipRectCanvasSpace;

		[NonSerialized]
		private bool m_ForceClip;

		[NonSerialized]
		private Canvas m_Canvas;

		private Vector3[] m_Corners = new Vector3[4];

		protected RectMask2D()
		{
		}

		private Canvas Canvas
		{
			get
			{
				if (this.m_Canvas == null)
				{
					List<Canvas> list = ListPool<Canvas>.Get();
					base.gameObject.GetComponentsInParent<Canvas>(false, list);
					if (list.Count > 0)
					{
						this.m_Canvas = list[list.Count - 1];
					}
					else
					{
						this.m_Canvas = null;
					}
					ListPool<Canvas>.Release(list);
				}
				return this.m_Canvas;
			}
		}

		public Rect canvasRect
		{
			get
			{
				return this.m_VertexClipper.GetCanvasRect(this.rectTransform, this.Canvas);
			}
		}

		public RectTransform rectTransform
		{
			get
			{
				RectTransform result;
				if ((result = this.m_RectTransform) == null)
				{
					result = (this.m_RectTransform = base.GetComponent<RectTransform>());
				}
				return result;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			this.m_ShouldRecalculateClipRects = true;
			ClipperRegistry.Register(this);
			MaskUtilities.Notify2DMaskStateChanged(this);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			this.m_ClipTargets.Clear();
			this.m_Clippers.Clear();
			ClipperRegistry.Unregister(this);
			MaskUtilities.Notify2DMaskStateChanged(this);
		}

		public virtual bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
		{
			return !base.isActiveAndEnabled || RectTransformUtility.RectangleContainsScreenPoint(this.rectTransform, sp, eventCamera);
		}

		private Rect rootCanvasRect
		{
			get
			{
				this.rectTransform.GetWorldCorners(this.m_Corners);
				if (!object.ReferenceEquals(this.Canvas, null))
				{
					Canvas rootCanvas = this.Canvas.rootCanvas;
					for (int i = 0; i < 4; i++)
					{
						this.m_Corners[i] = rootCanvas.transform.InverseTransformPoint(this.m_Corners[i]);
					}
				}
				return new Rect(this.m_Corners[0].x, this.m_Corners[0].y, this.m_Corners[2].x - this.m_Corners[0].x, this.m_Corners[2].y - this.m_Corners[0].y);
			}
		}

		public virtual void PerformClipping()
		{
			if (!object.ReferenceEquals(this.Canvas, null))
			{
				if (this.m_ShouldRecalculateClipRects)
				{
					MaskUtilities.GetRectMasksForClip(this, this.m_Clippers);
					this.m_ShouldRecalculateClipRects = false;
				}
				bool flag = true;
				Rect rect = Clipping.FindCullAndClipWorldRect(this.m_Clippers, out flag);
				RenderMode renderMode = this.Canvas.rootCanvas.renderMode;
				bool flag2 = (renderMode == RenderMode.ScreenSpaceCamera || renderMode == RenderMode.ScreenSpaceOverlay) && !rect.Overlaps(this.rootCanvasRect, true);
				bool flag3 = rect != this.m_LastClipRectCanvasSpace;
				bool forceClip = this.m_ForceClip;
				foreach (IClippable clippable in this.m_ClipTargets)
				{
					if (flag3 || forceClip)
					{
						clippable.SetClipRect(rect, flag);
					}
					MaskableGraphic maskableGraphic = clippable as MaskableGraphic;
					if (!(maskableGraphic != null) || maskableGraphic.canvasRenderer.hasMoved || flag3)
					{
						clippable.Cull((!flag2) ? rect : Rect.zero, !flag2 && flag);
					}
				}
				this.m_LastClipRectCanvasSpace = rect;
				this.m_ForceClip = false;
			}
		}

		public void AddClippable(IClippable clippable)
		{
			if (clippable != null)
			{
				this.m_ShouldRecalculateClipRects = true;
				if (!this.m_ClipTargets.Contains(clippable))
				{
					this.m_ClipTargets.Add(clippable);
				}
				this.m_ForceClip = true;
			}
		}

		public void RemoveClippable(IClippable clippable)
		{
			if (clippable != null)
			{
				this.m_ShouldRecalculateClipRects = true;
				clippable.SetClipRect(default(Rect), false);
				this.m_ClipTargets.Remove(clippable);
				this.m_ForceClip = true;
			}
		}

		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.m_ShouldRecalculateClipRects = true;
		}

		protected override void OnCanvasHierarchyChanged()
		{
			this.m_Canvas = null;
			base.OnCanvasHierarchyChanged();
			this.m_ShouldRecalculateClipRects = true;
		}
	}
}
