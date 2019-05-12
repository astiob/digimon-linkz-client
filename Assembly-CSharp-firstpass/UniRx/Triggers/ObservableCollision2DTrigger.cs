using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableCollision2DTrigger : ObservableTriggerBase
	{
		private Subject<Collision2D> onCollisionEnter2D;

		private Subject<Collision2D> onCollisionExit2D;

		private Subject<Collision2D> onCollisionStay2D;

		private void OnCollisionEnter2D(Collision2D coll)
		{
			if (this.onCollisionEnter2D != null)
			{
				this.onCollisionEnter2D.OnNext(coll);
			}
		}

		public IObservable<Collision2D> OnCollisionEnter2DAsObservable()
		{
			Subject<Collision2D> result;
			if ((result = this.onCollisionEnter2D) == null)
			{
				result = (this.onCollisionEnter2D = new Subject<Collision2D>());
			}
			return result;
		}

		private void OnCollisionExit2D(Collision2D coll)
		{
			if (this.onCollisionExit2D != null)
			{
				this.onCollisionExit2D.OnNext(coll);
			}
		}

		public IObservable<Collision2D> OnCollisionExit2DAsObservable()
		{
			Subject<Collision2D> result;
			if ((result = this.onCollisionExit2D) == null)
			{
				result = (this.onCollisionExit2D = new Subject<Collision2D>());
			}
			return result;
		}

		private void OnCollisionStay2D(Collision2D coll)
		{
			if (this.onCollisionStay2D != null)
			{
				this.onCollisionStay2D.OnNext(coll);
			}
		}

		public IObservable<Collision2D> OnCollisionStay2DAsObservable()
		{
			Subject<Collision2D> result;
			if ((result = this.onCollisionStay2D) == null)
			{
				result = (this.onCollisionStay2D = new Subject<Collision2D>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onCollisionEnter2D != null)
			{
				this.onCollisionEnter2D.OnCompleted();
			}
			if (this.onCollisionExit2D != null)
			{
				this.onCollisionExit2D.OnCompleted();
			}
			if (this.onCollisionStay2D != null)
			{
				this.onCollisionStay2D.OnCompleted();
			}
		}
	}
}
