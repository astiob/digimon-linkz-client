using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class CharaRotationCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_chara_rot";

		private int charaId;

		private Vector3 rotationEulerAngles;

		public CharaRotationCommand()
		{
			this.rotationEulerAngles = Vector3.zero;
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_chara_rot";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.charaId = int.Parse(commandParams[1]);
				float new_x = float.Parse(commandParams[2]);
				float new_y = float.Parse(commandParams[3]);
				float new_z = float.Parse(commandParams[4]);
				this.rotationEulerAngles.Set(new_x, new_y, new_z);
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
				digimonInfo.model.transform.localRotation = Quaternion.Euler(this.rotationEulerAngles);
				base.ResumeScriptEngine();
				result = true;
			}
			return result;
		}
	}
}
