using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
	public class ObservableYieldInstruction<T> : IEnumerator<T>, ICustomYieldInstructionErrorHandler, IEnumerator, IDisposable
	{
		private readonly IDisposable subscription;

		private readonly CancellationToken cancel;

		private bool reThrowOnError;

		private T current;

		private T result;

		private bool moveNext;

		private bool hasResult;

		private Exception error;

		public ObservableYieldInstruction(IObservable<T> source, bool reThrowOnError, CancellationToken cancel)
		{
			this.moveNext = true;
			this.reThrowOnError = reThrowOnError;
			this.cancel = cancel;
			try
			{
				this.subscription = source.Subscribe(new ObservableYieldInstruction<T>.ToYieldInstruction(this));
			}
			catch
			{
				this.moveNext = false;
				throw;
			}
		}

		public bool HasError
		{
			get
			{
				return this.error != null;
			}
		}

		public bool HasResult
		{
			get
			{
				return this.hasResult;
			}
		}

		public bool IsCanceled
		{
			get
			{
				return !this.hasResult && this.error == null && this.cancel.IsCancellationRequested;
			}
		}

		public bool IsDone
		{
			get
			{
				return this.HasResult || this.HasError || this.cancel.IsCancellationRequested;
			}
		}

		public T Result
		{
			get
			{
				return this.result;
			}
		}

		T IEnumerator<T>.Current
		{
			get
			{
				return this.current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this.current;
			}
		}

		public Exception Error
		{
			get
			{
				return this.error;
			}
		}

		bool IEnumerator.MoveNext()
		{
			if (!this.moveNext)
			{
				if (this.reThrowOnError && this.HasError)
				{
					throw this.Error;
				}
				return false;
			}
			else
			{
				if (this.cancel.IsCancellationRequested)
				{
					this.subscription.Dispose();
					return false;
				}
				return true;
			}
		}

		bool ICustomYieldInstructionErrorHandler.IsReThrowOnError
		{
			get
			{
				return this.reThrowOnError;
			}
		}

		void ICustomYieldInstructionErrorHandler.ForceDisableRethrowOnError()
		{
			this.reThrowOnError = false;
		}

		void ICustomYieldInstructionErrorHandler.ForceEnableRethrowOnError()
		{
			this.reThrowOnError = true;
		}

		public void Dispose()
		{
			this.subscription.Dispose();
		}

		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		private class ToYieldInstruction : IObserver<T>
		{
			private readonly ObservableYieldInstruction<T> parent;

			public ToYieldInstruction(ObservableYieldInstruction<T> parent)
			{
				this.parent = parent;
			}

			public void OnNext(T value)
			{
				this.parent.current = value;
			}

			public void OnError(Exception error)
			{
				this.parent.moveNext = false;
				this.parent.error = error;
			}

			public void OnCompleted()
			{
				this.parent.moveNext = false;
				this.parent.hasResult = true;
				this.parent.result = this.parent.current;
			}
		}
	}
}
