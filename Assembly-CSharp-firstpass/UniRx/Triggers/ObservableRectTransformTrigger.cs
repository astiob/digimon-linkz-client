using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableRectTransformTrigger : ObservableTriggerBase
	{
		private Subject<Unit> onRectTransformDimensionsChange;

		private Subject<Unit> onRectTransformRemoved;

		public void OnRectTransformDimensionsChange()
		{
			if (this.onRectTransformDimensionsChange != null)
			{
				this.onRectTransformDimensionsChange.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnRectTransformDimensionsChangeAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.onRectTransformDimensionsChange) == null)
			{
				result = (this.onRectTransformDimensionsChange = new Subject<Unit>());
			}
			return result;
		}

		public void OnRectTransformRemoved()
		{
			if (this.onRectTransformRemoved != null)
			{
				this.onRectTransformRemoved.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnRectTransformRemovedAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.onRectTransformRemoved) == null)
			{
				result = (this.onRectTransformRemoved = new Subject<Unit>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onRectTransformDimensionsChange != null)
			{
				this.onRectTransformDimensionsChange.OnCompleted();
			}
			if (this.onRectTransformRemoved != null)
			{
				this.onRectTransformRemoved.OnCompleted();
			}
		}
	}
}
