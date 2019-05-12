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
		if (this.hpUpLabel != null)
		{
			this.hpUpLabel.text = this.GetTextStatusUpValue(this.userMonster.hp, statusValue.hp + friendshipBonusValue.hp);
			flag = true;
		}
		if (this.atkUpLabel != null)
		{
			this.atkUpLabel.text = this.GetTextStatusUpValue(this.userMonster.attack, statusValue.attack + friendshipBonusValue.attack);
			flag = true;
		}
		if (this.defUpLabel != null)
		{
			this.defUpLabel.text = this.GetTextStatusUpValue(this.userMonster.defense, statusValue.defense + friendshipBonusValue.defense);
			flag = true;
		}
		if (this.sAtkUpLabel != null)
		{
			this.sAtkUpLabel.text = this.GetTextStatusUpValue(this.userMonster.spAttack, statusValue.magicAttack + friendshipBonusValue.magicAttack);
			flag = true;
		}
		if (this.sDefUpLabel != null)
		{
			this.sDefUpLabel.text = this.GetTextStatusUpValue(this.userMonster.spDefense, statusValue.magicDefense + friendshipBonusValue.magicDefense);
			flag = true;
		}
		if (this.spdUpLabel != null)
		{
			this.spdUpLabel.text = this.GetTextStatusUpValue(this.userMonster.speed, statusValue.speed + friendshipBonusValue.speed);
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
