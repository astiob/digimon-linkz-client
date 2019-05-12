using System;
using UnityEngine;

public class BattleUIComponentsPvP : BattleUIComponentsMultiBasic
{
	[Header("Menu/PanelのTransform")]
	[SerializeField]
	private Transform menuPanelTransform;

	[Header("味方のPvP用上に出る文字")]
	[SerializeField]
	private UIWidget _pvpBattleYourPartyUi;

	[Header("敵のPvP用上に出る文字")]
	[SerializeField]
	private UIWidget _pvpBattleEnemyPartyUi;

	[Header("シード同期中のもの")]
	[SerializeField]
	private UIWidget _pvpBattleSyncWaitUi;

	[SerializeField]
	public UILabel playerNameLabel;

	[SerializeField]
	public UITexture playerTitleIcon;

	[SerializeField]
	public UILabel enemyNameLabel;

	[SerializeField]
	public UITexture enemyTitleIcon;

	[NonSerialized]
	public EmotionSenderMulti emotionSenderMulti;

	[Header("Winの次のボタン")]
	[SerializeField]
	public UIButton winNextButton;

	[Header("Loseの次のボタン")]
	[SerializeField]
	public UIButton loseNextButton;

	public override GameObject enemyStatusDescriptionGO
	{
		get
		{
			return base.pvpEnemyStatusDescription.gameObject;
		}
	}

	public UIWidget pvpBattleYourPartyUi
	{
		get
		{
			return this._pvpBattleYourPartyUi;
		}
	}

	public UIWidget pvpBattleEnemyPartyUi
	{
		get
		{
			return this._pvpBattleEnemyPartyUi;
		}
	}

	public UIWidget pvpBattleSyncWaitUi
	{
		get
		{
			return this._pvpBattleSyncWaitUi;
		}
	}

	protected override void SetupDialogs(Transform dialogRetireTrans)
	{
		this.menuDialog = dialogRetireTrans.gameObject.AddComponent<MenuDialog>();
		dialogRetireTrans.SetParent(this.menuPanelTransform);
		UIWidget component = dialogRetireTrans.GetComponent<UIWidget>();
		if (component != null)
		{
			component.SetAnchor(this.menuPanelTransform);
		}
	}

	protected override void CreateStatusObjects()
	{
		base.characterStatusDescription = base.skillSelectUi.CreateStatusAlly();
		base.pvpEnemyStatusDescription = base.skillSelectUi.CreateStatusAlly();
	}
}
