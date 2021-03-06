﻿using System;
using UnityEngine.Events;

namespace UniRx
{
	public static class UnityEventExtensions
	{
		public static IObservable<Unit> AsObservable(this UnityEvent unityEvent)
		{
			return Observable.FromEvent<UnityAction>((Action h) => new UnityAction(h.Invoke), delegate(UnityAction h)
			{
				unityEvent.AddListener(h);
			}, delegate(UnityAction h)
			{
				unityEvent.RemoveListener(h);
			});
		}

		public static IObservable<T> AsObservable<T>(this UnityEvent<T> unityEvent)
		{
			return Observable.FromEvent<UnityAction<T>, T>((Action<T> h) => new UnityAction<T>(h.Invoke), delegate(UnityAction<T> h)
			{
				unityEvent.AddListener(h);
			}, delegate(UnityAction<T> h)
			{
				unityEvent.RemoveListener(h);
			});
		}

		public static IObservable<Tuple<T0, T1>> AsObservable<T0, T1>(this UnityEvent<T0, T1> unityEvent)
		{
			return Observable.FromEvent<UnityAction<T0, T1>, Tuple<T0, T1>>((Action<Tuple<T0, T1>> h) => delegate(T0 t0, T1 t1)
			{
				h(Tuple.Create<T0, T1>(t0, t1));
			}, delegate(UnityAction<T0, T1> h)
			{
				unityEvent.AddListener(h);
			}, delegate(UnityAction<T0, T1> h)
			{
				unityEvent.RemoveListener(h);
			});
		}

		public static IObservable<Tuple<T0, T1, T2>> AsObservable<T0, T1, T2>(this UnityEvent<T0, T1, T2> unityEvent)
		{
			return Observable.FromEvent<UnityAction<T0, T1, T2>, Tuple<T0, T1, T2>>((Action<Tuple<T0, T1, T2>> h) => delegate(T0 t0, T1 t1, T2 t2)
			{
				h(Tuple.Create<T0, T1, T2>(t0, t1, t2));
			}, delegate(UnityAction<T0, T1, T2> h)
			{
				unityEvent.AddListener(h);
			}, delegate(UnityAction<T0, T1, T2> h)
			{
				unityEvent.RemoveListener(h);
			});
		}

		public static IObservable<Tuple<T0, T1, T2, T3>> AsObservable<T0, T1, T2, T3>(this UnityEvent<T0, T1, T2, T3> unityEvent)
		{
			return Observable.FromEvent<UnityAction<T0, T1, T2, T3>, Tuple<T0, T1, T2, T3>>((Action<Tuple<T0, T1, T2, T3>> h) => delegate(T0 t0, T1 t1, T2 t2, T3 t3)
			{
				h(Tuple.Create<T0, T1, T2, T3>(t0, t1, t2, t3));
			}, delegate(UnityAction<T0, T1, T2, T3> h)
			{
				unityEvent.AddListener(h);
			}, delegate(UnityAction<T0, T1, T2, T3> h)
			{
				unityEvent.RemoveListener(h);
			});
		}
	}
}
