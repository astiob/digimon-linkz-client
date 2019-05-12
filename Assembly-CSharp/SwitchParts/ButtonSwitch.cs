using System;
using UnityEngine;

namespace SwitchParts
{
	public sealed class ButtonSwitch : SwitchPartsBase
	{
		[SerializeField]
		private ButtonSwitch.SwitchInfo on;

		[SerializeField]
		private ButtonSwitch.SwitchInfo off;

		[SerializeField]
		private UISprite background;

		public override void Switch(bool enable)
		{
			if (enable)
			{
				this.background.spriteName = this.on.spriteName;
			}
			else
			{
				this.background.spriteName = this.off.spriteName;
			}
		}

		public override bool Status()
		{
			return this.background.spriteName == this.on.spriteName;
		}

		[Serializable]
		private sealed class SwitchInfo
		{
			public string spriteName;
		}
	}
}
