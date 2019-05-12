using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableTransformChangedTrigger : ObservableTriggerBase
	{
		private Subject<Unit> onBeforeTransformParentChanged;

		private Subject<Unit> onTransformParentChanged;

		private Subject<Unit> onTransformChildrenChanged;

		private void OnBeforeTransformParentChanged()
		{
			if (this.onBeforeTransformParentChanged != null)
			{
				this.onBeforeTransformParentChanged.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnBeforeTransformParentChangedAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.onBeforeTransformParentChanged) == null)
			{
				result = (this.onBeforeTransformParentChanged = new Subject<Unit>());
			}
			return result;
		}

		private void OnTransformParentChanged()
		{
			if (this.onTransformParentChanged != null)
			{
				this.onTransformParentChanged.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnTransformParentChangedAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.onTransformParentChanged) == null)
			{
				result = (this.onTransformParentChanged = new Subject<Unit>());
			}
			return result;
		}

		private void OnTransformChildrenChanged()
		{
			if (this.onTransformChildrenChanged != null)
			{
				this.onTransformChildrenChanged.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnTransformChildrenChangedAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.onTransformChildrenChanged) == null)
			{
				result = (this.onTransformChildrenChanged = new Subject<Unit>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onBeforeTransformParentChanged != null)
			{
				this.onBeforeTransformParentChanged.OnCompleted();
			}
			if (this.onTransformParentChanged != null)
			{
				this.onTransformParentChanged.OnCompleted();
			}
			if (this.onTransformChildrenChanged != null)
			{
				this.onTransformChildrenChanged.OnCompleted();
			}
		}
	}
}
