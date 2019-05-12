using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class CameraTargetCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_camera_target";

		private string locatorOwnerFlag;

		private string locatorName;

		private Vector3 stageLocalPosition;

		private Vector3 lookAtPosition;

		private int charaId;

		private bool isFollowingFlag;

		public CameraTargetCommand()
		{
			this.stageLocalPosition = Vector3.zero;
			this.lookAtPosition = Vector3.zero;
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_camera_target";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = true;
			try
			{
				this.locatorOwnerFlag = commandParams[1];
				if ("stage" == this.locatorOwnerFlag)
				{
					float new_x = float.Parse(commandParams[2]);
					float new_y = float.Parse(commandParams[3]);
					float new_z = float.Parse(commandParams[4]);
					this.lookAtPosition.Set(new_x, new_y, new_z);
					new_x = float.Parse(commandParams[5]);
					new_y = float.Parse(commandParams[6]);
					new_z = float.Parse(commandParams[7]);
					this.stageLocalPosition.Set(new_x, new_y, new_z);
				}
				else if ("chara" == this.locatorOwnerFlag)
				{
					this.charaId = int.Parse(commandParams[2]);
					this.isFollowingFlag = ("fix" == commandParams[3]);
				}
				else if ("stageLoc" == this.locatorOwnerFlag)
				{
					this.locatorName = commandParams[2];
					this.isFollowingFlag = ("fix" == commandParams[3]);
				}
				else
				{
					if (!("charaLoc" == this.locatorOwnerFlag))
					{
						throw new Exception("引数が不定");
					}
					this.charaId = int.Parse(commandParams[2]);
					this.locatorName = commandParams[3];
					this.isFollowingFlag = ("fix" == commandParams[4]);
				}
				base.SetWaitScriptEngine(true);
			}
			catch
			{
				base.OnErrorGetParameter();
				result = false;
			}
			return result;
		}

		public override bool RunScriptCommand()
		{
			bool flag = false;
			AdventureCamera adventureCamera = ClassSingleton<AdventureSceneData>.Instance.adventureCamera;
			string text = this.locatorOwnerFlag;
			switch (text)
			{
			case "stage":
				adventureCamera.SetLookAt(this.lookAtPosition, this.stageLocalPosition);
				flag = true;
				break;
			case "chara":
			{
				AdventureDigimonInfo digimonInfo = ClassSingleton<AdventureSceneData>.Instance.GetDigimonInfo(this.charaId);
				if (digimonInfo != null)
				{
					adventureCamera.SetTargetChara(digimonInfo.model, this.isFollowingFlag);
					flag = true;
				}
				break;
			}
			case "stageLoc":
				flag = adventureCamera.SetTargetStageLocator(this.locatorName, this.isFollowingFlag);
				break;
			case "charaLoc":
			{
				AdventureDigimonInfo digimonInfo = ClassSingleton<AdventureSceneData>.Instance.GetDigimonInfo(this.charaId);
				if (digimonInfo != null)
				{
					flag = adventureCamera.SetTargetCharaLocator(digimonInfo.model, this.locatorName, this.isFollowingFlag);
				}
				break;
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
