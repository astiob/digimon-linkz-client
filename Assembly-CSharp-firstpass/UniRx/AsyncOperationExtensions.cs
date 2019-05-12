using System;
using System.Collections;
using UnityEngine;

namespace UniRx
{
	public static class AsyncOperationExtensions
	{
		public static IObservable<AsyncOperation> AsObservable(this AsyncOperation asyncOperation, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<AsyncOperation>((IObserver<AsyncOperation> observer, CancellationToken cancellation) => AsyncOperationExtensions.AsObservableCore<AsyncOperation>(asyncOperation, observer, progress, cancellation));
		}

		public static IObservable<T> AsAsyncOperationObservable<T>(this T asyncOperation, IProgress<float> progress = null) where T : AsyncOperation
		{
			return Observable.FromCoroutine<T>((IObserver<T> observer, CancellationToken cancellation) => AsyncOperationExtensions.AsObservableCore<T>(asyncOperation, observer, progress, cancellation));
		}

		private static IEnumerator AsObservableCore<T>(T asyncOperation, IObserver<T> observer, IProgress<float> reportProgress, CancellationToken cancel) where T : AsyncOperation
		{
			if (reportProgress != null)
			{
				while (!asyncOperation.isDone && !cancel.IsCancellationRequested)
				{
					try
					{
						reportProgress.Report(asyncOperation.progress);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						yield break;
					}
					yield return null;
				}
			}
			else if (!asyncOperation.isDone)
			{
				yield return asyncOperation;
			}
			if (cancel.IsCancellationRequested)
			{
				yield break;
			}
			if (reportProgress != null)
			{
				try
				{
					reportProgress.Report(asyncOperation.progress);
				}
				catch (Exception error2)
				{
					observer.OnError(error2);
					yield break;
				}
			}
			observer.OnNext(asyncOperation);
			observer.OnCompleted();
			yield break;
		}
	}
}
