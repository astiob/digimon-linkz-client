using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableLateUpdateTrigger : ObservableTriggerBase
	{
		private Subject<Unit> lateUpdate;

		private void LateUpdate()
		{
			if (this.lateUpdate != null)
			{
				this.lateUpdate.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> LateUpdateAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.lateUpdate) == null)
			{
				result = (this.lateUpdate = new Subject<Unit>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.lateUpdate != null)
			{
				this.lateUpdate.OnCompleted();
			}
		}
	}
}
