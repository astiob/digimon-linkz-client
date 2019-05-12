using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableAnimatorTrigger : ObservableTriggerBase
	{
		private Subject<int> onAnimatorIK;

		private Subject<Unit> onAnimatorMove;

		private void OnAnimatorIK(int layerIndex)
		{
			if (this.onAnimatorIK != null)
			{
				this.onAnimatorIK.OnNext(layerIndex);
			}
		}

		public IObservable<int> OnAnimatorIKAsObservable()
		{
			Subject<int> result;
			if ((result = this.onAnimatorIK) == null)
			{
				result = (this.onAnimatorIK = new Subject<int>());
			}
			return result;
		}

		private void OnAnimatorMove()
		{
			if (this.onAnimatorMove != null)
			{
				this.onAnimatorMove.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnAnimatorMoveAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.onAnimatorMove) == null)
			{
				result = (this.onAnimatorMove = new Subject<Unit>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onAnimatorIK != null)
			{
				this.onAnimatorIK.OnCompleted();
			}
			if (this.onAnimatorMove != null)
			{
				this.onAnimatorMove.OnCompleted();
			}
		}
	}
}
