using System;
using System.Collections.Generic;

namespace Monster
{
	public class MonsterUserData
	{
		private GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;

		private MonsterClientMaster monsterMaster;

		private MonsterSkillClientMaster leaderSkill;

		private MonsterSkillClientMaster uniqueSkill;

		private MonsterSkillClientMaster commonSkill;

		private MonsterSkillClientMaster extraCommonSkill;

		private MonsterChipEquipData chipEquip;

		protected MonsterUserData(string monsterId)
		{
		}

		public MonsterUserData(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList responseUserMonster)
		{
			this.userMonster = responseUserMonster;
			this.chipEquip = new MonsterChipEquipData();
			this.SetMonsterData();
		}

		private void SetMonsterData()
		{
			this.monsterMaster = MonsterMaster.GetMonsterMasterByMonsterId(this.userMonster.monsterId);
			this.uniqueSkill = MonsterSkillData.GetSkillMasterBySkillGroupId(this.monsterMaster.Simple.skillGroupId, this.userMonster.defaultSkillGroupSubId);
			this.commonSkill = MonsterSkillData.GetSkillMasterBySkillId(this.userMonster.commonSkillId);
			if (!string.IsNullOrEmpty(this.userMonster.extraCommonSkillId))
			{
				this.extraCommonSkill = MonsterSkillData.GetSkillMasterBySkillId(this.userMonster.extraCommonSkillId);
			}
			if ("0" != this.userMonster.leaderSkillId)
			{
				this.leaderSkill = MonsterSkillData.GetSkillMasterBySkillId(this.userMonster.leaderSkillId);
			}
			this.chipEquip.SetChipEquip(this.userMonster.userMonsterId);
		}

		protected void SetMonsterMaster(string monsterId)
		{
			this.monsterMaster = MonsterMaster.GetMonsterMasterByMonsterId(monsterId);
		}

		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList GetMonster()
		{
			return this.userMonster;
		}

		public void SetMonster(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster)
		{
			this.userMonster = userMonster;
			this.SetMonsterData();
		}

		public MonsterClientMaster GetMonsterMaster()
		{
			return this.monsterMaster;
		}

		public GameWebAPI.RespDataMA_GetSkillM.SkillM GetUniqueSkill()
		{
			return this.uniqueSkill.Simple;
		}

		public GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM GetUniqueSkillDetail()
		{
			return this.uniqueSkill.ViewSkillDetail;
		}

		public void SetUniqueSkill(string monsterId, string defaultSkillGroupSubId)
		{
			MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(monsterId);
			if (monsterMasterByMonsterId != null)
			{
				this.uniqueSkill = MonsterSkillData.GetSkillMasterBySkillGroupId(monsterMasterByMonsterId.Simple.skillGroupId, defaultSkillGroupSubId);
			}
		}

		public GameWebAPI.RespDataMA_GetSkillM.SkillM GetCommonSkill()
		{
			return this.commonSkill.Simple;
		}

		public GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM GetCommonSkillDetail()
		{
			return this.commonSkill.ViewSkillDetail;
		}

		public void SetCommonSkill(string skillId)
		{
			this.commonSkill = MonsterSkillData.GetSkillMasterBySkillId(skillId);
		}

		public GameWebAPI.RespDataMA_GetSkillM.SkillM GetExtraCommonSkill()
		{
			return (this.extraCommonSkill == null) ? null : this.extraCommonSkill.Simple;
		}

		public GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM GetExtraCommonSkillDetail()
		{
			return (this.extraCommonSkill == null) ? null : this.extraCommonSkill.ViewSkillDetail;
		}

		public void SetExtraCommonSkill(string skillId)
		{
			if (!string.IsNullOrEmpty(skillId))
			{
				this.extraCommonSkill = MonsterSkillData.GetSkillMasterBySkillId(skillId);
			}
		}

		public GameWebAPI.RespDataMA_GetSkillM.SkillM GetLeaderSkill()
		{
			GameWebAPI.RespDataMA_GetSkillM.SkillM result = null;
			if (this.leaderSkill != null)
			{
				result = this.leaderSkill.Simple;
			}
			return result;
		}

		public void SetLeaderSkill(string skillId)
		{
			if ("0" != skillId)
			{
				this.leaderSkill = MonsterSkillData.GetSkillMasterBySkillId(skillId);
			}
		}

		public int GetPrice()
		{
			int num = int.Parse(this.monsterMaster.Simple.price);
			int num2 = int.Parse(this.monsterMaster.Simple.priceRise);
			int num3 = int.Parse(this.userMonster.level);
			return num + num2 * (num3 - 1);
		}

		public StatusValue GetStatus()
		{
			return new StatusValue
			{
				hp = int.Parse(this.userMonster.hp),
				attack = int.Parse(this.userMonster.attack),
				defense = int.Parse(this.userMonster.defense),
				magicAttack = int.Parse(this.userMonster.spAttack),
				magicDefense = int.Parse(this.userMonster.spDefense),
				speed = int.Parse(this.userMonster.speed)
			};
		}

		public void SetStatus(StatusValue status)
		{
			this.userMonster.hp = status.hp.ToString();
			this.userMonster.attack = status.attack.ToString();
			this.userMonster.defense = status.defense.ToString();
			this.userMonster.spAttack = status.magicAttack.ToString();
			this.userMonster.spDefense = status.magicDefense.ToString();
			this.userMonster.speed = status.speed.ToString();
			this.userMonster.luck = status.luck.ToString();
		}

		public List<string> GetResistanceIdList()
		{
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(this.userMonster.tranceResistance))
			{
				list.AddRange(this.userMonster.tranceResistance.Split(new char[]
				{
					','
				}));
			}
			if (!string.IsNullOrEmpty(this.userMonster.tranceStatusAilment))
			{
				list.AddRange(this.userMonster.tranceStatusAilment.Split(new char[]
				{
					','
				}));
			}
			return list;
		}

		public bool ExistMedal()
		{
			bool result = false;
			if ("0" != this.userMonster.hpAbilityFlg)
			{
				result = true;
			}
			else if ("0" != this.userMonster.attackAbilityFlg)
			{
				result = true;
			}
			else if ("0" != this.userMonster.defenseAbilityFlg)
			{
				result = true;
			}
			else if ("0" != this.userMonster.spAttackAbilityFlg)
			{
				result = true;
			}
			else if ("0" != this.userMonster.spDefenseAbilityFlg)
			{
				result = true;
			}
			else if ("0" != this.userMonster.speedAbilityFlg)
			{
				result = true;
			}
			return result;
		}

		public bool IsMaxLevel()
		{
			int num = int.Parse(this.userMonster.level);
			int num2 = int.Parse(this.monsterMaster.Simple.maxLevel);
			return num2 <= num;
		}

		public MonsterChipEquipData GetChipEquip()
		{
			return this.chipEquip;
		}

		public void CreateEmptyChipEquip()
		{
			this.chipEquip.SetEmptyChipEquip(this.userMonster.userMonsterId);
		}

		public bool CanVersionUp()
		{
			bool result = false;
			if (MonsterGrowStepData.IsUltimateScope(this.monsterMaster.Group.growStep) && int.Parse(this.monsterMaster.Simple.rare) == 5)
			{
				result = true;
			}
			return result;
		}
	}
}
