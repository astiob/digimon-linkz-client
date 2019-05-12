using System;
using System.Text;

namespace WebSocketSharp
{
	public class MessageEventArgs : EventArgs
	{
		private string _data;

		private Opcode _opcode;

		private byte[] _rawData;

		internal MessageEventArgs(Opcode opcode, byte[] data)
		{
			if (data.LongLength > 9223372036854775807L)
			{
				throw new WebSocketException(CloseStatusCode.TooBig);
			}
			this._opcode = opcode;
			this._rawData = data;
			this._data = MessageEventArgs.convertToString(opcode, data);
		}

		internal MessageEventArgs(Opcode opcode, PayloadData payload)
		{
			this._opcode = opcode;
			this._rawData = payload.ApplicationData;
			this._data = MessageEventArgs.convertToString(opcode, this._rawData);
		}

		public string Data
		{
			get
			{
				return this._data;
			}
		}

		public byte[] RawData
		{
			get
			{
				return this._rawData;
			}
		}

		public Opcode Type
		{
			get
			{
				return this._opcode;
			}
		}

		private static string convertToString(Opcode opcode, byte[] data)
		{
			return (data.LongLength != 0L) ? ((opcode != Opcode.Text) ? opcode.ToString() : Encoding.UTF8.GetString(data)) : string.Empty;
		}
	}
}
