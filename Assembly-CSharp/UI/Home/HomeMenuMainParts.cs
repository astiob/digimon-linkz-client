using NGUI.Extensions;
using System;
using UnityEngine;

namespace UI.Home
{
	[RequireComponent(typeof(EfcCont))]
	public sealed class HomeMenuMainParts : MonoBehaviour
	{
		[SerializeField]
		private UIWidget frameWidget;

		[SerializeField]
		private float moveTime;

		private UISafeArea uiSafeArea;

		private EfcCont moveController;

		private bool isShow;

		private Vector2 workVector2;

		private float showPositionX;

		private float closePositionX;

		private void Awake()
		{
			this.uiSafeArea = base.GetComponentInParent<UISafeArea>();
			this.moveController = base.GetComponent<EfcCont>();
			float x = this.uiSafeArea.GetWidgetSize().x;
			this.showPositionX = this.uiSafeArea.transform.localPosition.x - x * 0.5f + (float)this.frameWidget.width;
			this.closePositionX = this.uiSafeArea.transform.localPosition.x + x * 0.5f;
		}

		private void Start()
		{
			Vector2 windowSize = GUIMain.GetUIPanel().GetWindowSize();
			UIWidget component = base.GetComponent<UIWidget>();
			component.width = Mathf.CeilToInt(windowSize.x + 0.5f);
		}

		public bool IsShow()
		{
			return this.isShow;
		}

		public void Open(Action<int> finishMove)
		{
			this.isShow = true;
			this.workVector2.x = this.showPositionX;
			this.workVector2.y = base.transform.localPosition.y;
			this.moveController.MoveTo(this.workVector2, this.moveTime, finishMove, iTween.EaseType.linear, 0f);
			SoundMng.Instance().TryPlaySE("SEInternal/Common/se_103", 0f, false, true, null, -1);
		}

		public void Close(bool playSound, Action<int> finishMove)
		{
			this.isShow = false;
			this.workVector2.x = this.closePositionX;
			this.workVector2.y = base.transform.localPosition.y;
			this.moveController.MoveTo(this.workVector2, this.moveTime, finishMove, iTween.EaseType.linear, 0f);
			if (playSound)
			{
				SoundMng.Instance().TryPlaySE("SEInternal/Common/se_104", 0f, false, true, null, -1);
			}
		}
	}
}
