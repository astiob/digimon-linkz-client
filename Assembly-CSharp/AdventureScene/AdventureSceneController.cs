using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class AdventureSceneController : ClassSingleton<AdventureSceneController>
	{
		private AdventureSceneRoot adventureScene;

		private void Delete()
		{
			if (null != this.adventureScene)
			{
				UnityEngine.Object.Destroy(this.adventureScene.gameObject);
				this.adventureScene = null;
			}
			ClassSingleton<AdventureSceneData>.Instance.scriptFileName = string.Empty;
			ClassSingleton<AdventureSceneData>.Instance.sceneBeginAction = null;
			ClassSingleton<AdventureSceneData>.Instance.sceneEndAction = null;
			ClassSingleton<AdventureSceneData>.Instance.sceneDeleteAction = null;
		}

		public void Ready(string fileName, Action beginAction, Action endAction)
		{
			ClassSingleton<AdventureSceneData>.Instance.scriptFileName = fileName;
			ClassSingleton<AdventureSceneData>.Instance.sceneBeginAction = beginAction;
			ClassSingleton<AdventureSceneData>.Instance.sceneEndAction = endAction;
			ClassSingleton<AdventureSceneData>.Instance.sceneDeleteAction = new Action(this.Delete);
		}

		public void Start()
		{
			GameObject original = AssetDataMng.Instance().LoadObject("AdventureScene/AdventureSceneRoot", null, true) as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			gameObject.transform.parent = null;
			gameObject.transform.position = Vector3.zero;
			this.adventureScene = gameObject.GetComponent<AdventureSceneRoot>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
	}
}
