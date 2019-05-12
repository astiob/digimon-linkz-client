using System;

namespace System.Threading
{
	/// <summary>Represents a lock that is used to manage access to a resource, allowing multiple threads for reading or exclusive access for writing.</summary>
	public class ReaderWriterLockSlim : IDisposable
	{
		private static readonly bool smp = Environment.ProcessorCount > 1;

		private int myLock;

		private int owners;

		private Thread upgradable_thread;

		private Thread write_thread;

		private uint numWriteWaiters;

		private uint numReadWaiters;

		private uint numUpgradeWaiters;

		private EventWaitHandle writeEvent;

		private EventWaitHandle readEvent;

		private EventWaitHandle upgradeEvent;

		private readonly LockRecursionPolicy recursionPolicy;

		private ReaderWriterLockSlim.LockDetails[] read_locks = new ReaderWriterLockSlim.LockDetails[8];

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.ReaderWriterLockSlim" /> class with default property values.</summary>
		public ReaderWriterLockSlim()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.ReaderWriterLockSlim" /> class, specifying the lock recursion policy.</summary>
		/// <param name="recursionPolicy">One of the enumeration values that specifies the lock recursion policy. </param>
		public ReaderWriterLockSlim(LockRecursionPolicy recursionPolicy)
		{
			this.recursionPolicy = recursionPolicy;
			if (recursionPolicy != LockRecursionPolicy.NoRecursion)
			{
				throw new NotImplementedException("recursionPolicy != NoRecursion not currently implemented");
			}
		}

		/// <summary>Tries to enter the lock in read mode.</summary>
		/// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered read mode. -or-The recursion number would exceed the capacity of the counter. This limit is so large that applications should never encounter it.</exception>
		public void EnterReadLock()
		{
			this.TryEnterReadLock(-1);
		}

		/// <summary>Tries to enter the lock in read mode, with an optional integer time-out.</summary>
		/// <returns>true if the calling thread entered read mode, otherwise, false.</returns>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or -1 (<see cref="F:System.Threading.Timeout.Infinite" />) to wait indefinitely.</param>
		/// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock. -or-The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="millisecondsTimeout" /> is negative, but it is not equal to <see cref="F:System.Threading.Timeout.Infinite" /> (-1), which is the only negative value allowed. </exception>
		public bool TryEnterReadLock(int millisecondsTimeout)
		{
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout");
			}
			if (this.read_locks == null)
			{
				throw new ObjectDisposedException(null);
			}
			if (Thread.CurrentThread == this.write_thread)
			{
				throw new LockRecursionException("Read lock cannot be acquired while write lock is held");
			}
			this.EnterMyLock();
			ReaderWriterLockSlim.LockDetails readLockDetails = this.GetReadLockDetails(Thread.CurrentThread.ManagedThreadId, true);
			if (readLockDetails.ReadLocks != 0)
			{
				this.ExitMyLock();
				throw new LockRecursionException("Recursive read lock can only be aquired in SupportsRecursion mode");
			}
			readLockDetails.ReadLocks++;
			while (this.owners < 0 || this.numWriteWaiters != 0u)
			{
				if (millisecondsTimeout == 0)
				{
					this.ExitMyLock();
					return false;
				}
				if (this.readEvent == null)
				{
					this.LazyCreateEvent(ref this.readEvent, false);
				}
				else if (!this.WaitOnEvent(this.readEvent, ref this.numReadWaiters, millisecondsTimeout))
				{
					return false;
				}
			}
			this.owners++;
			this.ExitMyLock();
			return true;
		}

		/// <summary>Tries to enter the lock in read mode, with an optional time-out.</summary>
		/// <returns>true if the calling thread entered read mode, otherwise, false.</returns>
		/// <param name="timeout">The interval to wait, or -1 milliseconds to wait indefinitely. </param>
		/// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock. -or-The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="timeout" /> is negative, but it is not equal to -1 milliseconds, which is the only negative value allowed.-or-The value of <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" /> milliseconds. </exception>
		public bool TryEnterReadLock(TimeSpan timeout)
		{
			return this.TryEnterReadLock(ReaderWriterLockSlim.CheckTimeout(timeout));
		}

		/// <summary>Reduces the recursion count for read mode, and exits read mode if the resulting count is 0 (zero).</summary>
		/// <exception cref="T:System.Threading.SynchronizationLockException">The current thread has not entered the lock in read mode.</exception>
		public void ExitReadLock()
		{
			this.EnterMyLock();
			if (this.owners < 1)
			{
				this.ExitMyLock();
				throw new SynchronizationLockException("Releasing lock and no read lock taken");
			}
			this.owners--;
			this.GetReadLockDetails(Thread.CurrentThread.ManagedThreadId, false).ReadLocks--;
			this.ExitAndWakeUpAppropriateWaiters();
		}

		/// <summary>Tries to enter the lock in write mode.</summary>
		/// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock in any mode. -or-The current thread has entered read mode, so trying to enter the lock in write mode would create the possibility of a deadlock. -or-The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
		public void EnterWriteLock()
		{
			this.TryEnterWriteLock(-1);
		}

		/// <summary>Tries to enter the lock in write mode, with an optional time-out.</summary>
		/// <returns>true if the calling thread entered write mode, otherwise, false.</returns>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or -1 (<see cref="F:System.Threading.Timeout.Infinite" />) to wait indefinitely.</param>
		/// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock. -or-The current thread initially entered the lock in read mode, and therefore trying to enter write mode would create the possibility of a deadlock. -or-The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="millisecondsTimeout" /> is negative, but it is not equal to <see cref="F:System.Threading.Timeout.Infinite" /> (-1), which is the only negative value allowed. </exception>
		public bool TryEnterWriteLock(int millisecondsTimeout)
		{
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout");
			}
			if (this.read_locks == null)
			{
				throw new ObjectDisposedException(null);
			}
			if (this.IsWriteLockHeld)
			{
				throw new LockRecursionException();
			}
			this.EnterMyLock();
			ReaderWriterLockSlim.LockDetails readLockDetails = this.GetReadLockDetails(Thread.CurrentThread.ManagedThreadId, false);
			if (readLockDetails != null && readLockDetails.ReadLocks > 0)
			{
				this.ExitMyLock();
				throw new LockRecursionException("Write lock cannot be acquired while read lock is held");
			}
			while (this.owners != 0)
			{
				if (this.owners == 1 && this.upgradable_thread == Thread.CurrentThread)
				{
					this.owners = -1;
					this.write_thread = Thread.CurrentThread;
					IL_178:
					this.ExitMyLock();
					return true;
				}
				if (millisecondsTimeout == 0)
				{
					this.ExitMyLock();
					return false;
				}
				if (this.upgradable_thread == Thread.CurrentThread)
				{
					if (this.upgradeEvent == null)
					{
						this.LazyCreateEvent(ref this.upgradeEvent, false);
					}
					else
					{
						if (this.numUpgradeWaiters > 0u)
						{
							this.ExitMyLock();
							throw new ApplicationException("Upgrading lock to writer lock already in process, deadlock");
						}
						if (!this.WaitOnEvent(this.upgradeEvent, ref this.numUpgradeWaiters, millisecondsTimeout))
						{
							return false;
						}
					}
				}
				else if (this.writeEvent == null)
				{
					this.LazyCreateEvent(ref this.writeEvent, true);
				}
				else if (!this.WaitOnEvent(this.writeEvent, ref this.numWriteWaiters, millisecondsTimeout))
				{
					return false;
				}
			}
			this.owners = -1;
			this.write_thread = Thread.CurrentThread;
			goto IL_178;
		}

		/// <summary>Tries to enter the lock in write mode, with an optional time-out.</summary>
		/// <returns>true if the calling thread entered write mode, otherwise, false.</returns>
		/// <param name="timeout">The interval to wait, or -1 milliseconds to wait indefinitely.</param>
		/// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock. -or-The current thread initially entered the lock in read mode, and therefore trying to enter write mode would create the possibility of a deadlock. -or-The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="timeout" /> is negative, but it is not equal to -1 milliseconds, which is the only negative value allowed.-or-The value of <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" /> milliseconds. </exception>
		public bool TryEnterWriteLock(TimeSpan timeout)
		{
			return this.TryEnterWriteLock(ReaderWriterLockSlim.CheckTimeout(timeout));
		}

		/// <summary>Reduces the recursion count for write mode, and exits write mode if the resulting count is 0 (zero).</summary>
		/// <exception cref="T:System.Threading.SynchronizationLockException">The current thread has not entered the lock in write mode.</exception>
		public void ExitWriteLock()
		{
			this.EnterMyLock();
			if (this.owners != -1)
			{
				this.ExitMyLock();
				throw new SynchronizationLockException("Calling ExitWriterLock when no write lock is held");
			}
			if (this.upgradable_thread == Thread.CurrentThread)
			{
				this.owners = 1;
			}
			else
			{
				this.owners = 0;
			}
			this.write_thread = null;
			this.ExitAndWakeUpAppropriateWaiters();
		}

		/// <summary>Tries to enter the lock in upgradeable mode.</summary>
		/// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock in any mode. -or-The current thread has entered read mode, so trying to enter upgradeable mode would create the possibility of a deadlock. -or-The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
		public void EnterUpgradeableReadLock()
		{
			this.TryEnterUpgradeableReadLock(-1);
		}

		/// <summary>Tries to enter the lock in upgradeable mode, with an optional time-out.</summary>
		/// <returns>true if the calling thread entered upgradeable mode, otherwise, false.</returns>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or -1 (<see cref="F:System.Threading.Timeout.Infinite" />) to wait indefinitely.</param>
		/// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock. -or-The current thread initially entered the lock in read mode, and therefore trying to enter upgradeable mode would create the possibility of a deadlock. -or-The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="millisecondsTimeout" /> is negative, but it is not equal to <see cref="F:System.Threading.Timeout.Infinite" /> (-1), which is the only negative value allowed. </exception>
		public bool TryEnterUpgradeableReadLock(int millisecondsTimeout)
		{
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout");
			}
			if (this.read_locks == null)
			{
				throw new ObjectDisposedException(null);
			}
			if (this.IsUpgradeableReadLockHeld)
			{
				throw new LockRecursionException();
			}
			if (this.IsWriteLockHeld)
			{
				throw new LockRecursionException();
			}
			this.EnterMyLock();
			while (this.owners != 0 || this.numWriteWaiters != 0u || this.upgradable_thread != null)
			{
				if (millisecondsTimeout == 0)
				{
					this.ExitMyLock();
					return false;
				}
				if (this.readEvent == null)
				{
					this.LazyCreateEvent(ref this.readEvent, false);
				}
				else if (!this.WaitOnEvent(this.readEvent, ref this.numReadWaiters, millisecondsTimeout))
				{
					return false;
				}
			}
			this.owners++;
			this.upgradable_thread = Thread.CurrentThread;
			this.ExitMyLock();
			return true;
		}

		/// <summary>Tries to enter the lock in upgradeable mode, with an optional time-out.</summary>
		/// <returns>true if the calling thread entered upgradeable mode, otherwise, false.</returns>
		/// <param name="timeout">The interval to wait, or -1 milliseconds to wait indefinitely.</param>
		/// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock. -or-The current thread initially entered the lock in read mode, and therefore trying to enter upgradeable mode would create the possibility of a deadlock. -or-The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="timeout" /> is negative, but it is not equal to -1 milliseconds, which is the only negative value allowed.-or-The value of <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" /> milliseconds. </exception>
		public bool TryEnterUpgradeableReadLock(TimeSpan timeout)
		{
			return this.TryEnterUpgradeableReadLock(ReaderWriterLockSlim.CheckTimeout(timeout));
		}

		/// <summary>Reduces the recursion count for upgradeable mode, and exits upgradeable mode if the resulting count is 0 (zero).</summary>
		/// <exception cref="T:System.Threading.SynchronizationLockException">The current thread has not entered the lock in upgradeable mode.</exception>
		public void ExitUpgradeableReadLock()
		{
			this.EnterMyLock();
			this.owners--;
			this.upgradable_thread = null;
			this.ExitAndWakeUpAppropriateWaiters();
		}

		/// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.ReaderWriterLockSlim" /> class.</summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.read_locks = null;
		}

		/// <summary>Gets a value that indicates whether the current thread has entered the lock in read mode.</summary>
		/// <returns>true if the current thread has entered read mode; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		public bool IsReadLockHeld
		{
			get
			{
				return this.RecursiveReadCount != 0;
			}
		}

		/// <summary>Gets a value that indicates whether the current thread has entered the lock in write mode.</summary>
		/// <returns>true if the current thread has entered write mode; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		public bool IsWriteLockHeld
		{
			get
			{
				return this.RecursiveWriteCount != 0;
			}
		}

		/// <summary>Gets a value that indicates whether the current thread has entered the lock in upgradeable mode. </summary>
		/// <returns>true if the current thread has entered upgradeable mode; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		public bool IsUpgradeableReadLockHeld
		{
			get
			{
				return this.RecursiveUpgradeCount != 0;
			}
		}

		/// <summary>Gets the total number of unique threads that have entered the lock in read mode.</summary>
		/// <returns>The number of unique threads that have entered the lock in read mode.</returns>
		public int CurrentReadCount
		{
			get
			{
				return this.owners & 268435455;
			}
		}

		/// <summary>Gets the number of times the current thread has entered the lock in read mode, as an indication of recursion.</summary>
		/// <returns>0 (zero) if the current thread has not entered read mode, 1 if the thread has entered read mode but has not entered it recursively, or n if the thread has entered the lock recursively n - 1 times.</returns>
		/// <filterpriority>2</filterpriority>
		public int RecursiveReadCount
		{
			get
			{
				this.EnterMyLock();
				ReaderWriterLockSlim.LockDetails readLockDetails = this.GetReadLockDetails(Thread.CurrentThread.ManagedThreadId, false);
				int result = (readLockDetails != null) ? readLockDetails.ReadLocks : 0;
				this.ExitMyLock();
				return result;
			}
		}

		/// <summary>Gets the number of times the current thread has entered the lock in upgradeable mode, as an indication of recursion.</summary>
		/// <returns>0 if the current thread has not entered upgradeable mode, 1 if the thread has entered upgradeable mode but has not entered it recursively, or n if the thread has entered upgradeable mode recursively n - 1 times.</returns>
		/// <filterpriority>2</filterpriority>
		public int RecursiveUpgradeCount
		{
			get
			{
				return (this.upgradable_thread != Thread.CurrentThread) ? 0 : 1;
			}
		}

		/// <summary>Gets the number of times the current thread has entered the lock in write mode, as an indication of recursion.</summary>
		/// <returns>0 if the current thread has not entered write mode, 1 if the thread has entered write mode but has not entered it recursively, or n if the thread has entered write mode recursively n - 1 times.</returns>
		/// <filterpriority>2</filterpriority>
		public int RecursiveWriteCount
		{
			get
			{
				return (this.write_thread != Thread.CurrentThread) ? 0 : 1;
			}
		}

		/// <summary>Gets the total number of threads that are waiting to enter the lock in read mode.</summary>
		/// <returns>The total number of threads that are waiting to enter read mode.</returns>
		/// <filterpriority>2</filterpriority>
		public int WaitingReadCount
		{
			get
			{
				return (int)this.numReadWaiters;
			}
		}

		/// <summary>Gets the total number of threads that are waiting to enter the lock in upgradeable mode.</summary>
		/// <returns>The total number of threads that are waiting to enter upgradeable mode.</returns>
		/// <filterpriority>2</filterpriority>
		public int WaitingUpgradeCount
		{
			get
			{
				return (int)this.numUpgradeWaiters;
			}
		}

		/// <summary>Gets the total number of threads that are waiting to enter the lock in write mode.</summary>
		/// <returns>The total number of threads that are waiting to enter write mode.</returns>
		/// <filterpriority>2</filterpriority>
		public int WaitingWriteCount
		{
			get
			{
				return (int)this.numWriteWaiters;
			}
		}

		/// <summary>Gets a value that indicates the recursion policy for the current <see cref="T:System.Threading.ReaderWriterLockSlim" /> object.</summary>
		/// <returns>One of the enumeration values that specifies the lock recursion policy.</returns>
		public LockRecursionPolicy RecursionPolicy
		{
			get
			{
				return this.recursionPolicy;
			}
		}

		private void EnterMyLock()
		{
			if (Interlocked.CompareExchange(ref this.myLock, 1, 0) != 0)
			{
				this.EnterMyLockSpin();
			}
		}

		private void EnterMyLockSpin()
		{
			int num = 0;
			for (;;)
			{
				if (num < 3 && ReaderWriterLockSlim.smp)
				{
					Thread.SpinWait(20);
				}
				else
				{
					Thread.Sleep(0);
				}
				if (Interlocked.CompareExchange(ref this.myLock, 1, 0) == 0)
				{
					break;
				}
				num++;
			}
		}

		private void ExitMyLock()
		{
			this.myLock = 0;
		}

		private bool MyLockHeld
		{
			get
			{
				return this.myLock != 0;
			}
		}

		private void ExitAndWakeUpAppropriateWaiters()
		{
			if (this.owners == 1 && this.numUpgradeWaiters != 0u)
			{
				this.ExitMyLock();
				this.upgradeEvent.Set();
			}
			else if (this.owners == 0 && this.numWriteWaiters > 0u)
			{
				this.ExitMyLock();
				this.writeEvent.Set();
			}
			else if (this.owners >= 0 && this.numReadWaiters != 0u)
			{
				this.ExitMyLock();
				this.readEvent.Set();
			}
			else
			{
				this.ExitMyLock();
			}
		}

		private void LazyCreateEvent(ref EventWaitHandle waitEvent, bool makeAutoResetEvent)
		{
			this.ExitMyLock();
			EventWaitHandle eventWaitHandle;
			if (makeAutoResetEvent)
			{
				eventWaitHandle = new AutoResetEvent(false);
			}
			else
			{
				eventWaitHandle = new ManualResetEvent(false);
			}
			this.EnterMyLock();
			if (waitEvent == null)
			{
				waitEvent = eventWaitHandle;
			}
		}

		private bool WaitOnEvent(EventWaitHandle waitEvent, ref uint numWaiters, int millisecondsTimeout)
		{
			waitEvent.Reset();
			numWaiters += 1u;
			bool flag = false;
			this.ExitMyLock();
			try
			{
				flag = waitEvent.WaitOne(millisecondsTimeout, false);
			}
			finally
			{
				this.EnterMyLock();
				numWaiters -= 1u;
				if (!flag)
				{
					this.ExitMyLock();
				}
			}
			return flag;
		}

		private static int CheckTimeout(TimeSpan timeout)
		{
			int result;
			try
			{
				result = checked((int)timeout.TotalMilliseconds);
			}
			catch (OverflowException)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			return result;
		}

		private ReaderWriterLockSlim.LockDetails GetReadLockDetails(int threadId, bool create)
		{
			int i;
			ReaderWriterLockSlim.LockDetails lockDetails;
			for (i = 0; i < this.read_locks.Length; i++)
			{
				lockDetails = this.read_locks[i];
				if (lockDetails == null)
				{
					break;
				}
				if (lockDetails.ThreadId == threadId)
				{
					return lockDetails;
				}
			}
			if (!create)
			{
				return null;
			}
			if (i == this.read_locks.Length)
			{
				Array.Resize<ReaderWriterLockSlim.LockDetails>(ref this.read_locks, this.read_locks.Length * 2);
			}
			lockDetails = (this.read_locks[i] = new ReaderWriterLockSlim.LockDetails());
			lockDetails.ThreadId = threadId;
			return lockDetails;
		}

		private sealed class LockDetails
		{
			public int ThreadId;

			public int ReadLocks;
		}
	}
}
