using System;
using UnityEngine;

public class BattleUIComponentsPvP : BattleUIComponentsMultiBasic
{
	[Header("Menu/PanelのTransform")]
	[SerializeField]
	private Transform menuPanelTransform;

	[SerializeField]
	private UIWidget _timeOverUi;

	[Header("VSの時のメッセージ")]
	[SerializeField]
	private UIWidget _pvpVSUi;

	[Header("味方のPvP用上に出る文字")]
	[SerializeField]
	private UIWidget _pvpBattleYourPartyUi;

	[SerializeField]
	[Header("敵のPvP用上に出る文字")]
	private UIWidget _pvpBattleEnemyPartyUi;

	[SerializeField]
	[Header("シード同期中のもの")]
	private UIWidget _pvpBattleSyncWaitUi;

	[SerializeField]
	public UILabel playerNameLabel;

	[SerializeField]
	public UILabel enemyNameLabel;

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

	public UIWidget timeOverUi
	{
		get
		{
			return this._timeOverUi;
		}
	}

	public UIWidget pvpVSUi
	{
		get
		{
			return this._pvpVSUi;
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
	}

	protected override void CreateStatusObjects()
	{
		base.characterStatusDescription = base.skillSelectUi.CreateStatusAlly();
		base.pvpEnemyStatusDescription = base.skillSelectUi.CreateStatusAlly();
	}
}
