using System;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Provides a simple structure for storing X509 chain status and error information.</summary>
	public struct X509ChainStatus
	{
		private X509ChainStatusFlags status;

		private string info;

		internal X509ChainStatus(X509ChainStatusFlags flag)
		{
			this.status = flag;
			this.info = X509ChainStatus.GetInformation(flag);
		}

		/// <summary>Specifies the status of the X509 chain.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509ChainStatusFlags" /> value.</returns>
		public X509ChainStatusFlags Status
		{
			get
			{
				return this.status;
			}
			set
			{
				this.status = value;
			}
		}

		/// <summary>Specifies a description of the <see cref="P:System.Security.Cryptography.X509Certificates.X509Chain.ChainStatus" /> value.</summary>
		/// <returns>A localizable string.</returns>
		public string StatusInformation
		{
			get
			{
				return this.info;
			}
			set
			{
				this.info = value;
			}
		}

		internal static string GetInformation(X509ChainStatusFlags flags)
		{
			switch (flags)
			{
			case X509ChainStatusFlags.NoError:
				goto IL_FF;
			case X509ChainStatusFlags.NotTimeValid:
			case X509ChainStatusFlags.NotTimeNested:
			case X509ChainStatusFlags.Revoked:
			case X509ChainStatusFlags.NotSignatureValid:
				break;
			default:
				if (flags != X509ChainStatusFlags.NotValidForUsage && flags != X509ChainStatusFlags.UntrustedRoot && flags != X509ChainStatusFlags.RevocationStatusUnknown && flags != X509ChainStatusFlags.Cyclic && flags != X509ChainStatusFlags.InvalidExtension && flags != X509ChainStatusFlags.InvalidPolicyConstraints && flags != X509ChainStatusFlags.InvalidBasicConstraints && flags != X509ChainStatusFlags.InvalidNameConstraints && flags != X509ChainStatusFlags.HasNotSupportedNameConstraint && flags != X509ChainStatusFlags.HasNotDefinedNameConstraint && flags != X509ChainStatusFlags.HasNotPermittedNameConstraint && flags != X509ChainStatusFlags.HasExcludedNameConstraint && flags != X509ChainStatusFlags.PartialChain && flags != X509ChainStatusFlags.CtlNotTimeValid && flags != X509ChainStatusFlags.CtlNotSignatureValid && flags != X509ChainStatusFlags.CtlNotValidForUsage && flags != X509ChainStatusFlags.OfflineRevocation && flags != X509ChainStatusFlags.NoIssuanceChainPolicy)
				{
					goto IL_FF;
				}
				break;
			}
			return Locale.GetText(flags.ToString());
			IL_FF:
			return string.Empty;
		}
	}
}
