using Master;
using Monster;
using System;
using UnityEngine;

public sealed class MonsterGimmickEffectStatusList : MonsterStatusList
{
	[SerializeField]
	private Color valueUpColor = new Color(0f, 0.6274f, 0f, 1f);

	[SerializeField]
	private Color valueDownColor = new Color(0f, 0.2745f, 0.9411f, 1f);

	[SerializeField]
	private UILabel.Effect valueEffectStyle;

	[SerializeField]
	private Color valueUpEffectColor = new Color(0f, 0.6274f, 0f, 1f);

	[SerializeField]
	private Color valueDownEffectColor = new Color(0f, 0.2745f, 0.9411f, 1f);

	[SerializeField]
	private Vector2 valueEffectDistanceChange = new Vector2(1f, 1f);

	[SerializeField]
	private Color gimmickUpColor = new Color(1f, 0.9411f, 0f, 1f);

	[SerializeField]
	private Color gimmickDownColor = new Color(0.9411f, 0.3921f, 1f, 1f);

	[SerializeField]
	private MonsterGimmickEffectStatusList.DescriptionType gimmickDescriptionType;

	[SerializeField]
	private UILabel hpGimmickLabel;

	[SerializeField]
	private UILabel attackGimmickLabel;

	[SerializeField]
	private UILabel defenseGimmickLabel;

	[SerializeField]
	private UILabel magicAttackGimmickLabel;

	[SerializeField]
	private UILabel magicDefenseGimmickLabel;

	[SerializeField]
	private UILabel speedGimmickLabel;

	[SerializeField]
	private StatusGimmickEffectArrow hpArrow;

	[SerializeField]
	private StatusGimmickEffectArrow attackArrow;

	[SerializeField]
	private StatusGimmickEffectArrow defenseArrow;

	[SerializeField]
	private StatusGimmickEffectArrow magicAttackArrow;

	[SerializeField]
	private StatusGimmickEffectArrow magicDefenseArrow;

	[SerializeField]
	private StatusGimmickEffectArrow speedArrow;

	private Color valueDefaultColor;

	private UILabel.Effect valueDefaultEffectStyle;

	private Color valueDefaultEffectColor;

	private Vector2 valueDefaultEffectDistance;

	private void Awake()
	{
		this.valueDefaultColor = this.hpLabel.color;
		this.valueDefaultEffectStyle = this.hpLabel.effectStyle;
		this.valueDefaultEffectColor = this.hpLabel.effectColor;
		this.valueDefaultEffectDistance = this.hpLabel.effectDistance;
	}

	public override void ClearValues()
	{
		base.ClearValues();
		this.ResetParameter(this.hpLabel, this.hpGimmickLabel, this.hpArrow);
		this.ResetParameter(this.attackLabel, this.attackGimmickLabel, this.attackArrow);
		this.ResetParameter(this.defenseLabel, this.defenseGimmickLabel, this.defenseArrow);
		this.ResetParameter(this.magicAttackLabel, this.magicAttackGimmickLabel, this.magicAttackArrow);
		this.ResetParameter(this.magicDefenceLabel, this.magicDefenseGimmickLabel, this.magicDefenseArrow);
		this.ResetParameter(this.speedLabel, this.speedGimmickLabel, this.speedArrow);
	}

	public bool SetValues(MonsterData monsterData, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] gimmickEffectArray)
	{
		bool result = false;
		int changeValue;
		int num;
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out changeValue, out num, monsterData, gimmickEffectArray, EffectStatusBase.ExtraEffectType.Hp, 0);
		this.SetParameter(monsterData.userMonster.hp, changeValue, this.hpLabel, this.hpGimmickLabel, this.hpArrow);
		if (num != 0)
		{
			result = true;
		}
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out changeValue, out num, monsterData, gimmickEffectArray, EffectStatusBase.ExtraEffectType.Atk, 0);
		this.SetParameter(monsterData.userMonster.attack, changeValue, this.attackLabel, this.attackGimmickLabel, this.attackArrow);
		if (num != 0)
		{
			result = true;
		}
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out changeValue, out num, monsterData, gimmickEffectArray, EffectStatusBase.ExtraEffectType.Def, 0);
		this.SetParameter(monsterData.userMonster.defense, changeValue, this.defenseLabel, this.defenseGimmickLabel, this.defenseArrow);
		if (num != 0)
		{
			result = true;
		}
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out changeValue, out num, monsterData, gimmickEffectArray, EffectStatusBase.ExtraEffectType.Satk, 0);
		this.SetParameter(monsterData.userMonster.spAttack, changeValue, this.magicAttackLabel, this.magicAttackGimmickLabel, this.magicAttackArrow);
		if (num != 0)
		{
			result = true;
		}
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out changeValue, out num, monsterData, gimmickEffectArray, EffectStatusBase.ExtraEffectType.Sdef, 0);
		this.SetParameter(monsterData.userMonster.spDefense, changeValue, this.magicDefenceLabel, this.magicDefenseGimmickLabel, this.magicDefenseArrow);
		if (num != 0)
		{
			result = true;
		}
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out changeValue, out num, monsterData, gimmickEffectArray, EffectStatusBase.ExtraEffectType.Speed, 0);
		this.SetParameter(monsterData.userMonster.speed, changeValue, this.speedLabel, this.speedGimmickLabel, this.speedArrow);
		if (num != 0)
		{
			result = true;
		}
		this.luckLabel.text = monsterData.userMonster.luck;
		if (!this.disableFriendshipMaxValue)
		{
			this.friendshipLabel.text = MonsterFriendshipData.GetMaxFriendshipFormat(monsterData.userMonster.friendship, monsterData.monsterMG.growStep);
		}
		else
		{
			this.friendshipLabel.text = monsterData.userMonster.friendship;
		}
		return result;
	}

	private void SetParameter(string defaultValue, int changeValue, UILabel statusValueLabel, UILabel gimmickLabel, StatusGimmickEffectArrow arrow)
	{
		statusValueLabel.text = changeValue.ToString();
		int num = int.Parse(defaultValue);
		if (changeValue > num)
		{
			statusValueLabel.effectStyle = this.valueEffectStyle;
			statusValueLabel.color = this.valueUpColor;
			statusValueLabel.effectColor = this.valueUpEffectColor;
			statusValueLabel.effectDistance = this.valueEffectDistanceChange;
			if (this.gimmickDescriptionType == MonsterGimmickEffectStatusList.DescriptionType.LABEL)
			{
				gimmickLabel.gameObject.SetActive(true);
				gimmickLabel.text = StringMaster.GetString("StatusUpGimmick");
				gimmickLabel.color = this.gimmickUpColor;
			}
			else
			{
				arrow.DisplayUpArrow();
			}
		}
		else if (changeValue < num)
		{
			statusValueLabel.effectStyle = this.valueEffectStyle;
			statusValueLabel.color = this.valueDownColor;
			statusValueLabel.effectColor = this.valueDownEffectColor;
			statusValueLabel.effectDistance = this.valueEffectDistanceChange;
			if (this.gimmickDescriptionType == MonsterGimmickEffectStatusList.DescriptionType.LABEL)
			{
				gimmickLabel.gameObject.SetActive(true);
				gimmickLabel.text = StringMaster.GetString("StatusDownGimmick");
				gimmickLabel.color = this.gimmickDownColor;
			}
			else
			{
				arrow.DisplayBottomArrow();
			}
		}
		else
		{
			this.ResetParameter(statusValueLabel, gimmickLabel, arrow);
		}
	}

	private void ResetParameter(UILabel statusValueLabel, UILabel gimmickLabel, StatusGimmickEffectArrow arrow)
	{
		statusValueLabel.effectStyle = this.valueDefaultEffectStyle;
		statusValueLabel.color = this.valueDefaultColor;
		statusValueLabel.effectColor = this.valueDefaultEffectColor;
		statusValueLabel.effectDistance = this.valueDefaultEffectDistance;
		if (this.gimmickDescriptionType == MonsterGimmickEffectStatusList.DescriptionType.LABEL)
		{
			gimmickLabel.gameObject.SetActive(false);
		}
		else
		{
			arrow.Reset();
		}
	}

	private enum DescriptionType
	{
		LABEL,
		ARROW
	}
}
