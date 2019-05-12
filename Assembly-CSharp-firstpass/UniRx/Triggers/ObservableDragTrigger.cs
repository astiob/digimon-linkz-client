using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableDragTrigger : ObservableTriggerBase, IEventSystemHandler, IDragHandler
	{
		private Subject<PointerEventData> onDrag;

		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (this.onDrag != null)
			{
				this.onDrag.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnDragAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onDrag) == null)
			{
				result = (this.onDrag = new Subject<PointerEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onDrag != null)
			{
				this.onDrag.OnCompleted();
			}
		}
	}
}
