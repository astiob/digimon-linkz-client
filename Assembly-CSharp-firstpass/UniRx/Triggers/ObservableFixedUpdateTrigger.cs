using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableFixedUpdateTrigger : ObservableTriggerBase
	{
		private Subject<Unit> fixedUpdate;

		private void FixedUpdate()
		{
			if (this.fixedUpdate != null)
			{
				this.fixedUpdate.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> FixedUpdateAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.fixedUpdate) == null)
			{
				result = (this.fixedUpdate = new Subject<Unit>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.fixedUpdate != null)
			{
				this.fixedUpdate.OnCompleted();
			}
		}
	}
}
