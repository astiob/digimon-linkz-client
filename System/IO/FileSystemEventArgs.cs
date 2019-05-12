using System;

namespace System.IO
{
	/// <summary>Provides data for the directory events: <see cref="E:System.IO.FileSystemWatcher.Changed" />, <see cref="E:System.IO.FileSystemWatcher.Created" />, <see cref="E:System.IO.FileSystemWatcher.Deleted" />.</summary>
	/// <filterpriority>2</filterpriority>
	public class FileSystemEventArgs : EventArgs
	{
		private WatcherChangeTypes changeType;

		private string directory;

		private string name;

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.FileSystemEventArgs" /> class.</summary>
		/// <param name="changeType">One of the <see cref="T:System.IO.WatcherChangeTypes" /> values, which represents the kind of change detected in the file system. </param>
		/// <param name="directory">The root directory of the affected file or directory. </param>
		/// <param name="name">The name of the affected file or directory. </param>
		public FileSystemEventArgs(WatcherChangeTypes changeType, string directory, string name)
		{
			this.changeType = changeType;
			this.directory = directory;
			this.name = name;
		}

		internal void SetName(string name)
		{
			this.name = name;
		}

		/// <summary>Gets the type of directory event that occurred.</summary>
		/// <returns>One of the <see cref="T:System.IO.WatcherChangeTypes" /> values that represents the kind of change detected in the file system.</returns>
		/// <filterpriority>2</filterpriority>
		public WatcherChangeTypes ChangeType
		{
			get
			{
				return this.changeType;
			}
		}

		/// <summary>Gets the fully qualifed path of the affected file or directory.</summary>
		/// <returns>The path of the affected file or directory.</returns>
		/// <filterpriority>2</filterpriority>
		public string FullPath
		{
			get
			{
				return Path.Combine(this.directory, this.name);
			}
		}

		/// <summary>Gets the name of the affected file or directory.</summary>
		/// <returns>The name of the affected file or directory.</returns>
		/// <filterpriority>2</filterpriority>
		public string Name
		{
			get
			{
				return this.name;
			}
		}
	}
}
