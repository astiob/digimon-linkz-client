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

	[Header("所持デジストーンの数")]
	[SerializeField]
	private UILabel digistoneNumLabel;

	[SerializeField]
	[Header("バックグラウンド")]
	private GameObject background;

	[Header("デジストーン説明のローカライズ")]
	[SerializeField]
	private UILabel digistoneDescription;

	[SerializeField]
	[Header("特定商取引ボタンのコライダー")]
	private Collider specificTradeCollider;

	[SerializeField]
	[Header("リタイアボタンのコライダー")]
	private Collider retireButtonCollider;

	[Header("復活ボタンボタンのコライダー")]
	[SerializeField]
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

	[Header("コンティニューのタイトルのローカライズ")]
	[SerializeField]
	private UILabel continueTitleLocalize;

	[SerializeField]
	[Header("復活する/ショップへ移動ローカライズ")]
	private UILabel revivalOrShopLocalize;

	[Header("所持デジストーンローカライズ")]
	[SerializeField]
	private UILabel haveDigistoneLocalize;

	[SerializeField]
	[Header("復活する/ショップへ移動ローカライズ(マルチバトル用)")]
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
		this.specificTradeButton.gameObject.SetActive(false);
	}

	private void SetLocalize()
	{
		this.specificTradeLocalize.text = StringMaster.GetString("ShopRule-02");
		this.retireLocalize.text = StringMaster.GetString("BattleUI-13");
		this.continueTitleLocalize.text = StringMaster.GetString("BattleUI-07");
		this.haveDigistoneLocalize.text = StringMaster.GetString("BattleUI-17");
	}
}
