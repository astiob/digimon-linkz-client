using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO
{
	/// <summary>Implements a <see cref="T:System.IO.TextWriter" /> for writing information to a string. The information is stored in an underlying <see cref="T:System.Text.StringBuilder" />.</summary>
	/// <filterpriority>2</filterpriority>
	[MonoTODO("Serialization format not compatible with .NET")]
	[ComVisible(true)]
	[Serializable]
	public class StringWriter : TextWriter
	{
		private StringBuilder internalString;

		private bool disposed;

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StringWriter" /> class.</summary>
		public StringWriter() : this(new StringBuilder())
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StringWriter" /> class with the specified format control.</summary>
		/// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that controls formatting. </param>
		public StringWriter(IFormatProvider formatProvider) : this(new StringBuilder(), formatProvider)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StringWriter" /> class that writes to the specified <see cref="T:System.Text.StringBuilder" />.</summary>
		/// <param name="sb">The StringBuilder to write to. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sb" /> is null. </exception>
		public StringWriter(StringBuilder sb) : this(sb, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StringWriter" /> class that writes to the specified <see cref="T:System.Text.StringBuilder" /> and has the specified format provider.</summary>
		/// <param name="sb">The StringBuilder to write to. </param>
		/// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that controls formatting. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sb" /> is null. </exception>
		public StringWriter(StringBuilder sb, IFormatProvider formatProvider)
		{
			if (sb == null)
			{
				throw new ArgumentNullException("sb");
			}
			this.internalString = sb;
			this.internalFormatProvider = formatProvider;
		}

		/// <summary>Gets the <see cref="T:System.Text.Encoding" /> in which the output is written.</summary>
		/// <returns>The Encoding in which the output is written.</returns>
		/// <filterpriority>1</filterpriority>
		public override Encoding Encoding
		{
			get
			{
				return Encoding.Unicode;
			}
		}

		/// <summary>Closes the current <see cref="T:System.IO.StringWriter" /> and the underlying stream.</summary>
		/// <filterpriority>1</filterpriority>
		public override void Close()
		{
			this.Dispose(true);
			this.disposed = true;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.StringWriter" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this.disposed = true;
		}

		/// <summary>Returns the underlying <see cref="T:System.Text.StringBuilder" />.</summary>
		/// <returns>The underlying StringBuilder.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual StringBuilder GetStringBuilder()
		{
			return this.internalString;
		}

		/// <summary>Returns a string containing the characters written to the current StringWriter so far.</summary>
		/// <returns>The string containing the characters written to the current StringWriter.</returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return this.internalString.ToString();
		}

		/// <summary>Writes a character to this instance of the StringWriter.</summary>
		/// <param name="value">The character to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The writer is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override void Write(char value)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("StringReader", Locale.GetText("Cannot write to a closed StringWriter"));
			}
			this.internalString.Append(value);
		}

		/// <summary>Writes a string to this instance of the StringWriter.</summary>
		/// <param name="value">The string to write. </param>
		/// <exception cref="T:System.ObjectDisposedException">The writer is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override void Write(string value)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("StringReader", Locale.GetText("Cannot write to a closed StringWriter"));
			}
			this.internalString.Append(value);
		}

		/// <summary>Writes the specified region of a character array to this instance of the StringWriter.</summary>
		/// <param name="buffer">The character array to read data from. </param>
		/// <param name="index">The index at which to begin reading from <paramref name="buffer" />. </param>
		/// <param name="count">The maximum number of characters to write. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative. </exception>
		/// <exception cref="T:System.ArgumentException">(<paramref name="index" /> + <paramref name="count" />)&gt; <paramref name="buffer" />. Length. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The writer is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override void Write(char[] buffer, int index, int count)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("StringReader", Locale.GetText("Cannot write to a closed StringWriter"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "< 0");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "< 0");
			}
			if (index > buffer.Length - count)
			{
				throw new ArgumentException("index + count > buffer.Length");
			}
			this.internalString.Append(buffer, index, count);
		}
	}
}
