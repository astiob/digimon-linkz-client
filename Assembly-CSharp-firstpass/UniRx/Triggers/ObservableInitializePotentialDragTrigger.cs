using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableInitializePotentialDragTrigger : ObservableTriggerBase, IEventSystemHandler, IInitializePotentialDragHandler
	{
		private Subject<PointerEventData> onInitializePotentialDrag;

		void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
		{
			if (this.onInitializePotentialDrag != null)
			{
				this.onInitializePotentialDrag.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnInitializePotentialDragAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onInitializePotentialDrag) == null)
			{
				result = (this.onInitializePotentialDrag = new Subject<PointerEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onInitializePotentialDrag != null)
			{
				this.onInitializePotentialDrag.OnCompleted();
			}
		}
	}
}
