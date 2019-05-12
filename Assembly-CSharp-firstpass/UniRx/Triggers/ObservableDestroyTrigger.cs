using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableDestroyTrigger : MonoBehaviour
	{
		private bool calledDestroy;

		private Subject<Unit> onDestroy;

		private CompositeDisposable disposablesOnDestroy;

		[Obsolete("Internal Use.")]
		internal bool IsMonitoredActivate { get; set; }

		public bool IsActivated { get; private set; }

		public bool IsCalledOnDestroy
		{
			get
			{
				return this.calledDestroy;
			}
		}

		private void Awake()
		{
			this.IsActivated = true;
		}

		private void OnDestroy()
		{
			if (!this.calledDestroy)
			{
				this.calledDestroy = true;
				if (this.disposablesOnDestroy != null)
				{
					this.disposablesOnDestroy.Dispose();
				}
				if (this.onDestroy != null)
				{
					this.onDestroy.OnNext(Unit.Default);
					this.onDestroy.OnCompleted();
				}
			}
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

		public void ForceRaiseOnDestroy()
		{
			this.OnDestroy();
		}

		public void AddDisposableOnDestroy(IDisposable disposable)
		{
			if (this.calledDestroy)
			{
				disposable.Dispose();
				return;
			}
			if (this.disposablesOnDestroy == null)
			{
				this.disposablesOnDestroy = new CompositeDisposable();
			}
			this.disposablesOnDestroy.Add(disposable);
		}
	}
}
