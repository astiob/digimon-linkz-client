using Master;
using System;
using UnityEngine;

public class BattleSkillBtn : MonoBehaviour
{
	[SerializeField]
	public UIButton button;

	[SerializeField]
	[Header("SkillText")]
	public UILabel skillName;

	[SerializeField]
	public UILabel skillDescription;

	[SerializeField]
	private UILabel ap;

	[SerializeField]
	private UILabel hitRate;

	[SerializeField]
	private UILabel power;

	[SerializeField]
	[Header("スキナー")]
	public UISpriteSkinnerBase attributeIcon;

	[SerializeField]
	public UIComponentSkinner skillButtonMode;

	[SerializeField]
	public UIComponentSkinner skillDescriptionEnabled;

	[SerializeField]
	public UIComponentSkinner enabledSkillLock;

	[Header("Tweener")]
	[SerializeField]
	public UITweener rotationEffect1;

	[SerializeField]
	public UITweener rotationEffect2;

	[SerializeField]
	[Header("Collider")]
	public Collider colliderValue;

	[SerializeField]
	[Header("HoldPress")]
	public HoldPressButton skillDescriptionSwitch;

	public void ApplySkillButtonData(SkillStatus skills, bool onEnable, bool onSkillLock, CharacterStateControl selectCharacter)
	{
		int skillPowerCorrectionValue = skills.power;
		float value = skills.hitRate;
		AffectEffectProperty affectEffectFirst = skills.GetAffectEffectFirst();
		if (affectEffectFirst != null)
		{
			if (affectEffectFirst.type == AffectEffect.Damage)
			{
				skillPowerCorrectionValue = ExtraEffectStatus.GetSkillPowerCorrectionValue(BattleStateManager.current.battleStateData.extraEffectStatus, affectEffectFirst, selectCharacter);
			}
			value = ExtraEffectStatus.GetSkillHitRateCorrectionValue(BattleStateManager.current.battleStateData.extraEffectStatus, affectEffectFirst, selectCharacter);
			value = Mathf.Clamp01(value);
		}
		this.skillButtonMode.isLock = false;
		this.skillName.text = skills.name;
		this.skillDescription.text = skills.description;
		this.hitRate.text = value.ToString("p0");
		this.power.text = skillPowerCorrectionValue.ToString();
		int correctedAp = skills.GetCorrectedAp(selectCharacter);
		if (correctedAp <= selectCharacter.ap)
		{
			this.ap.text = string.Format(StringMaster.GetString("BattleSkillUI-01"), correctedAp);
		}
		else
		{
			this.ap.text = StringMaster.GetString("BattleSkillUI-02");
		}
		this.attributeIcon.value = (int)skills.attribute;
		this.skillButtonMode.SetSkins(0);
		if (!onSkillLock)
		{
			this.enabledSkillLock.SetSkins(0);
		}
		else
		{
			this.enabledSkillLock.SetSkins(1);
		}
		if (onEnable)
		{
			return;
		}
		this.skillButtonMode.SetSkins(2);
		this.skillButtonMode.isLock = true;
	}

	public void SetColliderActive(bool active)
	{
		if (this.colliderValue != null)
		{
			this.colliderValue.enabled = active;
		}
	}
}
