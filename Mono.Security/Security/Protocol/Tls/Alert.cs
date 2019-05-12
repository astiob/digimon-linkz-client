using System;

namespace Mono.Security.Protocol.Tls
{
	internal class Alert
	{
		private AlertLevel level;

		private AlertDescription description;

		public Alert(AlertDescription description)
		{
			this.inferAlertLevel();
			this.description = description;
		}

		public Alert(AlertLevel level, AlertDescription description)
		{
			this.level = level;
			this.description = description;
		}

		public AlertLevel Level
		{
			get
			{
				return this.level;
			}
		}

		public AlertDescription Description
		{
			get
			{
				return this.description;
			}
		}

		public string Message
		{
			get
			{
				return Alert.GetAlertMessage(this.description);
			}
		}

		public bool IsWarning
		{
			get
			{
				return this.level == AlertLevel.Warning;
			}
		}

		public bool IsCloseNotify
		{
			get
			{
				return this.IsWarning && this.description == AlertDescription.CloseNotify;
			}
		}

		private void inferAlertLevel()
		{
			AlertDescription alertDescription = this.description;
			switch (alertDescription)
			{
			case AlertDescription.HandshakeFailiure:
			case AlertDescription.BadCertificate:
			case AlertDescription.UnsupportedCertificate:
			case AlertDescription.CertificateRevoked:
			case AlertDescription.CertificateExpired:
			case AlertDescription.CertificateUnknown:
			case AlertDescription.IlegalParameter:
			case AlertDescription.UnknownCA:
			case AlertDescription.AccessDenied:
			case AlertDescription.DecodeError:
			case AlertDescription.DecryptError:
			case AlertDescription.ExportRestriction:
				break;
			default:
				switch (alertDescription)
				{
				case AlertDescription.BadRecordMAC:
				case AlertDescription.DecryptionFailed:
				case AlertDescription.RecordOverflow:
					break;
				default:
					if (alertDescription != AlertDescription.ProtocolVersion && alertDescription != AlertDescription.InsuficientSecurity)
					{
						if (alertDescription != AlertDescription.CloseNotify)
						{
							if (alertDescription == AlertDescription.UnexpectedMessage || alertDescription == AlertDescription.DecompressionFailiure || alertDescription == AlertDescription.InternalError)
							{
								break;
							}
							if (alertDescription != AlertDescription.UserCancelled && alertDescription != AlertDescription.NoRenegotiation)
							{
								break;
							}
						}
						this.level = AlertLevel.Warning;
						return;
					}
					break;
				}
				break;
			}
			this.level = AlertLevel.Fatal;
		}

		public static string GetAlertMessage(AlertDescription description)
		{
			return "The authentication or decryption has failed.";
		}
	}
}
