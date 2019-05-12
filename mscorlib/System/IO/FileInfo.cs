using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO
{
	/// <summary>Provides instance methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of <see cref="T:System.IO.FileStream" /> objects. This class cannot be inherited.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public sealed class FileInfo : FileSystemInfo
	{
		private bool exists;

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.FileInfo" /> class, which acts as a wrapper for a file path.</summary>
		/// <param name="fileName">The fully qualified name of the new file, or the relative file name. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fileName" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">The file name is empty, contains only white spaces, or contains invalid characters. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">Access to <paramref name="fileName" /> is denied. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="fileName" /> contains a colon (:) in the middle of the string. </exception>
		public FileInfo(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			base.CheckPath(fileName);
			this.OriginalPath = fileName;
			this.FullPath = Path.GetFullPath(fileName);
		}

		private FileInfo(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		internal override void InternalRefresh()
		{
			this.exists = File.Exists(this.FullPath);
		}

		/// <summary>Gets a value indicating whether a file exists.</summary>
		/// <returns>true if the file exists; false if the file does not exist or if the file is a directory.</returns>
		/// <filterpriority>1</filterpriority>
		public override bool Exists
		{
			get
			{
				base.Refresh(false);
				return this.stat.Attributes != MonoIO.InvalidFileAttributes && (this.stat.Attributes & FileAttributes.Directory) == (FileAttributes)0 && this.exists;
			}
		}

		/// <summary>Gets the name of the file.</summary>
		/// <returns>The name of the file.</returns>
		/// <filterpriority>1</filterpriority>
		public override string Name
		{
			get
			{
				return Path.GetFileName(this.FullPath);
			}
		}

		/// <summary>Gets or sets a value that determines if the current file is read only.</summary>
		/// <returns>true if the current file is read only; otherwise, false.</returns>
		/// <exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo" /> object could not be found.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The file described by the current <see cref="T:System.IO.FileInfo" /> object is read-only.-or- This operation is not supported on the current platform.-or- The caller does not have the required permission.</exception>
		/// <filterpriority>1</filterpriority>
		public bool IsReadOnly
		{
			get
			{
				if (!this.Exists)
				{
					throw new FileNotFoundException("Could not find file \"" + this.OriginalPath + "\".", this.OriginalPath);
				}
				return (this.stat.Attributes & FileAttributes.ReadOnly) != (FileAttributes)0;
			}
			set
			{
				if (!this.Exists)
				{
					throw new FileNotFoundException("Could not find file \"" + this.OriginalPath + "\".", this.OriginalPath);
				}
				FileAttributes fileAttributes = File.GetAttributes(this.FullPath);
				if (value)
				{
					fileAttributes |= FileAttributes.ReadOnly;
				}
				else
				{
					fileAttributes &= ~FileAttributes.ReadOnly;
				}
				File.SetAttributes(this.FullPath, fileAttributes);
			}
		}

		/// <summary>Encrypts a file so that only the account used to encrypt the file can decrypt it.</summary>
		/// <exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo" /> object could not be found.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.NotSupportedException">The file system is not NTFS.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The file described by the current <see cref="T:System.IO.FileInfo" /> object is read-only.-or- This operation is not supported on the current platform.-or- The caller does not have the required permission.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[MonoLimitation("File encryption isn't supported (even on NTFS).")]
		[ComVisible(false)]
		public void Encrypt()
		{
			throw new NotSupportedException(Locale.GetText("File encryption isn't supported on any file system."));
		}

		/// <summary>Decrypts a file that was encrypted by the current account using the <see cref="M:System.IO.FileInfo.Encrypt" /> method.</summary>
		/// <exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo" /> object could not be found.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.NotSupportedException">The file system is not NTFS.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The file described by the current <see cref="T:System.IO.FileInfo" /> object is read-only.-or- This operation is not supported on the current platform.-or- The caller does not have the required permission.</exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[ComVisible(false)]
		[MonoLimitation("File encryption isn't supported (even on NTFS).")]
		public void Decrypt()
		{
			throw new NotSupportedException(Locale.GetText("File encryption isn't supported on any file system."));
		}

		/// <summary>Gets the size, in bytes, of the current file.</summary>
		/// <returns>The size of the current file in bytes.</returns>
		/// <exception cref="T:System.IO.IOException">
		///   <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot update the state of the file or directory. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file does not exist.-or- The Length property is called for a directory. </exception>
		/// <filterpriority>1</filterpriority>
		public long Length
		{
			get
			{
				if (!this.Exists)
				{
					throw new FileNotFoundException("Could not find file \"" + this.OriginalPath + "\".", this.OriginalPath);
				}
				return this.stat.Length;
			}
		}

		/// <summary>Gets a string representing the directory's full path.</summary>
		/// <returns>A string representing the directory's full path.</returns>
		/// <exception cref="T:System.ArgumentNullException">null was passed in for the directory name. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The fully qualified path is 260 or more characters.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public string DirectoryName
		{
			get
			{
				return Path.GetDirectoryName(this.FullPath);
			}
		}

		/// <summary>Gets an instance of the parent directory.</summary>
		/// <returns>A <see cref="T:System.IO.DirectoryInfo" /> object representing the parent directory of this file.</returns>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public DirectoryInfo Directory
		{
			get
			{
				return new DirectoryInfo(this.DirectoryName);
			}
		}

		/// <summary>Creates a <see cref="T:System.IO.StreamReader" /> with UTF8 encoding that reads from an existing text file.</summary>
		/// <returns>A new StreamReader with UTF8 encoding.</returns>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file is not found. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> is read-only or is a directory. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public StreamReader OpenText()
		{
			return new StreamReader(this.Open(FileMode.Open, FileAccess.Read));
		}

		/// <summary>Creates a <see cref="T:System.IO.StreamWriter" /> that writes a new text file.</summary>
		/// <returns>A new StreamWriter.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The file name is a directory. </exception>
		/// <exception cref="T:System.IO.IOException">The disk is read-only. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public StreamWriter CreateText()
		{
			return new StreamWriter(this.Open(FileMode.Create, FileAccess.Write));
		}

		/// <summary>Creates a <see cref="T:System.IO.StreamWriter" /> that appends text to the file represented by this instance of the <see cref="T:System.IO.FileInfo" />.</summary>
		/// <returns>A new StreamWriter.</returns>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public StreamWriter AppendText()
		{
			return new StreamWriter(this.Open(FileMode.Append, FileAccess.Write));
		}

		/// <summary>Creates a file.</summary>
		/// <returns>A new file.</returns>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public FileStream Create()
		{
			return File.Create(this.FullPath);
		}

		/// <summary>Creates a read-only <see cref="T:System.IO.FileStream" />.</summary>
		/// <returns>A new read-only <see cref="T:System.IO.FileStream" /> object.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> is read-only or is a directory. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.IO.IOException">The file is already open. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public FileStream OpenRead()
		{
			return this.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		/// <summary>Creates a write-only <see cref="T:System.IO.FileStream" />.</summary>
		/// <returns>A new write-only unshared <see cref="T:System.IO.FileStream" /> object.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> is read-only or is a directory. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public FileStream OpenWrite()
		{
			return this.Open(FileMode.OpenOrCreate, FileAccess.Write);
		}

		/// <summary>Opens a file in the specified mode.</summary>
		/// <returns>A file opened in the specified mode, with read/write access and unshared.</returns>
		/// <param name="mode">A <see cref="T:System.IO.FileMode" /> constant specifying the mode (for example, Open or Append) in which to open the file. </param>
		/// <exception cref="T:System.IO.FileNotFoundException">The file is not found. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The file is read-only or is a directory. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.IO.IOException">The file is already open. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public FileStream Open(FileMode mode)
		{
			return this.Open(mode, FileAccess.ReadWrite);
		}

		/// <summary>Opens a file in the specified mode with read, write, or read/write access.</summary>
		/// <returns>A <see cref="T:System.IO.FileStream" /> object opened in the specified mode and access, and unshared.</returns>
		/// <param name="mode">A <see cref="T:System.IO.FileMode" /> constant specifying the mode (for example, Open or Append) in which to open the file. </param>
		/// <param name="access">A <see cref="T:System.IO.FileAccess" /> constant specifying whether to open the file with Read, Write, or ReadWrite file access. </param>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is empty or contains only white spaces. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file is not found. </exception>
		/// <exception cref="T:System.ArgumentNullException">One or more arguments is null. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> is read-only or is a directory. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.IO.IOException">The file is already open. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public FileStream Open(FileMode mode, FileAccess access)
		{
			return this.Open(mode, access, FileShare.None);
		}

		/// <summary>Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.</summary>
		/// <returns>A <see cref="T:System.IO.FileStream" /> object opened with the specified mode, access, and sharing options.</returns>
		/// <param name="mode">A <see cref="T:System.IO.FileMode" /> constant specifying the mode (for example, Open or Append) in which to open the file. </param>
		/// <param name="access">A <see cref="T:System.IO.FileAccess" /> constant specifying whether to open the file with Read, Write, or ReadWrite file access. </param>
		/// <param name="share">A <see cref="T:System.IO.FileShare" /> constant specifying the type of access other FileStream objects have to this file. </param>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is empty or contains only white spaces. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file is not found. </exception>
		/// <exception cref="T:System.ArgumentNullException">One or more arguments is null. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="path" /> is read-only or is a directory. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.IO.IOException">The file is already open. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public FileStream Open(FileMode mode, FileAccess access, FileShare share)
		{
			return new FileStream(this.FullPath, mode, access, share);
		}

		/// <summary>Permanently deletes a file.</summary>
		/// <exception cref="T:System.IO.IOException">The target file is open or memory-mapped on a computer running Microsoft Windows NT. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The path is a directory. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override void Delete()
		{
			MonoIOError error;
			if (!MonoIO.Exists(this.FullPath, out error))
			{
				return;
			}
			if (MonoIO.ExistsDirectory(this.FullPath, out error))
			{
				throw new UnauthorizedAccessException("Access to the path \"" + this.FullPath + "\" is denied.");
			}
			if (!MonoIO.DeleteFile(this.FullPath, out error))
			{
				throw MonoIO.GetException(this.OriginalPath, error);
			}
		}

		/// <summary>Moves a specified file to a new location, providing the option to specify a new file name.</summary>
		/// <param name="destFileName">The path to move the file to, which can specify a different file name. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs, such as the destination file already exists or the destination device is not ready. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destFileName" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="destFileName" /> is empty, contains only white spaces, or contains invalid characters. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="destFileName" /> is read-only or is a directory. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file is not found. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="destFileName" /> contains a colon (:) in the middle of the string. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void MoveTo(string destFileName)
		{
			if (destFileName == null)
			{
				throw new ArgumentNullException("destFileName");
			}
			if (destFileName == this.Name || destFileName == this.FullName)
			{
				return;
			}
			if (!File.Exists(this.FullPath))
			{
				throw new FileNotFoundException();
			}
			File.Move(this.FullPath, destFileName);
			this.FullPath = Path.GetFullPath(destFileName);
		}

		/// <summary>Copies an existing file to a new file, disallowing the overwriting of an existing file.</summary>
		/// <returns>A new file with a fully qualified path.</returns>
		/// <param name="destFileName">The name of the new file to copy to. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="destFileName" /> is empty, contains only white spaces, or contains invalid characters. </exception>
		/// <exception cref="T:System.IO.IOException">An error occurs, or the destination file already exists. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destFileName" /> is null. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">A directory path is passed in, or the file is being moved to a different drive. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The directory specified in <paramref name="destFileName" /> does not exist.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="destFileName" /> contains a colon (:) within the string but does not specify the volume. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public FileInfo CopyTo(string destFileName)
		{
			return this.CopyTo(destFileName, false);
		}

		/// <summary>Copies an existing file to a new file, allowing the overwriting of an existing file.</summary>
		/// <returns>A new file, or an overwrite of an existing file if <paramref name="overwrite" /> is true. If the file exists and <paramref name="overwrite" /> is false, an <see cref="T:System.IO.IOException" /> is thrown.</returns>
		/// <param name="destFileName">The name of the new file to copy to. </param>
		/// <param name="overwrite">true to allow an existing file to be overwritten; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="destFileName" /> is empty, contains only white spaces, or contains invalid characters. </exception>
		/// <exception cref="T:System.IO.IOException">An error occurs, or the destination file already exists and <paramref name="overwrite" /> is false. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destFileName" /> is null. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The directory specified in <paramref name="destFileName" /> does not exist.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">A directory path is passed in, or the file is being moved to a different drive. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="destFileName" /> contains a colon (:) in the middle of the string. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public FileInfo CopyTo(string destFileName, bool overwrite)
		{
			if (destFileName == null)
			{
				throw new ArgumentNullException("destFileName");
			}
			if (destFileName.Length == 0)
			{
				throw new ArgumentException("An empty file name is not valid.", "destFileName");
			}
			string fullPath = Path.GetFullPath(destFileName);
			if (overwrite && File.Exists(fullPath))
			{
				File.Delete(fullPath);
			}
			File.Copy(this.FullPath, fullPath);
			return new FileInfo(fullPath);
		}

		/// <summary>Returns the path as a string.</summary>
		/// <returns>A string representing the path.</returns>
		/// <filterpriority>1</filterpriority>
		public override string ToString()
		{
			return this.Name;
		}

		/// <summary>Replaces the contents of a specified file with the file described by the current <see cref="T:System.IO.FileInfo" /> object, deleting the original file, and creating a backup of the replaced file.</summary>
		/// <returns>A <see cref="T:System.IO.FileInfo" /> object that encapsulates information about the file described by the <paramref name="destFileName" /> parameter.</returns>
		/// <param name="destinationFileName">The name of a file to replace with the current file.</param>
		/// <param name="destinationBackupFileName">The name of a file with which to create a backup of the file described by the <paramref name="destFileName" /> parameter.</param>
		/// <exception cref="T:System.ArgumentException">The path described by the <paramref name="destFileName" /> parameter was not of a legal form.-or-The path described by the <paramref name="destBackupFileName" /> parameter was not of a legal form.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="destFileName" /> parameter is null.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo" /> object could not be found.-or-The file described by the <paramref name="destinationFileName" /> parameter could not be found. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[ComVisible(false)]
		public FileInfo Replace(string destinationFileName, string destinationBackupFileName)
		{
			if (!this.Exists)
			{
				throw new FileNotFoundException();
			}
			if (destinationFileName == null)
			{
				throw new ArgumentNullException("destinationFileName");
			}
			if (destinationFileName.Length == 0)
			{
				throw new ArgumentException("An empty file name is not valid.", "destinationFileName");
			}
			string fullPath = Path.GetFullPath(destinationFileName);
			if (!File.Exists(fullPath))
			{
				throw new FileNotFoundException();
			}
			FileAttributes attributes = File.GetAttributes(fullPath);
			if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
			{
				throw new UnauthorizedAccessException();
			}
			if (destinationBackupFileName != null)
			{
				if (destinationBackupFileName.Length == 0)
				{
					throw new ArgumentException("An empty file name is not valid.", "destinationBackupFileName");
				}
				File.Copy(fullPath, Path.GetFullPath(destinationBackupFileName), true);
			}
			File.Copy(this.FullPath, fullPath, true);
			File.Delete(this.FullPath);
			return new FileInfo(fullPath);
		}

		/// <summary>Replaces the contents of a specified file with the file described by the current <see cref="T:System.IO.FileInfo" /> object, deleting the original file, and creating a backup of the replaced file.  Also specifies whether to ignore merge errors. </summary>
		/// <returns>A <see cref="T:System.IO.FileInfo" /> object that encapsulates information about the file described by the <paramref name="destFileName" /> parameter.</returns>
		/// <param name="destinationFileName">The name of a file to replace with the current file.</param>
		/// <param name="destinationBackupFileName">The name of a file with which to create a backup of the file described by the <paramref name="destFileName" /> parameter.</param>
		/// <param name="ignoreMetadataErrors">true to ignore merge errors (such as attributes and ACLs) from the replaced file to the replacement file; otherwise false. </param>
		/// <exception cref="T:System.ArgumentException">The path described by the <paramref name="destFileName" /> parameter was not of a legal form.-or-The path described by the <paramref name="destBackupFileName" /> parameter was not of a legal form.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="destFileName" /> parameter is null.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo" /> object could not be found.-or-The file described by the <paramref name="destinationFileName" /> parameter could not be found. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[ComVisible(false)]
		public FileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
		{
			throw new NotImplementedException();
		}
	}
}
