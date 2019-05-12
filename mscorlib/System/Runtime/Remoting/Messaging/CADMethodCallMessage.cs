using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Channels;

namespace System.Runtime.Remoting.Messaging
{
	internal class CADMethodCallMessage : CADMessageBase
	{
		private string _uri;

		internal RuntimeMethodHandle MethodHandle;

		internal string FullTypeName;

		internal CADMethodCallMessage(IMethodCallMessage callMsg)
		{
			this._uri = callMsg.Uri;
			this.MethodHandle = callMsg.MethodBase.MethodHandle;
			this.FullTypeName = callMsg.MethodBase.DeclaringType.AssemblyQualifiedName;
			ArrayList arrayList = null;
			this._propertyCount = CADMessageBase.MarshalProperties(callMsg.Properties, ref arrayList);
			this._args = base.MarshalArguments(callMsg.Args, ref arrayList);
			base.SaveLogicalCallContext(callMsg, ref arrayList);
			if (arrayList != null)
			{
				MemoryStream memoryStream = CADSerializer.SerializeObject(arrayList.ToArray());
				this._serializedArgs = memoryStream.GetBuffer();
			}
		}

		internal string Uri
		{
			get
			{
				return this._uri;
			}
		}

		internal static CADMethodCallMessage Create(IMessage callMsg)
		{
			IMethodCallMessage methodCallMessage = callMsg as IMethodCallMessage;
			if (methodCallMessage == null)
			{
				return null;
			}
			return new CADMethodCallMessage(methodCallMessage);
		}

		internal ArrayList GetArguments()
		{
			ArrayList result = null;
			if (this._serializedArgs != null)
			{
				object[] c = (object[])CADSerializer.DeserializeObject(new MemoryStream(this._serializedArgs));
				result = new ArrayList(c);
				this._serializedArgs = null;
			}
			return result;
		}

		internal object[] GetArgs(ArrayList args)
		{
			return base.UnmarshalArguments(this._args, args);
		}

		internal int PropertiesCount
		{
			get
			{
				return this._propertyCount;
			}
		}

		private static Type[] GetSignature(MethodBase methodBase, bool load)
		{
			ParameterInfo[] parameters = methodBase.GetParameters();
			Type[] array = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				if (load)
				{
					array[i] = Type.GetType(parameters[i].ParameterType.AssemblyQualifiedName, true);
				}
				else
				{
					array[i] = parameters[i].ParameterType;
				}
			}
			return array;
		}

		internal MethodBase GetMethod()
		{
			Type type = Type.GetType(this.FullTypeName);
			MethodBase methodBase;
			if (type.IsGenericType || type.IsGenericTypeDefinition)
			{
				methodBase = MethodBase.GetMethodFromHandleNoGenericCheck(this.MethodHandle);
			}
			else
			{
				methodBase = MethodBase.GetMethodFromHandle(this.MethodHandle);
			}
			if (type == methodBase.DeclaringType)
			{
				return methodBase;
			}
			Type[] signature = CADMethodCallMessage.GetSignature(methodBase, true);
			if (methodBase.IsGenericMethod)
			{
				MethodBase[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				Type[] genericArguments = methodBase.GetGenericArguments();
				foreach (MethodBase methodBase2 in methods)
				{
					if (methodBase2.IsGenericMethod && !(methodBase2.Name != methodBase.Name))
					{
						Type[] genericArguments2 = methodBase2.GetGenericArguments();
						if (genericArguments.Length == genericArguments2.Length)
						{
							MethodInfo methodInfo = ((MethodInfo)methodBase2).MakeGenericMethod(genericArguments);
							Type[] signature2 = CADMethodCallMessage.GetSignature(methodInfo, false);
							if (signature2.Length == signature.Length)
							{
								bool flag = false;
								for (int j = signature2.Length - 1; j >= 0; j--)
								{
									if (signature2[j] != signature[j])
									{
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									return methodInfo;
								}
							}
						}
					}
				}
				return methodBase;
			}
			MethodBase method = type.GetMethod(methodBase.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, signature, null);
			if (method == null)
			{
				throw new RemotingException(string.Concat(new object[]
				{
					"Method '",
					methodBase.Name,
					"' not found in type '",
					type,
					"'"
				}));
			}
			return method;
		}
	}
}
