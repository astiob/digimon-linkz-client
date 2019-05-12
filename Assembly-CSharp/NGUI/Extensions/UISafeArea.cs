using System;
using UnityEngine;

namespace NGUI.Extensions
{
	[RequireComponent(typeof(UIWidget))]
	public sealed class UISafeArea : MonoBehaviour
	{
		private UICoverSafeArea coverSafeArea;

		private void Awake()
		{
			UIWidget component = base.GetComponent<UIWidget>();
			if (null != component)
			{
				this.coverSafeArea = UnityEngine.Object.FindObjectOfType<UICoverSafeArea>();
				if (null != this.coverSafeArea)
				{
					Transform transform = base.transform;
					Vector3 localScale = transform.localScale;
					transform.parent = this.coverSafeArea.transform;
					transform.localScale = localScale;
					UICoverSafeArea.AnchorPoint anchorPoint = this.coverSafeArea.GetAnchorPoint();
					component.SetAnchor(this.coverSafeArea.gameObject, Mathf.CeilToInt(anchorPoint.left), Mathf.CeilToInt(anchorPoint.bottom), Mathf.CeilToInt(anchorPoint.right), Mathf.CeilToInt(anchorPoint.top));
					component.updateAnchors = UIRect.AnchorUpdate.OnUpdate;
				}
			}
		}

		public void ResizeWidgetSize()
		{
			if (null != this.coverSafeArea)
			{
				UIWidget component = base.GetComponent<UIWidget>();
				if (null != component)
				{
					UICoverSafeArea.AnchorPoint anchorPoint = this.coverSafeArea.GetAnchorPoint();
					component.leftAnchor.Set(0f, anchorPoint.left);
					component.rightAnchor.Set(1f, anchorPoint.right);
					component.topAnchor.Set(1f, anchorPoint.top);
					component.bottomAnchor.Set(0f, anchorPoint.bottom);
				}
			}
		}

		public Vector2 GetWidgetSize()
		{
			UIWidget component = base.GetComponent<UIWidget>();
			return component.localSize;
		}
	}
}
