using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableVisibleTrigger : ObservableTriggerBase
	{
		private Subject<Unit> onBecameInvisible;

		private Subject<Unit> onBecameVisible;

		private void OnBecameInvisible()
		{
			if (this.onBecameInvisible != null)
			{
				this.onBecameInvisible.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnBecameInvisibleAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.onBecameInvisible) == null)
			{
				result = (this.onBecameInvisible = new Subject<Unit>());
			}
			return result;
		}

		private void OnBecameVisible()
		{
			if (this.onBecameVisible != null)
			{
				this.onBecameVisible.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnBecameVisibleAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.onBecameVisible) == null)
			{
				result = (this.onBecameVisible = new Subject<Unit>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onBecameInvisible != null)
			{
				this.onBecameInvisible.OnCompleted();
			}
			if (this.onBecameVisible != null)
			{
				this.onBecameVisible.OnCompleted();
			}
		}
	}
}
