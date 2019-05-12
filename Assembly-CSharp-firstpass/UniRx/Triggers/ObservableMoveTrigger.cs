using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableMoveTrigger : ObservableTriggerBase, IEventSystemHandler, IMoveHandler
	{
		private Subject<AxisEventData> onMove;

		void IMoveHandler.OnMove(AxisEventData eventData)
		{
			if (this.onMove != null)
			{
				this.onMove.OnNext(eventData);
			}
		}

		public IObservable<AxisEventData> OnMoveAsObservable()
		{
			Subject<AxisEventData> result;
			if ((result = this.onMove) == null)
			{
				result = (this.onMove = new Subject<AxisEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onMove != null)
			{
				this.onMove.OnCompleted();
			}
		}
	}
}
