using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	/// <summary>Implements a <see cref="T:System.IO.TextReader" /> that reads from a string.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class StringReader : TextReader
	{
		private string source;

		private int nextChar;

		private int sourceLength;

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StringReader" /> class that reads from the specified string.</summary>
		/// <param name="s">The string to which the <see cref="T:System.IO.StringReader" /> should be initialized. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="s" /> parameter is null. </exception>
		public StringReader(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			this.source = s;
			this.nextChar = 0;
			this.sourceLength = s.Length;
		}

		/// <summary>Closes the <see cref="T:System.IO.StringReader" />.</summary>
		/// <filterpriority>2</filterpriority>
		public override void Close()
		{
			this.Dispose(true);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.StringReader" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected override void Dispose(bool disposing)
		{
			this.source = null;
			base.Dispose(disposing);
		}

		/// <summary>Returns the next available character but does not consume it.</summary>
		/// <returns>An integer representing the next character to be read, or -1 if no more characters are available or the stream does not support seeking.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current reader is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override int Peek()
		{
			this.CheckObjectDisposedException();
			if (this.nextChar >= this.sourceLength)
			{
				return -1;
			}
			return (int)this.source[this.nextChar];
		}

		/// <summary>Reads the next character from the input string and advances the character position by one character.</summary>
		/// <returns>The next character from the underlying string, or -1 if no more characters are available.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current reader is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override int Read()
		{
			this.CheckObjectDisposedException();
			if (this.nextChar >= this.sourceLength)
			{
				return -1;
			}
			return (int)this.source[this.nextChar++];
		}

		/// <summary>Reads a block of characters from the input string and advances the character position by <paramref name="count" />.</summary>
		/// <returns>The total number of characters read into the buffer. This can be less than the number of characters requested if that many characters are not currently available, or zero if the end of the underlying string has been reached.</returns>
		/// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source. </param>
		/// <param name="index">The starting index in the buffer. </param>
		/// <param name="count">The number of characters to read. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current reader is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override int Read([In] [Out] char[] buffer, int index, int count)
		{
			this.CheckObjectDisposedException();
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException();
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			int num;
			if (this.nextChar > this.sourceLength - count)
			{
				num = this.sourceLength - this.nextChar;
			}
			else
			{
				num = count;
			}
			this.source.CopyTo(this.nextChar, buffer, index, num);
			this.nextChar += num;
			return num;
		}

		/// <summary>Reads a line from the underlying string.</summary>
		/// <returns>The next line from the underlying string, or null if the end of the underlying string is reached.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current reader is closed. </exception>
		/// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string. </exception>
		/// <filterpriority>2</filterpriority>
		public override string ReadLine()
		{
			this.CheckObjectDisposedException();
			int i;
			for (i = this.nextChar; i < this.sourceLength; i++)
			{
				char c = this.source[i];
				if (c == '\r' || c == '\n')
				{
					string result = this.source.Substring(this.nextChar, i - this.nextChar);
					this.nextChar = i + 1;
					if (c == '\r' && this.nextChar < this.sourceLength && this.source[this.nextChar] == '\n')
					{
						this.nextChar++;
					}
					return result;
				}
			}
			if (i > this.nextChar)
			{
				string result2 = this.source.Substring(this.nextChar, i - this.nextChar);
				this.nextChar = i;
				return result2;
			}
			return null;
		}

		/// <summary>Reads the stream as a string, either in its entirety or from the current position to the end of the stream.</summary>
		/// <returns>The content from the current position to the end of the underlying string.</returns>
		/// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current reader is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override string ReadToEnd()
		{
			this.CheckObjectDisposedException();
			string result = this.source.Substring(this.nextChar, this.sourceLength - this.nextChar);
			this.nextChar = this.sourceLength;
			return result;
		}

		private void CheckObjectDisposedException()
		{
			if (this.source == null)
			{
				throw new ObjectDisposedException("StringReader", Locale.GetText("Cannot read from a closed StringReader"));
			}
		}
	}
}
