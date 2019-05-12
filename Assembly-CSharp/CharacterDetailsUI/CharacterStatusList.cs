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
		private ChipBaseSelect chipBaseSelect;

		[SerializeField]
		private UILabel nextMonsterResistanceAlert;

		private int pageNo;

		private bool viewExtraSkillPage;

		[SerializeField]
		private CharacterStatusResistance resistance;

		[SerializeField]
		private CharacterStatusReinforcement reinforcement;

		private bool enablePageChange;

		public CharacterStatusResistance GetResistance()
		{
			return this.resistance;
		}

		public CharacterStatusReinforcement GetReinforcement()
		{
			return this.reinforcement;
		}

		public void OnPushStatusList()
		{
			if (this.enablePageChange)
			{
				this.pageNo++;
				this.pageNo = this.SetPageContent(this.pageNo);
			}
		}

		public void EnablePage(bool enable)
		{
			this.enablePageChange = enable;
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
			CharacterStatusList.PageType pageType = this.pageContent[pageNo];
			if (pageType != CharacterStatusList.PageType.STATUS)
			{
				if (pageType != CharacterStatusList.PageType.SKILL)
				{
					if (pageType == CharacterStatusList.PageType.CHIP)
					{
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
					}
				}
				else if (!this.viewExtraSkillPage)
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
			}
			else
			{
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
			this.statusList.SetValues(monsterData, true, true);
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
