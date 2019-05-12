using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableDropTrigger : ObservableTriggerBase, IEventSystemHandler, IDropHandler
	{
		private Subject<PointerEventData> onDrop;

		void IDropHandler.OnDrop(PointerEventData eventData)
		{
			if (this.onDrop != null)
			{
				this.onDrop.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnDropAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onDrop) == null)
			{
				result = (this.onDrop = new Subject<PointerEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onDrop != null)
			{
				this.onDrop.OnCompleted();
			}
		}
	}
}
