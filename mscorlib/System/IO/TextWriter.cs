using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO
{
	/// <summary>Represents a writer that can write a sequential series of characters. This class is abstract.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public abstract class TextWriter : IDisposable
	{
		/// <summary>Stores the new line characters used for this TextWriter.</summary>
		protected char[] CoreNewLine;

		internal IFormatProvider internalFormatProvider;

		/// <summary>Provides a TextWriter with no backing store that can be written to, but not read from.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly TextWriter Null = new TextWriter.NullTextWriter();

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.TextWriter" /> class.</summary>
		protected TextWriter()
		{
			this.CoreNewLine = Environment.NewLine.ToCharArray();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.TextWriter" /> class with the specified format provider.</summary>
		/// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that controls formatting. </param>
		protected TextWriter(IFormatProvider formatProvider)
		{
			this.CoreNewLine = Environment.NewLine.ToCharArray();
			this.internalFormatProvider = formatProvider;
		}

		/// <summary>When overridden in a derived class, returns the <see cref="T:System.Text.Encoding" /> in which the output is written.</summary>
		/// <returns>The Encoding in which the output is written.</returns>
		/// <filterpriority>1</filterpriority>
		public abstract Encoding Encoding { get; }

		/// <summary>Gets an object that controls formatting.</summary>
		/// <returns>An <see cref="T:System.IFormatProvider" /> object for a specific culture, or the formatting of the current culture if no other culture is specified.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual IFormatProvider FormatProvider
		{
			get
			{
				return this.internalFormatProvider;
			}
		}

		/// <summary>Gets or sets the line terminator string used by the current TextWriter.</summary>
		/// <returns>The line terminator string for the current TextWriter.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual string NewLine
		{
			get
			{
				return new string(this.CoreNewLine);
			}
			set
			{
				if (value == null)
				{
					value = Environment.NewLine;
				}
				this.CoreNewLine = value.ToCharArray();
			}
		}

		/// <summary>Closes the current writer and releases any system resources associated with the writer.</summary>
		/// <filterpriority>1</filterpriority>
		public virtual void Close()
		{
			this.Dispose(true);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.TextWriter" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.IO.TextWriter" /> object.</summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Clears all buffers for the current writer and causes any buffered data to be written to the underlying device.</summary>
		/// <filterpriority>1</filterpriority>
		public virtual void Flush()
		{
		}

		/// <summary>Creates a thread-safe wrapper around the specified TextWriter.</summary>
		/// <returns>A thread-safe wrapper.</returns>
		/// <param name="writer">The TextWriter to synchronize. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="writer" /> is null. </exception>
		/// <filterpriority>2</filterpriority>
		public static TextWriter Synchronized(TextWriter writer)
		{
			return TextWriter.Synchronized(writer, false);
		}

		internal static TextWriter Synchronized(TextWriter writer, bool neverClose)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer is null");
			}
			if (writer is SynchronizedWriter)
			{
				return writer;
			}
			return new SynchronizedWriter(writer, neverClose);
		}

		/// <summary>Writes the text representation of a Boolean value to the text stream.</summary>
		/// <param name="value">The Boolean to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(bool value)
		{
			this.Write(value.ToString());
		}

		/// <summary>Writes a character to the text stream.</summary>
		/// <param name="value">The character to write to the text stream. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(char value)
		{
		}

		/// <summary>Writes a character array to the text stream.</summary>
		/// <param name="buffer">The character array to write to the text stream. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(char[] buffer)
		{
			if (buffer == null)
			{
				return;
			}
			this.Write(buffer, 0, buffer.Length);
		}

		/// <summary>Writes the text representation of a decimal value to the text stream.</summary>
		/// <param name="value">The decimal value to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(decimal value)
		{
			this.Write(value.ToString(this.internalFormatProvider));
		}

		/// <summary>Writes the text representation of an 8-byte floating-point value to the text stream.</summary>
		/// <param name="value">The 8-byte floating-point value to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(double value)
		{
			this.Write(value.ToString(this.internalFormatProvider));
		}

		/// <summary>Writes the text representation of a 4-byte signed integer to the text stream.</summary>
		/// <param name="value">The 4-byte signed integer to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(int value)
		{
			this.Write(value.ToString(this.internalFormatProvider));
		}

		/// <summary>Writes the text representation of an 8-byte signed integer to the text stream.</summary>
		/// <param name="value">The 8-byte signed integer to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(long value)
		{
			this.Write(value.ToString(this.internalFormatProvider));
		}

		/// <summary>Writes the text representation of an object to the text stream by calling ToString on that object.</summary>
		/// <param name="value">The object to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(object value)
		{
			if (value != null)
			{
				this.Write(value.ToString());
			}
		}

		/// <summary>Writes the text representation of a 4-byte floating-point value to the text stream.</summary>
		/// <param name="value">The 4-byte floating-point value to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(float value)
		{
			this.Write(value.ToString(this.internalFormatProvider));
		}

		/// <summary>Writes a string to the text stream.</summary>
		/// <param name="value">The string to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(string value)
		{
			if (value != null)
			{
				this.Write(value.ToCharArray());
			}
		}

		/// <summary>Writes the text representation of a 4-byte unsigned integer to the text stream.</summary>
		/// <param name="value">The 4-byte unsigned integer to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public virtual void Write(uint value)
		{
			this.Write(value.ToString(this.internalFormatProvider));
		}

		/// <summary>Writes the text representation of an 8-byte unsigned integer to the text stream.</summary>
		/// <param name="value">The 8-byte unsigned integer to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public virtual void Write(ulong value)
		{
			this.Write(value.ToString(this.internalFormatProvider));
		}

		/// <summary>Writes out a formatted string, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)" />.</summary>
		/// <param name="format">The formatting string. </param>
		/// <param name="arg0">An object to write into the formatted string. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.FormatException">The format specification in format is invalid.-or- The number indicating an argument to be formatted is less than zero, or larger than or equal to the number of provided objects to be formatted. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(string format, object arg0)
		{
			this.Write(string.Format(format, arg0));
		}

		/// <summary>Writes out a formatted string, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)" />.</summary>
		/// <param name="format">The formatting string. </param>
		/// <param name="arg">The object array to write into the formatted string. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> or <paramref name="arg" /> is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.FormatException">The format specification in format is invalid.-or- The number indicating an argument to be formatted is less than zero, or larger than or equal to <paramref name="arg" />. Length. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(string format, params object[] arg)
		{
			this.Write(string.Format(format, arg));
		}

		/// <summary>Writes a subarray of characters to the text stream.</summary>
		/// <param name="buffer">The character array to write data from. </param>
		/// <param name="index">Starting index in the buffer. </param>
		/// <param name="count">The number of characters to write. </param>
		/// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0 || index > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0 || index > buffer.Length - count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			while (count > 0)
			{
				this.Write(buffer[index]);
				count--;
				index++;
			}
		}

		/// <summary>Writes out a formatted string, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)" />.</summary>
		/// <param name="format">The formatting string. </param>
		/// <param name="arg0">An object to write into the formatted string. </param>
		/// <param name="arg1">An object to write into the formatted string. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.FormatException">The format specification in format is invalid.-or- The number indicating an argument to be formatted is less than zero, or larger than or equal to the number of provided objects to be formatted. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(string format, object arg0, object arg1)
		{
			this.Write(string.Format(format, arg0, arg1));
		}

		/// <summary>Writes out a formatted string, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)" />.</summary>
		/// <param name="format">The formatting string. </param>
		/// <param name="arg0">An object to write into the formatted string. </param>
		/// <param name="arg1">An object to write into the formatted string. </param>
		/// <param name="arg2">An object to write into the formatted string. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.FormatException">The format specification in format is invalid.-or- The number indicating an argument to be formatted is less than zero, or larger than or equal to the number of provided objects to be formatted. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Write(string format, object arg0, object arg1, object arg2)
		{
			this.Write(string.Format(format, arg0, arg1, arg2));
		}

		/// <summary>Writes a line terminator to the text stream.</summary>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine()
		{
			this.Write(this.CoreNewLine);
		}

		/// <summary>Writes the text representation of a Boolean followed by a line terminator to the text stream.</summary>
		/// <param name="value">The Boolean to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(bool value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes a character followed by a line terminator to the text stream.</summary>
		/// <param name="value">The character to write to the text stream. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(char value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes an array of characters followed by a line terminator to the text stream.</summary>
		/// <param name="buffer">The character array from which data is read. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(char[] buffer)
		{
			this.Write(buffer);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of a decimal value followed by a line terminator to the text stream.</summary>
		/// <param name="value">The decimal value to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(decimal value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of a 8-byte floating-point value followed by a line terminator to the text stream.</summary>
		/// <param name="value">The 8-byte floating-point value to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(double value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of a 4-byte signed integer followed by a line terminator to the text stream.</summary>
		/// <param name="value">The 4-byte signed integer to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(int value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of an 8-byte signed integer followed by a line terminator to the text stream.</summary>
		/// <param name="value">The 8-byte signed integer to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(long value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of an object by calling ToString on this object, followed by a line terminator to the text stream.</summary>
		/// <param name="value">The object to write. If <paramref name="value" /> is null, only the line termination characters are written. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(object value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of a 4-byte floating-point value followed by a line terminator to the text stream.</summary>
		/// <param name="value">The 4-byte floating-point value to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(float value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes a string followed by a line terminator to the text stream.</summary>
		/// <param name="value">The string to write. If <paramref name="value" /> is null, only the line termination characters are written. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(string value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of a 4-byte unsigned integer followed by a line terminator to the text stream.</summary>
		/// <param name="value">The 4-byte unsigned integer to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public virtual void WriteLine(uint value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of an 8-byte unsigned integer followed by a line terminator to the text stream.</summary>
		/// <param name="value">The 8-byte unsigned integer to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public virtual void WriteLine(ulong value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes out a formatted string and a new line, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)" />.</summary>
		/// <param name="format">The formatted string. </param>
		/// <param name="arg0">The object to write into the formatted string. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.FormatException">The format specification in format is invalid.-or- The number indicating an argument to be formatted is less than zero, or larger than or equal to the number of provided objects to be formatted. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(string format, object arg0)
		{
			this.Write(format, arg0);
			this.WriteLine();
		}

		/// <summary>Writes out a formatted string and a new line, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)" />.</summary>
		/// <param name="format">The formatting string. </param>
		/// <param name="arg">The object array to write into format string. </param>
		/// <exception cref="T:System.ArgumentNullException">A string or object is passed in as null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.FormatException">The format specification in format is invalid.-or- The number indicating an argument to be formatted is less than zero, or larger than or equal to arg.Length. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(string format, params object[] arg)
		{
			this.Write(format, arg);
			this.WriteLine();
		}

		/// <summary>Writes a subarray of characters followed by a line terminator to the text stream.</summary>
		/// <param name="buffer">The character array from which data is read. </param>
		/// <param name="index">The index into <paramref name="buffer" /> at which to begin reading. </param>
		/// <param name="count">The maximum number of characters to write. </param>
		/// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(char[] buffer, int index, int count)
		{
			this.Write(buffer, index, count);
			this.WriteLine();
		}

		/// <summary>Writes out a formatted string and a new line, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)" />.</summary>
		/// <param name="format">The formatting string. </param>
		/// <param name="arg0">The object to write into the format string. </param>
		/// <param name="arg1">The object to write into the format string. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.FormatException">The format specification in format is invalid.-or- The number indicating an argument to be formatted is less than zero, or larger than or equal to the number of provided objects to be formatted. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(string format, object arg0, object arg1)
		{
			this.Write(format, arg0, arg1);
			this.WriteLine();
		}

		/// <summary>Writes out a formatted string and a new line, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)" />.</summary>
		/// <param name="format">The formatting string. </param>
		/// <param name="arg0">The object to write into the format string. </param>
		/// <param name="arg1">The object to write into the format string. </param>
		/// <param name="arg2">The object to write into the format string. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.FormatException">The format specification in format is invalid.-or- The number indicating an argument to be formatted is less than zero, or larger than or equal to the number of provided objects to be formatted. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			this.Write(format, arg0, arg1, arg2);
			this.WriteLine();
		}

		private sealed class NullTextWriter : TextWriter
		{
			public override Encoding Encoding
			{
				get
				{
					return Encoding.Default;
				}
			}

			public override void Write(string s)
			{
			}

			public override void Write(char value)
			{
			}

			public override void Write(char[] value, int index, int count)
			{
			}
		}
	}
}
