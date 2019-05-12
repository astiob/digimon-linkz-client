using System;
using System.Collections;
using System.Threading;

namespace System.Diagnostics
{
	/// <summary>Provides trace event data specific to a thread and a process.</summary>
	/// <filterpriority>2</filterpriority>
	public class TraceEventCache
	{
		private DateTime started;

		private CorrelationManager manager;

		private string callstack;

		private string thread;

		private int process;

		private long timestamp;

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.TraceEventCache" /> class. </summary>
		public TraceEventCache()
		{
			this.started = DateTime.Now;
			this.manager = Trace.CorrelationManager;
			this.callstack = Environment.StackTrace;
			this.timestamp = Stopwatch.GetTimestamp();
			this.thread = Thread.CurrentThread.Name;
			this.process = Process.GetCurrentProcess().Id;
		}

		/// <summary>Gets the call stack for the current thread.</summary>
		/// <returns>A string containing stack trace information. This value can be an empty string ("").</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" />
		/// </PermissionSet>
		public string Callstack
		{
			get
			{
				return this.callstack;
			}
		}

		/// <summary>Gets the date and time at which the event trace occurred.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> structure whose value is a date and time expressed in Coordinated Universal Time (UTC).</returns>
		/// <filterpriority>2</filterpriority>
		public DateTime DateTime
		{
			get
			{
				return this.started;
			}
		}

		/// <summary>Gets the correlation data, contained in a stack. </summary>
		/// <returns>A <see cref="T:System.Collections.Stack" /> containing correlation data.</returns>
		/// <filterpriority>1</filterpriority>
		public Stack LogicalOperationStack
		{
			get
			{
				return this.manager.LogicalOperationStack;
			}
		}

		/// <summary>Gets the unique identifier of the current process.</summary>
		/// <returns>The system-generated unique identifier of the current process.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public int ProcessId
		{
			get
			{
				return this.process;
			}
		}

		/// <summary>Gets a unique identifier for the current managed thread.  </summary>
		/// <returns>A string that represents a unique integer identifier for this managed thread.</returns>
		/// <filterpriority>2</filterpriority>
		public string ThreadId
		{
			get
			{
				return this.thread;
			}
		}

		/// <summary>Gets the current number of ticks in the timer mechanism.</summary>
		/// <returns>The tick counter value of the underlying timer mechanism.</returns>
		/// <filterpriority>2</filterpriority>
		public long Timestamp
		{
			get
			{
				return this.timestamp;
			}
		}
	}
}
