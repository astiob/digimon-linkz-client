using System;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.IO
{
	internal sealed class MonoIO
	{
		public static readonly FileAttributes InvalidFileAttributes = (FileAttributes)(-1);

		public static readonly IntPtr InvalidHandle = (IntPtr)(-1L);

		public static Exception GetException(MonoIOError error)
		{
			if (error == MonoIOError.ERROR_ACCESS_DENIED)
			{
				return new UnauthorizedAccessException("Access to the path is denied.");
			}
			if (error != MonoIOError.ERROR_FILE_EXISTS)
			{
				return MonoIO.GetException(string.Empty, error);
			}
			string message = "Cannot create a file that already exist.";
			return new IOException(message, (int)((MonoIOError)(-2147024896) | error));
		}

		public static Exception GetException(string path, MonoIOError error)
		{
			switch (error)
			{
			case MonoIOError.ERROR_FILE_NOT_FOUND:
			{
				string message = string.Format("Could not find file \"{0}\"", path);
				return new IsolatedStorageException(message);
			}
			case MonoIOError.ERROR_PATH_NOT_FOUND:
			{
				string message = string.Format("Could not find a part of the path \"{0}\"", path);
				return new IsolatedStorageException(message);
			}
			case MonoIOError.ERROR_TOO_MANY_OPEN_FILES:
				return new IOException("Too many open files", (int)((MonoIOError)(-2147024896) | error));
			case MonoIOError.ERROR_ACCESS_DENIED:
			{
				string message = string.Format("Access to the path \"{0}\" is denied.", path);
				return new UnauthorizedAccessException(message);
			}
			case MonoIOError.ERROR_INVALID_HANDLE:
			{
				string message = string.Format("Invalid handle to path \"{0}\"", path);
				return new IOException(message, (int)((MonoIOError)(-2147024896) | error));
			}
			default:
				switch (error)
				{
				case MonoIOError.ERROR_WRITE_FAULT:
				{
					string message = string.Format("Write fault on path {0}", path);
					return new IOException(message, (int)((MonoIOError)(-2147024896) | error));
				}
				default:
					switch (error)
					{
					case MonoIOError.ERROR_INVALID_DRIVE:
					{
						string message = string.Format("Could not find the drive  '{0}'. The drive might not be ready or might not be mapped.", path);
						return new IOException(message, (int)((MonoIOError)(-2147024896) | error));
					}
					default:
						switch (error)
						{
						case MonoIOError.ERROR_FILE_EXISTS:
						{
							string message = string.Format("Could not create file \"{0}\". File already exists.", path);
							return new IOException(message, (int)((MonoIOError)(-2147024896) | error));
						}
						default:
							if (error == MonoIOError.ERROR_HANDLE_DISK_FULL)
							{
								string message = string.Format("Disk full. Path {0}", path);
								return new IOException(message, (int)((MonoIOError)(-2147024896) | error));
							}
							if (error == MonoIOError.ERROR_INVALID_PARAMETER)
							{
								string message = string.Format("Invalid parameter", new object[0]);
								return new IOException(message, (int)((MonoIOError)(-2147024896) | error));
							}
							if (error == MonoIOError.ERROR_DIR_NOT_EMPTY)
							{
								string message = string.Format("Directory {0} is not empty", path);
								return new IOException(message, (int)((MonoIOError)(-2147024896) | error));
							}
							if (error == MonoIOError.ERROR_FILENAME_EXCED_RANGE)
							{
								string message = string.Format("Path is too long. Path: {0}", path);
								return new PathTooLongException(message);
							}
							if (error != MonoIOError.ERROR_ENCRYPTION_FAILED)
							{
								string message = string.Format("Win32 IO returned {0}. Path: {1}", error, path);
								return new IOException(message, (int)((MonoIOError)(-2147024896) | error));
							}
							return new IOException("Encryption failed", (int)((MonoIOError)(-2147024896) | error));
						case MonoIOError.ERROR_CANNOT_MAKE:
						{
							string message = string.Format("Path {0} is a directory", path);
							return new IOException(message, (int)((MonoIOError)(-2147024896) | error));
						}
						}
						break;
					case MonoIOError.ERROR_NOT_SAME_DEVICE:
					{
						string message = "Source and destination are not on the same device";
						return new IOException(message, (int)((MonoIOError)(-2147024896) | error));
					}
					}
					break;
				case MonoIOError.ERROR_SHARING_VIOLATION:
				{
					string message = string.Format("Sharing violation on path {0}", path);
					return new IOException(message, (int)((MonoIOError)(-2147024896) | error));
				}
				case MonoIOError.ERROR_LOCK_VIOLATION:
				{
					string message = string.Format("Lock violation on path {0}", path);
					return new IOException(message, (int)((MonoIOError)(-2147024896) | error));
				}
				}
				break;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CreateDirectory(string path, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool RemoveDirectory(string path, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string[] GetFileSystemEntries(string path, string path_with_pattern, int attrs, int mask, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetCurrentDirectory(out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SetCurrentDirectory(string path, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool MoveFile(string path, string dest, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CopyFile(string path, string dest, bool overwrite, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool DeleteFile(string path, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool ReplaceFile(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern FileAttributes GetFileAttributes(string path, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SetFileAttributes(string path, FileAttributes attrs, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern MonoFileType GetFileType(IntPtr handle, out MonoIOError error);

		public static bool Exists(string path, out MonoIOError error)
		{
			FileAttributes fileAttributes = MonoIO.GetFileAttributes(path, out error);
			return fileAttributes != MonoIO.InvalidFileAttributes;
		}

		public static bool ExistsFile(string path, out MonoIOError error)
		{
			FileAttributes fileAttributes = MonoIO.GetFileAttributes(path, out error);
			return fileAttributes != MonoIO.InvalidFileAttributes && (fileAttributes & FileAttributes.Directory) == (FileAttributes)0;
		}

		public static bool ExistsDirectory(string path, out MonoIOError error)
		{
			FileAttributes fileAttributes = MonoIO.GetFileAttributes(path, out error);
			if (error == MonoIOError.ERROR_FILE_NOT_FOUND)
			{
				error = MonoIOError.ERROR_PATH_NOT_FOUND;
			}
			return fileAttributes != MonoIO.InvalidFileAttributes && (fileAttributes & FileAttributes.Directory) != (FileAttributes)0;
		}

		public static bool ExistsSymlink(string path, out MonoIOError error)
		{
			FileAttributes fileAttributes = MonoIO.GetFileAttributes(path, out error);
			return fileAttributes != MonoIO.InvalidFileAttributes && (fileAttributes & FileAttributes.ReparsePoint) != (FileAttributes)0;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetFileStat(string path, out MonoIOStat stat, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr Open(string filename, FileMode mode, FileAccess access, FileShare share, FileOptions options, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Close(IntPtr handle, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int Read(IntPtr handle, byte[] dest, int dest_offset, int count, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int Write(IntPtr handle, [In] byte[] src, int src_offset, int count, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long Seek(IntPtr handle, long offset, SeekOrigin origin, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Flush(IntPtr handle, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long GetLength(IntPtr handle, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SetLength(IntPtr handle, long length, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SetFileTime(IntPtr handle, long creation_time, long last_access_time, long last_write_time, out MonoIOError error);

		public static bool SetFileTime(string path, long creation_time, long last_access_time, long last_write_time, out MonoIOError error)
		{
			return MonoIO.SetFileTime(path, 0, creation_time, last_access_time, last_write_time, DateTime.MinValue, out error);
		}

		public static bool SetCreationTime(string path, DateTime dateTime, out MonoIOError error)
		{
			return MonoIO.SetFileTime(path, 1, -1L, -1L, -1L, dateTime, out error);
		}

		public static bool SetLastAccessTime(string path, DateTime dateTime, out MonoIOError error)
		{
			return MonoIO.SetFileTime(path, 2, -1L, -1L, -1L, dateTime, out error);
		}

		public static bool SetLastWriteTime(string path, DateTime dateTime, out MonoIOError error)
		{
			return MonoIO.SetFileTime(path, 3, -1L, -1L, -1L, dateTime, out error);
		}

		public static bool SetFileTime(string path, int type, long creation_time, long last_access_time, long last_write_time, DateTime dateTime, out MonoIOError error)
		{
			IntPtr intPtr = MonoIO.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, FileOptions.None, out error);
			if (intPtr == MonoIO.InvalidHandle)
			{
				return false;
			}
			switch (type)
			{
			case 1:
				creation_time = dateTime.ToFileTime();
				break;
			case 2:
				last_access_time = dateTime.ToFileTime();
				break;
			case 3:
				last_write_time = dateTime.ToFileTime();
				break;
			}
			bool result = MonoIO.SetFileTime(intPtr, creation_time, last_access_time, last_write_time, out error);
			MonoIOError monoIOError;
			MonoIO.Close(intPtr, out monoIOError);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Lock(IntPtr handle, long position, long length, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Unlock(IntPtr handle, long position, long length, out MonoIOError error);

		public static extern IntPtr ConsoleOutput { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern IntPtr ConsoleInput { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern IntPtr ConsoleError { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CreatePipe(out IntPtr read_handle, out IntPtr write_handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool DuplicateHandle(IntPtr source_process_handle, IntPtr source_handle, IntPtr target_process_handle, out IntPtr target_handle, int access, int inherit, int options);

		public static extern char VolumeSeparatorChar { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern char DirectorySeparatorChar { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern char AltDirectorySeparatorChar { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern char PathSeparator { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetTempPath(out string path);
	}
}
