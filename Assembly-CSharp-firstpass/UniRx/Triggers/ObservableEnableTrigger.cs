using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableEnableTrigger : ObservableTriggerBase
	{
		private Subject<Unit> onEnable;

		private Subject<Unit> onDisable;

		private void OnEnable()
		{
			if (this.onEnable != null)
			{
				this.onEnable.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnEnableAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.onEnable) == null)
			{
				result = (this.onEnable = new Subject<Unit>());
			}
			return result;
		}

		private void OnDisable()
		{
			if (this.onDisable != null)
			{
				this.onDisable.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnDisableAsObservable()
		{
			Subject<Unit> result;
			if ((result = this.onDisable) == null)
			{
				result = (this.onDisable = new Subject<Unit>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onEnable != null)
			{
				this.onEnable.OnCompleted();
			}
			if (this.onDisable != null)
			{
				this.onDisable.OnCompleted();
			}
		}
	}
}
