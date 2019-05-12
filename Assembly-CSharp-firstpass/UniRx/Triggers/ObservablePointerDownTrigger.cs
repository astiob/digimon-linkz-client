using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservablePointerDownTrigger : ObservableTriggerBase, IEventSystemHandler, IPointerDownHandler
	{
		private Subject<PointerEventData> onPointerDown;

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.onPointerDown != null)
			{
				this.onPointerDown.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnPointerDownAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onPointerDown) == null)
			{
				result = (this.onPointerDown = new Subject<PointerEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onPointerDown != null)
			{
				this.onPointerDown.OnCompleted();
			}
		}
	}
}
