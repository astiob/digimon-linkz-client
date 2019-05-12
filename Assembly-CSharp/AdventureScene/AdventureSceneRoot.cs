using AdventureScene.State;
using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class AdventureSceneRoot : MonoBehaviour
	{
		[SerializeField]
		private Light light3D;

		[SerializeField]
		private Camera camera3D;

		[SerializeField]
		private Animator cameraAnimator;

		private AdventureSceneRoot.StateType stateType;

		private BaseState state;

		private void Awake()
		{
			ClassSingleton<AdventureSceneData>.Instance.adventureCamera.SetComponent(this.camera3D, this.cameraAnimator, base.gameObject.transform);
			ClassSingleton<AdventureSceneData>.Instance.adventureLight = this.light3D;
			this.Initialize();
		}

		private void Initialize()
		{
			this.stateType = AdventureSceneRoot.StateType.INIT;
			this.camera3D.enabled = false;
			this.state = new InitializeState(base.transform);
		}

		private void Update()
		{
			if (!this.state.Update())
			{
				if (this.state.GetResultCode() == ResultCode.SUCCESS)
				{
					if (!this.NextState(ref this.state, ref this.stateType))
					{
						global::Debug.LogError("Error : " + this.state.GetType());
						base.enabled = false;
					}
					if (!base.enabled && ClassSingleton<AdventureSceneData>.Instance.sceneDeleteAction != null)
					{
						ClassSingleton<AdventureSceneData>.Instance.sceneDeleteAction();
					}
				}
				else
				{
					global::Debug.LogError("Error : " + this.state.GetType());
					base.enabled = false;
				}
			}
		}

		private void OnApplicationPause(bool isPause)
		{
			if (ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine != null)
			{
				ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine.OnApplicationPauseAction(isPause);
			}
		}

		private bool NextState(ref BaseState state, ref AdventureSceneRoot.StateType type)
		{
			global::Debug.Log(type);
			switch (type)
			{
			case AdventureSceneRoot.StateType.INIT:
				state = new BeginState();
				type = AdventureSceneRoot.StateType.BEGIN;
				break;
			case AdventureSceneRoot.StateType.BEGIN:
				state = new AdventureState();
				type = AdventureSceneRoot.StateType.MOVIE;
				break;
			case AdventureSceneRoot.StateType.MOVIE:
				state = new EndState();
				type = AdventureSceneRoot.StateType.END;
				break;
			case AdventureSceneRoot.StateType.END:
				state = new DeleteState();
				type = AdventureSceneRoot.StateType.DELETE;
				break;
			case AdventureSceneRoot.StateType.DELETE:
				base.enabled = false;
				break;
			default:
				return false;
			}
			return true;
		}

		private enum StateType
		{
			INIT,
			BEGIN,
			MOVIE,
			END,
			DELETE
		}
	}
}
