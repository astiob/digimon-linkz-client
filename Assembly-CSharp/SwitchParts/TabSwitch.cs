using System;
using UnityEngine;

namespace SwitchParts
{
	public sealed class TabSwitch : SwitchPartsBase
	{
		[SerializeField]
		private TabSwitch.SwitchInfo on;

		[SerializeField]
		private TabSwitch.SwitchInfo off;

		[SerializeField]
		private UISprite leftBackground;

		[SerializeField]
		private UISprite rightBackground;

		[SerializeField]
		private UILabel label;

		public override void Switch(bool enable)
		{
			if (enable)
			{
				this.leftBackground.spriteName = this.on.spriteName;
				this.rightBackground.spriteName = this.on.spriteName;
				this.label.effectColor = this.on.textOutLineColor;
			}
			else
			{
				this.leftBackground.spriteName = this.off.spriteName;
				this.rightBackground.spriteName = this.off.spriteName;
				this.label.effectColor = this.off.textOutLineColor;
			}
		}

		public override bool Status()
		{
			return this.leftBackground.spriteName == this.on.spriteName;
		}

		[Serializable]
		private sealed class SwitchInfo
		{
			[SerializeField]
			public string spriteName;

			[SerializeField]
			public Color textOutLineColor;
		}
	}
}
