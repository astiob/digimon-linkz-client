using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO
{
	internal class UnexceptionalStreamReader : StreamReader
	{
		private static bool[] newline = new bool[Environment.NewLine.Length];

		private static char newlineChar;

		public UnexceptionalStreamReader(Stream stream) : base(stream)
		{
		}

		public UnexceptionalStreamReader(Stream stream, bool detect_encoding_from_bytemarks) : base(stream, detect_encoding_from_bytemarks)
		{
		}

		public UnexceptionalStreamReader(Stream stream, Encoding encoding) : base(stream, encoding)
		{
		}

		public UnexceptionalStreamReader(Stream stream, Encoding encoding, bool detect_encoding_from_bytemarks) : base(stream, encoding, detect_encoding_from_bytemarks)
		{
		}

		public UnexceptionalStreamReader(Stream stream, Encoding encoding, bool detect_encoding_from_bytemarks, int buffer_size) : base(stream, encoding, detect_encoding_from_bytemarks, buffer_size)
		{
		}

		public UnexceptionalStreamReader(string path) : base(path)
		{
		}

		public UnexceptionalStreamReader(string path, bool detect_encoding_from_bytemarks) : base(path, detect_encoding_from_bytemarks)
		{
		}

		public UnexceptionalStreamReader(string path, Encoding encoding) : base(path, encoding)
		{
		}

		public UnexceptionalStreamReader(string path, Encoding encoding, bool detect_encoding_from_bytemarks) : base(path, encoding, detect_encoding_from_bytemarks)
		{
		}

		public UnexceptionalStreamReader(string path, Encoding encoding, bool detect_encoding_from_bytemarks, int buffer_size) : base(path, encoding, detect_encoding_from_bytemarks, buffer_size)
		{
		}

		static UnexceptionalStreamReader()
		{
			string newLine = Environment.NewLine;
			if (newLine.Length == 1)
			{
				UnexceptionalStreamReader.newlineChar = newLine[0];
			}
		}

		public override int Peek()
		{
			try
			{
				return base.Peek();
			}
			catch (IOException)
			{
			}
			return -1;
		}

		public override int Read()
		{
			try
			{
				return base.Read();
			}
			catch (IOException)
			{
			}
			return -1;
		}

		public override int Read([In] [Out] char[] dest_buffer, int index, int count)
		{
			if (dest_buffer == null)
			{
				throw new ArgumentNullException("dest_buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "< 0");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "< 0");
			}
			if (index > dest_buffer.Length - count)
			{
				throw new ArgumentException("index + count > dest_buffer.Length");
			}
			int num = 0;
			char c = UnexceptionalStreamReader.newlineChar;
			try
			{
				while (count > 0)
				{
					int num2 = base.Read();
					if (num2 < 0)
					{
						break;
					}
					num++;
					count--;
					dest_buffer[index] = (char)num2;
					if (c != '\0')
					{
						if ((char)num2 == c)
						{
							return num;
						}
					}
					else if (this.CheckEOL((char)num2))
					{
						return num;
					}
					index++;
				}
			}
			catch (IOException)
			{
			}
			return num;
		}

		private bool CheckEOL(char current)
		{
			int i = 0;
			while (i < UnexceptionalStreamReader.newline.Length)
			{
				if (!UnexceptionalStreamReader.newline[i])
				{
					if (current == Environment.NewLine[i])
					{
						UnexceptionalStreamReader.newline[i] = true;
						return i == UnexceptionalStreamReader.newline.Length - 1;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < UnexceptionalStreamReader.newline.Length; j++)
			{
				UnexceptionalStreamReader.newline[j] = false;
			}
			return false;
		}

		public override string ReadLine()
		{
			try
			{
				return base.ReadLine();
			}
			catch (IOException)
			{
			}
			return null;
		}

		public override string ReadToEnd()
		{
			try
			{
				return base.ReadToEnd();
			}
			catch (IOException)
			{
			}
			return null;
		}
	}
}
