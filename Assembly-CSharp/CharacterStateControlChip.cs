using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStateControlChip
{
	private CharacterStateControl characterStateControl;

	private Dictionary<EffectStatusBase.EffectTriggerType, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>> triggerChips = new Dictionary<EffectStatusBase.EffectTriggerType, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>>();

	private Dictionary<int, int> chipEffectCount = new Dictionary<int, int>();

	private BattleInvariant hittingTheTargetChip;

	private BattleInvariant criticalTheTargetChip;

	private CharacterStateControlChip()
	{
		this.hittingTheTargetChip = new BattleInvariant();
		this.criticalTheTargetChip = new BattleInvariant();
		this.stagingChipIdList = new Dictionary<int, int>();
		this.potencyChipIdList = new Dictionary<int, int>();
	}

	public CharacterStateControlChip(CharacterStateControl characterStateControl)
	{
		this.characterStateControl = characterStateControl;
		this.hittingTheTargetChip = new BattleInvariant();
		this.criticalTheTargetChip = new BattleInvariant();
		this.stagingChipIdList = new Dictionary<int, int>();
		this.potencyChipIdList = new Dictionary<int, int>();
		this.InitializeChipEffectCount();
	}

	public BattleInvariant.Type hittingTheTargetType
	{
		get
		{
			return this.hittingTheTargetChip.type;
		}
	}

	public BattleInvariant.Type criticalTheTargetType
	{
		get
		{
			return this.criticalTheTargetChip.type;
		}
	}

	public Dictionary<int, int> potencyChipIdList { get; private set; }

	public Dictionary<int, int> stagingChipIdList { get; private set; }

	public int chipAddMaxHp { get; private set; }

	public int chipAddAttackPower { get; private set; }

	public int chipAddDefencePower { get; private set; }

	public int chipAddSpecialAttackPower { get; private set; }

	public int chipAddSpecialDefencePower { get; private set; }

	public int chipAddSpeed { get; private set; }

	public float chipAddCritical { get; private set; }

	public float chipAddHit { get; private set; }

	public int stageChipAddMaxHp { get; private set; }

	public int stageChipAddAttackPower { get; private set; }

	public int stageChipAddDefencePower { get; private set; }

	public int stageChipAddSpecialAttackPower { get; private set; }

	public int stageChipAddSpecialDefencePower { get; private set; }

	public int stageChipAddSpeed { get; private set; }

	public bool isServerAddValue { get; private set; }

	public CharacterStateControlChip.GutsData gutsData { get; private set; }

	public static CharacterStateControlChip GetNullObject()
	{
		return new CharacterStateControlChip();
	}

	public void SetCharacterState(CharacterStateControlStore savedCSC)
	{
		this.SetChipEffectCount(savedCSC.chipEffectCount);
		this.SetPotencyChipIdList(savedCSC.potencyChipIdList);
		foreach (int num in this.potencyChipIdList.Keys)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(num.ToString());
			if (chipEffectDataToId != null)
			{
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects = new GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]
				{
					chipEffectDataToId
				};
				EffectStatusBase.EffectTriggerType effectTriggerType = (EffectStatusBase.EffectTriggerType)chipEffectDataToId.effectTrigger.ToInt32();
				if (effectTriggerType != EffectStatusBase.EffectTriggerType.Usually && effectTriggerType != EffectStatusBase.EffectTriggerType.Area)
				{
					this.AddChipParam(true, chipEffects, true, false);
				}
			}
		}
	}

	public string GetChipEffectCountToString()
	{
		string text = string.Empty;
		int num = 0;
		foreach (int key in this.chipEffectCount.Keys)
		{
			if (num > 0)
			{
				text += ",";
			}
			text = text + key.ToString() + ":" + this.chipEffectCount[key].ToString();
			num++;
		}
		return text;
	}

	private void SetChipEffectCount(string chiopCount)
	{
		string[] array = chiopCount.Split(",".ToCharArray());
		foreach (string text in array)
		{
			string[] array3 = text.Split(":".ToCharArray());
			if (array3.Length > 1)
			{
				int key = int.Parse(array3[0]);
				int value = int.Parse(array3[1]);
				if (this.chipEffectCount.ContainsKey(key))
				{
					this.chipEffectCount[key] = value;
				}
				else
				{
					this.chipEffectCount.Add(key, value);
				}
			}
		}
	}

	private void SetPotencyChipIdList(string chipIdList)
	{
		string[] array = chipIdList.Split(",".ToCharArray());
		foreach (string text in array)
		{
			string[] array3 = text.Split(":".ToCharArray());
			if (array3.Length > 1)
			{
				int key = int.Parse(array3[0]);
				int value = int.Parse(array3[1]);
				if (this.potencyChipIdList.ContainsKey(key))
				{
					this.potencyChipIdList[key] = value;
				}
				else
				{
					this.potencyChipIdList.Add(key, value);
				}
			}
		}
	}

	public string GetPotencyChipIdListToString()
	{
		string text = string.Empty;
		int num = 0;
		foreach (int key in this.potencyChipIdList.Keys)
		{
			if (num > 0)
			{
				text += ",";
			}
			text = text + key.ToString() + ":" + this.potencyChipIdList[key].ToString();
			num++;
		}
		return text;
	}

	private void InitializeChipEffectCount()
	{
		foreach (int num in this.characterStateControl.characterStatus.chipIds)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(num.ToString());
			if (chipEffectData != null)
			{
				foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffectData)
				{
					if (chipEffect.effectTurn.ToInt32() > 0)
					{
						this.chipEffectCount[chipEffect.chipEffectId.ToInt32()] = chipEffect.effectTurn.ToInt32();
					}
				}
			}
		}
	}

	public void OnChipTrigger(EffectStatusBase.EffectTriggerType triggerType)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> chipTriggerList = this.GetChipTriggerList(triggerType);
		if (chipTriggerList.Count == 0)
		{
			return;
		}
		GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] invocationList = ChipEffectStatus.GetInvocationList(chipTriggerList.ToArray(), triggerType, this.characterStateControl.groupId.ToInt32(), this.characterStateControl, BattleStateManager.current.hierarchyData.areaId);
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect;
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect2 in chipTriggerList)
		{
			chipEffect = chipEffect2;
			if (!invocationList.Where((GameWebAPI.RespDataMA_ChipEffectM.ChipEffect item) => item.chipEffectId == chipEffect.chipEffectId).Any<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>())
			{
				int key = chipEffect.chipEffectId.ToInt32();
				if (this.potencyChipIdList.ContainsKey(key))
				{
					this.potencyChipIdList.Remove(key);
					list.Add(chipEffect);
				}
			}
		}
		bool isAll = triggerType != EffectStatusBase.EffectTriggerType.Usually;
		bool isArea = triggerType == EffectStatusBase.EffectTriggerType.Area;
		this.AddChipParam(true, this.GetAddChipParamEffects(invocationList).ToArray(), isAll, isArea);
		if (list.Count > 0)
		{
			this.AddChipParam(false, list.ToArray(), true, false);
		}
	}

	public void RemovePotencyChip(EffectStatusBase.EffectTriggerType triggerType)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		foreach (int num in this.potencyChipIdList.Keys)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(num.ToString());
			if (chipEffectDataToId != null && chipEffectDataToId.effectTrigger.ToInt32() == (int)triggerType)
			{
				list.Add(chipEffectDataToId);
			}
		}
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in list)
		{
			this.potencyChipIdList.Remove(chipEffect.chipEffectId.ToInt32());
		}
		this.AddChipParam(false, list.ToArray(), true, false);
	}

	private List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> GetChipTriggerList(EffectStatusBase.EffectTriggerType triggerType)
	{
		if (!this.triggerChips.ContainsKey(triggerType))
		{
			List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
			foreach (int num in this.characterStateControl.characterStatus.chipIds)
			{
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(num.ToString());
				if (chipEffectData != null)
				{
					foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffectData)
					{
						if (chipEffect.effectTrigger.ToInt32() == (int)triggerType)
						{
							list.Add(chipEffect);
						}
					}
				}
			}
			this.triggerChips.Add(triggerType, list);
		}
		return this.triggerChips[triggerType];
	}

	private List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> GetAddChipParamEffects(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipInvocationEffects)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipInvocationEffects)
		{
			int num = chipEffect.chipEffectId.ToInt32();
			bool flag = true;
			if (!this.potencyChipIdList.ContainsKey(num))
			{
				if (this.chipEffectCount.ContainsKey(num))
				{
					if (this.chipEffectCount[num] <= 0)
					{
						goto IL_3AB;
					}
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary = dictionary2 = this.chipEffectCount;
					int num2;
					int key = num2 = num;
					num2 = dictionary2[num2];
					dictionary[key] = num2 - 1;
				}
				if (chipEffect.effectType.ToInt32() == 57)
				{
					int value;
					if (chipEffect.effectSubType.ToInt32() == 1)
					{
						value = chipEffect.effectValue.ToInt32();
					}
					else if (chipEffect.effectSubType.ToInt32() == 2)
					{
						value = chipEffect.effectValue.ToInt32();
					}
					else if (chipEffect.effectSubType.ToInt32() == 3)
					{
						value = (int)((float)this.characterStateControl.extraMaxHp * chipEffect.effectValue.ToFloat());
					}
					else
					{
						value = chipEffect.effectValue.ToInt32();
					}
					int hp = Mathf.Clamp(value, 1, this.characterStateControl.extraMaxHp);
					this.gutsData = new CharacterStateControlChip.GutsData(chipEffect.chipId, chipEffect.chipEffectId, hp);
					flag = false;
				}
				if (chipEffect.effectType.ToInt32() == 61)
				{
					bool flag2 = false;
					if (chipEffect.effectTrigger.ToInt32() == 31 || chipEffect.effectTrigger.ToInt32() == 32 || chipEffect.effectTrigger.ToInt32() == 33 || chipEffect.effectTrigger.ToInt32() == 34 || chipEffect.effectTrigger.ToInt32() == 35 || chipEffect.effectTrigger.ToInt32() == 36)
					{
						flag2 = true;
					}
					BattleServerControl serverControl = BattleStateManager.current.serverControl;
					List<AffectEffectProperty> affectEffectPropertyList = serverControl.GetAffectEffectPropertyList(chipEffect.effectValue);
					if (affectEffectPropertyList != null)
					{
						foreach (AffectEffectProperty affectEffectProperty in affectEffectPropertyList)
						{
							if (flag2)
							{
								EffectStatusBase.EffectTriggerType key2 = (EffectStatusBase.EffectTriggerType)chipEffect.effectTrigger.ToInt32();
								if (!this.characterStateControl.everySkillList.ContainsKey(key2))
								{
									this.characterStateControl.everySkillList.Add(key2, new List<AffectEffectProperty>());
								}
								this.characterStateControl.everySkillList[key2].Add(affectEffectProperty);
							}
							else
							{
								this.characterStateControl.currentSkillStatus.AddAffectEffect(affectEffectProperty);
							}
						}
					}
					flag = false;
				}
				if (chipEffect.effectType.ToInt32() == 70 || chipEffect.effectType.ToInt32() == 72 || chipEffect.effectType.ToInt32() == 71 || chipEffect.effectType.ToInt32() == 73 || chipEffect.effectType.ToInt32() == 74)
				{
					this.isServerAddValue = true;
				}
				list.Add(chipEffect);
				if (chipEffect.effectType.ToInt32() != 57 && chipEffect.effectType.ToInt32() != 60 && chipEffect.effectType.ToInt32() != 61 && chipEffect.effectType.ToInt32() != 56 && !this.potencyChipIdList.ContainsKey(num))
				{
					this.potencyChipIdList.Add(num, chipEffect.targetType.ToInt32());
				}
				if (flag && !this.stagingChipIdList.ContainsKey(num))
				{
					this.stagingChipIdList.Add(num, chipEffect.chipId.ToInt32());
				}
			}
			IL_3AB:;
		}
		return list;
	}

	public void RemoveDeadStagingChips()
	{
		if (BattleStateManager.current == null)
		{
			return;
		}
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (KeyValuePair<int, int> keyValuePair in this.stagingChipIdList)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(keyValuePair.Key.ToString());
			if (chipEffectDataToId.effectTrigger.ToInt32() == 6)
			{
				List<AffectEffectProperty> affectEffectPropertyList = BattleStateManager.current.serverControl.GetAffectEffectPropertyList(chipEffectDataToId.effectValue);
				if (affectEffectPropertyList != null)
				{
					foreach (AffectEffectProperty affectEffectProperty in affectEffectPropertyList)
					{
						CharacterStateControl[] skillTargetList = BattleStateManager.current.targetSelect.GetSkillTargetList(this.characterStateControl, affectEffectProperty.target);
						if (skillTargetList.Length != 0)
						{
							if (dictionary.ContainsKey(keyValuePair.Key))
							{
								dictionary[keyValuePair.Key] = keyValuePair.Value;
							}
							else
							{
								dictionary.Add(keyValuePair.Key, keyValuePair.Value);
							}
						}
					}
				}
			}
		}
		this.stagingChipIdList = dictionary;
	}

	private void AddChipParam(bool isAdd, GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, bool isAll = true, bool isArea = false)
	{
		CharacterStatus characterStatus = this.characterStateControl.characterStatus;
		int defaultMaxHpWithLeaderSkill = this.characterStateControl.defaultMaxHpWithLeaderSkill;
		int attackPower = characterStatus.attackPower;
		int defencePower = characterStatus.defencePower;
		int specialAttackPower = characterStatus.specialAttackPower;
		int specialDefencePower = characterStatus.specialDefencePower;
		int speed = characterStatus.speed;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = (!isAdd) ? -1 : 1;
		if (isAll)
		{
			num = ChipEffectStatus.GetChipEffectValue(chipEffects, defaultMaxHpWithLeaderSkill, this.characterStateControl, EffectStatusBase.ExtraEffectType.Hp);
			num2 = ChipEffectStatus.GetChipEffectValue(chipEffects, attackPower, this.characterStateControl, EffectStatusBase.ExtraEffectType.Atk);
			num3 = ChipEffectStatus.GetChipEffectValue(chipEffects, defencePower, this.characterStateControl, EffectStatusBase.ExtraEffectType.Def);
			num4 = ChipEffectStatus.GetChipEffectValue(chipEffects, specialAttackPower, this.characterStateControl, EffectStatusBase.ExtraEffectType.Satk);
			num5 = ChipEffectStatus.GetChipEffectValue(chipEffects, specialDefencePower, this.characterStateControl, EffectStatusBase.ExtraEffectType.Sdef);
			num6 = ChipEffectStatus.GetChipEffectValue(chipEffects, speed, this.characterStateControl, EffectStatusBase.ExtraEffectType.Speed);
		}
		float chipEffectValueToFloat = ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, 0f, this.characterStateControl, EffectStatusBase.ExtraEffectType.Critical);
		float chipEffectValueToFloat2 = ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, 0f, this.characterStateControl, EffectStatusBase.ExtraEffectType.Hit);
		if (isArea)
		{
			this.stageChipAddMaxHp += num * num7;
			this.stageChipAddAttackPower += num2 * num7;
			this.stageChipAddDefencePower += num3 * num7;
			this.stageChipAddSpecialAttackPower += num4 * num7;
			this.stageChipAddSpecialDefencePower += num5 * num7;
			this.stageChipAddSpeed += num6 * num7;
		}
		else
		{
			this.chipAddMaxHp += num * num7;
			this.chipAddAttackPower += num2 * num7;
			this.chipAddDefencePower += num3 * num7;
			this.chipAddSpecialAttackPower += num4 * num7;
			this.chipAddSpecialDefencePower += num5 * num7;
			this.chipAddSpeed += num6 * num7;
		}
		this.chipAddCritical += chipEffectValueToFloat * (float)num7 / 100f;
		this.chipAddHit += chipEffectValueToFloat2 * (float)num7 / 100f;
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffects)
		{
			BattleInvariant.Type type;
			if (chipEffect.effectValue != "0")
			{
				type = BattleInvariant.Type.Down;
			}
			else
			{
				type = BattleInvariant.Type.Up;
			}
			if (chipEffect.effectType.ToInt32() == 58)
			{
				if (isAdd)
				{
					this.hittingTheTargetChip.Add(type);
				}
				else
				{
					this.hittingTheTargetChip.Remove(type);
				}
			}
			if (chipEffect.effectType.ToInt32() == 59)
			{
				if (isAdd)
				{
					this.criticalTheTargetChip.Add(type);
				}
				else
				{
					this.criticalTheTargetChip.Remove(type);
				}
			}
		}
	}

	public void AddChipEffectCount(int chipEffectId, int value)
	{
		if (this.chipEffectCount.ContainsKey(chipEffectId))
		{
			Dictionary<int, int> dictionary2;
			Dictionary<int, int> dictionary = dictionary2 = this.chipEffectCount;
			int num = dictionary2[chipEffectId];
			dictionary[chipEffectId] = num + value;
		}
	}

	public void InitChipEffectCountForWave()
	{
		this.InitChipEffectCount("1");
	}

	public void InitChipEffectCountForTurn()
	{
		this.InitChipEffectCount("2");
	}

	private void InitChipEffectCount(string value)
	{
		foreach (int num in this.characterStateControl.chipIds)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(num.ToString());
			foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffectData)
			{
				if (chipEffect.effectTurnType == value && this.chipEffectCount.ContainsKey(chipEffect.chipEffectId.ToInt32()))
				{
					this.chipEffectCount[chipEffect.chipEffectId.ToInt32()] = chipEffect.effectTurn.ToInt32();
				}
			}
		}
	}

	public void ClearStagingChipIdList()
	{
		this.stagingChipIdList.Clear();
	}

	public void ClearGutsData()
	{
		if (this.gutsData != null)
		{
			if (!this.gutsData.isUse)
			{
				this.AddChipEffectCount(this.gutsData.chipEffectId.ToInt32(), 1);
			}
			else
			{
				int value = this.gutsData.chipId.ToInt32();
				int key = this.gutsData.chipEffectId.ToInt32();
				if (!this.stagingChipIdList.ContainsKey(key))
				{
					this.stagingChipIdList.Add(key, value);
				}
			}
		}
		this.gutsData = null;
	}

	public class GutsData
	{
		public bool isUse;

		public GutsData(string chipId, string chipEffectId, int hp)
		{
			this.chipId = chipId;
			this.chipEffectId = chipEffectId;
			this.hp = hp;
		}

		public string chipId { get; private set; }

		public string chipEffectId { get; private set; }

		public int hp { get; private set; }
	}
}
