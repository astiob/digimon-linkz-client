using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the restriction element from XML Schema as specified by the World Wide Web Consortium (W3C). This class is for complex types with a complex content model derived by restriction. It restricts the contents of the complex type to a subset of the inherited complex type.</summary>
	public class XmlSchemaComplexContentRestriction : XmlSchemaContent
	{
		private const string xmlname = "restriction";

		private XmlSchemaAnyAttribute any;

		private XmlSchemaObjectCollection attributes;

		private XmlQualifiedName baseTypeName;

		private XmlSchemaParticle particle;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaComplexContentRestriction" /> class.</summary>
		public XmlSchemaComplexContentRestriction()
		{
			this.baseTypeName = XmlQualifiedName.Empty;
			this.attributes = new XmlSchemaObjectCollection();
		}

		/// <summary>Gets or sets the name of a complex type from which this type is derived by restriction.</summary>
		/// <returns>The name of the complex type from which this type is derived by restriction.</returns>
		[XmlAttribute("base")]
		public XmlQualifiedName BaseTypeName
		{
			get
			{
				return this.baseTypeName;
			}
			set
			{
				this.baseTypeName = value;
			}
		}

		/// <summary>Gets or sets one of the <see cref="T:System.Xml.Schema.XmlSchemaGroupRef" />, <see cref="T:System.Xml.Schema.XmlSchemaChoice" />, <see cref="T:System.Xml.Schema.XmlSchemaAll" />, or <see cref="T:System.Xml.Schema.XmlSchemaSequence" /> classes.</summary>
		/// <returns>One of the <see cref="T:System.Xml.Schema.XmlSchemaGroupRef" />, <see cref="T:System.Xml.Schema.XmlSchemaChoice" />, <see cref="T:System.Xml.Schema.XmlSchemaAll" />, or <see cref="T:System.Xml.Schema.XmlSchemaSequence" /> classes.</returns>
		[XmlElement("group", typeof(XmlSchemaGroupRef))]
		[XmlElement("sequence", typeof(XmlSchemaSequence))]
		[XmlElement("choice", typeof(XmlSchemaChoice))]
		[XmlElement("all", typeof(XmlSchemaAll))]
		public XmlSchemaParticle Particle
		{
			get
			{
				return this.particle;
			}
			set
			{
				this.particle = value;
			}
		}

		/// <summary>Gets the collection of attributes for the complex type. Contains the <see cref="T:System.Xml.Schema.XmlSchemaAttribute" /> and <see cref="T:System.Xml.Schema.XmlSchemaAttributeGroupRef" /> elements.</summary>
		/// <returns>The collection of attributes for the complex type.</returns>
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroupRef))]
		[XmlElement("attribute", typeof(XmlSchemaAttribute))]
		public XmlSchemaObjectCollection Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.Schema.XmlSchemaAnyAttribute" /> component of the complex content model.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchemaAnyAttribute" /> component of the complex content model.</returns>
		[XmlElement("anyAttribute")]
		public XmlSchemaAnyAttribute AnyAttribute
		{
			get
			{
				return this.any;
			}
			set
			{
				this.any = value;
			}
		}

		internal override bool IsExtension
		{
			get
			{
				return false;
			}
		}

		internal override void SetParent(XmlSchemaObject parent)
		{
			base.SetParent(parent);
			if (this.Particle != null)
			{
				this.Particle.SetParent(this);
			}
			if (this.AnyAttribute != null)
			{
				this.AnyAttribute.SetParent(this);
			}
			foreach (XmlSchemaObject xmlSchemaObject in this.Attributes)
			{
				xmlSchemaObject.SetParent(this);
			}
		}

		internal override int Compile(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.CompilationId == schema.CompilationId)
			{
				return 0;
			}
			if (this.isRedefinedComponent)
			{
				if (base.Annotation != null)
				{
					base.Annotation.isRedefinedComponent = true;
				}
				if (this.AnyAttribute != null)
				{
					this.AnyAttribute.isRedefinedComponent = true;
				}
				foreach (XmlSchemaObject xmlSchemaObject in this.Attributes)
				{
					xmlSchemaObject.isRedefinedComponent = true;
				}
				if (this.Particle != null)
				{
					this.Particle.isRedefinedComponent = true;
				}
			}
			if (this.BaseTypeName == null || this.BaseTypeName.IsEmpty)
			{
				base.error(h, "base must be present, as a QName");
			}
			else if (!XmlSchemaUtil.CheckQName(this.BaseTypeName))
			{
				base.error(h, "BaseTypeName is not a valid XmlQualifiedName");
			}
			if (this.AnyAttribute != null)
			{
				this.errorCount += this.AnyAttribute.Compile(h, schema);
			}
			foreach (XmlSchemaObject xmlSchemaObject2 in this.Attributes)
			{
				if (xmlSchemaObject2 is XmlSchemaAttribute)
				{
					XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)xmlSchemaObject2;
					this.errorCount += xmlSchemaAttribute.Compile(h, schema);
				}
				else if (xmlSchemaObject2 is XmlSchemaAttributeGroupRef)
				{
					XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = (XmlSchemaAttributeGroupRef)xmlSchemaObject2;
					this.errorCount += xmlSchemaAttributeGroupRef.Compile(h, schema);
				}
				else
				{
					base.error(h, xmlSchemaObject2.GetType() + " is not valid in this place::ComplexContentRestriction");
				}
			}
			if (this.Particle != null)
			{
				if (this.Particle is XmlSchemaGroupRef)
				{
					this.errorCount += ((XmlSchemaGroupRef)this.Particle).Compile(h, schema);
				}
				else if (this.Particle is XmlSchemaAll)
				{
					this.errorCount += ((XmlSchemaAll)this.Particle).Compile(h, schema);
				}
				else if (this.Particle is XmlSchemaChoice)
				{
					this.errorCount += ((XmlSchemaChoice)this.Particle).Compile(h, schema);
				}
				else if (this.Particle is XmlSchemaSequence)
				{
					this.errorCount += ((XmlSchemaSequence)this.Particle).Compile(h, schema);
				}
				else
				{
					base.error(h, "Particle of a restriction is limited only to group, sequence, choice and all.");
				}
			}
			XmlSchemaUtil.CompileID(base.Id, this, schema.IDCollection, h);
			this.CompilationId = schema.CompilationId;
			return this.errorCount;
		}

		internal override XmlQualifiedName GetBaseTypeName()
		{
			return this.baseTypeName;
		}

		internal override XmlSchemaParticle GetParticle()
		{
			return this.particle;
		}

		internal override int Validate(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.Particle != null)
			{
				this.Particle.Validate(h, schema);
			}
			return this.errorCount;
		}

		internal static XmlSchemaComplexContentRestriction Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction = new XmlSchemaComplexContentRestriction();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "restriction")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaComplexContentRestriction.Read, name=" + reader.Name, null);
				reader.Skip();
				return null;
			}
			xmlSchemaComplexContentRestriction.LineNumber = reader.LineNumber;
			xmlSchemaComplexContentRestriction.LinePosition = reader.LinePosition;
			xmlSchemaComplexContentRestriction.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "base")
				{
					Exception ex;
					xmlSchemaComplexContentRestriction.baseTypeName = XmlSchemaUtil.ReadQNameAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is not a valid value for base attribute", ex);
					}
				}
				else if (reader.Name == "id")
				{
					xmlSchemaComplexContentRestriction.Id = reader.Value;
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for restriction", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaComplexContentRestriction);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaComplexContentRestriction;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "restriction")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaComplexContentRestriction.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaComplexContentRestriction.Annotation = xmlSchemaAnnotation;
					}
				}
				else
				{
					if (num <= 2)
					{
						if (reader.LocalName == "group")
						{
							num = 3;
							XmlSchemaGroupRef xmlSchemaGroupRef = XmlSchemaGroupRef.Read(reader, h);
							if (xmlSchemaGroupRef != null)
							{
								xmlSchemaComplexContentRestriction.particle = xmlSchemaGroupRef;
							}
							continue;
						}
						if (reader.LocalName == "all")
						{
							num = 3;
							XmlSchemaAll xmlSchemaAll = XmlSchemaAll.Read(reader, h);
							if (xmlSchemaAll != null)
							{
								xmlSchemaComplexContentRestriction.particle = xmlSchemaAll;
							}
							continue;
						}
						if (reader.LocalName == "choice")
						{
							num = 3;
							XmlSchemaChoice xmlSchemaChoice = XmlSchemaChoice.Read(reader, h);
							if (xmlSchemaChoice != null)
							{
								xmlSchemaComplexContentRestriction.particle = xmlSchemaChoice;
							}
							continue;
						}
						if (reader.LocalName == "sequence")
						{
							num = 3;
							XmlSchemaSequence xmlSchemaSequence = XmlSchemaSequence.Read(reader, h);
							if (xmlSchemaSequence != null)
							{
								xmlSchemaComplexContentRestriction.particle = xmlSchemaSequence;
							}
							continue;
						}
					}
					if (num <= 3)
					{
						if (reader.LocalName == "attribute")
						{
							num = 3;
							XmlSchemaAttribute xmlSchemaAttribute = XmlSchemaAttribute.Read(reader, h);
							if (xmlSchemaAttribute != null)
							{
								xmlSchemaComplexContentRestriction.Attributes.Add(xmlSchemaAttribute);
							}
							continue;
						}
						if (reader.LocalName == "attributeGroup")
						{
							num = 3;
							XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = XmlSchemaAttributeGroupRef.Read(reader, h);
							if (xmlSchemaAttributeGroupRef != null)
							{
								xmlSchemaComplexContentRestriction.attributes.Add(xmlSchemaAttributeGroupRef);
							}
							continue;
						}
					}
					if (num <= 4 && reader.LocalName == "anyAttribute")
					{
						num = 5;
						XmlSchemaAnyAttribute xmlSchemaAnyAttribute = XmlSchemaAnyAttribute.Read(reader, h);
						if (xmlSchemaAnyAttribute != null)
						{
							xmlSchemaComplexContentRestriction.AnyAttribute = xmlSchemaAnyAttribute;
						}
					}
					else
					{
						reader.RaiseInvalidElementError();
					}
				}
			}
			return xmlSchemaComplexContentRestriction;
		}
	}
}
