using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace System.Runtime.Remoting.Contexts
{
	internal class CrossContextChannel : IMessageSink
	{
		public IMessage SyncProcessMessage(IMessage msg)
		{
			ServerIdentity serverIdentity = (ServerIdentity)RemotingServices.GetMessageTargetIdentity(msg);
			Context context = null;
			if (Thread.CurrentContext != serverIdentity.Context)
			{
				context = Context.SwitchToContext(serverIdentity.Context);
			}
			IMessage result;
			try
			{
				Context.NotifyGlobalDynamicSinks(true, msg, false, false);
				Thread.CurrentContext.NotifyDynamicSinks(true, msg, false, false);
				result = serverIdentity.Context.GetServerContextSinkChain().SyncProcessMessage(msg);
				Context.NotifyGlobalDynamicSinks(false, msg, false, false);
				Thread.CurrentContext.NotifyDynamicSinks(false, msg, false, false);
			}
			catch (Exception e)
			{
				result = new ReturnMessage(e, (IMethodCallMessage)msg);
			}
			finally
			{
				if (context != null)
				{
					Context.SwitchToContext(context);
				}
			}
			return result;
		}

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			ServerIdentity serverIdentity = (ServerIdentity)RemotingServices.GetMessageTargetIdentity(msg);
			Context context = null;
			if (Thread.CurrentContext != serverIdentity.Context)
			{
				context = Context.SwitchToContext(serverIdentity.Context);
			}
			IMessageCtrl result;
			try
			{
				Context.NotifyGlobalDynamicSinks(true, msg, false, true);
				Thread.CurrentContext.NotifyDynamicSinks(true, msg, false, false);
				if (replySink != null)
				{
					replySink = new CrossContextChannel.ContextRestoreSink(replySink, context, msg);
				}
				IMessageCtrl messageCtrl = serverIdentity.AsyncObjectProcessMessage(msg, replySink);
				if (replySink == null)
				{
					Context.NotifyGlobalDynamicSinks(false, msg, false, false);
					Thread.CurrentContext.NotifyDynamicSinks(false, msg, false, false);
				}
				result = messageCtrl;
			}
			catch (Exception e)
			{
				if (replySink != null)
				{
					replySink.SyncProcessMessage(new ReturnMessage(e, (IMethodCallMessage)msg));
				}
				result = null;
			}
			finally
			{
				if (context != null)
				{
					Context.SwitchToContext(context);
				}
			}
			return result;
		}

		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		private class ContextRestoreSink : IMessageSink
		{
			private IMessageSink _next;

			private Context _context;

			private IMessage _call;

			public ContextRestoreSink(IMessageSink next, Context context, IMessage call)
			{
				this._next = next;
				this._context = context;
				this._call = call;
			}

			public IMessage SyncProcessMessage(IMessage msg)
			{
				IMessage result;
				try
				{
					Context.NotifyGlobalDynamicSinks(false, msg, false, false);
					Thread.CurrentContext.NotifyDynamicSinks(false, msg, false, false);
					result = this._next.SyncProcessMessage(msg);
				}
				catch (Exception e)
				{
					result = new ReturnMessage(e, (IMethodCallMessage)this._call);
				}
				finally
				{
					if (this._context != null)
					{
						Context.SwitchToContext(this._context);
					}
				}
				return result;
			}

			public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
			{
				throw new NotSupportedException();
			}

			public IMessageSink NextSink
			{
				get
				{
					return this._next;
				}
			}
		}
	}
}
