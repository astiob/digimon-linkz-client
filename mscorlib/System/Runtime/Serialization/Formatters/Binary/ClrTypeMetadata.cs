using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	internal abstract class ClrTypeMetadata : TypeMetadata
	{
		public Type InstanceType;

		public ClrTypeMetadata(Type instanceType)
		{
			this.InstanceType = instanceType;
			this.InstanceTypeName = instanceType.FullName;
			this.TypeAssemblyName = instanceType.Assembly.FullName;
		}

		public override bool RequiresTypes
		{
			get
			{
				return false;
			}
		}
	}
}
