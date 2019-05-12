using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservablePointerExitTrigger : ObservableTriggerBase, IEventSystemHandler, IPointerExitHandler
	{
		private Subject<PointerEventData> onPointerExit;

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			if (this.onPointerExit != null)
			{
				this.onPointerExit.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnPointerExitAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onPointerExit) == null)
			{
				result = (this.onPointerExit = new Subject<PointerEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onPointerExit != null)
			{
				this.onPointerExit.OnCompleted();
			}
		}
	}
}
