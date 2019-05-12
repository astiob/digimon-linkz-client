using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableCanvasGroupChangedTrigger : ObservableTriggerBase
	{
		private Subject<Unit> onCanvasGroupChanged;

		private void OnCanvasGroupChanged()
		{
			if (this.onCanvasGroupChanged != null)
			{
				this.onCanvasGroupChanged.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnCanvasGroupChangedAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.onCanvasGroupChanged) == null)
			{
				result = (this.onCanvasGroupChanged = new Subject<Unit>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onCanvasGroupChanged != null)
			{
				this.onCanvasGroupChanged.OnCompleted();
			}
		}
	}
}
