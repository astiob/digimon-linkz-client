using System;

namespace Neptune.OAuth
{
	public class NpOAuthErrData
	{
		public NpOAuthErrData()
		{
			this.FailedCode = NpOatuhFailedCodeE.None;
			this.NativeErrLog = string.Empty;
			this.VenusRespones_ = new NpOAuthVenusRespones();
		}

		public NpOAuthErrData(NpOatuhFailedCodeE failedCode, string nativeErrLog, NpOAuthVenusRespones venusRes)
		{
			this.FailedCode = failedCode;
			this.NativeErrLog = nativeErrLog;
			this.VenusRespones_ = new NpOAuthVenusRespones();
			this.VenusRespones_.VenusStatus = venusRes.VenusStatus;
			this.VenusRespones_.Message = venusRes.Message;
			this.VenusRespones_.Subject = venusRes.Subject;
			this.VenusRespones_.VenusErrLog = venusRes.VenusErrLog;
			this.VenusRespones_.ResJson = venusRes.ResJson;
		}

		public NpOatuhFailedCodeE FailedCode { get; set; }

		public string NativeErrLog { get; set; }

		public NpOAuthVenusRespones VenusRespones_ { get; set; }

		public void PrintLog()
		{
			Debug.Log(string.Format("NpOAuthErrData : FailedCode={0}, NativeErrLog={1}, VenusRespones_.VenusStatus={2}, VenusRespones_.Message={3}, VenusRespones_.Subject={4}, VenusRespones_.VenusErrLog={5}, VenusRespones_.ResJson={6}", new object[]
			{
				this.FailedCode,
				this.NativeErrLog,
				this.VenusRespones_.VenusStatus,
				this.VenusRespones_.Message,
				this.VenusRespones_.Subject,
				this.VenusRespones_.VenusErrLog,
				this.VenusRespones_.ResJson
			}));
		}
	}
}
