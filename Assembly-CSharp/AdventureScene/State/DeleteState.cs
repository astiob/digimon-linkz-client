using System;
using UnityEngine;

namespace AdventureScene.State
{
	public sealed class DeleteState : BaseState
	{
		public override bool Update()
		{
			this.resultCode = ResultCode.SUCCESS;
			for (int i = 0; i < ClassSingleton<AdventureSceneData>.Instance.effectInfoList.Count; i++)
			{
				ClassSingleton<AdventureSceneData>.Instance.effectInfoList[i].Delete();
			}
			ClassSingleton<AdventureSceneData>.Instance.effectInfoList.Clear();
			for (int j = 0; j < ClassSingleton<AdventureSceneData>.Instance.digimonInfoList.Count; j++)
			{
				ClassSingleton<AdventureSceneData>.Instance.digimonInfoList[j].Delete();
			}
			ClassSingleton<AdventureSceneData>.Instance.digimonInfoList.Clear();
			UnityEngine.Object.Destroy(ClassSingleton<AdventureSceneData>.Instance.stage);
			ClassSingleton<AdventureSceneData>.Instance.stage = null;
			UnityEngine.Object.Destroy(ClassSingleton<AdventureSceneData>.Instance.scriptObjectRoot);
			ClassSingleton<AdventureSceneData>.Instance.scriptObjectRoot = null;
			ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine.DeleteScript();
			ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine = null;
			ClassSingleton<AdventureSceneData>.Instance.adventureCamera.Reset();
			FollowTargetCamera followTargetCamera = ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D.GetComponent<FollowTargetCamera>();
			if (null != followTargetCamera)
			{
				followTargetCamera.RemoveCamera(ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D);
				UnityEngine.Object.Destroy(followTargetCamera);
			}
			else
			{
				followTargetCamera = UnityEngine.Object.FindObjectOfType<FollowTargetCamera>();
				if (null != followTargetCamera)
				{
					followTargetCamera.RemoveCamera(ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D);
				}
			}
			return false;
		}
	}
}
