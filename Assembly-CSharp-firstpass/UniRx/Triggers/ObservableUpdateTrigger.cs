using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableUpdateTrigger : ObservableTriggerBase
	{
		private Subject<Unit> update;

		private void Update()
		{
			if (this.update != null)
			{
				this.update.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> UpdateAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.update) == null)
			{
				result = (this.update = new Subject<Unit>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.update != null)
			{
				this.update.OnCompleted();
			}
		}
	}
}
