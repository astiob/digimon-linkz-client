using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.IO
{
	internal class KeventWatcher : IFileWatcher
	{
		private static bool failed;

		private static KeventWatcher instance;

		private static Hashtable watches;

		private static Hashtable requests;

		private static Thread thread;

		private static int conn;

		private static bool stop;

		private KeventWatcher()
		{
		}

		public static bool GetInstance(out IFileWatcher watcher)
		{
			if (KeventWatcher.failed)
			{
				watcher = null;
				return false;
			}
			if (KeventWatcher.instance != null)
			{
				watcher = KeventWatcher.instance;
				return true;
			}
			KeventWatcher.watches = Hashtable.Synchronized(new Hashtable());
			KeventWatcher.requests = Hashtable.Synchronized(new Hashtable());
			KeventWatcher.conn = KeventWatcher.kqueue();
			if (KeventWatcher.conn == -1)
			{
				KeventWatcher.failed = true;
				watcher = null;
				return false;
			}
			KeventWatcher.instance = new KeventWatcher();
			watcher = KeventWatcher.instance;
			return true;
		}

		public void StartDispatching(FileSystemWatcher fsw)
		{
			KeventData keventData;
			lock (this)
			{
				if (KeventWatcher.thread == null)
				{
					KeventWatcher.thread = new Thread(new ThreadStart(this.Monitor));
					KeventWatcher.thread.IsBackground = true;
					KeventWatcher.thread.Start();
				}
				keventData = (KeventData)KeventWatcher.watches[fsw];
			}
			if (keventData == null)
			{
				keventData = new KeventData();
				keventData.FSW = fsw;
				keventData.Directory = fsw.FullPath;
				keventData.FileMask = fsw.MangledFilter;
				keventData.IncludeSubdirs = fsw.IncludeSubdirectories;
				keventData.Enabled = true;
				lock (this)
				{
					KeventWatcher.StartMonitoringDirectory(keventData);
					KeventWatcher.watches[fsw] = keventData;
					KeventWatcher.stop = false;
				}
			}
		}

		private static void StartMonitoringDirectory(KeventData data)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(data.Directory);
			if (data.DirEntries == null)
			{
				data.DirEntries = new Hashtable();
				foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
				{
					data.DirEntries.Add(fileSystemInfo.FullName, new KeventFileData(fileSystemInfo, fileSystemInfo.LastAccessTime, fileSystemInfo.LastWriteTime));
				}
			}
			int num = KeventWatcher.open(data.Directory, 0, 0);
			kevent ev = default(kevent);
			ev.udata = IntPtr.Zero;
			timespec timespec = default(timespec);
			timespec.tv_sec = 0;
			timespec.tv_usec = 0;
			if (num > 0)
			{
				ev.ident = num;
				ev.filter = -4;
				ev.flags = 21;
				ev.fflags = 31u;
				ev.data = 0;
				ev.udata = Marshal.StringToHGlobalAuto(data.Directory);
				kevent kevent = default(kevent);
				kevent.udata = IntPtr.Zero;
				KeventWatcher.kevent(KeventWatcher.conn, ref ev, 1, ref kevent, 0, ref timespec);
				data.ev = ev;
				KeventWatcher.requests[num] = data;
			}
			if (!data.IncludeSubdirs)
			{
				return;
			}
		}

		public void StopDispatching(FileSystemWatcher fsw)
		{
			lock (this)
			{
				KeventData keventData = (KeventData)KeventWatcher.watches[fsw];
				if (keventData != null)
				{
					KeventWatcher.StopMonitoringDirectory(keventData);
					KeventWatcher.watches.Remove(fsw);
					if (KeventWatcher.watches.Count == 0)
					{
						KeventWatcher.stop = true;
					}
					if (!keventData.IncludeSubdirs)
					{
					}
				}
			}
		}

		private static void StopMonitoringDirectory(KeventData data)
		{
			KeventWatcher.close(data.ev.ident);
		}

		private void Monitor()
		{
			while (!KeventWatcher.stop)
			{
				kevent ev = default(kevent);
				ev.udata = IntPtr.Zero;
				kevent kevent = default(kevent);
				kevent.udata = IntPtr.Zero;
				timespec timespec = default(timespec);
				timespec.tv_sec = 0;
				timespec.tv_usec = 0;
				int num;
				lock (this)
				{
					num = KeventWatcher.kevent(KeventWatcher.conn, ref kevent, 0, ref ev, 1, ref timespec);
				}
				if (num > 0)
				{
					KeventData data = (KeventData)KeventWatcher.requests[ev.ident];
					KeventWatcher.StopMonitoringDirectory(data);
					KeventWatcher.StartMonitoringDirectory(data);
					this.ProcessEvent(ev);
				}
				else
				{
					Thread.Sleep(500);
				}
			}
			lock (this)
			{
				KeventWatcher.thread = null;
				KeventWatcher.stop = false;
			}
		}

		private void ProcessEvent(kevent ev)
		{
			lock (this)
			{
				KeventData keventData = (KeventData)KeventWatcher.requests[ev.ident];
				if (keventData.Enabled)
				{
					string text = string.Empty;
					FileSystemWatcher fsw = keventData.FSW;
					DirectoryInfo directoryInfo = new DirectoryInfo(keventData.Directory);
					FileSystemInfo changedFsi = null;
					try
					{
						foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
						{
							if (keventData.DirEntries.ContainsKey(fileSystemInfo.FullName) && fileSystemInfo is FileInfo)
							{
								KeventFileData keventFileData = (KeventFileData)keventData.DirEntries[fileSystemInfo.FullName];
								if (keventFileData.LastWriteTime != fileSystemInfo.LastWriteTime)
								{
									text = fileSystemInfo.Name;
									FileAction fa = FileAction.Modified;
									keventData.DirEntries[fileSystemInfo.FullName] = new KeventFileData(fileSystemInfo, fileSystemInfo.LastAccessTime, fileSystemInfo.LastWriteTime);
									if (fsw.IncludeSubdirectories && fileSystemInfo is DirectoryInfo)
									{
										keventData.Directory = text;
										KeventWatcher.requests[ev.ident] = keventData;
										this.ProcessEvent(ev);
									}
									this.PostEvent(text, fsw, fa, changedFsi);
								}
							}
						}
					}
					catch (Exception)
					{
					}
					try
					{
						bool flag = true;
						while (flag)
						{
							foreach (object obj in keventData.DirEntries.Values)
							{
								KeventFileData keventFileData2 = (KeventFileData)obj;
								if (!File.Exists(keventFileData2.fsi.FullName) && !Directory.Exists(keventFileData2.fsi.FullName))
								{
									text = keventFileData2.fsi.Name;
									FileAction fa = FileAction.Removed;
									keventData.DirEntries.Remove(keventFileData2.fsi.FullName);
									this.PostEvent(text, fsw, fa, changedFsi);
									break;
								}
							}
							flag = false;
						}
					}
					catch (Exception)
					{
					}
					try
					{
						foreach (FileSystemInfo fileSystemInfo2 in directoryInfo.GetFileSystemInfos())
						{
							if (!keventData.DirEntries.ContainsKey(fileSystemInfo2.FullName))
							{
								changedFsi = fileSystemInfo2;
								text = fileSystemInfo2.Name;
								FileAction fa = FileAction.Added;
								keventData.DirEntries[fileSystemInfo2.FullName] = new KeventFileData(fileSystemInfo2, fileSystemInfo2.LastAccessTime, fileSystemInfo2.LastWriteTime);
								this.PostEvent(text, fsw, fa, changedFsi);
							}
						}
					}
					catch (Exception)
					{
					}
				}
			}
		}

		private void PostEvent(string filename, FileSystemWatcher fsw, FileAction fa, FileSystemInfo changedFsi)
		{
			RenamedEventArgs renamedEventArgs = null;
			if (fa == (FileAction)0)
			{
				return;
			}
			if (fsw.IncludeSubdirectories && fa == FileAction.Added && changedFsi is DirectoryInfo)
			{
				KeventData keventData = new KeventData();
				keventData.FSW = fsw;
				keventData.Directory = changedFsi.FullName;
				keventData.FileMask = fsw.MangledFilter;
				keventData.IncludeSubdirs = fsw.IncludeSubdirectories;
				keventData.Enabled = true;
				lock (this)
				{
					KeventWatcher.StartMonitoringDirectory(keventData);
				}
			}
			if (!fsw.Pattern.IsMatch(filename, true))
			{
				return;
			}
			lock (fsw)
			{
				fsw.DispatchEvents(fa, filename, ref renamedEventArgs);
				if (fsw.Waiting)
				{
					fsw.Waiting = false;
					System.Threading.Monitor.PulseAll(fsw);
				}
			}
		}

		[DllImport("libc")]
		private static extern int open(string path, int flags, int mode_t);

		[DllImport("libc")]
		private static extern int close(int fd);

		[DllImport("libc")]
		private static extern int kqueue();

		[DllImport("libc")]
		private static extern int kevent(int kqueue, ref kevent ev, int nchanges, ref kevent evtlist, int nevents, ref timespec ts);
	}
}
