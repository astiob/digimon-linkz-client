using System;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;

namespace UniRx
{
	public static class DisposableExtensions
	{
		public static T AddTo<T>(this T disposable, ICollection<IDisposable> container) where T : IDisposable
		{
			if (disposable == null)
			{
				throw new ArgumentNullException("disposable");
			}
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(disposable);
			return disposable;
		}

		public static T AddTo<T>(this T disposable, GameObject gameObject) where T : IDisposable
		{
			if (gameObject == null)
			{
				disposable.Dispose();
				return disposable;
			}
			ObservableDestroyTrigger observableDestroyTrigger = gameObject.GetComponent<ObservableDestroyTrigger>();
			if (observableDestroyTrigger == null)
			{
				observableDestroyTrigger = gameObject.AddComponent<ObservableDestroyTrigger>();
			}
			if (!observableDestroyTrigger.IsActivated && !observableDestroyTrigger.IsMonitoredActivate && !observableDestroyTrigger.gameObject.activeInHierarchy)
			{
				observableDestroyTrigger.IsMonitoredActivate = true;
				MainThreadDispatcher.StartEndOfFrameMicroCoroutine(DisposableExtensions.MonitorTriggerHealth(observableDestroyTrigger, gameObject));
			}
			observableDestroyTrigger.AddDisposableOnDestroy(disposable);
			return disposable;
		}

		private static IEnumerator MonitorTriggerHealth(ObservableDestroyTrigger trigger, GameObject targetGameObject)
		{
			for (;;)
			{
				yield return null;
				if (trigger.IsActivated)
				{
					break;
				}
				if (targetGameObject == null)
				{
					goto Block_1;
				}
			}
			yield break;
			Block_1:
			trigger.ForceRaiseOnDestroy();
			yield break;
			yield break;
		}

		public static T AddTo<T>(this T disposable, Component gameObjectComponent) where T : IDisposable
		{
			if (gameObjectComponent == null)
			{
				disposable.Dispose();
				return disposable;
			}
			return disposable.AddTo(gameObjectComponent.gameObject);
		}

		public static T AddTo<T>(this T disposable, ICollection<IDisposable> container, GameObject gameObject) where T : IDisposable
		{
			return disposable.AddTo(container).AddTo(gameObject);
		}

		public static T AddTo<T>(this T disposable, ICollection<IDisposable> container, Component gameObjectComponent) where T : IDisposable
		{
			return disposable.AddTo(container).AddTo(gameObjectComponent);
		}
	}
}
