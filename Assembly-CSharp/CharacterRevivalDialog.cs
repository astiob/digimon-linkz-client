using Master;
using System;
using System.Collections;
using UnityEngine;

public class CharacterRevivalDialog : MonoBehaviour
{
	[SerializeField]
	private UIButton revivalButton;

	[SerializeField]
	private UIButton closeButton;

	[SerializeField]
	private UIButton specificTradeButton;

	[SerializeField]
	private UIComponentSkinner revivalOrGoShopButtonSwitch;

	[SerializeField]
	private UIOpenCloseDialog openCloseDialog;

	[SerializeField]
	private UILabel digistoneNumber;

	[Header("復活タイトル")]
	[SerializeField]
	private UILabel revivalLocalize;

	[Header("特定商取引法に基づく表記ローカライズ")]
	[SerializeField]
	private UILabel specificTradeLocalize;

	[Header("足りないエッセージローカライズ(マルチバトル用)")]
	[SerializeField]
	private UILabel notEnoughConsumeMessageLocalize;

	[SerializeField]
	[Header("消費メッセージローカライズ")]
	private UILabel consumeMessageLocalize;

	[SerializeField]
	[Header("所持ローカライズ")]
	private UILabel haveLocalize;

	[Header("閉じるローカライズ")]
	[SerializeField]
	private UILabel closeLocalize;

	[SerializeField]
	[Header("復活予約ローカライズ")]
	private UILabel bookRevivalLocalize;

	[SerializeField]
	[Header("ショップへローカライズ")]
	private UILabel goShopLocalize;

	public GameObject GetRevivalDialogEnterUIButton
	{
		get
		{
			return this.revivalButton.gameObject;
		}
	}

	public GameObject GetRevivalDialogCancelUIButton
	{
		get
		{
			return this.closeButton.gameObject;
		}
	}

	private void Awake()
	{
		this.SetupLocalize();
		this.specificTradeButton.gameObject.SetActive(false);
	}

	private void SetupLocalize()
	{
		if (this.notEnoughConsumeMessageLocalize != null)
		{
			this.notEnoughConsumeMessageLocalize.text = StringMaster.GetString("BattleUI-12");
		}
		this.consumeMessageLocalize.text = StringMaster.GetString("BattleUI-16");
		this.bookRevivalLocalize.text = StringMaster.GetString("BattleUI-14");
		this.goShopLocalize.text = StringMaster.GetString("SystemButtonGoShop");
		this.revivalLocalize.text = StringMaster.GetString("BattleUI-15");
		this.specificTradeLocalize.text = StringMaster.GetString("ShopRule-02");
		this.haveLocalize.text = StringMaster.GetString("BattleUI-17");
		this.closeLocalize.text = StringMaster.GetString("SystemButtonClose");
	}

	public void SetActive(bool value)
	{
		NGUITools.SetActiveSelf(base.gameObject, value);
	}

	public void AddRevivalButtonEvent(Action callback)
	{
		BattleInputUtility.AddEvent(this.revivalButton.onClick, callback);
	}

	public void AddCloseButtonEvent(Action callback)
	{
		BattleInputUtility.AddEvent(this.closeButton.onClick, callback);
	}

	public void AddSpecificTradeButtonEvent(Action callback)
	{
		BattleInputUtility.AddEvent(this.specificTradeButton.onClick, callback);
	}

	public IEnumerator ApplyEnableCharacterRevivalWindow(bool isShow, bool isPossibleRevival = false, Action onFinishedAction = null)
	{
		BattleUIControlBasic uiControl = BattleStateManager.current.uiControl;
		if (isShow)
		{
			this.revivalOrGoShopButtonSwitch.SetSkins((!isPossibleRevival) ? 1 : 0);
			return uiControl.WaitOpenCloseDialog(isShow, base.gameObject, this.openCloseDialog, null);
		}
		return uiControl.WaitOpenCloseDialog(isShow, base.gameObject, this.openCloseDialog, onFinishedAction);
	}

	public void ApplyDigiStoneNumber(int digiStoneNumber)
	{
		this.digistoneNumber.text = digiStoneNumber.ToString();
	}
}
