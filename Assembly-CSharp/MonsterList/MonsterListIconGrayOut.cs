using System;

namespace MonsterList
{
	public abstract class MonsterListIconGrayOut
	{
		protected MonsterListIconGrayOut.PushAction normalStateAction;

		protected MonsterListIconGrayOut.PushAction selectedStateAction;

		protected MonsterListIconGrayOut.PushAction blockStateAction;

		public void SetNormalAction(Action<MonsterData> onTouchAction, Action<MonsterData> onPressAction)
		{
			this.normalStateAction.onTouch = onTouchAction;
			this.normalStateAction.onPress = onPressAction;
		}

		public void SetSelectedAction(Action<MonsterData> onTouchAction, Action<MonsterData> onPressAction)
		{
			this.selectedStateAction.onTouch = onTouchAction;
			this.selectedStateAction.onPress = onPressAction;
		}

		public void SetBlockAction(Action<MonsterData> onTouchAction, Action<MonsterData> onPressAction)
		{
			this.blockStateAction.onTouch = onTouchAction;
			this.blockStateAction.onPress = onPressAction;
		}

		protected struct PushAction
		{
			public Action<MonsterData> onTouch;

			public Action<MonsterData> onPress;
		}
	}
}
