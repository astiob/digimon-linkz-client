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
					float newX = float.Parse(commandParams[2]);
					float newY = float.Parse(commandParams[3]);
					float newZ = float.Parse(commandParams[4]);
					this.stageLocalPosition.Set(newX, newY, newZ);
					this.time = float.Parse(commandParams[5]);
				}
				else
				{
					float newX2 = float.Parse(commandParams[2]);
					float newY2 = float.Parse(commandParams[3]);
					float newZ2 = float.Parse(commandParams[4]);
					this.rotationEulerAngles.Set(newX2, newY2, newZ2);
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
			if (text != null)
			{
				if (!(text == "fov"))
				{
					if (!(text == "pos"))
					{
						if (!(text == "rot"))
						{
							if (text == "round")
							{
								ClassSingleton<AdventureSceneData>.Instance.adventureCamera.SetRotation(this.rotationEulerAngles, true, this.time);
							}
						}
						else
						{
							ClassSingleton<AdventureSceneData>.Instance.adventureCamera.SetRotation(this.rotationEulerAngles, false, this.time);
						}
					}
					else
					{
						ClassSingleton<AdventureSceneData>.Instance.adventureCamera.SetMove(this.stageLocalPosition, this.time);
					}
				}
				else
				{
					ClassSingleton<AdventureSceneData>.Instance.adventureCamera.SetFieldOfView(this.fov, this.time);
				}
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
