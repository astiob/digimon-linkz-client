using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class CharaAnimeCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_chara_anm";

		private int charaId;

		private bool isWaitFlag;

		private string animatorStateName;

		private string finishAnimatorStateName;

		public override string GetCommandName()
		{
			return "#adv_chara_anm";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.isWaitFlag = ("wait" == commandParams[1]);
				this.charaId = int.Parse(commandParams[2]);
				this.animatorStateName = commandParams[3];
				if (4 < commandParams.Length)
				{
					this.finishAnimatorStateName = commandParams[4];
				}
				else
				{
					this.finishAnimatorStateName = this.animatorStateName;
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
			bool result = false;
			AdventureDigimonInfo digimonInfo = ClassSingleton<AdventureSceneData>.Instance.GetDigimonInfo(this.charaId);
			if (digimonInfo != null && null != digimonInfo.animator)
			{
				digimonInfo.animator.Play(this.animatorStateName, 0, 0f);
				base.SetContinueAnalyzeForAnimationWaitTime(this.isWaitFlag);
				result = true;
			}
			return result;
		}

		public override bool UpdateCommand()
		{
			if (this.isWaitFlag)
			{
				AdventureDigimonInfo digimonInfo = ClassSingleton<AdventureSceneData>.Instance.GetDigimonInfo(this.charaId);
				if (digimonInfo == null || !(null != digimonInfo.animator))
				{
					return false;
				}
				AnimatorStateInfo currentAnimatorStateInfo = digimonInfo.animator.GetCurrentAnimatorStateInfo(0);
				if (currentAnimatorStateInfo.IsName(this.finishAnimatorStateName) && 1f <= currentAnimatorStateInfo.normalizedTime)
				{
					base.ResumeScriptEngine();
				}
			}
			return true;
		}
	}
}
