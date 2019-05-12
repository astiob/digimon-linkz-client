using System;
using System.Text;

namespace System.IO
{
	internal class UnexceptionalStreamWriter : StreamWriter
	{
		public UnexceptionalStreamWriter(Stream stream) : base(stream)
		{
		}

		public UnexceptionalStreamWriter(Stream stream, Encoding encoding) : base(stream, encoding)
		{
		}

		public UnexceptionalStreamWriter(Stream stream, Encoding encoding, int bufferSize) : base(stream, encoding, bufferSize)
		{
		}

		public UnexceptionalStreamWriter(string path) : base(path)
		{
		}

		public UnexceptionalStreamWriter(string path, bool append) : base(path, append)
		{
		}

		public UnexceptionalStreamWriter(string path, bool append, Encoding encoding) : base(path, append, encoding)
		{
		}

		public UnexceptionalStreamWriter(string path, bool append, Encoding encoding, int bufferSize) : base(path, append, encoding, bufferSize)
		{
		}

		public override void Flush()
		{
			try
			{
				base.Flush();
			}
			catch (Exception)
			{
			}
		}

		public override void Write(char[] buffer, int index, int count)
		{
			try
			{
				base.Write(buffer, index, count);
			}
			catch (Exception)
			{
			}
		}

		public override void Write(char value)
		{
			try
			{
				base.Write(value);
			}
			catch (Exception)
			{
			}
		}

		public override void Write(char[] value)
		{
			try
			{
				base.Write(value);
			}
			catch (Exception)
			{
			}
		}

		public override void Write(string value)
		{
			try
			{
				base.Write(value);
			}
			catch (Exception)
			{
			}
		}
	}
}
