using Master;
using System;
using UnityEngine;

public sealed class GUIListPresentBoxParts : GUIListPartBS
{
	[SerializeField]
	private UILabel lbDescription;

	[SerializeField]
	private UILabel lbLimitTime;

	[SerializeField]
	private UILabel lbGetButton;

	private GameWebAPI.RespDataPR_PrizeData.PrizeData prizeData;

	public GameWebAPI.RespDataPR_PrizeData.PrizeData Data
	{
		get
		{
			return this.prizeData;
		}
		set
		{
			this.prizeData = value;
			this.ShowGUI();
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public override void ShowGUI()
	{
		this.SetCommonUI();
		base.ShowGUI();
	}

	private void SetCommonUI()
	{
		this.lbDescription.text = this.prizeData.message;
		this.lbGetButton.text = StringMaster.GetString("Present-11");
		DateTime dateTime;
		if (DateTime.TryParse(this.prizeData.receiveLimitTime, out dateTime))
		{
			string str = dateTime.ToString("yyyy/MM/dd");
			this.lbLimitTime.text = StringMaster.GetString("Present-05") + str;
		}
		else
		{
			this.lbLimitTime.text = StringMaster.GetString("Present-05") + this.prizeData.receiveLimitTime.ToString();
		}
		PresentBoxItem component = base.gameObject.GetComponent<PresentBoxItem>();
		component.SetItem(this.prizeData.assetCategoryId, this.prizeData.assetValue, this.prizeData.assetNum, false, null);
	}

	private void OnClickGetPresentOne()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		CMD_ModalPresentBox.Instance.SetCandidateList(this.prizeData);
		GameWebAPI.RequestPR_PrizeReceive requestPR_PrizeReceive = new GameWebAPI.RequestPR_PrizeReceive();
		requestPR_PrizeReceive.SetSendData = delegate(GameWebAPI.PR_Req_PrizeReceiveIds param)
		{
			param.receiveType = 2;
			param.receiveIds = new string[]
			{
				this.prizeData.receiveId
			};
		};
		requestPR_PrizeReceive.OnReceived = delegate(GameWebAPI.RespDataPR_PrizeReceiveIds response)
		{
			CMD_ModalPresentBox.Instance.DispReceiveResultModal(response);
		};
		GameWebAPI.RequestPR_PrizeReceive request = requestPR_PrizeReceive;
		base.StartCoroutine(request.RunOneTime(new Action(this.OnSuccessGetPresent), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void OnSuccessGetPresent()
	{
		if (this.prizeData.assetCategoryId.ToInt32() == 16)
		{
			Singleton<UserDataMng>.Instance.ClearUserFacilityCondition();
		}
		RestrictionInput.EndLoad();
	}
}
