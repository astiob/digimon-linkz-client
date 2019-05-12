using System;
using System.IO;
using Unity.Tasks.Internal;

namespace System.Threading.Tasks
{
	public static class TaskExtensions
	{
		public static Task Unwrap(this Task<Task> task)
		{
			TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
			task.ContinueWith(delegate(Task<Task> t)
			{
				if (t.IsFaulted)
				{
					tcs.TrySetException(t.Exception);
				}
				else if (t.IsCanceled)
				{
					tcs.TrySetCanceled();
				}
				else
				{
					task.Result.ContinueWith(delegate(Task inner)
					{
						if (inner.IsFaulted)
						{
							tcs.TrySetException(inner.Exception);
						}
						else if (inner.IsCanceled)
						{
							tcs.TrySetCanceled();
						}
						else
						{
							tcs.TrySetResult(0);
						}
					});
				}
			});
			return tcs.Task;
		}

		public static Task<T> Unwrap<T>(this Task<Task<T>> task)
		{
			TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
			task.ContinueWith(delegate(Task<Task<T>> t)
			{
				if (t.IsFaulted)
				{
					tcs.TrySetException(t.Exception);
				}
				else if (t.IsCanceled)
				{
					tcs.TrySetCanceled();
				}
				else
				{
					t.Result.ContinueWith(delegate(Task<T> inner)
					{
						if (inner.IsFaulted)
						{
							tcs.TrySetException(inner.Exception);
						}
						else if (inner.IsCanceled)
						{
							tcs.TrySetCanceled();
						}
						else
						{
							tcs.TrySetResult(inner.Result);
						}
					});
				}
			});
			return tcs.Task;
		}

		public static Task<string> ReadToEndAsync(this StreamReader reader)
		{
			return Task.Run<string>(() => reader.ReadToEnd());
		}

		public static Task CopyToAsync(this Stream stream, Stream destination)
		{
			return stream.CopyToAsync(destination, 2048, CancellationToken.None);
		}

		public static Task CopyToAsync(this Stream stream, Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			byte[] buffer = new byte[bufferSize];
			int bytesRead = 0;
			return InternalExtensions.WhileAsync(() => stream.ReadAsync(buffer, 0, bufferSize, cancellationToken).OnSuccess(delegate(Task<int> readTask)
			{
				bytesRead = readTask.Result;
				return bytesRead > 0;
			}), delegate
			{
				cancellationToken.ThrowIfCancellationRequested();
				return destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).OnSuccess(delegate(Task _)
				{
					cancellationToken.ThrowIfCancellationRequested();
				});
			});
		}

		public static Task<int> ReadAsync(this Stream stream, byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
				taskCompletionSource.SetCanceled();
				return taskCompletionSource.Task;
			}
			return Task.Factory.FromAsync<byte[], int, int, int>(new Func<byte[], int, int, AsyncCallback, object, IAsyncResult>(stream.BeginRead), new Func<IAsyncResult, int>(stream.EndRead), buffer, offset, count, null);
		}

		public static Task WriteAsync(this Stream stream, byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
				taskCompletionSource.SetCanceled();
				return taskCompletionSource.Task;
			}
			return Task.Factory.FromAsync<byte[], int, int>(new Func<byte[], int, int, AsyncCallback, object, IAsyncResult>(stream.BeginWrite), new Action<IAsyncResult>(stream.EndWrite), buffer, offset, count, null);
		}
	}
}
