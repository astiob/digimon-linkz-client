using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	[AddComponentMenu("Layout/Content Size Fitter", 141)]
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class ContentSizeFitter : UIBehaviour, ILayoutController, ILayoutSelfController
	{
		[SerializeField]
		protected ContentSizeFitter.FitMode m_HorizontalFit;

		[SerializeField]
		protected ContentSizeFitter.FitMode m_VerticalFit;

		[NonSerialized]
		private RectTransform m_Rect;

		private DrivenRectTransformTracker m_Tracker;

		protected ContentSizeFitter()
		{
		}

		public ContentSizeFitter.FitMode horizontalFit
		{
			get
			{
				return this.m_HorizontalFit;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<ContentSizeFitter.FitMode>(ref this.m_HorizontalFit, value))
				{
					this.SetDirty();
				}
			}
		}

		public ContentSizeFitter.FitMode verticalFit
		{
			get
			{
				return this.m_VerticalFit;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<ContentSizeFitter.FitMode>(ref this.m_VerticalFit, value))
				{
					this.SetDirty();
				}
			}
		}

		private RectTransform rectTransform
		{
			get
			{
				if (this.m_Rect == null)
				{
					this.m_Rect = base.GetComponent<RectTransform>();
				}
				return this.m_Rect;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetDirty();
		}

		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			base.OnDisable();
		}

		protected override void OnRectTransformDimensionsChange()
		{
			this.SetDirty();
		}

		private void HandleSelfFittingAlongAxis(int axis)
		{
			ContentSizeFitter.FitMode fitMode = (axis != 0) ? this.verticalFit : this.horizontalFit;
			if (fitMode == ContentSizeFitter.FitMode.Unconstrained)
			{
				return;
			}
			this.m_Tracker.Add(this, this.rectTransform, (axis != 0) ? DrivenTransformProperties.SizeDeltaY : DrivenTransformProperties.SizeDeltaX);
			if (fitMode == ContentSizeFitter.FitMode.MinSize)
			{
				this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetMinSize(this.m_Rect, axis));
			}
			else
			{
				this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetPreferredSize(this.m_Rect, axis));
			}
		}

		public virtual void SetLayoutHorizontal()
		{
			this.m_Tracker.Clear();
			this.HandleSelfFittingAlongAxis(0);
		}

		public virtual void SetLayoutVertical()
		{
			this.HandleSelfFittingAlongAxis(1);
		}

		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
		}

		public enum FitMode
		{
			Unconstrained,
			MinSize,
			PreferredSize
		}
	}
}
