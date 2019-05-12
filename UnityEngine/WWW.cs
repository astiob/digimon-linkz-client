using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Simple access to web pages.</para>
	/// </summary>
	public sealed class WWW : IDisposable
	{
		internal IntPtr m_Ptr;

		/// <summary>
		///   <para>Creates a WWW request with the given URL.</para>
		/// </summary>
		/// <param name="url">The url to download. Must be '%' escaped.</param>
		/// <returns>
		///   <para>A new WWW object. When it has been downloaded, the results can be fetched from the returned object.</para>
		/// </returns>
		public WWW(string url)
		{
			this.InitWWW(url, null, null);
		}

		/// <summary>
		///   <para>Creates a WWW request with the given URL.</para>
		/// </summary>
		/// <param name="url">The url to download. Must be '%' escaped.</param>
		/// <param name="form">A WWWForm instance containing the form data to post.</param>
		/// <returns>
		///   <para>A new WWW object. When it has been downloaded, the results can be fetched from the returned object.</para>
		/// </returns>
		public WWW(string url, WWWForm form)
		{
			string[] iHeaders = WWW.FlattenedHeadersFrom(form.headers);
			this.InitWWW(url, form.data, iHeaders);
		}

		/// <summary>
		///   <para>Creates a WWW request with the given URL.</para>
		/// </summary>
		/// <param name="url">The url to download. Must be '%' escaped.</param>
		/// <param name="postData">A byte array of data to be posted to the url.</param>
		/// <returns>
		///   <para>A new WWW object. When it has been downloaded, the results can be fetched from the returned object.</para>
		/// </returns>
		public WWW(string url, byte[] postData)
		{
			this.InitWWW(url, postData, null);
		}

		/// <summary>
		///   <para>Creates a WWW request with the given URL.</para>
		/// </summary>
		/// <param name="url">The url to download. Must be '%' escaped.</param>
		/// <param name="postData">A byte array of data to be posted to the url.</param>
		/// <param name="headers">A hash table of custom headers to send with the request.</param>
		/// <returns>
		///   <para>A new WWW object. When it has been downloaded, the results can be fetched from the returned object.</para>
		/// </returns>
		[Obsolete("This overload is deprecated. Use UnityEngine.WWW.WWW(string, byte[], System.Collections.Generic.Dictionary<string, string>) instead.", true)]
		public WWW(string url, byte[] postData, Hashtable headers)
		{
			Debug.LogError("This overload is deprecated. Use UnityEngine.WWW.WWW(string, byte[], System.Collections.Generic.Dictionary<string, string>) instead");
		}

		public WWW(string url, byte[] postData, Dictionary<string, string> headers)
		{
			string[] iHeaders = WWW.FlattenedHeadersFrom(headers);
			this.InitWWW(url, postData, iHeaders);
		}

		internal WWW(string url, Hash128 hash, uint crc)
		{
			WWW.INTERNAL_CALL_WWW(this, url, ref hash, crc);
		}

		/// <summary>
		///   <para>Disposes of an existing WWW object.</para>
		/// </summary>
		public void Dispose()
		{
			this.DestroyWWW(true);
		}

		~WWW()
		{
			this.DestroyWWW(false);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DestroyWWW(bool cancel);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void InitWWW(string url, byte[] postData, string[] iHeaders);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool enforceWebSecurityRestrictions();

		/// <summary>
		///   <para>Escapes characters in a string to ensure they are URL-friendly.</para>
		/// </summary>
		/// <param name="s">A string with characters to be escaped.</param>
		/// <param name="e">The text encoding to use.</param>
		[ExcludeFromDocs]
		public static string EscapeURL(string s)
		{
			Encoding utf = Encoding.UTF8;
			return WWW.EscapeURL(s, utf);
		}

		/// <summary>
		///   <para>Escapes characters in a string to ensure they are URL-friendly.</para>
		/// </summary>
		/// <param name="s">A string with characters to be escaped.</param>
		/// <param name="e">The text encoding to use.</param>
		public static string EscapeURL(string s, [DefaultValue("System.Text.Encoding.UTF8")] Encoding e)
		{
			if (s == null)
			{
				return null;
			}
			if (s == string.Empty)
			{
				return string.Empty;
			}
			if (e == null)
			{
				return null;
			}
			return WWWTranscoder.URLEncode(s, e);
		}

		/// <summary>
		///   <para>Converts URL-friendly escape sequences back to normal text.</para>
		/// </summary>
		/// <param name="s">A string containing escaped characters.</param>
		/// <param name="e">The text encoding to use.</param>
		[ExcludeFromDocs]
		public static string UnEscapeURL(string s)
		{
			Encoding utf = Encoding.UTF8;
			return WWW.UnEscapeURL(s, utf);
		}

		/// <summary>
		///   <para>Converts URL-friendly escape sequences back to normal text.</para>
		/// </summary>
		/// <param name="s">A string containing escaped characters.</param>
		/// <param name="e">The text encoding to use.</param>
		public static string UnEscapeURL(string s, [DefaultValue("System.Text.Encoding.UTF8")] Encoding e)
		{
			if (s == null)
			{
				return null;
			}
			if (s.IndexOf('%') == -1 && s.IndexOf('+') == -1)
			{
				return s;
			}
			return WWWTranscoder.URLDecode(s, e);
		}

		/// <summary>
		///   <para>Dictionary of headers returned by the request.</para>
		/// </summary>
		public Dictionary<string, string> responseHeaders
		{
			get
			{
				if (!this.isDone)
				{
					throw new UnityException("WWW is not finished downloading yet");
				}
				return WWW.ParseHTTPHeaderString(this.responseHeadersString);
			}
		}

		private extern string responseHeadersString { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns the contents of the fetched web page as a string (Read Only).</para>
		/// </summary>
		public string text
		{
			get
			{
				if (!this.isDone)
				{
					throw new UnityException("WWW is not ready downloading yet");
				}
				byte[] bytes = this.bytes;
				return this.GetTextEncoder().GetString(bytes, 0, bytes.Length);
			}
		}

		internal static Encoding DefaultEncoding
		{
			get
			{
				return Encoding.ASCII;
			}
		}

		private Encoding GetTextEncoder()
		{
			string text = null;
			if (this.responseHeaders.TryGetValue("CONTENT-TYPE", out text))
			{
				int num = text.IndexOf("charset", StringComparison.OrdinalIgnoreCase);
				if (num > -1)
				{
					int num2 = text.IndexOf('=', num);
					if (num2 > -1)
					{
						string text2 = text.Substring(num2 + 1).Trim().Trim(new char[]
						{
							'\'',
							'"'
						}).Trim();
						int num3 = text2.IndexOf(';');
						if (num3 > -1)
						{
							text2 = text2.Substring(0, num3);
						}
						try
						{
							return Encoding.GetEncoding(text2);
						}
						catch (Exception)
						{
							Debug.Log("Unsupported encoding: '" + text2 + "'");
						}
					}
				}
			}
			return Encoding.UTF8;
		}

		[Obsolete("Please use WWW.text instead")]
		public string data
		{
			get
			{
				return this.text;
			}
		}

		/// <summary>
		///   <para>Returns the contents of the fetched web page as a byte array (Read Only).</para>
		/// </summary>
		public extern byte[] bytes { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern int size { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns an error message if there was an error during the download (Read Only).</para>
		/// </summary>
		public extern string error { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Texture2D GetTexture(bool markNonReadable);

		/// <summary>
		///   <para>Returns a Texture2D generated from the downloaded data (Read Only).</para>
		/// </summary>
		public Texture2D texture
		{
			get
			{
				return this.GetTexture(false);
			}
		}

		/// <summary>
		///   <para>Returns a non-readable Texture2D generated from the downloaded data (Read Only).</para>
		/// </summary>
		public Texture2D textureNonReadable
		{
			get
			{
				return this.GetTexture(true);
			}
		}

		/// <summary>
		///   <para>Returns a AudioClip generated from the downloaded data (Read Only).</para>
		/// </summary>
		public AudioClip audioClip
		{
			get
			{
				return this.GetAudioClip(true);
			}
		}

		/// <summary>
		///   <para>Returns an AudioClip generated from the downloaded data (Read Only).</para>
		/// </summary>
		/// <param name="threeD">Use this to specify whether the clip should be a 2D or 3D clip
		/// the .audioClip property defaults to 3D.</param>
		/// <param name="stream">Sets whether the clip should be completely downloaded before it's ready to play (false) or the stream can be played even if only part of the clip is downloaded (true).
		/// The latter will disable seeking on the clip (with .time and/or .timeSamples).</param>
		/// <param name="audioType">The AudioType of the content your downloading. If this is not set Unity will try to determine the type from URL.</param>
		/// <returns>
		///   <para>The returned AudioClip.</para>
		/// </returns>
		public AudioClip GetAudioClip(bool threeD)
		{
			return this.GetAudioClip(threeD, false);
		}

		/// <summary>
		///   <para>Returns an AudioClip generated from the downloaded data (Read Only).</para>
		/// </summary>
		/// <param name="threeD">Use this to specify whether the clip should be a 2D or 3D clip
		/// the .audioClip property defaults to 3D.</param>
		/// <param name="stream">Sets whether the clip should be completely downloaded before it's ready to play (false) or the stream can be played even if only part of the clip is downloaded (true).
		/// The latter will disable seeking on the clip (with .time and/or .timeSamples).</param>
		/// <param name="audioType">The AudioType of the content your downloading. If this is not set Unity will try to determine the type from URL.</param>
		/// <returns>
		///   <para>The returned AudioClip.</para>
		/// </returns>
		public AudioClip GetAudioClip(bool threeD, bool stream)
		{
			return this.GetAudioClip(threeD, stream, AudioType.UNKNOWN);
		}

		/// <summary>
		///   <para>Returns an AudioClip generated from the downloaded data (Read Only).</para>
		/// </summary>
		/// <param name="threeD">Use this to specify whether the clip should be a 2D or 3D clip
		/// the .audioClip property defaults to 3D.</param>
		/// <param name="stream">Sets whether the clip should be completely downloaded before it's ready to play (false) or the stream can be played even if only part of the clip is downloaded (true).
		/// The latter will disable seeking on the clip (with .time and/or .timeSamples).</param>
		/// <param name="audioType">The AudioType of the content your downloading. If this is not set Unity will try to determine the type from URL.</param>
		/// <returns>
		///   <para>The returned AudioClip.</para>
		/// </returns>
		public AudioClip GetAudioClip(bool threeD, bool stream, AudioType audioType)
		{
			return this.GetAudioClipInternal(threeD, stream, false, audioType);
		}

		/// <summary>
		///   <para>Returns an AudioClip generated from the downloaded data that is compressed in memory (Read Only).</para>
		/// </summary>
		/// <param name="threeD">Use this to specify whether the clip should be a 2D or 3D clip.</param>
		/// <param name="audioType">The AudioType of the content your downloading. If this is not set Unity will try to determine the type from URL.</param>
		/// <returns>
		///   <para>The returned AudioClip.</para>
		/// </returns>
		public AudioClip GetAudioClipCompressed()
		{
			return this.GetAudioClipCompressed(true);
		}

		/// <summary>
		///   <para>Returns an AudioClip generated from the downloaded data that is compressed in memory (Read Only).</para>
		/// </summary>
		/// <param name="threeD">Use this to specify whether the clip should be a 2D or 3D clip.</param>
		/// <param name="audioType">The AudioType of the content your downloading. If this is not set Unity will try to determine the type from URL.</param>
		/// <returns>
		///   <para>The returned AudioClip.</para>
		/// </returns>
		public AudioClip GetAudioClipCompressed(bool threeD)
		{
			return this.GetAudioClipCompressed(threeD, AudioType.UNKNOWN);
		}

		/// <summary>
		///   <para>Returns an AudioClip generated from the downloaded data that is compressed in memory (Read Only).</para>
		/// </summary>
		/// <param name="threeD">Use this to specify whether the clip should be a 2D or 3D clip.</param>
		/// <param name="audioType">The AudioType of the content your downloading. If this is not set Unity will try to determine the type from URL.</param>
		/// <returns>
		///   <para>The returned AudioClip.</para>
		/// </returns>
		public AudioClip GetAudioClipCompressed(bool threeD, AudioType audioType)
		{
			return this.GetAudioClipInternal(threeD, false, true, audioType);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern AudioClip GetAudioClipInternal(bool threeD, bool stream, bool compressed, AudioType audioType);

		/// <summary>
		///   <para>Replaces the contents of an existing Texture2D with an image from the downloaded data.</para>
		/// </summary>
		/// <param name="tex">An existing texture object to be overwritten with the image data.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void LoadImageIntoTexture(Texture2D tex);

		/// <summary>
		///   <para>Is the download already finished? (Read Only)</para>
		/// </summary>
		public extern bool isDone { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[Obsolete("All blocking WWW functions have been deprecated, please use one of the asynchronous functions instead.", true)]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetURL(string url);

		[Obsolete("All blocking WWW functions have been deprecated, please use one of the asynchronous functions instead.", true)]
		public static Texture2D GetTextureFromURL(string url)
		{
			return new WWW(url).texture;
		}

		/// <summary>
		///   <para>How far has the download progressed (Read Only).</para>
		/// </summary>
		public extern float progress { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>How far has the upload progressed (Read Only).</para>
		/// </summary>
		public extern float uploadProgress { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The number of bytes downloaded by this WWW query (read only).</para>
		/// </summary>
		public extern int bytesDownloaded { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Load an Ogg Vorbis file into the audio clip.</para>
		/// </summary>
		[Obsolete("Property WWW.oggVorbis has been deprecated. Use WWW.audioClip instead (UnityUpgradable).", true)]
		public AudioClip oggVorbis
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		///   <para>Loads the new web player data file.</para>
		/// </summary>
		[Obsolete("LoadUnityWeb is no longer supported. Please use javascript to reload the web player on a different url instead", true)]
		public void LoadUnityWeb()
		{
		}

		/// <summary>
		///   <para>The URL of this WWW request (Read Only).</para>
		/// </summary>
		public extern string url { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Streams an AssetBundle that can contain any kind of asset from the project folder.</para>
		/// </summary>
		public extern AssetBundle assetBundle { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Priority of AssetBundle decompression thread.</para>
		/// </summary>
		public extern ThreadPriority threadPriority { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_WWW(WWW self, string url, ref Hash128 hash, uint crc);

		/// <summary>
		///   <para>Loads an AssetBundle with the specified version number from the cache. If the AssetBundle is not currently cached, it will automatically be downloaded and stored in the cache for future retrieval from local storage.</para>
		/// </summary>
		/// <param name="url">The URL to download the AssetBundle from, if it is not present in the cache. Must be '%' escaped.</param>
		/// <param name="version">Version of the AssetBundle. The file will only be loaded from the disk cache if it has previously been downloaded with the same version parameter. By incrementing the version number requested by your application, you can force Caching to download a new copy of the AssetBundle from url.</param>
		/// <param name="crc">An optional CRC-32 Checksum of the uncompressed contents. If this is non-zero, then the content will be compared against the checksum before loading it, and give an error if it does not match. You can use this to avoid data corruption from bad downloads or users tampering with the cached files on disk. If the CRC does not match, Unity will try to redownload the data, and if the CRC on the server does not match it will fail with an error. Look at the error string returned to see the correct CRC value to use for an AssetBundle.</param>
		/// <returns>
		///   <para>A WWW instance, which can be used to access the data once the load/download operation is completed.</para>
		/// </returns>
		[ExcludeFromDocs]
		public static WWW LoadFromCacheOrDownload(string url, int version)
		{
			uint crc = 0u;
			return WWW.LoadFromCacheOrDownload(url, version, crc);
		}

		/// <summary>
		///   <para>Loads an AssetBundle with the specified version number from the cache. If the AssetBundle is not currently cached, it will automatically be downloaded and stored in the cache for future retrieval from local storage.</para>
		/// </summary>
		/// <param name="url">The URL to download the AssetBundle from, if it is not present in the cache. Must be '%' escaped.</param>
		/// <param name="version">Version of the AssetBundle. The file will only be loaded from the disk cache if it has previously been downloaded with the same version parameter. By incrementing the version number requested by your application, you can force Caching to download a new copy of the AssetBundle from url.</param>
		/// <param name="crc">An optional CRC-32 Checksum of the uncompressed contents. If this is non-zero, then the content will be compared against the checksum before loading it, and give an error if it does not match. You can use this to avoid data corruption from bad downloads or users tampering with the cached files on disk. If the CRC does not match, Unity will try to redownload the data, and if the CRC on the server does not match it will fail with an error. Look at the error string returned to see the correct CRC value to use for an AssetBundle.</param>
		/// <returns>
		///   <para>A WWW instance, which can be used to access the data once the load/download operation is completed.</para>
		/// </returns>
		public static WWW LoadFromCacheOrDownload(string url, int version, [DefaultValue("0")] uint crc)
		{
			Hash128 hash = new Hash128(0u, 0u, 0u, (uint)version);
			return WWW.LoadFromCacheOrDownload(url, hash, crc);
		}

		[ExcludeFromDocs]
		public static WWW LoadFromCacheOrDownload(string url, Hash128 hash)
		{
			uint crc = 0u;
			return WWW.LoadFromCacheOrDownload(url, hash, crc);
		}

		public static WWW LoadFromCacheOrDownload(string url, Hash128 hash, [DefaultValue("0")] uint crc)
		{
			return new WWW(url, hash, crc);
		}

		private static string[] FlattenedHeadersFrom(Dictionary<string, string> headers)
		{
			if (headers == null)
			{
				return null;
			}
			string[] array = new string[headers.Count * 2];
			int num = 0;
			foreach (KeyValuePair<string, string> keyValuePair in headers)
			{
				array[num++] = keyValuePair.Key.ToString();
				array[num++] = keyValuePair.Value.ToString();
			}
			return array;
		}

		internal static Dictionary<string, string> ParseHTTPHeaderString(string input)
		{
			if (input == null)
			{
				throw new ArgumentException("input was null to ParseHTTPHeaderString");
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			StringReader stringReader = new StringReader(input);
			int num = 0;
			for (;;)
			{
				string text = stringReader.ReadLine();
				if (text == null)
				{
					break;
				}
				if (num++ == 0 && text.StartsWith("HTTP"))
				{
					dictionary["STATUS"] = text;
				}
				else
				{
					int num2 = text.IndexOf(": ");
					if (num2 != -1)
					{
						string key = text.Substring(0, num2).ToUpper();
						string value = text.Substring(num2 + 2);
						dictionary[key] = value;
					}
				}
			}
			return dictionary;
		}
	}
}
