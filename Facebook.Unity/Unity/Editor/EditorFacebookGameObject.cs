using System;
using UnityEngine.SceneManagement;

namespace Facebook.Unity.Editor
{
	internal class EditorFacebookGameObject : FacebookGameObject
	{
		protected override void OnAwake()
		{
			CodelessIAPAutoLog.addListenerToIAPButtons(this);
		}

		private void OnEnable()
		{
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			CodelessIAPAutoLog.addListenerToIAPButtons(this);
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
		}

		public void onPurchaseCompleteHandler(object data)
		{
			CodelessIAPAutoLog.handlePurchaseCompleted(data);
		}
	}
}
