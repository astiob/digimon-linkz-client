using System;

namespace AdventureScene
{
	public sealed class ShowPopupCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#show_popup";

		private string titleKey = string.Empty;

		private string bodyKey = string.Empty;

		public override string GetCommandName()
		{
			return "#show_popup";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.titleKey = commandParams[1];
				this.bodyKey = commandParams[2];
				base.SetWaitScriptEngine(true);
				result = true;
			}
			catch
			{
				base.OnErrorGetParameter();
			}
			return result;
		}

		public override bool RunScriptCommand()
		{
			TutorialMaster.NaviMessage naviMessage = MasterDataMng.Instance().Tutorial.GetNaviMessage(this.titleKey);
			TutorialMaster.NaviMessage naviMessage2 = MasterDataMng.Instance().Tutorial.GetNaviMessage(this.bodyKey);
			bool result = true;
			AlertManager.ShowAlertDialog(delegate(int i)
			{
				base.ResumeScriptEngine();
			}, naviMessage.message, naviMessage2.message, AlertManager.ButtonActionType.Close, false);
			return result;
		}
	}
}
