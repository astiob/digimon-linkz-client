using FarmData;
using System;
using System.Linq;

public class FacilityUpgradeConfirm
{
	protected CMD_UpgradeConfirmation parentUI;

	protected UserFacility userFacility;

	protected FarmObject targetFarmObject;

	public FacilityUpgradeConfirm(CMD_UpgradeConfirmation upgradeConfirmationUI, UserFacility userFacility)
	{
		this.parentUI = upgradeConfirmationUI;
		this.userFacility = userFacility;
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			this.targetFarmObject = instance.Scenery.farmObjects.SingleOrDefault((FarmObject x) => x.userFacilityID == userFacility.userFacilityId);
		}
	}

	public virtual void Upgrade()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		this.parentUI.StartCoroutine(this.RequestUpgrade().Run(delegate
		{
			this.parentUI.ClosePanel(true);
			RestrictionInput.EndLoad();
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	protected APIRequestTask RequestUpgrade()
	{
		RequestFA_FacilityUpgrade request = new RequestFA_FacilityUpgrade
		{
			SetSendData = delegate(FacilityUpgrade param)
			{
				param.userFacilityId = this.userFacility.userFacilityId;
			},
			OnReceived = delegate(WebAPI.ResponseData nop)
			{
				FarmRoot instance = FarmRoot.Instance;
				if (null != instance)
				{
					FarmScenery scenery = instance.Scenery;
					scenery.StartUpgrade(this.userFacility);
				}
			}
		};
		return new APIRequestTask(request, false);
	}
}
