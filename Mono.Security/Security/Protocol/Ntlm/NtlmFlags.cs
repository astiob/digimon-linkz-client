using System;

namespace Mono.Security.Protocol.Ntlm
{
	[Flags]
	public enum NtlmFlags
	{
		NegotiateUnicode = 1,
		NegotiateOem = 2,
		RequestTarget = 4,
		NegotiateNtlm = 512,
		NegotiateDomainSupplied = 4096,
		NegotiateWorkstationSupplied = 8192,
		NegotiateAlwaysSign = 32768,
		NegotiateNtlm2Key = 524288,
		Negotiate128 = 536870912,
		Negotiate56 = -2147483648
	}
}
