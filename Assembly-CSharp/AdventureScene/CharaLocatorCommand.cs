using System;

namespace AdventureScene
{
	public sealed class CharaLocatorCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_chara_loc";

		private int charaId;

		private string stageLocatorName;

		private bool isFollowingFlag;

		public CharaLocatorCommand()
		{
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_chara_loc";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.charaId = int.Parse(commandParams[1]);
				this.stageLocatorName = commandParams[2];
				if (3 < commandParams.Length)
				{
					this.isFollowingFlag = ("fix" == commandParams[3]);
				}
				else
				{
					this.isFollowingFlag = false;
				}
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
			bool flag = false;
			AdventureDigimonInfo digimonInfo = ClassSingleton<AdventureSceneData>.Instance.GetDigimonInfo(this.charaId);
			if (digimonInfo != null)
			{
				flag = AdventureObject.SetLocator(digimonInfo.model.transform, ClassSingleton<AdventureSceneData>.Instance.stage.transform, this.stageLocatorName, this.isFollowingFlag);
				if (flag)
				{
					base.ResumeScriptEngine();
				}
			}
			return flag;
		}
	}
}
