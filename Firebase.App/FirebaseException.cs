using System;

namespace Firebase
{
	public sealed class FirebaseException : Exception
	{
		public FirebaseException()
		{
			this.ErrorCode = 0;
		}

		public FirebaseException(int errorCode)
		{
			this.ErrorCode = errorCode;
		}

		public FirebaseException(int errorCode, string message) : base(message)
		{
			this.ErrorCode = errorCode;
		}

		public FirebaseException(int errorCode, string message, Exception inner) : base(message, inner)
		{
			this.ErrorCode = errorCode;
		}

		public int ErrorCode { get; private set; }
	}
}
