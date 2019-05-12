using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class ShowBackgroundCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#show_background";

		private string switchKey = string.Empty;

		private string path = string.Empty;

		private int depth;

		private const string prefabPath = "UIBackground/Background6";

		public const string objectName = "ShowBackgroundCommandObj";

		public override string GetCommandName()
		{
			return "#show_background";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.switchKey = commandParams[1];
				this.path = commandParams[2];
				this.depth = commandParams[3].ToInt32();
				base.SetWaitScriptEngine(true);
				result = true;
			}
			catch
			{
				base.OnErrorGetParameter();
			}
			return result;
		}

		public override bool RunScriptCommand()
		{
			UnityEngine.Object original = Resources.Load("UIBackground/Background6");
			GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
			if (ClassSingleton<AdventureSceneData>.Instance.commonBackground == null)
			{
				UIRoot uiroot = UnityEngine.Object.FindObjectOfType<UIRoot>();
				TutorialUI componentInChildren = uiroot.GetComponentInChildren<TutorialUI>();
				componentInChildren.CommonBackground = gameObject;
				gameObject.transform.SetParent(componentInChildren.transform);
				gameObject.transform.localScale = Vector3.one;
			}
			ClassSingleton<AdventureSceneData>.Instance.commonBackground = gameObject;
			gameObject.name = "ShowBackgroundCommandObj";
			UITexture componentInChildren2 = gameObject.GetComponentInChildren<UITexture>();
			componentInChildren2.depth = this.depth;
			Texture2D mainTexture;
			if (this.switchKey == "res")
			{
				mainTexture = (Resources.Load(this.path) as Texture2D);
			}
			else
			{
				mainTexture = (MonsterIconCacheBuffer.Instance().LoadAndCacheObj(this.path, null) as Texture2D);
			}
			componentInChildren2.mainTexture = mainTexture;
			base.ResumeScriptEngine();
			return true;
		}
	}
}
