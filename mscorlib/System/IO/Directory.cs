using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.IO
{
	/// <summary>Exposes static methods for creating, moving, and enumerating through directories and subdirectories. This class cannot be inherited.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	public static class Directory
	{
		/// <summary>Creates all directories and subdirectories as specified by <paramref name="path" />.</summary>
		/// <returns>A <see cref="T:System.IO.DirectoryInfo" /> as specified by <paramref name="path" />.</returns>
		/// <param name="path">The directory path to create. </param>
		/// <exception cref="T:System.IO.IOException">The directory specified by <paramref name="path" /> is a file.-or-The network name was not found.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.-or-<paramref name="path" /> is prefixed with, or contains only a colon character (:).</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> contains a colon character (:) that is not part of a drive label ("C:\").</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static DirectoryInfo CreateDirectory(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Path is empty");
			}
			if (path.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Path contains invalid chars");
			}
			if (path.Trim().Length == 0)
			{
				throw new ArgumentException("Only blank characters in path");
			}
			if (File.Exists(path))
			{
				throw new IOException("Cannot create " + path + " because a file with the same name already exists.");
			}
			if (path == ":")
			{
				throw new ArgumentException("Only ':' In path");
			}
			return Directory.CreateDirectoriesInternal(path);
		}

		private static DirectoryInfo CreateDirectoriesInternal(string path)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path, true);
			if (directoryInfo.Parent != null && !directoryInfo.Parent.Exists)
			{
				directoryInfo.Parent.Create();
			}
			MonoIOError monoIOError;
			if (!MonoIO.CreateDirectory(path, out monoIOError) && monoIOError != MonoIOError.ERROR_ALREADY_EXISTS && monoIOError != MonoIOError.ERROR_FILE_EXISTS)
			{
				throw MonoIO.GetException(path, monoIOError);
			}
			return directoryInfo;
		}

		/// <summary>Deletes an empty directory from a specified path.</summary>
		/// <param name="path">The name of the empty directory to remove. This directory must be writable or empty. </param>
		/// <exception cref="T:System.IO.IOException">A file with the same name and location specified by <paramref name="path" /> exists.-or-The directory is the application's current working directory.-or-The directory specified by <paramref name="path" /> is not empty.-or-The directory is read-only or contains a read-only file.-or-The directory is being used by another process..</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">
		///   <paramref name="path" /> does not exist or could not be found.-or-<paramref name="path" /> refers to a file instead of a directory.-or-The specified path is invalid (for example, it is on an unmapped drive). </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static void Delete(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Path is empty");
			}
			if (path.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Path contains invalid chars");
			}
			if (path.Trim().Length == 0)
			{
				throw new ArgumentException("Only blank characters in path");
			}
			if (path == ":")
			{
				throw new NotSupportedException("Only ':' In path");
			}
			MonoIOError monoIOError;
			bool flag;
			if (MonoIO.ExistsSymlink(path, out monoIOError))
			{
				flag = MonoIO.DeleteFile(path, out monoIOError);
			}
			else
			{
				flag = MonoIO.RemoveDirectory(path, out monoIOError);
			}
			if (flag)
			{
				return;
			}
			if (monoIOError != MonoIOError.ERROR_FILE_NOT_FOUND)
			{
				throw MonoIO.GetException(path, monoIOError);
			}
			if (File.Exists(path))
			{
				throw new IOException("Directory does not exist, but a file of the same name exist.");
			}
			throw new DirectoryNotFoundException("Directory does not exist.");
		}

		private static void RecursiveDelete(string path)
		{
			foreach (string path2 in Directory.GetDirectories(path))
			{
				MonoIOError monoIOError;
				if (MonoIO.ExistsSymlink(path2, out monoIOError))
				{
					MonoIO.DeleteFile(path2, out monoIOError);
				}
				else
				{
					Directory.RecursiveDelete(path2);
				}
			}
			foreach (string path3 in Directory.GetFiles(path))
			{
				File.Delete(path3);
			}
			Directory.Delete(path);
		}

		/// <summary>Deletes an empty directory and, if indicated, any subdirectories and files in the directory.  </summary>
		/// <param name="path">The name of the directory to remove. </param>
		/// <param name="recursive">true to remove directories, subdirectories, and files in <paramref name="path" />; otherwise, false. </param>
		/// <exception cref="T:System.IO.IOException">A file with the same name and location specified by <paramref name="path" /> exists.-or-The directory specified by <paramref name="path" /> is read-only, or <paramref name="recursive" /> is false and <paramref name="path" /> is not an empty directory. -or-The directory is the application's current working directory. -or-The directory contains a read-only file.-or-The directory is being used by another process.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">
		///   <paramref name="path" /> does not exist or could not be found.-or-<paramref name="path" /> refers to a file instead of a directory.-or-The specified path is invalid (for example, it is on an unmapped drive). </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static void Delete(string path, bool recursive)
		{
			Directory.CheckPathExceptions(path);
			if (recursive)
			{
				Directory.RecursiveDelete(path);
			}
			else
			{
				Directory.Delete(path);
			}
		}

		/// <summary>Determines whether the given path refers to an existing directory on disk.</summary>
		/// <returns>true if <paramref name="path" /> refers to an existing directory; otherwise, false.</returns>
		/// <param name="path">The path to test. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static bool Exists(string path)
		{
			MonoIOError monoIOError;
			return path != null && MonoIO.ExistsDirectory(path, out monoIOError);
		}

		/// <summary>Returns the date and time the specified file or directory was last accessed.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> structure set to the date and time the specified file or directory was last accessed. This value is expressed in local time.</returns>
		/// <param name="path">The file or directory for which to obtain access date and time information. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.NotSupportedException">The <paramref name="path" /> parameter is in an invalid format. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static DateTime GetLastAccessTime(string path)
		{
			return File.GetLastAccessTime(path);
		}

		/// <summary>Returns the date and time, in Coordinated Universal Time (UTC) format, that the specified file or directory was last accessed.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> structure set to the date and time the specified file or directory was last accessed. This value is expressed in UTC time.</returns>
		/// <param name="path">The file or directory for which to obtain access date and time information. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.NotSupportedException">The <paramref name="path" /> parameter is in an invalid format. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static DateTime GetLastAccessTimeUtc(string path)
		{
			return Directory.GetLastAccessTime(path).ToUniversalTime();
		}

		/// <summary>Returns the date and time the specified file or directory was last written to.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> structure set to the date and time the specified file or directory was last written to. This value is expressed in local time.</returns>
		/// <param name="path">The file or directory for which to obtain modification date and time information. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static DateTime GetLastWriteTime(string path)
		{
			return File.GetLastWriteTime(path);
		}

		/// <summary>Returns the date and time, in Coordinated Universal Time (UTC) format, that the specified file or directory was last written to.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> structure set to the date and time the specified file or directory was last written to. This value is expressed in UTC time.</returns>
		/// <param name="path">The file or directory for which to obtain modification date and time information. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static DateTime GetLastWriteTimeUtc(string path)
		{
			return Directory.GetLastWriteTime(path).ToUniversalTime();
		}

		/// <summary>Gets the creation date and time of a directory.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> structure set to the creation date and time for the specified directory. This value is expressed in local time.</returns>
		/// <param name="path">The path of the directory. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static DateTime GetCreationTime(string path)
		{
			return File.GetCreationTime(path);
		}

		/// <summary>Gets the creation date and time, in Coordinated Universal Time (UTC) format, of a directory.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> structure set to the creation date and time for the specified directory. This value is expressed in UTC time.</returns>
		/// <param name="path">The path of the directory. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static DateTime GetCreationTimeUtc(string path)
		{
			return Directory.GetCreationTime(path).ToUniversalTime();
		}

		/// <summary>Gets the current working directory of the application.</summary>
		/// <returns>A string that contains the path of the current working directory, and does not end with a backslash ("\").</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.NotSupportedException">The operating system is Windows CE, which does not have current directory functionality.This method is available in the .NET Compact Framework, but is not currently supported.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static string GetCurrentDirectory()
		{
			MonoIOError monoIOError;
			string currentDirectory = MonoIO.GetCurrentDirectory(out monoIOError);
			if (monoIOError != MonoIOError.ERROR_SUCCESS)
			{
				throw MonoIO.GetException(monoIOError);
			}
			return currentDirectory;
		}

		/// <summary>Gets the names of subdirectories in the specified directory.</summary>
		/// <returns>An array of type String containing the names of subdirectories in <paramref name="path" />.</returns>
		/// <param name="path">The path for which an array of subdirectory names is returned. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> is a file name. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static string[] GetDirectories(string path)
		{
			return Directory.GetDirectories(path, "*");
		}

		/// <summary>Gets an array of directories matching the specified search pattern from the current directory.</summary>
		/// <returns>A String array of directories matching the search pattern.</returns>
		/// <param name="path">The path to search. </param>
		/// <param name="searchPattern">The search string to match against the names of files in <paramref name="path" />. The parameter cannot end in two periods ("..") or contain two periods ("..") followed by <see cref="F:System.IO.Path.DirectorySeparatorChar" /> or <see cref="F:System.IO.Path.AltDirectorySeparatorChar" />, nor can it contain any of the characters in <see cref="F:System.IO.Path.InvalidPathChars" />. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.-or- <paramref name="searchPattern" /> does not contain a valid pattern. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> or <paramref name="searchPattern" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> is a file name. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static string[] GetDirectories(string path, string searchPattern)
		{
			return Directory.GetFileSystemEntries(path, searchPattern, FileAttributes.Directory, FileAttributes.Directory);
		}

		/// <summary>Gets an array of directories matching the specified search pattern from the current directory, using a value to determine whether to search subdirectories.</summary>
		/// <returns>A String array of directories matching the search pattern.</returns>
		/// <param name="path">The path to search. </param>
		/// <param name="searchPattern">The search string to match against the names of files in <paramref name="path" />. The parameter cannot end in two periods ("..") or contain two periods ("..") followed by <see cref="F:System.IO.Path.DirectorySeparatorChar" /> or <see cref="F:System.IO.Path.AltDirectorySeparatorChar" />, nor can it contain any of the characters in <see cref="F:System.IO.Path.InvalidPathChars" />. </param>
		/// <param name="searchOption">One of the <see cref="T:System.IO.SearchOption" /> values that specifies whether the search operation should include all subdirectories or only the current directory.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.-or- <paramref name="searchPattern" /> does not contain a valid pattern. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> or <paramref name="searchPattern" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> is a file name. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
		{
			if (searchOption == SearchOption.TopDirectoryOnly)
			{
				return Directory.GetDirectories(path, searchPattern);
			}
			ArrayList arrayList = new ArrayList();
			Directory.GetDirectoriesRecurse(path, searchPattern, arrayList);
			return (string[])arrayList.ToArray(typeof(string));
		}

		private static void GetDirectoriesRecurse(string path, string searchPattern, ArrayList all)
		{
			all.AddRange(Directory.GetDirectories(path, searchPattern));
			foreach (string path2 in Directory.GetDirectories(path))
			{
				Directory.GetDirectoriesRecurse(path2, searchPattern, all);
			}
		}

		/// <summary>Returns the volume information, root information, or both for the specified path.</summary>
		/// <returns>A string containing the volume information, root information, or both for the specified path.</returns>
		/// <param name="path">The path of a file or directory. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static string GetDirectoryRoot(string path)
		{
			return new string(Path.DirectorySeparatorChar, 1);
		}

		/// <summary>Returns the names of files (including their paths) in the specified directory.</summary>
		/// <returns>A String array of file names in the specified directory. File names include the full path.</returns>
		/// <param name="path">The directory from which to retrieve the files. </param>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> is a file name.-or-A network error has occurred. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static string[] GetFiles(string path)
		{
			return Directory.GetFiles(path, "*");
		}

		/// <summary>Returns the names of files (including their paths) in the specified directory that match the specified search pattern.</summary>
		/// <returns>A String array containing the names of files in the specified directory that match the specified search pattern. File names include the full path.</returns>
		/// <param name="path">The directory to search. </param>
		/// <param name="searchPattern">The search string to match against the names of files in <paramref name="path" />. The parameter cannot end in two periods ("..") or contain two periods ("..") followed by <see cref="F:System.IO.Path.DirectorySeparatorChar" /> or <see cref="F:System.IO.Path.AltDirectorySeparatorChar" />, nor can it contain any of the characters in <see cref="F:System.IO.Path.InvalidPathChars" />. </param>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> is a file name.-or-A network error has occurred. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.-or- <paramref name="searchPattern" /> does not contain a valid pattern. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> or <paramref name="searchPattern" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static string[] GetFiles(string path, string searchPattern)
		{
			return Directory.GetFileSystemEntries(path, searchPattern, FileAttributes.Directory, (FileAttributes)0);
		}

		/// <summary>Returns the names of files (including their paths) in the specified directory that match the specified search pattern, using a value to determine whether to search subdirectories.</summary>
		/// <returns>A String array containing the names of files in the specified directory that match the specified search pattern. File names include the full path.</returns>
		/// <param name="path">The directory to search. </param>
		/// <param name="searchPattern">The search string to match against the names of files in <paramref name="path" />. The parameter cannot end in two periods ("..") or contain two periods ("..") followed by <see cref="F:System.IO.Path.DirectorySeparatorChar" /> or <see cref="F:System.IO.Path.AltDirectorySeparatorChar" />, nor can it contain any of the characters in <see cref="F:System.IO.Path.InvalidPathChars" />. </param>
		/// <param name="searchOption">One of the <see cref="T:System.IO.SearchOption" /> values that specifies whether the search operation should include all subdirectories or only the current directory.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. -or- <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> or <paramref name="searchpattern" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> is a file name.-or-A network error has occurred. </exception>
		public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
		{
			if (searchOption == SearchOption.TopDirectoryOnly)
			{
				return Directory.GetFiles(path, searchPattern);
			}
			ArrayList arrayList = new ArrayList();
			Directory.GetFilesRecurse(path, searchPattern, arrayList);
			return (string[])arrayList.ToArray(typeof(string));
		}

		private static void GetFilesRecurse(string path, string searchPattern, ArrayList all)
		{
			all.AddRange(Directory.GetFiles(path, searchPattern));
			foreach (string path2 in Directory.GetDirectories(path))
			{
				Directory.GetFilesRecurse(path2, searchPattern, all);
			}
		}

		/// <summary>Returns the names of all files and subdirectories in the specified directory.</summary>
		/// <returns>A String array containing the names of file system entries in the specified directory.</returns>
		/// <param name="path">The directory for which file and subdirectory names are returned. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> is a file name. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static string[] GetFileSystemEntries(string path)
		{
			return Directory.GetFileSystemEntries(path, "*");
		}

		/// <summary>Returns an array of file system entries matching the specified search criteria.</summary>
		/// <returns>A String array of file system entries matching the search criteria.</returns>
		/// <param name="path">The path to be searched. </param>
		/// <param name="searchPattern">The search string to match against the names of files in <paramref name="path" />. The <paramref name="searchPattern" /> parameter cannot end in two periods ("..") or contain two periods ("..") followed by <see cref="F:System.IO.Path.DirectorySeparatorChar" /> or <see cref="F:System.IO.Path.AltDirectorySeparatorChar" />, nor can it contain any of the characters in <see cref="F:System.IO.Path.InvalidPathChars" />. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.-or- <paramref name="searchPattern" /> does not contain a valid pattern. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> or <paramref name="searchPattern" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> is a file name. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static string[] GetFileSystemEntries(string path, string searchPattern)
		{
			return Directory.GetFileSystemEntries(path, searchPattern, (FileAttributes)0, (FileAttributes)0);
		}

		/// <summary>Retrieves the names of the logical drives on this computer in the form "&lt;drive letter&gt;:\".</summary>
		/// <returns>The logical drives on this computer.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occured (for example, a disk error). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string[] GetLogicalDrives()
		{
			return Environment.GetLogicalDrives();
		}

		private static bool IsRootDirectory(string path)
		{
			return (Path.DirectorySeparatorChar == '/' && path == "/") || (Path.DirectorySeparatorChar == '\\' && path.Length == 3 && path.EndsWith(":\\"));
		}

		/// <summary>Retrieves the parent directory of the specified path, including both absolute and relative paths.</summary>
		/// <returns>The parent directory, or null if <paramref name="path" /> is the root directory, including the root of a UNC server or share name.</returns>
		/// <param name="path">The path for which to retrieve the parent directory. </param>
		/// <exception cref="T:System.IO.IOException">The directory specified by <paramref name="path" /> is read-only. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path was not found. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static DirectoryInfo GetParent(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Path contains invalid characters");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("The Path do not have a valid format");
			}
			if (Directory.IsRootDirectory(path))
			{
				return null;
			}
			string text = Path.GetDirectoryName(path);
			if (text.Length == 0)
			{
				text = Directory.GetCurrentDirectory();
			}
			return new DirectoryInfo(text);
		}

		/// <summary>Moves a file or a directory and its contents to a new location.</summary>
		/// <param name="sourceDirName">The path of the file or directory to move. </param>
		/// <param name="destDirName">The path to the new location for <paramref name="sourceDirName" />. If <paramref name="sourceDirName" /> is a file, then <paramref name="destDirName" /> must also be a file name.</param>
		/// <exception cref="T:System.IO.IOException">An attempt was made to move a directory to a different volume. -or- <paramref name="destDirName" /> already exists. -or- The <paramref name="sourceDirName" /> and <paramref name="destDirName" /> parameters refer to the same file or directory. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="sourceDirName" /> or <paramref name="destDirName" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceDirName" /> or <paramref name="destDirName" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified by <paramref name="sourceDirName" /> is invalid (for example, it is on an unmapped drive). </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static void Move(string sourceDirName, string destDirName)
		{
			if (sourceDirName == null)
			{
				throw new ArgumentNullException("sourceDirName");
			}
			if (destDirName == null)
			{
				throw new ArgumentNullException("destDirName");
			}
			if (sourceDirName.Trim().Length == 0 || sourceDirName.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Invalid source directory name: " + sourceDirName, "sourceDirName");
			}
			if (destDirName.Trim().Length == 0 || destDirName.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Invalid target directory name: " + destDirName, "destDirName");
			}
			if (sourceDirName == destDirName)
			{
				throw new IOException("Source and destination path must be different.");
			}
			if (Directory.Exists(destDirName))
			{
				throw new IOException(destDirName + " already exists.");
			}
			if (!Directory.Exists(sourceDirName) && !File.Exists(sourceDirName))
			{
				throw new DirectoryNotFoundException(sourceDirName + " does not exist");
			}
			MonoIOError error;
			if (!MonoIO.MoveFile(sourceDirName, destDirName, out error))
			{
				throw MonoIO.GetException(error);
			}
		}

		/// <summary>Sets the creation date and time for the specified file or directory.</summary>
		/// <param name="path">The file or directory for which to set the creation date and time information. </param>
		/// <param name="creationTime">A <see cref="T:System.DateTime" /> containing the value to set for the creation date and time of <paramref name="path" />. This value is expressed in local time. </param>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="creationTime" /> specifies a value outside the range of dates or times permitted for this operation. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static void SetCreationTime(string path, DateTime creationTime)
		{
			File.SetCreationTime(path, creationTime);
		}

		/// <summary>Sets the creation date and time, in Coordinated Universal Time (UTC) format, for the specified file or directory.</summary>
		/// <param name="path">The file or directory for which to set the creation date and time information. </param>
		/// <param name="creationTimeUtc">A <see cref="T:System.DateTime" /> containing the value to set for the creation date and time of <paramref name="path" />. This value is expressed in UTC time. </param>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="creationTime" /> specifies a value outside the range of dates or times permitted for this operation. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
		{
			Directory.SetCreationTime(path, creationTimeUtc.ToLocalTime());
		}

		/// <summary>Sets the application's current working directory to the specified directory.</summary>
		/// <param name="path">The path to which the current working directory is set. </param>
		/// <exception cref="T:System.IO.IOException">An IO error occurred. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to access unmanaged code. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified directory was not found.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static void SetCurrentDirectory(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Trim().Length == 0)
			{
				throw new ArgumentException("path string must not be an empty string or whitespace string");
			}
			if (!Directory.Exists(path))
			{
				throw new DirectoryNotFoundException("Directory \"" + path + "\" not found.");
			}
			MonoIOError monoIOError;
			MonoIO.SetCurrentDirectory(path, out monoIOError);
			if (monoIOError != MonoIOError.ERROR_SUCCESS)
			{
				throw MonoIO.GetException(path, monoIOError);
			}
		}

		/// <summary>Sets the date and time the specified file or directory was last accessed.</summary>
		/// <param name="path">The file or directory for which to set the access date and time information. </param>
		/// <param name="lastAccessTime">A <see cref="T:System.DateTime" /> containing the value to set for the access date and time of <paramref name="path" />. This value is expressed in local time. </param>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="lastAccessTime" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static void SetLastAccessTime(string path, DateTime lastAccessTime)
		{
			File.SetLastAccessTime(path, lastAccessTime);
		}

		/// <summary>Sets the date and time, in Coordinated Universal Time (UTC) format, that the specified file or directory was last accessed.</summary>
		/// <param name="path">The file or directory for which to set the access date and time information. </param>
		/// <param name="lastAccessTimeUtc">A <see cref="T:System.DateTime" /> containing the value to set for the access date and time of <paramref name="path" />. This value is expressed in UTC time. </param>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="lastAccessTimeUtc" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
		{
			Directory.SetLastAccessTime(path, lastAccessTimeUtc.ToLocalTime());
		}

		/// <summary>Sets the date and time a directory was last written to.</summary>
		/// <param name="path">The path of the directory. </param>
		/// <param name="lastWriteTime">The date and time the directory was last written to. This value is expressed in local time. </param>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="lastWriteTime" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static void SetLastWriteTime(string path, DateTime lastWriteTime)
		{
			File.SetLastWriteTime(path, lastWriteTime);
		}

		/// <summary>Sets the date and time, in Coordinated Universal Time (UTC) format, that a directory was last written to.</summary>
		/// <param name="path">The path of the directory. </param>
		/// <param name="lastWriteTimeUtc">The date and time the directory was last written to. This value is expressed in UTC time. </param>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="lastWriteTimeUtc" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
		{
			Directory.SetLastWriteTime(path, lastWriteTimeUtc.ToLocalTime());
		}

		private static void CheckPathExceptions(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Path is Empty");
			}
			if (path.Trim().Length == 0)
			{
				throw new ArgumentException("Only blank characters in path");
			}
			if (path.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Path contains invalid chars");
			}
		}

		private static string[] GetFileSystemEntries(string path, string searchPattern, FileAttributes mask, FileAttributes attrs)
		{
			if (path == null || searchPattern == null)
			{
				throw new ArgumentNullException();
			}
			if (searchPattern.Length == 0)
			{
				return new string[0];
			}
			if (path.Trim().Length == 0)
			{
				throw new ArgumentException("The Path does not have a valid format");
			}
			string path2 = Path.Combine(path, searchPattern);
			string directoryName = Path.GetDirectoryName(path2);
			if (directoryName.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Path contains invalid characters");
			}
			MonoIOError monoIOError;
			if (directoryName.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				if (path.IndexOfAny(SearchPattern.InvalidChars) == -1)
				{
					throw new ArgumentException("Path contains invalid characters", "path");
				}
				throw new ArgumentException("Pattern contains invalid characters", "pattern");
			}
			else if (!MonoIO.ExistsDirectory(directoryName, out monoIOError))
			{
				MonoIOError monoIOError2;
				if (monoIOError == MonoIOError.ERROR_SUCCESS && MonoIO.ExistsFile(directoryName, out monoIOError2))
				{
					return new string[]
					{
						directoryName
					};
				}
				if (monoIOError != MonoIOError.ERROR_PATH_NOT_FOUND)
				{
					throw MonoIO.GetException(directoryName, monoIOError);
				}
				if (directoryName.IndexOfAny(SearchPattern.WildcardChars) == -1)
				{
					throw new DirectoryNotFoundException("Directory '" + directoryName + "' not found.");
				}
				if (path.IndexOfAny(SearchPattern.WildcardChars) == -1)
				{
					throw new ArgumentException("Pattern is invalid", "searchPattern");
				}
				throw new ArgumentException("Path is invalid", "path");
			}
			else
			{
				string path_with_pattern = Path.Combine(directoryName, searchPattern);
				string[] fileSystemEntries = MonoIO.GetFileSystemEntries(path, path_with_pattern, (int)attrs, (int)mask, out monoIOError);
				if (monoIOError != MonoIOError.ERROR_SUCCESS)
				{
					throw MonoIO.GetException(directoryName, monoIOError);
				}
				return fileSystemEntries;
			}
		}
	}
}
