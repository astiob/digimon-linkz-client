using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class CameraMoveCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_camera_mov";

		private string activeType;

		private float time;

		private float fov;

		private Vector3 stageLocalPosition;

		private Vector3 rotationEulerAngles;

		public CameraMoveCommand()
		{
			this.stageLocalPosition = Vector3.zero;
			this.rotationEulerAngles = Vector3.zero;
		}

		public override string GetCommandName()
		{
			return "#adv_camera_mov";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.activeType = commandParams[1];
				if ("fov" == this.activeType)
				{
					this.fov = float.Parse(commandParams[2]);
					this.time = float.Parse(commandParams[3]);
				}
				else if ("pos" == this.activeType)
				{
					float new_x = float.Parse(commandParams[2]);
					float new_y = float.Parse(commandParams[3]);
					float new_z = float.Parse(commandParams[4]);
					this.stageLocalPosition.Set(new_x, new_y, new_z);
					this.time = float.Parse(commandParams[5]);
				}
				else
				{
					float new_x2 = float.Parse(commandParams[2]);
					float new_y2 = float.Parse(commandParams[3]);
					float new_z2 = float.Parse(commandParams[4]);
					this.rotationEulerAngles.Set(new_x2, new_y2, new_z2);
					this.time = float.Parse(commandParams[5]);
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
			bool result = true;
			string text = this.activeType;
			switch (text)
			{
			case "fov":
				ClassSingleton<AdventureSceneData>.Instance.adventureCamera.SetFieldOfView(this.fov, this.time);
				break;
			case "pos":
				ClassSingleton<AdventureSceneData>.Instance.adventureCamera.SetMove(this.stageLocalPosition, this.time);
				break;
			case "rot":
				ClassSingleton<AdventureSceneData>.Instance.adventureCamera.SetRotation(this.rotationEulerAngles, false, this.time);
				break;
			case "round":
				ClassSingleton<AdventureSceneData>.Instance.adventureCamera.SetRotation(this.rotationEulerAngles, true, this.time);
				break;
			}
			return result;
		}

		public override bool UpdateCommand()
		{
			if (!ClassSingleton<AdventureSceneData>.Instance.adventureCamera.UpdateAnimation())
			{
				base.ResumeScriptEngine();
			}
			return true;
		}
	}
}
