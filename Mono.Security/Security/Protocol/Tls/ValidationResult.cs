using System;

namespace Mono.Security.Protocol.Tls
{
	public class ValidationResult
	{
		private bool trusted;

		private bool user_denied;

		private int error_code;

		public ValidationResult(bool trusted, bool user_denied, int error_code)
		{
			this.trusted = trusted;
			this.user_denied = user_denied;
			this.error_code = error_code;
		}

		public bool Trusted
		{
			get
			{
				return this.trusted;
			}
		}

		public bool UserDenied
		{
			get
			{
				return this.user_denied;
			}
		}

		public int ErrorCode
		{
			get
			{
				return this.error_code;
			}
		}
	}
}
