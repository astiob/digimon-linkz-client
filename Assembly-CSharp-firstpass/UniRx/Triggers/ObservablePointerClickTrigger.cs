using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservablePointerClickTrigger : ObservableTriggerBase, IEventSystemHandler, IPointerClickHandler
	{
		private Subject<PointerEventData> onPointerClick;

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			if (this.onPointerClick != null)
			{
				this.onPointerClick.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnPointerClickAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onPointerClick) == null)
			{
				result = (this.onPointerClick = new Subject<PointerEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onPointerClick != null)
			{
				this.onPointerClick.OnCompleted();
			}
		}
	}
}
