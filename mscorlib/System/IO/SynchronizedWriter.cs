using System;
using System.Text;

namespace System.IO
{
	[Serializable]
	internal class SynchronizedWriter : TextWriter
	{
		private TextWriter writer;

		private bool neverClose;

		public SynchronizedWriter(TextWriter writer) : this(writer, false)
		{
		}

		public SynchronizedWriter(TextWriter writer, bool neverClose)
		{
			this.writer = writer;
			this.neverClose = neverClose;
		}

		public override void Close()
		{
			if (this.neverClose)
			{
				return;
			}
			lock (this)
			{
				this.writer.Close();
			}
		}

		public override void Flush()
		{
			lock (this)
			{
				this.writer.Flush();
			}
		}

		public override void Write(bool value)
		{
			lock (this)
			{
				this.writer.Write(value);
			}
		}

		public override void Write(char value)
		{
			lock (this)
			{
				this.writer.Write(value);
			}
		}

		public override void Write(char[] value)
		{
			lock (this)
			{
				this.writer.Write(value);
			}
		}

		public override void Write(decimal value)
		{
			lock (this)
			{
				this.writer.Write(value);
			}
		}

		public override void Write(int value)
		{
			lock (this)
			{
				this.writer.Write(value);
			}
		}

		public override void Write(long value)
		{
			lock (this)
			{
				this.writer.Write(value);
			}
		}

		public override void Write(object value)
		{
			lock (this)
			{
				this.writer.Write(value);
			}
		}

		public override void Write(float value)
		{
			lock (this)
			{
				this.writer.Write(value);
			}
		}

		public override void Write(string value)
		{
			lock (this)
			{
				this.writer.Write(value);
			}
		}

		public override void Write(uint value)
		{
			lock (this)
			{
				this.writer.Write(value);
			}
		}

		public override void Write(ulong value)
		{
			lock (this)
			{
				this.writer.Write(value);
			}
		}

		public override void Write(string format, object value)
		{
			lock (this)
			{
				this.writer.Write(format, value);
			}
		}

		public override void Write(string format, object[] value)
		{
			lock (this)
			{
				this.writer.Write(format, value);
			}
		}

		public override void Write(char[] buffer, int index, int count)
		{
			lock (this)
			{
				this.writer.Write(buffer, index, count);
			}
		}

		public override void Write(string format, object arg0, object arg1)
		{
			lock (this)
			{
				this.writer.Write(format, arg0, arg1);
			}
		}

		public override void Write(string format, object arg0, object arg1, object arg2)
		{
			lock (this)
			{
				this.writer.Write(format, arg0, arg1, arg2);
			}
		}

		public override void WriteLine()
		{
			lock (this)
			{
				this.writer.WriteLine();
			}
		}

		public override void WriteLine(bool value)
		{
			lock (this)
			{
				this.writer.WriteLine(value);
			}
		}

		public override void WriteLine(char value)
		{
			lock (this)
			{
				this.writer.WriteLine(value);
			}
		}

		public override void WriteLine(char[] value)
		{
			lock (this)
			{
				this.writer.WriteLine(value);
			}
		}

		public override void WriteLine(decimal value)
		{
			lock (this)
			{
				this.writer.WriteLine(value);
			}
		}

		public override void WriteLine(double value)
		{
			lock (this)
			{
				this.writer.WriteLine(value);
			}
		}

		public override void WriteLine(int value)
		{
			lock (this)
			{
				this.writer.WriteLine(value);
			}
		}

		public override void WriteLine(long value)
		{
			lock (this)
			{
				this.writer.WriteLine(value);
			}
		}

		public override void WriteLine(object value)
		{
			lock (this)
			{
				this.writer.WriteLine(value);
			}
		}

		public override void WriteLine(float value)
		{
			lock (this)
			{
				this.writer.WriteLine(value);
			}
		}

		public override void WriteLine(string value)
		{
			lock (this)
			{
				this.writer.WriteLine(value);
			}
		}

		public override void WriteLine(uint value)
		{
			lock (this)
			{
				this.writer.WriteLine(value);
			}
		}

		public override void WriteLine(ulong value)
		{
			lock (this)
			{
				this.writer.WriteLine(value);
			}
		}

		public override void WriteLine(string format, object value)
		{
			lock (this)
			{
				this.writer.WriteLine(format, value);
			}
		}

		public override void WriteLine(string format, object[] value)
		{
			lock (this)
			{
				this.writer.WriteLine(format, value);
			}
		}

		public override void WriteLine(char[] buffer, int index, int count)
		{
			lock (this)
			{
				this.writer.WriteLine(buffer, index, count);
			}
		}

		public override void WriteLine(string format, object arg0, object arg1)
		{
			lock (this)
			{
				this.writer.WriteLine(format, arg0, arg1);
			}
		}

		public override void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			lock (this)
			{
				this.writer.WriteLine(format, arg0, arg1, arg2);
			}
		}

		public override Encoding Encoding
		{
			get
			{
				Encoding encoding;
				lock (this)
				{
					encoding = this.writer.Encoding;
				}
				return encoding;
			}
		}

		public override IFormatProvider FormatProvider
		{
			get
			{
				IFormatProvider formatProvider;
				lock (this)
				{
					formatProvider = this.writer.FormatProvider;
				}
				return formatProvider;
			}
		}

		public override string NewLine
		{
			get
			{
				string newLine;
				lock (this)
				{
					newLine = this.writer.NewLine;
				}
				return newLine;
			}
			set
			{
				lock (this)
				{
					this.writer.NewLine = value;
				}
			}
		}
	}
}
