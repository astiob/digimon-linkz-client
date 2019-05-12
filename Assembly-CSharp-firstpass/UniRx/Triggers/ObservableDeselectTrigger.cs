using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableDeselectTrigger : ObservableTriggerBase, IEventSystemHandler, IDeselectHandler
	{
		private Subject<BaseEventData> onDeselect;

		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			if (this.onDeselect != null)
			{
				this.onDeselect.OnNext(eventData);
			}
		}

		public IObservable<BaseEventData> OnDeselectAsObservable()
		{
			Subject<BaseEventData> result;
			if ((result = this.onDeselect) == null)
			{
				result = (this.onDeselect = new Subject<BaseEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onDeselect != null)
			{
				this.onDeselect.OnCompleted();
			}
		}
	}
}
