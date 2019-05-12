using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableCancelTrigger : ObservableTriggerBase, IEventSystemHandler, ICancelHandler
	{
		private Subject<BaseEventData> onCancel;

		void ICancelHandler.OnCancel(BaseEventData eventData)
		{
			if (this.onCancel != null)
			{
				this.onCancel.OnNext(eventData);
			}
		}

		public IObservable<BaseEventData> OnCancelAsObservable()
		{
			Subject<BaseEventData> result;
			if ((result = this.onCancel) == null)
			{
				result = (this.onCancel = new Subject<BaseEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onCancel != null)
			{
				this.onCancel.OnCompleted();
			}
		}
	}
}
