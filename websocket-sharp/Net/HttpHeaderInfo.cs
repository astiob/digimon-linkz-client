using System;

namespace WebSocketSharp.Net
{
	internal class HttpHeaderInfo
	{
		public bool IsMultiValueInRequest
		{
			get
			{
				return (this.Type & HttpHeaderType.MultiValueInRequest) == HttpHeaderType.MultiValueInRequest;
			}
		}

		public bool IsMultiValueInResponse
		{
			get
			{
				return (this.Type & HttpHeaderType.MultiValueInResponse) == HttpHeaderType.MultiValueInResponse;
			}
		}

		public bool IsRequest
		{
			get
			{
				return (this.Type & HttpHeaderType.Request) == HttpHeaderType.Request;
			}
		}

		public bool IsResponse
		{
			get
			{
				return (this.Type & HttpHeaderType.Response) == HttpHeaderType.Response;
			}
		}

		public string Name { get; set; }

		public HttpHeaderType Type { get; set; }

		public bool IsMultiValue(bool response)
		{
			return ((this.Type & HttpHeaderType.MultiValue) == HttpHeaderType.MultiValue) ? ((!response) ? this.IsRequest : this.IsResponse) : ((!response) ? this.IsMultiValueInRequest : this.IsMultiValueInResponse);
		}

		public bool IsRestricted(bool response)
		{
			return (this.Type & HttpHeaderType.Restricted) == HttpHeaderType.Restricted && ((!response) ? this.IsRequest : this.IsResponse);
		}
	}
}
