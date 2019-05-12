using System;

namespace System.IO
{
	[Serializable]
	internal class SynchronizedReader : TextReader
	{
		private TextReader reader;

		public SynchronizedReader(TextReader reader)
		{
			this.reader = reader;
		}

		public override void Close()
		{
			lock (this)
			{
				this.reader.Close();
			}
		}

		public override int Peek()
		{
			int result;
			lock (this)
			{
				result = this.reader.Peek();
			}
			return result;
		}

		public override int ReadBlock(char[] buffer, int index, int count)
		{
			int result;
			lock (this)
			{
				result = this.reader.ReadBlock(buffer, index, count);
			}
			return result;
		}

		public override string ReadLine()
		{
			string result;
			lock (this)
			{
				result = this.reader.ReadLine();
			}
			return result;
		}

		public override string ReadToEnd()
		{
			string result;
			lock (this)
			{
				result = this.reader.ReadToEnd();
			}
			return result;
		}

		public override int Read()
		{
			int result;
			lock (this)
			{
				result = this.reader.Read();
			}
			return result;
		}

		public override int Read(char[] buffer, int index, int count)
		{
			int result;
			lock (this)
			{
				result = this.reader.Read(buffer, index, count);
			}
			return result;
		}
	}
}
