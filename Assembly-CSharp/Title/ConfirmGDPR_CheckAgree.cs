using SwitchParts;
using System;
using UnityEngine;

namespace Title
{
	public sealed class ConfirmGDPR_CheckAgree : MonoBehaviour
	{
		[SerializeField]
		private CheckBox checkBox;

		private Action<bool> pushAction;

		private void PushCheckBox()
		{
			this.checkBox.Switch(this.checkBox.Status() ^ true);
			if (this.pushAction != null)
			{
				this.pushAction(this.checkBox.Status());
			}
		}

		public void SetPushAction(Action<bool> action)
		{
			this.pushAction = action;
		}

		public void SetCheckBoxStatus(bool active)
		{
			this.checkBox.Switch(active);
		}
	}
}
