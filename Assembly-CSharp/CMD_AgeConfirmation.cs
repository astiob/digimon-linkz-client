using Master;
using System;
using UnityEngine;

public class CMD_AgeConfirmation : CMD
{
	[SerializeField]
	private UILabel ageConfirmTitleLabel;

	[SerializeField]
	private UILabel birthdayRegistLabel;

	[SerializeField]
	private UILabel ageConfirmMessageLabel;

	[SerializeField]
	private UILabel noteTextLabel;

	[SerializeField]
	private UILabel registYearLabel;

	[SerializeField]
	private UILabel registMonthLabel;

	[SerializeField]
	private UILabel limitationTextLabel;

	[SerializeField]
	private GameObject goSubmitButton_NG;

	[SerializeField]
	private GameObject goSubmitButton_OK;

	[SerializeField]
	private UILabel ngSubmitButtonLabel;

	[SerializeField]
	private CustomUIInput inYear;

	[SerializeField]
	private CustomUIInput inMonth;

	[SerializeField]
	private BoxCollider colCloseBtn;

	[SerializeField]
	private GUICollider gColCloseBtn;

	private bool validationOK_Year;

	private bool validationOK_Month;

	protected override void Awake()
	{
		base.Awake();
		this.ageConfirmTitleLabel.text = StringMaster.GetString("ShopAgeTitle");
		this.birthdayRegistLabel.text = StringMaster.GetString("ShopAge-01");
		this.ageConfirmMessageLabel.text = StringMaster.GetString("ShopAge-02");
		this.noteTextLabel.text = StringMaster.GetString("ShopAge-03");
		this.registYearLabel.text = StringMaster.GetString("ShopAge-04");
		this.registMonthLabel.text = StringMaster.GetString("ShopAge-05");
		this.limitationTextLabel.text = StringMaster.GetString("ShopAge-06");
		this.ngSubmitButtonLabel.text = StringMaster.GetString("ShopAge-07");
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
		this.InitComponents();
	}

	protected void UILock()
	{
		this.colCloseBtn.enabled = false;
	}

	protected void UIUnLock()
	{
		this.colCloseBtn.enabled = true;
	}

	protected void InitComponents()
	{
		this.inYear.value = string.Empty;
		this.inMonth.value = string.Empty;
	}

	public void ConfirmBirthday()
	{
		bool flag = this.ValidationYear();
		bool flag2 = this.ValidationMonth();
		if (!flag || !flag2)
		{
			return;
		}
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.SubmitBirthday), "CMD_Confirm") as CMD_Confirm;
		cmd_Confirm.Title = StringMaster.GetString("ShopAgeConfirmTitle");
		cmd_Confirm.Info = string.Format(StringMaster.GetString("ShopAgeConfirmInfo"), this.inYear.value, this.inMonth.value);
	}

	private void SubmitBirthday(int idx)
	{
		if (idx == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			this.UILock();
			string birth = this.inYear.value + this.inMonth.value + "01";
			APIRequestTask task = Singleton<UserDataMng>.Instance.RequestUpdateBirthDay(birth, false);
			base.StartCoroutine(task.Run(delegate
			{
				RestrictionInput.EndLoad();
				this.RecvAPI_UpdateBirthday(true);
			}, delegate(Exception nop)
			{
				RestrictionInput.EndLoad();
				this.RecvAPI_UpdateBirthday(false);
			}, null));
		}
	}

	private bool ValidationYear()
	{
		if (string.IsNullOrEmpty(this.inYear.value))
		{
			this.validationOK_Year = false;
			this.UpdateSubmitButton();
			return false;
		}
		int num = int.Parse(this.inYear.value);
		if (num > ServerDateTime.Now.Year || num < 1800)
		{
			this.inYear.value = ServerDateTime.Now.Year.ToString();
		}
		this.validationOK_Year = true;
		this.SolveFutureDay();
		this.UpdateSubmitButton();
		return true;
	}

	private bool ValidationMonth()
	{
		if (string.IsNullOrEmpty(this.inMonth.value))
		{
			this.validationOK_Month = false;
			this.UpdateSubmitButton();
			return false;
		}
		int num = int.Parse(this.inMonth.value);
		if (string.IsNullOrEmpty(this.inMonth.value))
		{
			this.inMonth.value = ServerDateTime.Now.Year.ToString();
		}
		if (num > 12 || num <= 1)
		{
			this.inMonth.value = "1";
		}
		this.validationOK_Month = true;
		this.SolveFutureDay();
		this.ZeroPading();
		this.UpdateSubmitButton();
		return true;
	}

	private void SolveFutureDay()
	{
		if (this.validationOK_Year && this.validationOK_Month)
		{
			int num = int.Parse(this.inYear.value);
			int num2 = int.Parse(this.inMonth.value);
			if (num2 > ServerDateTime.Now.Month && num == ServerDateTime.Now.Year)
			{
				this.inMonth.value = ServerDateTime.Now.Month.ToString();
			}
		}
	}

	private void UpdateSubmitButton()
	{
		if (this.validationOK_Year && this.validationOK_Month)
		{
			this.goSubmitButton_NG.SetActive(false);
			this.goSubmitButton_OK.SetActive(true);
		}
		else
		{
			this.goSubmitButton_NG.SetActive(true);
			this.goSubmitButton_OK.SetActive(false);
		}
	}

	public void ZeroPading()
	{
		if (this.inMonth.value.Length > 1)
		{
			return;
		}
		this.inMonth.value = "0" + this.inMonth.value;
	}

	protected void RecvAPI_UpdateBirthday(bool success)
	{
		RestrictionInput.EndLoad();
		if (success)
		{
			global::Debug.Log("[PRF ユーザー生年月の更新成功]");
			this.OnCloseGetBirthdayComplete();
		}
		else
		{
			global::Debug.Log("[PRF ユーザー生年月の更新失敗]");
			this.OnCloseUpdateBirthdayErr();
		}
	}

	protected void OnCloseUpdateBirthdayErr()
	{
		AlertManager.ShowAlertDialog(null, "C-SH03");
		this.UIUnLock();
	}

	protected void OnCloseGetBirthdayComplete()
	{
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.birthday = this.inYear.value + "-" + this.inMonth.value + "-01";
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int nop)
		{
			this.ClosePanel(true);
		}, "CMD_ModalMessage") as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("ShopAgeCompletedTitle");
		cmd_ModalMessage.Info = StringMaster.GetString("ShopAgeCompletedInfo");
	}

	public int GetCloseButtonIndex()
	{
		return base.TouchPanels.IndexOf(this.gColCloseBtn);
	}

	public void OnDeselectOrSubmit_Year()
	{
		this.ValidationYear();
	}

	public void OnDeselectOrSubmit_Month()
	{
		this.ValidationMonth();
	}
}
