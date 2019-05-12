using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableTriggerTrigger : ObservableTriggerBase
	{
		private Subject<Collider> onTriggerEnter;

		private Subject<Collider> onTriggerExit;

		private Subject<Collider> onTriggerStay;

		private void OnTriggerEnter(Collider other)
		{
			if (this.onTriggerEnter != null)
			{
				this.onTriggerEnter.OnNext(other);
			}
		}

		public IObservable<Collider> OnTriggerEnterAsObservable()
		{
			Subject<Collider> result;
			if ((result = this.onTriggerEnter) == null)
			{
				result = (this.onTriggerEnter = new Subject<Collider>());
			}
			return result;
		}

		private void OnTriggerExit(Collider other)
		{
			if (this.onTriggerExit != null)
			{
				this.onTriggerExit.OnNext(other);
			}
		}

		public IObservable<Collider> OnTriggerExitAsObservable()
		{
			Subject<Collider> result;
			if ((result = this.onTriggerExit) == null)
			{
				result = (this.onTriggerExit = new Subject<Collider>());
			}
			return result;
		}

		private void OnTriggerStay(Collider other)
		{
			if (this.onTriggerStay != null)
			{
				this.onTriggerStay.OnNext(other);
			}
		}

		public IObservable<Collider> OnTriggerStayAsObservable()
		{
			Subject<Collider> result;
			if ((result = this.onTriggerStay) == null)
			{
				result = (this.onTriggerStay = new Subject<Collider>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onTriggerEnter != null)
			{
				this.onTriggerEnter.OnCompleted();
			}
			if (this.onTriggerExit != null)
			{
				this.onTriggerExit.OnCompleted();
			}
			if (this.onTriggerStay != null)
			{
				this.onTriggerStay.OnCompleted();
			}
		}
	}
}
