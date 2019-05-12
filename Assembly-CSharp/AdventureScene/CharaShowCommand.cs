using System;

namespace AdventureScene
{
	public sealed class CharaShowCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_chara_show";

		private int charaId;

		private string showFlag;

		public CharaShowCommand()
		{
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_chara_show";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.charaId = int.Parse(commandParams[1]);
				this.showFlag = commandParams[2];
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
			AdventureDigimonInfo digimonInfo = ClassSingleton<AdventureSceneData>.Instance.GetDigimonInfo(this.charaId);
			if (digimonInfo != null)
			{
				bool active = "on" == this.showFlag;
				digimonInfo.model.SetActive(active);
				result = true;
			}
			base.ResumeScriptEngine();
			return result;
		}
	}
}
