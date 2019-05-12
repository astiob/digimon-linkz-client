using System;
using UnityEngine;

namespace UniRx.Triggers
{
	public abstract class ObservableTriggerBase : MonoBehaviour
	{
		private bool calledAwake;

		private Subject<Unit> awake;

		private bool calledStart;

		private Subject<Unit> start;

		private bool calledDestroy;

		private Subject<Unit> onDestroy;

		private void Awake()
		{
			this.calledAwake = true;
			if (this.awake != null)
			{
				this.awake.OnNext(Unit.Default);
				this.awake.OnCompleted();
			}
		}

		public IObservable<Unit> AwakeAsObservable()
		{
			if (this.calledAwake)
			{
				return Observable.Return(Unit.Default);
			}
			Subject<Unit> result;
			if ((result = this.awake) == null)
			{
				result = (this.awake = new Subject<Unit>());
			}
			return result;
		}

		private void Start()
		{
			this.calledStart = true;
			if (this.start != null)
			{
				this.start.OnNext(Unit.Default);
				this.start.OnCompleted();
			}
		}

		public IObservable<Unit> StartAsObservable()
		{
			if (this.calledStart)
			{
				return Observable.Return(Unit.Default);
			}
			Subject<Unit> result;
			if ((result = this.start) == null)
			{
				result = (this.start = new Subject<Unit>());
			}
			return result;
		}

		private void OnDestroy()
		{
			this.calledDestroy = true;
			if (this.onDestroy != null)
			{
				this.onDestroy.OnNext(Unit.Default);
				this.onDestroy.OnCompleted();
			}
			this.RaiseOnCompletedOnDestroy();
		}

		public IObservable<Unit> OnDestroyAsObservable()
		{
			if (this == null)
			{
				return Observable.Return(Unit.Default);
			}
			if (this.calledDestroy)
			{
				return Observable.Return(Unit.Default);
			}
			Subject<Unit> result;
			if ((result = this.onDestroy) == null)
			{
				result = (this.onDestroy = new Subject<Unit>());
			}
			return result;
		}

		protected abstract void RaiseOnCompletedOnDestroy();
	}
}
