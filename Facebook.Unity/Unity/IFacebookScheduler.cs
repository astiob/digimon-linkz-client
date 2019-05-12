using System;

namespace Facebook.Unity
{
	internal interface IFacebookScheduler
	{
		void Schedule(Action action, long delay = 0L);
	}
}
