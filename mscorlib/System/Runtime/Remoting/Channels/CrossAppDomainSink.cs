using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace System.Runtime.Remoting.Channels
{
	[MonoTODO("Handle domain unloading?")]
	internal class CrossAppDomainSink : IMessageSink
	{
		private static Hashtable s_sinks = new Hashtable();

		private static MethodInfo processMessageMethod = typeof(CrossAppDomainSink).GetMethod("ProcessMessageInDomain", BindingFlags.Static | BindingFlags.NonPublic);

		private int _domainID;

		internal CrossAppDomainSink(int domainID)
		{
			this._domainID = domainID;
		}

		internal static CrossAppDomainSink GetSink(int domainID)
		{
			object syncRoot = CrossAppDomainSink.s_sinks.SyncRoot;
			CrossAppDomainSink result;
			lock (syncRoot)
			{
				if (CrossAppDomainSink.s_sinks.ContainsKey(domainID))
				{
					result = (CrossAppDomainSink)CrossAppDomainSink.s_sinks[domainID];
				}
				else
				{
					CrossAppDomainSink crossAppDomainSink = new CrossAppDomainSink(domainID);
					CrossAppDomainSink.s_sinks[domainID] = crossAppDomainSink;
					result = crossAppDomainSink;
				}
			}
			return result;
		}

		internal int TargetDomainId
		{
			get
			{
				return this._domainID;
			}
		}

		private static CrossAppDomainSink.ProcessMessageRes ProcessMessageInDomain(byte[] arrRequest, CADMethodCallMessage cadMsg)
		{
			CrossAppDomainSink.ProcessMessageRes result = default(CrossAppDomainSink.ProcessMessageRes);
			try
			{
				AppDomain.CurrentDomain.ProcessMessageInDomain(arrRequest, cadMsg, out result.arrResponse, out result.cadMrm);
			}
			catch (Exception e)
			{
				IMessage msg = new MethodResponse(e, new ErrorMessage());
				result.arrResponse = CADSerializer.SerializeMessage(msg).GetBuffer();
			}
			return result;
		}

		public virtual IMessage SyncProcessMessage(IMessage msgRequest)
		{
			IMessage result = null;
			try
			{
				byte[] array = null;
				byte[] array2 = null;
				CADMethodReturnMessage retmsg = null;
				CADMethodCallMessage cadmethodCallMessage = CADMethodCallMessage.Create(msgRequest);
				if (cadmethodCallMessage == null)
				{
					MemoryStream memoryStream = CADSerializer.SerializeMessage(msgRequest);
					array2 = memoryStream.GetBuffer();
				}
				Context currentContext = Thread.CurrentContext;
				try
				{
					CrossAppDomainSink.ProcessMessageRes processMessageRes = (CrossAppDomainSink.ProcessMessageRes)AppDomain.InvokeInDomainByID(this._domainID, CrossAppDomainSink.processMessageMethod, null, new object[]
					{
						array2,
						cadmethodCallMessage
					});
					array = processMessageRes.arrResponse;
					retmsg = processMessageRes.cadMrm;
				}
				finally
				{
					AppDomain.InternalSetContext(currentContext);
				}
				if (array != null)
				{
					MemoryStream mem = new MemoryStream(array);
					result = CADSerializer.DeserializeMessage(mem, msgRequest as IMethodCallMessage);
				}
				else
				{
					result = new MethodResponse(msgRequest as IMethodCallMessage, retmsg);
				}
			}
			catch (Exception e)
			{
				try
				{
					result = new ReturnMessage(e, msgRequest as IMethodCallMessage);
				}
				catch (Exception)
				{
				}
			}
			return result;
		}

		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			AsyncRequest state = new AsyncRequest(reqMsg, replySink);
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendAsyncMessage), state);
			return null;
		}

		public void SendAsyncMessage(object data)
		{
			AsyncRequest asyncRequest = (AsyncRequest)data;
			IMessage msg = this.SyncProcessMessage(asyncRequest.MsgRequest);
			asyncRequest.ReplySink.SyncProcessMessage(msg);
		}

		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		private struct ProcessMessageRes
		{
			public byte[] arrResponse;

			public CADMethodReturnMessage cadMrm;
		}
	}
}
