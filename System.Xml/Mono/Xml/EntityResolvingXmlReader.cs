using System;
using System.Collections.Generic;
using System.Xml;

namespace Mono.Xml
{
	internal class EntityResolvingXmlReader : XmlReader, IHasXmlParserContext, IXmlLineInfo, IXmlNamespaceResolver
	{
		private EntityResolvingXmlReader entity;

		private XmlReader source;

		private XmlParserContext context;

		private XmlResolver resolver;

		private EntityHandling entity_handling;

		private bool entity_inside_attr;

		private bool inside_attr;

		private bool do_resolve;

		public EntityResolvingXmlReader(XmlReader source)
		{
			this.source = source;
			IHasXmlParserContext hasXmlParserContext = source as IHasXmlParserContext;
			if (hasXmlParserContext != null)
			{
				this.context = hasXmlParserContext.ParserContext;
			}
			else
			{
				this.context = new XmlParserContext(source.NameTable, new XmlNamespaceManager(source.NameTable), null, XmlSpace.None);
			}
		}

		private EntityResolvingXmlReader(XmlReader entityContainer, bool inside_attr)
		{
			this.source = entityContainer;
			this.entity_inside_attr = inside_attr;
		}

		XmlParserContext IHasXmlParserContext.ParserContext
		{
			get
			{
				return this.context;
			}
		}

		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.GetNamespacesInScope(scope);
		}

		string IXmlNamespaceResolver.LookupPrefix(string ns)
		{
			return ((IXmlNamespaceResolver)this.Current).LookupPrefix(ns);
		}

		private XmlReader Current
		{
			get
			{
				return (this.entity == null || this.entity.ReadState == ReadState.Initial) ? this.source : this.entity;
			}
		}

		public override int AttributeCount
		{
			get
			{
				return this.Current.AttributeCount;
			}
		}

		public override string BaseURI
		{
			get
			{
				return this.Current.BaseURI;
			}
		}

		public override bool CanResolveEntity
		{
			get
			{
				return true;
			}
		}

		public override int Depth
		{
			get
			{
				if (this.entity != null && this.entity.ReadState == ReadState.Interactive)
				{
					return this.source.Depth + this.entity.Depth + 1;
				}
				return this.source.Depth;
			}
		}

		public override bool EOF
		{
			get
			{
				return this.source.EOF;
			}
		}

		public override bool HasValue
		{
			get
			{
				return this.Current.HasValue;
			}
		}

		public override bool IsDefault
		{
			get
			{
				return this.Current.IsDefault;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return this.Current.IsEmptyElement;
			}
		}

		public override string LocalName
		{
			get
			{
				return this.Current.LocalName;
			}
		}

		public override string Name
		{
			get
			{
				return this.Current.Name;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				return this.Current.NamespaceURI;
			}
		}

		public override XmlNameTable NameTable
		{
			get
			{
				return this.Current.NameTable;
			}
		}

		public override XmlNodeType NodeType
		{
			get
			{
				if (this.entity == null)
				{
					return this.source.NodeType;
				}
				if (this.entity.ReadState == ReadState.Initial)
				{
					return this.source.NodeType;
				}
				return (!this.entity.EOF) ? this.entity.NodeType : XmlNodeType.EndEntity;
			}
		}

		internal XmlParserContext ParserContext
		{
			get
			{
				return this.context;
			}
		}

		public override string Prefix
		{
			get
			{
				return this.Current.Prefix;
			}
		}

		public override char QuoteChar
		{
			get
			{
				return this.Current.QuoteChar;
			}
		}

		public override ReadState ReadState
		{
			get
			{
				return (this.entity == null) ? this.source.ReadState : ReadState.Interactive;
			}
		}

		public override string Value
		{
			get
			{
				return this.Current.Value;
			}
		}

		public override string XmlLang
		{
			get
			{
				return this.Current.XmlLang;
			}
		}

		public override XmlSpace XmlSpace
		{
			get
			{
				return this.Current.XmlSpace;
			}
		}

		private void CopyProperties(EntityResolvingXmlReader other)
		{
			this.context = other.context;
			this.resolver = other.resolver;
			this.entity_handling = other.entity_handling;
		}

		public EntityHandling EntityHandling
		{
			get
			{
				return this.entity_handling;
			}
			set
			{
				if (this.entity != null)
				{
					this.entity.EntityHandling = value;
				}
				this.entity_handling = value;
			}
		}

		public int LineNumber
		{
			get
			{
				IXmlLineInfo xmlLineInfo = this.Current as IXmlLineInfo;
				return (xmlLineInfo != null) ? xmlLineInfo.LineNumber : 0;
			}
		}

		public int LinePosition
		{
			get
			{
				IXmlLineInfo xmlLineInfo = this.Current as IXmlLineInfo;
				return (xmlLineInfo != null) ? xmlLineInfo.LinePosition : 0;
			}
		}

		public XmlResolver XmlResolver
		{
			set
			{
				if (this.entity != null)
				{
					this.entity.XmlResolver = value;
				}
				this.resolver = value;
			}
		}

		public override void Close()
		{
			if (this.entity != null)
			{
				this.entity.Close();
			}
			this.source.Close();
		}

		public override string GetAttribute(int i)
		{
			return this.Current.GetAttribute(i);
		}

		public override string GetAttribute(string name)
		{
			return this.Current.GetAttribute(name);
		}

		public override string GetAttribute(string localName, string namespaceURI)
		{
			return this.Current.GetAttribute(localName, namespaceURI);
		}

		public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return ((IXmlNamespaceResolver)this.Current).GetNamespacesInScope(scope);
		}

		public override string LookupNamespace(string prefix)
		{
			return this.Current.LookupNamespace(prefix);
		}

		public override void MoveToAttribute(int i)
		{
			if (this.entity != null && this.entity_inside_attr)
			{
				this.entity.Close();
				this.entity = null;
			}
			this.Current.MoveToAttribute(i);
			this.inside_attr = true;
		}

		public override bool MoveToAttribute(string name)
		{
			if (this.entity != null && !this.entity_inside_attr)
			{
				return this.entity.MoveToAttribute(name);
			}
			if (!this.source.MoveToAttribute(name))
			{
				return false;
			}
			if (this.entity != null && this.entity_inside_attr)
			{
				this.entity.Close();
				this.entity = null;
			}
			this.inside_attr = true;
			return true;
		}

		public override bool MoveToAttribute(string localName, string namespaceName)
		{
			if (this.entity != null && !this.entity_inside_attr)
			{
				return this.entity.MoveToAttribute(localName, namespaceName);
			}
			if (!this.source.MoveToAttribute(localName, namespaceName))
			{
				return false;
			}
			if (this.entity != null && this.entity_inside_attr)
			{
				this.entity.Close();
				this.entity = null;
			}
			this.inside_attr = true;
			return true;
		}

		public override bool MoveToElement()
		{
			if (this.entity != null && this.entity_inside_attr)
			{
				this.entity.Close();
				this.entity = null;
			}
			if (!this.Current.MoveToElement())
			{
				return false;
			}
			this.inside_attr = false;
			return true;
		}

		public override bool MoveToFirstAttribute()
		{
			if (this.entity != null && !this.entity_inside_attr)
			{
				return this.entity.MoveToFirstAttribute();
			}
			if (!this.source.MoveToFirstAttribute())
			{
				return false;
			}
			if (this.entity != null && this.entity_inside_attr)
			{
				this.entity.Close();
				this.entity = null;
			}
			this.inside_attr = true;
			return true;
		}

		public override bool MoveToNextAttribute()
		{
			if (this.entity != null && !this.entity_inside_attr)
			{
				return this.entity.MoveToNextAttribute();
			}
			if (!this.source.MoveToNextAttribute())
			{
				return false;
			}
			if (this.entity != null && this.entity_inside_attr)
			{
				this.entity.Close();
				this.entity = null;
			}
			this.inside_attr = true;
			return true;
		}

		public override bool Read()
		{
			if (this.do_resolve)
			{
				this.DoResolveEntity();
				this.do_resolve = false;
			}
			this.inside_attr = false;
			if (this.entity != null && (this.entity_inside_attr || this.entity.EOF))
			{
				this.entity.Close();
				this.entity = null;
			}
			if (this.entity != null)
			{
				if (this.entity.Read())
				{
					return true;
				}
				if (this.EntityHandling == EntityHandling.ExpandEntities)
				{
					this.entity.Close();
					this.entity = null;
					return this.Read();
				}
				return true;
			}
			else
			{
				if (!this.source.Read())
				{
					return false;
				}
				if (this.EntityHandling == EntityHandling.ExpandEntities && this.source.NodeType == XmlNodeType.EntityReference)
				{
					this.ResolveEntity();
					return this.Read();
				}
				return true;
			}
		}

		public override bool ReadAttributeValue()
		{
			if (this.entity != null && this.entity_inside_attr)
			{
				if (!this.entity.EOF)
				{
					this.entity.Read();
					return true;
				}
				this.entity.Close();
				this.entity = null;
			}
			return this.Current.ReadAttributeValue();
		}

		public override string ReadString()
		{
			return base.ReadString();
		}

		public override void ResolveEntity()
		{
			this.DoResolveEntity();
		}

		private void DoResolveEntity()
		{
			if (this.entity != null)
			{
				this.entity.ResolveEntity();
			}
			else
			{
				if (this.source.NodeType != XmlNodeType.EntityReference)
				{
					throw new InvalidOperationException("The current node is not an Entity Reference");
				}
				if (this.ParserContext.Dtd == null)
				{
					throw new XmlException(this, this.BaseURI, string.Format("Cannot resolve entity without DTD: '{0}'", this.source.Name));
				}
				XmlReader xmlReader = this.ParserContext.Dtd.GenerateEntityContentReader(this.source.Name, this.ParserContext);
				if (xmlReader == null)
				{
					throw new XmlException(this, this.BaseURI, string.Format("Reference to undeclared entity '{0}'.", this.source.Name));
				}
				this.entity = new EntityResolvingXmlReader(xmlReader, this.inside_attr);
				this.entity.CopyProperties(this);
			}
		}

		public override void Skip()
		{
			base.Skip();
		}

		public bool HasLineInfo()
		{
			IXmlLineInfo xmlLineInfo = this.Current as IXmlLineInfo;
			return xmlLineInfo != null && xmlLineInfo.HasLineInfo();
		}
	}
}
