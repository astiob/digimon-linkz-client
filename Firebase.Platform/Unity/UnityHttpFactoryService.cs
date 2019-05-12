using Firebase.Platform;
using System;

namespace Firebase.Unity
{
	internal class UnityHttpFactoryService : IHttpFactoryService
	{
		private static UnityHttpFactoryService _instance = new UnityHttpFactoryService();

		public static UnityHttpFactoryService Instance
		{
			get
			{
				return UnityHttpFactoryService._instance;
			}
		}

		public FirebaseHttpRequest OpenConnection(Uri url)
		{
			return new WWWHttpRequest(url);
		}
	}
}
