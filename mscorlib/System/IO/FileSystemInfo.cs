using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO
{
	/// <summary>Provides the base class for both <see cref="T:System.IO.FileInfo" /> and <see cref="T:System.IO.DirectoryInfo" /> objects.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public abstract class FileSystemInfo : MarshalByRefObject, ISerializable
	{
		/// <summary>Represents the fully qualified path of the directory or file.</summary>
		/// <exception cref="T:System.IO.PathTooLongException">The fully qualified path is 260 or more characters.</exception>
		protected string FullPath;

		/// <summary>The path originally specified by the user, whether relative or absolute.</summary>
		protected string OriginalPath;

		internal MonoIOStat stat;

		internal bool valid;

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.FileSystemInfo" /> class.</summary>
		protected FileSystemInfo()
		{
			this.valid = false;
			this.FullPath = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.FileSystemInfo" /> class with serialized data.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
		/// <exception cref="T:System.ArgumentNullException">The specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> is null.</exception>
		protected FileSystemInfo(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.FullPath = info.GetString("FullPath");
			this.OriginalPath = info.GetString("OriginalPath");
		}

		/// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the file name and additional exception information.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
		/// </PermissionSet>
		[ComVisible(false)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("OriginalPath", this.OriginalPath, typeof(string));
			info.AddValue("FullPath", this.FullPath, typeof(string));
		}

		/// <summary>Gets a value indicating whether the file or directory exists.</summary>
		/// <returns>true if the file or directory exists; otherwise, false.</returns>
		/// <filterpriority>1</filterpriority>
		public abstract bool Exists { get; }

		/// <summary>For files, gets the name of the file. For directories, gets the name of the last directory in the hierarchy if a hierarchy exists. Otherwise, the Name property gets the name of the directory.</summary>
		/// <returns>A string that is the name of the parent directory, the name of the last directory in the hierarchy, or the name of a file, including the file name extension.</returns>
		/// <filterpriority>1</filterpriority>
		public abstract string Name { get; }

		/// <summary>Gets the full path of the directory or file.</summary>
		/// <returns>A string containing the full path.</returns>
		/// <exception cref="T:System.IO.PathTooLongException">The fully qualified path and file name is 260 or more characters.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public virtual string FullName
		{
			get
			{
				return this.FullPath;
			}
		}

		/// <summary>Gets the string representing the extension part of the file.</summary>
		/// <returns>A string containing the <see cref="T:System.IO.FileSystemInfo" /> extension.</returns>
		/// <filterpriority>1</filterpriority>
		public string Extension
		{
			get
			{
				return Path.GetExtension(this.Name);
			}
		}

		/// <summary>Gets or sets the current directory or file.</summary>
		/// <returns>
		///   <see cref="T:System.IO.FileAttributes" /> of the current <see cref="T:System.IO.FileSystemInfo" />.</returns>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified file does not exist. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">The caller attempts to set an invalid file attribute. </exception>
		/// <exception cref="T:System.IO.IOException">
		///   <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot initialize the data. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public FileAttributes Attributes
		{
			get
			{
				this.Refresh(false);
				return this.stat.Attributes;
			}
			set
			{
				MonoIOError error;
				if (!MonoIO.SetFileAttributes(this.FullName, value, out error))
				{
					throw MonoIO.GetException(this.FullName, error);
				}
				this.Refresh(true);
			}
		}

		/// <summary>Gets or sets the creation time of the current directory or file.</summary>
		/// <returns>The creation date and time of the current <see cref="T:System.IO.FileSystemInfo" /> object.</returns>
		/// <exception cref="T:System.IO.IOException">
		///   <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot initialize the data. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public DateTime CreationTime
		{
			get
			{
				this.Refresh(false);
				return DateTime.FromFileTime(this.stat.CreationTime);
			}
			set
			{
				long creation_time = value.ToFileTime();
				MonoIOError error;
				if (!MonoIO.SetFileTime(this.FullName, creation_time, -1L, -1L, out error))
				{
					throw MonoIO.GetException(this.FullName, error);
				}
				this.Refresh(true);
			}
		}

		/// <summary>Gets or sets the creation time, in coordinated universal time (UTC), of the current directory or file.</summary>
		/// <returns>The creation date and time in UTC format of the current <see cref="T:System.IO.FileSystemInfo" /> object.</returns>
		/// <exception cref="T:System.IO.IOException">
		///   <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot initialize the data. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[ComVisible(false)]
		public DateTime CreationTimeUtc
		{
			get
			{
				return this.CreationTime.ToUniversalTime();
			}
			set
			{
				this.CreationTime = value.ToLocalTime();
			}
		}

		/// <summary>Gets or sets the time the current file or directory was last accessed.</summary>
		/// <returns>The time that the current file or directory was last accessed.</returns>
		/// <exception cref="T:System.IO.IOException">
		///   <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot initialize the data. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public DateTime LastAccessTime
		{
			get
			{
				this.Refresh(false);
				return DateTime.FromFileTime(this.stat.LastAccessTime);
			}
			set
			{
				long last_access_time = value.ToFileTime();
				MonoIOError error;
				if (!MonoIO.SetFileTime(this.FullName, -1L, last_access_time, -1L, out error))
				{
					throw MonoIO.GetException(this.FullName, error);
				}
				this.Refresh(true);
			}
		}

		/// <summary>Gets or sets the time, in coordinated universal time (UTC), that the current file or directory was last accessed.</summary>
		/// <returns>The UTC time that the current file or directory was last accessed.</returns>
		/// <exception cref="T:System.IO.IOException">
		///   <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot initialize the data. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[ComVisible(false)]
		public DateTime LastAccessTimeUtc
		{
			get
			{
				this.Refresh(false);
				return this.LastAccessTime.ToUniversalTime();
			}
			set
			{
				this.LastAccessTime = value.ToLocalTime();
			}
		}

		/// <summary>Gets or sets the time when the current file or directory was last written to.</summary>
		/// <returns>The time the current file was last written.</returns>
		/// <exception cref="T:System.IO.IOException">
		///   <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot initialize the data. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public DateTime LastWriteTime
		{
			get
			{
				this.Refresh(false);
				return DateTime.FromFileTime(this.stat.LastWriteTime);
			}
			set
			{
				long last_write_time = value.ToFileTime();
				MonoIOError error;
				if (!MonoIO.SetFileTime(this.FullName, -1L, -1L, last_write_time, out error))
				{
					throw MonoIO.GetException(this.FullName, error);
				}
				this.Refresh(true);
			}
		}

		/// <summary>Gets or sets the time, in coordinated universal time (UTC), when the current file or directory was last written to.</summary>
		/// <returns>The UTC time when the current file was last written to.</returns>
		/// <exception cref="T:System.IO.IOException">
		///   <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot initialize the data. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[ComVisible(false)]
		public DateTime LastWriteTimeUtc
		{
			get
			{
				this.Refresh(false);
				return this.LastWriteTime.ToUniversalTime();
			}
			set
			{
				this.LastWriteTime = value.ToLocalTime();
			}
		}

		/// <summary>Deletes a file or directory.</summary>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid; for example, it is on an unmapped drive. </exception>
		/// <filterpriority>2</filterpriority>
		public abstract void Delete();

		/// <summary>Refreshes the state of the object.</summary>
		/// <exception cref="T:System.IO.IOException">A device such as a disk drive is not ready. </exception>
		/// <filterpriority>1</filterpriority>
		public void Refresh()
		{
			this.Refresh(true);
		}

		internal void Refresh(bool force)
		{
			if (this.valid && !force)
			{
				return;
			}
			MonoIOError monoIOError;
			MonoIO.GetFileStat(this.FullName, out this.stat, out monoIOError);
			this.valid = true;
			this.InternalRefresh();
		}

		internal virtual void InternalRefresh()
		{
		}

		internal void CheckPath(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("An empty file name is not valid.");
			}
			if (path.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Illegal characters in path.");
			}
		}
	}
}
