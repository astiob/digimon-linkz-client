using System;
using System.IO;

namespace Neptune.Queue
{
	public class NpQueue
	{
		private MemoryStream mMemory;

		private long mReadPoint;

		private object mLockObject = new object();

		public NpQueue()
		{
			this.mMemory = new MemoryStream();
		}

		public NpQueue(byte[] buffer)
		{
			this.mMemory = new MemoryStream();
			this.mMemory.Write(buffer, 0, buffer.Length);
			this.mMemory.Seek(0L, SeekOrigin.Begin);
		}

		public byte[] GetBuffer
		{
			get
			{
				return this.mMemory.GetBuffer();
			}
		}

		public void Write(byte[] writeData)
		{
			byte[] bytes = BitConverter.GetBytes(writeData.Length);
			object obj = this.mLockObject;
			lock (obj)
			{
				this.mMemory.Write(bytes, 0, bytes.Length);
				this.mMemory.Write(writeData, 0, writeData.Length);
			}
		}

		public bool Read(out byte[] readData)
		{
			readData = null;
			object obj = this.mLockObject;
			bool result;
			lock (obj)
			{
				if (this.mMemory.Length == 0L)
				{
					readData = new byte[0];
					result = false;
				}
				else
				{
					long position = this.mMemory.Position;
					this.mMemory.Seek(this.mReadPoint, SeekOrigin.Begin);
					byte[] array = new byte[4];
					this.mMemory.Read(array, 0, array.Length);
					int num = BitConverter.ToInt32(array, 0);
					readData = new byte[num];
					this.mMemory.Read(readData, 0, num);
					if (this.mMemory.Position >= this.mMemory.Length)
					{
						byte[] buffer = this.mMemory.GetBuffer();
						Array.Clear(buffer, 0, buffer.Length);
						this.mMemory.SetLength(0L);
						this.mReadPoint = 0L;
						result = true;
					}
					else
					{
						this.mReadPoint = this.mMemory.Position;
						this.mMemory.Seek(position, SeekOrigin.Begin);
						result = true;
					}
				}
			}
			return result;
		}

		public void Clear()
		{
			object obj = this.mLockObject;
			lock (obj)
			{
				byte[] buffer = this.mMemory.GetBuffer();
				Array.Clear(buffer, 0, buffer.Length);
				this.mMemory.SetLength(0L);
				this.mReadPoint = 0L;
			}
		}
	}
}
