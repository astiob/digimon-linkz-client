using System;
using System.ComponentModel;

namespace System
{
	[AttributeUsage(AttributeTargets.All)]
	internal class SRDescriptionAttribute : System.ComponentModel.DescriptionAttribute
	{
		private bool isReplaced;

		public SRDescriptionAttribute(string description) : base(description)
		{
		}

		public override string Description
		{
			get
			{
				if (!this.isReplaced)
				{
					this.isReplaced = true;
					base.DescriptionValue = Locale.GetText(base.DescriptionValue);
				}
				return base.DescriptionValue;
			}
		}
	}
}
