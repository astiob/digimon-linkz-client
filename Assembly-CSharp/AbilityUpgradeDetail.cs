using Ability;
using Master;
using System;
using UI.Common;
using UI.MedalInheritance;
using UnityEngine;

public class AbilityUpgradeDetail : MonoBehaviour
{
	[SerializeField]
	private UISprite charaIcon;

	[Header("0=HP, 1=ATK, 2=DEF, 3=S-ATK, 4=S-DEF, 5=SPD")]
	[SerializeField]
	private MonsterMedalInheritanceIcon[] medals;

	private GUIMonsterIcon csMonsterIcon;

	private void Awake()
	{
		for (int i = 0; i < this.medals.Length; i++)
		{
			this.medals[i].Initialize();
		}
	}

	public void ClearStatus()
	{
		for (int i = 0; i < this.medals.Length; i++)
		{
			this.medals[i].ClearMedal();
			this.medals[i].SetActive(false);
		}
		this.ShowIcon(null, false);
		this.charaIcon.spriteName = "Common02_Thumbnail_none";
	}

	private void SetInheritanceInfo(int medalIndex, bool isNoMaterial, bool isMaxLevel, string medalType, string medalParcentage, float inheritanceRate, string medalTypeSub, string medalParcentageSub, float inheritanceRateSub)
	{
		this.medals[medalIndex].ClearMedal();
		if ("0" == medalType)
		{
			this.medals[medalIndex].SetActive(false);
		}
		else
		{
			this.medals[medalIndex].SetActive(true);
			if (isNoMaterial)
			{
				string medalSpriteName = MonsterMedalIcon.GetMedalSpriteName(medalType, medalParcentage);
				this.medals[medalIndex].SetFirstView(medalSpriteName, StringMaster.GetString("SystemNoneHyphen"));
			}
			else
			{
				string medalSpriteName2 = MonsterMedalIcon.GetMedalSpriteName(medalType, medalParcentage);
				string rate = string.Format(StringMaster.GetString("SystemPercent"), inheritanceRate);
				this.medals[medalIndex].SetFirstView(medalSpriteName2, rate);
				if ("0" != medalTypeSub)
				{
					medalSpriteName2 = MonsterMedalIcon.GetMedalSpriteName(medalTypeSub, medalParcentageSub);
					rate = string.Format(StringMaster.GetString("SystemPercent"), inheritanceRateSub);
					this.medals[medalIndex].AddMedalInfo(medalSpriteName2, rate);
				}
			}
		}
	}

	public void SetStatus(MonsterAbilityStatusInfo abilityStatus)
	{
		this.SetInheritanceInfo(0, abilityStatus.hpNoMaterial, abilityStatus.hpIsAbilityMax, abilityStatus.hpAbilityFlg, abilityStatus.hpAbility, abilityStatus.hpAbilityRate, MonsterMedalIcon.GetMedalType(abilityStatus.hpAbilityMinGuarantee), abilityStatus.hpAbilityMinGuarantee, abilityStatus.hpAbilityMinGuaranteeRate);
		this.SetInheritanceInfo(1, abilityStatus.attackNoMaterial, abilityStatus.attackIsAbilityMax, abilityStatus.attackAbilityFlg, abilityStatus.attackAbility, abilityStatus.attackAbilityRate, MonsterMedalIcon.GetMedalType(abilityStatus.attackAbilityMinGuarantee), abilityStatus.attackAbilityMinGuarantee, abilityStatus.attackAbilityMinGuaranteeRate);
		this.SetInheritanceInfo(2, abilityStatus.defenseNoMaterial, abilityStatus.defenseIsAbilityMax, abilityStatus.defenseAbilityFlg, abilityStatus.defenseAbility, abilityStatus.defenseAbilityRate, MonsterMedalIcon.GetMedalType(abilityStatus.defenseAbilityMinGuarantee), abilityStatus.defenseAbilityMinGuarantee, abilityStatus.defenseAbilityMinGuaranteeRate);
		this.SetInheritanceInfo(3, abilityStatus.spAttackNoMaterial, abilityStatus.spAttackIsAbilityMax, abilityStatus.spAttackAbilityFlg, abilityStatus.spAttackAbility, abilityStatus.spAttackAbilityRate, MonsterMedalIcon.GetMedalType(abilityStatus.spAttackAbilityMinGuarantee), abilityStatus.spAttackAbilityMinGuarantee, abilityStatus.spAttackAbilityMinGuaranteeRate);
		this.SetInheritanceInfo(4, abilityStatus.spDefenseNoMaterial, abilityStatus.spDefenseIsAbilityMax, abilityStatus.spDefenseAbilityFlg, abilityStatus.spDefenseAbility, abilityStatus.spDefenseAbilityRate, MonsterMedalIcon.GetMedalType(abilityStatus.spDefenseAbilityMinGuarantee), abilityStatus.spDefenseAbilityMinGuarantee, abilityStatus.spDefenseAbilityMinGuaranteeRate);
		this.SetInheritanceInfo(5, abilityStatus.speedNoMaterial, abilityStatus.speedIsAbilityMax, abilityStatus.speedAbilityFlg, abilityStatus.speedAbility, abilityStatus.speedAbilityRate, MonsterMedalIcon.GetMedalType(abilityStatus.speedAbilityMinGuarantee), abilityStatus.speedAbilityMinGuarantee, abilityStatus.speedAbilityMinGuaranteeRate);
		for (int i = 0; i < this.medals.Length; i++)
		{
			this.medals[i].StartAnimation();
		}
	}

	public void ShowIcon(MonsterData md, bool active)
	{
		if (this.csMonsterIcon != null)
		{
			UnityEngine.Object.Destroy(this.csMonsterIcon.gameObject);
		}
		if (!active)
		{
			this.csMonsterIcon = null;
		}
		else
		{
			GameObject gameObject = this.charaIcon.gameObject;
			this.csMonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(md, gameObject.transform.localScale, gameObject.transform.localPosition, gameObject.transform.parent, true, false);
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component != null)
			{
				DepthController.SetWidgetDepth_Static(this.csMonsterIcon.gameObject.transform, component.depth + 2);
			}
		}
	}
}
