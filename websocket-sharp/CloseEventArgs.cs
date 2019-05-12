using System;
using System.Text;

namespace WebSocketSharp
{
	public class CloseEventArgs : EventArgs
	{
		private bool _clean;

		private ushort _code;

		private string _reason;

		internal CloseEventArgs(PayloadData payload)
		{
			byte[] applicationData = payload.ApplicationData;
			this._code = CloseEventArgs.getCodeFrom(applicationData);
			this._reason = CloseEventArgs.getReasonFrom(applicationData);
			this._clean = false;
		}

		public ushort Code
		{
			get
			{
				return this._code;
			}
		}

		public string Reason
		{
			get
			{
				return this._reason;
			}
		}

		public bool WasClean
		{
			get
			{
				return this._clean;
			}
			internal set
			{
				this._clean = value;
			}
		}

		private static ushort getCodeFrom(byte[] data)
		{
			return (data.Length <= 1) ? 1005 : data.SubArray(0, 2).ToUInt16(ByteOrder.Big);
		}

		private static string getReasonFrom(byte[] data)
		{
			int num = data.Length;
			return (num <= 2) ? string.Empty : Encoding.UTF8.GetString(data.SubArray(2, num - 2));
		}
	}
}
