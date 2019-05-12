using System;
using System.Xml;

namespace Mono.Xml
{
	internal abstract class DTDNode : IXmlLineInfo
	{
		private DTDObjectModel root;

		private bool isInternalSubset;

		private string baseURI;

		private int lineNumber;

		private int linePosition;

		public virtual string BaseURI
		{
			get
			{
				return this.baseURI;
			}
			set
			{
				this.baseURI = value;
			}
		}

		public bool IsInternalSubset
		{
			get
			{
				return this.isInternalSubset;
			}
			set
			{
				this.isInternalSubset = value;
			}
		}

		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
			set
			{
				this.lineNumber = value;
			}
		}

		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
			set
			{
				this.linePosition = value;
			}
		}

		public bool HasLineInfo()
		{
			return this.lineNumber != 0;
		}

		internal void SetRoot(DTDObjectModel root)
		{
			this.root = root;
			if (this.baseURI == null)
			{
				this.BaseURI = root.BaseURI;
			}
		}

		protected DTDObjectModel Root
		{
			get
			{
				return this.root;
			}
		}

		internal XmlException NotWFError(string message)
		{
			return new XmlException(this, this.BaseURI, message);
		}
	}
}
