using System;
using System.Collections.Generic;

namespace UniRx
{
	public static class ReactivePropertyExtensions
	{
		public static ReactiveProperty<T> ToReactiveProperty<T>(this IObservable<T> source)
		{
			return new ReactiveProperty<T>(source);
		}

		public static ReactiveProperty<T> ToReactiveProperty<T>(this IObservable<T> source, T initialValue)
		{
			return new ReactiveProperty<T>(source, initialValue);
		}

		public static ReadOnlyReactiveProperty<T> ToReadOnlyReactiveProperty<T>(this IObservable<T> source)
		{
			return new ReadOnlyReactiveProperty<T>(source);
		}

		public static ReadOnlyReactiveProperty<T> ToSequentialReadOnlyReactiveProperty<T>(this IObservable<T> source)
		{
			return new ReadOnlyReactiveProperty<T>(source, false);
		}

		public static ReadOnlyReactiveProperty<T> ToReadOnlyReactiveProperty<T>(this IObservable<T> source, T initialValue)
		{
			return new ReadOnlyReactiveProperty<T>(source, initialValue);
		}

		public static ReadOnlyReactiveProperty<T> ToSequentialReadOnlyReactiveProperty<T>(this IObservable<T> source, T initialValue)
		{
			return new ReadOnlyReactiveProperty<T>(source, initialValue, false);
		}

		public static IObservable<T> SkipLatestValueOnSubscribe<T>(this IReadOnlyReactiveProperty<T> source)
		{
			return (!source.HasValue) ? source : source.Skip(1);
		}

		public static IObservable<bool> CombineLatestValuesAreAllTrue(this IEnumerable<IObservable<bool>> sources)
		{
			return sources.CombineLatest<bool>().Select(delegate(IList<bool> xs)
			{
				using (IEnumerator<bool> enumerator = xs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current)
						{
							return false;
						}
					}
				}
				return true;
			});
		}

		public static IObservable<bool> CombineLatestValuesAreAllFalse(this IEnumerable<IObservable<bool>> sources)
		{
			return sources.CombineLatest<bool>().Select(delegate(IList<bool> xs)
			{
				foreach (bool flag in xs)
				{
					if (flag)
					{
						return false;
					}
				}
				return true;
			});
		}
	}
}
