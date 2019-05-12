using BattleStateMachineInternal;
using Master;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitIcon : MonoBehaviour
{
	[SerializeField]
	[Header("アニメーション 通常 有利 不利 の順")]
	private UITweener[] tween;

	[SerializeField]
	[Header("通常のUI")]
	private HitIcon.Data standard = new HitIcon.Data();

	[Header("ステージ効果用のUI")]
	[SerializeField]
	private HitIcon.Data gimmick = new HitIcon.Data();

	[SerializeField]
	[Header("耐性結果を表示するフォントテクスチャ")]
	private HitIcon.ResistanceFontTexture resistanceFontTexture;

	[SerializeField]
	[Header("通常効果を表示するフォントテクスチャ")]
	private HitIcon.StandardEffectFontTexture standardEffectFontTexture;

	private MaterialPropertyBlock materialPropertyBlock;

	private Dictionary<string, string> dictionary;

	[SerializeField]
	[Header("ステージ効果上昇UI")]
	private UISprite upSprite;

	[SerializeField]
	[Header("ステージ効果減少UI")]
	private UISprite downSprite;

	[Header("国内用フォントデータ")]
	[SerializeField]
	private HitIcon.LanguageFont languageFont;

	[SerializeField]
	[Header("海外用フォントデータ")]
	private HitIcon.LanguageFont usLanguageFont;

	private void Awake()
	{
		this.dictionary = new Dictionary<string, string>();
		this.materialPropertyBlock = new MaterialPropertyBlock();
		this.standard.numMesh.font = this.usLanguageFont.num.font;
		this.standard.topMesh.font = this.usLanguageFont.resistance.font;
		this.standard.bottomMesh.font = this.usLanguageFont.standard.font;
		this.standard.middleMesh.font = this.usLanguageFont.standard.font;
		this.standard.numMesh.fontSharedMaterial = this.usLanguageFont.num.material;
		this.standard.topMesh.fontSharedMaterial = this.usLanguageFont.resistance.material;
		this.standard.bottomMesh.fontSharedMaterial = this.usLanguageFont.standard.material;
		this.standard.middleMesh.fontSharedMaterial = this.usLanguageFont.standard.material;
	}

	public void HitIconReposition(Vector3 position)
	{
		BattleUIComponents battleUiComponents = BattleStateManager.current.battleUiComponents;
		Vector3 vector = battleUiComponents.uiCamera.ViewportToWorldPoint(position);
		base.transform.position = new Vector3(vector.x, vector.y, 0f);
	}

	public void ApplyShowHitIcon(AffectEffect affect, int onDamage, Strength onWeak, bool onMiss, bool onCrithical, bool isDrain, bool isCounter, bool isReflection, ExtraEffectType extraEffectType = ExtraEffectType.Non)
	{
		this.ApplyHitIconPlayAnimation(onWeak);
		bool flag = false;
		HitIcon.Data data = (!flag) ? this.standard : this.gimmick;
		this.standard.gameObject.SetActive(!flag);
		this.gimmick.gameObject.SetActive(flag);
		base.transform.localScale = Vector3.one;
		data.numMesh.text = string.Empty;
		data.topMesh.text = string.Empty;
		data.middleMesh.text = string.Empty;
		data.bottomMesh.text = string.Empty;
		this.upSprite.gameObject.SetActive(false);
		this.downSprite.gameObject.SetActive(false);
		if (onMiss)
		{
			this.ShowMiss(data);
		}
		else
		{
			switch (affect)
			{
			case AffectEffect.Damage:
			case AffectEffect.ReferenceTargetHpRate:
			case AffectEffect.HpBorderlineDamage:
			case AffectEffect.HpBorderlineSpDamage:
			case AffectEffect.DefenseThroughDamage:
			case AffectEffect.SpDefenseThroughDamage:
				this.ShowDamage(data, onDamage, onWeak, onCrithical, isCounter, isReflection, extraEffectType);
				return;
			case AffectEffect.AttackUp:
				this.ShowAttackUp(data);
				return;
			case AffectEffect.AttackDown:
				this.ShowAttackDown(data);
				return;
			case AffectEffect.DefenceUp:
				this.ShowDefenceUp(data);
				return;
			case AffectEffect.DefenceDown:
				this.ShowDefenceDown(data);
				return;
			case AffectEffect.SpAttackUp:
				this.ShowSpAttackUp(data);
				return;
			case AffectEffect.SpAttackDown:
				this.ShowSpAttackDown(data);
				return;
			case AffectEffect.SpDefenceUp:
				this.ShowSpDefenceUp(data);
				return;
			case AffectEffect.SpDefenceDown:
				this.ShowSpDefenceDown(data);
				return;
			case AffectEffect.SpeedUp:
				this.ShowSpeedUp(data);
				return;
			case AffectEffect.SpeedDown:
				this.ShowSpeedDown(data);
				return;
			case AffectEffect.CorrectionUpReset:
				this.ShowCorrectionUpReset(data);
				return;
			case AffectEffect.CorrectionDownReset:
				this.ShowCorrectionDownReset(data);
				return;
			case AffectEffect.HpRevival:
				this.ShowHpRevival(data, onDamage, isDrain);
				return;
			case AffectEffect.Counter:
				this.ShowCounter(data, onDamage);
				return;
			case AffectEffect.Reflection:
				this.ShowReflection(data, onDamage);
				return;
			case AffectEffect.Protect:
				this.ShowProtect(data);
				return;
			case AffectEffect.PowerCharge:
				this.ShowPowerCharge(data);
				return;
			case AffectEffect.Destruct:
				this.ShowDestruct(data);
				return;
			case AffectEffect.Paralysis:
				this.ShowParalysis(data);
				return;
			case AffectEffect.Poison:
				this.ShowPoison(data, onDamage);
				return;
			case AffectEffect.Sleep:
				this.ShowSleep(data);
				return;
			case AffectEffect.SkillLock:
				this.ShowSkillLock(data);
				return;
			case AffectEffect.HitRateUp:
				this.ShowHitRateUp(data);
				return;
			case AffectEffect.HitRateDown:
				this.ShowHitRateDown(data);
				return;
			case AffectEffect.InstantDeath:
				this.ShowInstantDeath(data);
				return;
			case AffectEffect.Confusion:
				this.ShowConfusion(data);
				return;
			case AffectEffect.Stun:
				this.ShowStun(data);
				return;
			case AffectEffect.SufferStatusClear:
				this.ShowSufferStatusClear(data);
				return;
			case AffectEffect.SatisfactionRateUp:
				this.ShowSatisfactionRateUp(data);
				return;
			case AffectEffect.SatisfactionRateDown:
				this.ShowSatisfactionRateDown(data);
				return;
			case AffectEffect.ApRevival:
				this.ShowApRevival(data);
				return;
			case AffectEffect.ApUp:
				this.ShowApUp(data);
				return;
			case AffectEffect.ApDown:
				this.ShowApDown(data);
				return;
			case AffectEffect.ApConsumptionUp:
				this.ShowApConsumptionUp(data);
				return;
			case AffectEffect.ApConsumptionDown:
				this.ShowApConsumptionDown(data);
				return;
			case AffectEffect.TurnBarrier:
				this.ShowTurnBarrier(data);
				return;
			case AffectEffect.CountBarrier:
				this.ShowCountBarrier(data);
				return;
			case AffectEffect.Invalid:
				this.ShowInvalid(data);
				return;
			case AffectEffect.Recommand:
				this.ShowRecommand(data);
				return;
			case AffectEffect.DamageRateUp:
				this.ShowDamageRateUp(data);
				return;
			case AffectEffect.DamageRateDown:
				this.ShowDamageRateDown(data);
				return;
			case AffectEffect.Regenerate:
				this.ShowRegenerate(data, onDamage);
				return;
			case AffectEffect.TurnEvasion:
				this.ShowTurnEvasion(data);
				return;
			case AffectEffect.CountEvasion:
				this.ShowCountEvasion(data);
				return;
			case AffectEffect.Escape:
				this.ShowEscape(data);
				return;
			}
			NGUITools.SetActiveSelf(base.gameObject, false);
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
		case Strength.Drain:
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
			return this.GetString("HitIconStrong");
		case Strength.Weak:
			return this.GetString("HitIconWeak");
		case Strength.Drain:
			return this.GetString("BattleTxtDamageDrain");
		case Strength.Invalid:
			return this.GetString("HitIconInvalid");
		default:
			return string.Empty;
		}
	}

	private Texture GetResistanceFontTextureForNumber(Strength strength)
	{
		switch (strength)
		{
		case Strength.None:
			return this.standardEffectFontTexture.white;
		case Strength.Strong:
			return this.standardEffectFontTexture.blue;
		case Strength.Weak:
			return this.standardEffectFontTexture.red;
		case Strength.Drain:
			return this.standardEffectFontTexture.green;
		case Strength.Invalid:
			return this.standardEffectFontTexture.blue;
		default:
			return this.standardEffectFontTexture.white;
		}
	}

	private Texture GetResistanceFontTextureForTop(Strength strength)
	{
		switch (strength)
		{
		case Strength.None:
			return this.resistanceFontTexture.blue;
		case Strength.Strong:
			return this.resistanceFontTexture.blue;
		case Strength.Weak:
			return this.resistanceFontTexture.red;
		case Strength.Drain:
			return this.resistanceFontTexture.green;
		case Strength.Invalid:
			return this.resistanceFontTexture.blue;
		default:
			return this.resistanceFontTexture.blue;
		}
	}

	private void ShowMiss(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconMiss");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowAttackUp(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconAttackUp");
		this.ChangeFontTexture(this.standardEffectFontTexture.red, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowAttackDown(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconAttackDown");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowDefenceUp(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconDefenceUp");
		this.ChangeFontTexture(this.standardEffectFontTexture.red, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowDefenceDown(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconDefenceDown");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowSpAttackUp(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconSpAttackUp");
		this.ChangeFontTexture(this.standardEffectFontTexture.red, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowSpAttackDown(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconSpAttackDown");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowSpDefenceUp(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconSpDefenceUp");
		this.ChangeFontTexture(this.standardEffectFontTexture.red, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowSpDefenceDown(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconSpDefenceDown");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowSpeedUp(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconSpeedUp");
		this.ChangeFontTexture(this.standardEffectFontTexture.red, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowSpeedDown(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconSpeedDown");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowCorrectionUpReset(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconCorrectionUpReset");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowCorrectionDownReset(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconCorrectionDownReset");
		this.ChangeFontTexture(this.standardEffectFontTexture.red, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowProtect(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconProtect");
		this.ChangeFontTexture(this.standardEffectFontTexture.yellow, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowPowerCharge(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconPowerCharge");
		this.ChangeFontTexture(this.standardEffectFontTexture.yellow, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowDestruct(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconDestruct");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowHitRateUp(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconHitRateUp");
		this.ChangeFontTexture(this.standardEffectFontTexture.red, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowHitRateDown(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconHitRateDown");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowSufferStatusClear(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconSufferStatusClear");
		this.ChangeFontTexture(this.standardEffectFontTexture.yellow, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowSatisfactionRateUp(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconSatisfactionRateUp");
		this.ChangeFontTexture(this.standardEffectFontTexture.red, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowSatisfactionRateDown(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconSatisfactionRateDown");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowApUp(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconApUp");
		this.ChangeFontTexture(this.standardEffectFontTexture.red, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowApDown(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconApDown");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowApConsumptionUp(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconApConsumptionUp");
		this.ChangeFontTexture(this.standardEffectFontTexture.red, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowApConsumptionDown(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconApConsumptionDown");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowApRevival(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconApRevival");
		this.ChangeFontTexture(this.standardEffectFontTexture.yellow, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowTurnBarrier(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconTurnBarrier");
		this.ChangeFontTexture(this.standardEffectFontTexture.yellow, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowCountBarrier(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconCountBarrier");
		this.ChangeFontTexture(this.standardEffectFontTexture.yellow, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowDamageRateUp(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconDamageRateUp");
		this.ChangeFontTexture(this.standardEffectFontTexture.red, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowDamageRateDown(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconDamageRateDown");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowInvalid(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconInvalid");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowTurnEvasion(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconTurnEvasion");
		this.ChangeFontTexture(this.standardEffectFontTexture.red, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowCountEvasion(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconCountEvasion");
		this.ChangeFontTexture(this.standardEffectFontTexture.red, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowRecommand(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconRecommand");
		this.ChangeFontTexture(this.standardEffectFontTexture.yellow, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowEscape(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconEscape");
		this.ChangeFontTexture(this.standardEffectFontTexture.yellow, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowParalysis(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconParalysis");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowSleep(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconSleep");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowSkillLock(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconSkillLock");
		this.ChangeFontTexture(this.standardEffectFontTexture.green, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowInstantDeath(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconInstantDeath");
		this.ChangeFontTexture(this.standardEffectFontTexture.red, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowConfusion(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconConfusion");
		this.ChangeFontTexture(this.standardEffectFontTexture.yellow, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowStun(HitIcon.Data data)
	{
		data.middleMesh.text = this.GetString("HitIconStun");
		this.ChangeFontTexture(this.standardEffectFontTexture.blue, new TextMeshPro[]
		{
			data.middleMesh
		});
	}

	private void ShowPoison(HitIcon.Data data, int damage)
	{
		if (damage > 0)
		{
			BattleStateUIProperty uiProperty = BattleStateManager.current.uiProperty;
			base.transform.localScale = uiProperty.hitIconLocalScale * uiProperty.onPoisonScalingSizeHitIcon;
			data.bottomMesh.text = this.GetString("HitIconPoison");
			data.numMesh.text = damage.ToString();
			this.ChangeFontTexture(this.standardEffectFontTexture.purple, new TextMeshPro[]
			{
				data.numMesh,
				data.bottomMesh
			});
		}
		else
		{
			data.middleMesh.text = this.GetString("HitIconPoison");
			this.ChangeFontTexture(this.standardEffectFontTexture.purple, new TextMeshPro[]
			{
				data.middleMesh
			});
		}
	}

	private void ShowRegenerate(HitIcon.Data data, int value)
	{
		if (value > 0)
		{
			BattleStateUIProperty uiProperty = BattleStateManager.current.uiProperty;
			base.transform.localScale = uiProperty.hitIconLocalScale * uiProperty.onPoisonScalingSizeHitIcon;
			data.bottomMesh.text = this.GetString("HitIconRegenerate");
			data.numMesh.text = value.ToString();
			this.ChangeFontTexture(this.standardEffectFontTexture.pink, new TextMeshPro[]
			{
				data.numMesh,
				data.bottomMesh
			});
		}
		else
		{
			data.middleMesh.text = this.GetString("HitIconRegenerate");
			this.ChangeFontTexture(this.standardEffectFontTexture.pink, new TextMeshPro[]
			{
				data.middleMesh
			});
		}
	}

	private void ShowHpRevival(HitIcon.Data data, int value, bool isDrain)
	{
		if (isDrain)
		{
			data.numMesh.text = value.ToString();
			data.bottomMesh.text = this.GetString("HitIconDrain");
		}
		else
		{
			data.numMesh.text = value.ToString();
			data.bottomMesh.text = this.GetString("HitIconHpRevival");
		}
		this.ChangeFontTexture(this.standardEffectFontTexture.pink, new TextMeshPro[]
		{
			data.numMesh,
			data.bottomMesh
		});
	}

	private void ShowCounter(HitIcon.Data data, int value)
	{
		data.bottomMesh.text = this.GetString("HitIconCounter");
		this.ChangeFontTexture(this.standardEffectFontTexture.yellow, new TextMeshPro[]
		{
			data.bottomMesh
		});
	}

	private void ShowReflection(HitIcon.Data data, int value)
	{
		data.bottomMesh.text = this.GetString("HitIconReflection");
		this.ChangeFontTexture(this.standardEffectFontTexture.yellow, new TextMeshPro[]
		{
			data.bottomMesh
		});
	}

	private void ShowDamage(HitIcon.Data data, int value, Strength strength, bool isCritical, bool isCounter, bool isReflection, ExtraEffectType extraEffectType)
	{
		if (isCounter)
		{
			data.bottomMesh.text = this.GetString("HitIconCounter");
			data.numMesh.text = value.ToString();
			this.ChangeFontTexture(this.standardEffectFontTexture.yellow, new TextMeshPro[]
			{
				data.numMesh,
				data.bottomMesh
			});
		}
		if (isReflection)
		{
			data.bottomMesh.text = this.GetString("HitIconReflection");
			data.numMesh.text = value.ToString();
			this.ChangeFontTexture(this.standardEffectFontTexture.yellow, new TextMeshPro[]
			{
				data.numMesh,
				data.bottomMesh
			});
		}
		else
		{
			if (isCritical)
			{
				data.bottomMesh.text = this.GetString("HitIconCritical");
				this.ChangeFontTexture(this.standardEffectFontTexture.yellow, new TextMeshPro[]
				{
					data.bottomMesh
				});
			}
			data.numMesh.text = value.ToString();
			Texture resistanceFontTextureForNumber = this.GetResistanceFontTextureForNumber(strength);
			this.ChangeFontTexture(resistanceFontTextureForNumber, new TextMeshPro[]
			{
				data.numMesh
			});
			data.topMesh.text = this.GetWeakName(strength);
			Texture resistanceFontTextureForTop = this.GetResistanceFontTextureForTop(strength);
			this.ChangeFontTexture(resistanceFontTextureForTop, new TextMeshPro[]
			{
				data.topMesh
			});
		}
	}

	private void ChangeFontTexture(Texture texture, params TextMeshPro[] textMeshPros)
	{
		this.materialPropertyBlock.SetTexture("_FaceTex", texture);
		foreach (TextMeshPro textMeshPro in textMeshPros)
		{
			textMeshPro.renderer.SetPropertyBlock(this.materialPropertyBlock);
		}
	}

	private string GetString(string key)
	{
		if (BattleStateManager.current.onServerConnect)
		{
			if (!this.dictionary.ContainsKey(key))
			{
				this.dictionary.Add(key, StringMaster.GetString(key));
			}
			return this.dictionary[key];
		}
		return string.Empty;
	}

	[Serializable]
	private class ResistanceFontTexture
	{
		public Texture red;

		public Texture blue;

		public Texture green;
	}

	[Serializable]
	private class StandardEffectFontTexture
	{
		public Texture red;

		public Texture blue;

		public Texture green;

		public Texture yellow;

		public Texture pink;

		public Texture purple;

		public Texture white;
	}

	[Serializable]
	private class StageEffectFontTexture
	{
		public Texture up;

		public Texture down;
	}

	[Serializable]
	private class LanguageData
	{
		public TMP_FontAsset font;

		public Material material;
	}

	[Serializable]
	private class LanguageFont
	{
		public HitIcon.LanguageData resistance;

		public HitIcon.LanguageData standard;

		public HitIcon.LanguageData num;

		public HitIcon.LanguageData stage;
	}

	[Serializable]
	public class Data
	{
		public GameObject gameObject;

		public TextMeshPro numMesh;

		public TextMeshPro topMesh;

		public TextMeshPro middleMesh;

		public TextMeshPro bottomMesh;
	}
}
