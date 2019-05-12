using System;
using System.Runtime.CompilerServices;

namespace System.Diagnostics
{
	/// <summary>Provides a set of methods and properties that you can use to accurately measure elapsed time.</summary>
	/// <filterpriority>1</filterpriority>
	public class Stopwatch
	{
		/// <summary>Gets the frequency of the timer as the number of ticks per second. This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly long Frequency = 10000000L;

		/// <summary>Indicates whether the timer is based on a high-resolution performance counter. This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly bool IsHighResolution = true;

		private long elapsed;

		private long started;

		private bool is_running;

		/// <summary>Gets the current number of ticks in the timer mechanism.</summary>
		/// <returns>A long integer representing the tick counter value of the underlying timer mechanism.</returns>
		/// <filterpriority>1</filterpriority>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long GetTimestamp();

		/// <summary>Initializes a new <see cref="T:System.Diagnostics.Stopwatch" /> instance, sets the elapsed time property to zero, and starts measuring elapsed time.</summary>
		/// <returns>A <see cref="T:System.Diagnostics.Stopwatch" /> that has just begun measuring elapsed time.</returns>
		/// <filterpriority>1</filterpriority>
		public static Stopwatch StartNew()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			return stopwatch;
		}

		/// <summary>Gets the total elapsed time measured by the current instance.</summary>
		/// <returns>A read-only <see cref="T:System.TimeSpan" /> representing the total elapsed time measured by the current instance.</returns>
		/// <filterpriority>1</filterpriority>
		public TimeSpan Elapsed
		{
			get
			{
				if (Stopwatch.IsHighResolution)
				{
					return TimeSpan.FromTicks(this.ElapsedTicks / (Stopwatch.Frequency / 10000000L));
				}
				return TimeSpan.FromTicks(this.ElapsedTicks);
			}
		}

		/// <summary>Gets the total elapsed time measured by the current instance, in milliseconds.</summary>
		/// <returns>A read-only long integer representing the total number of milliseconds measured by the current instance.</returns>
		/// <filterpriority>1</filterpriority>
		public long ElapsedMilliseconds
		{
			get
			{
				if (Stopwatch.IsHighResolution)
				{
					return this.ElapsedTicks / (Stopwatch.Frequency / 1000L);
				}
				return checked((long)this.Elapsed.TotalMilliseconds);
			}
		}

		/// <summary>Gets the total elapsed time measured by the current instance, in timer ticks.</summary>
		/// <returns>A read-only long integer representing the total number of timer ticks measured by the current instance.</returns>
		/// <filterpriority>1</filterpriority>
		public long ElapsedTicks
		{
			get
			{
				return (!this.is_running) ? this.elapsed : (Stopwatch.GetTimestamp() - this.started + this.elapsed);
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Diagnostics.Stopwatch" /> timer is running.</summary>
		/// <returns>true if the <see cref="T:System.Diagnostics.Stopwatch" /> instance is currently running and measuring elapsed time for an interval; otherwise, false.</returns>
		/// <filterpriority>1</filterpriority>
		public bool IsRunning
		{
			get
			{
				return this.is_running;
			}
		}

		/// <summary>Stops time interval measurement and resets the elapsed time to zero.</summary>
		/// <filterpriority>1</filterpriority>
		public void Reset()
		{
			this.elapsed = 0L;
			this.is_running = false;
		}

		/// <summary>Starts, or resumes, measuring elapsed time for an interval.</summary>
		/// <filterpriority>1</filterpriority>
		public void Start()
		{
			if (this.is_running)
			{
				return;
			}
			this.started = Stopwatch.GetTimestamp();
			this.is_running = true;
		}

		/// <summary>Stops measuring elapsed time for an interval.</summary>
		/// <filterpriority>1</filterpriority>
		public void Stop()
		{
			if (!this.is_running)
			{
				return;
			}
			this.elapsed += Stopwatch.GetTimestamp() - this.started;
			this.is_running = false;
		}
	}
}
