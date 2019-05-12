using System;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Activation
{
	internal class RemoteActivator : MarshalByRefObject, IActivator
	{
		public IConstructionReturnMessage Activate(IConstructionCallMessage msg)
		{
			if (!RemotingConfiguration.IsActivationAllowed(msg.ActivationType))
			{
				throw new RemotingException("The type " + msg.ActivationTypeName + " is not allowed to be client activated");
			}
			object[] activationAttributes = new object[]
			{
				new RemoteActivationAttribute(msg.ContextProperties)
			};
			MarshalByRefObject obj = (MarshalByRefObject)Activator.CreateInstance(msg.ActivationType, msg.Args, activationAttributes);
			ObjRef resultObject = RemotingServices.Marshal(obj);
			return new ConstructionResponse(resultObject, null, msg);
		}

		public override object InitializeLifetimeService()
		{
			ILease lease = (ILease)base.InitializeLifetimeService();
			if (lease.CurrentState == LeaseState.Initial)
			{
				lease.InitialLeaseTime = TimeSpan.FromMinutes(30.0);
				lease.SponsorshipTimeout = TimeSpan.FromMinutes(1.0);
				lease.RenewOnCallTime = TimeSpan.FromMinutes(10.0);
			}
			return lease;
		}

		public ActivatorLevel Level
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public IActivator NextActivator
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}
	}
}
