using System;
using UnityEngine;

public class UIPublicResources : ScriptableObject
{
	[SerializeField]
	private Sprite[] attributeSprites = new Sprite[7];

	[SerializeField]
	private Sprite[] attributeSpritesDisable = new Sprite[7];

	[SerializeField]
	private Sprite[] attributeSpritesNormal = new Sprite[7];

	[SerializeField]
	private Color[] attributeColors = new Color[]
	{
		Color.white,
		Color.red,
		Color.blue,
		Color.green,
		Color.yellow,
		Color.black
	};

	[SerializeField]
	private Sprite[] auxiliaryEffectIcons = new Sprite[32];

	public static UIPublicResources Get()
	{
		return Resources.Load<UIPublicResources>("UIPublicResources");
	}

	public Sprite GetAttributeSprite(global::Attribute attribute, bool enable = true, bool onNormal = false)
	{
		switch (attribute)
		{
		case global::Attribute.None:
			if (enable && !onNormal && this.attributeSprites[0] != null)
			{
				return this.attributeSprites[0];
			}
			if (enable && this.attributeSpritesNormal[0] != null)
			{
				return this.attributeSpritesNormal[0];
			}
			if (this.attributeSpritesDisable[0] != null)
			{
				return this.attributeSpritesDisable[0];
			}
			break;
		case global::Attribute.Red:
			if (enable && !onNormal && this.attributeSprites[1] != null)
			{
				return this.attributeSprites[1];
			}
			if (enable && this.attributeSpritesNormal[1] != null)
			{
				return this.attributeSpritesNormal[1];
			}
			if (this.attributeSpritesDisable[1] != null)
			{
				return this.attributeSpritesDisable[1];
			}
			break;
		case global::Attribute.Blue:
			if (enable && !onNormal && this.attributeSprites[2] != null)
			{
				return this.attributeSprites[2];
			}
			if (enable && this.attributeSpritesNormal[2] != null)
			{
				return this.attributeSpritesNormal[2];
			}
			if (this.attributeSpritesDisable[2] != null)
			{
				return this.attributeSpritesDisable[2];
			}
			break;
		case global::Attribute.Yellow:
			if (enable && !onNormal && this.attributeSprites[3] != null)
			{
				return this.attributeSprites[3];
			}
			if (enable && this.attributeSpritesNormal[3] != null)
			{
				return this.attributeSpritesNormal[3];
			}
			if (this.attributeSpritesDisable[3] != null)
			{
				return this.attributeSpritesDisable[3];
			}
			break;
		case global::Attribute.Green:
			if (enable && !onNormal && this.attributeSprites[4] != null)
			{
				return this.attributeSprites[4];
			}
			if (enable && this.attributeSpritesNormal[4] != null)
			{
				return this.attributeSpritesNormal[4];
			}
			if (this.attributeSpritesDisable[4] != null)
			{
				return this.attributeSpritesDisable[4];
			}
			break;
		case global::Attribute.White:
			if (enable && !onNormal && this.attributeSprites[5] != null)
			{
				return this.attributeSprites[5];
			}
			if (enable && this.attributeSpritesNormal[5] != null)
			{
				return this.attributeSpritesNormal[5];
			}
			if (this.attributeSpritesDisable[5] != null)
			{
				return this.attributeSpritesDisable[5];
			}
			break;
		case global::Attribute.Black:
			if (enable && !onNormal && this.attributeSprites[6] != null)
			{
				return this.attributeSprites[6];
			}
			if (enable && this.attributeSpritesNormal[6] != null)
			{
				return this.attributeSpritesNormal[6];
			}
			if (this.attributeSpritesDisable[6] != null)
			{
				return this.attributeSpritesDisable[6];
			}
			break;
		}
		return null;
	}

	public Color GetAttributeColor(global::Attribute attribute)
	{
		switch (attribute)
		{
		case global::Attribute.None:
			return this.attributeColors[0];
		case global::Attribute.Red:
			return this.attributeColors[1];
		case global::Attribute.Blue:
			return this.attributeColors[2];
		case global::Attribute.Yellow:
			return this.attributeColors[3];
		case global::Attribute.Green:
			return this.attributeColors[4];
		case global::Attribute.White:
			return this.attributeColors[5];
		case global::Attribute.Black:
			return this.attributeColors[4];
		default:
			return Color.white;
		}
	}

	public Sprite GetAffectEffectIcon(AffectEffect auxiliaryEffect)
	{
		switch (auxiliaryEffect)
		{
		case AffectEffect.AttackUp:
			return this.auxiliaryEffectIcons[0];
		case AffectEffect.AttackDown:
			return this.auxiliaryEffectIcons[1];
		case AffectEffect.DefenceUp:
			return this.auxiliaryEffectIcons[2];
		case AffectEffect.DefenceDown:
			return this.auxiliaryEffectIcons[3];
		case AffectEffect.SpAttackUp:
			return this.auxiliaryEffectIcons[4];
		case AffectEffect.SpAttackDown:
			return this.auxiliaryEffectIcons[5];
		case AffectEffect.SpDefenceUp:
			return this.auxiliaryEffectIcons[6];
		case AffectEffect.SpDefenceDown:
			return this.auxiliaryEffectIcons[7];
		case AffectEffect.SpeedUp:
			return this.auxiliaryEffectIcons[8];
		case AffectEffect.SpeedDown:
			return this.auxiliaryEffectIcons[10];
		case AffectEffect.CorrectionUpReset:
			return this.auxiliaryEffectIcons[11];
		case AffectEffect.CorrectionDownReset:
			return this.auxiliaryEffectIcons[12];
		case AffectEffect.HpRevival:
			return this.auxiliaryEffectIcons[13];
		case AffectEffect.Counter:
			return this.auxiliaryEffectIcons[14];
		case AffectEffect.Reflection:
			return this.auxiliaryEffectIcons[15];
		case AffectEffect.Protect:
			return this.auxiliaryEffectIcons[16];
		case AffectEffect.HateUp:
			return this.auxiliaryEffectIcons[17];
		case AffectEffect.HateDown:
			return this.auxiliaryEffectIcons[18];
		case AffectEffect.PowerCharge:
			return this.auxiliaryEffectIcons[19];
		case AffectEffect.Destruct:
			return this.auxiliaryEffectIcons[20];
		case AffectEffect.Paralysis:
			return this.auxiliaryEffectIcons[21];
		case AffectEffect.Poison:
			return this.auxiliaryEffectIcons[22];
		case AffectEffect.Sleep:
			return this.auxiliaryEffectIcons[24];
		case AffectEffect.SkillLock:
			return this.auxiliaryEffectIcons[25];
		case AffectEffect.HitRateUp:
			return this.auxiliaryEffectIcons[26];
		case AffectEffect.HitRateDown:
			return this.auxiliaryEffectIcons[27];
		case AffectEffect.InstantDeath:
			return this.auxiliaryEffectIcons[28];
		case AffectEffect.Confusion:
			return this.auxiliaryEffectIcons[29];
		case AffectEffect.Stun:
			return this.auxiliaryEffectIcons[30];
		case AffectEffect.SatisfactionRateUp:
			return this.auxiliaryEffectIcons[9];
		case AffectEffect.SatisfactionRateDown:
			return this.auxiliaryEffectIcons[31];
		}
		return null;
	}

	public Sprite GetSufferStatusIcon(SufferStateProperty.SufferType sufferType)
	{
		switch (sufferType)
		{
		case SufferStateProperty.SufferType.Poison:
			if (this.auxiliaryEffectIcons[22] != null)
			{
				return this.auxiliaryEffectIcons[22];
			}
			break;
		case SufferStateProperty.SufferType.Confusion:
			if (this.auxiliaryEffectIcons[29] != null)
			{
				return this.auxiliaryEffectIcons[29];
			}
			break;
		case SufferStateProperty.SufferType.Paralysis:
			if (this.auxiliaryEffectIcons[21] != null)
			{
				return this.auxiliaryEffectIcons[21];
			}
			break;
		case SufferStateProperty.SufferType.Sleep:
			if (this.auxiliaryEffectIcons[24] != null)
			{
				return this.auxiliaryEffectIcons[24];
			}
			break;
		case SufferStateProperty.SufferType.Stun:
			if (this.auxiliaryEffectIcons[30] != null)
			{
				return this.auxiliaryEffectIcons[30];
			}
			break;
		case SufferStateProperty.SufferType.SkillLock:
			if (this.auxiliaryEffectIcons[25] != null)
			{
				return this.auxiliaryEffectIcons[25];
			}
			break;
		case SufferStateProperty.SufferType.AttackUp:
			if (this.auxiliaryEffectIcons[0] != null)
			{
				return this.auxiliaryEffectIcons[0];
			}
			break;
		case SufferStateProperty.SufferType.AttackDown:
			if (this.auxiliaryEffectIcons[1] != null)
			{
				return this.auxiliaryEffectIcons[1];
			}
			break;
		case SufferStateProperty.SufferType.DefenceUp:
			if (this.auxiliaryEffectIcons[2] != null)
			{
				return this.auxiliaryEffectIcons[2];
			}
			break;
		case SufferStateProperty.SufferType.DefenceDown:
			if (this.auxiliaryEffectIcons[3] != null)
			{
				return this.auxiliaryEffectIcons[3];
			}
			break;
		case SufferStateProperty.SufferType.SpAttackUp:
			if (this.auxiliaryEffectIcons[4] != null)
			{
				return this.auxiliaryEffectIcons[4];
			}
			break;
		case SufferStateProperty.SufferType.SpAttackDown:
			if (this.auxiliaryEffectIcons[5] != null)
			{
				return this.auxiliaryEffectIcons[5];
			}
			break;
		case SufferStateProperty.SufferType.SpDefenceUp:
			if (this.auxiliaryEffectIcons[6] != null)
			{
				return this.auxiliaryEffectIcons[6];
			}
			break;
		case SufferStateProperty.SufferType.SpDefenceDown:
			if (this.auxiliaryEffectIcons[7] != null)
			{
				return this.auxiliaryEffectIcons[7];
			}
			break;
		case SufferStateProperty.SufferType.SpeedUp:
			if (this.auxiliaryEffectIcons[8] != null)
			{
				return this.auxiliaryEffectIcons[8];
			}
			break;
		case SufferStateProperty.SufferType.SpeedDown:
			if (this.auxiliaryEffectIcons[9] != null)
			{
				return this.auxiliaryEffectIcons[9];
			}
			break;
		case SufferStateProperty.SufferType.Counter:
			if (this.auxiliaryEffectIcons[14] != null)
			{
				return this.auxiliaryEffectIcons[14];
			}
			break;
		case SufferStateProperty.SufferType.Reflection:
			if (this.auxiliaryEffectIcons[15] != null)
			{
				return this.auxiliaryEffectIcons[15];
			}
			break;
		case SufferStateProperty.SufferType.Protect:
			if (this.auxiliaryEffectIcons[16] != null)
			{
				return this.auxiliaryEffectIcons[16];
			}
			break;
		case SufferStateProperty.SufferType.PowerCharge:
			if (this.auxiliaryEffectIcons[19] != null)
			{
				return this.auxiliaryEffectIcons[19];
			}
			break;
		case SufferStateProperty.SufferType.HitRateUp:
			if (this.auxiliaryEffectIcons[26] != null)
			{
				return this.auxiliaryEffectIcons[26];
			}
			break;
		case SufferStateProperty.SufferType.HitRateDown:
			if (this.auxiliaryEffectIcons[27] != null)
			{
				return this.auxiliaryEffectIcons[27];
			}
			break;
		case SufferStateProperty.SufferType.SatisfactionRateUp:
			if (this.auxiliaryEffectIcons[9] != null)
			{
				return this.auxiliaryEffectIcons[9];
			}
			break;
		case SufferStateProperty.SufferType.SatisfactionRateDown:
			if (this.auxiliaryEffectIcons[31] != null)
			{
				return this.auxiliaryEffectIcons[31];
			}
			break;
		}
		return null;
	}
}
