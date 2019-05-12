using System;

namespace Mono.Security.Protocol.Tls
{
	[Serializable]
	internal enum AlertDescription : byte
	{
		CloseNotify,
		UnexpectedMessage = 10,
		BadRecordMAC = 20,
		DecryptionFailed,
		RecordOverflow,
		DecompressionFailiure = 30,
		HandshakeFailiure = 40,
		NoCertificate,
		BadCertificate,
		UnsupportedCertificate,
		CertificateRevoked,
		CertificateExpired,
		CertificateUnknown,
		IlegalParameter,
		UnknownCA,
		AccessDenied,
		DecodeError,
		DecryptError,
		ExportRestriction = 60,
		ProtocolVersion = 70,
		InsuficientSecurity,
		InternalError = 80,
		UserCancelled = 90,
		NoRenegotiation = 100
	}
}
