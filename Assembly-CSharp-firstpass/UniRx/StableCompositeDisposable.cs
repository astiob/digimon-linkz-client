using System;
using System.Collections.Generic;
using System.Threading;

namespace UniRx
{
	public abstract class StableCompositeDisposable : ICancelable, IDisposable
	{
		public static ICancelable Create(IDisposable disposable1, IDisposable disposable2)
		{
			if (disposable1 == null)
			{
				throw new ArgumentNullException("disposable1");
			}
			if (disposable2 == null)
			{
				throw new ArgumentNullException("disposable2");
			}
			return new StableCompositeDisposable.Binary(disposable1, disposable2);
		}

		public static ICancelable Create(IDisposable disposable1, IDisposable disposable2, IDisposable disposable3)
		{
			if (disposable1 == null)
			{
				throw new ArgumentNullException("disposable1");
			}
			if (disposable2 == null)
			{
				throw new ArgumentNullException("disposable2");
			}
			if (disposable3 == null)
			{
				throw new ArgumentNullException("disposable3");
			}
			return new StableCompositeDisposable.Trinary(disposable1, disposable2, disposable3);
		}

		public static ICancelable Create(IDisposable disposable1, IDisposable disposable2, IDisposable disposable3, IDisposable disposable4)
		{
			if (disposable1 == null)
			{
				throw new ArgumentNullException("disposable1");
			}
			if (disposable2 == null)
			{
				throw new ArgumentNullException("disposable2");
			}
			if (disposable3 == null)
			{
				throw new ArgumentNullException("disposable3");
			}
			if (disposable4 == null)
			{
				throw new ArgumentNullException("disposable4");
			}
			return new StableCompositeDisposable.Quaternary(disposable1, disposable2, disposable3, disposable4);
		}

		public static ICancelable Create(params IDisposable[] disposables)
		{
			if (disposables == null)
			{
				throw new ArgumentNullException("disposables");
			}
			return new StableCompositeDisposable.NAry(disposables);
		}

		public static ICancelable CreateUnsafe(IDisposable[] disposables)
		{
			return new StableCompositeDisposable.NAryUnsafe(disposables);
		}

		public static ICancelable Create(IEnumerable<IDisposable> disposables)
		{
			if (disposables == null)
			{
				throw new ArgumentNullException("disposables");
			}
			return new StableCompositeDisposable.NAry(disposables);
		}

		public abstract void Dispose();

		public abstract bool IsDisposed { get; }

		private class Binary : StableCompositeDisposable
		{
			private int disposedCallCount = -1;

			private volatile IDisposable _disposable1;

			private volatile IDisposable _disposable2;

			public Binary(IDisposable disposable1, IDisposable disposable2)
			{
				this._disposable1 = disposable1;
				this._disposable2 = disposable2;
			}

			public override bool IsDisposed
			{
				get
				{
					return this.disposedCallCount != -1;
				}
			}

			public override void Dispose()
			{
				if (Interlocked.Increment(ref this.disposedCallCount) == 0)
				{
					this._disposable1.Dispose();
					this._disposable2.Dispose();
				}
			}
		}

		private class Trinary : StableCompositeDisposable
		{
			private int disposedCallCount = -1;

			private volatile IDisposable _disposable1;

			private volatile IDisposable _disposable2;

			private volatile IDisposable _disposable3;

			public Trinary(IDisposable disposable1, IDisposable disposable2, IDisposable disposable3)
			{
				this._disposable1 = disposable1;
				this._disposable2 = disposable2;
				this._disposable3 = disposable3;
			}

			public override bool IsDisposed
			{
				get
				{
					return this.disposedCallCount != -1;
				}
			}

			public override void Dispose()
			{
				if (Interlocked.Increment(ref this.disposedCallCount) == 0)
				{
					this._disposable1.Dispose();
					this._disposable2.Dispose();
					this._disposable3.Dispose();
				}
			}
		}

		private class Quaternary : StableCompositeDisposable
		{
			private int disposedCallCount = -1;

			private volatile IDisposable _disposable1;

			private volatile IDisposable _disposable2;

			private volatile IDisposable _disposable3;

			private volatile IDisposable _disposable4;

			public Quaternary(IDisposable disposable1, IDisposable disposable2, IDisposable disposable3, IDisposable disposable4)
			{
				this._disposable1 = disposable1;
				this._disposable2 = disposable2;
				this._disposable3 = disposable3;
				this._disposable4 = disposable4;
			}

			public override bool IsDisposed
			{
				get
				{
					return this.disposedCallCount != -1;
				}
			}

			public override void Dispose()
			{
				if (Interlocked.Increment(ref this.disposedCallCount) == 0)
				{
					this._disposable1.Dispose();
					this._disposable2.Dispose();
					this._disposable3.Dispose();
					this._disposable4.Dispose();
				}
			}
		}

		private class NAry : StableCompositeDisposable
		{
			private int disposedCallCount = -1;

			private volatile List<IDisposable> _disposables;

			public NAry(IDisposable[] disposables) : this(disposables)
			{
			}

			public NAry(IEnumerable<IDisposable> disposables)
			{
				this._disposables = new List<IDisposable>(disposables);
				if (this._disposables.Contains(null))
				{
					throw new ArgumentException("Disposables can't contains null", "disposables");
				}
			}

			public override bool IsDisposed
			{
				get
				{
					return this.disposedCallCount != -1;
				}
			}

			public override void Dispose()
			{
				if (Interlocked.Increment(ref this.disposedCallCount) == 0)
				{
					foreach (IDisposable disposable in this._disposables)
					{
						disposable.Dispose();
					}
				}
			}
		}

		private class NAryUnsafe : StableCompositeDisposable
		{
			private int disposedCallCount = -1;

			private volatile IDisposable[] _disposables;

			public NAryUnsafe(IDisposable[] disposables)
			{
				this._disposables = disposables;
			}

			public override bool IsDisposed
			{
				get
				{
					return this.disposedCallCount != -1;
				}
			}

			public override void Dispose()
			{
				if (Interlocked.Increment(ref this.disposedCallCount) == 0)
				{
					int num = this._disposables.Length;
					for (int i = 0; i < num; i++)
					{
						this._disposables[i].Dispose();
					}
				}
			}
		}
	}
}
