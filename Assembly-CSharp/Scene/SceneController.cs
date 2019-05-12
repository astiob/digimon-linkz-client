using System;
using UnityEngine.SceneManagement;

namespace Scene
{
	public static class SceneController
	{
		public static void DeleteCurrentScene()
		{
			SceneManager.LoadScene("Empty");
		}

		public static void ChangeBattleResultScene(float fadeoutTime = 0.5f)
		{
			GUIMain.FadeBlackReqFromScene("UIResult", fadeoutTime, 0.5f);
		}
	}
}
