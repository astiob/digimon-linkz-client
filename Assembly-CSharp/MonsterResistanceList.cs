using Master;
using System;
using UnityEngine;

public sealed class MonsterResistanceList : MonoBehaviour
{
	[SerializeField]
	private UILabel resistanceTitle;

	[SerializeField]
	private UISprite nothingnessIcon;

	[SerializeField]
	private UISprite fireIcon;

	[SerializeField]
	private UISprite waterIcon;

	[SerializeField]
	private UISprite thunderIcon;

	[SerializeField]
	private UISprite natureIcon;

	[SerializeField]
	private UISprite lightIcon;

	[SerializeField]
	private UISprite darkIcon;

	[SerializeField]
	private UISprite stunIcon;

	[SerializeField]
	private UISprite skillLockIcon;

	[SerializeField]
	private UISprite sleepIcon;

	[SerializeField]
	private UISprite paralysisIcon;

	[SerializeField]
	private UISprite confusionIcon;

	[SerializeField]
	private UISprite poisonIcon;

	[SerializeField]
	private UISprite deathIcon;

	public void Start()
	{
		if (null != this.resistanceTitle)
		{
			this.resistanceTitle.text = StringMaster.GetString("CharaStatus-22");
		}
	}

	public void ClearValues()
	{
		string value = "0";
		this.SetResistanceIcon(ConstValue.ResistanceType.NOTHINGNESS, value, this.nothingnessIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.FIRE, value, this.fireIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.WATER, value, this.waterIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.THUNDER, value, this.thunderIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.NATURE, value, this.natureIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.LIGHT, value, this.lightIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.DARK, value, this.darkIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.STUN, value, this.stunIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.SKILL_LOCK, value, this.skillLockIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.SLEEP, value, this.sleepIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.PARALYSIS, value, this.paralysisIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.CONFUSION, value, this.confusionIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.POISON, value, this.poisonIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.DEATH, value, this.deathIcon);
	}

	public void SetValues(MonsterData monsterData)
	{
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM values = monsterData.AddResistanceFromMultipleTranceData();
		this.SetValues(values);
	}

	public void SetValues(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistance)
	{
		this.SetResistanceIcon(ConstValue.ResistanceType.NOTHINGNESS, resistance.none, this.nothingnessIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.FIRE, resistance.fire, this.fireIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.WATER, resistance.water, this.waterIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.THUNDER, resistance.thunder, this.thunderIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.NATURE, resistance.nature, this.natureIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.LIGHT, resistance.light, this.lightIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.DARK, resistance.dark, this.darkIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.STUN, resistance.stun, this.stunIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.SKILL_LOCK, resistance.skillLock, this.skillLockIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.SLEEP, resistance.sleep, this.sleepIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.PARALYSIS, resistance.paralysis, this.paralysisIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.CONFUSION, resistance.confusion, this.confusionIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.POISON, resistance.poison, this.poisonIcon);
		this.SetResistanceIcon(ConstValue.ResistanceType.DEATH, resistance.death, this.deathIcon);
	}

	public void SetValue(ConstValue.ResistanceType type, string value)
	{
		UISprite icon = null;
		switch (type)
		{
		case ConstValue.ResistanceType.NOTHINGNESS:
			icon = this.nothingnessIcon;
			break;
		case ConstValue.ResistanceType.FIRE:
			icon = this.fireIcon;
			break;
		case ConstValue.ResistanceType.WATER:
			icon = this.waterIcon;
			break;
		case ConstValue.ResistanceType.THUNDER:
			icon = this.thunderIcon;
			break;
		case ConstValue.ResistanceType.NATURE:
			icon = this.natureIcon;
			break;
		case ConstValue.ResistanceType.LIGHT:
			icon = this.lightIcon;
			break;
		case ConstValue.ResistanceType.DARK:
			icon = this.darkIcon;
			break;
		case ConstValue.ResistanceType.STUN:
			icon = this.stunIcon;
			break;
		case ConstValue.ResistanceType.SKILL_LOCK:
			icon = this.skillLockIcon;
			break;
		case ConstValue.ResistanceType.SLEEP:
			icon = this.sleepIcon;
			break;
		case ConstValue.ResistanceType.PARALYSIS:
			icon = this.paralysisIcon;
			break;
		case ConstValue.ResistanceType.CONFUSION:
			icon = this.confusionIcon;
			break;
		case ConstValue.ResistanceType.POISON:
			icon = this.poisonIcon;
			break;
		case ConstValue.ResistanceType.DEATH:
			icon = this.deathIcon;
			break;
		}
		this.SetResistanceIcon(type, value, icon);
	}

	private void SetResistanceIcon(ConstValue.ResistanceType type, string value, UISprite icon)
	{
		if (null != icon)
		{
			icon.spriteName = MonsterDetailUtil.GetResistanceSpriteName(type);
			icon.color = MonsterDetailUtil.GetResistanceSpriteColor(value);
		}
	}
}
