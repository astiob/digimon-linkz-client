using System;

namespace System.Net
{
	/// <summary>Represents the method that specifies a local Internet Protocol address and port number for a <see cref="T:System.Net.ServicePoint" />.</summary>
	/// <returns>The local <see cref="T:System.Net.IPEndPoint" /> to which the <see cref="T:System.Net.ServicePoint" /> is bound.</returns>
	/// <param name="servicePoint">The <see cref="T:System.Net.ServicePoint" /> associated with the connection to be created.</param>
	/// <param name="remoteEndPoint">The remote <see cref="T:System.Net.IPEndPoint" /> that specifies the remote host.</param>
	/// <param name="retryCount">The number of times this delegate was called for a specified connection.</param>
	/// <exception cref="T:System.OverflowException">
	///   <paramref name="retryCount" /> is equal to <see cref="F:System.Int32.MaxValue" /></exception>
	public delegate IPEndPoint BindIPEndPoint(ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount);
}
