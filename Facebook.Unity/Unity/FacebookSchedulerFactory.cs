using System;

namespace Facebook.Unity
{
	internal class FacebookSchedulerFactory : IFacebookSchedulerFactory
	{
		public IFacebookScheduler GetInstance()
		{
			return ComponentFactory.GetComponent<FacebookScheduler>(ComponentFactory.IfNotExist.AddNew);
		}
	}
}
