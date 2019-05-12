using System;
using System.Collections;

namespace System.IO
{
	internal class KeventData
	{
		public FileSystemWatcher FSW;

		public string Directory;

		public string FileMask;

		public bool IncludeSubdirs;

		public bool Enabled;

		public Hashtable DirEntries;

		public kevent ev;
	}
}
