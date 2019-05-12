using System;
using System.Globalization;
using System.Text;

namespace Mono.Security.Protocol.Ntlm
{
	public class Type1Message : MessageBase
	{
		private string _host;

		private string _domain;

		public Type1Message() : base(1)
		{
			this._domain = Environment.UserDomainName;
			this._host = Environment.MachineName;
			base.Flags = (NtlmFlags.NegotiateUnicode | NtlmFlags.NegotiateOem | NtlmFlags.NegotiateNtlm | NtlmFlags.NegotiateDomainSupplied | NtlmFlags.NegotiateWorkstationSupplied | NtlmFlags.NegotiateAlwaysSign);
		}

		public Type1Message(byte[] message) : base(1)
		{
			this.Decode(message);
		}

		public string Domain
		{
			get
			{
				return this._domain;
			}
			set
			{
				this._domain = value;
			}
		}

		public string Host
		{
			get
			{
				return this._host;
			}
			set
			{
				this._host = value;
			}
		}

		protected override void Decode(byte[] message)
		{
			base.Decode(message);
			base.Flags = (NtlmFlags)BitConverterLE.ToUInt32(message, 12);
			int count = (int)BitConverterLE.ToUInt16(message, 16);
			int index = (int)BitConverterLE.ToUInt16(message, 20);
			this._domain = Encoding.ASCII.GetString(message, index, count);
			int count2 = (int)BitConverterLE.ToUInt16(message, 24);
			this._host = Encoding.ASCII.GetString(message, 32, count2);
		}

		public override byte[] GetBytes()
		{
			short num = (short)this._domain.Length;
			short num2 = (short)this._host.Length;
			byte[] array = base.PrepareMessage((int)(32 + num + num2));
			array[12] = (byte)base.Flags;
			array[13] = (byte)(base.Flags >> 8);
			array[14] = (byte)(base.Flags >> 16);
			array[15] = (byte)(base.Flags >> 24);
			short num3 = 32 + num2;
			array[16] = (byte)num;
			array[17] = (byte)(num >> 8);
			array[18] = array[16];
			array[19] = array[17];
			array[20] = (byte)num3;
			array[21] = (byte)(num3 >> 8);
			array[24] = (byte)num2;
			array[25] = (byte)(num2 >> 8);
			array[26] = array[24];
			array[27] = array[25];
			array[28] = 32;
			array[29] = 0;
			byte[] bytes = Encoding.ASCII.GetBytes(this._host.ToUpper(CultureInfo.InvariantCulture));
			Buffer.BlockCopy(bytes, 0, array, 32, bytes.Length);
			byte[] bytes2 = Encoding.ASCII.GetBytes(this._domain.ToUpper(CultureInfo.InvariantCulture));
			Buffer.BlockCopy(bytes2, 0, array, (int)num3, bytes2.Length);
			return array;
		}
	}
}
