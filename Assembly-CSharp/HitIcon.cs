using BattleStateMachineInternal;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HitIcon : MonoBehaviour
{
	private const string MISS_NAME = "Battle_text_MISS";

	private const string WEAK_NAME = "Battle_text_Preeminent";

	private const string STRONG_NAME = "Battle_text_Half";

	private const string CRITICAL_NAME = "Battle_text_CRITICAL";

	private const string INVALID_NAME = "Battle_text_Invalid";

	[SerializeField]
	private UITweener[] tween;

	[SerializeField]
	private HitIcon.Data standard = new HitIcon.Data();

	[SerializeField]
	private HitIcon.Data gimmick = new HitIcon.Data();

	[SerializeField]
	private List<UIFont> fontList = new List<UIFont>();

	private static Dictionary<AffectEffect, string> dictionary;

	private void Awake()
	{
		if (HitIcon.dictionary == null)
		{
			HitIcon.dictionary = new Dictionary<AffectEffect, string>();
			HitIcon.dictionary.Add(AffectEffect.Damage, string.Empty);
			HitIcon.dictionary.Add(AffectEffect.AttackUp, "Battle_text_ATKUP");
			HitIcon.dictionary.Add(AffectEffect.AttackDown, "Battle_text_ATKDOWN");
			HitIcon.dictionary.Add(AffectEffect.DefenceUp, "Battle_text_DEFUP");
			HitIcon.dictionary.Add(AffectEffect.DefenceDown, "Battle_text_DEFDOWN");
			HitIcon.dictionary.Add(AffectEffect.SpAttackUp, "Battle_text_S_ATKUP");
			HitIcon.dictionary.Add(AffectEffect.SpAttackDown, "Battle_text_S_ATKDOWN");
			HitIcon.dictionary.Add(AffectEffect.SpDefenceUp, "Battle_text_S_DEFUP");
			HitIcon.dictionary.Add(AffectEffect.SpDefenceDown, "Battle_text_S_DEFDOWN");
			HitIcon.dictionary.Add(AffectEffect.SpeedUp, "Battle_text_spdup");
			HitIcon.dictionary.Add(AffectEffect.SpeedDown, "Battle_text_spddown");
			HitIcon.dictionary.Add(AffectEffect.CorrectionUpReset, "Battle_text_UPClear");
			HitIcon.dictionary.Add(AffectEffect.CorrectionDownReset, "Battle_text_DOWNClear");
			HitIcon.dictionary.Add(AffectEffect.HpRevival, "Battle_text_hp");
			HitIcon.dictionary.Add(AffectEffect.Counter, "Battle_text_Counter");
			HitIcon.dictionary.Add(AffectEffect.Reflection, "Battle_text_Reflection");
			HitIcon.dictionary.Add(AffectEffect.Protect, "Battle_text_protected");
			HitIcon.dictionary.Add(AffectEffect.HateUp, "Battle_text_HaightUP");
			HitIcon.dictionary.Add(AffectEffect.HateDown, "Battle_text_HaightDOWN");
			HitIcon.dictionary.Add(AffectEffect.PowerCharge, "Battle_text_power2");
			HitIcon.dictionary.Add(AffectEffect.Destruct, "Battle_text_Destruction");
			HitIcon.dictionary.Add(AffectEffect.Paralysis, "Battle_text_Numbness");
			HitIcon.dictionary.Add(AffectEffect.Poison, "Battle_text_Poison");
			HitIcon.dictionary.Add(AffectEffect.Sleep, "Battle_text_Sleep");
			HitIcon.dictionary.Add(AffectEffect.SkillLock, "Battle_text_Skill_lock");
			HitIcon.dictionary.Add(AffectEffect.HitRateUp, "Battle_text_HitUP");
			HitIcon.dictionary.Add(AffectEffect.HitRateDown, "Battle_text_HitDOWN");
			HitIcon.dictionary.Add(AffectEffect.InstantDeath, "Battle_text_death");
			HitIcon.dictionary.Add(AffectEffect.Confusion, "Battle_text_Confusion");
			HitIcon.dictionary.Add(AffectEffect.Stun, "Battle_text_Stun");
			HitIcon.dictionary.Add(AffectEffect.SufferStatusClear, "Battle_text_Abnormal_Clear");
			HitIcon.dictionary.Add(AffectEffect.SatisfactionRateUp, "Battle_text_crtup");
			HitIcon.dictionary.Add(AffectEffect.SatisfactionRateDown, "Battle_text_crtdown");
			HitIcon.dictionary.Add(AffectEffect.ApRevival, "Battle_text_apb");
			HitIcon.dictionary.Add(AffectEffect.ApUp, "Battle_text_apup");
			HitIcon.dictionary.Add(AffectEffect.ApDown, "Battle_text_apdown");
			HitIcon.dictionary.Add(AffectEffect.ApConsumptionUp, "Battle_text_apcostup");
			HitIcon.dictionary.Add(AffectEffect.ApConsumptionDown, "Battle_text_apcostdown");
			HitIcon.dictionary.Add(AffectEffect.CountGuard, string.Empty);
			HitIcon.dictionary.Add(AffectEffect.TurnBarrier, "Battle_text_invincible");
			HitIcon.dictionary.Add(AffectEffect.CountBarrier, "Battle_text_invincible");
			HitIcon.dictionary.Add(AffectEffect.Invalid, "Battle_text_Invalid");
			HitIcon.dictionary.Add(AffectEffect.DamageRateUp, string.Empty);
			HitIcon.dictionary.Add(AffectEffect.DamageRateDown, string.Empty);
			HitIcon.dictionary.Add(AffectEffect.Regenerate, "Battle_text_hp");
			HitIcon.dictionary.Add(AffectEffect.TurnEvasion, "Battle_text_Avoid");
			HitIcon.dictionary.Add(AffectEffect.CountEvasion, "Battle_text_Avoid");
			HitIcon.dictionary.Add(AffectEffect.ApDrain, "Battle_text_ApDrain");
			HitIcon.dictionary.Add(AffectEffect.gimmickSpecialAttackUp, "Battle_text_gimi1");
			HitIcon.dictionary.Add(AffectEffect.gimmickSpecialAttackDown, "Battle_text_gimi2");
		}
	}

	public void HitIconReposition(Vector3 position)
	{
		BattleUIComponents battleUiComponents = BattleStateManager.current.battleUiComponents;
		Vector3 vector = battleUiComponents.uiCamera.ViewportToWorldPoint(position);
		base.transform.position = new Vector3(vector.x, vector.y, 0f);
	}

	public void ApplyShowHitIcon(AffectEffect affect, int onDamage, Strength onWeak, bool onMiss, bool onCrithical, bool isDrain, bool isRecoil, ExtraEffectType extraEffectType = ExtraEffectType.Non)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string text4 = string.Empty;
		bool flag = false;
		HitIcon.FontType index = HitIcon.FontType.Normal;
		this.ApplyHitIconPlayAnimation(onWeak);
		if (onMiss)
		{
			text2 = "Battle_text_MISS";
		}
		else
		{
			if (extraEffectType != ExtraEffectType.Non)
			{
				flag = true;
				if (extraEffectType == ExtraEffectType.Up)
				{
					text2 = HitIcon.dictionary[AffectEffect.gimmickSpecialAttackUp];
					index = HitIcon.FontType.Gimmick;
				}
				else
				{
					text2 = HitIcon.dictionary[AffectEffect.gimmickSpecialAttackDown];
					index = HitIcon.FontType.Gimmick2;
				}
			}
			switch (affect)
			{
			case AffectEffect.Damage:
				text4 = onDamage.ToString();
				if (isDrain)
				{
					text3 = HitIcon.dictionary[affect];
					index = HitIcon.FontType.Drain;
				}
				else
				{
					if (onCrithical)
					{
						text3 = "Battle_text_CRITICAL";
					}
					text = this.GetWeakName(onWeak);
					index = this.GetWeakFontType(onWeak);
				}
				goto IL_2C2;
			case AffectEffect.AttackUp:
			case AffectEffect.AttackDown:
			case AffectEffect.DefenceUp:
			case AffectEffect.DefenceDown:
			case AffectEffect.SpAttackUp:
			case AffectEffect.SpAttackDown:
			case AffectEffect.SpDefenceUp:
			case AffectEffect.SpDefenceDown:
			case AffectEffect.SpeedUp:
			case AffectEffect.SpeedDown:
			case AffectEffect.CorrectionUpReset:
			case AffectEffect.CorrectionDownReset:
			case AffectEffect.Protect:
			case AffectEffect.PowerCharge:
			case AffectEffect.Destruct:
			case AffectEffect.HitRateUp:
			case AffectEffect.HitRateDown:
			case AffectEffect.SufferStatusClear:
			case AffectEffect.SatisfactionRateUp:
			case AffectEffect.SatisfactionRateDown:
			case AffectEffect.ApRevival:
			case AffectEffect.ApUp:
			case AffectEffect.ApDown:
			case AffectEffect.ApConsumptionUp:
			case AffectEffect.ApConsumptionDown:
			case AffectEffect.TurnBarrier:
			case AffectEffect.CountBarrier:
			case AffectEffect.Invalid:
			case AffectEffect.DamageRateUp:
			case AffectEffect.DamageRateDown:
			case AffectEffect.TurnEvasion:
			case AffectEffect.CountEvasion:
				text2 = HitIcon.dictionary[affect];
				goto IL_2C2;
			case AffectEffect.HpRevival:
				text3 = HitIcon.dictionary[affect];
				text4 = onDamage.ToString();
				index = HitIcon.FontType.Drain;
				goto IL_2C2;
			case AffectEffect.Counter:
			case AffectEffect.Reflection:
				if (isRecoil)
				{
					text4 = onDamage.ToString();
				}
				text3 = HitIcon.dictionary[affect];
				index = HitIcon.FontType.Crithical;
				goto IL_2C2;
			case AffectEffect.Paralysis:
			case AffectEffect.Sleep:
			case AffectEffect.SkillLock:
			case AffectEffect.InstantDeath:
			case AffectEffect.Confusion:
			case AffectEffect.Stun:
				text2 = HitIcon.dictionary[affect];
				goto IL_2C2;
			case AffectEffect.Poison:
				if (onDamage > 0)
				{
					BattleStateUIProperty uiProperty = BattleStateManager.current.uiProperty;
					base.transform.localScale = uiProperty.hitIconLocalScale * uiProperty.onPoisonScalingSizeHitIcon;
					text4 = onDamage.ToString();
					text3 = HitIcon.dictionary[affect];
					index = HitIcon.FontType.Poison;
				}
				else
				{
					text2 = HitIcon.dictionary[affect];
				}
				goto IL_2C2;
			case AffectEffect.Regenerate:
				if (onDamage > 0)
				{
					BattleStateUIProperty uiProperty2 = BattleStateManager.current.uiProperty;
					base.transform.localScale = uiProperty2.hitIconLocalScale * uiProperty2.onPoisonScalingSizeHitIcon;
					text4 = onDamage.ToString();
					text3 = HitIcon.dictionary[affect];
					index = HitIcon.FontType.Drain;
				}
				else
				{
					text2 = HitIcon.dictionary[affect];
				}
				goto IL_2C2;
			case AffectEffect.ReferenceTargetHpRate:
				text4 = onDamage.ToString();
				goto IL_2C2;
			}
			NGUITools.SetActiveSelf(base.gameObject, false);
		}
		IL_2C2:
		HitIcon.Data data = (!flag) ? this.standard : this.gimmick;
		this.standard.gameObject.SetActive(!flag);
		this.gimmick.gameObject.SetActive(flag);
		data.top.spriteName = text;
		data.middle.spriteName = text2;
		data.bottom.spriteName = text3;
		data.num.text = text4;
		if (!string.IsNullOrEmpty(text4))
		{
			data.num.bitmapFont = this.fontList[(int)index];
		}
		if (!string.IsNullOrEmpty(text))
		{
			data.top.keepAspectRatio = UIWidget.AspectRatioSource.Free;
			data.top.MakePixelPerfect();
		}
		if (!string.IsNullOrEmpty(text2))
		{
			data.middle.keepAspectRatio = UIWidget.AspectRatioSource.Free;
			data.middle.MakePixelPerfect();
		}
		if (!string.IsNullOrEmpty(text3))
		{
			data.bottom.keepAspectRatio = UIWidget.AspectRatioSource.Free;
			data.bottom.MakePixelPerfect();
		}
	}

	public void ApplyHitIconPlayAnimation(Strength power = Strength.None)
	{
		foreach (UITweener uitweener in this.tween)
		{
			uitweener.ResetToBeginning();
			uitweener.enabled = false;
			uitweener.transform.localScale = Vector3.one;
		}
		UITweener uitweener2;
		switch (power)
		{
		case Strength.Strong:
			uitweener2 = this.tween[1];
			break;
		case Strength.Weak:
			uitweener2 = this.tween[2];
			break;
		case Strength.Invalid:
			uitweener2 = this.tween[1];
			break;
		default:
			uitweener2 = this.tween[0];
			break;
		}
		uitweener2.ResetToBeginning();
		uitweener2.enabled = true;
		uitweener2.PlayForward();
	}

	private string GetWeakName(Strength onWeak)
	{
		switch (onWeak)
		{
		case Strength.None:
			return string.Empty;
		case Strength.Strong:
			return "Battle_text_Half";
		case Strength.Weak:
			return "Battle_text_Preeminent";
		case Strength.Invalid:
			return "Battle_text_Half";
		default:
			return string.Empty;
		}
	}

	private HitIcon.FontType GetWeakFontType(Strength onWeak)
	{
		switch (onWeak)
		{
		case Strength.None:
			return HitIcon.FontType.Normal;
		case Strength.Strong:
			return HitIcon.FontType.Minus;
		case Strength.Weak:
			return HitIcon.FontType.Plus;
		case Strength.Invalid:
			return HitIcon.FontType.Minus;
		default:
			return HitIcon.FontType.Normal;
		}
	}

	private enum FontType
	{
		Normal,
		Plus,
		Minus,
		Poison,
		Drain,
		Crithical,
		Gimmick,
		Gimmick2
	}

	[Serializable]
	public class Data
	{
		public GameObject gameObject;

		public UILabel num;

		public UISprite top;

		public UISprite middle;

		public UISprite bottom;
	}
}
