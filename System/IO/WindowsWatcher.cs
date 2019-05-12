using System;

namespace System.IO
{
	internal class WindowsWatcher : IFileWatcher
	{
		private WindowsWatcher()
		{
		}

		public static bool GetInstance(out IFileWatcher watcher)
		{
			throw new NotSupportedException();
		}

		public void StartDispatching(FileSystemWatcher fsw)
		{
		}

		public void StopDispatching(FileSystemWatcher fsw)
		{
		}
	}
}
