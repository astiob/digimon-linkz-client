using System;

namespace UnityEngine
{
	internal class AudioExtensionDefinition
	{
		private string assemblyName;

		private string extensionNamespace;

		private string extensionTypeName;

		private Type extensionType;

		public AudioExtensionDefinition(AudioExtensionDefinition definition)
		{
			this.assemblyName = definition.assemblyName;
			this.extensionNamespace = definition.extensionNamespace;
			this.extensionTypeName = definition.extensionTypeName;
			this.extensionType = this.GetExtensionType();
		}

		public AudioExtensionDefinition(string assemblyNameIn, string extensionNamespaceIn, string extensionTypeNameIn)
		{
			this.assemblyName = assemblyNameIn;
			this.extensionNamespace = extensionNamespaceIn;
			this.extensionTypeName = extensionTypeNameIn;
			this.extensionType = this.GetExtensionType();
		}

		public Type GetExtensionType()
		{
			if (this.extensionType == null)
			{
				this.extensionType = Type.GetType(string.Concat(new string[]
				{
					this.extensionNamespace,
					".",
					this.extensionTypeName,
					", ",
					this.assemblyName
				}));
			}
			return this.extensionType;
		}
	}
}
