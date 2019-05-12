using System;
using System.Net.Security;
using System.Net.Sockets;

namespace WebSocketSharp.Net.Security
{
	internal class SslStream : SslStream
	{
		public SslStream(NetworkStream innerStream) : base(innerStream)
		{
		}

		public SslStream(NetworkStream innerStream, bool leaveInnerStreamOpen) : base(innerStream, leaveInnerStreamOpen)
		{
		}

		public SslStream(NetworkStream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback) : base(innerStream, leaveInnerStreamOpen, userCertificateValidationCallback)
		{
		}

		public SslStream(NetworkStream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback, LocalCertificateSelectionCallback userCertificateSelectionCallback) : base(innerStream, leaveInnerStreamOpen, userCertificateValidationCallback, userCertificateSelectionCallback)
		{
		}

		public bool DataAvailable
		{
			get
			{
				return ((NetworkStream)base.InnerStream).DataAvailable;
			}
		}
	}
}
