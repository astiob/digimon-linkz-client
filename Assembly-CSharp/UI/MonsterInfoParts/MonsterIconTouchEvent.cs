using System;
using UnityEngine;

namespace UI.MonsterInfoParts
{
	public abstract class MonsterIconTouchEvent : GUIColliderAddLongTouchEvent
	{
		protected Action actionTouch;

		private void OnTouchEnd(Touch noop1, Vector2 noop2, bool noop3)
		{
			if (this.actionTouch != null)
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
