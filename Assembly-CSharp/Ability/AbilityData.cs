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

		private float GetAbilityUpgradeRate(string baseAbility, string materialAbility, string baseGrowStep, ref bool isNext)
		{
			if (this.abilityUpgradeDataList == null)
			{
				this.SetupAbilityUpgradeDataList();
			}
			if (string.IsNullOrEmpty(materialAbility))
			{
				global::Debug.LogError("============================== Ability.AbilityData.GetAbilityUpgradeRate() materialAbility が、空");
			}
			int num = int.Parse(materialAbility);
			if (num < 5 || num > 20 || (10 < num && num < 15))
			{
				global::Debug.LogError("============================== Ability.AbilityData.GetAbilityUpgradeRate() materialAbility が、空");
			}
			int num2 = 0;
			if (string.IsNullOrEmpty(baseAbility))
			{
				isNext = false;
			}
			else
			{
				num2 = int.Parse(baseAbility);
				if (num2 < 5 || num2 > 20 || (10 < num2 && num2 < 15))
				{
					isNext = false;
				}
				else
				{
					isNext = true;
				}
			}
			int num3 = int.Parse(baseGrowStep);
			float num4 = 1f;
			float num5 = 1f;
			switch (num3)
			{
			case 4:
				num4 = ConstValue.ABILITY_UPGRADE_MULRATE_GROWING;
				num5 = (float)ConstValue.ABILITY_INHERITRATE_GROWING;
				break;
			case 5:
			case 8:
				num4 = ConstValue.ABILITY_UPGRADE_MULRATE_RIPE;
				num5 = (float)ConstValue.ABILITY_INHERITRATE_RIPE;
				break;
			case 6:
				num4 = ConstValue.ABILITY_UPGRADE_MULRATE_PERFECT;
				num5 = (float)ConstValue.ABILITY_INHERITRATE_PERFECT;
				break;
			case 7:
			case 9:
				num4 = ConstValue.ABILITY_UPGRADE_MULRATE_ULTIMATE;
				num5 = (float)ConstValue.ABILITY_INHERITRATE_ULTIMATE;
				break;
			}
			float result;
			if (num2 == 20)
			{
				result = 0f;
			}
			else if (isNext)
			{
				string key = baseAbility + "_" + materialAbility;
				if (!this.abilityUpgradeDataList.ContainsKey(key))
				{
					global::Debug.LogError("============================== Ability.AbilityData.GetAbilityUpgradeRate() Keyがない！");
				}
				float num6 = this.abilityUpgradeDataList[key];
				result = num6 * num4;
			}
			else
			{
				result = num5;
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

		public bool HasAbility(string materialAbility)
		{
			bool result;
			if (string.IsNullOrEmpty(materialAbility))
			{
				result = false;
			}
			else
			{
				int num = int.Parse(materialAbility);
				result = (num >= 5 && num <= 20 && (10 >= num || num >= 15));
			}
			return result;
		}

		public float GetMaxAbility(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster)
		{
			int num = 0;
			int num2 = int.Parse(userMonster.hpAbility);
			num2 = this.CheckAndFixAbility(num2);
			if (num2 > num)
			{
				num = num2;
			}
			num2 = int.Parse(userMonster.attackAbility);
			num2 = this.CheckAndFixAbility(num2);
			if (num2 > num)
			{
				num = num2;
			}
			num2 = int.Parse(userMonster.defenseAbility);
			num2 = this.CheckAndFixAbility(num2);
			if (num2 > num)
			{
				num = num2;
			}
			num2 = int.Parse(userMonster.spAttackAbility);
			num2 = this.CheckAndFixAbility(num2);
			if (num2 > num)
			{
				num = num2;
			}
			num2 = int.Parse(userMonster.spDefenseAbility);
			num2 = this.CheckAndFixAbility(num2);
			if (num2 > num)
			{
				num = num2;
			}
			num2 = int.Parse(userMonster.speedAbility);
			num2 = this.CheckAndFixAbility(num2);
			if (num2 > num)
			{
				num = num2;
			}
			return (float)num;
		}

		private int CheckAndFixAbility(int abilityNum)
		{
			if (abilityNum < 5 || abilityNum > 20 || (10 < abilityNum && abilityNum < 15))
			{
				return 0;
			}
			return abilityNum;
		}

		public int GetTotalAbilityCount(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster)
		{
			int num = 0;
			num += this.CheckValidMedal(userMonster.hpAbilityFlg);
			num += this.CheckValidMedal(userMonster.attackAbilityFlg);
			num += this.CheckValidMedal(userMonster.defenseAbilityFlg);
			num += this.CheckValidMedal(userMonster.spAttackAbilityFlg);
			num += this.CheckValidMedal(userMonster.spDefenseAbilityFlg);
			return num + this.CheckValidMedal(userMonster.speedAbilityFlg);
		}

		private int CheckValidMedal(string abilityFlg)
		{
			int num = int.Parse(abilityFlg);
			if (num == 1)
			{
				return 1;
			}
			if (num == 2)
			{
				return 1;
			}
			return 0;
		}

		public MonsterAbilityStatusInfo CreateAbilityStatus(MonsterData baseData, MonsterData partnerData, ref bool hasGoldOver)
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
			int hasGold = 0;
			int inheritGold = 0;
			int upgradeGold = 0;
			List<AbilityData.UpgradeState> goldMedalStateList = this.GetGoldMedalStateList(userMonster, userMonster2, ref hasGold, ref inheritGold, ref upgradeGold);
			hasGoldOver = this.FixGoldOverRate(goldMedalStateList, monsterAbilityStatusInfo, hasGold, inheritGold, upgradeGold);
			return monsterAbilityStatusInfo;
		}

		private void SetAbilityInfo(string baseAbility, string materialAbility, string baseGrowStep, float campFix, ref string abilityFlg, ref string ability, ref float abilityRate, ref bool noMaterial, ref bool isAbilityMax, ref string abilityMinGuarantee, ref float abilityMinGuaranteeRate)
		{
			bool isNext = false;
			noMaterial = false;
			if (baseAbility == "20")
			{
				isAbilityMax = true;
			}
			else
			{
				isAbilityMax = false;
			}
			if (!this.HasAbility(materialAbility))
			{
				abilityFlg = this.GetAbilityType(baseAbility);
				ability = baseAbility;
				abilityRate = 0f;
				noMaterial = true;
				return;
			}
			abilityRate = this.GetAbilityUpgradeRate(baseAbility, materialAbility, baseGrowStep, ref isNext);
			abilityMinGuarantee = this.GetMinGarantee(isNext, materialAbility, ref abilityMinGuaranteeRate);
			abilityRate *= campFix;
			abilityRate = Mathf.Floor(abilityRate * 10f);
			abilityRate /= 10f;
			if (abilityRate > 100f)
			{
				abilityRate = 100f;
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

		private string GetMinGarantee(bool isNext, string materialAbility, ref float abilityMinGuaranteeRate)
		{
			string result = "0";
			float num = 0f;
			if (!isNext)
			{
				int num2 = int.Parse(materialAbility);
				if (num2 >= 15)
				{
					result = "5";
					num = 100f;
				}
			}
			abilityMinGuaranteeRate = num;
			return result;
		}

		private float GetCampaignFix()
		{
			GameWebAPI.RespDataCP_Campaign respDataCP_Campaign = DataMng.Instance().RespDataCP_Campaign;
			List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> underwayCampaignList = this.GetUnderwayCampaignList(respDataCP_Campaign);
			for (int i = 0; i < underwayCampaignList.Count; i++)
			{
				GameWebAPI.RespDataCP_Campaign.CampaignType cmpIdByEnum = underwayCampaignList[i].GetCmpIdByEnum();
				if (cmpIdByEnum == GameWebAPI.RespDataCP_Campaign.CampaignType.MedalTakeOverUp)
				{
					return float.Parse(underwayCampaignList[i].rate);
				}
			}
			return 1f;
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

		private List<AbilityData.UpgradeState> GetGoldMedalStateList(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList baseMonster, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList matMonster, ref int hasGold, ref int inheritGold, ref int upgradeGold)
		{
			List<AbilityData.UpgradeState> list = new List<AbilityData.UpgradeState>();
			list.Add(new AbilityData.UpgradeState
			{
				abilityState = AbilityData.ABILITY_STATE.HP,
				goldUpState = this.CheckGoldMedalState(baseMonster.hpAbilityFlg, baseMonster.hpAbility, matMonster.hpAbilityFlg)
			});
			list.Add(new AbilityData.UpgradeState
			{
				abilityState = AbilityData.ABILITY_STATE.ATK,
				goldUpState = this.CheckGoldMedalState(baseMonster.attackAbilityFlg, baseMonster.attackAbility, matMonster.attackAbilityFlg)
			});
			list.Add(new AbilityData.UpgradeState
			{
				abilityState = AbilityData.ABILITY_STATE.DEF,
				goldUpState = this.CheckGoldMedalState(baseMonster.defenseAbilityFlg, baseMonster.defenseAbility, matMonster.defenseAbilityFlg)
			});
			list.Add(new AbilityData.UpgradeState
			{
				abilityState = AbilityData.ABILITY_STATE.S_ATK,
				goldUpState = this.CheckGoldMedalState(baseMonster.spAttackAbilityFlg, baseMonster.spAttackAbility, matMonster.spAttackAbilityFlg)
			});
			list.Add(new AbilityData.UpgradeState
			{
				abilityState = AbilityData.ABILITY_STATE.S_DEF,
				goldUpState = this.CheckGoldMedalState(baseMonster.spDefenseAbilityFlg, baseMonster.spDefenseAbility, matMonster.spDefenseAbilityFlg)
			});
			list.Add(new AbilityData.UpgradeState
			{
				abilityState = AbilityData.ABILITY_STATE.SPD,
				goldUpState = this.CheckGoldMedalState(baseMonster.speedAbilityFlg, baseMonster.speedAbility, matMonster.speedAbilityFlg)
			});
			list.Sort(new Comparison<AbilityData.UpgradeState>(this.CompareByGUSTATE));
			hasGold = 0;
			inheritGold = 0;
			upgradeGold = 0;
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				AbilityData.UpgradeState upgradeState = list[i];
				if (upgradeState.goldUpState == AbilityData.GOLD_UPGRADE_STATE.HAS || upgradeState.goldUpState == AbilityData.GOLD_UPGRADE_STATE.INHERIT || upgradeState.goldUpState == AbilityData.GOLD_UPGRADE_STATE.UPGRADE)
				{
					num++;
					upgradeState.totalGoldNum = num;
					if (upgradeState.totalGoldNum > ConstValue.MAX_GOLD_MEDAL_COUNT)
					{
						upgradeState.isGoldOver = true;
					}
					else
					{
						upgradeState.isGoldOver = false;
					}
					if (upgradeState.goldUpState == AbilityData.GOLD_UPGRADE_STATE.HAS)
					{
						hasGold++;
					}
					if (upgradeState.goldUpState == AbilityData.GOLD_UPGRADE_STATE.INHERIT)
					{
						inheritGold++;
					}
					if (upgradeState.goldUpState == AbilityData.GOLD_UPGRADE_STATE.UPGRADE)
					{
						upgradeGold++;
					}
				}
				else
				{
					upgradeState.totalGoldNum = num;
					upgradeState.isGoldOver = false;
				}
			}
			return list;
		}

		private int CompareByGUSTATE(AbilityData.UpgradeState s_a, AbilityData.UpgradeState s_b)
		{
			int goldUpState = (int)s_a.goldUpState;
			int goldUpState2 = (int)s_b.goldUpState;
			if (goldUpState < goldUpState2)
			{
				return -1;
			}
			if (goldUpState > goldUpState2)
			{
				return 1;
			}
			return 0;
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

		private bool FixGoldOverRate(List<AbilityData.UpgradeState> upsList, MonsterAbilityStatusInfo mas, int hasGold, int inheritGold, int upgradeGold)
		{
			bool result = false;
			for (int i = 0; i < upsList.Count; i++)
			{
				if (upsList[i].isGoldOver)
				{
					result = true;
					if (upsList[i].goldUpState == AbilityData.GOLD_UPGRADE_STATE.INHERIT && hasGold >= ConstValue.MAX_GOLD_MEDAL_COUNT)
					{
						switch (upsList[i].abilityState)
						{
						case AbilityData.ABILITY_STATE.HP:
							mas.hpAbilityRate = 0f;
							break;
						case AbilityData.ABILITY_STATE.ATK:
							mas.attackAbilityRate = 0f;
							break;
						case AbilityData.ABILITY_STATE.DEF:
							mas.defenseAbilityRate = 0f;
							break;
						case AbilityData.ABILITY_STATE.S_ATK:
							mas.spAttackAbilityRate = 0f;
							break;
						case AbilityData.ABILITY_STATE.S_DEF:
							mas.spDefenseAbilityRate = 0f;
							break;
						case AbilityData.ABILITY_STATE.SPD:
							mas.speedAbilityRate = 0f;
							break;
						}
					}
					else if (upsList[i].goldUpState == AbilityData.GOLD_UPGRADE_STATE.UPGRADE && hasGold + inheritGold >= ConstValue.MAX_GOLD_MEDAL_COUNT)
					{
						switch (upsList[i].abilityState)
						{
						case AbilityData.ABILITY_STATE.HP:
							mas.hpAbilityRate = 0f;
							break;
						case AbilityData.ABILITY_STATE.ATK:
							mas.attackAbilityRate = 0f;
							break;
						case AbilityData.ABILITY_STATE.DEF:
							mas.defenseAbilityRate = 0f;
							break;
						case AbilityData.ABILITY_STATE.S_ATK:
							mas.spAttackAbilityRate = 0f;
							break;
						case AbilityData.ABILITY_STATE.S_DEF:
							mas.spDefenseAbilityRate = 0f;
							break;
						case AbilityData.ABILITY_STATE.SPD:
							mas.speedAbilityRate = 0f;
							break;
						}
					}
				}
				else
				{
					switch (upsList[i].abilityState)
					{
					case AbilityData.ABILITY_STATE.HP:
						if (this.HasAbility(mas.hpAbility) && mas.hpAbilityRate >= 100f)
						{
							mas.hpAbilityMinGuarantee = "0";
							mas.hpAbilityMinGuaranteeRate = 0f;
						}
						break;
					case AbilityData.ABILITY_STATE.ATK:
						if (this.HasAbility(mas.attackAbility) && mas.attackAbilityRate >= 100f)
						{
							mas.attackAbilityMinGuarantee = "0";
							mas.attackAbilityMinGuaranteeRate = 0f;
						}
						break;
					case AbilityData.ABILITY_STATE.DEF:
						if (this.HasAbility(mas.defenseAbility) && mas.defenseAbilityRate >= 100f)
						{
							mas.defenseAbilityMinGuarantee = "0";
							mas.defenseAbilityMinGuaranteeRate = 0f;
						}
						break;
					case AbilityData.ABILITY_STATE.S_ATK:
						if (this.HasAbility(mas.spAttackAbility) && mas.spAttackAbilityRate >= 100f)
						{
							mas.spAttackAbilityMinGuarantee = "0";
							mas.spAttackAbilityMinGuaranteeRate = 0f;
						}
						break;
					case AbilityData.ABILITY_STATE.S_DEF:
						if (this.HasAbility(mas.spDefenseAbility) && mas.spDefenseAbilityRate >= 100f)
						{
							mas.spDefenseAbilityMinGuarantee = "0";
							mas.spDefenseAbilityMinGuaranteeRate = 0f;
						}
						break;
					case AbilityData.ABILITY_STATE.SPD:
						if (this.HasAbility(mas.speedAbility) && mas.speedAbilityRate >= 100f)
						{
							mas.speedAbilityMinGuarantee = "0";
							mas.speedAbilityMinGuaranteeRate = 0f;
						}
						break;
					}
				}
			}
			return result;
		}

		private void ClearMonsterAbilityStatusInfoRate(MonsterAbilityStatusInfo mas)
		{
			mas.hpAbilityRate = 0f;
			mas.attackAbilityRate = 0f;
			mas.defenseAbilityRate = 0f;
			mas.spAttackAbilityRate = 0f;
			mas.spDefenseAbilityRate = 0f;
			mas.speedAbilityRate = 0f;
		}

		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList MakeUseMonsterForAbility(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList _userMonster)
		{
			return new GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList
			{
				hpAbilityFlg = _userMonster.hpAbilityFlg,
				hpAbility = _userMonster.hpAbility,
				attackAbilityFlg = _userMonster.attackAbilityFlg,
				attackAbility = _userMonster.attackAbility,
				defenseAbilityFlg = _userMonster.defenseAbilityFlg,
				defenseAbility = _userMonster.defenseAbility,
				spAttackAbilityFlg = _userMonster.spAttackAbilityFlg,
				spAttackAbility = _userMonster.spAttackAbility,
				spDefenseAbilityFlg = _userMonster.spDefenseAbilityFlg,
				spDefenseAbility = _userMonster.spDefenseAbility,
				speedAbilityFlg = _userMonster.speedAbilityFlg,
				speedAbility = _userMonster.speedAbility
			};
		}

		public bool IsSuccsessAbilityUpgrade(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList beforeUserMonster, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList afterUserMonster)
		{
			int num = 0;
			int num2 = 0;
			int abilityNum = int.Parse(beforeUserMonster.hpAbility);
			num += this.CheckAndFixAbility(abilityNum);
			abilityNum = int.Parse(beforeUserMonster.attackAbility);
			num += this.CheckAndFixAbility(abilityNum);
			abilityNum = int.Parse(beforeUserMonster.defenseAbility);
			num += this.CheckAndFixAbility(abilityNum);
			abilityNum = int.Parse(beforeUserMonster.spAttackAbility);
			num += this.CheckAndFixAbility(abilityNum);
			abilityNum = int.Parse(beforeUserMonster.spDefenseAbility);
			num += this.CheckAndFixAbility(abilityNum);
			abilityNum = int.Parse(beforeUserMonster.speedAbility);
			num += this.CheckAndFixAbility(abilityNum);
			abilityNum = int.Parse(afterUserMonster.hpAbility);
			num2 += this.CheckAndFixAbility(abilityNum);
			abilityNum = int.Parse(afterUserMonster.attackAbility);
			num2 += this.CheckAndFixAbility(abilityNum);
			abilityNum = int.Parse(afterUserMonster.defenseAbility);
			num2 += this.CheckAndFixAbility(abilityNum);
			abilityNum = int.Parse(afterUserMonster.spAttackAbility);
			num2 += this.CheckAndFixAbility(abilityNum);
			abilityNum = int.Parse(afterUserMonster.spDefenseAbility);
			num2 += this.CheckAndFixAbility(abilityNum);
			abilityNum = int.Parse(afterUserMonster.speedAbility);
			num2 += this.CheckAndFixAbility(abilityNum);
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
			HAS = 1,
			INHERIT,
			UPGRADE,
			NONE
		}

		public class UpgradeState
		{
			public AbilityData.ABILITY_STATE abilityState;

			public AbilityData.GOLD_UPGRADE_STATE goldUpState;

			public int totalGoldNum;

			public bool isGoldOver;
		}
	}
}
