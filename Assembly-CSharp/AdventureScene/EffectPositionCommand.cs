using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class EffectPositionCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_effect_pos";

		private int effectId;

		private Vector3 stageLocalPosition;

		public EffectPositionCommand()
		{
			this.stageLocalPosition = Vector3.zero;
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_effect_pos";
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
				this.stageLocalPosition.Set(newX, newY, newZ);
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
				effectInfo.model.transform.localPosition = this.stageLocalPosition;
				result = true;
				base.ResumeScriptEngine();
			}
			return result;
		}
	}
}
