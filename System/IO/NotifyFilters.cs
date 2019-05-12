using System;

namespace System.IO
{
	/// <summary>Specifies changes to watch for in a file or folder.</summary>
	/// <filterpriority>2</filterpriority>
	[Flags]
	public enum NotifyFilters
	{
		/// <summary>The attributes of the file or folder.</summary>
		Attributes = 4,
		/// <summary>The time the file or folder was created.</summary>
		CreationTime = 64,
		/// <summary>The name of the directory.</summary>
		DirectoryName = 2,
		/// <summary>The name of the file.</summary>
		FileName = 1,
		/// <summary>The date the file or folder was last opened.</summary>
		LastAccess = 32,
		/// <summary>The date the file or folder last had anything written to it.</summary>
		LastWrite = 16,
		/// <summary>The security settings of the file or folder.</summary>
		Security = 256,
		/// <summary>The size of the file or folder.</summary>
		Size = 8
	}
}
