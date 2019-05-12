using System;
using System.Runtime.Remoting.Activation;

namespace System.Runtime.Remoting.Messaging
{
	internal class ServerContextTerminatorSink : IMessageSink
	{
		public IMessage SyncProcessMessage(IMessage msg)
		{
			if (msg is IConstructionCallMessage)
			{
				return ActivationServices.CreateInstanceFromMessage((IConstructionCallMessage)msg);
			}
			ServerIdentity serverIdentity = (ServerIdentity)RemotingServices.GetMessageTargetIdentity(msg);
			return serverIdentity.SyncObjectProcessMessage(msg);
		}

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			ServerIdentity serverIdentity = (ServerIdentity)RemotingServices.GetMessageTargetIdentity(msg);
			return serverIdentity.AsyncObjectProcessMessage(msg, replySink);
		}

		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}
	}
}
