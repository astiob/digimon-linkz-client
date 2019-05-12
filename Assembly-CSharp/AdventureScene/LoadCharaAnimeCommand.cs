using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class LoadCharaAnimeCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_load_chara_anm";

		private int charaId;

		private string animatorFileName;

		public override string GetCommandName()
		{
			return "#adv_load_chara_anm";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.charaId = int.Parse(commandParams[1]);
				this.animatorFileName = commandParams[2];
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
			bool result = false;
			AdventureDigimonInfo digimonInfo = ClassSingleton<AdventureSceneData>.Instance.GetDigimonInfo(this.charaId);
			if (digimonInfo != null)
			{
				Transform child = digimonInfo.model.transform.GetChild(0);
				if (null != child)
				{
					digimonInfo.animator = child.GetComponent<Animator>();
					if (null == digimonInfo.animator)
					{
						digimonInfo.animator = child.gameObject.AddComponent<Animator>();
					}
					string path = "AdventureScene/Animation/Character/" + this.animatorFileName;
					digimonInfo.animator.runtimeAnimatorController = (AssetDataMng.Instance().LoadObject(path, null, true) as RuntimeAnimatorController);
					if (null != digimonInfo.animator.runtimeAnimatorController)
					{
						result = true;
						base.ResumeScriptEngine();
					}
				}
			}
			return result;
		}
	}
}
