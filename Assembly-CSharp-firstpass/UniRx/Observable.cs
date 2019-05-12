using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UniRx.Operators;
using UniRx.Triggers;
using UnityEngine;

namespace UniRx
{
	public static class Observable
	{
		private static readonly TimeSpan InfiniteTimeSpan = new TimeSpan(0, 0, 0, 0, -1);

		private static readonly HashSet<Type> YieldInstructionTypes = new HashSet<Type>
		{
			typeof(WWW),
			typeof(WaitForEndOfFrame),
			typeof(WaitForFixedUpdate),
			typeof(WaitForSeconds),
			typeof(AsyncOperation),
			typeof(Coroutine)
		};

		[CompilerGenerated]
		private static Func<IObserver<long>, CancellationToken, IEnumerator> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<IObserver<long>, CancellationToken, IEnumerator> <>f__mg$cache1;

		[CompilerGenerated]
		private static Func<IObserver<long>, CancellationToken, IEnumerator> <>f__mg$cache2;

		[CompilerGenerated]
		private static Func<IObserver<Unit>, CancellationToken, IEnumerator> <>f__mg$cache3;

		private static IObservable<T> AddRef<T>(IObservable<T> xs, RefCountDisposable r)
		{
			return Observable.Create<T>((IObserver<T> observer) => new CompositeDisposable(new IDisposable[]
			{
				r.GetDisposable(),
				xs.Subscribe(observer)
			}));
		}

		public static IObservable<TSource> Scan<TSource>(this IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
		{
			return new ScanObservable<TSource>(source, accumulator);
		}

		public static IObservable<TAccumulate> Scan<TSource, TAccumulate>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
		{
			return new ScanObservable<TSource, TAccumulate>(source, seed, accumulator);
		}

		public static IObservable<TSource> Aggregate<TSource>(this IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
		{
			return new AggregateObservable<TSource>(source, accumulator);
		}

		public static IObservable<TAccumulate> Aggregate<TSource, TAccumulate>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
		{
			return new AggregateObservable<TSource, TAccumulate>(source, seed, accumulator);
		}

		public static IObservable<TResult> Aggregate<TSource, TAccumulate, TResult>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector)
		{
			return new AggregateObservable<TSource, TAccumulate, TResult>(source, seed, accumulator, resultSelector);
		}

		public static IConnectableObservable<T> Multicast<T>(this IObservable<T> source, ISubject<T> subject)
		{
			return new Observable.ConnectableObservable<T>(source, subject);
		}

		public static IConnectableObservable<T> Publish<T>(this IObservable<T> source)
		{
			return source.Multicast(new Subject<T>());
		}

		public static IConnectableObservable<T> Publish<T>(this IObservable<T> source, T initialValue)
		{
			return source.Multicast(new BehaviorSubject<T>(initialValue));
		}

		public static IConnectableObservable<T> PublishLast<T>(this IObservable<T> source)
		{
			return source.Multicast(new AsyncSubject<T>());
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source)
		{
			return source.Multicast(new ReplaySubject<T>());
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, IScheduler scheduler)
		{
			return source.Multicast(new ReplaySubject<T>(scheduler));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize)
		{
			return source.Multicast(new ReplaySubject<T>(bufferSize));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize, IScheduler scheduler)
		{
			return source.Multicast(new ReplaySubject<T>(bufferSize, scheduler));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, TimeSpan window)
		{
			return source.Multicast(new ReplaySubject<T>(window));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, TimeSpan window, IScheduler scheduler)
		{
			return source.Multicast(new ReplaySubject<T>(window, scheduler));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize, TimeSpan window, IScheduler scheduler)
		{
			return source.Multicast(new ReplaySubject<T>(bufferSize, window, scheduler));
		}

		public static IObservable<T> RefCount<T>(this IConnectableObservable<T> source)
		{
			return new RefCountObservable<T>(source);
		}

		public static IObservable<T> Share<T>(this IObservable<T> source)
		{
			return source.Publish<T>().RefCount<T>();
		}

		public static T Wait<T>(this IObservable<T> source)
		{
			return new Wait<T>(source, Observable.InfiniteTimeSpan).Run();
		}

		public static T Wait<T>(this IObservable<T> source, TimeSpan timeout)
		{
			return new Wait<T>(source, timeout).Run();
		}

		private static IEnumerable<IObservable<T>> CombineSources<T>(IObservable<T> first, IObservable<T>[] seconds)
		{
			yield return first;
			for (int i = 0; i < seconds.Length; i++)
			{
				yield return seconds[i];
			}
			yield break;
		}

		public static IObservable<TSource> Concat<TSource>(params IObservable<TSource>[] sources)
		{
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			return new ConcatObservable<TSource>(sources);
		}

		public static IObservable<TSource> Concat<TSource>(this IEnumerable<IObservable<TSource>> sources)
		{
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			return new ConcatObservable<TSource>(sources);
		}

		public static IObservable<TSource> Concat<TSource>(this IObservable<IObservable<TSource>> sources)
		{
			return sources.Merge(1);
		}

		public static IObservable<TSource> Concat<TSource>(this IObservable<TSource> first, params IObservable<TSource>[] seconds)
		{
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}
			if (seconds == null)
			{
				throw new ArgumentNullException("seconds");
			}
			ConcatObservable<TSource> concatObservable = first as ConcatObservable<TSource>;
			if (concatObservable != null)
			{
				return concatObservable.Combine(seconds);
			}
			return Observable.CombineSources<TSource>(first, seconds).Concat<TSource>();
		}

		public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources)
		{
			return sources.Merge(Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, IScheduler scheduler)
		{
			return new MergeObservable<TSource>(sources.ToObservable(scheduler), scheduler == Scheduler.CurrentThread);
		}

		public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, int maxConcurrent)
		{
			return sources.Merge(maxConcurrent, Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, int maxConcurrent, IScheduler scheduler)
		{
			return new MergeObservable<TSource>(sources.ToObservable(scheduler), maxConcurrent, scheduler == Scheduler.CurrentThread);
		}

		public static IObservable<TSource> Merge<TSource>(params IObservable<TSource>[] sources)
		{
			return Observable.Merge<TSource>(Scheduler.DefaultSchedulers.ConstantTimeOperations, sources);
		}

		public static IObservable<TSource> Merge<TSource>(IScheduler scheduler, params IObservable<TSource>[] sources)
		{
			return new MergeObservable<TSource>(sources.ToObservable(scheduler), scheduler == Scheduler.CurrentThread);
		}

		public static IObservable<T> Merge<T>(this IObservable<T> first, params IObservable<T>[] seconds)
		{
			return Observable.CombineSources<T>(first, seconds).Merge<T>();
		}

		public static IObservable<T> Merge<T>(this IObservable<T> first, IObservable<T> second, IScheduler scheduler)
		{
			return Observable.Merge<T>(scheduler, new IObservable<T>[]
			{
				first,
				second
			});
		}

		public static IObservable<T> Merge<T>(this IObservable<IObservable<T>> sources)
		{
			return new MergeObservable<T>(sources, false);
		}

		public static IObservable<T> Merge<T>(this IObservable<IObservable<T>> sources, int maxConcurrent)
		{
			return new MergeObservable<T>(sources, maxConcurrent, false);
		}

		public static IObservable<TResult> Zip<TLeft, TRight, TResult>(this IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector)
		{
			return new ZipObservable<TLeft, TRight, TResult>(left, right, selector);
		}

		public static IObservable<IList<T>> Zip<T>(this IEnumerable<IObservable<T>> sources)
		{
			return Observable.Zip<T>(sources.ToArray<IObservable<T>>());
		}

		public static IObservable<IList<T>> Zip<T>(params IObservable<T>[] sources)
		{
			return new ZipObservable<T>(sources);
		}

		public static IObservable<TR> Zip<T1, T2, T3, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, ZipFunc<T1, T2, T3, TR> resultSelector)
		{
			return new ZipObservable<T1, T2, T3, TR>(source1, source2, source3, resultSelector);
		}

		public static IObservable<TR> Zip<T1, T2, T3, T4, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, ZipFunc<T1, T2, T3, T4, TR> resultSelector)
		{
			return new ZipObservable<T1, T2, T3, T4, TR>(source1, source2, source3, source4, resultSelector);
		}

		public static IObservable<TR> Zip<T1, T2, T3, T4, T5, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, ZipFunc<T1, T2, T3, T4, T5, TR> resultSelector)
		{
			return new ZipObservable<T1, T2, T3, T4, T5, TR>(source1, source2, source3, source4, source5, resultSelector);
		}

		public static IObservable<TR> Zip<T1, T2, T3, T4, T5, T6, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, ZipFunc<T1, T2, T3, T4, T5, T6, TR> resultSelector)
		{
			return new ZipObservable<T1, T2, T3, T4, T5, T6, TR>(source1, source2, source3, source4, source5, source6, resultSelector);
		}

		public static IObservable<TR> Zip<T1, T2, T3, T4, T5, T6, T7, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, ZipFunc<T1, T2, T3, T4, T5, T6, T7, TR> resultSelector)
		{
			return new ZipObservable<T1, T2, T3, T4, T5, T6, T7, TR>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
		}

		public static IObservable<TResult> CombineLatest<TLeft, TRight, TResult>(this IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector)
		{
			return new CombineLatestObservable<TLeft, TRight, TResult>(left, right, selector);
		}

		public static IObservable<IList<T>> CombineLatest<T>(this IEnumerable<IObservable<T>> sources)
		{
			return Observable.CombineLatest<T>(sources.ToArray<IObservable<T>>());
		}

		public static IObservable<IList<TSource>> CombineLatest<TSource>(params IObservable<TSource>[] sources)
		{
			return new CombineLatestObservable<TSource>(sources);
		}

		public static IObservable<TR> CombineLatest<T1, T2, T3, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, CombineLatestFunc<T1, T2, T3, TR> resultSelector)
		{
			return new CombineLatestObservable<T1, T2, T3, TR>(source1, source2, source3, resultSelector);
		}

		public static IObservable<TR> CombineLatest<T1, T2, T3, T4, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, CombineLatestFunc<T1, T2, T3, T4, TR> resultSelector)
		{
			return new CombineLatestObservable<T1, T2, T3, T4, TR>(source1, source2, source3, source4, resultSelector);
		}

		public static IObservable<TR> CombineLatest<T1, T2, T3, T4, T5, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, CombineLatestFunc<T1, T2, T3, T4, T5, TR> resultSelector)
		{
			return new CombineLatestObservable<T1, T2, T3, T4, T5, TR>(source1, source2, source3, source4, source5, resultSelector);
		}

		public static IObservable<TR> CombineLatest<T1, T2, T3, T4, T5, T6, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, CombineLatestFunc<T1, T2, T3, T4, T5, T6, TR> resultSelector)
		{
			return new CombineLatestObservable<T1, T2, T3, T4, T5, T6, TR>(source1, source2, source3, source4, source5, source6, resultSelector);
		}

		public static IObservable<TR> CombineLatest<T1, T2, T3, T4, T5, T6, T7, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, CombineLatestFunc<T1, T2, T3, T4, T5, T6, T7, TR> resultSelector)
		{
			return new CombineLatestObservable<T1, T2, T3, T4, T5, T6, T7, TR>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
		}

		public static IObservable<TResult> ZipLatest<TLeft, TRight, TResult>(this IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector)
		{
			return new ZipLatestObservable<TLeft, TRight, TResult>(left, right, selector);
		}

		public static IObservable<IList<T>> ZipLatest<T>(this IEnumerable<IObservable<T>> sources)
		{
			return Observable.ZipLatest<T>(sources.ToArray<IObservable<T>>());
		}

		public static IObservable<IList<TSource>> ZipLatest<TSource>(params IObservable<TSource>[] sources)
		{
			return new ZipLatestObservable<TSource>(sources);
		}

		public static IObservable<TR> ZipLatest<T1, T2, T3, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, ZipLatestFunc<T1, T2, T3, TR> resultSelector)
		{
			return new ZipLatestObservable<T1, T2, T3, TR>(source1, source2, source3, resultSelector);
		}

		public static IObservable<TR> ZipLatest<T1, T2, T3, T4, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, ZipLatestFunc<T1, T2, T3, T4, TR> resultSelector)
		{
			return new ZipLatestObservable<T1, T2, T3, T4, TR>(source1, source2, source3, source4, resultSelector);
		}

		public static IObservable<TR> ZipLatest<T1, T2, T3, T4, T5, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, ZipLatestFunc<T1, T2, T3, T4, T5, TR> resultSelector)
		{
			return new ZipLatestObservable<T1, T2, T3, T4, T5, TR>(source1, source2, source3, source4, source5, resultSelector);
		}

		public static IObservable<TR> ZipLatest<T1, T2, T3, T4, T5, T6, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, ZipLatestFunc<T1, T2, T3, T4, T5, T6, TR> resultSelector)
		{
			return new ZipLatestObservable<T1, T2, T3, T4, T5, T6, TR>(source1, source2, source3, source4, source5, source6, resultSelector);
		}

		public static IObservable<TR> ZipLatest<T1, T2, T3, T4, T5, T6, T7, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, ZipLatestFunc<T1, T2, T3, T4, T5, T6, T7, TR> resultSelector)
		{
			return new ZipLatestObservable<T1, T2, T3, T4, T5, T6, T7, TR>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
		}

		public static IObservable<T> Switch<T>(this IObservable<IObservable<T>> sources)
		{
			return new SwitchObservable<T>(sources);
		}

		public static IObservable<TResult> WithLatestFrom<TLeft, TRight, TResult>(this IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector)
		{
			return new WithLatestFromObservable<TLeft, TRight, TResult>(left, right, selector);
		}

		public static IObservable<T[]> WhenAll<T>(params IObservable<T>[] sources)
		{
			if (sources.Length == 0)
			{
				return Observable.Return<T[]>(new T[0]);
			}
			return new WhenAllObservable<T>(sources);
		}

		public static IObservable<Unit> WhenAll(params IObservable<Unit>[] sources)
		{
			if (sources.Length == 0)
			{
				return Observable.ReturnUnit();
			}
			return new WhenAllObservable(sources);
		}

		public static IObservable<T[]> WhenAll<T>(this IEnumerable<IObservable<T>> sources)
		{
			IObservable<T>[] array = sources as IObservable<T>[];
			if (array != null)
			{
				return Observable.WhenAll<T>(array);
			}
			return new WhenAllObservable<T>(sources);
		}

		public static IObservable<Unit> WhenAll(this IEnumerable<IObservable<Unit>> sources)
		{
			IObservable<Unit>[] array = sources as IObservable<Unit>[];
			if (array != null)
			{
				return Observable.WhenAll(array);
			}
			return new WhenAllObservable(sources);
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, T value)
		{
			return new StartWithObservable<T>(source, value);
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, Func<T> valueFactory)
		{
			return new StartWithObservable<T>(source, valueFactory);
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, params T[] values)
		{
			return source.StartWith(Scheduler.DefaultSchedulers.ConstantTimeOperations, values);
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, IEnumerable<T> values)
		{
			return source.StartWith(Scheduler.DefaultSchedulers.ConstantTimeOperations, values);
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, IScheduler scheduler, T value)
		{
			return Observable.Return<T>(value, scheduler).Concat(new IObservable<T>[]
			{
				source
			});
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, IScheduler scheduler, IEnumerable<T> values)
		{
			T[] array = values as T[];
			if (array == null)
			{
				array = values.ToArray<T>();
			}
			return source.StartWith(scheduler, array);
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, IScheduler scheduler, params T[] values)
		{
			return values.ToObservable(scheduler).Concat(new IObservable<T>[]
			{
				source
			});
		}

		public static IObservable<T> Synchronize<T>(this IObservable<T> source)
		{
			return new SynchronizeObservable<T>(source, new object());
		}

		public static IObservable<T> Synchronize<T>(this IObservable<T> source, object gate)
		{
			return new SynchronizeObservable<T>(source, gate);
		}

		public static IObservable<T> ObserveOn<T>(this IObservable<T> source, IScheduler scheduler)
		{
			return new ObserveOnObservable<T>(source, scheduler);
		}

		public static IObservable<T> SubscribeOn<T>(this IObservable<T> source, IScheduler scheduler)
		{
			return new SubscribeOnObservable<T>(source, scheduler);
		}

		public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, TimeSpan dueTime)
		{
			return new DelaySubscriptionObservable<T>(source, dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return new DelaySubscriptionObservable<T>(source, dueTime, scheduler);
		}

		public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, DateTimeOffset dueTime)
		{
			return new DelaySubscriptionObservable<T>(source, dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, DateTimeOffset dueTime, IScheduler scheduler)
		{
			return new DelaySubscriptionObservable<T>(source, dueTime, scheduler);
		}

		public static IObservable<T> Amb<T>(params IObservable<T>[] sources)
		{
			return Observable.Amb<T>(sources);
		}

		public static IObservable<T> Amb<T>(IEnumerable<IObservable<T>> sources)
		{
			IObservable<T> observable = Observable.Never<T>();
			foreach (IObservable<T> observable2 in sources)
			{
				IObservable<T> second = observable2;
				observable = observable.Amb(second);
			}
			return observable;
		}

		public static IObservable<T> Amb<T>(this IObservable<T> source, IObservable<T> second)
		{
			return new AmbObservable<T>(source, second);
		}

		public static IObservable<T> AsObservable<T>(this IObservable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (source is AsObservableObservable<T>)
			{
				return source;
			}
			return new AsObservableObservable<T>(source);
		}

		public static IObservable<T> ToObservable<T>(this IEnumerable<T> source)
		{
			return source.ToObservable(Scheduler.DefaultSchedulers.Iteration);
		}

		public static IObservable<T> ToObservable<T>(this IEnumerable<T> source, IScheduler scheduler)
		{
			return new ToObservableObservable<T>(source, scheduler);
		}

		public static IObservable<TResult> Cast<TSource, TResult>(this IObservable<TSource> source)
		{
			return new CastObservable<TSource, TResult>(source);
		}

		public static IObservable<TResult> Cast<TSource, TResult>(this IObservable<TSource> source, TResult witness)
		{
			return new CastObservable<TSource, TResult>(source);
		}

		public static IObservable<TResult> OfType<TSource, TResult>(this IObservable<TSource> source)
		{
			return new OfTypeObservable<TSource, TResult>(source);
		}

		public static IObservable<TResult> OfType<TSource, TResult>(this IObservable<TSource> source, TResult witness)
		{
			return new OfTypeObservable<TSource, TResult>(source);
		}

		public static IObservable<Unit> AsUnitObservable<T>(this IObservable<T> source)
		{
			return new AsUnitObservableObservable<T>(source);
		}

		public static IObservable<Unit> AsSingleUnitObservable<T>(this IObservable<T> source)
		{
			return new AsSingleUnitObservableObservable<T>(source);
		}

		public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe)
		{
			if (subscribe == null)
			{
				throw new ArgumentNullException("subscribe");
			}
			return new CreateObservable<T>(subscribe);
		}

		public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread)
		{
			if (subscribe == null)
			{
				throw new ArgumentNullException("subscribe");
			}
			return new CreateObservable<T>(subscribe, isRequiredSubscribeOnCurrentThread);
		}

		public static IObservable<T> CreateWithState<T, TState>(TState state, Func<TState, IObserver<T>, IDisposable> subscribe)
		{
			if (subscribe == null)
			{
				throw new ArgumentNullException("subscribe");
			}
			return new CreateObservable<T, TState>(state, subscribe);
		}

		public static IObservable<T> CreateWithState<T, TState>(TState state, Func<TState, IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread)
		{
			if (subscribe == null)
			{
				throw new ArgumentNullException("subscribe");
			}
			return new CreateObservable<T, TState>(state, subscribe, isRequiredSubscribeOnCurrentThread);
		}

		public static IObservable<T> CreateSafe<T>(Func<IObserver<T>, IDisposable> subscribe)
		{
			if (subscribe == null)
			{
				throw new ArgumentNullException("subscribe");
			}
			return new CreateSafeObservable<T>(subscribe);
		}

		public static IObservable<T> CreateSafe<T>(Func<IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread)
		{
			if (subscribe == null)
			{
				throw new ArgumentNullException("subscribe");
			}
			return new CreateSafeObservable<T>(subscribe, isRequiredSubscribeOnCurrentThread);
		}

		public static IObservable<T> Empty<T>()
		{
			return Observable.Empty<T>(Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Empty<T>(IScheduler scheduler)
		{
			if (scheduler == Scheduler.Immediate)
			{
				return ImmutableEmptyObservable<T>.Instance;
			}
			return new EmptyObservable<T>(scheduler);
		}

		public static IObservable<T> Empty<T>(T witness)
		{
			return Observable.Empty<T>(Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Empty<T>(IScheduler scheduler, T witness)
		{
			return Observable.Empty<T>(scheduler);
		}

		public static IObservable<T> Never<T>()
		{
			return ImmutableNeverObservable<T>.Instance;
		}

		public static IObservable<T> Never<T>(T witness)
		{
			return ImmutableNeverObservable<T>.Instance;
		}

		public static IObservable<T> Return<T>(T value)
		{
			return Observable.Return<T>(value, Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Return<T>(T value, IScheduler scheduler)
		{
			if (scheduler == Scheduler.Immediate)
			{
				return new ImmediateReturnObservable<T>(value);
			}
			return new ReturnObservable<T>(value, scheduler);
		}

		public static IObservable<Unit> Return(Unit value)
		{
			return ImmutableReturnUnitObservable.Instance;
		}

		public static IObservable<bool> Return(bool value)
		{
			IObservable<bool> result;
			if (value)
			{
				IObservable<bool> instance = ImmutableReturnTrueObservable.Instance;
				result = instance;
			}
			else
			{
				result = ImmutableReturnFalseObservable.Instance;
			}
			return result;
		}

		public static IObservable<int> Return(int value)
		{
			return ImmutableReturnInt32Observable.GetInt32Observable(value);
		}

		public static IObservable<Unit> ReturnUnit()
		{
			return ImmutableReturnUnitObservable.Instance;
		}

		public static IObservable<T> Throw<T>(Exception error)
		{
			return Observable.Throw<T>(error, Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Throw<T>(Exception error, T witness)
		{
			return Observable.Throw<T>(error, Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Throw<T>(Exception error, IScheduler scheduler)
		{
			return new ThrowObservable<T>(error, scheduler);
		}

		public static IObservable<T> Throw<T>(Exception error, IScheduler scheduler, T witness)
		{
			return Observable.Throw<T>(error, scheduler);
		}

		public static IObservable<int> Range(int start, int count)
		{
			return Observable.Range(start, count, Scheduler.DefaultSchedulers.Iteration);
		}

		public static IObservable<int> Range(int start, int count, IScheduler scheduler)
		{
			return new RangeObservable(start, count, scheduler);
		}

		public static IObservable<T> Repeat<T>(T value)
		{
			return Observable.Repeat<T>(value, Scheduler.DefaultSchedulers.Iteration);
		}

		public static IObservable<T> Repeat<T>(T value, IScheduler scheduler)
		{
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			return new RepeatObservable<T>(value, null, scheduler);
		}

		public static IObservable<T> Repeat<T>(T value, int repeatCount)
		{
			return Observable.Repeat<T>(value, repeatCount, Scheduler.DefaultSchedulers.Iteration);
		}

		public static IObservable<T> Repeat<T>(T value, int repeatCount, IScheduler scheduler)
		{
			if (repeatCount < 0)
			{
				throw new ArgumentOutOfRangeException("repeatCount");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			return new RepeatObservable<T>(value, new int?(repeatCount), scheduler);
		}

		public static IObservable<T> Repeat<T>(this IObservable<T> source)
		{
			return Observable.RepeatInfinite<T>(source).Concat<T>();
		}

		private static IEnumerable<IObservable<T>> RepeatInfinite<T>(IObservable<T> source)
		{
			for (;;)
			{
				yield return source;
			}
			yield break;
		}

		public static IObservable<T> RepeatSafe<T>(this IObservable<T> source)
		{
			return new RepeatSafeObservable<T>(Observable.RepeatInfinite<T>(source), source.IsRequiredSubscribeOnCurrentThread<T>());
		}

		public static IObservable<T> Defer<T>(Func<IObservable<T>> observableFactory)
		{
			return new DeferObservable<T>(observableFactory);
		}

		public static IObservable<T> Start<T>(Func<T> function)
		{
			return new StartObservable<T>(function, null, Scheduler.DefaultSchedulers.AsyncConversions);
		}

		public static IObservable<T> Start<T>(Func<T> function, TimeSpan timeSpan)
		{
			return new StartObservable<T>(function, new TimeSpan?(timeSpan), Scheduler.DefaultSchedulers.AsyncConversions);
		}

		public static IObservable<T> Start<T>(Func<T> function, IScheduler scheduler)
		{
			return new StartObservable<T>(function, null, scheduler);
		}

		public static IObservable<T> Start<T>(Func<T> function, TimeSpan timeSpan, IScheduler scheduler)
		{
			return new StartObservable<T>(function, new TimeSpan?(timeSpan), scheduler);
		}

		public static IObservable<Unit> Start(Action action)
		{
			return new StartObservable<Unit>(action, null, Scheduler.DefaultSchedulers.AsyncConversions);
		}

		public static IObservable<Unit> Start(Action action, TimeSpan timeSpan)
		{
			return new StartObservable<Unit>(action, new TimeSpan?(timeSpan), Scheduler.DefaultSchedulers.AsyncConversions);
		}

		public static IObservable<Unit> Start(Action action, IScheduler scheduler)
		{
			return new StartObservable<Unit>(action, null, scheduler);
		}

		public static IObservable<Unit> Start(Action action, TimeSpan timeSpan, IScheduler scheduler)
		{
			return new StartObservable<Unit>(action, new TimeSpan?(timeSpan), scheduler);
		}

		public static Func<IObservable<T>> ToAsync<T>(Func<T> function)
		{
			return Observable.ToAsync<T>(function, Scheduler.DefaultSchedulers.AsyncConversions);
		}

		public static Func<IObservable<T>> ToAsync<T>(Func<T> function, IScheduler scheduler)
		{
			return delegate()
			{
				AsyncSubject<T> subject = new AsyncSubject<T>();
				scheduler.Schedule(delegate()
				{
					T value = default(T);
					try
					{
						value = function();
					}
					catch (Exception error)
					{
						subject.OnError(error);
						return;
					}
					subject.OnNext(value);
					subject.OnCompleted();
				});
				return subject.AsObservable<T>();
			};
		}

		public static Func<IObservable<Unit>> ToAsync(Action action)
		{
			return Observable.ToAsync(action, Scheduler.DefaultSchedulers.AsyncConversions);
		}

		public static Func<IObservable<Unit>> ToAsync(Action action, IScheduler scheduler)
		{
			return delegate()
			{
				AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
				scheduler.Schedule(delegate()
				{
					try
					{
						action();
					}
					catch (Exception error)
					{
						subject.OnError(error);
						return;
					}
					subject.OnNext(Unit.Default);
					subject.OnCompleted();
				});
				return subject.AsObservable<Unit>();
			};
		}

		public static IObservable<TR> Select<T, TR>(this IObservable<T> source, Func<T, TR> selector)
		{
			WhereObservable<T> whereObservable = source as WhereObservable<T>;
			if (whereObservable != null)
			{
				return whereObservable.CombineSelector<TR>(selector);
			}
			return new SelectObservable<T, TR>(source, selector);
		}

		public static IObservable<TR> Select<T, TR>(this IObservable<T> source, Func<T, int, TR> selector)
		{
			return new SelectObservable<T, TR>(source, selector);
		}

		public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			WhereObservable<T> whereObservable = source as WhereObservable<T>;
			if (whereObservable != null)
			{
				return whereObservable.CombinePredicate(predicate);
			}
			ISelect<T> select = source as ISelect<T>;
			if (select != null)
			{
				return select.CombinePredicate(predicate);
			}
			return new WhereObservable<T>(source, predicate);
		}

		public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, int, bool> predicate)
		{
			return new WhereObservable<T>(source, predicate);
		}

		public static IObservable<TR> ContinueWith<T, TR>(this IObservable<T> source, IObservable<TR> other)
		{
			return source.ContinueWith((T _) => other);
		}

		public static IObservable<TR> ContinueWith<T, TR>(this IObservable<T> source, Func<T, IObservable<TR>> selector)
		{
			return new ContinueWithObservable<T, TR>(source, selector);
		}

		public static IObservable<TR> SelectMany<T, TR>(this IObservable<T> source, IObservable<TR> other)
		{
			return source.SelectMany((T _) => other);
		}

		public static IObservable<TR> SelectMany<T, TR>(this IObservable<T> source, Func<T, IObservable<TR>> selector)
		{
			return new SelectManyObservable<T, TR>(source, selector);
		}

		public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector)
		{
			return new SelectManyObservable<TSource, TResult>(source, selector);
		}

		public static IObservable<TR> SelectMany<T, TC, TR>(this IObservable<T> source, Func<T, IObservable<TC>> collectionSelector, Func<T, TC, TR> resultSelector)
		{
			return new SelectManyObservable<T, TC, TR>(source, collectionSelector, resultSelector);
		}

		public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
		{
			return new SelectManyObservable<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
		{
			return new SelectManyObservable<TSource, TResult>(source, selector);
		}

		public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
		{
			return new SelectManyObservable<TSource, TResult>(source, selector);
		}

		public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			return new SelectManyObservable<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
		{
			return new SelectManyObservable<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		public static IObservable<T[]> ToArray<T>(this IObservable<T> source)
		{
			return new ToArrayObservable<T>(source);
		}

		public static IObservable<IList<T>> ToList<T>(this IObservable<T> source)
		{
			return new ToListObservable<T>(source);
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, IObserver<T> observer)
		{
			return new DoObserverObservable<T>(source, observer);
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext)
		{
			return new DoObservable<T>(source, onNext, Stubs.Throw, Stubs.Nop);
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError)
		{
			return new DoObservable<T>(source, onNext, onError, Stubs.Nop);
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext, Action onCompleted)
		{
			return new DoObservable<T>(source, onNext, Stubs.Throw, onCompleted);
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted)
		{
			return new DoObservable<T>(source, onNext, onError, onCompleted);
		}

		public static IObservable<T> DoOnError<T>(this IObservable<T> source, Action<Exception> onError)
		{
			return new DoOnErrorObservable<T>(source, onError);
		}

		public static IObservable<T> DoOnCompleted<T>(this IObservable<T> source, Action onCompleted)
		{
			return new DoOnCompletedObservable<T>(source, onCompleted);
		}

		public static IObservable<T> DoOnTerminate<T>(this IObservable<T> source, Action onTerminate)
		{
			return new DoOnTerminateObservable<T>(source, onTerminate);
		}

		public static IObservable<T> DoOnSubscribe<T>(this IObservable<T> source, Action onSubscribe)
		{
			return new DoOnSubscribeObservable<T>(source, onSubscribe);
		}

		public static IObservable<T> DoOnCancel<T>(this IObservable<T> source, Action onCancel)
		{
			return new DoOnCancelObservable<T>(source, onCancel);
		}

		public static IObservable<Notification<T>> Materialize<T>(this IObservable<T> source)
		{
			return new MaterializeObservable<T>(source);
		}

		public static IObservable<T> Dematerialize<T>(this IObservable<Notification<T>> source)
		{
			return new DematerializeObservable<T>(source);
		}

		public static IObservable<T> DefaultIfEmpty<T>(this IObservable<T> source)
		{
			return new DefaultIfEmptyObservable<T>(source, default(T));
		}

		public static IObservable<T> DefaultIfEmpty<T>(this IObservable<T> source, T defaultValue)
		{
			return new DefaultIfEmptyObservable<T>(source, defaultValue);
		}

		public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source)
		{
			IEqualityComparer<TSource> @default = UnityEqualityComparer.GetDefault<TSource>();
			return new DistinctObservable<TSource>(source, @default);
		}

		public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			return new DistinctObservable<TSource>(source, comparer);
		}

		public static IObservable<TSource> Distinct<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector)
		{
			IEqualityComparer<TKey> @default = UnityEqualityComparer.GetDefault<TKey>();
			return new DistinctObservable<TSource, TKey>(source, keySelector, @default);
		}

		public static IObservable<TSource> Distinct<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			return new DistinctObservable<TSource, TKey>(source, keySelector, comparer);
		}

		public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source)
		{
			IEqualityComparer<T> @default = UnityEqualityComparer.GetDefault<T>();
			return new DistinctUntilChangedObservable<T>(source, @default);
		}

		public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source, IEqualityComparer<T> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return new DistinctUntilChangedObservable<T>(source, comparer);
		}

		public static IObservable<T> DistinctUntilChanged<T, TKey>(this IObservable<T> source, Func<T, TKey> keySelector)
		{
			IEqualityComparer<TKey> @default = UnityEqualityComparer.GetDefault<TKey>();
			return new DistinctUntilChangedObservable<T, TKey>(source, keySelector, @default);
		}

		public static IObservable<T> DistinctUntilChanged<T, TKey>(this IObservable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return new DistinctUntilChangedObservable<T, TKey>(source, keySelector, comparer);
		}

		public static IObservable<T> IgnoreElements<T>(this IObservable<T> source)
		{
			return new IgnoreElementsObservable<T>(source);
		}

		public static IObservable<Unit> ForEachAsync<T>(this IObservable<T> source, Action<T> onNext)
		{
			return new ForEachAsyncObservable<T>(source, onNext);
		}

		public static IObservable<Unit> ForEachAsync<T>(this IObservable<T> source, Action<T, int> onNext)
		{
			return new ForEachAsyncObservable<T>(source, onNext);
		}

		public static IObservable<T> Finally<T>(this IObservable<T> source, Action finallyAction)
		{
			return new FinallyObservable<T>(source, finallyAction);
		}

		public static IObservable<T> Catch<T, TException>(this IObservable<T> source, Func<TException, IObservable<T>> errorHandler) where TException : Exception
		{
			return new CatchObservable<T, TException>(source, errorHandler);
		}

		public static IObservable<TSource> Catch<TSource>(this IEnumerable<IObservable<TSource>> sources)
		{
			return new CatchObservable<TSource>(sources);
		}

		public static IObservable<TSource> CatchIgnore<TSource>(this IObservable<TSource> source)
		{
			return source.Catch(new Func<Exception, IObservable<TSource>>(Stubs.CatchIgnore<TSource>));
		}

		public static IObservable<TSource> CatchIgnore<TSource, TException>(this IObservable<TSource> source, Action<TException> errorAction) where TException : Exception
		{
			return source.Catch(delegate(TException ex)
			{
				errorAction(ex);
				return Observable.Empty<TSource>();
			});
		}

		public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source)
		{
			return Observable.RepeatInfinite<TSource>(source).Catch<TSource>();
		}

		public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source, int retryCount)
		{
			return Enumerable.Repeat<IObservable<TSource>>(source, retryCount).Catch<TSource>();
		}

		public static IObservable<TSource> OnErrorRetry<TSource>(this IObservable<TSource> source)
		{
			return source.Retry<TSource>();
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError) where TException : Exception
		{
			return source.OnErrorRetry(onError, TimeSpan.Zero);
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError, TimeSpan delay) where TException : Exception
		{
			return source.OnErrorRetry(onError, int.MaxValue, delay);
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError, int retryCount) where TException : Exception
		{
			return source.OnErrorRetry(onError, retryCount, TimeSpan.Zero);
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError, int retryCount, TimeSpan delay) where TException : Exception
		{
			return source.OnErrorRetry(onError, retryCount, delay, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError, int retryCount, TimeSpan delay, IScheduler delayScheduler) where TException : Exception
		{
			return Observable.Defer<TSource>(delegate
			{
				TimeSpan dueTime = (delay.Ticks >= 0L) ? delay : TimeSpan.Zero;
				int count = 0;
				IObservable<TSource> self = null;
				self = source.Catch(delegate(TException ex)
				{
					onError(ex);
					return (++count >= retryCount) ? Observable.Throw<TSource>(ex) : ((!(dueTime == TimeSpan.Zero)) ? self.DelaySubscription(dueTime, delayScheduler).SubscribeOn(Scheduler.CurrentThread) : self.SubscribeOn(Scheduler.CurrentThread));
				});
				return self;
			});
		}

		public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler) where TEventArgs : EventArgs
		{
			return new FromEventPatternObservable<TDelegate, TEventArgs>(conversion, addHandler, removeHandler);
		}

		public static IObservable<Unit> FromEvent<TDelegate>(Func<Action, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
		{
			return new FromEventObservable<TDelegate>(conversion, addHandler, removeHandler);
		}

		public static IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
		{
			return new FromEventObservable<TDelegate, TEventArgs>(conversion, addHandler, removeHandler);
		}

		public static IObservable<Unit> FromEvent(Action<Action> addHandler, Action<Action> removeHandler)
		{
			return new FromEventObservable(addHandler, removeHandler);
		}

		public static IObservable<T> FromEvent<T>(Action<Action<T>> addHandler, Action<Action<T>> removeHandler)
		{
			return new FromEventObservable_<T>(addHandler, removeHandler);
		}

		public static Func<IObservable<TResult>> FromAsyncPattern<TResult>(Func<AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
		{
			return delegate()
			{
				AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
				try
				{
					begin(delegate(IAsyncResult iar)
					{
						TResult value;
						try
						{
							value = end(iar);
						}
						catch (Exception error2)
						{
							subject.OnError(error2);
							return;
						}
						subject.OnNext(value);
						subject.OnCompleted();
					}, null);
				}
				catch (Exception error)
				{
					return Observable.Throw<TResult>(error, Scheduler.DefaultSchedulers.AsyncConversions);
				}
				return subject.AsObservable<TResult>();
			};
		}

		public static Func<T1, IObservable<TResult>> FromAsyncPattern<T1, TResult>(Func<T1, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
		{
			return delegate(T1 x)
			{
				AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
				try
				{
					begin(x, delegate(IAsyncResult iar)
					{
						TResult value;
						try
						{
							value = end(iar);
						}
						catch (Exception error2)
						{
							subject.OnError(error2);
							return;
						}
						subject.OnNext(value);
						subject.OnCompleted();
					}, null);
				}
				catch (Exception error)
				{
					return Observable.Throw<TResult>(error, Scheduler.DefaultSchedulers.AsyncConversions);
				}
				return subject.AsObservable<TResult>();
			};
		}

		public static Func<T1, T2, IObservable<TResult>> FromAsyncPattern<T1, T2, TResult>(Func<T1, T2, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
		{
			return delegate(T1 x, T2 y)
			{
				AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
				try
				{
					begin(x, y, delegate(IAsyncResult iar)
					{
						TResult value;
						try
						{
							value = end(iar);
						}
						catch (Exception error2)
						{
							subject.OnError(error2);
							return;
						}
						subject.OnNext(value);
						subject.OnCompleted();
					}, null);
				}
				catch (Exception error)
				{
					return Observable.Throw<TResult>(error, Scheduler.DefaultSchedulers.AsyncConversions);
				}
				return subject.AsObservable<TResult>();
			};
		}

		public static Func<IObservable<Unit>> FromAsyncPattern(Func<AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
		{
			return Observable.FromAsyncPattern<Unit>(begin, delegate(IAsyncResult iar)
			{
				end(iar);
				return Unit.Default;
			});
		}

		public static Func<T1, IObservable<Unit>> FromAsyncPattern<T1>(Func<T1, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
		{
			return Observable.FromAsyncPattern<T1, Unit>(begin, delegate(IAsyncResult iar)
			{
				end(iar);
				return Unit.Default;
			});
		}

		public static Func<T1, T2, IObservable<Unit>> FromAsyncPattern<T1, T2>(Func<T1, T2, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
		{
			return Observable.FromAsyncPattern<T1, T2, Unit>(begin, delegate(IAsyncResult iar)
			{
				end(iar);
				return Unit.Default;
			});
		}

		public static IObservable<T> Take<T>(this IObservable<T> source, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count == 0)
			{
				return Observable.Empty<T>();
			}
			TakeObservable<T> takeObservable = source as TakeObservable<T>;
			if (takeObservable != null && takeObservable.scheduler == null)
			{
				return takeObservable.Combine(count);
			}
			return new TakeObservable<T>(source, count);
		}

		public static IObservable<T> Take<T>(this IObservable<T> source, TimeSpan duration)
		{
			return source.Take(duration, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> Take<T>(this IObservable<T> source, TimeSpan duration, IScheduler scheduler)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			TakeObservable<T> takeObservable = source as TakeObservable<T>;
			if (takeObservable != null && takeObservable.scheduler == scheduler)
			{
				return takeObservable.Combine(duration);
			}
			return new TakeObservable<T>(source, duration, scheduler);
		}

		public static IObservable<T> TakeWhile<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return new TakeWhileObservable<T>(source, predicate);
		}

		public static IObservable<T> TakeWhile<T>(this IObservable<T> source, Func<T, int, bool> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return new TakeWhileObservable<T>(source, predicate);
		}

		public static IObservable<T> TakeUntil<T, TOther>(this IObservable<T> source, IObservable<TOther> other)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			return new TakeUntilObservable<T, TOther>(source, other);
		}

		public static IObservable<T> TakeLast<T>(this IObservable<T> source, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			return new TakeLastObservable<T>(source, count);
		}

		public static IObservable<T> TakeLast<T>(this IObservable<T> source, TimeSpan duration)
		{
			return source.TakeLast(duration, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> TakeLast<T>(this IObservable<T> source, TimeSpan duration, IScheduler scheduler)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return new TakeLastObservable<T>(source, duration, scheduler);
		}

		public static IObservable<T> Skip<T>(this IObservable<T> source, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			SkipObservable<T> skipObservable = source as SkipObservable<T>;
			if (skipObservable != null && skipObservable.scheduler == null)
			{
				return skipObservable.Combine(count);
			}
			return new SkipObservable<T>(source, count);
		}

		public static IObservable<T> Skip<T>(this IObservable<T> source, TimeSpan duration)
		{
			return source.Skip(duration, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> Skip<T>(this IObservable<T> source, TimeSpan duration, IScheduler scheduler)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			SkipObservable<T> skipObservable = source as SkipObservable<T>;
			if (skipObservable != null && skipObservable.scheduler == scheduler)
			{
				return skipObservable.Combine(duration);
			}
			return new SkipObservable<T>(source, duration, scheduler);
		}

		public static IObservable<T> SkipWhile<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return new SkipWhileObservable<T>(source, predicate);
		}

		public static IObservable<T> SkipWhile<T>(this IObservable<T> source, Func<T, int, bool> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return new SkipWhileObservable<T>(source, predicate);
		}

		public static IObservable<T> SkipUntil<T, TOther>(this IObservable<T> source, IObservable<TOther> other)
		{
			return new SkipUntilObservable<T, TOther>(source, other);
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count <= 0");
			}
			return new BufferObservable<T>(source, count, 0);
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, int count, int skip)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count <= 0");
			}
			if (skip <= 0)
			{
				throw new ArgumentOutOfRangeException("skip <= 0");
			}
			return new BufferObservable<T>(source, count, skip);
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan)
		{
			return source.Buffer(timeSpan, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, IScheduler scheduler)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return new BufferObservable<T>(source, timeSpan, timeSpan, scheduler);
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, int count)
		{
			return source.Buffer(timeSpan, count, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, int count, IScheduler scheduler)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count <= 0");
			}
			return new BufferObservable<T>(source, timeSpan, count, scheduler);
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, TimeSpan timeShift)
		{
			return new BufferObservable<T>(source, timeSpan, timeShift, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return new BufferObservable<T>(source, timeSpan, timeShift, scheduler);
		}

		public static IObservable<IList<TSource>> Buffer<TSource, TWindowBoundary>(this IObservable<TSource> source, IObservable<TWindowBoundary> windowBoundaries)
		{
			return new BufferObservable<TSource, TWindowBoundary>(source, windowBoundaries);
		}

		public static IObservable<Pair<T>> Pairwise<T>(this IObservable<T> source)
		{
			return new PairwiseObservable<T>(source);
		}

		public static IObservable<TR> Pairwise<T, TR>(this IObservable<T> source, Func<T, T, TR> selector)
		{
			return new PairwiseObservable<T, TR>(source, selector);
		}

		public static IObservable<T> Last<T>(this IObservable<T> source)
		{
			return new LastObservable<T>(source, false);
		}

		public static IObservable<T> Last<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return new LastObservable<T>(source, predicate, false);
		}

		public static IObservable<T> LastOrDefault<T>(this IObservable<T> source)
		{
			return new LastObservable<T>(source, true);
		}

		public static IObservable<T> LastOrDefault<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return new LastObservable<T>(source, predicate, true);
		}

		public static IObservable<T> First<T>(this IObservable<T> source)
		{
			return new FirstObservable<T>(source, false);
		}

		public static IObservable<T> First<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return new FirstObservable<T>(source, predicate, false);
		}

		public static IObservable<T> FirstOrDefault<T>(this IObservable<T> source)
		{
			return new FirstObservable<T>(source, true);
		}

		public static IObservable<T> FirstOrDefault<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return new FirstObservable<T>(source, predicate, true);
		}

		public static IObservable<T> Single<T>(this IObservable<T> source)
		{
			return new SingleObservable<T>(source, false);
		}

		public static IObservable<T> Single<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return new SingleObservable<T>(source, predicate, false);
		}

		public static IObservable<T> SingleOrDefault<T>(this IObservable<T> source)
		{
			return new SingleObservable<T>(source, true);
		}

		public static IObservable<T> SingleOrDefault<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return new SingleObservable<T>(source, predicate, true);
		}

		public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.GroupBy(keySelector, Stubs<TSource>.Identity);
		}

		public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			return source.GroupBy(keySelector, Stubs<TSource>.Identity, comparer);
		}

		public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			IEqualityComparer<TKey> @default = UnityEqualityComparer.GetDefault<TKey>();
			return source.GroupBy(keySelector, elementSelector, @default);
		}

		public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			return new GroupByObservable<TSource, TKey, TElement>(source, keySelector, elementSelector, null, comparer);
		}

		public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, int capacity)
		{
			return source.GroupBy(keySelector, Stubs<TSource>.Identity, capacity);
		}

		public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, int capacity, IEqualityComparer<TKey> comparer)
		{
			return source.GroupBy(keySelector, Stubs<TSource>.Identity, capacity, comparer);
		}

		public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity)
		{
			IEqualityComparer<TKey> @default = UnityEqualityComparer.GetDefault<TKey>();
			return source.GroupBy(keySelector, elementSelector, capacity, @default);
		}

		public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity, IEqualityComparer<TKey> comparer)
		{
			return new GroupByObservable<TSource, TKey, TElement>(source, keySelector, elementSelector, new int?(capacity), comparer);
		}

		public static IObservable<long> Interval(TimeSpan period)
		{
			return new TimerObservable(period, new TimeSpan?(period), Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Interval(TimeSpan period, IScheduler scheduler)
		{
			return new TimerObservable(period, new TimeSpan?(period), scheduler);
		}

		public static IObservable<long> Timer(TimeSpan dueTime)
		{
			return new TimerObservable(dueTime, null, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Timer(DateTimeOffset dueTime)
		{
			return new TimerObservable(dueTime, null, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period)
		{
			return new TimerObservable(dueTime, new TimeSpan?(period), Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period)
		{
			return new TimerObservable(dueTime, new TimeSpan?(period), Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Timer(TimeSpan dueTime, IScheduler scheduler)
		{
			return new TimerObservable(dueTime, null, scheduler);
		}

		public static IObservable<long> Timer(DateTimeOffset dueTime, IScheduler scheduler)
		{
			return new TimerObservable(dueTime, null, scheduler);
		}

		public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
		{
			return new TimerObservable(dueTime, new TimeSpan?(period), scheduler);
		}

		public static IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler)
		{
			return new TimerObservable(dueTime, new TimeSpan?(period), scheduler);
		}

		public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source)
		{
			return source.Timestamp(Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source, IScheduler scheduler)
		{
			return new TimestampObservable<TSource>(source, scheduler);
		}

		public static IObservable<TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source)
		{
			return source.TimeInterval(Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source, IScheduler scheduler)
		{
			return new TimeIntervalObservable<TSource>(source, scheduler);
		}

		public static IObservable<T> Delay<T>(this IObservable<T> source, TimeSpan dueTime)
		{
			return source.Delay(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TSource> Delay<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return new DelayObservable<TSource>(source, dueTime, scheduler);
		}

		public static IObservable<T> Sample<T>(this IObservable<T> source, TimeSpan interval)
		{
			return source.Sample(interval, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> Sample<T>(this IObservable<T> source, TimeSpan interval, IScheduler scheduler)
		{
			return new SampleObservable<T>(source, interval, scheduler);
		}

		public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime)
		{
			return source.Throttle(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return new ThrottleObservable<TSource>(source, dueTime, scheduler);
		}

		public static IObservable<TSource> ThrottleFirst<TSource>(this IObservable<TSource> source, TimeSpan dueTime)
		{
			return source.ThrottleFirst(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TSource> ThrottleFirst<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return new ThrottleFirstObservable<TSource>(source, dueTime, scheduler);
		}

		public static IObservable<T> Timeout<T>(this IObservable<T> source, TimeSpan dueTime)
		{
			return source.Timeout(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> Timeout<T>(this IObservable<T> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return new TimeoutObservable<T>(source, dueTime, scheduler);
		}

		public static IObservable<T> Timeout<T>(this IObservable<T> source, DateTimeOffset dueTime)
		{
			return source.Timeout(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> Timeout<T>(this IObservable<T> source, DateTimeOffset dueTime, IScheduler scheduler)
		{
			return new TimeoutObservable<T>(source, dueTime, scheduler);
		}

		public static IObservable<Unit> FromCoroutine(Func<IEnumerator> coroutine, bool publishEveryYield = false)
		{
			return Observable.FromCoroutine<Unit>((IObserver<Unit> observer, CancellationToken cancellationToken) => Observable.WrapEnumerator(coroutine(), observer, cancellationToken, publishEveryYield));
		}

		public static IObservable<Unit> FromCoroutine(Func<CancellationToken, IEnumerator> coroutine, bool publishEveryYield = false)
		{
			return Observable.FromCoroutine<Unit>((IObserver<Unit> observer, CancellationToken cancellationToken) => Observable.WrapEnumerator(coroutine(cancellationToken), observer, cancellationToken, publishEveryYield));
		}

		public static IObservable<Unit> FromMicroCoroutine(Func<IEnumerator> coroutine, bool publishEveryYield = false, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Observable.FromMicroCoroutine<Unit>((IObserver<Unit> observer, CancellationToken cancellationToken) => Observable.WrapEnumerator(coroutine(), observer, cancellationToken, publishEveryYield), frameCountType);
		}

		public static IObservable<Unit> FromMicroCoroutine(Func<CancellationToken, IEnumerator> coroutine, bool publishEveryYield = false, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Observable.FromMicroCoroutine<Unit>((IObserver<Unit> observer, CancellationToken cancellationToken) => Observable.WrapEnumerator(coroutine(cancellationToken), observer, cancellationToken, publishEveryYield), frameCountType);
		}

		private static IEnumerator WrapEnumerator(IEnumerator enumerator, IObserver<Unit> observer, CancellationToken cancellationToken, bool publishEveryYield)
		{
			bool hasNext = false;
			bool raisedError = false;
			ICustomYieldInstructionErrorHandler customHandler;
			for (;;)
			{
				try
				{
					hasNext = enumerator.MoveNext();
				}
				catch (Exception error)
				{
					try
					{
						raisedError = true;
						observer.OnError(error);
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					yield break;
				}
				if (hasNext && publishEveryYield)
				{
					try
					{
						observer.OnNext(Unit.Default);
					}
					catch
					{
						IDisposable disposable2 = enumerator as IDisposable;
						if (disposable2 != null)
						{
							disposable2.Dispose();
						}
						throw;
					}
				}
				if (hasNext)
				{
					object current = enumerator.Current;
					customHandler = (current as ICustomYieldInstructionErrorHandler);
					if (customHandler != null && customHandler.IsReThrowOnError)
					{
						customHandler.ForceDisableRethrowOnError();
						yield return current;
						customHandler.ForceEnableRethrowOnError();
						if (customHandler.HasError)
						{
							break;
						}
					}
					else
					{
						yield return enumerator.Current;
					}
				}
				if (!hasNext)
				{
					goto IL_1DC;
				}
				if (cancellationToken.IsCancellationRequested)
				{
					goto Block_9;
				}
			}
			try
			{
				raisedError = true;
				observer.OnError(customHandler.Error);
			}
			finally
			{
				IDisposable disposable3 = enumerator as IDisposable;
				if (disposable3 != null)
				{
					disposable3.Dispose();
				}
			}
			yield break;
			Block_9:
			try
			{
				IL_1DC:
				if (!raisedError && !cancellationToken.IsCancellationRequested)
				{
					observer.OnNext(Unit.Default);
					observer.OnCompleted();
				}
			}
			finally
			{
				IDisposable disposable4 = enumerator as IDisposable;
				if (disposable4 != null)
				{
					disposable4.Dispose();
				}
			}
			yield break;
		}

		public static IObservable<T> FromCoroutineValue<T>(Func<IEnumerator> coroutine, bool nullAsNextUpdate = true)
		{
			return Observable.FromCoroutine<T>((IObserver<T> observer, CancellationToken cancellationToken) => Observable.WrapEnumeratorYieldValue<T>(coroutine(), observer, cancellationToken, nullAsNextUpdate));
		}

		public static IObservable<T> FromCoroutineValue<T>(Func<CancellationToken, IEnumerator> coroutine, bool nullAsNextUpdate = true)
		{
			return Observable.FromCoroutine<T>((IObserver<T> observer, CancellationToken cancellationToken) => Observable.WrapEnumeratorYieldValue<T>(coroutine(cancellationToken), observer, cancellationToken, nullAsNextUpdate));
		}

		private static IEnumerator WrapEnumeratorYieldValue<T>(IEnumerator enumerator, IObserver<T> observer, CancellationToken cancellationToken, bool nullAsNextUpdate)
		{
			bool hasNext = false;
			object current = null;
			bool raisedError = false;
			ICustomYieldInstructionErrorHandler customHandler;
			for (;;)
			{
				try
				{
					hasNext = enumerator.MoveNext();
					if (hasNext)
					{
						current = enumerator.Current;
					}
				}
				catch (Exception error)
				{
					try
					{
						raisedError = true;
						observer.OnError(error);
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					yield break;
				}
				if (hasNext)
				{
					if (current != null && Observable.YieldInstructionTypes.Contains(current.GetType()))
					{
						yield return current;
					}
					else if (current is IEnumerator)
					{
						customHandler = (current as ICustomYieldInstructionErrorHandler);
						if (customHandler != null && customHandler.IsReThrowOnError)
						{
							customHandler.ForceDisableRethrowOnError();
							yield return current;
							customHandler.ForceEnableRethrowOnError();
							if (customHandler.HasError)
							{
								break;
							}
						}
						else
						{
							yield return current;
						}
					}
					else if (current == null && nullAsNextUpdate)
					{
						yield return null;
					}
					else
					{
						try
						{
							observer.OnNext((T)((object)current));
						}
						catch
						{
							IDisposable disposable2 = enumerator as IDisposable;
							if (disposable2 != null)
							{
								disposable2.Dispose();
							}
							throw;
						}
					}
				}
				if (!hasNext)
				{
					goto IL_276;
				}
				if (cancellationToken.IsCancellationRequested)
				{
					goto Block_13;
				}
			}
			try
			{
				raisedError = true;
				observer.OnError(customHandler.Error);
			}
			finally
			{
				IDisposable disposable3 = enumerator as IDisposable;
				if (disposable3 != null)
				{
					disposable3.Dispose();
				}
			}
			yield break;
			Block_13:
			try
			{
				IL_276:
				if (!raisedError && !cancellationToken.IsCancellationRequested)
				{
					observer.OnCompleted();
				}
			}
			finally
			{
				IDisposable disposable4 = enumerator as IDisposable;
				if (disposable4 != null)
				{
					disposable4.Dispose();
				}
			}
			yield break;
		}

		public static IObservable<T> FromCoroutine<T>(Func<IObserver<T>, IEnumerator> coroutine)
		{
			return Observable.FromCoroutine<T>((IObserver<T> observer, CancellationToken cancellationToken) => Observable.WrapToCancellableEnumerator(coroutine(observer), cancellationToken));
		}

		public static IObservable<T> FromMicroCoroutine<T>(Func<IObserver<T>, IEnumerator> coroutine, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Observable.FromMicroCoroutine<T>((IObserver<T> observer, CancellationToken cancellationToken) => Observable.WrapToCancellableEnumerator(coroutine(observer), cancellationToken), frameCountType);
		}

		private static IEnumerator WrapToCancellableEnumerator(IEnumerator enumerator, CancellationToken cancellationToken)
		{
			bool hasNext = false;
			do
			{
				try
				{
					hasNext = enumerator.MoveNext();
				}
				catch
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
					yield break;
				}
				yield return enumerator.Current;
			}
			while (hasNext && !cancellationToken.IsCancellationRequested);
			IDisposable disposable2 = enumerator as IDisposable;
			if (disposable2 != null)
			{
				disposable2.Dispose();
			}
			yield break;
		}

		public static IObservable<T> FromCoroutine<T>(Func<IObserver<T>, CancellationToken, IEnumerator> coroutine)
		{
			return new FromCoroutineObservable<T>(coroutine);
		}

		public static IObservable<T> FromMicroCoroutine<T>(Func<IObserver<T>, CancellationToken, IEnumerator> coroutine, FrameCountType frameCountType = FrameCountType.Update)
		{
			return new FromMicroCoroutineObservable<T>(coroutine, frameCountType);
		}

		public static IObservable<Unit> SelectMany<T>(this IObservable<T> source, IEnumerator coroutine, bool publishEveryYield = false)
		{
			return source.SelectMany(Observable.FromCoroutine(() => coroutine, publishEveryYield));
		}

		public static IObservable<Unit> SelectMany<T>(this IObservable<T> source, Func<IEnumerator> selector, bool publishEveryYield = false)
		{
			return source.SelectMany(Observable.FromCoroutine(() => selector(), publishEveryYield));
		}

		public static IObservable<Unit> SelectMany<T>(this IObservable<T> source, Func<T, IEnumerator> selector)
		{
			return source.SelectMany((T x) => Observable.FromCoroutine(() => selector(x), false));
		}

		public static IObservable<Unit> ToObservable(this IEnumerator coroutine, bool publishEveryYield = false)
		{
			return Observable.FromCoroutine<Unit>((IObserver<Unit> observer, CancellationToken cancellationToken) => Observable.WrapEnumerator(coroutine, observer, cancellationToken, publishEveryYield));
		}

		public static ObservableYieldInstruction<Unit> ToYieldInstruction(this IEnumerator coroutine)
		{
			return coroutine.ToObservable(false).ToYieldInstruction<Unit>();
		}

		public static ObservableYieldInstruction<Unit> ToYieldInstruction(this IEnumerator coroutine, bool throwOnError)
		{
			return coroutine.ToObservable(false).ToYieldInstruction(throwOnError);
		}

		public static ObservableYieldInstruction<Unit> ToYieldInstruction(this IEnumerator coroutine, CancellationToken cancellationToken)
		{
			return coroutine.ToObservable(false).ToYieldInstruction(cancellationToken);
		}

		public static ObservableYieldInstruction<Unit> ToYieldInstruction(this IEnumerator coroutine, bool throwOnError, CancellationToken cancellationToken)
		{
			return coroutine.ToObservable(false).ToYieldInstruction(throwOnError, cancellationToken);
		}

		public static IObservable<long> EveryUpdate()
		{
			if (Observable.<>f__mg$cache0 == null)
			{
				Observable.<>f__mg$cache0 = new Func<IObserver<long>, CancellationToken, IEnumerator>(Observable.EveryCycleCore);
			}
			return Observable.FromMicroCoroutine<long>(Observable.<>f__mg$cache0, FrameCountType.Update);
		}

		public static IObservable<long> EveryFixedUpdate()
		{
			if (Observable.<>f__mg$cache1 == null)
			{
				Observable.<>f__mg$cache1 = new Func<IObserver<long>, CancellationToken, IEnumerator>(Observable.EveryCycleCore);
			}
			return Observable.FromMicroCoroutine<long>(Observable.<>f__mg$cache1, FrameCountType.FixedUpdate);
		}

		public static IObservable<long> EveryEndOfFrame()
		{
			if (Observable.<>f__mg$cache2 == null)
			{
				Observable.<>f__mg$cache2 = new Func<IObserver<long>, CancellationToken, IEnumerator>(Observable.EveryCycleCore);
			}
			return Observable.FromMicroCoroutine<long>(Observable.<>f__mg$cache2, FrameCountType.EndOfFrame);
		}

		private static IEnumerator EveryCycleCore(IObserver<long> observer, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				yield break;
			}
			long count = 0L;
			for (;;)
			{
				yield return null;
				if (cancellationToken.IsCancellationRequested)
				{
					break;
				}
				long value;
				count = (value = count) + 1L;
				observer.OnNext(value);
			}
			yield break;
			yield break;
		}

		public static IObservable<long> EveryGameObjectUpdate()
		{
			return MainThreadDispatcher.UpdateAsObservable().Scan(-1L, (long x, Unit y) => x + 1L);
		}

		public static IObservable<long> EveryLateUpdate()
		{
			return MainThreadDispatcher.LateUpdateAsObservable().Scan(-1L, (long x, Unit y) => x + 1L);
		}

		[Obsolete]
		public static IObservable<long> EveryAfterUpdate()
		{
			return Observable.FromCoroutine<long>((IObserver<long> observer, CancellationToken cancellationToken) => new Observable.EveryAfterUpdateInvoker(observer, cancellationToken));
		}

		public static IObservable<Unit> NextFrame(FrameCountType frameCountType = FrameCountType.Update)
		{
			if (Observable.<>f__mg$cache3 == null)
			{
				Observable.<>f__mg$cache3 = new Func<IObserver<Unit>, CancellationToken, IEnumerator>(Observable.NextFrameCore);
			}
			return Observable.FromMicroCoroutine<Unit>(Observable.<>f__mg$cache3, frameCountType);
		}

		private static IEnumerator NextFrameCore(IObserver<Unit> observer, CancellationToken cancellation)
		{
			yield return null;
			if (!cancellation.IsCancellationRequested)
			{
				observer.OnNext(Unit.Default);
				observer.OnCompleted();
			}
			yield break;
		}

		public static IObservable<long> IntervalFrame(int intervalFrameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Observable.TimerFrame(intervalFrameCount, intervalFrameCount, frameCountType);
		}

		public static IObservable<long> TimerFrame(int dueTimeFrameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Observable.FromMicroCoroutine<long>((IObserver<long> observer, CancellationToken cancellation) => Observable.TimerFrameCore(observer, dueTimeFrameCount, cancellation), frameCountType);
		}

		public static IObservable<long> TimerFrame(int dueTimeFrameCount, int periodFrameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Observable.FromMicroCoroutine<long>((IObserver<long> observer, CancellationToken cancellation) => Observable.TimerFrameCore(observer, dueTimeFrameCount, periodFrameCount, cancellation), frameCountType);
		}

		private static IEnumerator TimerFrameCore(IObserver<long> observer, int dueTimeFrameCount, CancellationToken cancel)
		{
			if (dueTimeFrameCount <= 0)
			{
				dueTimeFrameCount = 0;
			}
			int currentFrame = 0;
			while (!cancel.IsCancellationRequested)
			{
				int num;
				currentFrame = (num = currentFrame) + 1;
				if (num == dueTimeFrameCount)
				{
					observer.OnNext(0L);
					observer.OnCompleted();
					break;
				}
				yield return null;
			}
			yield break;
		}

		private static IEnumerator TimerFrameCore(IObserver<long> observer, int dueTimeFrameCount, int periodFrameCount, CancellationToken cancel)
		{
			if (dueTimeFrameCount <= 0)
			{
				dueTimeFrameCount = 0;
			}
			if (periodFrameCount <= 0)
			{
				periodFrameCount = 1;
			}
			long sendCount = 0L;
			int currentFrame = 0;
			while (!cancel.IsCancellationRequested)
			{
				int num;
				currentFrame = (num = currentFrame) + 1;
				if (num == dueTimeFrameCount)
				{
					long value;
					sendCount = (value = sendCount) + 1L;
					observer.OnNext(value);
					currentFrame = -1;
					break;
				}
				yield return null;
			}
			while (!cancel.IsCancellationRequested)
			{
				if (++currentFrame == periodFrameCount)
				{
					long value;
					sendCount = (value = sendCount) + 1L;
					observer.OnNext(value);
					currentFrame = 0;
				}
				yield return null;
			}
			yield break;
		}

		public static IObservable<T> DelayFrame<T>(this IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			if (frameCount < 0)
			{
				throw new ArgumentOutOfRangeException("frameCount");
			}
			return new DelayFrameObservable<T>(source, frameCount, frameCountType);
		}

		public static IObservable<T> Sample<T, T2>(this IObservable<T> source, IObservable<T2> sampler)
		{
			return new SampleObservable<T, T2>(source, sampler);
		}

		public static IObservable<T> SampleFrame<T>(this IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			if (frameCount < 0)
			{
				throw new ArgumentOutOfRangeException("frameCount");
			}
			return new SampleFrameObservable<T>(source, frameCount, frameCountType);
		}

		public static IObservable<TSource> ThrottleFrame<TSource>(this IObservable<TSource> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			if (frameCount < 0)
			{
				throw new ArgumentOutOfRangeException("frameCount");
			}
			return new ThrottleFrameObservable<TSource>(source, frameCount, frameCountType);
		}

		public static IObservable<TSource> ThrottleFirstFrame<TSource>(this IObservable<TSource> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			if (frameCount < 0)
			{
				throw new ArgumentOutOfRangeException("frameCount");
			}
			return new ThrottleFirstFrameObservable<TSource>(source, frameCount, frameCountType);
		}

		public static IObservable<T> TimeoutFrame<T>(this IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			if (frameCount < 0)
			{
				throw new ArgumentOutOfRangeException("frameCount");
			}
			return new TimeoutFrameObservable<T>(source, frameCount, frameCountType);
		}

		public static IObservable<T> DelayFrameSubscription<T>(this IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			if (frameCount < 0)
			{
				throw new ArgumentOutOfRangeException("frameCount");
			}
			return new DelayFrameSubscriptionObservable<T>(source, frameCount, frameCountType);
		}

		public static ObservableYieldInstruction<T> ToYieldInstruction<T>(this IObservable<T> source)
		{
			return new ObservableYieldInstruction<T>(source, true, CancellationToken.None);
		}

		public static ObservableYieldInstruction<T> ToYieldInstruction<T>(this IObservable<T> source, CancellationToken cancel)
		{
			return new ObservableYieldInstruction<T>(source, true, cancel);
		}

		public static ObservableYieldInstruction<T> ToYieldInstruction<T>(this IObservable<T> source, bool throwOnError)
		{
			return new ObservableYieldInstruction<T>(source, throwOnError, CancellationToken.None);
		}

		public static ObservableYieldInstruction<T> ToYieldInstruction<T>(this IObservable<T> source, bool throwOnError, CancellationToken cancel)
		{
			return new ObservableYieldInstruction<T>(source, throwOnError, cancel);
		}

		public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, CancellationToken cancel = default(CancellationToken))
		{
			return source.ToAwaitableEnumerator(Stubs<T>.Ignore, Stubs.Throw, cancel);
		}

		public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, Action<T> onResult, CancellationToken cancel = default(CancellationToken))
		{
			return source.ToAwaitableEnumerator(onResult, Stubs.Throw, cancel);
		}

		public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, Action<Exception> onError, CancellationToken cancel = default(CancellationToken))
		{
			return source.ToAwaitableEnumerator(Stubs<T>.Ignore, onError, cancel);
		}

		public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, Action<T> onResult, Action<Exception> onError, CancellationToken cancel = default(CancellationToken))
		{
			ObservableYieldInstruction<T> enumerator = new ObservableYieldInstruction<T>(source, false, cancel);
			IEnumerator<T> e = enumerator;
			while (e.MoveNext() && !cancel.IsCancellationRequested)
			{
				yield return null;
			}
			if (cancel.IsCancellationRequested)
			{
				enumerator.Dispose();
				yield break;
			}
			if (enumerator.HasResult)
			{
				onResult(enumerator.Result);
			}
			else if (enumerator.HasError)
			{
				onError(enumerator.Error);
			}
			yield break;
		}

		public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, CancellationToken cancel = default(CancellationToken))
		{
			return source.StartAsCoroutine(Stubs<T>.Ignore, Stubs.Throw, cancel);
		}

		public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, Action<T> onResult, CancellationToken cancel = default(CancellationToken))
		{
			return source.StartAsCoroutine(onResult, Stubs.Throw, cancel);
		}

		public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, Action<Exception> onError, CancellationToken cancel = default(CancellationToken))
		{
			return source.StartAsCoroutine(Stubs<T>.Ignore, onError, cancel);
		}

		public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, Action<T> onResult, Action<Exception> onError, CancellationToken cancel = default(CancellationToken))
		{
			return MainThreadDispatcher.StartCoroutine(source.ToAwaitableEnumerator(onResult, onError, cancel));
		}

		public static IObservable<T> ObserveOnMainThread<T>(this IObservable<T> source)
		{
			return source.ObserveOn(Scheduler.MainThread);
		}

		public static IObservable<T> ObserveOnMainThread<T>(this IObservable<T> source, MainThreadDispatchType dispatchType)
		{
			switch (dispatchType)
			{
			case MainThreadDispatchType.Update:
				return source.ObserveOnMainThread<T>();
			case MainThreadDispatchType.FixedUpdate:
				return source.SelectMany((T _) => Observable.EveryFixedUpdate().Take(1), (T x, long _) => x);
			case MainThreadDispatchType.EndOfFrame:
				return source.SelectMany((T _) => Observable.EveryEndOfFrame().Take(1), (T x, long _) => x);
			case MainThreadDispatchType.GameObjectUpdate:
				return source.SelectMany((T _) => MainThreadDispatcher.UpdateAsObservable().Take(1), (T x, Unit _) => x);
			case MainThreadDispatchType.LateUpdate:
				return source.SelectMany((T _) => MainThreadDispatcher.LateUpdateAsObservable().Take(1), (T x, Unit _) => x);
			case MainThreadDispatchType.AfterUpdate:
				return source.SelectMany((T _) => Observable.EveryAfterUpdate().Take(1), (T x, long _) => x);
			default:
				throw new ArgumentException("type is invalid");
			}
		}

		public static IObservable<T> SubscribeOnMainThread<T>(this IObservable<T> source)
		{
			return source.SubscribeOn(Scheduler.MainThread);
		}

		public static IObservable<bool> EveryApplicationPause()
		{
			return MainThreadDispatcher.OnApplicationPauseAsObservable().AsObservable<bool>();
		}

		public static IObservable<bool> EveryApplicationFocus()
		{
			return MainThreadDispatcher.OnApplicationFocusAsObservable().AsObservable<bool>();
		}

		public static IObservable<Unit> OnceApplicationQuit()
		{
			return MainThreadDispatcher.OnApplicationQuitAsObservable().Take(1);
		}

		public static IObservable<T> TakeUntilDestroy<T>(this IObservable<T> source, Component target)
		{
			return source.TakeUntil(target.OnDestroyAsObservable());
		}

		public static IObservable<T> TakeUntilDestroy<T>(this IObservable<T> source, GameObject target)
		{
			return source.TakeUntil(target.OnDestroyAsObservable());
		}

		public static IObservable<T> TakeUntilDisable<T>(this IObservable<T> source, Component target)
		{
			return source.TakeUntil(target.OnDisableAsObservable());
		}

		public static IObservable<T> TakeUntilDisable<T>(this IObservable<T> source, GameObject target)
		{
			return source.TakeUntil(target.OnDisableAsObservable());
		}

		public static IObservable<T> RepeatUntilDestroy<T>(this IObservable<T> source, GameObject target)
		{
			return Observable.RepeatInfinite<T>(source).RepeatUntilCore(target.OnDestroyAsObservable(), target);
		}

		public static IObservable<T> RepeatUntilDestroy<T>(this IObservable<T> source, Component target)
		{
			return Observable.RepeatInfinite<T>(source).RepeatUntilCore(target.OnDestroyAsObservable(), (!(target != null)) ? null : target.gameObject);
		}

		public static IObservable<T> RepeatUntilDisable<T>(this IObservable<T> source, GameObject target)
		{
			return Observable.RepeatInfinite<T>(source).RepeatUntilCore(target.OnDisableAsObservable(), target);
		}

		public static IObservable<T> RepeatUntilDisable<T>(this IObservable<T> source, Component target)
		{
			return Observable.RepeatInfinite<T>(source).RepeatUntilCore(target.OnDisableAsObservable(), (!(target != null)) ? null : target.gameObject);
		}

		private static IObservable<T> RepeatUntilCore<T>(this IEnumerable<IObservable<T>> sources, IObservable<Unit> trigger, GameObject lifeTimeChecker)
		{
			return new RepeatUntilObservable<T>(sources, trigger, lifeTimeChecker);
		}

		public static IObservable<FrameInterval<T>> FrameInterval<T>(this IObservable<T> source)
		{
			return new FrameIntervalObservable<T>(source);
		}

		public static IObservable<TimeInterval<T>> FrameTimeInterval<T>(this IObservable<T> source, bool ignoreTimeScale = false)
		{
			return new FrameTimeIntervalObservable<T>(source, ignoreTimeScale);
		}

		public static IObservable<IList<T>> BatchFrame<T>(this IObservable<T> source)
		{
			return source.BatchFrame(0, FrameCountType.EndOfFrame);
		}

		public static IObservable<IList<T>> BatchFrame<T>(this IObservable<T> source, int frameCount, FrameCountType frameCountType)
		{
			if (frameCount < 0)
			{
				throw new ArgumentException("frameCount must be >= 0, frameCount:" + frameCount);
			}
			return new BatchFrameObservable<T>(source, frameCount, frameCountType);
		}

		public static IObservable<Unit> BatchFrame(this IObservable<Unit> source)
		{
			return source.BatchFrame(0, FrameCountType.EndOfFrame);
		}

		public static IObservable<Unit> BatchFrame(this IObservable<Unit> source, int frameCount, FrameCountType frameCountType)
		{
			if (frameCount < 0)
			{
				throw new ArgumentException("frameCount must be >= 0, frameCount:" + frameCount);
			}
			return new BatchFrameObservable(source, frameCount, frameCountType);
		}

		private class ConnectableObservable<T> : IConnectableObservable<T>, IObservable<T>
		{
			private readonly IObservable<T> source;

			private readonly ISubject<T> subject;

			private readonly object gate = new object();

			private Observable.ConnectableObservable<T>.Connection connection;

			public ConnectableObservable(IObservable<T> source, ISubject<T> subject)
			{
				this.source = source.AsObservable<T>();
				this.subject = subject;
			}

			public IDisposable Connect()
			{
				object obj = this.gate;
				IDisposable result;
				lock (obj)
				{
					if (this.connection == null)
					{
						IDisposable subscription = this.source.Subscribe(this.subject);
						this.connection = new Observable.ConnectableObservable<T>.Connection(this, subscription);
					}
					result = this.connection;
				}
				return result;
			}

			public IDisposable Subscribe(IObserver<T> observer)
			{
				return this.subject.Subscribe(observer);
			}

			private class Connection : IDisposable
			{
				private readonly Observable.ConnectableObservable<T> parent;

				private IDisposable subscription;

				public Connection(Observable.ConnectableObservable<T> parent, IDisposable subscription)
				{
					this.parent = parent;
					this.subscription = subscription;
				}

				public void Dispose()
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (this.subscription != null)
						{
							this.subscription.Dispose();
							this.subscription = null;
							this.parent.connection = null;
						}
					}
				}
			}
		}

		private class EveryAfterUpdateInvoker : IEnumerator
		{
			private long count = -1L;

			private readonly IObserver<long> observer;

			private readonly CancellationToken cancellationToken;

			public EveryAfterUpdateInvoker(IObserver<long> observer, CancellationToken cancellationToken)
			{
				this.observer = observer;
				this.cancellationToken = cancellationToken;
			}

			public bool MoveNext()
			{
				if (!this.cancellationToken.IsCancellationRequested)
				{
					if (this.count != -1L)
					{
						IObserver<long> observer = this.observer;
						long value;
						this.count = (value = this.count) + 1L;
						observer.OnNext(value);
					}
					else
					{
						this.count += 1L;
					}
					return true;
				}
				return false;
			}

			public object Current
			{
				get
				{
					return null;
				}
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}
	}
}
