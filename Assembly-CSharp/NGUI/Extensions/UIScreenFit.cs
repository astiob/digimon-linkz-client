using System;
using UnityEngine;

namespace NGUI.Extensions
{
	[RequireComponent(typeof(UIWidget))]
	public sealed class UIScreenFit : MonoBehaviour
	{
		[SerializeField]
		private bool fitLeft;

		[SerializeField]
		private bool fitRight;

		[SerializeField]
		private bool fitTop;

		[SerializeField]
		private bool fitBottom;

		private void Awake()
		{
			UIWidget component = base.GetComponent<UIWidget>();
			if (null != component)
			{
				UICoverSafeArea uicoverSafeArea = UnityEngine.Object.FindObjectOfType<UICoverSafeArea>();
				if (null != uicoverSafeArea)
				{
					this.FitAnchorPoint(component, uicoverSafeArea.transform);
					component.updateAnchors = UIRect.AnchorUpdate.OnEnable;
				}
			}
		}

		private void FitAnchorPoint(UIWidget widget, Transform target)
		{
			bool flag = false;
			if (this.fitLeft)
			{
				widget.leftAnchor.target = target;
				widget.leftAnchor.Set(0f, 0f);
				flag = true;
			}
			if (this.fitRight)
			{
				widget.rightAnchor.target = target;
				widget.rightAnchor.Set(1f, 0f);
				flag = true;
			}
			if (this.fitTop)
			{
				widget.topAnchor.target = target;
				widget.topAnchor.Set(1f, 0f);
				flag = true;
			}
			if (this.fitBottom)
			{
				widget.bottomAnchor.target = target;
				widget.bottomAnchor.Set(0f, 0f);
				flag = true;
			}
			if (flag)
			{
				widget.ResetAnchors();
				widget.UpdateAnchors();
			}
		}
	}
}
