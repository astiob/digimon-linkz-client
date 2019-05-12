using System;

namespace AdventureScene.State
{
	public sealed class EndState : BaseState
	{
		private const int FADE_WAIT_FRAME_COUNT = 2;

		private EndState.Step step;

		private int fadeWaitFrameCount;

		public EndState()
		{
			this.step = EndState.Step.END_ACTION;
			this.fadeWaitFrameCount = 2;
		}

		public override bool Update()
		{
			bool result = true;
			switch (this.step)
			{
			case EndState.Step.END_ACTION:
				if (ClassSingleton<AdventureSceneData>.Instance.sceneEndAction != null)
				{
					ClassSingleton<AdventureSceneData>.Instance.sceneEndAction();
				}
				ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D.enabled = false;
				ClassSingleton<AdventureSceneData>.Instance.scriptObjectRoot.SetActive(false);
				this.step = EndState.Step.WAIT_FADE;
				break;
			case EndState.Step.WAIT_FADE:
				this.fadeWaitFrameCount--;
				if (0 >= this.fadeWaitFrameCount)
				{
					this.step = EndState.Step.END;
				}
				break;
			case EndState.Step.END:
				this.resultCode = ResultCode.SUCCESS;
				result = false;
				break;
			}
			return result;
		}

		private enum Step
		{
			END_ACTION,
			WAIT_FADE,
			END
		}
	}
}
