using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureScene
{
	public sealed class LoadCharaCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_load_chara";

		private int charaId;

		private string monsterGroupId;

		private string clearAnimeFlag;

		public LoadCharaCommand()
		{
			this.monsterGroupId = string.Empty;
			this.continueAnalyze = true;
		}

		private bool LoadUniqueSkill(string monsterGroupId, out List<GameWebAPI.RespDataMA_GetSkillM.SkillM> uniqueEffectList)
		{
			bool result = false;
			uniqueEffectList = new List<GameWebAPI.RespDataMA_GetSkillM.SkillM>();
			try
			{
				GameWebAPI.RespDataMA_GetMonsterMS respDataMA_MonsterMS = MasterDataMng.Instance().RespDataMA_MonsterMS;
				GameWebAPI.RespDataMA_GetSkillM respDataMA_SkillM = MasterDataMng.Instance().RespDataMA_SkillM;
				if (respDataMA_MonsterMS != null && respDataMA_SkillM != null)
				{
					string b = string.Empty;
					for (int i = 0; i < respDataMA_MonsterMS.monsterM.Length; i++)
					{
						if (respDataMA_MonsterMS.monsterM[i].monsterGroupId == monsterGroupId)
						{
							b = respDataMA_MonsterMS.monsterM[i].skillGroupId;
							break;
						}
					}
					uniqueEffectList = new List<GameWebAPI.RespDataMA_GetSkillM.SkillM>();
					for (int j = 0; j < respDataMA_SkillM.skillM.Length; j++)
					{
						if (respDataMA_SkillM.skillM[j].skillGroupId == b)
						{
							uniqueEffectList.Add(respDataMA_SkillM.skillM[j]);
							result = true;
						}
					}
					if (uniqueEffectList.Count == 2 && uniqueEffectList[0].attackEffect == uniqueEffectList[1].attackEffect)
					{
						uniqueEffectList.RemoveAt(1);
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		private bool LoadDigimon(string monsterGroupId, AdventureDigimonInfo digimonInfo)
		{
			bool result = true;
			try
			{
				string path = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId(monsterGroupId);
				GameObject original = AssetDataMng.Instance().LoadObject(path, null, true) as GameObject;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
				gameObject.name = "Digimon_" + monsterGroupId;
				gameObject.transform.parent = ClassSingleton<AdventureSceneData>.Instance.scriptObjectRoot.transform;
				gameObject.SetActive(false);
				digimonInfo.model = gameObject;
				List<GameWebAPI.RespDataMA_GetSkillM.SkillM> list;
				if (!this.LoadUniqueSkill(monsterGroupId, out list))
				{
					throw new Exception("Error : 固有スキルの読み込みに失敗");
				}
				for (int i = 0; i < list.Count; i++)
				{
					int num = list[i].skillGroupSubId.ToInt32();
					int num2 = num - 1;
					path = CommonResourcesDataMng.Instance().GetUniqueSkillPrefabPathByAttackEffectId(list[i].attackEffect);
					original = (AssetDataMng.Instance().LoadObject(path, null, true) as GameObject);
					gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
					gameObject.name = "SkillEffect_" + list[i].attackEffect;
					gameObject.transform.parent = ClassSingleton<AdventureSceneData>.Instance.scriptObjectRoot.transform;
					gameObject.SetActive(false);
					digimonInfo.skillEffectList[num2] = gameObject;
					InvocationEffectParams component = gameObject.GetComponent<InvocationEffectParams>();
					path = CommonResourcesDataMng.Instance().GetCameraMotionPrefabPathByCameraId(component.cameraMotionId);
					original = (AssetDataMng.Instance().LoadObject(path, null, true) as GameObject);
					gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
					gameObject.name = "CameraAnime_" + component.cameraMotionId;
					gameObject.transform.parent = ClassSingleton<AdventureSceneData>.Instance.scriptObjectRoot.transform;
					gameObject.SetActive(false);
					digimonInfo.skillCameraAnimation[num2] = gameObject;
					digimonInfo.skillEffectSeList[num2] = list[i].soundEffect;
				}
			}
			catch
			{
				result = false;
				digimonInfo.Delete();
			}
			return result;
		}

		private bool LoadDigimonNotCharaParams(string monsterGroupId, AdventureDigimonInfo digimonInfo)
		{
			bool result = true;
			try
			{
				string monsterCharaPathByMonsterGroupId = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId(this.monsterGroupId);
				GameObject original = AssetDataMng.Instance().LoadObject(monsterCharaPathByMonsterGroupId, null, true) as GameObject;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
				gameObject.name = "Digimon_" + this.monsterGroupId;
				gameObject.transform.parent = ClassSingleton<AdventureSceneData>.Instance.scriptObjectRoot.transform;
				gameObject.SetActive(false);
				digimonInfo.model = gameObject;
				CharacterParams component = gameObject.GetComponent<CharacterParams>();
				UnityEngine.Object.Destroy(component);
				CapsuleCollider component2 = gameObject.GetComponent<CapsuleCollider>();
				UnityEngine.Object.Destroy(component2);
				Transform transform = gameObject.transform;
				for (int i = 0; i < transform.childCount; i++)
				{
					Animation component3 = transform.GetChild(i).GetComponent<Animation>();
					UnityEngine.Object.Destroy(component3);
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public override string GetCommandName()
		{
			return "#adv_load_chara";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = true;
			try
			{
				this.charaId = int.Parse(commandParams[1]);
				this.monsterGroupId = commandParams[2];
				if (3 < commandParams.Length)
				{
					this.clearAnimeFlag = commandParams[3];
				}
				else
				{
					this.clearAnimeFlag = string.Empty;
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
			AdventureDigimonInfo adventureDigimonInfo = new AdventureDigimonInfo
			{
				id = this.charaId
			};
			bool flag;
			if ("clearAnime" == this.clearAnimeFlag)
			{
				flag = this.LoadDigimonNotCharaParams(this.monsterGroupId, adventureDigimonInfo);
				if (flag)
				{
					ClassSingleton<AdventureSceneData>.Instance.digimonInfoList.Add(adventureDigimonInfo);
					base.ResumeScriptEngine();
				}
			}
			else
			{
				flag = this.LoadDigimon(this.monsterGroupId, adventureDigimonInfo);
				if (flag)
				{
					ClassSingleton<AdventureSceneData>.Instance.digimonInfoList.Add(adventureDigimonInfo);
					base.ResumeScriptEngine();
				}
			}
			return flag;
		}
	}
}
