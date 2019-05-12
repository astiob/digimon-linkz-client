using System;
using System.Collections;
using UnityEngine;

namespace AdventureScene
{
	public sealed class CharaAnimeSkillCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_chara_anm_skill";

		private int charaId;

		private bool isWaitFlag;

		private int uniqueSkillIndex;

		public override string GetCommandName()
		{
			return "#adv_chara_anm_skill";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.isWaitFlag = ("wait" == commandParams[1]);
				this.charaId = int.Parse(commandParams[2]);
				this.uniqueSkillIndex = Mathf.Clamp(int.Parse(commandParams[3]) - 1, 0, 1);
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
				CharacterParams component = digimonInfo.model.GetComponent<CharacterParams>();
				InvocationEffectParams component2 = digimonInfo.skillEffectList[this.uniqueSkillIndex].GetComponent<InvocationEffectParams>();
				LightColorChanger component3 = ClassSingleton<AdventureSceneData>.Instance.adventureLight.GetComponent<LightColorChanger>();
				IEnumerator enumerator = component2.SkillInitialize(ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D, ClassSingleton<AdventureSceneData>.Instance.stage, component3);
				while (enumerator.MoveNext())
				{
				}
				CameraParams component4 = digimonInfo.skillCameraAnimation[this.uniqueSkillIndex].GetComponent<CameraParams>();
				string text = digimonInfo.skillEffectSeList[this.uniqueSkillIndex];
				if (null != component && null != component2 && null != component4 && !string.IsNullOrEmpty(text))
				{
					component.PlayAttackAnimation(SkillType.Deathblow, this.uniqueSkillIndex);
					component2.transform.position = component.transform.position;
					component4.transform.position = component.transform.position;
					AppCoroutine.Start(component2.PlaySkillAnimation(component), new Action(this.OnFinishCommand), false);
					component4.currentTargetCamera = ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D;
					component4.PlayCameraAnimation(component, false, false);
					SoundMng.Instance().TryPlaySE("SE/" + text + "/sound", 0f, false, true, null, -1);
					base.SetContinueAnalyzeForAnimationWaitTime(this.isWaitFlag);
					result = true;
				}
			}
			return result;
		}

		public void OnFinishCommand()
		{
			if (this.isWaitFlag)
			{
				base.ResumeScriptEngine();
				AdventureDigimonInfo digimonInfo = ClassSingleton<AdventureSceneData>.Instance.GetDigimonInfo(this.charaId);
				if (digimonInfo != null)
				{
					for (int i = 0; i < digimonInfo.skillCameraAnimation.Length; i++)
					{
						if (null != digimonInfo.skillCameraAnimation[i] && digimonInfo.skillCameraAnimation[i].activeSelf)
						{
							digimonInfo.skillCameraAnimation[i].SetActive(false);
						}
					}
				}
			}
		}
	}
}
