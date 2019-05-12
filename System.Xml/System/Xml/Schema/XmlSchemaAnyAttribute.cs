using Mono.Xml.Schema;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the World Wide Web Consortium (W3C) anyAttribute element.</summary>
	public class XmlSchemaAnyAttribute : XmlSchemaAnnotated
	{
		private const string xmlname = "anyAttribute";

		private string nameSpace;

		private XmlSchemaContentProcessing processing;

		private XsdWildcard wildcard;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaAnyAttribute" /> class.</summary>
		public XmlSchemaAnyAttribute()
		{
			this.wildcard = new XsdWildcard(this);
		}

		/// <summary>Gets or sets the namespaces containing the attributes that can be used.</summary>
		/// <returns>Namespaces for attributes that are available for use. The default is ##any.Optional.</returns>
		[XmlAttribute("namespace")]
		public string Namespace
		{
			get
			{
				return this.nameSpace;
			}
			set
			{
				this.nameSpace = value;
			}
		}

		/// <summary>Gets or sets information about how an application or XML processor should handle the validation of XML documents for the attributes specified by the anyAttribute element.</summary>
		/// <returns>One of the <see cref="T:System.Xml.Schema.XmlSchemaContentProcessing" /> values. If no processContents attribute is specified, the default is Strict.</returns>
		[XmlAttribute("processContents")]
		[DefaultValue(XmlSchemaContentProcessing.None)]
		public XmlSchemaContentProcessing ProcessContents
		{
			get
			{
				return this.processing;
			}
			set
			{
				this.processing = value;
			}
		}

		internal bool HasValueAny
		{
			get
			{
				return this.wildcard.HasValueAny;
			}
		}

		internal bool HasValueLocal
		{
			get
			{
				return this.wildcard.HasValueLocal;
			}
		}

		internal bool HasValueOther
		{
			get
			{
				return this.wildcard.HasValueOther;
			}
		}

		internal bool HasValueTargetNamespace
		{
			get
			{
				return this.wildcard.HasValueTargetNamespace;
			}
		}

		internal StringCollection ResolvedNamespaces
		{
			get
			{
				return this.wildcard.ResolvedNamespaces;
			}
		}

		internal XmlSchemaContentProcessing ResolvedProcessContents
		{
			get
			{
				return this.wildcard.ResolvedProcessing;
			}
		}

		internal string TargetNamespace
		{
			get
			{
				return this.wildcard.TargetNamespace;
			}
		}

		internal override int Compile(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.CompilationId == schema.CompilationId)
			{
				return 0;
			}
			this.errorCount = 0;
			this.wildcard.TargetNamespace = base.AncestorSchema.TargetNamespace;
			if (this.wildcard.TargetNamespace == null)
			{
				this.wildcard.TargetNamespace = string.Empty;
			}
			XmlSchemaUtil.CompileID(base.Id, this, schema.IDCollection, h);
			this.wildcard.Compile(this.Namespace, h, schema);
			if (this.processing == XmlSchemaContentProcessing.None)
			{
				this.wildcard.ResolvedProcessing = XmlSchemaContentProcessing.Strict;
			}
			else
			{
				this.wildcard.ResolvedProcessing = this.processing;
			}
			this.CompilationId = schema.CompilationId;
			return this.errorCount;
		}

		internal override int Validate(ValidationEventHandler h, XmlSchema schema)
		{
			return this.errorCount;
		}

		internal void ValidateWildcardSubset(XmlSchemaAnyAttribute other, ValidationEventHandler h, XmlSchema schema)
		{
			this.wildcard.ValidateWildcardSubset(other.wildcard, h, schema);
		}

		internal bool ValidateWildcardAllowsNamespaceName(string ns, XmlSchema schema)
		{
			return this.wildcard.ValidateWildcardAllowsNamespaceName(ns, null, schema, false);
		}

		internal static XmlSchemaAnyAttribute Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaAnyAttribute xmlSchemaAnyAttribute = new XmlSchemaAnyAttribute();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "anyAttribute")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaAnyAttribute.Read, name=" + reader.Name, null);
				reader.SkipToEnd();
				return null;
			}
			xmlSchemaAnyAttribute.LineNumber = reader.LineNumber;
			xmlSchemaAnyAttribute.LinePosition = reader.LinePosition;
			xmlSchemaAnyAttribute.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "id")
				{
					xmlSchemaAnyAttribute.Id = reader.Value;
				}
				else if (reader.Name == "namespace")
				{
					xmlSchemaAnyAttribute.nameSpace = reader.Value;
				}
				else if (reader.Name == "processContents")
				{
					Exception ex;
					xmlSchemaAnyAttribute.processing = XmlSchemaUtil.ReadProcessingAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is not a valid value for processContents", ex);
					}
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for anyAttribute", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaAnyAttribute);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaAnyAttribute;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "anyAttribute")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaAnyAttribute.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaAnyAttribute.Annotation = xmlSchemaAnnotation;
					}
				}
				else
				{
					reader.RaiseInvalidElementError();
				}
			}
			return xmlSchemaAnyAttribute;
		}
	}
}
