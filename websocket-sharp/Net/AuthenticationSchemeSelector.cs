using System;

namespace WebSocketSharp.Net
{
	public delegate AuthenticationSchemes AuthenticationSchemeSelector(HttpListenerRequest httpRequest);
}
