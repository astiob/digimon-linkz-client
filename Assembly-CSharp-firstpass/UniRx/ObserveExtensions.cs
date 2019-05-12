using System;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;

namespace UniRx
{
	public static class ObserveExtensions
	{
		public static IObservable<TProperty> ObserveEveryValueChanged<TSource, TProperty>(this TSource source, Func<TSource, TProperty> propertySelector, FrameCountType frameCountType = FrameCountType.Update, bool fastDestroyCheck = false) where TSource : class
		{
			return source.ObserveEveryValueChanged(propertySelector, frameCountType, UnityEqualityComparer.GetDefault<TProperty>(), fastDestroyCheck);
		}

		public static IObservable<TProperty> ObserveEveryValueChanged<TSource, TProperty>(this TSource source, Func<TSource, TProperty> propertySelector, FrameCountType frameCountType, IEqualityComparer<TProperty> comparer) where TSource : class
		{
			return source.ObserveEveryValueChanged(propertySelector, frameCountType, comparer, false);
		}

		public static IObservable<TProperty> ObserveEveryValueChanged<TSource, TProperty>(this TSource source, Func<TSource, TProperty> propertySelector, FrameCountType frameCountType, IEqualityComparer<TProperty> comparer, bool fastDestroyCheck) where TSource : class
		{
			ObserveExtensions.<ObserveEveryValueChanged>c__AnonStorey3<TSource, TProperty> <ObserveEveryValueChanged>c__AnonStorey = new ObserveExtensions.<ObserveEveryValueChanged>c__AnonStorey3<TSource, TProperty>();
			<ObserveEveryValueChanged>c__AnonStorey.propertySelector = propertySelector;
			<ObserveEveryValueChanged>c__AnonStorey.comparer = comparer;
			<ObserveEveryValueChanged>c__AnonStorey.fastDestroyCheck = fastDestroyCheck;
			if (source == null)
			{
				return Observable.Empty<TProperty>();
			}
			if (<ObserveEveryValueChanged>c__AnonStorey.comparer == null)
			{
				<ObserveEveryValueChanged>c__AnonStorey.comparer = UnityEqualityComparer.GetDefault<TProperty>();
			}
			<ObserveEveryValueChanged>c__AnonStorey.unityObject = (source as UnityEngine.Object);
			bool flag = source is UnityEngine.Object;
			if (flag && <ObserveEveryValueChanged>c__AnonStorey.unityObject == null)
			{
				return Observable.Empty<TProperty>();
			}
			if (flag)
			{
				return Observable.FromMicroCoroutine<TProperty>(delegate(IObserver<TProperty> observer, CancellationToken cancellationToken)
				{
					if (<ObserveEveryValueChanged>c__AnonStorey.unityObject != null)
					{
						TProperty tproperty = default(TProperty);
						try
						{
							tproperty = <ObserveEveryValueChanged>c__AnonStorey.propertySelector((TSource)((object)<ObserveEveryValueChanged>c__AnonStorey.unityObject));
						}
						catch (Exception error)
						{
							observer.OnError(error);
							return ObserveExtensions.EmptyEnumerator();
						}
						observer.OnNext(tproperty);
						return ObserveExtensions.PublishUnityObjectValueChanged<TSource, TProperty>(<ObserveEveryValueChanged>c__AnonStorey.unityObject, tproperty, <ObserveEveryValueChanged>c__AnonStorey.propertySelector, <ObserveEveryValueChanged>c__AnonStorey.comparer, observer, cancellationToken, <ObserveEveryValueChanged>c__AnonStorey.fastDestroyCheck);
					}
					observer.OnCompleted();
					return ObserveExtensions.EmptyEnumerator();
				}, frameCountType);
			}
			WeakReference reference = new WeakReference(source);
			source = (TSource)((object)null);
			return Observable.FromMicroCoroutine<TProperty>(delegate(IObserver<TProperty> observer, CancellationToken cancellationToken)
			{
				object target = reference.Target;
				if (target != null)
				{
					TProperty tproperty = default(TProperty);
					try
					{
						tproperty = <ObserveEveryValueChanged>c__AnonStorey.propertySelector((TSource)((object)target));
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return ObserveExtensions.EmptyEnumerator();
					}
					finally
					{
					}
					observer.OnNext(tproperty);
					return ObserveExtensions.PublishPocoValueChanged<TSource, TProperty>(reference, tproperty, <ObserveEveryValueChanged>c__AnonStorey.propertySelector, <ObserveEveryValueChanged>c__AnonStorey.comparer, observer, cancellationToken);
				}
				observer.OnCompleted();
				return ObserveExtensions.EmptyEnumerator();
			}, frameCountType);
		}

		private static IEnumerator EmptyEnumerator()
		{
			yield break;
		}

		private static IEnumerator PublishPocoValueChanged<TSource, TProperty>(WeakReference sourceReference, TProperty firstValue, Func<TSource, TProperty> propertySelector, IEqualityComparer<TProperty> comparer, IObserver<TProperty> observer, CancellationToken cancellationToken)
		{
			TProperty currentValue = default(TProperty);
			TProperty prevValue = firstValue;
			while (!cancellationToken.IsCancellationRequested)
			{
				object target = sourceReference.Target;
				if (target == null)
				{
					observer.OnCompleted();
					yield break;
				}
				try
				{
					currentValue = propertySelector((TSource)((object)target));
				}
				catch (Exception error)
				{
					observer.OnError(error);
					yield break;
				}
				finally
				{
					target = null;
				}
				if (!comparer.Equals(currentValue, prevValue))
				{
					observer.OnNext(currentValue);
					prevValue = currentValue;
				}
				yield return null;
			}
			yield break;
		}

		private static IEnumerator PublishUnityObjectValueChanged<TSource, TProperty>(UnityEngine.Object unityObject, TProperty firstValue, Func<TSource, TProperty> propertySelector, IEqualityComparer<TProperty> comparer, IObserver<TProperty> observer, CancellationToken cancellationToken, bool fastDestroyCheck)
		{
			TProperty currentValue = default(TProperty);
			TProperty prevValue = firstValue;
			TSource source = (TSource)((object)unityObject);
			if (fastDestroyCheck)
			{
				ObservableDestroyTrigger destroyTrigger = null;
				GameObject gameObject = unityObject as GameObject;
				if (gameObject == null)
				{
					Component component = unityObject as Component;
					if (component != null)
					{
						gameObject = component.gameObject;
					}
				}
				if (!(gameObject == null))
				{
					destroyTrigger = ObserveExtensions.GetOrAddDestroyTrigger(gameObject);
					while (!cancellationToken.IsCancellationRequested)
					{
						bool isDestroyed = (!destroyTrigger.IsActivated) ? (unityObject != null) : (!destroyTrigger.IsCalledOnDestroy);
						if (!isDestroyed)
						{
							observer.OnCompleted();
							yield break;
						}
						try
						{
							currentValue = propertySelector(source);
						}
						catch (Exception error)
						{
							observer.OnError(error);
							yield break;
						}
						if (!comparer.Equals(currentValue, prevValue))
						{
							observer.OnNext(currentValue);
							prevValue = currentValue;
						}
						yield return null;
					}
					yield break;
				}
			}
			while (!cancellationToken.IsCancellationRequested)
			{
				if (!(unityObject != null))
				{
					observer.OnCompleted();
					yield break;
				}
				try
				{
					currentValue = propertySelector(source);
				}
				catch (Exception error2)
				{
					observer.OnError(error2);
					yield break;
				}
				if (!comparer.Equals(currentValue, prevValue))
				{
					observer.OnNext(currentValue);
					prevValue = currentValue;
				}
				yield return null;
			}
			yield break;
		}

		private static ObservableDestroyTrigger GetOrAddDestroyTrigger(GameObject go)
		{
			ObservableDestroyTrigger observableDestroyTrigger = go.GetComponent<ObservableDestroyTrigger>();
			if (observableDestroyTrigger == null)
			{
				observableDestroyTrigger = go.AddComponent<ObservableDestroyTrigger>();
			}
			return observableDestroyTrigger;
		}
	}
}
