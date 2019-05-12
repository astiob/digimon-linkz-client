using System;

namespace Facebook.Unity.Gameroom
{
	internal interface IGameroomFacebookImplementation : IGameroomFacebook, IPayFacebook, IFacebook, IGameroomFacebookResultHandler, IFacebookResultHandler
	{
		bool HaveReceivedPipeResponse();

		string GetPipeResponse(string callbackId);
	}
}
