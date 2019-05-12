using System;

namespace Mono.Security.X509
{
	public class PKCS5
	{
		public const string pbeWithMD2AndDESCBC = "1.2.840.113549.1.5.1";

		public const string pbeWithMD5AndDESCBC = "1.2.840.113549.1.5.3";

		public const string pbeWithMD2AndRC2CBC = "1.2.840.113549.1.5.4";

		public const string pbeWithMD5AndRC2CBC = "1.2.840.113549.1.5.6";

		public const string pbeWithSHA1AndDESCBC = "1.2.840.113549.1.5.10";

		public const string pbeWithSHA1AndRC2CBC = "1.2.840.113549.1.5.11";
	}
}
