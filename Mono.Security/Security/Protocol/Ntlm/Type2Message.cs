using System;
using System.Security.Cryptography;

namespace Mono.Security.Protocol.Ntlm
{
	public class Type2Message : MessageBase
	{
		private byte[] _nonce;

		public Type2Message() : base(2)
		{
			this._nonce = new byte[8];
			RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
			randomNumberGenerator.GetBytes(this._nonce);
			base.Flags = (NtlmFlags.NegotiateUnicode | NtlmFlags.NegotiateNtlm | NtlmFlags.NegotiateAlwaysSign);
		}

		public Type2Message(byte[] message) : base(2)
		{
			this._nonce = new byte[8];
			this.Decode(message);
		}

		~Type2Message()
		{
			if (this._nonce != null)
			{
				Array.Clear(this._nonce, 0, this._nonce.Length);
			}
		}

		public byte[] Nonce
		{
			get
			{
				return (byte[])this._nonce.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Nonce");
				}
				if (value.Length != 8)
				{
					string text = Locale.GetText("Invalid Nonce Length (should be 8 bytes).");
					throw new ArgumentException(text, "Nonce");
				}
				this._nonce = (byte[])value.Clone();
			}
		}

		protected override void Decode(byte[] message)
		{
			base.Decode(message);
			base.Flags = (NtlmFlags)BitConverterLE.ToUInt32(message, 20);
			Buffer.BlockCopy(message, 24, this._nonce, 0, 8);
		}

		public override byte[] GetBytes()
		{
			byte[] array = base.PrepareMessage(40);
			short num = (short)array.Length;
			array[16] = (byte)num;
			array[17] = (byte)(num >> 8);
			array[20] = (byte)base.Flags;
			array[21] = (byte)(base.Flags >> 8);
			array[22] = (byte)(base.Flags >> 16);
			array[23] = (byte)(base.Flags >> 24);
			Buffer.BlockCopy(this._nonce, 0, array, 24, this._nonce.Length);
			return array;
		}
	}
}
