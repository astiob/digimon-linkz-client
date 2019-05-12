using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableEndDragTrigger : ObservableTriggerBase, IEventSystemHandler, IEndDragHandler
	{
		private Subject<PointerEventData> onEndDrag;

		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			if (this.onEndDrag != null)
			{
				this.onEndDrag.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnEndDragAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onEndDrag) == null)
			{
				result = (this.onEndDrag = new Subject<PointerEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onEndDrag != null)
			{
				this.onEndDrag.OnCompleted();
			}
		}
	}
}
