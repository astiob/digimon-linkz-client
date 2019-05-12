using BattleStateMachineInternal;
using Master;
using System;
using UnityEngine;

public sealed class DialogContinue : MonoBehaviour
{
	[SerializeField]
	[Header("特定商取引ボタン")]
	private UIButton specificTradeButton;

	[Header("リタイアボタン")]
	[SerializeField]
	private UIButton retireButton;

	[Header("復活ボタン")]
	[SerializeField]
	private UIButton revivalButton;

	[SerializeField]
	[Header("所持デジストーンの数")]
	private UILabel digistoneNumLabel;

	[SerializeField]
	[Header("バックグラウンドのスキナー")]
	private UIComponentSkinner backgroundSkinner;

	[Header("コンティニューかショップ切り替えスキナー")]
	[SerializeField]
	private UIComponentSkinner revivalOrShowShopButtonSwitch;

	[SerializeField]
	[Header("デジストーン説明Replacer")]
	private UILabel digistoneDescription;

	[Header("特定商取引ボタンのコライダー")]
	[SerializeField]
	private Collider specificTradeCollider;

	[SerializeField]
	[Header("リタイアボタンのコライダー")]
	private Collider retireButtonCollider;

	[SerializeField]
	[Header("復活ボタンボタンのコライダー")]
	private Collider revivalButtonCollider;

	[Header("UIOpenCloseDialog")]
	[SerializeField]
	public UIOpenCloseDialog openCloseDialog;

	[Header("特定商取引法に基づく表記ローカライズ")]
	[SerializeField]
	private UILabel specificTradeLocalize;

	[Header("諦めるローカライズ")]
	[SerializeField]
	private UILabel retireLocalize;

	[Header("コンティニューのタイトルのローカライズ")]
	[SerializeField]
	private UILabel continueTitleLocalize;

	[Header("復活する/ショップへ移動ローカライズ")]
	[SerializeField]
	private UITextReplacer revivalOrShopLocalize;

	[Header("復活する/ショップへ移動ローカライズ(マルチのみ)")]
	[SerializeField]
	private UITextReplacer multiRevivalOrShopLocalize;

	[Header("所持デジストーンローカライズ")]
	[SerializeField]
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
		this.revivalOrShowShopButtonSwitch.SetSkins((currentDigistone < needDigiStone) ? 1 : 0);
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
