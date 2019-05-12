using System;

namespace Neptune.Cloud.Core
{
	public class NpCloudException : Exception
	{
		private short mExitCode;

		public NpCloudException(short exitCode, string message) : base(message)
		{
			this.mExitCode = exitCode;
		}

		public NpCloudException(NpCloudException e) : base(e.Message, e)
		{
			this.mExitCode = e.ExitCode;
		}

		public NpCloudException(byte exitCode, Exception e) : base(e.Message, e)
		{
			this.mExitCode = (short)exitCode;
		}

		public NpCloudException(string message, Exception exception, byte exitCode) : base(message, exception)
		{
			this.mExitCode = (short)exitCode;
		}

		public short ExitCode
		{
			get
			{
				return this.mExitCode;
			}
		}
	}
}
