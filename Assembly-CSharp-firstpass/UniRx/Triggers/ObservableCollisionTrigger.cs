using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableCollisionTrigger : ObservableTriggerBase
	{
		private Subject<Collision> onCollisionEnter;

		private Subject<Collision> onCollisionExit;

		private Subject<Collision> onCollisionStay;

		private void OnCollisionEnter(Collision collision)
		{
			if (this.onCollisionEnter != null)
			{
				this.onCollisionEnter.OnNext(collision);
			}
		}

		public IObservable<Collision> OnCollisionEnterAsObservable()
		{
			Subject<Collision> result;
			if ((result = this.onCollisionEnter) == null)
			{
				result = (this.onCollisionEnter = new Subject<Collision>());
			}
			return result;
		}

		private void OnCollisionExit(Collision collisionInfo)
		{
			if (this.onCollisionExit != null)
			{
				this.onCollisionExit.OnNext(collisionInfo);
			}
		}

		public IObservable<Collision> OnCollisionExitAsObservable()
		{
			Subject<Collision> result;
			if ((result = this.onCollisionExit) == null)
			{
				result = (this.onCollisionExit = new Subject<Collision>());
			}
			return result;
		}

		private void OnCollisionStay(Collision collisionInfo)
		{
			if (this.onCollisionStay != null)
			{
				this.onCollisionStay.OnNext(collisionInfo);
			}
		}

		public IObservable<Collision> OnCollisionStayAsObservable()
		{
			Subject<Collision> result;
			if ((result = this.onCollisionStay) == null)
			{
				result = (this.onCollisionStay = new Subject<Collision>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onCollisionEnter != null)
			{
				this.onCollisionEnter.OnCompleted();
			}
			if (this.onCollisionExit != null)
			{
				this.onCollisionExit.OnCompleted();
			}
			if (this.onCollisionStay != null)
			{
				this.onCollisionStay.OnCompleted();
			}
		}
	}
}
