using Master;
using System;
using System.Collections;

namespace Title
{
	public sealed class AgreementConsent
	{
		private bool agreement;

		private IEnumerator OpenAgreement()
		{
			bool popupOpen = true;
			CMD_AgreementConsent dialog = GUIMain.ShowCommonDialog(delegate(int x)
			{
				popupOpen = false;
			}, "CMD_AgreementConsent", null) as CMD_AgreementConsent;
			dialog.SetActionAgreementPopupClosed(delegate(bool x)
			{
				this.agreement = x;
			});
			while (popupOpen)
			{
				yield return null;
			}
			yield break;
		}

		private IEnumerator OpenUpdateMessage()
		{
			bool popupOpen = true;
			CMD_ModalMessage dialog = GUIMain.ShowCommonDialog(delegate(int noop)
			{
				popupOpen = false;
			}, "CMD_ModalMessage", null) as CMD_ModalMessage;
			dialog.Title = StringMaster.GetString("AgreementTitle");
			dialog.Info = StringMaster.GetString("AgreementChangedInfo");
			while (popupOpen)
			{
				yield return null;
			}
			yield break;
		}

		public IEnumerator CheckAgreement(Action<bool> completed)
		{
			GameWebAPI.RespDataCM_Login.TutorialStatus tutorialStatus = DataMng.Instance().RespDataCM_Login.tutorialStatus;
			if ("0" == tutorialStatus.endFlg && "0" == tutorialStatus.statusId)
			{
				RestrictionInput.EndLoad();
				yield return AppCoroutine.Start(this.OpenAgreement(), false);
				if (this.agreement)
				{
					RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
				}
			}
			else if (!DataMng.Instance().RespDataCM_Login.ConfirmedPolicy())
			{
				RestrictionInput.EndLoad();
				yield return AppCoroutine.Start(this.OpenUpdateMessage(), false);
				yield return AppCoroutine.Start(this.OpenAgreement(), false);
				if (this.agreement)
				{
					RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
				}
			}
			else
			{
				this.agreement = true;
			}
			if (completed != null)
			{
				completed(this.agreement);
			}
			yield break;
		}
	}
}
