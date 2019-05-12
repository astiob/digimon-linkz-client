using System;

namespace Facebook.Unity.Gameroom
{
	internal interface IGameroomFacebook : IPayFacebook, IFacebook
	{
		void PayPremium(FacebookDelegate<IPayResult> callback);

		void HasLicense(FacebookDelegate<IHasLicenseResult> callback);
	}
}
