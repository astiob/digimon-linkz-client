using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableBeginDragTrigger : ObservableTriggerBase, IEventSystemHandler, IBeginDragHandler
	{
		private Subject<PointerEventData> onBeginDrag;

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			if (this.onBeginDrag != null)
			{
				this.onBeginDrag.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnBeginDragAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onBeginDrag) == null)
			{
				result = (this.onBeginDrag = new Subject<PointerEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onBeginDrag != null)
			{
				this.onBeginDrag.OnCompleted();
			}
		}
	}
}
