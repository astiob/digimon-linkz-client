using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Facebook.Unity.Mobile.Android
{
	internal class AndroidFacebookGameObject : MobileFacebookGameObject
	{
		protected override void OnAwake()
		{
			AndroidJNIHelper.debug = Debug.isDebugBuild;
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
