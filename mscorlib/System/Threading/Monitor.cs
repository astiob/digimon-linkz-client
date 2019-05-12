using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;

namespace System.Threading
{
	/// <summary>Provides a mechanism that synchronizes access to objects.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public static class Monitor
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Monitor_try_enter(object obj, int ms);

		/// <summary>Acquires an exclusive lock on the specified object.</summary>
		/// <param name="obj">The object on which to acquire the monitor lock. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <filterpriority>1</filterpriority>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Enter(object obj);

		/// <summary>Releases an exclusive lock on the specified object.</summary>
		/// <param name="obj">The object on which to release the lock. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.Threading.SynchronizationLockException">The current thread does not own the lock for the specified object. </exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Exit(object obj);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Monitor_pulse(object obj);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Monitor_test_synchronised(object obj);

		/// <summary>Notifies a thread in the waiting queue of a change in the locked object's state.</summary>
		/// <param name="obj">The object a thread is waiting for. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.Threading.SynchronizationLockException">The calling thread does not own the lock for the specified object. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Pulse(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (!Monitor.Monitor_test_synchronised(obj))
			{
				throw new SynchronizationLockException("Object is not synchronized");
			}
			Monitor.Monitor_pulse(obj);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Monitor_pulse_all(object obj);

		/// <summary>Notifies all waiting threads of a change in the object's state.</summary>
		/// <param name="obj">The object that sends the pulse. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.Threading.SynchronizationLockException">The calling thread does not own the lock for the specified object. </exception>
		/// <filterpriority>1</filterpriority>
		public static void PulseAll(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (!Monitor.Monitor_test_synchronised(obj))
			{
				throw new SynchronizationLockException("Object is not synchronized");
			}
			Monitor.Monitor_pulse_all(obj);
		}

		/// <summary>Attempts to acquire an exclusive lock on the specified object.</summary>
		/// <returns>true if the current thread acquires the lock; otherwise, false.</returns>
		/// <param name="obj">The object on which to acquire the lock. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool TryEnter(object obj)
		{
			return Monitor.TryEnter(obj, 0);
		}

		/// <summary>Attempts, for the specified number of milliseconds, to acquire an exclusive lock on the specified object.</summary>
		/// <returns>true if the current thread acquires the lock; otherwise, false.</returns>
		/// <param name="obj">The object on which to acquire the lock. </param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="millisecondsTimeout" /> is negative, and not equal to <see cref="F:System.Threading.Timeout.Infinite" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool TryEnter(object obj, int millisecondsTimeout)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (millisecondsTimeout == -1)
			{
				Monitor.Enter(obj);
				return true;
			}
			if (millisecondsTimeout < 0)
			{
				throw new ArgumentException("negative value for millisecondsTimeout", "millisecondsTimeout");
			}
			return Monitor.Monitor_try_enter(obj, millisecondsTimeout);
		}

		/// <summary>Attempts, for the specified amount of time, to acquire an exclusive lock on the specified object.</summary>
		/// <returns>true if the current thread acquires the lock without blocking; otherwise, false.</returns>
		/// <param name="obj">The object on which to acquire the lock. </param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> representing the amount of time to wait for the lock. A value of –1 millisecond specifies an infinite wait.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="timeout" /> in milliseconds is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite" /> (–1 millisecond), or is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool TryEnter(object obj, TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", "timeout out of range");
			}
			return Monitor.TryEnter(obj, (int)num);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Monitor_wait(object obj, int ms);

		/// <summary>Releases the lock on an object and blocks the current thread until it reacquires the lock.</summary>
		/// <returns>true if the call returned because the caller reacquired the lock for the specified object. This method does not return if the lock is not reacquired.</returns>
		/// <param name="obj">The object on which to wait. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.Threading.SynchronizationLockException">The calling thread does not own the lock for the specified object. </exception>
		/// <exception cref="T:System.Threading.ThreadInterruptedException">The thread that invokes Wait is later interrupted from the waiting state. This happens when another thread calls this thread's <see cref="M:System.Threading.Thread.Interrupt" /> method. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool Wait(object obj)
		{
			return Monitor.Wait(obj, -1);
		}

		/// <summary>Releases the lock on an object and blocks the current thread until it reacquires the lock. If the specified time-out interval elapses, the thread enters the ready queue.</summary>
		/// <returns>true if the lock was reacquired before the specified time elapsed; false if the lock was reacquired after the specified time elapsed. The method does not return until the lock is reacquired.</returns>
		/// <param name="obj">The object on which to wait. </param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait before the thread enters the ready queue. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.Threading.SynchronizationLockException">The calling thread does not own the lock for the specified object. </exception>
		/// <exception cref="T:System.Threading.ThreadInterruptedException">The thread that invokes Wait is later interrupted from the waiting state. This happens when another thread calls this thread's <see cref="M:System.Threading.Thread.Interrupt" /> method. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <paramref name="millisecondsTimeout" /> parameter is negative, and is not equal to <see cref="F:System.Threading.Timeout.Infinite" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool Wait(object obj, int millisecondsTimeout)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", "timeout out of range");
			}
			if (!Monitor.Monitor_test_synchronised(obj))
			{
				throw new SynchronizationLockException("Object is not synchronized");
			}
			return Monitor.Monitor_wait(obj, millisecondsTimeout);
		}

		/// <summary>Releases the lock on an object and blocks the current thread until it reacquires the lock. If the specified time-out interval elapses, the thread enters the ready queue.</summary>
		/// <returns>true if the lock was reacquired before the specified time elapsed; false if the lock was reacquired after the specified time elapsed. The method does not return until the lock is reacquired.</returns>
		/// <param name="obj">The object on which to wait. </param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> representing the amount of time to wait before the thread enters the ready queue. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.Threading.SynchronizationLockException">The calling thread does not own the lock for the specified object. </exception>
		/// <exception cref="T:System.Threading.ThreadInterruptedException">The thread that invokes Wait is later interrupted from the waiting state. This happens when another thread calls this thread's <see cref="M:System.Threading.Thread.Interrupt" /> method. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <paramref name="timeout" /> parameter in milliseconds is negative and does not represent <see cref="F:System.Threading.Timeout.Infinite" /> (–1 millisecond), or is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool Wait(object obj, TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", "timeout out of range");
			}
			return Monitor.Wait(obj, (int)num);
		}

		/// <summary>Releases the lock on an object and blocks the current thread until it reacquires the lock. If the specified time-out interval elapses, the thread enters the ready queue. This method also specifies whether the synchronization domain for the context (if in a synchronized context) is exited before the wait and reacquired afterward.</summary>
		/// <returns>true if the lock was reacquired before the specified time elapsed; false if the lock was reacquired after the specified time elapsed. The method does not return until the lock is reacquired.</returns>
		/// <param name="obj">The object on which to wait. </param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait before the thread enters the ready queue. </param>
		/// <param name="exitContext">true to exit and reacquire the synchronization domain for the context (if in a synchronized context) before the wait; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.Threading.SynchronizationLockException">Wait is not invoked from within a synchronized block of code. </exception>
		/// <exception cref="T:System.Threading.ThreadInterruptedException">The thread that invokes Wait is later interrupted from the waiting state. This happens when another thread calls this thread's <see cref="M:System.Threading.Thread.Interrupt" /> method. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <paramref name="millisecondsTimeout" /> parameter is negative, and is not equal to <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static bool Wait(object obj, int millisecondsTimeout, bool exitContext)
		{
			bool result;
			try
			{
				if (exitContext)
				{
					SynchronizationAttribute.ExitContext();
				}
				result = Monitor.Wait(obj, millisecondsTimeout);
			}
			finally
			{
				if (exitContext)
				{
					SynchronizationAttribute.EnterContext();
				}
			}
			return result;
		}

		/// <summary>Releases the lock on an object and blocks the current thread until it reacquires the lock. If the specified time-out interval elapses, the thread enters the ready queue. Optionally exits the synchronization domain for the synchronized context before the wait and reacquires the domain afterward.</summary>
		/// <returns>true if the lock was reacquired before the specified time elapsed; false if the lock was reacquired after the specified time elapsed. The method does not return until the lock is reacquired.</returns>
		/// <param name="obj">The object on which to wait. </param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> representing the amount of time to wait before the thread enters the ready queue. </param>
		/// <param name="exitContext">true to exit and reacquire the synchronization domain for the context (if in a synchronized context) before the wait; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.Threading.SynchronizationLockException">Wait is not invoked from within a synchronized block of code. </exception>
		/// <exception cref="T:System.Threading.ThreadInterruptedException">The thread that invokes Wait is later interrupted from the waiting state. This happens when another thread calls this thread's <see cref="M:System.Threading.Thread.Interrupt" /> method. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="timeout" /> parameter is negative and does not represent <see cref="F:System.Threading.Timeout.Infinite" /> (–1 millisecond), or is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool Wait(object obj, TimeSpan timeout, bool exitContext)
		{
			bool result;
			try
			{
				if (exitContext)
				{
					SynchronizationAttribute.ExitContext();
				}
				result = Monitor.Wait(obj, timeout);
			}
			finally
			{
				if (exitContext)
				{
					SynchronizationAttribute.EnterContext();
				}
			}
			return result;
		}
	}
}
