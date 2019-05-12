using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class LoadEffectCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_load_effect";

		private int effectId;

		private string effectFileName;

		public LoadEffectCommand()
		{
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_load_effect";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.effectId = int.Parse(commandParams[1]);
				this.effectFileName = commandParams[2];
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
			bool result = true;
			try
			{
				AdventureEffectInfo adventureEffectInfo = new AdventureEffectInfo
				{
					id = this.effectId
				};
				string path = "AdventureScene/Effect/" + this.effectFileName;
				GameObject original = AssetDataMng.Instance().LoadObject(path, null, true) as GameObject;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
				gameObject.name = "Effect_" + this.effectFileName;
				gameObject.transform.parent = ClassSingleton<AdventureSceneData>.Instance.scriptObjectRoot.transform;
				gameObject.SetActive(false);
				adventureEffectInfo.model = gameObject;
				adventureEffectInfo.particle = gameObject.GetComponent<ParticleSystem>();
				adventureEffectInfo.animator = gameObject.GetComponent<Animator>();
				ClassSingleton<AdventureSceneData>.Instance.effectInfoList.Add(adventureEffectInfo);
				base.ResumeScriptEngine();
			}
			catch
			{
				result = false;
			}
			return result;
		}
	}
}
