using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Activation
{
	[Serializable]
	internal class ContextLevelActivator : IActivator
	{
		private IActivator m_NextActivator;

		public ContextLevelActivator(IActivator next)
		{
			this.m_NextActivator = next;
		}

		public ActivatorLevel Level
		{
			get
			{
				return ActivatorLevel.Context;
			}
		}

		public IActivator NextActivator
		{
			get
			{
				return this.m_NextActivator;
			}
			set
			{
				this.m_NextActivator = value;
			}
		}

		public IConstructionReturnMessage Activate(IConstructionCallMessage ctorCall)
		{
			ServerIdentity serverIdentity = RemotingServices.CreateContextBoundObjectIdentity(ctorCall.ActivationType);
			RemotingServices.SetMessageTargetIdentity(ctorCall, serverIdentity);
			ConstructionCall constructionCall = ctorCall as ConstructionCall;
			if (constructionCall == null || !constructionCall.IsContextOk)
			{
				serverIdentity.Context = Context.CreateNewContext(ctorCall);
				Context newContext = Context.SwitchToContext(serverIdentity.Context);
				try
				{
					return this.m_NextActivator.Activate(ctorCall);
				}
				finally
				{
					Context.SwitchToContext(newContext);
				}
			}
			return this.m_NextActivator.Activate(ctorCall);
		}
	}
}
