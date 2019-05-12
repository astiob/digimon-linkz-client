using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO
{
	/// <summary>Exposes instance methods for creating, moving, and enumerating through directories and subdirectories. This class cannot be inherited.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public sealed class DirectoryInfo : FileSystemInfo
	{
		private string current;

		private string parent;

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.DirectoryInfo" /> class on the specified path.</summary>
		/// <param name="path">A string specifying the path on which to create the DirectoryInfo. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> contains invalid characters such as ", &lt;, &gt;, or |. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. The specified path, file name, or both are too long.</exception>
		public DirectoryInfo(string path) : this(path, false)
		{
		}

		internal DirectoryInfo(string path, bool simpleOriginalPath)
		{
			base.CheckPath(path);
			this.FullPath = Path.GetFullPath(path);
			if (simpleOriginalPath)
			{
				this.OriginalPath = Path.GetFileName(path);
			}
			else
			{
				this.OriginalPath = path;
			}
			this.Initialize();
		}

		private DirectoryInfo(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.Initialize();
		}

		private void Initialize()
		{
			int num = this.FullPath.Length - 1;
			if (num > 1 && this.FullPath[num] == Path.DirectorySeparatorChar)
			{
				num--;
			}
			int num2 = this.FullPath.LastIndexOf(Path.DirectorySeparatorChar, num);
			if (num2 == -1 || (num2 == 0 && num == 0))
			{
				this.current = this.FullPath;
				this.parent = null;
			}
			else
			{
				this.current = this.FullPath.Substring(num2 + 1, num - num2);
				if (num2 == 0 && !Environment.IsRunningOnWindows)
				{
					this.parent = Path.DirectorySeparatorStr;
				}
				else
				{
					this.parent = this.FullPath.Substring(0, num2);
				}
				if (Environment.IsRunningOnWindows && this.parent.Length == 2 && this.parent[1] == ':' && char.IsLetter(this.parent[0]))
				{
					this.parent += Path.DirectorySeparatorChar;
				}
			}
		}

		/// <summary>Gets a value indicating whether the directory exists.</summary>
		/// <returns>true if the directory exists; otherwise, false.</returns>
		/// <filterpriority>1</filterpriority>
		public override bool Exists
		{
			get
			{
				base.Refresh(false);
				return this.stat.Attributes != MonoIO.InvalidFileAttributes && (this.stat.Attributes & FileAttributes.Directory) != (FileAttributes)0;
			}
		}

		/// <summary>Gets the name of this <see cref="T:System.IO.DirectoryInfo" /> instance.</summary>
		/// <returns>The directory name.</returns>
		/// <filterpriority>1</filterpriority>
		public override string Name
		{
			get
			{
				return this.current;
			}
		}

		/// <summary>Gets the parent directory of a specified subdirectory.</summary>
		/// <returns>The parent directory, or null if the path is null or if the file path denotes a root (such as "\", "C:", or * "\\server\share").</returns>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public DirectoryInfo Parent
		{
			get
			{
				if (this.parent == null || this.parent.Length == 0)
				{
					return null;
				}
				return new DirectoryInfo(this.parent);
			}
		}

		/// <summary>Gets the root portion of a path.</summary>
		/// <returns>A <see cref="T:System.IO.DirectoryInfo" /> object representing the root of a path.</returns>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public DirectoryInfo Root
		{
			get
			{
				string pathRoot = Path.GetPathRoot(this.FullPath);
				if (pathRoot == null)
				{
					return null;
				}
				return new DirectoryInfo(pathRoot);
			}
		}

		/// <summary>Creates a directory.</summary>
		/// <exception cref="T:System.IO.IOException">The directory cannot be created. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Create()
		{
			Directory.CreateDirectory(this.FullPath);
		}

		/// <summary>Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="T:System.IO.DirectoryInfo" /> class.</summary>
		/// <returns>The last directory specified in <paramref name="path" />.</returns>
		/// <param name="path">The specified path. This cannot be a different disk volume or Universal Naming Convention (UNC) name. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> does not specify a valid file path or contains invalid DirectoryInfo characters. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.IO.IOException">The subdirectory cannot be created.-or- A file or directory already has the name specified by <paramref name="path" />. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. The specified path, file name, or both are too long.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have code access permission to create the directory.-or-The caller does not have code access permission to read the directory described by the returned <see cref="T:System.IO.DirectoryInfo" /> object.  This can occur when the <paramref name="path" /> parameter describes an existing directory.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> contains a colon (:) that is not part of a drive label ("C:\").</exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public DirectoryInfo CreateSubdirectory(string path)
		{
			base.CheckPath(path);
			path = Path.Combine(this.FullPath, path);
			Directory.CreateDirectory(path);
			return new DirectoryInfo(path);
		}

		/// <summary>Returns a file list from the current directory.</summary>
		/// <returns>An array of type <see cref="T:System.IO.FileInfo" />.</returns>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The path is invalid, such as being on an unmapped drive. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public FileInfo[] GetFiles()
		{
			return this.GetFiles("*");
		}

		/// <summary>Returns a file list from the current directory matching the given <paramref name="searchPattern" />.</summary>
		/// <returns>An array of type <see cref="T:System.IO.FileInfo" />.</returns>
		/// <param name="searchPattern">The search string, such as "*.txt". </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="searchPattern " />contains invalid characters. To determine the invalid characters, use the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="searchPattern" /> is null. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public FileInfo[] GetFiles(string searchPattern)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			string[] files = Directory.GetFiles(this.FullPath, searchPattern);
			FileInfo[] array = new FileInfo[files.Length];
			int num = 0;
			foreach (string fileName in files)
			{
				array[num++] = new FileInfo(fileName);
			}
			return array;
		}

		/// <summary>Returns the subdirectories of the current directory.</summary>
		/// <returns>An array of <see cref="T:System.IO.DirectoryInfo" /> objects.</returns>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the DirectoryInfo object is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public DirectoryInfo[] GetDirectories()
		{
			return this.GetDirectories("*");
		}

		/// <summary>Returns an array of directories in the current <see cref="T:System.IO.DirectoryInfo" /> matching the given search criteria.</summary>
		/// <returns>An array of type DirectoryInfo matching <paramref name="searchPattern" />.</returns>
		/// <param name="searchPattern">The search string, such as "System*", used to search for all directories beginning with the word "System". </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="searchPattern " />contains invalid characters. To determine the invalid characters, use the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="searchPattern" /> is null. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the DirectoryInfo object is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public DirectoryInfo[] GetDirectories(string searchPattern)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			string[] directories = Directory.GetDirectories(this.FullPath, searchPattern);
			DirectoryInfo[] array = new DirectoryInfo[directories.Length];
			int num = 0;
			foreach (string path in directories)
			{
				array[num++] = new DirectoryInfo(path);
			}
			return array;
		}

		/// <summary>Returns an array of strongly typed <see cref="T:System.IO.FileSystemInfo" /> entries representing all the files and subdirectories in a directory.</summary>
		/// <returns>An array of strongly typed <see cref="T:System.IO.FileSystemInfo" /> entries.</returns>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The path is invalid, such as being on an unmapped drive. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public FileSystemInfo[] GetFileSystemInfos()
		{
			return this.GetFileSystemInfos("*");
		}

		/// <summary>Retrieves an array of strongly typed <see cref="T:System.IO.FileSystemInfo" /> objects representing the files and subdirectories matching the specified search criteria.</summary>
		/// <returns>An array of strongly typed FileSystemInfo objects matching the search criteria.</returns>
		/// <param name="searchPattern">The search string, such as "System*", used to search for all directories beginning with the word "System". </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="searchPattern " />contains invalid characters. To determine the invalid characters, use the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="searchPattern" /> is null. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public FileSystemInfo[] GetFileSystemInfos(string searchPattern)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			if (!Directory.Exists(this.FullPath))
			{
				throw new IOException("Invalid directory");
			}
			string[] directories = Directory.GetDirectories(this.FullPath, searchPattern);
			string[] files = Directory.GetFiles(this.FullPath, searchPattern);
			FileSystemInfo[] array = new FileSystemInfo[directories.Length + files.Length];
			int num = 0;
			foreach (string path in directories)
			{
				array[num++] = new DirectoryInfo(path);
			}
			foreach (string fileName in files)
			{
				array[num++] = new FileInfo(fileName);
			}
			return array;
		}

		/// <summary>Deletes this <see cref="T:System.IO.DirectoryInfo" /> if it is empty.</summary>
		/// <exception cref="T:System.UnauthorizedAccessException">The directory contains a read-only file.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The directory described by this <see cref="T:System.IO.DirectoryInfo" /> object does not exist or could not be found.</exception>
		/// <exception cref="T:System.IO.IOException">The directory is not empty. -or-The directory is the application's current working directory.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override void Delete()
		{
			this.Delete(false);
		}

		/// <summary>Deletes this instance of a <see cref="T:System.IO.DirectoryInfo" />, specifying whether to delete subdirectories and files.</summary>
		/// <param name="recursive">true to delete this directory, its subdirectories, and all files; otherwise, false. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The directory contains a read-only file.</exception>
		/// <exception cref="T:System.IO.IOException">The directory is read-only.-or- The directory contains one or more files or subdirectories and <paramref name="recursive" /> is false.-or-The directory is the application's current working directory. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Delete(bool recursive)
		{
			Directory.Delete(this.FullPath, recursive);
		}

		/// <summary>Moves a <see cref="T:System.IO.DirectoryInfo" /> instance and its contents to a new path.</summary>
		/// <param name="destDirName">The name and path to which to move this directory. The destination cannot be another disk volume or a directory with the identical name. It can be an existing directory to which you want to add this directory as a subdirectory. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destDirName" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="destDirName" /> is an empty string (''"). </exception>
		/// <exception cref="T:System.IO.IOException">An attempt was made to move a directory to a different volume. -or-<paramref name="destDirName" /> already exists.-or-You are not authorized to access this path.-or- The directory being moved and the destination directory have the same name.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The destination directory cannot be found.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void MoveTo(string destDirName)
		{
			if (destDirName == null)
			{
				throw new ArgumentNullException("destDirName");
			}
			if (destDirName.Length == 0)
			{
				throw new ArgumentException("An empty file name is not valid.", "destDirName");
			}
			Directory.Move(this.FullPath, Path.GetFullPath(destDirName));
		}

		/// <summary>Returns the original path that was passed by the user.</summary>
		/// <returns>Returns the original path that was passed by the user.</returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return this.OriginalPath;
		}

		/// <summary>Returns an array of directories in the current <see cref="T:System.IO.DirectoryInfo" /> matching the given search criteria and using a value to determine whether to search subdirectories.</summary>
		/// <returns>An array of type DirectoryInfo matching <paramref name="searchPattern" />.</returns>
		/// <param name="searchPattern">The search string, such as "System*", used to search for all directories beginning with the word "System".</param>
		/// <param name="searchOption">One of the values of the <see cref="T:System.IO.SearchOption" /> enumeration that specifies whether the search operation should include only the current directory or should include all subdirectories.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="searchPattern " />contains invalid characters. To determine the invalid characters, use the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="searchPattern" /> is null. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the DirectoryInfo object is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
		public DirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
		{
			if (searchOption == SearchOption.TopDirectoryOnly)
			{
				return this.GetDirectories(searchPattern);
			}
			if (searchOption != SearchOption.AllDirectories)
			{
				string text = Locale.GetText("Invalid enum value '{0}' for '{1}'.", new object[]
				{
					searchOption,
					"SearchOption"
				});
				throw new ArgumentOutOfRangeException("searchOption", text);
			}
			Queue queue = new Queue(this.GetDirectories(searchPattern));
			Queue queue2 = new Queue();
			while (queue.Count > 0)
			{
				DirectoryInfo directoryInfo = (DirectoryInfo)queue.Dequeue();
				DirectoryInfo[] directories = directoryInfo.GetDirectories(searchPattern);
				foreach (DirectoryInfo obj in directories)
				{
					queue.Enqueue(obj);
				}
				queue2.Enqueue(directoryInfo);
			}
			DirectoryInfo[] array2 = new DirectoryInfo[queue2.Count];
			queue2.CopyTo(array2, 0);
			return array2;
		}

		internal int GetFilesSubdirs(ArrayList l, string pattern)
		{
			FileInfo[] array = null;
			try
			{
				array = this.GetFiles(pattern);
			}
			catch (UnauthorizedAccessException)
			{
				return 0;
			}
			int num = array.Length;
			l.Add(array);
			foreach (DirectoryInfo directoryInfo in this.GetDirectories())
			{
				num += directoryInfo.GetFilesSubdirs(l, pattern);
			}
			return num;
		}

		/// <summary>Returns a file list from the current directory matching the given <paramref name="searchPattern" /> and using a value to determine whether to search subdirectories.</summary>
		/// <returns>An array of type <see cref="T:System.IO.FileInfo" />.</returns>
		/// <param name="searchPattern">The search string, such as "System*", used to search for all directories beginning with the word "System".</param>
		/// <param name="searchOption">One of the values of the <see cref="T:System.IO.SearchOption" /> enumeration that specifies whether the search operation should include only the current directory or should include all subdirectories.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="searchPattern " />contains invalid characters. To determine invalid characters, use the  <see cref="M:System.IO.Path.GetInvalidPathChars" /> method. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="searchPattern" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public FileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
		{
			if (searchOption == SearchOption.TopDirectoryOnly)
			{
				return this.GetFiles(searchPattern);
			}
			if (searchOption != SearchOption.AllDirectories)
			{
				string text = Locale.GetText("Invalid enum value '{0}' for '{1}'.", new object[]
				{
					searchOption,
					"SearchOption"
				});
				throw new ArgumentOutOfRangeException("searchOption", text);
			}
			ArrayList arrayList = new ArrayList();
			int filesSubdirs = this.GetFilesSubdirs(arrayList, searchPattern);
			int num = 0;
			FileInfo[] array = new FileInfo[filesSubdirs];
			foreach (object obj in arrayList)
			{
				FileInfo[] array2 = (FileInfo[])obj;
				array2.CopyTo(array, num);
				num += array2.Length;
			}
			return array;
		}
	}
}
