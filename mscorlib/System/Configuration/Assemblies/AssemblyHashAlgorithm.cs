using System;
using System.Runtime.InteropServices;

namespace System.Configuration.Assemblies
{
	/// <summary>Specifies all the hash algorithms used for hashing files and for generating the strong name.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum AssemblyHashAlgorithm
	{
		/// <summary>A mask indicating that there is no hash algorithm. If you specify None for a multi-module assembly, the common language runtime defaults to the SHA1 algorithm, since multi-module assemblies need to generate a hash.</summary>
		None,
		/// <summary>Retrieves the MD5 message-digest algorithm. MD5 was developed by Rivest in 1991. It is basically MD4 with safety-belts and while it is slightly slower than MD4, it helps provide more security. The algorithm consists of four distinct rounds, which has a slightly different design from that of MD4. Message-digest size, as well as padding requirements, remain the same.</summary>
		MD5 = 32771,
		/// <summary>A mask used to retrieve a revision of the Secure Hash Algorithm that corrects an unpublished flaw in SHA.</summary>
		SHA1
	}
}
