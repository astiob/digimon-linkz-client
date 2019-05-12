using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableScrollTrigger : ObservableTriggerBase, IEventSystemHandler, IScrollHandler
	{
		private Subject<PointerEventData> onScroll;

		void IScrollHandler.OnScroll(PointerEventData eventData)
		{
			if (this.onScroll != null)
			{
				this.onScroll.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnScrollAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onScroll) == null)
			{
				result = (this.onScroll = new Subject<PointerEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onScroll != null)
			{
				this.onScroll.OnCompleted();
			}
		}
	}
}
