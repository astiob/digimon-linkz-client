using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace FacebookGames
{
	public class NamedPipeStream : Stream
	{
		private const uint GENERIC_READ = 2147483648u;

		private const uint GENERIC_WRITE = 1073741824u;

		private const int INVALID_HANDLE_VALUE = -1;

		private const uint FILE_FLAG_OVERLAPPED = 1073741824u;

		private const uint FILE_FLAG_NO_BUFFERING = 536870912u;

		private const uint OPEN_EXISTING = 3u;

		private const uint PIPE_ACCESS_OUTBOUND = 2u;

		private const uint PIPE_ACCESS_DUPLEX = 3u;

		private const uint PIPE_ACCESS_INBOUND = 1u;

		private const uint PIPE_WAIT = 0u;

		private const uint PIPE_NOWAIT = 1u;

		private const uint PIPE_READMODE_BYTE = 0u;

		private const uint PIPE_READMODE_MESSAGE = 2u;

		private const uint PIPE_TYPE_BYTE = 0u;

		private const uint PIPE_TYPE_MESSAGE = 4u;

		private const uint PIPE_CLIENT_END = 0u;

		private const uint PIPE_SERVER_END = 1u;

		private const uint PIPE_UNLIMITED_INSTANCES = 255u;

		private const uint NMPWAIT_WAIT_FOREVER = 4294967295u;

		private const uint NMPWAIT_NOWAIT = 1u;

		private const uint NMPWAIT_USE_DEFAULT_WAIT = 0u;

		private const ulong ERROR_PIPE_CONNECTED = 535UL;

		private const ulong ERROR_MORE_DATA = 234UL;

		private const ulong ERROR_OPERATION_ABORTED = 995UL;

		private IntPtr _handle;

		private FileAccess _mode;

		private readonly NamedPipeStream.PeerType _peerType;

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr CreateNamedPipe(string lpName, uint dwOpenMode, uint dwPipeMode, uint nMaxInstances, uint nOutBufferSize, uint nInBufferSize, uint nDefaultTimeOut, IntPtr pipeSecurityDescriptor);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool DisconnectNamedPipe(IntPtr hHandle);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool ConnectNamedPipe(IntPtr hHandle, IntPtr lpOverlapped);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool PeekNamedPipe(IntPtr handle, byte[] buffer, uint nBufferSize, ref uint bytesRead, ref uint bytesAvail, ref uint BytesLeftThisMessage);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool ReadFile(IntPtr handle, byte[] buffer, uint toRead, ref uint read, IntPtr lpOverLapped);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool WriteFile(IntPtr handle, byte[] buffer, uint count, ref uint written, IntPtr lpOverlapped);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool CloseHandle(IntPtr handle);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FlushFileBuffers(IntPtr handle);

		protected NamedPipeStream()
		{
			this._handle = IntPtr.Zero;
			this._mode = (FileAccess)0;
			this._peerType = NamedPipeStream.PeerType.Server;
		}

		public NamedPipeStream(string pipename, FileAccess mode)
		{
			this._handle = IntPtr.Zero;
			this._peerType = NamedPipeStream.PeerType.Client;
			this.Open(pipename, mode);
		}

		public NamedPipeStream(IntPtr handle, FileAccess mode)
		{
			this._handle = handle;
			this._mode = mode;
			this._peerType = NamedPipeStream.PeerType.Client;
		}

		public void Open(string pipename, FileAccess mode)
		{
			uint num = 0u;
			if ((mode & FileAccess.Read) > (FileAccess)0)
			{
				num |= 2147483648u;
			}
			if ((mode & FileAccess.Write) > (FileAccess)0)
			{
				num |= 1073741824u;
			}
			IntPtr handle = NamedPipeStream.CreateFile(pipename, num, 0u, IntPtr.Zero, 3u, 0u, IntPtr.Zero);
			if (handle.ToInt32() == -1)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new Win32Exception(lastWin32Error, string.Format("NamedPipeStream.Open failed, win32 error code {0}, pipename '{1}' ", lastWin32Error, pipename));
			}
			this._mode = mode;
			this._handle = handle;
		}

		public static NamedPipeStream Create(string pipeName, NamedPipeStream.ServerMode mode, uint maxInstances, uint outBufferSize, uint inBufferSize, uint timeout)
		{
			IntPtr handle = IntPtr.Zero;
			string text = "\\\\.\\pipe\\" + pipeName;
			handle = NamedPipeStream.CreateNamedPipe(text, (uint)mode, 4u, maxInstances, outBufferSize, inBufferSize, timeout, IntPtr.Zero);
			if (handle.ToInt32() == -1)
			{
				throw new Win32Exception(string.Concat(new object[]
				{
					"Error creating named pipe ",
					text,
					" . Internal error: ",
					Marshal.GetLastWin32Error()
				}));
			}
			NamedPipeStream namedPipeStream = new NamedPipeStream();
			namedPipeStream._handle = handle;
			switch (mode)
			{
			case NamedPipeStream.ServerMode.InboundOnly:
				namedPipeStream._mode = FileAccess.Read;
				break;
			case NamedPipeStream.ServerMode.OutboundOnly:
				namedPipeStream._mode = FileAccess.Write;
				break;
			case NamedPipeStream.ServerMode.Bidirectional:
				namedPipeStream._mode = FileAccess.ReadWrite;
				break;
			}
			return namedPipeStream;
		}

		public bool Listen()
		{
			if (this._peerType != NamedPipeStream.PeerType.Server)
			{
				throw new Exception("Listen() is only for server-side streams");
			}
			NamedPipeStream.DisconnectNamedPipe(this._handle);
			if (!NamedPipeStream.ConnectNamedPipe(this._handle, IntPtr.Zero))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if ((ulong)lastWin32Error == 995UL)
				{
					this.WasOperationAborted = true;
				}
				return (ulong)lastWin32Error == 535UL;
			}
			return true;
		}

		public void Disconnect()
		{
			if (this._peerType != NamedPipeStream.PeerType.Server)
			{
				throw new Exception("Disconnect() is only for server-side streams");
			}
			NamedPipeStream.DisconnectNamedPipe(this._handle);
		}

		public bool IsConnected
		{
			get
			{
				if (this._peerType != NamedPipeStream.PeerType.Server)
				{
					throw new Exception("IsConnected() is only for server-side streams");
				}
				return !NamedPipeStream.ConnectNamedPipe(this._handle, IntPtr.Zero) && (ulong)Marshal.GetLastWin32Error() == 535UL;
			}
		}

		public bool WasOperationAborted { get; protected set; }

		public bool IsMessageComplete { get; protected set; }

		public bool DataAvailable
		{
			get
			{
				uint num = 0u;
				uint num2 = 0u;
				uint num3 = 0u;
				return NamedPipeStream.PeekNamedPipe(this._handle, null, 0u, ref num, ref num2, ref num3) && num2 > 0u;
			}
		}

		public override bool CanRead
		{
			get
			{
				return (this._mode & FileAccess.Read) > (FileAccess)0;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return (this._mode & FileAccess.Write) > (FileAccess)0;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override long Length
		{
			get
			{
				throw new NotSupportedException("NamedPipeStream does not support seeking");
			}
		}

		public override long Position
		{
			get
			{
				throw new NotSupportedException("NamedPipeStream does not support seeking");
			}
			set
			{
			}
		}

		public override void Flush()
		{
			if (this._handle == IntPtr.Zero)
			{
				throw new ObjectDisposedException("NamedPipeStream", "The stream has already been closed");
			}
			NamedPipeStream.FlushFileBuffers(this._handle);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "The buffer to read into cannot be null");
			}
			if (buffer.Length < offset + count)
			{
				throw new ArgumentException("Buffer is not large enough to hold requested data", "buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", offset, "Offset cannot be negative");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", count, "Count cannot be negative");
			}
			if (!this.CanRead)
			{
				throw new NotSupportedException("The stream does not support reading");
			}
			if (this._handle == IntPtr.Zero)
			{
				throw new ObjectDisposedException("NamedPipeStream", "The stream has already been closed");
			}
			uint num = 0u;
			byte[] array = buffer;
			if (offset != 0)
			{
				array = new byte[count];
			}
			if (!NamedPipeStream.ReadFile(this._handle, array, (uint)count, ref num, IntPtr.Zero))
			{
				return -1;
			}
			uint lastWin32Error = (uint)Marshal.GetLastWin32Error();
			this.IsMessageComplete = ((ulong)lastWin32Error != 234UL);
			if (offset != 0)
			{
				Array.Copy(array, 0L, array, (long)offset, (long)((ulong)num));
			}
			return (int)num;
		}

		public override void Close()
		{
			NamedPipeStream.CloseHandle(this._handle);
			this._handle = IntPtr.Zero;
		}

		public override void SetLength(long length)
		{
			throw new NotSupportedException("NamedPipeStream doesn't support SetLength");
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "The buffer to write into cannot be null");
			}
			if (buffer.Length < offset + count)
			{
				throw new ArgumentException("Buffer does not contain amount of requested data", "buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", offset, "Offset cannot be negative");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", count, "Count cannot be negative");
			}
			if (!this.CanWrite)
			{
				throw new NotSupportedException("The stream does not support writing");
			}
			if (this._handle == IntPtr.Zero)
			{
				throw new ObjectDisposedException("NamedPipeStream", "The stream has already been closed");
			}
			if (offset != 0)
			{
				byte[] array = new byte[count];
				Array.Copy(buffer, offset, array, 0, count);
				buffer = array;
			}
			uint num = 0u;
			if (!NamedPipeStream.WriteFile(this._handle, buffer, (uint)count, ref num, IntPtr.Zero))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Writing to the stream failed");
			}
			if ((ulong)num < (ulong)((long)count))
			{
				throw new IOException("Unable to write entire buffer to stream");
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("NamedPipeStream doesn't support seeking");
		}

		public struct SECURITY_ATTRIBUTES
		{
			public int nLength;

			public IntPtr lpSecurityDescriptor;

			public int bInheritHandle;
		}

		public enum ServerMode
		{
			InboundOnly = 1,
			OutboundOnly,
			Bidirectional
		}

		public enum PeerType
		{
			Client,
			Server
		}
	}
}
