using System;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the complexType element from XML Schema as specified by the World Wide Web Consortium (W3C). This class defines a complex type that determines the set of attributes and content of an element.</summary>
	public class XmlSchemaComplexType : XmlSchemaType
	{
		private const string xmlname = "complexType";

		private XmlSchemaAnyAttribute anyAttribute;

		private XmlSchemaObjectCollection attributes;

		private XmlSchemaObjectTable attributeUses;

		private XmlSchemaAnyAttribute attributeWildcard;

		private XmlSchemaDerivationMethod block;

		private XmlSchemaDerivationMethod blockResolved;

		private XmlSchemaContentModel contentModel;

		private XmlSchemaParticle validatableParticle;

		private XmlSchemaParticle contentTypeParticle;

		private bool isAbstract;

		private bool isMixed;

		private XmlSchemaParticle particle;

		private XmlSchemaContentType resolvedContentType;

		internal bool ValidatedIsAbstract;

		private static XmlSchemaComplexType anyType;

		internal static readonly XmlQualifiedName AnyTypeName = new XmlQualifiedName("anyType", "http://www.w3.org/2001/XMLSchema");

		private Guid CollectProcessId;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaComplexType" /> class.</summary>
		public XmlSchemaComplexType()
		{
			this.attributes = new XmlSchemaObjectCollection();
			this.block = XmlSchemaDerivationMethod.None;
			this.attributeUses = new XmlSchemaObjectTable();
			this.validatableParticle = XmlSchemaParticle.Empty;
			this.contentTypeParticle = this.validatableParticle;
		}

		internal bool ParentIsSchema
		{
			get
			{
				return base.Parent is XmlSchema;
			}
		}

		internal static XmlSchemaComplexType AnyType
		{
			get
			{
				if (XmlSchemaComplexType.anyType == null)
				{
					XmlSchemaComplexType.anyType = new XmlSchemaComplexType();
					XmlSchemaComplexType.anyType.Name = "anyType";
					XmlSchemaComplexType.anyType.QNameInternal = new XmlQualifiedName("anyType", "http://www.w3.org/2001/XMLSchema");
					if (XmlSchemaUtil.StrictMsCompliant)
					{
						XmlSchemaComplexType.anyType.validatableParticle = XmlSchemaParticle.Empty;
					}
					else
					{
						XmlSchemaComplexType.anyType.validatableParticle = XmlSchemaAny.AnyTypeContent;
					}
					XmlSchemaComplexType.anyType.contentTypeParticle = XmlSchemaComplexType.anyType.validatableParticle;
					XmlSchemaComplexType.anyType.DatatypeInternal = XmlSchemaSimpleType.AnySimpleType;
					XmlSchemaComplexType.anyType.isMixed = true;
					XmlSchemaComplexType.anyType.resolvedContentType = XmlSchemaContentType.Mixed;
				}
				return XmlSchemaComplexType.anyType;
			}
		}

		/// <summary>Gets or sets the information that determines if the complexType element can be used in the instance document.</summary>
		/// <returns>If true, an element cannot use this complexType element directly and must use a complex type that is derived from this complexType element. The default is false.Optional.</returns>
		[DefaultValue(false)]
		[XmlAttribute("abstract")]
		public bool IsAbstract
		{
			get
			{
				return this.isAbstract;
			}
			set
			{
				this.isAbstract = value;
			}
		}

		/// <summary>Gets or sets the block attribute.</summary>
		/// <returns>The block attribute prevents a complex type from being used in the specified type of derivation. The default is XmlSchemaDerivationMethod.None.Optional.</returns>
		[DefaultValue(XmlSchemaDerivationMethod.None)]
		[XmlAttribute("block")]
		public XmlSchemaDerivationMethod Block
		{
			get
			{
				return this.block;
			}
			set
			{
				this.block = value;
			}
		}

		/// <summary>Gets or sets information that determines if the complex type has a mixed content model (markup within the content).</summary>
		/// <returns>true, if character data can appear between child elements of this complex type; otherwise, false. The default is false.Optional.</returns>
		[DefaultValue(false)]
		[XmlAttribute("mixed")]
		public override bool IsMixed
		{
			get
			{
				return this.isMixed;
			}
			set
			{
				this.isMixed = value;
			}
		}

		/// <summary>Gets or sets the post-compilation <see cref="T:System.Xml.Schema.XmlSchemaContentModel" /> of this complex type.</summary>
		/// <returns>The content model type that is one of the <see cref="T:System.Xml.Schema.XmlSchemaSimpleContent" /> or <see cref="T:System.Xml.Schema.XmlSchemaComplexContent" /> classes.</returns>
		[XmlElement("complexContent", typeof(XmlSchemaComplexContent))]
		[XmlElement("simpleContent", typeof(XmlSchemaSimpleContent))]
		public XmlSchemaContentModel ContentModel
		{
			get
			{
				return this.contentModel;
			}
			set
			{
				this.contentModel = value;
			}
		}

		/// <summary>Gets or sets the compositor type as one of the <see cref="T:System.Xml.Schema.XmlSchemaGroupRef" />, <see cref="T:System.Xml.Schema.XmlSchemaChoice" />, <see cref="T:System.Xml.Schema.XmlSchemaAll" />, or <see cref="T:System.Xml.Schema.XmlSchemaSequence" /> classes.</summary>
		/// <returns>The compositor type.</returns>
		[XmlElement("choice", typeof(XmlSchemaChoice))]
		[XmlElement("group", typeof(XmlSchemaGroupRef))]
		[XmlElement("sequence", typeof(XmlSchemaSequence))]
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

		/// <summary>Gets the collection of attributes for the complex type.</summary>
		/// <returns>Contains <see cref="T:System.Xml.Schema.XmlSchemaAttribute" /> and <see cref="T:System.Xml.Schema.XmlSchemaAttributeGroupRef" /> classes.</returns>
		[XmlElement("attribute", typeof(XmlSchemaAttribute))]
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroupRef))]
		public XmlSchemaObjectCollection Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		/// <summary>Gets or sets the value for the <see cref="T:System.Xml.Schema.XmlSchemaAnyAttribute" /> component of the complex type.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchemaAnyAttribute" /> component of the complex type.</returns>
		[XmlElement("anyAttribute")]
		public XmlSchemaAnyAttribute AnyAttribute
		{
			get
			{
				return this.anyAttribute;
			}
			set
			{
				this.anyAttribute = value;
			}
		}

		/// <summary>Gets the content model of the complex type which holds the post-compilation value.</summary>
		/// <returns>The post-compilation value of the content model for the complex type.</returns>
		[XmlIgnore]
		public XmlSchemaContentType ContentType
		{
			get
			{
				return this.resolvedContentType;
			}
		}

		/// <summary>Gets the particle that holds the post-compilation value of the <see cref="P:System.Xml.Schema.XmlSchemaComplexType.ContentType" /> particle.</summary>
		/// <returns>The particle for the content type. The post-compilation value of the <see cref="P:System.Xml.Schema.XmlSchemaComplexType.ContentType" /> particle.</returns>
		[XmlIgnore]
		public XmlSchemaParticle ContentTypeParticle
		{
			get
			{
				return this.contentTypeParticle;
			}
		}

		/// <summary>Gets the value after the type has been compiled to the post-schema-validation information set (infoset). This value indicates how the type is enforced when xsi:type is used in the instance document.</summary>
		/// <returns>The post-schema-validated infoset value. The default is BlockDefault value on the schema element.</returns>
		[XmlIgnore]
		public XmlSchemaDerivationMethod BlockResolved
		{
			get
			{
				return this.blockResolved;
			}
		}

		/// <summary>Gets the collection of all the complied attributes of this complex type and its base types.</summary>
		/// <returns>The collection of all the attributes from this complex type and its base types. The post-compilation value of the AttributeUses property.</returns>
		[XmlIgnore]
		public XmlSchemaObjectTable AttributeUses
		{
			get
			{
				return this.attributeUses;
			}
		}

		/// <summary>Gets the post-compilation value for anyAttribute for this complex type and its base type(s).</summary>
		/// <returns>The post-compilation value of the anyAttribute element.</returns>
		[XmlIgnore]
		public XmlSchemaAnyAttribute AttributeWildcard
		{
			get
			{
				return this.attributeWildcard;
			}
		}

		internal XmlSchemaParticle ValidatableParticle
		{
			get
			{
				return this.contentTypeParticle;
			}
		}

		internal override void SetParent(XmlSchemaObject parent)
		{
			base.SetParent(parent);
			if (this.ContentModel != null)
			{
				this.ContentModel.SetParent(this);
			}
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
				return this.errorCount;
			}
			this.ValidatedIsAbstract = this.isAbstract;
			this.attributeUses.Clear();
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
				if (this.ContentModel != null)
				{
					this.ContentModel.isRedefinedComponent = true;
				}
				if (this.Particle != null)
				{
					this.Particle.isRedefinedComponent = true;
				}
			}
			if (this.ParentIsSchema || this.isRedefineChild)
			{
				if (base.Name == null || base.Name == string.Empty)
				{
					base.error(h, "name must be present in a top level complex type");
				}
				else if (!XmlSchemaUtil.CheckNCName(base.Name))
				{
					base.error(h, "name must be a NCName");
				}
				else
				{
					this.QNameInternal = new XmlQualifiedName(base.Name, base.AncestorSchema.TargetNamespace);
				}
				if (this.Block != XmlSchemaDerivationMethod.None)
				{
					if (this.Block == XmlSchemaDerivationMethod.All)
					{
						this.blockResolved = XmlSchemaDerivationMethod.All;
					}
					else
					{
						if ((this.Block & XmlSchemaUtil.ComplexTypeBlockAllowed) != this.Block)
						{
							base.error(h, "Invalid block specification.");
						}
						this.blockResolved = (this.Block & XmlSchemaUtil.ComplexTypeBlockAllowed);
					}
				}
				else
				{
					XmlSchemaDerivationMethod xmlSchemaDerivationMethod = schema.BlockDefault;
					if (xmlSchemaDerivationMethod != XmlSchemaDerivationMethod.All)
					{
						if (xmlSchemaDerivationMethod != XmlSchemaDerivationMethod.None)
						{
							this.blockResolved = (schema.BlockDefault & XmlSchemaUtil.ComplexTypeBlockAllowed);
						}
						else
						{
							this.blockResolved = XmlSchemaDerivationMethod.Empty;
						}
					}
					else
					{
						this.blockResolved = XmlSchemaDerivationMethod.All;
					}
				}
				if (base.Final != XmlSchemaDerivationMethod.None)
				{
					if (base.Final == XmlSchemaDerivationMethod.All)
					{
						this.finalResolved = XmlSchemaDerivationMethod.All;
					}
					else if ((base.Final & XmlSchemaUtil.FinalAllowed) != base.Final)
					{
						base.error(h, "Invalid final specification.");
					}
					else
					{
						this.finalResolved = base.Final;
					}
				}
				else
				{
					XmlSchemaDerivationMethod xmlSchemaDerivationMethod = schema.FinalDefault;
					if (xmlSchemaDerivationMethod != XmlSchemaDerivationMethod.All)
					{
						if (xmlSchemaDerivationMethod != XmlSchemaDerivationMethod.None)
						{
							this.finalResolved = (schema.FinalDefault & XmlSchemaUtil.FinalAllowed);
						}
						else
						{
							this.finalResolved = XmlSchemaDerivationMethod.Empty;
						}
					}
					else
					{
						this.finalResolved = XmlSchemaDerivationMethod.All;
					}
				}
			}
			else
			{
				if (this.isAbstract)
				{
					base.error(h, "abstract must be false in a local complex type");
				}
				if (base.Name != null)
				{
					base.error(h, "name must be absent in a local complex type");
				}
				if (base.Final != XmlSchemaDerivationMethod.None)
				{
					base.error(h, "final must be absent in a local complex type");
				}
				if (this.block != XmlSchemaDerivationMethod.None)
				{
					base.error(h, "block must be absent in a local complex type");
				}
			}
			if (this.contentModel != null)
			{
				if (this.anyAttribute != null || this.Attributes.Count != 0 || this.Particle != null)
				{
					base.error(h, "attributes, particles or anyattribute is not allowed if ContentModel is present");
				}
				this.errorCount += this.contentModel.Compile(h, schema);
				XmlSchemaSimpleContent xmlSchemaSimpleContent = this.ContentModel as XmlSchemaSimpleContent;
				if (xmlSchemaSimpleContent != null && !(xmlSchemaSimpleContent.Content is XmlSchemaSimpleContentExtension))
				{
					XmlSchemaSimpleContentRestriction xmlSchemaSimpleContentRestriction = xmlSchemaSimpleContent.Content as XmlSchemaSimpleContentRestriction;
					if (xmlSchemaSimpleContentRestriction != null && xmlSchemaSimpleContentRestriction.BaseType != null)
					{
						xmlSchemaSimpleContentRestriction.BaseType.Compile(h, schema);
						this.BaseXmlSchemaTypeInternal = xmlSchemaSimpleContentRestriction.BaseType;
					}
				}
			}
			else
			{
				if (this.Particle != null)
				{
					this.errorCount += this.Particle.Compile(h, schema);
				}
				if (this.anyAttribute != null)
				{
					this.AnyAttribute.Compile(h, schema);
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
						base.error(h, xmlSchemaObject2.GetType() + " is not valid in this place::ComplexType");
					}
				}
			}
			XmlSchemaUtil.CompileID(base.Id, this, schema.IDCollection, h);
			this.CompilationId = schema.CompilationId;
			return this.errorCount;
		}

		private void CollectSchemaComponent(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.CollectProcessId == schema.CompilationId)
			{
				return;
			}
			if (this.contentModel != null)
			{
				this.BaseSchemaTypeName = ((this.contentModel.Content == null) ? XmlQualifiedName.Empty : this.contentModel.Content.GetBaseTypeName());
				this.BaseXmlSchemaTypeInternal = schema.FindSchemaType(this.BaseSchemaTypeName);
			}
			if (this.isRedefineChild && base.BaseXmlSchemaType != null && base.QualifiedName == this.BaseSchemaTypeName)
			{
				XmlSchemaType xmlSchemaType = (XmlSchemaType)this.redefinedObject;
				if (xmlSchemaType == null)
				{
					base.error(h, "Redefinition base type was not found.");
				}
				else
				{
					this.BaseXmlSchemaTypeInternal = xmlSchemaType;
				}
			}
			if (this.contentModel != null && this.contentModel.Content != null)
			{
				this.resolvedDerivedBy = ((!this.contentModel.Content.IsExtension) ? XmlSchemaDerivationMethod.Restriction : XmlSchemaDerivationMethod.Extension);
			}
			else
			{
				this.resolvedDerivedBy = XmlSchemaDerivationMethod.Empty;
			}
		}

		private void FillContentTypeParticle(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.ContentModel != null)
			{
				this.CollectContentTypeFromContentModel(h, schema);
			}
			else
			{
				this.CollectContentTypeFromImmediateContent();
			}
			this.contentTypeParticle = this.validatableParticle.GetOptimizedParticle(true);
			if (this.contentTypeParticle == XmlSchemaParticle.Empty && this.resolvedContentType == XmlSchemaContentType.ElementOnly)
			{
				this.resolvedContentType = XmlSchemaContentType.Empty;
			}
			this.CollectProcessId = schema.CompilationId;
		}

		private void CollectContentTypeFromImmediateContent()
		{
			if (this.Particle != null)
			{
				this.validatableParticle = this.Particle;
			}
			if (this == XmlSchemaComplexType.AnyType)
			{
				this.resolvedContentType = XmlSchemaContentType.Mixed;
				return;
			}
			if (this.validatableParticle == XmlSchemaParticle.Empty)
			{
				if (this.IsMixed)
				{
					this.resolvedContentType = XmlSchemaContentType.TextOnly;
				}
				else
				{
					this.resolvedContentType = XmlSchemaContentType.Empty;
				}
			}
			else if (this.IsMixed)
			{
				this.resolvedContentType = XmlSchemaContentType.Mixed;
			}
			else
			{
				this.resolvedContentType = XmlSchemaContentType.ElementOnly;
			}
			if (this != XmlSchemaComplexType.AnyType)
			{
				this.BaseXmlSchemaTypeInternal = XmlSchemaComplexType.AnyType;
			}
		}

		private void CollectContentTypeFromContentModel(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.ContentModel.Content == null)
			{
				this.validatableParticle = XmlSchemaParticle.Empty;
				this.resolvedContentType = XmlSchemaContentType.Empty;
				return;
			}
			if (this.ContentModel.Content is XmlSchemaComplexContentExtension)
			{
				this.CollectContentTypeFromComplexExtension(h, schema);
			}
			if (this.ContentModel.Content is XmlSchemaComplexContentRestriction)
			{
				this.CollectContentTypeFromComplexRestriction();
			}
		}

		private void CollectContentTypeFromComplexExtension(ValidationEventHandler h, XmlSchema schema)
		{
			XmlSchemaComplexContentExtension xmlSchemaComplexContentExtension = (XmlSchemaComplexContentExtension)this.ContentModel.Content;
			XmlSchemaComplexType xmlSchemaComplexType = base.BaseXmlSchemaType as XmlSchemaComplexType;
			if (xmlSchemaComplexType != null)
			{
				xmlSchemaComplexType.CollectSchemaComponent(h, schema);
			}
			if (this.BaseSchemaTypeName == XmlSchemaComplexType.AnyTypeName)
			{
				xmlSchemaComplexType = XmlSchemaComplexType.AnyType;
			}
			if (xmlSchemaComplexType == null)
			{
				this.validatableParticle = XmlSchemaParticle.Empty;
				this.resolvedContentType = XmlSchemaContentType.Empty;
				return;
			}
			if (xmlSchemaComplexContentExtension.Particle == null || xmlSchemaComplexContentExtension.Particle == XmlSchemaParticle.Empty)
			{
				if (xmlSchemaComplexType == null)
				{
					this.validatableParticle = XmlSchemaParticle.Empty;
					this.resolvedContentType = XmlSchemaContentType.Empty;
				}
				else
				{
					this.validatableParticle = xmlSchemaComplexType.ValidatableParticle;
					this.resolvedContentType = xmlSchemaComplexType.resolvedContentType;
					if (this.resolvedContentType == XmlSchemaContentType.Empty)
					{
						this.resolvedContentType = this.GetComplexContentType(this.contentModel);
					}
				}
			}
			else if (xmlSchemaComplexType.validatableParticle == XmlSchemaParticle.Empty || xmlSchemaComplexType == XmlSchemaComplexType.AnyType)
			{
				this.validatableParticle = xmlSchemaComplexContentExtension.Particle;
				this.resolvedContentType = this.GetComplexContentType(this.contentModel);
			}
			else
			{
				XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
				this.CopyInfo(xmlSchemaSequence);
				xmlSchemaSequence.Items.Add(xmlSchemaComplexType.validatableParticle);
				xmlSchemaSequence.Items.Add(xmlSchemaComplexContentExtension.Particle);
				xmlSchemaSequence.Compile(h, schema);
				xmlSchemaSequence.Validate(h, schema);
				this.validatableParticle = xmlSchemaSequence;
				this.resolvedContentType = this.GetComplexContentType(this.contentModel);
			}
			if (this.validatableParticle == null)
			{
				this.validatableParticle = XmlSchemaParticle.Empty;
			}
		}

		private void CollectContentTypeFromComplexRestriction()
		{
			XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction = (XmlSchemaComplexContentRestriction)this.ContentModel.Content;
			bool flag = false;
			if (xmlSchemaComplexContentRestriction.Particle == null)
			{
				flag = true;
			}
			else
			{
				XmlSchemaGroupBase xmlSchemaGroupBase = xmlSchemaComplexContentRestriction.Particle as XmlSchemaGroupBase;
				if (xmlSchemaGroupBase != null)
				{
					if (!(xmlSchemaGroupBase is XmlSchemaChoice) && xmlSchemaGroupBase.Items.Count == 0)
					{
						flag = true;
					}
					else if (xmlSchemaGroupBase is XmlSchemaChoice && xmlSchemaGroupBase.Items.Count == 0 && xmlSchemaGroupBase.ValidatedMinOccurs == 0m)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				this.resolvedContentType = XmlSchemaContentType.Empty;
				this.validatableParticle = XmlSchemaParticle.Empty;
			}
			else
			{
				this.resolvedContentType = this.GetComplexContentType(this.contentModel);
				this.validatableParticle = xmlSchemaComplexContentRestriction.Particle;
			}
		}

		private XmlSchemaContentType GetComplexContentType(XmlSchemaContentModel content)
		{
			if (this.IsMixed || ((XmlSchemaComplexContent)content).IsMixed)
			{
				return XmlSchemaContentType.Mixed;
			}
			return XmlSchemaContentType.ElementOnly;
		}

		internal override int Validate(ValidationEventHandler h, XmlSchema schema)
		{
			if (base.IsValidated(schema.ValidationId))
			{
				return this.errorCount;
			}
			this.ValidationId = schema.ValidationId;
			this.CollectSchemaComponent(h, schema);
			this.ValidateContentFirstPass(h, schema);
			this.FillContentTypeParticle(h, schema);
			if (this.ContentModel != null)
			{
				this.ValidateContentModel(h, schema);
			}
			else
			{
				this.ValidateImmediateAttributes(h, schema);
			}
			if (this.ContentTypeParticle != null)
			{
				XmlSchemaAll xmlSchemaAll = this.contentTypeParticle.GetOptimizedParticle(true) as XmlSchemaAll;
				if (xmlSchemaAll != null && (xmlSchemaAll.ValidatedMaxOccurs != 1m || this.contentTypeParticle.ValidatedMaxOccurs != 1m))
				{
					base.error(h, "Particle whose term is -all- and consists of complex type content particle must have maxOccurs = 1.");
				}
			}
			if (schema.Schemas.CompilationSettings != null && schema.Schemas.CompilationSettings.EnableUpaCheck)
			{
				this.contentTypeParticle.ValidateUniqueParticleAttribution(new XmlSchemaObjectTable(), new ArrayList(), h, schema);
			}
			this.contentTypeParticle.ValidateUniqueTypeAttribution(new XmlSchemaObjectTable(), h, schema);
			XmlSchemaAttribute xmlSchemaAttribute = null;
			foreach (object obj in this.attributeUses)
			{
				XmlSchemaAttribute xmlSchemaAttribute2 = (XmlSchemaAttribute)((DictionaryEntry)obj).Value;
				XmlSchemaDatatype xmlSchemaDatatype = xmlSchemaAttribute2.AttributeType as XmlSchemaDatatype;
				if (xmlSchemaDatatype == null || xmlSchemaDatatype.TokenizedType == XmlTokenizedType.ID)
				{
					if (xmlSchemaDatatype == null)
					{
						xmlSchemaDatatype = ((XmlSchemaSimpleType)xmlSchemaAttribute2.AttributeType).Datatype;
					}
					if (xmlSchemaDatatype != null && xmlSchemaDatatype.TokenizedType == XmlTokenizedType.ID)
					{
						if (xmlSchemaAttribute != null)
						{
							base.error(h, "Two or more ID typed attribute declarations in a complex type are found.");
						}
						else
						{
							xmlSchemaAttribute = xmlSchemaAttribute2;
						}
					}
				}
			}
			this.ValidationId = schema.ValidationId;
			return this.errorCount;
		}

		private void ValidateImmediateAttributes(ValidationEventHandler h, XmlSchema schema)
		{
			this.attributeUses = new XmlSchemaObjectTable();
			XmlSchemaUtil.ValidateAttributesResolved(this.attributeUses, h, schema, this.attributes, this.anyAttribute, ref this.attributeWildcard, null, false);
		}

		private void ValidateContentFirstPass(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.ContentModel != null)
			{
				this.errorCount += this.contentModel.Validate(h, schema);
				if (this.BaseXmlSchemaTypeInternal != null)
				{
					this.errorCount += this.BaseXmlSchemaTypeInternal.Validate(h, schema);
				}
			}
			else if (this.Particle != null)
			{
				this.errorCount += this.particle.Validate(h, schema);
				XmlSchemaGroupRef xmlSchemaGroupRef = this.Particle as XmlSchemaGroupRef;
				if (xmlSchemaGroupRef != null)
				{
					if (xmlSchemaGroupRef.TargetGroup != null)
					{
						this.errorCount += xmlSchemaGroupRef.TargetGroup.Validate(h, schema);
					}
					else if (!schema.IsNamespaceAbsent(xmlSchemaGroupRef.RefName.Namespace))
					{
						base.error(h, "Referenced group " + xmlSchemaGroupRef.RefName + " was not found in the corresponding schema.");
					}
				}
			}
		}

		private void ValidateContentModel(ValidationEventHandler h, XmlSchema schema)
		{
			XmlSchemaType baseXmlSchemaTypeInternal = this.BaseXmlSchemaTypeInternal;
			XmlSchemaComplexContentExtension xmlSchemaComplexContentExtension = this.contentModel.Content as XmlSchemaComplexContentExtension;
			XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction = this.contentModel.Content as XmlSchemaComplexContentRestriction;
			XmlSchemaSimpleContentExtension xmlSchemaSimpleContentExtension = this.contentModel.Content as XmlSchemaSimpleContentExtension;
			XmlSchemaSimpleContentRestriction xmlSchemaSimpleContentRestriction = this.contentModel.Content as XmlSchemaSimpleContentRestriction;
			XmlSchemaAnyAttribute xmlSchemaAnyAttribute = null;
			XmlSchemaAnyAttribute xmlSchemaAnyAttribute2 = null;
			if (base.ValidateRecursionCheck())
			{
				base.error(h, "Circular definition of schema types was found.");
			}
			if (baseXmlSchemaTypeInternal != null)
			{
				this.DatatypeInternal = baseXmlSchemaTypeInternal.Datatype;
			}
			else if (this.BaseSchemaTypeName == XmlSchemaComplexType.AnyTypeName)
			{
				this.DatatypeInternal = XmlSchemaSimpleType.AnySimpleType;
			}
			else if (XmlSchemaUtil.IsBuiltInDatatypeName(this.BaseSchemaTypeName))
			{
				this.DatatypeInternal = XmlSchemaDatatype.FromName(this.BaseSchemaTypeName);
			}
			XmlSchemaComplexType xmlSchemaComplexType = baseXmlSchemaTypeInternal as XmlSchemaComplexType;
			XmlSchemaSimpleType xmlSchemaSimpleType = baseXmlSchemaTypeInternal as XmlSchemaSimpleType;
			if (baseXmlSchemaTypeInternal != null && (baseXmlSchemaTypeInternal.FinalResolved & this.resolvedDerivedBy) != XmlSchemaDerivationMethod.Empty)
			{
				base.error(h, "Specified derivation is specified as final by derived schema type.");
			}
			if (xmlSchemaSimpleType != null && this.resolvedDerivedBy == XmlSchemaDerivationMethod.Restriction)
			{
				base.error(h, "If the base schema type is a simple type, then this type must be extension.");
			}
			if (xmlSchemaComplexContentExtension != null || xmlSchemaComplexContentRestriction != null)
			{
				if (this.BaseSchemaTypeName == XmlSchemaComplexType.AnyTypeName)
				{
					xmlSchemaComplexType = XmlSchemaComplexType.AnyType;
				}
				else if (XmlSchemaUtil.IsBuiltInDatatypeName(this.BaseSchemaTypeName))
				{
					base.error(h, "Referenced base schema type is XML Schema datatype.");
				}
				else if (xmlSchemaComplexType == null && !schema.IsNamespaceAbsent(this.BaseSchemaTypeName.Namespace))
				{
					base.error(h, "Referenced base schema type " + this.BaseSchemaTypeName + " was not complex type or not found in the corresponding schema.");
				}
			}
			else
			{
				this.resolvedContentType = XmlSchemaContentType.TextOnly;
				if (this.BaseSchemaTypeName == XmlSchemaComplexType.AnyTypeName)
				{
					xmlSchemaComplexType = XmlSchemaComplexType.AnyType;
				}
				if (xmlSchemaComplexType != null && xmlSchemaComplexType.ContentType != XmlSchemaContentType.TextOnly)
				{
					base.error(h, "Base schema complex type of a simple content must be simple content type. Base type is " + this.BaseSchemaTypeName);
				}
				else if (xmlSchemaSimpleContentExtension == null && xmlSchemaSimpleType != null && this.BaseSchemaTypeName.Namespace != "http://www.w3.org/2001/XMLSchema")
				{
					base.error(h, "If a simple content is not an extension, base schema type must be complex type. Base type is " + this.BaseSchemaTypeName);
				}
				else if (!XmlSchemaUtil.IsBuiltInDatatypeName(this.BaseSchemaTypeName))
				{
					if (baseXmlSchemaTypeInternal == null && !schema.IsNamespaceAbsent(this.BaseSchemaTypeName.Namespace))
					{
						base.error(h, "Referenced base schema type " + this.BaseSchemaTypeName + " was not found in the corresponding schema.");
					}
				}
				if (xmlSchemaComplexType != null)
				{
					if (xmlSchemaComplexType.ContentType != XmlSchemaContentType.TextOnly)
					{
						if (xmlSchemaSimpleContentRestriction == null || xmlSchemaComplexType.ContentType != XmlSchemaContentType.Mixed || xmlSchemaComplexType.Particle == null || !xmlSchemaComplexType.Particle.ValidateIsEmptiable() || xmlSchemaSimpleContentRestriction.BaseType == null)
						{
							base.error(h, "Base complex type of a simple content restriction must be text only.");
						}
					}
				}
				else if (xmlSchemaSimpleContentExtension == null || xmlSchemaComplexType != null)
				{
					base.error(h, "Not allowed base type of a simple content restriction.");
				}
			}
			if (xmlSchemaComplexContentExtension != null)
			{
				xmlSchemaAnyAttribute = xmlSchemaComplexContentExtension.AnyAttribute;
				if (xmlSchemaComplexType != null)
				{
					foreach (object obj in xmlSchemaComplexType.AttributeUses)
					{
						XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)((DictionaryEntry)obj).Value;
						XmlSchemaUtil.AddToTable(this.attributeUses, xmlSchemaAttribute, xmlSchemaAttribute.QualifiedName, h);
					}
					xmlSchemaAnyAttribute2 = xmlSchemaComplexType.AttributeWildcard;
				}
				this.errorCount += XmlSchemaUtil.ValidateAttributesResolved(this.attributeUses, h, schema, xmlSchemaComplexContentExtension.Attributes, xmlSchemaComplexContentExtension.AnyAttribute, ref this.attributeWildcard, null, true);
				if (xmlSchemaComplexType != null)
				{
					this.ValidateComplexBaseDerivationValidExtension(xmlSchemaComplexType, h, schema);
				}
				else if (xmlSchemaSimpleType != null)
				{
					this.ValidateSimpleBaseDerivationValidExtension(xmlSchemaSimpleType, h, schema);
				}
			}
			if (xmlSchemaComplexContentRestriction != null)
			{
				if (xmlSchemaComplexType == null)
				{
					xmlSchemaComplexType = XmlSchemaComplexType.AnyType;
				}
				xmlSchemaAnyAttribute = xmlSchemaComplexContentRestriction.AnyAttribute;
				this.attributeWildcard = xmlSchemaAnyAttribute;
				if (xmlSchemaComplexType != null)
				{
					xmlSchemaAnyAttribute2 = xmlSchemaComplexType.AttributeWildcard;
				}
				if (xmlSchemaAnyAttribute2 != null && xmlSchemaAnyAttribute != null)
				{
					xmlSchemaAnyAttribute.ValidateWildcardSubset(xmlSchemaAnyAttribute2, h, schema);
				}
				this.errorCount += XmlSchemaUtil.ValidateAttributesResolved(this.attributeUses, h, schema, xmlSchemaComplexContentRestriction.Attributes, xmlSchemaComplexContentRestriction.AnyAttribute, ref this.attributeWildcard, null, false);
				foreach (object obj2 in xmlSchemaComplexType.AttributeUses)
				{
					XmlSchemaAttribute xmlSchemaAttribute2 = (XmlSchemaAttribute)((DictionaryEntry)obj2).Value;
					if (this.attributeUses[xmlSchemaAttribute2.QualifiedName] == null)
					{
						XmlSchemaUtil.AddToTable(this.attributeUses, xmlSchemaAttribute2, xmlSchemaAttribute2.QualifiedName, h);
					}
				}
				this.ValidateDerivationValidRestriction(xmlSchemaComplexType, h, schema);
			}
			if (xmlSchemaSimpleContentExtension != null)
			{
				this.errorCount += XmlSchemaUtil.ValidateAttributesResolved(this.attributeUses, h, schema, xmlSchemaSimpleContentExtension.Attributes, xmlSchemaSimpleContentExtension.AnyAttribute, ref this.attributeWildcard, null, true);
				xmlSchemaAnyAttribute = xmlSchemaSimpleContentExtension.AnyAttribute;
				if (xmlSchemaComplexType != null)
				{
					xmlSchemaAnyAttribute2 = xmlSchemaComplexType.AttributeWildcard;
					foreach (object obj3 in xmlSchemaComplexType.AttributeUses)
					{
						XmlSchemaAttribute xmlSchemaAttribute3 = (XmlSchemaAttribute)((DictionaryEntry)obj3).Value;
						XmlSchemaUtil.AddToTable(this.attributeUses, xmlSchemaAttribute3, xmlSchemaAttribute3.QualifiedName, h);
					}
				}
				if (xmlSchemaAnyAttribute2 != null && xmlSchemaAnyAttribute != null)
				{
					xmlSchemaAnyAttribute.ValidateWildcardSubset(xmlSchemaAnyAttribute2, h, schema);
				}
			}
			if (xmlSchemaSimpleContentRestriction != null)
			{
				xmlSchemaAnyAttribute2 = ((xmlSchemaComplexType == null) ? null : xmlSchemaComplexType.AttributeWildcard);
				xmlSchemaAnyAttribute = xmlSchemaSimpleContentRestriction.AnyAttribute;
				if (xmlSchemaAnyAttribute != null && xmlSchemaAnyAttribute2 != null)
				{
					xmlSchemaAnyAttribute.ValidateWildcardSubset(xmlSchemaAnyAttribute2, h, schema);
				}
				this.errorCount += XmlSchemaUtil.ValidateAttributesResolved(this.attributeUses, h, schema, xmlSchemaSimpleContentRestriction.Attributes, xmlSchemaSimpleContentRestriction.AnyAttribute, ref this.attributeWildcard, null, false);
			}
			if (xmlSchemaAnyAttribute != null)
			{
				this.attributeWildcard = xmlSchemaAnyAttribute;
			}
			else
			{
				this.attributeWildcard = xmlSchemaAnyAttribute2;
			}
		}

		internal void ValidateTypeDerivationOK(object b, ValidationEventHandler h, XmlSchema schema)
		{
			if (this == XmlSchemaComplexType.AnyType && base.BaseXmlSchemaType == this)
			{
				return;
			}
			XmlSchemaType xmlSchemaType = b as XmlSchemaType;
			if (b == this)
			{
				return;
			}
			if (xmlSchemaType != null && (this.resolvedDerivedBy & xmlSchemaType.FinalResolved) != XmlSchemaDerivationMethod.Empty)
			{
				base.error(h, "Derivation type " + this.resolvedDerivedBy + " is prohibited by the base type.");
			}
			if (base.BaseSchemaType == b)
			{
				return;
			}
			if (base.BaseSchemaType == null || base.BaseXmlSchemaType == XmlSchemaComplexType.AnyType)
			{
				base.error(h, "Derived type's base schema type is anyType.");
				return;
			}
			XmlSchemaComplexType xmlSchemaComplexType = base.BaseXmlSchemaType as XmlSchemaComplexType;
			if (xmlSchemaComplexType != null)
			{
				xmlSchemaComplexType.ValidateTypeDerivationOK(b, h, schema);
				return;
			}
			XmlSchemaSimpleType xmlSchemaSimpleType = base.BaseXmlSchemaType as XmlSchemaSimpleType;
			if (xmlSchemaSimpleType != null)
			{
				xmlSchemaSimpleType.ValidateTypeDerivationOK(b, h, schema, true);
				return;
			}
		}

		internal void ValidateComplexBaseDerivationValidExtension(XmlSchemaComplexType baseComplexType, ValidationEventHandler h, XmlSchema schema)
		{
			if ((baseComplexType.FinalResolved & XmlSchemaDerivationMethod.Extension) != XmlSchemaDerivationMethod.Empty)
			{
				base.error(h, "Derivation by extension is prohibited.");
			}
			foreach (object obj in baseComplexType.AttributeUses)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)((DictionaryEntry)obj).Value;
				if (!(this.AttributeUses[xmlSchemaAttribute.QualifiedName] is XmlSchemaAttribute))
				{
					base.error(h, "Invalid complex type derivation by extension was found. Missing attribute was found: " + xmlSchemaAttribute.QualifiedName + " .");
				}
			}
			if (this.AnyAttribute != null)
			{
				if (baseComplexType.AnyAttribute == null)
				{
					base.error(h, "Invalid complex type derivation by extension was found. Base complex type does not have an attribute wildcard.");
				}
				else
				{
					baseComplexType.AnyAttribute.ValidateWildcardSubset(this.AnyAttribute, h, schema);
				}
			}
			if (baseComplexType.ContentType != XmlSchemaContentType.Empty)
			{
				if (this.ContentType != baseComplexType.ContentType)
				{
					base.error(h, "Base complex type has different content type " + baseComplexType.ContentType + ".");
				}
				else if (this.contentTypeParticle == null || !this.contentTypeParticle.ParticleEquals(baseComplexType.ContentTypeParticle))
				{
					XmlSchemaSequence xmlSchemaSequence = this.contentTypeParticle as XmlSchemaSequence;
					if (this.contentTypeParticle != XmlSchemaParticle.Empty && (xmlSchemaSequence == null || this.contentTypeParticle.ValidatedMinOccurs != 1m || this.contentTypeParticle.ValidatedMaxOccurs != 1m))
					{
						base.error(h, "Invalid complex content extension was found.");
					}
				}
			}
		}

		internal void ValidateSimpleBaseDerivationValidExtension(object baseType, ValidationEventHandler h, XmlSchema schema)
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = baseType as XmlSchemaSimpleType;
			if (xmlSchemaSimpleType != null && (xmlSchemaSimpleType.FinalResolved & XmlSchemaDerivationMethod.Extension) != XmlSchemaDerivationMethod.Empty)
			{
				base.error(h, "Extension is prohibited by the base type.");
			}
			XmlSchemaDatatype xmlSchemaDatatype = baseType as XmlSchemaDatatype;
			if (xmlSchemaDatatype == null)
			{
				xmlSchemaDatatype = xmlSchemaSimpleType.Datatype;
			}
			if (xmlSchemaDatatype != base.Datatype)
			{
				base.error(h, "To extend simple type, a complex type must have the same content type as the base type.");
			}
		}

		internal void ValidateDerivationValidRestriction(XmlSchemaComplexType baseType, ValidationEventHandler h, XmlSchema schema)
		{
			if (baseType == null)
			{
				base.error(h, "Base schema type is not a complex type.");
				return;
			}
			if ((baseType.FinalResolved & XmlSchemaDerivationMethod.Restriction) != XmlSchemaDerivationMethod.Empty)
			{
				base.error(h, "Prohibited derivation by restriction by base schema type.");
				return;
			}
			foreach (object obj in this.AttributeUses)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)((DictionaryEntry)obj).Value;
				XmlSchemaAttribute xmlSchemaAttribute2 = baseType.AttributeUses[xmlSchemaAttribute.QualifiedName] as XmlSchemaAttribute;
				if (xmlSchemaAttribute2 != null)
				{
					if (xmlSchemaAttribute2.ValidatedUse != XmlSchemaUse.Optional && xmlSchemaAttribute.ValidatedUse != XmlSchemaUse.Required)
					{
						base.error(h, "Invalid attribute derivation by restriction was found for " + xmlSchemaAttribute.QualifiedName + " .");
					}
					XmlSchemaSimpleType xmlSchemaSimpleType = xmlSchemaAttribute.AttributeType as XmlSchemaSimpleType;
					XmlSchemaSimpleType xmlSchemaSimpleType2 = xmlSchemaAttribute2.AttributeType as XmlSchemaSimpleType;
					bool flag = false;
					if (xmlSchemaSimpleType != null)
					{
						xmlSchemaSimpleType.ValidateDerivationValid(xmlSchemaSimpleType2, null, h, schema);
					}
					else if (xmlSchemaSimpleType == null && xmlSchemaSimpleType2 != null)
					{
						flag = true;
					}
					else
					{
						Type type = xmlSchemaAttribute.AttributeType.GetType();
						Type type2 = xmlSchemaAttribute2.AttributeType.GetType();
						if (type != type2 && type.IsSubclassOf(type2))
						{
							flag = true;
						}
					}
					if (flag)
					{
						base.error(h, "Invalid attribute derivation by restriction because of its type: " + xmlSchemaAttribute.QualifiedName + " .");
					}
					if (xmlSchemaAttribute2.ValidatedFixedValue != null && xmlSchemaAttribute.ValidatedFixedValue != xmlSchemaAttribute2.ValidatedFixedValue)
					{
						base.error(h, "Invalid attribute derivation by restriction because of its fixed value constraint: " + xmlSchemaAttribute.QualifiedName + " .");
					}
				}
				else if (baseType.AttributeWildcard != null && !baseType.AttributeWildcard.ValidateWildcardAllowsNamespaceName(xmlSchemaAttribute.QualifiedName.Namespace, schema) && !schema.IsNamespaceAbsent(xmlSchemaAttribute.QualifiedName.Namespace))
				{
					base.error(h, "Invalid attribute derivation by restriction was found for " + xmlSchemaAttribute.QualifiedName + " .");
				}
			}
			if (this.AttributeWildcard != null && baseType != XmlSchemaComplexType.AnyType)
			{
				if (baseType.AttributeWildcard == null)
				{
					base.error(h, "Invalid attribute derivation by restriction because of attribute wildcard.");
				}
				else
				{
					this.AttributeWildcard.ValidateWildcardSubset(baseType.AttributeWildcard, h, schema);
				}
			}
			if (this == XmlSchemaComplexType.AnyType)
			{
				return;
			}
			if (this.contentTypeParticle == XmlSchemaParticle.Empty)
			{
				if (this.ContentType != XmlSchemaContentType.Empty)
				{
					if (baseType.ContentType == XmlSchemaContentType.Mixed && !baseType.ContentTypeParticle.ValidateIsEmptiable())
					{
						base.error(h, "Invalid content type derivation.");
					}
				}
				else if (baseType.ContentTypeParticle != XmlSchemaParticle.Empty && !baseType.ContentTypeParticle.ValidateIsEmptiable())
				{
					base.error(h, "Invalid content type derivation.");
				}
			}
			else if (baseType.ContentTypeParticle != null && !this.contentTypeParticle.ParticleEquals(baseType.ContentTypeParticle))
			{
				this.contentTypeParticle.ValidateDerivationByRestriction(baseType.ContentTypeParticle, h, schema, true);
			}
		}

		internal static XmlSchemaComplexType Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "complexType")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaComplexType.Read, name=" + reader.Name, null);
				reader.SkipToEnd();
				return null;
			}
			xmlSchemaComplexType.LineNumber = reader.LineNumber;
			xmlSchemaComplexType.LinePosition = reader.LinePosition;
			xmlSchemaComplexType.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "abstract")
				{
					Exception ex;
					xmlSchemaComplexType.IsAbstract = XmlSchemaUtil.ReadBoolAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is invalid value for abstract", ex);
					}
				}
				else if (reader.Name == "block")
				{
					Exception ex;
					xmlSchemaComplexType.block = XmlSchemaUtil.ReadDerivationAttribute(reader, out ex, "block", XmlSchemaUtil.ComplexTypeBlockAllowed);
					if (ex != null)
					{
						XmlSchemaObject.error(h, "some invalid values for block attribute were found", ex);
					}
				}
				else if (reader.Name == "final")
				{
					Exception ex;
					xmlSchemaComplexType.Final = XmlSchemaUtil.ReadDerivationAttribute(reader, out ex, "final", XmlSchemaUtil.FinalAllowed);
					if (ex != null)
					{
						XmlSchemaObject.error(h, "some invalid values for final attribute were found", ex);
					}
				}
				else if (reader.Name == "id")
				{
					xmlSchemaComplexType.Id = reader.Value;
				}
				else if (reader.Name == "mixed")
				{
					Exception ex;
					xmlSchemaComplexType.isMixed = XmlSchemaUtil.ReadBoolAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is invalid value for mixed", ex);
					}
				}
				else if (reader.Name == "name")
				{
					xmlSchemaComplexType.Name = reader.Value;
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for complexType", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaComplexType);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaComplexType;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "complexType")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaComplexType.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaComplexType.Annotation = xmlSchemaAnnotation;
					}
				}
				else
				{
					if (num <= 2)
					{
						if (reader.LocalName == "simpleContent")
						{
							num = 6;
							XmlSchemaSimpleContent xmlSchemaSimpleContent = XmlSchemaSimpleContent.Read(reader, h);
							if (xmlSchemaSimpleContent != null)
							{
								xmlSchemaComplexType.ContentModel = xmlSchemaSimpleContent;
							}
							continue;
						}
						if (reader.LocalName == "complexContent")
						{
							num = 6;
							XmlSchemaComplexContent xmlSchemaComplexContent = XmlSchemaComplexContent.Read(reader, h);
							if (xmlSchemaComplexContent != null)
							{
								xmlSchemaComplexType.contentModel = xmlSchemaComplexContent;
							}
							continue;
						}
					}
					if (num <= 3)
					{
						if (reader.LocalName == "group")
						{
							num = 4;
							XmlSchemaGroupRef xmlSchemaGroupRef = XmlSchemaGroupRef.Read(reader, h);
							if (xmlSchemaGroupRef != null)
							{
								xmlSchemaComplexType.particle = xmlSchemaGroupRef;
							}
							continue;
						}
						if (reader.LocalName == "all")
						{
							num = 4;
							XmlSchemaAll xmlSchemaAll = XmlSchemaAll.Read(reader, h);
							if (xmlSchemaAll != null)
							{
								xmlSchemaComplexType.particle = xmlSchemaAll;
							}
							continue;
						}
						if (reader.LocalName == "choice")
						{
							num = 4;
							XmlSchemaChoice xmlSchemaChoice = XmlSchemaChoice.Read(reader, h);
							if (xmlSchemaChoice != null)
							{
								xmlSchemaComplexType.particle = xmlSchemaChoice;
							}
							continue;
						}
						if (reader.LocalName == "sequence")
						{
							num = 4;
							XmlSchemaSequence xmlSchemaSequence = XmlSchemaSequence.Read(reader, h);
							if (xmlSchemaSequence != null)
							{
								xmlSchemaComplexType.particle = xmlSchemaSequence;
							}
							continue;
						}
					}
					if (num <= 4)
					{
						if (reader.LocalName == "attribute")
						{
							num = 4;
							XmlSchemaAttribute xmlSchemaAttribute = XmlSchemaAttribute.Read(reader, h);
							if (xmlSchemaAttribute != null)
							{
								xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
							}
							continue;
						}
						if (reader.LocalName == "attributeGroup")
						{
							num = 4;
							XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = XmlSchemaAttributeGroupRef.Read(reader, h);
							if (xmlSchemaAttributeGroupRef != null)
							{
								xmlSchemaComplexType.attributes.Add(xmlSchemaAttributeGroupRef);
							}
							continue;
						}
					}
					if (num <= 5 && reader.LocalName == "anyAttribute")
					{
						num = 6;
						XmlSchemaAnyAttribute xmlSchemaAnyAttribute = XmlSchemaAnyAttribute.Read(reader, h);
						if (xmlSchemaAnyAttribute != null)
						{
							xmlSchemaComplexType.AnyAttribute = xmlSchemaAnyAttribute;
						}
					}
					else
					{
						reader.RaiseInvalidElementError();
					}
				}
			}
			return xmlSchemaComplexType;
		}
	}
}
