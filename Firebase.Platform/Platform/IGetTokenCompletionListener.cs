using System;

namespace Firebase.Platform
{
	internal interface IGetTokenCompletionListener
	{
		void OnSuccess(string token);

		void OnError(string error);
	}
}
