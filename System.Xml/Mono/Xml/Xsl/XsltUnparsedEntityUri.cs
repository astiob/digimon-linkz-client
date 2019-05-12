using System;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
	internal class XsltUnparsedEntityUri : XPathFunction
	{
		private Expression arg0;

		public XsltUnparsedEntityUri(FunctionArguments args) : base(args)
		{
			if (args == null || args.Tail != null)
			{
				throw new XPathException("unparsed-entity-uri takes 1 arg");
			}
			this.arg0 = args.Arg;
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.String;
			}
		}

		internal override bool Peer
		{
			get
			{
				return this.arg0.Peer;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			IHasXmlNode hasXmlNode = iter.Current as IHasXmlNode;
			if (hasXmlNode == null)
			{
				return string.Empty;
			}
			XmlNode node = hasXmlNode.GetNode();
			if (node.OwnerDocument == null)
			{
				return string.Empty;
			}
			XmlDocumentType documentType = node.OwnerDocument.DocumentType;
			if (documentType == null)
			{
				return string.Empty;
			}
			XmlEntity xmlEntity = documentType.Entities.GetNamedItem(this.arg0.EvaluateString(iter)) as XmlEntity;
			if (xmlEntity == null)
			{
				return string.Empty;
			}
			return (xmlEntity.SystemId == null) ? string.Empty : xmlEntity.SystemId;
		}
	}
}
