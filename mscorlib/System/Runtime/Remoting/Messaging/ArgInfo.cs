using System;
using System.Reflection;

namespace System.Runtime.Remoting.Messaging
{
	internal class ArgInfo
	{
		private int[] _paramMap;

		private int _inoutArgCount;

		private MethodBase _method;

		public ArgInfo(MethodBase method, ArgInfoType type)
		{
			this._method = method;
			ParameterInfo[] parameters = this._method.GetParameters();
			this._paramMap = new int[parameters.Length];
			this._inoutArgCount = 0;
			if (type == ArgInfoType.In)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					if (!parameters[i].ParameterType.IsByRef)
					{
						this._paramMap[this._inoutArgCount++] = i;
					}
				}
			}
			else
			{
				for (int j = 0; j < parameters.Length; j++)
				{
					if (parameters[j].ParameterType.IsByRef || parameters[j].IsOut)
					{
						this._paramMap[this._inoutArgCount++] = j;
					}
				}
			}
		}

		public int GetInOutArgIndex(int inoutArgNum)
		{
			return this._paramMap[inoutArgNum];
		}

		public virtual string GetInOutArgName(int index)
		{
			return this._method.GetParameters()[this._paramMap[index]].Name;
		}

		public int GetInOutArgCount()
		{
			return this._inoutArgCount;
		}

		public object[] GetInOutArgs(object[] args)
		{
			object[] array = new object[this._inoutArgCount];
			for (int i = 0; i < this._inoutArgCount; i++)
			{
				array[i] = args[this._paramMap[i]];
			}
			return array;
		}
	}
}
