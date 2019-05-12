using System;
using System.Collections;
using System.Threading;

namespace System.IO
{
	internal class DefaultWatcher : IFileWatcher
	{
		private static DefaultWatcher instance;

		private static Thread thread;

		private static Hashtable watches;

		private static string[] NoStringsArray = new string[0];

		private DefaultWatcher()
		{
		}

		public static bool GetInstance(out IFileWatcher watcher)
		{
			if (DefaultWatcher.instance != null)
			{
				watcher = DefaultWatcher.instance;
				return true;
			}
			DefaultWatcher.instance = new DefaultWatcher();
			watcher = DefaultWatcher.instance;
			return true;
		}

		public void StartDispatching(FileSystemWatcher fsw)
		{
			lock (this)
			{
				if (DefaultWatcher.watches == null)
				{
					DefaultWatcher.watches = new Hashtable();
				}
				if (DefaultWatcher.thread == null)
				{
					DefaultWatcher.thread = new Thread(new ThreadStart(this.Monitor));
					DefaultWatcher.thread.IsBackground = true;
					DefaultWatcher.thread.Start();
				}
			}
			Hashtable obj = DefaultWatcher.watches;
			lock (obj)
			{
				DefaultWatcherData defaultWatcherData = (DefaultWatcherData)DefaultWatcher.watches[fsw];
				if (defaultWatcherData == null)
				{
					defaultWatcherData = new DefaultWatcherData();
					defaultWatcherData.Files = new Hashtable();
					DefaultWatcher.watches[fsw] = defaultWatcherData;
				}
				defaultWatcherData.FSW = fsw;
				defaultWatcherData.Directory = fsw.FullPath;
				defaultWatcherData.NoWildcards = !fsw.Pattern.HasWildcard;
				if (defaultWatcherData.NoWildcards)
				{
					defaultWatcherData.FileMask = Path.Combine(defaultWatcherData.Directory, fsw.MangledFilter);
				}
				else
				{
					defaultWatcherData.FileMask = fsw.MangledFilter;
				}
				defaultWatcherData.IncludeSubdirs = fsw.IncludeSubdirectories;
				defaultWatcherData.Enabled = true;
				defaultWatcherData.DisabledTime = DateTime.MaxValue;
				this.UpdateDataAndDispatch(defaultWatcherData, false);
			}
		}

		public void StopDispatching(FileSystemWatcher fsw)
		{
			lock (this)
			{
				if (DefaultWatcher.watches == null)
				{
					return;
				}
			}
			Hashtable obj = DefaultWatcher.watches;
			lock (obj)
			{
				DefaultWatcherData defaultWatcherData = (DefaultWatcherData)DefaultWatcher.watches[fsw];
				if (defaultWatcherData != null)
				{
					defaultWatcherData.Enabled = false;
					defaultWatcherData.DisabledTime = DateTime.Now;
				}
			}
		}

		private void Monitor()
		{
			int num = 0;
			for (;;)
			{
				Thread.Sleep(750);
				Hashtable obj = DefaultWatcher.watches;
				Hashtable hashtable;
				lock (obj)
				{
					if (DefaultWatcher.watches.Count == 0)
					{
						if (++num == 20)
						{
							break;
						}
						continue;
					}
					else
					{
						hashtable = (Hashtable)DefaultWatcher.watches.Clone();
					}
				}
				if (hashtable.Count != 0)
				{
					num = 0;
					foreach (object obj2 in hashtable.Values)
					{
						DefaultWatcherData defaultWatcherData = (DefaultWatcherData)obj2;
						bool flag = this.UpdateDataAndDispatch(defaultWatcherData, true);
						if (flag)
						{
							Hashtable obj3 = DefaultWatcher.watches;
							lock (obj3)
							{
								DefaultWatcher.watches.Remove(defaultWatcherData.FSW);
							}
						}
					}
				}
			}
			lock (this)
			{
				DefaultWatcher.thread = null;
			}
		}

		private bool UpdateDataAndDispatch(DefaultWatcherData data, bool dispatch)
		{
			if (!data.Enabled)
			{
				return data.DisabledTime != DateTime.MaxValue && (DateTime.Now - data.DisabledTime).TotalSeconds > 5.0;
			}
			this.DoFiles(data, data.Directory, dispatch);
			return false;
		}

		private static void DispatchEvents(FileSystemWatcher fsw, FileAction action, string filename)
		{
			RenamedEventArgs renamedEventArgs = null;
			lock (fsw)
			{
				fsw.DispatchEvents(action, filename, ref renamedEventArgs);
				if (fsw.Waiting)
				{
					fsw.Waiting = false;
					System.Threading.Monitor.PulseAll(fsw);
				}
			}
		}

		private void DoFiles(DefaultWatcherData data, string directory, bool dispatch)
		{
			bool flag = Directory.Exists(directory);
			if (flag && data.IncludeSubdirs)
			{
				foreach (string directory2 in Directory.GetDirectories(directory))
				{
					this.DoFiles(data, directory2, dispatch);
				}
			}
			string[] array = null;
			if (!flag)
			{
				array = DefaultWatcher.NoStringsArray;
			}
			else if (!data.NoWildcards)
			{
				array = Directory.GetFileSystemEntries(directory, data.FileMask);
			}
			else if (File.Exists(data.FileMask) || Directory.Exists(data.FileMask))
			{
				array = new string[]
				{
					data.FileMask
				};
			}
			else
			{
				array = DefaultWatcher.NoStringsArray;
			}
			foreach (object obj in data.Files.Keys)
			{
				string key = (string)obj;
				FileData fileData = (FileData)data.Files[key];
				if (fileData.Directory == directory)
				{
					fileData.NotExists = true;
				}
			}
			foreach (string text in array)
			{
				FileData fileData2 = (FileData)data.Files[text];
				if (fileData2 == null)
				{
					try
					{
						data.Files.Add(text, DefaultWatcher.CreateFileData(directory, text));
					}
					catch
					{
						data.Files.Remove(text);
						goto IL_1BD;
					}
					if (dispatch)
					{
						DefaultWatcher.DispatchEvents(data.FSW, FileAction.Added, text);
					}
				}
				else if (fileData2.Directory == directory)
				{
					fileData2.NotExists = false;
				}
				IL_1BD:;
			}
			if (!dispatch)
			{
				return;
			}
			ArrayList arrayList = null;
			foreach (object obj2 in data.Files.Keys)
			{
				string text2 = (string)obj2;
				FileData fileData3 = (FileData)data.Files[text2];
				if (fileData3.NotExists)
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add(text2);
					DefaultWatcher.DispatchEvents(data.FSW, FileAction.Removed, text2);
				}
			}
			if (arrayList != null)
			{
				foreach (object obj3 in arrayList)
				{
					string key2 = (string)obj3;
					data.Files.Remove(key2);
				}
				arrayList = null;
			}
			foreach (object obj4 in data.Files.Keys)
			{
				string text3 = (string)obj4;
				FileData fileData4 = (FileData)data.Files[text3];
				DateTime creationTime;
				DateTime lastWriteTime;
				try
				{
					creationTime = File.GetCreationTime(text3);
					lastWriteTime = File.GetLastWriteTime(text3);
				}
				catch
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add(text3);
					DefaultWatcher.DispatchEvents(data.FSW, FileAction.Removed, text3);
					continue;
				}
				if (creationTime != fileData4.CreationTime || lastWriteTime != fileData4.LastWriteTime)
				{
					fileData4.CreationTime = creationTime;
					fileData4.LastWriteTime = lastWriteTime;
					DefaultWatcher.DispatchEvents(data.FSW, FileAction.Modified, text3);
				}
			}
			if (arrayList != null)
			{
				foreach (object obj5 in arrayList)
				{
					string key3 = (string)obj5;
					data.Files.Remove(key3);
				}
			}
		}

		private static FileData CreateFileData(string directory, string filename)
		{
			FileData fileData = new FileData();
			string path = Path.Combine(directory, filename);
			fileData.Directory = directory;
			fileData.Attributes = File.GetAttributes(path);
			fileData.CreationTime = File.GetCreationTime(path);
			fileData.LastWriteTime = File.GetLastWriteTime(path);
			return fileData;
		}
	}
}
