using System;

namespace System.Runtime.Remoting
{
	[Serializable]
	internal class TypeInfo : IRemotingTypeInfo
	{
		private string serverType;

		private string[] serverHierarchy;

		private string[] interfacesImplemented;

		public TypeInfo(Type type)
		{
			if (type.IsInterface)
			{
				this.serverType = typeof(MarshalByRefObject).AssemblyQualifiedName;
				this.serverHierarchy = new string[0];
				this.interfacesImplemented = new string[]
				{
					type.AssemblyQualifiedName
				};
			}
			else
			{
				this.serverType = type.AssemblyQualifiedName;
				int num = 0;
				Type baseType = type.BaseType;
				while (baseType != typeof(MarshalByRefObject) && baseType != typeof(object))
				{
					baseType = baseType.BaseType;
					num++;
				}
				this.serverHierarchy = new string[num];
				baseType = type.BaseType;
				for (int i = 0; i < num; i++)
				{
					this.serverHierarchy[i] = baseType.AssemblyQualifiedName;
					baseType = baseType.BaseType;
				}
				Type[] interfaces = type.GetInterfaces();
				this.interfacesImplemented = new string[interfaces.Length];
				for (int j = 0; j < interfaces.Length; j++)
				{
					this.interfacesImplemented[j] = interfaces[j].AssemblyQualifiedName;
				}
			}
		}

		public string TypeName
		{
			get
			{
				return this.serverType;
			}
			set
			{
				this.serverType = value;
			}
		}

		public bool CanCastTo(Type fromType, object o)
		{
			if (fromType == typeof(object))
			{
				return true;
			}
			if (fromType == typeof(MarshalByRefObject))
			{
				return true;
			}
			string text = fromType.AssemblyQualifiedName;
			int num = text.IndexOf(',');
			if (num != -1)
			{
				num = text.IndexOf(',', num + 1);
			}
			if (num != -1)
			{
				text = text.Substring(0, num + 1);
			}
			else
			{
				text += ",";
			}
			if ((this.serverType + ",").StartsWith(text))
			{
				return true;
			}
			if (this.serverHierarchy != null)
			{
				foreach (string str in this.serverHierarchy)
				{
					if ((str + ",").StartsWith(text))
					{
						return true;
					}
				}
			}
			if (this.interfacesImplemented != null)
			{
				foreach (string str2 in this.interfacesImplemented)
				{
					if ((str2 + ",").StartsWith(text))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
