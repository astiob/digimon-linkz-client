using System;

namespace System.IO
{
	/// <summary>Contains information on the change that occurred.</summary>
	/// <filterpriority>2</filterpriority>
	public struct WaitForChangedResult
	{
		private WatcherChangeTypes changeType;

		private string name;

		private string oldName;

		private bool timedOut;

		/// <summary>Gets or sets the type of change that occurred.</summary>
		/// <returns>One of the <see cref="T:System.IO.WatcherChangeTypes" /> values.</returns>
		/// <filterpriority>2</filterpriority>
		public WatcherChangeTypes ChangeType
		{
			get
			{
				return this.changeType;
			}
			set
			{
				this.changeType = value;
			}
		}

		/// <summary>Gets or sets the name of the file or directory that changed.</summary>
		/// <returns>The name of the file or directory that changed.</returns>
		/// <filterpriority>2</filterpriority>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		/// <summary>Gets or sets the original name of the file or directory that was renamed.</summary>
		/// <returns>The original name of the file or directory that was renamed.</returns>
		/// <filterpriority>2</filterpriority>
		public string OldName
		{
			get
			{
				return this.oldName;
			}
			set
			{
				this.oldName = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the wait operation timed out.</summary>
		/// <returns>true if the <see cref="M:System.IO.FileSystemWatcher.WaitForChanged(System.IO.WatcherChangeTypes)" /> method timed out; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		public bool TimedOut
		{
			get
			{
				return this.timedOut;
			}
			set
			{
				this.timedOut = value;
			}
		}
	}
}
