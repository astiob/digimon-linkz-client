using System;

namespace Firebase.Platform
{
	internal interface IFirebaseAppPlatform
	{
		object AppObject { get; }

		string Name { get; }

		Uri DatabaseUrl { get; }
	}
}
