using System;

namespace Facebook.Unity.Arcade
{
	internal interface IArcadeFacebookImplementation : IArcadeFacebook, IArcadeFacebookResultHandler, IPayFacebook, IFacebook, IFacebookResultHandler
	{
		bool HaveReceivedPipeResponse();

		string GetPipeResponse(string callbackId);
	}
}
