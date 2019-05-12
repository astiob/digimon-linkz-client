using System;

namespace UI.Common
{
	public static class FactoryLimitOverNotice
	{
		public static void CreateDialog(MasterDataMng.AssetCategory category, LimitOverNoticeType type)
		{
			if (category != MasterDataMng.AssetCategory.MONSTER)
			{
				if (category == MasterDataMng.AssetCategory.CHIP)
				{
					CMD_UpperlimitChip cmd_UpperlimitChip = GUIMain.ShowCommonDialog(null, "CMD_UpperlimitChip", null) as CMD_UpperlimitChip;
					cmd_UpperlimitChip.SetNoticeMessage(type);
				}
			}
			else
			{
				CMD_UpperLimit cmd_UpperLimit = GUIMain.ShowCommonDialog(null, "CMD_Upperlimit", null) as CMD_UpperLimit;
				cmd_UpperLimit.SetNoticeMessage(type);
			}
		}
	}
}
