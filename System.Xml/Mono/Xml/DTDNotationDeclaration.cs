using System;

namespace Mono.Xml
{
	internal class DTDNotationDeclaration : DTDNode
	{
		private string name;

		private string localName;

		private string prefix;

		private string publicId;

		private string systemId;

		internal DTDNotationDeclaration(DTDObjectModel root)
		{
			base.SetRoot(root);
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public string PublicId
		{
			get
			{
				return this.publicId;
			}
			set
			{
				this.publicId = value;
			}
		}

		public string SystemId
		{
			get
			{
				return this.systemId;
			}
			set
			{
				this.systemId = value;
			}
		}

		public string LocalName
		{
			get
			{
				return this.localName;
			}
			set
			{
				this.localName = value;
			}
		}

		public string Prefix
		{
			get
			{
				return this.prefix;
			}
			set
			{
				this.prefix = value;
			}
		}
	}
}
