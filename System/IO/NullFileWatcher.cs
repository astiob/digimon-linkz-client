using System;

namespace System.IO
{
	internal class NullFileWatcher : IFileWatcher
	{
		private static IFileWatcher instance;

		public void StartDispatching(FileSystemWatcher fsw)
		{
		}

		public void StopDispatching(FileSystemWatcher fsw)
		{
		}

		public static bool GetInstance(out IFileWatcher watcher)
		{
			if (NullFileWatcher.instance != null)
			{
				watcher = NullFileWatcher.instance;
				return true;
			}
			IFileWatcher fileWatcher;
			watcher = (fileWatcher = new NullFileWatcher());
			NullFileWatcher.instance = fileWatcher;
			return true;
		}
	}
}
