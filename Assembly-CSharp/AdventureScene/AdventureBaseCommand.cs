using System;

namespace AdventureScene
{
	public abstract class AdventureBaseCommand : BaseCommand
	{
		public AdventureBaseCommand()
		{
			this.scriptEngine = ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine.GetScriptEngine();
		}

		protected void OnErrorGetParameter()
		{
			Debug.Log("引数が足りない : " + this.GetCommandName());
		}

		protected void SetContinueAnalyzeForAnimationWaitTime(bool waitFlag)
		{
			if (!waitFlag)
			{
				base.ResumeScriptEngine();
				this.continueAnalyze = true;
			}
			else
			{
				this.continueAnalyze = false;
			}
		}
	}
}
