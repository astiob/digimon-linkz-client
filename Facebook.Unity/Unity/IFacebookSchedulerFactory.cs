using System;

namespace Facebook.Unity
{
	internal interface IFacebookSchedulerFactory
	{
		IFacebookScheduler GetInstance();
	}
}
