using System;
using TextureTimeScrollInternal;
using UnityEngine;

namespace AdventureScene.State
{
	public sealed class InitializeState : BaseState
	{
		private InitializeState.Step step;

		private Transform parentTransform;

		private bool isFinishInitialize;

		public InitializeState(Transform parent)
		{
			this.step = InitializeState.Step.INIT;
			this.parentTransform = parent;
			this.isFinishInitialize = false;
		}

		private void Initialize()
		{
			AdventureSceneData.DataType dataType = ClassSingleton<AdventureSceneData>.Instance.dataType;
			if (dataType != AdventureSceneData.DataType.USE_SCRIPT_FILE)
			{
				if (dataType != AdventureSceneData.DataType.NO_SCRIPT_FILE)
				{
				}
			}
			else
			{
				ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine = new AdventureScriptEngine();
				ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine.Initialize();
				ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine.LosdScriptEngine(ClassSingleton<AdventureSceneData>.Instance.scriptFileName, new Action(this.OnInitializeScriptEngine));
			}
		}

		private void OnInitializeScriptEngine()
		{
			ClassSingleton<AdventureSceneData>.Instance.scriptObjectRoot = new GameObject("ScriptObject");
			Transform transform = ClassSingleton<AdventureSceneData>.Instance.scriptObjectRoot.transform;
			transform.parent = this.parentTransform;
			transform.localScale = Vector3.one;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			TextureTimeScrollRealTime.TimeReset();
			FollowTargetCamera followTargetCamera = UnityEngine.Object.FindObjectOfType<FollowTargetCamera>();
			if (null == followTargetCamera)
			{
				Camera camera3D = ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D;
				camera3D.gameObject.AddComponent<FollowTargetCamera>();
			}
			else
			{
				followTargetCamera.AddCamera(ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D);
			}
			this.isFinishInitialize = true;
		}

		public override bool Update()
		{
			bool result = true;
			switch (this.step)
			{
			case InitializeState.Step.INIT:
				this.Initialize();
				this.step = InitializeState.Step.INIT_WAIT;
				break;
			case InitializeState.Step.INIT_WAIT:
				if (this.isFinishInitialize)
				{
					this.step = InitializeState.Step.RUN;
				}
				break;
			case InitializeState.Step.RUN:
				ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine.RunScript();
				if (ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine.GetState() != AdventureScriptEngine.ScriptState.INIT)
				{
					this.step = InitializeState.Step.END;
				}
				break;
			case InitializeState.Step.END:
				this.resultCode = ResultCode.SUCCESS;
				result = false;
				break;
			default:
				this.resultCode = ResultCode.ERROR;
				break;
			}
			return result;
		}

		private enum Step
		{
			INIT,
			INIT_WAIT,
			RUN,
			END
		}
	}
}
