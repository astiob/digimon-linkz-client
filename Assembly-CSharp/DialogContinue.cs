using BattleStateMachineInternal;
using Master;
using System;
using UnityEngine;

public sealed class DialogContinue : MonoBehaviour
{
	[Header("特定商取引ボタン")]
	[SerializeField]
	private UIButton specificTradeButton;

	[SerializeField]
	[Header("リタイアボタン")]
	private UIButton retireButton;

	[Header("復活ボタン")]
	[SerializeField]
	private UIButton revivalButton;

	[SerializeField]
	[Header("所持デジストーンの数")]
	private UILabel digistoneNumLabel;

	[Header("バックグラウンドのスキナー")]
	[SerializeField]
	private UIComponentSkinner backgroundSkinner;

	[SerializeField]
	[Header("コンティニューかショップ切り替えスキナー")]
	private UIComponentSkinner revivalOrShowShopButtonSwitch;

	[SerializeField]
	[Header("デジストーン説明Replacer")]
	private UILabel digistoneDescription;

	[SerializeField]
	[Header("特定商取引ボタンのコライダー")]
	private Collider specificTradeCollider;

	[SerializeField]
	[Header("リタイアボタンのコライダー")]
	private Collider retireButtonCollider;

	[SerializeField]
	[Header("復活ボタンボタンのコライダー")]
	private Collider revivalButtonCollider;

	[SerializeField]
	[Header("UIOpenCloseDialog")]
	public UIOpenCloseDialog openCloseDialog;

	[SerializeField]
	[Header("特定商取引法に基づく表記ローカライズ")]
	private UILabel specificTradeLocalize;

	[SerializeField]
	[Header("諦めるローカライズ")]
	private UILabel retireLocalize;

	[SerializeField]
	[Header("コンティニューのタイトルのローカライズ")]
	private UILabel continueTitleLocalize;

	[SerializeField]
	[Header("復活する/ショップへ移動ローカライズ")]
	private UITextReplacer revivalOrShopLocalize;

	[Header("復活する/ショップへ移動ローカライズ(マルチのみ)")]
	[SerializeField]
	private UITextReplacer multiRevivalOrShopLocalize;

	[SerializeField]
	[Header("所持デジストーンローカライズ")]
	private UILabel haveDigistoneLocalize;

	public void ApplySpecificTrade(bool isShow)
	{
		this.backgroundSkinner.SetSkins(isShow ? 1 : 0);
	}

	public void AddEvent(Action specificTradecallback, Action revivalCallBack, Action retireCallback, bool isOwner = true)
	{
		BattleInputUtility.AddEvent(this.specificTradeButton.onClick, specificTradecallback);
		if (isOwner)
		{
			BattleInputUtility.AddEvent(this.retireButton.onClick, retireCallback);
			BattleInputUtility.AddEvent(this.revivalButton.onClick, revivalCallBack);
		}
	}

	public void ApplyContinueNeedDigiStone(BattleStateData battleStateData, int needDigiStone, int currentDigistone, bool isCheckDigiStoneZero)
	{
		string @string = StringMaster.GetString("BattleUI-11");
		if (isCheckDigiStoneZero && currentDigistone < needDigiStone)
		{
			@string = StringMaster.GetString("BattleUI-11");
		}
		else if (needDigiStone < battleStateData.playerCharacters.Length + 2)
		{
			@string = StringMaster.GetString("BattleUI-09");
		}
		else
		{
			@string = StringMaster.GetString("BattleUI-08");
		}
		this.digistoneDescription.text = string.Format(@string, needDigiStone);
		if (battleStateData.isEnableShopMoveDigistoneZero)
		{
			this.revivalOrShowShopButtonSwitch.SetSkins((currentDigistone < needDigiStone) ? 1 : 0);
		}
		else
		{
			this.revivalOrShowShopButtonSwitch.SetSkins(0);
		}
	}

	public void ApplyDigiStoneNumber(int digiStoneNumber)
	{
		this.digistoneNumLabel.text = digiStoneNumber.ToString();
	}

	public void SetColliderActive(bool active)
	{
		this.specificTradeCollider.enabled = active;
		this.retireButtonCollider.enabled = active;
		this.revivalButtonCollider.enabled = active;
	}

	private void Awake()
	{
		this.SetLocalize();
	}

	private void SetLocalize()
	{
		this.specificTradeLocalize.text = StringMaster.GetString("ShopRule-02");
		this.retireLocalize.text = StringMaster.GetString("BattleUI-13");
		this.continueTitleLocalize.text = StringMaster.GetString("BattleUI-07");
		string @string = StringMaster.GetString("BattleUI-14");
		string string2 = StringMaster.GetString("SystemButtonGoShop");
		this.revivalOrShopLocalize.SetValue(0, new TextReplacerValue(@string));
		this.revivalOrShopLocalize.SetValue(1, new TextReplacerValue(string2));
		if (this.multiRevivalOrShopLocalize != null)
		{
			this.multiRevivalOrShopLocalize.SetValue(0, new TextReplacerValue(@string));
			this.multiRevivalOrShopLocalize.SetValue(1, new TextReplacerValue(string2));
		}
		this.haveDigistoneLocalize.text = StringMaster.GetString("BattleUI-17");
	}
}
