using System;

namespace AdventureScene.State
{
	public sealed class AdventureState : BaseState
	{
		private AdventureState.Step step;

		private bool isRunningScript;

		public AdventureState()
		{
			this.step = AdventureState.Step.INIT;
		}

		private void Initialize()
		{
		}

		public override bool Update()
		{
			bool result = true;
			switch (this.step)
			{
			case AdventureState.Step.INIT:
				this.Initialize();
				this.step = AdventureState.Step.RUN;
				break;
			case AdventureState.Step.RUN:
				ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine.RunScript();
				if (ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine.GetState() != AdventureScriptEngine.ScriptState.RUN)
				{
					this.step = AdventureState.Step.END;
				}
				break;
			case AdventureState.Step.END:
				this.resultCode = ResultCode.SUCCESS;
				result = false;
				break;
			default:
				this.resultCode = ResultCode.ERROR;
				result = false;
				break;
			}
			return result;
		}

		private enum Step
		{
			INIT,
			RUN,
			END
		}
	}
}
