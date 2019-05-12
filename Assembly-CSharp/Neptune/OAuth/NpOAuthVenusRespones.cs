using System;

namespace Neptune.OAuth
{
	public class NpOAuthVenusRespones
	{
		public NpOAuthVenusRespones()
		{
			this.VenusStatus = VenusStatusE.NONE;
			this.Message = string.Empty;
			this.Subject = string.Empty;
			this.VenusErrLog = string.Empty;
			this.ResJson = string.Empty;
		}

		public VenusStatusE VenusStatus { get; set; }

		public string Message { get; set; }

		public string Subject { get; set; }

		public string VenusErrLog { get; set; }

		public string ResJson { get; set; }
	}
}
