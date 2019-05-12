using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIHUD : UITweener
{
	[Header("[マルチバトル用]AP UP用オブジェクト")]
	[SerializeField]
	public GameObject apUpObject;

	[SerializeField]
	[Header("[マルチバトル用]HP UP用オブジェクト")]
	public GameObject hpUpObject;

	[SerializeField]
	[Header("ターン数のラベル")]
	private UILabel turnNumber;

	[Header("[マルチバトル用]AP数")]
	[SerializeField]
	public UILabel multiAPNumber;

	[Header("HPゲージ")]
	[SerializeField]
	private UIGaugeManager hpGauge;

	[SerializeField]
	[Header("APゲージ")]
	private UIGaugeManager apGauge;

	[SerializeField]
	[Header("押しっぱなしのボタン")]
	private HoldPressButton holdPressButton;

	[Header("アイコン画像(Group01)")]
	[SerializeField]
	private UISprite[] iconImageGroup01;

	[SerializeField]
	[Header("HUDコンポーネントのスキナー")]
	private UIComponentSkinner hudComponentSkinner;

	[SerializeField]
	[Header("StatePlateコンポーネントのスキナー")]
	private UIComponentSkinner statePlateComponentSkinner;

	[SerializeField]
	[Header("[マルチバトル用]Pnスプライトのコンポーネントのスキナー")]
	private UIComponentSkinner pnSpriteComponentSkinner;

	[SerializeField]
	[Header("[マルチバトル用]AP UP Rootのコンポーネントのスキナー")]
	public UIComponentSkinner apUpRootComponentSkinner;

	[Header("[マルチバトル用]HP UP Rootのコンポーネントのスキナー")]
	[SerializeField]
	public UIComponentSkinner hpUpRootComponentSkinner;

	[SerializeField]
	[Header("ターン数Tween")]
	private UITweener turnNumberTween;

	[Header("HUDのTween")]
	[SerializeField]
	private UITweener hudTween;

	[Header("デジモンの名前")]
	[SerializeField]
	private UITextReplacer digimonName;

	[SerializeField]
	[Range(0f, 1f)]
	private float from = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	private float to = 1f;

	[Header("デジモン名、状態異常表示エリアのオブジェクトグループ")]
	[SerializeField]
	private List<UIWidget> viewWidgets = new List<UIWidget>();

	private Dictionary<SufferStateProperty.SufferType, string> iconSpriteNames = new Dictionary<SufferStateProperty.SufferType, string>();

	private List<SufferStateProperty.SufferType> sufferOrderList = new List<SufferStateProperty.SufferType>();

	private int nextIndex;

	private int viewId;

	private float currentFactor;

	public float alpha
	{
		get
		{
			return this.viewWidgets[this.viewId].alpha;
		}
		set
		{
			this.viewWidgets[this.viewId].alpha = value;
		}
	}

	public void InitNum()
	{
		this.hpGauge.SetMin(0);
		if (this.apGauge != null)
		{
			this.apGauge.SetMin(0);
			this.apGauge.SetMax(5);
		}
	}

	public void AddEvent(Action<int> onPressHud, Action offPressHud, int onHoldWaitPressValue)
	{
		BattleInputUtility.AddEvent(this.holdPressButton.onHoldWaitPress, onPressHud, onHoldWaitPressValue);
		BattleInputUtility.AddEvent(this.holdPressButton.onDisengagePress, offPressHud);
	}

	public void ApplyCharacterHudBoss(bool isBoss)
	{
		this.statePlateComponentSkinner.SetSkins((!isBoss) ? 0 : 1);
	}

	public void ApplyCharacterHudContent(CharacterStateControl characterStatus = null)
	{
		this.hpGauge.SetMax(characterStatus.extraMaxHp);
		this.hpGauge.SetValue(characterStatus.hp);
		if (this.apGauge != null)
		{
			this.apGauge.SetValue(characterStatus.ap);
		}
		int skillOrder = characterStatus.skillOrder;
		if (skillOrder < 0)
		{
			this.turnNumber.text = "-";
		}
		else
		{
			this.turnNumber.text = skillOrder.ToString();
		}
		this.hudComponentSkinner.SetSkins((!characterStatus.isEnemy) ? 0 : 1);
		if (characterStatus.skillOrder == 1)
		{
			this.turnNumberTween.ResetToBeginning();
			this.turnNumberTween.enabled = true;
		}
		else
		{
			this.turnNumberTween.enabled = false;
			this.turnNumberTween.tweenFactor = 0f;
			this.turnNumberTween.ResetToBeginning();
		}
		this.digimonName.SetValue(0, new TextReplacerValue(characterStatus.name));
		this.sufferOrderList.Clear();
		foreach (SufferStateProperty.SufferType item in characterStatus.currentSufferState.GetSufferOrderList())
		{
			this.sufferOrderList.Add(item);
		}
		this.viewId = 0;
		base.ResetToBeginning();
		this.UpdateActiveWidgets();
		base.enabled = (this.sufferOrderList.Count > 0);
		if (characterStatus.hp > 0)
		{
			this.hudTween.enabled = false;
			this.hudTween.ResetToBeginning();
		}
		else
		{
			this.hudTween.enabled = true;
			this.hudTween.ResetToBeginning();
			this.hudTween.PlayForward();
		}
	}

	public void ApplyCharacterHudReset()
	{
		this.hpGauge.Reset();
	}

	public void SetHP(int hp)
	{
		this.hpGauge.SetValue(hp);
	}

	public void SetPnNo(int num)
	{
		this.pnSpriteComponentSkinner.SetSkins(num);
	}

	private string SufferTypeToSpriteName(SufferStateProperty.SufferType sufferType)
	{
		if (this.iconSpriteNames.Count == 0)
		{
			this.InitIconNames();
		}
		string result = string.Empty;
		if (this.iconSpriteNames.ContainsKey(sufferType))
		{
			result = this.iconSpriteNames[sufferType];
		}
		else
		{
			global::Debug.LogError("unknown type :" + sufferType);
		}
		return result;
	}

	private void InitIconNames()
	{
		if (this.iconSpriteNames.Count == 0)
		{
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.Poison, "Battle_icon_poison");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.Confusion, "Battle_icon_confuse");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.Paralysis, "Battle_icon_para");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.Sleep, "Battle_icon_sleep");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.Stun, "Battle_icon_stan");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.SkillLock, "Battle_icon_skilllock");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.AttackUp, "Battle_icon_atk");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.AttackDown, "Battle_icon_atkd");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.DefenceUp, "Battle_icon_def");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.DefenceDown, "Battle_icon_defd");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.SpAttackUp, "Battle_icon_satk");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.SpAttackDown, "Battle_icon_satkd");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.SpDefenceUp, "Battle_icon_sdef");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.SpDefenceDown, "Battle_icon_sdefd");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.SpeedUp, "Battle_icon_spda");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.SpeedDown, "Battle_icon_spdd");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.Counter, "Battle_icon_counter");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.Reflection, "Battle_icon_reflec");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.Protect, "Battle_icon_defence");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.PowerCharge, "Battle_icon_aup");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.HitRateUp, "Battle_icon_hit");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.HitRateDown, "Battle_icon_hitd");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.SatisfactionRateUp, "Battle_icon_crt");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.SatisfactionRateDown, "Battle_icon_crtd");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.ApConsumptionUp, "Battle_icon_apup");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.ApConsumptionDown, "Battle_icon_apdown");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.ApRevival, "Battle_icon_apb");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.CountGuard, "Battle_icon_Invalid");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.TurnBarrier, "Battle_icon_Invalid");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.CountBarrier, "Battle_icon_Invalid");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.DamageRateUp, "Battle_icon_atk");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.DamageRateDown, "Battle_icon_atkd");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.Regenerate, "Battle_icon_EveryHpUp");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.TurnEvasion, "Battle_icon_avoid");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.CountEvasion, "Battle_icon_avoid");
			this.iconSpriteNames.Add(SufferStateProperty.SufferType.Escape, "Battle_icon_escape");
		}
	}

	private void UpdateActiveWidgets()
	{
		for (int i = 0; i < this.viewWidgets.Count; i++)
		{
			if (i != this.viewId)
			{
				this.viewWidgets[i].alpha = 0f;
			}
			NGUITools.SetActiveSelf(this.viewWidgets[i].gameObject, i == this.viewId);
		}
	}

	private void UpdateIcon()
	{
		foreach (UISprite uisprite in this.iconImageGroup01)
		{
			if (this.sufferOrderList.Count > this.nextIndex)
			{
				uisprite.gameObject.SetActive(true);
				uisprite.spriteName = this.SufferTypeToSpriteName(this.sufferOrderList[this.nextIndex]);
				this.nextIndex++;
			}
			else
			{
				uisprite.gameObject.SetActive(false);
			}
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.alpha = Mathf.Lerp(this.from, this.to, factor);
		this.currentFactor = ((base.tweenFactor <= this.currentFactor) ? (this.currentFactor + ((!this.ignoreTimeScale) ? Time.deltaTime : Time.unscaledDeltaTime)) : base.tweenFactor);
		if (this.currentFactor >= 1f)
		{
			if (this.viewId == 0)
			{
				if (this.sufferOrderList.Count == 0)
				{
					this.viewId = 0;
				}
				else
				{
					this.viewId = 1;
					this.nextIndex = 0;
					this.UpdateIcon();
				}
			}
			else if (this.sufferOrderList.Count > this.nextIndex)
			{
				this.UpdateIcon();
			}
			else
			{
				this.viewId = 0;
			}
			this.UpdateActiveWidgets();
			this.currentFactor = base.tweenFactor;
		}
	}

	public override void SetStartToCurrentValue()
	{
		this.from = this.alpha;
	}

	public override void SetEndToCurrentValue()
	{
		this.to = this.alpha;
	}

	public override void ApplyTweenFromValue()
	{
		this.alpha = this.from;
	}

	public override void ApplyTweenToValue()
	{
		this.alpha = this.to;
	}
}
