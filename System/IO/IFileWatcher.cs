using System;

namespace System.IO
{
	internal interface IFileWatcher
	{
		void StartDispatching(FileSystemWatcher fsw);

		void StopDispatching(FileSystemWatcher fsw);
	}
}
