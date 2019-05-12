using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	[ExecuteInEditMode]
	[AddComponentMenu("UI/2D Rect Mask", 13)]
	[RequireComponent(typeof(RectTransform))]
	[DisallowMultipleComponent]
	public class RectMask2D : UIBehaviour, ICanvasRaycastFilter, IClipper
	{
		[NonSerialized]
		private readonly RectangularVertexClipper m_VertexClipper = new RectangularVertexClipper();

		[NonSerialized]
		private RectTransform m_RectTransform;

		[NonSerialized]
		private List<IClippable> m_ClipTargets = new List<IClippable>();

		[NonSerialized]
		private bool m_ShouldRecalculateClipRects;

		[NonSerialized]
		private List<RectMask2D> m_Clippers = new List<RectMask2D>();

		[NonSerialized]
		private Rect m_LastClipRectCanvasSpace;

		[NonSerialized]
		private bool m_LastValidClipRect;

		[NonSerialized]
		private bool m_ForceClip;

		protected RectMask2D()
		{
		}

		public Rect canvasRect
		{
			get
			{
				Canvas c = null;
				List<Canvas> list = ListPool<Canvas>.Get();
				base.gameObject.GetComponentsInParent<Canvas>(false, list);
				if (list.Count > 0)
				{
					c = list[list.Count - 1];
				}
				ListPool<Canvas>.Release(list);
				return this.m_VertexClipper.GetCanvasRect(this.rectTransform, c);
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

		public virtual void PerformClipping()
		{
			if (this.m_ShouldRecalculateClipRects)
			{
				MaskUtilities.GetRectMasksForClip(this, this.m_Clippers);
				this.m_ShouldRecalculateClipRects = false;
			}
			bool flag = true;
			Rect rect = Clipping.FindCullAndClipWorldRect(this.m_Clippers, out flag);
			if (rect != this.m_LastClipRectCanvasSpace || this.m_ForceClip)
			{
				for (int i = 0; i < this.m_ClipTargets.Count; i++)
				{
					this.m_ClipTargets[i].SetClipRect(rect, flag);
				}
				this.m_LastClipRectCanvasSpace = rect;
				this.m_LastValidClipRect = flag;
			}
			for (int j = 0; j < this.m_ClipTargets.Count; j++)
			{
				this.m_ClipTargets[j].Cull(this.m_LastClipRectCanvasSpace, this.m_LastValidClipRect);
			}
		}

		public void AddClippable(IClippable clippable)
		{
			if (clippable == null)
			{
				return;
			}
			if (!this.m_ClipTargets.Contains(clippable))
			{
				this.m_ClipTargets.Add(clippable);
			}
			this.m_ForceClip = true;
		}

		public void RemoveClippable(IClippable clippable)
		{
			if (clippable == null)
			{
				return;
			}
			clippable.SetClipRect(default(Rect), false);
			this.m_ClipTargets.Remove(clippable);
			this.m_ForceClip = true;
		}

		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.m_ShouldRecalculateClipRects = true;
		}

		protected override void OnCanvasHierarchyChanged()
		{
			base.OnCanvasHierarchyChanged();
			this.m_ShouldRecalculateClipRects = true;
		}
	}
}
