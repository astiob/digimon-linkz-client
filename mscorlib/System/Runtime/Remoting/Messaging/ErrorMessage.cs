using System;
using System.Collections;
using System.Reflection;

namespace System.Runtime.Remoting.Messaging
{
	[Serializable]
	internal class ErrorMessage : IMessage, IMethodCallMessage, IMethodMessage
	{
		private string _uri = "Exception";

		public int ArgCount
		{
			get
			{
				return 0;
			}
		}

		public object[] Args
		{
			get
			{
				return null;
			}
		}

		public bool HasVarArgs
		{
			get
			{
				return false;
			}
		}

		public MethodBase MethodBase
		{
			get
			{
				return null;
			}
		}

		public string MethodName
		{
			get
			{
				return "unknown";
			}
		}

		public object MethodSignature
		{
			get
			{
				return null;
			}
		}

		public virtual IDictionary Properties
		{
			get
			{
				return null;
			}
		}

		public string TypeName
		{
			get
			{
				return "unknown";
			}
		}

		public string Uri
		{
			get
			{
				return this._uri;
			}
			set
			{
				this._uri = value;
			}
		}

		public object GetArg(int arg_num)
		{
			return null;
		}

		public string GetArgName(int arg_num)
		{
			return "unknown";
		}

		public int InArgCount
		{
			get
			{
				return 0;
			}
		}

		public string GetInArgName(int index)
		{
			return null;
		}

		public object GetInArg(int argNum)
		{
			return null;
		}

		public object[] InArgs
		{
			get
			{
				return null;
			}
		}

		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return null;
			}
		}
	}
}
