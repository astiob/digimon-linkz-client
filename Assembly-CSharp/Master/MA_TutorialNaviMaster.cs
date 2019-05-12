using System;
using TutorialRequestHeader;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_TutorialNaviMaster : MasterBaseData<TutorialNaviMasterResponse>
	{
		public MA_TutorialNaviMaster()
		{
			base.ID = MasterId.TUTORIAL_NAVI_MASTER;
		}

		public override string GetTableName()
		{
			return "tutorial_navi_m";
		}

		public override RequestBase CreateRequest()
		{
			TutorialNaviMasterList tutorialNaviMasterList = new TutorialNaviMasterList();
			tutorialNaviMasterList.SetSendData = delegate(TutorialNaviMasterListQuery requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			tutorialNaviMasterList.OnReceived = new Action<TutorialNaviMasterResponse>(base.SetResponse);
			return tutorialNaviMasterList;
		}

		protected override void PrepareData(TutorialNaviMasterResponse src)
		{
			MasterDataMng.Instance().Tutorial.SetTutorialNaviMaster(src);
			this.data = src;
		}
	}
}
