using System;

namespace AdventureScene.State
{
	public sealed class BeginState : BaseState
	{
		public override bool Update()
		{
			if (ClassSingleton<AdventureSceneData>.Instance.sceneBeginAction != null)
			{
				ClassSingleton<AdventureSceneData>.Instance.sceneBeginAction();
			}
			ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D.enabled = true;
			this.resultCode = ResultCode.SUCCESS;
			return false;
		}
	}
}
