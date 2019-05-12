using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	internal class DynamicPropertyCollection
	{
		private ArrayList _properties = new ArrayList();

		public bool HasProperties
		{
			get
			{
				return this._properties.Count > 0;
			}
		}

		public bool RegisterDynamicProperty(IDynamicProperty prop)
		{
			bool result;
			lock (this)
			{
				if (this.FindProperty(prop.Name) != -1)
				{
					throw new InvalidOperationException("Another property by this name already exists");
				}
				ArrayList arrayList = new ArrayList(this._properties);
				DynamicPropertyCollection.DynamicPropertyReg dynamicPropertyReg = new DynamicPropertyCollection.DynamicPropertyReg();
				dynamicPropertyReg.Property = prop;
				IContributeDynamicSink contributeDynamicSink = prop as IContributeDynamicSink;
				if (contributeDynamicSink != null)
				{
					dynamicPropertyReg.Sink = contributeDynamicSink.GetDynamicSink();
				}
				arrayList.Add(dynamicPropertyReg);
				this._properties = arrayList;
				result = true;
			}
			return result;
		}

		public bool UnregisterDynamicProperty(string name)
		{
			bool result;
			lock (this)
			{
				int num = this.FindProperty(name);
				if (num == -1)
				{
					throw new RemotingException("A property with the name " + name + " was not found");
				}
				this._properties.RemoveAt(num);
				result = true;
			}
			return result;
		}

		public void NotifyMessage(bool start, IMessage msg, bool client_site, bool async)
		{
			ArrayList properties = this._properties;
			if (start)
			{
				foreach (object obj in properties)
				{
					DynamicPropertyCollection.DynamicPropertyReg dynamicPropertyReg = (DynamicPropertyCollection.DynamicPropertyReg)obj;
					if (dynamicPropertyReg.Sink != null)
					{
						dynamicPropertyReg.Sink.ProcessMessageStart(msg, client_site, async);
					}
				}
			}
			else
			{
				foreach (object obj2 in properties)
				{
					DynamicPropertyCollection.DynamicPropertyReg dynamicPropertyReg2 = (DynamicPropertyCollection.DynamicPropertyReg)obj2;
					if (dynamicPropertyReg2.Sink != null)
					{
						dynamicPropertyReg2.Sink.ProcessMessageFinish(msg, client_site, async);
					}
				}
			}
		}

		private int FindProperty(string name)
		{
			for (int i = 0; i < this._properties.Count; i++)
			{
				if (((DynamicPropertyCollection.DynamicPropertyReg)this._properties[i]).Property.Name == name)
				{
					return i;
				}
			}
			return -1;
		}

		private class DynamicPropertyReg
		{
			public IDynamicProperty Property;

			public IDynamicMessageSink Sink;
		}
	}
}
