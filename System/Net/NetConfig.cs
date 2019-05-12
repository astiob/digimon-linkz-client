using System;

namespace System.Net
{
	internal class NetConfig : ICloneable
	{
		internal bool ipv6Enabled;

		internal int MaxResponseHeadersLength = 64;

		internal NetConfig()
		{
		}

		object ICloneable.Clone()
		{
			return base.MemberwiseClone();
		}
	}
}
