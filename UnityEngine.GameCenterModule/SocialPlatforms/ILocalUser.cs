﻿using System;

namespace UnityEngine.SocialPlatforms
{
	public interface ILocalUser : IUserProfile
	{
		void Authenticate(Action<bool> callback);

		void Authenticate(Action<bool, string> callback);

		void LoadFriends(Action<bool> callback);

		IUserProfile[] friends { get; }

		bool authenticated { get; }

		bool underage { get; }
	}
}