using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Substance memory budget.</para>
	/// </summary>
	public enum ProceduralCacheSize
	{
		/// <summary>
		///   <para>A limit of 128MB for the cache or the working memory.</para>
		/// </summary>
		Tiny,
		/// <summary>
		///   <para>A limit of 256MB for the cache or the working memory.</para>
		/// </summary>
		Medium,
		/// <summary>
		///   <para>A limit of 512MB for the cache or the working memory.</para>
		/// </summary>
		Heavy,
		/// <summary>
		///   <para>No limit for the cache or the working memory.</para>
		/// </summary>
		NoLimit,
		/// <summary>
		///   <para>A limit of 1B (one byte) for the cache or the working memory.</para>
		/// </summary>
		None
	}
}
