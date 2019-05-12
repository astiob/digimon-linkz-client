using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Runtime.Remoting.Messaging
{
	[Serializable]
	internal class MonoMethodMessage : IInternalMessage, IMessage, IMethodCallMessage, IMethodMessage, IMethodReturnMessage
	{
		private MonoMethod method;

		private object[] args;

		private string[] names;

		private byte[] arg_types;

		public LogicalCallContext ctx;

		public object rval;

		public Exception exc;

		private AsyncResult asyncResult;

		private CallType call_type;

		private string uri;

		private MethodCallDictionary properties;

		private Type[] methodSignature;

		private Identity identity;

		public MonoMethodMessage(MethodBase method, object[] out_args)
		{
			if (method != null)
			{
				this.InitMessage((MonoMethod)method, out_args);
			}
			else
			{
				this.args = null;
			}
		}

		public MonoMethodMessage(Type type, string method_name, object[] in_args)
		{
			MethodInfo methodInfo = type.GetMethod(method_name);
			this.InitMessage((MonoMethod)methodInfo, null);
			int num = in_args.Length;
			for (int i = 0; i < num; i++)
			{
				this.args[i] = in_args[i];
			}
		}

		Identity IInternalMessage.TargetIdentity
		{
			get
			{
				return this.identity;
			}
			set
			{
				this.identity = value;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void InitMessage(MonoMethod method, object[] out_args);

		public IDictionary Properties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new MethodCallDictionary(this);
				}
				return this.properties;
			}
		}

		public int ArgCount
		{
			get
			{
				if (this.CallType == CallType.EndInvoke)
				{
					return -1;
				}
				if (this.args == null)
				{
					return 0;
				}
				return this.args.Length;
			}
		}

		public object[] Args
		{
			get
			{
				return this.args;
			}
		}

		public bool HasVarArgs
		{
			get
			{
				return false;
			}
		}

		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this.ctx;
			}
			set
			{
				this.ctx = value;
			}
		}

		public MethodBase MethodBase
		{
			get
			{
				return this.method;
			}
		}

		public string MethodName
		{
			get
			{
				if (this.method == null)
				{
					return string.Empty;
				}
				return this.method.Name;
			}
		}

		public object MethodSignature
		{
			get
			{
				if (this.methodSignature == null)
				{
					ParameterInfo[] parameters = this.method.GetParameters();
					this.methodSignature = new Type[parameters.Length];
					for (int i = 0; i < parameters.Length; i++)
					{
						this.methodSignature[i] = parameters[i].ParameterType;
					}
				}
				return this.methodSignature;
			}
		}

		public string TypeName
		{
			get
			{
				if (this.method == null)
				{
					return string.Empty;
				}
				return this.method.DeclaringType.AssemblyQualifiedName;
			}
		}

		public string Uri
		{
			get
			{
				return this.uri;
			}
			set
			{
				this.uri = value;
			}
		}

		public object GetArg(int arg_num)
		{
			if (this.args == null)
			{
				return null;
			}
			return this.args[arg_num];
		}

		public string GetArgName(int arg_num)
		{
			if (this.args == null)
			{
				return string.Empty;
			}
			return this.names[arg_num];
		}

		public int InArgCount
		{
			get
			{
				if (this.CallType == CallType.EndInvoke)
				{
					return -1;
				}
				if (this.args == null)
				{
					return 0;
				}
				int num = 0;
				foreach (byte b in this.arg_types)
				{
					if ((b & 1) != 0)
					{
						num++;
					}
				}
				return num;
			}
		}

		public object[] InArgs
		{
			get
			{
				int inArgCount = this.InArgCount;
				object[] array = new object[inArgCount];
				int num2;
				int num = num2 = 0;
				foreach (byte b in this.arg_types)
				{
					if ((b & 1) != 0)
					{
						array[num++] = this.args[num2];
					}
					num2++;
				}
				return array;
			}
		}

		public object GetInArg(int arg_num)
		{
			int num = 0;
			int num2 = 0;
			foreach (byte b in this.arg_types)
			{
				if ((b & 1) != 0 && num2++ == arg_num)
				{
					return this.args[num];
				}
				num++;
			}
			return null;
		}

		public string GetInArgName(int arg_num)
		{
			int num = 0;
			int num2 = 0;
			foreach (byte b in this.arg_types)
			{
				if ((b & 1) != 0 && num2++ == arg_num)
				{
					return this.names[num];
				}
				num++;
			}
			return null;
		}

		public Exception Exception
		{
			get
			{
				return this.exc;
			}
		}

		public int OutArgCount
		{
			get
			{
				if (this.args == null)
				{
					return 0;
				}
				int num = 0;
				foreach (byte b in this.arg_types)
				{
					if ((b & 2) != 0)
					{
						num++;
					}
				}
				return num;
			}
		}

		public object[] OutArgs
		{
			get
			{
				if (this.args == null)
				{
					return null;
				}
				int outArgCount = this.OutArgCount;
				object[] array = new object[outArgCount];
				int num2;
				int num = num2 = 0;
				foreach (byte b in this.arg_types)
				{
					if ((b & 2) != 0)
					{
						array[num++] = this.args[num2];
					}
					num2++;
				}
				return array;
			}
		}

		public object ReturnValue
		{
			get
			{
				return this.rval;
			}
		}

		public object GetOutArg(int arg_num)
		{
			int num = 0;
			int num2 = 0;
			foreach (byte b in this.arg_types)
			{
				if ((b & 2) != 0 && num2++ == arg_num)
				{
					return this.args[num];
				}
				num++;
			}
			return null;
		}

		public string GetOutArgName(int arg_num)
		{
			int num = 0;
			int num2 = 0;
			foreach (byte b in this.arg_types)
			{
				if ((b & 2) != 0 && num2++ == arg_num)
				{
					return this.names[num];
				}
				num++;
			}
			return null;
		}

		public bool IsAsync
		{
			get
			{
				return this.asyncResult != null;
			}
		}

		public AsyncResult AsyncResult
		{
			get
			{
				return this.asyncResult;
			}
		}

		internal CallType CallType
		{
			get
			{
				if (this.call_type == CallType.Sync && RemotingServices.IsOneWay(this.method))
				{
					this.call_type = CallType.OneWay;
				}
				return this.call_type;
			}
		}

		public bool NeedsOutProcessing(out int outCount)
		{
			bool flag = false;
			outCount = 0;
			foreach (byte b in this.arg_types)
			{
				if ((b & 2) != 0)
				{
					outCount++;
				}
				else if ((b & 4) != 0)
				{
					flag = true;
				}
			}
			return outCount > 0 || flag;
		}
	}
}
