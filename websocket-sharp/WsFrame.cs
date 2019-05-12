using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebSocketSharp
{
	internal class WsFrame : IEnumerable<byte>, IEnumerable
	{
		internal static readonly byte[] EmptyUnmaskPingData = WsFrame.CreatePingFrame(Mask.Unmask).ToByteArray();

		private WsFrame()
		{
		}

		public WsFrame(Opcode opcode, PayloadData payload) : this(opcode, Mask.Mask, payload)
		{
		}

		public WsFrame(Opcode opcode, Mask mask, PayloadData payload) : this(Fin.Final, opcode, mask, payload)
		{
		}

		public WsFrame(Fin fin, Opcode opcode, Mask mask, PayloadData payload) : this(fin, opcode, mask, payload, false)
		{
		}

		public WsFrame(Fin fin, Opcode opcode, Mask mask, PayloadData payload, bool compressed)
		{
			this.Fin = fin;
			this.Rsv1 = ((!WsFrame.isData(opcode) || !compressed) ? Rsv.Off : Rsv.On);
			this.Rsv2 = Rsv.Off;
			this.Rsv3 = Rsv.Off;
			this.Opcode = opcode;
			this.Mask = mask;
			ulong length = payload.Length;
			byte b = (length >= 126UL) ? ((length >= 65536UL) ? 127 : 126) : ((byte)length);
			this.PayloadLen = b;
			this.ExtPayloadLen = ((b >= 126) ? ((b != 126) ? length.ToByteArrayInternally(ByteOrder.Big) : ((ushort)length).ToByteArrayInternally(ByteOrder.Big)) : new byte[0]);
			bool flag = mask == Mask.Mask;
			byte[] maskingKey = (!flag) ? new byte[0] : WsFrame.createMaskingKey();
			this.MaskingKey = maskingKey;
			if (flag)
			{
				payload.Mask(maskingKey);
			}
			this.PayloadData = payload;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		internal bool IsBinary
		{
			get
			{
				return this.Opcode == Opcode.Binary;
			}
		}

		internal bool IsClose
		{
			get
			{
				return this.Opcode == Opcode.Close;
			}
		}

		internal bool IsCompressed
		{
			get
			{
				return this.Rsv1 == Rsv.On;
			}
		}

		internal bool IsContinuation
		{
			get
			{
				return this.Opcode == Opcode.Cont;
			}
		}

		internal bool IsControl
		{
			get
			{
				Opcode opcode = this.Opcode;
				return opcode == Opcode.Close || opcode == Opcode.Ping || opcode == Opcode.Pong;
			}
		}

		internal bool IsData
		{
			get
			{
				Opcode opcode = this.Opcode;
				return opcode == Opcode.Binary || opcode == Opcode.Text;
			}
		}

		internal bool IsFinal
		{
			get
			{
				return this.Fin == Fin.Final;
			}
		}

		internal bool IsFragmented
		{
			get
			{
				return this.Fin == Fin.More || this.Opcode == Opcode.Cont;
			}
		}

		internal bool IsMasked
		{
			get
			{
				return this.Mask == Mask.Mask;
			}
		}

		internal bool IsPerMessageCompressed
		{
			get
			{
				Opcode opcode = this.Opcode;
				return (opcode == Opcode.Binary || opcode == Opcode.Text) && this.Rsv1 == Rsv.On;
			}
		}

		internal bool IsPing
		{
			get
			{
				return this.Opcode == Opcode.Ping;
			}
		}

		internal bool IsPong
		{
			get
			{
				return this.Opcode == Opcode.Pong;
			}
		}

		internal bool IsText
		{
			get
			{
				return this.Opcode == Opcode.Text;
			}
		}

		internal ulong Length
		{
			get
			{
				return (ulong)(2L + (long)(this.ExtPayloadLen.Length + this.MaskingKey.Length) + (long)this.PayloadData.Length);
			}
		}

		public Fin Fin { get; private set; }

		public Rsv Rsv1 { get; private set; }

		public Rsv Rsv2 { get; private set; }

		public Rsv Rsv3 { get; private set; }

		public Opcode Opcode { get; private set; }

		public Mask Mask { get; private set; }

		public byte PayloadLen { get; private set; }

		public byte[] ExtPayloadLen { get; private set; }

		public byte[] MaskingKey { get; private set; }

		public PayloadData PayloadData { get; private set; }

		private static byte[] createMaskingKey()
		{
			byte[] array = new byte[4];
			Random random = new Random();
			random.NextBytes(array);
			return array;
		}

		private static string dump(WsFrame frame)
		{
			ulong length = frame.Length;
			long num = (long)(length / 4UL);
			int num2 = (int)(length % 4UL);
			string countFmt;
			int num3;
			if (num < 10000L)
			{
				num3 = 4;
				countFmt = "{0,4}";
			}
			else if (num < 65536L)
			{
				num3 = 4;
				countFmt = "{0,4:X}";
			}
			else if (num < 4294967296L)
			{
				num3 = 8;
				countFmt = "{0,8:X}";
			}
			else
			{
				num3 = 16;
				countFmt = "{0,16:X}";
			}
			string arg = string.Format("{{0,{0}}}", num3);
			string format = string.Format("{0} 01234567 89ABCDEF 01234567 89ABCDEF\n{0}+--------+--------+--------+--------+\\n", arg);
			string format2 = string.Format("{0}+--------+--------+--------+--------+", arg);
			StringBuilder buffer = new StringBuilder(64);
			Func<Action<string, string, string, string>> func = delegate()
			{
				long lineCount = 0L;
				string lineFmt = string.Format("{0}|{{1,8}} {{2,8}} {{3,8}} {{4,8}}|\n", countFmt);
				return delegate(string arg1, string arg2, string arg3, string arg4)
				{
					buffer.AppendFormat(lineFmt, new object[]
					{
						lineCount += 1L,
						arg1,
						arg2,
						arg3,
						arg4
					});
				};
			};
			Action<string, string, string, string> action = func();
			buffer.AppendFormat(format, string.Empty);
			byte[] array = frame.ToByteArray();
			int num4 = 0;
			while ((long)num4 <= num)
			{
				int num5 = num4 * 4;
				if ((long)num4 < num)
				{
					action(Convert.ToString(array[num5], 2).PadLeft(8, '0'), Convert.ToString(array[num5 + 1], 2).PadLeft(8, '0'), Convert.ToString(array[num5 + 2], 2).PadLeft(8, '0'), Convert.ToString(array[num5 + 3], 2).PadLeft(8, '0'));
				}
				else if (num2 > 0)
				{
					action(Convert.ToString(array[num5], 2).PadLeft(8, '0'), (num2 < 2) ? string.Empty : Convert.ToString(array[num5 + 1], 2).PadLeft(8, '0'), (num2 != 3) ? string.Empty : Convert.ToString(array[num5 + 2], 2).PadLeft(8, '0'), string.Empty);
				}
				num4++;
			}
			buffer.AppendFormat(format2, string.Empty);
			return buffer.ToString();
		}

		private static bool isBinary(Opcode opcode)
		{
			return opcode == Opcode.Binary;
		}

		private static bool isClose(Opcode opcode)
		{
			return opcode == Opcode.Close;
		}

		private static bool isContinuation(Opcode opcode)
		{
			return opcode == Opcode.Cont;
		}

		private static bool isControl(Opcode opcode)
		{
			return opcode == Opcode.Close || opcode == Opcode.Ping || opcode == Opcode.Pong;
		}

		private static bool isData(Opcode opcode)
		{
			return opcode == Opcode.Text || opcode == Opcode.Binary;
		}

		private static bool isFinal(Fin fin)
		{
			return fin == Fin.Final;
		}

		private static bool isMasked(Mask mask)
		{
			return mask == Mask.Mask;
		}

		private static bool isPing(Opcode opcode)
		{
			return opcode == Opcode.Ping;
		}

		private static bool isPong(Opcode opcode)
		{
			return opcode == Opcode.Pong;
		}

		private static bool isText(Opcode opcode)
		{
			return opcode == Opcode.Text;
		}

		private static WsFrame parse(byte[] header, Stream stream, bool unmask)
		{
			Fin fin = ((header[0] & 128) != 128) ? Fin.More : Fin.Final;
			Rsv rsv = ((header[0] & 64) != 64) ? Rsv.Off : Rsv.On;
			Rsv rsv2 = ((header[0] & 32) != 32) ? Rsv.Off : Rsv.On;
			Rsv rsv3 = ((header[0] & 16) != 16) ? Rsv.Off : Rsv.On;
			Opcode opcode = (Opcode)(header[0] & 15);
			Mask mask = ((header[1] & 128) != 128) ? Mask.Unmask : Mask.Mask;
			byte b = header[1] & 127;
			string text = (!WsFrame.isControl(opcode) || fin != Fin.More) ? ((WsFrame.isData(opcode) || rsv != Rsv.On) ? null : "A non data frame is compressed.") : "A control frame is fragmented.";
			if (text != null)
			{
				throw new WebSocketException(CloseStatusCode.IncorrectData, text);
			}
			if (WsFrame.isControl(opcode) && b > 125)
			{
				throw new WebSocketException(CloseStatusCode.InconsistentData, "The payload data length of a control frame is greater than 125 bytes.");
			}
			WsFrame wsFrame = new WsFrame
			{
				Fin = fin,
				Rsv1 = rsv,
				Rsv2 = rsv2,
				Rsv3 = rsv3,
				Opcode = opcode,
				Mask = mask,
				PayloadLen = b
			};
			int num = (b >= 126) ? ((b != 126) ? 8 : 2) : 0;
			byte[] array = (num <= 0) ? new byte[0] : stream.ReadBytes(num);
			if (num > 0 && array.Length != num)
			{
				throw new WebSocketException("The 'Extended Payload Length' of a frame cannot be read from the data source.");
			}
			wsFrame.ExtPayloadLen = array;
			bool flag = mask == Mask.Mask;
			byte[] array2 = (!flag) ? new byte[0] : stream.ReadBytes(4);
			if (flag && array2.Length != 4)
			{
				throw new WebSocketException("The 'Masking Key' of a frame cannot be read from the data source.");
			}
			wsFrame.MaskingKey = array2;
			ulong num2 = (b >= 126) ? ((b != 126) ? array.ToUInt64(ByteOrder.Big) : ((ulong)array.ToUInt16(ByteOrder.Big))) : ((ulong)b);
			byte[] array3;
			if (num2 > 0UL)
			{
				if (b > 126 && num2 > 9223372036854775807UL)
				{
					throw new WebSocketException(CloseStatusCode.TooBig, "The 'Payload Data' length is greater than the allowable length.");
				}
				array3 = ((b <= 126) ? stream.ReadBytes((int)num2) : stream.ReadBytes((long)num2, 1024));
				if (array3.LongLength != (long)num2)
				{
					throw new WebSocketException("The 'Payload Data' of a frame cannot be read from the data source.");
				}
			}
			else
			{
				array3 = new byte[0];
			}
			PayloadData payloadData = new PayloadData(array3, flag);
			if (flag && unmask)
			{
				payloadData.Mask(array2);
				wsFrame.Mask = Mask.Unmask;
				wsFrame.MaskingKey = new byte[0];
			}
			wsFrame.PayloadData = payloadData;
			return wsFrame;
		}

		private static string print(WsFrame frame)
		{
			string text = frame.Opcode.ToString();
			byte payloadLen = frame.PayloadLen;
			byte[] extPayloadLen = frame.ExtPayloadLen;
			int num = extPayloadLen.Length;
			string text2 = (num != 2) ? ((num != 8) ? string.Empty : extPayloadLen.ToUInt64(ByteOrder.Big).ToString()) : extPayloadLen.ToUInt16(ByteOrder.Big).ToString();
			bool isMasked = frame.IsMasked;
			string text3 = (!isMasked) ? string.Empty : BitConverter.ToString(frame.MaskingKey);
			string text4 = (payloadLen != 0) ? ((num <= 0) ? ((!isMasked && !frame.IsFragmented && !frame.IsBinary && !frame.IsClose) ? Encoding.UTF8.GetString(frame.PayloadData.ApplicationData) : BitConverter.ToString(frame.PayloadData.ToByteArray())) : string.Format("A {0} data with {1} bytes.", text.ToLower(), text2)) : string.Empty;
			string format = "                 FIN: {0}\n                RSV1: {1}\n                RSV2: {2}\n                RSV3: {3}\n              Opcode: {4}\n                MASK: {5}\n         Payload Len: {6}\nExtended Payload Len: {7}\n         Masking Key: {8}\n        Payload Data: {9}";
			return string.Format(format, new object[]
			{
				frame.Fin,
				frame.Rsv1,
				frame.Rsv2,
				frame.Rsv3,
				text,
				frame.Mask,
				payloadLen,
				text2,
				text3,
				text4
			});
		}

		internal static WsFrame CreateCloseFrame(Mask mask, PayloadData payload)
		{
			return new WsFrame(Opcode.Close, mask, payload);
		}

		internal static WsFrame CreatePongFrame(Mask mask, PayloadData payload)
		{
			return new WsFrame(Opcode.Pong, mask, payload);
		}

		public static WsFrame CreateCloseFrame(Mask mask, byte[] data)
		{
			return new WsFrame(Opcode.Close, mask, new PayloadData(data));
		}

		public static WsFrame CreateCloseFrame(Mask mask, CloseStatusCode code, string reason)
		{
			return new WsFrame(Opcode.Close, mask, new PayloadData(((ushort)code).Append(reason)));
		}

		public static WsFrame CreateFrame(Fin fin, Opcode opcode, Mask mask, byte[] data, bool compressed)
		{
			return new WsFrame(fin, opcode, mask, new PayloadData(data), compressed);
		}

		public static WsFrame CreatePingFrame(Mask mask)
		{
			return new WsFrame(Opcode.Ping, mask, new PayloadData());
		}

		public static WsFrame CreatePingFrame(Mask mask, byte[] data)
		{
			return new WsFrame(Opcode.Ping, mask, new PayloadData(data));
		}

		public IEnumerator<byte> GetEnumerator()
		{
			foreach (byte b in this.ToByteArray())
			{
				yield return b;
			}
			yield break;
		}

		public static WsFrame Parse(byte[] src)
		{
			return WsFrame.Parse(src, true);
		}

		public static WsFrame Parse(Stream stream)
		{
			return WsFrame.Parse(stream, true);
		}

		public static WsFrame Parse(byte[] src, bool unmask)
		{
			WsFrame result;
			using (MemoryStream memoryStream = new MemoryStream(src))
			{
				result = WsFrame.Parse(memoryStream, unmask);
			}
			return result;
		}

		public static WsFrame Parse(Stream stream, bool unmask)
		{
			byte[] array = stream.ReadBytes(2);
			if (array.Length != 2)
			{
				throw new WebSocketException("The header part of a frame cannot be read from the data source.");
			}
			return WsFrame.parse(array, stream, unmask);
		}

		public static void ParseAsync(Stream stream, Action<WsFrame> completed)
		{
			WsFrame.ParseAsync(stream, true, completed, null);
		}

		public static void ParseAsync(Stream stream, Action<WsFrame> completed, Action<Exception> error)
		{
			WsFrame.ParseAsync(stream, true, completed, error);
		}

		public static void ParseAsync(Stream stream, bool unmask, Action<WsFrame> completed, Action<Exception> error)
		{
			stream.ReadBytesAsync(2, delegate(byte[] header)
			{
				if (header.Length != 2)
				{
					throw new WebSocketException("The header part of a frame cannot be read from the data source.");
				}
				WsFrame obj = WsFrame.parse(header, stream, unmask);
				if (completed != null)
				{
					completed(obj);
				}
			}, error);
		}

		public void Print(bool dumped)
		{
			Console.WriteLine((!dumped) ? WsFrame.print(this) : WsFrame.dump(this));
		}

		public string PrintToString(bool dumped)
		{
			return (!dumped) ? WsFrame.print(this) : WsFrame.dump(this);
		}

		public byte[] ToByteArray()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num = (int)this.Fin;
				num = (int)((byte)(num << 1) + this.Rsv1);
				num = (int)((byte)(num << 1) + this.Rsv2);
				num = (int)((byte)(num << 1) + this.Rsv3);
				num = (int)((byte)(num << 4) + this.Opcode);
				num = (int)((byte)(num << 1) + this.Mask);
				num = (num << 7) + (int)this.PayloadLen;
				memoryStream.Write(((ushort)num).ToByteArrayInternally(ByteOrder.Big), 0, 2);
				if (this.PayloadLen > 125)
				{
					memoryStream.Write(this.ExtPayloadLen, 0, this.ExtPayloadLen.Length);
				}
				if (this.Mask == Mask.Mask)
				{
					memoryStream.Write(this.MaskingKey, 0, this.MaskingKey.Length);
				}
				if (this.PayloadLen > 0)
				{
					byte[] array = this.PayloadData.ToByteArray();
					if (this.PayloadLen < 127)
					{
						memoryStream.Write(array, 0, array.Length);
					}
					else
					{
						memoryStream.WriteBytes(array);
					}
				}
				memoryStream.Close();
				result = memoryStream.ToArray();
			}
			return result;
		}

		public override string ToString()
		{
			return BitConverter.ToString(this.ToByteArray());
		}
	}
}
