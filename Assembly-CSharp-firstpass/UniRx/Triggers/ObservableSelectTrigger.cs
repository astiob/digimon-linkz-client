using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableSelectTrigger : ObservableTriggerBase, IEventSystemHandler, ISelectHandler
	{
		private Subject<BaseEventData> onSelect;

		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			if (this.onSelect != null)
			{
				this.onSelect.OnNext(eventData);
			}
		}

		public IObservable<BaseEventData> OnSelectAsObservable()
		{
			Subject<BaseEventData> result;
			if ((result = this.onSelect) == null)
			{
				result = (this.onSelect = new Subject<BaseEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onSelect != null)
			{
				this.onSelect.OnCompleted();
			}
		}
	}
}
