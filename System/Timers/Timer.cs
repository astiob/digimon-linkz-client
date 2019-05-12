using System;
using System.ComponentModel;
using System.Threading;

namespace System.Timers
{
	/// <summary>Generates recurring events in an application.</summary>
	[System.ComponentModel.DefaultProperty("Interval")]
	[System.ComponentModel.DefaultEvent("Elapsed")]
	public class Timer : System.ComponentModel.Component, System.ComponentModel.ISupportInitialize
	{
		private double interval;

		private bool autoReset;

		private Timer timer;

		private object _lock = new object();

		private System.ComponentModel.ISynchronizeInvoke so;

		/// <summary>Initializes a new instance of the <see cref="T:System.Timers.Timer" /> class, and sets all the properties to their initial values.</summary>
		public Timer() : this(100.0)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Timers.Timer" /> class, and sets the <see cref="P:System.Timers.Timer.Interval" /> property to the specified number of milliseconds.</summary>
		/// <param name="interval">The time, in milliseconds, between events. The value must be greater than zero and less than or equal to <see cref="F:System.Int32.MaxValue" />.</param>
		/// <exception cref="T:System.ArgumentException">The value of the <paramref name="interval" /> parameter is less than or equal to zero, or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		public Timer(double interval)
		{
			if (interval > 2147483647.0)
			{
				throw new ArgumentException("Invalid value: " + interval, "interval");
			}
			this.autoReset = true;
			this.Interval = interval;
		}

		/// <summary>Occurs when the interval elapses.</summary>
		[TimersDescription("Occurs when the Interval has elapsed.")]
		[System.ComponentModel.Category("Behavior")]
		public event ElapsedEventHandler Elapsed;

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Timers.Timer" /> should raise the <see cref="E:System.Timers.Timer.Elapsed" /> event each time the specified interval elapses or only after the first time it elapses.</summary>
		/// <returns>true if the <see cref="T:System.Timers.Timer" /> should raise the <see cref="E:System.Timers.Timer.Elapsed" /> event each time the interval elapses; false if it should raise the <see cref="E:System.Timers.Timer.Elapsed" /> event only once, after the first time the interval elapses. The default is true.</returns>
		[System.ComponentModel.Category("Behavior")]
		[TimersDescription("Indicates whether the timer will be restarted when it is enabled.")]
		[System.ComponentModel.DefaultValue(true)]
		public bool AutoReset
		{
			get
			{
				return this.autoReset;
			}
			set
			{
				this.autoReset = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Timers.Timer" /> should raise the <see cref="E:System.Timers.Timer.Elapsed" /> event.</summary>
		/// <returns>true if the <see cref="T:System.Timers.Timer" /> should raise the <see cref="E:System.Timers.Timer.Elapsed" /> event; otherwise, false. The default is false.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This property cannot be set because the timer has been disposed.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Timers.Timer.Interval" /> property was set to a value greater than <see cref="F:System.Int32.MaxValue" /> before the timer was enabled.  </exception>
		[TimersDescription("Indicates whether the timer is enabled to fire events at a defined interval.")]
		[System.ComponentModel.DefaultValue(false)]
		[System.ComponentModel.Category("Behavior")]
		public bool Enabled
		{
			get
			{
				object @lock = this._lock;
				bool result;
				lock (@lock)
				{
					result = (this.timer != null);
				}
				return result;
			}
			set
			{
				object @lock = this._lock;
				lock (@lock)
				{
					bool flag = this.timer != null;
					if (flag != value)
					{
						if (value)
						{
							this.timer = new Timer(new TimerCallback(Timer.Callback), this, (int)this.interval, (!this.autoReset) ? 0 : ((int)this.interval));
						}
						else
						{
							this.timer.Dispose();
							this.timer = null;
						}
					}
				}
			}
		}

		/// <summary>Gets or sets the interval at which to raise the <see cref="E:System.Timers.Timer.Elapsed" /> event.</summary>
		/// <returns>The time, in milliseconds, between <see cref="E:System.Timers.Timer.Elapsed" /> events. The value must be greater than zero, and less than or equal to <see cref="F:System.Int32.MaxValue" />. The default is 100 milliseconds.</returns>
		/// <exception cref="T:System.ArgumentException">The interval is less than or equal to zero.-or-The interval is greater than <see cref="F:System.Int32.MaxValue" />, and the timer is currently enabled. (If the timer is not currently enabled, no exception is thrown until it becomes enabled.)  </exception>
		[TimersDescription("The number of milliseconds between timer events.")]
		[System.ComponentModel.Category("Behavior")]
		[System.ComponentModel.DefaultValue(100)]
		[System.ComponentModel.RecommendedAsConfigurable(true)]
		public double Interval
		{
			get
			{
				return this.interval;
			}
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentException("Invalid value: " + value);
				}
				object @lock = this._lock;
				lock (@lock)
				{
					this.interval = value;
					if (this.timer != null)
					{
						this.timer.Change((int)this.interval, (!this.autoReset) ? 0 : ((int)this.interval));
					}
				}
			}
		}

		/// <summary>Gets or sets the site that binds the <see cref="T:System.Timers.Timer" /> to its container in design mode.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.ISite" /> interface representing the site that binds the <see cref="T:System.Timers.Timer" /> object to its container.</returns>
		public override System.ComponentModel.ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				base.Site = value;
			}
		}

		/// <summary>Gets or sets the object used to marshal event-handler calls that are issued when an interval has elapsed.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ISynchronizeInvoke" /> representing the object used to marshal the event-handler calls that are issued when an interval has elapsed. The default is null.</returns>
		[TimersDescription("The object used to marshal the event handler calls issued when an interval has elapsed.")]
		[System.ComponentModel.DefaultValue(null)]
		[System.ComponentModel.Browsable(false)]
		public System.ComponentModel.ISynchronizeInvoke SynchronizingObject
		{
			get
			{
				return this.so;
			}
			set
			{
				this.so = value;
			}
		}

		/// <summary>Begins the run-time initialization of a <see cref="T:System.Timers.Timer" /> that is used on a form or by another component.</summary>
		public void BeginInit()
		{
		}

		/// <summary>Releases the resources used by the <see cref="T:System.Timers.Timer" />.</summary>
		public void Close()
		{
			this.Enabled = false;
		}

		/// <summary>Ends the run-time initialization of a <see cref="T:System.Timers.Timer" /> that is used on a form or by another component.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void EndInit()
		{
		}

		/// <summary>Starts raising the <see cref="E:System.Timers.Timer.Elapsed" /> event by setting <see cref="P:System.Timers.Timer.Enabled" /> to true.</summary>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <see cref="T:System.Timers.Timer" /> is created with an interval equal to or greater than <see cref="F:System.Int32.MaxValue" /> + 1, or set to an interval less than zero.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void Start()
		{
			this.Enabled = true;
		}

		/// <summary>Stops raising the <see cref="E:System.Timers.Timer.Elapsed" /> event by setting <see cref="P:System.Timers.Timer.Enabled" /> to false.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void Stop()
		{
			this.Enabled = false;
		}

		/// <summary>Releases all resources used by the current <see cref="T:System.Timers.Timer" />.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected override void Dispose(bool disposing)
		{
			this.Close();
			base.Dispose(disposing);
		}

		private static void Callback(object state)
		{
			Timer timer = (Timer)state;
			if (!timer.Enabled)
			{
				return;
			}
			ElapsedEventHandler elapsed = timer.Elapsed;
			if (!timer.autoReset)
			{
				timer.Enabled = false;
			}
			if (elapsed == null)
			{
				return;
			}
			ElapsedEventArgs elapsedEventArgs = new ElapsedEventArgs(DateTime.Now);
			if (timer.so != null && timer.so.InvokeRequired)
			{
				timer.so.BeginInvoke(elapsed, new object[]
				{
					timer,
					elapsedEventArgs
				});
			}
			else
			{
				try
				{
					elapsed(timer, elapsedEventArgs);
				}
				catch
				{
				}
			}
		}
	}
}
