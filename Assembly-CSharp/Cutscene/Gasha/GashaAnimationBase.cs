using System;

namespace Cutscene.Gasha
{
	public abstract class GashaAnimationBase
	{
		private Action<GashaAnimationBase> endCallback;

		protected void EndCallback()
		{
			if (this.endCallback != null)
			{
				this.endCallback(this);
			}
		}

		public void ClearEndCallback()
		{
			this.endCallback = null;
		}

		public void SetEndCallback(Action<GashaAnimationBase> action)
		{
			this.endCallback = action;
		}
	}
}
