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
			return new TutorialNaviMasterList
			{
				OnReceived = new Action<TutorialNaviMasterResponse>(base.SetResponse)
			};
		}

		protected override void PrepareData(TutorialNaviMasterResponse src)
		{
			MasterDataMng.Instance().Tutorial.SetTutorialNaviMaster(src);
			this.data = src;
		}
	}
}
