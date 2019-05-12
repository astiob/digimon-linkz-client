using Master;
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

	public int defaultLevel { get; set; }

	public MonsterData monsterData { get; set; }

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

	public void Initialize(MonsterData monsterData, int defaultLevel)
	{
		this.monsterData = monsterData;
		this.defaultLevel = defaultLevel;
		if (this.levelLabel != null)
		{
			this.levelLabel.text = string.Format(StringMaster.GetString("SystemFraction"), this.defaultLevel.ToString(), this.monsterData.monsterM.maxLevel);
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
		if (this.levelUpLabel != null)
		{
			string text = string.Empty;
			if (this.displayLevel > this.defaultLevel)
			{
				text = string.Format(StringMaster.GetString("SystemFraction"), this.displayLevel.ToString(), this.monsterData.monsterM.maxLevel);
			}
			this.levelUpLabel.text = text;
			flag = true;
		}
		if (this.hpUpLabel != null)
		{
			this.hpUpLabel.text = this.GetTextStatusUpValue(this.monsterData.userMonster.hp, this.monsterData.Now_HP(this.displayLevel));
			flag = true;
		}
		if (this.atkUpLabel != null)
		{
			this.atkUpLabel.text = this.GetTextStatusUpValue(this.monsterData.userMonster.attack, this.monsterData.Now_ATK(this.displayLevel));
			flag = true;
		}
		if (this.defUpLabel != null)
		{
			this.defUpLabel.text = this.GetTextStatusUpValue(this.monsterData.userMonster.defense, this.monsterData.Now_DEF(this.displayLevel));
			flag = true;
		}
		if (this.sAtkUpLabel != null)
		{
			this.sAtkUpLabel.text = this.GetTextStatusUpValue(this.monsterData.userMonster.spAttack, this.monsterData.Now_SATK(this.displayLevel));
			flag = true;
		}
		if (this.sDefUpLabel != null)
		{
			this.sDefUpLabel.text = this.GetTextStatusUpValue(this.monsterData.userMonster.spDefense, this.monsterData.Now_SDEF(this.displayLevel));
			flag = true;
		}
		if (this.spdUpLabel != null)
		{
			this.spdUpLabel.text = this.GetTextStatusUpValue(this.monsterData.userMonster.speed, this.monsterData.Now_SPD(this.displayLevel));
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
