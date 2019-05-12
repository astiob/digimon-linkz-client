using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class EffectRotationCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_effect_rot";

		private int effectId;

		private Vector3 localEulerAngles;

		public EffectRotationCommand()
		{
			this.localEulerAngles = Vector3.zero;
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_effect_rot";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.effectId = int.Parse(commandParams[1]);
				float newX = float.Parse(commandParams[2]);
				float newY = float.Parse(commandParams[3]);
				float newZ = float.Parse(commandParams[4]);
				this.localEulerAngles.Set(newX, newY, newZ);
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
			AdventureEffectInfo effectInfo = ClassSingleton<AdventureSceneData>.Instance.GetEffectInfo(this.effectId);
			if (effectInfo != null)
			{
				effectInfo.model.transform.localRotation = Quaternion.Euler(this.localEulerAngles);
				result = true;
				base.ResumeScriptEngine();
			}
			return result;
		}
	}
}
