using Master;
using System;
using UnityEngine;

public sealed class TutorialUserName : MonoBehaviour
{
	private const int mMaxByteCount = 20;

	[SerializeField]
	private UISprite fedeSprite;

	[SerializeField]
	private UIInput mUIInput;

	[SerializeField]
	private UILabel inputLabel;

	[SerializeField]
	private UILabel adviceLabel;

	[SerializeField]
	private UILabel submitButtonLabel;

	[SerializeField]
	private BoxCollider submitButtonCollider;

	[SerializeField]
	private BoxCollider userNameInpuCollider;

	[SerializeField]
	private GameObject submitButton;

	[SerializeField]
	private GameObject submitButtonGray;

	private Transform myTransform;

	public Action FinishAction { private get; set; }

	private void Awake()
	{
		this.fedeSprite.enabled = false;
		this.myTransform = base.transform;
		this.adviceLabel.text = StringMaster.GetString("TutorialNameCaution");
		this.submitButtonLabel.text = StringMaster.GetString("SystemButtonDecision");
		this.SetGrayButton(true);
		this.inputLabel.text = string.Empty;
		this.mUIInput.value = StringMaster.GetString("TutorialDefaultName");
	}

	public void OnSubmit()
	{
		if (TextUtil.IsOverLengthFullAndHalf(this.mUIInput.value, 20))
		{
			this.ShowDaialogOverTheLength();
		}
		else if (TextUtil.SurrogateCheck(this.mUIInput.value))
		{
			this.ShowDaialogForbiddenChar();
		}
		else
		{
			this.mUIInput.value = this.mUIInput.value;
			this.mUIInput.label.text = this.mUIInput.value;
		}
	}

	private void ShowDaialogOverTheLength()
	{
		if (AlertManager.ShowAlertDialog(null, "E-US08"))
		{
			this.SetGrayButton(true);
		}
	}

	private void ShowDaialogForbiddenChar()
	{
		if (AlertManager.ShowAlertDialog(null, "E-US17"))
		{
			this.mUIInput.value = string.Empty;
			this.SetGrayButton(true);
		}
	}

	public void CheckInput()
	{
		bool grayButton = false;
		if (string.IsNullOrEmpty(this.mUIInput.value))
		{
			grayButton = true;
		}
		this.mUIInput.value = this.mUIInput.value.Replace("\n", string.Empty).Replace("\r", string.Empty);
		this.mUIInput.label.text = this.mUIInput.value;
		this.SetGrayButton(grayButton);
	}

	private void SetGrayButton(bool isGray)
	{
		if (isGray)
		{
			this.submitButton.SetActive(false);
			this.submitButtonGray.SetActive(true);
		}
		else
		{
			this.submitButton.SetActive(true);
			this.submitButtonGray.SetActive(false);
		}
		this.submitButtonLabel.color = ((!isGray) ? Color.white : Color.gray);
		this.submitButtonCollider.enabled = !isGray;
	}

	private void PushedSubmitButton()
	{
		this.myTransform.SetY(3000f);
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.PushedDialog), "CMD_Confirm") as CMD_Confirm;
		cmd_Confirm.Title = StringMaster.GetString("TutorialNameConfirmTitle");
		cmd_Confirm.Info = string.Format(StringMaster.GetString("TutorialNameConfirmInfo"), this.inputLabel.text);
		cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonYes");
		cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonNo");
	}

	private void PushedDialog(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			GameWebAPI.RequestUS_UserUpdateNicknameLogic request = new GameWebAPI.RequestUS_UserUpdateNicknameLogic
			{
				SetSendData = delegate(GameWebAPI.PRF_Req_UpdateNickname param)
				{
					param.nickname = this.inputLabel.text;
				}
			};
			base.StartCoroutine(request.RunOneTime(delegate()
			{
				RestrictionInput.EndLoad();
				this.RecvAPI_UpdateNickname(true);
			}, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
				this.RecvAPI_UpdateNickname(false);
			}, null));
		}
		else
		{
			this.myTransform.SetLocalY(0f);
		}
	}

	private void RecvAPI_UpdateNickname(bool isSuccess)
	{
		if (isSuccess)
		{
			this.userNameInpuCollider.enabled = false;
			this.SetGrayButton(true);
			this.fedeSprite.enabled = true;
			iTween.ScaleTo(this.fedeSprite.gameObject, iTween.Hash(new object[]
			{
				"y",
				1000f,
				"time",
				0.3f,
				"easetype",
				iTween.EaseType.easeOutSine,
				"oncomplete",
				"GoNextPage",
				"oncompletetarget",
				base.gameObject
			}));
		}
		else
		{
			this.myTransform.SetY(0f);
		}
	}

	private void GoNextPage()
	{
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.nickname = this.inputLabel.text;
		this.FinishAction();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
