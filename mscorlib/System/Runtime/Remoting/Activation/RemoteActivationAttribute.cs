using System;
using System.Collections;
using System.Runtime.Remoting.Contexts;

namespace System.Runtime.Remoting.Activation
{
	internal class RemoteActivationAttribute : Attribute, IContextAttribute
	{
		private IList _contextProperties;

		public RemoteActivationAttribute()
		{
		}

		public RemoteActivationAttribute(IList contextProperties)
		{
			this._contextProperties = contextProperties;
		}

		public bool IsContextOK(Context ctx, IConstructionCallMessage ctor)
		{
			return false;
		}

		public void GetPropertiesForNewContext(IConstructionCallMessage ctor)
		{
			if (this._contextProperties != null)
			{
				foreach (object value in this._contextProperties)
				{
					ctor.ContextProperties.Add(value);
				}
			}
		}
	}
}
