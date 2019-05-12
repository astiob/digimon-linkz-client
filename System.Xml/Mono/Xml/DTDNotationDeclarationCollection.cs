using System;

namespace Mono.Xml
{
	internal class DTDNotationDeclarationCollection : DTDCollectionBase
	{
		public DTDNotationDeclarationCollection(DTDObjectModel root) : base(root)
		{
		}

		public DTDNotationDeclaration this[string name]
		{
			get
			{
				return base.BaseGet(name) as DTDNotationDeclaration;
			}
		}

		public void Add(string name, DTDNotationDeclaration decl)
		{
			if (base.Contains(name))
			{
				throw new InvalidOperationException(string.Format("Notation declaration for {0} was already added.", name));
			}
			decl.SetRoot(base.Root);
			base.BaseAdd(name, decl);
		}
	}
}
