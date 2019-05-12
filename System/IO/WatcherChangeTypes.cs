using System;

namespace System.IO
{
	/// <summary>Changes that might occur to a file or directory.</summary>
	/// <filterpriority>2</filterpriority>
	[Flags]
	public enum WatcherChangeTypes
	{
		/// <summary>The creation, deletion, change, or renaming of a file or folder.</summary>
		All = 15,
		/// <summary>The change of a file or folder. The types of changes include: changes to size, attributes, security settings, last write, and last access time.</summary>
		Changed = 4,
		/// <summary>The creation of a file or folder.</summary>
		Created = 1,
		/// <summary>The deletion of a file or folder.</summary>
		Deleted = 2,
		/// <summary>The renaming of a file or folder.</summary>
		Renamed = 8
	}
}
