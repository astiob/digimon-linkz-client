using System;

namespace Mono.Security.Protocol.Ntlm
{
	public abstract class MessageBase
	{
		private static byte[] header = new byte[]
		{
			78,
			84,
			76,
			77,
			83,
			83,
			80,
			0
		};

		private int _type;

		private NtlmFlags _flags;

		protected MessageBase(int messageType)
		{
			this._type = messageType;
		}

		public NtlmFlags Flags
		{
			get
			{
				return this._flags;
			}
			set
			{
				this._flags = value;
			}
		}

		public int Type
		{
			get
			{
				return this._type;
			}
		}

		protected byte[] PrepareMessage(int messageSize)
		{
			byte[] array = new byte[messageSize];
			Buffer.BlockCopy(MessageBase.header, 0, array, 0, 8);
			array[8] = (byte)this._type;
			array[9] = (byte)(this._type >> 8);
			array[10] = (byte)(this._type >> 16);
			array[11] = (byte)(this._type >> 24);
			return array;
		}

		protected virtual void Decode(byte[] message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			if (message.Length < 12)
			{
				string text = Locale.GetText("Minimum message length is 12 bytes.");
				throw new ArgumentOutOfRangeException("message", message.Length, text);
			}
			if (!this.CheckHeader(message))
			{
				string message2 = string.Format(Locale.GetText("Invalid Type{0} message."), this._type);
				throw new ArgumentException(message2, "message");
			}
		}

		protected bool CheckHeader(byte[] message)
		{
			for (int i = 0; i < MessageBase.header.Length; i++)
			{
				if (message[i] != MessageBase.header[i])
				{
					return false;
				}
			}
			return (ulong)BitConverterLE.ToUInt32(message, 8) == (ulong)((long)this._type);
		}

		public abstract byte[] GetBytes();
	}
}
