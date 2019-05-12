using System;

namespace System.IO.Compression
{
	/// <summary> Specifies whether to compress or decompress the underlying stream.</summary>
	public enum CompressionMode
	{
		/// <summary>Decompresses the underlying stream.</summary>
		Decompress,
		/// <summary>Compresses the underlying stream.</summary>
		Compress
	}
}
