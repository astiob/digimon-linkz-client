using System;
using UnityEngine;

namespace Scene
{
	public static class SceneController
	{
		public static void DeleteCurrentScene()
		{
			Application.LoadLevel("Empty");
		}

		public static void ChangeBattleResultScene(float fadeoutTime = 0.5f)
		{
			GUIMain.FadeBlackReqFromScene("UIResult", fadeoutTime, 0.5f);
		}
	}
}
