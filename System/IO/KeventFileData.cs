using System;

namespace System.IO
{
	internal class KeventFileData
	{
		public FileSystemInfo fsi;

		public DateTime LastAccessTime;

		public DateTime LastWriteTime;

		public KeventFileData(FileSystemInfo fsi, DateTime LastAccessTime, DateTime LastWriteTime)
		{
			this.fsi = fsi;
			this.LastAccessTime = LastAccessTime;
			this.LastWriteTime = LastWriteTime;
		}
	}
}
