using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableParticleTrigger : ObservableTriggerBase
	{
		private Subject<GameObject> onParticleCollision;

		private Subject<Unit> onParticleTrigger;

		private void OnParticleCollision(GameObject other)
		{
			if (this.onParticleCollision != null)
			{
				this.onParticleCollision.OnNext(other);
			}
		}

		public IObservable<GameObject> OnParticleCollisionAsObservable()
		{
			Subject<GameObject> result;
			if ((result = this.onParticleCollision) == null)
			{
				result = (this.onParticleCollision = new Subject<GameObject>());
			}
			return result;
		}

		private void OnParticleTrigger()
		{
			if (this.onParticleTrigger != null)
			{
				this.onParticleTrigger.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnParticleTriggerAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.onParticleTrigger) == null)
			{
				result = (this.onParticleTrigger = new Subject<Unit>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onParticleCollision != null)
			{
				this.onParticleCollision.OnCompleted();
			}
			if (this.onParticleTrigger != null)
			{
				this.onParticleTrigger.OnCompleted();
			}
		}
	}
}
