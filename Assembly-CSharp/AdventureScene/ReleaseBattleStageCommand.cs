using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class ReleaseBattleStageCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_release_battle_stage";

		public ReleaseBattleStageCommand()
		{
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_release_battle_stage";
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
			BattleStateManager current = BattleStateManager.current;
			bool result;
			if (current != null)
			{
				result = true;
				BattleCameraObject component = ClassSingleton<AdventureSceneData>.Instance.stage.GetComponent<BattleCameraObject>();
				if (component != null)
				{
					UnityEngine.Object.Destroy(component);
				}
				ClassSingleton<AdventureSceneData>.Instance.stage = null;
				base.ResumeScriptEngine();
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
