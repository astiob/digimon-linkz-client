using System;
using System.IO;
using System.Text;

namespace System
{
	/// <summary>Represents the standard input, output, and error streams for console applications. This class cannot be inherited.</summary>
	/// <filterpriority>1</filterpriority>
	public static class Console
	{
		internal static TextWriter stdout;

		private static TextWriter stderr;

		private static TextReader stdin;

		private static Encoding inputEncoding;

		private static Encoding outputEncoding;

		static Console()
		{
			if (Environment.IsRunningOnWindows)
			{
				Console.inputEncoding = (Console.outputEncoding = Encoding.Default);
			}
			else
			{
				int num = 0;
				Encoding.InternalCodePage(ref num);
				if (num != -1 && ((num & 268435455) == 3 || (num & 268435456) != 0))
				{
					Console.inputEncoding = (Console.outputEncoding = Encoding.UTF8Unmarked);
				}
				else
				{
					Console.inputEncoding = (Console.outputEncoding = Encoding.Default);
				}
			}
			Console.SetEncodings(Console.inputEncoding, Console.outputEncoding);
		}

		private static void SetEncodings(Encoding inputEncoding, Encoding outputEncoding)
		{
			Console.stderr = new UnexceptionalStreamWriter(Console.OpenStandardError(0), outputEncoding);
			((StreamWriter)Console.stderr).AutoFlush = true;
			Console.stderr = TextWriter.Synchronized(Console.stderr, true);
			Console.stdout = new UnexceptionalStreamWriter(Console.OpenStandardOutput(0), outputEncoding);
			((StreamWriter)Console.stdout).AutoFlush = true;
			Console.stdout = TextWriter.Synchronized(Console.stdout, true);
			Console.stdin = new UnexceptionalStreamReader(Console.OpenStandardInput(0), inputEncoding);
			Console.stdin = TextReader.Synchronized(Console.stdin);
			GC.SuppressFinalize(Console.stdout);
			GC.SuppressFinalize(Console.stderr);
			GC.SuppressFinalize(Console.stdin);
		}

		/// <summary>Gets the standard error output stream.</summary>
		/// <returns>A <see cref="T:System.IO.TextWriter" /> that represents the standard error output stream.</returns>
		/// <filterpriority>1</filterpriority>
		public static TextWriter Error
		{
			get
			{
				return Console.stderr;
			}
		}

		/// <summary>Gets the standard output stream.</summary>
		/// <returns>A <see cref="T:System.IO.TextWriter" /> that represents the standard output stream.</returns>
		/// <filterpriority>1</filterpriority>
		public static TextWriter Out
		{
			get
			{
				return Console.stdout;
			}
		}

		/// <summary>Gets the standard input stream.</summary>
		/// <returns>A <see cref="T:System.IO.TextReader" /> that represents the standard input stream.</returns>
		/// <filterpriority>1</filterpriority>
		public static TextReader In
		{
			get
			{
				return Console.stdin;
			}
		}

		/// <summary>Acquires the standard error stream.</summary>
		/// <returns>The standard error stream.</returns>
		/// <filterpriority>1</filterpriority>
		public static Stream OpenStandardError()
		{
			return Console.OpenStandardError(0);
		}

		private static Stream Open(IntPtr handle, FileAccess access, int bufferSize)
		{
			Stream result;
			try
			{
				result = new FileStream(handle, access, false, bufferSize, false, bufferSize == 0);
			}
			catch (IOException)
			{
				result = new NullStream();
			}
			return result;
		}

		/// <summary>Acquires the standard error stream, which is set to a specified buffer size.</summary>
		/// <returns>The standard error stream.</returns>
		/// <param name="bufferSize">The internal stream buffer size. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is less than or equal to zero. </exception>
		/// <filterpriority>1</filterpriority>
		public static Stream OpenStandardError(int bufferSize)
		{
			return Console.Open(MonoIO.ConsoleError, FileAccess.Write, bufferSize);
		}

		/// <summary>Acquires the standard input stream.</summary>
		/// <returns>The standard input stream.</returns>
		/// <filterpriority>1</filterpriority>
		public static Stream OpenStandardInput()
		{
			return Console.OpenStandardInput(0);
		}

		/// <summary>Acquires the standard input stream, which is set to a specified buffer size.</summary>
		/// <returns>The standard input stream.</returns>
		/// <param name="bufferSize">The internal stream buffer size. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is less than or equal to zero. </exception>
		/// <filterpriority>1</filterpriority>
		public static Stream OpenStandardInput(int bufferSize)
		{
			return Console.Open(MonoIO.ConsoleInput, FileAccess.Read, bufferSize);
		}

		/// <summary>Acquires the standard output stream.</summary>
		/// <returns>The standard output stream.</returns>
		/// <filterpriority>1</filterpriority>
		public static Stream OpenStandardOutput()
		{
			return Console.OpenStandardOutput(0);
		}

		/// <summary>Acquires the standard output stream, which is set to a specified buffer size.</summary>
		/// <returns>The standard output stream.</returns>
		/// <param name="bufferSize">The internal stream buffer size. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is less than or equal to zero. </exception>
		/// <filterpriority>1</filterpriority>
		public static Stream OpenStandardOutput(int bufferSize)
		{
			return Console.Open(MonoIO.ConsoleOutput, FileAccess.Write, bufferSize);
		}

		/// <summary>Sets the <see cref="P:System.Console.Error" /> property to the specified <see cref="T:System.IO.TextWriter" /> object.</summary>
		/// <param name="newError">A <see cref="T:System.IO.TextWriter" /> stream that is the new standard error output. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="newError" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static void SetError(TextWriter newError)
		{
			if (newError == null)
			{
				throw new ArgumentNullException("newError");
			}
			Console.stderr = newError;
		}

		/// <summary>Sets the <see cref="P:System.Console.In" /> property to the specified <see cref="T:System.IO.TextReader" /> object.</summary>
		/// <param name="newIn">A <see cref="T:System.IO.TextReader" /> stream that is the new standard input. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="newIn" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static void SetIn(TextReader newIn)
		{
			if (newIn == null)
			{
				throw new ArgumentNullException("newIn");
			}
			Console.stdin = newIn;
		}

		/// <summary>Sets the <see cref="P:System.Console.Out" /> property to the specified <see cref="T:System.IO.TextWriter" /> object.</summary>
		/// <param name="newOut">A <see cref="T:System.IO.TextWriter" /> stream that is the new standard output. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="newOut" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static void SetOut(TextWriter newOut)
		{
			if (newOut == null)
			{
				throw new ArgumentNullException("newOut");
			}
			Console.stdout = newOut;
		}

		/// <summary>Writes the text representation of the specified Boolean value to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(bool value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the specified Unicode character value to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(char value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the specified array of Unicode characters to the standard output stream.</summary>
		/// <param name="buffer">A Unicode character array. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(char[] buffer)
		{
			Console.stdout.Write(buffer);
		}

		/// <summary>Writes the text representation of the specified <see cref="T:System.Decimal" /> value to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(decimal value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified double-precision floating-point value to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(double value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified 32-bit signed integer value to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(int value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified 64-bit signed integer value to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(long value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified object to the standard output stream.</summary>
		/// <param name="value">The value to write, or null. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(object value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified single-precision floating-point value to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(float value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the specified string value to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(string value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified 32-bit unsigned integer value to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static void Write(uint value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified 64-bit unsigned integer value to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static void Write(ulong value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified object to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string (see Remarks). </param>
		/// <param name="arg0">An object to write using <paramref name="format" />. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(string format, object arg0)
		{
			Console.stdout.Write(format, arg0);
		}

		/// <summary>Writes the text representation of the specified array of objects to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string (see Remarks). </param>
		/// <param name="arg">An array of objects to write using <paramref name="format" />. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> or <paramref name="arg" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(string format, params object[] arg)
		{
			Console.stdout.Write(format, arg);
		}

		/// <summary>Writes the specified subarray of Unicode characters to the standard output stream.</summary>
		/// <param name="buffer">An array of Unicode characters. </param>
		/// <param name="index">The starting position in <paramref name="buffer" />. </param>
		/// <param name="count">The number of characters to write. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> plus <paramref name="count" /> specify a position that is not within <paramref name="buffer" />. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(char[] buffer, int index, int count)
		{
			Console.stdout.Write(buffer, index, count);
		}

		/// <summary>Writes the text representation of the specified objects to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string (see Remarks). </param>
		/// <param name="arg0">The first object to write using <paramref name="format" />. </param>
		/// <param name="arg1">The second object to write using <paramref name="format" />. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(string format, object arg0, object arg1)
		{
			Console.stdout.Write(format, arg0, arg1);
		}

		/// <summary>Writes the text representation of the specified objects to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string (see Remarks). </param>
		/// <param name="arg0">The first object to write using <paramref name="format" />. </param>
		/// <param name="arg1">The second object to write using <paramref name="format" />. </param>
		/// <param name="arg2">The third object to write using <paramref name="format" />. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Write(string format, object arg0, object arg1, object arg2)
		{
			Console.stdout.Write(format, arg0, arg1, arg2);
		}

		/// <summary>Writes the text representation of the specified objects and variable-length parameter list to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string (see Remarks). </param>
		/// <param name="arg0">The first object to write using <paramref name="format" />. </param>
		/// <param name="arg1">The second object to write using <paramref name="format" />. </param>
		/// <param name="arg2">The third object to write using <paramref name="format" />. </param>
		/// <param name="arg3">The fourth object to write using <paramref name="format" />. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static void Write(string format, object arg0, object arg1, object arg2, object arg3, __arglist)
		{
			ArgIterator argIterator = new ArgIterator(__arglist);
			int remainingCount = argIterator.GetRemainingCount();
			object[] array = new object[remainingCount + 4];
			array[0] = arg0;
			array[1] = arg1;
			array[2] = arg2;
			array[3] = arg3;
			for (int i = 0; i < remainingCount; i++)
			{
				TypedReference nextArg = argIterator.GetNextArg();
				array[i + 4] = TypedReference.ToObject(nextArg);
			}
			Console.stdout.Write(string.Format(format, array));
		}

		/// <summary>Writes the current line terminator to the standard output stream.</summary>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine()
		{
			Console.stdout.WriteLine();
		}

		/// <summary>Writes the text representation of the specified Boolean value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(bool value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the specified Unicode character, followed by the current line terminator, value to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(char value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the specified array of Unicode characters, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="buffer">A Unicode character array. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(char[] buffer)
		{
			Console.stdout.WriteLine(buffer);
		}

		/// <summary>Writes the text representation of the specified <see cref="T:System.Decimal" /> value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(decimal value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified double-precision floating-point value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(double value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified 32-bit signed integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(int value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified 64-bit signed integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(long value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(object value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified single-precision floating-point value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(float value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the specified string value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(string value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified 32-bit unsigned integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static void WriteLine(uint value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified 64-bit unsigned integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static void WriteLine(ulong value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string (see Remarks). </param>
		/// <param name="arg0">An object to write using <paramref name="format" />. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(string format, object arg0)
		{
			Console.stdout.WriteLine(format, arg0);
		}

		/// <summary>Writes the text representation of the specified array of objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string (see Remarks). </param>
		/// <param name="arg">An array of objects to write using <paramref name="format" />. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> or <paramref name="arg" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(string format, params object[] arg)
		{
			Console.stdout.WriteLine(format, arg);
		}

		/// <summary>Writes the specified subarray of Unicode characters, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="buffer">An array of Unicode characters. </param>
		/// <param name="index">The starting position in <paramref name="buffer" />. </param>
		/// <param name="count">The number of characters to write. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> plus <paramref name="count" /> specify a position that is not within <paramref name="buffer" />. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(char[] buffer, int index, int count)
		{
			Console.stdout.WriteLine(buffer, index, count);
		}

		/// <summary>Writes the text representation of the specified objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string (see Remarks). </param>
		/// <param name="arg0">The first object to write using <paramref name="format" />. </param>
		/// <param name="arg1">The second object to write using <paramref name="format" />. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(string format, object arg0, object arg1)
		{
			Console.stdout.WriteLine(format, arg0, arg1);
		}

		/// <summary>Writes the text representation of the specified objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string (see Remarks). </param>
		/// <param name="arg0">The first object to write using <paramref name="format" />. </param>
		/// <param name="arg1">The second object to write using <paramref name="format" />. </param>
		/// <param name="arg2">The third object to write using <paramref name="format" />. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public static void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			Console.stdout.WriteLine(format, arg0, arg1, arg2);
		}

		/// <summary>Writes the text representation of the specified objects and variable-length parameter list, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string (see Remarks). </param>
		/// <param name="arg0">The first object to write using <paramref name="format" />. </param>
		/// <param name="arg1">The second object to write using <paramref name="format" />. </param>
		/// <param name="arg2">The third object to write using <paramref name="format" />. </param>
		/// <param name="arg3">The fourth object to write using <paramref name="format" />. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static void WriteLine(string format, object arg0, object arg1, object arg2, object arg3, __arglist)
		{
			ArgIterator argIterator = new ArgIterator(__arglist);
			int remainingCount = argIterator.GetRemainingCount();
			object[] array = new object[remainingCount + 4];
			array[0] = arg0;
			array[1] = arg1;
			array[2] = arg2;
			array[3] = arg3;
			for (int i = 0; i < remainingCount; i++)
			{
				TypedReference nextArg = argIterator.GetNextArg();
				array[i + 4] = TypedReference.ToObject(nextArg);
			}
			Console.stdout.WriteLine(string.Format(format, array));
		}

		/// <summary>Reads the next character from the standard input stream.</summary>
		/// <returns>The next character from the input stream, or negative one (-1) if there are currently no more characters to be read.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <filterpriority>1</filterpriority>
		public static int Read()
		{
			return Console.stdin.Read();
		}

		/// <summary>Reads the next line of characters from the standard input stream.</summary>
		/// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters in the next line of characters is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static string ReadLine()
		{
			return Console.stdin.ReadLine();
		}

		/// <summary>Gets or sets the encoding the console uses to read input. </summary>
		/// <returns>The encoding used to read console input.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property value in a set operation is null.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">This property's set operation is not supported on Windows 98, Windows 98 Second Edition, or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred during the execution of this operation.</exception>
		/// <exception cref="T:System.Security.SecurityException">Your application does not have permission to perform this operation.</exception>
		/// <filterpriority>1</filterpriority>
		public static Encoding InputEncoding
		{
			get
			{
				return Console.inputEncoding;
			}
			set
			{
				Console.inputEncoding = value;
				Console.SetEncodings(Console.inputEncoding, Console.outputEncoding);
			}
		}

		/// <summary>Gets or sets the encoding the console uses to write output. </summary>
		/// <returns>The encoding used to write console output.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property value in a set operation is null.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">This property's set operation is not supported on Windows 98, Windows 98 Second Edition, or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred during the execution of this operation.</exception>
		/// <exception cref="T:System.Security.SecurityException">Your application does not have permission to perform this operation.</exception>
		/// <filterpriority>1</filterpriority>
		public static Encoding OutputEncoding
		{
			get
			{
				return Console.outputEncoding;
			}
			set
			{
				Console.outputEncoding = value;
				Console.SetEncodings(Console.inputEncoding, Console.outputEncoding);
			}
		}
	}
}
