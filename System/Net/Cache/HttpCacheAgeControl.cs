using System;

namespace System.Net.Cache
{
	/// <summary>Specifies the meaning of time values that control caching behavior for resources obtained using <see cref="T:System.Net.HttpWebRequest" /> objects.</summary>
	public enum HttpCacheAgeControl
	{
		/// <summary>For internal use only. The Framework will throw an <see cref="T:System.ArgumentException" /> if you try to use this member.</summary>
		None,
		/// <summary>Content can be taken from the cache if the time remaining before expiration is greater than or equal to the time specified with this value.</summary>
		MinFresh,
		/// <summary>Content can be taken from the cache until it is older than the age specified with this value.</summary>
		MaxAge,
		/// <summary>
		///   <see cref="P:System.Net.Cache.HttpRequestCachePolicy.MaxAge" /> and <see cref="P:System.Net.Cache.HttpRequestCachePolicy.MinFresh" />.</summary>
		MaxAgeAndMinFresh,
		/// <summary>Content can be taken from the cache after it has expired, until the time specified with this value elapses.</summary>
		MaxStale,
		/// <summary>
		///   <see cref="P:System.Net.Cache.HttpRequestCachePolicy.MaxAge" /> and <see cref="P:System.Net.Cache.HttpRequestCachePolicy.MaxStale" />.</summary>
		MaxAgeAndMaxStale = 6
	}
}
