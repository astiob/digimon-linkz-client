using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace System.IO
{
	/// <summary>Performs operations on <see cref="T:System.String" /> instances that contain file or directory path information. These operations are performed in a cross-platform manner.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	public static class Path
	{
		/// <summary>Provides a platform-specific array of characters that cannot be specified in path string arguments passed to members of the <see cref="T:System.IO.Path" /> class.</summary>
		/// <returns>A character array of invalid path characters for the current platform.</returns>
		/// <filterpriority>1</filterpriority>
		[Obsolete("see GetInvalidPathChars and GetInvalidFileNameChars methods.")]
		public static readonly char[] InvalidPathChars;

		/// <summary>Provides a platform-specific alternate character used to separate directory levels in a path string that reflects a hierarchical file system organization.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly char AltDirectorySeparatorChar;

		/// <summary>Provides a platform-specific character used to separate directory levels in a path string that reflects a hierarchical file system organization.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly char DirectorySeparatorChar;

		/// <summary>A platform-specific separator character used to separate path strings in environment variables.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly char PathSeparator;

		internal static readonly string DirectorySeparatorStr;

		/// <summary>Provides a platform-specific volume separator character.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly char VolumeSeparatorChar = MonoIO.VolumeSeparatorChar;

		internal static readonly char[] PathSeparatorChars;

		private static readonly bool dirEqualsVolume;

		static Path()
		{
			Path.DirectorySeparatorChar = MonoIO.DirectorySeparatorChar;
			Path.AltDirectorySeparatorChar = MonoIO.AltDirectorySeparatorChar;
			Path.PathSeparator = MonoIO.PathSeparator;
			Path.InvalidPathChars = Path.GetInvalidPathChars();
			Path.DirectorySeparatorStr = Path.DirectorySeparatorChar.ToString();
			Path.PathSeparatorChars = new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar,
				Path.VolumeSeparatorChar
			};
			Path.dirEqualsVolume = (Path.DirectorySeparatorChar == Path.VolumeSeparatorChar);
		}

		/// <summary>Changes the extension of a path string.</summary>
		/// <returns>A string containing the modified path information.On Windows-based desktop platforms, if <paramref name="path" /> is null or an empty string (""), the path information is returned unmodified. If <paramref name="extension" /> is null, the returned string contains the specified path with its extension removed. If <paramref name="path" /> has no extension, and <paramref name="extension" /> is not null, the returned path string contains <paramref name="extension" /> appended to the end of <paramref name="path" />.</returns>
		/// <param name="path">The path information to modify. The path cannot contain any of the characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />. </param>
		/// <param name="extension">The new extension (with or without a leading period). Specify null to remove an existing extension from <paramref name="path" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static string ChangeExtension(string path, string extension)
		{
			if (path == null)
			{
				return null;
			}
			if (path.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Illegal characters in path.");
			}
			int num = Path.findExtension(path);
			if (extension == null)
			{
				return (num >= 0) ? path.Substring(0, num) : path;
			}
			if (extension.Length == 0)
			{
				return (num >= 0) ? path.Substring(0, num + 1) : (path + '.');
			}
			if (path.Length != 0)
			{
				if (extension.Length > 0 && extension[0] != '.')
				{
					extension = "." + extension;
				}
			}
			else
			{
				extension = string.Empty;
			}
			if (num < 0)
			{
				return path + extension;
			}
			if (num > 0)
			{
				string str = path.Substring(0, num);
				return str + extension;
			}
			return extension;
		}

		/// <summary>Combines two path strings.</summary>
		/// <returns>A string containing the combined paths. If one of the specified paths is a zero-length string, this method returns the other path. If <paramref name="path2" /> contains an absolute path, this method returns <paramref name="path2" />.</returns>
		/// <param name="path1">The first path. </param>
		/// <param name="path2">The second path. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path1" /> or <paramref name="path2" /> contain one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path1" /> or <paramref name="path2" /> is null. </exception>
		/// <filterpriority>1</filterpriority>
		public static string Combine(string path1, string path2)
		{
			if (path1 == null)
			{
				throw new ArgumentNullException("path1");
			}
			if (path2 == null)
			{
				throw new ArgumentNullException("path2");
			}
			if (path1.Length == 0)
			{
				return path2;
			}
			if (path2.Length == 0)
			{
				return path1;
			}
			if (path1.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Illegal characters in path.");
			}
			if (path2.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Illegal characters in path.");
			}
			if (Path.IsPathRooted(path2))
			{
				return path2;
			}
			char c = path1[path1.Length - 1];
			if (c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar && c != Path.VolumeSeparatorChar)
			{
				return path1 + Path.DirectorySeparatorStr + path2;
			}
			return path1 + path2;
		}

		internal static string CleanPath(string s)
		{
			int length = s.Length;
			int num = 0;
			int num2 = 0;
			char c = s[0];
			if (length > 2 && c == '\\' && s[1] == '\\')
			{
				num2 = 2;
			}
			if (length == 1 && (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar))
			{
				return s;
			}
			for (int i = num2; i < length; i++)
			{
				char c2 = s[i];
				if (c2 == Path.DirectorySeparatorChar || c2 == Path.AltDirectorySeparatorChar)
				{
					if (i + 1 == length)
					{
						num++;
					}
					else
					{
						c2 = s[i + 1];
						if (c2 == Path.DirectorySeparatorChar || c2 == Path.AltDirectorySeparatorChar)
						{
							num++;
						}
					}
				}
			}
			if (num == 0)
			{
				return s;
			}
			char[] array = new char[length - num];
			if (num2 != 0)
			{
				array[0] = '\\';
				array[1] = '\\';
			}
			int j = num2;
			int num3 = num2;
			while (j < length && num3 < array.Length)
			{
				char c3 = s[j];
				if (c3 != Path.DirectorySeparatorChar && c3 != Path.AltDirectorySeparatorChar)
				{
					array[num3++] = c3;
				}
				else if (num3 + 1 != array.Length)
				{
					array[num3++] = Path.DirectorySeparatorChar;
					while (j < length - 1)
					{
						c3 = s[j + 1];
						if (c3 != Path.DirectorySeparatorChar && c3 != Path.AltDirectorySeparatorChar)
						{
							break;
						}
						j++;
					}
				}
				j++;
			}
			return new string(array);
		}

		/// <summary>Returns the directory information for the specified path string.</summary>
		/// <returns>A <see cref="T:System.String" /> containing directory information for <paramref name="path" />, or null if <paramref name="path" /> denotes a root directory or is null. Returns <see cref="F:System.String.Empty" /> if <paramref name="path" /> does not contain directory information.</returns>
		/// <param name="path">The path of a file or directory. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="path" /> parameter contains invalid characters, is empty, or contains only white spaces. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The <paramref name="path" /> parameter is longer than the system-defined maximum length.</exception>
		/// <filterpriority>1</filterpriority>
		public static string GetDirectoryName(string path)
		{
			if (path == string.Empty)
			{
				throw new ArgumentException("Invalid path");
			}
			if (path == null || Path.GetPathRoot(path) == path)
			{
				return null;
			}
			if (path.Trim().Length == 0)
			{
				throw new ArgumentException("Argument string consists of whitespace characters only.");
			}
			if (path.IndexOfAny(Path.InvalidPathChars) > -1)
			{
				throw new ArgumentException("Path contains invalid characters");
			}
			int num = path.LastIndexOfAny(Path.PathSeparatorChars);
			if (num == 0)
			{
				num++;
			}
			if (num <= 0)
			{
				return string.Empty;
			}
			string text = path.Substring(0, num);
			int length = text.Length;
			if (length >= 2 && Path.DirectorySeparatorChar == '\\' && text[length - 1] == Path.VolumeSeparatorChar)
			{
				return text + Path.DirectorySeparatorChar;
			}
			return Path.CleanPath(text);
		}

		/// <summary>Returns the extension of the specified path string.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the extension of the specified path (including the "."), null, or <see cref="F:System.String.Empty" />. If <paramref name="path" /> is null, GetExtension returns null. If <paramref name="path" /> does not have extension information, GetExtension returns Empty.</returns>
		/// <param name="path">The path string from which to get the extension. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.  </exception>
		/// <filterpriority>1</filterpriority>
		public static string GetExtension(string path)
		{
			if (path == null)
			{
				return null;
			}
			if (path.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Illegal characters in path.");
			}
			int num = Path.findExtension(path);
			if (num > -1 && num < path.Length - 1)
			{
				return path.Substring(num);
			}
			return string.Empty;
		}

		/// <summary>Returns the file name and extension of the specified path string.</summary>
		/// <returns>A <see cref="T:System.String" /> consisting of the characters after the last directory character in <paramref name="path" />. If the last character of <paramref name="path" /> is a directory or volume separator character, this method returns <see cref="F:System.String.Empty" />. If <paramref name="path" /> is null, this method returns null.</returns>
		/// <param name="path">The path string from which to obtain the file name and extension. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static string GetFileName(string path)
		{
			if (path == null || path.Length == 0)
			{
				return path;
			}
			if (path.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Illegal characters in path.");
			}
			int num = path.LastIndexOfAny(Path.PathSeparatorChars);
			if (num >= 0)
			{
				return path.Substring(num + 1);
			}
			return path;
		}

		/// <summary>Returns the file name of the specified path string without the extension.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the string returned by <see cref="M:System.IO.Path.GetFileName(System.String)" />, minus the last period (.) and all characters following it.</returns>
		/// <param name="path">The path of the file. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static string GetFileNameWithoutExtension(string path)
		{
			return Path.ChangeExtension(Path.GetFileName(path), null);
		}

		/// <summary>Returns the absolute path for the specified path string.</summary>
		/// <returns>A string containing the fully qualified location of <paramref name="path" />, such as "C:\MyFile.txt".</returns>
		/// <param name="path">The file or directory for which to obtain absolute path information. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.-or- The system could not retrieve the absolute path. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permissions. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> contains a colon (":") that is not part of a volume identifier (for example, "c:\"). </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*" />
		/// </PermissionSet>
		public static string GetFullPath(string path)
		{
			return Path.InsecureGetFullPath(path);
		}

		internal static string WindowsDriveAdjustment(string path)
		{
			if (path.Length < 2)
			{
				return path;
			}
			if (path[1] != ':' || !char.IsLetter(path[0]))
			{
				return path;
			}
			string currentDirectory = Directory.GetCurrentDirectory();
			if (path.Length == 2)
			{
				if (currentDirectory[0] == path[0])
				{
					path = currentDirectory;
				}
				else
				{
					path += '\\';
				}
			}
			else if (path[2] != Path.DirectorySeparatorChar && path[2] != Path.AltDirectorySeparatorChar)
			{
				if (currentDirectory[0] == path[0])
				{
					path = Path.Combine(currentDirectory, path.Substring(2, path.Length - 2));
				}
				else
				{
					path = path.Substring(0, 2) + Path.DirectorySeparatorStr + path.Substring(2, path.Length - 2);
				}
			}
			return path;
		}

		internal static string InsecureGetFullPath(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Trim().Length == 0)
			{
				string text = Locale.GetText("The specified path is not of a legal form (empty).");
				throw new ArgumentException(text);
			}
			if (Environment.IsRunningOnWindows)
			{
				path = Path.WindowsDriveAdjustment(path);
			}
			char c = path[path.Length - 1];
			if (path.Length >= 2 && Path.IsDsc(path[0]) && Path.IsDsc(path[1]))
			{
				if (path.Length == 2 || path.IndexOf(path[0], 2) < 0)
				{
					throw new ArgumentException("UNC paths should be of the form \\\\server\\share.");
				}
				if (path[0] != Path.DirectorySeparatorChar)
				{
					path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
				}
				path = Path.CanonicalizePath(path);
			}
			else
			{
				if (!Path.IsPathRooted(path))
				{
					path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorStr + path;
				}
				else if (Path.DirectorySeparatorChar == '\\' && path.Length >= 2 && Path.IsDsc(path[0]) && !Path.IsDsc(path[1]))
				{
					string currentDirectory = Directory.GetCurrentDirectory();
					if (currentDirectory[1] == Path.VolumeSeparatorChar)
					{
						path = currentDirectory.Substring(0, 2) + path;
					}
					else
					{
						path = currentDirectory.Substring(0, currentDirectory.IndexOf('\\', currentDirectory.IndexOf("\\\\") + 1));
					}
				}
				path = Path.CanonicalizePath(path);
			}
			if (Path.IsDsc(c) && path[path.Length - 1] != Path.DirectorySeparatorChar)
			{
				path += Path.DirectorySeparatorChar;
			}
			return path;
		}

		private static bool IsDsc(char c)
		{
			return c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar;
		}

		/// <summary>Gets the root directory information of the specified path.</summary>
		/// <returns>A string containing the root directory of <paramref name="path" />, such as "C:\", or null if <paramref name="path" /> is null, or an empty string if <paramref name="path" /> does not contain root directory information.</returns>
		/// <param name="path">The path from which to obtain root directory information. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.-or- <see cref="F:System.String.Empty" /> was passed to <paramref name="path" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static string GetPathRoot(string path)
		{
			if (path == null)
			{
				return null;
			}
			if (path.Trim().Length == 0)
			{
				throw new ArgumentException("The specified path is not of a legal form.");
			}
			if (!Path.IsPathRooted(path))
			{
				return string.Empty;
			}
			if (Path.DirectorySeparatorChar == '/')
			{
				return (!Path.IsDsc(path[0])) ? string.Empty : Path.DirectorySeparatorStr;
			}
			int num = 2;
			if (path.Length == 1 && Path.IsDsc(path[0]))
			{
				return Path.DirectorySeparatorStr;
			}
			if (path.Length < 2)
			{
				return string.Empty;
			}
			if (Path.IsDsc(path[0]) && Path.IsDsc(path[1]))
			{
				while (num < path.Length && !Path.IsDsc(path[num]))
				{
					num++;
				}
				if (num < path.Length)
				{
					num++;
					while (num < path.Length && !Path.IsDsc(path[num]))
					{
						num++;
					}
				}
				return Path.DirectorySeparatorStr + Path.DirectorySeparatorStr + path.Substring(2, num - 2).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			}
			if (Path.IsDsc(path[0]))
			{
				return Path.DirectorySeparatorStr;
			}
			if (path[1] == Path.VolumeSeparatorChar)
			{
				if (path.Length >= 3 && Path.IsDsc(path[2]))
				{
					num++;
				}
				return path.Substring(0, num);
			}
			return Directory.GetCurrentDirectory().Substring(0, 2);
		}

		/// <summary>Creates a uniquely named, zero-byte temporary file on disk and returns the full path of that file.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the full path of the temporary file.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs, such as no unique temporary file name is available.- or -This method was unable to create a temporary file.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static string GetTempFileName()
		{
			FileStream fileStream = null;
			Random random = new Random();
			string text;
			do
			{
				int num = random.Next();
				num++;
				text = Path.Combine(Path.GetTempPath(), "tmp" + num.ToString("x") + ".tmp");
				try
				{
					fileStream = new FileStream(text, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read, 8192, false, (FileOptions)1);
				}
				catch (SecurityException)
				{
					throw;
				}
				catch (UnauthorizedAccessException)
				{
					throw;
				}
				catch (DirectoryNotFoundException)
				{
					throw;
				}
				catch
				{
				}
			}
			while (fileStream == null);
			fileStream.Close();
			return text;
		}

		/// <summary>Returns the path of the current system's temporary folder.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the path information of a temporary directory.</returns>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permissions. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static string GetTempPath()
		{
			string temp_path = Path.get_temp_path();
			if (temp_path.Length > 0 && temp_path[temp_path.Length - 1] != Path.DirectorySeparatorChar)
			{
				return temp_path + Path.DirectorySeparatorChar;
			}
			return temp_path;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string get_temp_path();

		/// <summary>Determines whether a path includes a file name extension.</summary>
		/// <returns>true if the characters that follow the last directory separator (\\ or /) or volume separator (:) in the path include a period (.) followed by one or more characters; otherwise, false.</returns>
		/// <param name="path">The path to search for an extension. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool HasExtension(string path)
		{
			if (path == null || path.Trim().Length == 0)
			{
				return false;
			}
			if (path.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Illegal characters in path.");
			}
			int num = Path.findExtension(path);
			return 0 <= num && num < path.Length - 1;
		}

		/// <summary>Gets a value indicating whether the specified path string contains absolute or relative path information.</summary>
		/// <returns>true if <paramref name="path" /> contains an absolute path; otherwise, false.</returns>
		/// <param name="path">The path to test. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsPathRooted(string path)
		{
			if (path == null || path.Length == 0)
			{
				return false;
			}
			if (path.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Illegal characters in path.");
			}
			char c = path[0];
			return c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || (!Path.dirEqualsVolume && path.Length > 1 && path[1] == Path.VolumeSeparatorChar);
		}

		/// <summary>Gets an array containing the characters that are not allowed in file names.</summary>
		/// <returns>An array containing the characters that are not allowed in file names.</returns>
		public static char[] GetInvalidFileNameChars()
		{
			if (Environment.IsRunningOnWindows)
			{
				return new char[]
				{
					'\0',
					'\u0001',
					'\u0002',
					'\u0003',
					'\u0004',
					'\u0005',
					'\u0006',
					'\a',
					'\b',
					'\t',
					'\n',
					'\v',
					'\f',
					'\r',
					'\u000e',
					'\u000f',
					'\u0010',
					'\u0011',
					'\u0012',
					'\u0013',
					'\u0014',
					'\u0015',
					'\u0016',
					'\u0017',
					'\u0018',
					'\u0019',
					'\u001a',
					'\u001b',
					'\u001c',
					'\u001d',
					'\u001e',
					'\u001f',
					'"',
					'<',
					'>',
					'|',
					':',
					'*',
					'?',
					'\\',
					'/'
				};
			}
			return new char[]
			{
				'\0',
				'/'
			};
		}

		/// <summary>Gets an array containing the characters that are not allowed in path names.</summary>
		/// <returns>An array containing the characters that are not allowed in path names.</returns>
		public static char[] GetInvalidPathChars()
		{
			if (Environment.IsRunningOnWindows)
			{
				return new char[]
				{
					'"',
					'<',
					'>',
					'|',
					'\0',
					'\u0001',
					'\u0002',
					'\u0003',
					'\u0004',
					'\u0005',
					'\u0006',
					'\a',
					'\b',
					'\t',
					'\n',
					'\v',
					'\f',
					'\r',
					'\u000e',
					'\u000f',
					'\u0010',
					'\u0011',
					'\u0012',
					'\u0013',
					'\u0014',
					'\u0015',
					'\u0016',
					'\u0017',
					'\u0018',
					'\u0019',
					'\u001a',
					'\u001b',
					'\u001c',
					'\u001d',
					'\u001e',
					'\u001f'
				};
			}
			return new char[1];
		}

		/// <summary>Returns a random folder name or file name.</summary>
		/// <returns>A random folder name or file name.</returns>
		public static string GetRandomFileName()
		{
			StringBuilder stringBuilder = new StringBuilder(12);
			RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
			byte[] array = new byte[11];
			randomNumberGenerator.GetBytes(array);
			for (int i = 0; i < array.Length; i++)
			{
				if (stringBuilder.Length == 8)
				{
					stringBuilder.Append('.');
				}
				int num = (int)(array[i] % 36);
				char value = (char)((num >= 26) ? (num - 26 + 48) : (num + 97));
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}

		private static int findExtension(string path)
		{
			if (path != null)
			{
				int num = path.LastIndexOf('.');
				int num2 = path.LastIndexOfAny(Path.PathSeparatorChars);
				if (num > num2)
				{
					return num;
				}
			}
			return -1;
		}

		private static string GetServerAndShare(string path)
		{
			int num = 2;
			while (num < path.Length && !Path.IsDsc(path[num]))
			{
				num++;
			}
			if (num < path.Length)
			{
				num++;
				while (num < path.Length && !Path.IsDsc(path[num]))
				{
					num++;
				}
			}
			return path.Substring(2, num - 2).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
		}

		private static bool SameRoot(string root, string path)
		{
			if (root.Length < 2 || path.Length < 2)
			{
				return false;
			}
			if (!Path.IsDsc(root[0]) || !Path.IsDsc(root[1]))
			{
				return root[0].Equals(path[0]) && path[1] == Path.VolumeSeparatorChar && (root.Length <= 2 || path.Length <= 2 || (Path.IsDsc(root[2]) && Path.IsDsc(path[2])));
			}
			if (!Path.IsDsc(path[0]) || !Path.IsDsc(path[1]))
			{
				return false;
			}
			string serverAndShare = Path.GetServerAndShare(root);
			string serverAndShare2 = Path.GetServerAndShare(path);
			return string.Compare(serverAndShare, serverAndShare2, true, CultureInfo.InvariantCulture) == 0;
		}

		private static string CanonicalizePath(string path)
		{
			if (path == null)
			{
				return path;
			}
			if (Environment.IsRunningOnWindows)
			{
				path = path.Trim();
			}
			if (path.Length == 0)
			{
				return path;
			}
			string pathRoot = Path.GetPathRoot(path);
			string[] array = path.Split(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			});
			int num = 0;
			bool flag = Environment.IsRunningOnWindows && pathRoot.Length > 2 && Path.IsDsc(pathRoot[0]) && Path.IsDsc(pathRoot[1]);
			int num2 = (!flag) ? 0 : 3;
			for (int i = 0; i < array.Length; i++)
			{
				if (Environment.IsRunningOnWindows)
				{
					array[i] = array[i].TrimEnd(new char[0]);
				}
				if (!(array[i] == ".") && (i == 0 || array[i].Length != 0))
				{
					if (array[i] == "..")
					{
						if (num > num2)
						{
							num--;
						}
					}
					else
					{
						array[num++] = array[i];
					}
				}
			}
			if (num == 0 || (num == 1 && array[0] == string.Empty))
			{
				return pathRoot;
			}
			string text = string.Join(Path.DirectorySeparatorStr, array, 0, num);
			if (!Environment.IsRunningOnWindows)
			{
				return text;
			}
			if (flag)
			{
				text = Path.DirectorySeparatorStr + text;
			}
			if (!Path.SameRoot(pathRoot, text))
			{
				text = pathRoot + text;
			}
			if (flag)
			{
				return text;
			}
			if (!Path.IsDsc(path[0]) && Path.SameRoot(pathRoot, path))
			{
				if (text.Length <= 2 && !text.EndsWith(Path.DirectorySeparatorStr))
				{
					text += Path.DirectorySeparatorChar;
				}
				return text;
			}
			string currentDirectory = Directory.GetCurrentDirectory();
			if (currentDirectory.Length > 1 && currentDirectory[1] == Path.VolumeSeparatorChar)
			{
				if (text.Length == 0 || Path.IsDsc(text[0]))
				{
					text += '\\';
				}
				return currentDirectory.Substring(0, 2) + text;
			}
			if (Path.IsDsc(currentDirectory[currentDirectory.Length - 1]) && Path.IsDsc(text[0]))
			{
				return currentDirectory + text.Substring(1);
			}
			return currentDirectory + text;
		}

		internal static bool IsPathSubsetOf(string subset, string path)
		{
			if (subset.Length > path.Length)
			{
				return false;
			}
			int num = subset.LastIndexOfAny(Path.PathSeparatorChars);
			if (string.Compare(subset, 0, path, 0, num) != 0)
			{
				return false;
			}
			num++;
			int num2 = path.IndexOfAny(Path.PathSeparatorChars, num);
			if (num2 >= num)
			{
				return string.Compare(subset, num, path, num, path.Length - num2) == 0;
			}
			return subset.Length == path.Length && string.Compare(subset, num, path, num, subset.Length - num) == 0;
		}
	}
}
