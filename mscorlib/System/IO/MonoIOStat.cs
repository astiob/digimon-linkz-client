using System;

namespace System.IO
{
	internal struct MonoIOStat
	{
		public string Name;

		public FileAttributes Attributes;

		public long Length;

		public long CreationTime;

		public long LastAccessTime;

		public long LastWriteTime;
	}
}
