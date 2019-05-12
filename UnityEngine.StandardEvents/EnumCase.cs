using System;

namespace UnityEngine.Analytics
{
	public class EnumCase : AnalyticsEventAttribute
	{
		public EnumCase.Styles Style;

		public EnumCase(EnumCase.Styles style)
		{
			this.Style = style;
		}

		public enum Styles
		{
			None,
			Snake,
			Lower
		}
	}
}
