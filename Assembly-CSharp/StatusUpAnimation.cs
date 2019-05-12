using Master;
using Monster;
using System;
using System.Collections;
using UnityEngine;

public class StatusUpAnimation : MonoBehaviour
{
	private readonly float waitTime = 1f;

	[SerializeField]
	private UILabel levelLabel;

	[SerializeField]
	private UILabel hpLabel;

	[SerializeField]
	private UILabel atkLabel;

	[SerializeField]
	private UILabel defLabel;

	[SerializeField]
	private UILabel sAtkLabel;

	[SerializeField]
	private UILabel sDefLabel;

	[SerializeField]
	private UILabel spdLabel;

	[SerializeField]
	private UILabel luckLabel;

	[SerializeField]
	private UILabel levelUpLabel;

	[SerializeField]
	private UILabel hpUpLabel;

	[SerializeField]
	private UILabel atkUpLabel;

	[SerializeField]
	private UILabel defUpLabel;

	[SerializeField]
	private UILabel sAtkUpLabel;

	[SerializeField]
	private UILabel sDefUpLabel;

	[SerializeField]
	private UILabel spdUpLabel;

	[SerializeField]
	private UILabel luckUpLabel;

	[SerializeField]
	private UILabel hpAddLabel;

	[SerializeField]
	private UILabel atkAddLabel;

	[SerializeField]
	private UILabel defAddLabel;

	[SerializeField]
	private UILabel sAtkAddLabel;

	[SerializeField]
	private UILabel sDefAddLabel;

	[SerializeField]
	private UILabel spdAddLabel;

	private int displayLevel;

	private int luckUpValue;

	private Coroutine animationCoroutine;

	public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;

	public int defaultLevel { get; set; }

	public int Displaylevel
	{
		get
		{
			return this.displayLevel;
		}
		set
		{
			this.displayLevel = value;
			if (this.displayLevel > this.defaultLevel || this.luckUpValue > 0)
			{
				this.DisplayDifference();
			}
			else
			{
				this.HideDifference();
			}
		}
	}

	public int LuckUpValue
	{
		set
		{
			this.luckUpValue = value;
			if (this.displayLevel > this.defaultLevel || this.luckUpValue > 0)
			{
				this.DisplayDifference();
			}
			else
			{
				this.HideDifference();
			}
		}
	}

	private void OnDestroy()
	{
		AppCoroutine.Stop(this.animationCoroutine, false);
		this.animationCoroutine = null;
	}

	public void Initialize(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster, int defaultLevel)
	{
		this.userMonster = userMonster;
		this.defaultLevel = defaultLevel;
		if (this.levelLabel != null)
		{
			MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(this.userMonster.monsterId);
			this.levelLabel.text = string.Format(StringMaster.GetString("SystemFraction"), this.defaultLevel.ToString(), monsterMasterByMonsterId.Simple.maxLevel);
		}
		this.displayLevel = defaultLevel;
		this.HideDifference();
	}

	private void HideDifference()
	{
		if (this.levelUpLabel != null)
		{
			this.levelUpLabel.gameObject.SetActive(false);
		}
		if (this.hpUpLabel != null)
		{
			this.hpUpLabel.gameObject.SetActive(false);
		}
		if (this.atkUpLabel != null)
		{
			this.atkUpLabel.gameObject.SetActive(false);
		}
		if (this.defUpLabel != null)
		{
			this.defUpLabel.gameObject.SetActive(false);
		}
		if (this.sAtkUpLabel != null)
		{
			this.sAtkUpLabel.gameObject.SetActive(false);
		}
		if (this.sDefUpLabel != null)
		{
			this.sDefUpLabel.gameObject.SetActive(false);
		}
		if (this.spdUpLabel != null)
		{
			this.spdUpLabel.gameObject.SetActive(false);
		}
		if (this.luckUpLabel != null)
		{
			this.luckUpLabel.gameObject.SetActive(false);
		}
		if (this.animationCoroutine != null)
		{
			AppCoroutine.Stop(this.animationCoroutine, false);
			this.ChangeStatusLabel(false);
		}
		this.animationCoroutine = null;
	}

	public void DisplayDifference(int DisplayLevel, int LuckUpValue = 0)
	{
		this.LuckUpValue = LuckUpValue;
		this.Displaylevel = DisplayLevel;
	}

	private void DisplayDifference()
	{
		bool flag = false;
		MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(this.userMonster.monsterId);
		if (this.levelUpLabel != null)
		{
			string text = string.Empty;
			if (this.displayLevel > this.defaultLevel)
			{
				text = string.Format(StringMaster.GetString("SystemFraction"), this.displayLevel.ToString(), monsterMasterByMonsterId.Simple.maxLevel);
			}
			this.levelUpLabel.text = text;
			flag = true;
		}
		int bonusStep = int.Parse(this.userMonster.friendship) / ConstValue.RIZE_CONDITION_FRENDSHIPSTATUS;
		StatusValue friendshipBonusValue = MonsterFriendshipData.GetFriendshipBonusValue(monsterMasterByMonsterId.Simple, bonusStep);
		StatusValue statusValue = MonsterStatusData.GetStatusValue(this.userMonster.monsterId, this.displayLevel.ToString());
		StatusValue statusValue2 = MonsterStatusData.GetStatusValue(this.userMonster.monsterId, this.userMonster.level);
		if (this.hpUpLabel != null)
		{
			float num = 0f;
			float num2 = 0f;
			int num3 = 0;
			if (int.TryParse(this.userMonster.hpAbility, out num3))
			{
				num = Mathf.Floor((float)statusValue.hp * (float)num3 / 100f);
				num2 = Mathf.Floor((float)statusValue2.hp * (float)num3 / 100f);
			}
			int num4 = int.Parse(this.userMonster.hp) - (statusValue2.hp + friendshipBonusValue.hp + (int)num2);
			this.hpUpLabel.text = this.GetTextStatusUpValue(this.userMonster.hp, statusValue.hp + friendshipBonusValue.hp + (int)num + num4);
			flag = true;
		}
		if (this.atkUpLabel != null)
		{
			float num5 = 0f;
			float num6 = 0f;
			int num7 = 0;
			if (int.TryParse(this.userMonster.attackAbility, out num7))
			{
				num5 = Mathf.Floor((float)statusValue.attack * (float)num7 / 100f);
				num6 = Mathf.Floor((float)statusValue2.attack * (float)num7 / 100f);
			}
			int num8 = int.Parse(this.userMonster.attack) - (statusValue2.attack + friendshipBonusValue.attack + (int)num6);
			this.atkUpLabel.text = this.GetTextStatusUpValue(this.userMonster.attack, statusValue.attack + friendshipBonusValue.attack + (int)num5 + num8);
			flag = true;
		}
		if (this.defUpLabel != null)
		{
			float num9 = 0f;
			float num10 = 0f;
			int num11 = 0;
			if (int.TryParse(this.userMonster.defenseAbility, out num11))
			{
				num9 = Mathf.Floor((float)statusValue.defense * (float)num11 / 100f);
				num10 = Mathf.Floor((float)statusValue2.defense * (float)num11 / 100f);
			}
			int num12 = int.Parse(this.userMonster.defense) - (statusValue2.defense + friendshipBonusValue.defense + (int)num10);
			this.defUpLabel.text = this.GetTextStatusUpValue(this.userMonster.defense, statusValue.defense + friendshipBonusValue.defense + (int)num9 + num12);
			flag = true;
		}
		if (this.sAtkUpLabel != null)
		{
			float num13 = 0f;
			float num14 = 0f;
			int num15 = 0;
			if (int.TryParse(this.userMonster.spAttackAbility, out num15))
			{
				num13 = Mathf.Floor((float)statusValue.magicAttack * (float)num15 / 100f);
				num14 = Mathf.Floor((float)statusValue2.magicAttack * (float)num15 / 100f);
			}
			int num16 = int.Parse(this.userMonster.spAttack) - (statusValue2.magicAttack + friendshipBonusValue.magicAttack + (int)num14);
			this.sAtkUpLabel.text = this.GetTextStatusUpValue(this.userMonster.spAttack, statusValue.magicAttack + friendshipBonusValue.magicAttack + (int)num13 + num16);
			flag = true;
		}
		if (this.sDefUpLabel != null)
		{
			float num17 = 0f;
			float num18 = 0f;
			int num19 = 0;
			if (int.TryParse(this.userMonster.spDefenseAbility, out num19))
			{
				num17 = Mathf.Floor((float)statusValue.magicDefense * (float)num19 / 100f);
				num18 = Mathf.Floor((float)statusValue2.magicDefense * (float)num19 / 100f);
			}
			int num20 = int.Parse(this.userMonster.spDefense) - (statusValue2.magicDefense + friendshipBonusValue.magicDefense + (int)num18);
			this.sDefUpLabel.text = this.GetTextStatusUpValue(this.userMonster.spDefense, statusValue.magicDefense + friendshipBonusValue.magicDefense + (int)num17 + num20);
			flag = true;
		}
		if (this.spdUpLabel != null)
		{
			float num21 = 0f;
			float num22 = 0f;
			int num23 = 0;
			if (int.TryParse(this.userMonster.speedAbility, out num23))
			{
				num21 = Mathf.Floor((float)statusValue.speed * (float)num23 / 100f);
				num22 = Mathf.Floor((float)statusValue2.speed * (float)num23 / 100f);
			}
			int num24 = int.Parse(this.userMonster.speed) - (statusValue2.speed + friendshipBonusValue.speed + (int)num22);
			this.spdUpLabel.text = this.GetTextStatusUpValue(this.userMonster.speed, statusValue.speed + friendshipBonusValue.speed + (int)num21 + num24);
			flag = true;
		}
		if (this.luckUpLabel != null)
		{
			this.luckUpLabel.text = this.GetTextStatusUpValue("0", this.luckUpValue);
			flag = true;
		}
		if (flag && this.animationCoroutine == null)
		{
			this.animationCoroutine = AppCoroutine.Start(this.PlayStatusUpAnimation(), false);
		}
	}

	private string GetTextStatusUpValue(string nowValue, int newValue)
	{
		int num = newValue - nowValue.ToInt32();
		if (0 < num)
		{
			return string.Format(StringMaster.GetString("SystemPlusCount"), num);
		}
		return string.Empty;
	}

	private IEnumerator PlayStatusUpAnimation()
	{
		for (;;)
		{
			yield return new WaitForSeconds(this.waitTime);
			this.ChangeStatusLabel(true);
			yield return new WaitForSeconds(this.waitTime);
			this.ChangeStatusLabel(false);
		}
		yield break;
	}

	private void ChangeStatusLabel(bool IsShowUpValue)
	{
		if (this.levelLabel != null && this.levelUpLabel != null && !string.IsNullOrEmpty(this.levelUpLabel.text))
		{
			this.levelLabel.gameObject.SetActive(!IsShowUpValue);
			this.levelUpLabel.gameObject.SetActive(IsShowUpValue);
		}
		if (this.hpLabel != null && this.hpUpLabel != null && !string.IsNullOrEmpty(this.hpUpLabel.text))
		{
			this.hpLabel.gameObject.SetActive(!IsShowUpValue);
			this.hpUpLabel.gameObject.SetActive(IsShowUpValue);
			if (this.hpAddLabel != null && !string.IsNullOrEmpty(this.hpAddLabel.text))
			{
				this.hpAddLabel.gameObject.SetActive(!IsShowUpValue);
			}
		}
		if (this.atkLabel != null && this.atkUpLabel != null && !string.IsNullOrEmpty(this.atkUpLabel.text))
		{
			this.atkLabel.gameObject.SetActive(!IsShowUpValue);
			this.atkUpLabel.gameObject.SetActive(IsShowUpValue);
			if (this.atkAddLabel != null && !string.IsNullOrEmpty(this.atkAddLabel.text))
			{
				this.atkAddLabel.gameObject.SetActive(!IsShowUpValue);
			}
		}
		if (this.defLabel != null && this.defUpLabel != null && !string.IsNullOrEmpty(this.defUpLabel.text))
		{
			this.defLabel.gameObject.SetActive(!IsShowUpValue);
			this.defUpLabel.gameObject.SetActive(IsShowUpValue);
			if (this.defAddLabel != null && !string.IsNullOrEmpty(this.defAddLabel.text))
			{
				this.defAddLabel.gameObject.SetActive(!IsShowUpValue);
			}
		}
		if (this.sAtkLabel != null && this.sAtkUpLabel != null && !string.IsNullOrEmpty(this.sAtkUpLabel.text))
		{
			this.sAtkLabel.gameObject.SetActive(!IsShowUpValue);
			this.sAtkUpLabel.gameObject.SetActive(IsShowUpValue);
			if (this.sAtkAddLabel != null && !string.IsNullOrEmpty(this.sAtkAddLabel.text))
			{
				this.sAtkAddLabel.gameObject.SetActive(!IsShowUpValue);
			}
		}
		if (this.sDefLabel != null && this.sDefUpLabel != null && !string.IsNullOrEmpty(this.sDefUpLabel.text))
		{
			this.sDefLabel.gameObject.SetActive(!IsShowUpValue);
			this.sDefUpLabel.gameObject.SetActive(IsShowUpValue);
			if (this.sDefAddLabel != null && !string.IsNullOrEmpty(this.sDefAddLabel.text))
			{
				this.sDefAddLabel.gameObject.SetActive(!IsShowUpValue);
			}
		}
		if (this.spdLabel != null && this.spdUpLabel != null && !string.IsNullOrEmpty(this.spdUpLabel.text))
		{
			this.spdLabel.gameObject.SetActive(!IsShowUpValue);
			this.spdUpLabel.gameObject.SetActive(IsShowUpValue);
			if (this.spdAddLabel != null && !string.IsNullOrEmpty(this.spdAddLabel.text))
			{
				this.spdAddLabel.gameObject.SetActive(!IsShowUpValue);
			}
		}
		if (this.luckLabel != null && this.luckUpLabel != null && !string.IsNullOrEmpty(this.luckUpLabel.text))
		{
			this.luckLabel.gameObject.SetActive(!IsShowUpValue);
			this.luckUpLabel.gameObject.SetActive(IsShowUpValue);
		}
	}
}
