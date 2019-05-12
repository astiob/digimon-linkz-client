using System;
using UnityEngine;

namespace SwitchParts
{
	public sealed class CheckBox : SwitchPartsBase
	{
		[SerializeField]
		private UISprite check;

		public override void Switch(bool enable)
		{
			this.check.enabled = enable;
		}

		public override bool Status()
		{
			return this.check.enabled;
		}
	}
}
