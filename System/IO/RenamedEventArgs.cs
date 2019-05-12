using System;

namespace System.IO
{
	/// <summary>Provides data for the <see cref="E:System.IO.FileSystemWatcher.Renamed" /> event.</summary>
	/// <filterpriority>2</filterpriority>
	public class RenamedEventArgs : FileSystemEventArgs
	{
		private string oldName;

		private string oldFullPath;

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.RenamedEventArgs" /> class.</summary>
		/// <param name="changeType">One of the <see cref="T:System.IO.WatcherChangeTypes" /> values. </param>
		/// <param name="directory">The name of the affected file or directory. </param>
		/// <param name="name">The name of the affected file or directory. </param>
		/// <param name="oldName">The old name of the affected file or directory. </param>
		public RenamedEventArgs(WatcherChangeTypes changeType, string directory, string name, string oldName) : base(changeType, directory, name)
		{
			this.oldName = oldName;
			this.oldFullPath = Path.Combine(directory, oldName);
		}

		/// <summary>Gets the previous fully qualified path of the affected file or directory.</summary>
		/// <returns>The previous fully qualified path of the affected file or directory.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public string OldFullPath
		{
			get
			{
				return this.oldFullPath;
			}
		}

		/// <summary>Gets the old name of the affected file or directory.</summary>
		/// <returns>The previous name of the affected file or directory.</returns>
		/// <filterpriority>2</filterpriority>
		public string OldName
		{
			get
			{
				return this.oldName;
			}
		}
	}
}
