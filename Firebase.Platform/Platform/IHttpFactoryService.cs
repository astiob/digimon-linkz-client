using System;

namespace Firebase.Platform
{
	internal interface IHttpFactoryService
	{
		FirebaseHttpRequest OpenConnection(Uri url);
	}
}
