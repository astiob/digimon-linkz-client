using BattleStateMachineInternal;
using Master;
using System;
using UnityEngine;

public sealed class DialogContinue : MonoBehaviour
{
	[Header("特定商取引ボタン")]
	[SerializeField]
	private UIButton specificTradeButton;

	[Header("リタイアボタン")]
	[SerializeField]
	private UIButton retireButton;

	[Header("復活ボタン")]
	[SerializeField]
	private UIButton revivalButton;

	[Header("所持デジストーンの数")]
	[SerializeField]
	private UILabel digistoneNumLabel;

	[Header("バックグラウンド")]
	[SerializeField]
	private GameObject background;

	[Header("デジストーン説明のローカライズ")]
	[SerializeField]
	private UILabel digistoneDescription;

	[Header("特定商取引ボタンのコライダー")]
	[SerializeField]
	private Collider specificTradeCollider;

	[Header("リタイアボタンのコライダー")]
	[SerializeField]
	private Collider retireButtonCollider;

	[Header("復活ボタンボタンのコライダー")]
	[SerializeField]
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
	private UILabel revivalOrShopLocalize;

	[Header("所持デジストーンローカライズ")]
	[SerializeField]
	private UILabel haveDigistoneLocalize;

	[Header("復活する/ショップへ移動ローカライズ(マルチバトル用)")]
	[SerializeField]
	private UILabel offRevivalOrShopLocalize;

	public void ApplySpecificTrade(bool isShow)
	{
		this.background.SetActive(isShow);
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
		if (currentDigistone >= needDigiStone)
		{
			this.revivalOrShopLocalize.text = StringMaster.GetString("BattleUI-14");
		}
		else
		{
			this.revivalOrShopLocalize.text = StringMaster.GetString("SystemButtonGoShop");
		}
		if (this.offRevivalOrShopLocalize != null)
		{
			this.offRevivalOrShopLocalize.text = this.revivalOrShopLocalize.text;
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
		this.haveDigistoneLocalize.text = StringMaster.GetString("BattleUI-17");
	}
}
