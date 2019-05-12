using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableSubmitTrigger : ObservableTriggerBase, IEventSystemHandler, ISubmitHandler
	{
		private Subject<BaseEventData> onSubmit;

		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			if (this.onSubmit != null)
			{
				this.onSubmit.OnNext(eventData);
			}
		}

		public IObservable<BaseEventData> OnSubmitAsObservable()
		{
			Subject<BaseEventData> result;
			if ((result = this.onSubmit) == null)
			{
				result = (this.onSubmit = new Subject<BaseEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onSubmit != null)
			{
				this.onSubmit.OnCompleted();
			}
		}
	}
}
