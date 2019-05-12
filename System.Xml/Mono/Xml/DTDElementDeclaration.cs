using System;

namespace Mono.Xml
{
	internal class DTDElementDeclaration : DTDNode
	{
		private DTDObjectModel root;

		private DTDContentModel contentModel;

		private string name;

		private bool isEmpty;

		private bool isAny;

		private bool isMixedContent;

		internal DTDElementDeclaration(DTDObjectModel root)
		{
			this.root = root;
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

		public bool IsEmpty
		{
			get
			{
				return this.isEmpty;
			}
			set
			{
				this.isEmpty = value;
			}
		}

		public bool IsAny
		{
			get
			{
				return this.isAny;
			}
			set
			{
				this.isAny = value;
			}
		}

		public bool IsMixedContent
		{
			get
			{
				return this.isMixedContent;
			}
			set
			{
				this.isMixedContent = value;
			}
		}

		public DTDContentModel ContentModel
		{
			get
			{
				if (this.contentModel == null)
				{
					this.contentModel = new DTDContentModel(this.root, this.Name);
				}
				return this.contentModel;
			}
		}

		public DTDAttListDeclaration Attributes
		{
			get
			{
				return base.Root.AttListDecls[this.Name];
			}
		}
	}
}
