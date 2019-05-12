using System;
using UnityEngine;

namespace UI.MonsterInfoParts
{
	public abstract class MonsterIconTouchEventUD : GUIColliderAddLongTouchEventUD
	{
		protected Action actionTouch;

		private void OnTouchEnd(Touch noop1, Vector2 noop2, bool isDrag)
		{
			if (!isDrag && this.actionTouch != null)
			{
				this.actionTouch();
			}
		}

		public void InitializeInputEvent()
		{
			base.onTouchEnded += this.OnTouchEnd;
		}

		public void SetTouchAction(Action action)
		{
			this.actionTouch = action;
		}

		public void SetPressAction(Action action)
		{
			this.actionLongPress = action;
		}
	}
}
