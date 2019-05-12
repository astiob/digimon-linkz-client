using System;

namespace Mono.Xml
{
	internal class DTDEntityDeclarationCollection : DTDCollectionBase
	{
		public DTDEntityDeclarationCollection(DTDObjectModel root) : base(root)
		{
		}

		public DTDEntityDeclaration this[string name]
		{
			get
			{
				return base.BaseGet(name) as DTDEntityDeclaration;
			}
		}

		public void Add(string name, DTDEntityDeclaration decl)
		{
			if (base.Contains(name))
			{
				throw new InvalidOperationException(string.Format("Entity declaration for {0} was already added.", name));
			}
			decl.SetRoot(base.Root);
			base.BaseAdd(name, decl);
		}
	}
}
