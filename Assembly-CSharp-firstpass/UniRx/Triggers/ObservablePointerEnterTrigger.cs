using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservablePointerEnterTrigger : ObservableTriggerBase, IEventSystemHandler, IPointerEnterHandler
	{
		private Subject<PointerEventData> onPointerEnter;

		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			if (this.onPointerEnter != null)
			{
				this.onPointerEnter.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnPointerEnterAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onPointerEnter) == null)
			{
				result = (this.onPointerEnter = new Subject<PointerEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onPointerEnter != null)
			{
				this.onPointerEnter.OnCompleted();
			}
		}
	}
}
