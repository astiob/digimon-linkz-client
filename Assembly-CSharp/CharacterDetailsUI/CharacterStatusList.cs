using Master;
using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterDetailsUI
{
	public sealed class CharacterStatusList : MonoBehaviour
	{
		[SerializeField]
		private UISprite windowBackground;

		[SerializeField]
		private GameObject statusPage;

		[SerializeField]
		private GameObject skillPage;

		[SerializeField]
		private GameObject extraSkillPage;

		[SerializeField]
		private GameObject chipPage;

		[SerializeField]
		private CharacterStatusList.PageType[] pageContent;

		[SerializeField]
		private MonsterBasicInfoExpGauge basicInfo;

		[SerializeField]
		private MonsterResistanceList resistanceList;

		[SerializeField]
		private MonsterStatusList statusList;

		[SerializeField]
		private MonsterMedalList medalList;

		[SerializeField]
		private MonsterStatusChangeValueList changeValueList;

		[SerializeField]
		private CharacterSkillInfo skillInfo;

		[SerializeField]
		private CharacterSkillInfo extraSkillInfo;

		[SerializeField]
		private GameObject resistanceNone;

		[SerializeField]
		private GameObject resistanceFire;

		[SerializeField]
		private GameObject resistanceWater;

		[SerializeField]
		private GameObject resistanceThunder;

		[SerializeField]
		private GameObject resistanceNature;

		[SerializeField]
		private GameObject resistanceLight;

		[SerializeField]
		private GameObject resistanceDark;

		[SerializeField]
		private GameObject resistanceStun;

		[SerializeField]
		private GameObject resistanceSkillLock;

		[SerializeField]
		private GameObject resistanceSleep;

		[SerializeField]
		private GameObject resistanceParalysis;

		[SerializeField]
		private GameObject resistanceConfusion;

		[SerializeField]
		private GameObject resistancePoison;

		[SerializeField]
		private GameObject resistanceDeath;

		[SerializeField]
		private Transform levelUpParent;

		[SerializeField]
		private Transform tranceParent;

		[SerializeField]
		private ChipBaseSelect chipBaseSelect;

		[SerializeField]
		private UILabel nextMonsterResistanceAlert;

		private GameObject particleGameObject;

		private int pageNo;

		private bool viewExtraSkillPage;

		private Animation ShowLevelUpAnimation()
		{
			string path = "UICommon/Parts/LevelUp";
			GameObject original = Resources.Load(path, typeof(GameObject)) as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			DepthController depthController = gameObject.GetComponent<DepthController>();
			if (null == depthController)
			{
				depthController = gameObject.AddComponent<DepthController>();
			}
			Transform transform = gameObject.transform;
			int depth = this.windowBackground.depth;
			depthController.AddWidgetDepth(transform, depth + 10);
			transform.SetParent(this.levelUpParent);
			transform.localPosition = Vector3.zero;
			transform.localScale = Vector3.one;
			Animation component = gameObject.GetComponent<Animation>();
			component.Play("LevelUp");
			return component;
		}

		private Animation ShowTranceAnimation()
		{
			string path = "UICommon/Parts/AwakeningParts";
			GameObject original = Resources.Load(path, typeof(GameObject)) as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			DepthController depthController = gameObject.GetComponent<DepthController>();
			if (null == depthController)
			{
				depthController = gameObject.AddComponent<DepthController>();
			}
			Transform transform = gameObject.transform;
			int depth = this.windowBackground.depth;
			depthController.AddWidgetDepth(transform, depth + 10);
			transform.SetParent(this.tranceParent);
			transform.localPosition = Vector3.zero;
			transform.localScale = Vector3.one;
			Animation component = gameObject.GetComponent<Animation>();
			component.Play("Awakening");
			return component;
		}

		private bool AnyMatchStrongResistance(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceMaster, string tranceResistance, string tranceStatusAilment)
		{
			bool result = false;
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(tranceResistance))
			{
				list.AddRange(tranceResistance.Split(new char[]
				{
					','
				}));
			}
			if (!string.IsNullOrEmpty(tranceStatusAilment))
			{
				list.AddRange(tranceStatusAilment.Split(new char[]
				{
					','
				}));
			}
			for (int i = 0; i < list.Count; i++)
			{
				GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceMaster2 = this.GetResistanceMaster(list[i]);
				if ("1" == resistanceMaster2.fire && "1" == resistanceMaster.fire)
				{
					result = true;
					break;
				}
				if ("1" == resistanceMaster2.water && "1" == resistanceMaster.water)
				{
					result = true;
					break;
				}
				if ("1" == resistanceMaster2.thunder && "1" == resistanceMaster.thunder)
				{
					result = true;
					break;
				}
				if ("1" == resistanceMaster2.nature && "1" == resistanceMaster.nature)
				{
					result = true;
					break;
				}
				if ("1" == resistanceMaster2.none && "1" == resistanceMaster.none)
				{
					result = true;
					break;
				}
				if ("1" == resistanceMaster2.light && "1" == resistanceMaster.light)
				{
					result = true;
					break;
				}
				if ("1" == resistanceMaster2.dark && "1" == resistanceMaster.dark)
				{
					result = true;
					break;
				}
				if ("1" == resistanceMaster2.poison && "1" == resistanceMaster.poison)
				{
					result = true;
					break;
				}
				if ("1" == resistanceMaster2.confusion && "1" == resistanceMaster.confusion)
				{
					result = true;
					break;
				}
				if ("1" == resistanceMaster2.paralysis && "1" == resistanceMaster.paralysis)
				{
					result = true;
					break;
				}
				if ("1" == resistanceMaster2.sleep && "1" == resistanceMaster.sleep)
				{
					result = true;
					break;
				}
				if ("1" == resistanceMaster2.stun && "1" == resistanceMaster.stun)
				{
					result = true;
					break;
				}
				if ("1" == resistanceMaster2.skillLock && "1" == resistanceMaster.skillLock)
				{
					result = true;
					break;
				}
				if ("1" == resistanceMaster2.death && "1" == resistanceMaster.death)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		private int SetPageContent(int pageNo)
		{
			if (pageNo < 0 || this.pageContent.Length <= pageNo)
			{
				pageNo = 0;
			}
			switch (this.pageContent[pageNo])
			{
			case CharacterStatusList.PageType.STATUS:
				if (!this.statusPage.activeSelf)
				{
					this.statusPage.SetActive(true);
				}
				if (this.skillPage.activeSelf)
				{
					this.skillPage.SetActive(false);
				}
				if (this.extraSkillPage.activeSelf)
				{
					this.extraSkillPage.SetActive(false);
				}
				if (this.chipPage.activeSelf)
				{
					this.chipPage.SetActive(false);
				}
				break;
			case CharacterStatusList.PageType.SKILL:
				if (!this.viewExtraSkillPage)
				{
					if (this.statusPage.activeSelf)
					{
						this.statusPage.SetActive(false);
					}
					if (!this.skillPage.activeSelf)
					{
						this.skillPage.SetActive(true);
					}
					if (this.extraSkillPage.activeSelf)
					{
						this.extraSkillPage.SetActive(false);
					}
					if (this.chipPage.activeSelf)
					{
						this.chipPage.SetActive(false);
					}
				}
				else
				{
					if (this.statusPage.activeSelf)
					{
						this.statusPage.SetActive(false);
					}
					if (this.skillPage.activeSelf)
					{
						this.skillPage.SetActive(false);
					}
					if (!this.extraSkillPage.activeSelf)
					{
						this.extraSkillPage.SetActive(true);
					}
					if (this.chipPage.activeSelf)
					{
						this.chipPage.SetActive(false);
					}
				}
				break;
			case CharacterStatusList.PageType.CHIP:
				if (this.statusPage.activeSelf)
				{
					this.statusPage.SetActive(false);
				}
				if (this.skillPage.activeSelf)
				{
					this.skillPage.SetActive(false);
				}
				if (this.extraSkillPage.activeSelf)
				{
					this.extraSkillPage.SetActive(false);
				}
				if (!this.chipPage.activeSelf)
				{
					this.chipPage.SetActive(true);
				}
				break;
			}
			return pageNo;
		}

		public void SetMonsterStatus(MonsterData monsterData)
		{
			this.chipBaseSelect.SetSelectedCharChg(monsterData);
			if (!MonsterStatusData.IsVersionUp(monsterData.GetMonsterMaster().Simple.rare))
			{
				this.skillInfo.SetSkill(monsterData);
			}
			else
			{
				this.extraSkillInfo.SetSkill(monsterData);
			}
			int exp = int.Parse(monsterData.userMonster.ex);
			DataMng.ExperienceInfo experienceInfo = DataMng.Instance().GetExperienceInfo(exp);
			this.basicInfo.SetMonsterData(monsterData, experienceInfo);
			this.statusList.SetValues(monsterData, true);
			this.changeValueList.SetValues(monsterData);
			this.resistanceList.SetValues(monsterData);
			this.medalList.SetValues(monsterData.userMonster);
			this.viewExtraSkillPage = MonsterStatusData.IsVersionUp(monsterData.GetMonsterMaster().Simple.rare);
		}

		public void SetEggStatus(MonsterData monsterData)
		{
			this.chipBaseSelect.SetSelectedCharChg(monsterData);
			string eggName = StringMaster.GetString("CharaStatus-06");
			int num = MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM.Length;
			for (int i = 0; i < num; i++)
			{
				GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM monsterEvolutionRouteM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM[i];
				if (monsterEvolutionRouteM.monsterEvolutionRouteId == monsterData.userMonster.monsterEvolutionRouteId)
				{
					GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = MonsterMaster.GetMonsterMasterByMonsterGroupId(monsterEvolutionRouteM.eggMonsterId).Group;
					if (group != null)
					{
						eggName = group.monsterName;
					}
				}
			}
			if (!MonsterStatusData.IsVersionUp(monsterData.GetMonsterMaster().Simple.rare))
			{
				this.skillInfo.ClearSkill();
			}
			else
			{
				this.extraSkillInfo.ClearSkill();
			}
			this.basicInfo.SetEggData(eggName, monsterData.monsterM.rare);
			this.statusList.ClearEggCandidateMedalValues();
			this.changeValueList.SetEggStatusValues();
			this.resistanceList.ClearValues();
			this.medalList.SetValues(monsterData.userMonster);
		}

		public void SetPage(int page)
		{
			this.pageNo = this.SetPageContent(page);
		}

		public void NextPage()
		{
			this.pageNo++;
			this.pageNo = this.SetPageContent(this.pageNo);
		}

		public Animation ShowLevelUpParticle(Transform windowRootTransform)
		{
			string path = "Cutscenes/NewFX6";
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(path, typeof(GameObject)));
			gameObject.name = "LevelUpParticle";
			Transform transform = gameObject.transform;
			transform.SetParent(this.levelUpParent);
			transform.localPosition = Vector3.zero;
			Animation result = this.ShowLevelUpAnimation();
			SoundMng.Instance().TryPlaySE("SEInternal/Common/se_101", 0f, false, true, null, -1);
			return result;
		}

		public Animation ShowTranceParticle(Transform windowRootTransform)
		{
			string path = "Cutscenes/NewFX10";
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(path, typeof(GameObject)));
			gameObject.name = "TranceParticle";
			Transform transform = gameObject.transform;
			transform.SetParent(this.tranceParent);
			transform.localPosition = Vector3.zero;
			Animation result = this.ShowTranceAnimation();
			SoundMng.Instance().TryPlaySE("SEInternal/Common/se_101", 0f, false, true, null, -1);
			return result;
		}

		public GameObject GetTranceEffectObject(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM oldResistance, string newResistanceIds)
		{
			List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> uniqueResistanceListByJson = MonsterResistanceData.GetUniqueResistanceListByJson(newResistanceIds);
			this.particleGameObject = null;
			for (int i = 0; i < uniqueResistanceListByJson.Count; i++)
			{
				if ("1" == uniqueResistanceListByJson[i].none && "1" != oldResistance.none)
				{
					this.particleGameObject = this.resistanceNone;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].fire && "1" != oldResistance.fire)
				{
					this.particleGameObject = this.resistanceFire;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].water && "1" != oldResistance.water)
				{
					this.particleGameObject = this.resistanceWater;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].thunder && "1" != oldResistance.thunder)
				{
					this.particleGameObject = this.resistanceThunder;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].nature && "1" != oldResistance.nature)
				{
					this.particleGameObject = this.resistanceNature;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].dark && "1" != oldResistance.dark)
				{
					this.particleGameObject = this.resistanceDark;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].light && "1" != oldResistance.light)
				{
					this.particleGameObject = this.resistanceLight;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].stun && "1" != oldResistance.stun)
				{
					this.particleGameObject = this.resistanceStun;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].skillLock && "1" != oldResistance.skillLock)
				{
					this.particleGameObject = this.resistanceSkillLock;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].sleep && "1" != oldResistance.sleep)
				{
					this.particleGameObject = this.resistanceSleep;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].paralysis && "1" != oldResistance.paralysis)
				{
					this.particleGameObject = this.resistanceParalysis;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].confusion && "1" != oldResistance.confusion)
				{
					this.particleGameObject = this.resistanceConfusion;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].poison && "1" != oldResistance.poison)
				{
					this.particleGameObject = this.resistancePoison;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].death && "1" != oldResistance.death)
				{
					this.particleGameObject = this.resistanceDeath;
					break;
				}
			}
			if (null != this.particleGameObject)
			{
				this.particleGameObject.SetActive(true);
			}
			return this.particleGameObject;
		}

		public void TranceEffectActiveSet(bool active)
		{
			if (this.particleGameObject != null)
			{
				this.particleGameObject.SetActive(active);
			}
		}

		public void StartTranceEffect(GameObject particleGameObject, UIPanel cutinPanel)
		{
			if (null != particleGameObject)
			{
				particleGameObject.SetActive(true);
				RenderFrontThanNGUI component = particleGameObject.GetComponent<RenderFrontThanNGUI>();
				if (component != null)
				{
					UIPanel uipanel = this.tranceParent.GetComponent<UIPanel>();
					if (null == uipanel)
					{
						uipanel = this.tranceParent.gameObject.AddComponent<UIPanel>();
					}
					uipanel.sortingOrder = component.GetSortOrder() + 1;
					cutinPanel.sortingOrder = component.GetSortOrder() + 1;
				}
			}
		}

		public void SetViewNextEvolutionMonster(string monsterId, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster)
		{
			GameWebAPI.RespDataMA_GetMonsterMS.MonsterM simple = MonsterMaster.GetMonsterMasterByMonsterId(monsterId).Simple;
			GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = MonsterMaster.GetMonsterMasterByMonsterGroupId(simple.monsterGroupId).Group;
			DataMng.ExperienceInfo experienceInfo = DataMng.Instance().GetExperienceInfo(0);
			MonsterData monsterData = new MonsterData(new GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList(userMonster)
			{
				monsterId = monsterId,
				level = "1",
				ex = "0",
				levelEx = "0",
				nextLevelEx = experienceInfo.expLevNext.ToString()
			});
			if (!string.IsNullOrEmpty(group.leaderSkillId) && "0" != group.leaderSkillId)
			{
				monsterData.userMonster.leaderSkillId = group.leaderSkillId;
				monsterData.InitSkillInfo();
			}
			StatusValue statusValue = MonsterStatusData.GetStatusValue(monsterId, "1");
			statusValue.luck = int.Parse(userMonster.luck);
			monsterData.SetStatus(statusValue);
			CMD_CharacterDetailed.DataChg = monsterData;
			GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceMaster = MonsterResistanceData.GetResistanceMaster(monsterData.monsterM.resistanceId);
			bool active = this.AnyMatchStrongResistance(resistanceMaster, userMonster.tranceResistance, userMonster.tranceStatusAilment);
			this.nextMonsterResistanceAlert.gameObject.SetActive(active);
		}

		private GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM GetResistanceMaster(string resistanceId)
		{
			GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM result = null;
			GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM[] monsterResistanceM = MasterDataMng.Instance().RespDataMA_MonsterResistanceM.monsterResistanceM;
			for (int i = 0; i < monsterResistanceM.Length; i++)
			{
				if (resistanceId == monsterResistanceM[i].monsterResistanceId)
				{
					result = monsterResistanceM[i];
					break;
				}
			}
			return result;
		}

		private enum PageType
		{
			STATUS,
			SKILL,
			CHIP
		}
	}
}
