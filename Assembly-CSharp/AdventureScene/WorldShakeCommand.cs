using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class WorldShakeCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_world_shake";

		private float intensity;

		private float decay;

		private bool isWaitFlag;

		public override string GetCommandName()
		{
			return "#adv_world_shake";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.isWaitFlag = ("wait" == commandParams[1]);
				this.intensity = float.Parse(commandParams[2]);
				this.decay = float.Parse(commandParams[3]);
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
			ObjectShake objectShake = ClassSingleton<AdventureSceneData>.Instance.scriptObjectRoot.AddComponent<ObjectShake>();
			if (null != objectShake)
			{
				objectShake.StartShake(this.intensity, this.decay, new Action(this.OnFinishedShake));
				if (!this.isWaitFlag)
				{
					base.ResumeScriptEngine();
				}
				result = true;
			}
			return result;
		}

		private void OnFinishedShake()
		{
			ObjectShake component = ClassSingleton<AdventureSceneData>.Instance.scriptObjectRoot.GetComponent<ObjectShake>();
			if (null != component)
			{
				component.ResetPosition();
				if (this.isWaitFlag)
				{
					base.ResumeScriptEngine();
				}
				UnityEngine.Object.Destroy(component);
			}
		}
	}
}
