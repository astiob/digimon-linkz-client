using System;

namespace AdventureScene
{
	public sealed class EffectShowCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_effect_show";

		private int effectId;

		private bool isShowFlag;

		public EffectShowCommand()
		{
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_effect_show";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.effectId = int.Parse(commandParams[1]);
				this.isShowFlag = ("on" == commandParams[2]);
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
			bool result = false;
			AdventureEffectInfo effectInfo = ClassSingleton<AdventureSceneData>.Instance.GetEffectInfo(this.effectId);
			if (effectInfo != null)
			{
				effectInfo.model.SetActive(this.isShowFlag);
				result = true;
				base.ResumeScriptEngine();
			}
			return result;
		}
	}
}
