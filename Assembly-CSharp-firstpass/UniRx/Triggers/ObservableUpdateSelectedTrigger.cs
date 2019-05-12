using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableUpdateSelectedTrigger : ObservableTriggerBase, IEventSystemHandler, IUpdateSelectedHandler
	{
		private Subject<BaseEventData> onUpdateSelected;

		void IUpdateSelectedHandler.OnUpdateSelected(BaseEventData eventData)
		{
			if (this.onUpdateSelected != null)
			{
				this.onUpdateSelected.OnNext(eventData);
			}
		}

		public IObservable<BaseEventData> OnUpdateSelectedAsObservable()
		{
			Subject<BaseEventData> result;
			if ((result = this.onUpdateSelected) == null)
			{
				result = (this.onUpdateSelected = new Subject<BaseEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onUpdateSelected != null)
			{
				this.onUpdateSelected.OnCompleted();
			}
		}
	}
}
