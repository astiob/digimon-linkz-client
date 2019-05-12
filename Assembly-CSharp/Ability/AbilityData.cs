using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ability
{
	public sealed class AbilityData : ClassSingleton<AbilityData>
	{
		private Dictionary<string, float> abilityUpgradeDataList;

		private void SetupAbilityUpgradeDataList()
		{
			this.abilityUpgradeDataList = new Dictionary<string, float>();
			GameWebAPI.RespDataMA_AbilityUpgradeM.AbilityUpgradeRateM[] abilityUpgradeRateM = MasterDataMng.Instance().RespDataMA_AbilityUpgradeM.abilityUpgradeRateM;
			for (int i = 0; i < abilityUpgradeRateM.Length; i++)
			{
				string key = abilityUpgradeRateM[i].baseAbility + "_" + abilityUpgradeRateM[i].materialAbility;
				string rate = abilityUpgradeRateM[i].rate;
				float num = (float)int.Parse(rate);
				float value = num / 100f;
				this.abilityUpgradeDataList.Add(key, value);
			}
		}

		private float GetAbilityUpgradeRate(string baseAbility, string materialAbility, string baseGrowStep, ref bool hasMedal)
		{
			if (this.abilityUpgradeDataList == null)
			{
				this.SetupAbilityUpgradeDataList();
			}
			float result = 0f;
			hasMedal = this.ExistMedalByParcentage(baseAbility);
			int medalParcentage = this.GetMedalParcentage(baseAbility);
			int growStep = int.Parse(baseGrowStep);
			float num = 1f;
			float num2 = 1f;
			if (MonsterGrowStepData.IsGrowingScope(growStep))
			{
				num = ConstValue.ABILITY_UPGRADE_MULRATE_GROWING;
				num2 = (float)ConstValue.ABILITY_INHERITRATE_GROWING;
			}
			else if (MonsterGrowStepData.IsRipeScope(growStep))
			{
				num = ConstValue.ABILITY_UPGRADE_MULRATE_RIPE;
				num2 = (float)ConstValue.ABILITY_INHERITRATE_RIPE;
			}
			else if (MonsterGrowStepData.IsPerfectScope(growStep))
			{
				num = ConstValue.ABILITY_UPGRADE_MULRATE_PERFECT;
				num2 = (float)ConstValue.ABILITY_INHERITRATE_PERFECT;
			}
			else if (MonsterGrowStepData.IsUltimateScope(growStep))
			{
				num = ConstValue.ABILITY_UPGRADE_MULRATE_ULTIMATE;
				num2 = (float)ConstValue.ABILITY_INHERITRATE_ULTIMATE;
			}
			if (medalParcentage < 20)
			{
				if (hasMedal)
				{
					string key = baseAbility + "_" + materialAbility;
					if (this.abilityUpgradeDataList.ContainsKey(key))
					{
						result = num * this.abilityUpgradeDataList[key];
					}
				}
				else
				{
					result = num2;
				}
			}
			return result;
		}

		public int GetNextAbility(string baseAbility)
		{
			int num = 0;
			bool flag;
			if (string.IsNullOrEmpty(baseAbility))
			{
				flag = false;
			}
			else
			{
				num = int.Parse(baseAbility);
				flag = (num >= 5 && num <= 20 && (10 >= num || num >= 15));
			}
			int num2;
			if (!flag)
			{
				num2 = 5;
			}
			else
			{
				num2 = num + 1;
				if (10 < num2 && num2 < 15)
				{
					num2 = 15;
				}
				if (num2 > 20)
				{
					num2 = 20;
				}
			}
			return num2;
		}

		public string GetAbilityType(string baseAbility)
		{
			int num = 0;
			bool flag;
			if (string.IsNullOrEmpty(baseAbility))
			{
				flag = false;
			}
			else
			{
				num = int.Parse(baseAbility);
				flag = (num >= 5 && num <= 20 && (10 >= num || num >= 15));
			}
			int num2 = 0;
			if (!flag)
			{
				num2 = 0;
			}
			else if (5 <= num && num <= 10)
			{
				num2 = 2;
			}
			else if (15 <= num && num <= 20)
			{
				num2 = 1;
			}
			return num2.ToString();
		}

		private bool ExistMedalByParcentage(string upParcentage)
		{
			bool result = false;
			int num;
			if (!string.IsNullOrEmpty(upParcentage) && int.TryParse(upParcentage, out num))
			{
				result = ((5 <= num && num <= 10) || (15 <= num && num <= 20));
			}
			return result;
		}

		public float GetMaxAbility(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList baseMonster)
		{
			return (float)Mathf.Max(new int[]
			{
				this.GetMedalParcentage(baseMonster.hpAbility),
				this.GetMedalParcentage(baseMonster.attackAbility),
				this.GetMedalParcentage(baseMonster.defenseAbility),
				this.GetMedalParcentage(baseMonster.spAttackAbility),
				this.GetMedalParcentage(baseMonster.spDefenseAbility),
				this.GetMedalParcentage(baseMonster.speedAbility)
			});
		}

		private int GetMedalParcentage(string upParcentage)
		{
			int num = 0;
			if (!string.IsNullOrEmpty(upParcentage) && int.TryParse(upParcentage, out num))
			{
				num = ((0 <= num) ? num : 0);
				num = ((20 >= num) ? num : 20);
			}
			return num;
		}

		public int GetTotalAbilityCount(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList materialMonster)
		{
			int num = 0;
			num += ((!this.ExistMedalByMedalType(materialMonster.hpAbilityFlg)) ? 0 : 1);
			num += ((!this.ExistMedalByMedalType(materialMonster.attackAbilityFlg)) ? 0 : 1);
			num += ((!this.ExistMedalByMedalType(materialMonster.defenseAbilityFlg)) ? 0 : 1);
			num += ((!this.ExistMedalByMedalType(materialMonster.spAttackAbilityFlg)) ? 0 : 1);
			num += ((!this.ExistMedalByMedalType(materialMonster.spDefenseAbilityFlg)) ? 0 : 1);
			return num + ((!this.ExistMedalByMedalType(materialMonster.speedAbilityFlg)) ? 0 : 1);
		}

		private bool ExistMedalByMedalType(string medalType)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(medalType))
			{
				int num = int.Parse(medalType);
				if (num == 1 || num == 2)
				{
					result = true;
				}
			}
			return result;
		}

		public MonsterAbilityStatusInfo CreateAbilityStatus(MonsterData baseData, MonsterData partnerData)
		{
			MonsterAbilityStatusInfo monsterAbilityStatusInfo = new MonsterAbilityStatusInfo();
			float campaignFix = this.GetCampaignFix();
			GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster = baseData.userMonster;
			GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster2 = partnerData.userMonster;
			string growStep = partnerData.monsterMG.growStep;
			this.SetAbilityInfo(userMonster.hpAbility, userMonster2.hpAbility, growStep, campaignFix, ref monsterAbilityStatusInfo.hpAbilityFlg, ref monsterAbilityStatusInfo.hpAbility, ref monsterAbilityStatusInfo.hpAbilityRate, ref monsterAbilityStatusInfo.hpNoMaterial, ref monsterAbilityStatusInfo.hpIsAbilityMax, ref monsterAbilityStatusInfo.hpAbilityMinGuarantee, ref monsterAbilityStatusInfo.hpAbilityMinGuaranteeRate);
			this.SetAbilityInfo(userMonster.attackAbility, userMonster2.attackAbility, growStep, campaignFix, ref monsterAbilityStatusInfo.attackAbilityFlg, ref monsterAbilityStatusInfo.attackAbility, ref monsterAbilityStatusInfo.attackAbilityRate, ref monsterAbilityStatusInfo.attackNoMaterial, ref monsterAbilityStatusInfo.attackIsAbilityMax, ref monsterAbilityStatusInfo.attackAbilityMinGuarantee, ref monsterAbilityStatusInfo.attackAbilityMinGuaranteeRate);
			this.SetAbilityInfo(userMonster.defenseAbility, userMonster2.defenseAbility, growStep, campaignFix, ref monsterAbilityStatusInfo.defenseAbilityFlg, ref monsterAbilityStatusInfo.defenseAbility, ref monsterAbilityStatusInfo.defenseAbilityRate, ref monsterAbilityStatusInfo.defenseNoMaterial, ref monsterAbilityStatusInfo.defenseIsAbilityMax, ref monsterAbilityStatusInfo.defenseAbilityMinGuarantee, ref monsterAbilityStatusInfo.defenseAbilityMinGuaranteeRate);
			this.SetAbilityInfo(userMonster.spAttackAbility, userMonster2.spAttackAbility, growStep, campaignFix, ref monsterAbilityStatusInfo.spAttackAbilityFlg, ref monsterAbilityStatusInfo.spAttackAbility, ref monsterAbilityStatusInfo.spAttackAbilityRate, ref monsterAbilityStatusInfo.spAttackNoMaterial, ref monsterAbilityStatusInfo.spAttackIsAbilityMax, ref monsterAbilityStatusInfo.spAttackAbilityMinGuarantee, ref monsterAbilityStatusInfo.spAttackAbilityMinGuaranteeRate);
			this.SetAbilityInfo(userMonster.spDefenseAbility, userMonster2.spDefenseAbility, growStep, campaignFix, ref monsterAbilityStatusInfo.spDefenseAbilityFlg, ref monsterAbilityStatusInfo.spDefenseAbility, ref monsterAbilityStatusInfo.spDefenseAbilityRate, ref monsterAbilityStatusInfo.spDefenseNoMaterial, ref monsterAbilityStatusInfo.spDefenseIsAbilityMax, ref monsterAbilityStatusInfo.spDefenseAbilityMinGuarantee, ref monsterAbilityStatusInfo.spDefenseAbilityMinGuaranteeRate);
			this.SetAbilityInfo(userMonster.speedAbility, userMonster2.speedAbility, growStep, campaignFix, ref monsterAbilityStatusInfo.speedAbilityFlg, ref monsterAbilityStatusInfo.speedAbility, ref monsterAbilityStatusInfo.speedAbilityRate, ref monsterAbilityStatusInfo.speedNoMaterial, ref monsterAbilityStatusInfo.speedIsAbilityMax, ref monsterAbilityStatusInfo.speedAbilityMinGuarantee, ref monsterAbilityStatusInfo.speedAbilityMinGuaranteeRate);
			return monsterAbilityStatusInfo;
		}

		public void AdjustMedalInheritanceRate(MonsterAbilityStatusInfo statusInfo, List<AbilityData.GoldMedalInheritanceState> inheritanceStates, int maxGoldMedalCount)
		{
			int num = 0;
			for (int i = 0; i < inheritanceStates.Count; i++)
			{
				if (inheritanceStates[i].goldUpState == AbilityData.GOLD_UPGRADE_STATE.HAS)
				{
					num++;
				}
			}
			for (int j = 0; j < inheritanceStates.Count; j++)
			{
				if (num >= maxGoldMedalCount)
				{
					this.AdjustGoldMedalInheritanceRate(inheritanceStates[j], statusInfo);
				}
			}
		}

		public int GetCountInheritanceGoldMedal(List<AbilityData.GoldMedalInheritanceState> inheritanceStates)
		{
			int num = 0;
			for (int i = 0; i < inheritanceStates.Count; i++)
			{
				if (inheritanceStates[i].goldUpState != AbilityData.GOLD_UPGRADE_STATE.NONE)
				{
					num++;
				}
			}
			return num;
		}

		private void SetAbilityInfo(string baseAbility, string materialAbility, string baseGrowStep, float campFix, ref string abilityFlg, ref string ability, ref float abilityRate, ref bool noMaterial, ref bool isAbilityMax, ref string abilityMinGuarantee, ref float abilityMinGuaranteeRate)
		{
			bool hasMedalBaseMonster = false;
			noMaterial = false;
			if (baseAbility == "20")
			{
				isAbilityMax = true;
			}
			else
			{
				isAbilityMax = false;
			}
			if (!this.ExistMedalByParcentage(materialAbility))
			{
				abilityFlg = this.GetAbilityType(baseAbility);
				ability = baseAbility;
				abilityRate = 0f;
				noMaterial = true;
				return;
			}
			abilityRate = this.GetAbilityUpgradeRate(baseAbility, materialAbility, baseGrowStep, ref hasMedalBaseMonster);
			abilityRate *= campFix;
			abilityRate = Mathf.Floor(abilityRate * 10f);
			abilityRate = Mathf.Min(abilityRate / 10f, 100f);
			if (this.CheckMinGarantee(hasMedalBaseMonster, materialAbility, abilityRate))
			{
				abilityMinGuarantee = "5";
				abilityMinGuaranteeRate = 100f;
			}
			else
			{
				abilityMinGuarantee = "0";
				abilityMinGuaranteeRate = 0f;
			}
			int nextAbility = this.GetNextAbility(baseAbility);
			if (nextAbility == 5)
			{
				ability = materialAbility;
			}
			else
			{
				ability = nextAbility.ToString();
			}
			abilityFlg = this.GetAbilityType(ability);
		}

		private bool CheckMinGarantee(bool hasMedalBaseMonster, string materialUpParcentage, float inheritanceRate)
		{
			bool result = false;
			if (!hasMedalBaseMonster && !string.IsNullOrEmpty(materialUpParcentage) && 100f > inheritanceRate)
			{
				int medalParcentage = this.GetMedalParcentage(materialUpParcentage);
				result = (15 <= medalParcentage && medalParcentage <= 20);
			}
			return result;
		}

		private float GetCampaignFix()
		{
			float result = 1f;
			GameWebAPI.RespDataCP_Campaign respDataCP_Campaign = DataMng.Instance().RespDataCP_Campaign;
			List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> underwayCampaignList = this.GetUnderwayCampaignList(respDataCP_Campaign);
			for (int i = 0; i < underwayCampaignList.Count; i++)
			{
				GameWebAPI.RespDataCP_Campaign.CampaignType cmpIdByEnum = underwayCampaignList[i].GetCmpIdByEnum();
				if (cmpIdByEnum == GameWebAPI.RespDataCP_Campaign.CampaignType.MedalTakeOverUp)
				{
					result = float.Parse(underwayCampaignList[i].rate);
					break;
				}
			}
			return result;
		}

		private List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> GetUnderwayCampaignList(GameWebAPI.RespDataCP_Campaign campaign)
		{
			List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> list = new List<GameWebAPI.RespDataCP_Campaign.CampaignInfo>();
			DateTime now = ServerDateTime.Now;
			for (int i = 0; i < campaign.campaignInfo.Length; i++)
			{
				if (campaign.campaignInfo[i].IsUnderway(now))
				{
					list.Add(campaign.campaignInfo[i]);
				}
			}
			return list;
		}

		public List<AbilityData.GoldMedalInheritanceState> GetGoldMedalInheritanceList(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList baseMonster, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList matMonster)
		{
			return new List<AbilityData.GoldMedalInheritanceState>
			{
				new AbilityData.GoldMedalInheritanceState
				{
					abilityState = AbilityData.ABILITY_STATE.HP,
					goldUpState = this.CheckGoldMedalState(baseMonster.hpAbilityFlg, baseMonster.hpAbility, matMonster.hpAbilityFlg)
				},
				new AbilityData.GoldMedalInheritanceState
				{
					abilityState = AbilityData.ABILITY_STATE.ATK,
					goldUpState = this.CheckGoldMedalState(baseMonster.attackAbilityFlg, baseMonster.attackAbility, matMonster.attackAbilityFlg)
				},
				new AbilityData.GoldMedalInheritanceState
				{
					abilityState = AbilityData.ABILITY_STATE.DEF,
					goldUpState = this.CheckGoldMedalState(baseMonster.defenseAbilityFlg, baseMonster.defenseAbility, matMonster.defenseAbilityFlg)
				},
				new AbilityData.GoldMedalInheritanceState
				{
					abilityState = AbilityData.ABILITY_STATE.S_ATK,
					goldUpState = this.CheckGoldMedalState(baseMonster.spAttackAbilityFlg, baseMonster.spAttackAbility, matMonster.spAttackAbilityFlg)
				},
				new AbilityData.GoldMedalInheritanceState
				{
					abilityState = AbilityData.ABILITY_STATE.S_DEF,
					goldUpState = this.CheckGoldMedalState(baseMonster.spDefenseAbilityFlg, baseMonster.spDefenseAbility, matMonster.spDefenseAbilityFlg)
				},
				new AbilityData.GoldMedalInheritanceState
				{
					abilityState = AbilityData.ABILITY_STATE.SPD,
					goldUpState = this.CheckGoldMedalState(baseMonster.speedAbilityFlg, baseMonster.speedAbility, matMonster.speedAbilityFlg)
				}
			};
		}

		private AbilityData.GOLD_UPGRADE_STATE CheckGoldMedalState(string baseAbilityFlg, string baseAbility, string matAbilityFlg)
		{
			int num = int.Parse(baseAbilityFlg);
			int num2 = int.Parse(baseAbility);
			int num3 = int.Parse(matAbilityFlg);
			if (num == 1)
			{
				return AbilityData.GOLD_UPGRADE_STATE.HAS;
			}
			if (num == 2)
			{
				if (num2 == 10 && num3 > 0)
				{
					return AbilityData.GOLD_UPGRADE_STATE.UPGRADE;
				}
			}
			else if (num3 == 1)
			{
				return AbilityData.GOLD_UPGRADE_STATE.INHERIT;
			}
			return AbilityData.GOLD_UPGRADE_STATE.NONE;
		}

		private void AdjustGoldMedalInheritanceRate(AbilityData.GoldMedalInheritanceState upgradeState, MonsterAbilityStatusInfo info)
		{
			if (upgradeState.goldUpState == AbilityData.GOLD_UPGRADE_STATE.INHERIT || upgradeState.goldUpState == AbilityData.GOLD_UPGRADE_STATE.UPGRADE)
			{
				switch (upgradeState.abilityState)
				{
				case AbilityData.ABILITY_STATE.HP:
					info.hpAbilityRate = 0f;
					break;
				case AbilityData.ABILITY_STATE.ATK:
					info.attackAbilityRate = 0f;
					break;
				case AbilityData.ABILITY_STATE.DEF:
					info.defenseAbilityRate = 0f;
					break;
				case AbilityData.ABILITY_STATE.S_ATK:
					info.spAttackAbilityRate = 0f;
					break;
				case AbilityData.ABILITY_STATE.S_DEF:
					info.spDefenseAbilityRate = 0f;
					break;
				case AbilityData.ABILITY_STATE.SPD:
					info.speedAbilityRate = 0f;
					break;
				}
			}
		}

		public bool IsSuccsessAbilityUpgrade(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList beforeUserMonster, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList afterUserMonster)
		{
			int num = 0;
			int num2 = 0;
			num += this.GetMedalParcentage(beforeUserMonster.hpAbility);
			num += this.GetMedalParcentage(beforeUserMonster.attackAbility);
			num += this.GetMedalParcentage(beforeUserMonster.defenseAbility);
			num += this.GetMedalParcentage(beforeUserMonster.spAttackAbility);
			num += this.GetMedalParcentage(beforeUserMonster.spDefenseAbility);
			num += this.GetMedalParcentage(beforeUserMonster.speedAbility);
			num2 += this.GetMedalParcentage(afterUserMonster.hpAbility);
			num2 += this.GetMedalParcentage(afterUserMonster.attackAbility);
			num2 += this.GetMedalParcentage(afterUserMonster.defenseAbility);
			num2 += this.GetMedalParcentage(afterUserMonster.spAttackAbility);
			num2 += this.GetMedalParcentage(afterUserMonster.spDefenseAbility);
			num2 += this.GetMedalParcentage(afterUserMonster.speedAbility);
			return num < num2;
		}

		public float GetTotalAbilityRate(MonsterAbilityStatusInfo mas)
		{
			float num = 0f;
			num += mas.hpAbilityRate;
			num += mas.attackAbilityRate;
			num += mas.defenseAbilityRate;
			num += mas.spAttackAbilityRate;
			num += mas.spDefenseAbilityRate;
			num += mas.speedAbilityRate;
			num += mas.hpAbilityMinGuaranteeRate;
			num += mas.attackAbilityMinGuaranteeRate;
			num += mas.defenseAbilityMinGuaranteeRate;
			num += mas.spAttackAbilityMinGuaranteeRate;
			num += mas.spDefenseAbilityMinGuaranteeRate;
			return num + mas.speedAbilityMinGuaranteeRate;
		}

		public bool IsAnyNotUpdate(MonsterAbilityStatusInfo mas)
		{
			bool result = false;
			if (!mas.hpNoMaterial && mas.hpAbilityRate <= 0f)
			{
				result = true;
			}
			if (!mas.attackNoMaterial && mas.attackAbilityRate <= 0f)
			{
				result = true;
			}
			if (!mas.defenseNoMaterial && mas.defenseAbilityRate <= 0f)
			{
				result = true;
			}
			if (!mas.spAttackNoMaterial && mas.spAttackAbilityRate <= 0f)
			{
				result = true;
			}
			if (!mas.spDefenseNoMaterial && mas.spDefenseAbilityRate <= 0f)
			{
				result = true;
			}
			if (!mas.speedNoMaterial && mas.speedAbilityRate <= 0f)
			{
				result = true;
			}
			return result;
		}

		public enum ABILITY_STATE
		{
			NONE,
			HP,
			ATK,
			DEF,
			S_ATK,
			S_DEF,
			SPD
		}

		public enum GOLD_UPGRADE_STATE
		{
			NONE,
			HAS,
			INHERIT,
			UPGRADE
		}

		public class GoldMedalInheritanceState
		{
			public AbilityData.ABILITY_STATE abilityState;

			public AbilityData.GOLD_UPGRADE_STATE goldUpState;
		}
	}
}
