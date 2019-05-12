using DeviceSafeArea;
using System;
using UnityEngine;

namespace NGUI.Extensions
{
	public sealed class UICoverSafeArea : MonoBehaviour
	{
		[SerializeField]
		private Vector2 baseScreenSize;

		[SerializeField]
		private GameObject uiSafeArea;

		private Vector2 deviceScreenSize;

		private Rect safeAreaSize;

		private UICoverSafeArea.SafeAreaMargin margin;

		private Vector2 gameScreenSize;

		private DeviceOrientation nowOrientation;

		private UICoverSafeArea.AnchorPoint anchorPoint;

		private void Awake()
		{
			this.margin = default(UICoverSafeArea.SafeAreaMargin);
			this.gameScreenSize = default(Vector2);
			this.anchorPoint = default(UICoverSafeArea.AnchorPoint);
		}

		private void Start()
		{
			this.LoadDeviceInfo();
			this.AjustScreenAndAnchor();
			this.nowOrientation = Input.deviceOrientation;
		}

		private void Update()
		{
			if (Input.deviceOrientation != this.nowOrientation)
			{
				this.nowOrientation = Input.deviceOrientation;
				this.LoadDeviceInfo();
				this.AjustScreenAndAnchor();
			}
		}

		private void LoadDeviceInfo()
		{
			this.deviceScreenSize = SafeArea.GetDeviceScreenSize();
			this.safeAreaSize = SafeArea.GetSafeArea();
			this.margin.left = Mathf.CeilToInt(this.safeAreaSize.x);
			this.margin.right = Mathf.CeilToInt(this.deviceScreenSize.x - (this.safeAreaSize.x + this.safeAreaSize.width));
			this.margin.bottom = Mathf.CeilToInt(this.safeAreaSize.y);
			this.margin.top = Mathf.CeilToInt(this.deviceScreenSize.y - (this.safeAreaSize.y + this.safeAreaSize.height));
		}

		private void AjustScreenAndAnchor()
		{
			float num = this.safeAreaSize.width / this.deviceScreenSize.x;
			float num2 = this.safeAreaSize.height / this.deviceScreenSize.y;
			this.gameScreenSize.x = this.baseScreenSize.x / num;
			this.gameScreenSize.y = this.baseScreenSize.y / num2;
			num = this.gameScreenSize.x / this.deviceScreenSize.x;
			num2 = this.gameScreenSize.y / this.deviceScreenSize.y;
			this.anchorPoint.left = Mathf.Ceil((float)this.margin.left * num);
			this.anchorPoint.right = Mathf.Ceil((float)this.margin.right * num) * -1f;
			this.anchorPoint.top = Mathf.Ceil((float)this.margin.top * num2) * -1f;
			this.anchorPoint.bottom = Mathf.Ceil((float)this.margin.bottom * num2);
			UIRoot component = base.GetComponent<UIRoot>();
			component.manualWidth = Mathf.CeilToInt(this.gameScreenSize.x);
			component.manualHeight = Mathf.CeilToInt(this.gameScreenSize.y);
			this.uiSafeArea.BroadcastMessage("ResizeWidgetSize", SendMessageOptions.DontRequireReceiver);
		}

		public UICoverSafeArea.AnchorPoint GetAnchorPoint()
		{
			return this.anchorPoint;
		}

		public void UpdateAnchorPoint()
		{
			this.LoadDeviceInfo();
			this.AjustScreenAndAnchor();
		}

		private struct SafeAreaMargin
		{
			public int left;

			public int right;

			public int top;

			public int bottom;
		}

		public struct AnchorPoint
		{
			public float left;

			public float right;

			public float top;

			public float bottom;
		}
	}
}
