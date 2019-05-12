using System;

namespace UI.Common
{
	public class UIAssetsNumberComma : UIAssetsNumber
	{
		protected override string ConvertFormat(string number)
		{
			int num = 0;
			int.TryParse(number, out num);
			return num.ToString("#,0");
		}
	}
}
