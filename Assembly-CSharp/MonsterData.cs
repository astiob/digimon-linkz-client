using Master;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterData
{
	private const float UP_RATE_FRENDSHIPSTATUS = 0.01f;

	private const string NOT_ABILITY = "0";

	public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;

	public GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterM;

	public GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterMG;

	public GameWebAPI.RespDataMA_GetSkillM.SkillM leaderSkillM;

	public GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM leaderSkillDetailM;

	public GameWebAPI.RespDataMA_GetSkillM.SkillM actionSkillM;

	public GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM actionSkillDetailM;

	public GameWebAPI.RespDataMA_GetSkillM.SkillM commonSkillM;

	public GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM commonSkillDetailM;

	public GameWebAPI.RespDataMA_GetSkillM.SkillM commonSkillM2;

	public GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM commonSkillDetailM2;

	public GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceM;

	public GameWebAPI.RespDataMA_GetMonsterGrowStepM.MonsterGrowStepM growStepM;

	public GameWebAPI.RespDataMA_GetMonsterTribeM.MonsterTribeM tribeM;

	public GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo userMonsterSlotInfo;

	private int[] chipIdList;

	public int idx;

	public bool isTrash;

	public bool isEvolve;

	public GUIMonsterIcon.DIMM_LEVEL dimmLevel;

	public int selectNum;

	public string dimmMess;

	public string sortMess;

	public string levelMess;

	public bool New;

	public bool Lock;

	public GUIMonsterIcon csMIcon;

	public bool IsLevelMax()
	{
		int num = int.Parse(this.userMonster.level);
		int num2 = int.Parse(this.monsterM.maxLevel);
		return num >= num2;
	}

	public void DuplicateUserMonster(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList um)
	{
		this.userMonster = this.GetDuplicateUserMonster(um);
	}

	public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList GetDuplicateUserMonster(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList um)
	{
		return new GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList(um);
	}

	public string GetEvolutionType(string nextMonsterId)
	{
		GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution[] monsterEvolutionM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionM.monsterEvolutionM;
		string monsterId = this.userMonster.monsterId;
		GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution evolution = Algorithm.BinarySearch<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>(monsterEvolutionM, monsterId, nextMonsterId, 0, monsterEvolutionM.Length - 1, "baseMonsterId", "nextMonsterId", 8);
		return (evolution != null) ? evolution.effectType : "1";
	}

	public bool IsGrowStepHigh()
	{
		int num = int.Parse(this.monsterMG.growStep);
		return num >= ConstValue.GROW_STEP_HIGH && num != 8;
	}

	public bool IsArousal()
	{
		int num = int.Parse(this.monsterM.rare);
		return num >= 2;
	}

	public bool IsAttachedChip()
	{
		return this.userMonsterSlotInfo.equip != null && this.userMonsterSlotInfo.equip.Length > 0;
	}

	public bool IsVersionUp()
	{
		int num = int.Parse(this.monsterM.rare);
		return num >= 6;
	}

	public bool CanVersionUp()
	{
		int num = int.Parse(this.monsterMG.growStep);
		int num2 = int.Parse(this.monsterM.rare);
		return (num == 7 || num == 9) && num2 >= 5;
	}

	public void InitSkillInfo()
	{
		GameWebAPI.RespDataMA_GetSkillM.SkillM[] skillM = MasterDataMng.Instance().RespDataMA_SkillM.skillM;
		GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM[] convertSkillDetailM = MasterDataMng.Instance().RespDataMA_SkillDetailM.convertSkillDetailM;
		int num = 4;
		for (int i = 0; i < skillM.Length; i++)
		{
			if (skillM[i].skillId == this.userMonster.leaderSkillId)
			{
				this.leaderSkillM = skillM[i];
				num--;
			}
			if (skillM[i].skillGroupId == this.monsterM.skillGroupId && skillM[i].skillGroupSubId == this.userMonster.defaultSkillGroupSubId)
			{
				this.actionSkillM = skillM[i];
				num--;
			}
			if (skillM[i].skillId == this.userMonster.commonSkillId)
			{
				this.commonSkillM = skillM[i];
				num--;
			}
			if (skillM[i].skillId == this.userMonster.extraCommonSkillId)
			{
				this.commonSkillM2 = skillM[i];
				num--;
			}
			if (num == 0)
			{
				break;
			}
		}
		List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM> list = new List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM>();
		List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM> list2 = new List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM>();
		List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM> list3 = new List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM>();
		List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM> list4 = new List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM>();
		for (int i = 0; i < convertSkillDetailM.Length; i++)
		{
			if (convertSkillDetailM[i].skillId == this.userMonster.leaderSkillId)
			{
				list.Add(convertSkillDetailM[i]);
			}
			if (convertSkillDetailM[i].skillId == this.userMonster.uniqueSkillId)
			{
				list2.Add(convertSkillDetailM[i]);
			}
			if (convertSkillDetailM[i].skillId == this.userMonster.commonSkillId)
			{
				list3.Add(convertSkillDetailM[i]);
			}
			if (convertSkillDetailM[i].skillId == this.userMonster.extraCommonSkillId)
			{
				list4.Add(convertSkillDetailM[i]);
			}
		}
		this.SetSkillDetailData(out this.leaderSkillDetailM, list);
		this.SetSkillDetailData(out this.actionSkillDetailM, list2);
		this.SetSkillDetailData(out this.commonSkillDetailM, list3);
		this.SetSkillDetailData(out this.commonSkillDetailM2, list4);
	}

	private void SetSkillDetailData(out GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM dust, List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM> source)
	{
		dust = null;
		for (int i = 0; i < source.Count; i++)
		{
			if (dust == null || source[i].effectType == 1)
			{
				dust = source[i];
			}
		}
	}

	public void InitResistanceInfo()
	{
		this.resistanceM = this.SerchResistanceById(this.monsterM.resistanceId);
	}

	public GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM SerchResistanceById(string resistanceId)
	{
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM[] monsterResistanceM = MasterDataMng.Instance().RespDataMA_MonsterResistanceM.monsterResistanceM;
		return Algorithm.BinarySearch<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM>(monsterResistanceM, resistanceId, 0, monsterResistanceM.Length - 1, "monsterResistanceId", 8);
	}

	public List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> GetUserUnitResistanceList()
	{
		return this.GetUserUnitResistanceList(this.userMonster);
	}

	public GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM AddResistanceFromMultipleTranceData()
	{
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM monsterResistanceM = this.SerchResistanceById(this.monsterM.resistanceId);
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM monsterResistanceM2 = new GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM();
		monsterResistanceM2.monsterResistanceId = monsterResistanceM.monsterResistanceId;
		monsterResistanceM2.description = monsterResistanceM.description;
		monsterResistanceM2.fire = monsterResistanceM.fire;
		monsterResistanceM2.water = monsterResistanceM.water;
		monsterResistanceM2.thunder = monsterResistanceM.thunder;
		monsterResistanceM2.nature = monsterResistanceM.nature;
		monsterResistanceM2.none = monsterResistanceM.none;
		monsterResistanceM2.light = monsterResistanceM.light;
		monsterResistanceM2.dark = monsterResistanceM.dark;
		monsterResistanceM2.poison = monsterResistanceM.poison;
		monsterResistanceM2.confusion = monsterResistanceM.confusion;
		monsterResistanceM2.paralysis = monsterResistanceM.paralysis;
		monsterResistanceM2.sleep = monsterResistanceM.sleep;
		monsterResistanceM2.stun = monsterResistanceM.stun;
		monsterResistanceM2.skillLock = monsterResistanceM.skillLock;
		monsterResistanceM2.death = monsterResistanceM.death;
		List<string> resistanceIdList = this.GetResistanceIdList(this.userMonster);
		for (int i = 0; i < resistanceIdList.Count; i++)
		{
			GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM data = this.SerchResistanceById(resistanceIdList[i]);
			this.AddResistanceFromTranceData(data, monsterResistanceM2);
		}
		return monsterResistanceM2;
	}

	public GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM GetUserMonsterTranceData()
	{
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM monsterResistanceM = new GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM();
		string text = "0";
		monsterResistanceM.fire = text;
		monsterResistanceM.water = text;
		monsterResistanceM.thunder = text;
		monsterResistanceM.nature = text;
		monsterResistanceM.none = text;
		monsterResistanceM.light = text;
		monsterResistanceM.dark = text;
		monsterResistanceM.poison = text;
		monsterResistanceM.confusion = text;
		monsterResistanceM.paralysis = text;
		monsterResistanceM.sleep = text;
		monsterResistanceM.stun = text;
		monsterResistanceM.skillLock = text;
		monsterResistanceM.death = text;
		List<string> resistanceIdList = this.GetResistanceIdList(this.userMonster);
		for (int i = 0; i < resistanceIdList.Count; i++)
		{
			GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM data = this.SerchResistanceById(resistanceIdList[i]);
			this.AddResistanceFromTranceData(data, monsterResistanceM);
		}
		return monsterResistanceM;
	}

	public GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM AddResistanceFromTranceData(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM data, GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM baseData)
	{
		string b = "0";
		if (data.none != b)
		{
			baseData.none = data.none;
		}
		if (data.fire != b)
		{
			baseData.fire = data.fire;
		}
		if (data.water != b)
		{
			baseData.water = data.water;
		}
		if (data.nature != b)
		{
			baseData.nature = data.nature;
		}
		if (data.thunder != b)
		{
			baseData.thunder = data.thunder;
		}
		if (data.dark != b)
		{
			baseData.dark = data.dark;
		}
		if (data.light != b)
		{
			baseData.light = data.light;
		}
		if (data.stun != b)
		{
			baseData.stun = data.stun;
		}
		if (data.skillLock != b)
		{
			baseData.skillLock = data.skillLock;
		}
		if (data.sleep != b)
		{
			baseData.sleep = data.sleep;
		}
		if (data.paralysis != b)
		{
			baseData.paralysis = data.paralysis;
		}
		if (data.confusion != b)
		{
			baseData.confusion = data.confusion;
		}
		if (data.poison != b)
		{
			baseData.poison = data.poison;
		}
		if (data.death != b)
		{
			baseData.death = data.death;
		}
		return baseData;
	}

	public void InitGrowStepInfo()
	{
		GameWebAPI.RespDataMA_GetMonsterGrowStepM.MonsterGrowStepM[] monsterGrowStepM = MasterDataMng.Instance().RespDataMA_MonsterGrowStepM.monsterGrowStepM;
		this.growStepM = monsterGrowStepM.SingleOrDefault((GameWebAPI.RespDataMA_GetMonsterGrowStepM.MonsterGrowStepM x) => x.monsterGrowStepId == this.monsterMG.growStep);
	}

	public void InitTribeInfo()
	{
		GameWebAPI.RespDataMA_GetMonsterTribeM.MonsterTribeM[] monsterTribeM = MasterDataMng.Instance().RespDataMA_MonsterTribeM.monsterTribeM;
		this.tribeM = monsterTribeM.SingleOrDefault((GameWebAPI.RespDataMA_GetMonsterTribeM.MonsterTribeM x) => x.monsterTribeId == this.monsterMG.tribe);
	}

	public void InitChipInfo()
	{
		GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo[] slotInfo = MonsterDataMng.Instance().userMonsterSlotInfoListLogic.slotInfo;
		if (slotInfo != null)
		{
			IEnumerable<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo> source = slotInfo.Where((GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo s) => s.userMonsterId == int.Parse(this.userMonster.userMonsterId));
			if (source.Count<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo>() > 0)
			{
				this.userMonsterSlotInfo = source.First<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo>();
				if (this.userMonsterSlotInfo.equip != null)
				{
					this.chipIdList = new int[this.userMonsterSlotInfo.equip.Length];
					for (int i = 0; i < this.userMonsterSlotInfo.equip.Length; i++)
					{
						if (this.userMonsterSlotInfo.equip[i] != null)
						{
							GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipDataByUserChipId = ChipDataMng.GetUserChipDataByUserChipId(this.userMonsterSlotInfo.equip[i].userChipId);
							if (userChipDataByUserChipId != null)
							{
								this.chipIdList[i] = userChipDataByUserChipId.chipId;
							}
						}
					}
				}
			}
		}
	}

	public bool UpdateSlotInfo(GameWebAPI.ReqDataCS_ChipEquipLogic equip)
	{
		if (this.userMonsterSlotInfo == null)
		{
			return false;
		}
		List<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip> list;
		if (this.userMonsterSlotInfo.equip != null)
		{
			list = new List<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip>(this.userMonsterSlotInfo.equip);
		}
		else
		{
			list = new List<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip>();
		}
		GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip change = new GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip();
		change.dispNum = equip.dispNum;
		change.type = equip.type;
		change.userChipId = equip.userChipId;
		if (equip.act == 1)
		{
			list.Add(change);
		}
		else if (list.Count<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip>() > 0)
		{
			list.RemoveAll((GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip c) => c.userChipId == change.userChipId);
		}
		this.userMonsterSlotInfo.equip = list.ToArray();
		return true;
	}

	public void RefleshNormalChipManagSlot()
	{
		switch (int.Parse(this.growStepM.monsterGrowStepId))
		{
		case 5:
		case 6:
		case 7:
			this.userMonsterSlotInfo.manage.slotNum++;
			break;
		}
	}

	public void UpdateNowParam(int level = -1)
	{
		int level2 = this.GetLevel(level);
		this.userMonster.level = level2.ToString();
		this.userMonster.hp = this.Now_HP(level2).ToString();
		this.userMonster.attack = this.Now_ATK(level2).ToString();
		this.userMonster.defense = this.Now_DEF(level2).ToString();
		this.userMonster.spAttack = this.Now_SATK(level2).ToString();
		this.userMonster.spDefense = this.Now_SDEF(level2).ToString();
		this.userMonster.speed = this.Now_SPD(level2).ToString();
		this.userMonster.luck = "1";
	}

	public int Now_Price(int level = -1)
	{
		int level2 = this.GetLevel(level);
		int num = int.Parse(this.monsterM.price);
		int num2 = int.Parse(this.monsterM.priceRise);
		return num + num2 * (level2 - 1);
	}

	public float Base_HP(int level = -1)
	{
		int level2 = this.GetLevel(level);
		return this.CalcParamLinear((float)int.Parse(this.monsterM.defaultHp), (float)int.Parse(this.monsterM.maxHp), (float)level2, (float)int.Parse(this.monsterM.maxLevel), 1f);
	}

	public int Now_HP(int level = -1)
	{
		float num = 0f;
		if (!string.IsNullOrEmpty(this.userMonster.hp))
		{
			num = float.Parse(this.userMonster.hp);
			num -= this.CalcParamLinear((float)int.Parse(this.monsterM.defaultHp), (float)int.Parse(this.monsterM.maxHp), float.Parse(this.userMonster.level), (float)int.Parse(this.monsterM.maxLevel), 1f);
		}
		return (int)(this.Base_HP(level) + num);
	}

	public float Base_ATK(int level = -1)
	{
		int level2 = this.GetLevel(level);
		return this.CalcParamLinear((float)int.Parse(this.monsterM.defaultAttack), (float)int.Parse(this.monsterM.maxAttack), (float)level2, (float)int.Parse(this.monsterM.maxLevel), 1f);
	}

	public int Now_ATK(int level = -1)
	{
		float num = 0f;
		if (!string.IsNullOrEmpty(this.userMonster.attack))
		{
			num = float.Parse(this.userMonster.attack);
			num -= this.CalcParamLinear((float)int.Parse(this.monsterM.defaultAttack), (float)int.Parse(this.monsterM.maxAttack), float.Parse(this.userMonster.level), (float)int.Parse(this.monsterM.maxLevel), 1f);
		}
		return (int)(this.Base_ATK(level) + num);
	}

	public float Base_DEF(int level = -1)
	{
		int level2 = this.GetLevel(level);
		return this.CalcParamLinear((float)int.Parse(this.monsterM.defaultDefense), (float)int.Parse(this.monsterM.maxDefense), (float)level2, (float)int.Parse(this.monsterM.maxLevel), 1f);
	}

	public int Now_DEF(int level = -1)
	{
		float num = 0f;
		if (!string.IsNullOrEmpty(this.userMonster.defense))
		{
			num = float.Parse(this.userMonster.defense);
			num -= this.CalcParamLinear((float)int.Parse(this.monsterM.defaultDefense), (float)int.Parse(this.monsterM.maxDefense), float.Parse(this.userMonster.level), (float)int.Parse(this.monsterM.maxLevel), 1f);
		}
		return (int)(this.Base_DEF(level) + num);
	}

	public float Base_SATK(int level = -1)
	{
		int level2 = this.GetLevel(level);
		return this.CalcParamLinear((float)int.Parse(this.monsterM.defaultSpAttack), (float)int.Parse(this.monsterM.maxSpAttack), (float)level2, (float)int.Parse(this.monsterM.maxLevel), 1f);
	}

	public int Now_SATK(int level = -1)
	{
		float num = 0f;
		if (!string.IsNullOrEmpty(this.userMonster.spAttack))
		{
			num = float.Parse(this.userMonster.spAttack);
			num -= this.CalcParamLinear((float)int.Parse(this.monsterM.defaultSpAttack), (float)int.Parse(this.monsterM.maxSpAttack), float.Parse(this.userMonster.level), (float)int.Parse(this.monsterM.maxLevel), 1f);
		}
		return (int)(this.Base_SATK(level) + num);
	}

	public float Base_SDEF(int level = -1)
	{
		int level2 = this.GetLevel(level);
		return this.CalcParamLinear((float)int.Parse(this.monsterM.defaultSpDefense), (float)int.Parse(this.monsterM.maxSpDefense), (float)level2, (float)int.Parse(this.monsterM.maxLevel), 1f);
	}

	public int Now_SDEF(int level = -1)
	{
		float num = 0f;
		if (!string.IsNullOrEmpty(this.userMonster.spDefense))
		{
			num = float.Parse(this.userMonster.spDefense);
			num -= this.CalcParamLinear((float)int.Parse(this.monsterM.defaultSpDefense), (float)int.Parse(this.monsterM.maxSpDefense), float.Parse(this.userMonster.level), (float)int.Parse(this.monsterM.maxLevel), 1f);
		}
		return (int)(this.Base_SDEF(level) + num);
	}

	public float Base_SPD(int level = -1)
	{
		int level2 = this.GetLevel(level);
		return this.CalcParamLinear((float)int.Parse(this.monsterM.speed), (float)int.Parse(this.monsterM.speed), (float)level2, (float)int.Parse(this.monsterM.maxLevel), 1f);
	}

	public int Now_SPD(int level = -1)
	{
		float num = 0f;
		if (!string.IsNullOrEmpty(this.userMonster.speed))
		{
			num = float.Parse(this.userMonster.speed);
			num -= this.CalcParamLinear((float)int.Parse(this.monsterM.speed), (float)int.Parse(this.monsterM.speed), float.Parse(this.userMonster.level), (float)int.Parse(this.monsterM.maxLevel), 1f);
		}
		return (int)(this.Base_SPD(level) + num);
	}

	private float CalcParamLinear(float min, float max, float now_lv, float max_lv, float min_lv = 1f)
	{
		float num = min + (max - min) / (max_lv - min_lv) * (now_lv - min_lv);
		return (float)Math.Round((double)num, MidpointRounding.AwayFromZero);
	}

	private int GetLevel(int level)
	{
		if (level == -1)
		{
			return int.Parse(this.userMonster.level);
		}
		return level;
	}

	private int CalcRealParam(float param, string ability, string flag)
	{
		if (flag == "0")
		{
			return (int)param;
		}
		float f = param * (float)(100 + ability.ToInt32()) / 100f;
		return (int)Mathf.Floor(f);
	}

	public bool CheckHaveMedal()
	{
		List<string> list = new List<string>();
		list.Add(this.userMonster.hpAbilityFlg);
		list.Add(this.userMonster.attackAbilityFlg);
		list.Add(this.userMonster.defenseAbilityFlg);
		list.Add(this.userMonster.spAttackAbilityFlg);
		list.Add(this.userMonster.spDefenseAbilityFlg);
		list.Add(this.userMonster.speedAbilityFlg);
		int num = 0;
		foreach (string s in list)
		{
			if (int.TryParse(s, out num))
			{
				int num2 = num;
				if (num2 == 1)
				{
					return true;
				}
				if (num2 == 2)
				{
					return true;
				}
			}
		}
		return false;
	}

	public List<int> GetHaveMedal()
	{
		List<string> list = new List<string>();
		List<int> list2 = new List<int>();
		list.Add(this.userMonster.hpAbilityFlg);
		list.Add(this.userMonster.attackAbilityFlg);
		list.Add(this.userMonster.defenseAbilityFlg);
		list.Add(this.userMonster.spAttackAbilityFlg);
		list.Add(this.userMonster.spDefenseAbilityFlg);
		list.Add(this.userMonster.speedAbilityFlg);
		int num = 0;
		foreach (string s in list)
		{
			if (int.TryParse(s, out num))
			{
				int num2 = num;
				if (num2 != 1)
				{
					if (num2 == 2)
					{
						list2.Add(num);
					}
				}
				else
				{
					list2.Add(num);
				}
			}
		}
		list2.Sort(new Comparison<int>(this.CompareMedalGS));
		return list2;
	}

	private int CompareMedalGS(int x, int y)
	{
		if (x < y)
		{
			return -1;
		}
		if (x > y)
		{
			return 1;
		}
		return 0;
	}

	public void SetChipIdList(int[] chipIdList)
	{
		if (chipIdList != null)
		{
			int num = 0;
			for (int i = 0; i < chipIdList.Length; i++)
			{
				if (0 < chipIdList[i])
				{
					num++;
				}
			}
			this.chipIdList = new int[num];
			num = 0;
			for (int i = 0; i < chipIdList.Length; i++)
			{
				if (0 < chipIdList[i])
				{
					this.chipIdList[num] = chipIdList[i];
					num++;
				}
			}
		}
	}

	public int[] GetChipIdList()
	{
		int[] result;
		if (this.chipIdList != null)
		{
			result = this.chipIdList;
		}
		else
		{
			result = new int[0];
		}
		return result;
	}

	public List<string> GetChipGroupList()
	{
		List<string> list = null;
		if (this.chipIdList != null)
		{
			list = new List<string>();
			foreach (int num in this.chipIdList)
			{
				GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(num.ToString());
				list.Add(chipMainData.chipGroupId);
			}
		}
		return list;
	}

	public int GetFriendshipBonusHP(int bonusStep)
	{
		return Mathf.FloorToInt((float)(this.monsterM.maxHp.ToInt32() * bonusStep) * 0.01f);
	}

	public int GetFriendshipBonusAttack(int bonusStep)
	{
		return Mathf.FloorToInt((float)(this.monsterM.maxAttack.ToInt32() * bonusStep) * 0.01f);
	}

	public int GetFriendshipBonusDefense(int bonusStep)
	{
		return Mathf.FloorToInt((float)(this.monsterM.maxDefense.ToInt32() * bonusStep) * 0.01f);
	}

	public int GetFriendshipBonusSpAttack(int bonusStep)
	{
		return Mathf.FloorToInt((float)(this.monsterM.maxSpAttack.ToInt32() * bonusStep) * 0.01f);
	}

	public int GetFriendshipBonusSpDefense(int bonusStep)
	{
		return Mathf.FloorToInt((float)(this.monsterM.maxSpDefense.ToInt32() * bonusStep) * 0.01f);
	}

	public int GetFriendshipBonusSpeed(int bonusStep)
	{
		return Mathf.FloorToInt((float)(this.monsterM.speed.ToInt32() * bonusStep) * 0.01f);
	}

	public static GrowStep ToGrowStepId(string growStep)
	{
		int result = 0;
		if (!int.TryParse(growStep, out result))
		{
			global::Debug.Log("成長帯IDの変換に失敗");
		}
		return (GrowStep)result;
	}

	public static bool IsEgg(string growStep)
	{
		GrowStep growStep2 = MonsterData.ToGrowStepId(growStep);
		return GrowStep.EGG == growStep2;
	}

	private List<string> GetResistanceIdList(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster)
	{
		List<string> list = new List<string>();
		if (!string.IsNullOrEmpty(userMonster.tranceResistance))
		{
			list.AddRange(userMonster.tranceResistance.Split(new char[]
			{
				','
			}));
		}
		if (!string.IsNullOrEmpty(userMonster.tranceStatusAilment))
		{
			list.AddRange(userMonster.tranceStatusAilment.Split(new char[]
			{
				','
			}));
		}
		return list;
	}

	private List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> GetUserUnitResistanceList(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster)
	{
		List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> list = new List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM>();
		List<string> resistanceIdList = this.GetResistanceIdList(userMonster);
		for (int i = 0; i < resistanceIdList.Count; i++)
		{
			GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM monsterResistanceM = this.SerchResistanceById(resistanceIdList[i]);
			if (monsterResistanceM != null)
			{
				list.Add(monsterResistanceM);
			}
		}
		return list;
	}
}
