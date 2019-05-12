using System;

namespace Neptune.OAuth
{
	public enum NpOatuhFailedCodeE
	{
		None,
		InitFaield = 400,
		WWWFaield,
		TimeOut,
		ServerFailed,
		OtherException,
		NoneUserID
	}
}
