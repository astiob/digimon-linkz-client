using System;
using System.Threading.Tasks;

namespace Firebase.Platform
{
	internal interface IAuthService
	{
		void GetTokenAsync(IFirebaseAppPlatform app, bool forceRefresh, IGetTokenCompletionListener listener);

		Task<string> GetTokenAsync(IFirebaseAppPlatform app, bool forceRefresh);

		void AddTokenChangeListener(IFirebaseAppPlatform app, ITokenChangeListener listener);

		string GetCurrentUserId(IFirebaseAppPlatform app);
	}
}
