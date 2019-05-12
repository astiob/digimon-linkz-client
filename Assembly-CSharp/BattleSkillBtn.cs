﻿using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleSkillBtn : MonoBehaviour
{
	[Header("スキルボタンが2つのときの位置")]
	[SerializeField]
	private Vector3 twoButtonPosition = Vector3.zero;

	[Header("スキルボタンが3つのときの位置")]
	[SerializeField]
	private Vector3 threeButtonPosition = Vector3.zero;

	[Header("ボタン")]
	[SerializeField]
	private UIButton button;

	[Header("スキル名")]
	[SerializeField]
	private UILabel skillName;

	[SerializeField]
	[Header("スキル説明")]
	private UILabel skillDescription;

	[SerializeField]
	[Header("必要AP数")]
	private UILabel ap;

	[Header("命中率")]
	[SerializeField]
	private UILabel hitRate;

	[SerializeField]
	[Header("威力")]
	private UILabel power;

	[SerializeField]
	[Header("Tweener1（開く）")]
	private UITweener rotationEffect1;

	[SerializeField]
	[Header("Tweener2（閉じる）")]
	private UITweener rotationEffect2;

	[Header("Collider")]
	[SerializeField]
	private Collider colliderValue;

	[SerializeField]
	[Header("HoldPress")]
	private HoldPressButton skillDescriptionSwitch;

	[Header("属性アイコン")]
	[SerializeField]
	private UISprite attributeSprite;

	[SerializeField]
	[Header("スキル説明UIのルート")]
	private GameObject skillDescriptionRoot;

	[Header("スキルロックアイコン")]
	[SerializeField]
	private UISprite skillLockSprite;

	[SerializeField]
	[Header("スキルONボタン")]
	private GameObject onSkillButton;

	[Header("実行ボタンスプライト")]
	[SerializeField]
	private UISprite execButtonSprite;

	[SerializeField]
	[Header("スキルOFfボタン")]
	private GameObject offSkillButton;

	[SerializeField]
	[Header("スキルボタンスプライト")]
	private UISprite skillButtonSprite;

	private SkillType skillType;

	public void Awake()
	{
		this.execButtonSprite.spriteName = string.Format("{0}_{1}", this.execButtonSprite.spriteName, CountrySetting.GetCountryPrefix(CountrySetting.CountryCode.EN));
	}

	public void ApplySkillButtonData(SkillStatus skills, bool onEnable, bool onSkillLock, CharacterStateControl selectCharacter)
	{
		List<ExtraEffectStatus> extraEffectStatus = BattleStateManager.current.battleStateData.extraEffectStatus;
		List<ExtraEffectStatus> invocationList = ExtraEffectStatus.GetInvocationList(extraEffectStatus, EffectStatusBase.EffectTriggerType.Usually);
		AffectEffectProperty affectEffectProperty = skills.GetAffectEffectFirst();
		foreach (AffectEffectProperty affectEffectProperty2 in skills.affectEffect)
		{
			if (affectEffectProperty2.ThisSkillIsAttack)
			{
				affectEffectProperty = affectEffectProperty2;
				break;
			}
		}
		int num = 0;
		if (AffectEffectProperty.IsDamage(affectEffectProperty.type))
		{
			num = ExtraEffectStatus.GetSkillPowerCorrectionValue(invocationList, affectEffectProperty, selectCharacter);
		}
		float num2 = ExtraEffectStatus.GetSkillHitRateCorrectionValue(invocationList, affectEffectProperty, selectCharacter);
		num2 = Mathf.Clamp01(num2);
		this.skillType = skills.skillType;
		this.skillName.text = skills.name;
		this.skillDescription.text = skills.description;
		int num3 = (int)Math.Round((double)(num2 * 100f), MidpointRounding.AwayFromZero);
		string @string = StringMaster.GetString("SystemPercent");
		this.hitRate.text = string.Format(@string, num3);
		this.power.text = num.ToString();
		int correctedAp = skills.GetCorrectedAp(selectCharacter);
		if (correctedAp <= selectCharacter.ap)
		{
			this.ap.text = string.Format(StringMaster.GetString("BattleSkillUI-01"), correctedAp);
		}
		else
		{
			this.ap.text = StringMaster.GetString("BattleSkillUI-02");
		}
		this.SetAttributeIcon(skills.attribute);
		if (onSkillLock)
		{
			this.SetButtonType(BattleSkillBtn.Type.Invalid);
			this.SetSkillLock(onSkillLock);
		}
		else if (onEnable)
		{
			this.SetButtonType(BattleSkillBtn.Type.Off);
			this.SetSkillLock(false);
		}
		else
		{
			this.SetButtonType(BattleSkillBtn.Type.Invalid);
			this.SetSkillLock(false);
		}
	}

	public void SetColliderActive(bool active)
	{
		if (this.colliderValue != null)
		{
			this.colliderValue.enabled = active;
		}
	}

	public void Reflesh()
	{
		this.rotationEffect2.enabled = false;
		this.rotationEffect2.ResetToBeginning();
		this.rotationEffect2.tweenFactor = 0f;
		this.rotationEffect1.enabled = false;
		this.rotationEffect1.ResetToBeginning();
		this.rotationEffect1.tweenFactor = 0f;
		this.rotationEffect1.transform.localScale = Vector3.one;
		this.rotationEffect2.transform.localScale = Vector3.one;
		this.SetColliderActive(true);
		this.SetButtonType(BattleSkillBtn.Type.Off);
	}

	public void ApplySkillDescriptionEnable(bool onEnable)
	{
		if (this.skillDescriptionRoot != null)
		{
			this.skillDescriptionRoot.SetActive(onEnable);
		}
	}

	public void SetButtonCallback(Action<int> callback, int value)
	{
		BattleInputUtility.AddEvent(this.button.onClick, callback, value);
	}

	public void SetClickCallback(Action<int> callback, int value)
	{
		BattleInputUtility.AddEvent(this.skillDescriptionSwitch.onClick, callback, value);
	}

	public void SetHoldWaitPressCallback(Action<int> callback, int value)
	{
		BattleInputUtility.AddEvent(this.skillDescriptionSwitch.onHoldWaitPress, callback, value);
	}

	public void SetDisengagePressCallback(Action<int> callback, int value)
	{
		BattleInputUtility.AddEvent(this.skillDescriptionSwitch.onDisengagePress, callback, value);
	}

	public void SetRotationEffectCallback(Action<int> callback, int value)
	{
		UITweenEventSystem component = this.rotationEffect1.gameObject.GetComponent<UITweenEventSystem>();
		BattleInputUtility.AddEvent(component.onFinished, callback, value);
		UITweenEventSystem component2 = this.rotationEffect2.gameObject.GetComponent<UITweenEventSystem>();
		BattleInputUtility.AddEvent(component2.onFinished, callback, value);
	}

	public void PlayOpenRotationEffect()
	{
		this.rotationEffect1.enabled = true;
		this.rotationEffect1.PlayForward();
	}

	public void PlayCloseRotationEffect()
	{
		this.rotationEffect2.enabled = true;
		this.rotationEffect2.PlayForward();
	}

	public void SetButtonType(BattleSkillBtn.Type type)
	{
		switch (type)
		{
		case BattleSkillBtn.Type.On:
			this.onSkillButton.SetActive(true);
			this.offSkillButton.SetActive(false);
			if (this.skillType == SkillType.Attack)
			{
				this.skillButtonSprite.spriteName = "Battle_Attackbtn";
			}
			else
			{
				this.skillButtonSprite.spriteName = "Battle_Skillbtn";
			}
			break;
		case BattleSkillBtn.Type.Off:
			this.onSkillButton.SetActive(false);
			this.offSkillButton.SetActive(true);
			if (this.skillType == SkillType.Attack)
			{
				this.skillButtonSprite.spriteName = "Battle_Attackbtn";
			}
			else
			{
				this.skillButtonSprite.spriteName = "Battle_Skillbtn";
			}
			break;
		case BattleSkillBtn.Type.Invalid:
			this.onSkillButton.SetActive(false);
			this.offSkillButton.SetActive(true);
			this.skillButtonSprite.spriteName = "Battle_Skillbtn_g";
			break;
		}
	}

	private void SetAttributeIcon(global::Attribute attribute)
	{
		if (this.attributeSprite == null)
		{
			return;
		}
		switch (attribute)
		{
		case global::Attribute.None:
			this.attributeSprite.spriteName = "Battle_Attribute_1";
			break;
		case global::Attribute.Red:
			this.attributeSprite.spriteName = "Battle_Attribute_2";
			break;
		case global::Attribute.Blue:
			this.attributeSprite.spriteName = "Battle_Attribute_3";
			break;
		case global::Attribute.Yellow:
			this.attributeSprite.spriteName = "Battle_Attribute_4";
			break;
		case global::Attribute.Green:
			this.attributeSprite.spriteName = "Battle_Attribute_5";
			break;
		case global::Attribute.White:
			this.attributeSprite.spriteName = "Battle_Attribute_6";
			break;
		case global::Attribute.Black:
			this.attributeSprite.spriteName = "Battle_Attribute_7";
			break;
		}
	}

	private void SetSkillLock(bool islock)
	{
		if (this.skillLockSprite != null)
		{
			this.skillLockSprite.gameObject.SetActive(islock);
		}
	}

	public void ApplyTwoButtonPosition()
	{
		base.transform.localPosition = this.twoButtonPosition;
	}

	public void ApplyThreeButtonPosition()
	{
		base.transform.localPosition = this.threeButtonPosition;
	}

	public enum Type
	{
		On,
		Off,
		Invalid
	}
}
