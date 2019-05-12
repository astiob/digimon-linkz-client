using System;

namespace AdventureScene
{
	public sealed class HideBackgroundCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#hide_background";

		public override string GetCommandName()
		{
			return "#hide_background";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
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
			ClassSingleton<AdventureSceneData>.Instance.commonBackground.SetActive(false);
			bool result = true;
			base.ResumeScriptEngine();
			return result;
		}
	}
}
