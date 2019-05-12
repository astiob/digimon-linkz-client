using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class EffectLocatorCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_effect_loc";

		private int effectId;

		private string locatorType;

		private int charaId;

		private string locatorName;

		private bool isFollowingFlag;

		public EffectLocatorCommand()
		{
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_effect_loc";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.effectId = int.Parse(commandParams[1]);
				this.locatorType = commandParams[2];
				if ("stage" == this.locatorType)
				{
					this.locatorName = commandParams[3];
					if (4 < commandParams.Length)
					{
						this.isFollowingFlag = ("fix" == commandParams[4]);
					}
					else
					{
						this.isFollowingFlag = false;
					}
				}
				else if ("chara" == this.locatorType)
				{
					this.charaId = int.Parse(commandParams[3]);
					this.locatorName = commandParams[4];
					if (5 < commandParams.Length)
					{
						this.isFollowingFlag = ("fix" == commandParams[5]);
					}
					else
					{
						this.isFollowingFlag = false;
					}
				}
				else if (3 < commandParams.Length)
				{
					this.isFollowingFlag = ("fix" == commandParams[3]);
				}
				else
				{
					this.isFollowingFlag = false;
				}
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
			bool flag = true;
			AdventureEffectInfo effectInfo = ClassSingleton<AdventureSceneData>.Instance.GetEffectInfo(this.effectId);
			if (effectInfo != null)
			{
				if ("stage" == this.locatorType)
				{
					flag = AdventureObject.SetLocator(effectInfo.model.transform, ClassSingleton<AdventureSceneData>.Instance.stage.transform, this.locatorName, this.isFollowingFlag);
				}
				else if ("chara" == this.locatorType)
				{
					AdventureDigimonInfo digimonInfo = ClassSingleton<AdventureSceneData>.Instance.GetDigimonInfo(this.charaId);
					if (digimonInfo != null)
					{
						flag = AdventureObject.SetLocator(effectInfo.model.transform, digimonInfo.model.transform, this.locatorName, this.isFollowingFlag);
					}
				}
				else
				{
					Quaternion localRotation = effectInfo.model.transform.localRotation;
					Vector3 localPosition = effectInfo.model.transform.localPosition;
					Vector3 localScale = effectInfo.model.transform.localScale;
					effectInfo.model.transform.parent = ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D.transform;
					effectInfo.model.transform.localScale = localScale;
					effectInfo.model.transform.localPosition = localPosition;
					effectInfo.model.transform.localRotation = localRotation;
				}
			}
			if (flag)
			{
				base.ResumeScriptEngine();
			}
			return flag;
		}
	}
}
