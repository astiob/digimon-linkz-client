using System;

namespace System.Net
{
	/// <summary>Contains the values of status codes defined for HTTP.</summary>
	public enum HttpStatusCode
	{
		/// <summary>Equivalent to HTTP status 100. <see cref="F:System.Net.HttpStatusCode.Continue" /> indicates that the client can continue with its request.</summary>
		Continue = 100,
		/// <summary>Equivalent to HTTP status 101. <see cref="F:System.Net.HttpStatusCode.SwitchingProtocols" /> indicates that the protocol version or protocol is being changed.</summary>
		SwitchingProtocols,
		/// <summary>Equivalent to HTTP status 200. <see cref="F:System.Net.HttpStatusCode.OK" /> indicates that the request succeeded and that the requested information is in the response. This is the most common status code to receive.</summary>
		OK = 200,
		/// <summary>Equivalent to HTTP status 201. <see cref="F:System.Net.HttpStatusCode.Created" /> indicates that the request resulted in a new resource created before the response was sent.</summary>
		Created,
		/// <summary>Equivalent to HTTP status 202. <see cref="F:System.Net.HttpStatusCode.Accepted" /> indicates that the request has been accepted for further processing.</summary>
		Accepted,
		/// <summary>Equivalent to HTTP status 203. <see cref="F:System.Net.HttpStatusCode.NonAuthoritativeInformation" /> indicates that the returned metainformation is from a cached copy instead of the origin server and therefore may be incorrect.</summary>
		NonAuthoritativeInformation,
		/// <summary>Equivalent to HTTP status 204. <see cref="F:System.Net.HttpStatusCode.NoContent" /> indicates that the request has been successfully processed and that the response is intentionally blank.</summary>
		NoContent,
		/// <summary>Equivalent to HTTP status 205. <see cref="F:System.Net.HttpStatusCode.ResetContent" /> indicates that the client should reset (not reload) the current resource.</summary>
		ResetContent,
		/// <summary>Equivalent to HTTP status 206. <see cref="F:System.Net.HttpStatusCode.PartialContent" /> indicates that the response is a partial response as requested by a GET request that includes a byte range.</summary>
		PartialContent,
		/// <summary>Equivalent to HTTP status 300. <see cref="F:System.Net.HttpStatusCode.MultipleChoices" /> indicates that the requested information has multiple representations. The default action is to treat this status as a redirect and follow the contents of the Location header associated with this response.</summary>
		MultipleChoices = 300,
		/// <summary>Equivalent to HTTP status 300. <see cref="F:System.Net.HttpStatusCode.Ambiguous" /> indicates that the requested information has multiple representations. The default action is to treat this status as a redirect and follow the contents of the Location header associated with this response.</summary>
		Ambiguous = 300,
		/// <summary>Equivalent to HTTP status 301. <see cref="F:System.Net.HttpStatusCode.MovedPermanently" /> indicates that the requested information has been moved to the URI specified in the Location header. The default action when this status is received is to follow the Location header associated with the response.</summary>
		MovedPermanently,
		/// <summary>Equivalent to HTTP status 301. <see cref="F:System.Net.HttpStatusCode.Moved" /> indicates that the requested information has been moved to the URI specified in the Location header. The default action when this status is received is to follow the Location header associated with the response. When the original request method was POST, the redirected request will use the GET method.</summary>
		Moved = 301,
		/// <summary>Equivalent to HTTP status 302. <see cref="F:System.Net.HttpStatusCode.Found" /> indicates that the requested information is located at the URI specified in the Location header. The default action when this status is received is to follow the Location header associated with the response. When the original request method was POST, the redirected request will use the GET method.</summary>
		Found,
		/// <summary>Equivalent to HTTP status 302. <see cref="F:System.Net.HttpStatusCode.Redirect" /> indicates that the requested information is located at the URI specified in the Location header. The default action when this status is received is to follow the Location header associated with the response. When the original request method was POST, the redirected request will use the GET method.</summary>
		Redirect = 302,
		/// <summary>Equivalent to HTTP status 303. <see cref="F:System.Net.HttpStatusCode.SeeOther" /> automatically redirects the client to the URI specified in the Location header as the result of a POST. The request to the resource specified by the Location header will be made with a GET.</summary>
		SeeOther,
		/// <summary>Equivalent to HTTP status 303. <see cref="F:System.Net.HttpStatusCode.RedirectMethod" /> automatically redirects the client to the URI specified in the Location header as the result of a POST. The request to the resource specified by the Location header will be made with a GET.</summary>
		RedirectMethod = 303,
		/// <summary>Equivalent to HTTP status 304. <see cref="F:System.Net.HttpStatusCode.NotModified" /> indicates that the client's cached copy is up to date. The contents of the resource are not transferred.</summary>
		NotModified,
		/// <summary>Equivalent to HTTP status 305. <see cref="F:System.Net.HttpStatusCode.UseProxy" /> indicates that the request should use the proxy server at the URI specified in the Location header.</summary>
		UseProxy,
		/// <summary>Equivalent to HTTP status 306. <see cref="F:System.Net.HttpStatusCode.Unused" /> is a proposed extension to the HTTP/1.1 specification that is not fully specified.</summary>
		Unused,
		/// <summary>Equivalent to HTTP status 307. <see cref="F:System.Net.HttpStatusCode.TemporaryRedirect" /> indicates that the request information is located at the URI specified in the Location header. The default action when this status is received is to follow the Location header associated with the response. When the original request method was POST, the redirected request will also use the POST method.</summary>
		TemporaryRedirect,
		/// <summary>Equivalent to HTTP status 307. <see cref="F:System.Net.HttpStatusCode.RedirectKeepVerb" /> indicates that the request information is located at the URI specified in the Location header. The default action when this status is received is to follow the Location header associated with the response. When the original request method was POST, the redirected request will also use the POST method.</summary>
		RedirectKeepVerb = 307,
		/// <summary>Equivalent to HTTP status 400. <see cref="F:System.Net.HttpStatusCode.BadRequest" /> indicates that the request could not be understood by the server. <see cref="F:System.Net.HttpStatusCode.BadRequest" /> is sent when no other error is applicable, or if the exact error is unknown or does not have its own error code.</summary>
		BadRequest = 400,
		/// <summary>Equivalent to HTTP status 401. <see cref="F:System.Net.HttpStatusCode.Unauthorized" /> indicates that the requested resource requires authentication. The WWW-Authenticate header contains the details of how to perform the authentication.</summary>
		Unauthorized,
		/// <summary>Equivalent to HTTP status 402. <see cref="F:System.Net.HttpStatusCode.PaymentRequired" /> is reserved for future use.</summary>
		PaymentRequired,
		/// <summary>Equivalent to HTTP status 403. <see cref="F:System.Net.HttpStatusCode.Forbidden" /> indicates that the server refuses to fulfill the request.</summary>
		Forbidden,
		/// <summary>Equivalent to HTTP status 404. <see cref="F:System.Net.HttpStatusCode.NotFound" /> indicates that the requested resource does not exist on the server.</summary>
		NotFound,
		/// <summary>Equivalent to HTTP status 405. <see cref="F:System.Net.HttpStatusCode.MethodNotAllowed" /> indicates that the request method (POST or GET) is not allowed on the requested resource.</summary>
		MethodNotAllowed,
		/// <summary>Equivalent to HTTP status 406. <see cref="F:System.Net.HttpStatusCode.NotAcceptable" /> indicates that the client has indicated with Accept headers that it will not accept any of the available representations of the resource.</summary>
		NotAcceptable,
		/// <summary>Equivalent to HTTP status 407. <see cref="F:System.Net.HttpStatusCode.ProxyAuthenticationRequired" /> indicates that the requested proxy requires authentication. The Proxy-authenticate header contains the details of how to perform the authentication.</summary>
		ProxyAuthenticationRequired,
		/// <summary>Equivalent to HTTP status 408. <see cref="F:System.Net.HttpStatusCode.RequestTimeout" /> indicates that the client did not send a request within the time the server was expecting the request.</summary>
		RequestTimeout,
		/// <summary>Equivalent to HTTP status 409. <see cref="F:System.Net.HttpStatusCode.Conflict" /> indicates that the request could not be carried out because of a conflict on the server.</summary>
		Conflict,
		/// <summary>Equivalent to HTTP status 410. <see cref="F:System.Net.HttpStatusCode.Gone" /> indicates that the requested resource is no longer available.</summary>
		Gone,
		/// <summary>Equivalent to HTTP status 411. <see cref="F:System.Net.HttpStatusCode.LengthRequired" /> indicates that the required Content-length header is missing.</summary>
		LengthRequired,
		/// <summary>Equivalent to HTTP status 412. <see cref="F:System.Net.HttpStatusCode.PreconditionFailed" /> indicates that a condition set for this request failed, and the request cannot be carried out. Conditions are set with conditional request headers like If-Match, If-None-Match, or If-Unmodified-Since.</summary>
		PreconditionFailed,
		/// <summary>Equivalent to HTTP status 413. <see cref="F:System.Net.HttpStatusCode.RequestEntityTooLarge" /> indicates that the request is too large for the server to process.</summary>
		RequestEntityTooLarge,
		/// <summary>Equivalent to HTTP status 414. <see cref="F:System.Net.HttpStatusCode.RequestUriTooLong" /> indicates that the URI is too long.</summary>
		RequestUriTooLong,
		/// <summary>Equivalent to HTTP status 415. <see cref="F:System.Net.HttpStatusCode.UnsupportedMediaType" /> indicates that the request is an unsupported type.</summary>
		UnsupportedMediaType,
		/// <summary>Equivalent to HTTP status 416. <see cref="F:System.Net.HttpStatusCode.RequestedRangeNotSatisfiable" /> indicates that the range of data requested from the resource cannot be returned, either because the beginning of the range is before the beginning of the resource, or the end of the range is after the end of the resource.</summary>
		RequestedRangeNotSatisfiable,
		/// <summary>Equivalent to HTTP status 417. <see cref="F:System.Net.HttpStatusCode.ExpectationFailed" /> indicates that an expectation given in an Expect header could not be met by the server.</summary>
		ExpectationFailed,
		/// <summary>Equivalent to HTTP status 500. <see cref="F:System.Net.HttpStatusCode.InternalServerError" /> indicates that a generic error has occurred on the server.</summary>
		InternalServerError = 500,
		/// <summary>Equivalent to HTTP status 501. <see cref="F:System.Net.HttpStatusCode.NotImplemented" /> indicates that the server does not support the requested function.</summary>
		NotImplemented,
		/// <summary>Equivalent to HTTP status 502. <see cref="F:System.Net.HttpStatusCode.BadGateway" /> indicates that an intermediate proxy server received a bad response from another proxy or the origin server.</summary>
		BadGateway,
		/// <summary>Equivalent to HTTP status 503. <see cref="F:System.Net.HttpStatusCode.ServiceUnavailable" /> indicates that the server is temporarily unavailable, usually due to high load or maintenance.</summary>
		ServiceUnavailable,
		/// <summary>Equivalent to HTTP status 504. <see cref="F:System.Net.HttpStatusCode.GatewayTimeout" /> indicates that an intermediate proxy server timed out while waiting for a response from another proxy or the origin server.</summary>
		GatewayTimeout,
		/// <summary>Equivalent to HTTP status 505. <see cref="F:System.Net.HttpStatusCode.HttpVersionNotSupported" /> indicates that the requested HTTP version is not supported by the server.</summary>
		HttpVersionNotSupported
	}
}
