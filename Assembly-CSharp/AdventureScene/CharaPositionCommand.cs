using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class CharaPositionCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_chara_pos";

		private int charaId;

		private Vector3 stageLocalPosition;

		public CharaPositionCommand()
		{
			this.stageLocalPosition = Vector3.zero;
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_chara_pos";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.charaId = int.Parse(commandParams[1]);
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
			AdventureDigimonInfo digimonInfo = ClassSingleton<AdventureSceneData>.Instance.GetDigimonInfo(this.charaId);
			if (digimonInfo != null)
			{
				digimonInfo.model.transform.localPosition = this.stageLocalPosition;
				result = true;
				base.ResumeScriptEngine();
			}
			return result;
		}
	}
}
