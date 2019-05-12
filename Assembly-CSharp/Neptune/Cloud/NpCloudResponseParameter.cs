using System;

namespace Neptune.Cloud
{
	public class NpCloudResponseParameter<T> where T : class
	{
		public string sys;

		public T body;
	}
}
