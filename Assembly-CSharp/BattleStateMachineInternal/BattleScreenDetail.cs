using System;

namespace BattleStateMachineInternal
{
	public class BattleScreenDetail
	{
		private Action previousStateMethod;

		private Action nextStateMethod;

		private UIWidget[] previousStateDeactiveWidgets;

		private UIWidget[] nextStateActiveWidgets;

		public BattleScreenDetail(bool IsAlwaysScreen)
		{
			this.isAlwaysScreen = IsAlwaysScreen;
			this.previousStateMethod = null;
			this.nextStateMethod = null;
		}

		public BattleScreenDetail(Action PreviousStateMethod, Action NextStateMethod, bool IsAlwaysScreen)
		{
			this.isAlwaysScreen = IsAlwaysScreen;
			this.previousStateMethod = PreviousStateMethod;
			this.nextStateMethod = NextStateMethod;
			this.previousStateDeactiveWidgets = null;
			this.nextStateActiveWidgets = null;
		}

		public BattleScreenDetail(UIWidget PreviousStateDeactiveWidget, UIWidget NextStateActiveWidget, bool IsAlwaysScreen)
		{
			this.isAlwaysScreen = IsAlwaysScreen;
			this.previousStateMethod = new Action(this.DeactiveObjects);
			this.nextStateMethod = new Action(this.ActiveObjects);
			this.previousStateDeactiveWidgets = new UIWidget[]
			{
				PreviousStateDeactiveWidget
			};
			this.nextStateActiveWidgets = new UIWidget[]
			{
				NextStateActiveWidget
			};
		}

		public BattleScreenDetail(UIWidget[] PreviousStateDeactiveWidgets, UIWidget[] NextStateActiveWidgets, bool IsAlwaysScreen)
		{
			this.isAlwaysScreen = IsAlwaysScreen;
			this.previousStateMethod = new Action(this.DeactiveObjects);
			this.nextStateMethod = new Action(this.ActiveObjects);
			this.previousStateDeactiveWidgets = PreviousStateDeactiveWidgets;
			this.nextStateActiveWidgets = NextStateActiveWidgets;
		}

		public BattleScreenDetail(UIWidget StateActiveChangeWidget, bool IsAlwaysScreen)
		{
			this.isAlwaysScreen = IsAlwaysScreen;
			this.previousStateMethod = new Action(this.DeactiveObjects);
			this.nextStateMethod = new Action(this.ActiveObjects);
			this.previousStateDeactiveWidgets = new UIWidget[]
			{
				StateActiveChangeWidget
			};
			this.nextStateActiveWidgets = this.previousStateDeactiveWidgets;
		}

		public BattleScreenDetail(UIWidget[] StateActiveChangeWidgets, bool IsAlwaysScreen)
		{
			this.isAlwaysScreen = IsAlwaysScreen;
			this.previousStateMethod = new Action(this.DeactiveObjects);
			this.nextStateMethod = new Action(this.ActiveObjects);
			this.previousStateDeactiveWidgets = StateActiveChangeWidgets;
			this.nextStateActiveWidgets = this.previousStateDeactiveWidgets;
		}

		public bool isAlwaysScreen { get; private set; }

		public void PreviousState()
		{
			if (this.previousStateMethod != null)
			{
				this.previousStateMethod();
			}
		}

		public void NextState()
		{
			if (this.nextStateMethod != null)
			{
				this.nextStateMethod();
			}
		}

		private void DeactiveObjects()
		{
			BattleScreenDetail.DeactiveObjects(this.previousStateDeactiveWidgets);
		}

		private void ActiveObjects()
		{
			BattleScreenDetail.ActiveObjects(this.nextStateActiveWidgets);
		}

		public static void DeactiveObjects(params UIWidget[] previousStateDeactiveWidgets)
		{
			foreach (UIWidget uiwidget in previousStateDeactiveWidgets)
			{
				NGUITools.SetActiveSelf(uiwidget.gameObject, false);
			}
		}

		public static void ActiveObjects(params UIWidget[] nextStateActiveWidgets)
		{
			foreach (UIWidget uiwidget in nextStateActiveWidgets)
			{
				NGUITools.SetActiveSelf(uiwidget.gameObject, true);
			}
		}
	}
}
