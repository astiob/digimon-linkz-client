using System;
using System.Collections;

namespace Mono.Xml
{
	internal class DTDParameterEntityDeclarationCollection
	{
		private Hashtable peDecls = new Hashtable();

		private DTDObjectModel root;

		public DTDParameterEntityDeclarationCollection(DTDObjectModel root)
		{
			this.root = root;
		}

		public DTDParameterEntityDeclaration this[string name]
		{
			get
			{
				return this.peDecls[name] as DTDParameterEntityDeclaration;
			}
		}

		public void Add(string name, DTDParameterEntityDeclaration decl)
		{
			if (this.peDecls[name] != null)
			{
				return;
			}
			decl.SetRoot(this.root);
			this.peDecls.Add(name, decl);
		}

		public ICollection Keys
		{
			get
			{
				return this.peDecls.Keys;
			}
		}

		public ICollection Values
		{
			get
			{
				return this.peDecls.Values;
			}
		}
	}
}
