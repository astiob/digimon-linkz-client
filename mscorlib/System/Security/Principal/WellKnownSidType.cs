using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	/// <summary>Defines a set of commonly used security identifiers (SIDs).</summary>
	[ComVisible(false)]
	public enum WellKnownSidType
	{
		/// <summary>Indicates a null SID.</summary>
		NullSid,
		/// <summary>Indicates a SID that matches everyone.</summary>
		WorldSid,
		/// <summary>Indicates a local SID.</summary>
		LocalSid,
		/// <summary>Indicates a SID that matches the owner or creator of an object.</summary>
		CreatorOwnerSid,
		/// <summary>Indicates a SID that matches the creator group of an object.</summary>
		CreatorGroupSid,
		/// <summary>Indicates a creator owner server SID.</summary>
		CreatorOwnerServerSid,
		/// <summary>Indicates a creator group server SID.</summary>
		CreatorGroupServerSid,
		/// <summary>Indicates a SID for the Windows NT authority.</summary>
		NTAuthoritySid,
		/// <summary>Indicates a SID for a dial-up account.</summary>
		DialupSid,
		/// <summary>Indicates a SID for a network account. This SID is added to the process of a token when it logs on across a network.</summary>
		NetworkSid,
		/// <summary>Indicates a SID for a batch process. This SID is added to the process of a token when it logs on as a batch job.</summary>
		BatchSid,
		/// <summary>Indicates a SID for an interactive account. This SID is added to the process of a token when it logs on interactively.</summary>
		InteractiveSid,
		/// <summary>Indicates a SID for a service. This SID is added to the process of a token when it logs on as a service.</summary>
		ServiceSid,
		/// <summary>Indicates a SID for the anonymous account.</summary>
		AnonymousSid,
		/// <summary>Indicates a proxy SID.</summary>
		ProxySid,
		/// <summary>Indicates a SID for an enterprise controller.</summary>
		EnterpriseControllersSid,
		/// <summary>Indicates a SID for self.</summary>
		SelfSid,
		/// <summary>Indicates a SID for an authenticated user.</summary>
		AuthenticatedUserSid,
		/// <summary>Indicates a SID for restricted code.</summary>
		RestrictedCodeSid,
		/// <summary>Indicates a SID that matches a terminal server account.</summary>
		TerminalServerSid,
		/// <summary>Indicates a SID that matches remote logons.</summary>
		RemoteLogonIdSid,
		/// <summary>Indicates a SID that matches logon IDs.</summary>
		LogonIdsSid,
		/// <summary>Indicates a SID that matches the local system.</summary>
		LocalSystemSid,
		/// <summary>Indicates a SID that matches a local service.</summary>
		LocalServiceSid,
		/// <summary>Indicates a SID that matches a network service.</summary>
		NetworkServiceSid,
		/// <summary>Indicates a SID that matches the domain account.</summary>
		BuiltinDomainSid,
		/// <summary>Indicates a SID that matches the administrator account.</summary>
		BuiltinAdministratorsSid,
		/// <summary>Indicates a SID that matches built-in user accounts.</summary>
		BuiltinUsersSid,
		/// <summary>Indicates a SID that matches the guest account.</summary>
		BuiltinGuestsSid,
		/// <summary>Indicates a SID that matches the power users group.</summary>
		BuiltinPowerUsersSid,
		/// <summary>Indicates a SID that matches the account operators account.</summary>
		BuiltinAccountOperatorsSid,
		/// <summary>Indicates a SID that matches the system operators group.</summary>
		BuiltinSystemOperatorsSid,
		/// <summary>Indicates a SID that matches the print operators group.</summary>
		BuiltinPrintOperatorsSid,
		/// <summary>Indicates a SID that matches the backup operators group.</summary>
		BuiltinBackupOperatorsSid,
		/// <summary>Indicates a SID that matches the replicator account.</summary>
		BuiltinReplicatorSid,
		/// <summary>Indicates a SID that matches pre-Windows 2000 compatible accounts.</summary>
		BuiltinPreWindows2000CompatibleAccessSid,
		/// <summary>Indicates a SID that matches remote desktop users.</summary>
		BuiltinRemoteDesktopUsersSid,
		/// <summary>Indicates a SID that matches the network operators group.</summary>
		BuiltinNetworkConfigurationOperatorsSid,
		/// <summary>Indicates a SID that matches the account administrators group.</summary>
		AccountAdministratorSid,
		/// <summary>Indicates a SID that matches the account guest group.</summary>
		AccountGuestSid,
		/// <summary>Indicates a SID that matches the account Kerberos target group.</summary>
		AccountKrbtgtSid,
		/// <summary>Indicates a SID that matches the account domain administrator group.</summary>
		AccountDomainAdminsSid,
		/// <summary>Indicates a SID that matches the account domain users group.</summary>
		AccountDomainUsersSid,
		/// <summary>Indicates a SID that matches the account domain guests group.</summary>
		AccountDomainGuestsSid,
		/// <summary>Indicates a SID that matches the account computer group.</summary>
		AccountComputersSid,
		/// <summary>Indicates a SID that matches the account controller group.</summary>
		AccountControllersSid,
		/// <summary>Indicates a SID that matches the certificate administrators group.</summary>
		AccountCertAdminsSid,
		/// <summary>Indicates a SID that matches the schema administrators group.</summary>
		AccountSchemaAdminsSid,
		/// <summary>Indicates a SID that matches the enterprise administrators group.</summary>
		AccountEnterpriseAdminsSid,
		/// <summary>Indicates a SID that matches the policy administrators group.</summary>
		AccountPolicyAdminsSid,
		/// <summary>Indicates a SID that matches the RAS and IAS server account.</summary>
		AccountRasAndIasServersSid,
		/// <summary>Indicates a SID present when the Microsoft NTLM authentication package authenticated the client.</summary>
		NtlmAuthenticationSid,
		/// <summary>Indicates a SID present when the Microsoft Digest authentication package authenticated the client.</summary>
		DigestAuthenticationSid,
		/// <summary>Indicates a SID present when the Secure Channel (SSL/TLS) authentication package authenticated the client.</summary>
		SChannelAuthenticationSid,
		/// <summary>Indicates a SID present when the user authenticated from within the forest or across a trust that does not have the selective authentication option enabled. If this SID is present, then <see cref="F:System.Security.Principal.WellKnownSidType.OtherOrganizationSid" /> cannot be present.</summary>
		ThisOrganizationSid,
		/// <summary>Indicates a SID present when the user authenticated across a forest with the selective authentication option enabled. If this SID is present, then <see cref="F:System.Security.Principal.WellKnownSidType.ThisOrganizationSid" /> cannot be present.</summary>
		OtherOrganizationSid,
		/// <summary>Indicates a SID that allows a user to create incoming forest trusts. It is added to the token of users who are a member of the Incoming Forest Trust Builders built-in group in the root domain of the forest.</summary>
		BuiltinIncomingForestTrustBuildersSid,
		/// <summary>Indicates a SID that matches the group of users that have remote access to schedule logging of performance counters on this computer.</summary>
		BuiltinPerformanceMonitoringUsersSid,
		/// <summary>Indicates a SID that matches the group of users that have remote access to monitor the computer.</summary>
		BuiltinPerformanceLoggingUsersSid,
		/// <summary>Indicates a SID that matches the Windows Authorization Access group.</summary>
		BuiltinAuthorizationAccessSid,
		/// <summary>Indicates a SID is present in a server that can issue Terminal Server licenses.</summary>
		WinBuiltinTerminalServerLicenseServersSid,
		/// <summary>Indicates the maximum defined SID in the <see cref="T:System.Security.Principal.WellKnownSidType" /> enumeration.</summary>
		MaxDefined = 60
	}
}
