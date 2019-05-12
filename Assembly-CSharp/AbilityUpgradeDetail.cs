using Ability;
using System;
using UnityEngine;

public class AbilityUpgradeDetail : MonoBehaviour
{
	[SerializeField]
	private UISprite charaIcon;

	[SerializeField]
	private MonsterMedalList medalList;

	[SerializeField]
	private MonsterMedalRateList medalRateList;

	[SerializeField]
	private MonsterMedalList medalList_S;

	[SerializeField]
	private MonsterMedalRateList medalRateList_S;

	private GUIMonsterIcon csMonsterIcon;

	public void ClearStatus()
	{
		this.medalList.SetActive(false);
		this.medalRateList.SetActive(false);
		this.medalList_S.SetActive(false);
		this.medalRateList_S.SetActive(false);
		this.ShowIcon(null, false);
		this.charaIcon.spriteName = "Common02_Thumbnail_none";
	}

	public void SetStatus(MonsterAbilityStatusInfo abilityStatus)
	{
		if (abilityStatus == null)
		{
			this.ClearStatus();
		}
		else
		{
			this.medalList.SetActive(true);
			this.medalRateList.SetActive(true);
			this.medalList_S.SetActive(true);
			this.medalRateList_S.SetActive(true);
			GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonsterList = new GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList();
			userMonsterList.hpAbilityFlg = abilityStatus.hpAbilityFlg;
			userMonsterList.hpAbility = abilityStatus.hpAbility;
			userMonsterList.attackAbilityFlg = abilityStatus.attackAbilityFlg;
			userMonsterList.attackAbility = abilityStatus.attackAbility;
			userMonsterList.defenseAbilityFlg = abilityStatus.defenseAbilityFlg;
			userMonsterList.defenseAbility = abilityStatus.defenseAbility;
			userMonsterList.spAttackAbilityFlg = abilityStatus.spAttackAbilityFlg;
			userMonsterList.spAttackAbility = abilityStatus.spAttackAbility;
			userMonsterList.spDefenseAbilityFlg = abilityStatus.spDefenseAbilityFlg;
			userMonsterList.spDefenseAbility = abilityStatus.spDefenseAbility;
			userMonsterList.speedAbilityFlg = abilityStatus.speedAbilityFlg;
			userMonsterList.speedAbility = abilityStatus.speedAbility;
			this.medalList.SetValues(userMonsterList);
			string hp = string.Empty;
			string atk = string.Empty;
			string def = string.Empty;
			string s_atk = string.Empty;
			string s_def = string.Empty;
			string spd = string.Empty;
			if (ClassSingleton<AbilityData>.Instance.HasAbility(abilityStatus.hpAbilityMinGuarantee))
			{
				userMonsterList.hpAbilityFlg = ClassSingleton<AbilityData>.Instance.GetAbilityType(abilityStatus.hpAbilityMinGuarantee);
				userMonsterList.hpAbility = abilityStatus.hpAbilityMinGuarantee;
				hp = abilityStatus.hpAbilityMinGuaranteeRate.ToString();
			}
			else
			{
				userMonsterList.hpAbilityFlg = "0";
				userMonsterList.hpAbility = "0";
			}
			if (ClassSingleton<AbilityData>.Instance.HasAbility(abilityStatus.attackAbilityMinGuarantee))
			{
				userMonsterList.attackAbilityFlg = ClassSingleton<AbilityData>.Instance.GetAbilityType(abilityStatus.attackAbilityMinGuarantee);
				userMonsterList.attackAbility = abilityStatus.attackAbilityMinGuarantee;
				atk = abilityStatus.attackAbilityMinGuaranteeRate.ToString();
			}
			else
			{
				userMonsterList.attackAbilityFlg = "0";
				userMonsterList.attackAbility = "0";
			}
			if (ClassSingleton<AbilityData>.Instance.HasAbility(abilityStatus.defenseAbilityMinGuarantee))
			{
				userMonsterList.defenseAbilityFlg = ClassSingleton<AbilityData>.Instance.GetAbilityType(abilityStatus.defenseAbilityMinGuarantee);
				userMonsterList.defenseAbility = abilityStatus.defenseAbilityMinGuarantee;
				def = abilityStatus.defenseAbilityMinGuaranteeRate.ToString();
			}
			else
			{
				userMonsterList.defenseAbilityFlg = "0";
				userMonsterList.defenseAbility = "0";
			}
			if (ClassSingleton<AbilityData>.Instance.HasAbility(abilityStatus.spAttackAbilityMinGuarantee))
			{
				userMonsterList.spAttackAbilityFlg = ClassSingleton<AbilityData>.Instance.GetAbilityType(abilityStatus.spAttackAbilityMinGuarantee);
				userMonsterList.spAttackAbility = abilityStatus.spAttackAbilityMinGuarantee;
				s_atk = abilityStatus.spAttackAbilityMinGuaranteeRate.ToString();
			}
			else
			{
				userMonsterList.spAttackAbilityFlg = "0";
				userMonsterList.spAttackAbility = "0";
			}
			if (ClassSingleton<AbilityData>.Instance.HasAbility(abilityStatus.spDefenseAbilityMinGuarantee))
			{
				userMonsterList.spDefenseAbilityFlg = ClassSingleton<AbilityData>.Instance.GetAbilityType(abilityStatus.spDefenseAbilityMinGuarantee);
				userMonsterList.spDefenseAbility = abilityStatus.spDefenseAbilityMinGuarantee;
				s_def = abilityStatus.spDefenseAbilityMinGuaranteeRate.ToString();
			}
			else
			{
				userMonsterList.spDefenseAbilityFlg = "0";
				userMonsterList.spDefenseAbility = "0";
			}
			if (ClassSingleton<AbilityData>.Instance.HasAbility(abilityStatus.speedAbilityMinGuarantee))
			{
				userMonsterList.speedAbilityFlg = ClassSingleton<AbilityData>.Instance.GetAbilityType(abilityStatus.speedAbilityMinGuarantee);
				userMonsterList.speedAbility = abilityStatus.speedAbilityMinGuarantee;
				spd = abilityStatus.speedAbilityMinGuaranteeRate.ToString();
			}
			else
			{
				userMonsterList.speedAbilityFlg = "0";
				userMonsterList.speedAbility = "0";
			}
			this.medalList_S.SetValues(userMonsterList);
			this.medalRateList.SetValues(abilityStatus);
			this.medalRateList_S.SetValues(hp, atk, def, s_atk, s_def, spd);
			this.SetAlphaTween();
		}
	}

	private void SetAlphaTween()
	{
		AlphaTweenerController component = base.gameObject.GetComponent<AlphaTweenerController>();
		component.ClearWidgetList();
		if (this.medalList.HpIcon.gameObject.activeSelf && this.medalList_S.HpIcon.gameObject.activeSelf)
		{
			component.AddPairWidget(this.medalList.HpIcon, this.medalList_S.HpIcon);
			component.AddPairWidget(this.medalRateList.HpLabel, this.medalRateList_S.HpLabel);
		}
		if (this.medalList.AttackIcon.gameObject.activeSelf && this.medalList_S.AttackIcon.gameObject.activeSelf)
		{
			component.AddPairWidget(this.medalList.AttackIcon, this.medalList_S.AttackIcon);
			component.AddPairWidget(this.medalRateList.AttackLabel, this.medalRateList_S.AttackLabel);
		}
		if (this.medalList.DefenseIcon.gameObject.activeSelf && this.medalList_S.DefenseIcon.gameObject.activeSelf)
		{
			component.AddPairWidget(this.medalList.DefenseIcon, this.medalList_S.DefenseIcon);
			component.AddPairWidget(this.medalRateList.DefenseLabel, this.medalRateList_S.DefenseLabel);
		}
		if (this.medalList.MagicAttackIcon.gameObject.activeSelf && this.medalList_S.MagicAttackIcon.gameObject.activeSelf)
		{
			component.AddPairWidget(this.medalList.MagicAttackIcon, this.medalList_S.MagicAttackIcon);
			component.AddPairWidget(this.medalRateList.MagicAttackLabel, this.medalRateList_S.MagicAttackLabel);
		}
		if (this.medalList.MagicDefenseIcon.gameObject.activeSelf && this.medalList_S.MagicDefenseIcon.gameObject.activeSelf)
		{
			component.AddPairWidget(this.medalList.MagicDefenseIcon, this.medalList_S.MagicDefenseIcon);
			component.AddPairWidget(this.medalRateList.MagicDefenseLabel, this.medalRateList_S.MagicDefenseLabel);
		}
		if (this.medalList.SpeedIcon.gameObject.activeSelf && this.medalList_S.SpeedIcon.gameObject.activeSelf)
		{
			component.AddPairWidget(this.medalList.SpeedIcon, this.medalList_S.SpeedIcon);
			component.AddPairWidget(this.medalRateList.SpeedLabel, this.medalRateList_S.SpeedLabel);
		}
	}

	public void ShowIcon(MonsterData md, bool active)
	{
		if (!active)
		{
			if (this.csMonsterIcon != null)
			{
				UnityEngine.Object.Destroy(this.csMonsterIcon.gameObject);
				this.csMonsterIcon = null;
			}
		}
		else
		{
			if (this.csMonsterIcon != null)
			{
				UnityEngine.Object.Destroy(this.csMonsterIcon.gameObject);
			}
			GameObject gameObject = this.charaIcon.gameObject;
			this.csMonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(md, gameObject.transform.localScale, gameObject.transform.localPosition, gameObject.transform.parent, true, false);
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component != null)
			{
				DepthController.SetWidgetDepth_2(this.csMonsterIcon.gameObject.transform, component.depth + 2);
			}
		}
	}
}
