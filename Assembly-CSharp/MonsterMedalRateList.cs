using Ability;
using Master;
using System;
using UnityEngine;

public sealed class MonsterMedalRateList : MonoBehaviour
{
	[SerializeField]
	private UILabel hpLabel;

	[SerializeField]
	private UILabel attackLabel;

	[SerializeField]
	private UILabel defenseLabel;

	[SerializeField]
	private UILabel magicAttackLabel;

	[SerializeField]
	private UILabel magicDefenseLabel;

	[SerializeField]
	private UILabel speedLabel;

	private void Awake()
	{
	}

	public void SetActive(bool isActive)
	{
		this.hpLabel.gameObject.SetActive(isActive);
		this.FixAlpha(this.hpLabel, 1f);
		this.attackLabel.gameObject.SetActive(isActive);
		this.FixAlpha(this.attackLabel, 1f);
		this.defenseLabel.gameObject.SetActive(isActive);
		this.FixAlpha(this.defenseLabel, 1f);
		this.magicAttackLabel.gameObject.SetActive(isActive);
		this.FixAlpha(this.magicAttackLabel, 1f);
		this.magicDefenseLabel.gameObject.SetActive(isActive);
		this.FixAlpha(this.magicDefenseLabel, 1f);
		this.speedLabel.gameObject.SetActive(isActive);
		this.FixAlpha(this.speedLabel, 1f);
	}

	private void FixAlpha(UIWidget wdg, float alpha = 1f)
	{
		Color color = wdg.color;
		color.a = alpha;
		wdg.color = color;
	}

	public void SetValues(string hp, string atk, string def, string s_atk, string s_def, string spd)
	{
		if (hp != string.Empty)
		{
			this.hpLabel.text = string.Format(StringMaster.GetString("SystemPercent"), hp);
		}
		else
		{
			this.hpLabel.text = string.Empty;
		}
		if (atk != string.Empty)
		{
			this.attackLabel.text = string.Format(StringMaster.GetString("SystemPercent"), atk);
		}
		else
		{
			this.attackLabel.text = string.Empty;
		}
		if (def != string.Empty)
		{
			this.defenseLabel.text = string.Format(StringMaster.GetString("SystemPercent"), def);
		}
		else
		{
			this.defenseLabel.text = string.Empty;
		}
		if (s_atk != string.Empty)
		{
			this.magicAttackLabel.text = string.Format(StringMaster.GetString("SystemPercent"), s_atk);
		}
		else
		{
			this.magicAttackLabel.text = string.Empty;
		}
		if (s_def != string.Empty)
		{
			this.magicDefenseLabel.text = string.Format(StringMaster.GetString("SystemPercent"), s_def);
		}
		else
		{
			this.magicDefenseLabel.text = string.Empty;
		}
		if (spd != string.Empty)
		{
			this.speedLabel.text = string.Format(StringMaster.GetString("SystemPercent"), spd);
		}
		else
		{
			this.speedLabel.text = string.Empty;
		}
	}

	public void SetValues(MonsterAbilityStatusInfo abilityStatus)
	{
		this.hpLabel.text = this.ResolveSituation(abilityStatus.hpAbilityFlg, abilityStatus.hpAbilityRate, abilityStatus.hpNoMaterial, abilityStatus.hpIsAbilityMax);
		this.attackLabel.text = this.ResolveSituation(abilityStatus.attackAbilityFlg, abilityStatus.attackAbilityRate, abilityStatus.attackNoMaterial, abilityStatus.attackIsAbilityMax);
		this.defenseLabel.text = this.ResolveSituation(abilityStatus.defenseAbilityFlg, abilityStatus.defenseAbilityRate, abilityStatus.defenseNoMaterial, abilityStatus.defenseIsAbilityMax);
		this.magicAttackLabel.text = this.ResolveSituation(abilityStatus.spAttackAbilityFlg, abilityStatus.spAttackAbilityRate, abilityStatus.spAttackNoMaterial, abilityStatus.spAttackIsAbilityMax);
		this.magicDefenseLabel.text = this.ResolveSituation(abilityStatus.spDefenseAbilityFlg, abilityStatus.spDefenseAbilityRate, abilityStatus.spDefenseNoMaterial, abilityStatus.spDefenseIsAbilityMax);
		this.speedLabel.text = this.ResolveSituation(abilityStatus.speedAbilityFlg, abilityStatus.speedAbilityRate, abilityStatus.speedNoMaterial, abilityStatus.speedIsAbilityMax);
	}

	private string ResolveSituation(string abilityFlg, float abilityRate, bool noMaterial, bool isAbilityMax)
	{
		string result = string.Empty;
		int num = int.Parse(abilityFlg);
		if (num == 0)
		{
			result = StringMaster.GetString("SystemNoneHyphen");
		}
		else
		{
			result = string.Format(StringMaster.GetString("SystemPercent"), abilityRate);
		}
		if (noMaterial)
		{
			if (num > 0)
			{
				result = string.Empty;
			}
		}
		else if (num > 0 && abilityRate == 0f && isAbilityMax)
		{
			result = string.Empty;
		}
		return result;
	}

	public UILabel HpLabel
	{
		get
		{
			return this.hpLabel;
		}
	}

	public UILabel AttackLabel
	{
		get
		{
			return this.attackLabel;
		}
	}

	public UILabel DefenseLabel
	{
		get
		{
			return this.defenseLabel;
		}
	}

	public UILabel MagicAttackLabel
	{
		get
		{
			return this.magicAttackLabel;
		}
	}

	public UILabel MagicDefenseLabel
	{
		get
		{
			return this.magicDefenseLabel;
		}
	}

	public UILabel SpeedLabel
	{
		get
		{
			return this.speedLabel;
		}
	}
}
