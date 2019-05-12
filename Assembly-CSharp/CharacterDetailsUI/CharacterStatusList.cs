using Master;
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
		private Vector3 levelUpImagePosition;

		[SerializeField]
		private Vector3 levelUpEffectPosition;

		[SerializeField]
		private Transform tranceParent;

		[SerializeField]
		private Vector3 tranceImagePosition;

		[SerializeField]
		private Vector3 tranceEffectPosition;

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
			transform.localPosition = this.levelUpImagePosition;
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
			transform.localPosition = this.tranceImagePosition;
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
			if (!monsterData.IsVersionUp())
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
			this.viewExtraSkillPage = monsterData.IsVersionUp();
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
					GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterGroupId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterEvolutionRouteM.eggMonsterId);
					if (monsterGroupMasterByMonsterGroupId != null)
					{
						eggName = monsterGroupMasterByMonsterGroupId.monsterName;
					}
				}
			}
			if (!monsterData.IsVersionUp())
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
			transform.SetParent(windowRootTransform);
			transform.localPosition = this.levelUpEffectPosition;
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
			transform.SetParent(windowRootTransform);
			transform.localPosition = this.tranceEffectPosition;
			Animation result = this.ShowTranceAnimation();
			SoundMng.Instance().TryPlaySE("SEInternal/Common/se_101", 0f, false, true, null, -1);
			return result;
		}

		public GameObject GetTranceEffectObject(MonsterData oldMonsterData, MonsterData newMonsterData)
		{
			GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM monsterResistanceM = oldMonsterData.AddResistanceFromMultipleTranceData();
			List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> userUnitResistanceList = newMonsterData.GetUserUnitResistanceList();
			this.particleGameObject = null;
			for (int i = 0; i < userUnitResistanceList.Count; i++)
			{
				if ("1" == userUnitResistanceList[i].none && "1" != monsterResistanceM.none)
				{
					this.particleGameObject = this.resistanceNone;
					break;
				}
				if ("1" == userUnitResistanceList[i].fire && "1" != monsterResistanceM.fire)
				{
					this.particleGameObject = this.resistanceFire;
					break;
				}
				if ("1" == userUnitResistanceList[i].water && "1" != monsterResistanceM.water)
				{
					this.particleGameObject = this.resistanceWater;
					break;
				}
				if ("1" == userUnitResistanceList[i].thunder && "1" != monsterResistanceM.thunder)
				{
					this.particleGameObject = this.resistanceThunder;
					break;
				}
				if ("1" == userUnitResistanceList[i].nature && "1" != monsterResistanceM.nature)
				{
					this.particleGameObject = this.resistanceNature;
					break;
				}
				if ("1" == userUnitResistanceList[i].dark && "1" != monsterResistanceM.dark)
				{
					this.particleGameObject = this.resistanceDark;
					break;
				}
				if ("1" == userUnitResistanceList[i].light && "1" != monsterResistanceM.light)
				{
					this.particleGameObject = this.resistanceLight;
					break;
				}
				if ("1" == userUnitResistanceList[i].stun && "1" != monsterResistanceM.stun)
				{
					this.particleGameObject = this.resistanceStun;
					break;
				}
				if ("1" == userUnitResistanceList[i].skillLock && "1" != monsterResistanceM.skillLock)
				{
					this.particleGameObject = this.resistanceSkillLock;
					break;
				}
				if ("1" == userUnitResistanceList[i].sleep && "1" != monsterResistanceM.sleep)
				{
					this.particleGameObject = this.resistanceSleep;
					break;
				}
				if ("1" == userUnitResistanceList[i].paralysis && "1" != monsterResistanceM.paralysis)
				{
					this.particleGameObject = this.resistanceParalysis;
					break;
				}
				if ("1" == userUnitResistanceList[i].confusion && "1" != monsterResistanceM.confusion)
				{
					this.particleGameObject = this.resistanceConfusion;
					break;
				}
				if ("1" == userUnitResistanceList[i].poison && "1" != monsterResistanceM.poison)
				{
					this.particleGameObject = this.resistancePoison;
					break;
				}
				if ("1" == userUnitResistanceList[i].death && "1" != monsterResistanceM.death)
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
			GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterMasterByMonsterId = MonsterDataMng.Instance().GetMonsterMasterByMonsterId(monsterId);
			GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterGroupId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterMasterByMonsterId.monsterGroupId);
			MonsterData monsterData = new MonsterData();
			DataMng.ExperienceInfo experienceInfo = DataMng.Instance().GetExperienceInfo(0);
			monsterData.userMonster = new GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList(userMonster);
			monsterData.userMonster.monsterId = monsterId;
			monsterData.userMonster.level = "1";
			monsterData.userMonster.ex = "0";
			monsterData.userMonster.levelEx = "0";
			monsterData.userMonster.nextLevelEx = experienceInfo.expLevNext.ToString();
			if (!string.IsNullOrEmpty(monsterGroupMasterByMonsterGroupId.leaderSkillId) && "0" != monsterGroupMasterByMonsterGroupId.leaderSkillId)
			{
				monsterData.userMonster.leaderSkillId = monsterGroupMasterByMonsterGroupId.leaderSkillId;
			}
			monsterData.monsterM = monsterMasterByMonsterId;
			monsterData.monsterMG = monsterGroupMasterByMonsterGroupId;
			monsterData.InitSkillInfo();
			monsterData.InitGrowStepInfo();
			monsterData.InitResistanceInfo();
			monsterData.InitTribeInfo();
			monsterData.userMonster.hp = string.Empty;
			monsterData.userMonster.attack = string.Empty;
			monsterData.userMonster.defense = string.Empty;
			monsterData.userMonster.spAttack = string.Empty;
			monsterData.userMonster.spDefense = string.Empty;
			monsterData.userMonster.speed = string.Empty;
			monsterData.UpdateNowParam(1);
			monsterData.userMonster.luck = userMonster.luck;
			monsterData.InitChipInfo();
			CMD_CharacterDetailed.DataChg = monsterData;
			bool active = this.AnyMatchStrongResistance(monsterData.resistanceM, userMonster.tranceResistance, userMonster.tranceStatusAilment);
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
