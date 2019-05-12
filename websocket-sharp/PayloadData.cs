using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSocketSharp
{
	internal class PayloadData : IEnumerable<byte>, IEnumerable
	{
		public const ulong MaxLength = 9223372036854775807UL;

		public PayloadData() : this(new byte[0])
		{
		}

		public PayloadData(byte[] appData) : this(new byte[0], appData)
		{
		}

		public PayloadData(string appData) : this(Encoding.UTF8.GetBytes(appData))
		{
		}

		public PayloadData(byte[] appData, bool masked) : this(new byte[0], appData, masked)
		{
		}

		public PayloadData(byte[] extData, byte[] appData) : this(extData, appData, false)
		{
		}

		public PayloadData(byte[] extData, byte[] appData, bool masked)
		{
			if (extData.LongLength + appData.LongLength > 9223372036854775807L)
			{
				throw new ArgumentOutOfRangeException("The length of 'extData' plus 'appData' must be less than MaxLength.");
			}
			this.ExtensionData = extData;
			this.ApplicationData = appData;
			this.IsMasked = masked;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		internal bool ContainsReservedCloseStatusCode
		{
			get
			{
				return this.ApplicationData.Length > 1 && this.ApplicationData.SubArray(0, 2).ToUInt16(ByteOrder.Big).IsReserved();
			}
		}

		internal bool IsMasked { get; private set; }

		public byte[] ExtensionData { get; private set; }

		public byte[] ApplicationData { get; private set; }

		public ulong Length
		{
			get
			{
				return (ulong)(this.ExtensionData.LongLength + this.ApplicationData.LongLength);
			}
		}

		private static void mask(byte[] src, byte[] key)
		{
			for (long num = 0L; num < src.LongLength; num += 1L)
			{
				checked
				{
					src[(int)((IntPtr)num)] = (src[(int)((IntPtr)num)] ^ key[(int)((IntPtr)(num % 4L))]);
				}
			}
		}

		public IEnumerator<byte> GetEnumerator()
		{
			foreach (byte b in this.ExtensionData)
			{
				yield return b;
			}
			foreach (byte b2 in this.ApplicationData)
			{
				yield return b2;
			}
			yield break;
		}

		public void Mask(byte[] maskingKey)
		{
			if (this.ExtensionData.LongLength > 0L)
			{
				PayloadData.mask(this.ExtensionData, maskingKey);
			}
			if (this.ApplicationData.LongLength > 0L)
			{
				PayloadData.mask(this.ApplicationData, maskingKey);
			}
			this.IsMasked = !this.IsMasked;
		}

		public byte[] ToByteArray()
		{
			return (this.ExtensionData.LongLength <= 0L) ? this.ApplicationData : this.ToArray<byte>();
		}

		public override string ToString()
		{
			return BitConverter.ToString(this.ToByteArray());
		}
	}
}
