using System;

namespace Firebase.Platform.Security
{
	internal static class GoogleAuthConsts
	{
		public const string AuthorizationUrl = "https://accounts.google.com/o/oauth2/auth";

		public const string ApprovalUrl = "https://accounts.google.com/o/oauth2/approval";

		public const string TokenUrl = "https://accounts.google.com/o/oauth2/token";

		public const string ComputeTokenUrl = "http://metadata/computeMetadata/v1/instance/service-accounts/default/token";

		public const string RevokeTokenUrl = "https://accounts.google.com/o/oauth2/revoke";

		public const string InstalledAppRedirectUri = "urn:ietf:wg:oauth:2.0:oob";

		public const string LocalhostRedirectUri = "http://localhost";
	}
}
