using System;

namespace Mono.Xml
{
	internal class DTDAttListDeclarationCollection : DTDCollectionBase
	{
		public DTDAttListDeclarationCollection(DTDObjectModel root) : base(root)
		{
		}

		public DTDAttListDeclaration this[string name]
		{
			get
			{
				return base.BaseGet(name) as DTDAttListDeclaration;
			}
		}

		public void Add(string name, DTDAttListDeclaration decl)
		{
			DTDAttListDeclaration dtdattListDeclaration = this[name];
			if (dtdattListDeclaration != null)
			{
				foreach (object obj in decl.Definitions)
				{
					DTDAttributeDefinition dtdattributeDefinition = (DTDAttributeDefinition)obj;
					if (decl.Get(dtdattributeDefinition.Name) == null)
					{
						dtdattListDeclaration.Add(dtdattributeDefinition);
					}
				}
			}
			else
			{
				decl.SetRoot(base.Root);
				base.BaseAdd(name, decl);
			}
		}
	}
}
