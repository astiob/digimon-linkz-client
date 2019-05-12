using System;

namespace System.Net
{
	/// <summary>Defines status codes for the <see cref="T:System.Net.WebException" /> class.</summary>
	public enum WebExceptionStatus
	{
		/// <summary>No error was encountered.</summary>
		Success,
		/// <summary>The name resolver service could not resolve the host name.</summary>
		NameResolutionFailure,
		/// <summary>The remote service point could not be contacted at the transport level.</summary>
		ConnectFailure,
		/// <summary>A complete response was not received from the remote server.</summary>
		ReceiveFailure,
		/// <summary>A complete request could not be sent to the remote server.</summary>
		SendFailure,
		/// <summary>The request was a piplined request and the connection was closed before the response was received.</summary>
		PipelineFailure,
		/// <summary>The request was canceled, the <see cref="M:System.Net.WebRequest.Abort" /> method was called, or an unclassifiable error occurred. This is the default value for <see cref="P:System.Net.WebException.Status" />.</summary>
		RequestCanceled,
		/// <summary>The response received from the server was complete but indicated a protocol-level error. For example, an HTTP protocol error such as 401 Access Denied would use this status.</summary>
		ProtocolError,
		/// <summary>The connection was prematurely closed.</summary>
		ConnectionClosed,
		/// <summary>A server certificate could not be validated.</summary>
		TrustFailure,
		/// <summary>An error occurred while establishing a connection using SSL.</summary>
		SecureChannelFailure,
		/// <summary>The server response was not a valid HTTP response.</summary>
		ServerProtocolViolation,
		/// <summary>The connection for a request that specifies the Keep-alive header was closed unexpectedly.</summary>
		KeepAliveFailure,
		/// <summary>An internal asynchronous request is pending.</summary>
		Pending,
		/// <summary>No response was received during the time-out period for a request.</summary>
		Timeout,
		/// <summary>The name resolver service could not resolve the proxy host name.</summary>
		ProxyNameResolutionFailure,
		/// <summary>An exception of unknown type has occurred.</summary>
		UnknownError,
		/// <summary>A message was received that exceeded the specified limit when sending a request or receiving a response from the server.</summary>
		MessageLengthLimitExceeded,
		/// <summary>The specified cache entry was not found.</summary>
		CacheEntryNotFound,
		/// <summary>The request was not permitted by the cache policy. In general, this occurs when a request is not cacheable and the effective policy prohibits sending the request to the server. You might receive this status if a request method implies the presence of a request body, a request method requires direct interaction with the server, or a request contains a conditional header.</summary>
		RequestProhibitedByCachePolicy,
		/// <summary>This request was not permitted by the proxy.</summary>
		RequestProhibitedByProxy
	}
}
