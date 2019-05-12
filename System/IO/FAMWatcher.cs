using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.IO
{
	internal class FAMWatcher : IFileWatcher
	{
		private const NotifyFilters changed = NotifyFilters.Attributes | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size;

		private static bool failed;

		private static FAMWatcher instance;

		private static Hashtable watches;

		private static Hashtable requests;

		private static FAMConnection conn;

		private static Thread thread;

		private static bool stop;

		private static bool use_gamin;

		private FAMWatcher()
		{
		}

		public static bool GetInstance(out IFileWatcher watcher, bool gamin)
		{
			if (FAMWatcher.failed)
			{
				watcher = null;
				return false;
			}
			if (FAMWatcher.instance != null)
			{
				watcher = FAMWatcher.instance;
				return true;
			}
			FAMWatcher.use_gamin = gamin;
			FAMWatcher.watches = Hashtable.Synchronized(new Hashtable());
			FAMWatcher.requests = Hashtable.Synchronized(new Hashtable());
			if (FAMWatcher.FAMOpen(out FAMWatcher.conn) == -1)
			{
				FAMWatcher.failed = true;
				watcher = null;
				return false;
			}
			FAMWatcher.instance = new FAMWatcher();
			watcher = FAMWatcher.instance;
			return true;
		}

		public void StartDispatching(FileSystemWatcher fsw)
		{
			FAMData famdata;
			lock (this)
			{
				if (FAMWatcher.thread == null)
				{
					FAMWatcher.thread = new Thread(new ThreadStart(this.Monitor));
					FAMWatcher.thread.IsBackground = true;
					FAMWatcher.thread.Start();
				}
				famdata = (FAMData)FAMWatcher.watches[fsw];
			}
			if (famdata == null)
			{
				famdata = new FAMData();
				famdata.FSW = fsw;
				famdata.Directory = fsw.FullPath;
				famdata.FileMask = fsw.MangledFilter;
				famdata.IncludeSubdirs = fsw.IncludeSubdirectories;
				if (famdata.IncludeSubdirs)
				{
					famdata.SubDirs = new Hashtable();
				}
				famdata.Enabled = true;
				FAMWatcher.StartMonitoringDirectory(famdata, false);
				lock (this)
				{
					FAMWatcher.watches[fsw] = famdata;
					FAMWatcher.requests[famdata.Request.ReqNum] = famdata;
					FAMWatcher.stop = false;
				}
			}
		}

		private static void StartMonitoringDirectory(FAMData data, bool justcreated)
		{
			FAMRequest request;
			if (FAMWatcher.FAMMonitorDirectory(ref FAMWatcher.conn, data.Directory, out request, IntPtr.Zero) == -1)
			{
				throw new System.ComponentModel.Win32Exception();
			}
			FileSystemWatcher fsw = data.FSW;
			data.Request = request;
			if (data.IncludeSubdirs)
			{
				foreach (string text in Directory.GetDirectories(data.Directory))
				{
					FAMData famdata = new FAMData();
					famdata.FSW = data.FSW;
					famdata.Directory = text;
					famdata.FileMask = data.FSW.MangledFilter;
					famdata.IncludeSubdirs = true;
					famdata.SubDirs = new Hashtable();
					famdata.Enabled = true;
					if (justcreated)
					{
						FileSystemWatcher obj = fsw;
						lock (obj)
						{
							RenamedEventArgs renamedEventArgs = null;
							fsw.DispatchEvents(FileAction.Added, text, ref renamedEventArgs);
							if (fsw.Waiting)
							{
								fsw.Waiting = false;
								System.Threading.Monitor.PulseAll(fsw);
							}
						}
					}
					FAMWatcher.StartMonitoringDirectory(famdata, justcreated);
					data.SubDirs[text] = famdata;
					FAMWatcher.requests[famdata.Request.ReqNum] = famdata;
				}
			}
			if (justcreated)
			{
				foreach (string filename in Directory.GetFiles(data.Directory))
				{
					FileSystemWatcher obj2 = fsw;
					lock (obj2)
					{
						RenamedEventArgs renamedEventArgs2 = null;
						fsw.DispatchEvents(FileAction.Added, filename, ref renamedEventArgs2);
						fsw.DispatchEvents(FileAction.Modified, filename, ref renamedEventArgs2);
						if (fsw.Waiting)
						{
							fsw.Waiting = false;
							System.Threading.Monitor.PulseAll(fsw);
						}
					}
				}
			}
		}

		public void StopDispatching(FileSystemWatcher fsw)
		{
			lock (this)
			{
				FAMData famdata = (FAMData)FAMWatcher.watches[fsw];
				if (famdata != null)
				{
					FAMWatcher.StopMonitoringDirectory(famdata);
					FAMWatcher.watches.Remove(fsw);
					FAMWatcher.requests.Remove(famdata.Request.ReqNum);
					if (FAMWatcher.watches.Count == 0)
					{
						FAMWatcher.stop = true;
					}
					if (famdata.IncludeSubdirs)
					{
						foreach (object obj in famdata.SubDirs.Values)
						{
							FAMData famdata2 = (FAMData)obj;
							FAMWatcher.StopMonitoringDirectory(famdata2);
							FAMWatcher.requests.Remove(famdata2.Request.ReqNum);
						}
					}
				}
			}
		}

		private static void StopMonitoringDirectory(FAMData data)
		{
			if (FAMWatcher.FAMCancelMonitor(ref FAMWatcher.conn, ref data.Request) == -1)
			{
				throw new System.ComponentModel.Win32Exception();
			}
		}

		private void Monitor()
		{
			while (!FAMWatcher.stop)
			{
				int num;
				lock (this)
				{
					num = FAMWatcher.FAMPending(ref FAMWatcher.conn);
				}
				if (num > 0)
				{
					this.ProcessEvents();
				}
				else
				{
					Thread.Sleep(500);
				}
			}
			lock (this)
			{
				FAMWatcher.thread = null;
				FAMWatcher.stop = false;
			}
		}

		private void ProcessEvents()
		{
			ArrayList arrayList = null;
			lock (this)
			{
				string text;
				int num;
				int num2;
				while (FAMWatcher.InternalFAMNextEvent(ref FAMWatcher.conn, out text, out num, out num2) == 1)
				{
					bool flag;
					switch (num)
					{
					case 1:
					case 2:
					case 5:
						flag = FAMWatcher.requests.ContainsKey(num2);
						break;
					case 3:
					case 4:
					case 6:
					case 7:
					case 8:
					case 9:
						goto IL_75;
					default:
						goto IL_75;
					}
					IL_7D:
					if (flag)
					{
						FAMData famdata = (FAMData)FAMWatcher.requests[num2];
						if (famdata.Enabled)
						{
							FileSystemWatcher fsw = famdata.FSW;
							NotifyFilters notifyFilter = fsw.NotifyFilter;
							RenamedEventArgs renamedEventArgs = null;
							FileAction fileAction = (FileAction)0;
							if (num == 1 && (notifyFilter & (NotifyFilters.Attributes | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size)) != (NotifyFilters)0)
							{
								fileAction = FileAction.Modified;
							}
							else if (num == 2)
							{
								fileAction = FileAction.Removed;
							}
							else if (num == 5)
							{
								fileAction = FileAction.Added;
							}
							if (fileAction != (FileAction)0)
							{
								if (fsw.IncludeSubdirectories)
								{
									string fullPath = fsw.FullPath;
									string text2 = famdata.Directory;
									if (text2 != fullPath)
									{
										int length = fullPath.Length;
										int num3 = 1;
										if (length > 1 && fullPath[length - 1] == Path.DirectorySeparatorChar)
										{
											num3 = 0;
										}
										string path = text2.Substring(fullPath.Length + num3);
										text2 = Path.Combine(text2, text);
										text = Path.Combine(path, text);
									}
									else
									{
										text2 = Path.Combine(fullPath, text);
									}
									if (fileAction == FileAction.Added && Directory.Exists(text2))
									{
										if (arrayList == null)
										{
											arrayList = new ArrayList(4);
										}
										arrayList.Add(new FAMData
										{
											FSW = fsw,
											Directory = text2,
											FileMask = fsw.MangledFilter,
											IncludeSubdirs = true,
											SubDirs = new Hashtable(),
											Enabled = true
										});
										arrayList.Add(famdata);
									}
								}
								if (!(text != famdata.Directory) || fsw.Pattern.IsMatch(text))
								{
									FileSystemWatcher obj = fsw;
									lock (obj)
									{
										fsw.DispatchEvents(fileAction, text, ref renamedEventArgs);
										if (fsw.Waiting)
										{
											fsw.Waiting = false;
											System.Threading.Monitor.PulseAll(fsw);
										}
									}
								}
							}
						}
					}
					if (FAMWatcher.FAMPending(ref FAMWatcher.conn) <= 0)
					{
						goto IL_28F;
					}
					continue;
					IL_75:
					flag = false;
					goto IL_7D;
				}
				return;
			}
			IL_28F:
			if (arrayList != null)
			{
				int count = arrayList.Count;
				for (int i = 0; i < count; i += 2)
				{
					FAMData famdata2 = (FAMData)arrayList[i];
					FAMData famdata3 = (FAMData)arrayList[i + 1];
					FAMWatcher.StartMonitoringDirectory(famdata2, true);
					FAMWatcher.requests[famdata2.Request.ReqNum] = famdata2;
					FAMData obj2 = famdata3;
					lock (obj2)
					{
						famdata3.SubDirs[famdata2.Directory] = famdata2;
					}
				}
				arrayList.Clear();
			}
		}

		~FAMWatcher()
		{
			FAMWatcher.FAMClose(ref FAMWatcher.conn);
		}

		private static int FAMOpen(out FAMConnection fc)
		{
			if (FAMWatcher.use_gamin)
			{
				return FAMWatcher.gamin_Open(out fc);
			}
			return FAMWatcher.fam_Open(out fc);
		}

		private static int FAMClose(ref FAMConnection fc)
		{
			if (FAMWatcher.use_gamin)
			{
				return FAMWatcher.gamin_Close(ref fc);
			}
			return FAMWatcher.fam_Close(ref fc);
		}

		private static int FAMMonitorDirectory(ref FAMConnection fc, string filename, out FAMRequest fr, IntPtr user_data)
		{
			if (FAMWatcher.use_gamin)
			{
				return FAMWatcher.gamin_MonitorDirectory(ref fc, filename, out fr, user_data);
			}
			return FAMWatcher.fam_MonitorDirectory(ref fc, filename, out fr, user_data);
		}

		private static int FAMCancelMonitor(ref FAMConnection fc, ref FAMRequest fr)
		{
			if (FAMWatcher.use_gamin)
			{
				return FAMWatcher.gamin_CancelMonitor(ref fc, ref fr);
			}
			return FAMWatcher.fam_CancelMonitor(ref fc, ref fr);
		}

		private static int FAMPending(ref FAMConnection fc)
		{
			if (FAMWatcher.use_gamin)
			{
				return FAMWatcher.gamin_Pending(ref fc);
			}
			return FAMWatcher.fam_Pending(ref fc);
		}

		[DllImport("libfam.so.0", EntryPoint = "FAMOpen")]
		private static extern int fam_Open(out FAMConnection fc);

		[DllImport("libfam.so.0", EntryPoint = "FAMClose")]
		private static extern int fam_Close(ref FAMConnection fc);

		[DllImport("libfam.so.0", EntryPoint = "FAMMonitorDirectory")]
		private static extern int fam_MonitorDirectory(ref FAMConnection fc, string filename, out FAMRequest fr, IntPtr user_data);

		[DllImport("libfam.so.0", EntryPoint = "FAMCancelMonitor")]
		private static extern int fam_CancelMonitor(ref FAMConnection fc, ref FAMRequest fr);

		[DllImport("libfam.so.0", EntryPoint = "FAMPending")]
		private static extern int fam_Pending(ref FAMConnection fc);

		[DllImport("libgamin-1.so.0", EntryPoint = "FAMOpen")]
		private static extern int gamin_Open(out FAMConnection fc);

		[DllImport("libgamin-1.so.0", EntryPoint = "FAMClose")]
		private static extern int gamin_Close(ref FAMConnection fc);

		[DllImport("libgamin-1.so.0", EntryPoint = "FAMMonitorDirectory")]
		private static extern int gamin_MonitorDirectory(ref FAMConnection fc, string filename, out FAMRequest fr, IntPtr user_data);

		[DllImport("libgamin-1.so.0", EntryPoint = "FAMCancelMonitor")]
		private static extern int gamin_CancelMonitor(ref FAMConnection fc, ref FAMRequest fr);

		[DllImport("libgamin-1.so.0", EntryPoint = "FAMPending")]
		private static extern int gamin_Pending(ref FAMConnection fc);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int InternalFAMNextEvent(ref FAMConnection fc, out string filename, out int code, out int reqnum);
	}
}
